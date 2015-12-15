using Assisticant;
using Assisticant.Fields;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Commuter.Search
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        private Computed<string> _state;
        private ComputedSubscription _stateSubscription;
        private bool _isInitialized = false;

        public SearchPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += SearchPage_BackRequested;
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= SearchPage_BackRequested;
            base.OnNavigatingFrom(e);
        }

        private void SearchPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            ForView.Unwrap<SearchViewModel>(DataContext, vm =>
            {
                vm.GoBack();
                e.Handled = true;
            });
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ForView.Unwrap<SearchViewModel>(DataContext, vm =>
            {
                vm.QuerySubmitted();
            });
            ResultsList.Focus(FocusState.Keyboard);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ForView.Unwrap<SearchViewModel>(DataContext, vm =>
            {
                _state = new Computed<string>(() => vm.HasSelectedSearchResult
                    ? "ShowDetail"
                    : "ShowMaster");
                _stateSubscription = _state.Subscribe(s =>
                {
                    VisualStateManager.GoToState(this, s, _isInitialized);
                    _isInitialized = true;
                });
            });
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_stateSubscription != null)
            {
                _stateSubscription.Unsubscribe();
                _state.Dispose();
            }
        }
    }
}
