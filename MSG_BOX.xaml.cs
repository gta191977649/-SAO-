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
using System.Windows.Media.Animation;
using System.ComponentModel;
namespace SAP_WPF
{
    /// <summary>
    /// Interaction logic for MSG_BOX.xaml
    /// </summary>
    ///
    
    public partial class MSG_BOX : Window
    {
        public string Title_Msg;
        public MSG_BOX()
        {
            InitializeComponent();
            MSG_TITLE.Content = Title_Msg;
            
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Closing -= Window_Closing;
            e.Cancel = true;
            var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromMilliseconds(500));

            anim.Completed += (s, _) => this.Close();
            this.BeginAnimation(UIElement.OpacityProperty, anim);

        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window 
            this.DragMove();
        }
        private void MSG_BUOK_MouseMove(object sender, MouseEventArgs e)
        {
            MSG_BUOK.Content = FindResource("OK_Hover");
        }

        private void MSG_BUOK_MouseLeave(object sender, MouseEventArgs e)
        {
            MSG_BUOK.Content = FindResource("OK");
        }

        private void MSG_BUOK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

 
    }
}
