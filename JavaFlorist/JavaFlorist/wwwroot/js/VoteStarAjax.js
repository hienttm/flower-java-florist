$(document).ready(function () {
    var starIndex = null; // Biến toàn cục để lưu giá trị starIndex

    $('.star').click(function () {
        starIndex = $(this).data('rate');
    });

    $('#postCommentButton').click(function () {
        var comment = $('#commentInput').val();
        var product = $('.star-vote').data('product');

        var data = {
            comment: comment,
            rating: starIndex,
            product: product,
        };

        $.ajax({
            url: '/Product/VoteRate',
            type: 'POST',
            data: data,
            success: function (response) {
                if (response.success) {
                    // Xử lý phản hồi thành công
                    var message = response.message;
                    alert(message); // Hiển thị thông báo thành công
                    location.reload(); // Tải lại trang (tuỳ vào yêu cầu của bạn)
                } else {
                    // Xử lý phản hồi lỗi
                    var message = response.message;
                    alert(message); // Hiển thị thông báo lỗi
                }
            },
            error: function (error) {
                // Xử lý lỗi
            }
        });
    });
});

function getSelectedRating() {
    // Hàm này để lấy giá trị rating được chọn từ các icon sao
    // Bạn có thể sử dụng các phương thức jQuery để lấy giá trị rating được chọn
}
