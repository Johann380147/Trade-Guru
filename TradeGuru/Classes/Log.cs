using System;

namespace TradeGuru.Classes
{
    [Serializable()]
    public class Log
    {
        public string queryName { get; set; }
        public string queryDate { get; set; }
        private byte[] _html;
        public string html
        {
            get { return Serializer.Unzip(_html); }
            set { this._html = Serializer.Zip(value); }
        }

        public Log(string queryName, string queryDate, string html)
        {
            this.queryName = queryName;
            this.queryDate = queryDate;
            this.html = html;
        }
    }
}
