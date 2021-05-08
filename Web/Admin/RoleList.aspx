<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="RoleList.aspx.cs" Inherits="Web.Admin.RoleList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--Tables-->
    <h5 class="mt-2"></h5>
    <a class="anchor" name="tables"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Current Members</h3>
                </div>
                <div class="card-block">
                    <asp:GridView ID="GridView1" runat="server">
                        
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
