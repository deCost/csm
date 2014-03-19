using System;
using CSM.Common;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using CSM.DataLayer;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;
using System.Data;
using CSM.Classes;

namespace CSM.DataAccess
{
	public class ClassRoomDL
	{


		private static string dbType = ConfigurationManager.AppSettings ["DBType"];

		public ClassRoomDL ()
		{

		}

		#region Program

		/// <summary>
		/// Modifies privacy from a rule
		/// </summary>
		/// <param name="privacy"></param>
		/// <returns></returns>
		public static bool ProgramInsert (ref Program program)
		{
			bool ok = true;

			try {

				switch (dbType) {
				case "MySQL":

					List<MySqlParameter> mysql = new List<MySqlParameter> ();

					mysql.Add (new MySqlParameter ("pprogramname", program.Name));
					mysql.Add (new MySqlParameter ("pprogramdesc", program.Desc));
					mysql.Add (new MySqlParameter ("pprogramdate", DateTime.Now));
					mysql.Add (new MySqlParameter ("plevel", (int)program.Level));
					MySQLMgr.ExecuteNonQuery ("program_insert", mysql.ToArray ());
					break;
				case "SSQL":

					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@programname", program.Name));
					sql.Add (new SqlParameter ("@programdesc", program.Desc));
					sql.Add (new SqlParameter ("@programdate", DateTime.Now));
					sql.Add (new SqlParameter ("@level", (int)program.Level));
					SSQLMgr.ExecuteNonQuery ("program_insert", sql.ToArray ());
					break;
				}
					
			} catch (Exception e) {
				Utilities.LogException ("ClassRoomDL", MethodInfo.GetCurrentMethod ().Name, e);
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
		public static bool ProgramUpdate (ref Program program)
		{
			bool ok = true;

			try {

				switch (dbType) {
				case "MySQL":

					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("pprogramid", program.ID));
					mysql.Add (new MySqlParameter ("pprogramname", program.Name));
					mysql.Add (new MySqlParameter ("pprogramdesc", program.Desc));
					mysql.Add (new MySqlParameter ("pprogramdate", program.Date));
					mysql.Add (new MySqlParameter ("plevel", (int)program.Level));
					MySQLMgr.ExecuteNonQuery ("program_update", mysql.ToArray ());
					break;
				case "SSQL":

					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@programid", program.ID));
					sql.Add (new SqlParameter ("@programname", program.Name));
					sql.Add (new SqlParameter ("@programdesc", program.Desc));
					sql.Add (new SqlParameter ("@programdate", program.Date));
					sql.Add (new SqlParameter ("@level", (int)program.Level));
					SSQLMgr.ExecuteNonQuery ("program_update", sql.ToArray ());
					break;
				}

			} catch (Exception e) {
				Utilities.LogException ("ClassRoomDL", MethodInfo.GetCurrentMethod ().Name, e);
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
		public static bool ProgramDelete (ref Program program)
		{
			bool ok = true;

			try {

				switch (dbType) {
				case "MySQL":

					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("pprogramid", program.ID));
					MySQLMgr.ExecuteNonQuery ("program_delete", mysql.ToArray ());
					break;
				case "SSQL":

					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@programid", program.ID));
					SSQLMgr.ExecuteNonQuery ("program_delete", sql.ToArray ());
					break;
				}

			} catch (Exception e) {
				Utilities.LogException ("ClassRoomDL", MethodInfo.GetCurrentMethod ().Name, e);
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
		public static bool GetPrograms (ref List<Program> lstprogram)
		{
			bool ok = true;

			try {
				DataTable dt = new DataTable ();
				switch (dbType) {
				case "MySQL":

					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					dt = MySQLMgr.ExecuteQuery ("program_select", "Programs", mysql.ToArray ());
					break;
				case "SSQL":

					List<SqlParameter> sql = new List<SqlParameter> ();
					dt = SSQLMgr.ExecuteQuery ("program_select", "Programs", sql.ToArray ());
					break;
				}

				if (dt != null) {
					foreach (DataRow dr in dt.Rows) {
						lstprogram.Add (new Program () {
							ID = (int)dr ["programid"],
							Name = dr ["programname"].ToString (),
							Desc = dr ["programdesc"].ToString (),
							Date = (DateTime)dr ["programdate"],
							Level = (Level)(int)dr ["level"],
						});
					}
				} else {
					throw new Exception ("No se obtenieron datos");
				}



			} catch (Exception e) {
				Utilities.LogException ("ClassRoomDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

			}

			return ok;
		}

		#endregion

		#region Subject

		/// <summary>
		/// Modifies privacy from a rule
		/// </summary>
		/// <param name="privacy"></param>
		/// <returns></returns>
		public static bool SubjectInsert (ref Subject subject)
		{
			bool ok = true;

			try {

				switch (dbType) {
				case "MySQL":

					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("pprogramid", subject.ProgramID));
					mysql.Add (new MySqlParameter ("pclassname", subject.Name));
					mysql.Add (new MySqlParameter ("pclassdesc", subject.Desc));
					MySQLMgr.ExecuteNonQuery ("class_insert", mysql.ToArray ());
					break;
				case "SSQL":

					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@programid", subject.ProgramID));
					sql.Add (new SqlParameter ("@classname", subject.Name));
					sql.Add (new SqlParameter ("@classdesc", subject.Desc));
					SSQLMgr.ExecuteNonQuery ("class_insert", sql.ToArray ());
					break;
				}

			} catch (Exception e) {
				Utilities.LogException ("ClassRoomDL", MethodInfo.GetCurrentMethod ().Name, e);
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
		public static bool SubjectUpdate (ref Subject subject)
		{
			bool ok = true;

			try {

				switch (dbType) {
				case "MySQL":

					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("pprogramid", subject.ProgramID));
					mysql.Add (new MySqlParameter ("pclassid", subject.SubjectID));
					mysql.Add (new MySqlParameter ("pclassname", subject.Name));
					mysql.Add (new MySqlParameter ("pclassdesc", subject.Desc));
					MySQLMgr.ExecuteNonQuery ("class_update", mysql.ToArray ());
					break;
				case "SSQL":

					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@programid", subject.ProgramID));
					sql.Add (new SqlParameter ("@classid", subject.SubjectID));
					sql.Add (new SqlParameter ("@classname", subject.Name));
					sql.Add (new SqlParameter ("@classdesc", subject.Desc));
					SSQLMgr.ExecuteNonQuery ("class_update", sql.ToArray ());
					break;
				}

			} catch (Exception e) {
				Utilities.LogException ("ClassRoomDL", MethodInfo.GetCurrentMethod ().Name, e);
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
		public static bool SubjectDelete (ref Subject subject)
		{
			bool ok = true;

			try {

				switch (dbType) {
				case "MySQL":

					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("pclassid", subject.SubjectID));
					MySQLMgr.ExecuteNonQuery ("class_delete", mysql.ToArray ());
					break;
				case "SSQL":

					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@classid", subject.SubjectID));
					SSQLMgr.ExecuteNonQuery ("class_delete", sql.ToArray ());
					break;
				}

			} catch (Exception e) {
				Utilities.LogException ("ClassRoomDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

			}

			return ok;
		}

		public static bool GetSubjects (ref Program program, ref List<Subject> lstSubject)
		{
			bool ok = true;

			try {
				DataTable dt = new DataTable ();
				switch (dbType) {
				case "MySQL":

					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					dt = MySQLMgr.ExecuteQuery ("class_select", "Classes", mysql.ToArray ());
					break;
				case "SSQL":

					List<SqlParameter> sql = new List<SqlParameter> ();
					dt = SSQLMgr.ExecuteQuery ("class_select", "Classes", sql.ToArray ());
					break;
				}

				if (dt != null) {
				foreach (DataRow dr in dt.Rows) {
					lstSubject.Add (new Subject () {
						SubjectID = (int)dr ["classid"],
						Name = dr ["classname"].ToString (),
						Desc = dr ["classdesc"].ToString (),
						ProgramID =  (int)dr ["programid"]
					});
				}
				} else {
					throw new Exception ("No se obtenieron datos");
				}



			} catch (Exception e) {
				Utilities.LogException ("ClassRoomDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

			}

			return ok;
		}

		#endregion

		#region Group

		/// <summary>
		/// Modifies privacy from a rule
		/// </summary>
		/// <param name="privacy"></param>
		/// <returns></returns>
		public static bool GroupInsert (ref Group group)
		{
			bool ok = true;

			try {

				switch (dbType) {
				case "MySQL":

					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("pprogramid", group.ProgramID));
					mysql.Add (new MySqlParameter ("pgroupname", group.Name));
					mysql.Add (new MySqlParameter ("pgroupdate", group.Date));
					mysql.Add (new MySqlParameter ("plevel", (int)group.Level));
					mysql.Add (new MySqlParameter ("presponsible", group.Responsible.UserID));
					MySQLMgr.ExecuteNonQuery ("group_insert", mysql.ToArray ());
					break;
				case "SSQL":

					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@programid", group.ProgramID));
					sql.Add (new SqlParameter ("@groupname", group.Name));
					sql.Add (new SqlParameter ("@groupdate", group.Date));
					sql.Add (new SqlParameter ("@level", (int)group.Level));
					sql.Add (new SqlParameter ("@responsible", group.Responsible.UserID));
					SSQLMgr.ExecuteNonQuery ("group_insert", sql.ToArray ());
					break;
				}

			} catch (Exception e) {
				Utilities.LogException ("ClassRoomDL", MethodInfo.GetCurrentMethod ().Name, e);
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
		public static bool GroupUpdate (ref Group group)
		{
			bool ok = true;

			try {

				switch (dbType) {
				case "MySQL":

					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("pgroupid", group.ID));
					mysql.Add (new MySqlParameter ("pprogramid", group.ProgramID));
					mysql.Add (new MySqlParameter ("pgroupname", group.Name));
					mysql.Add (new MySqlParameter ("plevel", (int)group.Level));
					mysql.Add (new MySqlParameter ("presponsible", group.Responsible.UserID));
					MySQLMgr.ExecuteNonQuery ("group_update", mysql.ToArray ());
					break;
				case "SSQL":

					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@groupid", group.ID));
					sql.Add (new SqlParameter ("@programid", group.ProgramID));
					sql.Add (new SqlParameter ("@groupname", group.Name));
					sql.Add (new SqlParameter ("@level", (int)group.Level));
					sql.Add (new SqlParameter ("@responsible", group.Responsible.UserID));
					SSQLMgr.ExecuteNonQuery ("group_update", sql.ToArray ());
					break;
				}

			} catch (Exception e) {
				Utilities.LogException ("ClassRoomDL", MethodInfo.GetCurrentMethod ().Name, e);
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
		public static bool GroupDelete (ref Group group)
		{
			bool ok = true;

			try {

				switch (dbType) {
				case "MySQL":

					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("pgroupid", group.ID));
					MySQLMgr.ExecuteNonQuery ("group_delete", mysql.ToArray ());
					break;
				case "SSQL":

					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@groupid", group.ID));
					SSQLMgr.ExecuteNonQuery ("group_delete", sql.ToArray ());
					break;
				}

			} catch (Exception e) {
				Utilities.LogException ("ClassRoomDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

			}

			return ok;
		}

		#endregion

		#region GroupUser

		/// <summary>
		/// Modifies privacy from a rule
		/// </summary>
		/// <param name="privacy"></param>
		/// <returns></returns>
		public static bool GroupUserInsert (ref User user)
		{
			bool ok = true;

			try {

				switch (dbType) {
				case "MySQL":

					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("pgroupid", user.Group.ID));
					mysql.Add (new MySqlParameter ("puserid", user.UserID));
					mysql.Add (new MySqlParameter ("pstatusid", (int) Status.Active));
					MySQLMgr.ExecuteNonQuery ("groupuser_insert", mysql.ToArray ());
					break;
				case "SSQL":

					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@groupid", user.Group.ID));
					sql.Add (new SqlParameter ("@userid", user.UserID));
					sql.Add (new SqlParameter ("@statusid", (int) Status.Active));
					SSQLMgr.ExecuteNonQuery ("groupuser_insert", sql.ToArray ());
					break;
				}

			} catch (Exception e) {
				Utilities.LogException ("ClassRoomDL", MethodInfo.GetCurrentMethod ().Name, e);
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
		public static bool GroupUserUpdate (ref User user, Status status)
		{
			bool ok = true;

			try {

				switch (dbType) {
				case "MySQL":

					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("pgroupid", user.Group.ID));
					mysql.Add (new MySqlParameter ("puserid", user.UserID));
					mysql.Add (new MySqlParameter ("pstatusid", (int) status));
					MySQLMgr.ExecuteNonQuery ("groupuser_update", mysql.ToArray ());
					break;
				case "SSQL":

					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@groupid", user.Group.ID));
					sql.Add (new SqlParameter ("@userid", user.UserID));
					sql.Add (new SqlParameter ("@statusid", (int) status));
					SSQLMgr.ExecuteNonQuery ("groupuser_update", sql.ToArray ());
					break;
				}

			} catch (Exception e) {
				Utilities.LogException ("ClassRoomDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

			}

			return ok;
		}

		#endregion

		#region Lesson

		/// <summary>
		/// Modifies privacy from a rule
		/// </summary>
		/// <param name="privacy"></param>
		/// <returns></returns>
		public static bool LessonInsert (ref Lesson lesson)
		{
			bool ok = true;

			try {

				switch (dbType) {
				case "MySQL":

					List<MySqlParameter> mysql = new List<MySqlParameter> ();
					mysql.Add (new MySqlParameter ("pclassid", lesson.ClassID));
					mysql.Add (new MySqlParameter ("plessondate", lesson.Date));
					mysql.Add (new MySqlParameter ("plessonuser", lesson.Responsible));
					mysql.Add (new MySqlParameter ("pgroupid", lesson.GroupID));
					MySQLMgr.ExecuteNonQuery ("lesson_insert", mysql.ToArray ());
					break;
				case "SSQL":

					List<SqlParameter> sql = new List<SqlParameter> ();
					sql.Add (new SqlParameter ("@classid", lesson.ClassID));
					sql.Add (new SqlParameter ("@lessondate", lesson.Date));
					sql.Add (new SqlParameter ("@lessonuser", lesson.Responsible));
					sql.Add (new SqlParameter ("@groupid", lesson.GroupID));
					SSQLMgr.ExecuteNonQuery ("lesson_insert", sql.ToArray ());
					break;
				}

			} catch (Exception e) {
				Utilities.LogException ("ClassRoomDL", MethodInfo.GetCurrentMethod ().Name, e);
				return false;
			} finally {

			}

			return ok;
		}

		#endregion
	}
}

