$(document).ready(function () {
    $('#addBut').tab('show');
    var button = null;

    $('#partBut').on('click', function () {
        // Make it so you know which button was pressed
        console.log('partBut click!');
        button = $(this);
    });

    $('#assBut').on('click', function () {
        // Make it so you know which button was pressed
        console.log('assBut click!');
        button = $(this);
    });

    $('#partTabs li a').on('click', function (e) {
        // Prevent default behaviour
        e.preventDefault();
        // Show content for the tab that was clicked
        $(this).tab('show');
        // Get id of the content connected to the tab
        var id = $(this).attr('href').replace('#', '');
        // Get content connected to the tab
        var content = document.getElementById(id).children;

        // change IDs for submit
        console.log('clearing old ids...');
        document.getElementById('partName').id = '';
        document.getElementById('partDescr').id = '';
        document.getElementById('partWeight').id = '';
        $('#partStatus').text('');

        console.log('setting new ids...');
        content[1].id = 'partId';
        content[2].children[1].children[0].id = 'partName';
        content[3].children[1].children[0].id = 'partDescr';
        content[4].children[1].children[0].id = 'partWeight';
    });

    $('form').on('submit', function (data) {
        // Stop default submit
        data.preventDefault();
        // Submit part
        // TODO: Finish else statement
        if (button.attr('id') == 'partBut') {
            console.log('partBut');
            // Create AssignmentViewModel using JSON
            var model = {
                title: $('#assTitle').val(),
                description: $('#assDescr').val(),
                weight: $('#assWeight').val(),
                id: $('#assId').val(),
                courseId: $('#courseId').val(),
                assignmentParts: [
                    {
                        title: $('#partName').val(),
                        description: $('#partDescr').val(),
                        weight: $('#partWeight').val(),
                        id: $('#partId').val()
                    }
                ]
            };
            console.log(model);
            
            $.post("/Assignment/addPart", model, function (data) {
                // Reload part view
                $('#partTabs').replaceWith(
                    $(data).find('#partTabs')
                    );
                $('#partStatus').text('Part saved!');
                // TODO: Show newest tab
            });
        }
        // Submit assignment
        else if (button.attr('id') == 'assBut') {
            console.log('assBut');
            var model = {
                title: $('#assTitle').val(),
                description: $('#assDescr').val(),
                weight: $('#assWeight').val(),
                id: $('#assId').val(),
                courseId: $('#courseId').val(),
                assignmentParts: [
                    {
                        title: $('#partName').val(),
                        description: $('#partDescr').val(),
                        weight: $('#partWeight').val()
                    }
                ]
            };
            console.log(model);
        }
    });
});