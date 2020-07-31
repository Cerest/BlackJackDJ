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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ACNS_Blackjack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Controller controller;
        int pot;
        int wallet;
        public MainWindow()
        {
            InitializeComponent();
            controller = new Controller(Dealerhand, Playerhand, Mask, WalletLabel, PotLabel);
            pot = 0;
            wallet = 100;
        }
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = new Storyboard();
            DoubleAnimation ani = new DoubleAnimation();
            ani.From = 1;
            ani.To = 0;
            ani.Duration = new Duration(new TimeSpan(0, 0, 0, 1));
            ani.Completed += Ani_Completed;
            Storyboard.SetTarget(ani, TitleScreen);
            Storyboard.SetTargetProperty(ani, new PropertyPath(Grid.OpacityProperty));
            sb.Children.Add(ani);
            sb.Begin();
            Play.IsEnabled = false;
            Task.Run(() =>
            {
                Console.Beep(220, 250);
                Console.Beep(523, 250);
                Console.Beep(261, 250);
                Console.Beep(294, 250);
                Console.Beep(587, 250);
                Console.Beep(261, 250);
                Console.Beep(440, 250);
                Console.Beep(523, 250);
                Console.Beep(587, 250);
                Console.Beep(658, 500);
            });
        }

        private async void Ani_Completed(object sender, EventArgs e)
        {

            TitleScreen.Width = 0;
            TitleScreen.Height = 0;
            await Task.Run(() =>
            {
                controller.PlayGame();
            });
        }

        private void Hit_Click(object sender, RoutedEventArgs e)
        {
            controller.PlayerAction = "Hit";
        }

        private void Stand_Click(object sender, RoutedEventArgs e)
        {
            controller.PlayerAction = "Stand";
        }

        private void Bet1_Click(object sender, RoutedEventArgs e)
        {
            if (controller.wallet >= 1)
            {
                controller.pot += 1;
                controller.wallet -= 1;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    WalletLabel.Content = "$" + controller.wallet;
                    PotLabel.Content = "$" + controller.pot;
                });
            }
        }

        private void Bet5_Click(object sender, RoutedEventArgs e)
        {
                if (controller.wallet >= 5)
                {
                    controller.pot += 5;
                    controller.wallet -= 5;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        WalletLabel.Content = "$" + controller.wallet;
                        PotLabel.Content = "$" + controller.pot;
                    });
                }
        }

        private void Bet10_Click(object sender, RoutedEventArgs e)
        {
            if (controller.wallet >= 10)
            {
                controller.pot += 10;
                controller.wallet -= 10;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    WalletLabel.Content = "$" + controller.wallet;
                    PotLabel.Content = "$" + controller.pot;
                });
            }
        }
    }
}
