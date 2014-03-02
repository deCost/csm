using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using CSM.Classes;
using MySql.Data.MySqlClient;
using System.Data;
using CSM;


namespace CSM.DataLayer
{
    public class DefaultDL
    {
        /// <summary>
        /// Method to do login.
        /// </summary>
        /// <param name="user">Object with info to login. Once logged, it return user info</param>
        /// <returns>Method goes fine</returns>
        public static bool ProcessLoginForm(ref User user)
        {
            bool ok = true;
            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();

                parameters.Add(new MySqlParameter("puserlogin", user.UserLogin));
                parameters.Add(new MySqlParameter("puserpass", user.UserPass));

                using (DataTable dt = SQLMgr.ExecuteQuery("user_login", "login", parameters.ToArray()))
                {
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
                        user.StatuID = Status.Active;
                        user.SessionID = Utilities.EncodeMD5(Guid.NewGuid().ToString());
                        user.LoginDate = DateTime.Now;
                        user.ProfileImage = dt.Rows[0]["picpath"].ToString();
                        user.AlbumProfileID = Decimal.Parse(dt.Rows[0]["albumprofile"].ToString());
                        user.AlbumPublID = Decimal.Parse(dt.Rows[0]["albumpublication"].ToString());
                       
                    }
                    else
                    {
                        throw new WrongDataException("Los datos facilitados no coinciden con ningún usuario de nuestra base de datos");
                    }

                }

            }
            catch (WrongDataException e)
            {
                // We ge foward the exception
                throw e;
            }
			catch (Exception ex)
            {
				Utilities.LogException("DefaultDL", MethodInfo.GetCurrentMethod().Name, ex);
                return false;
            }
            finally
            {

            }
            return ok;

            
        }

        /// <summary>
        /// Method to change the pass of an user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Method goes fine</returns>
        public static bool ChangePass(ref User user)
        {
            bool ok = true;
            try
            {
                List<MySqlParameter> parameters = new List<MySqlParameter>();

                parameters.Add(new MySqlParameter("puseremail", user.UserEmail));
                parameters.Add(new MySqlParameter("puserlogin", user.UserLogin));
                parameters.Add(new MySqlParameter("pnewpass", user.UserPass));
                SQLMgr.ExecuteNonQuery("user_newpass", parameters.ToArray());

            }
            catch (Exception e)
            {
                Utilities.LogException("DefaultDL", MethodInfo.GetCurrentMethod().Name, e);
                return false;
            }
            finally
            {

            }
            return ok;
        }
    }
}
