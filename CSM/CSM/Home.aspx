<%@ Page Language="C#" Inherits="CSM.Home" MasterPageFile="~/Master/Private.Master" EnableEventValidation="false" %>
<%@ MasterType VirtualPath="~/Master/Private.Master" %>

<%@ Register Src="~/Control/Profile.ascx" TagName="profile" TagPrefix="uc" %>
<%@ Register Src="~/Control/UserBubblesList.ascx" TagName="bubbles" TagPrefix="uc" %>
<%@ Register Src="~/Control/PublicationTools.ascx" TagName="tools" TagPrefix="uc" %>

<asp:Content ContentPlaceHolderID="bodyContent" ID="bodyContentContent" runat="server">

<div class="eight columns">
	<uc:profile runat="server" id="profile" />
	<uc:tools runat="server" id="tools" />
</div>
<div class="seven columns">
	<uc:bubbles runat="server" id="listBubbles" />
</div>
<asp:HiddenField ID="hdnuserid" ClientIDMode="Static" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="scripts" ID="scriptsContent" runat="server">
</asp:Content>


