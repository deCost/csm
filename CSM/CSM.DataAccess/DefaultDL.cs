using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using CSM.Classes;
using MySql.Data.MySqlClient;
using System.Data;
using CSM;
using CSM.DataAccess;
using System.Configuration;
using System.Data.SqlClient;

namespace CSM.DataLayer
{
	public class DefaultDL
	{
		private static string dbType = ConfigurationManager.AppSettings ["DBType"];

		/// <summary>
		/// Method to do login.
		/// </summary>
		/// <param name="user">Object with info to login. Once logged, it return user info</param>
		/// <returns>Method goes fine</returns>
		public static bool ProcessLoginForm (ref User user)
		{
			bool ok = true;
			try {
				DataTable dt = new DataTable ();

				switch (dbType) {
				case "MySQL":
					List<MySqlParameter> mysql = new List<MySqlParameter> ();

					mysql.Add (new MySqlParameter ("puserlogin", user.UserLogin));
					mysql.Add (new MySqlParameter ("puserpass", user.UserPass));
					dt = MySQLMgr.ExecuteQuery ("user_login", "login", mysql.ToArray ());		
					break;
				case "SSQL":
					List<SqlParameter> sql = new List<SqlParameter> ();

					sql.Add (new SqlParameter ("@userlogin", user.UserLogin));
					sql.Add (new SqlParameter ("@userpass", user.UserPass));
					dt = SSQLMgr.ExecuteQuery ("user_login", "login", sql.ToArray ());		
					break;
				}


                
				//Check results
				if (dt.Rows.Count > 0) {
					//Check no issues
					user.UserName = dt.Rows [0] ["name"].ToString ();
					user.UserSurname = dt.Rows [0] ["surname"].ToString ();
					user.UserEmail = dt.Rows [0] ["email"].ToString ();
					user.UserID = Decimal.Parse (dt.Rows [0] ["id"].ToString ());
					user.UserBirth = DateTime.Parse (dt.Rows [0] ["birthdate"].ToString ());
					user.UserAddress = dt.Rows [0] ["address"].ToString ();
					user.StatuID = Status.Active;
					user.SessionID = Utilities.EncodeMD5 (Guid.NewGuid ().ToString ());
					user.LoginDate = DateTime.Now;
					user.ProfileImage = dt.Rows [0] ["picpath"].ToString ();
					user.AlbumProfileID = Decimal.Parse (dt.Rows [0] ["albumprofile"].ToString ());
					user.AlbumPublID = Decimal.Parse (dt.Rows [0] ["albumpublication"].ToString ());
					user.IsAdmin = decimal.Parse (dt.Rows [0] ["useradmin"].ToString ()) > 0;
					user.LastDate = (DateTime)dt.Rows [0] ["lastDate"];
					user.TotalPerformance = Decimal.Parse (dt.Rows [0] ["totalperformance"].ToString ());
                   
				} else {
					throw new WrongDataException ("Los datos facilitados no coinciden con ningún usuario de nuestra base de datos");
				}

                

			} catch (WrongDataException e) {
				// We ge foward the exception
				throw e;
			} catch (Exception ex) {
				Utilities.LogException ("DefaultDL", MethodInfo.GetCurrentMethod ().Name, ex);
				return false;
			} finally {

			}
			return ok;

            
		}

		/// <summary>
		/// Method to change the pass of an user
		/// </summary>
		/// <param name="user"></param>
		/// <returns>Method goes fine</returns>
		public static bool ChangePass (ref User user)
		{
			bool ok = true;
			try {

				switch (dbType) {
				case "MySQL":
					List<MySqlParameter> mysql = new List<MySqlParameter> ();

					mysql.Add (new MySqlParameter ("puseremail", user.UserEmail));
					mysql.Add (new MySqlParameter ("puserlogin", user.UserLogin));
					mysql.Add (new MySqlParameter ("pnewpass", user.UserPass));
					MySQLMgr.ExecuteNonQuery ("user_newpass", mysql.ToArray ());		
					break;
				case "SSQL":
					List<SqlParameter> sql = new List<SqlParameter> ();

					sql.Add (new SqlParameter ("@useremail", user.UserEmail));
					sql.Add (new SqlParameter ("@userlogin", user.UserLogin));
					sql.Add (new SqlParameter ("@newpass", user.UserPass));
					SSQLMgr.ExecuteNonQuery ("user_newpass", sql.ToArray ());		
					break;
				}



			} catch (Exception e) {
				Utilities.LogException ("DefaultDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

			}
			return ok;
		}
	}
}
