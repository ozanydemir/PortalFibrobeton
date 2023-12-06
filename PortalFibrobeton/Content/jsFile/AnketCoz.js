//Nav-linkler için Butonlar
$(function () {
    // İlk tab'a geri butonu devre dışı
    $('.btn-prev').prop('disabled', true);

    // İleri butonu tıklanınca aktif tab'ı bir sonraki tab yap
    $('.btn-next').on('click', function () {
        var activePills = $('.nav-pills .nav-link.active');
        var nextPills = activePills.next('a.nav-link');

        if (nextPills.length > 0) {
            activePills.removeClass('active show');
            nextPills.addClass('active show');
            $('.nav-tabs .active').removeClass('active');
            $('.nav-tabs [href="' + nextPills.attr('href') + '"]').addClass('active');
            $('.btn-prev').prop('disabled', false);
            if (nextPills.next('.nav-link').length === 0) {
                $('.btn-next').prop('disabled', true);
            }
        }
    });

    // Geri butonu tıklanınca aktif tab'ı bir önceki tab yap
    $('.btn-prev').on('click', function () {
        var activePills = $('.nav-pills .nav-link.active');
        var prevPills = activePills.prev('a.nav-link');

        if (prevPills.length > 0) {
            activePills.removeClass('active show');
            prevPills.addClass('active show');
            $('.nav-tabs .active').removeClass('active');
            $('.nav-tabs [href="' + prevPills.attr('href') + '"]').addClass('active');
            $('.btn-next').prop('disabled', false);
            if (prevPills.prev('.nav-link').length === 0) {
                $('.btn-prev').prop('disabled', true);
            }
        }
    });
});




//Tab page için butonlar
$(function () {
    // İlk tab'a geri butonu devre dışı
    $('.btn-prev').prop('disabled', true);

    // İleri butonu tıklanınca aktif tab'ı bir sonraki tab yap
    $('.btn-next').on('click', function () {
        var activeTab = $('.tab-content .tab-pane.active');
        var nextTab = activeTab.next('.tab-pane');
        if (nextTab.length > 0) {
            activeTab.removeClass('active show');
            nextTab.addClass('active show');
            $('.nav-tabs .active').removeClass('active');
            $('.nav-tabs [href="#' + nextTab.attr('id') + '"]').addClass('active');
            $('.btn-prev').prop('disabled', false);
            if (nextTab.next('.tab-pane').length === 0) {
                $('.btn-next').prop('disabled', true);
            }
        }

    });

    // Geri butonu tıklanınca aktif tab'ı bir önceki tab yap
    $('.btn-prev').on('click', function () {

        var activeTab = $('.tab-content .tab-pane.active');
        var prevTab = activeTab.prev('.tab-pane');
        if (prevTab.length > 0) {
            activeTab.removeClass('active show');
            prevTab.addClass('active show');
            $('.nav-tabs .active').removeClass('active');
            $('.nav-tabs [href="#' + prevTab.attr('id') + '"]').addClass('active');
            $('.btn-next').prop('disabled', false);
            if (prevTab.prev('.tab-pane').length === 0) {
                $('.btn-prev').prop('disabled', true);
            }
        }
    });
});


//Nav-links için butonlar

$(function () {
    $('.nav-link').on('click', function () {
        // seçili olan nav-link elemanının sınıfını güncelle
        $('.nav-link.active').removeClass('active');
        $(this).addClass('active');
        var target = $(this).attr('href');
        // gösterilecek tab-pane elemanını göster
        $('.tab-pane.active').removeClass('active show');
        $(target).addClass('active show');
        // Geri ve İleri butonlarının durumlarını güncelle
        if ($(target).next('.tab-pane').length === 0) {
            $('.btn-next').prop('disabled', true);
        } else {
            $('.btn-next').prop('disabled', false);
        }
        if ($(target).prev('.tab-pane').length === 0) {
            $('.btn-prev').prop('disabled', true);
        } else {
            $('.btn-prev').prop('disabled', false);
        }
    });
});


//Anketi Kaydet

function checkEmptyAnswers() {
    let emptyAnswersExist = false;
    let empty_cells = [];
    $('#tblKalip tr').each(function () {
        const cevap = $(this).find('td:nth-child(5)');
        if (cevap.text() === '') {
            emptyAnswersExist = true;
            empty_cells.push($(this).find('td:nth-child(2)').text()); // AsamaAdi hücresinin değerini empty_cells listesine ekle
            return false; // döngüyü sonlandır
        }
    });
    if (emptyAnswersExist) {
        alert('Lütfen bütün sorulara cevap verin! Boş cevapların olduğu adımlar: ' + empty_cells.join(', '));
        return false;
    }
    return true;
}

$("body").on("click", "#btnSave", function () {
    if (checkEmptyAnswers()) {
        //Loop through the Table rows and build a JSON array.
        var anket = new Array();
        $("#tblKalip TBODY TR").each(function () {
            var row = $(this);
            var proje = {};
            proje.SoruNo = row.find("TD").eq(0).html();
            proje.AsamaAdi = row.find("TD").eq(1).html();
            proje.AnketID = row.find("TD").eq(2).html();
            proje.Soru = row.find("TD").eq(3).html();
            proje.CevapNo = row.find("TD").eq(4).html();
            proje.AnketiCozen = row.find("TD").eq(5).html();
            proje.CozumTarihi = row.find("TD").eq(6).html();
            proje.ProjeId = row.find("TD").eq(7).html();

            anket.push(proje);
        });

        //Send the JSON array to Controller using AJAX.
        $.ajax({
            type: "POST",
            url: "/Management/InsertAnswers",
            data: JSON.stringify(anket),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                alert("Cevaplar başarıyla kaydedildi.");
                window.location.href = '/Management/AnketSonuclari';
            }
        });
    }

});


//Seçilen Değeri Tabloya Yazdırma

//Switcher function:
$(".tabY").click(function () {
    //Spot switcher:
    $(this).parent().find(".tabY").removeClass("activeTab");
    $(this).addClass("activeTab");

    const anaDiv = this.parentNode;
    const selectedTabYElements = anaDiv.querySelectorAll('.tabY.activeTab');
    selectedTabYElements.forEach(function (element) {
        const dataValue = element.getAttribute("data-value");
        const soruNoAsamaAdi = anaDiv.id.split("-");
        const soruNo = soruNoAsamaAdi[0];
        const asamaAdi = soruNoAsamaAdi[1];


        $("#cevap-" + soruNo + "-" + asamaAdi.replace(/ /g, "-")).text(dataValue);
    });
});


