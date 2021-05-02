
setTimeout(function (e) {
    var path = window.location.pathname;
    if (path == "/account/register/success") {
        alert("Đăng ký thành công, sẽ chuyển hướng đến trang xác minh tài khoản");
        location.replace(window.location.protocol + "//" + window.location.hostname + "/account/confirm");
    } else if (path === "/account/register/failed") {
        alert("Đăng ký thất bại, vui lòng đăng ký lại");
        location.replace(window.location.protocol + "//" + window.location.hostname + "/account/register");
    } else if (path === "/accont/register/success_no-payment-info") {
        alert("Đăng ký thành công, không có thông tin thanh toán. Sẽ chuyển hướng đến trang xác minh tài khoản")
        location.replace(window.location.protocol + "//" + window.location.hostname + "/account/confirm");
    } else if (path === "/account/register/already-exist") {
        alert("Đăng ký thất bại, tài khoản này với username này đã có người dùng");
    }
}, 2000);