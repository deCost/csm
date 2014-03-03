using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSM.Classes;
using System.IO;
using System.Reflection;

namespace CSM.Master
{
    public partial class Private : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                User user = null;
                if (!isLoggedSession(ref user) )
                {
                    //Script register to restore button functionality and show any issue or message
					ScriptManager.RegisterStartupScript(this, this.GetType(), "showMsg", @"jsError('Lo sentimos pero su sesión ha caducado');", true);
                    return;
                }

				//hdnuserid.Value = user.UserID.ToString();
				//ucmyprofile.ProfileUser = ucmylinks.ProfileUser = ucpicslide.ProfileUser = user;
				//ucmyprofile.isMyProfile = ucmylinks.isMyProfile = ucpicslide.isMyProfile = true;
            }
            catch (WrongDataException ex)
            {
                //Script register to restore button functionality and show any issue or message
				ScriptManager.RegisterStartupScript(this, this.GetType(), "showMsg", string.Format(@"jsError('{0}');", ex.Message), true);
                return;
            }
            catch (Exception ex)
            {
                Utilities.LogException("AjaxHandler.ashx",
                           MethodInfo.GetCurrentMethod().Name, ex);
                //Script register to restore button functionality and show any issue or message
				ScriptManager.RegisterStartupScript(this, this.GetType(), "showMsg", @"jsError('Lo sentimos pero ocurrió un error inexperado');", true);
                return;
            }
        }

        /// <summary>
        /// Method to recover session from cache
        /// </summary>
        /// <param name="user">true recovered, false no</param>
        internal bool isLoggedSession(ref User user)
        {
            user = null;

            if (Context.Request.Cookies["session"] != null)
            {

                string session = Context.Request.Cookies["session"].Value;
                user = (User)Global.sessionsTable[session];
                return user != null && user.StatuID == Status.Active;
            }

            return false;
        }

        
    }
}