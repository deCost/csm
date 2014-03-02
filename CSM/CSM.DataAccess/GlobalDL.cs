using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Reflection;
using CSM.Classes;
using MySql.Data.MySqlClient;
using CSM;
using System.Data;



namespace CSM.DataLayer
{
    public class GlobalDL
    {

        /// <summary>
        /// Method to get total number of current users
        /// </summary>
        /// <returns>Total number of users</returns>
        public static int getMaxUsers()
        {
            try
            {
                return SQLMgr.ExecuteScaler("user_count", new List<MySqlParameter>().ToArray());
            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                throw new Exception("Ha ocurrido un error al obtener el número total de usuarios");
            }

            
        }

        /// <summary>
        /// Method to update status for an user
        /// </summary>
        /// <param name="user">User object to be processed</param>
        /// <returns>Method goes fine</returns>
        public static bool UpdateUserStatus(User user)
        {
            bool ok = true;

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("puserid", user.UserID));
                parameters.Add(new MySqlParameter("pusersession", user.SessionID));
                parameters.Add(new MySqlParameter("puserstatus", (int)user.StatuID));
                SQLMgr.ExecuteNonQuery("user_setstatus", parameters.ToArray());
            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

            }
            return ok;
        }

        /// <summary>
        /// Gets users linked with where name is similar to prefixText
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="prefixText"></param>
        /// <param name="userList"></param>
        /// <returns></returns>
        public static bool GetUsersLike(string prefixText, ref List<UserLink> userList)
        {
            bool ok = true;

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("pprefixText", prefixText));
                
                using (DataTable dt = SQLMgr.ExecuteQuery("user_getmyuserslike", "MyUsersLike", parameters.ToArray()))
                {

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            userList.Add(new UserLink()
                            {
                                Name = string.Format("{0} {1}",dr["username"].ToString() ,dr["usersurname"].ToString()),
                                UserIDReq = Decimal.Parse(dr["userid"].ToString())
                            });
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

            }
            return ok;
        }

        /// <summary>
        /// Method to get user information
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool GetUser(ref User user)
        {
            bool ok = true;

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();

                parameters.Add(new MySqlParameter("puserid", user.UserID));
                
                using (DataTable dt = new DataTable())
                {
                    // Try to login
                    dt.Merge(SQLMgr.ExecuteQuery("user_getuser", "UserInfo", parameters.ToArray()));

                    //Check results
                    if (dt.Rows.Count > 0)
                    {
                         //Check no issues
                        user.UserName = dt.Rows[0]["name"].ToString();
                        user.UserSurname = dt.Rows[0]["surname"].ToString();
                        user.UserEmail = dt.Rows[0]["email"].ToString();
                        user.UserID = Decimal.Parse(dt.Rows[0]["id"].ToString());
                        user.UserBirth = DateTime.Parse(dt.Rows[0]["birthdate"].ToString());
                        user.UserAddress = dt.Rows[0]["address"].ToString();
                        user.StatuID = Utilities.GetStatus((int) dt.Rows[0]["status"]);
                        user.SessionID = Utilities.EncodeMD5(Guid.NewGuid().ToString());
                        user.LoginDate = DateTime.Now;
                        user.ProfileImage = dt.Rows[0]["picpath"].ToString();
                        user.AlbumProfileID = Decimal.Parse(dt.Rows[0]["albumprofile"].ToString());

                    }
                    else
                    {
                        throw new WrongDataException("Los datos facilitados no coinciden con ningún usuario de nuestra base de datos");
                    }

                }

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

            }
            return ok;
        }

        /// <summary>
        /// Method to get user with pending request of connection
        /// </summary>
        /// <param name="user"></param>
        /// <param name="requestList"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool GetUsersLinkBy(ref User user, ref List<UserLink> requestList, Status status)
        {
            bool ok = true;

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();

                parameters.Add(new MySqlParameter("puserfrom", user.UserID));
				parameters.Add(new MySqlParameter("puserto", -1));
                parameters.Add(new MySqlParameter("pstatus", (int) status));

                using (DataTable dt = new DataTable())
                {
                    // Try to login
                    dt.Merge(SQLMgr.ExecuteQuery("userlink_select", "UserRequest", parameters.ToArray()));

                    //Check results
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            requestList.Add(new UserLink()
                            {
                                UserIDReq = Decimal.Parse(dr["id"].ToString()),
                                //ProfileImage = dr["picpath"].ToString(),
                                StatusID = Utilities.GetStatus(int.Parse(dr["linkstatus"].ToString())),
                                LinkDate = DateTime.Parse(dr["daterequest"].ToString()),
                                Name = string.Format("{0} {1}", dr["name"].ToString(), dr["surname"].ToString())
                            });
                        }
                        
                    }

                }

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

            }
            return ok;
        }

        /// <summary>
        /// Method to get users links status
        /// </summary>
        /// <param name="user"></param>
        /// <param name="requestList"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool GetLinkStatus(User userfrom, User userto, ref Status status)
        {
            bool ok = true;

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();

                parameters.Add(new MySqlParameter("puserfrom", userfrom.UserID));
                parameters.Add(new MySqlParameter("puserto", userto.UserID));

                status = (Status)SQLMgr.ExecuteScaler("userlink_linkstatus", parameters.ToArray());


            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

            }
            return ok;
        }

        /// <summary>
        /// Method to get user with status of request for connection
        /// </summary>
        /// <param name="user"></param>
        /// <param name="requestList"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool GetLinkStatus(ref User userFrom, ref User userTo, ref UserLink linkStatus)
        {
            bool ok = true;

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("puserFrom", userFrom.UserID));
                parameters.Add(new MySqlParameter("puserto", userTo.UserID));
                
                using (DataTable dt = new DataTable())
                {
                    // Try to login
                    dt.Merge(SQLMgr.ExecuteQuery("userlink_select", "LinkStatus", parameters.ToArray()));

                    //Check results
                    if (dt.Rows.Count > 0)
                    {
                        linkStatus.StatusID = Utilities.GetStatus((int)dt.Rows[0]["linkstatus"]);
                        linkStatus.UserIDReq = userFrom.UserID;
                        linkStatus.UserIDTo = userTo.UserID;

                    }
                    else
                    {
                        throw new WrongDataException("Los datos facilitados no coinciden con ningún usuario de nuestra base de datos");
                    }

                }

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

            }
            return ok;
        }

        /// <summary>
        /// Method to create a new publication
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pic"></param>
        /// <param name="rule"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
		public static bool InsertNewPublication(User user, Picture pic, CSM.Classes.Rule rule, Decimal parent, Decimal usertoid, string msg)
        {
            bool ok = true;

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("puserid", user.UserID));
				if(rule.RuleID > 0){
					parameters.Add(new MySqlParameter("pruleid", rule.RuleID));
				}else{
					parameters.Add(new MySqlParameter("pruleid", null));
				}
                parameters.Add(new MySqlParameter("ppubldesc", msg));
                parameters.Add(new MySqlParameter("ppubldate", DateTime.Now));

				if(parent > 0){
					parameters.Add(new MySqlParameter("pparentid", parent));
				}else{
					parameters.Add(new MySqlParameter("pparentid", null));
				}

				if(usertoid > 0){
					parameters.Add(new MySqlParameter("pusertoid", usertoid));
				}else{
					parameters.Add(new MySqlParameter("pusertoid", null));
				}

                int publid = SQLMgr.ExecuteScaler("publication_insert", parameters.ToArray());

                ok = publid > 0;

                if (pic != null && ok)
                {
                    if (!InsertNewPicture(user, pic, publid))
                    {
                        ok = false;
                    }
                }

            }
            catch (Exception e)
            {
				Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

            }

            return ok;
        }

        /// <summary>
        /// Method to inser a new picture
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pic"></param>
        /// <param name="rule"></param>
        /// <param name="publid"></param>
        /// <returns></returns>
        public static bool InsertNewPicture(User user, Picture pic, int publid)
        {

            bool ok = true;

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("ppublid", publid));
                parameters.Add(new MySqlParameter("palbumid", pic.AlbumID));
                parameters.Add(new MySqlParameter("ppicpath", pic.PicPath));
                parameters.Add(new MySqlParameter("ppicdesc", string.IsNullOrEmpty(pic.PicDesc) ? "" : pic.PicDesc));
                parameters.Add(new MySqlParameter("ppicname", pic.PicName));
                parameters.Add(new MySqlParameter("ppicdate", pic.PicDate));
                parameters.Add(new MySqlParameter("puserid", user.UserID));
                using(DataTable dt = SQLMgr.ExecuteQuery("picture_insert", "NewPicInfo", parameters.ToArray()))
                {
                    if (dt.Rows.Count > 0)
                    {
                        pic.AlbumID = Decimal.Parse(dt.Rows[0][0].ToString());
                        pic.PicID = Decimal.Parse(dt.Rows[0][1].ToString());
                    }
                }

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

            }

            return ok;

        }

        /// <summary>
        /// Gets user publications from database
        /// </summary>
        /// <param name="user"></param>
        /// <param name="requestList"></param>
        /// <returns></returns>
        public static bool GetUserPublications(User user, User userFrom, ref List<Publication> requestList)
        {
            bool ok = true;

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("puserid", user.UserID));
                parameters.Add(new MySqlParameter("puserfrom", userFrom.UserID));

                using (DataTable dt = SQLMgr.ExecuteQuery("publication_getuserpublications", "UserPublications", parameters.ToArray()))
                {

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {

                            Publication pub = new Publication()
                            {
                                PublDate = DateTime.Parse(dr["publdate"].ToString()),
                                User = new User() { UserID = Decimal.Parse(dr["userid"].ToString()), UserName = dr["username"].ToString(), UserSurname = dr["usersurname"].ToString(), ProfileImage = dr["profilepic"].ToString() },
                                PublDesc = dr["publdesc"].ToString(),
                                PublID = Decimal.Parse(dr["publid"].ToString()),
                                RuleID = Decimal.Parse(dr["ruleid"].ToString())
                            };

                            try
                            {
                                GetUserPublicationImage(ref pub);
                                GetUserPublicationComments(ref pub);
                            }
                            catch (Exception e)
                            {
                                Utilities.LogException("GlobalDL", string.Format("{0} - Error al recuperar las imágenes y comentarios de la publicación:{1}", MethodInfo.GetCurrentMethod().Name, pub.PublID), e);
                            }

                            requestList.Add(pub);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

            }
            return ok;
        }

        /// <summary>
        /// Gets image from a publication
        /// </summary>
        /// <param name="pub"></param>
        public static void GetUserPublicationImage(ref Publication pub)
        {
            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("ppublid", pub.PublID));

                using (DataTable dt = SQLMgr.ExecuteQuery("publication_getuserpublicationimage", "UserPublicationsImage", parameters.ToArray()))
                {

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {

                            pub.Image = new Picture()
                            {

                                AlbumID = Decimal.Parse(dr["albumid"].ToString()),
                                PicDate = DateTime.Parse(dr["picdate"].ToString()),
                                PicDesc = dr["picdesc"].ToString(),
                                PicID = Decimal.Parse(dr["picid"].ToString()),
                                PicName = dr["picdesc"].ToString(),
                                PicPath = dr["picpath"].ToString()

                            };

                        }
                    }
                }

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
            }
            finally
            {

            }
        }

        /// <summary>
        /// Gets publications comments
        /// </summary>
        /// <param name="pub"></param>
        public static void GetUserPublicationComments(ref Publication pub)
        {
            try
            {

                pub.Comments = new List<Publication>();
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("ppublid", pub.PublID));

                using (DataTable dt = SQLMgr.ExecuteQuery("publication_getuserpublicationcomments", "UserPublicationsComments", parameters.ToArray()))
                {

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {


                            Publication c = new Publication()
                            {

                                PublID = Decimal.Parse(dr["commid"].ToString()),
                                PublDate = DateTime.Parse(dr["commdate"].ToString()),
                                PublDesc = dr["commdesc"].ToString(),
                                RuleID = Decimal.Parse(dr["ruleid"].ToString()),
								User = new User(){ UserID = Decimal.Parse(dr["userid"].ToString()), ProfileImage = dr["picpath"].ToString(), UserName = dr["username"].ToString(), UserSurname = dr["usersurname"].ToString()}

                            };
                            GetUserPublicationImage(ref c);

                            pub.Comments.Add(c);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
            }
            finally
            {

            }
        }

        /// <summary>
        /// Insert a new link request from an user into database
        /// </summary>
        /// <param name="userfrom"></param>
        /// <param name="userto"></param>
        /// <returns></returns>
        public static bool InsertNewLinkRequest(User userfrom, User userto)
        {
            bool ok = true;

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("puserfrom", userfrom.UserID));
                parameters.Add(new MySqlParameter("puserto", userto.UserID));
                SQLMgr.ExecuteNonQuery("userlink_add", parameters.ToArray());

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

            }

            return ok;

        }

        /// <summary>
        /// Updates a link status
        /// </summary>
        /// <param name="userFrom"></param>
        /// <param name="userto"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool UpdateLinkRequest(User userFrom, User userto, Status status)
        {
            bool ok = true;

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("puserfrom", userFrom.UserID));
                parameters.Add(new MySqlParameter("puserto", userto.UserID));
                parameters.Add(new MySqlParameter("pstatus", (int) status));
                SQLMgr.ExecuteNonQuery("userlink_update", parameters.ToArray());

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

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

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("puserfrom", userFrom.UserID));
                parameters.Add(new MySqlParameter("puserto", userto.UserID));
                SQLMgr.ExecuteNonQuery("userlink_delete", parameters.ToArray());

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

            }

            return ok;
        }

        /// <summary>
        /// Method to update user info
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool UpdateUser(ref User user)
        {
            bool ok = true;

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("puserbirth", user.UserBirth));
                parameters.Add(new MySqlParameter("puseremail", user.UserEmail));
                parameters.Add(new MySqlParameter("puserlogin", user.UserLogin));
                parameters.Add(new MySqlParameter("puserpass", user.UserPass));
                parameters.Add(new MySqlParameter("pusername", user.UserName));
                parameters.Add(new MySqlParameter("pusersurname", user.UserSurname));
                parameters.Add(new MySqlParameter("puserid", user.UserID));
                SQLMgr.ExecuteNonQuery("user_updateinfo", parameters.ToArray());

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

            }

            return ok;
        }

        /// <summary>
        /// Method to get albums available from an user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="albumList"></param>
        /// <returns></returns>
        public static bool GetAlbums(User userFrom, User userTo, ref List<Album> albumList)
        {
            bool ok = true;

            try
            {

                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("puserfrom", userFrom.UserID));
                parameters.Add(new MySqlParameter("puserid", userTo.UserID));

                using (DataTable dt = SQLMgr.ExecuteQuery("album_getuseralbums", "UserAlbums", parameters.ToArray()))
                {

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {

                            albumList.Add(new Album()
                            {
                                UserID = Decimal.Parse(dr["userid"].ToString()),
                                AlbumDesc = dr["albumdesc"].ToString(),
                                AlbumID = Decimal.Parse(dr["albumid"].ToString()),
                                AlbumKeyPic = dr["albumkeypic"].ToString(),
                                AlbumName = dr["albumname"].ToString(),
                                IsProfile = int.Parse(dr["isalbumprofile"].ToString()) == 1,
                                IsPublications = int.Parse(dr["isalbumpublication"].ToString()) == 1
                            });

                        }
                    }
                }


            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                ok = false;
            }
            finally
            {

            }

            return ok;
        }

        /// <summary>
        /// Method to get pictures from an album
        /// </summary>
        /// <param name="album"></param>
        /// <param name="pictureList"></param>
        /// <returns></returns>
        public static bool GetPicturesFromAlbum(ref Album album, ref List<Picture> pictureList)
        {
            bool ok = true;

            try
            {

                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("palbumid", album.AlbumID));

                using (DataTable dt = SQLMgr.ExecuteQuery("album_getpictures", "AlbumPictures", parameters.ToArray()))
                {

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            pictureList.Add(new Picture()
                            {
                                PicID = Decimal.Parse(dr["picid"].ToString()),
                                PicDesc = dr["picdesc"].ToString(),
                                AlbumID = Decimal.Parse(dr["albumid"].ToString()),
                                PicPath = dr["picpath"].ToString(),
                                PicDate = DateTime.Parse(dr["picdate"].ToString()),
                            });

                        }
                    }
                }


            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                ok = false;
            }
            finally
            {

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

            try
            {

                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("puserid", album.UserID));
                parameters.Add(new MySqlParameter("palbumname", album.AlbumName));
                parameters.Add(new MySqlParameter("palbumdesc", album.AlbumDesc));
                if(album.RuleID > 0)
                    parameters.Add(new MySqlParameter("pruleid", album.RuleID));
                album.AlbumID = SQLMgr.ExecuteScaler("album_newalbum", parameters.ToArray());

                ok = album.AlbumID > 0;


            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                ok = false;
            }
            finally
            {

            }

            return ok;
        }

        /// <summary>
        /// Inserts new schedule register into DB
        /// </summary>
        /// <param name="schd"></param>
        /// <returns></returns>
        public static bool InsertNewSchedule(ref Schedule schd)
        {
            bool ok = true;

            try
            {

                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("puserid", schd.UserID));
                parameters.Add(new MySqlParameter("pruleid", schd.RuleID));
                parameters.Add(new MySqlParameter("pscheddesc", schd.SchedDesc));
                parameters.Add(new MySqlParameter("pschedtitle", schd.SchedTitle));
                parameters.Add(new MySqlParameter("pschedtypeid", (int) schd.SchedTypeID));
                parameters.Add(new MySqlParameter("pscheddate", schd.SchedDate));
                parameters.Add(new MySqlParameter("pschedrepeat", schd.SchedRepeat));
				parameters.Add(new MySqlParameter("pschedbookingid", schd.SchedBooking));

                schd.SchedID = SQLMgr.ExecuteScaler("schedule_new", parameters.ToArray());

                ok = schd.SchedID > 0;

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                ok = false;
            }
            finally
            {

            }

            return ok;
        }

        /// <summary>
        /// Inserts new schedule link with user into DB
        /// </summary>
        /// <param name="schd"></param>
        /// <param name="userto"></param>
        /// <returns></returns>
        public static bool InsertNewScheduleLink(ref Schedule schd, Decimal userto)
        {
            bool ok = true;

            try
            {

                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("puserid", userto));
                parameters.Add(new MySqlParameter("pschdid", schd.SchedID));

                ok = SQLMgr.ExecuteScaler("schedulelink_new", parameters.ToArray()) > 0;

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                ok = false;
            }
            finally
            {

            }

            return ok;
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

            try
            {

                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("puserid", user.UserID));

                using (DataTable dt = SQLMgr.ExecuteQuery("schedulelink_select", "Schedule", parameters.ToArray()))
                {

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            scheduleList.Add(new Schedule()
                            {
                                SchedID = Decimal.Parse(dr["schedid"].ToString()),
                                SchedDesc = dr["scheddesc"].ToString(),
                                SchedTypeID = Utilities.GetScheduleType(int.Parse(dr["schedtypeid"].ToString())),
                                SchedTitle = dr["schedtitle"].ToString(),
                                SchedDate = DateTime.Parse(dr["scheddate"].ToString()),
								SchedBooking = (int)dr["schedbookingId"]
                            });

                        }
                    }
                }


            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                ok = false;
            }
            finally
            {

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

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("puserid", user.UserID));
                parameters.Add(new MySqlParameter("pschedid", schedule.SchedID));
                SQLMgr.ExecuteNonQuery("schedule_finish", parameters.ToArray());

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

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

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("ppicid", pic.PicID));
                parameters.Add(new MySqlParameter("palbumid", pic.AlbumID));
                using (DataTable dt = SQLMgr.ExecuteQuery("picture_delete", "DeletePath", parameters.ToArray()))
                {
                    if (dt.Rows.Count > 0)
                    {
                        pic.PicPath = dt.Rows[0][0].ToString();
                    }
                }

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

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

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("puserid", user.UserID));
                using (DataTable dt = SQLMgr.ExecuteQuery("picture_getlatestpictures", "LatestPictures", parameters.ToArray()))
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            latestPicturesList.Add(new Album()
                            {
                                AlbumDesc = dr["albumdesc"].ToString(),
                                AlbumID = Decimal.Parse(dr["albumid"].ToString()),
                                AlbumKeyPic = dr["picpath"].ToString(),
                                AlbumName = dr["albumname"].ToString(),
                                UserID = Decimal.Parse(dr["userid"].ToString())
                            });
                        }
                        
                    }
                }

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

            }

            return ok;
        }

        /// <summary>
        /// Gets list of messages between users
        /// </summary>
        /// <param name="userFrom"></param>
        /// <param name="userTo"></param>
        /// <param name="requestList"></param>
        /// <returns></returns>
        public static bool GetUserMessagesWith(User userFrom, User userTo, ref List<Publication> requestList)
        {
            bool ok = true;

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("puserFrom", userFrom.UserID));
                parameters.Add(new MySqlParameter("puserTo", userTo.UserID));
                using (DataTable dt = SQLMgr.ExecuteQuery("message_select", "Messages", parameters.ToArray()))
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            requestList.Add(new Publication()
                            {
                                PublDesc = dr["publDesc"].ToString(),
                                PublID = Decimal.Parse(dr["publid"].ToString()),
                                PublDate = DateTime.Parse(dr["publdate"].ToString()),
                                User = new User() { UserID = Decimal.Parse(dr["userid"].ToString()), ProfileImage = dr["profilepic"].ToString() },
                                Comments = new List<Publication>()
                            });
                        }

                    }
                }

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

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

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("puserFrom", userFrom.UserID));
                parameters.Add(new MySqlParameter("puserTo", userTo.UserID));
                parameters.Add(new MySqlParameter("pmessagetxt", msg));
                SQLMgr.ExecuteNonQuery("message_insert", parameters.ToArray());

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

            }

            return ok;
        }

        /// <summary>
        /// Insert new rule for an user and return its id
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
		public static bool InsertNewRule(ref CSM.Classes.Rule rule)
        {
            bool ok = true;

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("pruletypeid", (int)rule.RuleTypeID));
                parameters.Add(new MySqlParameter("puserid", rule.UserID));
                parameters.Add(new MySqlParameter("prulename", rule.RuleName));
                parameters.Add(new MySqlParameter("pruledesc", rule.RuleDesc));
                using (DataTable dt = SQLMgr.ExecuteQuery("rule_insert", "InsertedRule", parameters.ToArray()))
                {
                    if (dt.Rows.Count > 0)
                    {
                        rule.RuleID = Decimal.Parse(dt.Rows[0][0].ToString());
                    }
                }

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

            }

            return ok;
        }

        /// <summary>
        /// Gets user rule list from db
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ruleList"></param>
        /// <returns></returns>
        public static bool GetRulesFromUser(User user, ref List<CSM.Classes.Rule> ruleList)
        {
            bool ok = true;

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("puserid", user.UserID));
                using (DataTable dt = SQLMgr.ExecuteQuery("rule_select", "RulesFromUser", parameters.ToArray()))
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            ruleList.Add(new CSM.Classes.Rule()
                            {
                                RuleDesc = dr["ruleDesc"].ToString(),
                                RuleID = Decimal.Parse(dr["ruleid"].ToString()),
                                RuleName = dr["rulename"].ToString(),
                                RuleTypeID = Utilities.GetRuleTypeID(dr["ruletypeid"].ToString())

                            });
                        }

                    }
                }

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

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

            try
            {

                foreach (string u in privacy.Users)
                {
                    List<MySqlParameter> parameters = new List<MySqlParameter>();
                    parameters.Add(new MySqlParameter("pruleid", privacy.RuleID));
                    parameters.Add(new MySqlParameter("puserid", int.Parse(u)));
                    parameters.Add(new MySqlParameter("pprivname", privacy.PrivName));
                    parameters.Add(new MySqlParameter("pprivoptionid", privacy.PrivOptionID));
                    SQLMgr.ExecuteNonQuery("privacy_insert", parameters.ToArray());
                }

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

            }

            return ok;
        }

        /// <summary>
        /// Gets rule list between users
        /// </summary>
        /// <param name="userFrom"></param>
        /// <param name="_user"></param>
        /// <param name="ruleList"></param>
        /// <returns></returns>
        public static bool GetRulesFromUser(User userFrom, User userTo, ref List<CSM.Classes.Rule> ruleList)
        {
            bool ok = true;

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("puserFrom", userFrom.UserID));
                parameters.Add(new MySqlParameter("puserTo", userTo.UserID));
                using (DataTable dt = SQLMgr.ExecuteQuery("rules_getrulesfromuser", "ListRulesFromUser", parameters.ToArray()))
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            ruleList.Add(new CSM.Classes.Rule()
                            {
                                RuleDesc = dr["ruleDesc"].ToString(),
                                RuleID = Decimal.Parse(dr["ruleid"].ToString()),
                                RuleName = dr["rulename"].ToString(),
                                RuleTypeID = Utilities.GetRuleTypeID(dr["ruletypeid"].ToString())

                            });
                        }

                    }
                }

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

            }

            return ok;
        }

        /// <summary>
        /// Gets rule info
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="privacy"></param>
        /// <returns></returns>
        public static bool GetRuleInfo(ref CSM.Classes.Rule rule, ref Privacy privacy)
        {
            bool ok = true;

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("pruleid", rule.RuleID));
                using (DataTable dt = SQLMgr.ExecuteQuery("rules_getruleinfo", "RulesInfo", parameters.ToArray()))
                {
                    if (dt.Rows.Count > 0)
                    {

                        rule.RuleName = dt.Rows[0]["rulename"].ToString();
                        rule.RuleDesc = dt.Rows[0]["ruledesc"].ToString();
                        rule.RuleTypeID = Utilities.GetRuleTypeID(dt.Rows[0]["ruletypeid"].ToString());
                        privacy.Users = new List<string>();

                        foreach (DataRow dr in dt.Rows)
                        {
                            privacy.Users.Add(dr["userto"].ToString());
                        }

                    }
                }

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

            }

            return ok;
        }

        /// <summary>
        /// Modify rule info
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public static bool ModifyRule(ref CSM.Classes.Rule rule)
        {
            bool ok = true;

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("pruletypeid", (int)rule.RuleTypeID));
                parameters.Add(new MySqlParameter("puserid", rule.UserID));
                parameters.Add(new MySqlParameter("prulename", rule.RuleName));
                parameters.Add(new MySqlParameter("pruledesc", rule.RuleDesc));
                parameters.Add(new MySqlParameter("pruleid", rule.RuleID));
                SQLMgr.ExecuteNonQuery("rule_modify", parameters.ToArray());

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

            }

            return ok;
        }

        /// <summary>
        /// Modifies privacy from a rule
        /// </summary>
        /// <param name="privacy"></param>
        /// <returns></returns>
        public static bool ModifyPrivacy(ref Privacy privacy)
        {
            bool ok = true;

            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();
                parameters.Add(new MySqlParameter("pruleid", privacy.RuleID));
                SQLMgr.ExecuteNonQuery("privacy_delete", parameters.ToArray());

                foreach (string u in privacy.Users)
                {
                    parameters = new List<MySqlParameter>();
                    parameters.Add(new MySqlParameter("pruleid", privacy.RuleID));
                    parameters.Add(new MySqlParameter("puserid", int.Parse(u)));
                    parameters.Add(new MySqlParameter("pprivname", privacy.PrivName));
                    parameters.Add(new MySqlParameter("pprivoptionid", privacy.PrivOptionID));
                    SQLMgr.ExecuteNonQuery("privacy_insert", parameters.ToArray());
                }

                

            }
            catch (Exception e)
            {
                Utilities.LogException("GlobalDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

            }

            return ok;
        }
    }
}
