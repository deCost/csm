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
    public partial class UserBubblesList : System.Web.UI.UserControl
    {

        protected User _user;
        private bool _isMyProfile;
        
        public int CurrentPage
        {
            get
            {
                // look for current page in ViewState
                object o = this.ViewState["_CurrentPage"];
                if (o == null)
                    return 1; // default page index of 0
                else
                    return (int)o;
            }

            set
            {
                this.ViewState["_CurrentPage"] = value;
            }
        }

        public User CurrentUser
        {
            get
            {   
                Private privateFunctions = new Private();
                User user = new User();

                privateFunctions.isLoggedSession(ref user);
                return user;
            }
        }

        public int PageSize
        {
            get
            {
                // look for current page in ViewState
                object o = this.ViewState["_PageSize"];
                if (o == null)
                    return 20; // default page index of 0
                else
                    return (int)o;
            }

            set
            {
                this.ViewState["_PageSize"] = value;
            }
        }

		public bool canComment { get; set; }

        private bool _isMessages;

        public bool isMessages
        {
            get { return _isMessages; }
            set { _isMessages = value; }
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

        protected string ShowImageInPublication(Picture i)
        {
            StringBuilder str = new StringBuilder("");
            if(i != null)
            {

                str.Append("<div class=\"attach\">");
                str.Append("<a href=\"/userData/{0}\" title=\"{2}\" class=\"fancybox-thumb\" rel=\"fancybox-thumb\"><img alt=\"{1}\" src=\"\\userData\\{0}\" class=\"pic\" /></a>");
                str.Append("<span>{2}</span>");
                str.Append("</div>");

                return string.Format(str.ToString(), i.PicPath, i.PicName, i.PicDesc);
            }
            return string.Empty;
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (_user != null)
            {
                FillRepeater();
            }
        }

        /// <summary>
        /// Private method to fill paged repeater
        /// </summary>
        private void FillRepeater()
        {
            // By using private.Master public mehods, we don't need to create a utilities class for website
            Private privateFunctions = new Private();
            try
            {
                
                List<Publication> requestList = new List<Publication>();
                if (!_isMessages)
                {
                    User userFrom = null;

                    privateFunctions.isLoggedSession(ref userFrom);

                    /*******************************************************************
                     * Profile bubbles
                     *******************************************************************/
                    
                    // Gets users with connection request pending
                    if (!GlobalBS.GetUserPublications(_user, userFrom,  ref requestList))
                    {
                        throw new WrongDataException("Lo sentimos pero ocurrió un error al recuperar las publicaciones del usuario");
                    }

                    // Recover number of user connections request
                    if (requestList.Count > 0)
                    {
                        PagedDataSource pg = new PagedDataSource();

						pg.DataSource = requestList;
                        pg.PageSize = requestList.Count < PageSize ? PageSize : CurrentPage * PageSize > requestList.Count ? requestList.Count : CurrentPage * PageSize;
                        pg.AllowPaging = true;
                        pg.CurrentPageIndex = 0;

                        rptBubble.DataSource = pg;
                        rptBubble.DataBind();

                        btnViewMore.Visible = !pg.IsLastPage;
                        btnViewMore.Visible = !pg.IsFirstPage;

                        pnlViewMore.Visible = btnViewMore.Visible;

                    }
                    else
                    {
                        btnViewMore.Text = "No existen publicaciones del usuario visibles en estos momentos...";
                    }
                }
                else
                {
                    /*******************************************************************
                    * Message bubbles
                    *******************************************************************/
                    // Gets users with connection request pending
                    if (!GlobalBS.GetUserMessagesWith(CurrentUser, ProfileUser, ref requestList))
                    {
                        throw new WrongDataException("Lo sentimos pero ocurrió un error al recuperar las publicaciones del usuario");
                    }

                    // Recover number of user connections request
                    if (requestList.Count > 0)
                    {
                        PagedDataSource pg = new PagedDataSource();

                        pg.DataSource = requestList;
                        pg.PageSize = requestList.Count < PageSize ? PageSize : CurrentPage * PageSize > requestList.Count ? requestList.Count : CurrentPage * PageSize;
                        pg.AllowPaging = true;
                        pg.CurrentPageIndex = 0;

                        rptBubble.DataSource = pg;
                        rptBubble.DataBind();

                        btnViewMore.Visible = !pg.IsLastPage;
                        btnViewMore.Visible = !pg.IsFirstPage;
                        pnlViewMore.Visible = btnViewMore.Visible;

                    }
                    else
                    {
                        pnlViewMore.Visible = false;
                        btnViewMore.Text = "No existen mensajes con el usuario...";
                    }
                    
                }

            }
            catch (WrongDataException ex)
            {
                //Script register to show exception info
				ScriptManager.RegisterStartupScript(this, this.GetType(), "showMsg", string.Format("jsAlert('{0}');", ex.Message), true);
            }
            catch (Exception ex)
            {
                //Script register to show exception info
				ScriptManager.RegisterStartupScript(this, this.GetType(), "showMsg", "jsError('Lo sentimos pero ha ocurrido un error inexperado');", true);
                Utilities.LogException(Path.GetFileName(Request.Path),
                            MethodInfo.GetCurrentMethod().Name,
                            ex);
            }
        }

        /// <summary>
        /// Event handler for view more
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnViewMore_Click(object sender, EventArgs e)
        {
            // Set viewstate variable to the next page
            CurrentPage += 1;

            FillRepeater();

        }

        /// <summary>
        /// Event handler for view more
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnViewLess_Click(object sender, EventArgs e)
        {
            // Set viewstate variable to the next page
            CurrentPage -= 1;

            FillRepeater();

        }

        ///// <summary>
        ///// Event handler for file upload completed
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void AsyncCommentFileUpload_UploadedComplete(Object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        //{

        //    if (AsyncCommentFileUpload.HasFile)
        //    {
        //        try
        //        {

        //            Private privateFunctions = new Private();
        //            User user = new User();

        //            privateFunctions.isLoggedSession(ref user);

        //            Picture pic = null;
        //            HttpPostedFile file = AsyncCommentFileUpload.PostedFile;
        //            string imgmsg = "";
        //            pic = new Picture() { AlbumID = 0, PicDate = DateTime.Now, PicDesc = "" };
        //            if (!Utilities.UploadImageFromUser(file, ref user, ref pic, ref imgmsg))
        //            {
        //                //Script register to show exception info
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), "showMsg", string.Format(@"$(function(){jsAlert('{0}');});", imgmsg), true);
        //                return;
        //            }

        //            Session.Add(user.SessionID + "_commimg", pic);


        //        }
        //        catch (Exception ex)
        //        {
        //            Utilities.LogException(Path.GetFileName(Request.Path),
        //                MethodInfo.GetCurrentMethod().Name,
        //                ex);
                    
        //            string msg= "Ha ocurrido un error inexperado al intentar subir su imagen. Por favor, inténtelo más tarde.";
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "showMsg", string.Format(@"$(function(){jsError('{0}');});", msg), true);
        //        }
        //    }

        //}

        ///// <summary>
        ///// Event handler for file upload error
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void AsyncCommentFileUpload_UploadedFileError(Object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        //{

        //    string msg = "Lo sentimos pero ocurrió un error al procesar su imagen";
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "showMsg", string.Format(@"$(function(){jsAlert('{0}');});", msg), true);
        //    Utilities.LogException("AjaxLoadingImage", MethodInfo.GetCurrentMethod().ToString(), new WrongDataException(e.StatusMessage));


        //}

        
    }
}