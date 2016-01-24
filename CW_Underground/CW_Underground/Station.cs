using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
namespace CW_Underground
{
    class Station 
    {
        private TextBlock _stationName;
        private Point _coordinate;
        private Path _ellipsePath;//for station ellipse on field
        private Path _startLinePath;//for railway in station
        private Board _stationBoard;
        public int lineNumber;
        public int Number;
        private Path _endLinePath;//for railway in station
        private List<String> violationsSchedules = new List<string>();
        public Path ellipsePath
        {
            get { return _ellipsePath; }
            set { _ellipsePath = value; }
        }
        public List<string> GetViolations
        {
            get { return violationsSchedules; }
            set { violationsSchedules = value; }
        }
        public void AddViolations(String str)
        {
            violationsSchedules.Add(str);
        }
        public Board Board
        {
            get { return _stationBoard; }
            set { _stationBoard = value; }
        }
        public Path startLinePath
        {
            get { return _startLinePath; }
            set { _startLinePath = value; }
        }
        public Path endLinePath
        {
            get { return _endLinePath; }
            set { _endLinePath = value; }
        }
   
        public TextBlock stationName
        {
            get { return _stationName; }
            set { _stationName = value; }
        }
        public Point Coordinate
        {
            get { return _coordinate; }
            set { _coordinate = value; }
        }
        public Station(string name)
        {
            _stationName = new TextBlock();
            _stationName.Text = name;
            Board = new Board(Number, stationName.Text);
        }
        public Station()
        {
            Board = new Board(Number, "name");
        }
    }
}
