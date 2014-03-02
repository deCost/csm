<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserBubblesList.ascx.cs" Inherits="CSM.Control.UserBubblesList" %>

 <div class="seven columns publications">
	<asp:Repeater ID="rptBubble" runat="server">
	    <HeaderTemplate>
	    </HeaderTemplate>
	    <ItemTemplate>
	        <div class="<%# ((CSM.Classes.Publication)Container.DataItem).GetsCssStyle(_user.UserID)%>">
	            <div class="image">
		            <img alt="" width="50" height="50" src="<%#((CSM.Classes.Publication)Container.DataItem).User.ProfileImage%>" class="pic" />
	            </div>
	            <div class="content">
		            <div class="leftarrow"></div>
		            <a class="userlink" href="Profile.aspx?user=<%#((CSM.Classes.Publication)Container.DataItem).User.UserID%>"><%#((CSM.Classes.Publication)Container.DataItem).User.Name%></a>
	                <span class="text">
	                    <%# DataBinder.Eval(Container.DataItem, "PublDesc")%>
		            </span>
	                <%# ShowImageInPublication(((CSM.Classes.Publication)Container.DataItem).Image)%>
	                
	                <%if (!isMessages)
	                  { %>
		            <div class="summary">
			            <div class="date"><%# CSM.Utilities.TimeSpanToString(((CSM.Classes.Publication)Container.DataItem).datediff())%></div>
			            <div class="comments"><a href="#"><%# ((CSM.Classes.Publication)Container.DataItem).Comments.Count %> comentarios</a><div class="icon"></div></div>
		            </div>
	                
		            <div class="pub-comments">
	                    <asp:Repeater runat="server" ID="repComm" DataSource="<%# ((CSM.Classes.Publication)Container.DataItem).Comments %>">
	                        <HeaderTemplate></HeaderTemplate>
	                        <ItemTemplate>
	                            <div class="comm">
				                    <div class="commimg">
					                    <img alt="" width="25" height="25" src="<%#((CSM.Classes.Publication)Container.DataItem).User.ProfileImage%>" class="pic" />
				                    </div>
				                    <div class="commtxt">
				                    	<a class="userlink" href="Profile.aspx?user=<%#((CSM.Classes.Publication)Container.DataItem).User.UserID%>"><%#((CSM.Classes.Publication)Container.DataItem).User.Name%></a>
					                    <span class="text">
						                    <%# DataBinder.Eval(Container.DataItem, "PublDesc")%>
					                    </span>
					                    <%# ShowImageInPublication(((CSM.Classes.Publication)Container.DataItem).Image)%>
				                    </div>
				                    <div class="commsummary">
					                    <div class="date"><%# CSM.Utilities.TimeSpanToString(((CSM.Classes.Publication)Container.DataItem).datediff())%></div>
				                    </div>
			                    </div>
	                        </ItemTemplate>
	                        <SeparatorTemplate><div class="clear"></div></SeparatorTemplate>
	                        <FooterTemplate></FooterTemplate>
	                    </asp:Repeater>
	                    <div class="clear"></div>
					<%if(canComment){%>
	                    <asp:Panel runat="server" id="pnlCommentWritter" CssClass="comm">
				            <div class="commimg">
	                            <img alt="" width="25" height="25"  src="<%=CurrentUser.ProfileImage%>" class="pic" />
				            </div>
				            <div class="commtxt">
					            <span class="text">
						            <asp:TextBox TextMode="SingleLine" ID="txtcomment" onkeyup="clearHTMLStrips(this)" MaxLength="255" runat="server" />
	                                <asp:Button runat="server" ID="btnComment" CssClass="preventDouble" Text="Enviar" OnClientClick="SendNewComment(this);" />
	                                <asp:HiddenField ID="hdnruleid" runat="server" Value="<%# ((CSM.Classes.Publication)Container.DataItem).RuleID%>" />
	                                <asp:HiddenField ID="hdnpublid" runat="server" Value="<%# ((CSM.Classes.Publication)Container.DataItem).PublID%>" />
					            </span>
					            
				            </div>
			            </asp:Panel>		
			           <%}%>				
		            </div>
	                <%} %>
	            </div>
	            </div>
	    </ItemTemplate>
	    <FooterTemplate>
	    </FooterTemplate>
	</asp:Repeater>
	<div class="clear"></div>
	<asp:Panel ID="pnlViewMore" runat="server" CssClass="viewmorebox">
	    <asp:LinkButton runat="server" ID="btnViewMore" OnClick="btnViewMore_Click" Text="Ver más..." />
	</asp:Panel>
</div>