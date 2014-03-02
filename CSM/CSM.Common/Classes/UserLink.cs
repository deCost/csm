using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSM.Classes
{
    public class UserLink
    {
        private Decimal _userIDReq;
        private DateTime _linkDate;
        private Decimal _userIDTo;
        private Status _statusID;
        private String _name;

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Decimal UserIDReq
        {
            get { return _userIDReq; }
            set { _userIDReq = value; }
        }

        public Decimal UserIDTo
        {
            get { return _userIDTo; }
            set { _userIDTo = value; }
        }
        
        public Status StatusID
        {
            get { return _statusID; }
            set { _statusID = value; }
        }
        
        public DateTime LinkDate
        {
            get { return _linkDate; }
            set { _linkDate = value; }
        }

        private string _profileImage;

        public string ProfileImage
        {
            get { return _profileImage == "" ? "/images/noimageprofile.jpg" : "/userData/" + _profileImage; }
            set { _profileImage = value; }
        }
    }
}
