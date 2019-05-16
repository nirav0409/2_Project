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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
 
        }

        private void addLayout(object sender, RoutedEventArgs e)
        {
            CreateWindow createWindow = new CreateWindow();
            createWindow.Show();

        }
    }
}
