using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSM.Classes
{
    public class Publication : IPublication
    {
        private Decimal _publID;

        public Decimal PublID
        {
            get { return _publID; }
            set { _publID = value; }
        }

        private Decimal _parentpublID;

        public Decimal ParentPublID
        {
            get { return _parentpublID; }
            set { _parentpublID = value; }
        }
        
        private User _user;

        public User User
        {
            get { return _user; }
            set { _user = value; }
        }

        private Decimal _ruleID;

        public Decimal RuleID
        {
            get { return _ruleID; }
            set { _ruleID = value; }
        }
        private String _publDesc;

        public String PublDesc
        {
            get { return _publDesc; }
            set { _publDesc = value; }
        }
        private DateTime _publDate;

        public DateTime PublDate
        {
            get { return _publDate; }
            set { _publDate = value; }
        }

        private Picture _image;

        public Picture Image
        {
            get { return _image; }
            set { _image = value; }
        }

        private List<Publication> _comments;

        public List<Publication> Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        /// <summary>
        /// Return a TimeSpan with date difference between Now and messageDate
        /// </summary>
        /// <returns>Time</returns>
        public TimeSpan datediff()
        {
            return DateTime.Now.Subtract(_publDate);
        }

        /// <summary>
        /// Returns css style for bubbles
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        public string GetsCssStyle(Decimal u)
        {
            if (this.User.UserID == u)
            {
                return "mybubble";
            }
            else
            {
                return "friendbubble";
            }

        }
    }
}
