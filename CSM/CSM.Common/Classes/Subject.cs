using System;

namespace CSM.Common
{
	public class Subject
	{
		public int SubjectID {
			get;
			set;
		}

		public string Desc {
			get;
			set;
		}

		public string Name {
			get;
			set;
		}

		public int ProgramID {
			get;
			set;
		}

		public string DescAbrev {
			get{ 
				string res = Desc.Substring(0, Math.Min(10, Desc.Length));

				res = Desc.Length > 10 ? res + "..." : res;

				return res;}

		}

		public Subject ()
		{
		}
	}
}

