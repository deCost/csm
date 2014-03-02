using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using CSM.Classes;
using MySql.Data.MySqlClient;
using CSM;

namespace CSM.DataLayer
{
    public class RegisterFormDL
    {

        public static bool InsertRegisterForm(User user)
        {
            bool ok = true;
            try
            {
                bool exist = false;
                ok = CheckUserExists(user, ref exist);

                if (!exist && ok)
                {

                    List<MySqlParameter> parameters = new List<MySqlParameter>();

                    parameters.Add(new MySqlParameter("puseraddress", user.UserAddress));
                    parameters.Add(new MySqlParameter("puserbirth", user.UserBirth));
                    parameters.Add(new MySqlParameter("puseremail", user.UserEmail));
                    parameters.Add(new MySqlParameter("puserlogin", user.UserLogin));
                    parameters.Add(new MySqlParameter("pusername", user.UserName));
                    parameters.Add(new MySqlParameter("puserpass", user.UserPass));
                    parameters.Add(new MySqlParameter("pusersurname", user.UserSurname));
                    parameters.Add(new MySqlParameter("pstatus", (int)user.StatuID));
                    ok = SQLMgr.ExecuteScaler("user_register", parameters.ToArray()) > 0;
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
            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();

                parameters.Add(new MySqlParameter("puseremail", user.UserEmail));
                parameters.Add(new MySqlParameter("puserlogin", user.UserLogin));
                switch (SQLMgr.ExecuteScaler("user_exists", parameters.ToArray()))
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
