using System.Collections.Specialized;
using System.Windows;
using System.Drawing;

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
                { "TOPBOTINNING", radioButtonTop.IsChecked == true ? "Top" : "Bot" }, /* TODO - Generic */
                { "INNING", (string)labelCurrentInning.Content },
                { "ATBATCOLOR", ColorToHTML(GetBaseColor()) }
            };
            HTMLTemplate.Fill("C:\\Projects\\WindowsFormsApp1\\WindowsFormsApp1\\bug1-template.html",
                              "C:\\Projects\\WindowsFormsApp1\\WindowsFormsApp1\\bug1.html",
                              pairs);
        }
    }
}
