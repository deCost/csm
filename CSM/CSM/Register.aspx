<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Public.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="CSM.Register" %>
<%@ MasterType VirtualPath="~/Master/Public.Master" %>

<asp:Content ID="Content" ContentPlaceHolderID="bodyContent" runat="server">

<div class="sprite cajalarga">
    <div class="registerform">
		<asp:TextBox runat="server"  MaxLength="25" ID="nameinput" CssClass="nameinput" TabIndex="1" />
		<asp:TextBox runat="server"  MaxLength="25" ID="surnameinput" CssClass="surnameinput" TabIndex="2" />
		<asp:TextBox runat="server"  MaxLength="25" ID="birthdateinput" CssClass="birthdateinput datepicker" TabIndex="3" />
		<div class="clear"></div>
		<asp:TextBox runat="server"  MaxLength="25" ID="nickinput" CssClass="nickinput" TabIndex="4" />
		<div class="clear"></div>
		<asp:TextBox runat="server"  MaxLength="100" ID="emailinput" CssClass="emailinput" TabIndex="5" />
		<asp:TextBox runat="server"  MaxLength="100" ID="repemailinput" CssClass="repemailinput" TabIndex="6" />
		<div class="clear"></div>
		<asp:TextBox TextMode="Password" runat="server"  MaxLength="25" ID="passinput" CssClass="passinput" TabIndex="7" />
		<asp:TextBox TextMode="Password" runat="server"  MaxLength="25" ID="reppassinput" CssClass="reppassinput" TabIndex="8" />
		<div class="clear"></div>
	    <asp:CheckBox runat="server" ID="chkPrivacy" TabIndex="9" /> 
	    <span class="conditions">		        
	        Acepto la <a href="Privacy.aspx" class="iframe fancybox.iframe">Pol&iacute;tica de privacidad</a>
		</span>
	    <div class="clear"></div>
	    <div style="position:relative">
	        <div class="shadow box hidden msg <%=registerStatus %>">
	            <div class="rightarrow"></div>
	            <asp:Label ID="lblMsg" runat="server" />
	        </div>
	    </div>
	    <asp:Button runat="server" ID="btnRegister" TabIndex="10" CssClass="preventDouble" Text="Registrarme" OnClick="btnRegister_Click" />
	</div>
</div>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="scripts" runat="server">
    <script type="text/javascript" src="js/lib/jquery-ui-1.8.18.custom.min.js"></script>
<script type="text/javascript" src="js/lib/jquery-ui-localize.js"></script>
<script type="text/javascript">

    $(document).ready(function () {
        $(".nameinput").labeledInput({ label: "Nombre" });
        $(".surnameinput").labeledInput({ label: "Apellidos" });
        $(".emailinput").labeledInput({ label: "correo@social" });
        $(".repemailinput").labeledInput({ label: "Repetir email" });
        $(".nickinput").labeledInput({ label: "Usuario" });
        $(".passinput").labeledInput({ label: "Contraseña" });
        $(".reppassinput").labeledInput({ label: "Repetir contraseña" });

        $(".datepicker").datepicker({
            buttonImage: 'images/calendar.png',
            maxDate: '-18Y',
            minDate: '-99Y',
            buttonImageOnly: true,
            changeMonth: true,
            changeYear: true,
            yearRange: "-100:+0"
        });

        $.datepicker.setDefaults($.datepicker.regional["es"]);

    });

		  
</script>
</asp:Content>
