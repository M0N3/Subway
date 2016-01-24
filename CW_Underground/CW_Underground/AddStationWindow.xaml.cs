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
    /// Interaction logic for AddStationWindow.xaml
    /// </summary>
    public partial class AddStationWindow : Window
    {
        MainWindow window;
        public AddStationWindow(MainWindow win)
        {
            InitializeComponent();
            window = win;
        }

        private void Add_BT_Click(object sender, RoutedEventArgs e)
        {
            string name = StationNameTBox.Text;
            if (name != "")
            {
                if(name.Length > 20)
                {
                    name = name.Substring(0, 20);
                }
                window.AddStation(name);
                window.DialogResultOk = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Enter name!");
            }

        }

        private void Cancel_BT_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
