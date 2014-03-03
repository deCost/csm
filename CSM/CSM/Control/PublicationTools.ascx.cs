using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Reflection;
using CSM.Master;
using CSM.Classes;
using CMS.DataManager;

namespace CSM.Control
{
    public partial class PublicationTools : System.Web.UI.UserControl
    {
        Decimal _userTo;

        /// <summary>
        /// Defines a publication into another profile
        /// </summary>
        public Decimal UserTo
        {

            get
            {
                // look for current page in ViewState
                object o = this.ViewState["_PubUser"];
                if (o == null)
                {

                    if (string.IsNullOrEmpty(Request["user"]) || !Decimal.TryParse(Request["user"], out _userTo))
                    {
                        return -1;
                    }
                    else
                    {
                        this.ViewState["_PubUser"] = _userTo;
                        return _userTo;
                    }
                }
                else
                {
                    return (Decimal)o;
                }
            }

            set
            {
                this.ViewState["_PubUser"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Private privateFunctions = new Private();
                User user = new User();
                Session.Remove(user.SessionID + "_pubimg");
                try
                {

                    privateFunctions.isLoggedSession(ref user);
                    List<Rule> ruleList = new List<Rule>();

                    if (GlobalBS.GetRulesFromUser(user, ruleList))
                    {
                        ruleList.RemoveAll(delegate(Rule r) { return r.RuleTypeID == RuleType.Images; });
                        ruleList.Add(new Rule() { RuleName = "Público", RuleID = -1 });
                        drpRule.DataSource = ruleList;
                        drpRule.DataTextField = "RuleName";
                        drpRule.DataValueField = "RuleID";
                        drpRule.DataBind();
                    }
                }
                catch (WrongDataException ex)
                {
                    //Script register to show exception info
					ScriptManager.RegisterStartupScript(this, this.GetType(), "showMsg", string.Format(@"jsAlert('{0}');", ex.Message), true);
                    responseTxt.Text = ex.Message;
                    return;
                }
                catch (Exception ex)
                {
                    //Script register to show exception info
					ScriptManager.RegisterStartupScript(this, this.GetType(), "showMsg", @"jsError('Lo sentimos pero ha ocurrido un error inexperado');", true);

                    responseTxt.Text = "Lo sentimos pero ha ocurrido un error inexperado";

                    Utilities.LogException(Path.GetFileName(Request.Path),
                                MethodInfo.GetCurrentMethod().Name,
                                ex);
                    return;
                }
            }
        }

        /// <summary>
        /// Event handler for publication button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPublication_Click(object sender, EventArgs e)
        {

            Private privateFunctions = new Private();
            User user = new User();
            try
            {

                privateFunctions.isLoggedSession(ref user);

                string msg = Utilities.StripHtml(txtpublication.Text, true);
                Picture pic = null;
                if (Session[user.SessionID + "_pubimg"] != null)
                {
                    pic = (Picture)Session[user.SessionID + "_pubimg"];

					//pic.PicDesc = txtimage.Text;
                }
                if (!string.IsNullOrEmpty(msg))
                {

                    Rule rule = new Rule()
                    {
                        RuleID = Decimal.Parse(drpRule.SelectedValue),
                        UserID = user.UserID
                    };

                    if (GlobalBS.InsertNewPublication(user, pic, rule, -1, UserTo, msg))
                    {
						//txtimage.Text = "";
                        txtpublication.Text = "";

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "hideFancybox", @"setTimeout(function(){$.fancybox.close();document.location.refresh();},5000)", true);
                        Session.Remove(user.SessionID + "_pubimg");

                        if (user.UserID != UserTo)
                        {
                            Context.Response.Redirect(Request.Url.ToString());
                        }
                        else
                        {
                            Context.Response.Redirect("Home.aspx");
                        }
                    }
                }
                else
                {
                    responseTxt.Text = "Por favor, introduzca un texto para su publicación";
                }
            }
            catch (WrongDataException ex)
            {
                //Script register to show exception info
				ScriptManager.RegisterStartupScript(this, this.GetType(), "showMsg", string.Format(@"jsAlert('{0}');", ex.Message), true);
                responseTxt.Text = ex.Message;
                return;
            }
            catch (Exception ex)
            {
                //Script register to show exception info
				ScriptManager.RegisterStartupScript(this, this.GetType(), "showMsg", @"jsError('Lo sentimos pero ha ocurrido un error inexperado');", true);
			
                responseTxt.Text = "Lo sentimos pero ha ocurrido un error inexperado";
                
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
        protected void AsyncFileUpload_UploadedComplete(Object sender, EventArgs e)
        {

            

            responseTxt.Text = "Imagen subida correctamente";
        }

        /// <summary>
        /// Event handler for file upload error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AsyncFileUpload_UploadedFileError(Object sender, EventArgs e) 
        {

            responseTxt.Text = "Lo sentimos pero ocurrió un error al procesar su imagen"; 
			Utilities.LogException("AjaxLoadingImage",MethodInfo.GetCurrentMethod().ToString(), new WrongDataException("Error Subida"));
            

        }

    }
}