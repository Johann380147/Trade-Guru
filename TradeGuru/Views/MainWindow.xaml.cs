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
        private const int MINUTES_BETWEEN_REQUESTS = 5;
        private const int SECONDS_BETWEEN_SEARCH_OBJECTS = 60;
        private const int SECONDS_BETWEEN_PAGE_REQUESTS = 20;

        private bool isCaptchaActivated = false;
        private bool IsCaptchaActivated {
            get { return isCaptchaActivated; } 
            set 
            {
                isCaptchaActivated = value;
                if (value == true)
                {
                    ContinueButton.Visibility = Visibility.Visible;
                    WarningText.Visibility = Visibility.Visible;
                }
                else
                {
                    ContinueButton.Visibility = Visibility.Hidden;
                    WarningText.Visibility = Visibility.Hidden;
                }
            } }
        private Boolean AutoScroll = true;
        private Boolean debugAutoScroll = true;
        System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();

        private NotificationManager NotificationManager { get; set; }
        private SearchManager SearchManager { get; set; }
        private HistoryManager HistoryManager { get; set; }
        private DebugManager DebugManager { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            NotificationManager = new NotificationManager(this);
            SearchManager = new SearchManager(ActiveSearchesPanel);
            HistoryManager = new HistoryManager(ItemsPanel);
            DebugManager = new DebugManager(DebugPanel);

            using (var stream = Application.GetResourceStream(new Uri("pack://application:,,,/Resources/scales.ico")).Stream)
            {
                ni.Icon = new System.Drawing.Icon(stream);
            }
            ni.Visible = false;
            ni.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                    ni.Visible = false;
                };

            StartRequest();
        }

        private async Task StartRequest()
        {
            var searchObjects = SearchManager.serializedSearchList;

            while (true)
            {
                if (IsCaptchaActivated == false)
                {
                    foreach (var obj in searchObjects)
                    {
                        ItemList items = null;

                        await Task.Run(async () =>
                        {
                            items = await Scraper.GetItems(obj.url, obj.last_seen_max_minutes, SECONDS_BETWEEN_PAGE_REQUESTS);
                        });

                        IsCaptchaActivated = DebugManager.AddRawHtml(items.rawHtml, items.queryDate, obj.pattern);
                        if (IsCaptchaActivated == true)
                        {
                            NotificationManager.ShowDesktopNotification("Captcha Activated! Go to website and solve captcha to continue.");
                            break;
                        }
                        else if (items.Count > 0)
                        {
                            HistoryManager.AddItems(items);
                            NotificationManager.ShowDesktopNotification(items);
                        }
                        await Task.Delay(TimeSpan.FromSeconds(SECONDS_BETWEEN_SEARCH_OBJECTS));
                    }
                }
                await Task.Delay(TimeSpan.FromMinutes(MINUTES_BETWEEN_REQUESTS));
            }
        }


        #region Events Handlers

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Serializer.SerializeItemList(HistoryManager.serializedHistoryList);
            Serializer.SerializeSearchObjectList(SearchManager.serializedSearchList);
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
            {
                this.Hide();
                ni.Visible = true;
            }

            base.OnStateChanged(e);
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange == 0)
            {   // Content unchanged : user scroll event
                if (itemsScrollViewer.VerticalOffset == itemsScrollViewer.ScrollableHeight)
                {   // Scroll bar is in bottom
                    // Set auto-scroll mode
                    AutoScroll = true;
                }
                else
                {   // Scroll bar isn't in bottom
                    // Unset auto-scroll mode
                    AutoScroll = false;
                }
            }

            // Content scroll event : auto-scroll eventually
            if (AutoScroll && e.ExtentHeightChange != 0)
            {   // Content changed and auto-scroll mode set
                // Autoscroll
                itemsScrollViewer.ScrollToVerticalOffset(itemsScrollViewer.ExtentHeight);
            }
        }

        private void DebugScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange == 0)
            {   // Content unchanged : user scroll event
                if (DebugScrollViewer.VerticalOffset == DebugScrollViewer.ScrollableHeight)
                {   // Scroll bar is in bottom
                    // Set auto-scroll mode
                    debugAutoScroll = true;
                }
                else
                {   // Scroll bar isn't in bottom
                    // Unset auto-scroll mode
                    debugAutoScroll = false;
                }
            }

            // Content scroll event : auto-scroll eventually
            if (debugAutoScroll && e.ExtentHeightChange != 0)
            {   // Content changed and auto-scroll mode set
                // Autoscroll
                DebugScrollViewer.ScrollToVerticalOffset(DebugScrollViewer.ExtentHeight);
            }
        }

        private void AddSearchObjectButton_Click(object sender, RoutedEventArgs e)
        {
            Wrappers.Boolean isConfirmed = new Wrappers.Boolean(false);
            var searchObject = new SearchObject();
            var addSearchObjectWindow = new AddSearchObject(searchObject, isConfirmed);
            addSearchObjectWindow.ShowDialog();

            if (isConfirmed.Value)
            {
                searchObject.url = UrlBuilder.Build(searchObject);
                SearchManager.AddObject(searchObject);
            }
        }

        private void ClearHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            ItemsPanel.Children.Clear();
            HistoryManager.serializedHistoryList.Clear();
        }

        private void ClearDebugButton_Click(object sender, RoutedEventArgs e)
        {
            DebugPanel.Children.Clear();
        }

        private void BrowserTab_Selected(object sender, RoutedEventArgs e)
        {
            IsCaptchaActivated = false;
            Tab_Selected(sender, e);
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e) 
        {
            IsCaptchaActivated = false;
        }

        private void Tab_Selected(object sender, RoutedEventArgs e)
        {
            var tab = (TabItem)sender;
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
