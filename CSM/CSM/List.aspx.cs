using CSM.Classes;
using CSM.Master;
using CMS.DataManager;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Reflection;
using System.IO;
using System.Text;


namespace CSM
{
	using System;
	using System.Web;
	using System.Web.UI;

	public partial class List : System.Web.UI.Page
	{
		private User _user;

		public int CurrentPage
		{
			get
			{
				// look for current page in ViewState
				object o = this.ViewState["_CurrentPage"];
				if (o == null)
					return 0; // default page index of 0
				else
					return (int)o;
			}

			set
			{
				this.ViewState["_CurrentPage"] = value;
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{

			Private privateFunctions = new Private ();
			User user = null;

			if (!privateFunctions.isLoggedSession (ref user)) {
				Response.Redirect ("~/");
			}

			if (!string.IsNullOrWhiteSpace (Request ["fn"])) {
				switch (Request ["fn"]) {
				case "e":
					litTitle.Text = "3, 2, 1... Impro!";
					schedule.lstEventToShow.Add (EventType.MartesAlternos);
					pnlUsers.Visible = linkeds.Visible = false;
					break;
				case "c":
					litTitle.Text = "Impro Training";
					schedule.lstEventToShow.Add (EventType.Clase);
					pnlUsers.Visible = linkeds.Visible = false;
					break;
				case "a":
					litTitle.Text = "Compimpros";
					schedule.Visible = false;
					linkeds.ProfileUser = user;
					linkeds.isMyProfile = true;
					pnlUsers.Visible = false;
					break;
				case "user":
					UserList ();
					schedule.Visible = linkeds.Visible = false;
					break;

				}
			} else {
			}
		}

		private void UserList()
		{
			// By using private.Master public mehods, we don't need to create a utilities class for website
			Private privateFunctions = new Private();

			try
			{
				List<UserLink> requestList = new List<UserLink>();


				if (string.IsNullOrEmpty(Request["search"]))
				{

					if(string.IsNullOrEmpty(Request["userid"]))
					{
						privateFunctions.isLoggedSession(ref _user);
						// Gets users with connection request pending
						if (!GlobalBS.GetUsersLinkBy(ref _user, ref requestList, Status.Active))
						{
							throw new WrongDataException("Lo sentimos pero ocurrió un error al recuperar los usuarios enlazados");
						}

						// Gets users with connection request pending
						if (!GlobalBS.GetUsersLinkBy(ref _user, ref requestList, Status.Pending))
						{
							throw new WrongDataException("Lo sentimos pero ocurrió un error al recuperar los usuarios enlazados");
						}

					}else{

						Decimal userid;
						if (Decimal.TryParse(Request["userid"], out userid))
						{
							_user = new User() { UserID = userid };
							if (GlobalBS.GetUser(ref _user))
							{

								// Gets users with connection request pending
								if (!GlobalBS.GetUsersLinkBy(ref _user, ref requestList, Status.Active))
								{
									throw new WrongDataException("Lo sentimos pero ocurrió un error al recuperar los usuarios enlazados");
								}

								// Gets users with connection request pending
								if (!GlobalBS.GetUsersLinkBy(ref _user, ref requestList, Status.Pending))
								{
									throw new WrongDataException("Lo sentimos pero ocurrió un error al recuperar los usuarios enlazados");
								}
							}
							else
							{
								throw new WrongDataException("Lo sentimos pero ocurrió un error al recuperar el usuario solicitado");
							}
						}
						else
						{
							throw new WrongDataException("El usuario solicitado no es correcto");
						}


					}
					lblContentList.Text = string.Format("Amigos de {0}", _user.Name);

				}
				else
				{
					// Gets users matches search
					if (!GlobalBS.GetUsersLike(Request["search"],ref requestList))
					{
						throw new WrongDataException("Lo sentimos pero ocurrió un error al recuperar los usuarios enlazados");
					}

					lblContentList.Text = string.Format("Buscado: {0}", Request["search"]);

				}

				// Recover number of user connections request
				if (requestList.Count > 0)
				{
					PagedDataSource pg = new PagedDataSource();

					pg.DataSource = requestList;
					pg.PageSize = 15;
					pg.CurrentPageIndex = CurrentPage;

					rptList.DataSource = pg;
					rptList.DataBind();

				}
				else
				{
					rptList.DataSource = null;
					rptList.DataBind();
					responseTxt.Visible = true;
				}

			}
			catch (WrongDataException ex)
			{
				//Script register to show exception info
				ScriptManager.RegisterStartupScript(this, this.GetType(), "showMsg", string.Format(@"jsError('{0}');});", ex.Message), true);
			}
			catch (Exception ex)
			{
				//Script register to show exception info
				ScriptManager.RegisterStartupScript(this, this.GetType(), "showMsg", @"jsError('Lo sentimos pero ha ocurrido un error inexperado');", true);
				Utilities.LogException(Path.GetFileName(Request.Path),
					MethodInfo.GetCurrentMethod().Name,
					ex);
			}
		}

		/// <summary>
		/// Method to write actions for the user
		/// </summary>
		/// <param name="u"></param>
		/// <returns></returns>
		protected string ShowActions(UserLink u)
		{

			StringBuilder str = new StringBuilder("");
			switch(u.StatusID)
			{
			case Status.Active:
				str.Append("<div class=\"message\" onclick=\"document.location.href='Messages.aspx?userid=" + u.UserIDReq + "'\"></div>");
				str.Append("<div class=\"delete\" onclick=\"UpdateLinkStatus(" + u.UserIDReq + ",-1);\"></div>");
				break;
			case Status.Pending:
				str.Append("<div class=\"accept\" onclick=\"UpdateLinkStatus(" + u.UserIDReq + ",1);\"></div>");
				str.Append("<div class=\"delete\" onclick=\"UpdateLinkStatus(" + u.UserIDReq + ",-1);\"></div>");
				break;
			default:
				str.Append("<div class=\"add\" onclick=\"UpdateLinkStatus(" + u.UserIDReq + ",0);\"></div>");
				break;
			}

			return string.IsNullOrEmpty(Request["search"]) ? str.ToString() : "";
		}

		/// <summary>
		/// Row command event handler
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		protected void btnCommand_Click(object sender, EventArgs e)
		{
			Decimal userid = 0;


			Private privateFunctions = new Private();
			User user = new User();
			try
			{
				if (Decimal.TryParse(e.ToString(), out userid))
				{
					if (privateFunctions.isLoggedSession(ref user))
					{

						if (!GlobalBS.InsertNewLinkRequest(user, new User() { UserID = userid }))
						{
							throw new WrongDataException("No se ha podido completar la petición. Por favor, inténtelo más tarde");
						}

						string msg = "Su petición ha sido registrada con éxito. Cuando el usuario te acepte, te lo notificaremos.";
						//Script register to show exception info
						ScriptManager.RegisterStartupScript(this, this.GetType(), "showMsg", string.Format(@"jsAlert({0});", msg), true);
					}
					else
					{
						throw new WrongDataException("No se ha podido recuperar el usuario actual. Por favor, refresque la página");
					}
				}
				else
				{
					throw new WrongDataException("No se ha podido recuperar el usuario actual. Por favor, refresque la página");
				}

			}
			catch (WrongDataException ex)
			{
				//Script register to show exception info
				ScriptManager.RegisterStartupScript(this, this.GetType(), "showMsg", string.Format(@"jsAlert('{0}');", ex.Message), true);
				responseTxt.Text = ex.Message;
				return;
			}
			catch (Exception ex)
			{
				//Script register to show exception info
				ScriptManager.RegisterStartupScript(this, this.GetType(), "showMsg", @"jsError('Lo sentimos pero ha ocurrido un error inexperado');", true);

				responseTxt.Text = "Lo sentimos pero ha ocurrido un error inexperado";

				Utilities.LogException(Path.GetFileName(Request.Path),
					MethodInfo.GetCurrentMethod().Name,
					ex);
				return;
			}
		}
	}
}

