using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using CSM.Classes;
using MySql.Data.MySqlClient;
using CSM;
using System.Data.SqlClient;
using System.Configuration;

namespace CSM.DataLayer
{
    public class RegisterFormDL
    {
		private static string dbType = ConfigurationManager.AppSettings ["DBType"];

        public static bool InsertRegisterForm(User user)
        {
            bool ok = true;
            try
            {
                bool exist = false;
                ok = CheckUserExists(user, ref exist);

                if (!exist && ok)
                {

					switch (dbType) {
					case "MySQL":
						List<MySqlParameter> mysql = new List<MySqlParameter>();

						mysql.Add(new MySqlParameter("puseraddress", user.UserAddress));
						mysql.Add(new MySqlParameter("puserbirth", user.UserBirth));
						mysql.Add(new MySqlParameter("puseremail", user.UserEmail));
						mysql.Add(new MySqlParameter("puserlogin", user.UserLogin));
						mysql.Add(new MySqlParameter("pusername", user.UserName));
						mysql.Add(new MySqlParameter("puserpass", user.UserPass));
						mysql.Add(new MySqlParameter("pusersurname", user.UserSurname));
						mysql.Add(new MySqlParameter("pstatus", (int)user.StatuID));
						ok = MySQLMgr.ExecuteScaler("user_register", mysql.ToArray()) > 0;		
						break;
					case "SSQL":
						List<SqlParameter> sql = new List<SqlParameter>();

						sql.Add(new SqlParameter("@useraddress", user.UserAddress));
						sql.Add(new SqlParameter("@userbirth", user.UserBirth));
						sql.Add(new SqlParameter("@useremail", user.UserEmail));
						sql.Add(new SqlParameter("@userlogin", user.UserLogin));
						sql.Add(new SqlParameter("@username", user.UserName));
						sql.Add(new SqlParameter("@userpass", user.UserPass));
						sql.Add(new SqlParameter("@usersurname", user.UserSurname));
						sql.Add(new SqlParameter("@status", (int)user.StatuID));
						ok = SSQLMgr.ExecuteScaler("user_register", sql.ToArray()) > 0;		
						break;
					}

                    
                }
                else
                {
                    if (user.UserEmail == "###")
                    {
                        throw new WrongDataException("El email ya existe en nuestro sistema");
                    }

                    if (user.UserLogin == "###")
                    {
                        throw new WrongDataException("El usuario de login ya existe en nuestro sistema");
                    }
                }
            }
            catch (WrongDataException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                Utilities.LogException("RegisterFormDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {
                
            }
            return ok;

            
        }

        /// <summary>
        /// Method to check if an user exists
        /// </summary>
        /// <param name="user">User object to be checked</param>
        /// <param name="exists">Response of user exists</param>
        /// <returns>Method goes fine</returns>
        public static bool CheckUserExists(User user, ref bool exists)
        {
            bool ok = true;
			int res = 0;
            try
            {
				switch (dbType) {
				case "MySQL":
					List<MySqlParameter> mysql = new List<MySqlParameter>();

					mysql.Add(new MySqlParameter("puseremail", user.UserEmail));
					mysql.Add(new MySqlParameter("puserlogin", user.UserLogin));
					res = MySQLMgr.ExecuteScaler("user_exists", mysql.ToArray());
					break;
				case "SSQL":
					List<SqlParameter> sql = new List<SqlParameter>();

					sql.Add(new SqlParameter("@useremail", user.UserEmail));
					sql.Add(new SqlParameter("@userlogin", user.UserLogin));
					res = SSQLMgr.ExecuteScaler("user_exists", sql.ToArray());		
					break;
				}

				switch (res)
                {
                    case 0:
                        exists = false;
                        break;
                    case 1:
                        user.UserEmail = "###";
                        exists = true;
                        ok = false;
                        break;
                    case 2:
                        user.UserLogin = "###";
                        exists = true;
                        ok = false;
                        break;
                    case 3:
                        user.UserEmail = "###";
                        user.UserLogin = "###";
                        exists = true;
                        ok = false;
                        break;
                }
            }
            catch (Exception e)
            {
                Utilities.LogException("RegisterFormDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

            }
            return ok;


        }


    }
}
