setTimeout(function (e) {
    var path = window.location.pathname;
    var regex = new RegExp("(\/account\/confirm\/)[a-zA-Z0-9-]{1,}(/)[a-zA-Z0-9]{1,}(/register-success)$");
    if (regex.test(path)) {
        alert("Đăng ký thành công, hãy nhập mã xác nhận được gửi tới email của bạn để hoàn tất đăng ký");
    }
    regex = null;
    regex = new RegExp("(\/account\/confirm\/)[a-zA-Z0-9-]{1,}(/)[a-zA-Z0-9]{1,}(/register-success_no-payment-info)$")
    if (regex.test(path)) {
        alert(
            "Đăng ký thành công, thông tin thanh toán chưa được nhập, bạn có thể thêm vào sau."
            + " Hãy nhập mã xác nhận được gửi tới email của bạn để hoàn tất đăng ký"
        );
    }
    regex = null;
    regex = new RegExp("(\/account\/confirm\/)[a-zA-Z0-9-]{1,}(/)[a-zA-Z0-9]{1,}(/login-success_unconfirmed)$");
    if (regex.test(path)) {
        alert(
            "Bạn đã có tài khoản và đăng nhập thành công, hoàn thành bước cuối cùng để sử dụng tài khoản"
            + " Hãy nhập mã xác nhận được gửi tới email của bạn để hoàn tất đăng ký"
        );
    }
    regex = null;
    regex = new RegExp("(\/account\/confirm\/)[a-zA-Z0-9-]{1,}(/)[a-zA-Z0-9]{1,}(/re-confirm)$")
    if (regex.test(path)) {
        alert(
            "Đã gửi tới email của bạn mã xác nhận mới, vui lòng nhập mã xác nhận để hoàn tất"
        );
    }
    regex = null;
    regex = new RegExp("(\/account\/confirm\/)[a-zA-Z0-9-]{1,}(/)[a-zA-Z0-9]{1,}(/confirm-failed)$");
    if (regex.test(path)) {
        alert("Mã xác nhận không đúng, vui lòng nhập lại");
    }
}, 2000);