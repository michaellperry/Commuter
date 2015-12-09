using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Commuter.MyCommute
{
    class CustomMediaTransportControls : MediaTransportControls
    {
        public static DependencyProperty ManageSubscriptionsCommandProperty =
            DependencyProperty.Register(
                "ManageSubscriptionsCommand",
                typeof(ICommand),
                typeof(CustomMediaTransportControls),
                new PropertyMetadata(null));

        public CustomMediaTransportControls()
        {
            DefaultStyleKey = typeof(CustomMediaTransportControls);
        }

        public ICommand ManageSubscriptionsCommand
        {
            get { return (ICommand)GetValue(ManageSubscriptionsCommandProperty); }
            set { SetValue(ManageSubscriptionsCommandProperty, value); }
        }
    }
}
