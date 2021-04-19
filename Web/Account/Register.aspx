<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Web.Account.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Đăng ký tài khoản</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/account/register.css") %>">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css">
</head>
<body>
    <form id="form1" runat="server">
        <div class="register-form">
            <div class="register-form-title">
                <h3>Đăng ký tài khoản</h3>
                <p>Hãy đăng ký để thưởng thức những tác phẩm điện ảnh hay, chất lượng nhất</p>
                <hr/>
            </div>
            <div class="register-form-data">
                <div class="register-form-group register-form-group-left">
                    <h4>Thông tin cơ bản</h4>
                    <p>Tên người dùng</p>
                    <input type="text" name="username" value="" placeholder="Nhập vào tên người dùng">
                    <asp:TextBox ID="txtUsername" Text="" Placeholder="Nhập vào tên người dùng" runat="server"></asp:TextBox>
                    <p>Địa chỉ Email</p>
                    <input type="text" name="email" value="" placeholder="Nhập vào địa chỉ Email">
                    <p>Số điện thoại</p>
                    <input type="text" name="phoneNumber" value="" placeholder="Nhập vào số điện thoại">
                    <p>Mật khẩu</p>
                    <input type="password" name="password" value="" placeholder="Nhập vào mật khẩu">
                    <p>Xác nhận mật khẩu</p>
                    <input type="password" name="rePassword" value="" placeholder="Nhập vào xác nhận mật khẩu">
                </div>
                <div class="register-form-group register-form-group-right">
                    <h4>Thông tin thanh toán</h4>
                    <p>Số thẻ</p>
                    <input type="text" name="cardNumber" value="" placeholder="Nhập vào số thẻ">
                    <p>CVV</p>
                    <input type="text" name="cvv" value="" placeholder="Nhập vào số CVV">
                    <p>Tên chủ tài khoản</p>
                    <input type="text" name="accountName" value="" placeholder="Nhập vào tên tài khoản">
                    <p>Ngày hết hạn</p>
                    <input type="text" name="expirationDate" value="" placeholder="Nhập vào ngày hết hạn">
                    <p>Phương thức thanh toán</p>
                    <select name="paymentMethod" id="cars">
                        <option value="visa">Thẻ Visa</option>
                        <option value="mastercard">Thẻ Mastercard</option>
                    </select>
                </div>
            </div>
            <div class="register-form-submit">
                <button class="button button-red button-register">Đăng ký</button>
            </div>
        </div>
    </form>
</body>
</html>
