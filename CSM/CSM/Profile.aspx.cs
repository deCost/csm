using CSM.Classes;
using CSM.Master;
using CMS.DataManager;
using System.Reflection;
using System.IO;


namespace CSM
{
	using System;
	using System.Web;
	using System.Web.UI;

	public partial class Profile : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

			Decimal userid = 0;

			if (!string.IsNullOrEmpty(Request["user"]) && Decimal.TryParse(Request["user"], out userid))
			{
				try
				{

					User user = new User()
					{
						UserID = userid
					};

					// Get user information
					if (!GlobalBS.GetUser(ref user))
					{
						throw new WrongDataException("Lo sentimos pero ocurrió un error al recuperar el usuario");
					}

					listBubbles.ProfileUser = profile.ProfileUser = user;
					listBubbles.isMyProfile = profile.isMyProfile = false;
					tools.UserTo = userid;

				}
				catch (WrongDataException ex)
				{
					//Script register to show exception info
					ClientScript.RegisterStartupScript(this.GetType(), "showMsg", string.Format(@"jsError('{0}');", ex.Message));
					return;
				}
				catch (Exception ex)
				{
					//Script register to show exception info
					ClientScript.RegisterStartupScript(this.GetType(), "showMsg", @"jsError('Lo sentimos pero ha ocurrido un error inexperado');");
					Utilities.LogException(Path.GetFileName(Request.Path),
						MethodInfo.GetCurrentMethod().Name,
						ex);
					return;
				}
			}
		}
	}
}

