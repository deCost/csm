using System;

namespace CSM.Common
{
	public class Program
	{
		public int ID {
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

		public string Desc {
			get;
			set;
		}

		public string DescAbrev {
			get{ 
				string res = Desc.Substring(0, Math.Min(10, Desc.Length));

				res = Desc.Length > 10 ? res + "..." : res;

				return res;}

		}

		public string Name {
			get;
			set;
		}

		public Program ()
		{
		}
	}
}

