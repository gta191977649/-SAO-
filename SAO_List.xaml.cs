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
using System.Collections;
using System.ComponentModel;
using System.Windows.Media.Animation;
using System.IO;

namespace SAP_WPF
{
    /// <summary>
    /// Interaction logic for SAO_List.xaml
    /// </summary>
    public partial class SAO_List : Window
    {

    
       
     
        MainWindow Main;
        readPlaylist readList = new readPlaylist();
        List<string> list = new List<string>();
       
        bool Searchmode;
        public int SelectedIndex;
        public int MaxListItems;
        public static StreamWriter sw;
        public SAO_List(MainWindow main)
        {

         
            Main =main;
            //Load Play list
            //Create OpenDialog control

            //Crete Controls
            InitializeComponent();
            // var main = new MainWindow();

            //自动加载播放列表
          /*
            if (main.PlayListPath == null)
            {
                LoadPlayList(main.PlayListPath);

            }
           */
            //LoadPlayList(main.PlayListPath);


        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Closing -= Window_Closing;
            e.Cancel = true;
            var anim = new DoubleAnimation(270, 0, TimeSpan.FromMilliseconds(500));
            var anim1 = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(500));
            anim.Completed += (s, _) => this.Close();
            this.BeginAnimation(FrameworkElement.WidthProperty,anim);
            this.BeginAnimation(UIElement.OpacityProperty, anim1);
         
           
        }
        public void HelloWorld()
        {

        }
        public void LoadPlayList(string filepath)
        {
            
            //Load Play list
            //Create OpenDialog control
           
            


            sao_playlist.Items.Clear();

             
            Main.PlayListPath = filepath;
            readList.playListPath = filepath;
           
            Main.nowPlaylist.AddRange(readList.PlayList);
            //臨時ERROR解決辦法
            /*
            if (nowPlaylist.Count == 0)
            {
                Main.ResetPlayListPath();
                Main.PlayListMode = false;
                
                this.Close();
            } 
             * */
            for (int x = 0; x < Main.nowPlaylist.Count; x++)
            {
                sao_playlist.Items.Add(System.IO.Path.GetFileNameWithoutExtension(Main.nowPlaylist[x].ToString()));
            }

            RefreashSearchList();

            SelectedIndex = 0;
            sao_playlist.SelectedIndex = SelectedIndex;
            MaxListItems = Main.nowPlaylist.Count;
            Title.Content = "Playlist" + sao_playlist.SelectedIndex + "/" + Main.nowPlaylist.Count;
        }
        private void sao_playlist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Searchmode == true)
            {
                //Handle while results is  get from search
                for (int x = 0; x < Main.nowPlaylist.Count; x++)
                {
                  if (System.IO.Path.GetFileNameWithoutExtension(Main.nowPlaylist[x].ToString()).Contains(sao_playlist.SelectedItem.ToString()))
                    {
                        PlayListStream(x);
                        break;
                    }
        

                }
            }
            else
            {
                PlayListStream(sao_playlist.SelectedIndex);
            }
            


        }
        public void PlayListStream(int index)
        {
            if (sao_playlist.Items.Count == 0) return;
           
            try
            {
                Main.nowPlaylist[index].ToString();
            }
            catch
            {
               // Main.ShowSAOMsgBox("列表Index值错误!");
                return;
            }
            
     
            
            SelectedIndex = index;
            Main.list_SelectedIndex = index;
            sao_playlist.SelectedIndex = index;
            Title.Content = "Playlist" + sao_playlist.SelectedIndex + "/" + Main.nowPlaylist.Count;
           
           // sao_playlist.SelectedItem = index;
            //Draw List Box

            /*
            file_info.Inlines.Clear();
            file_info.Inlines.Add(new Run(lyrics.PreviousLyrics));

            file_info.Inlines.Add(new Bold(new Run("\n" + lyrics.CurrentLyrics)) { Foreground = Lyrics_Color });
            file_info.Inlines.Add(new Run("\n" + lyrics.NextLyrics));
            */

            if (File.Exists(Main.nowPlaylist[index].ToString()))
            {
              
                file_info.Text = "Title:\n" + System.IO.Path.GetFileNameWithoutExtension(Main.nowPlaylist[index].ToString()) + "\n" + "File Path:\n" + Main.nowPlaylist[index].ToString();
                Main.PlayStream(Main.nowPlaylist[index].ToString(), 0);
                Main.PlayListMode = true;
            }
        }


        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window 
            this.DragMove();
        }

        private void sao_playlist_Drop(object sender, DragEventArgs e)
        {
          
        }

        private void SAO_WINLIST_Drop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (var file in s)
            {
                string extension = System.IO.Path.GetExtension(file);
                if (extension == ".wpl")
                {
                    LoadPlayList(file);
                    return;
                }
                Main.nowPlaylist.Add(file);
                sao_playlist.Items.Add(System.IO.Path.GetFileNameWithoutExtension(file));
                MaxListItems = Main.nowPlaylist.Count;
                
            }
           /*
            int i;
            for (i = 0; i < s.Length; i++)
                
                */
       //     Main.nowPlaylist.Add(s[i]);
        }



        private void RefreashSearchList()
        {
            list.Clear();
            foreach (String str in sao_playlist.Items)
            {
                list.Add(str);
            }
        }
        private void CreatePlayList(string savepath)
        {
            // Open a file to write
            string sFileName = savepath;
            string listAuthor = "SAO Player";
            string listTitle = "Default";
            FileStream fs = File.Create(sFileName);
            sw = new StreamWriter(fs,Encoding.UTF8);

            try
            {
                sw.WriteLine("<?wpl version=\"1.0\"?>");    // File Header
                sw.WriteLine("<smil>");                     // Start of File Tag

                sw.WriteLine("\t<head>");                     // Playlist File Header Information Start Tag
                sw.WriteLine("\t\t<meta name=\"Generator\" content=\"Microsoft Windows Media Player -- 10.0.0.4036\"/>");
                sw.WriteLine("\t\t<author>" + listAuthor + "</author>");
                sw.WriteLine("\t\t<title>" + listTitle + "</title>");
                sw.WriteLine("\t</head>");                    // Playlist File Header Information End Tag

                sw.WriteLine("\t<body>");                     // Start of body Tag
                sw.WriteLine("\t\t<seq>");                      // Start of filelist Tag


                // Get Directory's File list and Add files
                //DirectoryListing(txtDir.Text);
                for (int x = 0; x < Main.nowPlaylist.Count; x++)
                {
                    string fileLine = "\t\t\t<media src=\"";
                    fileLine = fileLine + Main.nowPlaylist[x] + "\"/>";
                    sw.WriteLine(fileLine);

                }
                

                sw.WriteLine("\t\t</seq>");                      // End of filelist Tag
                sw.WriteLine("\t</body>");                    // End of body Tag
                sw.WriteLine("</smil>");                    // End of File Tag

               // sFileName = sFileName + " Successfully created.";

                Main.ShowSAOMsgBox("列表創建成功了喲♪");

            }
            catch (Exception ex)
            {
                Main.ShowSAOMsgBox(ex.Message);
            }
            finally
            {
                sw.Close();
                fs.Close();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
            if (sao_playlist.Items.Count == 0)
            {
                Main.ShowSAOMsgBox("不能保存空列表哦~");
                return;
            }
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.press);
            player.Play();
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            dlg.Title = "Save Windows Playlist As...";
            dlg.Filter = "wpl files (*.wpl)|*.wpl|All files (*.*)|*.*";
            dlg.DefaultExt = "wpl";
            dlg.RestoreDirectory = true;
            Nullable<bool> result = dlg.ShowDialog();
    
            
            if (result == true)
            {
                CreatePlayList(dlg.FileName);
            }
        }

        private void menu_clean_Click(object sender, RoutedEventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.press);
            player.Play();
            Main.nowPlaylist.Clear();
            sao_playlist.Items.Clear();
            MaxListItems = Main.nowPlaylist.Count;
            RefreashSearchList();
        }
       
        private void sao_playlist_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.open);
            player.Play();
        }

        private void menu_search_Click(object sender, RoutedEventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.press);
            player.Play();
            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
            dlg.Description = "請選擇要掃描的檔案夾";
            dlg.ShowNewFolderButton = false;           // Hide 'Make New Folder' button
            System.Windows.Forms.DialogResult result = dlg.ShowDialog();

         //   DirectoryListing(dlg.SelectedPath);
          
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                DirectoryListing(dlg.SelectedPath);
                RefreashSearchList();
                //Auto Play Index 0 Music
                PlayListStream(0);
            }
           
        }
        private int DirectoryListing(string sPath)
        {
            int iFileCount = 0;

            if (string.IsNullOrEmpty(sPath) == true)
            {
                Main.ShowSAOMsgBox("Directory not specified. Please select Valid directory.");
            //    MessageBox.Show("Directory not specified. Please select Valid directory.");
                return iFileCount;
            }

            //            if (Directory.Exists(sPath) == false)
            //            {
            //                MessageBox.Show("Directory not exist. Please select Valid directory.");
            //                return iFileCount;
            //            }

            ArrayList searchList = new ArrayList();
            
            if (File.Exists(sPath))
            {
                // This path is a file
                iFileCount = ProcessFile(sPath, searchList);

                
            }
            else if (Directory.Exists(sPath))
            {
                // This path is a directory
                iFileCount = ProcessDirectory(sPath, searchList);
               
       
            }
            else
            {
               // MessageBox.Show(sPath + " is not a valid file or directory.");
                Main.ShowSAOMsgBox("非正常的檔案或檔案夾!");
            }
            
            return iFileCount;

        }
        public int ProcessFile(string fileName, ArrayList searchList)
        {
           
            string sFileExt;
            int iFileCount = 0;

            if (string.IsNullOrEmpty(fileName) == true)
                return iFileCount;

            if (searchList.Count != 0)                     // If it's not All files
            {
                sFileExt = fileName.Substring(fileName.IndexOf('.'));
                if (searchList.IndexOf(sFileExt) == -1)
                    return iFileCount;
            }
            //Added if the format is correct
            if (Main.CheckFileFormat(fileName))
            {
                Main.nowPlaylist.Add(fileName);
                sao_playlist.Items.Add(System.IO.Path.GetFileNameWithoutExtension(fileName));
                MaxListItems = Main.nowPlaylist.Count;
           
            }
 
           
            /*
             * 
            fileLine = "\t\t\t<media src=\"";
            fileLine = fileLine + fileName + "\"/>";
            sw.WriteLine(fileLine);
            */
            return (++iFileCount);
        }
        public int ProcessDirectory(string targetDirectory, ArrayList searchList)
        {
            int iFileCount = 0;

            if (string.IsNullOrEmpty(targetDirectory) == true)
                return iFileCount;

            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            if (fileEntries.Length > 0)
            {
                foreach (string fileInfo in fileEntries)
                iFileCount += ProcessFile(fileInfo, searchList);
                  
                    
            }
       
            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            if (subdirectoryEntries.Length > 0)
            {
                foreach (string subdirectory in subdirectoryEntries)
                    ProcessDirectory(subdirectory, searchList);
            }
            return iFileCount;
        }

        private void Search_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
           
    

        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
        
            if (!String.IsNullOrEmpty(Search.Text.Trim()))
            {

                sao_playlist.Items.Clear();
                foreach (string str in list)
                {
                    if (str.Contains(Search.Text.Trim()))
                    {
                        sao_playlist.Items.Add(str);
                        Searchmode = true;

                    }
                }
                if (sao_playlist.Items.Count == 0)
                {
                    sao_playlist.Items.Add("無結果 :(");
                    Searchmode = false;
                }
            }
            else
            {
                sao_playlist.Items.Clear();
                foreach (string str in list)
                {
                    sao_playlist.Items.Add(str);     
                }
                Searchmode = false;
            }                         
           
        }

        private void Search_MouseMove(object sender, MouseEventArgs e)
        {
          
            Search.Background = System.Windows.Media.Brushes.Orange;
            Search.Foreground = System.Windows.Media.Brushes.White;
          
        }

        private void Search_MouseLeave(object sender, MouseEventArgs e)
        {
            Search.Background = System.Windows.Media.Brushes.Transparent;
            Search.Foreground = System.Windows.Media.Brushes.Black;
        }

        private void SAO_WINLIST_Initialized(object sender, EventArgs e)
        {
          
               // Main.ShowSAOMsgBox(Main.PlayListPath);
                //LoadPlayList(Main.PlayListPath);
                
            

            for (int x = 0; x < Main.nowPlaylist.Count; x++)
            {
                sao_playlist.Items.Add(System.IO.Path.GetFileNameWithoutExtension(Main.nowPlaylist[x].ToString()));
                MaxListItems = Main.nowPlaylist.Count;
            }
            SelectedIndex = Main.list_SelectedIndex;
            sao_playlist.SelectedIndex = SelectedIndex;
            //Add into search list
            RefreashSearchList();


        }

  
       

   
      
    }
}
