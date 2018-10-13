using System;
using System.Windows;
using System.Windows.Media;

namespace HTMLScoreboard
{
    public partial class MainWindow : Window
    {
        private static string ColorToHTML(System.Windows.Media.Color color)
        {
            return "rgba(" + color.R + "," + color.G + "," + color.B + "," + color.A / 255.0 + ")";
        }

        public static Color HTMLtoColor(string html)
        {
            var left = html.IndexOf('(');
            var right = html.IndexOf(')');
            string[] comps = html.Substring(left + 1, right - left - 1).Split(',');
            var a = (byte)(float.Parse(comps[3]) * 255);
            var r = byte.Parse(comps[0]);
            var g = byte.Parse(comps[1]);
            var b = byte.Parse(comps[2]);
            return Color.FromArgb(a, r, g, b);
        }

        private static int Increment(String number, int limit = 0)
        {
            int value = Convert.ToInt32(number);
            if ((limit == 0) || (value < limit))
                value++;
            return value;
        }

        private static int Decrement(String number, int limit = 0)
        {
            int value = Convert.ToInt32(number);
            if (value > limit)
                --value;
            return value;
        }

        private static System.Drawing.Color SWMColorToSDColor(System.Windows.Media.Color swmc)
        {
            return System.Drawing.Color.FromArgb(swmc.A,
                                                 swmc.R,
                                                 swmc.G,
                                                 swmc.B);
        }

        private static System.Windows.Media.Color SDColorToSWMColor(System.Drawing.Color sdc)
        {
            return System.Windows.Media.Color.FromArgb(sdc.A,
                                                       sdc.R,
                                                       sdc.G,
                                                       sdc.B);
        }
    }
}