using System;
using System.Collections;
using System.Text;
using System.Xml;
using System.Windows;
namespace SAP_WPF
{
    class readPlaylist
    {
        //private string m_xmlFile = "C:\\Documents and Settings\\loneferret\\My Documents\\My Music\\My Playlists\\New Playlist.wpl";
        private ArrayList name = new ArrayList();
        private string m_xmlFile;
        MainWindow MainHandle;
 
  
        /// <summary>
        /// The Windows Media Playlist Path xxx.wpl file
        /// </summary>
        public string playListPath
        {
            get
            {
                return m_xmlFile;
            }
            set
            {
                m_xmlFile = value;
                Makeplaylist();
            }
        }
    
        /// <summary>
        /// Return an Arraylist of file found in Windows Media Playlist file
        /// </summary>
        public ArrayList PlayList
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Fills up an Arraylist with titles found in the Windows Media Playlist file.
        /// Using XmlTextReader
        /// </summary>
        private bool Makeplaylist()
        {

           
               // readList = null;
            MainHandle = new MainWindow();
           try{
            //   m_xmlFile = new System.IO.StreamReader(pathToXML, System.Text.Encoding.GetEncoding("Windows-1252"), true);
               XmlTextReader readList = new XmlTextReader(m_xmlFile);
              // XmlDeclaration xmlDecl = readList.CreateXmlDeclaration("1.0", "UTF-8", null);

               while (readList.Read())
               {
                   if (readList.NodeType == XmlNodeType.Element)
                   {
                       if (readList.LocalName.Equals("media"))
                       {
                           name.Add(readList.GetAttribute(0).ToString());
                           

                       }
                       
                   }
               }
              
             
           } 
            catch (Exception e)
            {
                
                MainHandle.ResetPlayListPath();
                MainHandle.ShowSAOMsgBox(e.Message);
        
               return false;
             }

           return true;
            
        }
       
    }
}
