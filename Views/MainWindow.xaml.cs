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
using System.Runtime.InteropServices;
using System.Drawing;
using System.Numerics;

namespace JClientBot
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

        private void AddPointMap(double x, double y, SolidColorBrush color)
        {
            Ellipse tri = new Ellipse()
            {
                Width = 10,
                Height = 10,
                Stroke = color, //Brushes.Red,
                StrokeThickness = 6
            };

            //map.Children.Add(tri);

            //좌표
            tri.SetValue(Canvas.LeftProperty, x);
            tri.SetValue(Canvas.TopProperty, y);
        }

        private void ID_Button_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Count_Text_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int i = 0;
            e.Handled = !int.TryParse(e.Text, out i);
        }

        private void Chat_Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Logout_Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void RandomMove_Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Ellipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
          
        }
    }
}
