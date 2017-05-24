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
//Add Refrences
using Un4seen.Bass;
using Un4seen.Bass.Misc;
using Un4seen.Bass.AddOn.Midi;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.IO;
using System.Windows.Media.Animation;
using System.Threading;
using System.ComponentModel;
using System.Collections;
using System.Globalization;
using System.Resources;
namespace SAP_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

            //MultiLanguage
            ResourceManager res_man;    // declare Resource manager to access to specific cultureinfo
            CultureInfo cul;            //declare culture info


            private System.Windows.Forms.NotifyIcon notifyIcon;
           // private ContextMenu m_menu;  
            //Create Compments 手動創建控件
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            System.Windows.Threading.DispatcherTimer Effect_Timer = new System.Windows.Threading.DispatcherTimer();
            System.Windows.Threading.DispatcherTimer Progress_Timer = new System.Windows.Threading.DispatcherTimer();
            System.Windows.Threading.DispatcherTimer Lyrics_Timer = new System.Windows.Threading.DispatcherTimer();
            System.Windows.Threading.DispatcherTimer UpdateCheckTimer = new System.Windows.Threading.DispatcherTimer();

            System.Windows.Forms.PictureBox picturebox1 = new System.Windows.Forms.PictureBox();
            //Stream Handle 窗體句柄
            IntPtr windowHandle;


            public float GobalVol =100;
            //Stream Handle 播放句柄
            public int StreamHandle;
            //VST Handle
            public int DSPHandle;
            public bool LoadDsp=false;
            public string DspPath;
            //封裝
            private static Un4seen.Bass.Misc.Visuals Sp = new Un4seen.Bass.Misc.Visuals();
            //Gobal Default Settings Change if you wish to edit
            //必須使用全局變量，否則子窗口無法訪問!!
            public System.Drawing.Color Line_COR1 = System.Drawing.Color.Black;
            public System.Drawing.Color Line_COR2 = System.Drawing.Color.Black;
            public System.Drawing.Color Tick_COR = System.Drawing.Color.Black;
            public System.Drawing.Color Visual_BK = System.Drawing.Color.Transparent;
            public System.Windows.Media.Brush Lyrics_Color = System.Windows.Media.Brushes.Black;
            public int Stream_width = 2;
            public int Effect_Type = 0; //Change the type of effect
            public int Draw_Timer=1;

            public int Draw_Width = 230;
            public int Draw_Height=80;
            public string SoundFontPath;
            public string PlayListPath;
            public string Visual_Text;
            public string Song_Name;
            public string sTitle;
            public string sSinger;
            public string sAlbum;
            public string sYear;
            public string sComm;
        //String For Titiles
            string tempChar = string.Empty;
            string tempText = string.Empty;

            //囘調
        public delegate void ScrollTextboxCallback(string t);
            //Bools
        public bool IsPlay = false;
        public bool PlayListMode = false;
        public bool Lyrics = false;
        public bool AutoCheckLyrics = true;
        public bool AutoCheckOnlineLyrics = false;
        public bool SAO_MINIWINDOW = false;
        private bool LyricWindowsOpen = false;
        public bool AlwaysTop = false;
        public bool CheckUpdate = false;
        public int PLAY_MODE = 0;//0 sequence, 1 single repeat 2 list repeat, 3 radom

        //播放列表全局變量
        public ArrayList nowPlaylist = new ArrayList();
        public int list_SelectedIndex;
        public string LyricsPath;
        public string StreamFilePath;
        LyricParser lyrics;

        SAO_Widget SAO_WidgetWin;
        SAO_SETFORMS SAO_SettingWin;
        DestopLyrics SAO_DestopLRC;
    
        //Colors Tem
        public byte COLOR_R;
        public byte COLOR_G;
        public byte COLOR_B;
        //Destop Back LRCColor
        public byte LRC_BCOLOR_R;
        public byte LRC_BCOLOR_G;
        public byte LRC_BCOLOR_B;
        //Destop Mask LRCColor
        public byte LRC_MCOLOR_R;
        public byte LRC_MCOLOR_G;
        public byte LRC_MCOLOR_B;

        //Defult INI PATH
        //string AppPath = Convert.ToString(AppDomain.CurrentDomain.BaseDirectory);
        public string iniPath = Convert.ToString(AppDomain.CurrentDomain.BaseDirectory) + @"\System\Settings.ini";
        Storyboard ScrollTextInAnimation;
        public MainWindow()
        {

            
            InitializeComponent();

            //共享
         
            
            //Window Handle 獲取窗體句柄
            windowHandle = new WindowInteropHelper(Application.Current.MainWindow).Handle;

            SAO_WidgetWin = new SAO_Widget(this);
            SAO_SettingWin = new SAO_SETFORMS(this);
            SAO_DestopLRC = new DestopLyrics(this);
            Effect_Timer.Tick += new EventHandler(EffectTimerTickEvent);
            Progress_Timer.Tick += new EventHandler(Progress_TimerTickEvent);
    
            Lyrics_Timer.Tick += new EventHandler(LyricsTimerTickEvent);
            UpdateCheckTimer.Tick += new EventHandler(UpdateCheckDio);
            ScrollTextInAnimation = FindResource("ScrollText") as Storyboard;
            //加載插件
            LoadPlugin();

            //加載設置
            LoadSettings();

           // SAO_ListWin = new SAO_List(this);
            //加载播放列表
           // SAO_ListWin.LoadPlayList(PlayListPath);


           // ShowSAOMsgBox(PlayListPath);
            /*
            
            tem.playListPath = PlayListPath;
            nowPlaylist.AddRange(tem.PlayList);
             * */
            //加載音色庫
            //string AppPath = Convert.ToString(AppDomain.CurrentDomain.BaseDirectory);
            //LoadSoundFont(AppPath+@"\SoudFont\SF.sf2");
            LoadSoundFont(SoundFontPath);
            //加载托盘图标
            this.notifyIcon = new System.Windows.Forms.NotifyIcon();
           

            this.notifyIcon.Text = "SAO Music Player";
            this.notifyIcon.Icon = Properties.Resources.sound2_normal;
            this.notifyIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseDown);

            //更新检查
         //   if (CheckUpdate) MessageBox.Show("is checked");
            if (CheckUpdate)
            {
                UpdateCheckTimer.Interval = TimeSpan.FromSeconds(5);
                UpdateCheckTimer.IsEnabled = true;
            }


         
            
        }
        //Able to drage window
        public void LoadDefultSettings()
        {

            //當配置文件不存在時，加載默認設置.
            string AppPath = Convert.ToString(AppDomain.CurrentDomain.BaseDirectory);
            SoundFontPath = AppPath + @"\SoudFont\SF.sf2";
            Stream_width = 2;
            Draw_Timer = 1;
            AutoCheckLyrics = true;
            AutoCheckOnlineLyrics = false;
            AlwaysTop = false;
            CheckUpdate = true;
            COLOR_R = 0; COLOR_G = 0; COLOR_B = 0;
            Visual_Text = "Hello World";


        }
        private void LoadSettings()
        {

            
            if(!File.Exists(iniPath))
            {
                ShowSAOMsgBox("無法找到系統配置文件ini,系統將加載默認設置。");
                LoadDefultSettings();
      
               
                return;
            }
            IniParser SettingInI = new IniParser(iniPath);

            SoundFontPath = SettingInI.GetSetting("SAO_PLAYER_SOUNDFONT", "SOUNDFONTPATH");
            PlayListPath = SettingInI.GetSetting("SAO_PLAYER_PLAYLIST", "PLAYLISTPATH");

           // ShowSAOMsgBox(PlayListPath);


          /*  var newWindow = new SAO_List(this);
            newWindow.LoadPlayList(PlayListPath);
            */

            Stream_width = Convert.ToInt32(SettingInI.GetSetting("SAO_PlAYER_SETTINGS", "STREAM_WIDTH"));
            Draw_Timer = Convert.ToInt32(SettingInI.GetSetting("SAO_PlAYER_SETTINGS", "DRAW_TIMER"));
            if (Convert.ToBoolean(SettingInI.GetSetting("SAO_PlAYER_SETTINGS", "AutolyricDetectTick")))
            {
                AutoCheckLyrics = true;
            }
            else
            {
                AutoCheckLyrics = false;
            }
            if (Convert.ToBoolean(SettingInI.GetSetting("SAO_PlAYER_SETTINGS", "AutoOnlineLyricDetectTick")))
            {
                AutoCheckOnlineLyrics = true;
            }
            else
            {
                AutoCheckOnlineLyrics = false;
            }
            if (Convert.ToBoolean(SettingInI.GetSetting("SAO_PlAYER_SETTINGS", "AlwaysTop")))
            {
                this.Topmost = true;
                SAO_WidgetWin.Topmost = true;
                SAO_WidgetWin.SAO_PlaylistWin.Topmost = true;
                AlwaysTop = true;
            }
            else
            {
                AlwaysTop = false;
            }
            if (Convert.ToBoolean(SettingInI.GetSetting("SAO_PlAYER_SETTINGS", "CheckUpdate")))
            {
                CheckUpdate = true;
            }
            else
            {
                CheckUpdate = false;
            }
            COLOR_R = Convert.ToByte(SettingInI.GetSetting("SAO_PlAYER_SETTINGS", "LYRICS_COLOR_R"));
            COLOR_G = Convert.ToByte(SettingInI.GetSetting("SAO_PlAYER_SETTINGS", "LYRICS_COLOR_G"));
            COLOR_B = Convert.ToByte(SettingInI.GetSetting("SAO_PlAYER_SETTINGS", "LYRICS_COLOR_B"));
            Lyrics_Color = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(COLOR_R, COLOR_G, COLOR_B));
            //Destop LRC Back Color
            LRC_BCOLOR_R = Convert.ToByte(SettingInI.GetSetting("SAO_PlAYER_SETTINGS", "LYRICS_BCOLOR_R"));
            LRC_BCOLOR_G = Convert.ToByte(SettingInI.GetSetting("SAO_PlAYER_SETTINGS", "LYRICS_BCOLOR_G"));
            LRC_BCOLOR_B = Convert.ToByte(SettingInI.GetSetting("SAO_PlAYER_SETTINGS", "LYRICS_BCOLOR_B"));
            //Destop LRC Mask Color
            LRC_MCOLOR_R = Convert.ToByte(SettingInI.GetSetting("SAO_PlAYER_SETTINGS", "LYRICS_MCOLOR_R"));
            LRC_MCOLOR_G = Convert.ToByte(SettingInI.GetSetting("SAO_PlAYER_SETTINGS", "LYRICS_MCOLOR_G"));
            LRC_MCOLOR_B = Convert.ToByte(SettingInI.GetSetting("SAO_PlAYER_SETTINGS", "LYRICS_MCOLOR_B"));
            //Apply Color
           // Color Mask =  Sys.FromArgb(1,LRC_MCOLOR_R, LRC_MCOLOR_G, LRC_MCOLOR_B);
            //Deal with WPF List
       

            DestopLRC.ApplyLyricsColor(System.Drawing.Color.FromArgb(LRC_MCOLOR_R, LRC_MCOLOR_G, LRC_MCOLOR_B));
            DestopLRC.ApplyLyricsMColor(System.Drawing.Color.FromArgb(LRC_BCOLOR_R, LRC_BCOLOR_G, LRC_BCOLOR_B));
           // DestopLRC.ApplyLyricsMColor(Color.FromArgb(Main.LRC_BCOLOR_R, Main.LRC_BCOLOR_G, Main.LRC_BCOLOR_B));


            Visual_Text = SettingInI.GetSetting("SAO_PlAYER_SETTINGS", "VISUAL_TEXT");

            /*
               
               

               
            //   Lyrics_Color = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.);
               
               Lyrics_Color = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(COLOR_R, COLOR_G, COLOR_B));

               //SettingInI.GetSetting("SAO_PlAYER_SETTINGS", "Lyrics_Font");
               SoundFontPath = SettingInI.GetSetting("SAO_PLAYER_SOUNDFONT", "SOUNDFONTPATH");
               */
        }
        public void SaveSettings()
        {

            // IniParser SettingInI = new IniParser(@"C:\\test.ini");
            if (!File.Exists(iniPath))
            {
                File.WriteAllText(@iniPath, "");
            }


            IniParser SettingInI = new IniParser(@iniPath);
            /*
                     public bool IsPlay = false;
        public bool PlayListMode = false;
        public bool Lyrics = false;
        public bool AutoCheckLyrics = true;
        public bool AutoCheckOnlineLyrics = false;
        public bool SAO_MINIWINDOW = false;
        private bool LyricWindowsOpen = false;
        public bool AlwaysTop = false;
        public bool CheckUpdate = false;
        public int PLAY_MODE = 0;//0 sequence, 1 single repeat 2 list repeat, 3 radom
             * 
             * */
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "Stream_width", Stream_width.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "Draw_Timer", Draw_Timer.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "AutolyricDetectTick", AutoCheckLyrics.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "AutoOnlineLyricDetectTick", AutoCheckOnlineLyrics.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "AlwaysTop", AlwaysTop.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "CheckUpdate", CheckUpdate.ToString());

            //Save Color
           // lyrics_pre.Select(8, 10);
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "Lyrics_Color_R", COLOR_R.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "Lyrics_Color_G", COLOR_G.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "Lyrics_Color_B", COLOR_B.ToString());
            //Back Color
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "LYRICS_BCOLOR_R", LRC_BCOLOR_R.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "LYRICS_BCOLOR_G", LRC_BCOLOR_G.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "LYRICS_BCOLOR_B", LRC_BCOLOR_B.ToString());
            //Mask Color

            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "LYRICS_MCOLOR_R", LRC_MCOLOR_R.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "LYRICS_MCOLOR_G", LRC_MCOLOR_G.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "LYRICS_MCOLOR_B", LRC_MCOLOR_B.ToString());


            //lyrics_pre.DeselectAll();

            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "Lyrics_Font", lyrics_box.FontFamily.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SOUNDFONT", "SoundFontPath", SoundFontPath);
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "VISUAL_TEXT", Visual_Text);
            //播放列表处理
            SettingInI.AddSetting("SAO_PLAYER_PLAYLIST", "PLAYLISTPATH", PlayListPath);

            SettingInI.SaveSettings();


        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            dlg.Filter = "All Media Type|*.mid;*.mp3;*.wav;*.flac;*.ape;*.tta;*.aac";
            Nullable<bool> result = dlg.ShowDialog();
          
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;

               // string safefilename = dlg.SafeFileName;
              
                
                PlayStream(filename, 0);

                //GetStreamInfo(safefilename);
               
            }

        }


        public void PlayStream(string URL, int type)
        {

     
            //Check File Format
            if (!CheckFileFormat(URL))
            {
                ShowSAOMsgBox("不是有效的音頻文件哦!");
                return;
            }

            ResetTimers();

            if (Lyrics == true)
            {
                Lyrics_Timer.IsEnabled = false;
                lyrics_box.Text = "";
                DestopLRC.ResetLRC();
                Lyrics = false;
            }

            string Title = System.IO.Path.GetFileNameWithoutExtension(URL);
            //Given Value
            StreamFilePath = URL;
            
            
            
            
            // Bass.BASS_ChannelGetTags.
            Song_Name = Title;
            ScrollTextBox(Song_Name);
           //Prevet crash becaouse window not created
            var window = IsWindowOpen<Window>("SAO_MINI");
            if (window != null)
            {
                SAO_WidgetWin.ScrollTextBox(Title);
            }

            
            switch(type)
            {
                case 0:
                    {

                        Bass.BASS_Free();
                        //Prevent Timer Set twice
                        
                        Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, windowHandle);
                        //Load file from URLS

                        //Here we have to prevent if the file is invaild
                        /*
                         * As the bass.dll API said
                         * Return value
                         * If successful, the new stream's handle is returned, else 0 is returned. Use BASS_ErrorGetCode to get the error code.
                         * So if the return value is 0, that mans the file is invaild and not being created in memory.
                         * */
                        if (Bass.BASS_StreamCreateFile(URL, 0, 0, 0)!=0)
                        {
                            StreamHandle = Bass.BASS_StreamCreateFile(URL, 0, 0, 0);
                           // GetStreamInfo(URL, StreamHandle);
                            Bass.BASS_ChannelPlay(StreamHandle, false);

                            SetVolume(GobalVol);
                            //Check Lyrics
                            if (AutoCheckLyrics) CheckLyricsExists(URL);
                            SetTimers();
                            
                        }
                        else
                        {
                            //if file is invaild, Stop Drawing Time to prevent crash.
                            ResetTimers();
                        }

                        if(LoadDsp)
                        {
                            LoadDSPlugin(DspPath);
                        }
                       
               
                       

                        break;
                    }
                case 1:
                    {

                        break;
                    }
                   

            }

            //Tell is in play mode
            IsPlay = true;

        }

        //Scrolling Text title 滾蛋標題文件實現

        private void ScrollTextBox(string Title)
        {

  
            //If > Max Display Space we generate the timer.
            //Prevent timer was not killed
            
            title.Text = Title;
            if (Title.Length>18)
            {
                /*
                var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromMilliseconds(500));
                anim.Completed += (s, _) => this.Close();

                TranslateTransform myTranslate = new TranslateTransform();
                 **/
                
                ScrollTextInAnimation.Begin();
               
             //   title.BeginAnimation(myTranslate.X, anim);

            }
            else
            {
                //If is not
                ScrollTextInAnimation.Stop();
                title.Text= Title;
            }
           
        }

  

        private void UpdateCheckDio (object sender, EventArgs e)
        {
            UpdateCheckTimer.Stop();
            SAO_SettingWin.updatecheck();
        }
        private void LyricsTimerTickEvent(object sender, EventArgs e)
        {
            // Timer For Scrolling Text
            

         //   if (lyrics.Lyrics(GetStreamPos(StreamHandle)) == null) Lyrics_Timer.Stop(); return;
            TimeSpan Current_Time = TimeSpan.FromSeconds(GetStreamPos(StreamHandle));
            string CurrentTime = string.Format("{0}:{1}.{2}", Current_Time.Minutes,Current_Time.Seconds,Current_Time.Milliseconds);
          
            //prevent get null value caused FC
            
            lyrics.Refresh(GetStreamPos(StreamHandle));
          
            //歌词绘制
       
            lyrics_box.Inlines.Clear();

         //   lyrics_box.Inlines.Add(new Run(lyrics.TimeArray[lyrics.CurrentIndex].ToString()));

            lyrics_box.Inlines.Add(new Run(lyrics.PreviousLyrics));
           
            lyrics_box.Inlines.Add(new Bold(new Run("\n"+lyrics.CurrentLyrics)) {Foreground = Lyrics_Color});
            lyrics_box.Inlines.Add(new Run("\n" + lyrics.NextLyrics));


            if(LyricWindowsOpen) DestopLRC.UpdateLyrics(lyrics.CurrentLyrics, lyrics.TimeArray[lyrics.CurrentIndex].ToString(), lyrics.TimeArray[lyrics.CurrentIndex+1].ToString());

        }
        /*
        public void  UI_Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ShowSAOMsgBox("Hello World");
        }
        */
        private void Progress_TimerTickEvent(object sender, EventArgs e)
        {
            
            // Timer For Scrolling Text
            //First we have to convent it
            TimeSpan Max_Time = TimeSpan.FromSeconds(GetStreamLength(StreamHandle));
            string MaxTime = Max_Time.ToString("mm':'ss");
            TimeSpan Now_POS = TimeSpan.FromSeconds(GetStreamPos(StreamHandle));
            string NowPos = Now_POS.ToString("mm':'ss");

            SAO_Label_Progress.Content = NowPos + "/" + MaxTime;
            //For reset timers otherwise you may get program crashed~~!!!

            if(SAO_WidgetWin.UI_Slider.Value ==  SAO_WidgetWin.UI_Slider.Maximum)
            //if (GetStreamPos(StreamHandle) == GetStreamLength(StreamHandle))
            {
                //ResetTimers();
              
                if (PlayListMode == true)
                {
                    //Stop Timers otherwise get bug
                    ResetTimers();
                    PlayNext();
                  //  PlayNext();
                  //  SAO_ListWin.nowPlaylist;

                }
                else 
                {

                    ResetTimers();
                    var window = IsWindowOpen<Window>("SAO_MINI");
                    if (window != null)
                    {
                        SAO_WidgetWin.PlayNextSingle();
                    }

                    
                    
                }
              
            }
            

        }
        public void ResetPlayListPath()
        {
            PlayListPath = null;
        }
        public void PlayNext()
        {

            SAO_WidgetWin.PlayNext();
            SetVolume(GobalVol);
        }

        public void PlayPrew()
        {
            SAO_WidgetWin.PlayPre();
            SetVolume(GobalVol);
        }

        public double GetStreamLength(int stream_handle)
        {
             return Convert.ToDouble( Math.Round(Bass.BASS_ChannelBytes2Seconds(stream_handle, Bass.BASS_ChannelGetLength(stream_handle, Un4seen.Bass.BASSMode.BASS_POS_BYTES))));
        }
        public double GetStreamPos(int stream_handle)
        {
            return  Convert.ToDouble(Math.Round(Bass.BASS_ChannelBytes2Seconds(stream_handle, Bass.BASS_ChannelGetPosition(stream_handle, Un4seen.Bass.BASSMode.BASS_POS_BYTES))));
        }
        public void SetStreamPos(int stream_handle, double SliderbarValue)
        {
            Bass.BASS_ChannelSetPosition(stream_handle, Bass.BASS_ChannelSeconds2Bytes(stream_handle, SliderbarValue), BASSMode.BASS_POS_BYTES);


        }

        public void SetVolume(float Vol)
        {
            GobalVol = Vol;
            Bass.BASS_ChannelSetAttribute(StreamHandle, Un4seen.Bass.BASSAttribute.BASS_ATTRIB_VOL, Vol / 100);
     
        }

       
        public float GetSystemVol()
        {

            return  Bass.BASS_GetVolume() * 100;
        }
        
        public void ResetTimers()
        {
            Effect_Timer.IsEnabled = false;
            Progress_Timer.IsEnabled = false;
           // ScrollTextInAnimation.Stop();
            IsPlay = false;

           
        }
        public void SetTimers()
        {
            //Start Timer for Bars displays

            Effect_Timer.Interval = TimeSpan.FromMilliseconds(Draw_Timer);
            Effect_Timer.IsEnabled = true;

            //Start Timer for progress Display

            Progress_Timer.Interval = TimeSpan.FromMilliseconds(1000);
            Progress_Timer.IsEnabled = true;

            if (Lyrics == true)
            {
                Lyrics_Timer.Interval = TimeSpan.FromMilliseconds(500);
                Lyrics_Timer.IsEnabled = true;
            }
            //Start Scrool Title Timers
          //  Title_Timer.IsEnabled = true;
            IsPlay = true;
        }
        private void EffectTimerTickEvent(object sender, EventArgs e)
        {
            // Timer For Draw Bars
            if (!Bass.BASS_ChannelPlay(StreamHandle, false))
            {
                return;
            }
            switch(Effect_Type)
            {
                case 0:
                    {
                       
                        Bar_Display.Source = CreateBitmapSourceFromBitmap(Sp.Create ectrumLinePeak(StreamHandle, Draw_Width, Draw_Height, Line_COR1, Line_COR2, Tick_COR, Visual_BK, Stream_width, 3, 1, 30, false, false, true));
                        break;
                    }
                case 1:
                    {
                        Bar_Display.Source = CreateBitmapSourceFromBitmap(Sp.CreateWaveForm(StreamHandle, Draw_Width, Draw_Height, Line_COR1, Line_COR2, Tick_COR, Visual_BK, Stream_width, false, false, true));
                        break;
                    }
                case 2://Dacing Text?
                    {
                        //Bar_Display.Source = CreateBitmapSourceFromBitmap(Sp.CreateSpectrumEllipse(StreamHandle,Draw_Width,Draw_Height,Line_COR1,Line_COR2,Visual_BK,Stream_width,10,false,false,false));
                        Bar_Display.Source = CreateBitmapSourceFromBitmap(Sp.CreateSpectrumText(StreamHandle, Draw_Width, Draw_Height, Line_COR1, Line_COR2, Visual_BK,Visual_Text, false, false, false));
                        
                        break;
                    }
                case 3:
                    {
                        Bar_Display.Source = CreateBitmapSourceFromBitmap(Sp.CreateSpectrumBean(StreamHandle, Draw_Width, Draw_Height, Line_COR1, Line_COR2, Visual_BK, Stream_width, false, false, true));
               
                        break;
                    }

            }

            

           
        }
        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

     
        public static BitmapSource CreateBitmapSourceFromBitmap(System.Drawing.Bitmap bitmap)
        {
            
            if (bitmap == null)
               throw new ArgumentNullException("bitmap");

            IntPtr hBitmap = bitmap.GetHbitmap();

            try
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }
        }

        private void LoadPlugin()
        {

            string AppPath = Convert.ToString(AppDomain.CurrentDomain.BaseDirectory);
            Bass.BASS_PluginLoad("bass.dll");
            Bass.BASS_PluginLoad(AppPath + @"\Plugins\bassmidi.dll");
            Bass.BASS_PluginLoad(AppPath + @"\Plugins\bass_ape.dll");
            Bass.BASS_PluginLoad(AppPath + @"\Plugins\bassflac.dll");
            Bass.BASS_PluginLoad(AppPath + @"\Plugins\bass_tta.dll");
            Bass.BASS_PluginLoad(AppPath + @"\Plugins\basswma.dll");
            Bass.BASS_PluginLoad(AppPath + @"\Plugins\bass_aac.dll");
            Bass.BASS_PluginLoad("bass_wadsp.dll");

            
           

            
        }
        private void GetStreamInfo(string URL,int StreamHandle)
        {

            string extension = System.IO.Path.GetExtension(URL);
            string SongFileName = System.IO.Path.GetFileNameWithoutExtension(URL);
           // IntPtr tag;
            string[] tags;

            // "All Media Type|*.mid;*.mp3;*.wav;*.flac;*.ape;*.tta;*.aac"
             

            switch (extension)
            {
         
                case ".mp3":
                    {
                    tags = Un4seen.Bass.Bass.BASS_ChannelGetTagsID3V1(StreamHandle);
                    if (tags != null)
                    {
                        if (tags[0] ==string.Empty)
                        {
                            tags[0] = SongFileName;
                        }
                        ScrollTextBox(tags[1] + " - " + tags[0]);

                        sSinger = tags[1];
                    }
                    sSinger = "Unknown";
                        break;

                    }
                case ".wav":
                    {

                    tags = Un4seen.Bass.Bass.BASS_ChannelGetTagsWMA(StreamHandle);
                    if (tags != null)
                    {
                        ScrollTextBox(tags[1] + " - " + tags[0]);
                        sSinger = tags[1];
                    }
                    sSinger = "Unknown";
                        break;

                    }
                default : {
                    Un4seen.Bass.AddOn.Tags.TAG_INFO tagInfo = new Un4seen.Bass.AddOn.Tags.TAG_INFO(URL);
                        //   ScrollTextBox( tagInfo.album.ToString());
                    if (Un4seen.Bass.AddOn.Tags.BassTags.BASS_TAG_GetFromURL(StreamHandle, tagInfo))
                    {
                        ScrollTextBox(tagInfo.ToString() + " - " + tagInfo.artist.ToString());
                        //If Window is create, passing values
                        var window = IsWindowOpen<Window>("SAO_MINI");
                        if (window != null)
                        {
                            SAO_WidgetWin.ScrollTextBox(tagInfo.ToString() + " - " + tagInfo.artist.ToString());
                        }

                
                        Song_Name= tagInfo.ToString() + " - " + tagInfo.artist.ToString();
                        sSinger = tagInfo.artist.ToString();
                    }
                    sSinger = "Unknown";
                    break;
                }
        

            }



       

            
          //   MEDIAINFO.Content = tags;
            
           
        }
        private void LoadSoundFont(string url)
        {
            SoundFontPath = url;
            IntPtr fontlocation = Marshal.StringToHGlobalAnsi(url);
            Bass.BASS_SetConfigPtr(Un4seen.Bass.BASSConfig.BASS_CONFIG_MIDI_DEFFONT, fontlocation);

        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window 
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var window = IsWindowOpen<Window>("EXT_MGS");
            if (window != null)
            {
                window.Focus();
            }
            else
            {
                var newWindow = new SAO_EXIT(this);
              

                //Make Window pop up with mous pos
                newWindow.Left = this.Left;
                newWindow.Top = this.Top;

                System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.alert);
                player.Play();
                newWindow.Show();
                
            }  
        }

        public void ShowSAOMsgBox(string msg, string caption = "Alert")
        {

            var window = IsWindowOpen<Window>("SAO_MSG");
            if (window != null)
            {
                window.Focus();
            }
            else
            {
                var MsgWindow = new MSG_BOX();
                MsgWindow.Left = this.Left;
                MsgWindow.Top = this.Top;


                System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.alert);
                player.Play();
                MsgWindow.MSG_TITLE.Content = msg;
                MsgWindow.MSG_CAP.Content = caption;
                MsgWindow.Show();
                MsgWindow.Focus();
            }
        }


        public static T IsWindowOpen<T>(string name = null)
        where T : Window
        {
            var windows = Application.Current.Windows.OfType<T>();
            
            return string.IsNullOrEmpty(name) ? windows.FirstOrDefault() : windows.FirstOrDefault(w => w.Name.Equals(name));
        }

 
  
        private void SAO_BU_EFFECT_Click(object sender, RoutedEventArgs e)
        {
            switch (Effect_Type)
            {
                case 0:
                {
                    Effect_Type=1;
                    break;
                }
                case 1:
                {
                    Effect_Type =2;
                    break;
                }
                case 2:
                {
                    Effect_Type = 3;
                    break;
                }
                case 3:
                {
                    Effect_Type = 0;
                    break;
                }
                default:
                {
                    Effect_Type = 0;
                    break;
                }

            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Call Setting Dialog and parmeter 1 is for this window handle
                
                new SAO_SETFORMS(this).Show();
            //prevent to open twice times
                this.IsEnabled = false;
       
        }

        private void SAO_PlayerWindow_Drop(object sender, DragEventArgs e)
        {
           var files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach(var file in files)
            {
             //   string Filename = System.IO.Path.GetFileName(file);
              
                //    Song_Name = Filename;
                  //  ScrollTextBox(Song_Name);
                    PlayStream(file, 0);
            }
        }

        public bool CheckFileFormat(string filepath){
            string extension = System.IO.Path.GetExtension(filepath);
            string Filename = System.IO.Path.GetFileName(filepath);
            // "All Media Type|*.mid;*.mp3;*.wav;*.flac;*.ape;*.tta;*.aac"
            if (extension == ".mid" || extension == ".mp3" || extension == ".wav" || extension == ".flac" || extension == ".ape" || extension == ".tta" || extension == ".aac" || extension == ".ogg")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void CheckLyricsExists(string filepath)
        {
            string LyricPath = System.IO.Path.GetDirectoryName(filepath)+@"\"+System.IO.Path.GetFileNameWithoutExtension(filepath)+".lrc";

          //  MessageBox.Show(LyricPath);
            if (System.IO.File.Exists(LyricPath))
            {
                LoadLyrics(LyricPath);

            }
            else
            {
                if (AutoCheckOnlineLyrics)
                {
                    lyric_search LyricDialog = new lyric_search(this);
                    LyricDialog.ShowDialog();
                }
                else
                {
                    return;
                }
          
            }
 
    
        }
        private void SAO_Widget_Switch_Click(object sender, RoutedEventArgs e)
        {
            var window = IsWindowOpen<Window>("SAO_MINI");
            
        

            if (!SAO_MINIWINDOW)
            {

                SAO_WidgetWin.Show();
                var anim = new DoubleAnimation(1, (Duration)TimeSpan.FromSeconds(1));
                SAO_WidgetWin.BeginAnimation(UIElement.OpacityProperty, anim);
                SAO_MINIWINDOW = true;
            }
            else
            {
                var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(1));
                anim.Completed += (s, _) => SAO_WidgetWin.Hide();
                SAO_WidgetWin.BeginAnimation(UIElement.OpacityProperty, anim);
                
                SAO_MINIWINDOW = false;
            }
            
            /*
            
            if (window != null)
            {
                //Stop Weiget Timer 
               
               // SAO_WidgetWin.Check_Timer.IsEnabled = false;

                //SAO_WidgetWin.Close();
                window.Hide();
                SAO_MINIWINDOW = false;
            }
            else
            {
            
                SAO_WidgetWin.Show();
            }  
       * */




        }

        private void DSP_Button_Click_3(object sender, RoutedEventArgs e)
        {
           
            if(IsPlay)
            {
                ShowSAOMsgBox("請先停止播放音樂,在加載DSP插件哦~");
            }
            else
            {

                dlg.Filter = "DLLs|*.dll";
                Nullable<bool> result = dlg.ShowDialog();
                DspPath = dlg.FileName;
                //tell we gonna load dsp after createdsteams
                LoadDsp = true;
            }

  
            

      
          
             
          


            
        }



        private void LoadDSPlugin(string url)
        {
            //title.Content = filename;
            Un4seen.Bass.AddOn.WaDsp.BassWaDsp.BASS_WADSP_Free();
            Un4seen.Bass.AddOn.WaDsp.BassWaDsp.BASS_WADSP_Init(windowHandle);
            DSPHandle = Un4seen.Bass.AddOn.WaDsp.BassWaDsp.BASS_WADSP_Load(url, 5, 5, 100, 100, null);
            Un4seen.Bass.AddOn.WaDsp.BassWaDsp.BASS_WADSP_Start(DSPHandle, 0, 0);
            Un4seen.Bass.AddOn.WaDsp.BassWaDsp.BASS_WADSP_ChannelSetDSP(DSPHandle, StreamHandle, 1);

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
        
        }

        private void sf_Click(object sender, RoutedEventArgs e)
        {

            dlg.Filter = "Soundfont files|*.sf2";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;

                // string safefilename = dlg.SafeFileName;


                LoadSoundFont(filename);
                //GetStreamInfo(safefilename);

            }
        }

        public void LoadLyrics(string LyricsPath)
        {
            lyrics = new LyricParser(LyricsPath, Encoding.Default);
            Lyrics = true;


        }
        private void lrc_Click(object sender, RoutedEventArgs e)
        {
       /*
            if(LyricsPath==null)
            {
                dlg.Filter = "LRC lyrics file|*.lrc";
                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    // Open document 
                    LyricsPath = dlg.FileName;
                    lyrics = new LyricParser(LyricsPath, Encoding.Default);
                    Lyrics = true;

                }

            }
            else
            {
                new lyrics_settings(this).Show();
                //prevent to open twice times
                this.IsEnabled = false;

            }
        * */
         /*
            dlg.Filter = "LRC lyrics file|*.lrc";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                LoadLyrics(dlg.FileName);
                // Open document 
            }
            */
            /*
            if (AutoCheckOnlineLyrics)
            {
                AutoCheckOnlineLyrics = false;
                lrc.Content = FindResource("LRC");
            }
            else
            {
                AutoCheckOnlineLyrics = true;
                lrc.Content = FindResource("LRC_hover");
                
            }
             */
            if(LyricWindowsOpen)
            {

            }
            else
            {

            }

            if (!LyricWindowsOpen)
            {

                SAO_DestopLRC.Show();
                var anim = new DoubleAnimation(1, (Duration)TimeSpan.FromSeconds(1));
                SAO_DestopLRC.BeginAnimation(UIElement.OpacityProperty, anim);
                LyricWindowsOpen = true;
            }
            else
            {
                var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(1));
                anim.Completed += (s, _) => SAO_DestopLRC.Hide();
                SAO_DestopLRC.BeginAnimation(UIElement.OpacityProperty, anim);

                LyricWindowsOpen = false;
            }

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            new lyric_search(this).Show();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            new lyric_search(this).Show();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Closing -= Window_Closing;
            e.Cancel = true;
            var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(1));
            anim.Completed += (s, _) => Application.Current.Shutdown();
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            ResetPlayListPath();
        }

        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
          
            add.Content = FindResource("Add_Hover");
          /*
            Storyboard ButtonAnimation = FindResource("buttonopc") as Storyboard;
            ButtonAnimation.Begin();
           * */
        }

        private void add_MouseLeave(object sender, MouseEventArgs e)
        {
            add.Content = FindResource("Add");

        }

        private void Button_MouseMove_1(object sender, MouseEventArgs e)
        {
            setting.Content = FindResource("Setting_Hover");
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            setting.Content = FindResource("Setting");
        }

        private void sf_MouseMove(object sender, MouseEventArgs e)
        {
            sf.Content = FindResource("Soundfont_hover");
        }

        private void sf_MouseLeave(object sender, MouseEventArgs e)
        {
            sf.Content = FindResource("Soundfont");
        }


   

        

        private void lrc_MouseMove(object sender, MouseEventArgs e)
        {
          //  lrc.Content =  FindResource("LRC_hover");
        }

        private void lrc_MouseLeave(object sender, MouseEventArgs e)
        {
            //lrc.Content = FindResource("LRC");
        }

        private void Button_MouseMove_2(object sender, MouseEventArgs e)
        {
            bu_exit.Content = FindResource("exit_hover");
        }

        private void bu_exit_MouseLeave(object sender, MouseEventArgs e)
        {
            bu_exit.Content = FindResource("exit");
        }

        private void SAO_Widget_Switch_MouseMove_1(object sender, MouseEventArgs e)
        {
            SAO_Widget_Switch.Content = FindResource("BU_Weiget_hover");
         
        }

        private void SAO_Widget_Switch_MouseLeave_1(object sender, MouseEventArgs e)
        {
            SAO_Widget_Switch.Content = FindResource("BU_Weiget");
        }

        private void SAO_Widget_Switch_MouseDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void SAO_BU_DSP_MouseMove(object sender, MouseEventArgs e)
        {
            
            SAO_BU_DSP.Content = FindResource("dsp_hover"); 
        }

        private void SAO_BU_DSP_MouseLeave(object sender, MouseEventArgs e)
        {
            SAO_BU_DSP.Content = FindResource("dsp");
        }

        private void minimized_Click(object sender, RoutedEventArgs e)
        {


       
            var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(1));
            anim.Completed += (s, _) => this.Hide();
            this.BeginAnimation(UIElement.OpacityProperty, anim);
 
          
            this.notifyIcon.BalloonTipTitle = "SAO Music Player";
            this.notifyIcon.BalloonTipText = "已经隐藏到这里了哦♪";
            this.notifyIcon.Visible = true;
            this.notifyIcon.ShowBalloonTip(2000);

        }


        void notifyIcon_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ContextMenu menu = (ContextMenu)this.FindResource("NotifierContextMenu");
            menu.IsOpen = true;
        }
        private void switch_language(string lang)
        {
            switch (lang)
            {
                case "en": cul = CultureInfo.CreateSpecificCulture("en"); break;
            }

            title.Text = res_man.GetString("Main_Title",cul);
 

        }

        private void Try_showmain_Click(object sender, RoutedEventArgs e)
        {
            this.Show();
            var anim = new DoubleAnimation(1, (Duration)TimeSpan.FromSeconds(1));
    
            this.BeginAnimation(UIElement.OpacityProperty, anim);
            this.notifyIcon.Visible = false;
        }

        private void minimized_MouseMove(object sender, MouseEventArgs e)
        {
            minimized.Content = FindResource("Tray_hover");
        }

        private void minimized_MouseLeave(object sender, MouseEventArgs e)
        {
            minimized.Content = FindResource("Tray_normal");
            
        }

 
       

    

  

    }
}
