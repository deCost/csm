using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Reflection;
using System.Text;
using CSM.Master;
using CSM.Classes;
using CMS.DataManager;


namespace CSM
{
    public partial class Settings : System.Web.UI.Page
    {
        private string _profileImageUrl;
        private string _userSessionID;

        public string UserSessionID
        {
            get { return _userSessionID; }
            set { _userSessionID = value; }
        }

        public string ProfileImageUrl
        {
            get { return _profileImageUrl; }
            set { _profileImageUrl = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Private privateFunctions = new Private();
            User user = new User();
            List<Rule> rules = new List<Rule>();
            try
            {

                if (privateFunctions.isLoggedSession(ref user))
                {
                    UserSessionID = user.SessionID;
                    if (!IsPostBack)
                    {
                        LoadForm(ref user, ref rules);
                    }
                }
            }
            catch (WrongDataException ex)
            {
                //Script register to show exception info
				//ScriptManager.RegisterStartupScript(this.Page,this.GetType(), "showMsg", string.Format(@"jsAlert('{0}');", ex.Message), true);
                return;
            }
            catch (Exception ex)
            {
                //Script register to show exception info
				//ScriptManager.RegisterStartupScript(this.Page,this.GetType(), "showMsg", @"jsError('Lo sentimos pero ha ocurrido un error inexperado');", true);

                Utilities.LogException(Path.GetFileName(Request.Path),
                            MethodInfo.GetCurrentMethod().Name,
                            ex);
                return;
            }
        }

        /// <summary>
        /// Handler from profile modifications send
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProfile_Click(object sender, EventArgs e)
        {

            Private privateFunctions = new Private();
            User user = new User();
            StringBuilder msg = new StringBuilder("");
            try
            {

                privateFunctions.isLoggedSession(ref user);
				UploadFile();

                Picture pic = null;
                if (Session[user.SessionID + "_profile"] != null)
                {
                    pic = (Picture)Session[user.SessionID + "_profile"];
                    pic.AlbumID = user.AlbumProfileID;
                }

                if (_validateProfileForm(ref msg))
                {


                    if( pic != null) user.ProfileImage = pic.PicPath;
                    user.UserName = nameinput.Text;
                    user.UserSurname = surnameinput.Text;
                    DateTime b;
                    if (DateTime.TryParse(birthdateinput.Text, out b))
                    {
                        user.UserBirth = b;
                    }
                    user.UserEmail = emailinput.Text;

                    if (GlobalBS.UpdateUser(ref user))
                    {
                        if (pic != null)
                        {

                            if (GlobalBS.InsertNewImage(user, pic))
                            {
                                user.AlbumProfileID = pic.AlbumID;
                                Session.Remove(user.SessionID + "_profile");

                            }
                        }
                    }                    

                    Global.sessionsTable[user.SessionID] = user;

                    Context.Response.Redirect(Request.Url.ToString());
                }
                else
                {
                    throw new WrongDataException(msg.ToString());
                }
            }
            catch (WrongDataException ex)
            {
                //Script register to show exception info
				ScriptManager.RegisterStartupScript(this.Page,this.GetType(), "showMsg", string.Format(@"jsAlert('{0}');", string.Format(ex.Message,user.UserLogin)),true);
                lblprofilemsg.Text = ex.Message;
                return;
            }
            catch (Exception ex)
            {
                //Script register to show exception info
				ScriptManager.RegisterStartupScript(this.Page,this.GetType(), "showMsg", @"jsError('Lo sentimos pero ha ocurrido un error inexperado');",true);

                lblprofilemsg.Text = "Lo sentimos pero ha ocurrido un error inexperado";

                Utilities.LogException(Path.GetFileName(Request.Path),
                            MethodInfo.GetCurrentMethod().Name,
                            ex);
                return;
            }

        }

		/// <summary>
		/// Event handler for file upload completed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void UploadFile()
		{

			if (uplProfile.HasFile)
			{
				try
				{

					Private privateFunctions = new Private();
					User user = new User();

					privateFunctions.isLoggedSession(ref user);

					Picture pic = null;
					HttpPostedFile file = uplProfile.PostedFile;
					string imgmsg = "";
					pic = new Picture() { AlbumID = 0, PicDate = DateTime.Now };
					if (!Utilities.UploadImageFromUser(file, ref user, ref pic, ref imgmsg))
					{
						return;
					}

					Session[user.SessionID + "_profile"] = pic;                   

				}
				catch (Exception ex)
				{
					Utilities.LogException(Path.GetFileName(Request.Path),
						MethodInfo.GetCurrentMethod().Name,
						ex);

				}
			}


		}

        /// <summary>
        /// Method to validate register form
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool _validateProfileForm(ref StringBuilder msg)
        {
            if (nameinput.Text == string.Empty || surnameinput.Text == string.Empty)
            {
                msg.Append("<p>Por favor, rellene su nombre</p>");
            }

            DateTime tmp;
            if (birthdateinput.Text == string.Empty ||
                !DateTime.TryParse(birthdateinput.Text, out tmp))
            {
                msg.Append("<p>Por favor, introduce una fecha de nacimiento correcta</p>");
            }

            if (passinput.Text != string.Empty && passinput.Text == "Contraseña" && (
                reppassinput.Text != passinput.Text))
            {
                msg.Append("<p>Por favor, introduce una contraseña correcta</p>");
                if (reppassinput.Text != passinput.Text) msg.Append("Las contraseñas no coinciden</p>");
            }

            if (emailinput.Text == string.Empty ||
                !Utilities.checkEmail(emailinput.Text) ||
                emailinput.Text != repemailinput.Text)
            {
                msg.Append("<p>Por favor, introduce un email correcto</p>");
                if (emailinput.Text != repemailinput.Text) msg.Append("<p>Los emails no coinciden</p>");
            }

            if (msg.Length > 0)
                return false;

            msg.AppendLine("Petición de modificación:");
            msg.AppendLine(string.Format("Nombre:{0}", nameinput.Text));
            msg.AppendLine(string.Format("Apellidos:{0}", surnameinput.Text));
            msg.AppendLine("Usuario:{0}");
            msg.AppendLine(string.Format("Email:{0}\n", emailinput.Text));

            return true;
        }


        /// <summary>
        /// Method to load form
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rules"></param>
        private void LoadForm(ref User user, ref List<Rule> rules)
        {
			imgProfile.ImageUrl = user.ProfileImage;

            nameinput.Text = user.UserName;
            surnameinput.Text = user.UserSurname;
            birthdateinput.Text = user.UserBirth.ToString("dd/MM/yyyy");

            emailinput.Text = repemailinput.Text = user.UserEmail;
            
            //Remove session to prevent unfinished work interferences
            Session.Remove(user.SessionID + "_profile");
            
            List<UserLink> requestList = new List<UserLink>();

            // Gets users with connection request pending
            if (!GlobalBS.GetUsersLinkBy(ref user, ref requestList, Status.Active))
            {
                throw new WrongDataException("Lo sentimos pero ocurrió un error al recuperar los usuarios enlazados");
            }

            // Recover number of user connections request
            if (requestList.Count > 0)
            {
                chkLinked.DataSource = requestList;
                chkLinked.DataTextField = "Name";
                chkLinked.DataValueField = "UserIDReq";
                chkLinked.DataBind();


            }
            else
            {
                chkLinked.DataSource = null;
                chkLinked.DataBind();
                //txtNoFriends.Visible = true;
            }

            List<PrivacyOptionClass> privacyOptionList = GlobalBS.GetPrivacyOptionTypes();
            drpVisibility.DataSource = privacyOptionList;
            drpVisibility.DataTextField = "Name";
            drpVisibility.DataValueField = "Type";
            drpVisibility.DataBind();

            List<RuleTypeClass> ruleTypeList = GlobalBS.GetRuleTypes();
            rdbRuleType.DataSource = ruleTypeList;
            rdbRuleType.DataTextField = "Name";
            rdbRuleType.DataValueField = "Type";
            rdbRuleType.DataBind();

            List<Rule> ruleList = new List<Rule>();

            if (GlobalBS.GetRulesFromUser(user, ruleList))
            {
                ruleList.Add(new Rule() { RuleName = "Nueva regla", RuleID = -1});
                drpRule.DataSource = ruleList;
                drpRule.DataTextField = "RuleName";
                drpRule.DataValueField = "RuleID";
                drpRule.SelectedValue = "-1";
                drpRule.DataBind();
            }

        }

        


        protected void chkLinked_SelectedIndexChanged(object sender, EventArgs e)
        {
            var q = chkLinked.Items.Cast<ListItem>().Where(n => n.Selected).ToList();
            string str = string.Join(", ", q);

            if (q.Count > 2)
            {
                str = string.Format("{0}, {1} y otros {2} amigos más...", q[0], q[1], q.Count - 2);
            }

            txtFriend.Text = str;
        }

        protected void btnRule_Click(object sender, EventArgs e)
        {

             Private privateFunctions = new Private();
            User user = new User();
            StringBuilder msg = new StringBuilder("");
            try
            {

                if (_validateRulesForm(ref msg))
                {

                    privateFunctions.isLoggedSession(ref user);

                    Rule rule = new Rule()
                    {
                        RuleID = Decimal.Parse(drpRule.SelectedValue),
                        RuleName = txtRuleName.Text,
                        UserID = user.UserID,
                        RuleDesc = txtRuleDesc.Text,
                        RuleTypeID = Utilities.GetRuleTypeID(rdbRuleType.SelectedValue)
                    };

					Classes.Privacy privacy = new Classes.Privacy()
                    {
                        RuleID = Decimal.Parse(drpRule.SelectedValue),
                        PrivName = drpVisibility.SelectedItem.Text,
                        PrivOptionID = Decimal.Parse(drpVisibility.SelectedValue),
                        Users = chkLinked.Items.Cast<ListItem>().Where(n => n.Selected).Select(n => n.Value).ToList()
                    };

                    if (rule.RuleID < 0)
                    {
                        /***************************************
                         * New Rule
                         ***************************************/
                        if (GlobalBS.InsertNewRule(ref rule))
                        {

                            privacy.RuleID = rule.RuleID;

                            if (GlobalBS.InsertNewPrivacy(ref privacy))
                            {
                                lblResponseRule.Text = "Se ha completado la generación de la nueva regla. Ahora estará disponible para sus publicaciones";
                            }

                        }
                    }
                    else
                    {
                        /***************************************
                         * Modify Rule
                         ***************************************/
                        if (GlobalBS.ModifyRule(rule, privacy))
                        {
                            lblResponseRule.Text = "Se ha completado la modificación de la nueva regla.";
                        }
                    }
                }
            }
            catch (WrongDataException ex)
            {
                //Script register to show exception info
				ScriptManager.RegisterStartupScript(this.Page,this.GetType(), "showMsg", string.Format(@"jsAlert('{0}');", string.Format(ex.Message, user.UserLogin)), true);
                lblprofilemsg.Text = ex.Message;
                return;
            }
            catch (Exception ex)
            {
                //Script register to show exception info
                ScriptManager.RegisterStartupScript(this.Page,this.GetType(), "showMsg", @"jsError('Lo sentimos pero ha ocurrido un error inexperado');", true);

                lblprofilemsg.Text = "Lo sentimos pero ha ocurrido un error inexperado";

                Utilities.LogException(Path.GetFileName(Request.Path),
                            MethodInfo.GetCurrentMethod().Name,
                            ex);
                return;
            }
        }

        private bool _validateRulesForm(ref StringBuilder msg)
        {
            if (string.IsNullOrEmpty(rdbRuleType.SelectedValue))
            {
                msg.Append("Por favor, seleccione un tipo");
            }

            if (chkLinked.Items.Cast<ListItem>().Where(n => n.Selected).Count() == 0)
            {
                msg.Append("Por favor, seleccione los usuarios sobre los que generar la regla");
            }

            if (drpRule.SelectedValue == "-1" && string.IsNullOrEmpty(txtRuleName.Text))
            {
                msg.Append("Por favor, seleccione los usuarios sobre los que generar la regla");
            }

            if (msg.Length > 0)
                return false;

            msg.AppendLine("Generación/Modificación de regla:");
            msg.AppendLine(string.Format("Regla:{0}", drpRule.SelectedValue));

            return true;
        }

        protected void drpRule_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (drpRule.SelectedValue != "-1")
            {

				Classes.Privacy privacy = new Classes.Privacy();
                Rule rule = new Rule() { RuleID = Decimal.Parse(drpRule.SelectedValue) };
                if (GlobalBS.GetRuleInfo(ref rule, ref privacy))
                {
                    txtRuleName.Text = rule.RuleName;
                    txtRuleDesc.Text = rule.RuleDesc;
                    rdbRuleType.SelectedValue = ((int)rule.RuleTypeID).ToString();
                    
                    privacy.Users.ForEach(delegate(string s)
                    {
                        chkLinked.Items.FindByValue(s).Selected = true;
                    });

                    chkLinked_SelectedIndexChanged(sender, e);                    
                    
                }
            }
        }

        protected void lnkCheckAll_Click(object sender, EventArgs e)
        {

            foreach (ListItem li in chkLinked.Items)
            {
                li.Selected = true;
            }      
        }

        protected void lnkUnCheckAll_Click(object sender, EventArgs e)
        {

            foreach (ListItem li in chkLinked.Items)
            {
                li.Selected = false;
            }   
        }

    }
}