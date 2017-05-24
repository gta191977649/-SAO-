using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace SAP_WPF
{

    /// <summary>
    /// Interaction logic for DestopLyrics.xaml
    /// </summary>
 
    public partial class DestopLyrics : Window
    {

     //   MainWindow Main;
        public DestopLyrics(MainWindow main)
        {
            InitializeComponent();
            //句柄腻值
            DestopLRC.InitLRC(canvasDeskLyric, canvasDeskLyricFore, textBlockDeskLyricBack, textBlockDeskLyricFore);
            
        }
 

        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            }
            catch { }
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            BTN_CLOSE.Visibility = Visibility.Visible;
            canvasDeskLyric.Background = Brushes.Black;
            canvasDeskLyric.Opacity = .3;
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            BTN_CLOSE.Visibility = Visibility.Hidden;
            canvasDeskLyric.Background = Brushes.Transparent;
            canvasDeskLyric.Opacity = 1;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            canvasDeskLyric.Background = Brushes.Black;
            canvasDeskLyric.Opacity = .5;
        }

        private void MSG_BUOK_MouseEnter(object sender, MouseEventArgs e)
        {
           // BTN_CLOSE.Visibility = Visibility.Visible;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
          //  BTN_CLOSE.Visibility = Visibility.Hidden;
        }

        private void BTN_CLOSE_MouseDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(1));
            anim.Completed += (s, _) => this.Hide();
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }
    }
}
