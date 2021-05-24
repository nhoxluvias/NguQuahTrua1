<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="CreateDirector.aspx.cs" Inherits="Web.Admin.DirectorManagement.CreateDirector" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <% if (enableShowResult)
        { %>
    <h5 class="mt-2">Trạng thái thêm đạo diễn</h5>
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
    <h5 class="mt-2">Tạo mới đạo diễn</h5>
    <a class="anchor" name="forms"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Nhập dữ liệu vào các trường bên dưới để tạo mới 1 đạo diễn</h3>
                </div>
                <div class="card-block">
                    <div>
                        <fieldset>
                            <label for="txtDirectorName">Tên đạo diễn</label>
                            <asp:TextBox ID="txtDirectorName" placeholder="Nhập vào tên đạo diễn" runat="server"></asp:TextBox>
                            <asp:CustomValidator ID="cvDirectorName" CssClass="text-red" runat="server"></asp:CustomValidator>
                            <label for="txtDirectorDescription">Mô tả đạo diễn</label>
                            <asp:TextBox ID="txtDirectorDescription" placeholder="Nhập vào mô tả đạo diễn" CssClass="text-area" TextMode="MultiLine" runat="server"></asp:TextBox>
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
