<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserScheduleList.ascx.cs" Inherits="CSM.Control.UserScheduleList" %>
<asp:Panel runat="server" ID="pnlSchedule" CssClass="events">

    <asp:Repeater runat="server" ID="rptSchedule">
        <HeaderTemplate>
            
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

                		<asp:Button runat="server" id="btnStudent" Text="Apuntarse como alumno" Visible="<%# ((CSM.Classes.Schedule)Container.DataItem).SchedTypeID == CSM.Utilities.GetScheduleType(2) %>" />
                	</div>
                	<div class="clear"></div>
        </ItemTemplate>
        <FooterTemplate>
            
        </FooterTemplate>
    </asp:Repeater>

</asp:Panel>
