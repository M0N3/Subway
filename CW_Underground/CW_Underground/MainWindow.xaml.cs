using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;



namespace CW_Underground
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            fieldCanvas.MouseMove += Mouse_Move_On_Field;
            fieldCanvas.MouseLeftButtonUp += Field_Mouse_Left_Up;
            fieldCanvas.MouseLeave += Field_Mouse_Leave;
            ContextMenu menu = new ContextMenu();
            MenuItem create = new MenuItem();
            create.Header = "Add station";
            menu.Items.Add(create);
            create.Click += Add_Station_Ellipse_On_Field;
            fieldCanvas.ContextMenu = menu;
        }

        private void On_Load(object sender, RoutedEventArgs e)
        {
            //set the size of the canvas relative to the maximum window size
            if (this.WindowState == WindowState.Maximized)
            {
                fieldCanvas.Width = mainWindow.ActualWidth - 300;
                menuCanvas.Height = fieldCanvas.Height = mainWindow.ActualHeight;
                ImageBrush ib = new ImageBrush(new BitmapImage(new Uri("back.png", UriKind.Relative)));
                fieldCanvas.Background = ib;
            }
        }
        private void On_Closing(object sender, CancelEventArgs e)
        {
            foreach (Railway rl in subway.GetRailways)
            {
                foreach (Station st in rl.GetStations)
                {
                    st.Board.Stop();
                    if (st.Board.BoardWindow != null)
                    {
                        st.Board.BoardWindow.Close();
                    }
                }
            }
            timer.Stop();
            Application.Current.Shutdown();
        }

        private void Mouse_Left_Button_Down_menuCanvas(object sender, MouseEventArgs e)
        {
            SubwayLinesLBox.SelectedIndex = -1;
            if (fieldCanvas.Children.Count > 0 && fieldCanvas.Children[fieldCanvas.Children.Count - 1] is Menu)
            {
                //clear right click menu in field
                fieldCanvas.Children.RemoveAt(fieldCanvas.Children.Count - 1);
            }
        }


        internal Subway subway = new Subway();
        private void Add_Station_Ellipse_On_Field(object sender, RoutedEventArgs e)
        {
            if (on)
            {
                if (fieldCanvas.Children.Count > 0 && fieldCanvas.Children[fieldCanvas.Children.Count - 1] is Menu)
                {
                    //clear right click menu in field
                    fieldCanvas.Children.RemoveAt(fieldCanvas.Children.Count - 1);
                }

                if (SubwayLinesLBox.SelectedIndex != -1)
                {
                    double x = Mouse.GetPosition(fieldCanvas).X;
                    double y = Mouse.GetPosition(fieldCanvas).Y;

                    //if too near for window borders
                    if (x < 20 || y < 20 ||
                        x > (fieldCanvas.Width - 20) || y > (fieldCanvas.Height - 20))
                    { return; }

                    Point point = new Point(x, y);
                    foreach (Railway rl in subway.GetRailways)
                    {
                        foreach (Station st in rl.GetStations)
                        {
                            //if too near for 2 any station
                            if (Math.Sqrt((st.Coordinate.X - point.X) * (st.Coordinate.X - point.X) + (st.Coordinate.Y - point.Y) * (st.Coordinate.Y - point.Y)) < 40)
                            {
                                TooNearStationPP.IsOpen = true;
                                return;
                            }
                        }
                    }

                    foreach (Railway rl in subway.GetRailways)
                    {
                        //if too near for station and railway
                        if (rl.GetStations.Count > 1)
                        {
                            for (int i = 0; i < rl.GetStations.Count - 1; i++)
                            {
                                if (IntersectionCircleSegment
                                    (rl.GetStations.ElementAt(i).Coordinate.X, rl.GetStations.ElementAt(i).Coordinate.Y,
                                     rl.GetStations.ElementAt(i + 1).Coordinate.X, rl.GetStations.ElementAt(i + 1).Coordinate.Y,
                                     x, y, 30))
                                {
                                    TooNearPathPP.IsOpen = true;
                                    return;
                                }
                            }
                        }
                    }
                    foreach (Station st in subway.GetRailways.ElementAt(SubwayLinesLBox.SelectedIndex).GetStations)
                    {
                        //if too near for 2 station at 1 railway
                        if (Math.Sqrt((st.Coordinate.X - point.X) * (st.Coordinate.X - point.X) +
                            (st.Coordinate.Y - point.Y) * (st.Coordinate.Y - point.Y)) < 120)
                        {
                            TooNearStationPP.IsOpen = true;
                            return;
                        }
                    }

                    foreach (Railway rl in subway.GetRailways)
                    {
                        int count = (subway.GetRailways.IndexOf(rl) == SubwayLinesLBox.SelectedIndex) ? rl.GetStations.Count - 1 : rl.GetStations.Count;
                        for (int i = 0; i < count; i++)
                        {
                            //if the railway crosses stations
                            if (subway.GetRailways.ElementAt(SubwayLinesLBox.SelectedIndex).GetStations.Count > 0)
                            {
                                if (IntersectionCircleSegment
                                           (subway.GetRailways.ElementAt(SubwayLinesLBox.SelectedIndex).GetStations.
                                     ElementAt(subway.GetRailways.ElementAt(SubwayLinesLBox.SelectedIndex).GetStations.Count - 1).Coordinate.X,
                                     subway.GetRailways.ElementAt(SubwayLinesLBox.SelectedIndex).GetStations.
                                     ElementAt(subway.GetRailways.ElementAt(SubwayLinesLBox.SelectedIndex).GetStations.Count - 1).Coordinate.Y,
                                           point.X, point.Y, rl.GetStations.ElementAt(i).Coordinate.X, rl.GetStations.ElementAt(i).Coordinate.Y, 30))
                                {
                                    CrossesStationPP.IsOpen = true;
                                    return;
                                }
                            }
                        }
                    }
                    AddStationWindow win = new AddStationWindow(this);
                    win.Owner = this;
                    win.ShowDialog();

                    if (DialogResultOk)
                    {
                        DialogResultOk = false;
                        //Paint railway and station
                        if (subway.GetRailways.ElementAt(SubwayLinesLBox.SelectedIndex).GetStations.Count > 1)
                        {
                            System.Windows.Shapes.Path linePath = new System.Windows.Shapes.Path();
                            linePath.Stroke = SwitchFillBrushes(SubwayLinesLBox.SelectedIndex);
                            linePath.StrokeThickness = 4;
                            linePath.HorizontalAlignment = HorizontalAlignment.Center;
                            linePath.VerticalAlignment = VerticalAlignment.Center;
                            List<Point> pointsList = new List<Point>();
                            pointsList.Add(point);
                            pointsList.Add(point);
                            pointsList.Add(point);
                            PathFigure lineFigure = new PathFigure();
                            Point ps = subway.GetRailways.ElementAt(SubwayLinesLBox.SelectedIndex).GetStations.
                              ElementAt(subway.GetRailways.ElementAt(SubwayLinesLBox.SelectedIndex).GetStations.Count - 2).Coordinate;
                            lineFigure.StartPoint = ps;
                            lineFigure.Segments.Add(new PolyBezierSegment(pointsList, true));
                            PathGeometry lineGeometry = new PathGeometry();
                            lineGeometry.Figures.Add(lineFigure);
                            linePath.Data = lineGeometry;
                            linePath.MouseLeftButtonDown += Line_Mouse_Left_Down;
                            linePath.MouseLeftButtonUp += Line_Mouse_Left_Up;
                            Canvas.SetZIndex(linePath, -1);
                            subway.GetRailways.ElementAt(SubwayLinesLBox.SelectedIndex).GetStations.
                           ElementAt(subway.GetRailways.ElementAt(SubwayLinesLBox.SelectedIndex).GetStations.Count - 2).startLinePath = linePath;
                            fieldCanvas.Children.Add(linePath);

                            linePath = new System.Windows.Shapes.Path();
                            lineFigure = new PathFigure();
                            pointsList = new List<Point>();
                            pointsList.Add(ps);
                            pointsList.Add(ps);
                            pointsList.Add(ps);
                            lineFigure.StartPoint = point;
                            lineFigure.Segments.Add(new PolyBezierSegment(pointsList, true));
                            lineGeometry = new PathGeometry();
                            lineGeometry.Figures.Add(lineFigure);
                            linePath.Data = lineGeometry;
                            subway.GetRailways.ElementAt(SubwayLinesLBox.SelectedIndex).GetStations.
                           ElementAt(subway.GetRailways.ElementAt(SubwayLinesLBox.SelectedIndex).GetStations.Count - 1).endLinePath = linePath;
                        }

                        System.Windows.Shapes.Path ellipsePath = new System.Windows.Shapes.Path();
                        ellipsePath.Stroke = Brushes.LightSlateGray;
                        ellipsePath.Fill = SwitchFillBrushes(SubwayLinesLBox.SelectedIndex);
                        ellipsePath.StrokeThickness = 2;
                        ellipsePath.HorizontalAlignment = HorizontalAlignment.Center;
                        ellipsePath.VerticalAlignment = VerticalAlignment.Center;
                        EllipseGeometry myEllipseGeometry = new EllipseGeometry();
                        myEllipseGeometry.Center = new Point(x, y);
                        myEllipseGeometry.RadiusX = 20;
                        myEllipseGeometry.RadiusY = 20;
                        ellipsePath.Data = myEllipseGeometry;
                        ellipsePath.MouseLeftButtonDown += Ellipse_Mouse_Left_Down;
                        ellipsePath.MouseLeftButtonUp += Ellipse_Mouse_Left_Up;
                        ellipsePath.MouseRightButtonDown += Ellipse_Mouse_Right_Down;
                        subway.GetRailways.ElementAt(SubwayLinesLBox.SelectedIndex).GetStations.
                            ElementAt(subway.GetRailways.ElementAt(SubwayLinesLBox.SelectedIndex).GetStations.Count - 1).ellipsePath = ellipsePath;

                        ContextMenu menu = new ContextMenu();
                        MenuItem delete = new MenuItem();
                        delete.Header = "Delete station";
                        menu.Items.Add(delete);
                        delete.Click += Delete_Station_Click;
                        MenuItem board = new MenuItem();
                        board.Header = "Open board";
                        menu.Items.Add(board);
                        board.Click += Open_Board_Click;
                        ellipsePath.ContextMenu = menu;

                        fieldCanvas.Children.Add(ellipsePath);
                        TextBlock stationName = new TextBlock();
                        stationName.FontFamily = new FontFamily("Impact");
                        stationName.FontWeight = FontWeights.Light;
                        stationName.Text = subway.GetRailways.ElementAt(SubwayLinesLBox.SelectedIndex).GetStations.
                            ElementAt(subway.GetRailways.ElementAt(SubwayLinesLBox.SelectedIndex).GetStations.Count - 1).stationName.Text;
                        subway.GetRailways.ElementAt(SubwayLinesLBox.SelectedIndex).GetStations.
                            ElementAt(subway.GetRailways.ElementAt(SubwayLinesLBox.SelectedIndex).GetStations.Count - 1).stationName = stationName;
                        stationName.Margin = new Thickness(x - 35, y - 35, x, y);
                        fieldCanvas.Children.Add(stationName);
                        Canvas.SetZIndex(stationName, 100);
                        //Added  station coordinates
                        subway.GetRailways.ElementAt(SubwayLinesLBox.SelectedIndex).GetStations.
                            ElementAt(subway.GetRailways.ElementAt(SubwayLinesLBox.SelectedIndex).GetStations.Count - 1).
                            Coordinate = new Point(x, y);
                    }
                }
            }
        }

        private bool isAttached = false;
        private FrameworkElement attachedElement;
        private FrameworkElement attachedElementText;
        private FrameworkElement attachedElementLine;
        private Point clickPoint;
        private void Line_Mouse_Left_Down(object sender, MouseEventArgs e)
        {
            isAttached = true;
            attachedElementLine = sender as FrameworkElement;

            foreach (Railway rl in subway.GetRailways)
            {
                //need for offset lines
                for (int i = 0; i < rl.GetStations.Count - 1; i++)
                {
                    if (rl.GetStations.ElementAt(i).startLinePath.Equals(sender as System.Windows.Shapes.Path))
                    {
                        attachedElementLine = rl.GetStations.ElementAt(i).startLinePath as FrameworkElement;
                        clickPoint = rl.GetStations.ElementAt(i + 1).Coordinate;
                    }
                }
            }
        }
        private void Line_Mouse_Left_Up(object sender, MouseEventArgs e)
        {
            isAttached = false;
        }
        private void Field_Mouse_Leave(object sender, RoutedEventArgs e)
        {
            isAttached = false;
        }
        private void Ellipse_Mouse_Left_Down(object sender, MouseEventArgs e)
        {
            isAttached = true;
            attachedElement = sender as FrameworkElement;
            clickPoint = e.GetPosition(sender as FrameworkElement);
            foreach (Railway rl in subway.GetRailways)
            {
                //need for offset station name 
                foreach (Station st in rl.GetStations)
                {
                    if (st.ellipsePath.Equals(sender as System.Windows.Shapes.Path))
                    {
                        attachedElementText = st.stationName as FrameworkElement;
                    }
                }
            }
        }
        private void Ellipse_Mouse_Left_Up(object sender, MouseEventArgs e)
        {
            attachedElement = null;
            isAttached = false;
        }
        private void Field_Mouse_Left_Up(object sender, MouseEventArgs e)
        {
            isAttached = false;

        }
        private void Mouse_Move_On_Field(object sender, MouseEventArgs e)
        {
            if (on)
            {
                bool block = false;
                EllipseGeometry currentEllipse = new EllipseGeometry();
                PathGeometry ellipseLine = new PathGeometry();
                PathFigure startLineFigure = new PathFigure();
                PathFigure endLineFigure = new PathFigure();
                PathFigure backLineFigure = new PathFigure();
                PathFigure backendLineFigure = new PathFigure();
                Station st = new Station();
                if (isAttached && attachedElement != null)
                {
                    foreach (Railway rl in subway.GetRailways)
                    {
                        for (int i = 0; i < rl.GetStations.Count; i++)
                        {
                            if (rl.GetStations[i].ellipsePath == attachedElement as System.Windows.Shapes.Path)
                            {
                                currentEllipse = rl.GetStations[i].ellipsePath.Data as EllipseGeometry;
                                st = rl.GetStations[i];
                                if (rl.GetStations[i].startLinePath != null)
                                {
                                    ellipseLine = rl.GetStations[i].startLinePath.Data as PathGeometry;
                                    startLineFigure = ellipseLine.Figures.ElementAt(0);
                                    if (i + 1 < rl.GetStations.Count)
                                    {
                                        backLineFigure = (rl.GetStations[i + 1].endLinePath.Data as PathGeometry).Figures.ElementAt(0);
                                    }
                                }
                                if (i > 0)
                                {
                                    ellipseLine = rl.GetStations[i - 1].startLinePath.Data as PathGeometry;
                                    endLineFigure = ellipseLine.Figures.ElementAt(0);
                                    backendLineFigure = (rl.GetStations[i].endLinePath.Data as PathGeometry).Figures.ElementAt(0);
                                }
                            }
                        }
                    }
                    Point p = new Point(Mouse.GetPosition(fieldCanvas).X, Mouse.GetPosition(fieldCanvas).Y);
                    foreach (Railway rl in subway.GetRailways)
                    {
                        foreach (Station s in rl.GetStations)
                        {
                            if (s.ellipsePath != st.ellipsePath)
                            {
                                if (s.lineNumber == st.lineNumber)
                                {
                                    if (Math.Sqrt((s.Coordinate.X - Mouse.GetPosition(fieldCanvas).X) * (s.Coordinate.X - Mouse.GetPosition(fieldCanvas).X) + (s.Coordinate.Y - Mouse.GetPosition(fieldCanvas).Y) * (s.Coordinate.Y - Mouse.GetPosition(fieldCanvas).Y)) < 100)
                                    {
                                        block = true;
                                    }
                                }
                                else
                                {
                                    if (Math.Sqrt((s.Coordinate.X - Mouse.GetPosition(fieldCanvas).X) * (s.Coordinate.X - Mouse.GetPosition(fieldCanvas).X) + (s.Coordinate.Y - Mouse.GetPosition(fieldCanvas).Y) * (s.Coordinate.Y - Mouse.GetPosition(fieldCanvas).Y)) < 40)
                                    {
                                        block = true;
                                    }
                                }
                            }

                        }
                    }
                    if (!block)
                    {
                        double X = 35;
                        double Y = 30;
                        if (Mouse.GetPosition(fieldCanvas).X > 30)
                        {
                            X = Mouse.GetPosition(fieldCanvas).X;
                        }
                        if (Mouse.GetPosition(fieldCanvas).Y > 30)
                        {
                            Y = Mouse.GetPosition(fieldCanvas).Y;
                        }
                        currentEllipse.Center = new Point(X, Y);
                        st.Coordinate = currentEllipse.Center;
                        st.stationName.Margin = new Thickness(currentEllipse.Center.X - 35, currentEllipse.Center.Y - 35, currentEllipse.Center.X, currentEllipse.Center.Y);

                        startLineFigure.StartPoint = currentEllipse.Center;
                        backendLineFigure.StartPoint = currentEllipse.Center;
                        if (endLineFigure.Segments.Count > 0)
                        {
                            endLineFigure.Segments.RemoveAt(0);

                        }
                        if (backLineFigure.Segments.Count > 0)
                        {

                            backLineFigure.Segments.RemoveAt(0);
                        }

                        List<Point> pointsList = new List<Point>();
                        pointsList.Add(currentEllipse.Center);
                        pointsList.Add(currentEllipse.Center);
                        pointsList.Add(currentEllipse.Center);
                        endLineFigure.Segments.Add(new PolyBezierSegment(pointsList, true));
                        backLineFigure.Segments.Add(new PolyBezierSegment(pointsList, true));
                    }
                }
            }
        }
        private void Delete_Station_Click(object sender, RoutedEventArgs e)
        {
            if (on)
            {
                for (int i = 0; i < subway.GetRailways.Count; i++)
                {
                    Railway rl = subway.GetRailways.ElementAt(i);
                    for (int j = 0; j < subway.GetRailways.ElementAt(i).GetStations.Count; j++)
                    {
                        Station st = rl.GetStations.ElementAt(j);
                        if (st.ellipsePath == currElleipse)
                        {
                            if (st.Number == rl.GetStations.Count - 1)
                            {
                                fieldCanvas.Children.Remove(st.stationName);
                                fieldCanvas.Children.Remove(currElleipse);
                                if (st.Number > 0)
                                {
                                    fieldCanvas.Children.Remove(rl.GetStations.ElementAt(j - 1).startLinePath);
                                }
                                rl.GetStations.RemoveAt(rl.GetStations.Count - 1);
                            }
                            else
                            {
                                MessageBox.Show("You can delete only the last station!");
                            }
                        }
                    }
                }
            }
        }

        private System.Windows.Shapes.Path currElleipse;
        private void Ellipse_Mouse_Right_Down(object sender, RoutedEventArgs e)
        {

            currElleipse = sender as System.Windows.Shapes.Path;
        }
        private void Open_Board_Click(object sender, RoutedEventArgs e)
        {
            Station station = new Station();
            String nameLeft = "";
            string nameRight = "";
            foreach (Railway rl in subway.GetRailways)
            {
                foreach (Station st in rl.GetStations)
                {
                    if (st.ellipsePath.Equals(currElleipse))
                    {
                        station = st;
                        nameLeft = rl.GetStations.ElementAt(rl.GetStations.Count - 1).stationName.Text;
                        nameRight = rl.GetStations.ElementAt(0).stationName.Text;
                        break;
                    }
                }
            }
            if (station.Board.BoardWindow == null || !station.Board.BoardWindow.IsLoaded)
            {
                station.Board.BoardWindow = new BoardWindow();
                station.Board.BoardWindow.lastLeftLB.Content = nameLeft;
                station.Board.BoardWindow.lastRightLB.Content = nameRight;
                station.Board.BoardWindow.Show();
            }
        }
        private bool IntersectionCircleSegment(double x1, double y1, double x2, double y2, double xC, double yC, double R)
        {
            x1 -= xC;
            y1 -= yC;
            x2 -= xC;
            y2 -= yC;

            double dx = x2 - x1;
            double dy = y2 - y1;

            //составляем коэффициенты квадратного уравнения на пересечение прямой и окружности.
            //если на отрезке [0..1] есть отрицательные значения, значит отрезок пересекает окружность
            double a = dx * dx + dy * dy;
            double b = 2.0 * (x1 * dx + y1 * dy);
            double c = x1 * x1 + y1 * y1 - R * R;

            //а теперь проверяем, есть ли на отрезке [0..1] решения
            if (-b < 0)
                return (c < 0);
            if (-b < (2.0 * a))
                return ((4.0 * a * c - b * b) < 0);

            return (a + b + c < 0);
        }
        internal bool DialogResultOk
        {
            get;
            set;
        }
        private System.Windows.Media.SolidColorBrush SwitchFillBrushes(int index)
        {
            switch (index)
            {
                case 0:
                    return System.Windows.Media.Brushes.SpringGreen;
                case 1:
                    return System.Windows.Media.Brushes.MediumTurquoise;
                case 2:
                    return System.Windows.Media.Brushes.Crimson;
                case 3:
                    return System.Windows.Media.Brushes.Gold;
                default:
                    return System.Windows.Media.Brushes.Orchid;

            }
        }

        private void Create_Subway_Line_BT_Click(object sender, RoutedEventArgs e)
        {
            if (on)
            {
                AddSubwayLinesWindow win = new AddSubwayLinesWindow(this);
                win.Owner = this;
                win.ShowDialog();
            }
        }

        internal void AddSubwayLine(String name)
        {
            if (on)
            {
                Railway rl = new Railway(name);
                subway.addRailway(rl);
                SubwayLinesLBox.Items.Add(name);
            }
        }

        internal void AddStation(String name)
        {
            if (on)
            {
                Station st = new Station(name);
                st.lineNumber = SubwayLinesLBox.SelectedIndex;
                st.Number = subway.GetRailways.ElementAt(SubwayLinesLBox.SelectedIndex).GetStations.Count;
                subway.GetRailways.ElementAt(SubwayLinesLBox.SelectedIndex).addStation(st);
            }
        }

        private void AddTrain_Click(object sender, RoutedEventArgs e)
        {
            if (on)
            {
                AddTrainWindow win = new AddTrainWindow(this);
                win.Owner = this;
                win.ShowDialog();
            }

        }
        public bool addTrain(Train t, int i, int num, String time)
        {
            try
            {
                if (subway.GetRailways.ElementAt(i).GetStations.Count > 1)
                {


                    int hour, min, sec;
                    String[] arr = time.Split(':');
                    hour = Convert.ToInt32(arr[0]);
                    min = Convert.ToInt32(arr[1]);
                    sec = Convert.ToInt32(arr[2]);
                    TimeSpan tp = new TimeSpan(hour, min, sec);
                    foreach (Railway rl in subway.GetRailways)
                    {
                        foreach (Train tr in rl.Trains)
                        {
                            TimeSpan tps = tr.DepartureTime.Subtract(tp);
                            if (tps < new TimeSpan(0, 0, 0))
                            {
                                tps = -tps;
                            }
                            if (tps < new TimeSpan(0, 0, 10) && tr.LineNumber == t.LineNumber)
                            {
                                MessageBox.Show("Min interval across trains - 15 seconds");
                                return false;
                            }
                            if (t.Number == tr.Number)
                            {
                                MessageBox.Show("Already exists with this number!");
                                return false;
                            }
                        }
                    }
                    t.DepartureTime = tp;
                    t.TrainPath.Stroke = Brushes.LightSlateGray;
                    t.TrainPath.Fill = Brushes.AliceBlue;
                    t.TrainPath.StrokeThickness = 2;
                    t.TrainPath.HorizontalAlignment = HorizontalAlignment.Center;
                    t.TrainPath.VerticalAlignment = VerticalAlignment.Center;
                    RectangleGeometry myEllipseGeometry = new RectangleGeometry();
                    Point p = subway.GetRailways.ElementAt(i).GetStations.ElementAt(0).Coordinate;
                    myEllipseGeometry.Rect = new Rect(p.X - 15, p.Y - 15, 30, 30);
                    myEllipseGeometry.RadiusX = 10;
                    myEllipseGeometry.RadiusY = 10;
                    t.TrainPath.Data = myEllipseGeometry;
                    t.numOfPassage = num + 1;
                    t.passage = num + 1;
                    subway.GetRailways.ElementAt(i).addTrain(t);
                    return true;
                }
                else
                {
                    MessageBox.Show("On the railway should be more than one station");
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No this railway");
                return false;
            }
        }
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        private bool on = true;
        private int hour = 0, min = 0, sec = 0;
        private void runBT_Click(object sender, RoutedEventArgs e)
        {
            if (on)
            {
                try
                {
                    String[] arr = timeTBox.Text.Split(':');
                    hour = Convert.ToInt32(arr[0]);
                    min = Convert.ToInt32(arr[1]);
                    sec = Convert.ToInt32(arr[2]);
                    timeTBlock.Text = new TimeSpan(hour, min, sec).ToString();
                }
                catch (Exception)
                {
                    MessageBox.Show("Enter Time!");
                    return;
                }

                timer.Tick += new EventHandler(tick);
                timer.Interval = new TimeSpan(0, 0, 0, 1);
                timer.Start();
                on = false;
                foreach (Railway rl in subway.GetRailways)
                {
                    foreach (Station st in rl.GetStations)
                    {
                        st.Board.Start(hour, min, sec);
                    }
                }

            }
            else
            {
                timer.Stop();
                on = true;
                foreach (Railway rl in subway.GetRailways)
                {
                    foreach (Train t in rl.Trains)
                    {
                        t.Stop(fieldCanvas, subway);
                    }
                    foreach (Station st in rl.GetStations)
                    {
                        st.Board.Stop();
                    }
                }
                timer.Tick -= new EventHandler(tick);
            }
        }


        private TimeSpan currentTime;

        private void removeSubwayLineBT_Click(object sender, RoutedEventArgs e)
        {
            if (on)
            {
                RemoveSubwayLineWindow win = new RemoveSubwayLineWindow(this);
                win.Owner = this;
                win.ShowDialog();
            }
        }
        public void RemoveRailWay(int num)
        {
            try
            {
                if (subway.GetRailways.ElementAt(num - 1).GetStations.Count == 0)
                {
                    SubwayLinesLBox.Items.RemoveAt(num - 1);
                }
                else
                {
                    MessageBox.Show("It is necessary to remove all stations!");
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Wrong number!");
            }
        }

        private void removeTrainBT_Click(object sender, RoutedEventArgs e)
        {
            if (on)
            {
                RemoveTrainWindow win = new RemoveTrainWindow(this);
                win.Owner = this;
                win.ShowDialog();
            }
        }
        public bool RemoveTrain(int num)
        {
            foreach (Railway rl in subway.GetRailways)
            {
                foreach (Train t in rl.Trains)
                {
                    if (t.Number == num)
                    {
                        rl.Trains.Remove(t);
                        return true;
                    }
                }
            }
            return false;
        }

        private void violationBT_Click(object sender, RoutedEventArgs e)
        {
            ViolationWindow win = new ViolationWindow(this);
            win.Owner = this;
            win.ShowDialog();
        }
        public string GetViolation()
        {
            string str = "";
            foreach (Railway rl in subway.GetRailways)
            {
                foreach (Station st in rl.GetStations)
                {
                    foreach (string s in st.GetViolations)
                    {
                        str += s;
                        str += '\n';
                    }
                }
            }
            return str;
        }

        private void HelpBT_Click(object sender, RoutedEventArgs e)
        {
            string strLine; StringBuilder str = new StringBuilder();
            try
            {
                FileStream aFile = new FileStream("Help.txt", FileMode.Open);
                StreamReader sr = new StreamReader(aFile);
                strLine = sr.ReadLine();
                while (strLine != null)
                {
                    str.Append(strLine);
                    str.Append(" ");
                    str.Append("\n");
                    strLine = sr.ReadLine();
                }
                sr.Close();
            }
            catch (IOException)
            {
                MessageBox.Show("IO ERROR");
            }
            MessageBox.Show(str.ToString());
        }

        //private void backgroundBT_Click(object sender, RoutedEventArgs e)
        //{
        //    ImageBrush ib = new ImageBrush(new BitmapImage(new Uri("map1.jpg", UriKind.Relative)));
        //    fieldCanvas.Width = mainWindow.ActualWidth - 300 + ib.ImageSource.Width;
        //    menuCanvas.Height = fieldCanvas.Height = mainWindow.ActualHeight + ib.ImageSource.Height;
        //    fieldCanvas.Background = ib;
        //}

        private void AddTick(ref int hour, ref int min, ref int sec)//add one second
        {
            sec++;
            if (sec > 59) { sec = 0; min++; }
            if (min > 59) { min = 0; hour++; }
            if (hour > 23) { hour = 0; }
            currentTime = new TimeSpan(hour, min, sec);
            timeTBlock.Text = new TimeSpan(hour, min, sec).ToString();
        }

        private void tick(object sender, EventArgs e)
        {
            Railway rl = new Railway();
            Train t = new Train();
            for (int i = 0; i < subway.GetRailways.Count; i++)
            {
                rl = subway.GetRailways.ElementAt(i);
                for (int j = 0; j < subway.GetRailways.ElementAt(i).Trains.Count; j++)
                {
                    t = rl.Trains.ElementAt(j);
                    if (rl.GetStations.Count > 1)
                    {
                        ////////////////////////
                        if (subway.GetRailways.ElementAt(i).Trains.ElementAt(j).Start && subway.GetRailways.ElementAt(i).Trains.ElementAt(j).travelTime == 0 &&
                            subway.GetRailways.ElementAt(i).Trains.ElementAt(j).timeOnStation != 0 || !subway.GetRailways.ElementAt(i).Trains.ElementAt(j).Start &&
                            subway.GetRailways.ElementAt(i).Trains.ElementAt(j).DepartureTime == currentTime)
                        {
                            if (subway.GetRailways.ElementAt(i).Trains.ElementAt(j).DepartureTime == currentTime)
                            {
                                if (!fieldCanvas.Children.Contains(subway.GetRailways.ElementAt(i).Trains.ElementAt(j).TrainPath))
                                {
                                    subway.GetRailways.ElementAt(i).Trains.ElementAt(j).Stop(fieldCanvas, subway);
                                    fieldCanvas.Children.Add(subway.GetRailways.ElementAt(i).Trains.ElementAt(j).TrainPath);
                                }
                                subway.GetRailways.ElementAt(i).Trains.ElementAt(j).Start = true;
                            }
                            subway.GetRailways.ElementAt(i).Trains.ElementAt(j).onStation = true;
                            subway.GetRailways.ElementAt(i).Trains.ElementAt(j).timeOnStation--;
                            if (subway.GetRailways.ElementAt(i).Trains.ElementAt(j).timeOnStation != 0)
                            {
                                Random r = new Random(DateTime.Now.Millisecond);
                                if (r.Next(1, 100) % 55 == 0)
                                {
                                    String str = "Train number: " + subway.GetRailways.ElementAt(i).Trains.ElementAt(j).Number + " went ahead by " + subway.GetRailways.ElementAt(i).Trains.ElementAt(j).timeOnStation + " seconds at the station: " + subway.GetRailways.ElementAt(i).Trains.ElementAt(j).id;
                                    subway.GetRailways.ElementAt(i).Trains.ElementAt(j).timeOnStation = 0;
                                    subway.GetRailways.ElementAt(i).GetStations.ElementAt(subway.GetRailways.ElementAt(i).Trains.ElementAt(j).id).AddViolations(str);

                                }
                            }
                            if (subway.GetRailways.ElementAt(i).Trains.ElementAt(j).away && subway.GetRailways.ElementAt(i).Trains.ElementAt(j).timeOnStation == 0)
                            {
                                if (t.Back)
                                {
                                    rl.GetStations.ElementAt(t.id).Board.TrainLeft(true);
                                }
                                else
                                {
                                    rl.GetStations.ElementAt(t.id).Board.TrainLeft(false);
                                }
                                subway.GetRailways.ElementAt(i).Trains.ElementAt(j).Back = !subway.GetRailways.ElementAt(i).Trains.ElementAt(j).Back;
                                if (subway.GetRailways.ElementAt(i).Trains.ElementAt(j).numOfPassage != 1)
                                {
                                    subway.GetRailways.ElementAt(i).Trains.ElementAt(j).timeOnStation = 3;
                                }
                                subway.GetRailways.ElementAt(i).Trains.ElementAt(j).currStationInd = 0;
                                subway.GetRailways.ElementAt(i).Trains.ElementAt(j).away = false;
                            }
                            if (subway.GetRailways.ElementAt(i).Trains.ElementAt(j).timeOnStation == 0)
                            {
                                subway.GetRailways.ElementAt(i).Trains.ElementAt(j).onStation = false;
                                subway.GetRailways.ElementAt(i).Trains.ElementAt(j).leftStation = true;
                            }
                            subway.GetRailways.ElementAt(i).Trains.ElementAt(j).SetCurrId(subway);
                        }
                        if (subway.GetRailways.ElementAt(i).Trains.ElementAt(j).numOfPassage == 1 &&
                            subway.GetRailways.ElementAt(i).Trains.ElementAt(j).travelTime == 0 && subway.GetRailways.ElementAt(i).Trains.ElementAt(j).timeOnStation == 0)
                        {
                            subway.GetRailways.ElementAt(i).Trains.ElementAt(j).Start = false;
                            subway.GetRailways.ElementAt(i).Trains.ElementAt(j).Stop(fieldCanvas, subway);
                        }
                        /////////////////////////////////////
                        if (subway.GetRailways.ElementAt(i).Trains.ElementAt(j).travelTime != 0 && subway.GetRailways.ElementAt(i).Trains.ElementAt(j).travelTime > 0)
                        {
                            subway.GetRailways.ElementAt(i).Trains.ElementAt(j).travelTime--;
                        }
                        ///////////////////////////
                        if (t.onStation)
                        {
                            if (t.Back)
                            {
                                rl.GetStations.ElementAt(t.id).Board.TrainOnStation(t.Number, true);
                            }
                            else
                            {
                                rl.GetStations.ElementAt(t.id).Board.TrainOnStation(t.Number, false);
                            }
                        }
                        if (t.leftStation)
                        {
                            if (t.Back)
                            {
                                rl.GetStations.ElementAt(t.id).Board.TrainLeft(true);
                            }
                            else
                            {
                                rl.GetStations.ElementAt(t.id).Board.TrainLeft(false);
                            }
                            t.leftStation = false;
                        }
                        //////////////////////////
                        if (subway.GetRailways.ElementAt(i).Trains.ElementAt(j).Start && subway.GetRailways.ElementAt(i).Trains.ElementAt(j).travelTime == 0 && subway.GetRailways.ElementAt(i).Trains.ElementAt(j).timeOnStation == 0)
                        {
                            if (!fieldCanvas.Children.Contains(subway.GetRailways.ElementAt(i).Trains.ElementAt(j).TrainPath))
                            {
                                subway.GetRailways.ElementAt(i).Trains.ElementAt(j).Stop(fieldCanvas, subway);
                                fieldCanvas.Children.Add(subway.GetRailways.ElementAt(i).Trains.ElementAt(j).TrainPath);
                            }
                            subway.GetRailways.ElementAt(i).Trains.ElementAt(j).calcTime(subway, i, j);
                            subway.GetRailways.ElementAt(i).Trains.ElementAt(j).Animate(subway);
                            if (subway.GetRailways.ElementAt(i).Trains.ElementAt(j).currStationInd < subway.GetRailways.ElementAt(i).GetStations.Count - 2)
                            {
                                subway.GetRailways.ElementAt(i).Trains.ElementAt(j).currStationInd++;
                                subway.GetRailways.ElementAt(i).Trains.ElementAt(j).timeOnStation = 3;
                            }
                            else
                            {
                                if (subway.GetRailways.ElementAt(i).Trains.ElementAt(j).numOfPassage != 1)
                                {
                                    subway.GetRailways.ElementAt(i).Trains.ElementAt(j).numOfPassage--;
                                    subway.GetRailways.ElementAt(i).Trains.ElementAt(j).away = true;
                                    subway.GetRailways.ElementAt(i).Trains.ElementAt(j).currStationInd++;
                                    subway.GetRailways.ElementAt(i).Trains.ElementAt(j).timeOnStation = 3;
                                }
                                else
                                {
                                    subway.GetRailways.ElementAt(i).Trains.ElementAt(j).timeOnStation = 3;
                                }
                            }
                        }
                        //////////////////////////////////////////
                    }
                }
            }
            AddTick(ref hour, ref min, ref sec);
        }
    }
}
