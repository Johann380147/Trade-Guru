using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

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

        public static string UncleanString(this string str)
        {
            return str.Replace(" ", "_");
        }

        public static Type GetType(this string typeName)
        {
            return Type.GetType(typeName);
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

        public static double ToDouble(this string str)
        {
            if (str == String.Empty)
                return 1;
            else
            {
                var num = Convert.ToDouble(str);
                return num < 1 ? 1 : num;
            }
        }

        public static string ToText(this int num, bool addThousandthSeparator = false)
        {
            if (num == -1)
            {
                return String.Empty;
            }
            else
            {
                if (addThousandthSeparator == true)
                {
                    return String.Format("{0:#,##0.##}", num);
                }
                else
                {
                    return num.ToString();
                }
            }
        }

        public static string ToText(this double num, bool addThousandthSeparator = false)
        {
            if (num == -1)
            {
                return String.Empty;
            }
            else
            {
                if (addThousandthSeparator == true)
                {
                    return String.Format("{0:#,##0.##}", num);
                }
                else
                {
                    return num.ToString();
                }
            }
        }

        public static System.Drawing.Icon GetIconFromUri(Uri uri)
        {
            using (var stream = Application.GetResourceStream(uri).Stream)
            {
                return new System.Drawing.Icon(stream);
            }
        }
    }
}
