<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopBar.ascx.cs" Inherits="CSM.Control.TopBar" %>

<div class="topbar sixteen columns">
	<div class="sixteen columns">
		<div class="compositionMini left"></div>
		<div class="compositionMini middle fifteen columns">
			<asp:LinkButton runat="server" id="lknLogout" OnClick="lnkLogout_Click" CssClass="logoMini right">
				Logout
				<img src="images/trampa.jpg" width="25" height="25" />
			</asp:LinkButton>
			<asp:HyperLink runat="server" id="btnHome" NavigateUrl="~/Home.aspx" CssClass="logoMini">
				Home 
				<img src="images/logo35.jpg" width="25" height="25" />
			</asp:HyperLink>
			<asp:HyperLink runat="server" id="btnList" NavigateUrl="~/List.aspx?fn=e">Eventos</asp:HyperLink> | 
			<asp:HyperLink runat="server" id="btnClassroom" NavigateUrl="~/List.aspx?fn=c">Clases</asp:HyperLink> |
			<asp:HyperLink runat="server" id="btnSettings" NavigateUrl="~/Settings.aspx">Mis datos</asp:HyperLink> | 
			<asp:HyperLink runat="server" id="btnSchedule" NavigateUrl="~/CreateSchedule.aspx">Crear evento</asp:HyperLink> | 
		</div>
		<div class="compositionMini right"></div>
	</div>
</div>