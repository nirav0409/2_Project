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
using System.Windows.Shapes;



namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            if (!String.IsNullOrEmpty(Constants.ardiunoDir)) {
                ardiunoPathTextBox.Text = Constants.ardiunoDir;
            }
        }

 

        private void browseButtonClicked(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if(result.ToString() == "OK")
            {
                ardiunoPathTextBox.Text = dialog.SelectedPath;
            }
        }

        private void OkButtonClicked(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(ardiunoPathTextBox.Text))
            {
                MessageBox.Show("Please Enter Ardiuno Folder path ");
            }
            else
            {
                Constants.ardiunoDir = ardiunoPathTextBox.Text;
                StreamWriter stream = new StreamWriter(".config");
                stream.WriteLine("ARDIUNO_PATH"+"="+ardiunoPathTextBox.Text);
                stream.Close();
                this.Close();
            }
        }
    }
}
