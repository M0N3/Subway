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
    /// Interaction logic for ViolationWindow.xaml
    /// </summary>
    public partial class ViolationWindow : Window
    {
        MainWindow window;
        public ViolationWindow(MainWindow win)
        {
            InitializeComponent();
            window = win;
        }

        private void cancelBT_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void getBT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string str = window.GetViolation();
                richTextBox.Document.Blocks.Clear();
                richTextBox.AppendText(str);
            }
            catch(Exception)
            {
                MessageBox.Show("Wrong number!");
            }
        }
    }
}
