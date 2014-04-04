using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using CSM.Classes;
using MySql.Data.MySqlClient;
using CSM;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using CSM.Common;

namespace CSM.DataLayer
{
	public class GlobalDL
	{
		private static string dbType = ConfigurationManager.AppSettings ["DBType"];

		/// <summary>
		/// Method to get total number of current users
		/// </summary>
		/// <returns>Total number of users</returns>
		public static int getMaxUsers ()
		{
			try {
				switch (dbType) {
				case "MySQL":
					return MySQLMgr.ExecuteScaler ("user_count", new List<MySqlParameter> ().ToArray ());

				case "SSQL":
					return SSQLMgr.ExecuteScaler ("user_count", new List<SqlParameter> ().ToArray ());		

				}

				return 0;
			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				throw new Exception ("Ha ocurrido un error al obtener el número total de usuarios");
			}

            
		}

		/// <summary>
		/// Method to update status for an user
		/// </summary>
		/// <param name="user">User object to be processed</param>
		/// <returns>Method goes fine</returns>
		public static bool UpdateUserStatus (User user)
		{
			bool ok = true;

			try {
				switch (dbType) {
				case "MySQL":
					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("puserid", user.UserID));
					mysql.Add (new MySqlParameter ("pusersession", user.SessionID));
					mysql.Add (new MySqlParameter ("puserstatus", (int)user.StatuID));
					MySQLMgr.ExecuteNonQuery ("user_setstatus", mysql.ToArray ());
					break;
				case "SSQL":
					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@userid", user.UserID));
					sql.Add (new SqlParameter ("@usersession", user.SessionID));
					sql.Add (new SqlParameter ("@userstatus", (int)user.StatuID));
					SSQLMgr.ExecuteNonQuery ("user_setstatus", sql.ToArray ());
					break;
				}

                
			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

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
		public static bool GetUsersLike (string prefixText, ref List<UserLink> userList)
		{
			bool ok = true;

			try {
				DataTable dt = new DataTable ();

				switch (dbType) {
				case "MySQL":
					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("pprefixText", prefixText));

					dt = MySQLMgr.ExecuteQuery ("user_getmyuserslike", "MyUsersLike", mysql.ToArray ());
					break;
				case "SSQL":
					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@prefixText", prefixText));

					dt = SSQLMgr.ExecuteQuery ("user_getmyuserslike", "MyUsersLike", sql.ToArray ());
					break;
				}

				if (dt != null && dt.Rows.Count > 0) {
					foreach (DataRow dr in dt.Rows) {
						userList.Add (new UserLink () {
							Name = string.Format ("{0} {1}", dr ["username"].ToString (), dr ["usersurname"].ToString ()),
							UserIDReq = Decimal.Parse (dr ["userid"].ToString ())
						});
					}
				}
                

			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

			}
			return ok;
		}

