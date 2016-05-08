
$(document).ready(function () {
    // When delete button is pressed
    $(".modalDeleteButton").one('click', function () {
        // Get user information
        var name = $(this).attr('data-name');
        var uname = $(this).attr('data-uname');
        var email = $(this).attr('data-email');
        var ssn = $(this).attr('data-ssn');
        var role = $(this).attr('data-role');

        // Add user text to modal form
        $('#deleteUserLabel').text(name);
        $('#toDelUserName').text(uname);
        $('#toDelEmail').text(email);
        $('#toDelSsn').text(ssn);
        $('#toDelRole').text(role);
    });
    // When delete form is submitted
    $('#deleteUserForm').submit(function (submitData) {
        // Prevent regular post method
        submitData.preventDefault();
        // Get model information
        var model = {
            name: $('#deleteUserLabel').text(),
            ssn: $('#ssn').text(),
            userModel: {
                userName: $('#toDelUserName').text(),
                email: $('#toDelEmail').text(),
                roleName: $('#toDelRole').text()
            }
        };
        // Call Delete [Post] using model as a parameter
        $.post("/User/Delete", model, function (data) {
            // Update user list
            $('.table').replaceWith($(data).find('.table'));
            // Close modal dialog
            $('#deleteUser').modal('hide');
        });

    });
})
