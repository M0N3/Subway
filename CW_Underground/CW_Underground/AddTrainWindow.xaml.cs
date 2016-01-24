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
    /// Interaction logic for AddTrainWindow.xaml
    /// </summary>
    public partial class AddTrainWindow : Window
    {
        private MainWindow win;
        public AddTrainWindow(MainWindow w)
        {
            InitializeComponent();
            win = w;
        }

        private void Add_BT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Train t = new Train(Convert.ToInt32(lineNumberTBox.Text) - 1, Convert.ToInt32(numberOfTrainTBox.Text));
                if(win.addTrain(t, t.LineNumber, Convert.ToInt32(numberOfPassageTBox.Text), departureTimeTBox.Text))
                {
                    AddedLb.Content = "Added";
                }
                else
                {
                    AddedLb.Content = "Not Added";
                }
              
            }
            catch (Exception)
            {
                MessageBox.Show("Not all fields are filled in correctly!");
            }
          

        }

        private void Cancel_BT_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
