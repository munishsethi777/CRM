


function ClearClliForm() {
    try {

        //clear hidden ctrl = 0
        $('#CreateForm input[type=hidden]').each(function () {
            $(this).val('0');
        });

        ////clear text ctrl = 0
        $('#CreateForm input[type=text]').each(function () {
            $(this).val('');
        });

        ////drop down controls
        $('#CreateForm select').each(function () {
            $(this)[0].selectedIndex = 0;
            $(this).change();
        });
    }
    catch (e) {
        ScriptDialogError(e);
    }
}

// Dialog - Delete Entry
function DeleteItem(prmId) {
    Confirmation('Are you sure to delete?', prmId);
}

//delete selected item
function DeleteSelectedItem() {
    //get selected rows
    var selMulti = $.map($('#tblGrid').jqGrid('getGridParam', 'selarrrow'), function (el, i) {
        return el;
    });
    if (selMulti.length > 0)
        DeleteItem(selMulti.toString());
    else
        AlertMsg('Please select the Item');
}
function SubmitValidation() {
    var bool = $("#CreateForm").valid()
    return bool;
}

// status - true - click yes, false - click no
function CallbackConfirmation(status, prmId) {
    if (status) {
        $.post("Delete", { sIds: prmId }, function (data) {

            if (data.result == "fail") {
                AlertError(data.message);
                return;
            }
            //refress the grid again
            ReloadGrid('tblGrid');
            AlertMsg(data.message)
        });


    }
}

// Alert message
$("#dialog-message").dialog({
    modal: true,
    autoOpen: false,
    buttons: {
        Ok: function () {
            $(this).dialog("close");
        }
    }
});

//dialog for confirmation
$("#dialog-confirm").dialog({
    resizable: true,
    height: 170,
    width: 350,
    modal: true,
    autoOpen: false,
    buttons: {
        'Yes': function () {
            $(this).dialog('close');
            var item = $(this).dialog('option', 'value')
            CallbackConfirmation(true, item);
        },
        'No': function () {
            $(this).dialog('close');
            CallbackConfirmation(false, null);
        }
    }
});
function Confirmation(msg, item) {
    $("#dialog-confirm .confirmmsg").text(msg);
    $("#dialog-confirm").dialog('option', 'value', item);
    $("#dialog-confirm").dialog('open');
}


// alert msg prototype
function AlertMsg(msg) {
    $("#dialog-message .confirmmsg").text(msg);
    $("#dialog-message").dialog("open");
}
// alert msg prototype - for pop-up window
function AlertDialogSuccess(msg) {

    $("#alertDialogSuccess").css('display', 'block');
    var success = "<div class='alert alert-success'><button type='button' class='close' data-dismiss='alert'>&times;</button><span>" + msg + "</span></div>";
    $("#alertDialogSuccess").html(success);
}
// alert msg prototype - pop - up window
function AlertDialogError(msg) {
    $("#alertDialogError").css('display', 'block');
    var error = "<div class='alert alert-error'><button type='button' class='close' data-dismiss='alert'>&times;</button><span>" + msg + "</span></div>";
    $("#alertDialogError").html(error);
}

// alert msg prototype - for main page
function AlertSuccess(msg) {
    $("#alertSuccess").css('display', 'block');
    var success = "<div class='alert alert-success'><button type='button' class='close' data-dismiss='alert'>&times;</button><span>" + msg + "</span></div>";
    $("#alertSuccess").html(success);
}
// alert msg prototype - main page
function AlertError(msg) {
    $("#alertError").css('display', 'block');
    var error = "<div class='alert alert-error'><button type='button' class='close' data-dismiss='alert'>&times;</button><span class='errorMsg'>" + msg + "</span></div>";
    $("#alertError").html(error);
}

function ScriptError(ex) {
        AlertError('Runtime script error occured.');
}

function ScriptDialogError(ex) {
    if ($('#hdnLoginRoleId').val() == "2")
        AlertDialogError(ex);
    else
        AlertDialogError('Runtime script error occured.');
}
function ShowDialog(dialogtitle, width, height, URL) {
    // clear dialog window
    $('#dialog-add').html('');
    // Dialogs window for add
    $("#dialog-add").dialog({
        title: dialogtitle,
        autoOpen: false,
        resizable: true,
        width: width,
        height: height,
        position: 'center',
        modal: true,
        draggable: true,
        open: function (event, ui) {
            $.post(URL,
                {
                },
                function (data) {
                    $('#dialog-add').html(data.Data.view);
                }
            );
        },
        close: function (event, ui) {
            $(this).dialog('close');
        }
    });
    //open dialog
    $('#dialog-add').dialog('open');
    return false;
}

function EditItem(id) {
    location.href = "Edit\\" + id;
}



function onSubmitSuccess(data) {
    if (data.result == "fail") {
        CloseDialog();
        AlertError(data.message);
        return;
    }
    //refress the grid again
    ReloadGrid('tblGrid');

    if (!data.isUpdate) {
        ClearClliForm();
        AlertDialogSuccess(data.message);
    }
    else {
        CloseDialog();
        AlertMsg(data.message);
    }
}

function LoadComplete(gridName) {

    //alert('complted');
    $('#' + gridName).bind('jqGridLoadComplete', function (e, data) {

        //        if (this.p.records === 0) {
        //            setTimeout(function () {
        //                alert("No Matches Found.");
        //            }, 500);
        //        }

        if ($(this).jqGrid('getGridParam', 'reccount') == 0) {
            $(".ui-jqgrid-hdiv").css("overflow-x", "auto")
        }
        else {
            $(".ui-jqgrid-hdiv").css("overflow-x", "hidden")
        }
    });
}
function ReloadGrid(gridName) {
    $('#' + gridName).jqGrid('setGridParam', { 'datatype': 'json' });
    $('#' + gridName).trigger("reloadGrid");
}

//hide and show the column - runtime
function GridColumnChooser(gridName) {
    var grid = $('#' + gridName);
    grid.jqGrid('navButtonAdd', '#pager',
                     {
                         caption: "Hide/Show", buttonicon: "ui-icon-calculator",
                         title: "Hide/Show Columns",
                         onClickButton: function () {
                             grid.jqGrid('columnChooser');
                         }
                     });
}

//hide the grid columns
function HideGridColumn(gridName, columns) {
    var grid = $('#' + gridName);
    grid.jqGrid('hideCol', columns);

    $.each(columns, function (index, item) {
        grid.jqGrid('setColProp', item, { classes: 'Hide' });
    });

}