using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using System.Reflection;
using System.Threading;

using System.IO;
using System.Net.Mail;
using CSM.Classes;
using CMS.DataManager;

namespace CSM
{
    public partial class Register : System.Web.UI.Page
    {
        /// <summary>
        /// Attribute that contais the CSS-class attached to message reported
        /// </summary>
        public string registerStatus;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Changes master CSS-class
			this.Master.TextRegister = "register";
        }

        /// <summary>
        /// Event handler of send register form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            StringBuilder msg = new StringBuilder("");
            if (validateForm(ref msg))
            {
                User user = new User(){ 
                    UserAddress = "",
                    UserBirth = DateTime.Parse(birthdateinput.Text),
                    UserEmail = emailinput.Text,
                    UserLogin = nickinput.Text,
                    UserName = nameinput.Text,
                    UserPass = Utilities.EncodeMD5(passinput.Text),
                    UserSurname = surnameinput.Text
                };

                try
                {
                    if (RegisterFormBS.RegisterUser(user))
                    {
                        registerStatus = "completed";
                        lblMsg.Text = "Su registro ha sido completado con éxito. Revise su correo para comprobar sus datos";

                    }
                    else
                    {
                        registerStatus = "exception";
                        lblMsg.Text = "Ha ocurrido un error al procesar su petición de registro. Por favor pruebe más tarde";
                        Utilities.LogException(Path.GetFileName(Request.Path),
                            MethodInfo.GetCurrentMethod().Name,
                            new Exception(string.Concat(msg.ToString(), "No ha sido posible completar el registro")));
                    }
                }
                catch (WrongDataException ex)
                {
                    registerStatus = "exception";
                    lblMsg.Text = ex.Message;
                }
                catch (SmtpException ex)
                {
                    registerStatus = "exception";
                    lblMsg.Text = "Su petición se registró correctamente pero no hemos podido enviar el email de confirmación. Por favor, póngase en contacto con nosotros.";
                    Utilities.LogException(Path.GetFileName(Request.Path),
                            MethodInfo.GetCurrentMethod().Name,
                            ex);
                }
                catch (Exception ex)
                {
                    registerStatus = "exception";
                    lblMsg.Text = "Ha ocurrido un error al procesar su petición de registro. Por favor pruebe más tarde";
                    Utilities.LogException(Path.GetFileName(Request.Path),
                            MethodInfo.GetCurrentMethod().Name,
                            ex);
                }
                

            }
            else
            {
                registerStatus = "exception";
                lblMsg.Text = msg.ToString();
            }

            //Script register to restore button functionality and show any issue or message
			ClientScript.RegisterStartupScript(this.GetType(), "showMsg", @"$('.registerform .msg').show();$('.preventDouble').enableSubmit();");
        }

        /// <summary>
        /// Method to validate register form
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool validateForm(ref StringBuilder msg)
        {
            if (nameinput.Text == string.Empty ||
               nameinput.Text == "Nombre" ||
                surnameinput.Text == string.Empty ||
                surnameinput.Text == "Apellidos")
            {
                msg.Append("<p>Por favor, rellene su nombre</p>");
            }

            DateTime tmp;
            if (birthdateinput.Text == string.Empty ||
                !DateTime.TryParse(birthdateinput.Text,out tmp))
            {
                msg.Append("<p>Por favor, introduce una fecha de nacimiento correcta</p>");
            }

            if (nickinput.Text == string.Empty ||
                nickinput.Text == "Usuario")
            {
                msg.Append("<p>Por favor, introduce un usuario correcto</p>");
            }

            if (passinput.Text == string.Empty ||
                passinput.Text == "Contraseña" ||
                reppassinput.Text != passinput.Text )
            {
                msg.Append("<p>Por favor, introduce una contraseña correcta</p>");
                if (reppassinput.Text != passinput.Text) msg.Append("Las contraseñas no coinciden</p>");
            }

            if (emailinput.Text == string.Empty ||
                emailinput.Text == "correo@social" ||
                !Utilities.checkEmail(emailinput.Text) ||
                emailinput.Text != repemailinput.Text)
            {
                msg.Append("<p>Por favor, introduce un email correcto</p>");
                if (emailinput.Text != repemailinput.Text) msg.Append("<p>Los emails no coinciden</p>");
            }

            if (!chkPrivacy.Checked)
            {
                msg.Append("<p>Debe aceptar las condiciones de privacidad</p>");
            }

            if (msg.Length > 0)
                return false;

            msg.AppendLine("Petición de Registro:");
            msg.AppendLine(string.Format("Nombre:{0}", nameinput.Text));
            msg.AppendLine(string.Format("Apellidos:{0}", surnameinput.Text));
            msg.AppendLine(string.Format("Usuario:{0}", nickinput.Text));
            msg.AppendLine(string.Format("Email:{0}\n", emailinput.Text));
            
            return true;
        }


    }
}