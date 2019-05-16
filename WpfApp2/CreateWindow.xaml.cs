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
 

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for CreateWindow.xaml
    /// </summary>
    public partial class CreateWindow : Window
    {
        public CreateWindow()
        {
            InitializeComponent();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void OkButtonClicked(object sender, RoutedEventArgs e)
        {
            String layoutName =layoutNameTextBox.Text;
            if (colorPicker.SelectedColor != null && !String.IsNullOrEmpty(layoutName))
            {
                var R = colorPicker.SelectedColor.Value.R;
                var G = colorPicker.SelectedColor.Value.G;
                var B = colorPicker.SelectedColor.Value.B;
               
                String text = layoutName + "," + R + "," + G + "," + B;
                ((MainWindow)Application.Current.MainWindow).listBox.Items.Add(text);
                this.Close();
            }
            else {
                MessageBox.Show("Please check Color or Name");
            }


        

        }
    }
}
