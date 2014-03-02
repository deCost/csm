<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PublicationTools.ascx.cs" Inherits="CSM.Control.PublicationTools" %>


<asp:Panel ID="toolbox" ClientIDMode="Static" runat="server" CssClass="seven columns tools">
    <div class="publication">
        <div class="writte">
		    <asp:TextBox TextMode="MultiLine" ID="txtpublication" MaxLength="255" runat="server" ClientIDMode="Static" />
	    </div>						
		<asp:DropDownList ID="drpRule" runat="server" Width="100px"></asp:DropDownList>
		<asp:Button ID="btnPublication" runat="server" CssClass="preventDouble" OnClick="btnPublication_Click" Text="Enviar" OnClientClick="setImageLoad('responseTxt')" />
    </div>
    <div class="publicationresponse">
        <asp:Image ID="imgUploaded" runat="server" Visible="false" />
        <asp:Label ID="imgjaxload" runat="server" Style="display:none">
            <img src="images/ajax-loader.gif" alt="loading" />
        </asp:Label>
        <asp:Label runat="server" ID="responseTxt" ClientIDMode="Static" />
    </div>
    <asp:HiddenField ID="hdnusertoid" runat="server" />   
</asp:Panel>
    
    
