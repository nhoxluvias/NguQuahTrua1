<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="FilmDetail.aspx.cs" Inherits="Web.Admin.FilmManagement.FilmDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% if (enableShowDetail)
        { %>
    <!--Tables-->
    <h5 class="mt-2">Chi tiết phim</h5>
    <a class="anchor" name="tables"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Chi tiết phim: <% = filmInfo.name %></h3>
                </div>
                <div class="card-block">
                    <table>
                        <tr>
                            <th>ID của phim</th>
                            <td><% = filmInfo.ID %></td>
                        </tr>
                        <tr>
                            <th>Tên của phim</th>
                            <td><% = filmInfo.name %></td>
                        </tr>
                        <tr>
                            <th>Quốc gia</th>
                            <td><% = filmInfo.Country.name %></td>
                        </tr>
                        <tr>
                            <th>Công ty sản xuất</th>
                            <td><% = filmInfo.productionCompany %></td>
                        </tr>
                        <tr>
                            <th>Mô tả của phim</th>
                            <td><% = filmInfo.description %></td>
                        </tr>
                        <tr>
                            <th>Ngày tạo của phim</th>
                            <td><% = filmInfo.createAt %></td>
                        </tr>
                        <tr>
                            <th>Ngày cập nhật của phim</th>
                            <td><% = filmInfo.updateAt %></td>
                        </tr>
                        <tr>
                            <th>Công cụ</th>
                            <td>
                                <asp:HyperLink ID="hyplnkList" CssClass="button button-blue" runat="server">Quay về trang danh sách</asp:HyperLink>
                                <asp:HyperLink ID="hyplnkEdit_Category" CssClass="button button-green" runat="server">Thêm/xóa thể loại</asp:HyperLink>
                                <asp:HyperLink ID="hyplnkEdit_Image" CssClass="button button-green" runat="server">Thêm/xóa hình ảnh</asp:HyperLink>
                                <asp:HyperLink ID="hyplnkEdit" CssClass="button button-green" runat="server">Chỉnh sửa</asp:HyperLink>
                                <asp:HyperLink ID="hyplnkDelete" CssClass="button button-red" runat="server">Xóa</asp:HyperLink>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <%} %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
