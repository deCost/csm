<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Private.Master" AutoEventWireup="true" CodeBehind="CreateSchedule.aspx.cs" Inherits="CSM.CreateSchedule" %>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" runat="server">

<div class="sixteen columns">
	<h1>Nueva programaci&oacute;n</h1>
	<div class="clear"></div>
	<div class="composition">
        <div class="five columns">
	        <p>Selecciona tipo *:</p> 
			<p>
	            <asp:DropDownList runat="server" ID="drpType"></asp:DropDownList>
	        </p>
	       	<p>Evento asociado:</p>
	        <p><asp:TextBox runat="server" id="bookinginput" /></p>
			<p>Fecha:</p>
	        <p><asp:TextBox runat="server" ID="dateinput" CssClass="birthdateinput datepicker" /> <asp:DropDownList runat="server" ID="drpHours"></asp:DropDownList>&nbsp;<asp:DropDownList runat="server" ID="drpMinutes"></asp:DropDownList></p>
        </div>
        <div class="five columns">
			<p>T&iacute;tulo: </p>
			<p><asp:TextBox runat="server" ID="txtTitle" MaxLength="25" CssClass="biginput txttitle" /></p>
			<p>Descripci&oacute;n: </p>
			<p><asp:TextBox runat="server" ID="txtDesc" MaxLength="255" TextMode="MultiLine" CssClass="biginput txtdesc" /></p>
		</div>
		<div class="five columns">
			<p>Amigos invitados: </p>
	        <p><asp:TextBox runat="server" ID="txtFriend" ReadOnly="true" CssClass="biginput txtfriends" /></p>
	        <h6>Selecciona los amigos invitados</h6>
	        <asp:CheckBoxList runat="server" id="chkUserLinked" />
		                
	        <p><asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Almacenar" /></p>
			<asp:Label runat="server" ID="lblresponse"></asp:Label>
        </div>
        <div class="clear"></div>
	</div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="server">
<script type="text/javascript" src="js/lib/jquery-ui-1.8.18.custom.min.js"></script>
<script type="text/javascript" src="js/lib/jquery-ui-localize.js"></script>
<script type="text/javascript">

    $(document).ready(function () {
        $(".txttitle").labeledInput({ label: "Título de la programación" });
        $(".txtdesc").labeledInput({ label: "Descripción de la programación" });

        $(".datepicker").datepicker({
            maxDate: '+1Y',
            minDate: '0',
            buttonImageOnly: false,
            changeMonth: true,
            changeYear: true,
            yearRange: "-100:+0"
        });

        $.datepicker.setDefaults($.datepicker.regional["es"]);

    });
        
</script>
</asp:Content>
