using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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

namespace TradeGuru.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Boolean AutoScroll = true;
        private Boolean rawHtmlAutoScroll = true;
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
            StartEvent(ItemsPanel, RawHtmlPanel, serializedSearchList);
        }

        private static async Task StartEvent(StackPanel historyPanel, StackPanel htmlPanel, List<SearchObject> searchObjects)
        {
            while (true)
            {
                foreach (var obj in searchObjects)
                {
                    ItemList items = null;

                    await Task.Run(async () =>
                    {
                        items = await Scraper.GetItems(obj.url, obj.last_seen_max_minutes);
                    });

                    AddToRawHtml(htmlPanel, items.rawHtml, items.queryDate);
                    if (items.Count == 0) continue;
                    
                    AddItemsToHistory(historyPanel, items);

                    if (items.Count <= 5)
                    {
                        var notification = new NotificationWindow(items);
                        notification.Show();
                        SystemSounds.Hand.Play();
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
                

                await Task.Delay(TimeSpan.FromMinutes(5));
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

        public static void AddToSearchPanel(WrapPanel panel, SearchObject obj)
        {
            AddToSearchPanel(panel, new List<SearchObject> { obj });
        }

        public static void AddToSearchPanel(WrapPanel panel, List<SearchObject> searchObjects)
        {
            serializedSearchList.AddRange(searchObjects);
            foreach (var obj in searchObjects)
            {
                // UI styling
                var border = new Border();
                border.BorderThickness = new Thickness(2);
                border.BorderBrush = Brushes.Gray;
                border.CornerRadius = new CornerRadius(5);
                border.Margin = new Thickness(15, 15, 10, 10);
                border.Height = 150;
                border.VerticalAlignment = VerticalAlignment.Top;

                var btn = new Button();
                btn.Background = new SolidColorBrush(Color.FromArgb(5, 0, 0, 0));
                btn.BorderThickness = new Thickness(0);
                btn.Width = 200;
                btn.Height = 150;
                btn.Padding = new Thickness(10);
                btn.VerticalContentAlignment = VerticalAlignment.Top;
                btn.Cursor = Cursors.Hand;

                var dockPanel = new DockPanel();
                var itemName = new TextBlock();
                var itemPrice = new TextBlock();
                var itemCategory = new TextBlock();
                var itemRecency = new TextBlock();
                DockPanel.SetDock(itemName, Dock.Top);
                DockPanel.SetDock(itemPrice, Dock.Top);
                DockPanel.SetDock(itemCategory, Dock.Top);
                DockPanel.SetDock(itemRecency, Dock.Bottom);
                itemPrice.FontSize = 13;
                itemCategory.FontSize = 11;
                itemPrice.FontStyle = FontStyles.Italic;

                // Item Name
                var run = new Run(obj.pattern);
                run.FontSize = 20;
                
                if (SearchAttributeTranslator.GetItemQualityText(obj.qualityId) == SearchAttributeTranslator.Qualities.Normal ||
                    SearchAttributeTranslator.GetItemQualityText(obj.qualityId) == SearchAttributeTranslator.Qualities.Any_Quality)
                    run.Foreground = Brushes.Gray;
                else if (SearchAttributeTranslator.GetItemQualityText(obj.qualityId) == SearchAttributeTranslator.Qualities.Fine)
                    run.Foreground = Brushes.Green;
                else if (SearchAttributeTranslator.GetItemQualityText(obj.qualityId) == SearchAttributeTranslator.Qualities.Superior)
                    run.Foreground = Brushes.Blue;
                else if (SearchAttributeTranslator.GetItemQualityText(obj.qualityId) == SearchAttributeTranslator.Qualities.Epic)
                    run.Foreground = Brushes.Purple;
                else if (SearchAttributeTranslator.GetItemQualityText(obj.qualityId) == SearchAttributeTranslator.Qualities.Legendary)
                    run.Foreground = Brushes.Yellow;

                var bold = new Bold(run);
                itemName.Inlines.Add(bold);

                // Item Price
                var price_min = obj.price_min.ToText();
                var price_max = obj.price_max.ToText();
                itemPrice.Inlines.Add(String.Format("{0} ~ {1}", price_min, price_max));

                // Item Categories
                itemCategory.Inlines.Add(String.Format("{0}", SearchAttributeTranslator.GetSearchCategoryText(obj.category1Id, obj.category2Id, obj.category3Id)));

                // Item Recency
                var recency = obj.last_seen_max_minutes.ToText() == String.Empty ? String.Empty : obj.last_seen_max_minutes.ToText() + " min(s)";
                itemRecency.Inlines.Add(String.Format("Recency: {0}", recency));

                // Content and event binding
                border.Child = btn;
                dockPanel.Children.Add(itemName);
                dockPanel.Children.Add(itemPrice);
                dockPanel.Children.Add(itemCategory);
                dockPanel.Children.Add(itemRecency);

                btn.Content = dockPanel;
                btn.Tag = obj;
                btn.Click += (sender, e) => 
                {
                    var isDeleted = new Wrappers.Boolean(false);
                    var button = sender as Button;
                    var searchObj = button.Tag as SearchObject;
                    var edit = new EditSearchObject(searchObj, isDeleted);
                    
                    edit.ShowDialog();

                    if (isDeleted.Value == true)
                    {
                        RemoveFromSearchPanel(panel, border, searchObj);
                    }
                };
                
                panel.Children.Add(border);
            }
        }

        public static void AddToRawHtml(StackPanel panel, string html, string queryDate)
        {
            var text = new TextBlock();
            var bold = new Bold(new Run(queryDate + "\n-------------------------------------------------------------------"));
            text.Inlines.Add(bold);
            text.Inlines.Add(html);
            bold = new Bold(new Run("\n-------------------------------------------------------------------"));
            text.Inlines.Add(bold);
            panel.Children.Add(text);
        }

        public static void RemoveFromSearchPanel(WrapPanel panel, Border button, SearchObject obj)
        {
            panel.Children.Remove(button);
            serializedSearchList.Remove(obj);
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

        private void ClearRawHtmlButton_Click(object sender, RoutedEventArgs e)
        {
            RawHtmlPanel.Children.Clear();
        }

        private void RawHtmlScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange == 0)
            {   // Content unchanged : user scroll event
                if (rawHtmlScrollViewer.VerticalOffset == rawHtmlScrollViewer.ScrollableHeight)
                {   // Scroll bar is in bottom
                    // Set auto-scroll mode
                    rawHtmlAutoScroll = true;
                }
                else
                {   // Scroll bar isn't in bottom
                    // Unset auto-scroll mode
                    rawHtmlAutoScroll = false;
                }
            }

            // Content scroll event : auto-scroll eventually
            if (rawHtmlAutoScroll && e.ExtentHeightChange != 0)
            {   // Content changed and auto-scroll mode set
                // Autoscroll
                rawHtmlScrollViewer.ScrollToVerticalOffset(rawHtmlScrollViewer.ExtentHeight);
            }
        }
    }
    
}
