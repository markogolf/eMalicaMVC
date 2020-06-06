$(function () {
    var deleteLinkObj;
    // delete Link
    $('.order-link').click(function () {
        deleteLinkObj = $(this);  //for future use
        $('#order-dialog').dialog('open');
        return false; // prevents the default behaviour
    });

    //definition of the delete dialog.
    $('#order-dialog').dialog({
        autoOpen: false, width: 400, resizable: false, modal: true, //Dialog options
        buttons: {
            "OK": function () {
                $.post(deleteLinkObj[0].href, function (data) {  //Post to action
                    //alert(data);
                    if (data === "True") {
                        deleteLinkObj.closest("tr").hide('slow'); //Hide Row
                        //(optional) Display Confirmation
                    }
                    else {
                        alert(data + ' else');
                        //(optional) Display Error
                    }
                });
                $(this).dialog("close");
            },
            "Prekliči": function () {
                $(this).dialog("close");
            }
        }
    });
});

$(function () {
    var deleteLinkObj;
    // delete Link
    $('.delete-link').click(function () {
        deleteLinkObj = $(this);  //for future use
        $('#delete-dialog').dialog('open');
        return false; // prevents the default behaviour
    });

    //definition of the delete dialog.
    $('#delete-dialog').dialog({
        autoOpen: false, width: 400, resizable: false, modal: true, //Dialog options
        buttons: {
            "OK": function () {
                $.post(deleteLinkObj[0].href, function (data) {  //Post to action
                    //alert(data);
                    if (data === "True") {
                        deleteLinkObj.closest("div.row").hide('slow'); //Hide Row
                        //(optional) Display Confirmation
                    }
                    else {
                        alert(data + ' else');
                        //(optional) Display Error
                    }
                });
                $(this).dialog("close");
            },
            "Prekliči": function () {
                $(this).dialog("close");
            }
        }
    });
});