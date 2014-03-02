using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Reflection;
using System.Drawing;
using System.Drawing.Drawing2D;
using CSM.Classes;
using System.Web;

namespace CSM
{
    public class Utilities
    {
        /// <summary>
        /// Method to validate an string as an email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool checkEmail(String email)
        {

            //regular expression pattern for valid email
            //addresses, allows for the following domains:
            //com,edu,info,gov,int,mil,net,org,biz,name,museum,coop,aero,pro,tv
            string pattern = @"^[-a-zA-Z0-9][-.a-zA-Z0-9]*@[-.a-zA-Z0-9]+(\.[-.a-zA-Z0-9]+)*\.
    (com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|tv|[a-zA-Z]{2})$";
            //Regular expression object
            Regex check = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);
            //boolean variable to return to calling method
            bool valid = false;

            //make sure an email address was provided
            if (string.IsNullOrEmpty(email))
            {
                valid = false;
            }
            else
            {
                //use IsMatch to validate the address
                valid = check.IsMatch(email);
            }
            //return the value to the calling method MailMessage class
            return valid;
        }

        /// <summary>
        /// Method to remove HTML tags from a text
        /// </summary>
        /// <param name="html"></param>
        /// <param name="allowHarmlessTags"></param>
        /// <returns></returns>
        public static string StripHtml(string html, bool allowHarmlessTags)
        {
            if (html == null || html == string.Empty)
                return string.Empty;

            if (allowHarmlessTags)
                return System.Text.RegularExpressions.Regex.Replace(html, "", string.Empty);

            return System.Text.RegularExpressions.Regex.Replace(html, "<[^>]*>", string.Empty);
        }

