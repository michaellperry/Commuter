namespace Commuter
{
    class Model
    {
        private readonly Subscriptions.SubscriptionService _subscriptionService;
        private readonly Media.MediaCacheService _mediaCacheService;
        private readonly CommuterApplication _application;

        public Model(
            Subscriptions.SubscriptionService subscriptionService,
            Media.MediaCacheService mediaCacheService,
            CommuterApplication application)
        {
            _subscriptionService = subscriptionService;
            _mediaCacheService = mediaCacheService;
            _application = application;
        }

        public Subscriptions.SubscriptionService SubscriptionService
        {
            get { return _subscriptionService; }
        }

        public CommuterApplication Application
        {
            get { return _application; }
        }
    }
}