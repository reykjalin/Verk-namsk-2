$(document).ready(function () {
    $('#addBut').tab('show');
    var button = null;

    // Make it so you know which button was pressed
    $('#partBut').on('click', function () {
        console.log('partBut click!');
        button = $(this);
    });
    $('#assBut').on('click', function () {
        console.log('assBut click!');
        button = $(this);
    });
    $('#partDelBut').on('click', function () {
        console.log('delBut click!');
        button = $(this);
    });


    // Define behaviour when a tab is clicked
    $('#partDiv').on('click', "#partTabs li a", function (e) {
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
        document.getElementById('partId').id = '';
        $('#partStatus').text('');

        console.log('setting new ids...');
        content[1].id = 'partId';
        content[2].children[1].children[0].id = 'partName';
        content[3].children[1].children[0].id = 'partDescr';
        content[4].children[1].children[0].id = 'partWeight';
    });


    // Define submit behaviour
    $('form').on('submit', function (data) {
        // Submit part
        // TODO: Finish else statement
        if (button.attr('id') == 'partBut') {
            // Stop default submit
            data.preventDefault();
            console.log('partBut');
            // Create AssignmentViewModel using JSON
            var model = createModel();
            console.log(model);
            
            $.post("/Assignment/addPart", model, function (data) {
                //// Reload part view
                //$('#partTabs').replaceWith(
                //    $(data).find('#partTabs')
                //    );
                // Reload page, TODO: make it dynamic; no reload
                location.reload();
                $('#partStatus').text('Part saved!');
                // TODO: Show newest tab
            });
        }
        else if (button.attr('id') == 'partDelBut') {
            console.log('delBut');
            data.preventDefault();
            var model = createModel();
            $.post("/Assignment/delPart", model, function() {
                location.reload();
                $("partStatus").text('Part deleted!');
            });
        } // Submit assignment
        else if (button.attr('id') == 'assBut') {
            console.log('assBut');
            var model = createModel();
            console.log(model);
            $('.form').submit(model);
            window.location.replace("/Assignment/List");
        }
    });


    // Create model using form data
    function createModel() {
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
        return model;
    }
});