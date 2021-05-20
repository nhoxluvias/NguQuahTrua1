<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="CreateCategory.aspx.cs" Inherits="Web.Admin.CreateCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% if (enableShowResult)
        { %>
    <h5 class="mt-2">Trạng thái thêm thể loại</h5>
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
    <h5 class="mt-2">Tạo mới thể loại</h5>
    <a class="anchor" name="forms"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Nhập dữ liệu vào các trường bên dưới để tạo mới 1 thể loại</h3>
                </div>
                <div class="card-block">
                    <div>
                        <fieldset>
                            <label for="txtCategoryName">Tên thể loại</label>
                            <asp:TextBox ID="txtCategoryName" placeholder="Nhập vào tên thể loại" runat="server"></asp:TextBox>
                            <asp:CustomValidator ID="cvCategoryName" CssClass="text-red" runat="server"></asp:CustomValidator>
                            <label for="txtCategoryDescription">Mô tả thể loại</label>
                            <asp:TextBox ID="txtCategoryDescription" placeholder="Nhập vào mô tả thể loại" CssClass="text-area" TextMode="MultiLine" runat="server"></asp:TextBox>
                        </fieldset>
                    </div>
                </div>
                <div class="card-block mt-0">
                    <asp:Button ID="btnSubmit" CssClass="button-primary" runat="server" Text="Tạo mới" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
