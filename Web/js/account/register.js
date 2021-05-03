
setTimeout(function (e) {
    var path = window.location.pathname;
    if (path === "/account/register/register-failed") {
        alert("Đăng ký thất bại, vui lòng đăng ký lại");
        location.replace(window.location.protocol + "//" + window.location.hostname + "/account/register");
    }
    if (path === "/account/register/register-failed_already-exist") {
        alert("Đăng ký thất bại, tài khoản này với username này đã có người dùng");
    }
}, 2000);