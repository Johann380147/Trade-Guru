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
        private static List<SearchObject> serializedSearchList = new List<SearchObject>();

        public MainWindow()
        {
            InitializeComponent();
            var searchList = Serializer.GetSerializedSearchObjectList();
            var itemList = Serializer.GetSerializedItemList();
            
            if (searchList != null)
            {
                AddToSearchPanel(ActiveSearchesPanel, searchList);
            }
            if (itemList != null)
            {
                AddItemsToHistory(ItemsPanel, itemList);
            }
            StartEvent(ItemsPanel, serializedSearchList);
        }

        private static async Task StartEvent(StackPanel panel, List<SearchObject> searchObjects)
        {
            while (true)
            {
                foreach (var obj in searchObjects)
                {
                    ItemList items = null;

                    await Task.Run(() =>
                    {
                        items = Scraper.GetItems(obj.url, obj.last_seen_max_minutes);
                    });

                    if (items == null || items.Count == 0) continue;
                    
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
                

                await Task.Delay(TimeSpan.FromMinutes(3));
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

        public static void AddToSearchPanel(DockPanel panel, SearchObject obj)
        {
            AddToSearchPanel(panel, new List<SearchObject> { obj });
        }

        public static void AddToSearchPanel(DockPanel panel, List<SearchObject> searchObjects)
        {
            serializedSearchList.AddRange(searchObjects);
            foreach (var obj in searchObjects)
            {
                var btn = new Button();
                btn.Width = 200;
                btn.Height = 150;
                btn.Margin = new Thickness(15, 15, 10, 10);
                btn.Padding = new Thickness(10);
                btn.VerticalAlignment = VerticalAlignment.Top;
                btn.VerticalContentAlignment = VerticalAlignment.Top;
                var content = obj.pattern + "\n" +
                    SearchAttributeTranslator.GetSearchCategoryText(obj.category1Id, obj.category2Id, obj.category3Id) + "\n" +
                    (obj.price_min == -1 ? "" : obj.price_min.ToString()) + "~" + (obj.price_max == -1 ? "" : obj.price_max.ToString()) + "\n" +
                    "Recency: " + obj.last_seen_max_minutes;
                btn.Content = content;
                btn.Tag = obj;
                btn.Click += (sender, e) => 
                {
                    var button = sender as Button;
                    var searchObj = button.Tag as SearchObject;
                };
                
                panel.Children.Add(btn);
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
            Serializer.SerializeSearchObjectList(serializedSearchList);
        }

        private void AddSearchObjectButton_Click(object sender, RoutedEventArgs e)
        {
            Wrappers.Boolean isConfirmed = new Wrappers.Boolean(false);
            var searchObject = new SearchObject();
            var addSearchObjectWindow = new AddSearchObject(searchObject, isConfirmed);
            addSearchObjectWindow.ShowDialog();

            if (isConfirmed.Value)
            {
                searchObject.url = UrlBuilder.Build(
                    searchObject.pattern, searchObject.traitId, searchObject.qualityId,
                    searchObject.isChampionPoint, searchObject.level_min, searchObject.level_max,
                    searchObject.voucher_min, searchObject.voucher_max, searchObject.amount_min,
                    searchObject.amount_max, searchObject.price_min, searchObject.price_max, 
                    searchObject.category1Id, searchObject.category2Id, searchObject.category3Id);
                
                AddToSearchPanel(ActiveSearchesPanel, searchObject);
            }
        }

        private void ClearHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            ItemsPanel.Children.Clear();
            serializedItemList.Clear();
        }
    }
    
}
