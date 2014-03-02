using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSM.Classes
{
    public class ScheduleLink
    {
        private Decimal _schedID;

        public Decimal SchedID
        {
            get { return _schedID; }
            set { _schedID = value; }
        }
        private Decimal _userID;

        public Decimal UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }
    }
}
