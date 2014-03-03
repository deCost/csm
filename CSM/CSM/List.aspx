<%@ Page Language="C#" Inherits="CSM.List" MasterPageFile="~/Master/Private.Master" %>
<%@ MasterType VirtualPath="~/Master/Private.Master" %>

<%@ Register Src="~/Control/UserScheduleList.ascx" TagName="sched" TagPrefix="uc" %>
<%@ Register Src="~/Control/UserLinkedList.ascx" TagName="links" TagPrefix="uc" %>

<asp:Content ContentPlaceHolderID="bodyContent" ID="bodyContentContent" runat="server">
	<div class="eleven columns">
		<h1><asp:Literal runat="server" id="litTitle" /></h1>
		<uc:sched runat="server" id="schedule" />
		<uc:links runat="server" id="linkeds" />
	</div>
	<div class="two columns">
		<div class="trampabottle"></div>
	</div>

</asp:Content>
<asp:Content ContentPlaceHolderID="scripts" ID="scriptsContent" runat="server">
</asp:Content>


