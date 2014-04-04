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
    public partial class TopBar : System.Web.UI.UserControl
    {
        protected string UserName;

        protected void Page_Load(object sender, EventArgs e)
        {
            // By using private.Master public mehods, we don't need to create a utilities class for website
            Private privateFunctions = new Private();
            
            try
            {
                User user = null;
                if (privateFunctions.isLoggedSession(ref user))
                {
                    UserName = user.Name;

                    
                }

				List<KeyValuePair<string,string>> lstLinks = new List<KeyValuePair<string, string>>();

				lstLinks.Add(new KeyValuePair<string, string>("Menú",string.Empty));
				lstLinks.Add(new KeyValuePair<string, string>("Eventos","~/List.aspx?fn=e"));
				lstLinks.Add(new KeyValuePair<string, string>("Mis datos","~/Settings.aspx"));

				if(user.IsAdmin)
				{
					lstLinks.Add(new KeyValuePair<string, string>("Clases","~/List.aspx?fn=c"));
					lstLinks.Add(new KeyValuePair<string, string>("Amigos","~/List.aspx?fn=a"));
					lstLinks.Add(new KeyValuePair<string, string>("Crear evento","~/CreateSchedule.aspx"));
					lstLinks.Add(new KeyValuePair<string, string>("Crear programa","~/ProgramGenerator.aspx"));
				}

				drpMenu.DataSource = rptMenu.DataSource = lstLinks;
				drpMenu.DataTextField = "Key";
				drpMenu.DataValueField = "Value";

				rptMenu.DataBind();
				drpMenu.DataBind();
            }
            catch (WrongDataException ex)
            {
                Utilities.LogException("AjaxHandler.ashx",
                           MethodInfo.GetCurrentMethod().Name, ex);
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

		protected void drpMenu_Change(object sender, EventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(drpMenu.SelectedValue)) {
				Response.Redirect (drpMenu.SelectedValue,true);
			}
		}

        /// <summary>
        /// Event handler for logout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkLogout_Click(object sender, EventArgs e)
        {

            Private privateFunctions = new Private();
            try
            {
                User user = null;
                if (privateFunctions.isLoggedSession(ref user))
                {
                    // Remove connection
                    Global.sessionsTable.Remove(user.SessionID);

                    Response.Redirect("Default.aspx");
                    
                }
            }
            catch (WrongDataException ex)
            {
                Utilities.LogException("AjaxHandler.ashx",
                           MethodInfo.GetCurrentMethod().Name, ex);
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