$(document).ready(function () {
    $(".modalEditButton").on('click', function () {
        // Get course details
        var title = $(this).attr('data-title');
        var descr = $(this).attr('data-descr');
        var weight = $(this).attr('data-weight');
        var courseId = $(this).attr('data-courseId');
        var id = $(this).attr('data-id');

        // Add text to modal dialog
        $('#assignmentTitleEdit').val(title);
        $('#assignmentDescrEdit').val(descr);
        $('#assignmentWeightEdit').val(weight);
        $('#assignmentCourseIdEdit').val(courseId);
        $('#getId').val(id);
    });

    // When edit form is submitted
    $('#editAssignmentForm').submit(function (submitData) {
        // Prevent regular post method
        submitData.preventDefault();
        // Get model information
        var model = {
            title: $('#assignmentTitleEdit').val(),
            description: $('#assignmentDescrEdit').val(),
            weight: $('#assignmentWeightEdit').val(),
            courseId: $('#assignmentCourseIdEdit').val(),
            id: $('#getId').val(),
        };

        // Call Edit [Post] using model as a parameter
        $.post("/Assignment/Edit", model, function (data) {
            // Update course list
            $('.table').replaceWith($(data).find('.table'));
            // Close modal dialog
            $('#assignmentEdit').modal('hide');
        });

    });
});