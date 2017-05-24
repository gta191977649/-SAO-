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
using Un4seen.Bass;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace SAP_WPF
{
    /// <summary>
    /// Interaction logic for SAO_Widget.xaml
    /// </summary>
    /// 

    public partial class SAO_Widget : Window
    {

        System.Windows.Threading.DispatcherTimer Title_Timer = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer Progress_Timer = new System.Windows.Threading.DispatcherTimer();
        //Goble EVENT CHECK TIMER
        public System.Windows.Threading.DispatcherTimer Check_Timer = new System.Windows.Threading.DispatcherTimer();
        string tempChar = string.Empty;
        string tempText = string.Empty;
        public delegate void ScrollTextboxCallback(string t);
        private bool ListWindowsOpen = false;

        MainWindow Main;
        public SAO_List SAO_PlaylistWin;

        public SAO_Widget(MainWindow main)
        {
            InitializeComponent();
            Main=main;
            Title_Timer.Tick += new EventHandler(Title_TimerTickEvent);
            //Active Check Timer

            Check_Timer.Tick += new EventHandler(Check_TimerTickEvent);
            Check_Timer.Interval = TimeSpan.FromMilliseconds(1000);
            Check_Timer.IsEnabled = true;

            SAO_PlaylistWin = new SAO_List(Main);

            //Get　ｓｙｓｔｅｍ　ｖｏｌ
            var Vol_UI = new VOL_BG(main,this);

            Vol_UI.UI_SLIDER_VOL.Value = Main.GobalVol;

            if (main.GetSystemVol()<0)
            {
                UI_VOL.Content = "VOL:0";
            }
            else
            {
                UI_VOL.Content = "VOL:" + Main.GobalVol;
            }
            
           // UI_Title.Content = main.title.Content;
            if (main.IsPlay)
            {
                //Enable Title scrool timers
                ScrollTextBox(main.Song_Name);
                //Enables Time Display Timers
                Progress_Timer.Tick += new EventHandler(Progress_TimerTickEvent);
                Progress_Timer.Interval = TimeSpan.FromMilliseconds(1000);//Remember 1000ms = 1sec is enough for syn the time!!
                Progress_Timer.Start();
                UI_BU_PLAY.IsEnabled = true;


                
            }
            //Check if is playlist mode
            if (main.PlayListMode)
            {
                UI_BU_PREW.IsEnabled = true;
                UI_BU_SKIP.IsEnabled = true;
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window 
            this.DragMove();

        }
        public void ScrollTextBox(string Title)
        {


            //If > Max Display Space we generate the timer.
            //Prevent timer was not killed
            Title_Timer.IsEnabled = false;
            UI_Title.Content = Title;
            if (Title.Length > 20)
            {
                tempText = Title + "                    ";  // adds spaces at the end of the text so there is a clear ending to the text  
                //Start Timer for Bars displays
                Title_Timer.Interval = TimeSpan.FromMilliseconds(500);
                Title_Timer.IsEnabled = true;
            }
            else
            {
                //If is not
                UpdateTextBox(Title);
            }

        }



        private void Check_TimerTickEvent(object sender, EventArgs e)
        {
            //Make Sure it auto detected 
            if (Main.IsPlay)
            {
         
                //Enables Time Display Timers
                Progress_Timer.Tick += new EventHandler(Progress_TimerTickEvent);
                Progress_Timer.Interval = TimeSpan.FromMilliseconds(1000);//Remember 1000ms = 1sec is enough for syn the time!!
                Progress_Timer.Start();
                UI_BU_PLAY.IsEnabled = true;
                UI_BU_PLAY.Content = FindResource("Pause");

            }
            //Check for playlist mode
            if (Main.PlayListMode)
            {
                UI_BU_PREW.IsEnabled = true;
                UI_BU_SKIP.IsEnabled = true;
            }
            else
            {
                UI_BU_PREW.IsEnabled = false;
                UI_BU_SKIP.IsEnabled = false;
            }

        }
        private void Progress_TimerTickEvent(object sender, EventArgs e)
        {
            // Timer For Scrolling Text
            //First we have to convent it
            TimeSpan Max_Time = TimeSpan.FromSeconds(Main.GetStreamLength(Main.StreamHandle));
            string MaxTime = Max_Time.ToString("mm':'ss");
            TimeSpan Now_POS = TimeSpan.FromSeconds(Main.GetStreamPos(Main.StreamHandle));
            string NowPos = Now_POS.ToString("mm':'ss");

            UI_Time.Content = NowPos + "/" + MaxTime;
            
            //Don't let timer run all the way if the song is not playing cuz it waste of our resources!!!
            UI_Slider.Maximum = Main.GetStreamLength(Main.StreamHandle);
            UI_Slider.Value = Main.GetStreamPos(Main.StreamHandle);
            if (Main.GetStreamPos(Main.StreamHandle) == Main.GetStreamLength(Main.StreamHandle)-1) StopUITimer();

        }
        
        private void StopUITimer(){
            //Stop Running Timers
            Title_Timer.IsEnabled = false;
            Progress_Timer.IsEnabled = false;


        }
        private void StartUITimer()
        {
            ScrollTextBox(Main.Song_Name);
            Progress_Timer.Tick += new EventHandler(Progress_TimerTickEvent);
            Progress_Timer.Interval = TimeSpan.FromMilliseconds(1000);//Remember 1000ms = 1sec is enough for syn the time!!
            Progress_Timer.IsEnabled = true;
        }
        private void Title_TimerTickEvent(object sender, EventArgs e)
        {
            // Timer For Scrolling Text
            tempChar = tempText.Substring(0, 1);
            tempText = tempText.Remove(0, 1) + tempChar;
            UI_Title.Dispatcher.Invoke(new ScrollTextboxCallback(this.UpdateTextBox), new object[] { tempText });
        }

        private void UpdateTextBox(string m_text)
        {

            UI_Title.Content = m_text;

        }
        private void UI_BU_Plus_Click(object sender, RoutedEventArgs e)
        {

           // var window = IsWindowOpen<Window>("SAO_WINLIST");

            SAO_PlaylistWin.Left = this.Left - 270;
            SAO_PlaylistWin.Top = this.Top - 170;
            if (!ListWindowsOpen)
            {
                
                SAO_PlaylistWin.Show();
              
                var anim = new DoubleAnimation(275,429, (Duration)TimeSpan.FromMilliseconds(300));
                var oc = new DoubleAnimation(1, (Duration)TimeSpan.FromMilliseconds(200));
               
                SAO_PlaylistWin.BeginAnimation(FrameworkElement.HeightProperty, anim);
                SAO_PlaylistWin.BeginAnimation(FrameworkElement.OpacityProperty, oc);
                

           
                SAO_PlaylistWin.Focus();
                ListWindowsOpen = true;
            }
            else
            {
                

                //var anim = new DoubleAnimation(270,0, (Duration)TimeSpan.FromSeconds(1));
                var oc = new DoubleAnimation(0, (Duration)TimeSpan.FromMilliseconds(500));
                oc.Completed += (s, _) => SAO_PlaylistWin.Hide();
            //    SAO_PlaylistWin.BeginAnimation(FrameworkElement.WidthProperty, anim);
                SAO_PlaylistWin.BeginAnimation(FrameworkElement.OpacityProperty, oc);
                ListWindowsOpen = false;
            }
            /*
     
           if (window != null)
           {
               window.Hide();

           }
           else
           {
               
              SAO_PlaylistWin.Left = this.Left - SAO_PlaylistWin.Width;
              SAO_PlaylistWin.Top = this.Top - 170;
              SAO_PlaylistWin.Show();
              SAO_PlaylistWin.Focus();
           }
           */
           
        }

        private void UI_BU_PLAY_Click(object sender, RoutedEventArgs e)
        {
            if(Main.IsPlay)
            {
                Stop();

            }
            else
            {
                Play();
            }
        }


        private void Stop()
        {
            UI_BU_PLAY.Content = FindResource("Play");
            Bass.BASS_ChannelSlideAttribute(Main.StreamHandle, BASSAttribute.BASS_ATTRIB_VOL, 0f, 1000);
            System.Threading.Thread.Sleep(1000);
            Bass.BASS_ChannelPause(Main.StreamHandle);
            //Stop All timers
            Main.ResetTimers();
            StopUITimer();
            Main.IsPlay = false;
        }
        private void Play()
        {
            UI_BU_PLAY.Content = FindResource("Pause");
            Bass.BASS_ChannelSlideAttribute(Main.StreamHandle, BASSAttribute.BASS_ATTRIB_VOL, 1, 1);
            Bass.BASS_ChannelPlay(Main.StreamHandle, false);
            //Set Vol
          
            Main.SetVolume(Main.GobalVol);

            //Star all timers
            Main.SetTimers();
            StartUITimer();
            Main.IsPlay = true;
        }


        private void UI_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //If Value Changed?

        }

        /*
        private void UI_Vol_DragCompleted(object sender, DragCompletedEventArgs e)
        {
         //If User Dragged

            Main.SetVolume(Convert.ToSingle(Slider_Vol.Value));
            UI_VOL.Content = "VOL:" + Math.Round(Slider_Vol.Value);
        }
              */
        private void UI_Slider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
         //If User Dragged
            Main.SetStreamPos(Main.StreamHandle, UI_Slider.Value);
            
        }
   
        private void UI_BU_VOL_Click(object sender, RoutedEventArgs e)
        {
            var window = IsWindowOpen<Window>("VOL_BG1");
            if (window != null)
            {
                window.Close();

            }
            else
            {
                var Vol_WIN = new VOL_BG(Main,this);




                Vol_WIN.Left = this.Left + Vol_WIN.Width+130;
                Vol_WIN.Top = this.Top + Vol_WIN.Height -100;

                Vol_WIN.UI_SLIDER_VOL.Value = Main.GobalVol;
                Vol_WIN.Show();


                Vol_WIN.Focus();
            }
        }
        public static T IsWindowOpen<T>(string name = null)

        where T : Window
        {
            var windows = Application.Current.Windows.OfType<T>();

            return string.IsNullOrEmpty(name) ? windows.FirstOrDefault() : windows.FirstOrDefault(w => w.Name.Equals(name));
        }

        private void UI_BU_PREW_Click(object sender, RoutedEventArgs e)
        {
            PlayPre();
        }

        private void UI_BU_SKIP_Click(object sender, RoutedEventArgs e)
        {
            PlayNext();
        }

        public void PlayNextSingle()
        {
            switch (Main.PLAY_MODE)
            {
                case 0:
                    {
                        Bass.BASS_ChannelStop(Main.StreamHandle);
                        break;
                    }
                case 1:
                    {
                        Main.PlayStream(Main.StreamFilePath, 0);
                        
                        break;
                    }
                default:{
                    Bass.BASS_ChannelStop(Main.StreamHandle);
                    break;
                    }
                 
            }
        }
        public void PlayNext()
        {
            if (SAO_PlaylistWin.MaxListItems == 0)
            {
                Stop();
                return;
            }
            switch (Main.PLAY_MODE)
            {
                case 0://Sequence
                    {
                        SAO_PlaylistWin.SelectedIndex++;

                        if (SAO_PlaylistWin.SelectedIndex == SAO_PlaylistWin.MaxListItems )
                        {
                            SAO_PlaylistWin.SelectedIndex = 0;
                            Bass.BASS_ChannelPause(Main.StreamHandle);
                            UI_BU_PLAY.Content = FindResource("Play");
                            //Stop All timers
                            Main.ResetTimers();
                            StopUITimer();
                            Main.IsPlay = false;
                            return;
                        }
                        if (SAO_PlaylistWin.SelectedIndex > SAO_PlaylistWin.MaxListItems)
                        {
                            SAO_PlaylistWin.SelectedIndex = 0;
                        }
                        //If is incorrect format, jump next
                        if (!Main.CheckFileFormat(Main.nowPlaylist[SAO_PlaylistWin.SelectedIndex].ToString()))
                        {
                            SAO_PlaylistWin.SelectedIndex++;
                        }

                        SAO_PlaylistWin.PlayListStream(SAO_PlaylistWin.SelectedIndex);
                        break;
                    }
                case 1://Single Repear
                    {
                        SAO_PlaylistWin.PlayListStream(SAO_PlaylistWin.SelectedIndex);
                        break;
                    }
                case 2://List Repeat
                    {
                        SAO_PlaylistWin.SelectedIndex++;
                       
                        if (SAO_PlaylistWin.SelectedIndex == SAO_PlaylistWin.MaxListItems) SAO_PlaylistWin.SelectedIndex = 0;
                        if (SAO_PlaylistWin.SelectedIndex > SAO_PlaylistWin.MaxListItems) SAO_PlaylistWin.SelectedIndex = 0;
                        //If is incorrect format, jump next
                        if (!Main.CheckFileFormat(Main.nowPlaylist[SAO_PlaylistWin.SelectedIndex].ToString()))
                        {
                            SAO_PlaylistWin.SelectedIndex++;
                        }
                   
                     

                        SAO_PlaylistWin.PlayListStream(SAO_PlaylistWin.SelectedIndex);
                        break;
                    }
                case 3://Radom
                    {
                        Random r = new Random();
                        SAO_PlaylistWin.SelectedIndex = r.Next(1, SAO_PlaylistWin.MaxListItems - 1);

                        //If is incorrect format, jump next
                        if (!Main.CheckFileFormat(Main.nowPlaylist[SAO_PlaylistWin.SelectedIndex].ToString()))
                        {
                            SAO_PlaylistWin.SelectedIndex = r.Next(1, SAO_PlaylistWin.MaxListItems - 1);
                        }
                        SAO_PlaylistWin.PlayListStream(SAO_PlaylistWin.SelectedIndex);
                        break;
                    }
                default://Sequence
                    {
                        SAO_PlaylistWin.SelectedIndex++;

                        if (SAO_PlaylistWin.SelectedIndex == SAO_PlaylistWin.MaxListItems)
                        {
                            SAO_PlaylistWin.SelectedIndex = 0;
                            Bass.BASS_ChannelPause(Main.StreamHandle);
                            UI_BU_PLAY.Content = FindResource("Play");
                            //Stop All timers
                            Main.ResetTimers();
                            StopUITimer();
                            Main.IsPlay = false;
                            return;
                        }
                        if (SAO_PlaylistWin.SelectedIndex > SAO_PlaylistWin.MaxListItems)
                        {
                            SAO_PlaylistWin.SelectedIndex = 0;
                        }
                        //If is incorrect format, jump next
                        if (!Main.CheckFileFormat(Main.nowPlaylist[SAO_PlaylistWin.SelectedIndex].ToString()))
                        {
                            SAO_PlaylistWin.SelectedIndex++;
                        }

                        SAO_PlaylistWin.PlayListStream(SAO_PlaylistWin.SelectedIndex);
                        break;
                    }
            }
           
        }
        public void PlayPre()
        {
            SAO_PlaylistWin.SelectedIndex--;

            if (SAO_PlaylistWin.MaxListItems == 0)
            {
                SAO_PlaylistWin.SelectedIndex = 0;
                Stop();
                return;
            }
        
            if (SAO_PlaylistWin.SelectedIndex < 0 || SAO_PlaylistWin.SelectedIndex == 0)
            {
                //If index is reach to top
                SAO_PlaylistWin.SelectedIndex = 0;
                Main.ShowSAOMsgBox("已經到達最頂了!");
                return;
            }

            if (!Main.CheckFileFormat(Main.nowPlaylist[SAO_PlaylistWin.SelectedIndex].ToString()))
            {
                SAO_PlaylistWin.SelectedIndex--;
            }
            SAO_PlaylistWin.PlayListStream(SAO_PlaylistWin.SelectedIndex);
        }

        private void PLAYMODE_Click(object sender, RoutedEventArgs e)
        {
            if (Main.PlayListMode)
            {
                //if is play list mode
                switch (Main.PLAY_MODE)
                {
                    case 0:
                        {
                            Main.PLAY_MODE = 1;
                            PLAYMODE.Content = FindResource("SINGLE");
                            break;
                        }
                    case 1:
                        {
                            Main.PLAY_MODE = 2;
                            PLAYMODE.Content = FindResource("LIST");
                            break;
                        }
                    case 2:
                        {
                            Main.PLAY_MODE = 3;
                            PLAYMODE.Content = FindResource("RADOM");
                            break;
                        }
                    default:
                        {
                            Main.PLAY_MODE = 0;
                            PLAYMODE.Content = FindResource("SEQ");
                            break;
                        }
                }
            }
            else
            {
                //Single PlayMode
                switch (Main.PLAY_MODE)
                {
                    case 0:
                        {
                            Main.PLAY_MODE = 1;
                            PLAYMODE.Content = FindResource("SINGLE");
                            break;
                        }
                    case 1:
                        {
                            Main.PLAY_MODE = 0;
                            PLAYMODE.Content = FindResource("SEQ");
                            break;
                        }
                }
            }
        }

        private void SAO_MINI_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Closing -= SAO_MINI_Closing;
            e.Cancel = true;
            var anim = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(500));
            anim.Completed += (s, _) => this.Close();
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        private void UI_BU_Plus_MouseMove(object sender, MouseEventArgs e)
        {
            UI_BU_Plus.Content = FindResource("playlist_hover");
        }

        private void UI_BU_Plus_MouseLeave(object sender, MouseEventArgs e)
        {
            UI_BU_Plus.Content = FindResource("playlist");
        }
        
        public void UI_Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            /*
            EventHandler handler = UI_Slider_ValueChanged_1;
            if (handler != null)
                handler(this, e);
             * */
        }
        /*
        public event EventHandler UI_Slider_ValueChanged_1;
    
    
        */
   
    }
}
