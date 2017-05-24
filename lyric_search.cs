using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Xml;
using System.IO;

namespace SAP_WPF
{
    public partial class lyric_search : Form
    {
        MainWindow Main;
        QianQianOnlineLyrics q = null;
        XmlNodeList list;
        DataTable dt;
        string singer;
        string title;
        string RecievedLyrics;
        XmlNode current_song;
        public lyric_search(MainWindow main)
        {
            InitializeComponent();
            q = new QianQianOnlineLyrics(false);
            //  q.InitializeProxy += new EventHandler(q_InitializeProxy);
            q.WebException += new EventHandler(q_WebException);
            q.SelectSong += new EventHandler(q_SelectSong);
            Application.EnableVisualStyles();
            Main = main;
        }
        void q_SelectSong(object sender, EventArgs e)
        {
            
            list = sender as XmlNodeList;
            if (list != null)
            {
                dt = new DataTable();
                dt.Columns.Add("编号");
                dt.Columns.Add("歌手");
                dt.Columns.Add("歌名");
                foreach (XmlNode node in list)
                {
                    DataRow row = dt.NewRow();
                    row[0] = node.Attributes["id"].Value;
                    row[1] = node.Attributes["artist"].Value;
                    row[2] = node.Attributes["title"].Value;
                    dt.Rows.Add(row);
                }

                this.dataGridView1.DataSource = dt.DefaultView;
                q.CurrentSong = current_song;
                /*
                dataGridView1.RowHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(delegate(object _sender, DataGridViewCellMouseEventArgs _e)
                {
                    int index = _e.RowIndex;
                    if (index >= 0 & index < list.Count)
                    {
                        q.CurrentSong = list[_e.RowIndex];

                     
                        
                        
                        RecievedLyrics += q.DownloadLrc(singer, title);
                        richTextBox1.Text = RecievedLyrics;
                         
                        //MessageBox.Show("Test");
                    }
                    
                });
        */
                
            }
          //  MessageBox.Show("Test");
       //     richTextBox1.Text = list.ToString();

        }
  

        void q_WebException(object sender, EventArgs e)
        {
            WebException ex = sender as WebException;
            MessageBox.Show(ex.Message);
        }
 

        private void lyric_search_Load(object sender, EventArgs e)
        {
            lyricTitle.Text = Main.Song_Name;
            lyricAretest.Text = Main.sSinger;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            int index = e.RowIndex;
      //      MessageBox.Show(index.ToString());
            if (index >= 0 & index < list.Count)
            {
                q.CurrentSong = list[e.RowIndex];
                current_song = list[e.RowIndex];
      
            }
            RecievedLyrics = q.DownloadLrc(singer, title);
            richTextBox1.Text = RecievedLyrics;
          
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            singer = lyricAretest.Text;
            title = lyricTitle.Text;
            RecievedLyrics = "";
           
            q.DownloadLrc(singer, title);
        //    RecievedLyrics = q.DownloadLrc(singer, title);
        //    richTextBox1.Text = RecievedLyrics;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string LyricWritePath = System.IO.Path.GetDirectoryName(Main.StreamFilePath) + @"\" + System.IO.Path.GetFileNameWithoutExtension(Main.StreamFilePath) + ".lrc";
            File.WriteAllText(@LyricWritePath, RecievedLyrics, Encoding.Default);
            Main.CheckLyricsExists(Main.StreamFilePath);
            this.Close();

        }

       
    }
    
}
