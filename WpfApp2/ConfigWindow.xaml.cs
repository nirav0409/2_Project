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
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {


        public ConfigWindow()
        {
            InitializeComponent();
            InitUI();
            sp = this.layoutPanel;
        }

        private void InitUI()
        {
            for (int i = 0; i < 5; i++)
            {
                Card c = new Card();
                c.layoutName.Content = "layoutName " + i;
                c.layoutColor.Background = Brushes.Blue;
                // layoutPanel.Items.Add(c);
                layoutPanel.Children.Add(c);
            }
        }
        Boolean isDown , isDragging;
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
            if(isDown == true && isDragging == false)
            {
                System.Diagnostics.Debug.WriteLine("Mouse Click");
            }
            isDown = false;
            isDragging = false;
            if(realDragSource != null)
                realDragSource.ReleaseMouseCapture();

        }

        private void myPreviewMouseMove(object sender, MouseEventArgs e)
        {
          //  Debug.WriteLine("PreViewMouseMove");
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
                    this.sp.Children.Remove(realDragSource);
                    this.sp.Children.Insert(droptargetIndex, realDragSource);
                }

                isDown = false;
                isDragging = false;
                realDragSource.ReleaseMouseCapture();
            }
        }


    }
}
