using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Rhino.Licensing.AdminTool.Controls
{
    public class FXPopup : Popup
    {
        public static readonly RoutedEvent PopupClosedEvent;
        public static readonly RoutedEvent PopupOpenedEvent;

        static FXPopup()
        {
            PopupOpenedEvent = EventManager.RegisterRoutedEvent("PopupOpened", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FXPopup));
            PopupClosedEvent = EventManager.RegisterRoutedEvent("PopupClosed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FXPopup));
        }

        protected override void OnClosed(EventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(PopupClosedEvent, this));
            base.OnClosed(e);
        }

        protected override void OnOpened(EventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(PopupOpenedEvent, this));
            base.OnOpened(e);
        }
    }
}