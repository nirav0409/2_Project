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
                int R = colorPicker.SelectedColor.Value.R;
                int G = colorPicker.SelectedColor.Value.G;
                int B = colorPicker.SelectedColor.Value.B;
               
                Layout layout = new Layout();
                layout.setLayoutName(layoutName);
                layout.setColor(R,G,B);
                ((MainWindow)Application.Current.MainWindow).layouts.Add(layout);
                ((MainWindow)Application.Current.MainWindow).listBox.Items.Add(layoutName);

                this.Close();
            }
            else {
                MessageBox.Show("Please check Color or Name");
            }


        

        }
    }
}
