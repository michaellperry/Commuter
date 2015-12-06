using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commuter.Search
{
    class SearchViewModel
    {
        private Model _model;

        public SearchViewModel(Model model)
        {
            _model = model;
        }

        public void ClearSearchResults()
        {
            _model.ClearSearchResults();
        }
    }
}
