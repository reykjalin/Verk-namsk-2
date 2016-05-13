$(document).ready(function () {

    $('#userSearch').on('submit', function (data) {
        data.preventDefault();
        var courseId = $('#course_id').val();
        var query = $('#userSearch div input').val();
        console.log(query);

        $.get("/Course/Edit/" + courseId + '?query=' + query, function(data) {
            $('.table tbody').replaceWith($(data).find('.table tbody'));
        });
    });


    $('tbody tr td button').one('click', function () {
        console.log('button press');
        var userName = $(this).attr('data-uname');
        var email = $(this).attr('data-email');
        var role = $(this).attr('data-role');

        console.log(userName);
        console.log(email);
        console.log(role);

        $('#userName').val(userName);
        $('#email').val(email);
        $('#roleName').val(role);
    });


    $('#addUser').on('submit', function(submitData) {
        submitData.preventDefault();
        var model = {
            userName: $('#userName').val(),
            email: $('#email').val(),
            roleName: $('#roleName').val()
        }
        var courseId = $('#course_id').val();
        console.log(courseId);
        $.post("/Course/addUser?courseId=" + courseId, model, function (data) {
            
        });
    });


    function createModel() {
        var model = {
            course: {
                id: $('#course_id'),
                title: $('#course_title'),
                description: $('#course_description')
            },
            assignmentList: [],
            studentList: [],
            taList: [],
            teacherList: []
        };
        return model;
    }
});