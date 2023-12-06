//$("body").on("click", "#btnSablonAddProje", function () {
//    // Yeni bir satır ekleyin
//    var row = $("<tr>");

//    // Satırın hücrelerini oluşturun ve içeriklerini ayarlayın
//    var soruNo = $("<td>").text($("#txtNoProje").val());
//    var soru = $("<td>").text($("#txtSoruProje").val());
//    var k1 = $("<td>").text($("#txtK1Proje").val());
//    var k2 = $("<td>").text($("#txtK2Proje").val());
//    var k3 = $("<td>").text($("#txtK3Proje").val());
//    var k4 = $("<td>").text($("#txtK4Proje").val());
//    var k5 = $("<td>").text($("#txtK5Proje").val());
//    var asama = $("<td>").text($("#txtAsamaProje").val());
//    var anketAdi = $("<td>").text($("#anketAdi").val());
//    var projeGenelKatsayi = $("<td>").text($("#projeGenel").val());


//    // Hücreleri satıra ekleyin
//    row.append(soruNo);
//    row.append(soru);
//    row.append(k1);
//    row.append(k2);
//    row.append(k3);
//    row.append(k4);
//    row.append(k5);
//    row.append(asama);
//    row.append(anketAdi);
//    row.append(projeGenelKatsayi);

//    $()
//    // Satırı tabloya ekleyin
//    $("#tblProje").append(row);


//})

$("body").on("click", "#btnSablonAddProje", function () {
    //Reference the Name and Country TextBoxes.
    var txtNo = $("#txtNoProje");
    var txtSoru = $("#txtSoruProje");
    var txtK1 = $("#txtK1Proje");
    var txtK2 = $("#txtK2Proje");
    var txtK3 = $("#txtK3Proje");
    var txtK4 = $("#txtK4Proje");
    var txtK5 = $("#txtK5Proje");
    var txtAsama = $("#txtAsamaProje");
    var txtAnketAdi = $("#anketAdi");
    var projeGenelValue = $("#projeGenel").val();

    var rowCount = $('#tblProje').find('tr').length;



    //Get the reference of the Table's TBODY element.
    var tBody = $("#tblProje > TBODY")[0];

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

    //Proje Genel Katsayi Cell
    cell = $(row.insertCell(-1));
    cell.html(projeGenelValue);

    //Add Button cell.
    cell = $(row.insertCell(-1));
    var btnRemove = $("<input />");
    btnRemove.attr("type", "button");
    btnRemove.attr("onclick", "RemoveProje(this);");
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
    txtAsama.val("Proje Aşaması");


});