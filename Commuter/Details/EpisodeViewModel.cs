using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commuter.Details
{
    public class EpisodeViewModel
    {
        private readonly Episode _episode;

        public EpisodeViewModel(Episode episode)
        {
            _episode = episode;
        }

        public string Title => _episode.Title;
        public DateTime PublishDate => _episode.PublishDate;
    }
}
