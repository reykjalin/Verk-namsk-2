$(".modalDetailButton").on('click', function () {
    // Get user details
    var title = $(this).attr('data-title');
    var descr = $(this).attr('data-descr');
    var id = $(this).attr('data-id');

    // Add text to modal dialog
    $('#courseTitle').text(title);
    $('#courseDescription').text(descr);
    $('.panel-group').hide();
    $('#' + id + ' .panel-group').show();
});