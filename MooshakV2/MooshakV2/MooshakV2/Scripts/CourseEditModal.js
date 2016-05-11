$(document).ready(function () {
    $(".modalEditButton").on('click', function () {
        // Get course details
        var title = $(this).attr('data-title');
        var descr = $(this).attr('data-descr');
        var id = $(this).attr('data-id');

        // Add text to modal dialog
        $('#courseTitleEdit').val(title);
        $('#courseDescriptionEdit').val(descr);
        $('#getId').val(id);
    });

    // When edit form is submitted
    $('#editCourseForm').submit(function (submitData) {
        // Prevent regular post method
        submitData.preventDefault();
        // Get model information
        var model = {
            title: $('#courseTitleEdit').val(),
            description: $('#courseDescriptionEdit').val(),
            id: $('#getId').val(),
            
        };

        // Call Edit [Post] using model as a parameter
        $.post("/Course/Edit", model, function (data) {
            // Update course list
            $('.table').replaceWith($(data).find('.table'));
            // Close modal dialog
            $('#courseEdit').modal('hide');
        });

    });
});