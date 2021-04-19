<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="Web.Account.ResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lấy lại mật khẩu</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/account/reset-password.css") %>">
</head>
<body>
    <form id="form1" runat="server">
        <div class="reset-password-form">
            <div class="reset-password-form-title">
                <h3>Đặt lại mật khẩu</h3>
                <p>Lấy lại mật khẩu để truy cập lại tài khoản</p>
                <hr/>
            </div>
            <div class="reset-password-form-data">
                <div class="reset-password-form-group">
                    <p>Địa chỉ Email</p>
                    <asp:TextBox ID="txtEmail" TextMode="Email" Text="" placeholder="Nhập vào địa chỉ Email" runat="server"></asp:TextBox>
                    <div class="show-error">
                        <span>
                            <asp:CustomValidator ID="cvEmail" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                        </span>
                    </div>
                </div>
                <div class="reset-password-form-group">
                    <p>Mã xác thực</p>
                    <asp:TextBox ID="txtConfirmCode"  TextMode="SingleLine" Text="" Placeholder="Nhập vào mã xác nhận" runat="server"></asp:TextBox>
                    <div class="show-error">
                        <span>
                            <asp:CustomValidator ID="cvConfirm" runat="server" ErrorMessage="CustomValidator"></asp:CustomValidator>
                        </span>
                    </div>
                </div>
            </div>
            <div class="reset-password-form-submit">
                <button class="button button-red button-reset-password">Xác nhận</button>
            </div>
        </div>
    </form>
</body>
</html>
