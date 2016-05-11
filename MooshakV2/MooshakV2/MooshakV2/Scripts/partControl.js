$(document).ready(function () {
    var button = null;

    $('#partBut').on('click', function () {
        button = $(this);
    });

    $('#assBut').on('click', function (data) {
        button = $(this);
    });

    $('form').on('submit', function(data) {
        data.preventDefault();
        if (button.attr('id') == 'partBut') {
            console.log('partBut');
        }
        else if (button.attr('id') == 'assBut') {
            console.log('assBut');
        }
    });
});