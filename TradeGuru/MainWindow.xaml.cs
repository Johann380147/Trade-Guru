using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TradeGuru
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Boolean AutoScroll = true;
        private static List<ItemList> serializedItemList = new List<ItemList>();

        public MainWindow()
        {
            InitializeComponent();
            var lst = Serializer.GetSerializedItemList();
            if (lst != null)
            {
                AddItemsToHistory(ItemsPanel, lst);
            }
            StartEvent(ItemsPanel);
        }

        private static async Task StartEvent(StackPanel panel)
        {
            List<SearchObject> searchObjects = new List<SearchObject> {
                new SearchObject { url = "https://us.tamrieltradecentre.com/pc/Trade/SearchResult?SearchType=Sell&ItemID=&ItemNamePattern=sorrow&ItemCategory1ID=0&ItemCategory2ID=1&ItemCategory3ID=5&ItemTraitID=&ItemQualityID=&IsChampionPoint=false&LevelMin=&LevelMax=&MasterWritVoucherMin=&MasterWritVoucherMax=&AmountMin=&AmountMax=&PriceMin=&PriceMax=&SortBy=Price&Order=asc",
                price_min = 0,
                price_max = 10000,
                last_seen_max_minutes = 10 },

                 new SearchObject { url = "https://us.tamrieltradecentre.com/pc/Trade/SearchResult?SearchType=Sell&ItemID=&ItemNamePattern=sorrow&ItemCategory1ID=0&ItemCategory2ID=0&ItemCategory3ID=1&ItemTraitID=&ItemQualityID=&IsChampionPoint=false&LevelMin=&LevelMax=&MasterWritVoucherMin=&MasterWritVoucherMax=&AmountMin=&AmountMax=&PriceMin=&PriceMax=&SortBy=Price&Order=asc",
                price_min = 0,
                price_max = 100000,
                last_seen_max_minutes = 1000 }
            };
            


            while (true)
            {
                ItemList items = null;

                foreach (var obj in searchObjects)
                {
                    await Task.Run(() =>
                    {
                        items = Scraper.GetItems(obj.url, obj.price_min, obj.price_max, obj.last_seen_max_minutes);
                    });

                    if (items != null && items.Count > 0)
                    {
                        AddItemsToHistory(panel, items);

                        if (items.Count <= 5)
                        {
                            var notification = new NotificationWindow(items);
                            notification.Show();
                        }
                        else
                        {
                            var index = 0;
                            var count = 5;

                            for (int i = 0; i < items.Count; i = index)
                            {
                                index = i + 5;
                                if (index > items.Count)
                                    count = index - items.Count - 1;

                                var subset = items.GetRange(i, count);
                                var notification = new NotificationWindow(subset);
                                notification.Show();

                                await Task.Delay(TimeSpan.FromSeconds(10.5));
                            }
                        }
                    }
                }
                

                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }

        public static void AddItemsToHistory(StackPanel panel,  ItemList items)
        {
            AddItemsToHistory(panel, new List<ItemList> { items });
        }

        public static void AddItemsToHistory(StackPanel panel, List<ItemList> lst)
        {
            serializedItemList.AddRange(lst);
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
                    if (item.rarity == Item.Rarity.White)
                        run.Foreground = Brushes.Gray;
                    else if (item.rarity == Item.Rarity.Green)
                        run.Foreground = Brushes.Green;
                    else if (item.rarity == Item.Rarity.Blue)
                        run.Foreground = Brushes.Blue;
                    else if (item.rarity == Item.Rarity.Purple)
                        run.Foreground = Brushes.Purple;
                    else if (item.rarity == Item.Rarity.Yellow)
                        run.Foreground = Brushes.Yellow;
                    else if (item.rarity == Item.Rarity.Orange)
                        run.Foreground = Brushes.Orange;

                    var bold = new Bold(run);
                    itemText.Inlines.Add(bold);
                    itemText.Inlines.Add(String.Format("Price: {0:n0}\n", item.price));
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

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange == 0)
            {   // Content unchanged : user scroll event
                if (itemsScrollViewer.VerticalOffset == itemsScrollViewer.ScrollableHeight)
                {   // Scroll bar is in bottom
                    // Set auto-scroll mode
                    AutoScroll = true;
                }
                else
                {   // Scroll bar isn't in bottom
                    // Unset auto-scroll mode
                    AutoScroll = false;
                }
            }

            // Content scroll event : auto-scroll eventually
            if (AutoScroll && e.ExtentHeightChange != 0)
            {   // Content changed and auto-scroll mode set
                // Autoscroll
                itemsScrollViewer.ScrollToVerticalOffset(itemsScrollViewer.ExtentHeight);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Serializer.SerializeItemList(serializedItemList);
        }
    }
    
}
