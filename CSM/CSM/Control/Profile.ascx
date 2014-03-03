<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Profile.ascx.cs" Inherits="CSM.Control.Profile" %>

<%if(isMyProfile){%>
<div class="myprofile">
<%}else{%>
<div class="profile">
<%}%>
	<div class="portfolio">
		<div class="image">
			<asp:Image runat="server" ID="profileimage" CssClass="pic" />
		</div>
		<div class="clear"></div>
		<%if(isMyProfile){%>
		<h4>Ola k ase,</h4>
		<%}else{%>
		<h4>Planta un personaje a</h4>
		<%}%>
		<h2><%=ProfileUser.UserName%></h2>
	</div>
</div>
<asp:Panel runat="server" ID="connect" CssClass="bubble">
	<asp:LinkButton runat="server" ID="btnLink" OnClick="btnLink_Click">
    	<h3>Quiero salir a escena con este energúmeno!</h3>
    </asp:LinkButton>
	<h3 runat="server" id="titPending"><%=ProfileUser.UserName%> por dió! Acepta mi solicitud de amistad...</h3>
</asp:Panel>
<%if (isMyProfile)
              { %>
<div class="notificationmenu hidden">
	<div class="leftarrow"></div>
	<ul>
		<li class="friend"><a href="List.aspx?fn=a"><asp:Literal ID="txtfriendrequest" runat="server"></asp:Literal></a></li>
	</ul>
</div>
<%}%>
