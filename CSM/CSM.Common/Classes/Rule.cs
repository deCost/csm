using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace CSM.Classes
{
    public class Rule : IEqualityComparer
    {
        private Decimal _ruleID;

        public Decimal RuleID
        {
            get { return _ruleID; }
            set { _ruleID = value; }
        }
        private RuleType _ruleTypeID;

        public RuleType RuleTypeID
        {
            get { return _ruleTypeID; }
            set { _ruleTypeID = value; }
        }
        private Decimal _userID;

        public Decimal UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }
        private String _ruleName;

        public String RuleName
        {
            get { return _ruleName; }
            set { _ruleName = value; }
        }
        private String _ruleDesc;

        public String RuleDesc
        {
            get { return _ruleDesc; }
            set { _ruleDesc = value; }
        }

        public new bool Equals(object x, object y)
        {
            return ((Rule)x).RuleID.CompareTo(((Rule)y).RuleID) == 0;
        }

        public int GetHashCode(object obj)
        {
            return obj.ToString().ToLower().GetHashCode();
        }
    }
}
