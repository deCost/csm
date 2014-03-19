using System;
using CSM.DataAccess;
using CSM.Common;
using System.Collections.Generic;
using CSM.Classes;

namespace CSM.DataManager
{
	public class ClassRoomBS
	{


		#region Program

		/// <summary>
		/// Modifies privacy from a rule
		/// </summary>
		/// <param name="privacy"></param>
		/// <returns></returns>
		public static bool ProgramInsert (ref Program program)
		{
			return ClassRoomDL.ProgramInsert (ref program);
		}

		/// <summary>
		/// Modifies privacy from a rule
		/// </summary>
		/// <param name="privacy"></param>
		/// <returns></returns>
		public static bool ProgramUpdate (ref Program program)
		{
			return ClassRoomDL.ProgramUpdate (ref program);
		}

		/// <summary>
		/// Modifies privacy from a rule
		/// </summary>
		/// <param name="privacy"></param>
		/// <returns></returns>
		public static bool ProgramDelete (ref Program program)
		{
			return ClassRoomDL.ProgramDelete (ref program);
		}

		/// <summary>
		/// Modifies privacy from a rule
		/// </summary>
		/// <param name="privacy"></param>
		/// <returns></returns>
		public static bool GetPrograms (ref List<Program> lstprogram)
		{
			return ClassRoomDL.GetPrograms (ref lstprogram);
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
			return ClassRoomDL.SubjectInsert (ref subject);
		}

		/// <summary>
		/// Modifies privacy from a rule
		/// </summary>
		/// <param name="privacy"></param>
		/// <returns></returns>
		public static bool SubjectUpdate (ref Subject subject)
		{
			return ClassRoomDL.SubjectUpdate (ref subject);
		}

		/// <summary>
		/// Modifies privacy from a rule
		/// </summary>
		/// <param name="privacy"></param>
		/// <returns></returns>
		public static bool SubjectDelete (ref Subject subject)
		{
			return ClassRoomDL.SubjectDelete (ref subject);
		}

		public static bool GetSubjects (ref Program program, ref List<Subject> lstSubject)
		{
			return ClassRoomDL.GetSubjects (ref program, ref lstSubject);
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
			return ClassRoomDL.GroupInsert (ref group);
		}

		/// <summary>
		/// Modifies privacy from a rule
		/// </summary>
		/// <param name="privacy"></param>
		/// <returns></returns>
		public static bool GroupUpdate (ref Group group)
		{
			return ClassRoomDL.GroupUpdate (ref group);
		}

		/// <summary>
		/// Modifies privacy from a rule
		/// </summary>
		/// <param name="privacy"></param>
		/// <returns></returns>
		public static bool GroupDelete (ref Group group)
		{
			return ClassRoomDL.GroupDelete (ref group);
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
			return ClassRoomDL.GroupUserInsert (ref user);
		}

		/// <summary>
		/// Modifies privacy from a rule
		/// </summary>
		/// <param name="privacy"></param>
		/// <returns></returns>
		public static bool GroupUserUpdate (ref User user, Status status)
		{
			return ClassRoomDL.GroupUserUpdate (ref user, status);
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
			return ClassRoomDL.LessonInsert (ref lesson);
		}

		#endregion
	}
}

