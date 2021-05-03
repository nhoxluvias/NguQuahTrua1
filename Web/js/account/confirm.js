setTimeout(function (e) {
    var path = window.location.pathname;
    var regex1 = new RegExp("(\/account\/confirm\/)[a-zA-Z0-9-]{1,}(/register-success)$");
    if (regex1.test(path)) {
        alert("Đăng ký thành công, hãy nhập mã xác nhận được gửi tới email của bạn để hoàn tất đăng ký");
    }
    var regex2 = new RegExp("(\/account\/confirm\/)[a-zA-Z0-9-]{1,}(/register-success_no-payment-info)$")
    if (regex2.test(path)) {
        alert(
            "Đăng ký thành công, thông tin thanh toán chưa được nhập, bạn có thể thêm vào sau."
            + " Hãy nhập mã xác nhận được gửi tới email của bạn để hoàn tất đăng ký"
        );
    }
    var regex3 = new RegExp("(\/account\/confirm\/)[a-zA-Z0-9-]{1,}(/login-success_unconfirmed)$");
    if (regex3.test(path)) {
        alert(
            "Bạn đã có tài khoản và đăng nhập thành công, hoàn thành bước cuối cùng để sử dụng tài khoản"
            + " Hãy nhập mã xác nhận được gửi tới email của bạn để hoàn tất đăng ký"
        );
    }
    var regex4 = new RegExp("(\/account\/confirm\/)[a-zA-Z0-9-]{1,}(/confirm-failed)$");
    if (regex4.test(path)) {
        alert("Mã xác nhận không đúng, vui lòng nhập lại");
    }
}, 2000);