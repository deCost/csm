using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSM.Classes
{
    public enum ScheduleType
    {
        Task = 0,
        Event = 1,
		Students = 2
    }

    public class ScheduleClass
    {
        int type;

        public int Type
        {
            get { return type; }
            set { type = value; }
        }
        string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }



    }
}
