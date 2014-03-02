<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Private.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="CSM.Settings" %>


<asp:Content ID="Content2" ContentPlaceHolderID="bodyContent" runat="server">

<div class="sixteen columns">
	<h1>Configuraci&oacute;n</h1>
	<div class="clear"></div>
    <div class="composition">
        <div class="five columns">
	        <h4>Datos personales</h4>
 	        <div class="form-personaldata">
	            <asp:Image runat="server" id="imgProfile" CssClass="pic" Width="150" Height="150" />
	            <asp:FileUpload runat="server" id="uplProfile" />
              
		        <p>Nombre completo *:</p> 
		        <p>
                <asp:TextBox runat="server"  MaxLength="25" ID="nameinput" CssClass="nameinput" TabIndex="1" />
	            <asp:TextBox runat="server"  MaxLength="25" ID="surnameinput" CssClass="surnameinput" tabindex="2" />
                </p>
		        <p>Fecha Nacimiento:</p>
		        <p><asp:TextBox runat="server"  MaxLength="25" ID="birthdateinput" CssClass="birthdateinput datepicker" TabIndex="3" /></p>
		        <p>Email *:</p> 
		        <p>
                <asp:TextBox runat="server"  MaxLength="100" ID="emailinput" CssClass="emailinput" tabindex="4" />
	            <asp:TextBox runat="server"  MaxLength="100" ID="repemailinput" CssClass="repemailinput" tabindex="5" />
                </p>
		        <p>Contrase&ntilde;a *:</p> 
		        <p><asp:TextBox runat="server"  MaxLength="25" ID="passinput" CssClass="passinput" tabindex="7" />
	            <asp:TextBox runat="server"  MaxLength="25" ID="reppassinput" CssClass="reppassinput" tabindex="8" /></p>
                <asp:Label runat="server" ID="lblprofilemsg" ></asp:Label>
                <asp:Button runat="server" ID="btnProfile" Text="Almacenar cambios" OnClick="btnProfile_Click" />
	        </div>
        </div>
        <div class="five columns">
		    <h4>Privacidad</h4>
		    <div class="form-privacy">
			    <p>Selecciona tipo *:<p> 
			    <asp:RadioButtonList runat="server" ID="rdbRuleType" RepeatDirection="Vertical" AutoPostBack="false">
	                <asp:ListItem Text="Todas" Value="0"></asp:ListItem>
	                <asp:ListItem Text="Imágenes" Value="1"></asp:ListItem>
	                <asp:ListItem Text="Publicaciones" Value="2"></asp:ListItem>
	            </asp:RadioButtonList>
			    <p>Regla privacidad *:</p>
			    <p><asp:DropDownList runat="server" ID="drpRule" AutoPostBack="true" OnSelectedIndexChanged="drpRule_SelectedIndexChanged">
	                <asp:ListItem Text="Nueva regla" Value="-1" />
	            </asp:DropDownList>
	            </p>
	            <asp:Panel ID="pnlRule" runat="server">
	                <p>Nombre de la regla: <asp:TextBox runat="server" ID="txtRuleName" MaxLength="25" /></p>
	                <p>Descripción de la regla:</p>
	                <p><asp:TextBox runat="server" ID="txtRuleDesc" MaxLength="255" CssClass="biginput" TextMode="MultiLine" /></p>
	            </asp:Panel>            
			    <p>Visibilidad *:</p>
			    <p><asp:DropDownList runat="server" ID="drpVisibility" AutoPostBack="false" Enabled="false">
	                <asp:ListItem Text="Únicamente los usuarios seleccionados" Value="1" />
	            </asp:DropDownList></p>
			    <p>Selecciona los usuarios sobre los que aplica: </p>
			    <p><asp:TextBox runat="server" ID="txtFriend" ReadOnly="true" CssClass="biginput txtfriends" /></p>
	            <p><asp:Button ID="btnRule" runat="server" OnClick="btnRule_Click" Text="Guardar regla" /></p>
	      
	            <p><asp:Label runat="server" ID="lblResponseRule"></asp:Label></p>

            </div>
        </div>
		<div class="five columns">
			<h4>Selecciona los usuarios</h4>
		    <div>
		        <a href="javascript:void(0);" onclick="CheckAll(true)">Marcar todos</a> | <a href="javascript:void(0);" onclick="CheckAll(false)">Desmarcar todos</a>
		    </div>
		    <asp:CheckBoxList runat="server" ID="chkLinked" AutoPostBack="true" OnSelectedIndexChanged="chkLinked_SelectedIndexChanged"></asp:CheckBoxList>
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
        
        $(".datepicker").datepicker({
            
            maxDate: '-18Y',
            minDate: '-99Y',
            buttonImageOnly: false,
            changeMonth: true,
            changeYear: true,
            yearRange: "-100:+0"
        });

        $.datepicker.setDefaults($.datepicker.regional["es"]);
        
        $("ul.tabs").tabs("div.panes > div.contenttab");
    });

    function OnClientAsyncFileUploadComplete(sender, args) {
        var imageUrl = AjaxHandler;
        imageUrl += "?fn=ImagePreview&size=settings&sessiontype=profile&sessionid=<%=UserSessionID %>"

        $('.profile img').attr("src", imageUrl);
    }

    function OpenModalUsers() {
        $.fancybox({
            width: '480px',
            height: '320px',
            autoScale: true,
            transitionIn: 'fade',
            transitionOut: 'fade',
            type: 'inline',
            closeBtn: true,
            href: '#userlist'
        });
    }

    function CheckAll(bool) {
        $("#userlist input").each(function () {
            if (bool) {
                $(this).attr("checked", true);
                $(".txtfriends").val("Todos tus amigos");
            } else {
                $(this).removeAttr("checked");
                $(".txtfriends").val("");
            }
        });
    }	  
		  
</script>

</asp:Content>
