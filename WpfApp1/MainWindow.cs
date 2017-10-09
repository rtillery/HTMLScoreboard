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

        private void UpdateHTML()
        {
            NameValueCollection pairs = new NameValueCollection()
            {
                { "HOMESHORT", this.textBoxHomeShort.Text },
                { "VISITORSHORT", this.textBoxVisitorShort.Text },
                { "HOMEBKGND", ColorToHTML(colorHomeBkgnd) },
                { "HOMEFOREGND", ColorToHTML(colorHomeForegnd) },
                { "VISITORBKGND", ColorToHTML(colorVisitorBkgnd) },
                { "VISITORFOREGND", ColorToHTML(colorVisitorForegnd) },
                { "VISITORSCORE", (string)this.labelBoxVisitorTotalRuns.Content },
                { "HOMESCORE", (string)this.labelBoxHomeTotalRuns.Content },
                { "BALLS", (string)this.labelCurrentBalls.Content },
                { "STRIKES", (string)this.labelCurrentStrikes.Content },
                { "OUTS", (string)this.labelCurrentOuts.Content },
                { "TOPBOTINNING", this.radioButtonTop.IsChecked == true ? "Top" : "Bot" }, /* TODO - Generic */
                { "INNING", (string)this.labelCurrentInning.Content }
            };
            HTMLTemplate.Fill("C:\\Projects\\WindowsFormsApp1\\WindowsFormsApp1\\bug1-template.html",
                              "C:\\Projects\\WindowsFormsApp1\\WindowsFormsApp1\\bug1.html",
                              pairs);
        }
    }
}
