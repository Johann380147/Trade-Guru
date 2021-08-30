using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TradeGuru.Managers;

namespace TradeGuru.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();

        public MainWindow()
        {
            InitializeComponent();

            SetNormalIcon();
            ni.Visible = false;
            ni.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    HideIcon();
                };

            ApplicationManager.Initialize(this);
            ApplicationManager.Begin();
        }

        public void ShowIcon()
        {
            this.Hide();
            ni.Visible = true;
        }

        public void HideIcon()
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            ni.Visible = false;

            if (ApplicationManager.IsCaptchaActivated == true && MainTab.SelectedItem == BrowserTab)
            {
                ApplicationManager.IsCaptchaActivated = false;
                SetNormalIcon();
            }
            else if (ApplicationManager.HasNewSalesAlert == true && MainTab.SelectedItem == HistoryTab)
            {
                ApplicationManager.HasNewSalesAlert = false;
                SetNormalIcon();
            }
        }

        public void SetNormalIcon()
        {
            ni.Icon = Helpers.GetIconFromUri(new Uri("pack://application:,,,/Resources/scales.ico"));
        }

        public void SetNotifyIcon()
        {
            ni.Icon = Helpers.GetIconFromUri(new Uri("pack://application:,,,/Resources/scales_notify.ico"));
        }

        public bool isBrowserTabSelected()
        {
            return MainTab.SelectedItem == BrowserTab;
        }

        public bool isHistoryTabSelected()
        {
            return MainTab.SelectedItem == HistoryTab;
        }

        #region Events Handlers

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ApplicationManager.SaveActivity();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
            {
                ShowIcon();
            }

            base.OnStateChanged(e);
        }

        private void Tab_Selected(object sender, RoutedEventArgs e)
        {
            var tab = (TabItem)sender;
            if (tab == BrowserTab)
            {
                ApplicationManager.IsCaptchaActivated = false;
                SetNormalIcon();
            }
            else if (tab == HistoryTab)
            {
                ApplicationManager.HasNewSalesAlert = false;
                SetNormalIcon();
            }
            tab.Foreground = new SolidColorBrush(Color.FromRgb(45, 45, 48));
        }

        private void Tab_Unselected(object sender, RoutedEventArgs e)
        {
            var tab = (TabItem)sender;
            tab.Foreground = new SolidColorBrush(Color.FromRgb(224, 224, 224));
        }

        private void Tab_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var tab = (TabItem)sender;
            if (MainTab.SelectedItem == tab) return;
            tab.Foreground = new SolidColorBrush(Color.FromRgb(45, 45, 48));
        }

        private void Tab_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var tab = (TabItem)sender;
            if (MainTab.SelectedItem == tab) return;
            tab.Foreground = new SolidColorBrush(Color.FromRgb(224, 224, 224));
        }

        #endregion

    }
}
