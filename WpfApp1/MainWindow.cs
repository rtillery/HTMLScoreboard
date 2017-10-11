using System.Collections.Specialized;
using System.Windows;
using System.Drawing;
using System.IO;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private string ColorToHTML(System.Windows.Media.Color color)
        {
            return "rgba(" + color.R + "," + color.G + "," + color.B + "," + color.A / 255.0 + ")";
        }

        private System.Windows.Media.Color GetBaseColor()
        {
            return (radioButtonTop.IsChecked == true) ?
                    ((radioButtonVisitorFillBkgnd.IsChecked == true) ? colorVisitorBkgnd : colorVisitorForegnd) :
                    ((radioButtonHomeFillBkgnd.IsChecked == true) ? colorHomeBkgnd : colorHomeForegnd);
        }

        private void UpdateHTML()
        {
            NameValueCollection pairs = new NameValueCollection()
            {
                { "HOMESHORT", textBoxHomeShort.Text },
                { "VISITORSHORT", textBoxVisitorShort.Text },
                { "HOMEBKGND", ColorToHTML(colorHomeBkgnd) },
                { "HOMEFOREGND", ColorToHTML(colorHomeForegnd) },
                { "VISITORBKGND", ColorToHTML(colorVisitorBkgnd) },
                { "VISITORFOREGND", ColorToHTML(colorVisitorForegnd) },
                { "VISITORSCORE", (string)labelBoxVisitorTotalRuns.Content },
                { "HOMESCORE", (string)labelBoxHomeTotalRuns.Content },
                { "BALLS", (string)labelCurrentBalls.Content },
                { "STRIKES", (string)labelCurrentStrikes.Content },
                { "OUTS", (string)labelCurrentOuts.Content },
                { "TOPVISIBLE", radioButtonTop.IsChecked == true ? "visible" : "hidden" },
                { "BOTTOMVISIBLE", radioButtonTop.IsChecked == true ? "hidden" : "visible" },
                { "TOPDISPLAY", radioButtonTop.IsChecked == true ? "inherit" : "none" },
                { "BOTTOMDISPLAY", radioButtonTop.IsChecked == true ? "none" : "inherit" },
                { "INNING", (string)labelCurrentInning.Content },
                { "ATBATCOLOR", ColorToHTML(GetBaseColor()) },
                { "1STBASEVISIBILITY", toggle1stBase.IsChecked == true ? "visible" : "hidden" },
                { "2NDBASEVISIBILITY", toggle2ndBase.IsChecked == true ? "visible" : "hidden" },
                { "3RDBASEVISIBILITY", toggle3rdBase.IsChecked == true ? "visible" : "hidden" },
            };
            foreach(var item in listboxTemplates.Items)
            {
                var template = item.ToString();
                var filename = Path.GetFileName(template);
                filename = filename.Replace("-template", "");
                var output = buttonDestText.Text + @"\" + filename;
                HTMLTemplate.Fill(template,
                                  output,
                                  pairs);
            }
        }
    }
}
