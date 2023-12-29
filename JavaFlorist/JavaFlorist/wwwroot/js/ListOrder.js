$(document).ready(function () {
    $('.OrderStatus').change(function () {
        var selectedValue = $(this).val(); // Lấy giá trị đã chọn từ <select>
        var product = $(this).data('order');
        // Gửi giá trị đã chọn lên máy chủ bằng Ajax
        var data = {
            product: product,
            order: selectedValue,
        };
        $.ajax({
            url: '/Order/StatusOrder', // Đường dẫn đến phương thức xử lý Ajax trong controller của bạn
            type: 'POST',
            data: data, // Dữ liệu gửi lên máy chủ
            success: function (response) {
                // Xử lý phản hồi từ máy chủ (nếu cần)
                console.log(response);
            },
            error: function (error) {
                // Xử lý lỗi (nếu có)
                console.log(error);
            }
        });
    });
});
