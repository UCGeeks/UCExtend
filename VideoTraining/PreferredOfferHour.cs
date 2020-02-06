using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCExtend.VideoTraining
{
    public class PreferredOfferHour
    {
        private int hour;
        public int Hour
        {
            get { return hour; }
            set { hour = value; }
        }

        private bool isOfferedForHour;
        public bool IsOfferedForHour
        {
            get { return isOfferedForHour; }
            set { isOfferedForHour = value; }
        }

        private int tries;
        public int Tries
        {
            get { return tries; }
            set { tries = value; }
        }
    }
}
