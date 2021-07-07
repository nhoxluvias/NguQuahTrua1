<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="EditCategory.aspx.cs" Inherits="Web.Admin.FilmManagement.EditCategory" %>

<%@ Import Namespace="Data.DTO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Chỉnh sửa thể loại cho phim - Trang quản trị</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% if (enableShowResult)
        { %>
    <h5 class="mt-2">Trạng thái thêm phim</h5>
    <a class="anchor" name="alerts"></a>
    <div class="row grid-responsive">
        <div class="column">
            <%if (stateString == "Success")
                { %>
            <div class="alert background-success"><em class="fa fa-thumbs-up"></em><% = stateDetail %></div>
            <%}
                else if (stateString == "AlreadyExists")
                { %>
            <div class="alert background-warning"><em class="fa fa-warning"></em><% = stateDetail %></div>
            <%}
                else
                { %>
            <div class="alert background-danger"><em class="fa fa-times-circle"></em><% = stateDetail %></div>
            <%} %>
        </div>
    </div>
    <%} %>
    <h5 class="mt-2">Thêm thể loại cho phim</h5>
    <a class="anchor" name="forms"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Thêm thể loại vào phim: <% = filmName %></h3>
                </div>
                <div class="card-block">
                    <div>
                        <fieldset>
                            <asp:Label ID="lbFilmCategory" runat="server" Text="Thể loại" AssociatedControlID="drdlFilmCategory"></asp:Label>
                            <asp:DropDownList ID="drdlFilmCategory" runat="server"></asp:DropDownList>
                        </fieldset>
                    </div>
                </div>
                <div class="card-block mt-0">
                    <asp:Button ID="btnAddCategory" CssClass="button-primary" runat="server" Text="Thêm" OnClick="btnAddCategory_Click" />
                </div>
            </div>
        </div>
    </div>

    <% if (enableShowDetail)
        {%>
    <!--Tables-->
    <h5 class="mt-2">Thể loại của phim</h5>
    <a class="anchor" name="tables"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Danh sách thể loại của phim: <% = filmName %></h3>
                </div>
                <div class="card-block">
                    <table>
                        <tbody>
                            <% int count = 1; %>
                            <% foreach (CategoryInfo categoryInfo in categoriesByFilmId)
                                {%>
                            <tr>
                                <th>Thể loại <% = count++ %></th>
                                <td><% = categoryInfo.name %></td>
                                <td></td>
                            </tr>
                            <% } %>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <% } %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
