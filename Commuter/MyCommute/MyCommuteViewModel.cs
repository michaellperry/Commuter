using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commuter.MyCommute
{
    class MyCommuteViewModel
    {
        private Subscriptions.SubscriptionService _subscriptions;
        private Search.SearchService _search;

        public MyCommuteViewModel(
            Subscriptions.SubscriptionService subscriptions,
            Search.SearchService search)
        {
            _subscriptions = subscriptions;
            _search = search;
        }

        public void ManageSubscriptions()
        {
            _subscriptions.ManagingSubscriptions = true;
            _search.ClearSearchResults();
        }
    }
}
