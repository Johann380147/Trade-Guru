using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TradeGuru
{
    [Serializable()]
    public class Item
    {
        public enum Rarity { White, Green, Blue, Purple, Yellow, Orange }

        public string name { get; set; }
        public string trait { get; set; }
        public Rarity rarity { get; set; }
        public string location { get; set; }
        public int amount { get; set; }
        public double price { get; set; }
        public int last_seen { get; set; }

        public override string ToString()
        {
            string text = "";
            text += name + "\n";
            text += trait + "\n";
            text += location + "\n";
            text += price.ToString() + "\n";
            text += last_seen.ToString() + "\n";

            return text;
        }
    }
}
