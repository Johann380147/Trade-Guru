﻿using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TradeGuru
{
    public static class Scraper
    {
        static ScrapingBrowser _browser = new ScrapingBrowser();
        public static async Task<ItemList> GetItems(string url, int last_seen, double seconds)
        {
            var count = 1;
            var lstFilteredItems = new ItemList();
            ItemList lstItems = GetPageDetails(url);

            lstFilteredItems.rawHtml = lstItems.rawHtml;
            lstFilteredItems.queryDate = lstItems.queryDate;
            var _last_seen = last_seen == -1 ? 99999 : last_seen;

            while (lstItems != null && lstItems.Count > 0)
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
                {
                    var new_url = UrlBuilder.GetNextPage(url);
                    lstItems = GetPageDetails(new_url);
                    count++;
                }
                else
                {
                    lstItems = null;
                }

                await Task.Delay(TimeSpan.FromSeconds(seconds));
            }

            return lstFilteredItems;
        }

        private static ItemList GetPageDetails(string url)
        {

            var htmlNode = GetHtml(url);
            var lstItems = new ItemList();
            lstItems.queryDate = String.Format("{0:hh:mm:ss tt (dd MMM)}", DateTime.Now);
            lstItems.rawHtml = htmlNode.OwnerDocument.DocumentNode.OuterHtml;

            if (htmlNode.OwnerDocument.DocumentNode.SelectNodes("//tr[@class='cursor-pointer']") == null) return lstItems;

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
                    item.quality = Item.Quality.Normal;
                }
                else if (rarity == "fine")
                {
                    item.quality = Item.Quality.Fine;
                }
                else if (rarity == "superior")
                {
                    item.quality = Item.Quality.Superior;
                }
                else if (rarity == "epic")
                {
                    item.quality = Item.Quality.Epic;
                }
                else if (rarity == "legendary")
                {
                    item.quality = Item.Quality.Legendary;
                }
                else if (rarity == "mythic")
                {
                    item.quality = Item.Quality.Mythic;
                }

                var locationNodes = node.SelectNodes(".//td[@class='hidden-xs']")[1].SelectNodes("./div");
                if (locationNodes == null || locationNodes.Count == 0) continue;
                var location = locationNodes[0].InnerText;
                item.location = location.Trim().CleanInnerText();

                var guild = locationNodes[1].InnerText;
                item.location += "\n" + guild.Trim().CleanInnerText();

                var priceNode = node.SelectSingleNode(".//td[@class='gold-amount bold']").InnerText;
                var price = Regex.Match(priceNode, @"([0-9,.]+)").Value;
                if (price == String.Empty) continue;
                item.price = Convert.ToDouble(price);

                var amountNode = node.SelectSingleNode(".//td[@class='gold-amount bold']").SelectNodes("./img");
                var amount = amountNode[1].NextSibling.InnerText;
                amount = Regex.Match(amount, @"([0-9]+)").Value;
                if (amount == String.Empty) continue;
                item.amount = Convert.ToInt32(amount);

                var lastseen = node.SelectSingleNode("./td[@class='bold hidden-xs']").GetAttributeValue("data-mins-elapsed");
                lastseen = Regex.Match(lastseen, @"([0-9]+)").Value;
                if (lastseen == String.Empty) continue;
                item.last_seen = Convert.ToInt32(lastseen);

                lstItems.Add(item);
            }

            return lstItems;
        }

        private static HtmlNode GetHtml(string url)
        {
            WebPage webpage = _browser.NavigateToPage(new Uri(url));
            return webpage.Html;
        }
    }
}
