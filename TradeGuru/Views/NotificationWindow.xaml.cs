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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TradeGuru.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        public NotificationWindow(string text)
        {
            InitializeComponent();

            var itemText = new TextBlock();
            itemText.TextWrapping = TextWrapping.Wrap;
            itemText.Margin = new Thickness(5);

            var run = new Run(text);
            run.Foreground = Brushes.Red;
            
            var bold = new Bold(run);
            itemText.Inlines.Add(bold);
            ItemsPanel.Children.Add(itemText);
        }

        public NotificationWindow(List<Item> items)
        {
            InitializeComponent();

            foreach (var item in items)
            {
                var itemText = new TextBlock();
                itemText.TextWrapping = TextWrapping.Wrap;
                itemText.Margin = new Thickness(5);

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
                    run.Foreground = Brushes.Gold;
                else if (item.rarity == Item.Rarity.Orange)
                    run.Foreground = Brushes.OrangeRed;

                var bold = new Bold(run);
                itemText.Inlines.Add(bold);
                itemText.Inlines.Add(String.Format("Price: {0:0.##}\n", item.price));
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

                ItemsPanel.Children.Add(itemText);
            }

        }

        public void ShowNotification()
        {
            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                var workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
                var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
                var corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));

                this.Left = corner.X - this.ActualWidth - 50;
                this.Top = corner.Y - this.ActualHeight;
            }));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowNotification();
        }

        private void DoubleAnimationUsingKeyFrames_Completed(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
