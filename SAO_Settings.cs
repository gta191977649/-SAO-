using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Text;
using System.IO;
using System.Xml;

namespace SAP_WPF
{
    public partial class SAO_SETFORMS : Form
    {
        public static string Client_Ver = "v0.2.4";//目前客户端版本
        public string[] Servers = 
        {
            "http://java-sparrow.tk/SAO/update/version.xml", 
        };

        //Make a constructor 桥接父窗口組件
        MainWindow Main;
  
        PrivateFontCollection pfc = new PrivateFontCollection(); //using System.Drawing.Text 
        System.Drawing.Font SAO_Font;
     //   ToolTip toolTip1;
        public SAO_SETFORMS(MainWindow main)
        {
            InitializeComponent();
            Main = main;
            Application.EnableVisualStyles();
            this.Closing += new System.ComponentModel.CancelEventHandler(SAO_SETFORMS_Closing);
            label2.Text = Client_Ver + " Build";
            verlabel.Text = Client_Ver;
            ser_select.SelectedIndex = 0;

           // //Init Color

            
        }

        private void SAO_SETFORMS_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Main.IsEnabled = true;
        }
        private void SAO_SETFORMS_Load(object sender, EventArgs e)
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            onlinelyricURL.Text = QianQianOnlineLyrics.SearchPath;
            lyrics_pre.Select(8,10);
            lyrics_pre.SelectionColor = Color.Black;
            lyrics_pre.SelectionFont = new Font(lyrics_pre.Font, FontStyle.Bold);
            lyrics_pre.SelectAll();
            lyrics_pre.SelectionAlignment = HorizontalAlignment.Center;
            
                 
        


