using System.Collections.Specialized;
using System.Windows;
using System.Drawing;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private void UpdateHTML()
        {
            NameValueCollection pairs = new NameValueCollection()
            {
                { "HOMESHORT", this.textBoxHomeShort.Text },
                { "VISITORSHORT", this.textBoxVisitorShort.Text },
                { "HOMEBKGND", colorHomeBkgnd.ToString() },
                { "HOMEFOREGND", colorHomeForegnd.ToString() },
                { "VISITORBKGND", colorVisitorBkgnd.ToString() },
                { "VISITORFOREGND", colorVisitorForegnd.ToString() },
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
