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
                <hr />
            </div>
            <div class="register-form-data">
                <div class="register-form-group register-form-group-left">
                    <h4>Thông tin cơ bản</h4>
                    <p>Tên người dùng</p>
                    <asp:TextBox ID="txtUsername" Placeholder="Nhập vào tên người dùng" Text="" TextMode="SingleLine" runat="server"></asp:TextBox>
                    <div class="show-error">
                        <asp:CustomValidator ID="cvUsername" runat="server"></asp:CustomValidator>
                    </div>
                    <p>Địa chỉ Email</p>
                    <asp:TextBox ID="txtEmail" Placeholder="Nhập vào địa chỉ Email" Text="" TextMode="Email" runat="server"></asp:TextBox>
                    <div class="show-error">
                        <asp:CustomValidator ID="cvEmail" runat="server"></asp:CustomValidator>
                    </div>
                    <p>Số điện thoại</p>
                    <asp:TextBox ID="txtPhoneNumber" Placeholder="Nhập vào số điện thoại" Text="" TextMode="Email" runat="server"></asp:TextBox>
                    <div class="show-error">
                        <<asp:CustomValidator ID="cvPhoneNumber" runat="server"></asp:CustomValidator>
                    </div>
                    <p>Mật khẩu</p>
                    <asp:TextBox ID="txtPassword" Placeholder="Nhập vào mật khẩu" Text="" TextMode="Password" runat="server"></asp:TextBox>
                    <div class="show-error">
                        <asp:CustomValidator ID="cvPassword" runat="server"></asp:CustomValidator>
                    </div>
                    <p>Xác nhận mật khẩu</p>
                    <asp:TextBox ID="txtRePassword" Placeholder="Nhập vào xác nhận mật khẩu" Text="" TextMode="Password" runat="server"></asp:TextBox>
                    <div class="show-error">
                        <asp:CustomValidator ID="cvRePassword" runat="server"></asp:CustomValidator>
                    </div>
                </div>
                <div class="register-form-group register-form-group-right">
                    <h4>Thông tin thanh toán</h4>
                    <p>Số thẻ</p>
                    <asp:TextBox ID="txtCardNumber" Placeholder="Nhập vào số thẻ" Text="" TextMode="SingleLine" runat="server"></asp:TextBox>
                    <div class="show-error">
                        <asp:CustomValidator ID="cvCardNumber" runat="server"></asp:CustomValidator>
                    </div>
                    <p>CVV</p>
                    <input type="text" name="cvv" value="" placeholder="Nhập vào số CVV">
                    <asp:TextBox ID="txtCvv" Placeholder="Nhập vào số CVV" Text="" TextMode="SingleLine" runat="server"></asp:TextBox>
                    <div class="show-error">
                        <asp:CustomValidator ID="cvCvv" runat="server"></asp:CustomValidator>
                    </div>
                    <p>Tên chủ tài khoản</p>
                    <input type="text" name="accountName" value="" placeholder="Nhập vào tên tài khoản">
                    <asp:TextBox ID="txtAccountName" Placeholder="Nhập vào tên tài khoản" Text="" TextMode="SingleLine" runat="server"></asp:TextBox>
                    <div class="show-error">
                        <asp:CustomValidator ID="cvAccountName" runat="server"></asp:CustomValidator>
                    </div>
                    <p>Ngày hết hạn</p>
                    <input type="text" name="expirationDate" value="" placeholder="Nhập vào ngày hết hạn">
                    <asp:TextBox ID="txtExprationDate" Placeholder="Nhập vào ngày hết hạn" Text="" TextMode="SingleLine" runat="server"></asp:TextBox>
                    <div class="show-error">
                        <asp:CustomValidator ID="cvEpirationDate" runat="server"></asp:CustomValidator>
                    </div>
                    <p>Phương thức thanh toán</p>
                    <select name="paymentMethod" id="cars">
                        <option value="visa">Thẻ Visa</option>
                        <option value="mastercard">Thẻ Mastercard</option>
                    </select>
                </div>
            </div>
            <div class="register-form-submit">
                <asp:Button ID="btnSubmit" CssClass="button button-red button-register" runat="server" Text="Button" />
            </div>
        </div>
    </form>
</body>
</html>
