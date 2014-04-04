using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSM.Common;

namespace CSM.Classes
{
	public class User : ITicket
	{
		public Group Group {
			get;
			set;
		}

		private Decimal _userID;

		public Decimal UserID {
			get { return _userID; }
			set { _userID = value; }
		}

		private String _userEmail;

		public String UserEmail {
			get { return _userEmail; }
			set { _userEmail = value; }
		}

		private String _userName;

		public String UserName {
			get { return _userName; }
			set { _userName = value; }
		}

		private String _userSurname;

		public String UserSurname {
			get { return _userSurname; }
			set { _userSurname = value; }
		}

		private DateTime _userBirth;

		public DateTime UserBirth {
			get { return _userBirth; }
			set { _userBirth = value; }
		}

		private String _userAddress;

		public String UserAddress {
			get { return _userAddress; }
			set { _userAddress = value; }
		}

		private String _userLogin;

		public String UserLogin {
			get { return _userLogin; }
			set { _userLogin = value; }
		}

		private String _userPass;

		public String UserPass {
			get { return _userPass; }
			set { _userPass = value; }
		}

		private Status _statuID;

		public Status StatuID {
			get { return _statuID; }
			set { _statuID = value; }
		}

		private DateTime _loginDate;

		public DateTime LoginDate {
			get { return _loginDate; }
			set { _loginDate = value; }
		}

		private String _sessionID;

		public String SessionID {
			get { return _sessionID; }
			set { _sessionID = value; }
		}

		private string _profileImage;

		public string ProfileImage {
			get { return _profileImage == "" ? "/images/noimageprofile.jpg" : "/userData/" + _profileImage; }
			set { _profileImage = value; }
		}

		private Decimal _albumProfileID;

		public Decimal AlbumProfileID {
			get { return _albumProfileID; }
			set { _albumProfileID = value; }
		}

		private Decimal _albumPublID;

		public Decimal AlbumPublID {
			get { return _albumPublID; }
			set { _albumPublID = value; }
		}

		public string AuthenticationType {
			get { return ""; }
		}

		public bool IsAdmin {
			get;
			set;
		}

		public decimal TotalPerformance { get; set; }

		public DateTime LastDate {get;set;}

		private decimal _myPoints;

		public decimal MyPoints
		{ get{ return _myPoints;} set{ _myPoints = value;} }

		public decimal TotalPoints {
			get{
				decimal totalSum = 0;
				 

				if(TotalPerformance > 0)
				{
					totalSum += (decimal)((DateTime.Now.Subtract(LastDate).TotalDays / 14) * -50);

					totalSum += TotalPerformance * 25;
				}

				return totalSum;
			}

		}

		public bool IsAuthenticated {
			get { return _statuID == Status.Logged; }
		}

		public string Name {
			get { return string.Format ("{0} {1}", _userName, _userSurname); }
		}
	}
}
