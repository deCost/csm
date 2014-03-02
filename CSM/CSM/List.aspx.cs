using CSM.Classes;
using CSM.Master;


namespace CSM
{
	using System;
	using System.Web;
	using System.Web.UI;

	public partial class List : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

			Private privateFunctions = new Private ();
			User user = null;

			if (!privateFunctions.isLoggedSession (ref user)) {
				Response.Redirect ("~/");
			}

			if (!string.IsNullOrWhiteSpace (Request ["fn"])) {
				switch (Request ["fn"]) {
				case "e":
					schedule.lstSchedToShow.Add (ScheduleType.Event);
					schedule.lstSchedToShow.Add (ScheduleType.Students);
					linkeds.Visible = false;
					break;
				case "c":
					schedule.lstSchedToShow.Add (ScheduleType.Task);
					linkeds.Visible = false;
					break;
				case "a":
					schedule.Visible = false;
					linkeds.ProfileUser = user;
					linkeds.isMyProfile = true;
					break;
				}
			} else {
			}
		}
	}
}

