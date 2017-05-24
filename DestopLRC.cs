using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SAP_WPF
{
    class DestopLRC
    {
        public static Canvas LRC_CANVAS;
        public static Canvas LRC_MASK;
        public static TextBlock LRC_TextBoxHandle;
        public static TextBlock LRC_TextBoxMaskHandle;

        private static DoubleAnimation deskLyricBrushAni = new DoubleAnimation();

        public static void InitLRC(Canvas LrcCanvas, Canvas LrcMask,TextBlock LrcTextBoxHandle,TextBlock LrcTextBoxMaskHandle)
        {
            DestopLRC.LRC_CANVAS = LrcCanvas;
            DestopLRC.LRC_MASK = LrcMask;
            DestopLRC.LRC_TextBoxHandle = LrcTextBoxHandle;
            DestopLRC.LRC_TextBoxMaskHandle = LrcTextBoxMaskHandle;
             
        }

        public static void ApplyLyricsColor(System.Drawing.Color color)
        {
            //先转Media Color 然后 --> Media Brush
            var drawingcolor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
            Color newColor = drawingcolor;
            Brush imageColor = new SolidColorBrush(newColor);
             // Console.WriteLine("Processed ApplyLyricsColor()");
            DestopLRC.LRC_TextBoxMaskHandle.Foreground = imageColor;
        }

        public static void ApplyLyricsMColor(System.Drawing.Color color)
        {
            //先转Media Color 然后 --> Media Brush
            var drawingcolor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
            Color newColor = drawingcolor;
            Brush imageColor = new SolidColorBrush(newColor);
            // Console.WriteLine("Processed ApplyLyricsColor()");
            DestopLRC.LRC_TextBoxHandle.Foreground = imageColor;

        }
        public static void ResetLRC()
        {
            DestopLRC.LRC_TextBoxHandle.Text = "NO Lyric";

        }
        private static String oldlrc="";
        public static void UpdateLyrics(String Lyrics,String currenttime, String nexttime)
        {
            
            
            DestopLRC.LRC_TextBoxHandle.Text = Lyrics;
            if (Lyrics ==null) return;
            if (!oldlrc.Equals(Lyrics))
            {

                TimeSpan intervalVal;
                TimeSpan.TryParse("00:" + currenttime, out intervalVal);

                TimeSpan start = intervalVal;

                TimeSpan.TryParse("00:"+nexttime, out intervalVal);
                TimeSpan end = intervalVal;
                

                //TimeSpan TimeDifference = currenttime - nexttime;

                /*
                DateTime start = DateTime.ParseExact(start, "HH:mm:ss",
                                        CultureInfo.InvariantCulture);
                DateTime end = DateTime.Parse(nexttime);
                */
                deskLyricBrushAni.From = 0;
                deskLyricBrushAni.To = 1298;
             //   deskLyricBrushAni.Duration = new Duration(TimeSpan.Parse("0:0:2"));
                //     Math.Abs(end - start);


                deskLyricBrushAni.Duration = new Duration(end - start);
              //  Console.WriteLine(nexttime);
                Console.WriteLine(end-start);
                LRC_MASK.BeginAnimation(Canvas.WidthProperty, deskLyricBrushAni);
            }
            oldlrc = Lyrics;
        }





    }
}