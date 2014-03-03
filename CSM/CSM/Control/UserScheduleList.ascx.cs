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


namespace CSM.Control
{
    public partial class UserScheduleList : System.Web.UI.UserControl
    {
		public List<ScheduleType> lstSchedToShow = new List<ScheduleType>();

        protected void Page_Load(object sender, EventArgs e)
        {
            // By using private.Master public mehods, we don't need to create a utilities class for website
            Private privateFunctions = new Private();

            try
            {
                User user = null;
                if (privateFunctions.isLoggedSession(ref user))
                {
                    string eventArgs = Request["__EVENTARGUMENT"];
                    if (!Page.IsPostBack || eventArgs.StartsWith("RefreshSchedule"))
                    {
                        List<Schedule> scheduleList = new List<Schedule>();

                        if (GlobalBS.GetScheduleFromUser(ref user, ref scheduleList))
                        {
							rptSchedule.DataSource = scheduleList.FindAll(s => lstSchedToShow.Contains(s.SchedTypeID));
							rptSchedule.DataBind();
                        }
                    }
                }

            }
            catch (WrongDataException ex)
            {
                Utilities.LogException(Path.GetFileName(Request.Path),
                            MethodInfo.GetCurrentMethod().Name,
                            ex);
            }
            catch (Exception ex)
            {
                //Script register to show exception info
				ScriptManager.RegisterStartupScript(this, this.GetType(), "showMsg", @"alertError('Lo sentimos pero ha ocurrido un error inexperado');", true);
                Utilities.LogException(Path.GetFileName(Request.Path),
                            MethodInfo.GetCurrentMethod().Name,
                            ex);
            }
        }
    }
}