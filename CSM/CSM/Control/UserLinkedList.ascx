<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserLinkedList.ascx.cs" Inherits="CSM.Control.UserLinkedList" %>
<div class="module">
    <asp:Label runat="server" ID="lblUserName" CssClass="bold"></asp:Label>
    <%if (this.ProfileUser != null && this.ProfileUser.UserID != null)
      { %>
      <ul class="friendlist">
	<asp:Repeater ID="repLinked" runat="server">
        <HeaderTemplate>
            
        </HeaderTemplate>
        <ItemTemplate>
            <li class="connected"><a href="Profile.aspx?user=<%# DataBinder.Eval(Container.DataItem, "UserIDReq") %>"><%# DataBinder.Eval(Container.DataItem, "Name") %></a></li>
		</ItemTemplate>
        <FooterTemplate>
            
        </FooterTemplate>
    </asp:Repeater>
</ul>
	        <a href="List.aspx?fn=user&userid=<%=this.ProfileUser.UserID %>"><%=repLinked.Items.Count %> amigos...</a>
    <%} %>
    <asp:Panel runat="server" ID="txtNoFriends">
        <asp:Image runat="server" ID="imgNoFriends" ImageUrl="~/images/foreveralone.jpg" />
        <div class="clear"></div>
        Aún no tiene amigos...
    </asp:Panel>
</div>