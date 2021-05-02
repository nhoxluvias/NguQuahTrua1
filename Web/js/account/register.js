
setTimeout(function (e) {
    var path = window.location.pathname;
    if (path == "/account/register/success")
        alert("Đăng ký thành công, sẽ chuyển hướng tới trang đăng nhập");
    location.replace(window.location.protocol + "//" + window.location.hostname + "/account/login");
    if (path === "/account/login/failed")
        alert("Đăng ký thất bại, vui lòng đăng ký lại");
    location.replace(window.location.protocol + "//" + window.location.hostname + "/account/register");
}, 2000);