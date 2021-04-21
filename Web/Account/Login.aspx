<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Web.Account.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="<%= ResolveUrl("~/css/account/login.css") %>">
    <script type="text/javascript" src="<%= ResolveUrl("~/libraries/js/video.js") %>"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css">
</head>
<body>
    <form id="form1" runat="server">
        <video id="backgroundVideo" class="background-video" autoplay>
            <source src="https://www.phanxuanchanh.com/trailer.mp4" type='video/mp4'>
            <p class="cc">Không thể phát video</p>
          </video>
        <div class="audio-tool">
            <div class="space">

            </div>
            <div class="audio-control" onclick="switchAudioState();">
                <i id="audioControlTitle" class="fa fa-volume-up"></i>
            </div>
        </div>
        <div class="login-status">
            <div class="login-status-space"></div>
            <div class="login-status-message">
                <h3>Chào mừng bạn quay trở lại</h3>
            </div>
        </div>
        <div class="video-info">
            <h3>Bạn đang xem? </h3>
            <h4>Marvel Studios' Loki | Official Trailer | Disney+</h4>
            <p class="cc">Loki’s time has come. Watch the brand-new trailer for "Loki," and start streaming the Marvel Studios Original Series June 11 on Disney+.</p>
        </div>
        <div class="notify">
            <div class="notify-title">
                <h3>Bạn không thể không xem</h3>
                <hr/>
            </div>
            <div class="notify-list">
                <div class="notify-item">
                    <h5>Phim 1</h5>
                    <p class="cc">Mô tả phim 1</p>
                </div>
                <div class="notify-item">
                    <h5>Phim 1</h5>
                    <p class="cc">Mô tả phim 1</p>
                </div>
                <div class="notify-item">
                    <h5>Phim 1</h5>
                    <p class="cc">Mô tả phim 1</p>
                </div>
                <div class="notify-item">
                    <h5>Phim 1</h5>
                    <p class="cc">Mô tả phim 1</p>
                </div>
                <div class="notify-item">
                    <h5>Phim 1</h5>
                    <p class="cc">Mô tả phim 1</p>
                </div>
            </div>
        </div>
        <div class="account-form login">
            <div class="account-form-title">
                <h3>Đăng nhập</h3>
                <p class="cc">Đăng nhập ngay để không bỏ lỡ những bộ phim hấp dẫn</p>
            </div>
            <div class="account-form-data">
                <input type="text" name="username" value="" placeholder="Nhập vào tên người dùng">
                <input type="password" name="password" value="" placeholder="Nhập vào mật khẩu">
            </div>
            <div class="account-form-submit">
                <button class="button button-red button-login">Đăng nhập</button>
            </div>
            <div class="show-error">
                <div class="show-error-item">
                    <span>Lỗi sẽ xuất hiện tại đây</span>
                </div>
                <div class="show-error-item">
                    <span>Lỗi sẽ xuất hiện tại đây</span>
                </div>
            </div>
            <div class="account-form-support">
                <span><asp:HyperLink ID="hylnkResetPassword" runat="server">Bạn quên mật khẩu?</asp:HyperLink></span>
                <span><asp:HyperLink ID="hylnkRegister" runat="server">Bạn chưa có tài khoản?</asp:HyperLink></span>
                <span><asp:HyperLink ID="hylnkFeedback" runat="server">Gửi ý kiến phản hồi</asp:HyperLink></span>
                <span><asp:HyperLink ID="hylnkContact" runat="server">Liên hệ với chúng tôi</asp:HyperLink></span>
                <span><asp:HyperLink ID="hylnkTermOfUse" runat="server">Điều khoản sử dụng dịch vụ</asp:HyperLink></span>
            </div>
        </div>
        <script src="<%= ResolveUrl("~/js/account/account.js") %>">
           
        </script>
    </form>
</body>
</html>
