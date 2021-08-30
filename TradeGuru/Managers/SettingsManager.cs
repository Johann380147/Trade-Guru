using TradeGuru.Classes;

namespace TradeGuru.Managers
{
    public static class SettingsManager
    {
        private static Setting currentSetting { get; set; }
        public static Setting CurrentSetting
        {
            get
            {
                return CurrentSetting = currentSetting ?? new Setting
                {
                    SearchFrequency = Properties.Settings.Default.MINUTES_BETWEEN_REQUESTS,
                    ItemInterval = Properties.Settings.Default.SECONDS_BETWEEN_SEARCH_OBJECTS,
                    PageInterval = Properties.Settings.Default.SECONDS_BETWEEN_PAGE_REQUESTS,

                    SaveSearches = Properties.Settings.Default.SAVE_ACTIVE_SEARCHES,
                    SaveHistory = Properties.Settings.Default.SAVE_ITEM_HISTORY,
                    SaveLog = Properties.Settings.Default.SAVE_DEBUG_LOG,

                    SearchFileLocation = Properties.Settings.Default.SAVED_SEARCH_FILENAME,
                    HistoryFileLocation = Properties.Settings.Default.SAVED_HISTORY_FILENAME,
                    LogFileLocation = Properties.Settings.Default.SAVED_LOG_FILENAME
                };
            }
            private set
            {
                currentSetting = value;
            }
        }

        private static Setting DefaultSettings = new Setting
        {
            SearchFrequency = 5,
            ItemInterval = 60,
            PageInterval = 20,

            SaveSearches = true,
            SaveHistory = true,
            SaveLog = false,

            SearchFileLocation = "../../../../Search.bin",
            HistoryFileLocation = "../../../../History.bin",
            LogFileLocation = "../../../../Log.bin"
        };

        public static void Save(Setting setting)
        {
            Properties.Settings.Default.MINUTES_BETWEEN_REQUESTS = setting.SearchFrequency;
            Properties.Settings.Default.SECONDS_BETWEEN_SEARCH_OBJECTS = setting.ItemInterval;
            Properties.Settings.Default.SECONDS_BETWEEN_PAGE_REQUESTS = setting.PageInterval;

            Properties.Settings.Default.SAVE_ACTIVE_SEARCHES = setting.SaveSearches;
            Properties.Settings.Default.SAVE_ITEM_HISTORY = setting.SaveHistory;
            Properties.Settings.Default.SAVE_DEBUG_LOG = setting.SaveLog;

            Properties.Settings.Default.SAVED_SEARCH_FILENAME = setting.SearchFileLocation;
            Properties.Settings.Default.SAVED_HISTORY_FILENAME = setting.HistoryFileLocation;
            Properties.Settings.Default.SAVED_LOG_FILENAME = setting.LogFileLocation;

            Properties.Settings.Default.Save();

            CurrentSetting = setting;
        }

        public static Setting Undo()
        {
            return new Setting
            {
                SearchFrequency = Properties.Settings.Default.MINUTES_BETWEEN_REQUESTS,
                ItemInterval = Properties.Settings.Default.SECONDS_BETWEEN_SEARCH_OBJECTS,
                PageInterval = Properties.Settings.Default.SECONDS_BETWEEN_PAGE_REQUESTS,

                SaveSearches = Properties.Settings.Default.SAVE_ACTIVE_SEARCHES,
                SaveHistory = Properties.Settings.Default.SAVE_ITEM_HISTORY,
                SaveLog = Properties.Settings.Default.SAVE_DEBUG_LOG,

                SearchFileLocation = Properties.Settings.Default.SAVED_SEARCH_FILENAME,
                HistoryFileLocation = Properties.Settings.Default.SAVED_HISTORY_FILENAME,
                LogFileLocation = Properties.Settings.Default.SAVED_LOG_FILENAME
            };
        }

        public static void Reset()
        {
            Properties.Settings.Default.MINUTES_BETWEEN_REQUESTS = DefaultSettings.SearchFrequency;
            Properties.Settings.Default.SECONDS_BETWEEN_SEARCH_OBJECTS = DefaultSettings.ItemInterval;
            Properties.Settings.Default.SECONDS_BETWEEN_PAGE_REQUESTS = DefaultSettings.PageInterval;

            Properties.Settings.Default.SAVE_ACTIVE_SEARCHES = DefaultSettings.SaveSearches;
            Properties.Settings.Default.SAVE_ITEM_HISTORY = DefaultSettings.SaveHistory;
            Properties.Settings.Default.SAVE_DEBUG_LOG = DefaultSettings.SaveLog;

            Properties.Settings.Default.SAVED_SEARCH_FILENAME = DefaultSettings.SearchFileLocation;
            Properties.Settings.Default.SAVED_HISTORY_FILENAME = DefaultSettings.HistoryFileLocation;
            Properties.Settings.Default.SAVED_LOG_FILENAME = DefaultSettings.LogFileLocation;

            Properties.Settings.Default.Save();

            CurrentSetting = DefaultSettings;
        }
    }
}
