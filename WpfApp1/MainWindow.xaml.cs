using System;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int MAXINNING = 13;
        private Label[] labelBoxInningVisitorRuns = new Label[MAXINNING];
        private Label[] labelBoxInningHomeRuns = new Label[MAXINNING];

        public MainWindow()
        {
            InitializeComponent();
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
        }

        private int Decrement(String number, int limit = 0)
        {
            int value = Convert.ToInt32(number);
            if (value > limit)
                --value;
            return value;
        }

        private int Increment(String number, int limit = 0)
        {
            int value = Convert.ToInt32(number);
            if ((limit == 0) || (value < limit))
                value++;
            return value;
        }

        private void UpdateCurrentInningRuns()
        {
            int inning = int.Parse((string)labelCurrentInning.Content);
            string runs = (string)((radioButtonTop.IsChecked == true) ?
                                    labelBoxInningVisitorRuns[inning - 1].Content :
                                    labelBoxInningHomeRuns[inning - 1].Content);
            labelCurrentInningRuns.Content = runs;
        }

        private void buttonInningUp_Click(object sender, RoutedEventArgs e)
        {
            labelCurrentInning.Content = Increment((String)labelCurrentInning.Content, MAXINNING).ToString();
            UpdateCurrentInningRuns();
        }

        private void buttonInningDown_Click(object sender, RoutedEventArgs e)
        {
            labelCurrentInning.Content = Decrement((String)labelCurrentInning.Content, 1).ToString();
            UpdateCurrentInningRuns();
        }

        private void radioButtonTop_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentInningRuns();
        }

        private void radioButtonBottom_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentInningRuns();
        }

        private void buttonRunsUp_Click(object sender, RoutedEventArgs e)
        {
            int inning = int.Parse((string)labelCurrentInning.Content);
            string runs = Increment((String)labelCurrentInningRuns.Content).ToString();
            labelCurrentInningRuns.Content = runs;
            if (radioButtonTop.IsChecked == true)
                labelBoxInningVisitorRuns[inning - 1].Content = runs;
            else
                labelBoxInningHomeRuns[inning - 1].Content = runs;
        }

        private void buttonRunsDown_Click(object sender, RoutedEventArgs e)
        {
            int inning = int.Parse((string)labelCurrentInning.Content);
            string runs = Decrement((String)labelCurrentInningRuns.Content).ToString();
            labelCurrentInningRuns.Content = runs;
            if (radioButtonTop.IsChecked == true)
                labelBoxInningVisitorRuns[inning - 1].Content = runs;
            else
                labelBoxInningHomeRuns[inning - 1].Content = runs;
        }
    }
}
