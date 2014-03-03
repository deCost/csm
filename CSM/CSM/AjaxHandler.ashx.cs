using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using System.Reflection;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.SessionState;

using System.IO;
using CSM.Classes;
using CSM.Master;
using CMS.DataManager;
using CSM;
using CSM.DataManager;

namespace TFC.NET
{
    /// <summary>
    /// Descripción breve de AjaxHandler
    /// </summary>
    public class AjaxHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            switch (context.Request["fn"])
            {
                case "ContactForm":
                    _ProcessContactForm(context);                    
                    break;
                case "ResizeImage":
                    _ResizeImage(context);
                    break;
                case "ResizeRatio":
                    _ResizeRatio(context);
                    break;
                case "ChangePass":
                    _ChangePass(context);
                    break;
                case "NewComment":
                    _NewComment(context);
                    break;
                case "ImagePreview":
                    _ImagePreview(context);
                    break;
                case "UpdateLinkStatus":
                    _UpdateLinkStatus(context);
                    break;
                case "FinishSchedule":
                    _FinishSchedule(context);
                    break;
                case "DeleteImage":
                    _DeleteImage(context);
                    break;
                default:
                    context.Response.Write("0: No se ha seleccionado una función correcta");
                    context.Response.End();
                    break;

            }
        }

        private void _DeleteImage(HttpContext context)
        {
            Decimal picid = 0, albumid = 0;
            Private privateFunctions = new Private();
            User user = null;

            if (privateFunctions.isLoggedSession(ref user) &&
                !string.IsNullOrEmpty(context.Request["picid"]) && Decimal.TryParse(context.Request["picid"], out picid) &&
                !string.IsNullOrEmpty(context.Request["albumid"]) && Decimal.TryParse(context.Request["albumid"], out albumid))
            {
                Picture pic = new Picture() { PicID = picid, AlbumID = albumid };

                if (GlobalBS.DeleteImage(ref pic))
                {

                    try{
                        string dirbase = AppDomain.CurrentDomain.BaseDirectory;
                        File.Delete(string.Format("{0}\\userData\\{1}", dirbase, pic.PicPath.Replace("/", "\\")));

                    }
                    catch(Exception ex)
                    {
                        Utilities.LogException("AjaxHandler",
                            MethodInfo.GetCurrentMethod().Name,
                            ex);
                    }
                    context.Response.Write("1: Su imagen ha sido eliminada correctamente");
                }
                else
                {
                    context.Response.Write("0: Ocurrió un error al procesar su solicitud");
                }

            }
            else
            {
                context.Response.Write("0: Parámetros incorrectos");
            }
        }

        private void _FinishSchedule(HttpContext context)
        {
            Decimal schedid = 0;
            Private privateFunctions = new Private();
            User user = null;

            if (privateFunctions.isLoggedSession(ref user) &&
                !string.IsNullOrEmpty(context.Request["schedid"]) && Decimal.TryParse(context.Request["schedid"], out schedid))
            {
                if (GlobalBS.FinishSchedule(new Schedule() { SchedID = schedid }, user))
                {
                    context.Response.Write("1: Su tarea/evento ha sido finalizado");
                }

            }
            else
            {
                context.Response.Write("0: Parámetros incorrectos");
            }
        }

        private void _ImagePreview(HttpContext context)
        {
            int height = 0, width = 0;

            if (!string.IsNullOrEmpty(context.Request["sessiontype"]) &&
                !string.IsNullOrEmpty(context.Request["sessionid"]) &&
                ((!string.IsNullOrEmpty(context.Request["h"]) && !string.IsNullOrEmpty(context.Request["w"]) && int.TryParse(context.Request["h"], out height) && int.TryParse(context.Request["w"], out width)) || (!string.IsNullOrEmpty(context.Request["size"]))))
            {

                string session = string.Format("{0}_{1}",context.Request["sessionid"],context.Request["sessiontype"]);
                try{
                    Picture pic = (Picture) context.Session[session];
                    
                    string dirbase = AppDomain.CurrentDomain.BaseDirectory;
                    Image FullsizeImage = Image.FromFile(string.Format("{0}\\userData\\{1}", dirbase, pic.PicPath.Replace("/", "\\")));

                    Image NewImage = null;

                    if (width > 0 && height > 0)
                    {

                        NewImage = Utilities.ResizeImage(FullsizeImage, new Size(width, height));
                    }
                    else
                    {
                        Utilities.GetsImageSizeByName(context.Request["size"], ref height, ref width);
                        
                        NewImage = Utilities.ResizeImage(FullsizeImage, new Size(width, height));

                        // Save resized picture
                        context.Response.ContentType = "image/jpeg";
                        NewImage.Save(context.Response.OutputStream, ImageFormat.Jpeg);

                    }

                }catch(Exception e){
                    Utilities.LogException("AjaxHandler.ashx",
                           MethodInfo.GetCurrentMethod().Name,e);
                }

            }
        }

        

        #region Link Status

        private void _UpdateLinkStatus(HttpContext context)
        {
            Decimal userid = 0, userto = 0;
            int status = 0;

            if (!string.IsNullOrEmpty(context.Request["userid"]) && Decimal.TryParse(context.Request["userid"], out userid) &&
                !string.IsNullOrEmpty(context.Request["userto"]) && Decimal.TryParse(context.Request["userto"], out userto) &&
                !string.IsNullOrEmpty(context.Request["status"]) && int.TryParse(context.Request["status"], out status))
            {
                if (status == (int)Status.Pending)
                {

                    if (GlobalBS.InsertNewLinkRequest(new User() { UserID = userid }, new User() { UserID = userto }))
                    {
                        context.Response.Write("1: Su petición se ha registrado correctamente");
                    }
                }
                else if (status == (int)Status.Active)
                {
                    if (GlobalBS.UpdateLinkRequest(new User() { UserID = userto }, new User() { UserID = userid }))
                    {
                        context.Response.Write("1: Se ha añadido al usuario correctamente");
                    }
                }
                else if(status == -1)
                {
                    if (GlobalBS.DeleteLinkRequest(new User() { UserID = userid }, new User() { UserID = userto }))
                    {
                        context.Response.Write("1: Se ha eliminado al contacto correctamente de sus usuarios enlazados");
                    }
                }

            }
            else
            {
                context.Response.Write("0: Parámetros incorrectos");
            }
        }

        #endregion

        #region Comments

        private void _NewComment(HttpContext context)
        {

            Decimal userid = 0, ruleid = 0, publid =0;

            if (!string.IsNullOrEmpty(context.Request["userid"]) && Decimal.TryParse(context.Request["ruleid"], out userid) &&
                !string.IsNullOrEmpty(context.Request["ruleid"]) && Decimal.TryParse(context.Request["ruleid"], out ruleid) &&
                !string.IsNullOrEmpty(context.Request["publid"]) && Decimal.TryParse(context.Request["publid"], out publid) &&
                !string.IsNullOrEmpty(context.Request["comment"]))
            {
                StringBuilder msg = new StringBuilder();

				if (_validateCommentForm(new Publication() { PublDesc = HttpUtility.UrlDecode(context.Request["comment"]) }, ref msg))
                {

                    if (GlobalBS.InsertNewPublication(new User() { UserID = Decimal.Parse(context.Request["userid"]) },
                        null,
                        new Rule() { RuleID = Decimal.Parse(context.Request["ruleid"]) },
                        publid,
                        -1,
                        context.Request["comment"]))
                    {
                        context.Response.Write("1: Su comentario se ha registrado correctamente");
                    }

                }
                else
                {
                    context.Response.Write(string.Format("0:{0}", msg));
                }
            }
            else
            {
                context.Response.Write("0: Parámetros incorrectos");
            }
        }


        private bool _validateCommentForm(Publication pub, ref StringBuilder msg)
        {
            if (pub.PublDesc == string.Empty)
            {
                msg.Append("Por favor, introduce un mensaje");
            }

            if (pub.PublDesc == string.Empty)
            {
                msg.Append("Por favor, introduce un mensaje");
            }

            if (msg.Length > 0)
                return false;

            return true;
        }
