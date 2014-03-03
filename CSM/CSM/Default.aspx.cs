using System.Text;
using CSM.Classes;
using System.IO;
using System.Reflection;
using CSM.DataManager;


namespace CSM
{
	using System;
	using System.Web;
	using System.Web.UI;


	public partial class Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		/// <summary>
		/// EventHandler of login button click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void btnLogin_Click(object sender, EventArgs e)
		{
			StringBuilder msg = new StringBuilder("");
			if (validateForm(ref msg))
			{
				//Creates user object
				User user = new User()
				{
					UserLogin = txtUser.Text,
					UserPass = Utilities.EncodeMD5(txtPass.Text),
					StatuID = Status.Pending
				};

				try
				{
					// Try to login by using this data
					if (DefaultBS.ProcessLoginForm(user))
					{
						// Login active and sets into sessions keeper
						Global.sessionsTable.Add(user.SessionID, 
							user);
						//Saves on cookies the session
						Context.Response.Cookies.Add(new HttpCookie("socialMe") { 
							Expires = DateTime.Now.AddDays(1),
							HttpOnly = true,
							Value = user.SessionID,
							Name = "session"                        
						});

						// All goes fine and then can enter to application
						Response.Redirect("Home.aspx");
					}
					else
					{

						lblMsg.Text = "Ha ocurrido un error al procesar su petición de login. Por favor pruebe más tarde";
						Utilities.LogException(Path.GetFileName(Request.Path),
							MethodInfo.GetCurrentMethod().Name,
							new Exception(string.Concat(msg.ToString(), "No ha sido posible completar el login")));
					}
				}
				catch (WrongDataException ex)
				{
					lblMsg.Text = ex.Message;
				}

			}
			else
			{
				lblMsg.Text = msg.ToString();
			}


			//Script register to restore button functionality and show any issue or message
			ScriptManager.RegisterStartupScript(this.Page,this.GetType(), "showMsg", @"$('.loginform .msg').show();",true);
		}

		/// <summary>
		/// Method to validate login form
		/// </summary>
		/// <param name="msg"></param>
		/// <returns></returns>
		private bool validateForm(ref StringBuilder msg)
		{
			if (txtUser.Text == string.Empty ||
				txtUser.Text == "usuario")
			{
				msg.Append("<p>Por favor, rellene su nombre</p>");
			}

			if (txtPass.Text == string.Empty ||
				txtPass.Text == "contraseña")
			{
				msg.Append("<p>Por favor, introduce una contraseña correcta</p>");
			}

			if (msg.Length > 0)
				return false;

			msg.AppendLine("Petición de Login:");
			msg.AppendLine(string.Format("User:{0}", txtUser.Text));

			return true;
		}
	}
}

