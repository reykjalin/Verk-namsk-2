$(".modalDetailButton").on('click',function() {
    // Get user details
    var name = $(this).attr('data-name');
    var uname = $(this).attr('data-uname');
    var email = $(this).attr('data-email');
    var ssn = $(this).attr('data-ssn');
    var role = $(this).attr('data-role');

    // Add text to modal dialog
    $('#userDetailsLabel').text(name);
    $('#userName').text(uname);
    $('#email').text(email);
    $('#ssn_detail').text(ssn);
    $('#role').text(role);
});