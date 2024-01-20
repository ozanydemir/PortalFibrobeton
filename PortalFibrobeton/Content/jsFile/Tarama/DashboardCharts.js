document.addEventListener("DOMContentLoaded", function () {

    var allDays = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];

    // Her gün için kayıt olup olmadığını kontrol edin ve eksik günler için kayıtlar ekleyin
    allDays.forEach(function (day) {
        if (!groupedData.some(item => translateDay(item.Tarih) === translateDay(day))) {
            groupedData.push({
                Tarih: day,
                MetraScan: 0,
                Black: 0
            });
        }
    });

    // Bugünün gün numarasını al
    var todayDayNumber = new Date().getDay();
    // Günleri sayısal değerlere çevir
    function getDayNumber(day) {
        var days = { "Sunday": 0, "Monday": 1, "Tuesday": 2, "Wednesday": 3, "Thursday": 4, "Friday": 5, "Saturday": 6 };
        return days[day];
    }
    // groupedData içindeki her bir elemana dayNumber ekleyin ve sıralayın
    groupedData.forEach(function (item) {
        item.dayNumber = getDayNumber(item.Tarih);
    });
    groupedData.sort(function (a, b) {
        // Bugünün gününe göre sıralayarak bugünü sona alın
        return (7 + a.dayNumber - todayDayNumber) % 7 - (7 + b.dayNumber - todayDayNumber) % 7;
    });

    var today = new Date().getDay();
    var todayDataIndex = groupedData.findIndex(item => item.dayNumber === today);
    var todayData = groupedData.splice(todayDataIndex, 1)[0];
    groupedData.push(todayData);

    // İngilizce gün adlarını Türkçe'ye çeviren bir fonksiyon
    function translateDay(day) {
        var days = { "Monday": "Pzt", "Tuesday": "Salı", "Wednesday": "Çrş", "Thursday": "Perş", "Friday": "Cuma", "Saturday": "Cmt", "Sunday": "Pazar" };
        return days[day] || day;
    }

    Morris.Bar({
        element: 'morris-dashboard-cihaz',
        data: groupedData.map(function (item) {
            return {
                y: translateDay(item.Tarih),
                MetraScan: item.MetraScan,
                Black: item.Black
            };
        }),
        xkey: 'y',
        ykeys: ['MetraScan', 'Black'],
        labels: ['MetraSCAN', 'Black Elite'],
        barColors: ['#FC6C8E', '#7571f9'],
        hideHover: 'auto',
        gridLineColor: 'transparent',
        resize: true
    });
})