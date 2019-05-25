using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
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

        public List<Layout> layouts;

        public MainWindow()
        {
            InitializeComponent();
            InitFromFile();
            InitUI();
            initFilepaths();
           // comPortBox.Items.Add("COM9");

        }

        private void initFilepaths()
        {
            if (File.Exists(Constants.pathConfig))
            {
                StreamReader stream = new StreamReader(Constants.pathConfig);
                String line = stream.ReadLine();

                if (String.IsNullOrEmpty(line))
                {
                    stream.Close();
                    Settings settings = new Settings();
                    settings.Show();
                }
                else
                {
                    String[] map = line.Split('=');
                    Constants.ardiunoDir = map[1];
                }
                stream.Close();
            }
            else
            {
                Settings settings = new Settings();
                settings.Activate();
                settings.ShowDialog();
            }
        }

        public List<Layout> GetLayout()
        {
            return this.layouts;
        }
        private void InitUI()
        {
            foreach (var layout in layouts)
            {
                listBox.Items.Add(layout.getLayoutName());
            }
            string[] ports = SerialPort.GetPortNames();
            foreach (String port in ports) { 
            comPortBox.Items.Add(port);
            }
        }

        private void InitFromFile()
        {
            layouts = new List<Layout>();
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(Constants.path))
                {

                    // Read and display lines from the file until 
                    // the end of the file is reached. 
                    int numOfLayouts = Int32.Parse(sr.ReadLine());

                    for (int i = 0; i < numOfLayouts; i++)
                    {
                        layouts.Add(new Layout());
                        String layoutName = sr.ReadLine();
                        String[] colors = sr.ReadLine().Split(',');


                        layouts[i].setColor(Int32.Parse(colors[0]), Int32.Parse(colors[1]), Int32.Parse(colors[2]));
                        layouts[i].setLayoutIndex(i);
                        layouts[i].setLayoutName(layoutName);
                        for (int buttonIndex = 0; buttonIndex < 6; buttonIndex++)
                        {
                            String[] buttonTypeAndValue = sr.ReadLine().Split(',');
                            layouts[i].setValueofButton(buttonIndex, Int32.Parse(buttonTypeAndValue[0]), buttonTypeAndValue[1]);
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



        private void remoteButtonClick(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex < 0)
            {
                MessageBox.Show("Please select Layout");
                return;
            }
            var button = sender as Button;
            String buttonName = button.Name.ToString();
            int buttonIndex = buttonName[buttonName.Length - 1] - '0';
            int buttonType = layouts[listBox.SelectedIndex].getTypeOfButton(buttonIndex);
            PromptBox obj = new PromptBox();
            obj.Title = "Button Config Window of Button :" + (buttonIndex + 1);
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
            if (listBox.SelectedIndex < 0)
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
            if (layouts.Count > 32)
            {
                MessageBox.Show("Only 32 layouts Allowed");
            }
            else
            {
                CreateWindow createWindow = new CreateWindow();
                createWindow.Show();
            }
        }

        private void layoutSelected(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedIndex > -1)
            {
                String selectedItem = listBox.Items[listBox.SelectedIndex].ToString();
                String[] color = selectedItem.Split(',');
                byte R = Convert.ToByte(layouts[listBox.SelectedIndex].getR());
                byte G = Convert.ToByte(layouts[listBox.SelectedIndex].getG());
                byte B = Convert.ToByte(layouts[listBox.SelectedIndex].getB());

                buttonGrid.Background = new SolidColorBrush(Color.FromRgb(R, G, B));
            }
        }

        private void saveButtonClicked(object sender, RoutedEventArgs e)
        {
            StreamWriter streamWriter = new StreamWriter(Constants.path);
            streamWriter.WriteLine(this.layouts.Count);

            foreach (Layout layout in layouts)
            {
                streamWriter.WriteLine(layout.getLayoutName());
                streamWriter.WriteLine(layout.getR() + "," + layout.getG() + "," + layout.getB());
                for (int buttonIndex = 0; buttonIndex < 6; buttonIndex++)
                {
                    streamWriter.WriteLine(layout.getTypeOfButton(buttonIndex) + "," + layout.getValueofButton(buttonIndex));
                }

            }
            streamWriter.Close();
            createArdiunoFile();

            MessageBox.Show("Saved\nLocation : " + Constants.pathUno);
        }

        private void createArdiunoFile()
        {
            StreamWriter stream = new StreamWriter(Constants.pathUno);
            stream.WriteLine(createLoadConfig());
            createHIDaction(stream);

        }

        private String createCurrentConf()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("//----------------------\n");
            for (int i = 0; i < layouts.Count; i++)
            {

                // Color R
                stringBuilder.Append("currentConf[" + (4 * i + 1) + "]");
                stringBuilder.Append(" = ");
                stringBuilder.Append(layouts[i].getR() + ";\n");

                // Color G
                stringBuilder.Append("currentConf[" + (4 * i + 2) + "]");
                stringBuilder.Append(" = ");
                stringBuilder.Append(layouts[i].getG() + ";\n");

                // Color B
                stringBuilder.Append("currentConf[" + (4 * i + 3) + "]");
                stringBuilder.Append(" = ");
                stringBuilder.Append(layouts[i].getB() + ";\n");

            }
            stringBuilder.Append("//----------------------\n\n");
            return stringBuilder.ToString();


        }

        private String createLoadConfig()
        {
            return ("void loadConfig() { \n" +
                        "\tfor (int i = 0 ; i < 32 ; i++) { \n" +
                            "\t\tcurrentConf[i * 4] = i;\n" +
                        "\t}\n" +
                        createCurrentConf() +
                      "}");
        }

        private void createHIDaction(StreamWriter stream)
        {
            stream.WriteLine("void HIDaction(byte input) { \n\t byte mode = input >> 3; \n\t byte button = input & 0b00000111;");
            int numberOfLayouts = layouts.Count;
            String switchVar = "mode";
            stream.WriteLine("\n\t" + createSwitchCase(numberOfLayouts, switchVar));
            stream.WriteLine("\n}");
            stream.Close();


        }

        private String createSwitchCase(int numberOfLayouts, string switchVar)
        {
            StringBuilder switchCase = new StringBuilder();

            switchCase.Append("\n\tswitch(" + switchVar + ")" + " {");
            for (int i = 0; i < numberOfLayouts; i++)
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
            for (int i = 0; i < 7; i++)
            {
                caseCodeForLayout.Append("\n\t\t\t\tcase " + i + ":");
                caseCodeForLayout.Append(getButtonCaseCode(layoutNumber, i));
                caseCodeForLayout.Append("\n\t\t\t\tbreak;");

            }
            caseCodeForLayout.Append("\n\t\t\t}");

            return caseCodeForLayout.ToString();
        }

        private String getButtonCaseCode(int layoutNumber, int buttonIndex)
        {
            int buttonType = layouts[layoutNumber].getTypeOfButton(buttonIndex);
            switch (buttonType)
            {
                case 0:
                    String[] holdCmds = layouts[layoutNumber].getValueofButton(buttonIndex).ToUpper().Replace(' ', '_').Split('+');
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (String holdCmd in holdCmds)
                    {
                        if (!String.IsNullOrEmpty(holdCmd))
                            stringBuilder.Append("\n\t\t\t\t" + "Keyboard.press(" + holdCmd + ");");
                    }
                    if (holdCmds.Length > 0)
                    {
                        stringBuilder.Append("\n\t\t\t\t" + "Keyboard.releaseAll();");
                    }
                    return stringBuilder.ToString();

                case 1:
                    return ("\n\t\t\t\t" + "Keyboard.print(\"" + layouts[layoutNumber].getValueofButton(buttonIndex) + "\");\n");
                case 2:
                    String mediaCmd = layouts[layoutNumber].getValueofButton(buttonIndex).ToUpper().Replace(' ', '_');
                    return ("\n\t\t\t\t" + "Consumer.write(" + mediaCmd + ");\n");

                case 3:
                    break;
                case -1:

                    return "\n\t\t\t\t" + "//Button Not Configured\n";
            }
            return "";
        }

        private void uploadButtonClicked(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(comPortBox.Text))
            {
                MessageBox.Show("Please Enter COM Port");
            }
            else
            {
                outputTextBlock.Text = "Running Command, Please Wait.....\n";
                String port = comPortBox.Text;
                Thread thread = new Thread(() => runCmd(port));
                thread.Start();  
            }
        }

        private void runCmd(String port)
        {
            try
            {
                //outputTextBlock.Text = "Hello World";
                String cmd = "arduino_debug --upload --port " + port + " " + Constants.pathUno;
                Debug.WriteLine(cmd);
                System.Diagnostics.ProcessStartInfo procStartInfo =
                        new System.Diagnostics.ProcessStartInfo("cmd", "/c " + cmd);

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.RedirectStandardError = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.WorkingDirectory = Constants.ardiunoDir;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                String line = "";
                while (!proc.StandardOutput.EndOfStream)
                {
                    line = line + proc.StandardOutput.ReadToEnd();
                }
                while (!proc.StandardError.EndOfStream)
                {
                    line = line + proc.StandardError.ReadToEnd();
                }



                Dispatcher.BeginInvoke(new ThreadStart(() => outputTextBlock.Text = line.Trim()));

                


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private void settingsButtonClicked(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
        }

        private void loadButtonClicked(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    StreamReader stream = new StreamReader(fileDialog.FileName.ToString());
                    //loadFromFile();
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    MessageBox.Show("No File Selected");
                    break;
            }
        }
    }
}
