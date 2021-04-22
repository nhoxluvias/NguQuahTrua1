<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Form.aspx.cs" Inherits="Web.Test.Form" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="CustomValidator" OnServerValidate="TxtInputValidator"></asp:CustomValidator>
        </div>
    </form>
</body>
</html>
