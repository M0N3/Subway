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
    /// Interaction logic for RemoveSubwayLineWindow.xaml
    /// </summary>
    public partial class RemoveSubwayLineWindow : Window
    {
        MainWindow window;
        public RemoveSubwayLineWindow(MainWindow win)
        {
            window = win;
            InitializeComponent();
        }

        private void removeBT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int num = Convert.ToInt32(removeTB.Text);
                window.RemoveRailWay(num);
            }
            catch (Exception)
            {
                MessageBox.Show("Wrong Number!");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
