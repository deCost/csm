using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Reflection;
using System.IO;
using CSM.Master;
using CSM.Classes;
using CMS.DataManager;


namespace CSM
{
    public partial class CreateSchedule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Private privateFunctions = new Private();
            User user = new User();
            
            try
            {

                if (privateFunctions.isLoggedSession(ref user))
                {
                    if (!IsPostBack)
                    {
                        List<String> hoursList = new List<String>();
                        List<String> minutesList = new List<String>();

                        for (int i = 0; i < 60; i++) minutesList.Add(i > 9 ? i.ToString() : "0" + i.ToString());
                        for (int i = 0; i < 24; i++) hoursList.Add(i > 9 ? i.ToString() : "0" + i.ToString());

                        drpHours.DataSource = hoursList;
                        drpMinutes.DataSource = minutesList;

                        drpMinutes.DataBind();
                        drpHours.DataBind();

                        List<UserLink> requestList = new List<UserLink>();

                        // Gets users with connection request pending
                        if (!GlobalBS.GetUsersLinkBy(ref user, ref requestList, Status.Active))
                        {
                            throw new WrongDataException("Lo sentimos pero ocurrió un error al recuperar los usuarios enlazados");
                        }

                        // Recover number of user connections request
                        if (requestList.Count > 0)
                        {
							chkUserLinked.DataSource = requestList;
							chkUserLinked.DataTextField = "Name";
							chkUserLinked.DataValueField = "UserIDReq";
							chkUserLinked.DataBind();
                            

                        }
                        else
                        {
							chkUserLinked.DataSource = null;
							chkUserLinked.DataBind();
                            //txtNoFriends.Visible = true;
                        }

                        drpType.DataSource = GlobalBS.GetScheduleTypes();
                        drpType.DataValueField = "Type";
                        drpType.DataTextField = "Name";
                        drpType.DataBind();

                    }
                }
            }
            catch (WrongDataException ex)
            {
                //Script register to show exception info
				ScriptManager.RegisterStartupScript(this.Page,this.GetType(), "showMsg", string.Format(@"alertWarning('{0}');", ex.Message),true);
                return;
            }
            catch (Exception ex)
            {
                //Script register to show exception info
				ScriptManager.RegisterStartupScript(this.Page,this.GetType(), "showMsg", @"alertError('Lo sentimos pero ha ocurrido un error inexperado');",true);

                Utilities.LogException(Path.GetFileName(Request.Path),
                            MethodInfo.GetCurrentMethod().Name,
                            ex);
                return;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            StringBuilder msg = new StringBuilder();
            User user = null;
            Private privateFunctions = new Private();

            if (validateForm(ref msg))
            {
                DateTime dt = DateTime.Parse(string.Format("{0} {1}:{2}", dateinput.Text, drpHours.SelectedValue, drpMinutes.SelectedValue));
                
                dt.AddHours(double.Parse(drpHours.SelectedValue));
                dt.AddMinutes(double.Parse(drpMinutes.SelectedValue));

                privateFunctions.isLoggedSession(ref user);
                Schedule schd = new Schedule(){
					SchedDesc = (string.IsNullOrEmpty(txtDesc.Text) || txtDesc.Text == "Descripción de la programación") ? "" : txtDesc.Text,
                    SchedTitle = txtTitle.Text,
                    SchedTypeID = Utilities.GetScheduleType(int.Parse(drpType.SelectedValue)),
                    UserID= user.UserID,
                    SchedDate = dt,
					Friends = chkUserLinked.Items.Cast<ListItem>().Where(n => n.Selected).Select(n => n.Value).ToList()
                };

				try{
					schd.SchedBooking = int.Parse(bookinginput.Text);
				}catch{
				}


                if (GlobalBS.InsertNewSchedule(ref schd))
                {
                    ClearForm();
					lblresponse.Text = "Su programación ha sido registrada correctamente";
                }
            }

        }

        private void ClearForm()
        {
            txtTitle.Text = string.Empty;
            txtDesc.Text = string.Empty;
            dateinput.Text = string.Empty;
            txtFriend.Text = string.Empty;
			chkUserLinked.Items.Cast<ListItem>().Where(n => n.Selected).ToList().ForEach((delegate(ListItem item) { item.Selected = false; }));
        }

        /// <summary>
        /// Method to validate register form
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool validateForm(ref StringBuilder msg)
        {
            if (string.IsNullOrEmpty(txtTitle.Text) || txtTitle.Text == "Título de la programación")
            {
                msg.Append("<p>Por favor, rellene el título</p>");
            }

            DateTime tmp;
            if (dateinput.Text == string.Empty ||
                !DateTime.TryParse(dateinput.Text, out tmp))
            {
                msg.Append("<p>Por favor, introduce una fecha correcta</p>");
            }

			int booking;
			if (bookinginput.Text != string.Empty &&
				!int.TryParse(bookinginput.Text, out booking))
			{
				msg.Append("<p>Por favor, introduce un evento correcto</p>");
			}

            if (msg.Length > 0)
                return false;
            
            return true;
        }

        protected void chkLinked_SelectedIndexChanged(object sender, EventArgs e)
        {
			var q = chkUserLinked.Items.Cast<ListItem>().Where(n => n.Selected).ToList();
            string str = string.Join(", ", q);           

            if (q.Count > 2)
            {
                str = string.Format("{0}, {1} y otros {2} amigos más...", q[0], q[1], q.Count-2);
            }

            txtFriend.Text = str;
        }
    }
}