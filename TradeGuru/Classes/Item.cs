using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Media;

namespace TradeGuru
{
    [Serializable()]
    public class Item
    {
        public enum Quality { Normal, Fine, Superior, Epic, Legendary, Mythic }

        public string name { get; set; }
        public string trait { get; set; }
        public Quality quality { get; set; }
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

        public static Brush GetBrushForItemQuality(Quality quality)
        {
            if (quality == Item.Quality.Normal)
                return Brushes.Gray;
            else if (quality == Item.Quality.Fine)
                return Brushes.Green;
            else if (quality == Item.Quality.Superior)
                return Brushes.Blue;
            else if (quality == Item.Quality.Epic)
                return Brushes.Purple;
            else if (quality == Item.Quality.Legendary)
                return Brushes.Gold;
            else if (quality == Item.Quality.Mythic)
                return Brushes.OrangeRed;
            else
                return Brushes.Gray;
        }
    }
}
