using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {


        public ConfigWindow()
        {
            InitializeComponent();
            gridInit();
        }
        private void gridInit()
        {
            Ellipse ellipse = new Ellipse() { Height = 152, Width = 152 };
            //ellipse.Stroke = new SolidColorBrush(Color.FromRgb(R, G, B));
            ellipse.Stroke = Brushes.Black;
            Ellipse ellipse2 = new Ellipse() { Height = 140, Width = 140 };
            //ellipse.Stroke = new SolidColorBrush(Color.FromRgb(R, G, B));
            ellipse2.Stroke = Brushes.Black;

            Ellipse ellipse3 = new Ellipse() { Height = 170, Width = 170 };
            //ellipse.Stroke = new SolidColorBrush(Color.FromRgb(R, G, B));
            ellipse3.Stroke = Brushes.Black;
            ellipse3.StrokeThickness = 2;
            glowGrid_Copy.Children.Add(ellipse3);

            glowGrid.Children.Add(ellipse);
            glowGrid.Children.Add(ellipse2);



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Ellipse ellipse3 = new Ellipse() { Height = 150, Width = 150 };
            //ellipse.Stroke = new SolidColorBrush(Color.FromRgb(R, G, B));
            ellipse3.Stroke = Brushes.Yellow;
            ellipse3.StrokeThickness = 5;
            glowGrid_Copy.Children.Add(ellipse3);
        }

        private void remoteButtonClick(object sender, MouseButtonEventArgs e)
        {
            Debug.Print("button 5 clicked");
        }
        private void remoteButtonClick_6(object sender, MouseButtonEventArgs e)
        {
            Debug.Print("button 6 clicked");
        }
    }
}