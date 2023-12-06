$(function ($) {
    $.fn.datepicker.dates['tr'] =
    {
        days: ["Pazar", "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi", "Pazar"],
        daysShort: ["Pz", "Pzt", "Sal", "Çrş", "Prş", "Cu", "Cts", "Pz"],
        daysMin: ["Pz", "Pzt", "Sa", "Çr", "Pr", "Cu", "Ct", "Pz"],
        months: ["Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"],
        monthsShort: ["Oca", "Şub", "Mar", "Nis", "May", "Haz", "Tem", "Ağu", "Eyl", "Eki", "Kas", "Ara"],
        today: "Bugün",
    };
});

$(function ($) {
    $("#dp1").datepicker(
        {
            language: "tr",
            format: "dd/mm/yyyy",
            autoclose: true
        });
});

$(function ($) {
    $.fn.datepicker.dates['tr'] =
    {
        days: ["Pazar", "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi", "Pazar"],
        daysShort: ["Pz", "Pzt", "Sal", "Çrş", "Prş", "Cu", "Cts", "Pz"],
        daysMin: ["Pz", "Pzt", "Sa", "Çr", "Pr", "Cu", "Ct", "Pz"],
        months: ["Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"],
        monthsShort: ["Oca", "Şub", "Mar", "Nis", "May", "Haz", "Tem", "Ağu", "Eyl", "Eki", "Kas", "Ara"],
        today: "Bugün",
    };
});

$(function ($) {
    $("#dp2").datepicker(
        {
            language: "tr",
            format: "dd/mm/yyyy",
            autoclose: true
        });
});
