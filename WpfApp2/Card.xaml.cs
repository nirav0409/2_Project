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

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for Card.xaml
    /// </summary>
    public partial class Card : UserControl
    {
        int cardIndex;
        public Card()
        {
            InitializeComponent();
        }
        public Card(Layout layout , int layoutIndex) {

            InitializeComponent();
            this.cardIndex = layoutIndex;
            this.layoutName.Content = layout.getLayoutName();
            byte R = Convert.ToByte(layout.getR());
            byte G = Convert.ToByte(layout.getG());
            byte B = Convert.ToByte(layout.getB());

            //buttonGrid.Background = new SolidColorBrush(Color.FromRgb(R, G, B));
           // this.layoutColor.Background = new SolidColorBrush(Color.FromRgb(R, G, B));
            Ellipse ellipse = new Ellipse() { Height = 22, Width = 22 };
            ellipse.Stroke = Brushes.Black;
            ellipse.Fill   = new SolidColorBrush(Color.FromRgb(R, G, B));
            this.layoutColor.Children.Add(ellipse);
        }
        public String getLayoutName()
        {
            return this.layoutName.Content.ToString();
        }
        public void setLayoutName( String layoutName)
        {
            this.layoutName.Content = layoutName;
        }
        public void setLayoutColor( int Red , int Green , int Blue)
        {
            byte R = Convert.ToByte(Red);
            byte G = Convert.ToByte(Green);
            byte B = Convert.ToByte(Blue);

            //buttonGrid.Background = new SolidColorBrush(Color.FromRgb(R, G, B));
            //this.layoutColor.Background = new SolidColorBrush(Color.FromRgb(R, G, B));
            if(this.layoutColor.Children.Count <= 0)
            {
                Ellipse ellipse = new Ellipse() { Height = 22, Width = 22 };
                ellipse.Fill = new SolidColorBrush(Color.FromRgb(R, G, B));
                this.layoutColor.Children.Add(ellipse);
            }
            else
            {
                ((Ellipse)this.layoutColor.Children[0]).Fill = new SolidColorBrush(Color.FromRgb(R, G, B));
            }
        }
        public int getLayoutIndex()
        {
            return this.cardIndex;
        }
    }
}
