using System;
using System.IO;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CW_Underground
{
    /// <summary>
    /// Interaction logic for BoardWindow.xaml
    /// </summary>
    public partial class BoardWindow : Window
    {
        private bool loaded = false;
        public BoardWindow()
        {
            InitializeComponent();
            AdvertisingTBlock.Text = Advertising();
           
        }
        public string Advertising()//add advertising on board from file
        {
        
            string strLine; StringBuilder str = new StringBuilder();
            try
            {
                FileStream aFile = new FileStream("Reklama.txt", FileMode.Open);
                StreamReader sr = new StreamReader(aFile);
                strLine = sr.ReadLine();
                while (strLine != null)
                {
                    str.Append(strLine);
                    str.Append(" ");
                    strLine = sr.ReadLine();
                }
                sr.Close();
            }
            catch (IOException)
            {
                MessageBox.Show("IO ERROR");
            }
            return str.ToString();
        }
        private void On_Close(object sender, EventArgs e)
        {
            if (loaded)
            {
                animation.Completed -= myanim_Completed;
            }
            this.Close();
        }

        DoubleAnimation animation;
        // Скорость анимации
        Double speed = 4;   // (20 px за 0.1 секунду)
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            loaded = true;
            // Центрируем строку в канвасе
            Canvas.SetLeft(AdvertisingTBlock, (canvas.ActualWidth - AdvertisingTBlock.ActualWidth) / 2);
            animation = new DoubleAnimation();
            animation.Duration = TimeSpan.FromSeconds(0.1);
            // При завершении анимации, запускаем функцию MyAnimation снова
            // (указано в обработчике)
            animation.Completed += myanim_Completed;
            MyAnimation(Canvas.GetLeft(AdvertisingTBlock), Canvas.GetLeft(AdvertisingTBlock) - speed);
        }
        private void MyAnimation(double from, double to)
        {
            // Если строка вышла за пределы канваса (отриц. Canvas.Left)
            // то возвращаем с другой стороны
            if (Canvas.GetLeft(AdvertisingTBlock) + AdvertisingTBlock.ActualWidth <= 0)
            {
                animation.From = canvas.ActualWidth;
                animation.To = canvas.ActualWidth - speed;
                AdvertisingTBlock.BeginAnimation(Canvas.LeftProperty, animation);
            }
            else
            {
                animation.From = from;
                animation.To = to;
                AdvertisingTBlock.BeginAnimation(Canvas.LeftProperty, animation);
            }
        }
        private void myanim_Completed(object sender, EventArgs e)
        {
            loaded = true;
            MyAnimation(Canvas.GetLeft(AdvertisingTBlock), Canvas.GetLeft(AdvertisingTBlock) - speed);
        }
    }
}
