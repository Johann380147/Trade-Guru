using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeGuru
{
    [Serializable()]
    public class ItemList : List<Item>
    {
        public string queryDate { get; set; }
    }
}
