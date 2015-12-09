using Assisticant;
using Assisticant.Fields;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Commuter
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private Frame _rootFrame;
        private Computed<ImmutableList<Type>> _pageStack;
        private ComputedSubscription _pageStackSubscription;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync();
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            _pageStack = new Computed<ImmutableList<Type>>(ComputePageStack);
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            _rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (_rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                _rootFrame = new Frame();

                _rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = _rootFrame;
            }
            // Ensure the current window is active
            Window.Current.Activate();

            _pageStackSubscription = _pageStack.Subscribe(NavigatePageStack);

            ForView.Initialize();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private ImmutableList<Type> ComputePageStack()
        {
            var pages = ImmutableList<Type>.Empty;
            var viewModelLocator = (ViewModelLocator)Resources["Locator"];
            var model = viewModelLocator.Model;
            if (model != null)
            {
                if (!model.Subscriptions.Any())
                {
                    pages = pages.Add(typeof(Onboarding.OnboardingPage));
                    if (model.SearchService.SearchResults.Any())
                    {
                        pages = pages.Add(typeof(Search.SearchPage));
                    }
                }
                else
                {
                    pages = pages.Add(typeof(MyCommute.MyCommutePage));
                    if (model.ManagingSubscriptions)
                    {
                        pages = pages.Add(typeof(Subscriptions.SubscriptionsPage));
                        if (model.SearchService.SearchResults.Any())
                        {
                            pages = pages.Add(typeof(Search.SearchPage));
                        }
                    }
                }
            }
            return pages;
        }

        private void NavigatePageStack(ImmutableList<Type> pages)
        {
            while (_rootFrame.CanGoBack)
                _rootFrame.GoBack();
            if (pages.Any())
                _rootFrame.Navigate(pages.Last());
        }
    }
}
