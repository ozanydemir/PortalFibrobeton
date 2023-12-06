
$("body").on("click", "#btnSave", function () {
    var rapor = new Array();
    var COURT = {};
    COURT.PlanlananParcaSiparis = $("#OrderPieceCOURTSiparis").val();
    COURT.PlanlananM2Siparis = $("#OrderM2COURTSiparis").val();
    COURT.PlanlananParcaDokum = $("#CastingPieceCOURTDokum").val();
    COURT.PlanlananM2Dokum = $("#CastingM2COURTDokum").val();
    COURT.PlanlananParcaSevkiyat = $("#ShippedPieceCOURTSevkiyat").val();
    COURT.PlanlananM2Sevkiyat = $("#ShippedM2COURTSevkiyat").val();
    COURT.ProjeID = "112113";
    COURT.Cephe = "COB-ROOF";

    rapor.push(COURT);


    var NORTH = {};
    NORTH.PlanlananParcaSiparis = $("#OrderPieceNORTHSiparis").val();
    NORTH.PlanlananM2Siparis = $("#OrderM2NORTHSiparis").val();
    NORTH.PlanlananParcaDokum = $("#CastingPieceNORTHDokum").val();
    NORTH.PlanlananM2Dokum = $("#CastingM2NORTHDokum").val();
    NORTH.PlanlananParcaSevkiyat = $("#ShippedPieceNORTHSevkiyat").val();
    NORTH.PlanlananM2Sevkiyat = $("#ShippedM2NORTHSevkiyat").val();
    NORTH.ProjeID = "112113";
    NORTH.Cephe = "COB-NORTH";


    rapor.push(NORTH);

    var EAST = {};
    EAST.PlanlananParcaSiparis = $("#OrderPieceEASTSiparis").val();
    EAST.PlanlananM2Siparis = $("#OrderM2EASTSiparis").val();
    EAST.PlanlananParcaDokum = $("#CastingPieceEASTDokum").val();
    EAST.PlanlananM2Dokum = $("#CastingM2EASTDokum").val();
    EAST.PlanlananParcaSevkiyat = $("#ShippedPieceEASTSevkiyat").val();
    EAST.PlanlananM2Sevkiyat = $("#ShippedM2EASTSevkiyat").val();
    EAST.ProjeID = "112113";
    EAST.Cephe = "COB-EAST";

    rapor.push(EAST);

    var WEST = {};
    WEST.PlanlananParcaSiparis = $("#OrderPieceWESTSiparis").val();
    WEST.PlanlananM2Siparis = $("#OrderM2WESTSiparis").val();
    WEST.PlanlananParcaDokum = $("#CastingPieceWESTDokum").val();
    WEST.PlanlananM2Dokum = $("#CastingM2WESTDokum").val();
    WEST.PlanlananParcaSevkiyat = $("#ShippedPieceWESTSevkiyat").val();
    WEST.PlanlananM2Sevkiyat = $("#ShippedM2WESTSevkiyat").val();
    WEST.ProjeID = "112113";
    WEST.Cephe = "COB-WEST";

    rapor.push(WEST);

    var SOUTH = {};
    SOUTH.PlanlananParcaSiparis = $("#OrderPieceSOUTHSiparis").val();
    SOUTH.PlanlananM2Siparis = $("#OrderM2SOUTHSiparis").val();
    SOUTH.PlanlananParcaDokum = $("#CastingPieceSOUTHDokum").val();
    SOUTH.PlanlananM2Dokum = $("#CastingM2SOUTHDokum").val();
    SOUTH.PlanlananParcaSevkiyat = $("#ShippedPieceSOUTHSevkiyat").val();
    SOUTH.PlanlananM2Sevkiyat = $("#ShippedM2SOUTHSevkiyat").val();
    SOUTH.ProjeID = "112113";
    SOUTH.Cephe = "COB-SOUTH";

    rapor.push(SOUTH);


    $.ajax({
        url: "/UretimSevkiyatRapor/InsertHedefler",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(rapor),
        success: function (result) {
            alert("Kaydedildi");
        },
        error: function () {
            alert("Kaydetme sırasında hata oluştu.");
        }
    });
})