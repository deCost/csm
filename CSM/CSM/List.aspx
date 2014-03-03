<%@ Page Language="C#" Inherits="CSM.List" MasterPageFile="~/Master/Private.Master" %>
<%@ MasterType VirtualPath="~/Master/Private.Master" %>

<%@ Register Src="~/Control/UserScheduleList.ascx" TagName="sched" TagPrefix="uc" %>
<%@ Register Src="~/Control/UserLinkedList.ascx" TagName="links" TagPrefix="uc" %>

<asp:Content ContentPlaceHolderID="bodyContent" ID="bodyContentContent" runat="server">
	<div class="eleven columns">
		<h1><asp:Literal runat="server" id="litTitle" /></h1>
		<uc:sched runat="server" id="schedule" />
		<uc:links runat="server" id="linkeds" />
		<asp:Panel runat="server" id="pnlUsers">
			<h1><asp:Label ID="lblContentList" runat="server"></asp:Label></h1>
	        <asp:Repeater ID="rptList" Runat="server">
	            <HeaderTemplate>

	            </HeaderTemplate>
	            <ItemTemplate>
	                
					    <img src="images/friend.png" /> 
	                    <a href="Profile.aspx?user=<%# DataBinder.Eval(Container.DataItem, "UserIDReq") %>"><%# DataBinder.Eval(Container.DataItem, "Name") %></a>
					    <div class="actions">
	                        <%# ShowActions(((CSM.Classes.UserLink)Container.DataItem))%>
	                        
					    </div>
				    
	            </ItemTemplate>
	            <FooterTemplate>
	                
	            </FooterTemplate>
	        </asp:Repeater>
	        <asp:Label ID="responseTxt" runat="server" Visible="false">No se encontraron resultados que coincidan con los parámetros solicitados</asp:Label>

		</asp:Panel>
	</div>
	<div class="two columns">
		<div class="trampabottle"></div>
	</div>

</asp:Content>
<asp:Content ContentPlaceHolderID="scripts" ID="scriptsContent" runat="server">
</asp:Content>


