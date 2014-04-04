using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.IO;
using CSM.Master;
using CSM.Classes;
using CMS.DataManager;
using CSM.Common;

namespace CSM.Control
{
	public partial class UserScheduleList : System.Web.UI.UserControl
	{
		public List<EventType> lstEventToShow = new List<EventType> ();
		Private privateFunctions = new Private ();
		User user = null;
		List<StudentSchedule> studentList;

		protected void Page_Load (object sender, EventArgs e)
		{
			// By using private.Master public mehods, we don't need to create a utilities class for website
            

			try {
				if (privateFunctions.isLoggedSession (ref user)) {
					string eventArgs = Request ["__EVENTARGUMENT"];
					if (!Page.IsPostBack || eventArgs.StartsWith ("RefreshSchedule")) {
						LoadScheduleList ();

					}
				}

			} catch (WrongDataException ex) {
				Utilities.LogException (Path.GetFileName (Request.Path),
					MethodInfo.GetCurrentMethod ().Name,
					ex);
			} catch (Exception ex) {
				//Script register to show exception info
				ScriptManager.RegisterStartupScript (this, this.GetType (), "showMsg", @"alertError('Lo sentimos pero ha ocurrido un error inexperado');", true);
				Utilities.LogException (Path.GetFileName (Request.Path),
					MethodInfo.GetCurrentMethod ().Name,
					ex);
			}
		}

		protected void btnStuden_Click (object sender, EventArgs e)
		{
			Button btn = (Button)sender;

			int eventID = int.Parse (btn.Attributes ["data-value"]);

			privateFunctions.isLoggedSession (ref user);

			if (GlobalBS.InsertNewStudentRequest (eventID, user.UserID)) {
				LoadScheduleList ();
			}

		}

		private void LoadScheduleList ()
		{
			List<Schedule> scheduleList = new List<Schedule> ();
			studentList = new List<StudentSchedule> ();

			if (GlobalBS.GetScheduleFromUser (ref user, ref scheduleList, ref studentList)) {
				List<Schedule> noavailableEvents = scheduleList.FindAll (ss => ss.UserID == user.UserID);

				// Remove non listable items
				scheduleList.RemoveAll (ss => !(lstEventToShow.Contains (ss.EventType) || ss.UserID == user.UserID));

				// Filtering for each event the user has been booked
				noavailableEvents.ForEach (ss => scheduleList.RemoveAll (ss2 => ss2.SchedID == ss.SchedID));

				rptSchedule.DataSource = scheduleList;
				rptSchedule.DataBind ();

				rptBooked.DataSource = noavailableEvents;
				rptBooked.DataBind ();
			}
		}

		protected void rptBooked_ItemDataBound (object sender, RepeaterItemEventArgs e)
		{
			if ((e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) && studentList != null && studentList.Count > 0) {

				List<StudentSchedule> students = studentList.FindAll (s => s.SchedID == ((Schedule)e.Item.DataItem).SchedID);

				Utilities.GetStudentsTotalPoints (ref students);

				/*students.Add (new StudentSchedule () { 
					UserName = user.UserName,
					UserSurname = user.UserSurname,
					UserID = user.UserID,
					TotalPoints = user.TotalPoints
				});*/

				students.OrderByDescending (s => s.TotalPoints);

				((Repeater)e.Item.FindControl ("rptStudents")).DataSource = students;
				((Repeater)e.Item.FindControl ("rptStudents")).DataBind ();
			}

		}
	}
}