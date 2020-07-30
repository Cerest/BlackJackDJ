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
        public MainWindow()
        {
            InitializeComponent();
            controller = new Controller();
        }
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = new Storyboard();
            DoubleAnimation ani = new DoubleAnimation();
            ani.From = 1;
            ani.To = 0;
            ani.Duration = new Duration(new TimeSpan(0,0,0,2));
            Storyboard.SetTarget(ani, TitleScreen);
            Storyboard.SetTargetProperty(ani, new PropertyPath(Grid.OpacityProperty));
            sb.Children.Add(ani);
            sb.Begin();
        }
    }
}
