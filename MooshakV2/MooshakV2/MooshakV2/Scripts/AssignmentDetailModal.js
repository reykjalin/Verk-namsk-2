$(".modalDetailButton").on('click', function () {
    // Get user details
    var title = $(this).attr('data-title');
    var descr = $(this).attr('data-descr');
    var weight = $(this).attr('data-weight');
    var courseId = $(this).attr('data-courseId');


    // Add text to modal dialog
    $('#assignmentTitle').text(title);
    $('#assignmentDescr').text(descr);
    $('#assignmentWeight').text(weight);
    $('#courseId').text(courseId);

});

$("button.modalSubmitButton").on('click', function () {
    // Get user details
    var id = $(this).attr('data-assignmentid');
   



    // Add text to modal dialog
    $('#uploadId').val(id);


});