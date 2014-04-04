using System;

namespace CSM.Classes
{
	public class StudentSchedule
	{
		public decimal SchedID{ get; set;}
		public decimal UserID { get; set;}
		public string UserName { get; set;}
		public string UserSurname { get; set;}
		public string CompleteName {
			get{ 
				return string.Concat (UserName, " ", UserSurname);
			}
		}
		public DateTime SchedDate { get; set;}
		public decimal Points { get; set;}
		public decimal TotalPoints { get; set;}

		public StudentSchedule ()
		{
		}
	}
}

