using System.Collections.Generic;

namespace CW_Underground
{
    class Subway
    {
        private List<Railway> _railways = new List<Railway>();
        public List<Railway> GetRailways
        {
            get { return _railways; }
        }
        public void addRailway(Railway rl)
        {
            _railways.Add(rl);
        }
        public Subway(){ }
    }
}
