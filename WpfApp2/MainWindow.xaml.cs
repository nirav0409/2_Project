using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<Layout> layouts;

        public MainWindow()
        {
            InitializeComponent();
            InitFromFile();
            InitUI();

        }

        private void InitUI()
        {
            foreach (var layout in layouts)
            {
                listBox.Items.Add(layout.getLayoutName() + "," + layout.getR()+"," + layout.getG() + "," + layout.getB());
            }
        }

        private void InitFromFile()
        {
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(Constants.path))
                {

                    // Read and display lines from the file until 
                    // the end of the file is reached. 
                    int numOfLayouts = Int32.Parse(sr.ReadLine());
                    layouts = new List<Layout>();
                   
                    for(int i = 0; i < numOfLayouts; i++)
                    {
                        layouts.Add(new Layout());
                        String layoutName = sr.ReadLine();
                        String[] colors = sr.ReadLine().Split(',');
                        String[] buttonTypes = sr.ReadLine().Split(',');
                        String[] buttonValues = sr.ReadLine().Split(',');

                        layouts[i].setColor(Int32.Parse(colors[0]), Int32.Parse(colors[1]), Int32.Parse(colors[2]));
                        layouts[i].setLayoutIndex(i);
                        layouts[i].setLayoutName(layoutName);
                        for ( int buttonIndex = 0; buttonIndex < 6; buttonIndex++)
                        {
                            layouts[i].setValueofButton(buttonIndex,Int32.Parse(buttonTypes[buttonIndex]),buttonValues[buttonIndex]); 
                        }
                        

                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.

            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
 

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ConfigWindow configWindow = new ConfigWindow();
            configWindow.Show();
        }

        private void remoteButtonClick(object sender, RoutedEventArgs e)
        {
            PromptBox obj = new PromptBox();
            obj.Show();
        }

        private void removeSelectedLayoutItem(object sender, RoutedEventArgs e)
        {
            //String selectedItem = listBox.Items[listBox.SelectedIndex].ToString();
            listBox.Items.Remove(listBox.SelectedIndex);
        }

        private void addLayout(object sender, RoutedEventArgs e)
        {
            CreateWindow createWindow = new CreateWindow();
            createWindow.Show();

        }

        private void layoutSelected(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.Items[listBox.SelectedIndex] != null)
            {
                String selectedItem = listBox.Items[listBox.SelectedIndex].ToString();
                String[] color = selectedItem.Split(',');
                byte R = Convert.ToByte(color[1]);
                byte G = Convert.ToByte(color[2]);
                byte B = Convert.ToByte(color[3]);

                //MessageBox.Show("R" +R + "G" + G + "B" + B);

                buttonGrid.Background = new SolidColorBrush(Color.FromRgb(R, G, B));
            }
        }
    }
}
