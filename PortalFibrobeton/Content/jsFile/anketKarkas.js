$("body").on("click", "#btnAddKarkas", function () {
    //Reference the Name and Country TextBoxes.
    var txtNo = $("#txtNoKarkas");
    var txtSoru = $("#txtSoruKarkas");
    var txtK1 = $("#txtK1Karkas");
    var txtK2 = $("#txtK2Karkas");
    var txtK3 = $("#txtK3Karkas");
    var txtK4 = $("#txtK4Karkas");
    var txtK5 = $("#txtK5Karkas");
    var txtAsama = $("#txtAsamaKarkas");
    var txtAnketAdi = $("#anketAdi");
    var karkasGenelValue = $("#karkasGenel").val();

    var rowCount = $('#tblKarkas').find('tr').length;



    //Get the reference of the Table's TBODY element.
    var tBody = $("#tblKarkas > TBODY")[0];

    //Add Row.
    var row = tBody.insertRow(-1);

    //Add SoruNo cell.
    var cell = $(row.insertCell(-1));
    cell.html(txtNo.val());

    //Add Soru cell
    cell = $(row.insertCell(-1));
    cell.html(txtSoru.val());

    //Add K Series cell.
    cell = $(row.insertCell(-1));
    cell.html(txtK1.val());
    cell = $(row.insertCell(-1));
    cell.html(txtK2.val());
    cell = $(row.insertCell(-1));
    cell.html(txtK3.val());
    cell = $(row.insertCell(-1));
    cell.html(txtK4.val());
    cell = $(row.insertCell(-1));
    cell.html(txtK5.val());


    //Add Aşama cell
    cell = $(row.insertCell(-1));
    cell.html(txtAsama.val());

    //Add Anket Adi Cell
    cell = $(row.insertCell(-1));
    cell.html(txtAnketAdi.val());

    //Karkas Genel Katsayi Cell
    cell = $(row.insertCell(-1));
    cell.html(karkasGenelValue);


    //Add Button cell.
    cell = $(row.insertCell(-1));
    var btnRemove = $("<input />");
    btnRemove.attr("type", "button");
    btnRemove.attr("onclick", "RemoveKarkas(this);");
    btnRemove.val("Sil");
    cell.append(btnRemove);

    //Clear the TextBoxes.

    txtNo.val(rowCount);
    txtSoru.val("");
    txtK1.val("");
    txtK2.val("");
    txtK3.val("");
    txtK4.val("");
    txtK5.val("");
    txtAsama.val("Karkas");


});

function RemoveKarkas(button) {
    //Determine the reference of the Row using the Button.
    var row = $(button).closest("TR");
    var name = $("TD", row).eq(0).html();
    if (confirm("Bu satırı silmek istediğinize emin misiniz?: " + name)) {
        //Get the reference of the Table.
        var table = $("#tblKarkas")[0];

        //Delete the Table row using it's Index.
        table.deleteRow(row[0].rowIndex);
    }
};

$("body").on("click", "#btnSave", function () {
    //Loop through the Table rows and build a JSON array.
    var anket = new Array();
    $("#tblKarkas TBODY TR").each(function () {
        var row = $(this);
        var proje = {};
        proje.SoruNo = row.find("TD").eq(0).html();
        proje.Soru = row.find("TD").eq(1).html();
        proje.K1 = row.find("TD").eq(2).html();
        proje.K2 = row.find("TD").eq(3).html();
        proje.K3 = row.find("TD").eq(4).html();
        proje.K4 = row.find("TD").eq(5).html();
        proje.K5 = row.find("TD").eq(6).html();
        proje.AsamaAdi = row.find("TD").eq(7).html();
        proje.AsamaKatsayiDegeri = row.find("TD").eq(9).html();

        anket.push(proje);
    });

    //Send the JSON array to Controller using AJAX.
    $.ajax({
        type: "POST",
        url: "/Management/InsertQuestions",
        data: JSON.stringify(anket),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Hata oluştu - Karkas: " + errorThrown);
        }
    });
});