<%@ Page Language="C#" Inherits="CSM.ProgramGenerator" MasterPageFile="~/Master/Private.Master" %>
<%@ MasterType VirtualPath="~/Master/Private.Master" %>
<asp:Content ContentPlaceHolderID="bodyContent" ID="bodyContentContent" runat="server">

<div class="sixteen columns">
	<h1>Gestión de programas</h1>
	<div class="clear"></div>
    <div class="composition">
        <div class="eight columns">
	        <h4>Programas</h4>
	        <ul class="eight columns table-layout header">
	        	<li class="spacer">
        			&nbsp;
    			</li>
        		<li class="name">Nombre</li>
        		<li class="name">Descripción</li>
        		<li class="date">Fecha</li>
        		<li class="level">Nivel</li>
	        </ul>
	        <ul class="eight columns table-layout">
	        <asp:Repeater runat="server" id="rptProgram">
	        	<HeaderTemplate>
	        		
	        	</HeaderTemplate>
	        	<ItemTemplate>
	        		<li class="spacer">
	        			<asp:RadioButton runat="server" GroupName="programID" id="rdbProgramID" AutoPostBack="True" OnCheckedChanged="rdbProgramID_CheckedChange" data-val='<%#Eval("ID")%>'></asp:RadioButton>
        			</li>
	        		<li class="name"><%#Eval("Name")%></li>
	        		<li class="name"><%#Eval("DescAbrev").ToString()%></li>
	        		<li class="date"><%#((DateTime)Eval("Date")).ToShortDateString()%></li>
	        		<li class="level"><%#Eval("Level").ToString()%></li>
	        	</ItemTemplate>
	        	<FooterTemplate>
	        		
	        	</FooterTemplate>
	        </asp:Repeater>
			</ul>
	    </div>
	    <div class="five columns">
	        <h4>Clases del programa</h4>
			<ul class="eight columns table-layout header">
	        	<li class="spacer">
        			&nbsp;
    			</li>
        		<li class="name">Asignatura</li>
        		<li class="name">Descripción</li>
	        </ul>
	        <ul class="eight columns table-layout">
	        <asp:Repeater runat="server" id="rptSubject">
	        	<HeaderTemplate>
	        		
	        	</HeaderTemplate>
	        	<ItemTemplate>
	        		<li class="spacer">
	        			<asp:RadioButton runat="server" GroupName="subjectID" id="rdbSubject" data-val='<%#Eval("ID")%>'></asp:RadioButton>
        			</li>
	        		<li class="name"><%#Eval("Name")%></li>
	        		<li class="name"><%#Eval("DescAbrev").ToString()%></li>
	        	</ItemTemplate>
	        	<FooterTemplate>
	        		
	        	</FooterTemplate>
	        </asp:Repeater>
			</ul>
	    </div>
	    <div class="clear"></div>
	</div>

</div>


</asp:Content>
<asp:Content ContentPlaceHolderID="scripts" ID="scriptsContent" runat="server">
</asp:Content>


