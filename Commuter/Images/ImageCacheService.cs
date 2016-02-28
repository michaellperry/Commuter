using RoverMob.Tasks;
using System;
using System.Collections.Generic;

namespace Commuter.Images
{
    class ImageCacheService : Process
    {
        private Dictionary<string, ImageCacheCell> _cells =
            new Dictionary<string, ImageCacheCell>();

        public Uri GetCachedImageUri(Uri imageUri)
        {
            ImageCacheCell cell;
            string key = imageUri.ToString();
            if (!_cells.TryGetValue(key, out cell))
            {
                cell = new ImageCacheCell(key);
                _cells.Add(key, cell);
                Perform(() => cell.LoadAsync());
            }
            return cell.ImageUrl;
        }
    }
}