		/// <summary>
		/// Method to get user information
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public static bool GetUser (ref User user)
		{
			bool ok = true;

			try {

                
				using (DataTable dt = new DataTable ()) {


					switch (dbType) {
					case "MySQL":
						List<MySqlParameter> mysql = new List<MySqlParameter> ();
						mysql.Add (new MySqlParameter ("puserid", user.UserID));
						dt.Merge (MySQLMgr.ExecuteQuery ("user_getuser", "UserInfo", mysql.ToArray ()));
						break;
					case "SSQL":
						List<SqlParameter> sql = new List<SqlParameter> ();
						sql.Add (new SqlParameter ("@userid", user.UserID));
						dt.Merge (SSQLMgr.ExecuteQuery ("user_getuser", "UserInfo", sql.ToArray ()));
						break;
					}

					// Try to login


					//Check results
					if (dt.Rows.Count > 0) {
						//Check no issues
						user.UserName = dt.Rows [0] ["name"].ToString ();
						user.UserSurname = dt.Rows [0] ["surname"].ToString ();
						user.UserEmail = dt.Rows [0] ["email"].ToString ();
						user.UserID = Decimal.Parse (dt.Rows [0] ["id"].ToString ());
						user.UserBirth = DateTime.Parse (dt.Rows [0] ["birthdate"].ToString ());
						user.UserAddress = dt.Rows [0] ["address"].ToString ();
						user.StatuID = Utilities.GetStatus ((int)dt.Rows [0] ["status"]);
						user.SessionID = Utilities.EncodeMD5 (Guid.NewGuid ().ToString ());
						user.LoginDate = DateTime.Now;
						user.ProfileImage = dt.Rows [0] ["picpath"].ToString ();
						user.AlbumProfileID = Decimal.Parse (dt.Rows [0] ["albumprofile"].ToString ());

					} else {
						throw new WrongDataException ("Los datos facilitados no coinciden con ningún usuario de nuestra base de datos");
					}

				}

			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

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
		public static bool GetUsersLinkBy (ref User user, ref List<UserLink> requestList, Status status)
		{
			bool ok = true;

			try {


				using (DataTable dt = new DataTable ()) {


					switch (dbType) {
					case "MySQL":
						List<MySqlParameter> mysql = new List<MySqlParameter> ();

						mysql.Add (new MySqlParameter ("puserfrom", user.UserID));
						mysql.Add (new MySqlParameter ("puserto", -1));
						mysql.Add (new MySqlParameter ("pstatus", (int)status));
						dt.Merge (MySQLMgr.ExecuteQuery ("userlink_select", "UserRequest", mysql.ToArray ()));
						break;
					case "SSQL":
						List<SqlParameter> sql = new List<SqlParameter> ();

						sql.Add (new SqlParameter ("@userfrom", user.UserID));
						sql.Add (new SqlParameter ("@userto", -1));
						sql.Add (new SqlParameter ("@status", (int)status));
						dt.Merge (SSQLMgr.ExecuteQuery ("userlink_select", "UserRequest", sql.ToArray ()));
						break;
					}

					// Try to login


					//Check results
					if (dt.Rows.Count > 0) {
						foreach (DataRow dr in dt.Rows) {
							requestList.Add (new UserLink () {
								UserIDReq = Decimal.Parse (dr ["id"].ToString ()),
								//ProfileImage = dr["picpath"].ToString(),
								StatusID = Utilities.GetStatus (int.Parse (dr ["linkstatus"].ToString ())),
								LinkDate = DateTime.Parse (dr ["daterequest"].ToString ()),
								Name = string.Format ("{0} {1}", dr ["name"].ToString (), dr ["surname"].ToString ())
							});
						}
                        
					}

				}

			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

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
		public static bool GetLinkStatus (User userfrom, User userto, ref Status status)
		{
			bool ok = true;

			try {

				switch (dbType) {
				case "MySQL":
					List<MySqlParameter> mysql = new List<MySqlParameter> ();

					mysql.Add (new MySqlParameter ("puserfrom", userfrom.UserID));
					mysql.Add (new MySqlParameter ("puserto", userto.UserID));

					status = (Status)MySQLMgr.ExecuteScaler ("userlink_linkstatus", mysql.ToArray ());
					break;
				case "SSQL":
					List<SqlParameter> sql = new List<SqlParameter> ();

					sql.Add (new SqlParameter ("@userfrom", userfrom.UserID));
					sql.Add (new SqlParameter ("@userto", userto.UserID));

					status = (Status)SSQLMgr.ExecuteScaler ("userlink_linkstatus", sql.ToArray ());
					break;
				}




			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

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
		public static bool GetLinkStatus (ref User userFrom, ref User userTo, ref UserLink linkStatus)
		{
			bool ok = true;

			try {

                
				using (DataTable dt = new DataTable ()) {


					switch (dbType) {
					case "MySQL":
						List<MySqlParameter> mysql = new List<MySqlParameter> ();
						mysql.Add (new MySqlParameter ("puserFrom", userFrom.UserID));
						mysql.Add (new MySqlParameter ("puserto", userTo.UserID));
						// Try to login
						dt.Merge (MySQLMgr.ExecuteQuery ("userlink_select", "LinkStatus", mysql.ToArray ()));
						break;
					case "SSQL":
						List<SqlParameter> sql = new List<SqlParameter> ();
						sql.Add (new SqlParameter ("@userFrom", userFrom.UserID));
						sql.Add (new SqlParameter ("@userto", userTo.UserID));
						// Try to login
						dt.Merge (SSQLMgr.ExecuteQuery ("userlink_select", "LinkStatus", sql.ToArray ()));
						break;
					}



					//Check results
					if (dt.Rows.Count > 0) {
						linkStatus.StatusID = Utilities.GetStatus ((int)dt.Rows [0] ["linkstatus"]);
						linkStatus.UserIDReq = userFrom.UserID;
						linkStatus.UserIDTo = userTo.UserID;

					} else {
						throw new WrongDataException ("Los datos facilitados no coinciden con ningún usuario de nuestra base de datos");
					}

				}

			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

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
		public static bool InsertNewPublication (User user, Picture pic, CSM.Classes.Rule rule, Decimal parent, Decimal usertoid, string msg)
		{
			bool ok = true;

			try {
				int publid = 0;
				switch (dbType) {
				case "MySQL":
					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("puserid", user.UserID));
					if (rule.RuleID > 0) {
						mysql.Add (new MySqlParameter ("pruleid", rule.RuleID));
					} else {
						mysql.Add (new MySqlParameter ("pruleid", null));
					}
					mysql.Add (new MySqlParameter ("ppubldesc", msg));
					mysql.Add (new MySqlParameter ("ppubldate", DateTime.Now));

					if (parent > 0) {
						mysql.Add (new MySqlParameter ("pparentid", parent));
					} else {
						mysql.Add (new MySqlParameter ("pparentid", null));
					}

					if (usertoid > 0) {
						mysql.Add (new MySqlParameter ("pusertoid", usertoid));
					} else {
						mysql.Add (new MySqlParameter ("pusertoid", null));
					}

					publid = MySQLMgr.ExecuteScaler ("publication_insert", mysql.ToArray ());
					break;
				case "SSQL":
					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@userid", user.UserID));
					if (rule.RuleID > 0) {
						sql.Add (new SqlParameter ("@ruleid", rule.RuleID));
					} else {
						sql.Add (new SqlParameter ("@ruleid", null));
					}
					sql.Add (new SqlParameter ("@publdesc", msg));
					sql.Add (new SqlParameter ("@publdate", DateTime.Now));

					if (parent > 0) {
						sql.Add (new SqlParameter ("@parentid", parent));
					} else {
						sql.Add (new SqlParameter ("@parentid", null));
					}

					if (usertoid > 0) {
						sql.Add (new SqlParameter ("@usertoid", usertoid));
					} else {
						sql.Add (new SqlParameter ("@usertoid", null));
					}

					publid = SSQLMgr.ExecuteScaler ("publication_insert", sql.ToArray ());
					break;
				}



				ok = publid > 0;

				if (pic != null && ok) {
					if (!InsertNewPicture (user, pic, publid)) {
						ok = false;
					}
				}

			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

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
		public static bool InsertNewPicture (User user, Picture pic, int publid)
		{

			bool ok = true;

			try {
				DataTable dt = new DataTable ();

				switch (dbType) {
				case "MySQL":
					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("ppublid", publid));
					mysql.Add (new MySqlParameter ("palbumid", pic.AlbumID));
					mysql.Add (new MySqlParameter ("ppicpath", pic.PicPath));
					mysql.Add (new MySqlParameter ("ppicdesc", string.IsNullOrEmpty (pic.PicDesc) ? "" : pic.PicDesc));
					mysql.Add (new MySqlParameter ("ppicname", pic.PicName));
					mysql.Add (new MySqlParameter ("ppicdate", pic.PicDate));
					mysql.Add (new MySqlParameter ("puserid", user.UserID));
					dt = MySQLMgr.ExecuteQuery ("picture_insert", "NewPicInfo", mysql.ToArray ());
					break;
				case "SSQL":
					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@publid", publid));
					sql.Add (new SqlParameter ("@albumid", pic.AlbumID));
					sql.Add (new SqlParameter ("@picpath", pic.PicPath));
					sql.Add (new SqlParameter ("@picdesc", string.IsNullOrEmpty (pic.PicDesc) ? "" : pic.PicDesc));
					sql.Add (new SqlParameter ("@picname", pic.PicName));
					sql.Add (new SqlParameter ("@picdate", pic.PicDate));
					sql.Add (new SqlParameter ("@userid", user.UserID));
					dt = SSQLMgr.ExecuteQuery ("picture_insert", "NewPicInfo", sql.ToArray ());
					break;
				}


				if (dt.Rows.Count > 0) {
					pic.AlbumID = Decimal.Parse (dt.Rows [0] [0].ToString ());
					pic.PicID = Decimal.Parse (dt.Rows [0] [1].ToString ());
				}


			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

			}

			return ok;

		}

		/// <summary>
		/// Gets user publications from database
		/// </summary>
		/// <param name="user"></param>
		/// <param name="requestList"></param>
		/// <returns></returns>
		public static bool GetUserPublications (User user, User userFrom, ref List<Publication> requestList)
		{
			bool ok = true;

			try {


				using (DataTable dt = new DataTable ()) {


					switch (dbType) {
					case "MySQL":
						List<MySqlParameter> mysql = new List<MySqlParameter> ();
						mysql.Add (new MySqlParameter ("puserid", user.UserID));
						mysql.Add (new MySqlParameter ("puserfrom", userFrom.UserID));
						dt.Merge (MySQLMgr.ExecuteQuery ("publication_getuserpublications", "UserPublications", mysql.ToArray ()));
						break;
					case "SSQL":
						List<SqlParameter> sql = new List<SqlParameter> ();
						sql.Add (new SqlParameter ("@userid", user.UserID));
						sql.Add (new SqlParameter ("@userfrom", userFrom.UserID));
						dt.Merge (SSQLMgr.ExecuteQuery ("publication_getuserpublications", "UserPublications", sql.ToArray ()));
						break;
					}

					if (dt != null && dt.Rows.Count > 0) {
						foreach (DataRow dr in dt.Rows) {

							Publication pub = new Publication () {
								PublDate = DateTime.Parse (dr ["publdate"].ToString ()),
								User = new User () {
									UserID = Decimal.Parse (dr ["userid"].ToString ()),
									UserName = dr ["username"].ToString (),
									UserSurname = dr ["usersurname"].ToString (),
									ProfileImage = dr ["profilepic"].ToString ()
								},
								PublDesc = dr ["publdesc"].ToString (),
								PublID = Decimal.Parse (dr ["publid"].ToString ()),
								RuleID = Decimal.Parse (dr ["ruleid"].ToString ())
							};

							try {
								GetUserPublicationImage (ref pub);
								GetUserPublicationComments (ref pub);
							} catch (Exception e) {
								Utilities.LogException ("GlobalDL", string.Format ("{0} - Error al recuperar las imágenes y comentarios de la publicación:{1}", MethodInfo.GetCurrentMethod ().Name, pub.PublID), e);
							}

							requestList.Add (pub);
						}
					}
				}

			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

			}
			return ok;
		}

		/// <summary>
		/// Gets image from a publication
		/// </summary>
		/// <param name="pub"></param>
		public static void GetUserPublicationImage (ref Publication pub)
		{
			try {





				using (DataTable dt = new DataTable ()) {

					switch (dbType) {
					case "MySQL":
						List<MySqlParameter> mysql = new List<MySqlParameter> ();
						mysql.Add (new MySqlParameter ("ppublid", pub.PublID));
						dt.Merge (MySQLMgr.ExecuteQuery ("publication_getuserpublicationimage", "UserPublicationsImage", mysql.ToArray ()));
						break;
					case "SSQL":
						List<SqlParameter> sql = new List<SqlParameter> ();
						sql.Add (new SqlParameter ("@publid", pub.PublID));
						dt.Merge (SSQLMgr.ExecuteQuery ("publication_getuserpublicationimage", "UserPublicationsImage", sql.ToArray ()));
						break;
					}


					if (dt != null && dt.Rows.Count > 0) {
						foreach (DataRow dr in dt.Rows) {

							pub.Image = new Picture () {

								AlbumID = Decimal.Parse (dr ["albumid"].ToString ()),
								PicDate = DateTime.Parse (dr ["picdate"].ToString ()),
								PicDesc = dr ["picdesc"].ToString (),
								PicID = Decimal.Parse (dr ["picid"].ToString ()),
								PicName = dr ["picdesc"].ToString (),
								PicPath = dr ["picpath"].ToString ()

							};

						}
					}
				}

			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
			} finally {

			}
		}

		/// <summary>
		/// Gets publications comments
		/// </summary>
		/// <param name="pub"></param>
		public static void GetUserPublicationComments (ref Publication pub)
		{
			try {



				using (DataTable dt = new DataTable ()) {

					pub.Comments = new List<Publication> ();


					switch (dbType) {
					case "MySQL":
						List<MySqlParameter> mysql = new List<MySqlParameter> ();
						mysql.Add (new MySqlParameter ("ppublid", pub.PublID));
						dt.Merge (MySQLMgr.ExecuteQuery ("publication_getuserpublicationcomments", "UserPublicationsComments", mysql.ToArray ()));
						break;
					case "SSQL":
						List<SqlParameter> sql = new List<SqlParameter> ();
						sql.Add (new SqlParameter ("@publid", pub.PublID));
						dt.Merge (SSQLMgr.ExecuteQuery ("publication_getuserpublicationcomments", "UserPublicationsComments", sql.ToArray ()));
						break;
					}



					if (dt != null && dt.Rows.Count > 0) {
						foreach (DataRow dr in dt.Rows) {


							Publication c = new Publication () {

								PublID = Decimal.Parse (dr ["commid"].ToString ()),
								PublDate = DateTime.Parse (dr ["commdate"].ToString ()),
								PublDesc = dr ["commdesc"].ToString (),
								RuleID = Decimal.Parse (dr ["ruleid"].ToString ()),
								User = new User () {
									UserID = Decimal.Parse (dr ["userid"].ToString ()),
									ProfileImage = dr ["picpath"].ToString (),
									UserName = dr ["username"].ToString (),
									UserSurname = dr ["usersurname"].ToString ()
								}

							};
							GetUserPublicationImage (ref c);

							pub.Comments.Add (c);
						}
					}
				}

			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
			} finally {

			}
		}

		/// <summary>
		/// Insert a new link request from an user into database
		/// </summary>
		/// <param name="userfrom"></param>
		/// <param name="userto"></param>
		/// <returns></returns>
		public static bool InsertNewLinkRequest (User userfrom, User userto)
		{
			bool ok = true;

			try {


				switch (dbType) {
				case "MySQL":
					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("puserfrom", userfrom.UserID));
					mysql.Add (new MySqlParameter ("puserto", userto.UserID));
					MySQLMgr.ExecuteNonQuery ("userlink_add", mysql.ToArray ());

					break;
				case "SSQL":
					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@userfrom", userfrom.UserID));
					sql.Add (new SqlParameter ("@userto", userto.UserID));
					SSQLMgr.ExecuteNonQuery ("userlink_add", sql.ToArray ());
					break;
				}


			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

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
		public static bool UpdateLinkRequest (User userFrom, User userto, Status status)
		{
			bool ok = true;

			try {


				switch (dbType) {
				case "MySQL":
					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("puserfrom", userFrom.UserID));
					mysql.Add (new MySqlParameter ("puserto", userto.UserID));
					mysql.Add (new MySqlParameter ("pstatus", (int)status));
					MySQLMgr.ExecuteNonQuery ("userlink_update", mysql.ToArray ());

					break;
				case "SSQL":
					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@userfrom", userFrom.UserID));
					sql.Add (new SqlParameter ("@userto", userto.UserID));
					sql.Add (new SqlParameter ("@status", (int)status));
					SSQLMgr.ExecuteNonQuery ("userlink_update", sql.ToArray ());
					break;
				}

			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

			}

			return ok;
		}

		/// <summary>
		/// Deletes a relationship between users
		/// </summary>
		/// <param name="userFrom"></param>
		/// <param name="userto"></param>
		/// <returns></returns>
		public static bool DeleteLinkRequest (User userFrom, User userto)
		{
			bool ok = true;

			try {


				switch (dbType) {
				case "MySQL":
					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("puserfrom", userFrom.UserID));
					mysql.Add (new MySqlParameter ("puserto", userto.UserID));
					MySQLMgr.ExecuteNonQuery ("userlink_delete", mysql.ToArray ());
					break;
				case "SSQL":
					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@userfrom", userFrom.UserID));
					sql.Add (new SqlParameter ("@userto", userto.UserID));
					SSQLMgr.ExecuteNonQuery ("userlink_delete", sql.ToArray ());
					break;
				}

			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

			}

			return ok;
		}

		/// <summary>
		/// Method to update user info
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public static bool UpdateUser (ref User user)
		{
			bool ok = true;

			try {


				switch (dbType) {
				case "MySQL":
					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("puserbirth", user.UserBirth));
					mysql.Add (new MySqlParameter ("puseremail", user.UserEmail));
					mysql.Add (new MySqlParameter ("puserlogin", user.UserLogin));
					mysql.Add (new MySqlParameter ("puserpass", user.UserPass));
					mysql.Add (new MySqlParameter ("pusername", user.UserName));
					mysql.Add (new MySqlParameter ("pusersurname", user.UserSurname));
					mysql.Add (new MySqlParameter ("puserid", user.UserID));
					MySQLMgr.ExecuteNonQuery ("user_updateinfo", mysql.ToArray ());

					break;
				case "SSQL":
					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@userbirth", user.UserBirth));
					sql.Add (new SqlParameter ("@useremail", user.UserEmail));
					sql.Add (new SqlParameter ("@userlogin", user.UserLogin));
					sql.Add (new SqlParameter ("@userpass", user.UserPass));
					sql.Add (new SqlParameter ("@username", user.UserName));
					sql.Add (new SqlParameter ("@usersurname", user.UserSurname));
					sql.Add (new SqlParameter ("@userid", user.UserID));
					SSQLMgr.ExecuteNonQuery ("user_updateinfo", sql.ToArray ());
					break;
				}

			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

			}

			return ok;
		}

		/// <summary>
		/// Method to get albums available from an user
		/// </summary>
		/// <param name="user"></param>
		/// <param name="albumList"></param>
		/// <returns></returns>
		public static bool GetAlbums (User userFrom, User userTo, ref List<Album> albumList)
		{
			bool ok = true;

			try {



				using (DataTable dt = new DataTable ()) {

					switch (dbType) {
					case "MySQL":
						List<MySqlParameter> mysql = new List<MySqlParameter> ();
						mysql.Add (new MySqlParameter ("puserfrom", userFrom.UserID));
						mysql.Add (new MySqlParameter ("puserid", userTo.UserID));
						dt.Merge (MySQLMgr.ExecuteQuery ("album_getuseralbums", "UserAlbums", mysql.ToArray ()));
						break;
					case "SSQL":
						List<SqlParameter> sql = new List<SqlParameter> ();
						sql.Add (new SqlParameter ("@userfrom", userFrom.UserID));
						sql.Add (new SqlParameter ("@userid", userTo.UserID));
						dt.Merge (SSQLMgr.ExecuteQuery ("album_getuseralbums", "UserAlbums", sql.ToArray ()));
						break;
					}

					if (dt != null && dt.Rows.Count > 0) {
						foreach (DataRow dr in dt.Rows) {

							albumList.Add (new Album () {
								UserID = Decimal.Parse (dr ["userid"].ToString ()),
								AlbumDesc = dr ["albumdesc"].ToString (),
								AlbumID = Decimal.Parse (dr ["albumid"].ToString ()),
								AlbumKeyPic = dr ["albumkeypic"].ToString (),
								AlbumName = dr ["albumname"].ToString (),
								IsProfile = int.Parse (dr ["isalbumprofile"].ToString ()) == 1,
								IsPublications = int.Parse (dr ["isalbumpublication"].ToString ()) == 1
							});

						}
					}
				}


			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				ok = false;
			} finally {

			}

			return ok;
		}

		/// <summary>
		/// Method to get pictures from an album
		/// </summary>
		/// <param name="album"></param>
		/// <param name="pictureList"></param>
		/// <returns></returns>
		public static bool GetPicturesFromAlbum (ref Album album, ref List<Picture> pictureList)
		{
			bool ok = true;

			try {


				using (DataTable dt = new DataTable ()) {

					switch (dbType) {
					case "MySQL":
						List<MySqlParameter> mysql = new List<MySqlParameter> ();
						mysql.Add (new MySqlParameter ("palbumid", album.AlbumID));

						dt.Merge (MySQLMgr.ExecuteQuery ("album_getpictures", "AlbumPictures", mysql.ToArray ()));
						break;
					case "SSQL":
						List<SqlParameter> sql = new List<SqlParameter> ();
						sql.Add (new SqlParameter ("@albumid", album.AlbumID));

						dt.Merge (SSQLMgr.ExecuteQuery ("album_getpictures", "AlbumPictures", sql.ToArray ()));
						break;
					}

					if (dt != null && dt.Rows.Count > 0) {
						foreach (DataRow dr in dt.Rows) {
							pictureList.Add (new Picture () {
								PicID = Decimal.Parse (dr ["picid"].ToString ()),
								PicDesc = dr ["picdesc"].ToString (),
								AlbumID = Decimal.Parse (dr ["albumid"].ToString ()),
								PicPath = dr ["picpath"].ToString (),
								PicDate = DateTime.Parse (dr ["picdate"].ToString ()),
							});

						}
					}
				}


			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				ok = false;
			} finally {

			}

			return ok;
		}

		/// <summary>
		/// Method to insert a new album into DB
		/// </summary>
		/// <param name="album"></param>
		/// <returns></returns>
		public static bool InsertNewAlbum (ref Album album)
		{
			bool ok = true;

			try {



				switch (dbType) {
				case "MySQL":

					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("puserid", album.UserID));
					mysql.Add (new MySqlParameter ("palbumname", album.AlbumName));
					mysql.Add (new MySqlParameter ("palbumdesc", album.AlbumDesc));
					if (album.RuleID > 0)
						mysql.Add (new MySqlParameter ("pruleid", album.RuleID));
					album.AlbumID = MySQLMgr.ExecuteScaler ("album_newalbum", mysql.ToArray ());		
					break;
				case "SSQL":
					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@userid", album.UserID));
					sql.Add (new SqlParameter ("@albumname", album.AlbumName));
					sql.Add (new SqlParameter ("@albumdesc", album.AlbumDesc));
					if (album.RuleID > 0)
						sql.Add (new SqlParameter ("pruleid", album.RuleID));
					album.AlbumID = SSQLMgr.ExecuteScaler ("album_newalbum", sql.ToArray ());
					break;
				}

				ok = album.AlbumID > 0;


			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				ok = false;
			} finally {

			}

			return ok;
		}

		/// <summary>
		/// Inserts new schedule register into DB
		/// </summary>
		/// <param name="schd"></param>
		/// <returns></returns>
		public static bool InsertNewSchedule (ref Schedule schd)
		{
			bool ok = true;

			try {



				switch (dbType) {
				case "MySQL":

					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("puserid", schd.UserID));
					mysql.Add (new MySqlParameter ("pruleid", schd.RuleID));
					mysql.Add (new MySqlParameter ("pscheddesc", schd.SchedDesc));
					mysql.Add (new MySqlParameter ("pschedtitle", schd.SchedTitle));
					mysql.Add (new MySqlParameter ("pschedtypeid", (int)schd.EventType));
					mysql.Add (new MySqlParameter ("pscheddate", schd.SchedDate));
					mysql.Add (new MySqlParameter ("pschedrepeat", schd.SchedRepeat));
					mysql.Add (new MySqlParameter ("pschedbookingid", schd.SchedBooking));

					schd.SchedID = MySQLMgr.ExecuteScaler ("schedule_new", mysql.ToArray ());		
					break;
				case "SSQL":
					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@userid", schd.UserID));
					sql.Add (new SqlParameter ("@ruleid", schd.RuleID));
					sql.Add (new SqlParameter ("@scheddesc", schd.SchedDesc));
					sql.Add (new SqlParameter ("@schedtitle", schd.SchedTitle));
					sql.Add (new SqlParameter ("@schedtypeid", (int)schd.EventType));
					sql.Add (new SqlParameter ("@scheddate", schd.SchedDate));
					sql.Add (new SqlParameter ("@schedrepeat", schd.SchedRepeat));
					sql.Add (new SqlParameter ("@schedbookingid", schd.SchedBooking));

					schd.SchedID = SSQLMgr.ExecuteScaler ("schedule_new", sql.ToArray ());	
					break;

				}

				ok = schd.SchedID > 0;

			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				ok = false;
			} finally {

			}

			return ok;
		}

		public static bool InsertNewStudentRequest (int eventID, decimal userID)
		{
			bool ok = true;

			try {



				switch (dbType) {
				case "MySQL":

					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("puserid", userID));
					mysql.Add (new MySqlParameter ("pschedid", eventID));

					MySQLMgr.ExecuteNonQuery ("schedulelink_insert", mysql.ToArray ());		
					break;
				case "SSQL":
					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@userid", userID));
					sql.Add (new SqlParameter ("@schedid", eventID));

					SSQLMgr.ExecuteNonQuery ("schedulelink_insert", sql.ToArray ());	
					break;

				}
					
			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				ok = false;
			} finally {

			}

			return ok;
		}

		/// <summary>
		/// Inserts new schedule link with user into DB
		/// </summary>
		/// <param name="schd"></param>
		/// <param name="userto"></param>
		/// <returns></returns>
		public static bool InsertNewScheduleLink (ref Schedule schd, Decimal userto)
		{
			bool ok = true;

			try {



				switch (dbType) {
				case "MySQL":

					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("puserid", userto));
					mysql.Add (new MySqlParameter ("pschdid", schd.SchedID));

					ok = MySQLMgr.ExecuteScaler ("schedulelink_new", mysql.ToArray ()) > 0;		
					break;
				case "SSQL":
					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@userid", userto));
					sql.Add (new SqlParameter ("@schdid", schd.SchedID));

					ok = SSQLMgr.ExecuteScaler ("schedulelink_new", sql.ToArray ()) > 0;	
					break;
				}


			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				ok = false;
			} finally {

			}

			return ok;
		}

		/// <summary>
		/// Gets schedule list from an user
		/// </summary>
		/// <param name="user"></param>
		/// <param name="scheduleList"></param>
		/// <returns></returns>
		public static bool GetScheduleFromUser (ref User user, ref List<Schedule> scheduleList, ref List<StudentSchedule> studentList)
		{
			bool ok = true;

			try {



				using (DataSet ds = new DataSet ()) {

					switch (dbType) {
					case "MySQL":

						List<MySqlParameter> mysql = new List<MySqlParameter> ();
						mysql.Add (new MySqlParameter ("puserid", user.UserID));
						ds.Merge (MySQLMgr.ExecuteQuery ("schedulelink_select", mysql.ToArray ()));
						break;
					case "SSQL":
						List<SqlParameter> sql = new List<SqlParameter> ();
						sql.Add (new SqlParameter ("@userid", user.UserID));
						ds.Merge (SSQLMgr.ExecuteQuery ("schedulelink_select", sql.ToArray ()));
						break;
					}

					if (ds != null && ds.Tables.Count > 0) {
						foreach (DataRow dr in ds.Tables[0].Rows) {
							scheduleList.Add (new Schedule () {
								SchedID = Decimal.Parse (dr ["schedid"].ToString ()),
								SchedDesc = dr ["scheddesc"].ToString (),
								UserID = Decimal.Parse (dr ["userid"].ToString ()),
								EventType = (EventType)(int.Parse (dr ["schedtypeid"].ToString ())),
								SchedTitle = dr ["schedtitle"].ToString (),
								SchedDate = DateTime.Parse (dr ["scheddate"].ToString ()),
								SchedBooking = (int)dr ["schedbookingId"]
							});

						}

						if (ds.Tables.Count > 1) {
							foreach (DataRow dr in ds.Tables[1].Rows) {
							studentList.Add (new StudentSchedule () {
									SchedID = Decimal.Parse (dr ["schedid"].ToString ()),
									UserID = Decimal.Parse (dr ["userid"].ToString ()),
									UserName = dr ["username"].ToString (),
									UserSurname = dr ["usersurname"].ToString (),
									SchedDate = DateTime.Parse (dr ["scheddate"].ToString ()),
									Points = Decimal.Parse(dr["puntos"].ToString())
								});

							}
						}

					}



				}


			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				ok = false;
			} finally {

			}

			return ok;
		}

		/// <summary>
		/// Sets a schedule as finished
		/// </summary>
		/// <param name="schedule"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		public static bool FinishSchedule (Schedule schedule, User user)
		{
			bool ok = true;

			try {


				switch (dbType) {
				case "MySQL":

					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("puserid", user.UserID));
					mysql.Add (new MySqlParameter ("pschedid", schedule.SchedID));
					MySQLMgr.ExecuteNonQuery ("schedule_finish", mysql.ToArray ());		
					break;
				case "SSQL":
					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@userid", user.UserID));
					sql.Add (new SqlParameter ("@schedid", schedule.SchedID));
					SSQLMgr.ExecuteNonQuery ("schedule_finish", sql.ToArray ());	
					break;
				}

			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

			}

			return ok;
		}

		/// <summary>
		/// Deletes an image and return fille PicPath property from pic parameter with deleted image path
		/// </summary>
		/// <param name="pic"></param>
		/// <returns></returns>
		public static bool DeleteImage (ref Picture pic)
		{
			bool ok = true;

			try {

				using (DataTable dt = new DataTable ()) {

					switch (dbType) {
					case "MySQL":

						List<MySqlParameter> mysql = new List<MySqlParameter> ();
						mysql.Add (new MySqlParameter ("ppicid", pic.PicID));
						mysql.Add (new MySqlParameter ("palbumid", pic.AlbumID));			
						dt.Merge (MySQLMgr.ExecuteQuery ("picture_delete", "DeletePath", mysql.ToArray ()));
						break;
					case "SSQL":
						List<SqlParameter> sql = new List<SqlParameter> ();
						sql.Add (new SqlParameter ("@picid", pic.PicID));
						sql.Add (new SqlParameter ("@albumid", pic.AlbumID));			
						dt.Merge (SSQLMgr.ExecuteQuery ("picture_delete", "DeletePath", sql.ToArray ()));
						break;
					}

					if (dt.Rows.Count > 0) {
						pic.PicPath = dt.Rows [0] [0].ToString ();
					}
				}

			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

			}

			return ok;
		}

		/// <summary>
		/// Gets a list of the latest uploaded images from linked users
		/// </summary>
		/// <param name="user"></param>
		/// <param name="latestPicturesList"></param>
		/// <returns></returns>
		public static bool GetLatestImages (User user, ref List<Album> latestPicturesList)
		{
			bool ok = true;

			try {

				using (DataTable dt = new DataTable ()) {

					switch (dbType) {
					case "MySQL":

						List<MySqlParameter> mysql = new List<MySqlParameter> ();
						mysql.Add (new MySqlParameter ("puserid", user.UserID));			
						dt.Merge (MySQLMgr.ExecuteQuery ("picture_getlatestpictures", "LatestPictures", mysql.ToArray ()));
						break;
					case "SSQL":
						List<SqlParameter> sql = new List<SqlParameter> ();
						sql.Add (new SqlParameter ("@userid", user.UserID));			
						dt.Merge (SSQLMgr.ExecuteQuery ("picture_getlatestpictures", "LatestPictures", sql.ToArray ()));
						break;
					}

					if (dt.Rows.Count > 0) {
						foreach (DataRow dr in dt.Rows) {
							latestPicturesList.Add (new Album () {
								AlbumDesc = dr ["albumdesc"].ToString (),
								AlbumID = Decimal.Parse (dr ["albumid"].ToString ()),
								AlbumKeyPic = dr ["picpath"].ToString (),
								AlbumName = dr ["albumname"].ToString (),
								UserID = Decimal.Parse (dr ["userid"].ToString ())
							});
						}
                        
					}
				}

			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

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
		public static bool GetUserMessagesWith (User userFrom, User userTo, ref List<Publication> requestList)
		{
			bool ok = true;

			try {

				using (DataTable dt = new DataTable ()) {

					switch (dbType) {
					case "MySQL":

						List<MySqlParameter> mysql = new List<MySqlParameter> ();
						mysql.Add (new MySqlParameter ("puserFrom", userFrom.UserID));
						mysql.Add (new MySqlParameter ("puserTo", userTo.UserID));			
						dt.Merge (MySQLMgr.ExecuteQuery ("message_select", "Messages", mysql.ToArray ()));
						break;
					case "SSQL":
						List<SqlParameter> sql = new List<SqlParameter> ();
						sql.Add (new SqlParameter ("@userFrom", userFrom.UserID));
						sql.Add (new SqlParameter ("@userTo", userTo.UserID));			
						dt.Merge (SSQLMgr.ExecuteQuery ("message_select", "Messages", sql.ToArray ()));
						break;
					}

					if (dt.Rows.Count > 0) {
						foreach (DataRow dr in dt.Rows) {
							requestList.Add (new Publication () {
								PublDesc = dr ["publDesc"].ToString (),
								PublID = Decimal.Parse (dr ["publid"].ToString ()),
								PublDate = DateTime.Parse (dr ["publdate"].ToString ()),
								User = new User () {
									UserID = Decimal.Parse (dr ["userid"].ToString ()),
									ProfileImage = dr ["profilepic"].ToString ()
								},
								Comments = new List<Publication> ()
							});
						}

					}
				}

			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

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
		public static bool InsertNewMessage (User userFrom, User userTo, string msg)
		{
			bool ok = true;

			try {


				switch (dbType) {
				case "MySQL":

					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("puserFrom", userFrom.UserID));
					mysql.Add (new MySqlParameter ("puserTo", userTo.UserID));
					mysql.Add (new MySqlParameter ("pmessagetxt", msg));
					MySQLMgr.ExecuteNonQuery ("message_insert", mysql.ToArray ());		

					break;
				case "SSQL":
					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@userFrom", userFrom.UserID));
					sql.Add (new SqlParameter ("@userTo", userTo.UserID));
					sql.Add (new SqlParameter ("@messagetxt", msg));
					SSQLMgr.ExecuteNonQuery ("message_insert", sql.ToArray ());
					break;
				}


			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

			}

			return ok;
		}

		/// <summary>
		/// Insert new rule for an user and return its id
		/// </summary>
		/// <param name="rule"></param>
		/// <returns></returns>
		public static bool InsertNewRule (ref CSM.Classes.Rule rule)
		{
			bool ok = true;

			try {

				using (DataTable dt = new DataTable ()) {

					switch (dbType) {
					case "MySQL":

						List<MySqlParameter> mysql = new List<MySqlParameter> ();
						mysql.Add (new MySqlParameter ("pruletypeid", (int)rule.RuleTypeID));
						mysql.Add (new MySqlParameter ("puserid", rule.UserID));
						mysql.Add (new MySqlParameter ("prulename", rule.RuleName));
						mysql.Add (new MySqlParameter ("pruledesc", rule.RuleDesc));			
						dt.Merge (MySQLMgr.ExecuteQuery ("rule_insert", "InsertedRule", mysql.ToArray ()));
						break;
					case "SSQL":
						List<SqlParameter> sql = new List<SqlParameter> ();
						sql.Add (new SqlParameter ("@ruletypeid", (int)rule.RuleTypeID));
						sql.Add (new SqlParameter ("@userid", rule.UserID));
						sql.Add (new SqlParameter ("@rulename", rule.RuleName));
						sql.Add (new SqlParameter ("@ruledesc", rule.RuleDesc));			
						dt.Merge (SSQLMgr.ExecuteQuery ("rule_insert", "InsertedRule", sql.ToArray ()));
						break;
					}

					if (dt.Rows.Count > 0) {
						rule.RuleID = Decimal.Parse (dt.Rows [0] [0].ToString ());
					}
				}

			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

			}

			return ok;
		}

		/// <summary>
		/// Gets user rule list from db
		/// </summary>
		/// <param name="user"></param>
		/// <param name="ruleList"></param>
		/// <returns></returns>
		public static bool GetRulesFromUser (User user, ref List<CSM.Classes.Rule> ruleList)
		{
			bool ok = true;

			try {

				using (DataTable dt = new DataTable ()) {

					switch (dbType) {
					case "MySQL":

						List<MySqlParameter> mysql = new List<MySqlParameter> ();
						mysql.Add (new MySqlParameter ("puserid", user.UserID));
						dt.Merge (MySQLMgr.ExecuteQuery ("rule_select", "RulesFromUser", mysql.ToArray ()));
						break;
					case "SSQL":
						List<SqlParameter> sql = new List<SqlParameter> ();
						sql.Add (new SqlParameter ("@userid", user.UserID));
						dt.Merge (SSQLMgr.ExecuteQuery ("rule_select", "RulesFromUser", sql.ToArray ()));
						break;
					}


					if (dt.Rows.Count > 0) {
						foreach (DataRow dr in dt.Rows) {
							ruleList.Add (new CSM.Classes.Rule () {
								RuleDesc = dr ["ruleDesc"].ToString (),
								RuleID = Decimal.Parse (dr ["ruleid"].ToString ()),
								RuleName = dr ["rulename"].ToString (),
								RuleTypeID = Utilities.GetRuleTypeID (dr ["ruletypeid"].ToString ())

							});
						}

					}
				}

			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

			}

			return ok;
		}

		/// <summary>
		/// Inserts new privacy into db
		/// </summary>
		/// <param name="privacy"></param>
		/// <returns></returns>
		public static bool InsertNewPrivacy (ref Privacy privacy)
		{
			bool ok = true;

			try {

				foreach (string u in privacy.Users) {


					switch (dbType) {
					case "MySQL":

						List<MySqlParameter> mysql = new List<MySqlParameter> ();
						mysql.Add (new MySqlParameter ("pruleid", privacy.RuleID));
						mysql.Add (new MySqlParameter ("puserid", int.Parse (u)));
						mysql.Add (new MySqlParameter ("pprivname", privacy.PrivName));
						mysql.Add (new MySqlParameter ("pprivoptionid", privacy.PrivOptionID));
						MySQLMgr.ExecuteNonQuery ("privacy_insert", mysql.ToArray ());		
						break;
					case "SSQL":
						List<SqlParameter> sql = new List<SqlParameter> ();
						sql.Add (new SqlParameter ("@ruleid", privacy.RuleID));
						sql.Add (new SqlParameter ("@userid", int.Parse (u)));
						sql.Add (new SqlParameter ("@privname", privacy.PrivName));
						sql.Add (new SqlParameter ("@privoptionid", privacy.PrivOptionID));
						SSQLMgr.ExecuteNonQuery ("privacy_insert", sql.ToArray ());		
						break;
					}

				}

			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

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
		public static bool GetRulesFromUser (User userFrom, User userTo, ref List<CSM.Classes.Rule> ruleList)
		{
			bool ok = true;

			try {

				using (DataTable dt = new DataTable ()) {

					switch (dbType) {
					case "MySQL":

						List<MySqlParameter> mysql = new List<MySqlParameter> ();
						mysql.Add (new MySqlParameter ("puserFrom", userFrom.UserID));
						mysql.Add (new MySqlParameter ("puserTo", userTo.UserID));			
						dt.Merge (MySQLMgr.ExecuteQuery ("rules_getrulesfromuser", "ListRulesFromUser", mysql.ToArray ()));
						break;
					case "SSQL":
						List<MySqlParameter> sql = new List<MySqlParameter> ();
						sql.Add (new MySqlParameter ("@userFrom", userFrom.UserID));
						sql.Add (new MySqlParameter ("@userTo", userTo.UserID));			
						dt.Merge (SSQLMgr.ExecuteQuery ("rules_getrulesfromuser", "ListRulesFromUser", sql.ToArray ()));
						break;
					}


					if (dt.Rows.Count > 0) {
						foreach (DataRow dr in dt.Rows) {
							ruleList.Add (new CSM.Classes.Rule () {
								RuleDesc = dr ["ruleDesc"].ToString (),
								RuleID = Decimal.Parse (dr ["ruleid"].ToString ()),
								RuleName = dr ["rulename"].ToString (),
								RuleTypeID = Utilities.GetRuleTypeID (dr ["ruletypeid"].ToString ())

							});
						}

					}
				}

			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

			}

			return ok;
		}

		/// <summary>
		/// Gets rule info
		/// </summary>
		/// <param name="rule"></param>
		/// <param name="privacy"></param>
		/// <returns></returns>
		public static bool GetRuleInfo (ref CSM.Classes.Rule rule, ref Privacy privacy)
		{
			bool ok = true;

			try {

				using (DataTable dt = new DataTable ()) {

					switch (dbType) {
					case "MySQL":

						List<MySqlParameter> mysql = new List<MySqlParameter> ();
						mysql.Add (new MySqlParameter ("pruleid", rule.RuleID));			
						dt.Merge (MySQLMgr.ExecuteQuery ("rules_getruleinfo", "RulesInfo", mysql.ToArray ()));
						break;
					case "SSQL":
						List<SqlParameter> sql = new List<SqlParameter> ();
						sql.Add (new SqlParameter ("@ruleid", rule.RuleID));			
						dt.Merge (SSQLMgr.ExecuteQuery ("rules_getruleinfo", "RulesInfo", sql.ToArray ()));
						break;
					}


					if (dt.Rows.Count > 0) {

						rule.RuleName = dt.Rows [0] ["rulename"].ToString ();
						rule.RuleDesc = dt.Rows [0] ["ruledesc"].ToString ();
						rule.RuleTypeID = Utilities.GetRuleTypeID (dt.Rows [0] ["ruletypeid"].ToString ());
						privacy.Users = new List<string> ();

						foreach (DataRow dr in dt.Rows) {
							privacy.Users.Add (dr ["userto"].ToString ());
						}

					}
				}

			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

			}

			return ok;
		}

		/// <summary>
		/// Modify rule info
		/// </summary>
		/// <param name="rule"></param>
		/// <returns></returns>
		public static bool ModifyRule (ref CSM.Classes.Rule rule)
		{
			bool ok = true;

			try {


				switch (dbType) {
				case "MySQL":

					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("pruletypeid", (int)rule.RuleTypeID));
					mysql.Add (new MySqlParameter ("puserid", rule.UserID));
					mysql.Add (new MySqlParameter ("prulename", rule.RuleName));
					mysql.Add (new MySqlParameter ("pruledesc", rule.RuleDesc));
					mysql.Add (new MySqlParameter ("pruleid", rule.RuleID));
					MySQLMgr.ExecuteNonQuery ("rule_modify", mysql.ToArray ());		
					break;
				case "SSQL":
					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@ruletypeid", (int)rule.RuleTypeID));
					sql.Add (new SqlParameter ("@userid", rule.UserID));
					sql.Add (new SqlParameter ("@rulename", rule.RuleName));
					sql.Add (new SqlParameter ("@ruledesc", rule.RuleDesc));
					sql.Add (new SqlParameter ("@ruleid", rule.RuleID));
					SSQLMgr.ExecuteNonQuery ("rule_modify", sql.ToArray ());	
					break;
				}


			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

			}

			return ok;
		}

		/// <summary>
		/// Modifies privacy from a rule
		/// </summary>
		/// <param name="privacy"></param>
		/// <returns></returns>
		public static bool ModifyPrivacy (ref Privacy privacy)
		{
			bool ok = true;

			try {

				switch (dbType) {
				case "MySQL":

					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("pruleid", privacy.RuleID));
					MySQLMgr.ExecuteNonQuery ("privacy_delete", mysql.ToArray ());
					break;
				case "SSQL":

					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@ruleid", privacy.RuleID));
					SSQLMgr.ExecuteNonQuery ("privacy_delete", sql.ToArray ());
					break;
				}

				foreach (string u in privacy.Users) {


					switch (dbType) {
					case "MySQL":

						List<MySqlParameter> mysql = new List<MySqlParameter> ();
						mysql.Add (new MySqlParameter ("pruleid", privacy.RuleID));
						mysql.Add (new MySqlParameter ("puserid", int.Parse (u)));
						mysql.Add (new MySqlParameter ("pprivname", privacy.PrivName));
						mysql.Add (new MySqlParameter ("pprivoptionid", privacy.PrivOptionID));
						MySQLMgr.ExecuteNonQuery ("privacy_insert", mysql.ToArray ());		
						break;
					case "SSQL":
						List<SqlParameter> sql = new List<SqlParameter> ();
						sql.Add (new SqlParameter ("pruleid", privacy.RuleID));
						sql.Add (new SqlParameter ("puserid", int.Parse (u)));
						sql.Add (new SqlParameter ("pprivname", privacy.PrivName));
						sql.Add (new SqlParameter ("pprivoptionid", privacy.PrivOptionID));
						SSQLMgr.ExecuteNonQuery ("privacy_insert", sql.ToArray ());	
						break;
					}


				}

                

			} catch (Exception e) {
				Utilities.LogException ("GlobalDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

			}

			return ok;
		}
	}
}
