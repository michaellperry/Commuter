using Assisticant.Fields;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.IO;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Commuter.Images
{
    public class ImageCacheCell
    {
        public const string DefaultImageUrl = "ms-appx:///Assets/StoreLogo.scale-400.png";

        private readonly string _sourceImageUrl;

        private Observable<string> _cachedImageUrl = new Observable<string>();

        public ImageCacheCell(string sourceImageUrl)
        {
            _sourceImageUrl = sourceImageUrl;
        }

        public Uri ImageUrl
        {
            get
            {
                lock (this)
                {
                    return new Uri(_cachedImageUrl.Value ?? DefaultImageUrl,
                        UriKind.Absolute);
                }
            }
        }

        private void SetImageUrl(string fileName)
        {
            lock (this)
            {
                _cachedImageUrl.Value = $"ms-appdata:///local/images/{fileName}";
            }
        }

        public async Task LoadAsync()
        {
            string fileName = GetFileName(_sourceImageUrl);
            var imagesFolder = await OpenImagesFolderAsync();

            if (await FileDoesNotExistAsync(fileName, imagesFolder))
            {
                bool success = await DownloadFileAsync(fileName, imagesFolder, _sourceImageUrl);
                if (success)
                    SetImageUrl(fileName);
            }
            else
            {
                SetImageUrl(fileName);
            }
        }

        private static string GetFileName(string imageUri)
        {
            var sha = new Sha256Digest();
            var stream = new DigestStream(new MemoryStream(), null, sha);
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(imageUri);
            }
            byte[] buffer = new byte[sha.GetDigestSize()];
            sha.DoFinal(buffer, 0);
            string hex = BitConverter.ToString(buffer);
            string fileName = hex.Replace("-", "");
            return fileName;
        }

        private static async Task<StorageFolder> OpenImagesFolderAsync()
        {
            var imagesFolder = await ApplicationData.Current.LocalFolder
                .CreateFolderAsync("images", CreationCollisionOption.OpenIfExists);
            return imagesFolder;
        }

        private static async Task<bool> FileDoesNotExistAsync(string fileName, StorageFolder imagesFolder)
        {
            return await imagesFolder.TryGetItemAsync(fileName) == null;
        }

        private static async Task<bool> DownloadFileAsync(string fileName, StorageFolder imagesFolder, string sourceUrl)
        {
            HttpClient client = new HttpClient();
            var sourceStream = await client.GetStreamAsync(sourceUrl);

            var imageFile = await imagesFolder.CreateFileAsync(fileName,
                CreationCollisionOption.ReplaceExisting);
            try
            {
                using (var targetStream = await imageFile.OpenStreamForWriteAsync())
                {
                    await sourceStream.CopyToAsync(targetStream);
                }
                return true;
            }
            catch (Exception)
            {
                await imageFile.DeleteAsync();
                return false;
            }
        }
    }
}
