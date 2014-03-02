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
