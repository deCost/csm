<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Footer.ascx.cs" Inherits="CSM.Control.Footer" %>
<div class="footer sixteen columns">
	&copy; Copyright <span style="text-decoration: underline;">deCost</span> 2012 | <a href="Privacy.aspx" class="iframe fancybox.iframe">Pol&iacute;tica de privacidad</a> | <a href="#contactform" class="modal">Contacto</a>
	<div id="contactform" class="hidden">
        <asp:TextBox CssClass="ctcnameinput ctcfield" ID="ctcnameinput" runat="server" />
        <asp:TextBox CssClass="ctcemailinput ctcfield" ID="ctcemailinput" runat="server" />
        <asp:TextBox CssClass="ctcsubjinput ctcfield" ID="ctcsubjinput" runat="server" />
        <asp:TextBox CssClass="ctcmessageinput ctcfield" ID="ctcmessageinput" TextMode="MultiLine" runat="server" Height="100px" />
        <input type="button" value="Enviar" onclick="SendContactForm();" id="btnContact" />
        <div class="clear"></div>
        <div id="ctcResponse"></div>
	</div>
</div>
