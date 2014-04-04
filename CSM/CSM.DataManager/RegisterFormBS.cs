using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using System.IO;
using CSM.Classes;
using CSM.DataLayer;
using CSM;

namespace CMS.DataManager
{
    public class RegisterFormBS
    {
        public static bool RegisterUser(User user)
        {
            bool ok = true;

            if (RegisterFormDL.InsertRegisterForm(user))
            {
                try
                {
                    Utilities.SendMail(ConfigurationManager.AppSettings["noReply"],
                        user.UserEmail,
                        "Social Me! - Bienvenido",
                        getBody(user),
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
                    Utilities.LogException("RegisterFormBS",
                            MethodInfo.GetCurrentMethod().Name,
                            ex);
                    throw new WrongDataException(@"Su petición se registró correctamente pero no hemos podido enviar 
                                                el email de confirmación. Por favor, póngase en contacto con nosotros.");
                }
            }
            else
            {
                ok = false;
            }

            return ok;
        }

        private static string getBody(User user)
        {
            StringBuilder body = new StringBuilder(File.ReadAllText(Utilities.GetEmailTemplatePath(EmailTemplateType.REGISTER)));

            body.Replace("#Nombre#", user.Name);
            body.Replace("#Email#", user.UserEmail);
            body.Replace("#Login#", user.UserLogin);
            
            return body.ToString();
        }
    }
}
