using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeGuru
{
    class SearchObject
    {
        public string url { get; set; }
        public double price_min { get; set; }
        public double price_max { get; set; }
        public int last_seen_max_minutes { get; set; }
    }
}
