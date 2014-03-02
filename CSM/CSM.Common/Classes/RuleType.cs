using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSM.Classes
{
    public enum RuleType
    {
        All = 0,
        Images = 1,
        Publications = 2
    }

    public class RuleTypeClass
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
