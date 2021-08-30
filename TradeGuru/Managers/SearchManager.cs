using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using TradeGuru.Views;

namespace TradeGuru.Managers
{
    public class SearchManager
    {
        private static SearchManager searchManager { get; set; }

        private SearchUserControl control { get; set; }
        public List<SearchObject> SearchList { get; private set; }

        private SearchManager(SearchUserControl control, Wrappers.Boolean isCaptchaActivated)
        {
            this.control = control;
            control.Initialize(isCaptchaActivated);
            this.SearchList = new List<SearchObject>();

            var searchList = Serializer.DeserializeObjectList<SearchObject>(SettingsManager.CurrentSetting.SearchFileLocation);
            if (searchList != null)
            {
                AddObject(searchList);
            }
        }
        
        public static SearchManager Create(SearchUserControl control, Wrappers.Boolean isCaptchaActivated)
        {
            searchManager = searchManager ?? new SearchManager(control, isCaptchaActivated);
            return searchManager;
        }

        public void AddObject(SearchObject obj)
        {
            AddObject(new List<SearchObject> { obj });
        }

        public void AddObject(List<SearchObject> searchObjects)
        {
            SearchList.AddRange(searchObjects);

            foreach (var obj in searchObjects)
            {
                // UI styling
                var border = new Border();
                border.BorderThickness = new Thickness(2);
                border.BorderBrush = Brushes.Gray;
                border.CornerRadius = new CornerRadius(5);
                border.Margin = new Thickness(15, 15, 10, 10);
                border.MinHeight = 150;
                border.VerticalAlignment = VerticalAlignment.Top;

                var btn = new Button();
                btn.Background = new SolidColorBrush(Color.FromArgb(5, 0, 0, 0));
                btn.BorderThickness = new Thickness(0);
                btn.Width = 200;
                btn.MinHeight = 150;
                btn.Padding = new Thickness(10);
                btn.VerticalContentAlignment = VerticalAlignment.Top;
                btn.Cursor = Cursors.Hand;

                var dockPanel = FormatSearchObjectContent(obj);

                // Content and event binding
                border.Child = btn;
                btn.Content = dockPanel;
                btn.Tag = obj;
                btn.Click += (sender, e) =>
                {
                    var isConfirmed = new Wrappers.Boolean(false);
                    var isDeleted = new Wrappers.Boolean(false);
                    var button = sender as Button;
                    var searchObj = button.Tag as SearchObject;
                    var edit = new EditSearchObject(searchObj, isConfirmed, isDeleted);

                    edit.ShowDialog();

                    if (isConfirmed.Value == true)
                    {
                        searchObj.url = UrlBuilder.Build(searchObj);

                        dockPanel = FormatSearchObjectContent(searchObj);
                        btn.Content = dockPanel;
                    }
                    else if (isDeleted.Value == true)
                    {
                        RemoveObject(border, searchObj);
                    }
                };

                control.Panel.Children.Add(border);
            }
        }

        public virtual void RemoveObject(Border button, SearchObject obj)
        {
            SearchList.Remove(obj);
            control.Panel.Children.Remove(button);
        }

        public void SaveActiveSearches()
        {
            Serializer.SerializeObjectList(SettingsManager.CurrentSetting.SearchFileLocation, SearchList);
        }

        private DockPanel FormatSearchObjectContent(SearchObject obj)
        {
            var dockPanel = new DockPanel();
            var itemName = new TextBlock();
            var itemPrice = new TextBlock();
            var itemVoucher = new TextBlock();
            var itemCategory = new TextBlock();
            var itemSortType = new TextBlock();
            var itemRecency = new TextBlock();
            DockPanel.SetDock(itemName, Dock.Top);
            DockPanel.SetDock(itemPrice, Dock.Top);
            DockPanel.SetDock(itemVoucher, Dock.Top);
            DockPanel.SetDock(itemCategory, Dock.Top);
            DockPanel.SetDock(itemSortType, Dock.Top);
            DockPanel.SetDock(itemRecency, Dock.Bottom);
            itemPrice.FontSize = 13;
            itemPrice.FontStyle = FontStyles.Italic;
            itemCategory.FontSize = 11;
            itemCategory.Foreground = Brushes.DarkGray;

            // Item Name
            var name = obj.pattern == String.Empty ? "-" : obj.pattern;
            var run = new Run(name);
            run.FontSize = 20;
            var itemQuality = SearchAttributeTranslator.GetItemQualityFromId(obj.qualityId);
            run.Foreground = SearchAttributeTranslator.GetBrushForItemQuality(itemQuality);

            var bold = new Bold(run);
            itemName.Inlines.Add(bold);
            
            // Item Price
            var price_min = obj.price_min.ToText(true);
            var price_max = obj.price_max.ToText(true);
            itemPrice.Inlines.Add(String.Format("Price: {0} ~ {1}", price_min, price_max));

            // Item Writ Vouchers
            var voucher_min = obj.voucher_min.ToText(true);
            var voucher_max = obj.voucher_max.ToText(true);
            itemVoucher.Inlines.Add(String.Format("Writs: {0} ~ {1}", voucher_min, voucher_max));

            // Item Categories
            itemCategory.Inlines.Add(String.Format("{0}", SearchAttributeTranslator.GetSearchCategoryText(obj.category1Id, obj.category2Id, obj.category3Id)));

            // Item Trait
            var trait = SearchAttributeTranslator.GetItemTraitFromId(obj.traitId).ToString().CleanString();
            if (trait != String.Empty && trait != "Any Trait")
            {
                itemCategory.Inlines.Add("\n   " + trait);
            }

            // Item Sort type
            itemSortType.Inlines.Add(String.Format("Sort by: {0}", obj.sortType.ToString().CleanString()));

            // Item Recency
            var recency = obj.last_seen_max_minutes.ToText() == String.Empty ? String.Empty : obj.last_seen_max_minutes.ToText() + " min(s)";
            itemRecency.Inlines.Add(String.Format("Recency: {0}", recency));

            dockPanel.Children.Add(itemName);
            dockPanel.Children.Add(itemPrice);
            if (voucher_min != String.Empty && voucher_max != String.Empty)
                dockPanel.Children.Add(itemVoucher);
            dockPanel.Children.Add(itemCategory);
            dockPanel.Children.Add(itemSortType);
            dockPanel.Children.Add(itemRecency);

            return dockPanel;
        }

        public void ToggleContinueButton(bool value)
        {
            control.ToggleContinueButton(value);
        }
    }
}
