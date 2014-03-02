using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Reflection;
using CSM.Classes;
using CSM;
using CSM.DataLayer;


namespace CMS.DataManager
{
    public class GlobalBS
    {

        /// <summary>
        /// Method to get total number of current users
        /// </summary>
        /// <returns>Total number of users</returns>
        public static int getMaxUsers()
        {
            try
            {
                return GlobalDL.getMaxUsers();
            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalBS", MethodInfo.GetCurrentMethod().Name, e);
                return 0;
            }
        }

        /// <summary>
        /// Method to process a contact form request
        /// </summary>
        /// <param name="contact">ContactForm class to be treated</param>
        /// <returns>Method goes fine</returns>
        public static bool ProcessContactForm(ContactForm contact)
        {
            bool ok = true;

            try
            {

                Utilities.SendMail(contact.Email,
                       ConfigurationManager.AppSettings["noReply"],
                       "Social Me! - Nuevo formulario de contacto",
                       getBody(contact),
                       true,
                       ConfigurationManager.AppSettings["smtpServer"],
                       null,
                       false,
                       new string[] { ConfigurationManager.AppSettings["smtpUser"], ConfigurationManager.AppSettings["smtpPass"] },
                       null,
                       null);
            }
            catch (Exception e)
            {
                ok = false;
                Utilities.LogException("GlobalBS", MethodInfo.GetCurrentMethod().Name, e);
            }
            finally
            {
                Utilities.WriteLog(string.Format("Nuevo formulario de contacto [{0}]: \n\n {1}", ok ? "OK" : "ERROR" ,contact.ToString()), "log", "FormContacto");
            }
            return ok;
        }

        /// <summary>
        /// Gets email body as HTML from file assigned to ProcessContactForm
        /// </summary>
        /// <param name="ctc"></param>
        /// <returns></returns>
        private static string getBody(ContactForm ctc)
        {
            StringBuilder body = new StringBuilder(File.ReadAllText(Utilities.GetEmailTemplatePath(EmailTemplateType.CONTACT)));

            body.Replace("#Nombre#", ctc.Nombre);
            body.Replace("#Email#", ctc.Email);
            body.Replace("#Asunto#", ctc.Asunto);
            body.Replace("#Mensaje#", ctc.Mensaje);

            return body.ToString();
        }

