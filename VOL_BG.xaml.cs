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
using System.Windows.Controls.Primitives;
namespace SAP_WPF
{
    /// <summary>
    /// Interaction logic for VOL_BG.xaml
    /// </summary>
    public partial class VOL_BG : Window
    {

        MainWindow Main;
        SAO_Widget Widget;
        public VOL_BG(MainWindow main,SAO_Widget widget)
        {
            InitializeComponent( );
            Main = main;
            Widget = widget;
        }

        private void VOL_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window 
            this.DragMove();
        }

 
        private void UI_SLIDER_VOL_Dragged(object sender, DragCompletedEventArgs e)
        {
            //If User Dragged

            Main.SetVolume(Convert.ToSingle(UI_SLIDER_VOL.Value));
            Widget.UI_VOL.Content = "VOL:" + Math.Round(UI_SLIDER_VOL.Value);
        }

        private void UI_SLIDER_VOL_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Main.SetVolume(Convert.ToSingle(UI_SLIDER_VOL.Value));
            Widget.UI_VOL.Content = "VOL:" + Math.Round(UI_SLIDER_VOL.Value);
        }

        private void VOL_BG1_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Close();
        }

  
    }
}
