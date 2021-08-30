using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TradeGuru.Managers;

namespace TradeGuru.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsUserControl : UserControl
    {
        private bool hasSettingsChanged = false;
        private bool HasSettingsChanged 
        { 
            get { return hasSettingsChanged; } 
            set 
            {
                hasSettingsChanged = value;
                if (hasSettingsChanged == true)
                {
                    Settings_ConfirmButton.Visibility = Visibility.Visible;
                    Settings_UndoButton.Visibility = Visibility.Visible;
                }
                else
                {
                    Settings_ConfirmButton.Visibility = Visibility.Hidden;
                    Settings_UndoButton.Visibility = Visibility.Hidden;
                }
            }
        }

        public SettingsUserControl()
        {
            InitializeComponent();
            PopulateControls();
        }

        private void Settings_ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsManager.Save(new Classes.Setting
            {
                SearchFrequency = Settings_SearchFrequency_Text.Text.ToDouble(),
                ItemInterval = Settings_ItemInterval_Text.Text.ToDouble(),
                PageInterval = Settings_PageInterval_Text.Text.ToDouble(),

                SaveSearches = Settings_SaveSearch_CheckBox.IsChecked.HasValue ? Settings_SaveSearch_CheckBox.IsChecked.Value : true,
                SaveHistory = Settings_SaveHistory_CheckBox.IsChecked.HasValue ? Settings_SaveHistory_CheckBox.IsChecked.Value : true,
                SaveLog = Settings_SaveLog_CheckBox.IsChecked.HasValue ? Settings_SaveLog_CheckBox.IsChecked.Value : false,

                SearchFileLocation = Settings_SearchFileLocation_Label.Content.ToString(),
                HistoryFileLocation = Settings_HistoryFileLocation_Label.Content.ToString(),
                LogFileLocation = Settings_LogFileLocation_Label.Content.ToString()
            });
            PopulateControls();
            SettingsChanged(false);
        }

        private void Settings_UndoButton_Click(object sender, RoutedEventArgs e)
        {
            var setting = SettingsManager.Undo();
            Settings_SearchFrequency_Text.Text = setting.SearchFrequency.ToString();
            Settings_ItemInterval_Text.Text = setting.ItemInterval.ToString();
            Settings_PageInterval_Text.Text = setting.PageInterval.ToString();

            Settings_SaveSearch_CheckBox.IsChecked = setting.SaveSearches;
            Settings_SaveHistory_CheckBox.IsChecked = setting.SaveHistory;
            Settings_SaveLog_CheckBox.IsChecked = setting.SaveLog;

            Settings_SearchFileLocation_Label.Content = setting.SearchFileLocation;
            Settings_HistoryFileLocation_Label.Content = setting.HistoryFileLocation;
            Settings_LogFileLocation_Label.Content = setting.LogFileLocation;
            SettingsChanged(false);
        }

        private void Settings_ResetButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsManager.Reset();
            PopulateControls();
            SettingsChanged(false);
        }

        private void Settings_SearchFileBrowse_Button_Click(object sender, RoutedEventArgs e)
        {
            var path = FileManager.GetUserDefinedFolder();
            if (path != string.Empty)
            {
                Settings_SearchFileLocation_Label.Content = path + "/Search.bin";
                SettingsChanged(true);
            }
        }
        
        private void Settings_History_FileBrowse_Button_Click(object sender, RoutedEventArgs e)
        {
            var path = FileManager.GetUserDefinedFolder();
            if (path != string.Empty)
            {
                Settings_HistoryFileLocation_Label.Content = path + "/History.bin";
                SettingsChanged(true);
            }
        }

        private void Settings_LogFileBrowse_Button_Click(object sender, RoutedEventArgs e)
        {
            var path = FileManager.GetUserDefinedFolder();
            if (path != string.Empty)
            {
                Settings_LogFileLocation_Label.Content = path + "/Log.bin";
                SettingsChanged(true);
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void PopulateControls()
        {
            var setting = SettingsManager.CurrentSetting;
            Settings_SearchFrequency_Text.Text = setting.SearchFrequency.ToString();
            Settings_ItemInterval_Text.Text = setting.ItemInterval.ToString();
            Settings_PageInterval_Text.Text = setting.PageInterval.ToString();

            Settings_SaveSearch_CheckBox.IsChecked = setting.SaveSearches;
            Settings_SaveHistory_CheckBox.IsChecked = setting.SaveHistory;
            Settings_SaveLog_CheckBox.IsChecked = setting.SaveLog;

            Settings_SearchFileLocation_Label.Content = setting.SearchFileLocation;
            Settings_HistoryFileLocation_Label.Content = setting.HistoryFileLocation;
            Settings_LogFileLocation_Label.Content = setting.LogFileLocation;

            SettingsChanged(false);
        }

        private void SettingsChanged(bool value)
        {
            HasSettingsChanged = value;
        }

        private void Settings_SearchFrequency_Text_TextChanged(object sender, TextChangedEventArgs e)
        {
            SettingsChanged(true);
        }

        private void Settings_ItemInterval_Text_TextChanged(object sender, TextChangedEventArgs e)
        {
            SettingsChanged(true);
        }

        private void Settings_PageInterval_Text_TextChanged(object sender, TextChangedEventArgs e)
        {
            SettingsChanged(true);
        }

        private void Settings_SaveSearch_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SettingsChanged(true);
        }

        private void Settings_SaveSearch_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SettingsChanged(true);
        }

        private void Settings_SaveHistory_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SettingsChanged(true);
        }

        private void Settings_SaveHistory_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SettingsChanged(true);
        }

        private void Settings_SaveLog_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SettingsChanged(true);
        }

        private void Settings_SaveLog_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SettingsChanged(true);
        }
    }
}