        /// <summary>
        /// Method to get current user likes to linked users
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="prefixText"></param>
        /// <returns>DataTable with users matched</returns>
        public static bool GetUsersLike(string prefixText, ref List<UserLink> userList)
        {

            bool ok = true;

            if (!GlobalDL.GetUsersLike(prefixText, ref userList))
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Gets user information
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool GetUser(ref User user)
        {
            bool ok = true;

            if (!GlobalDL.GetUser(ref user))
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Gets user's link
        /// </summary>
        /// <param name="user"></param>
        /// <param name="requestList"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool GetUsersLinkBy(ref User user, ref List<UserLink> requestList, Status status)
        {
            bool ok = true;

            if (!GlobalDL.GetUsersLinkBy(ref user, ref requestList, status))
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Inserts a new Publication into system
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pic"></param>
        /// <param name="rule"></param>
        /// <param name="parent"></param>
        /// <param name="usertoid"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool InsertNewPublication(User user, Picture pic, Rule rule,  Decimal parent, Decimal usertoid, string msg)
        {
            bool ok = true;

            if (!GlobalDL.InsertNewPublication(user, pic, rule, parent,usertoid,msg))
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Gets an user publication
        /// </summary>
        /// <param name="_user"></param>
        /// <param name="requestList"></param>
        /// <returns></returns>
        public static bool GetUserPublications(User user, User userFrom, ref List<Publication> requestList)
        {
            bool ok = true;

            if (!GlobalDL.GetUserPublications(user, userFrom, ref requestList))
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Insert a new Link request into system from one user
        /// </summary>
        /// <param name="userfrom"></param>
        /// <param name="userto"></param>
        /// <returns></returns>
        public static bool InsertNewLinkRequest(User userfrom, User userto)
        {
            bool ok = true;

            if (!GlobalDL.InsertNewLinkRequest(userfrom, userto))
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Gets link status between users
        /// </summary>
        /// <param name="userfrom"></param>
        /// <param name="userto"></param>
        /// <param name="linkStatus"></param>
        /// <returns></returns>
        public static bool GetLinkStatus(User userfrom, User userto, ref Status linkStatus)
        {

            bool ok = true;

            if (!GlobalDL.GetLinkStatus(userfrom, userto, ref linkStatus))
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Updates an users link status
        /// </summary>
        /// <param name="userFrom"></param>
        /// <param name="userto"></param>
        /// <returns></returns>
        public static bool UpdateLinkRequest(User userFrom, User userto)
        {
            bool ok = true;

            if (!GlobalDL.UpdateLinkRequest(userFrom, userto, Status.Active))
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Deletes a relationship between users
        /// </summary>
        /// <param name="userFrom"></param>
        /// <param name="userto"></param>
        /// <returns></returns>
        public static bool DeleteLinkRequest(User userFrom, User userto)
        {
            bool ok = true;

            if (!GlobalDL.DeleteLinkRequest(userFrom, userto))
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Inserts new picture
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pic"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        public static bool InsertNewImage(User user, Picture pic)
        {
            bool ok = true;

            if (!GlobalDL.InsertNewPicture(user, pic, -1))
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Updates user info
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool UpdateUser(ref User user)
        {
            bool ok = true;

            if (!string.IsNullOrEmpty(user.UserPass) && !string.IsNullOrEmpty(user.UserLogin))
            {
                if (!GlobalDL.UpdateUser(ref user))
                {
                    ok = false;
                }
            }
            else
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Gets user album
        /// </summary>
        /// <param name="user"></param>
        /// <param name="albumList"></param>
        /// <returns></returns>
        public static bool GetAlbums(User userFrom, User userTo, ref List<Album> albumList)
        {
            bool ok = true;

            if (!GlobalDL.GetAlbums(userFrom, userTo, ref albumList))
            {
                ok = false;
            }
          
            return ok;
        }

        /// <summary>
        /// Gets complete list of pictures attached to an album
        /// </summary>
        /// <param name="album"></param>
        /// <param name="pictureList"></param>
        /// <returns></returns>
        public static bool GetPicturesFromAlbum(ref Album album, ref List<Picture> pictureList)
        {
            bool ok = true;

            if (!GlobalDL.GetPicturesFromAlbum(ref album, ref pictureList))
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Method to insert a new album into DB
        /// </summary>
        /// <param name="album"></param>
        /// <returns></returns>
        public static bool InsertNewAlbum(ref Album album)
        {
            bool ok = true;

            if (!GlobalDL.InsertNewAlbum(ref album))
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Inserts new schedule link with user into DB
        /// </summary>
        /// <param name="schd"></param>
        /// <param name="userto"></param>
        /// <returns></returns>
        public static bool InsertNewSchedule(ref Schedule schd)
        {
            bool ok = true;

            if (!GlobalDL.InsertNewSchedule(ref schd))
            {
                return false;
            }

            foreach (string f in schd.Friends)
            {
                if (!GlobalDL.InsertNewScheduleLink(ref schd, Decimal.Parse(f)))
                {
                    ok = false;
                    break;
                }
            }

            return ok;
        }

        /// <summary>
        /// Gets a list of different schedule types available
        /// </summary>
        /// <param name="schd"></param>
        /// <param name="userto"></param>
        /// <returns></returns>
        public static List<ScheduleClass> GetScheduleTypes()
        {
			int[] values = new int[3] { (int)ScheduleType.Event, (int)ScheduleType.Task, (int)ScheduleType.Students };
			string[] name = new string[3] { "Evento", "Tarea", "Alumnos" };
            List<ScheduleClass> scheduleTypeList = new List<ScheduleClass>();
            for (int i = 0; i < values.Length; i++ )
            {
                scheduleTypeList.Add(new ScheduleClass() { Name = name[i], Type = values[i] });
            }

            return scheduleTypeList;
        }

        /// <summary>
        /// Gets a list of different privacy option types available
        /// </summary>
        /// <param name="schd"></param>
        /// <param name="userto"></param>
        /// <returns></returns>
        public static List<PrivacyOptionClass> GetPrivacyOptionTypes()
        {
            int[] values = new int[1] { (int)PrivacyOption.OnlyFriends/*, (int)PrivacyOption.FriendsAndLinked*/};
            string[] name = new string[1] { "Únicamente usuarios seleccionados"/*, "Usuarios seleccionados y sus enlazados"*/ };
            List<PrivacyOptionClass> privacyOptionList = new List<PrivacyOptionClass>();
            for (int i = 0; i < values.Length; i++)
            {
                privacyOptionList.Add(new PrivacyOptionClass() { Name = name[i], Type = values[i] });
            }

            return privacyOptionList;
        }

        /// <summary>
        /// Gets a list of different rule types available
        /// </summary>
        /// <param name="schd"></param>
        /// <param name="userto"></param>
        /// <returns></returns>
        public static List<RuleTypeClass> GetRuleTypes()
        {
            int[] values = new int[3] { (int)RuleType.All, (int)RuleType.Images, (int)RuleType.Publications };
            string[] name = new string[3] { "Todo", "Imágenes", "Publicaciones" };
            List<RuleTypeClass> ruleTypeList = new List<RuleTypeClass>();
            for (int i = 0; i < values.Length; i++)
            {
                ruleTypeList.Add(new RuleTypeClass() { Name = name[i], Type = values[i] });
            }

            return ruleTypeList;
        }

        /// <summary>
        /// Gets schedule list from an user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="scheduleList"></param>
        /// <returns></returns>
        public static bool GetScheduleFromUser(ref User user, ref List<Schedule> scheduleList)
        {
            bool ok = true;

            if (!GlobalDL.GetScheduleFromUser(ref user, ref scheduleList))
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Sets a schedule as finished
        /// </summary>
        /// <param name="schedule"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool FinishSchedule(Schedule schedule, User user)
        {
            bool ok = true;

            if (!GlobalDL.FinishSchedule(schedule, user))
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Deletes an image and return fille PicPath property from pic parameter with deleted image path
        /// </summary>
        /// <param name="pic"></param>
        /// <returns></returns>
        public static bool DeleteImage(ref Picture pic)
        {
            bool ok = true;

            if (!GlobalDL.DeleteImage(ref pic))
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Gets a list of the latest uploaded images from linked users
        /// </summary>
        /// <param name="user"></param>
        /// <param name="latestPicturesList"></param>
        /// <returns></returns>
        public static bool GetLatestImages(User user, ref List<Album> latestPicturesList)
        {
            bool ok = true;

            if (!GlobalDL.GetLatestImages(user, ref latestPicturesList))
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Gets list of messages between users
        /// </summary>
        /// <param name="CurrentUser"></param>
        /// <param name="ProfileUser"></param>
        /// <param name="requestList"></param>
        /// <returns></returns>
        public static bool GetUserMessagesWith(User userFrom, User userTo, ref List<Publication> requestList)
        {
            bool ok = true;

            if (!GlobalDL.GetUserMessagesWith(userFrom, userTo, ref requestList))
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Inserts a new message between users
        /// </summary>
        /// <param name="userFrom"></param>
        /// <param name="userTo"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool InsertNewMessage(User userFrom, User userTo, string msg)
        {
            bool ok = true;

            if (!GlobalDL.InsertNewMessage(userFrom, userTo, msg))
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Insert new rule for an user and return its id
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public static bool InsertNewRule(ref Rule rule)
        {
            bool ok = true;

            if (!GlobalDL.InsertNewRule(ref rule))
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Gets user rules list from db
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ruleList"></param>
        /// <returns></returns>
        public static bool GetRulesFromUser(User user, List<Rule> ruleList)
        {
            bool ok = true;

            if (!GlobalDL.GetRulesFromUser(user, ref ruleList))
            {
                ok = false;
            }

            return ok;
        }

        /// <summary>
        /// Inserts new privacy into db
        /// </summary>
        /// <param name="privacy"></param>
        /// <returns></returns>
        public static bool InsertNewPrivacy(ref Privacy privacy)
        {
            bool ok = true;

            if (!GlobalDL.InsertNewPrivacy(ref privacy))
            {
                ok = false;
            }

            return ok;
        }
        
        /// <summary>
        /// Gets rule info
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="privacy"></param>
        /// <returns></returns>
        public static bool GetRuleInfo(ref Rule rule, ref Privacy privacy)
        {
            bool ok = true;

            if (!GlobalDL.GetRuleInfo(ref rule, ref privacy))
            {
                ok = false;
            }

            return ok;
        }

        public static bool ModifyRule(Rule rule, Privacy privacy)
        {
            bool ok = true;

            if (!GlobalDL.ModifyRule(ref rule) || !GlobalDL.ModifyPrivacy(ref privacy))
            {
                ok = false;
            }

            return ok;
        }
    }
}
