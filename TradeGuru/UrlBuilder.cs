using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeGuru
{
    public static class UrlBuilder
    {
        const string base_url = "https://us.tamrieltradecentre.com/pc/Trade/SearchResult?SearchType=Sell&";
        
        public static string Build(string pattern, int traitId, int qualityId, 
            bool isChampionPoint, int levelMin, int levelMax, 
            int voucherMin, int voucherMax, 
            int amountMin, int amountMax,
            double priceMin, double priceMax, int cat1Id)
        {
            return Build(pattern, traitId, qualityId, isChampionPoint, levelMin, levelMax, voucherMin, voucherMax, amountMin, amountMax, priceMin, priceMax, cat1Id, -1);
        }

        public static string Build(string pattern, int traitId, int qualityId,
            bool isChampionPoint, int levelMin, int levelMax,
            int voucherMin, int voucherMax,
            int amountMin, int amountMax,
            double priceMin, double priceMax, int cat1Id, int cat2Id)
        {
            return Build(pattern, traitId, qualityId, isChampionPoint, levelMin, levelMax, voucherMin, voucherMax, amountMin, amountMax, priceMin, priceMax, cat1Id, cat2Id, -1);
        }

        public static string Build(string pattern, int traitId, int qualityId,
            bool isChampionPoint, int levelMin, int levelMax,
            int voucherMin, int voucherMax,
            int amountMin, int amountMax,
            double priceMin, double priceMax, int cat1Id, int cat2Id, int cat3Id)
        {
            string target = base_url +
                            "ItemNamePattern=" + pattern.Replace(" ", "+") + "&" +
                            "ItemCategory1ID=" + GetIntAttributeString(cat1Id) + "&" +
                            "ItemCategory2ID=" + GetIntAttributeString(cat2Id) + "&" +
                            GetCategory3(cat3Id) +
                            "ItemTraitID=" + GetIntAttributeString(traitId) + "&" +
                            "ItemQualityID=" + GetIntAttributeString(qualityId) + "&" +
                            "IsChampionPoint=" + isChampionPoint.ToString() + "&" +
                            "LevelMin=" + GetIntAttributeString(levelMin) + "&" +
                            "LevelMax=" + GetIntAttributeString(levelMax) + "&" +
                            "MasterWritVoucherMin=" + GetIntAttributeString(voucherMin) + "&" +
                            "MasterWritVoucherMax=" + GetIntAttributeString(voucherMax) + "&" +
                            "AmountMin=" + GetIntAttributeString(amountMin) + "&" +
                            "AmountMax=" + GetIntAttributeString(amountMax) + "&" +
                            "PriceMin=" + GetDoubleAttributeString(priceMin) + "&" +
                            "PriceMax=" + GetDoubleAttributeString(priceMax) + "&" + 
                            "SortBy=Price&" +
                            "Order=asc&" +
                            "page=1";

            return target;
        }

        public static string GetNextPage(string url)
        {
            var newUrl = url;
            var pageNum = -1;
            var index = url.LastIndexOf("page=");
            try
            {
                if (index != -1)
                {
                    if (index + 5 < url.Count())
                    {
                        pageNum = url.Substring(index + 5).ToNumber() + 1;
                        if (pageNum != -1)
                        {
                            newUrl = newUrl.Insert(index + 5, pageNum.ToString());
                        }
                        else
                        {
                            newUrl = newUrl.Insert(index + 5, "2");
                        }
                    }
                    
                }
                else
                {
                    if(newUrl.EndsWith("&"))
                        newUrl = newUrl.Insert(newUrl.Count(), "page=2");
                    else
                        newUrl = newUrl.Insert(newUrl.Count(), "&page=2");
                }
            } 
            catch
            {
                if (newUrl.EndsWith("&"))
                    newUrl = newUrl.Insert(newUrl.Count(), "page=2");
                else
                    newUrl = newUrl.Insert(newUrl.Count(), "&page=2");
                return newUrl;
            }

            return newUrl;
        }

        private static string GetIntAttributeString(int attrValue)
        {
            return attrValue == -1 ? "" : attrValue.ToString();
        }

        private static string GetDoubleAttributeString(double attrValue)
        {
            return attrValue == -1 ? "" : attrValue.ToString();
        }

        private static string GetCategory3(int cat3Id)
        {
            // -1 not set, -2 does not exist
            if (cat3Id == -1)
                return "ItemCategory3ID=&";
            else if (cat3Id == -2)
                return "";
            else
                return "ItemCategory3ID=" + cat3Id + "&";
        }
    }
}
