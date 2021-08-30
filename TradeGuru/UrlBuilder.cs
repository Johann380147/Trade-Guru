using System.Linq;

namespace TradeGuru
{
    public static class UrlBuilder
    {
        const string base_url = "https://us.tamrieltradecentre.com/pc/Trade/SearchResult?SearchType=Sell&";
        
        public static string Build(SearchObject obj)
        {
            string target = base_url +
                            "ItemNamePattern=" + obj.pattern.Replace(" ", "+") + "&" +
                            "ItemCategory1ID=" + GetIntAttributeString(obj.category1Id) + "&" +
                            "ItemCategory2ID=" + GetIntAttributeString(obj.category2Id) + "&" +
                            GetCategory3(obj.category3Id) +
                            "ItemTraitID=" + GetIntAttributeString(obj.traitId) + "&" +
                            "ItemQualityID=" + GetIntAttributeString(obj.qualityId) + "&" +
                            "IsChampionPoint=" + obj.isChampionPoint.ToString() + "&" +
                            "LevelMin=" + GetIntAttributeString(obj.level_min) + "&" +
                            "LevelMax=" + GetIntAttributeString(obj.level_max) + "&" +
                            "MasterWritVoucherMin=" + GetIntAttributeString(obj.voucher_min) + "&" +
                            "MasterWritVoucherMax=" + GetIntAttributeString(obj.voucher_max) + "&" +
                            "AmountMin=" + GetIntAttributeString(obj.amount_min) + "&" +
                            "AmountMax=" + GetIntAttributeString(obj.amount_max) + "&" +
                            "PriceMin=" + GetDoubleAttributeString(obj.price_min) + "&" +
                            "PriceMax=" + GetDoubleAttributeString(obj.price_max) + "&" + 
                            (obj.sortType == SearchObject.SortType.Price ? "SortBy=Price&" : "SortBy=LastSeen&") +
                            (obj.sortType == SearchObject.SortType.Price ? "Order=asc&" : "Order=desc&") +
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
                            newUrl = newUrl.Remove(index + 5);
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
