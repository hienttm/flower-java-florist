$(document).ready(function () {
    $('#rangeInput').on('input', function () {
        var price = $(this).val();
        $.ajax({
            url: '/Product/FilterByPrice',
            type: 'GET',
            data: { Price: price }, // Điều chỉnh "price" thành "Price" để phù hợp với tên tham số trong Controller
            success: function (result) {
                $('#filteredDataContainer').html(result);
            },
            error: function (error) {
                // Xử lý lỗi
                alert(error.responseText);
            }
        });
    });
});