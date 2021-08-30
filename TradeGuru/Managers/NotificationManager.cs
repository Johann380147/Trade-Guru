using System;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using TradeGuru.Views;

namespace TradeGuru.Managers
{
    public class NotificationManager
    {
        private MainWindow window { get; set; }
        private ItemList items { get; set; }
        private int start { get; set; }
        public int take { get; set; }

        public NotificationManager(MainWindow window, int start = 0, int take = 5)
        {
            this.window = window;
            this.start = start;
            this.take = take;
        }

        [DllImport("user32")]
        private static extern int FlashWindow(IntPtr hwnd, bool bInvert);

        public void ShowDesktopNotification(string text)
        {
            var notification = new NotificationWindow(text);
            notification.Cursor = Cursors.Hand;
            notification.MouseLeftButtonUp += ShowBrowserTab;
            notification.Show();

            PlayNotificationSound();
            FlashTaskbarIcon();
            if (window.isBrowserTabSelected() == false || window.IsVisible == false)
            {
                window.SetNotifyIcon();
            }
        }

        public void ShowDesktopNotification(ItemList items)
        {
            if (items.Count <= 5)
            {
                var notification = new NotificationWindow(items);
                notification.Cursor = Cursors.Hand;
                notification.MouseLeftButtonUp += ShowHistoryTab;
                notification.Show();
            }
            else
            {
                this.items = items;
                var subset = items.GetRange(start, 5);
                var notification = new NotificationWindow(subset);
                notification.Cursor = Cursors.Hand;
                notification.Closed += DesktopNotification_Closed;
                notification.MouseLeftButtonUp += ShowHistoryTab;
                notification.Show();
            }
            PlayNotificationSound();
            FlashTaskbarIcon();
            if (window.isHistoryTabSelected() == false || window.IsVisible == false )
            {
                window.SetNotifyIcon();
            }
        }

        private void DesktopNotification_Closed(object sender, EventArgs e)
        {
            start += take;
            var end = start + take;
            var _take = (end >= items.Count) ? items.Count - start : take;

            if (_take <= 0)
            {
                start = 0;
                return;
            }   

            var subset = items.GetRange(start, _take);
            var notification = new NotificationWindow(subset);
            notification.Cursor = Cursors.Hand;
            notification.Closed += DesktopNotification_Closed;
            notification.MouseLeftButtonUp += ShowHistoryTab;
            notification.Show();
        }

        private void PlayNotificationSound()
        {
            SystemSounds.Hand.Play();
        }

        private void FlashTaskbarIcon()
        {
            WindowInteropHelper wih = new WindowInteropHelper(window);
            FlashWindow(wih.Handle, true);
        }

        private void ShowHistoryTab(object sender, EventArgs e)
        {
            ShowTab(window.HistoryTab);
            window.HideIcon();
        }

        private void ShowBrowserTab(object sender, EventArgs e)
        {
            ShowTab(window.BrowserTab);
            window.HideIcon();
        }

        private void ShowTab(TabItem tabItem)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() => window.MainTab.SelectedItem = tabItem));
            window.Activate();
            window.Topmost = true;
            window.Topmost = false;
        }
    }
}
