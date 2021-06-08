<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="DirectorDetail.aspx.cs" Inherits="Web.Admin.DirectorManagement.DirectorDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% if (enableShowDetail)
        { %>
    <div class="row grid-responsive">
        <div class="column page-heading">
            <div class="large-card">
                <h1>Chi tiết đạo diễn: <% = directorInfo.name %></h1>
                <p>ID của đạo diễn: <% = directorInfo.ID %></p>
                <p>Tên của đạo diễn: <% = directorInfo.name %></p>
                <p>Mô tả của đạo diễn: <% = directorInfo.description %></p>
                <p>Ngày tạo của đạo diễn: <% = directorInfo.createAt %></p>
                <p>Ngày cập nhật của đạo diễn: <% = directorInfo.updateAt %></p>
                <asp:HyperLink ID="hyplnkList" CssClass="button button-blue" runat="server">Quay về trang danh sách</asp:HyperLink>
                <asp:HyperLink ID="hyplnkEdit" CssClass="button button-green" runat="server">Chỉnh sửa</asp:HyperLink>
                <asp:HyperLink ID="hyplnkDelete" CssClass="button button-red" runat="server">Xóa</asp:HyperLink>
            </div>
        </div>
    </div>
    <%} %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
