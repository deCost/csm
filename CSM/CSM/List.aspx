<%@ Page Language="C#" Inherits="CSM.List" MasterPageFile="~/Master/Private.Master" %>
<%@ MasterType VirtualPath="~/Master/Private.Master" %>

<%@ Register Src="~/Control/UserScheduleList.ascx" TagName="sched" TagPrefix="uc" %>

<asp:Content ContentPlaceHolderID="bodyContent" ID="bodyContentContent" runat="server">
	<div class="eleven columns">
		<uc:sched runat="server" id="schedule" />
	</div>
	<div class="two columns">
		<div class="trampabottle"></div>
	</div>

</asp:Content>
<asp:Content ContentPlaceHolderID="scripts" ID="scriptsContent" runat="server">
</asp:Content>


