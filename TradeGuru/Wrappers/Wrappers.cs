using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeGuru
{
    public static class Wrappers
    {
        public class Boolean
        {
            public bool Value { get; set; } = false;

            public Boolean(bool value)
            {
                this.Value = value;
            }
        }
    }
}
