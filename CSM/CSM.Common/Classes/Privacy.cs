using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSM.Classes
{
    public class Privacy
    {
        private Decimal _privID;

        public Decimal PrivID
        {
            get { return _privID; }
            set { _privID = value; }
        }
        private Decimal _ruleID;

        public Decimal RuleID
        {
            get { return _ruleID; }
            set { _ruleID = value; }
        }
        private String _privName;

        public String PrivName
        {
            get { return _privName; }
            set { _privName = value; }
        }
        private Decimal _privOptionID;

        public Decimal PrivOptionID
        {
            get { return _privOptionID; }
            set { _privOptionID = value; }
        }
        private List<string> _users;

        public List<string> Users
        {
            get { return _users; }
            set { _users = value; }
        }
    }
}
