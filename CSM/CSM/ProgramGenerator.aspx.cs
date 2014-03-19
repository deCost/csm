using CSM.DataManager;
using CSM.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace CSM
{
	using System;
	using System.Web;
	using System.Web.UI;

	public partial class ProgramGenerator : System.Web.UI.Page
	{
		protected void Page_Load (object sender, EventArgs e)
		{
			List<Program> lstPrograms = new List<Program> ();
			
			if (ClassRoomBS.GetPrograms (ref lstPrograms)) {
				rptProgram.DataSource = lstPrograms;
				rptProgram.DataBind ();
			} else {
				//TODO: Mensaje no hay
			}

		}

		protected void rdbProgramID_CheckedChange(object sender, EventArgs e)
		{
			RadioButton rdb = (RadioButton)sender;
			List<Subject> lstSubject = new List<Subject> ();
			Program program = new Program (){ ID = int.Parse (rdb.Attributes ["data-val"]) };
			if (ClassRoomBS.GetSubjects (ref program, ref lstSubject)) {
				rptSubject.DataSource = lstSubject;
				rptSubject.DataBind ();
			} else {
				//TODO: Mensaje de no hay
			}




		}
	}
}

