using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CW_Underground
{
    public class Train : FrameworkElement
    {
        private TimeSpan departureTime;
        private int lineNumber;
        private Path trainPath = new Path();
        public int travelTime = 1;
        public bool Back;
        public int passage;
        public int numOfPassage;
        public int timeOnStation = 3;
        public bool Start = false;
        public int currStationInd = 0;
        public int id = 0;
        public int Number;
        public bool away = false;
        public bool onStation = false;
        public bool leftStation = false;
        private Storyboard pathAnimationStoryboard = new Storyboard();
        internal void Stop(Canvas field, Subway subway)
        {
            pathAnimationStoryboard.Children.Clear();
            field.Children.Remove(trainPath);
            RectangleGeometry rc = trainPath.Data as RectangleGeometry;
            trainPath.RenderTransform = null;
            travelTime = 0;
            timeOnStation = 3;
            Start = false;
            onStation = false;
            leftStation = false;
            numOfPassage = passage;
            currStationInd = 0;
            id = 0;
            Back = false;
            away = false;
            if (subway.GetRailways.ElementAt(lineNumber).GetStations.Count > 1)
            {
                if (subway.GetRailways.ElementAt(lineNumber).GetStations.ElementAt(0) != null)
                {
                    Station st = subway.GetRailways.ElementAt(lineNumber).GetStations.ElementAt(0);
                    rc.Rect = new Rect(st.Coordinate.X - 15, st.Coordinate.Y - 15, 30, 30);
                }
                else
                {
                    trainPath = null;
                }
            }
        }
        internal void SetCurrId(Subway subway)
        {
            if (Back)
            {
                id = subway.GetRailways.ElementAt(lineNumber).GetStations.Count - currStationInd - 1;
            }
            else
            {
                id = currStationInd;
            }
        }
        internal void calcTime(Subway subway, int i, int j)
        {
            Point ps = subway.GetRailways.ElementAt(i).GetStations.ElementAt(id).Coordinate;
            Point p = new Point();
            if (Back)
            {
                p = subway.GetRailways.ElementAt(i).GetStations.ElementAt(id - 1).Coordinate;
            }
            else
            {
                if (id + 1 < subway.GetRailways.ElementAt(i).GetStations.Count)
                {
                    p = subway.GetRailways.ElementAt(i).GetStations.ElementAt(id + 1).Coordinate;
                }
            }
            int time = (int)(Math.Sqrt((p.X - ps.X) * (p.X - ps.X) + (p.Y - ps.Y) * (p.Y - ps.Y))) / 50;
            Random r = new Random(DateTime.Now.Millisecond);
            if (r.Next(1, 30) % 20 == 0)
            {
                int temp = time / 10;
                if (temp == 0)
                {
                    temp = 1;
                }
                time += temp;
                String str = "Train number: " + subway.GetRailways.ElementAt(i).Trains.ElementAt(j).Number + " was late by " + temp + " seconds at the station: " + id;
                subway.GetRailways.ElementAt(i).GetStations.ElementAt(id).AddViolations(str);
            }
            travelTime = time;
        }
        internal void Animate(Subway subway)
        {
            pathAnimationStoryboard = new Storyboard();
            Start = true;
            MatrixTransform buttonMatrixTransform = new MatrixTransform();
            this.TrainPath.RenderTransform = buttonMatrixTransform;
            NameScope.SetNameScope(this, new NameScope());
            RectangleGeometry rc = TrainPath.Data as RectangleGeometry;
            EllipseGeometry e = subway.GetRailways.ElementAt(lineNumber).GetStations.ElementAt(id).ellipsePath.Data as EllipseGeometry;
            Point c = e.Center;
            rc.Rect = new Rect(c.X, c.Y, 30, 30);
            this.RegisterName("ButtonMatrixTransform", buttonMatrixTransform);
            PathGeometry animationPath = new PathGeometry();
            if (Back)
            {
                animationPath = subway.GetRailways.ElementAt(lineNumber).GetStations.ElementAt(id).endLinePath.Data as PathGeometry;
            }
            else
            {
                if (subway.GetRailways.ElementAt(lineNumber).GetStations.ElementAt(id).startLinePath != null)
                {
                    animationPath = subway.GetRailways.ElementAt(lineNumber).GetStations.ElementAt(id).startLinePath.Data as PathGeometry;
                }
            }
            Point p = subway.GetRailways.ElementAt(lineNumber).GetStations.ElementAt(id).Coordinate;
            rc.Rect = new Rect(rc.Rect.X - p.X - 15, rc.Rect.Y - p.Y - 15, 30, 30);
            MatrixAnimationUsingPath matrixAnimation =
                new MatrixAnimationUsingPath();
            matrixAnimation.PathGeometry = animationPath;
            matrixAnimation.Duration = TimeSpan.FromSeconds(travelTime);
            matrixAnimation.DoesRotateWithTangent = true;
            Storyboard.SetTargetName(matrixAnimation, "ButtonMatrixTransform");
            Storyboard.SetTargetProperty(matrixAnimation,
                new PropertyPath(MatrixTransform.MatrixProperty));
            pathAnimationStoryboard.Children.Add(matrixAnimation);
            pathAnimationStoryboard.Begin(this);
        }
        public Path TrainPath
        {
            set { trainPath = value; }
            get { return trainPath; }
        }
        public int LineNumber
        {
            set { lineNumber = value; }
            get { return lineNumber; }
        }
        public TimeSpan DepartureTime
        {
            set { departureTime = value; }
            get { return departureTime; }
        }
        public Train(int i, int j)
        {
            lineNumber = i;
            Number = j;
            Back = false;
        }
        public Train() { }
    }
}
