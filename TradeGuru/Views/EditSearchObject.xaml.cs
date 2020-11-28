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
using System.Text.RegularExpressions;

namespace TradeGuru.Views
{
    public partial class EditSearchObject : Window
    {
        SearchObject obj;
        Wrappers.Boolean isConfirmed;
        Wrappers.Boolean isDeleted;

        public EditSearchObject(SearchObject obj, Wrappers.Boolean isConfirmed, Wrappers.Boolean isDeleted)
        {
            InitializeComponent();
            this.obj = obj;
            this.isConfirmed = isConfirmed;
            this.isDeleted = isDeleted;

            Category1ComboBox.ItemsSource = SearchAttributeTranslator.GetDictionaryOfCategory1()?.OrderBy(t => t.Key);
            TraitComboBox.ItemsSource = SearchAttributeTranslator.GetDictionaryOfTraits()?.OrderBy(t => t.Value);
            QualityComboBox.ItemsSource = SearchAttributeTranslator.GetDictionaryOfQualities()?.OrderBy(t => t.Key);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            obj.pattern = SearchTermTextBox.Text;
            obj.category1Id = (int)Category1ComboBox.SelectedValue;
            obj.category2Id = Category2ComboBox.SelectedValue == null ? -1 : (int)Category2ComboBox.SelectedValue;
            obj.category3Id = Category3ComboBox.SelectedValue == null ? -1 : (int)Category3ComboBox.SelectedValue;
            obj.traitId = (int)TraitComboBox.SelectedValue;
            obj.qualityId = (int)QualityComboBox.SelectedValue;
            obj.level_min = LevelMinTextBox.Text.ToNumber();
            obj.level_max = LevelMaxTextBox.Text.ToNumber();
            obj.voucher_min = VoucherMinTextBox.Text.ToNumber();
            obj.voucher_max = VoucherMaxTextBox.Text.ToNumber();
            obj.amount_min = AmountMinTextBox.Text.ToNumber();
            obj.amount_max = AmountMaxTextBox.Text.ToNumber();
            obj.price_min = PriceMinTextBox.Text.ToNumber();
            obj.price_max = PriceMaxTextBox.Text.ToNumber();
            obj.isChampionPoint = ChampionPointCheckBox.IsChecked.HasValue ? ChampionPointCheckBox.IsChecked.Value : true;
            obj.sortType = SortTypePriceRadioButton.IsChecked == true ? SearchObject.SortType.Price : SearchObject.SortType.Last_Seen;
            obj.last_seen_max_minutes = LastSeenTextBox.Text.ToNumber();

            isConfirmed.Value = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SearchTermTextBox.Text = obj.pattern;
            Category1ComboBox.SelectedValue = obj.category1Id;
            Category2ComboBox.SelectedValue = obj.category2Id;
            Category3ComboBox.SelectedValue = obj.category3Id;
            TraitComboBox.SelectedValue = obj.traitId;
            QualityComboBox.SelectedValue = obj.qualityId;
            LevelMinTextBox.Text = obj.level_min.ToText();
            LevelMaxTextBox.Text = obj.level_max.ToText();
            VoucherMinTextBox.Text = obj.voucher_min.ToText();
            VoucherMaxTextBox.Text = obj.voucher_max.ToText();
            AmountMinTextBox.Text = obj.amount_min.ToText();
            AmountMaxTextBox.Text = obj.amount_max.ToText();
            PriceMinTextBox.Text = obj.price_min.ToText();
            PriceMaxTextBox.Text = obj.price_max.ToText();
            ChampionPointCheckBox.IsChecked = obj.isChampionPoint;
            LastSeenTextBox.Text = obj.last_seen_max_minutes.ToText();

            if (obj.sortType == SearchObject.SortType.Price)
                SortTypePriceRadioButton.IsChecked = true;
            else
                SortTypeLastSeenRadioButton.IsChecked = true;
        }

        private void Category1ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int cat1 = (int)Category1ComboBox.SelectedValue;
            Category2ComboBox.ItemsSource = SearchAttributeTranslator.GetDictionaryOfCategory2(cat1)?.OrderBy(t => t.Key);
            if (Category2ComboBox.ItemsSource != null)
            {
                Category2ComboBox.Visibility = Visibility.Visible;
                Category2ComboBox.SelectedIndex = 0;
            }
            else
            {
                Category2ComboBox.Visibility = Visibility.Collapsed;
            }
        }

        private void Category2ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int? cat1 = (int?)Category1ComboBox.SelectedValue;
            int? cat2 = (int?)Category2ComboBox.SelectedValue;
            if (cat1.HasValue && cat2.HasValue)
            {
                Category3ComboBox.ItemsSource = SearchAttributeTranslator.GetDictionaryOfCategory3(cat1.Value, cat2.Value)?.OrderBy(t => t.Key);
                Category3ComboBox.Visibility = Visibility.Visible;
                Category3ComboBox.SelectedIndex = 0;
            }
            else
            {
                Category3ComboBox.ItemsSource = null;
                Category3ComboBox.Visibility = Visibility.Collapsed;
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            isDeleted.Value = true;
            this.Close();
        }

        private void SortTypePriceRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (SortTypeLastSeenRadioButton != null)
                SortTypeLastSeenRadioButton.IsChecked = false;
        }

        private void SortTypeLastSeenRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (SortTypePriceRadioButton != null)
                SortTypePriceRadioButton.IsChecked = false;
        }
    }
}
