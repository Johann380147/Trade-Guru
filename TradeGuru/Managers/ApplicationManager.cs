using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeGuru.Classes;
using TradeGuru.Views;

namespace TradeGuru.Managers
{
    public static class ApplicationManager
    {
        private static NotificationManager NotificationManager { get; set; }
        private static SearchManager SearchManager { get; set; }
        private static HistoryManager HistoryManager { get; set; }
        private static DebugManager DebugManager { get; set; }

        public static bool HasNewSalesAlert { get; set; } = false;
        private static Wrappers.Boolean isCaptchaActivated { get; set; } = new Wrappers.Boolean(false);
        public static bool IsCaptchaActivated
        {
            get { return isCaptchaActivated.Value; }
            set { 
                isCaptchaActivated.Value = value;
                SearchManager.ToggleContinueButton(value);
            }
        }

        public static void Initialize(MainWindow window)
        {
            NotificationManager = new NotificationManager(window);
            SearchManager = SearchManager.Create(window.Search, isCaptchaActivated);
            HistoryManager = HistoryManager.Create(window.History);
            DebugManager = DebugManager.Create(window.Debug);
        }

        public static async Task Begin()
        {
            while (true)
            {
                var searchObjects = new List<SearchObject>();
                searchObjects.AddRange(SearchManager.SearchList);
                var setting = SettingsManager.CurrentSetting;

                if (IsCaptchaActivated == false)
                {
                    foreach (var obj in searchObjects)
                    {
                        ItemList items = null;

                        await Task.Run(async () =>
                        {
                            items = await Scraper.GetItems(obj.url, obj.last_seen_max_minutes, setting.PageInterval);
                        });

                        IsCaptchaActivated = DebugManager.AddRawHtml(new Log(obj.pattern, items.queryDate, items.rawHtml));
                        if (IsCaptchaActivated == true)
                        {
                            NotificationManager.ShowDesktopNotification("Captcha Activated! Go to website and solve captcha to continue.");
                            break;
                        }
                        else if (items.Count > 0)
                        {
                            HistoryManager.AddItems(items);
                            HasNewSalesAlert = true;
                            NotificationManager.ShowDesktopNotification(items);
                        }
                        await Task.Delay(TimeSpan.FromSeconds(setting.ItemInterval));
                    }
                }
                await Task.Delay(TimeSpan.FromMinutes(setting.SearchFrequency));
            }
        }

        public static void SaveActivity()
        {
            if (SettingsManager.CurrentSetting.SaveSearches == true)
                SearchManager.SaveActiveSearches();
            if (SettingsManager.CurrentSetting.SaveHistory == true)
                HistoryManager.SaveHistory();
            if (SettingsManager.CurrentSetting.SaveLog == true)
                DebugManager.SaveLog();
        }
    }
}
