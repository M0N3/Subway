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
using System.Windows.Shapes;

namespace CW_Underground
{
    /// <summary>
    /// Interaction logic for RemoveTrainWindow.xaml
    /// </summary>
    public partial class RemoveTrainWindow : Window
    {
        MainWindow window;
        public RemoveTrainWindow(MainWindow win)
        {
            window = win;
            InitializeComponent();
            foreach (Railway rl in window.subway.GetRailways)
            {
                foreach (Train t in rl.Trains)
                {
                    richTextBox.AppendText("Train number: " + t.Number.ToString() + " at railway number: " + (t.LineNumber + 1).ToString() + "\n");
                }
            }
        }

        private void removeBT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int num = Convert.ToInt32(removeTBox.Text);
                
                if (window.RemoveTrain(num))
                {
                    resLB.Content = "Removed";
                    richTextBox.Document.Blocks.Clear();
                    foreach (Railway rl in window.subway.GetRailways)
                    {
                        foreach (Train t in rl.Trains)
                        {
                            richTextBox.AppendText("Train number: " + t.Number.ToString() + " at railway number: " + (t.LineNumber + 1).ToString());
                            richTextBox.AppendText("\n");
                        }
                    }
                }
                else
                {
                    resLB.Content = "No this train";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Wrong number!");
            }
        }

        private void CancelBT_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
