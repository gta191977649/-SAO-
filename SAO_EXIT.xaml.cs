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
using System.ComponentModel;
using System.Windows.Media.Animation;
namespace SAP_WPF
{
    /// <summary>
    /// Interaction logic for SAO_EXIT.xaml
    /// </summary>
    public partial class SAO_EXIT : Window
    {
       //声明继承句柄
        MainWindow MainWinHandle;
      
        public SAO_EXIT(MainWindow main)
        {
            MainWinHandle = main;//继承类

            InitializeComponent();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {

            
            Closing -= Window_Closing;
            e.Cancel = true;
            var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromMilliseconds(500));

            anim.Completed += (s, _) => this.Close();
            this.BeginAnimation(UIElement.OpacityProperty, anim);




            
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window 
            this.DragMove();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //先保存现存设定

            MainWinHandle.SaveSettings();
            //退出本程序
            Application.Current.Shutdown();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Close(); 
        }

        private void Button_MouseMove(object sender, MouseEventArgs e)
        {

            SAO_BUCANCEL.Content = FindResource("Cancel_Hover");

        }

        private void SAO_BUCANCEL_MouseLeave(object sender, MouseEventArgs e)
        {
            SAO_BUCANCEL.Content = FindResource("Cancel");
        }

        private void SAO_BUOK_MouseMove(object sender, MouseEventArgs e)
        {
            SAO_BUOK.Content = FindResource("OK_Hover");
        }

        private void SAO_BUOK_MouseLeave(object sender, MouseEventArgs e)
        {
            SAO_BUOK.Content = FindResource("OK");
        }
    

    }


}
