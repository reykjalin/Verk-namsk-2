$(document).ready(function () {
    // When delete button is pressed
    $(".modalDeleteButton").one('click', function () {
        // Get course information
        var title = $(this).attr('data-title');
        var descr = $(this).attr('data-descr');
        var id = $(this).attr('data-id');


        // Add course text to modal form
        $('#toDelTitle').text(title);
        $('#toDelDescr').text(descr);
        $('#deleteID').val(id);
    });
    // When delete form is submitted
    $('#deleteCourseForm').submit(function (submitData) {
        // Prevent regular post method
        submitData.preventDefault();
        // Get model information
        var model = {
            title: $('#toDelTitle').text(),
            descr: $('#toDelDescr').text(),
            id: $('#deleteID').val(),
        };

        // Call Delete [Post] using model as a parameter
        $.post("/Course/Remove", model, function (data) {
            // Update course list
            $('.table').replaceWith($(data).find('.table'));
            // Close modal dialog
            $('#deleteCourse').modal('hide');
        });

    });
})
