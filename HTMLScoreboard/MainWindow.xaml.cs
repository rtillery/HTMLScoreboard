﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace HTMLScoreboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int MAXINNING = 13;
        private Label[] labelBoxInningVisitorRuns = new Label[MAXINNING];
        private Label[] labelBoxInningHomeRuns = new Label[MAXINNING];
        private int[] homeInningHits = new int[MAXINNING];
        private int[] visitorInningHits = new int[MAXINNING];
        private int[] homeInningErrors = new int[MAXINNING];
        private int[] visitorInningErrors = new int[MAXINNING];

        private string initialFileDir;

        private Color colorHomeBkgnd;
        private Color colorHomeForegnd;
        private Color colorVisitorBkgnd;
        private Color colorVisitorForegnd;

        public MainWindow()
        {
            InitializeComponent();

            LoadSettings();

            // Collect references to the box inning labels into arrays for easy use below
            for (int i = 0; i < gridBox.Children.Count; i++)
            {
                UIElement element = gridBox.Children[i];
                int row = Grid.GetRow(element);
                if (row > 0)
                {
                    int column = Grid.GetColumn(element);
                    if ((column > 0) && (column <= MAXINNING))
                    {
                        if (row == 1)
                            labelBoxInningVisitorRuns[column - 1] = (Label)element;
                        else
                            labelBoxInningHomeRuns[column - 1] = (Label)element;
                    }
                }
            }

            // Clear per-inning hits and errors
            for (int i = 0; i < MAXINNING; i++)
            {
                homeInningHits[i] =
                visitorInningHits[i] =
                homeInningErrors[i] =
                visitorInningErrors[i] = 0;
            }
        }

        private void textboxHomeLong_LostFocus(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.HomeLong = textBoxHomeLong.Text;
        }

        private void textboxHomeShort_LostFocus(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.HomeShort = textBoxHomeShort.Text;
        }

        private void textboxVisitorLong_LostFocus(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.VisitorLong = textBoxVisitorLong.Text;
        }

        private void textboxVisitorShort_LostFocus(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.VisitorShort = textBoxVisitorShort.Text;
        }

        public void LoadSettings()
        {
            //            if (System.Diagnostics.Debugger.IsAttached)
            //                Properties.Settings.Default.Reset();

            string localDir = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location);

            initialFileDir = Properties.Settings.Default.LastTemplatePath;
            if (initialFileDir == "default")
                initialFileDir = System.IO.Path.Combine(localDir, "templates");

            buttonDestText.Text = Properties.Settings.Default.HTMLPath;
            if (buttonDestText.Text == "default")
                buttonDestText.Text = System.IO.Path.Combine(localDir, "scoreboards");

            colorHomeBkgnd = HTMLtoColor(Properties.Settings.Default.HomeBkgnd);
            colorHomeForegnd = HTMLtoColor(Properties.Settings.Default.HomeForegnd);
            colorVisitorBkgnd = HTMLtoColor(Properties.Settings.Default.VisitorBkgnd);
            colorVisitorForegnd = HTMLtoColor(Properties.Settings.Default.VisitorForegnd);
            buttonHomeBkgndColor.Background = new SolidColorBrush(colorHomeBkgnd);
            buttonHomeForegndColor.Background = new SolidColorBrush(colorHomeForegnd);
            buttonVisitorBkgndColor.Background = new SolidColorBrush(colorVisitorBkgnd);
            buttonVisitorForegndColor.Background = new SolidColorBrush(colorVisitorForegnd);

            textBoxHomeLong.Text = Properties.Settings.Default.HomeLong;
            textBoxHomeShort.Text = Properties.Settings.Default.HomeShort;
            textBoxVisitorLong.Text = Properties.Settings.Default.VisitorLong;
            textBoxVisitorShort.Text = Properties.Settings.Default.VisitorShort;

            listboxTemplates.Items.Clear();
            foreach (object item in Properties.Settings.Default.TemplateList)
                listboxTemplates.Items.Add(item);
        }

        private void RestoreCurrentInningRunsHitsErrors()
        {
            RestoreCurrentInningRuns();
            RestoreCurrentInningHits();
            RestoreCurrentInningErrors();
        }

        private void buttonInningUp_Click(object sender, RoutedEventArgs e)
        {
            labelCurrentInning.Content = Increment((String)labelCurrentInning.Content, MAXINNING).ToString();
            RestoreCurrentInningRunsHitsErrors();
        }

        private void buttonInningDown_Click(object sender, RoutedEventArgs e)
        {
            labelCurrentInning.Content = Decrement((String)labelCurrentInning.Content, 1).ToString();
            RestoreCurrentInningRunsHitsErrors();
        }

        private void radioButtonTop_Click(object sender, RoutedEventArgs e)
        {
            RestoreCurrentInningRunsHitsErrors();
        }

        private void radioButtonBottom_Click(object sender, RoutedEventArgs e)
        {
            RestoreCurrentInningRunsHitsErrors();
        }

        private void buttonBallsUp_Click(object sender, RoutedEventArgs e)
        {
            labelCurrentBalls.Content = Increment((String)labelCurrentBalls.Content, 3).ToString();
        }

        private void buttonBallsDown_Click(object sender, RoutedEventArgs e)
        {
            labelCurrentBalls.Content = Decrement((String)labelCurrentBalls.Content).ToString();
        }

        private void buttonStrikesUp_Click(object sender, RoutedEventArgs e)
        {
            labelCurrentStrikes.Content = Increment((String)labelCurrentStrikes.Content, 2).ToString();
        }

        private void buttonStrikesDown_Click(object sender, RoutedEventArgs e)
        {
            labelCurrentStrikes.Content = Decrement((String)labelCurrentStrikes.Content).ToString();
        }

        private void buttonOutsUp_Click(object sender, RoutedEventArgs e)
        {
            labelCurrentOuts.Content = Increment((String)labelCurrentOuts.Content, 2).ToString();
        }

        private void buttonOutsDown_Click(object sender, RoutedEventArgs e)
        {
            labelCurrentOuts.Content = Decrement((String)labelCurrentOuts.Content).ToString();
        }

        private void RestoreCurrentInningRuns()
        {
            int inning = int.Parse((string)labelCurrentInning.Content);
            string runs = (string)((radioButtonTop.IsChecked == true) ?
                                    labelBoxInningVisitorRuns[inning - 1].Content :
                                    labelBoxInningHomeRuns[inning - 1].Content);
            labelCurrentInningRuns.Content = runs;
        }

        private void buttonRunsUp_Click(object sender, RoutedEventArgs e)
        {
            labelCurrentInningRuns.Content = (Convert.ToInt32((string)labelCurrentInningRuns.Content) + 1).ToString();
            int inning = int.Parse((string)labelCurrentInning.Content);
            if (radioButtonTop.IsChecked == true)
            {
                labelBoxInningVisitorRuns[inning - 1].Content = Increment((string)labelBoxInningVisitorRuns[inning - 1].Content).ToString();
                labelBoxVisitorTotalRuns.Content = Increment((string)labelBoxVisitorTotalRuns.Content).ToString();
            }
            else
            {
                labelBoxInningHomeRuns[inning - 1].Content = Increment((string)labelBoxInningHomeRuns[inning - 1].Content).ToString();
                labelBoxHomeTotalRuns.Content = Increment((string)labelBoxHomeTotalRuns.Content).ToString();
            }
        }

        private void buttonRunsDown_Click(object sender, RoutedEventArgs e)
        {
            int runs = Convert.ToInt32((string)labelCurrentInningRuns.Content);
            if (runs > 0)
            {
                --runs;
                labelCurrentInningRuns.Content = runs.ToString();
                int inning = int.Parse((string)labelCurrentInning.Content);
                if (radioButtonTop.IsChecked == true)
                {
                    labelBoxInningVisitorRuns[inning - 1].Content = Decrement((string)labelBoxInningVisitorRuns[inning - 1].Content).ToString();
                    labelBoxVisitorTotalRuns.Content = Decrement((string)labelBoxVisitorTotalRuns.Content).ToString();
                }
                else
                {
                    labelBoxInningHomeRuns[inning - 1].Content = Decrement((string)labelBoxInningHomeRuns[inning - 1].Content).ToString();
                    labelBoxHomeTotalRuns.Content = Decrement((string)labelBoxHomeTotalRuns.Content).ToString();
                }
            }
        }

        private void RestoreCurrentInningHits()
        {
            int inning = int.Parse((string)labelCurrentInning.Content);
            int hits = ((radioButtonTop.IsChecked == true) ?
                        visitorInningHits[inning - 1] :
                        homeInningHits[inning - 1]);
            labelCurrentInningHits.Content = hits.ToString();
        }

        private void UpdateCurrentInningHits(int hits)
        {
            int inning = int.Parse((string)labelCurrentInning.Content);
            labelCurrentInningHits.Content = hits.ToString();
            if (radioButtonTop.IsChecked == true)
                visitorInningHits[inning - 1] = hits;
            else
                homeInningHits[inning - 1] = hits;
        }

        private void buttonHitsUp_Click(object sender, RoutedEventArgs e)
        {
            labelCurrentInningHits.Content = (Convert.ToInt32((string)labelCurrentInningHits.Content) + 1).ToString();
            int inning = int.Parse((string)labelCurrentInning.Content);
            if (radioButtonTop.IsChecked == true)
            {
                visitorInningHits[inning - 1]++;
                labelBoxVisitorTotalHits.Content = Increment((string)labelBoxVisitorTotalHits.Content).ToString();
            }
            else
            {
                homeInningHits[inning - 1]++;
                labelBoxHomeTotalHits.Content = Increment((string)labelBoxHomeTotalHits.Content).ToString();
            }
        }

        private void buttonHitsDown_Click(object sender, RoutedEventArgs e)
        {
            int hits = Convert.ToInt32((string)labelCurrentInningHits.Content);
            if (hits > 0)
            {
                --hits;
                labelCurrentInningHits.Content = hits.ToString();
                int inning = int.Parse((string)labelCurrentInning.Content);
                if (radioButtonTop.IsChecked == true)
                {
                    --visitorInningHits[inning - 1];
                    labelBoxVisitorTotalHits.Content = Decrement((string)labelBoxVisitorTotalHits.Content).ToString();
                }
                else
                {
                    --homeInningHits[inning - 1];
                    labelBoxHomeTotalHits.Content = Decrement((string)labelBoxHomeTotalHits.Content).ToString();
                }
            }
        }

        private void RestoreCurrentInningErrors()
        {
            int inning = int.Parse((string)labelCurrentInning.Content);
            int errs = ((radioButtonTop.IsChecked == true) ?
                        visitorInningErrors[inning - 1] :
                        homeInningErrors[inning - 1]);
            labelCurrentInningErrors.Content = errs.ToString();
        }

        private void buttonErrorsUp_Click(object sender, RoutedEventArgs e)
        {
            labelCurrentInningErrors.Content = (Convert.ToInt32((string)labelCurrentInningErrors.Content) + 1).ToString();
            int inning = int.Parse((string)labelCurrentInning.Content);
            if (radioButtonTop.IsChecked == true)
            {
                visitorInningErrors[inning - 1]++;
                labelBoxVisitorTotalErrors.Content = Increment((string)labelBoxVisitorTotalErrors.Content).ToString();
            }
            else
            {
                homeInningErrors[inning - 1]++;
                labelBoxHomeTotalErrors.Content = Increment((string)labelBoxHomeTotalErrors.Content).ToString();
            }
        }

        private void buttonErrorsDown_Click(object sender, RoutedEventArgs e)
        {
            int errs = Convert.ToInt32((string)labelCurrentInningErrors.Content);
            if (errs > 0)
            {
                --errs;
                labelCurrentInningErrors.Content = errs.ToString();
                int inning = int.Parse((string)labelCurrentInning.Content);
                if (radioButtonTop.IsChecked == true)
                {
                    --visitorInningErrors[inning - 1];
                    labelBoxVisitorTotalErrors.Content = Decrement((string)labelBoxVisitorTotalErrors.Content).ToString();
                }
                else
                {
                    --homeInningErrors[inning - 1];
                    labelBoxHomeTotalErrors.Content = Decrement((string)labelBoxHomeTotalErrors.Content).ToString();
                }
            }
        }

        private void buttonNewBatter_Click(object sender, RoutedEventArgs e)
        {
            labelCurrentBalls.Content = "0";
            labelCurrentStrikes.Content = "0";
        }

        private Color ShowColorDialog(Color _initialColor, object sender, EventArgs e)
        {
            var initialColor = SWMColorToSDColor(_initialColor);
            var MyDialog = new System.Windows.Forms.ColorDialog();
            // Keeps the user from selecting a custom color.
            MyDialog.AllowFullOpen = false;
            // Allows the user to get help. (The default is false.)
            MyDialog.ShowHelp = true;
            // Sets the initial color select to the current text color.
            MyDialog.Color = initialColor;
            if (MyDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return _initialColor;
            else
                return SDColorToSWMColor(MyDialog.Color);
        }

        private Color ChooseColor(Color initialColor, object sender, EventArgs e)
        {
            Color newColor = ShowColorDialog(initialColor, sender, e);
            if (newColor == null)
                newColor = initialColor;
            return newColor;
        }

        private void buttonHomeBkgndColor_Click(object sender, RoutedEventArgs e)
        {
            colorHomeBkgnd = ChooseColor(colorHomeBkgnd, sender, e);
            buttonHomeBkgndColor.Background = new SolidColorBrush(colorHomeBkgnd);
            Properties.Settings.Default.HomeBkgnd = ColorToHTML(colorHomeBkgnd);
        }

        private void buttonHomeForegndColor_Click(object sender, RoutedEventArgs e)
        {
            colorHomeForegnd = ChooseColor(colorHomeForegnd, sender, e);
            buttonHomeForegndColor.Background = new SolidColorBrush(colorHomeForegnd);
            Properties.Settings.Default.HomeForegnd = ColorToHTML(colorHomeForegnd);
        }

        private void buttonVisitorBkgndColor_Click(object sender, RoutedEventArgs e)
        {
            colorVisitorBkgnd = ChooseColor(colorVisitorBkgnd, sender, e);
            buttonVisitorBkgndColor.Background = new SolidColorBrush(colorVisitorBkgnd);
            Properties.Settings.Default.VisitorBkgnd = ColorToHTML(colorVisitorBkgnd);
        }

        private void buttonVisitorForegndColor_Click(object sender, RoutedEventArgs e)
        {
            colorVisitorForegnd = ChooseColor(colorVisitorForegnd, sender, e);
            buttonVisitorForegndColor.Background = new SolidColorBrush(colorVisitorForegnd);
            Properties.Settings.Default.VisitorForegnd = ColorToHTML(colorVisitorForegnd);
        }

        private void buttonUpdateScoreboards_Click(object sender, RoutedEventArgs e)
        {
            UpdateHTML();
        }

        private void buttonAddTemplate_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Title = "Choose scoreboard HTML template file(s).";
            ofd.Multiselect = true;
            ofd.InitialDirectory = initialFileDir;
            ofd.Filter = "OBS Scoreboard Template Files (*-template.html)|*-template.html";
            var result = ofd.ShowDialog();
            if (result == true)
                for (int i = 0; i < ofd.FileNames.Length; i++)
                    listboxTemplates.Items.Add(ofd.FileNames[i]);
            initialFileDir = ofd.InitialDirectory;
            Properties.Settings.Default.LastTemplatePath = initialFileDir;
        }

        private void buttonRemoveTemplates_Click(object sender, RoutedEventArgs e)
        {
            while (listboxTemplates.SelectedItems.Count > 0)
                listboxTemplates.Items.Remove(listboxTemplates.SelectedItem);
        }

        private void buttonDest_Click(object sender, RoutedEventArgs e)
        {
            var fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.Description = "Choose destination for scoreboard HTML files.";
            fbd.SelectedPath = buttonDestText.Text;
            var result = fbd.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                buttonDestText.Text = fbd.SelectedPath;
                Properties.Settings.Default.HTMLPath = buttonDestText.Text;
            }
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.TemplateList.Clear();
            foreach (object item in listboxTemplates.Items)
                Properties.Settings.Default.TemplateList.Add(item);

            Properties.Settings.Default.Save();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveSettings();
        }
    }
}
