$(".modalDetailButton").on('click', function () {
    // Get user details
    var title = $(this).attr('data-title');
    var descr = $(this).attr('data-descr');

    // Add text to modal dialog
    $('#courseTitle').text(title);
    $('#courseDescription').text(descr);
});