using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeGuru
{
    public static class Helpers
    {
        public static Dictionary<int, string> EnumDictionary<T>()
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Type must be an enum");
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .ToDictionary(t => (int)(object)t, t => t.ToString().CleanString());
        }

        public static Dictionary<int, string> GetNestedTypes(Type source)
        {
            var dict = new Dictionary<int, string>();
            var types = source.GetNestedTypes();

            foreach (var type in types)
            {
                var property = type.GetField("id");
                var value = (int)property.GetRawConstantValue();

                dict[value] = type.Name.CleanString();
            }

            return dict;
        }

        public static string CleanString(this string str)
        {
            return str.Replace("_", " ");
        }

        public static int ToNumber(this string str)
        {
            if (str == String.Empty)
                return -1;
            else
            {
                return Convert.ToInt32(str.Replace("&", ""));
            }
        }
    }
}
