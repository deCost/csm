using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSM.Classes
{
    public class Album
    {
        private Decimal _albumID;

        public Decimal AlbumID
        {
            get { return _albumID; }
            set { _albumID = value; }
        }
        private String _albumName;

        public String AlbumName
        {
            get { return _albumName; }
            set { _albumName = value; }
        }
        private String _albumDesc;

        public String AlbumDesc
        {
            get { return _albumDesc; }
            set { _albumDesc = value; }
        }
        private string _albumKeyPic;

        public string AlbumKeyPic
        {
            get { return _albumKeyPic; }
            set { _albumKeyPic = value; }
        }
        private Decimal _userID;

        public Decimal UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        private bool _isProfile;

        public bool IsProfile
        {
            get { return _isProfile; }
            set { _isProfile = value; }
        }

        private bool _isPublications;

        public bool IsPublications
        {
            get { return _isPublications; }
            set { _isPublications = value; }
        }

        private Decimal _ruleID;

        public Decimal RuleID
        {
            get { return _ruleID; }
            set { _ruleID = value; }
        }
    }
}
