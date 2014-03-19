using System;
using CSM.Classes;

namespace CSM.Common
{
	public class Group
	{
		public int ID {
			get;
			set;
		}

		public int ProgramID {
			get;
			set;
		}

		public string Name {
			get;
			set;
		}

		public DateTime Date {
			get;
			set;
		}

		public Level Level {
			get;
			set;
		}

		public User Responsible {
			get;
			set;
		}

		public Group ()
		{
		}
	}
}

