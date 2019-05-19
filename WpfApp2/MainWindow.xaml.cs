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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public  List<Layout> layouts;

        public MainWindow()
        {
            InitializeComponent();
            InitFromFile();
            InitUI();

        }
   
        public List<Layout> GetLayout()
        {
            return this.layouts;
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


                        layouts[i].setColor(Int32.Parse(colors[0]), Int32.Parse(colors[1]), Int32.Parse(colors[2]));
                        layouts[i].setLayoutIndex(i);
                        layouts[i].setLayoutName(layoutName);
                        for ( int buttonIndex = 0; buttonIndex < 6; buttonIndex++)
                        {
                            String[] buttonTypeAndValue = sr.ReadLine().Split(',');
                            layouts[i].setValueofButton(buttonIndex,Int32.Parse(buttonTypeAndValue[0]), buttonTypeAndValue[1]); 
                        }
                        

                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Debug.WriteLine(e.ToString());
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
            if(listBox.SelectedIndex < 0)
            {
                MessageBox.Show("Please select Layout");
                return;
            }  
            var button = sender as Button;
            String buttonName = button.Name.ToString();
            int buttonIndex = buttonName[buttonName.Length-1] - '0';
            int buttonType = layouts[listBox.SelectedIndex].getTypeOfButton(buttonIndex);
            PromptBox obj = new PromptBox();
            obj.Title = "Button Config Window of Button :" + (buttonIndex+1);
            switch (buttonType)
            {

                case -1:
                    //nothing selected case
                    break;
                case 0:
                    //hold selected case
                    String buttonValue = layouts[listBox.SelectedIndex].getValueofButton(buttonIndex);
                    string[] splitedvalues = buttonValue.Split('+');
                    obj.holdCombo1.Text = splitedvalues[0];
                    obj.holdCombo2.Text = splitedvalues[1];
                    obj.holdCombo3.Text = splitedvalues[2];
                    obj.holdCheckbox.IsChecked = true;

                    break;
                case 1:
                    //letter/Text selected case
                    buttonValue = layouts[listBox.SelectedIndex].getValueofButton(buttonIndex);
                    obj.letterTextbox.Text = buttonValue;
                    obj.letterCheckbox.IsChecked = true;
                    break;
                case 2:
                    //Media selected case
                    buttonValue = layouts[listBox.SelectedIndex].getValueofButton(buttonIndex);
                    obj.mediaCombobox.Text = buttonValue;
                    obj.mediaCheckbox.IsChecked = true;

                    break;
                case 3:
                    // letter/Text and Media selected case
                     buttonValue = layouts[listBox.SelectedIndex].getValueofButton(buttonIndex);
                     splitedvalues = buttonValue.Split('+');
                     obj.mediaCombobox.Text = splitedvalues[0];
                     obj.letterTextbox.Text = splitedvalues[1];
                     obj.letterCheckbox.IsChecked = true;
                     obj.mediaCheckbox.IsChecked = true;
                    break;
            }
            obj.Show();
        }

        private void removeSelectedLayoutItem(object sender, RoutedEventArgs e)
        {
            //String selectedItem = listBox.Items[listBox.SelectedIndex].ToString();
            if(listBox.SelectedIndex < 0)
            {
                MessageBox.Show("Please Select Layout");
                return;
            }
            layouts.RemoveAt(listBox.SelectedIndex);
            listBox.Items.RemoveAt(listBox.SelectedIndex);
            this.buttonGrid.Background = Brushes.White;
        }

        private void addLayout(object sender, RoutedEventArgs e)
        {
            CreateWindow createWindow = new CreateWindow();
            createWindow.Show();

        }

        private void layoutSelected(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedIndex > 0)
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

        private void saveButtonClicked(object sender, RoutedEventArgs e)
        {
            StreamWriter streamWriter = new StreamWriter(Constants.pathOut);
            streamWriter.WriteLine(this.layouts.Count);

            foreach(Layout layout in layouts)
            {
                streamWriter.WriteLine(layout.getLayoutName());
                streamWriter.WriteLine(layout.getR()+","+ layout.getG() + ","+layout.getB());
                for (int buttonIndex = 0; buttonIndex < 6; buttonIndex++)
                {
                    streamWriter.WriteLine(layout.getTypeOfButton(buttonIndex) + ","+ layout.getValueofButton(buttonIndex));
                }

            }
            streamWriter.Close();
            createHIDaction();

            MessageBox.Show("Saved");
        }

        private void createHIDaction()
        {
            StreamWriter stream = new StreamWriter(Constants.pathUno);
            stream.WriteLine("void HIDaction(byte input) { \n\t byte mode = input >> 3; \n\t byte button = input & 0b00000111;");
            int numberOfLayouts = layouts.Count;
            String switchVar = "mode";
            stream.WriteLine("\n\t" + createSwitchCase(numberOfLayouts, switchVar));
            stream.WriteLine("\n}");
            stream.Close();

           
        }

        private String createSwitchCase(int numberOfCases, string switchVar)
        {
            StringBuilder switchCase = new StringBuilder();

            switchCase.Append("\n\tswitch(" + switchVar + ")" + " {");
            for (int i = 1; i < numberOfCases + 1; i++)
            {
                switchCase.Append("\n\t\tcase " + i + ":");
                //add function to add cmds
                switchCase.Append("\n" + getCaseCodeForLayout(i));
                switchCase.Append("\n\t\tbreak;");
            }
            switchCase.Append("\n\t}");
            return switchCase.ToString();
        }

        private string getCaseCodeForLayout(int layoutNumber)
        {
            StringBuilder caseCodeForLayout = new StringBuilder();
            caseCodeForLayout.Append("\n\t\t\tswitch(" + "button" + ")" + " {");
            for (int i = 1; i <= 7; i++)
            {
                caseCodeForLayout.Append("\n\t\t\t\tcase " + i + ":");
                caseCodeForLayout.Append(getButtonCaseCode(layoutNumber, i));
                caseCodeForLayout.Append("\n\t\t\t\tbreak;");

            }
            caseCodeForLayout.Append("\n\t\t\t}");

            return caseCodeForLayout.ToString();
        }

        private String getButtonCaseCode(object layoutNumber, int i)
        {
            return "Nirav";
        }
    }
}
