using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System.Text.RegularExpressions;

namespace TradeGuru
{
    public static class Scraper
    {
        static ScrapingBrowser _browser = new ScrapingBrowser();
        public static ItemList GetItems(string url, int last_seen)
        {
            var count = 0;
            var lstFilteredItems = new ItemList();
            ItemList lstItems = GetPageDetails(url);

            if (lstItems == null) return null;

            lstFilteredItems.queryDate = lstItems.queryDate;
            var _last_seen = last_seen == -1 ? 99999 : last_seen;

            while (lstItems != null)
            {
                foreach (var item in lstItems)
                {
                    if (item.last_seen <= _last_seen)
                    {
                        lstFilteredItems.Add(item);
                    }
                }

                // Avoid spamming the ttc website
                if (count < 3)
                    lstItems = GetPageDetails(UrlBuilder.GetNextPage(url));
                else
                    lstItems = null;
            }

            return lstFilteredItems;
        }

        static ItemList GetPageDetails(string url)
        {

            var htmlNode = GetHtml(url);
            var lstItems = new ItemList();
            lstItems.queryDate = String.Format("{0:hh:mm:ss (dd MMM)}", DateTime.Now);

            if (htmlNode.OwnerDocument.DocumentNode.SelectNodes("//tr[@class='cursor-pointer']") == null) return null;

            foreach (HtmlAgilityPack.HtmlNode node in htmlNode.OwnerDocument.DocumentNode.SelectNodes("//tr[@class='cursor-pointer']"))
            {
                var item = new Item();

                var name = node.SelectSingleNode("./td").SelectSingleNode("./div").InnerText;
                item.name = name.CleanInnerText();

                var traitNode = node.SelectSingleNode("./td").SelectSingleNode("./img");
                var trait = traitNode.GetAttributeValue("data-trait");
                item.trait = trait;

                var rarityNode = node.SelectSingleNode("./td").SelectSingleNode("./div");
                var rarity = rarityNode.GetAttributeValue("class").Replace("item-quality-", "");

                if (rarity == "normal")
                {
                    item.rarity = Item.Rarity.White;
                }
                else if (rarity == "fine")
                {
                    item.rarity = Item.Rarity.Green;
                }
                else if (rarity == "superior")
                {
                    item.rarity = Item.Rarity.Blue;
                }
                else if (rarity == "epic")
                {
                    item.rarity = Item.Rarity.Purple;
                }
                else if (rarity == "legendary")
                {
                    item.rarity = Item.Rarity.Yellow;
                }
                else if (rarity == "legendary")
                {
                    item.rarity = Item.Rarity.Orange;
                }

                var locationNodes = node.SelectNodes(".//td[@class='hidden-xs']")[1].SelectNodes("./div");
                if (locationNodes == null || locationNodes.Count == 0) continue;
                var location = locationNodes[0].InnerText;
                item.location = location.Trim().CleanInnerText();

                var guild = locationNodes[1].InnerText;
                item.location += "\n" + guild.Trim().CleanInnerText();

                var priceNode = node.SelectSingleNode(".//td[@class='gold-amount bold']").InnerText;
                var price = Regex.Match(priceNode, @"([0-9,.]+)").Value;
                item.price = Convert.ToDouble(price);

                var lastseen = node.SelectSingleNode("./td[@class='bold hidden-xs']").GetAttributeValue("data-mins-elapsed");
                item.last_seen = Convert.ToInt32(lastseen);

                lstItems.Add(item);
            }

            return lstItems;
        }

        static HtmlNode GetHtml(string url)
        {
            WebPage webpage = _browser.NavigateToPage(new Uri(url));
            return webpage.Html;
        }
    }
}
