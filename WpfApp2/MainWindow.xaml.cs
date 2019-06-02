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
        public List<Button> buttons;
        public List<Canvas> canvases;
        public int selectedLayoutIndex;

        public MainWindow()
        {
            InitializeComponent();
            buttons = new List<Button>();
            canvases = new List<Canvas>();
            buttons.Add(Button0);
            buttons.Add(Button1);
            buttons.Add(Button2);
            buttons.Add(Button3);
            buttons.Add(Button4);
            canvases.Add(Button5);
            canvases.Add(Button6);

            InitFromFile();
            InitUI();
            initFilepaths();
            sp = this.layoutPanel;
            //comPortBox.Items.Add("COM9");

        }

        private void initFilepaths()
        {
            if (Directory.Exists(Constants.defaultardiunoDir))
            {
                Constants.ardiunoDir = Constants.defaultardiunoDir;
            }
            else
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
        }

        public List<Layout> GetLayout()
        {
            return this.layouts;
        }

        private void InitUI()
        {

            for(int i = 0; i < layouts.Count; i++)
            {
                layoutPanel.Children.Add(new Card(layouts[i],i));
            }
            string[] ports = SerialPort.GetPortNames();
            foreach (String port in ports)
            {
                comPortBox.Items.Add(port);
            }
            foreach (Button b in buttons)
                b.ToolTip = "Select Layout";

            foreach (Canvas c in canvases)
                c.ToolTip = "Select Layout";

            selectedLayoutIndex = -1;
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
                        for (int buttonIndex = 0; buttonIndex < 7; buttonIndex++)
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
            if (selectedLayoutIndex < 0)
            {
                MessageBox.Show("Please select Layout");
                return;
            }
            var button = sender as Button;
            String buttonName;
            if (button != null)
            {
                buttonName = button.Name.ToString();
            }
            else
            {
                var canvas = sender as Canvas;
                buttonName = canvas.Name.ToString();
            }
            int buttonIndex = buttonName[buttonName.Length - 1] - '0';
            int buttonType = layouts[selectedLayoutIndex].getTypeOfButton(buttonIndex);
            PromptBox obj = new PromptBox();
            obj.Title = "Button Config Window of Button :" + (buttonIndex + 1);
            switch (buttonType)
            {

                case -1:
                    //nothing selected case
                    break;
                case 0:
                    //hold selected case
                    String buttonValue = layouts[selectedLayoutIndex].getValueofButton(buttonIndex);
                    string[] splitedvalues = buttonValue.Split('+');
                    obj.holdCombo1.Text = splitedvalues[0];
                    obj.holdCombo2.Text = splitedvalues[1];
                    obj.holdCombo3.Text = splitedvalues[2];
                    obj.holdCheckbox.IsChecked = true;

                    break;
                case 1:
                    //letter/Text selected case
                    buttonValue = layouts[selectedLayoutIndex].getValueofButton(buttonIndex);
                    obj.letterTextbox.Text = buttonValue;
                    obj.letterCheckbox.IsChecked = true;
                    break;
                case 2:
                    //Media selected case
                    buttonValue = layouts[selectedLayoutIndex].getValueofButton(buttonIndex);
                    obj.mediaCombobox.Text = buttonValue;
                    obj.mediaCheckbox.IsChecked = true;

                    break;
                case 3:
                    // Hold and Letter selected case
                    buttonValue = layouts[selectedLayoutIndex].getValueofButton(buttonIndex);
                    splitedvalues = buttonValue.Split('+');
                    obj.holdCombo1.Text = splitedvalues[0];
                    obj.holdCombo2.Text = splitedvalues[1];
                    obj.holdCombo3.Text = splitedvalues[2];
                    obj.letterTextbox.Text = splitedvalues[3];
                    obj.letterCheckbox.IsChecked = true;
                    obj.holdCheckbox.IsChecked = true;
                    break;
            }
            obj.Show();
        }

        private void removeSelectedLayoutItem(object sender, RoutedEventArgs e)
        {
            if (selectedLayoutIndex < 0)
            {
                MessageBox.Show("Please Select Layout");
                return;
            }
            layouts.RemoveAt(selectedLayoutIndex);
            layoutPanel.Children.RemoveAt(selectedLayoutIndex);

            if (glowCanvas.Children != null)
                glowCanvas.Children.Clear();

            foreach (Button b in buttons)
                b.ToolTip = "Select Layout";

            foreach (Canvas c in canvases)
                c.ToolTip = "Select Layout";
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


        public String getTooltip(int layoutIndex, int buttonIndex)
        {
            int buttonType = layouts[layoutIndex].getTypeOfButton(buttonIndex);
            switch (buttonType)
            {
                case -1:
                    return "Button Not Configured";
                case 0:
                    //only hold case
                    String [] holdcmds = layouts[layoutIndex].getValueofButton(buttonIndex).Split('+');
                    StringBuilder stringBuilder = new StringBuilder();
                    for(int i = 0; i<holdcmds.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(holdcmds[i]))
                        {
                            stringBuilder.Append(holdcmds[i]);
                            if(i != holdcmds.Length - 1)
                            {
                                stringBuilder.Append(" + ");

                            }
                        }
                    }
                    return stringBuilder.ToString();
                case 1:
                    return layouts[layoutIndex].getValueofButton(buttonIndex);
                case 2:
                    //one media case
                    return layouts[layoutIndex].getValueofButton(buttonIndex);
                case 3:
                    String[] mixedcmds = layouts[layoutIndex].getValueofButton(buttonIndex).Split('+');
                    StringBuilder mixedCmdBuilder = new StringBuilder();
                    for(int i = 0; i < mixedcmds.Length -1; i++)
                    {
                        if (!String.IsNullOrEmpty(mixedcmds[i]))
                        {
                            mixedCmdBuilder.Append(mixedcmds[i] + " + ");
                        }
                    }
                    mixedCmdBuilder.Append(mixedcmds[mixedcmds.Length-1]);
                    return mixedCmdBuilder.ToString();

            }
            return "";
        }

        private void saveButtonClicked(object sender, RoutedEventArgs e)
        {
            StreamWriter streamWriter = new StreamWriter(Constants.path);
            streamWriter.WriteLine(this.layouts.Count);

            foreach (Layout layout in layouts)
            {
                streamWriter.WriteLine(layout.getLayoutName());
                streamWriter.WriteLine(layout.getR() + "," + layout.getG() + "," + layout.getB());
                for (int buttonIndex = 0; buttonIndex < 7; buttonIndex++)
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
                    //Only Hold Button is Selected
                    String[] holdCmds = layouts[layoutNumber].getValueofButton(buttonIndex).ToUpper().Replace(' ', '_').Split('+');
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (String holdCmd in holdCmds)
                    {
                        if (!String.IsNullOrEmpty(holdCmd))
                            stringBuilder.Append("\n\t\t\t\t" + "Keyboard.press(KEY_" + holdCmd + ");");
                    }
                    if (holdCmds.Length > 0)
                    {
                        stringBuilder.Append("\n\t\t\t\t" + "Keyboard.releaseAll();");
                    }
                    return stringBuilder.ToString();

                case 1:
                    //Only Letter is Selected

                    return ("\n\t\t\t\t" + "Keyboard.print(\"" + layouts[layoutNumber].getValueofButton(buttonIndex) + "\");\n");
                case 2:
                    //Only Media is Selected

                    String mediaCmd = layouts[layoutNumber].getValueofButton(buttonIndex).ToUpper().Replace(' ', '_');
                    return ("\n\t\t\t\t" + "Consumer.write(" + mediaCmd + ");\n");

                case 3:
                    //Hold and Letter is Selected
                    String[] mixedCmd = layouts[layoutNumber].getValueofButton(buttonIndex).Split('+');
                    StringBuilder mixedCmdBuilder = new StringBuilder();
                    for(int i = 0; i < (mixedCmd.Length-1); i++)
                    {
                        if (!String.IsNullOrEmpty(mixedCmd[i]))
                            mixedCmdBuilder.Append("\n\t\t\t\t" + "Keyboard.press(KEY_" + mixedCmd[i].ToUpper().Replace(' ','_') + ");");
                    }
                    mixedCmdBuilder.Append("\n\t\t\t\t" + "Keyboard.print(" + mixedCmd[mixedCmd.Length-1] + ");");

                    if (mixedCmd.Length > 1)
                    {
                        mixedCmdBuilder.Append("\n\t\t\t\t" + "Keyboard.releaseAll();");
                    }
                    return mixedCmdBuilder.ToString();
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
                outputTextBlock.Text = "Running Command,This May Take 2-3 Minutes, Please Wait.....\n";
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
                String cmd = "arduino_debug --upload --port " + port + " \"" + Constants.pathUno+"\"";
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
                // Now we create a process, assign its Process StartInfo and start it
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
                    loadFromFile(stream);
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    MessageBox.Show("No File Selected");
                    break;
            }
        }

        private void loadFromFile(StreamReader stream)
        {
            List<Layout> layouts = new List<Layout>();
            int numOfLayouts = Int32.Parse(stream.ReadLine());

            for (int i = 0; i < numOfLayouts; i++)
            {
                layouts.Add(new Layout());
                String layoutName = stream.ReadLine();
                String[] colors = stream.ReadLine().Split(',');


                layouts[i].setColor(Int32.Parse(colors[0]), Int32.Parse(colors[1]), Int32.Parse(colors[2]));
                layouts[i].setLayoutIndex(i);
                layouts[i].setLayoutName(layoutName);
                for (int buttonIndex = 0; buttonIndex < 7; buttonIndex++)
                {
                    String[] buttonTypeAndValue = stream.ReadLine().Split(',');
                    layouts[i].setValueofButton(buttonIndex, Int32.Parse(buttonTypeAndValue[0]), buttonTypeAndValue[1]);
                }


            }
            this.layouts = layouts;
            layoutPanel.Children.Clear();
            InitUI();
            if(glowCanvas.Children != null)
                glowCanvas.Children.Clear();



        }



        ///new code
        Boolean isDown, isDragging;
        Point startPoint;
        StackPanel sp;
        UIElement realDragSource;
        UIElement dummyDragSource = new UIElement();

        private void myPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("PreviewMouseLeftButtonDown");
            if (e.Source == this.sp)
            {
            }
            else
            {
                isDown = true;
                startPoint = e.GetPosition(this.sp);
            }


        }

        private void myPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            System.Diagnostics.Debug.WriteLine("PreviewMouseLeftButtonUp");
            if (isDown == true && isDragging == false)
            {

                Card ex = (Card)(e.Source as UIElement);
                System.Diagnostics.Debug.WriteLine("Mouse Click");
                layoutSelected(layoutPanel.Children.IndexOf(ex));

                
            }
            isDown = false;
            isDragging = false;
            if (realDragSource != null)
            {
                realDragSource.ReleaseMouseCapture();
            }
        }

         public void layoutSelected(int layoutIndex)
        {   
            if(selectedLayoutIndex > -1)
                ((Card)layoutPanel.Children[selectedLayoutIndex]).layoutName.FontWeight = FontWeights.Normal;
            this.selectedLayoutIndex = layoutIndex;
            ((Card)layoutPanel.Children[selectedLayoutIndex]).layoutName.FontWeight = FontWeights.Bold;
            byte R = Convert.ToByte(layouts[selectedLayoutIndex].getR());
            byte G = Convert.ToByte(layouts[selectedLayoutIndex].getG());
            byte B = Convert.ToByte(layouts[selectedLayoutIndex].getB());

            //buttonGrid.Background = new SolidColorBrush(Color.FromRgb(R, G, B));
            Ellipse ellipse = new Ellipse() { Height = 120, Width = 120 };
            ellipse.Stroke = new SolidColorBrush(Color.FromRgb(R, G, B));
            ellipse.StrokeThickness = 5;
            glowCanvas.Children.Add(ellipse);

            //set tooltips
            Button0.ToolTip = getTooltip(selectedLayoutIndex, 0);
            Button1.ToolTip = getTooltip(selectedLayoutIndex, 1);
            Button2.ToolTip = getTooltip(selectedLayoutIndex, 2);
            Button3.ToolTip = getTooltip(selectedLayoutIndex, 3);
            Button4.ToolTip = getTooltip(selectedLayoutIndex, 4);
            Button5.ToolTip = getTooltip(selectedLayoutIndex, 5);
            Button6.ToolTip = getTooltip(selectedLayoutIndex, 6);
        }
        private void myPreviewMouseMove(object sender, MouseEventArgs e)
        {
           // Debug.WriteLine("PreViewMouseMove");
            if (isDown)
            {
                if ((isDragging == false) && ((Math.Abs(e.GetPosition(this.sp).X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                    (Math.Abs(e.GetPosition(this.sp).Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
                {
                    isDragging = true;
                    realDragSource = e.Source as UIElement;
                    realDragSource.CaptureMouse();
                    DragDrop.DoDragDrop(dummyDragSource, new DataObject("UIElement", e.Source, true), DragDropEffects.Move);
                }
            }
        }

        private void myDragEnter(object sender, DragEventArgs e)
        {
            Debug.WriteLine("DragEnter");
            if (e.Data.GetDataPresent("UIElement"))
            {
                e.Effects = DragDropEffects.Move;
            }
        }

        private void editLayoutClicked(object sender, RoutedEventArgs e)
        {
            if (selectedLayoutIndex < 0) {

                MessageBox.Show("Please Selecte Layout");
            }
            else
            { 
                CreateWindow createWindow = new CreateWindow(true);
                createWindow.layoutNameTextBox.Text = layouts[selectedLayoutIndex].getLayoutName();
                byte R = Convert.ToByte(layouts[selectedLayoutIndex].getR());
                byte G = Convert.ToByte(layouts[selectedLayoutIndex].getG());
                byte B = Convert.ToByte(layouts[selectedLayoutIndex].getB());

                createWindow.colorPicker.SelectedColor = Color.FromRgb(R, G, B);
                createWindow.Show();
            }
        }
        
        private void myDrop(object sender, DragEventArgs e)

        {
            Debug.WriteLine("Drop");
            if (e.Data.GetDataPresent("UIElement"))
            {
                UIElement droptarget = e.Source as UIElement;
                int droptargetIndex = -1, i = 0;
                foreach (UIElement element in this.sp.Children)
                {
                    if (element.Equals(droptarget))
                    {
                        droptargetIndex = i;
                        break;
                    }
                    i++;
                }
                if (droptargetIndex != -1)
                {
                    int layoutIndex = sp.Children.IndexOf(realDragSource);
                    this.sp.Children.Remove(realDragSource);
                    this.sp.Children.Insert(droptargetIndex, realDragSource);
                    Layout layout = layouts[layoutIndex];
                    this.layouts.Remove(layout);
                    this.layouts.Insert(droptargetIndex, layout);
                    if (selectedLayoutIndex == layoutIndex)
                    {
                        this.layoutSelected(droptargetIndex);
                    } else if (droptargetIndex > layoutIndex && selectedLayoutIndex < droptargetIndex && selectedLayoutIndex > layoutIndex)
                    {
                        selectedLayoutIndex -= 1;
                    }
                    else if (droptargetIndex < layoutIndex && selectedLayoutIndex > droptargetIndex && selectedLayoutIndex < layoutIndex)
                    {
                        selectedLayoutIndex += 1;

                    }
                }

                isDown = false;
                isDragging = false;
                realDragSource.ReleaseMouseCapture();
            }
        }

    }
}
