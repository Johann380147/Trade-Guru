using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace TradeGuru.Managers
{
    public class DebugManager
    {
        private StackPanel panel { get; set; }

        public DebugManager(StackPanel panel)
        {
            this.panel = panel;
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
            
            panel.Children.Add(expander);

            return isCaptchaActivated;
        }
    }
}
