namespace TradeGuru.Classes
{
    public class Setting
    {
        public double SearchFrequency { get; set; }
        public double ItemInterval { get; set; }
        public double PageInterval { get; set; }

        public bool SaveSearches { get; set; }
        public bool SaveHistory { get; set; }
        public bool SaveLog { get; set; }

        public string SearchFileLocation { get; set; }
        public string HistoryFileLocation { get; set; }
        public string LogFileLocation { get; set; }
    }
}
