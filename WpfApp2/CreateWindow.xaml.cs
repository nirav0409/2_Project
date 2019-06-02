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
        Boolean editMode;
        public CreateWindow()
        {
            InitializeComponent();
            editMode = false;
        }

     public CreateWindow(Boolean isEditMode)
        {
            InitializeComponent();
            editMode = true;
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
                if (editMode)
                {
                    int selectedIndex = ((MainWindow)Application.Current.MainWindow).selectedLayoutIndex;
                    ((MainWindow)Application.Current.MainWindow).layouts[selectedIndex].setLayoutName(layoutName);
                    ((MainWindow)Application.Current.MainWindow).layouts[selectedIndex].setColor(R, G, B);
                    ((Card)((MainWindow)Application.Current.MainWindow).layoutPanel.Children[selectedIndex]).setLayoutName(layoutName);
                    ((Card)((MainWindow)Application.Current.MainWindow).layoutPanel.Children[selectedIndex]).setLayoutColor(R,G,B);
                    ((MainWindow)Application.Current.MainWindow).layoutSelected(selectedIndex);

                }
                else
                {
                    Layout layout = new Layout();
                    layout.setLayoutName(layoutName);
                    layout.setColor(R, G, B);
                    ((MainWindow)Application.Current.MainWindow).layouts.Add(layout);
                    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                    ((MainWindow)Application.Current.MainWindow).layoutPanel.Children.Add(new Card(layout, mainWindow.layouts.Count - 1));
                    ((MainWindow)Application.Current.MainWindow).layoutSelected(mainWindow.layouts.Count - 1);
                } this.Close();
            }
            else {
                MessageBox.Show("Please check Color or Name");
            }


        

        }
    }
}
