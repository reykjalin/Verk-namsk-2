$(document).ready(function () {
    // When delete button is pressed
    $(".modalDeleteButton").one('click', function () {
        // Get assignment information
        var title = $(this).attr('data-title');
        var descr = $(this).attr('data-descr');
        var weight = $(this).attr('data-weight');
        var courseId = $(this).attr('data-courseId');
        var id = $(this).attr('data-id');


        // Add assignment text to modal form
        $('#assignmentDelTitle').text(title);
        $('#assignmentDelDescr').text(descr);
        $('#assignmentDelWeight').text(weight);
        $('#courseDelId').text(courseId);
        $('#deleteId').val(id);
    });
    // When delete form is submitted
    $('#deleteAssignmentForm').submit(function (submitData) {
        // Prevent regular post method
        submitData.preventDefault();
        // Get model information
        var model = {
            title: $('#assignmentDelTitle').text(),
            description: $('#assignmentDelDescr').text(),
            weight: $('#assignmentDelWeight').text(),
            courseId: $('#courseDelId').text(),
            id: $('#deleteId').val(),
        };
        // Call Delete [Post] using model as a parameter
        $.post("/Assignment/Remove", model, function (data) {
            // Update assignment list
            $('.table').replaceWith($(data).find('.table'));
            // Close modal dialog
            $('#deleteAssignment').modal('hide');
        });

    });
})
