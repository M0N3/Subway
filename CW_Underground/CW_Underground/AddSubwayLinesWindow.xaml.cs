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
    /// Interaction logic for AddSubwayLinesWindow.xaml
    /// </summary>
    public partial class AddSubwayLinesWindow : Window
    {
        private MainWindow window;
        public AddSubwayLinesWindow(MainWindow win)
        {
            InitializeComponent();
            window = win;
        }

        private void Add_BT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResultLB.Content = "";
                string name = SubwayLinesNameTBox.Text;
                if(name == "")
                {
                    MessageBox.Show("Enter name!");
                    return;
                }
                window.AddSubwayLine(name);
                ResultLB.Content = "Added!";
                SubwayLinesNameTBox.Text = "";
            }
            catch(Exception)
            {
                ResultLB.Content = "Not Added!";
            }
        }

        private void Cancel_BT_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
