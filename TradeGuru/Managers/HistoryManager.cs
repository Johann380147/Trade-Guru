using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace TradeGuru.Managers
{
    public class HistoryManager
    {
        private StackPanel panel { get; set; }
        public List<ItemList> serializedHistoryList { get; set; }

        public HistoryManager(StackPanel panel)
        {
            this.panel = panel;
            this.serializedHistoryList = new List<ItemList>();

            var itemList = Serializer.GetSerializedItemList();
            if (itemList != null)
            {
                AddItems(itemList);
            }
        }

        public void AddItems(ItemList items)
        {
            AddItems(new List<ItemList> { items });
        }

        public void AddItems(List<ItemList> lst)
        {
            serializedHistoryList.AddRange(lst);

            foreach (var items in lst)
            {
                panel.Children.Add(new Separator());

                var itemText = new TextBlock();
                itemText.Margin = new Thickness(5);
                itemText.Inlines.Add(items.queryDate);
                panel.Children.Add(itemText);

                foreach (var item in items)
                {
                    itemText = new TextBlock();
                    itemText.Margin = new Thickness(5);
                    itemText.TextWrapping = TextWrapping.Wrap;

                    var run = new Run(item.name + "\n");
                    run.Foreground = Item.GetBrushForItemQuality(item.quality);

                    var bold = new Bold(run);
                    itemText.Inlines.Add(bold);
                    itemText.Inlines.Add(String.Format("Price: {0:#,##0.##}\n", item.price));
                    itemText.Inlines.Add(String.Format("Amount: {0}x\n", item.amount));
                    itemText.Inlines.Add(item.location + "\n");

                    var last_seen = item.last_seen;
                    if (last_seen == 0)
                        itemText.Inlines.Add(item.last_seen.ToString() + " seconds ago" + "\n");
                    else if (last_seen < 60)
                        itemText.Inlines.Add(last_seen.ToString() + " minutes ago" + "\n");
                    else if (last_seen >= 60 && last_seen < 1440)
                        itemText.Inlines.Add((last_seen / 60).ToString() + " hours ago" + "\n");
                    else if (last_seen >= 1440 && last_seen < 10080)
                        itemText.Inlines.Add((last_seen / 60 / 24).ToString() + " days ago" + "\n");

                    panel.Children.Add(itemText);
                }
            }

        }
    }
}