        /// <summary>
        /// Method to send an email by using 
        /// </summary>
        /// <param name="mailFrom"></param>
        /// <param name="mailTo"></param>
        /// <param name="mailSubject"></param>
        /// <param name="mailBody"></param>
        /// <param name="isBodyHtml"></param>
        /// <param name="smtpServer"></param>
        /// <param name="offerReportStream"></param>
        /// <param name="canSendAttachment"></param>
        /// <param name="credentials">[0] = username, [1] = password. </param>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public static void SendMail(string mailFrom, string mailTo, string mailSubject, string mailBody, bool isBodyHtml,
            string smtpServer, MemoryStream offerReportStream, bool canSendAttachment, 
            string[] credentials, string filename, string format)
        {
            MailMessage mailMessage = new MailMessage();
            try
            {
                mailMessage.From = new MailAddress(mailFrom);
                mailMessage.To.Add(mailTo);
                mailMessage.Subject = mailSubject;
                mailMessage.Body = mailBody;
                mailMessage.IsBodyHtml = isBodyHtml;
                if (canSendAttachment)
                {
                    Attachment attach = new Attachment(offerReportStream, filename, format);
                    mailMessage.Attachments.Add(attach);
                }
                SmtpClient smtpClient = new SmtpClient(smtpServer);

                if (smtpServer.Contains("gmail"))
                {
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                }

                //SMTP Authentication
                // Modification to work on localmachine
                if (credentials != null && credentials.Length == 2 && !string.IsNullOrEmpty(credentials[0]) )
                {

                    NetworkCredential smtpUserInfo =
                        new NetworkCredential(credentials[0],
                                              credentials[1]);

                    smtpClient.Credentials = smtpUserInfo;
                }
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Send(mailMessage);
            }
            catch (SmtpException smtpEx)
            {
                throw smtpEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to write into log
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logFolder"></param>
        /// <param name="logName"></param>
        public static void WriteLog(string message, string logFolder, string logName)
        {
            StreamWriter logWriter = null;
            try
            {
                string dir = AppDomain.CurrentDomain.BaseDirectory;
                logFolder = string.IsNullOrEmpty(logFolder) ? "" : logFolder ;

                if (dir.IndexOf("bin") > 0)
                {
                    dir = dir.Substring(0, dir.IndexOf("bin"));
                }
                //Search log folder or create ir
                dir = dir + logFolder;
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                // Create log name
                string fileName = string.Format("\\{0}_{1}.log", DateTime.Now.ToString("ddMMyyyy"), logName);
                fileName = string.Format(dir + fileName);
                logWriter = File.AppendText(fileName);
                logWriter.WriteLine(string.Format("{0}#{1}",DateTime.Now.ToString("dd/MM/yyyyy hh:mm"),message));
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                logWriter.Flush();
                logWriter.Close();
            }
        }

        /// <summary>
        /// Write exception to log file
        /// </summary>
        /// <param name="className">source page name</param>
        /// <param name="methodName">method name</param>
        /// <param name="exception">full exception</param>
        public static void LogException(string pageName, string methodName, Exception exception)
        {
            WriteLog(string.Format("Error occured from {0}[{1}] | Error message: {2} \n Stacktrace:", pageName, 
                methodName, exception.Message, exception.StackTrace), 
                "log", 
                "Exceptions");

        } 

        /// <summary>
        /// Checks if this column has same value
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        private static bool ColumnEqual(object A, object B)
        {

            // Compares two values to see if they are equal. Also compares DBNULL.Value.
            // Note: If your DataTable contains object fields, then you must extend this
            // function to handle them in a meaningful way if you intend to group on them.

            if (A == DBNull.Value && B == DBNull.Value) //  both are DBNull.Value
                return true;
            if (A == DBNull.Value || B == DBNull.Value) //  only one is DBNull.Value
                return false;
            return (A.Equals(B));  // value type standard comparison
        }

        /// <summary>
        /// Method to get hashed a string to MD5 encryption
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncodeMD5(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = Encoding.ASCII.GetBytes(str);
            data = md5.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < data.Length; i++)
                ret += data[i].ToString("x2").ToLower();
            return ret;
        }

        /// <summary>
        /// Gets email template path from enum
        /// </summary>
        /// <param name="emailTemplateType"></param>
        /// <returns></returns>
        public static string GetEmailTemplatePath(EmailTemplateType emailTemplateType)
        {
            string domainPath = AppDomain.CurrentDomain.BaseDirectory;
            domainPath = domainPath + "\\EmailTemplates";
            switch (emailTemplateType)
            {
                case EmailTemplateType.REGISTER:
                    domainPath = domainPath + "\\register.html";
                    break;
                case EmailTemplateType.CONTACT:
                    domainPath = domainPath + "\\contact.html";
                    break;
                case EmailTemplateType.NEWPASS:
                    domainPath = domainPath + "\\newpass.html";
                    break;
                default:
                    throw new Exception("Email template type not supported.");
            }
            return domainPath;
        }

        /// <summary>
        /// Method to get status from value
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static Status GetStatus(int i)
        {

            switch (i)
            {
                case -1:
                    return Status.All;
                case 0:
                    return Status.Pending;
                case 1:
                    return Status.Active;
                default:
                case 2:
                    return Status.Unlinked;
                case 3:
                    return Status.Logged;
                    
            }
        }

        /// <summary>
        /// Method to get status from value
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static ScheduleType GetScheduleType(int i)
        {

            switch (i)
            {
                default:
                case 0:
                    return ScheduleType.Task;
                case 1:
                    return ScheduleType.Event;
                case 2:
					return ScheduleType.Students;

            }
        }

        /// <summary>
        /// Method to upload an image to user folder
        /// </summary>
        /// <param name="user"></param>
        /// <param name="file"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool UploadImageFromUser(HttpPostedFile file, ref User user, ref Picture pic, ref string imgmsg)
        {
            bool ok = true;

            if (file.ContentType == "image/jpeg")
            {
                // We accept files until 10 Mb
                if (file.ContentLength < 10240000)
                {
                    string albumFolder = pic.AlbumID != 0 ? pic.AlbumID.ToString() : "publications";
                    string dir = AppDomain.CurrentDomain.BaseDirectory;

                    if (dir.IndexOf("bin") > 0)
                    {
                        dir = dir.Substring(0, dir.IndexOf("bin"));
                    }

                    //Search folder or create ir
                    if (!Directory.Exists(dir + "/userData"))
                    {
                        Directory.CreateDirectory(dir + "/userData");
                    }

                    dir = dir + "/userData";

                    //Search folder or create ir
                    if (!Directory.Exists(string.Format("{0}/{1}",dir,user.UserID)))
                    {
                        Directory.CreateDirectory(string.Format("{0}/{1}", dir, user.UserID));
                    }

                    //Search folder or create ir
                    if (!Directory.Exists(string.Format("{0}/{1}/{2}",dir,user.UserID, albumFolder)))
                    {
                        Directory.CreateDirectory(string.Format("{0}/{1}/{2}", dir, user.UserID, albumFolder));
                    }

                    // Encode file name to void overwrite
                    pic.PicName = string.Format("{0}_{1}.jpg", DateTime.Now.ToString("yyyyMMdd"), Utilities.EncodeMD5(file.FileName+DateTime.Now.ToLongTimeString()).Substring(0,25));
                    pic.PicPath = string.Format("{0}/{1}/{2}", user.UserID, albumFolder, pic.PicName);

                    try
                    {
                        //Bigger than 2Mb, resize!
                        if (file.ContentLength > 2048000)
                        {
                            //Accept as bigger as initial design ratio
                            Image img = ResizeImage(Image.FromStream(file.InputStream), new Size(1024,768));

                            // Save resized picture
                            img.Save(string.Format("{0}/{1}", dir, pic.PicPath));

                        }
                        else
                        {
                            file.SaveAs(string.Format("{0}/{1}", dir, pic.PicPath));
                        }
                    }
                    catch (Exception ex)
                    {
                        Utilities.LogException("Utilities",
                            MethodInfo.GetCurrentMethod().Name,
                            ex);
                        throw new WrongDataException("Ha ocurrido un error inexperado al intentar subir su imagen. Por favor, inténtelo más tarde.");
                    }
                }
                else
                {
                    ok = false;
                    imgmsg = "El límite de tamaño para una imagen es de 1MB";
                }
            }
            else
            {
                ok = false;
                imgmsg = "Lo sentimos pero solamente se aceptan imágenes de tipo jpeg";
            }

            return ok;
        }

        /// <summary>
        /// Checks 2 size are ratio
        /// </summary>
        /// <param name="sizefrom"></param>
        /// <param name="sizeto"></param>
        /// <returns></returns>
        public static bool CheckRatio(Size sizefrom, Size sizeto)
        {
            return (sizeto.Width % sizefrom.Width == 0) && (sizeto.Height % sizefrom.Height == 0);
        }

        /// <summary>
        /// Method for resizing image
        /// </summary>
        /// <param name="imgToResize"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Image ResizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }

        /// <summary>
        /// Return a language expresion for a TimeSpan
        /// </summary>
        /// <param name="user"></param>
        /// <param name="file"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string TimeSpanToString(TimeSpan span)
        {
            string res = "Hace";

            if (span.TotalDays >= 365)
            {
                res += " mucho tiempo...";
            }
            else if (span.TotalDays > 30)
            {
                res += string.Format(" aproximadamente {0} meses", Math.Truncate(span.TotalDays/30));
            }
            else if (span.TotalDays > 1)
            {
                res += string.Format(" {0} dias", Math.Truncate(span.TotalDays));
            }
            else if (span.TotalHours > 1)
            {
                res += string.Format(" {0} horas", Math.Truncate(span.TotalHours));
            }
            else if (span.TotalMinutes > 1)
            {
                res += string.Format(" {0} minutos", Math.Truncate(span.TotalMinutes));
            }
            else if (span.TotalSeconds > 15)
            {
                res += string.Format(" {0} segundos", Math.Truncate(span.TotalSeconds));
            }
            else
            {
                res += " un momento";
            }
            return res;
        }


        /// <summary>
        /// Converts size name to real size
        /// </summary>
        /// <param name="context"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public static void GetsImageSizeByName(string size, ref int height, ref int width)
        {
            switch (size)
            {
                case "profile":
                    width = 194;
                    height = 768;
                    break;
                case "settings":
                    height = 120;
                    width = 120;
                    break;
                case "pub":
                default:
                    height = 100;
                    width = 100;
                    break;
            }
        }





        public static RuleType GetRuleTypeID(string p)
        {
            switch (p)
            {
                default:
                case "0":
                    return RuleType.All;
                case "1":
                    return RuleType.Images;
                case "2":
                    return RuleType.Publications;

            }
        }

        
    }
}
