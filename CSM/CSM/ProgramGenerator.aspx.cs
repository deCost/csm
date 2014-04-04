using CSM.DataManager;
using CSM.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Text;

namespace CSM
{
	using System;
	using System.Web;
	using System.Web.UI;
	using System.Linq;

	public partial class ProgramGenerator : System.Web.UI.Page
	{
		protected void Page_Load (object sender, EventArgs e)
		{

			if (!Page.IsPostBack) {
				LoadPrograms ();
			

				var lstLevel = from Level l in Enum.GetValues (typeof(Level))
				               select new { Id = l, Name = l.ToString () };
				;

				drpPrgLevel.DataSource = lstLevel;
				drpPrgLevel.DataTextField = "Name";
				drpPrgLevel.DataValueField = "Id";
				drpPrgLevel.DataBind ();

				drpPrgLevel.Items.Insert (0, new ListItem (){ Text = "Seleccione nivel", Value = "0" });


			}

		}

		private void LoadPrograms ()
		{
			List<Program> lstPrograms = new List<Program> ();
			if (ClassRoomBS.GetPrograms (ref lstPrograms)) {
				rptProgram.DataSource = lstPrograms;
				rptProgram.DataBind ();
			}
			else {
				//TODO: Mensaje no hay
			}
		}

		protected void rdbProgramID_CheckedChange (object sender, EventArgs e)
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

		protected void btnNewProgram_Click (object sender, EventArgs e)
		{
			StringBuilder msg = new StringBuilder ();

			if (validateProgramForm (ref msg)) { 

				Program p = new Program (){ };

				if (ClassRoomBS.ProgramInsert (ref p)) {
					LoadPrograms ();
				}

			}

		}

		private bool validateProgramForm (ref StringBuilder msg)
		{
			bool ok = true;

			if (string.IsNullOrEmpty (txtPrgName.Text)) {
				msg.AppendLine ("Por favor, introduzca un nombre para la programación");
				ok = false;
			}

			if (string.IsNullOrEmpty (txtPrgDesc.Text)) {
				msg.AppendLine ("Por favor, introduzca una descripción para la programación");
				ok = false;
			}

			return ok;
		}
	}
}

