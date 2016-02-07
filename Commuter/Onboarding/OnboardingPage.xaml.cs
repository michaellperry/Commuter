using Assisticant;
using Assisticant.Fields;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Commuter.Onboarding
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OnboardingPage : Page
    {
        private Computed<string> _lastException;
        private ComputedSubscription _subscription;

        public OnboardingPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ForView.Unwrap<OnboardingViewModel>(DataContext, vm =>
            {
                _lastException = new Computed<string>(() => vm.LastException);
                _subscription = _lastException.Subscribe(v => { if (v != null) { ShowError.Begin(); } });
            });
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_subscription != null)
            {
                _subscription.Unsubscribe();
                _subscription = null;
            }
            if (_lastException != null)
            {
                _lastException.Dispose();
                _lastException = null;
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ForView.Unwrap<OnboardingViewModel>(DataContext, vm =>
            {
                vm.QuerySubmitted();
            });
        }
    }
}
