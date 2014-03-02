using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSM.Classes
{
    public class Message
    {
        private Decimal _useridFrom;
        private String _messageTxt;
        private Decimal _messageID;

        public Decimal MessageID
        {
            get { return _messageID; }
            set { _messageID = value; }
        }
        private DateTime _messageDate;

        public DateTime MessageDate
        {
            get { return _messageDate; }
            set { _messageDate = value; }
        }
        private Decimal _useridTo;

        /// <summary>
        /// Gets userID of message reciver
        /// </summary>
        public Decimal UseridFrom
        {
            get { return _useridFrom; }
            set { _useridFrom = value; }
        }
        
        /// <summary>
        /// Gets userID of message creator
        /// </summary>
        public Decimal UseridTo
        {
            get { return _useridTo; }
            set { _useridTo = value; }
        }
        
        /// <summary>
        /// Gets message text
        /// </summary>
        public String MessageTxt
        {
            get { return _messageTxt; }
            set { _messageTxt = value; }
        }

        /// <summary>
        /// Return a TimeSpan with date difference between Now and messageDate
        /// </summary>
        /// <returns>Time</returns>
        public TimeSpan datediff()
        {
            return DateTime.Now.Subtract(_messageDate);
        }

    }
}
