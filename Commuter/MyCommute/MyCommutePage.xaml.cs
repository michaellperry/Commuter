using Assisticant;
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
using Assisticant.Fields;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Commuter.MyCommute
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyCommutePage : Page
    {
        private ComputedSubscription _position;

        public MyCommutePage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _position = new Computed<TimeSpan>(() =>
                ForView.Unwrap<MyCommuteViewModel, TimeSpan>(DataContext,
                    vm => vm.Position))
                .Subscribe(SetMediaElementPosition);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            _position.Unsubscribe();
        }

        private void MediaElement_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            if (PodcastMedia.CurrentState == MediaElementState.Paused)
            {
                var position = PodcastMedia.Position;
                ForView.Unwrap<MyCommuteViewModel>(DataContext, vm =>
                    vm.Paused(position));
            }
        }

        private void SetMediaElementPosition(TimeSpan position)
        {
            PodcastMedia.Position = position;
        }
    }
}
