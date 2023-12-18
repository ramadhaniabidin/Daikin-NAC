

NWF.FormFiller.Events.RegisterAfterReady(function () {
    if ((window.location.href.indexOf("NewForm") > -1)) {
        console.log('NewForm');
    } else {
        GetWorkflowHistoryList();
        if (NWF$('#' + cvFormStatus).val() == '' || NWF$('#' + cvFormStatus).val() == 'Revise' || NWF$('#' + cvFormStatus).val() == 'Draft') {
            $(".nf-attachmentsLink").css("display", "block");
            $(".propertysheet").css("display", "block");
        } else {
            $(".nf-attachmentsLink").css("display", "none");
            $(".propertysheet").css("display", "none");
        }
    }
});


function GetWorkflowHistoryList() {
    try{
        var objek = new Object();
        console.log('object textFormNo', NWF$('#' + textFormNo).val());
        console.log('object cvFormNo', NWF$('#' + cvFormNo).html());


        if (location.href.toUpperCase().includes('DISP')) {
            objek.Form_No = NWF$('#' + cvFormNo).val(); //not textbox
        } else {
            objek.Form_No = NWF$('#' + textFormNo).val(); //Actually textbox object
        }

        console.log('Form No', objek.Form_No);


        NWF$.ajax({
            type: "POST",
            url: '/_layouts/15/Daikin.Application/WebServices/NACWebService.asmx/GetHistoryLog2',
            async: false,
            cache: false,
            data: JSON.stringify(objek),
            contentType: "application/json; charset=utf-8",
            success: onSuccessGetWorkflowHistoryLog,
            error: function (xhr, ajaxOptions, thrownError) {
                console.log('Status : ', xhr.status, 'responseText: ', xhr.responseText, '-', thrownError);
                //alert(xhr.status);
                //alert(xhr.responseText);
                //alert(thrownError);
            }
        });
    }catch(e){
        console.log('GetWorkflowHistoryList : ' + e.message);
    }
        

}


function onSuccessGetWorkflowHistoryLog(response) {
    var trHTML = '';
    var parsedData = JSON.parse(response.d);
    var TotalRows = $("#tblHistory").find("tr:not(:first)").length;
    if (TotalRows > 0) {
        $("#tblHistory").find("tr:not(:first)").remove();
    }
    $.each(parsedData, function () {
        trHTML += '<tr><td>' + this.Personal_Name + '</td>';
        trHTML += '<td>' + this.Position + '</td>';
        trHTML += '<td>' + this.Comments + '</td>';
        trHTML += '<td>' + this.Action_Name + '</td>';
        trHTML += '<td>' + this.Action_Date + '</td></tr>';
    });
    console.log(trHTML);
    $("#tblHistory > tbody").append(trHTML);
    console.log($('#tblHistory').html());
}

//function onSuccessGetWorkflowHistoryLog(data) {
//    var trHTML = '';
//    console.log('data', data);
//    console.log('data.d', data.d);

//    //var parsedData = JSON.parse(response.data.d.Logs);
//    //var TotalRows = $("#tblHistory").find("tr:not(:first)").length;
//    //if (TotalRows > 0) {
//    //    $("#tblHistory").find("tr:not(:first)").remove();
//    //}
//    //$.each(parsedData, function () {
//    //    trHTML += '<tr><td>' + this.Personal_Name + '</td>';
//    //    trHTML += '<td>' + this.Position + '</td>';
//    //    trHTML += '<td>' + this.Comment + '</td>';
//    //    trHTML += '<td>' + this.Action_Name + '</td></tr>';
//    //    trHTML += '<td>' + this.Action_Date + '</td></tr>';
//    //});
//    //$("#tblHistory").append(trHTML);
//}