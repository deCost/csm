<%@ Page Language="C#" MasterPageFile="~/Master/Public.Master" AutoEventWireup="true" Inherits="CSM.Default" %>

<asp:Content ID="Content" ContentPlaceHolderID="bodyContent" runat="server">

	
 <div class="sprite caja">
 	<label for="txtUser">User</label>
 	<asp:TextBox runat="server" id="txtUser" ClientIDMode="Static" />
 	<label for="txtPass">Pass</label>
 	<asp:TextBox runat="server" id="txtPass" TextMode="Password" ClientIDMode="Static" />
 	<asp:Button runat="server" Text="Entrar" ID="btnLogin" OnClick="btnLogin_Click" />
 	o <asp:HyperLink NavigateUrl="Register.aspx" runat="server" id="lnkRegister">Crea tu cuenta aquí</asp:HyperLink>
 	<asp:Label runat="server" id="lblMsg" />
 </div>
	 
</asp:Content>	