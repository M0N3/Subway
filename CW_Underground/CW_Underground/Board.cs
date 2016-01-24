using System;

namespace CW_Underground
{
    class Board
    {
        private BoardWindow bWin;
        private TimeSpan _currentTime;
        private int hour, min, sec, rhour = 0, rmin = 0, rsec = 0, lhour = 0, lmin = 0, lsec = 0;
        public int StationNumber { get; set; }
        public string StationName { get; set; }
        private bool leftTimer = false;
        private bool rightTimer = false;
        private System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
       
        public void Start(int h, int m, int s)
        {
            timer.Stop();
            timer.Tick -= new EventHandler(tick);
            timer.Tick += new EventHandler(tick);
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            hour = h;
            min = m;
            sec = s;
            timer.Start();
        }
        public void Stop() {
            rhour = 0; rmin = 0; rsec = 0;
            lhour = 0; lmin = 0; lsec = 0;
            leftTimer = false;
            rightTimer = false;
            bWin.lastTrainRightLB.Content = "nope";
            bWin.lastTrainLeftLB.Content = "nope";
            timer.Stop();
            timer.Tick -= new EventHandler(tick);
        }
        private void AddTick(ref int hour, ref int min, ref int sec)//add one second
        {
            sec++;
            if (sec > 59) { sec = 0; min++; }
            if (min > 59) { min = 0; hour++; }
            if (hour > 23) { hour = 0; }
            CurrentTime = new TimeSpan(hour, min, sec);
        }

        private void tick(object sender, EventArgs e)
        {
            AddTick(ref hour, ref min, ref sec);
            if (bWin != null && bWin.IsLoaded)
            {
                bWin.currTimeLB.Content = CurrentTime;
                bWin.NameLB.Content = StationName;
            }
            if (leftTimer)
            {
                AddTick(ref lhour, ref lmin, ref lsec);
                bWin.lastTrainLeftLB.Content = new TimeSpan(lhour, lmin, lsec);
            }
            if (rightTimer)
            {
                AddTick(ref rhour, ref rmin, ref rsec);
                bWin.lastTrainRightLB.Content ="                                   " + new TimeSpan(rhour, rmin, rsec);
            }
        }
        public void TrainOnStation(int num, bool back)
        {
            string str = "Train № " + num + " On station now!";
            if (bWin != null && bWin.IsLoaded)
            {
                if (back)
                {
                    rightTimer = false;
                    bWin.lastTrainRightLB.Content = str;
                }
                else
                {
                    leftTimer = false;
                    bWin.lastTrainLeftLB.Content = str;
                }
            }
        }
        public void TrainLeft(bool back)
        {
            if (back)
            {
                rhour = 0; rmin = 0; rsec = 0;
                rightTimer = true;
            }
            else
            {
                lhour = 0; lmin = 0; lsec = 0;
                leftTimer = true;
            }
        }
        public TimeSpan CurrentTime
        {
            get { return _currentTime; }
            set { _currentTime = value; }
        }
        public BoardWindow BoardWindow
        {
            get { return bWin; }
            set { bWin = value; }
        }
        public Board(int num, string n)
        {

            bWin = new BoardWindow();
            StationNumber = num;
            StationName = n;
        }
    }
}
