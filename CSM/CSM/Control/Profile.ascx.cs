using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Reflection;
using System.Text;
using CSM.Classes;
using CSM.Master;
using CMS.DataManager;

namespace CSM.Control
{
    public partial class Profile : System.Web.UI.UserControl
    {
        private User _user;
        private bool _isMyProfile;
        private Status _linkStatus;
        
        /// <summary>
        /// Gets profile image from userprofile
        /// </summary>
        public string ProfileImage
        {
			get { return "images/costiProfile.jpg"; }//_user.ProfileImage == "" ? "/images/noimageprofile.jpg" : _user.ProfileImage; }
        }

        /// <summary>
        /// User linked control
        /// </summary>
        public Status LinkStatus
        {
            get { return _linkStatus; }
            set { _linkStatus = value; }
        }

        /// <summary>
        /// User control user
        /// </summary>
        public User ProfileUser
        {
            get { return _user; }
            set { _user = value; }
        }

        /// <summary>
        /// User control user
        /// </summary>
        public bool isMyProfile
        {
            get { return _isMyProfile; }
            set { _isMyProfile = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (_user != null)
            {
                // By using private.Master public mehods, we don't need to create a utilities class for website
                Private privateFunctions = new Private();
                int friendRequestsCount = 0;
                
                try
                {
                    if (!string.IsNullOrEmpty(_user.ProfileImage))
                    {

						profileimage.ImageUrl = _user.ProfileImage;//string.Format("~/AjaxHandler.ashx?fn=ResizeRatio&size=profile&src={0}", _user.ProfileImage);
                    }
                    else
                    {
                        profileimage.ImageUrl = "~/images/noimageprofile.jpg";
                    }

                    if (_isMyProfile)
                    {
                        List<UserLink> requestList = new List<UserLink>();

                        // Gets users with connection request pending
                        if (!GlobalBS.GetUsersLinkBy(ref _user, ref requestList, Status.Pending))
                        {
                            throw new WrongDataException("Lo sentimos pero ocurrió un error al recuperar las solicitudes de conexión");
                        }

                        // Recover number of user connections request
                        if (requestList.Count > 0)
                        {
                            friendRequestsCount = requestList.Count;
                        }

                        // Sets text for notification counter
						//txttotalnotifications.Text = (friendRequestsCount + messagesRequestCount).ToString();
						//txtfriendrequest.Text = "Amigos" + (friendRequestsCount > 0 ? string.Format(" ({0})", friendRequestsCount) : "");
                        //txtmessagesrequest.Text = "Mensajes" + (messagesRequestCount > 0 ? string.Format(" ({0})", messagesRequestCount) : "");
                        // Sets visible notification bubble just in case it needed to be
						//notification.Visible = friendRequestsCount + messagesRequestCount > 0;
						//connect.Visible = !(LinkStatus == Status.Pending || LinkStatus == Status.Active);

                    }
                    else
                    {
                        // Get user information
                        if (GlobalBS.GetUser(ref _user))
                        {
                            User user = new User();

                            if (privateFunctions.isLoggedSession(ref user))
                            {
                                if (user.UserID == _user.UserID)
                                {
                                    Response.Redirect("Home.aspx");
                                    return;
                                }

                                if (!GlobalBS.GetLinkStatus(user, _user, ref _linkStatus))
                                {
                                    throw new WrongDataException("Lo sentimos pero ocurrió un error al recuperar el usuario");
                                }
                            }

                            
                        }
                    }

                }
                catch (WrongDataException ex)
                {
                    //Script register to show exception info
					ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showMsg", string.Format(@"jsError('{0}');",ex.Message),true);
                    return;
                }
                catch (Exception ex)
                {
                    //Script register to show exception info
					ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showMsg", @"jsError('Lo sentimos pero ha ocurrido un error inexperado');", true);
                    Utilities.LogException(Path.GetFileName(Request.Path),
                                MethodInfo.GetCurrentMethod().Name,
                                ex);
                    return;
                }
            }


        }


        /// <summary>
        /// Event handler of send register form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLink_Click(object sender, EventArgs e)
        {
            try
            {
                Private privateFunctions = new Private();

                User user = new User();

                if (privateFunctions.isLoggedSession(ref user))
                {

                    if(!GlobalBS.InsertNewLinkRequest(user,this.ProfileUser))
                    {
                        throw new WrongDataException("No se ha podido completar la petición. Por favor, inténtelo más tarde");
                    }

                    string msg = "Su petición ha sido registrada con éxito. Cuando el usuario te acepte, te lo notificaremos.";
                    LinkStatus = Status.Pending;
                    //Script register to show exception info
					ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showMsg", "jsAlert('" + msg + "');$('.icon.noconnected').hide();", true);
                }
                else
                {
                    throw new WrongDataException("No se ha podido recuperar el usuario actual. Por favor, refresque la página");
                }
            }
            catch (WrongDataException ex)
            {
                //Script register to show exception info
				ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showMsg", string.Format(@"jsAlert('{0}');", ex.Message), true);
                return;
            }
            catch (Exception ex)
            {
                //Script register to show exception info
				//ClientScript.RegisterStartupScript(this.GetType(), "showMsg", @"jsError('Lo sentimos pero ha ocurrido un error inexperado');", true);
                Utilities.LogException(Path.GetFileName(Request.Path),
                            MethodInfo.GetCurrentMethod().Name,
                            ex);
                return;
            }
            
        }

    }
}