$(function () {
    var mqttHub = $.connection.chatHub;

    mqttHub.client.updateTopicSicaklik1 = function (message) {
        $('#dataContainer').html(message);
        
    };

    // Süreyi güncelleyen işlev
    mqttHub.client.updateTimeElapsed = function (topic, timeElapsed) {
        console.log(timeElapsed + " süre" + topic + " :topic");
        $('#timeElapsed_' + topic).text(timeElapsed);
    };

    //Min sıcaklık verisini güncelleyen işlev
    mqttHub.client.minTemp = function (topic, minTemp) {
        $('#minTemp_' + topic).text(minTemp);
        localStorage.setItem("minTemp_" + topic, minTemp);
    };

    $(document).ready(function () {
        // Local storage'da bulunan bütün "minTemp_" öneki ile başlayan öğeleri al.
        for (var i = 0; i < localStorage.length; i++) {
            var key = localStorage.key(i);
            if (key.startsWith('minTemp_')) {
                var minTemp = localStorage.getItem(key);
                var topic = key.substring(8); // "minTemp_" öncekini kaldır
                $('#minTemp_' + topic).text(minTemp);
            }
        }

    });

    //Timeout Image
    mqttHub.client.Timeout = function (topic) {
        var imgElement = document.getElementById("imgTest_" + topic);
        imgElement.src = "/Content/icons/radio-red.png"
    };

    //Gevşet Image
    mqttHub.client.Gevset = function (topic) {
        var imgElem = document.getElementById("imgGevset_" + topic);
        imgElem.src = "/Content/icons/radio-green.png"
    };

    //Olgunluk Verisi
    mqttHub.client.olg = function (topic, olgunluk) {
        $('#olgunlukData_' + topic).text(olgunluk);
    }



    // Süreç başladığında, süreyi düzenli olarak güncelleyin
    $.connection.hub.start().done(function () {
        console.log('Hub connection started');

        // Süreyi güncellemek için ChatHub yöntemi düzenli olarak çağırılıyor...
        setInterval(function () {
            mqttHub.server.sendCurrentTimeElapsed("Olgunluk1_pub"); // topicler
            mqttHub.server.sendCurrentTimeElapsed("Olgunluk2_pub");
            mqttHub.server.sendCurrentTimeElapsed("Olgunluk3_pub");
            mqttHub.server.sendCurrentTimeElapsed("Olgunluk4_pub");
            mqttHub.server.sendCurrentTimeElapsed("Olgunluk5_pub");
            mqttHub.server.sendCurrentTimeElapsed("Olgunluk6_pub");
            mqttHub.server.sendCurrentTimeElapsed("Olgunluk7_pub");
            mqttHub.server.sendCurrentTimeElapsed("Olgunluk8_pub");
            mqttHub.server.sendCurrentTimeElapsed("Olgunluk9_pub");
            mqttHub.server.sendCurrentTimeElapsed("Olgunluk10_pub");
        }, 1000);
    }).fail(function (reason) {
        console.log('Hub connection failed: ' + reason);
    });

});

//Testi Bitir
function showConfirm(cihazNo) {
    var confirmed = confirm("Testi bitirmek istediğinizden emin misiniz?")
    if (confirmed) {
        document.getElementById('hiddenCihazNo').value = cihazNo;
        resetLocalStorage(cihazNo);
        document.forms[0].submit;
    }
}


//Publish Data
$(function () {
    var mqttHub = $.connection.chatHub;

    $('#publishButton').click(function () {
        mqttHub.server.publishData('Olgunluk1_olgunluk', 'Selams!!');
    });
});


