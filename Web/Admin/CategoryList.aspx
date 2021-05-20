<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Admin/Layout/AdminLayout.Master" AutoEventWireup="true" CodeBehind="CategoryList.aspx.cs" Inherits="Web.Admin.CategoryManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <!--Tables-->
    <h5 class="mt-2"></h5>
    <a class="anchor" name="tables"></a>
    <div class="row grid-responsive">
        <div class="column ">
            <div class="card">
                <div class="card-title">
                    <h3>Danh sách vai trò</h3>
                </div>
                <div class="card-block">
                    <asp:GridView ID="grvCategory" runat="server" BorderColor="Silver" BorderStyle="Solid">
                        <AlternatingRowStyle BackColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

    <h5 class="mt-2">Buttons</h5>
    <a class="anchor" name="buttons"></a>
    <div class="row grid-responsive">
				<div class="column">
                    <asp:HyperLink ID="hyplnkCreateCategory" CssClass="button" runat="server">Thêm mới</asp:HyperLink>
				</div>
			</div>
</asp:Content>
