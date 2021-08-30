using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using TradeGuru.Managers;

namespace TradeGuru.Views
{
    /// <summary>
    /// Interaction logic for DebugUserControl.xaml
    /// </summary>
    public partial class DebugUserControl : UserControl
    {
        private DebugManager DebugManager { get; set; }
        private bool AutoScroll = true;

        public DebugUserControl()
        {
            InitializeComponent();
            DebugManager = DebugManager.Create(this);
        }

        public bool AddRawHtml(string html, string queryDate, string queryName)
        {
            bool isCaptchaActivated = false;

            var queryText = new TextBlock();
            var run = new Run(queryName + " - " + queryDate);
            var bold = new Bold(run);
            queryText.Inlines.Add(bold);

            if (html.Contains("g-recaptcha"))
            {
                run = new Run("\nCaptcha Activated! Go to website and solve captcha to continue.");
                run.Foreground = Brushes.Red;
                bold = new Bold(run);
                queryText.Inlines.Add(bold);

                isCaptchaActivated = true;
            }

            Expander expander = new Expander();
            expander.Header = queryText;
            var content = new TextBlock();
            content.Inlines.Add(html);
            content.Cursor = Cursors.Hand;
            content.MouseLeftButtonUp += (sender, e) =>
            {
                expander.IsExpanded = !expander.IsExpanded;
            };
            expander.Content = content;

            Panel.Children.Add(expander);

            return isCaptchaActivated;
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange == 0)
            {   // Content unchanged : user scroll event
                if (ScrollViewer.VerticalOffset == ScrollViewer.ScrollableHeight)
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
                ScrollViewer.ScrollToVerticalOffset(ScrollViewer.ExtentHeight);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Panel.Children.Clear();
            DebugManager.ClearLog();
        }
    }
}