            PrivateFontCollection privateFonts = new PrivateFontCollection();
            try
            {
                string AppPath = Convert.ToString(AppDomain.CurrentDomain.BaseDirectory);
                privateFonts.AddFontFile(AppPath + @"\Font\EN.ttf");//加载字体
                SAO_Font = new Font(privateFonts.Families[0], 40);
            


            }
            catch
            {
                Main.ShowSAOMsgBox("字體文件丟失!\n請確認必要的ttf文件在Font文件夾內!");


            }
            //Loading Settings
            LoadSettings();
            //检查更新
            if (updatetoggle.Checked) GettingUpdateInfo();
        }

        public void LoadSettings (){
            //同步歌词颜色

      //      DestopLRC.ApplyLyricsColor();
            

            SPLineWith.Value = Main.Stream_width;
            SAO_DrawTimer.Value = Main.Draw_Timer;
            groupBox1.Text = "波形寬度: " + SPLineWith.Value + "px";
            groupBox2.Text = "渲染速度: " + SAO_DrawTimer.Value + "ms";
            Soundfontpath.Text = Main.SoundFontPath;
            if (Main.AutoCheckLyrics)
            {
                AutolyricDetectTick.Checked = true;
                AutoOnlineLyricDetectTick.Enabled = true;
            }
            else
            {
                AutolyricDetectTick.Checked = false;
                AutoOnlineLyricDetectTick.Enabled = false;
            }
            if (Main.AutoCheckOnlineLyrics)
            {
                AutoOnlineLyricDetectTick.Checked = true;
            }
            else
            {
                AutoOnlineLyricDetectTick.Checked = false;
            }
            if (Main.AlwaysTop)
            {
                alwaytop.Checked = true;
            }
            else
            {
                alwaytop.Checked = false;
            }
            if (Main.CheckUpdate)
            {
                updatetoggle.Checked = true;
            }
            else
            {
                updatetoggle.Checked = false;
            }
            

            lyrics_pre.Select(8, 10);
            lyrics_pre.SelectionColor = Color.FromArgb(Main.COLOR_R, Main.COLOR_G, Main.COLOR_B);
            lyrics_pre.DeselectAll();

            DestopLRC.ApplyLyricsColor(Color.FromArgb(Main.LRC_MCOLOR_R, Main.LRC_MCOLOR_G, Main.LRC_MCOLOR_B));
            DestopLRC.ApplyLyricsMColor(Color.FromArgb(Main.LRC_BCOLOR_R, Main.LRC_BCOLOR_G, Main.LRC_BCOLOR_B));

            tex_pre.ForeColor = Color.FromArgb(Main.LRC_MCOLOR_R, Main.LRC_MCOLOR_G, Main.LRC_MCOLOR_B);
            text_mask.ForeColor = Color.FromArgb(Main.LRC_BCOLOR_R, Main.LRC_BCOLOR_G, Main.LRC_BCOLOR_B);
          
            colorselectfore.BackColor = Color.FromArgb(Main.LRC_BCOLOR_R, Main.LRC_BCOLOR_G, Main.LRC_BCOLOR_B);
            colorselectback.BackColor = Color.FromArgb(Main.LRC_MCOLOR_R, Main.LRC_MCOLOR_G, Main.LRC_MCOLOR_B);
            
            
           // colorselectfore
            visualtext.Text = Main.Visual_Text;

        }
     
        private void button1_Click(object sender, EventArgs e)
        {
            ApplySettings();
           
        }

   

        private void SPLineWith_Scroll(object sender, EventArgs e)
        {
            //When Scroll is Scrolled event

            groupBox1.Text = "频谱宽度: " + SPLineWith.Value + "px";

        }

        private void SAO_DrawTimer_Scroll(object sender, EventArgs e)
        {

            groupBox2.Text = "描绘速度: " + SAO_DrawTimer.Value + "ms";

        }

  

        private void ResetSettings()
        {
            Main.Stream_width = 2;
            Main.Draw_Timer = 1;

        }

        private void changelog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Changelog changlogdia = new Changelog();
            changlogdia.ShowDialog();
        }

        private void SAO_BU_APPLY_Click(object sender, EventArgs e)
        {
            ApplySettings();
            Main.IsEnabled = true;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Main.IsEnabled = true;
            Close();
        }

        private void SAO_SET_RESET_Click(object sender, EventArgs e)
        {
            Main.LoadDefultSettings();
            Main.IsEnabled = true;
            Close();
        }
      

    
 
        private void soundfontbrose_Click(object sender, EventArgs e)
        {
           DialogResult result=soundfontselectdialog.ShowDialog();
           if (result == DialogResult.OK)
           {
               Main.SoundFontPath = soundfontselectdialog.FileName;
               Soundfontpath.Text = soundfontselectdialog.FileName;
           }
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://java-sparrow.tk/SAO/");
        }

        private void richTextBox1_Click(object sender, EventArgs e)
        {
            
            DialogResult result = fontDialog1.ShowDialog();
            try
            {
                if (result == DialogResult.OK)
                {
                    richTextBox1.SelectAll();
                    Font settingFont = new Font(fontDialog1.Font.FontFamily, 21);
                    richTextBox1.SelectionFont = settingFont;
                    richTextBox1.DeselectAll();
                    System.Windows.Media.FontFamily mfont = new System.Windows.Media.FontFamily(settingFont.Name);
                    Main.lyrics_box.FontFamily = mfont;
                }
            }
            catch
            {
                MessageBox.Show( "Font not changed.", "Font Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
          

        }

        private void richTextBox2_Click(object sender, EventArgs e)
        {
            DialogResult color_dialog = colorDialog1.ShowDialog();

            if (color_dialog == DialogResult.OK)
            {
                lyrics_pre.Select(8, 10);
                lyrics_pre.SelectionColor = colorDialog1.Color;
                //Update Settings
                Main.COLOR_R = Main.COLOR_R = lyrics_pre.SelectionColor.R;
                Main.COLOR_G = Main.COLOR_G = lyrics_pre.SelectionColor.G;
                Main.COLOR_B = Main.COLOR_B = lyrics_pre.SelectionColor.B;
                lyrics_pre.SelectionFont = new Font(lyrics_pre.Font, FontStyle.Bold);
                lyrics_pre.SelectAll();
                lyrics_pre.SelectionAlignment = HorizontalAlignment.Center;
                lyrics_pre.DeselectAll();
                Main.Lyrics_Color = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B));

                

             
            }
   
        }

        private void AutolyricDetectTick_Click(object sender, EventArgs e)
        {
            
            if (AutolyricDetectTick.Checked == true)
            {
                AutoOnlineLyricDetectTick.Enabled = true;
            }
            else
            {
                AutoOnlineLyricDetectTick.Enabled = false;
            }
        }

        public void ApplySettings()
        {
            Main.Stream_width = SPLineWith.Value;
            Main.Draw_Timer = SAO_DrawTimer.Value;
            //For Lyrics
            if (AutolyricDetectTick.Checked)
            {
                Main.AutoCheckLyrics = true;
            }
            else
            {
                Main.AutoCheckLyrics = false;
            }
            if (AutoOnlineLyricDetectTick.Checked)
            {
                Main.AutoCheckOnlineLyrics = true;
            }
            else
            {
                Main.AutoCheckOnlineLyrics = false;
            }
            //For Update
           if(updatetoggle.Checked)
           {
               Main.CheckUpdate = true;

           }
           else
           {
               Main.CheckUpdate = false;
           }


            if (visualtext.Text != string.Empty)
            {
                if (visualtext.Text.Length > 30)
                {
                    Main.ShowSAOMsgBox("文字長度不能超過30哦~");
                    return;
                }
                else
                {
                    Main.Visual_Text = visualtext.Text;
                }
            }
            else
            {
                Main.ShowSAOMsgBox("文字不能為空哦");
                return;
            }
            
           // IniParser SettingInI = new IniParser(@"C:\\test.ini");
            if (!File.Exists(Main.iniPath))
            {
                File.WriteAllText(@Main.iniPath, "");
            }
            

            IniParser SettingInI = new IniParser(@Main.iniPath);

            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "Stream_width", Main.Stream_width.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "Draw_Timer", Main.Draw_Timer.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "AutolyricDetectTick", AutolyricDetectTick.Checked.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "AutoOnlineLyricDetectTick", AutoOnlineLyricDetectTick.Checked.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "AlwaysTop",alwaytop.Checked.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "CheckUpdate", updatetoggle.Checked.ToString());
            
            //Save Color
            lyrics_pre.Select(8, 10);
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "Lyrics_Color_R", lyrics_pre.SelectionColor.R.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "Lyrics_Color_G", lyrics_pre.SelectionColor.G.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "Lyrics_Color_B", lyrics_pre.SelectionColor.B.ToString());
            //Back Color
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "LYRICS_BCOLOR_R", colorselectfore.BackColor.R.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "LYRICS_BCOLOR_G", colorselectfore.BackColor.G.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "LYRICS_BCOLOR_B", colorselectfore.BackColor.B.ToString());
            //Mask Color

            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "LYRICS_MCOLOR_R", colorselectback.BackColor.R.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "LYRICS_MCOLOR_G", colorselectback.BackColor.G.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "LYRICS_MCOLOR_B", colorselectback.BackColor.B.ToString());


            lyrics_pre.DeselectAll();

            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "Lyrics_Font", Main.lyrics_box.FontFamily.ToString());
            SettingInI.AddSetting("SAO_PlAYER_SOUNDFONT", "SoundFontPath", Main.SoundFontPath);
            SettingInI.AddSetting("SAO_PlAYER_SETTINGS", "VISUAL_TEXT", Main.Visual_Text);
            //播放列表处理
            SettingInI.AddSetting("SAO_PLAYER_PLAYLIST", "PLAYLISTPATH", Main.PlayListPath);

            SettingInI.SaveSettings();
            //Update Local Var
            Main.LRC_BCOLOR_R = colorselectfore.BackColor.R;
            Main.LRC_BCOLOR_G = colorselectfore.BackColor.G;
            Main.LRC_BCOLOR_B = colorselectfore.BackColor.B;

            Main.LRC_MCOLOR_R = colorselectback.BackColor.R;
            Main.LRC_MCOLOR_G = colorselectback.BackColor.G;
            Main.LRC_MCOLOR_B = colorselectback.BackColor.B;
        }
        

        private void AutolyricDetectTick_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.Show("自動關聯歌詞\n開啟后，系統會自動檢索是否有和歌曲文件名一樣的歌詞文件存在,如果有則自動加載它。\n注:如果此功能導致程式崩潰，請禁用它！", AutolyricDetectTick,5000); 
        }

        private void AutoOnlineLyricDetectTick_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.Show("自動線上歌詞檢測\n開啟后，系統自動將彈出線上歌詞檢索窗口，幫助你查找到適合的歌詞。", AutoOnlineLyricDetectTick, 5000); 
        }

        private void AutoOnlineLyricDetectTick_CheckedChanged(object sender, EventArgs e)
        {
        
        }

        private void AutolyricDetectTick_CheckedChanged(object sender, EventArgs e)
        {
          
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Main.IsEnabled = true;
            this.Close();
        }

        private void lyrics_pre_TextChanged(object sender, EventArgs e)
        {

        }

        private void alwaytop_CheckedChanged(object sender, EventArgs e)
        {
            if (alwaytop.Checked)
            {
                Main.AlwaysTop = true;
            }
            else
            {
                Main.AlwaysTop = false;
            }
            //Main.ShowSAOMsgBox("設置成功,將在下次啟動生效!");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            toolTip1.IsBalloon = true;
           
            toolTip1.SetToolTip(sender as Control, "捂脸~\n虽然是免费软件，但是人家也是花了好多功夫才做画出来呢 T.T\n如果你喜欢本软件,请帮组作者:)\n哪怕多少，也是对作者的一种鼓励~谢谢！");
        }

        private void groupBox11_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            GettingUpdateInfo();
        }
        public void GettingUpdateInfo()
        {
            XmlDocument xml = new XmlDocument();//声明xml
            XmlDeclaration xmlDecl = xml.CreateXmlDeclaration("1.0", "UTF-8", null);
            xml.Load(Servers[ser_select.SelectedIndex]);//按路径读xml文件

            XmlNode root = xml.SelectSingleNode("ver");
            XmlNode xn = root.SelectSingleNode("lastst");//指向根节点下的serv_ip节点
            XmlNode d = root.SelectSingleNode("des");
            
            label_newver.Text= xn.InnerText;
            desbox.Text = d.InnerText;
       
       
          
        }
        public  void updatecheck()
        {
            XmlDocument xml = new XmlDocument();//声明xml
            XmlDeclaration xmlDecl = xml.CreateXmlDeclaration("1.0", "UTF-8", null);

            xml.Load(Servers[ser_select.SelectedIndex]);//按路径读xml文件

            XmlNode root = xml.SelectSingleNode("ver");
            XmlNode xn = root.SelectSingleNode("lastst");//指向根节点下的serv_ip节点
            XmlNode d = root.SelectSingleNode("des");
            string version = xn.InnerText;
            string des = d.InnerText;
            label_newver.Text = version;
            desbox.Text = des;
            if (version != Client_Ver) Main.ShowSAOMsgBox("最新版本:\n" + version + "\n发现新版本,请去设置里查看详细.");
        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void colorselectfore_Click(object sender, EventArgs e)
        {
            DialogResult color_dialog = colorDialog1.ShowDialog();

            if (color_dialog == DialogResult.OK)
            {

                colorselectfore.BackColor = colorDialog1.Color;
                text_mask.ForeColor = colorDialog1.Color;
                var brush = new SolidBrush(Color.FromArgb(colorselectfore.BackColor.ToArgb()));
                DestopLRC.ApplyLyricsMColor(colorDialog1.Color);



            }
        }

        private void colorselectfore_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            DialogResult color_dialog = colorDialog1.ShowDialog();

            if (color_dialog == DialogResult.OK)
            {

                colorselectback.BackColor = colorDialog1.Color;
                tex_pre.ForeColor = colorDialog1.Color;
                var brush = new SolidBrush(Color.FromArgb(colorselectback.BackColor.ToArgb()));
                DestopLRC.ApplyLyricsColor(colorDialog1.Color);
                


            }
        }
    


 

  
    }
}
