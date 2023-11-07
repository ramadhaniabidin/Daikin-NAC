function SaveApproval(approvalValue, ListName, ListItemID) {
    param = {
        approvalValue: approvalValue,
        ListName: ListName,
        ListItemID: ListItemID,
    };

    console.log('Save & Submit Triggered.');

    console.log('PARAM', param)
    try {
        jQuery.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/_layouts/15/Daikin.Application/WebServices/NACWebService.asmx/ApproveRequest",
            data: JSON.stringify(param),
            dataType: "json",
            async: true,
            success: function () {
                alert('Successfully Added!');
            },
            error: function (xhr) {-
                console.log('Error: ' + xhr.responseText)
                alert(xhr.responseText);
            }
        })
    }
    catch (ex)
    {
        alert(ex.message)
    }

}