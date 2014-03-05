<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopBar.ascx.cs" Inherits="CSM.Control.TopBar" %>

<div class="topbar sixteen columns">
	<div class="sixteen columns">
		<div class="compositionMini left"></div>
		<div class="compositionMini middle fifteen columns">
			<asp:LinkButton runat="server" id="lknLogout" OnClick="lnkLogout_Click" CssClass="logoMini right">
				Logout
			</asp:LinkButton>
			<asp:HyperLink runat="server" id="btnHome" NavigateUrl="~/Home.aspx" CssClass="logoMini">
				Home 
				<img src="images/logo35.jpg" width="25" height="25" />
			</asp:HyperLink>
			
			<asp:Repeater runat="server" id="rptMenu">
				<ItemTemplate>
					<asp:HyperLink runat="server" id="btnNavigate" CssClass="linkMenu" NavigateUrl='<%#Eval("Value")%>'><%#Eval("Key")%> | </asp:HyperLink> 
				</ItemTemplate>
			</asp:Repeater>


		</div>
		<div class="compositionMini right"></div>
	</div>
</div>
<div class="clear"></div>
<asp:DropDownList runat="server" id="drpMenu" ClientIDMode="Static" CssClass="sixteen columns" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="drpMenu_Change" />
<div class="clear"></div>