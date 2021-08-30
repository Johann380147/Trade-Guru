using System.Windows;
using System.Windows.Controls;
using TradeGuru.Managers;

namespace TradeGuru.Views
{
    /// <summary>
    /// Interaction logic for SearchUserControl.xaml
    /// </summary>
    public partial class SearchUserControl : UserControl
    {
        private SearchManager SearchManager { get; set; }
        private Wrappers.Boolean isCaptchaActivated { get; set; }

        public SearchUserControl()
        {
            InitializeComponent();
            SearchManager = SearchManager.Create(this, null);
        }

        public void Initialize(Wrappers.Boolean isCaptchaActivated)
        {
            this.isCaptchaActivated = isCaptchaActivated;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Wrappers.Boolean isConfirmed = new Wrappers.Boolean(false);
            var searchObject = new SearchObject();
            var addSearchObjectWindow = new AddSearchObject(searchObject, isConfirmed);
            addSearchObjectWindow.ShowDialog();

            if (isConfirmed.Value)
            {
                searchObject.url = UrlBuilder.Build(searchObject);
                SearchManager.AddObject(searchObject);
            }
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleContinueButton(false);
            isCaptchaActivated.Value = false;
        }

        public void ToggleContinueButton(bool value)
        {
            if (value == true)
            {
                ContinueButton.Visibility = Visibility.Visible;
                WarningText.Visibility = Visibility.Visible;
            }
            else
            {
                ContinueButton.Visibility = Visibility.Hidden;
                WarningText.Visibility = Visibility.Hidden;
            }
        }
    }
}
