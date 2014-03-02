using CSM.Classes;
using CSM.Master;


namespace CSM
{
	using System;
	using System.Web;
	using System.Web.UI;

	public partial class Home : System.Web.UI.Page
	{

		protected void Page_Load(object sender, EventArgs e)
		{

			Private privateFunctions = new Private ();
			User user = null;

			if (!privateFunctions.isLoggedSession (ref user)) {
				Response.Redirect ("~/");
			}
			hdnuserid.Value = user.UserID.ToString();
			listBubbles.ProfileUser = profile.ProfileUser = user;
			listBubbles.isMyProfile = profile.isMyProfile = true;
			listBubbles.canComment = true;

		}
	}
}