#endregion

        #region Crop image

        

        #endregion

        #region Resize image

        private void _ResizeRatio(HttpContext context)
        {
            int height = 0, width = 0;
            
            if (!string.IsNullOrEmpty(context.Request["src"]) &&
                ((!string.IsNullOrEmpty(context.Request["h"]) && !string.IsNullOrEmpty(context.Request["w"]) && int.TryParse(context.Request["h"], out height) && int.TryParse(context.Request["w"], out width)) || (!string.IsNullOrEmpty(context.Request["size"]))))
            {
				string dirbase = AppDomain.CurrentDomain.BaseDirectory;
				dirbase = dirbase.Remove (dirbase.Length - 1, 1);
				Image FullsizeImage = Image.FromFile(string.Concat(dirbase, context.Request["src"]));

                Image NewImage = null;

                if (width > 0 && height > 0)
                {
                    Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbCallback);

                    NewImage = FullsizeImage.GetThumbnailImage(width, height, myCallback, IntPtr.Zero);
                    //NewImage = Utilities.ResizeImage(FullsizeImage, new Size(width, height));
                }
                else
                {
                    Utilities.GetsImageSizeByName(context.Request["size"], ref height, ref width);
                    
                    NewImage = Utilities.ResizeImage(FullsizeImage, new Size(width, height));
                }
                
                // Save resized picture
                context.Response.ContentType = "image/jpeg";
                NewImage.Save(context.Response.OutputStream, ImageFormat.Jpeg);
            }
        }


        public bool ThumbCallback()
        {
            return false;
        }

        /// <summary>
        /// Resizes an image to desire size
        /// </summary>
        /// <param name="context"></param>
        private void _ResizeImage(HttpContext context)
        {
            int height = 0, width = 0;

            if(!string.IsNullOrEmpty(context.Request["src"]) &&
                ((!string.IsNullOrEmpty(context.Request["h"]) && !string.IsNullOrEmpty(context.Request["w"]) && int.TryParse(context.Request["h"], out height) && int.TryParse(context.Request["w"], out width)) || (!string.IsNullOrEmpty(context.Request["size"]))))
            {
                string dirbase = AppDomain.CurrentDomain.BaseDirectory;
                Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);

                Image original = Image.FromFile(string.Format("{0}\\{1}", dirbase, context.Request["src"].Replace("/", "\\")));
                Image thumb;


                if (width > 0 && height > 0)
                {

                    thumb = original.GetThumbnailImage(width, height, myCallback, IntPtr.Zero);
                }
                else
                {
                    height = 75;
                    width = 100;

                    if (original.Width < original.Height)
                    {
                        width = 80;
                        height = 100;
                    }
                    

                    thumb = original.GetThumbnailImage(width, height, myCallback, IntPtr.Zero);
                }

                context.Response.ContentType = "image/jpeg";
                thumb.Save(context.Response.OutputStream, ImageFormat.Jpeg);

            }
        }

        /// <summary>
        /// Event handler for image resize
        /// </summary>
        /// <returns></returns>
        public bool ThumbnailCallback()
        {
            return false;
        }

        #endregion

        #region ChangePass

        private void _ChangePass(HttpContext context)
        {
            StringBuilder msg = new StringBuilder("");
            string response;
            User usr = null;
            try
            {
                if (!(string.IsNullOrEmpty(context.Request["login"]) ||
                    string.IsNullOrEmpty(context.Request["email"])))
                {

                    usr = new User()
                    {
                        UserLogin = context.Request["login"],
                        UserEmail = context.Request["email"]
                    };
                }
                else
                {
                    throw new Exception(string.Format("Los parámetros introducidos no son correctos. QueryString: {0}",
                        context.Request.QueryString));
                }
            }
            catch (Exception ex)
            {
                response = "Se ha producido un error al procesar los datos introducidos";
                Utilities.LogException("AjaxHandler.ashx", MethodInfo.GetCurrentMethod().Name, ex);
                context.Response.Write(string.Format("0: {0}", response));
            }

            string newpass;

            if (_validateRememberForm(usr, ref msg))
            {
                newpass = Utilities.EncodeMD5(Guid.NewGuid().ToString()).Substring(0, 8);
                usr.UserPass = Utilities.EncodeMD5(newpass);
                try
                {
                    if (DefaultBS.ChangePass(usr, newpass))
                    {
                        response = @"Su contraseña ha sido modificada. 
                                                Por favor, revise su correo para comprobar la información.";
                        context.Response.Write(string.Format("1: {0}", response));
                    }
                    else
                    {
                        response = @"Se ha producido un error al procesar su petición de cambio de contraseña. 
                                    Por favor, vuelva a probarlo un poco más tarde.";
                        Utilities.LogException("AjaxHandler.ashx",
                            MethodInfo.GetCurrentMethod().Name,
                            new Exception(string.Concat(string.Format(msg.ToString(),newpass), "No ha sido posible completar el cambio de contraseña")));
                        context.Response.Write(string.Format("0: {0}", response));
                    }
                }
                catch (WrongDataException e)
                {
                    Utilities.LogException("AjaxHandler.ashx",
                        MethodInfo.GetCurrentMethod().Name,
                        new Exception(string.Concat(string.Format(msg.ToString(), newpass), "No ha sido posible completar el envío de email")));
                    context.Response.Write(string.Format("0: {0}", e.Message));
                }
            }
            else
            {
                response = msg.ToString();
                context.Response.Write(string.Format("0: {0}", response));
            }
        }

        private bool _validateRememberForm(User usr, ref StringBuilder msg)
        {
            if (usr.UserLogin == string.Empty ||
               usr.UserLogin == "Introduce tu usuario")
            {
                msg.Append("<p>Por favor, introduzca un mensaje</p>");
            }

            if (usr.UserEmail == string.Empty ||
               usr.UserEmail == "Introduce tu correo registrado" ||
               !Utilities.checkEmail(usr.UserEmail))
            {
                msg.Append("<p>Por favor, introduzca una dirección de correo</p>");
            }

            if (msg.Length > 0)
                return false;

            msg.AppendLine("Modificación de contraseña:");
            msg.AppendLine(string.Format("Login:{0}", usr.UserLogin));
            msg.AppendLine(string.Format("Email:{0}", usr.UserEmail));
            msg.AppendLine("Contraseña:{0}");

            return true;
        }

        #endregion

        #region Contact Form

        private void _ProcessContactForm(HttpContext context)
        {
            StringBuilder msg = new StringBuilder("");
            string response;
            ContactForm ctc = null;
            try
            {
                if(!(string.IsNullOrEmpty(context.Request["subj"]) ||
                    string.IsNullOrEmpty(context.Request["email"]) ||
                    string.IsNullOrEmpty(context.Request["msg"]) ||
                    string.IsNullOrEmpty(context.Request["name"])))
                {

                    ctc = new ContactForm()
                    {
                        Asunto = context.Request["subj"],
                        Email = context.Request["email"],
                        Mensaje = context.Request["msg"],
                        Nombre = context.Request["name"]
                    };
                }
                else
                {
                    throw new Exception(string.Format("Los parámetros introducidos no son correctos. QueryString: {0}",
                        context.Request.QueryString));
                }
            }
            catch (Exception ex)
            {
                response = "Se ha producido un error al procesar los datos introducidos";
                Utilities.LogException("AjaxHandler.ashx", MethodInfo.GetCurrentMethod().Name, ex);
                context.Response.Write(string.Format("0: {0}",response));
            }

            if (_validateContactForm(ctc, ref msg))
            {
                if (GlobalBS.ProcessContactForm(ctc))
                {
                    response = "El formulario de contacto ha sido procesado. En breve nos pondremos en contacto con usted";
                    context.Response.Write(string.Format("1: {0}", response));
                }
                else
                {
                    response = "Se ha producido un error al procesar su petición de contacto. Por favor, vuelva a probarlo un poco más tarde.";
                    Utilities.LogException("AjaxHandler.ashx",
                        MethodInfo.GetCurrentMethod().Name,
                        new Exception(string.Concat(msg.ToString(), "No ha sido posible completar el formulario de contacto")));
                    context.Response.Write(string.Format("0: {0}",response));
                }
            }
            else
            {
                response = msg.ToString();
                context.Response.Write(string.Format("0: {0}",response));
            }
        }

        /// <summary>
        /// Method to validate contact form
        /// </summary>
        /// <param name="ctc"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool _validateContactForm(ContactForm ctc, ref StringBuilder msg)
        {
            if (ctc.Nombre == string.Empty ||
               ctc.Nombre == "Nombre de contacto")
            {
                msg.Append("<p>Por favor, rellene su nombre</p>");
            }

            if (ctc.Asunto == string.Empty ||
               ctc.Asunto == "Asunto")
            {
                msg.Append("<p>Por favor, introduzca el asunto</p>");
            }

            if (ctc.Email == string.Empty ||
               ctc.Email == "Email de contacto" ||
               !Utilities.checkEmail(ctc.Email))
            {
                msg.Append("<p>Por favor, introduzca un email válido</p>");
            }

            if (ctc.Mensaje == string.Empty ||
               ctc.Mensaje == "Mensaje")
            {
                msg.Append("<p>Por favor, introduzca un mensaje</p>");
            }

            if (ctc.Mensaje.Length > 255)
            {
                msg.Append("<p>El mensaje introducido supera el tamaño máximo permitido (255 caracteres)</p>");
            }

            if (msg.Length > 0)
                return false;

            msg.AppendLine("Formulario de Contacto:");
            msg.AppendLine(string.Format("Nombre:{0}", ctc.Nombre));
            msg.AppendLine(string.Format("Asunto:{0}", ctc.Asunto));
            msg.AppendLine(string.Format("Mensaje:{0}", ctc.Mensaje));
            msg.AppendLine(string.Format("Email:{0}", ctc.Email));

            return true;
        }

        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}