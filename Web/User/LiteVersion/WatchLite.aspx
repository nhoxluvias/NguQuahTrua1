<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/User/Layout/UserLayoutLite.Master" AutoEventWireup="true" CodeBehind="WatchLite.aspx.cs" Inherits="Web.User.LiteVersion.WatchLite" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <% if (filmInfo != null)
        {%>
    <title><% = filmInfo.name %> - Trang xem phim - Phiên bản rút gọn</title>
    <%} %>
    <link href="<% = ResolveUrl("~/common_assets/video-js/video-js.min.css") %>" rel="stylesheet">
    <script src="<% = ResolveUrl("~/common_assets/video-js/video.js") %>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% if (filmInfo != null)
        { %>
    <div class="main-content-title">
        <h3>Xem phim: <% = filmInfo.name %></h3>
    </div>
    <div class="watch">
        <video id="vid" class="video-js vjs-default-skin" controls preload="auto" data-setup="{}">
            <source src="<% = filmInfo.source %>" type="video/mp4">
            Your browser does not support the video tag.
        </video>
    </div>
    <%} %>
</asp:Content>
