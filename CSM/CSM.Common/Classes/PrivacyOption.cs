using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSM.Classes
{
    public enum PrivacyOption
    {
        OnlyFriends = 0,
        FriendsAndLinked = 1
    }

    public class PrivacyOptionClass
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
