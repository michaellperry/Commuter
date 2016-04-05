using Assisticant.Fields;
using Commuter.MyCommute;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.IO;
using RoverMob.Messaging;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage;

namespace Commuter.Media
{
    public class MediaDownloader
    {
        private readonly CommuterApplication _application;
        private readonly Queue _queue;

        private Observable<bool> _started = new Observable<bool>();

        public MediaDownloader(CommuterApplication application, Queue queue)
        {
            _application = application;
            _queue = queue;
        }

        public bool ShouldStart => !_started.Value && !_queue.IsDownloaded;

        public async Task Start()
        {
            if (!_started.Value)
            {
                try
                {
                    _started.Value = true;

                    using (var client = new HttpClient())
                    using (var request = new HttpRequestMessage(HttpMethod.Get, _queue.MediaUrl))
                    {
                        var response = await client.SendAsync(request);
                        if (response.IsSuccessStatusCode)
                        {
                            var mediaFolder = await ApplicationData.Current.LocalFolder
                                .CreateFolderAsync("media", CreationCollisionOption.OpenIfExists);
                            var fileName = GetFileName(_queue.MediaUrl);
                            var mediaFile = await mediaFolder.CreateFileAsync(fileName,
                                CreationCollisionOption.ReplaceExisting);
                            var outStream = await mediaFile.OpenStreamForWriteAsync();
                            await response.Content.CopyToAsync(outStream);

                            _application.EmitMessage(Message.CreateMessage(
                                null,
                                "Downloaded",
                                _queue.GetObjectId(),
                                new
                                {
                                    FileName = mediaFile.Path
                                }));
                        }
                    }
                }
                finally
                {
                    _started.Value = false;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return _queue == ((MediaDownloader)obj)._queue;
        }

        public override int GetHashCode()
        {
            return _queue.GetHashCode();
        }

        private static string GetFileName(Uri mediaUrl)
        {
            var sha = new Sha256Digest();
            var stream = new DigestStream(new MemoryStream(), null, sha);
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(mediaUrl.ToString());
            }
            byte[] buffer = new byte[sha.GetDigestSize()];
            sha.DoFinal(buffer, 0);
            string hex = BitConverter.ToString(buffer);
            string fileName = hex.Replace("-", "");
            return fileName + ".mp3";
        }
    }
}
