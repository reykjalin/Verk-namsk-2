$(".modalDetailButton").on('click', function () {
    // Get user details
    var title = $(this).attr('data-title');
    var descr = $(this).attr('data-descr');
    var weight = $(this).attr('data-weight');
    var courseID = $(this).attr('data-courseID');


    // Add text to modal dialog
    $('#assignmentTitle').text(title);
    $('#assignmentDescr').text(descr);
    $('#assignmentWeight').text(weight);
    $('#assignmentID').text(courseID);

});