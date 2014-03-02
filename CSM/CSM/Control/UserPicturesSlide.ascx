<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserPicturesSlide.ascx.cs" Inherits="CSM.Control.UserPicturesSlide" %>
<div class="module pictures">
                <div class="imagelist">
                    <!-- "previous page" action -->
                    <a class="prev browse left"></a>
                    <!-- "next page" action -->
                    <div class="latestscrollable" id="latestcroll">
                        <div class="items"> 
							<asp:Repeater runat="server" ID="rptLatest">
						        <HeaderTemplate>
						            
						        </HeaderTemplate>
						        <ItemTemplate>
						                <div>
						                    <img alt="<%# DataBinder.Eval(Container.DataItem, "AlbumName")%>" onclick="document.location.href='Album.aspx?userid=<%# DataBinder.Eval(Container.DataItem, "UserID")%>';" desc="<%# DataBinder.Eval(Container.DataItem, "AlbumDesc")%>" title="<%# DataBinder.Eval(Container.DataItem, "AlbumName")%>" class="pic tooltipimg" src="AjaxHandler.ashx?fn=ResizeRatio&w=130&h=100&src=/userData/<%# DataBinder.Eval(Container.DataItem, "AlbumKeyPic")%>" />
						                </div>
						        </ItemTemplate>
						        <FooterTemplate>
						                        
						        </FooterTemplate>
						    </asp:Repeater>
						</div>
                    </div>
                    <!-- "next page" action -->
                    <a class="next browse right"></a>
                </div>
            </div>
    <div id="LatestPicturesTooltip" class="tooltip"></div>

