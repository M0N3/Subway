using System.Collections.Generic;

namespace CW_Underground
{
    class Railway
    {
        private string _railwayName;
 
        private List<Station> _stations = new List<Station>();
        public List<Station> GetStations
        {
            get { return _stations; }
        }
        private List<Train> _trains = new List<Train>();
        public List<Train> Trains
        {
            get { return _trains; }
            set { _trains = value; }
        }
        public void addTrain(Train t)
        {
            _trains.Add(t);
        }
        public void addStation(Station st)
        {
            _stations.Add(st);
        }
        public Railway()
        {
            _railwayName = "Railway";
        }
        public Railway(string name)
        {
            _railwayName = name;
           
        }
    }
}
