$("body").on("click", "#btnAddProjeAd", function () {
    //Reference the Name and Country TextBoxes.
    var txtProjeAdSoruNo = $("#txtProjeAdNo");
    var txtProjeAdSoru = $("#txtProjeAdSoru")

    var rowCount = $('#tblProjeEkle').find('tr').length;

    //Get the reference of the Table's TBODY element.
    var tBody = $("#tblProjeEkle > TBODY")[0];

    //Add Row.
    var row = tBody.insertRow(-1);

    //Add SoruNo cell.
    var cell = $(row.insertCell(-1));
    cell.html(txtProjeAdSoruNo.val());



    //Add Proje Ad cell
    cell = $(row.insertCell(-1));
    cell.html(txtProjeAdSoru.val());

    //Add Button cell.
    cell = $(row.insertCell(-1));
    var btnRemove = $("<input />");
    btnRemove.attr("type", "button");
    btnRemove.attr("onclick", "RemoveProjeAd(this);");
    btnRemove.val("Sil");
    cell.append(btnRemove);

    //Clear the TextBoxes.

    txtProjeAdSoruNo.val(rowCount);
    txtProjeAdSoru.val("");


});


$("body").on("click", "#btnAddProjeAdSablon", function () {
    //Get the parent row of the clicked button.
    var parentRow = $(this).closest("tr");

    //Get the value of the ProjeAdi input in the parent row.
    var projeAdi = parentRow.find("input[type='text']").val();

    //Get the reference of the Table's TBODY element.
    var tBody = $("#tblProjeEkle > TBODY")[0];

    //Add Row.
    var row = tBody.insertRow(-1);

    //Add SoruNo cell.
    var cell = $(row.insertCell(-1));
    cell.html($('#tblProjeEkle').find('tr').length - 1);

    //Add Proje Ad cell
    cell = $(row.insertCell(-1));
    cell.html(projeAdi);

    //Add Button cell.
    cell = $(row.insertCell(-1));
    var btnRemove = $("<input />");
    btnRemove.attr("type", "button");
    btnRemove.attr("onclick", "RemoveProjeAd(this);");
    btnRemove.val("Sil");
    cell.append(btnRemove);

    //Clear the TextBoxes.
    parentRow.find("input[type='text']").val("");
});

function RemoveProjeAd(button) {
    //Determine the reference of the Row using the Button.
    var row = $(button).closest("TR");
    var name = $("TD", row).eq(0).html();
    if (confirm("Bu satırı silmek istediğinize emin misiniz?" + name)) {
        //Get the reference of the Table.
        var table = $("#tblProjeEkle")[0];

        //Delete the Table row using it's Index.
        table.deleteRow(row[0].rowIndex);
    }
};

$("body").on("click", "#btnUpdate", function () {
    //Loop through the Table rows and build a JSON array.
    var anket = new Array();
    $("#tblProjeEkle TBODY TR").each(function () {
        var row = $(this);
        var proje = {};
        proje.ProjeAdi = row.find("TD").eq(1).html();

        anket.push(proje);
    });

    //Send the JSON array to Controller using AJAX.
    $.ajax({
        type: "POST",
        url: "/Management/UpdateProjeler",
        data: JSON.stringify(anket),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Hata oluştu: " + errorThrown);
        }
    });
});