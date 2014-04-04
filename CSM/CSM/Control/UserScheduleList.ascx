<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserScheduleList.ascx.cs" Inherits="CSM.Control.UserScheduleList" %>
<asp:Panel runat="server" ID="pnlSchedule" CssClass="events">

	<asp:Repeater runat="server" ID="rptBooked" OnItemDataBound="rptBooked_ItemDataBound">
        <HeaderTemplate>
            <h3>Tus eventos</h3>
        </HeaderTemplate>
        <ItemTemplate>        	
                	<div class="sprite tecla left">
                		<span class="date"><%# ((CSM.Classes.Schedule)Container.DataItem).SchedDate.ToString("dd/MM") %></span>
                		<span class="time"><%# ((CSM.Classes.Schedule)Container.DataItem).SchedDate.ToString("HH:mm") %></span>
                	</div>
					<div class="seven columns listcontent left">
                		<h2><%# ((CSM.Classes.Schedule)Container.DataItem).SchedTitle %>,</h2> 
                		<p>Junto a tí, los siguientes compañeros están esperando ansiosos salir a escena. Conócelos un poco más:</p>
                		<ol class="studentsList">
                		<asp:Repeater runat="server" id="rptStudents">
                			<ItemTemplate>
                				<li><a href="Profile.aspx?user=<%# ((CSM.Classes.StudentSchedule)Container.DataItem).UserID %>"><%# ((CSM.Classes.StudentSchedule)Container.DataItem).CompleteName %></a></li>
                			</ItemTemplate>
                		</asp:Repeater>
						</ol>
                	</div>
                	<div class="seven columns listcontent left">
                		<asp:Button runat="server" id="btnBookingNo" Text="Reservar" PostBackUrl="<%# ((CSM.Classes.Schedule)Container.DataItem).SchedBookingUrl %>" Visible="<%# ((CSM.Classes.Schedule)Container.DataItem).SchedBooking > 0 %>" />
                
                	</div>
                	<div class="clear"></div>
        </ItemTemplate>
        <FooterTemplate>
            
        </FooterTemplate>
    </asp:Repeater>

    <asp:Repeater runat="server" ID="rptSchedule">
        <HeaderTemplate>
            <h3>Próximos eventos</h3>
        </HeaderTemplate>
        <ItemTemplate>
                
                	          	
                	<div class="sprite tecla left">
                		<span class="date"><%# ((CSM.Classes.Schedule)Container.DataItem).SchedDate.ToString("dd/MM") %></span>
                		<span class="time"><%# ((CSM.Classes.Schedule)Container.DataItem).SchedDate.ToString("HH:mm") %></span>
                	</div>
					<div class="seven columns listcontent left">
                		<h2><%# ((CSM.Classes.Schedule)Container.DataItem).SchedTitle %>,</h2> 
                		<p><%# ((CSM.Classes.Schedule)Container.DataItem).SchedDesc %></p>
                	</div>
                	<div class="seven columns listcontent left">
                		<asp:Button runat="server" id="btnBooking" Text="Reservar" PostBackUrl="<%# ((CSM.Classes.Schedule)Container.DataItem).SchedBookingUrl %>" Visible="<%# ((CSM.Classes.Schedule)Container.DataItem).SchedBooking > 0 %>" />

                		<asp:Button runat="server" id="btnStudent" OnClick="btnStuden_Click" data-value="<%# ((CSM.Classes.Schedule)Container.DataItem).SchedID %>" Text="Apuntarse como alumno" Visible="<%# ((CSM.Classes.Schedule)Container.DataItem).EventType == CSM.Classes.EventType.MartesAlternos %>" />
                	</div>
                	<div class="clear"></div>
        </ItemTemplate>
        <FooterTemplate>
            
        </FooterTemplate>
    </asp:Repeater>

</asp:Panel>
