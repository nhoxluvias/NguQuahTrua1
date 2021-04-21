<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Confirm.aspx.cs" Inherits="Web.Account.Confirm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/account/confirm.css") %>">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css">
</head>
<body>
    <form id="form1" runat="server">
       <div class="confirm-form">
            <div class="confirm-form-title">
                <h3>Xác minh tài khoản</h3>
                <p>Bước cuối cùng để truy cập tài khoản</p>
                <hr/>
            </div>
            <div class="confirm-form-data">
                <div class="confirm-form-group">
                    <p>Mã xác nhận</p>
                    <input type="text" name="confirm-code" value="" placeholder="Nhập vào mã xác nhận">
                    <div class="show-error">
                        <span>Lỗi sẽ xuất hiện tại đây</span>
                    </div>
                </div>
            </div>
            <div class="confirm-form-submit">
                <button class="button button-red button-confirm">Xác nhận</button>
            </div>
        </div>
    </form>
</body>
</html>
