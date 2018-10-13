/* HTMLScoreboard
 * Copyright(C) 2017 Rick Tillery
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.If not, see<http://www.gnu.org/licenses/>.
 */

using System.Collections.Specialized;
using System.Windows;
using System.Drawing;
using System.IO;

namespace HTMLScoreboard
{
    public partial class MainWindow : Window
    {
        private NameValueCollection pairs = new NameValueCollection();

        private System.Windows.Media.Color GetBaseColor()
        {
            return (radioButtonTop.IsChecked == true) ?
                    ((radioButtonVisitorFillBkgnd.IsChecked == true) ? colorVisitorBkgnd : colorVisitorForegnd) :
                    ((radioButtonHomeFillBkgnd.IsChecked == true) ? colorHomeBkgnd : colorHomeForegnd);
        }

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
