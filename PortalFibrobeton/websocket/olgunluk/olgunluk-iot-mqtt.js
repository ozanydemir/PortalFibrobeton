// Create a client instance

var clientID = "ws" + Math.random();

var client = new Paho.MQTT.Client("193.100.100.30", 9001, clientID);

let selectedValueCihaz;
let selectedValueHammadde;


// set callback handlers
client.onConnectionLost = onConnectionLost;
client.onMessageArrived = onMessageArrived;

// connect the client
client.connect({ onSuccess: onConnect });


// called when the client connects
function onConnect() {
    // Once a connection has been made, make a subscription and send a message.
    console.log("Bağlantı başarılı");

    //Olgunluk Topicler

    client.subscribe("Test_gonderilen");

    //mes2 = new Paho.MQTT.Message("deneme");
    //mes2.destinationName = "Test_alinan";
    //client.send(mes2);
}

// called when the client loses its connection
function onConnectionLost(responseObject) {
    if (responseObject.errorCode !== 0) {
        console.log("onConnectionLost:" + responseObject.errorMessage);

    }
}

// called when a message arrives
function onMessageArrived(message) {

    //Olgunluk

    if (message.destinationName == "Test_gonderilen") {

        var temp = JSON.parse(message.payloadString);

        var x = new Date().getTime(),
            y = parseFloat(temp);

        //dataArr dizisine yeni verileri ekleyin
        dataArr.push({ x: x, y: y });
        if (dataArr.length > 10) {
            dataArr.shift(); //diziye 10'dan fazla veri eklenirse, en eski veriyi çıkarın
        }

        chart.updateSeries([{
            data: dataArr // güncellenen veri dizisini grafikle güncelleyin
        }]);

        console.log(message.payloadString);
    }

}


