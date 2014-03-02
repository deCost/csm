using System;
using CSM.Classes;
using CSM.DataLayer;
using System.Reflection;
using System.Configuration;
using System.Text;
using System.IO;

namespace CSM.DataManager
{
	public class DefaultBS
	{
		/// <summary>
		/// Method to do login.
		/// </summary>
		/// <param name="user">Object with info to login. Once logged, it return user info</param>
		/// <returns>Method goes fine</returns>
		public static bool ProcessLoginForm(User user)
		{
			bool ok = true;

			if (!DefaultDL.ProcessLoginForm(ref user))
			{
				ok = false;
			}

			// Updating status on DB
			try
			{

				if (!GlobalDL.UpdateUserStatus(user))
					throw new Exception(string.Format(@"Ha ocurrido un error al actualizar 
                                                                    el estado del usuario en la BBDD: 
                                                                    UserId: {0}, Status: {1}",
						user.UserID,
						(int)user.StatuID));
			}
			catch (Exception e)
			{
				Utilities.LogException("DefaultBS", MethodInfo.GetCurrentMethod().Name, e);
			}

			return ok;
		}

		/// <summary>
		/// Method to create a new pass and it to the user
		/// </summary>
		/// <param name="user">Object with info to login. Once logged, it return user info</param>
		/// <returns>Method goes fine</returns>
		public static bool ChangePass(User user, string newpass)
		{
			bool ok = true;

			if (DefaultDL.ChangePass(ref user))
			{

				try
				{
					Utilities.SendMail(ConfigurationManager.AppSettings["noReply"],
						user.UserEmail,
						"Social Me! - Recordatorio de contraseña",
						getBody(user, newpass),
						true,
						ConfigurationManager.AppSettings["smtpServer"],
						null,
						false,
						new string[] { ConfigurationManager.AppSettings["smtpUser"], ConfigurationManager.AppSettings["smtpPass"] },
						null,
						null);
				}
				catch (Exception ex)
				{
					Utilities.LogException("DefaultBS",
						MethodInfo.GetCurrentMethod().Name,
						ex);
					throw new WrongDataException(@"Su contraseña ha sido modificada
                                            pero no se ha podido enviar el email con los datos. Por favor, pongase en contacto 
                                            con nosotros si tiene algún problema para acceder.");
				}

			}
			else
			{
				ok = false;
			}

			return ok;
		}

		private static string getBody(User user, string newpass)
		{
			StringBuilder body = new StringBuilder(File.ReadAllText(Utilities.GetEmailTemplatePath(EmailTemplateType.NEWPASS)));

			body.Replace("#Login#", user.UserLogin);
			body.Replace("#Pass#", newpass);

			return body.ToString();
		}
	}
}

