﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commuter.Search
{
    class SearchResult
    {
        private readonly float _quality = 1.0f;

        public float Quality
        {
            get { return _quality; }
        }
    }
}
