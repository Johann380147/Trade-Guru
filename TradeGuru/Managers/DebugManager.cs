using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using TradeGuru.Classes;
using TradeGuru.Views;

namespace TradeGuru.Managers
{
    public class DebugManager
    {
        private static DebugManager debugManager { get; set; }
        private DebugUserControl control { get; set; }
        private List<Log> LogList { get; set; }

        private DebugManager(DebugUserControl control)
        {
            this.control = control;
            this.LogList = new List<Log>();

            var logList = Serializer.DeserializeObjectList<Log>(SettingsManager.CurrentSetting.LogFileLocation);
            if (logList != null)
            {
                AddRawHtml(logList, true);
            }
        }

        public static DebugManager Create(DebugUserControl control)
        {
            debugManager = debugManager ?? new DebugManager(control);
            return debugManager;
        }

        public bool AddRawHtml(Log log, bool suppressCaptchaWarning = false)
        {
            return AddRawHtml(new List<Log> { log }, suppressCaptchaWarning);
        }

        public bool AddRawHtml(List<Log> logList, bool suppressCaptchaWarning = false)
        {
            LogList.AddRange(logList);
            bool isCaptchaActivated = false;

            foreach (var log in logList)
            {
                string queryName = log.queryName;
                string queryDate = log.queryDate;
                string html = log.html;

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

                control.Panel.Children.Add(expander);
            }
            
            return suppressCaptchaWarning == true ? false : isCaptchaActivated;
        }

        public void SaveLog()
        {
            Serializer.SerializeObjectList(SettingsManager.CurrentSetting.LogFileLocation, LogList);
        }

        public void ClearLog()
        {
            LogList.Clear();
        }
    }
}
