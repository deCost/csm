using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Reflection;
using CSM.Classes;
using CSM.Master;
using CMS.DataManager;

namespace CSM.Control
{
    public partial class UserPicturesSlide : System.Web.UI.UserControl
    {
        private User _user;
        private bool _isMyProfile;

        public int CurrentPage
        {
            get
            {
                // look for current page in ViewState
                object o = this.ViewState["_CurrentPage"];
                if (o == null)
                    return 0; // default page index of 0
                else
                    return (int)o;
            }

            set
            {
                this.ViewState["_CurrentPage"] = value;
            }
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
                FillRepeater();
            }
        }

        private void FillRepeater()
        {
            // By using private.Master public mehods, we don't need to create a utilities class for website
            

            try
            {

                List<Album> latestPicturesList = new List<Album>();

                // Gets users with connection request pending
                if (!GlobalBS.GetLatestImages(_user, ref latestPicturesList))
                {
                    throw new WrongDataException("Lo sentimos pero ocurrió un error al recuperar las últimas imágenes del carrusel");
                }

                // Recover number of user connections request
                if (latestPicturesList.Count > 0)
                {
                    PagedDataSource pg = new PagedDataSource();

                    pg.DataSource = latestPicturesList;
                    pg.PageSize = 25;
                    pg.CurrentPageIndex = CurrentPage;

                    rptLatest.DataSource = pg;
                    rptLatest.DataBind();

                }
                else
                {
                    rptLatest.DataSource = null;
                    rptLatest.DataBind();
                }

            }
            catch (WrongDataException ex)
            {
                //Script register to show exception info
				//ScriptManager.RegisterStartupScript(this, this.GetType(), "showMsg", string.Format(@"jsError('{0}');", ex.Message), true);
            }
            catch (Exception ex)
            {
                //Script register to show exception info
				//ScriptManager.RegisterStartupScript(this, this.GetType(), "showMsg", @"jsError('Lo sentimos pero ha ocurrido un error inexperado');", true);
                Utilities.LogException(Path.GetFileName(Request.Path),
                            MethodInfo.GetCurrentMethod().Name,
                            ex);
            }
        }
    }
}