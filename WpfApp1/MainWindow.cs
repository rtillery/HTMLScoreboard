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

        private NameValueCollection pairs = new NameValueCollection();

        private void InitHTMLMapping()
        {
            pairs["HOMELONG"] = textBoxHomeLong.Text;
            pairs["HOMESHORT"] = textBoxHomeShort.Text;
            pairs["HOMEBKGND"] = ColorToHTML(colorHomeBkgnd);
            pairs["HOMEFOREGND"] = ColorToHTML(colorHomeForegnd);
            pairs["HOMEHITS"] = (string)labelBoxHomeTotalHits.Content;
            pairs["HOMEERRORS"] = (string)labelBoxHomeTotalErrors.Content;
            pairs["HOMESCORE"] = (string)labelBoxHomeTotalRuns.Content;

            pairs["VISITORLONG"] = textBoxVisitorLong.Text;
            pairs["VISITORSHORT"] = textBoxVisitorShort.Text;
            pairs["VISITORBKGND"] = ColorToHTML(colorVisitorBkgnd);
            pairs["VISITORFOREGND"] = ColorToHTML(colorVisitorForegnd);
            pairs["VISITORHITS"] = (string)labelBoxVisitorTotalHits.Content;
            pairs["VISITORERRORS"] = (string)labelBoxVisitorTotalErrors.Content;
            pairs["VISITORSCORE"] = (string)labelBoxVisitorTotalRuns.Content;

            pairs["INNING"] = (string)labelCurrentInning.Content;
            pairs["TOPVISIBLE"] = radioButtonTop.IsChecked == true ? "visible" : "hidden";
            pairs["TOPDISPLAY"] = radioButtonTop.IsChecked == true ? "inherit" : "none";
            pairs["BOTTOMVISIBLE"] = radioButtonTop.IsChecked == true ? "hidden" : "visible";
            pairs["BOTTOMDISPLAY"] = radioButtonTop.IsChecked == true ? "none" : "inherit";
            pairs["ATBATCOLOR"] = ColorToHTML(GetBaseColor());

            pairs["BALLS"] = (string)labelCurrentBalls.Content;
            pairs["STRIKES"] = (string)labelCurrentStrikes.Content;
            pairs["OUTS"] = (string)labelCurrentOuts.Content;

            pairs["1STBASEVISIBILITY"] = toggle1stBase.IsChecked == true ? "visible" : "hidden";
            pairs["2NDBASEVISIBILITY"] = toggle2ndBase.IsChecked == true ? "visible" : "hidden";
            pairs["3RDBASEVISIBILITY"] = toggle3rdBase.IsChecked == true ? "visible" : "hidden";

            pairs["MAXINNINGS"] = MAXINNING.ToString();

            int inning = int.Parse((string)labelCurrentInning.Content);
            for (int i = 0; i < MAXINNING; i++)
            {
                pairs["HOME" + i] = (string)labelBoxInningHomeRuns[i].Content;
                pairs["VISITOR" + i] = (string)labelBoxInningVisitorRuns[i].Content;
                pairs["INNING" + i + "VISIBILITY"] = inning > i ? "visible" : "hidden";
                pairs["INNING" + i + "DISPLAY"] = inning > i ? "inherit" : "none";
            }
        }

        private void UpdateHTML()
        {
            InitHTMLMapping();

            foreach (var item in listboxTemplates.Items)
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
