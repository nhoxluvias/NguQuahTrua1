<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/User/Layout/UserLayout.Master" AutoEventWireup="true" CodeBehind="Watch.aspx.cs" Inherits="Web.User.Watch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title><% = title_HeadTag %> - Trang xem phim</title>
    <meta charset="UTF-8">
    <meta name="description" content="<% = description_MetaTag %>">
    <meta name="keywords" content="<% = keywords_MetaTag %>">
    <meta name="author" content="">
    <link rel="profile" href="#">

    <link href="<% = ResolveUrl("~/common_assets/video-js/video-js.min.css") %>" rel="stylesheet">
    <script src="<% = ResolveUrl("~/common_assets/video-js/video.js") %>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-single">
        <div class="container">
            <div class="col-lg-12 col-md-12 col-sm-12" style="height: 150px;"></div>
            <div class="col-lg-12 col-md-12 col-sm-12" style="height: 600px;">
                <video id="vid" class="video-js vjs-default-skin" controls preload="auto" data-setup="{}">
                    <source src="<% = filmInfo.source %>" type="video/mp4">
                    Your browser does not support the video tag.
                </video>
            </div>
        </div>
    </div>
    <style type="text/css">
        #vid {
            width: 100% !important;
            height: 600px !important;
        }
    </style>
</asp:Content>
