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
    /// Interaction logic for PromptBox.xaml
    /// </summary>
    public partial class PromptBox : Window
    {
        public PromptBox()
        {
            InitializeComponent();
            fill_media_combo();
            fill_hold_combo();
        }

        private void fill_hold_combo()
        {
            String[] holdArray = new string[] { "LEFT CTRL", "LEFT SHIFT", "LEFT ALT", "LEFT GUI", "RIGHT CTRL", "RIGHT SHIFT",
                                        "RIGHT ALT", "RIGHT GUI", "UP ARROW", "DOWN ARROW", "LEFT ARROW", "RIGHT ARROW",
                                        "BACKSPACE", "TAB", "RETURN", "ESC", "INSERT", "DELETE", "PAGE UP", "PAGE DOWN",
                                        "HOME", "END","CAPS LOCK", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9",
                                        "F10", "F11", "F12", "F12" };
            for (int i = 0; i < holdArray.Length; i++)
            {
                holdCombo1.Items.Add(holdArray[i]);
                holdCombo2.Items.Add(holdArray[i]);
                holdCombo3.Items.Add(holdArray[i]);

            }
        }

        private void fill_media_combo()
        {


            String[] mediaArray = new String[]{"Media Fast Forward","Media Rewind","Media Next","Media Previous","Media Stop",
                                "Media Play/Pause","Media Volume Mute","Media Volume Up","Media Volume Down" };
            for (int i = 0; i < mediaArray.Length; i++)
            {
                MediaCombo.Items.Add(mediaArray[i]);
            }
          


        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {


        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void Media_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cancelButtonClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void okButtonClicked(object sender, RoutedEventArgs e)
        {
            bool holdChecked = (bool)holdCheckbox.IsChecked;
            bool letterChecked = (bool)letterCheckbox.IsChecked;
            bool mediacChecked = (bool)mediaCheckbox.IsChecked;

        }
    }
}
