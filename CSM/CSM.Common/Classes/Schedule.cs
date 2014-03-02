using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CSM.Classes
{
    public class Schedule
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
        private Decimal _ruleID;

        public Decimal RuleID
        {
            get { return _ruleID; }
            set { _ruleID = value; }
        }
        private ScheduleType _schedTypeID;

        public ScheduleType SchedTypeID
        {
            get { return _schedTypeID; }
            set { _schedTypeID = value; }
        }
        private DateTime _schedDate;

        public DateTime SchedDate
        {
            get { return _schedDate; }
            set { _schedDate = value; }
        }
        private String _schedTitle;

        public String SchedTitle
        {
            get { return _schedTitle; }
            set { _schedTitle = value; }
        }
        private String _schedDesc;

        public String SchedDesc
        {
            get { return _schedDesc; }
            set { _schedDesc = value; }
        }
        private bool _schedRepeat;

        public bool SchedRepeat
        {
            get { return _schedRepeat; }
            set { _schedRepeat = value; }
        }

		private int _schedBooking;

		public int SchedBooking
		{
			get { return _schedBooking; }
			set { _schedBooking = value; }
		}

		public string SchedBookingUrl
		{
			get{
				return ConfigurationManager.AppSettings["bookingUrl"] + SchedBooking.ToString ();
			}
		}

        private List<string> _friends;

        public List<string> Friends
        {
            get { return _friends; }
            set { _friends = value; }
        }


        public string CssClass
        {
            get { return _schedTypeID == ScheduleType.Task 
                ? "task" 
                : _schedTypeID == ScheduleType.Event
                    ? "event"
                    : "birthdate";}
        }
    }
}
