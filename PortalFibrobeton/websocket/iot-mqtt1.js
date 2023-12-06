// Create a client instance

//CNC Parametreler
var sayi;
var clientID = "ws" + Math.random();
var MN01Rpm = 0;
var MN01A = 0;
var MN01Watt = 0;
var MN02Rpm = 0;
var MN02A = 0;
var MN02Watt = 0;
var CMA03Rpm = 0;
var CMA03A = 0;
var CMA04Rpm = 0;
var CMA04A = 0;
let durumdeneme;
var test;
let mn01 = 0;
let mn02 = 0;
let cma03 = 0;
let cma04 = 0;
let cma05 = 0;
let cma06 = 0;
let cma07 = 0;


var client = new Paho.MQTT.Client("193.100.100.30", 9001, clientID);

// set callback handlers
client.onConnectionLost = onConnectionLost;
client.onMessageArrived = onMessageArrived;

// connect the client
client.connect({ onSuccess: onConnect });


// called when the client connects
function onConnect() {
    // Once a connection has been made, make a subscription and send a message.
    //console.log("Bağlantı başarılı");

    //Cnc Topicler
    client.subscribe("MN01_sure");
    client.subscribe("MN01_durum");
    client.subscribe("MN01_akim")
    client.subscribe("MN01_devir");
    client.subscribe("MN01_tuketim");

    client.subscribe("MN02_sure");
    client.subscribe("MN02_durum");
    client.subscribe("MN02_akim")
    client.subscribe("MN02_devir");
    client.subscribe("MN02_tuketim");

    client.subscribe("CMA03_sure");
    client.subscribe("CMA03_durum");
    client.subscribe("CMA03_akim");
    client.subscribe("CMA03_devir");
    client.subscribe("CMA03_tuketim");

    client.subscribe("CMA04_sure");
    client.subscribe("CMA04_durum");
    client.subscribe("CMA04_akim");
    client.subscribe("CMA04_devir");
    client.subscribe("CMA04_tuketim");

    client.subscribe("CMA05_sure");
    client.subscribe("CMA05_durum");
    client.subscribe("CMA05_akim");
    client.subscribe("CMA05_devir");
    client.subscribe("CMA05_tuketim");

    client.subscribe("CMA06_sure");
    client.subscribe("CMA06_durum");
    client.subscribe("CMA06_akim");
    client.subscribe("CMA06_devir");
    client.subscribe("CMA06_tuketim");

    client.subscribe("CMA07_sure");
    client.subscribe("CMA07_durum");
    client.subscribe("CMA07_akim");
    client.subscribe("CMA07_devir");
    client.subscribe("CMA07_tuketim");

    //CNC Gönderilen Mesajlar
    mess = new Paho.MQTT.Message("1");
    mess.destinationName = "cnc_on";
    client.send(mess);

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


    if (message.destinationName == "MN01_sure") {

        sayi = message.payloadString;


        let milliseconds = sayi;
        let totalSeconds = milliseconds / 1000;

        let hours = Math.floor(totalSeconds / 3600);
        totalSeconds %= 3600;
        let minutes = Math.floor(totalSeconds / 60);
        let seconds = totalSeconds % 60;

        //Ana Sayfa Grafik
        let mn01Chart = document.querySelector('.denemeMN01');
        mn01Chart.innerHTML = sayi;

        let timerRef = document.querySelector('.timerDisplayMN01');
        let int = null;

        document.getElementById('startTimerMN01').addEventListener('click', () => {
            if (int !== null) {
                clearInterval(int);
            }

            int = setInterval(displayTimer, 10);

        });

        document.getElementById('pauseTimerMN01').addEventListener('click', () => {
            clearInterval(int);

            showTimer();
        });


        function displayTimer() {
            milliseconds += 10;
            if (milliseconds >= 1000) {
                milliseconds = 0;
                seconds++;
                if (seconds >= 60) {
                    seconds = 0;
                    minutes++;
                    if (minutes >= 60) {
                        minutes = 0;
                        hours++;
                    }
                }
            }
            let h = hours < 10 ? "0" + hours : hours;
            let m = minutes < 10 ? "0" + minutes : minutes;
            let s = seconds < 10 ? "0" + seconds : seconds;

            timerRef.innerHTML = ` ${h}:${m}:${s}`;
        }

        function showTimer() {

            let h = hours < 10 ? "0" + hours : hours;
            let m = minutes < 10 ? "0" + minutes : minutes;
            let s = seconds < 10 ? "0" + seconds : seconds;

            timerRef.innerHTML = ` ${h}:${m}:${s}`;
        }
    }
    if (message.destinationName == "MN01_durum") {

        var lasercutMN01 = document.getElementById('lasercutMN01');
        var onOffBulbMN01 = document.getElementById('bulbMN01');


        function onPicLas() {
            lasercutMN01.src = "../Content/gifs/laser-cutting.gif"
            onOffBulbMN01.src = "../Content/icons/switch-on.png"
        }

        function offPicLas() {
            lasercutMN01.src = "../Content/gifs/laser-cutting-off.png"
            onOffBulbMN01.src = "../Content/icons/switch-off.png"
        }

        if (message.payloadString == "true") {
            document.getElementById("startTimerMN01").click();
            onPicLas()

        }
        else {
            document.getElementById("pauseTimerMN01").click();
            offPicLas()
        }
    }


    //MN02

    if (message.destinationName == "MN02_sure") {


        sayi = message.payloadString;


        let milliseconds = sayi;
        let totalSeconds = milliseconds / 1000;

        let hours = Math.floor(totalSeconds / 3600);
        totalSeconds %= 3600;
        let minutes = Math.floor(totalSeconds / 60);
        let seconds = totalSeconds % 60;


        //Ana Sayfa Grafik

        let mn02Chart = document.querySelector('.denemeMN02');
        mn02Chart.innerHTML = sayi;

        denemeDegisken = sayi;


        ////mn02 = sayi;
        
        //let mn02Chart = document.querySelector('.denemeMN02');
        //mn02Chart.innerHTML = sayi;

        let timerRefMN02 = document.querySelector('.timerDisplayMN02');
        let int = null;

        document.getElementById('startTimerMN02').addEventListener('click', () => {
            if (int !== null) {
                clearInterval(int);
            }
            int = setInterval(displayTimerMN02, 10);
        });

        document.getElementById('pauseTimerMN02').addEventListener('click', () => {
            clearInterval(int);

            showTimerMN02();
        });


        function displayTimerMN02() {
            milliseconds += 10;
            if (milliseconds >= 1000) {
                milliseconds = 0;
                seconds++;
                if (seconds >= 60) {
                    seconds = 0;
                    minutes++;
                    if (minutes >= 60) {
                        minutes = 0;
                        hours++;
                    }
                }
            }
            let h = hours < 10 ? "0" + hours : hours;
            let m = minutes < 10 ? "0" + minutes : minutes;
            let s = seconds < 10 ? "0" + seconds : seconds;

            timerRefMN02.innerHTML = ` ${h}:${m}:${s}`;
        }

        function showTimerMN02() {

            let h = hours < 10 ? "0" + hours : hours;
            let m = minutes < 10 ? "0" + minutes : minutes;
            let s = seconds < 10 ? "0" + seconds : seconds;

            timerRefMN02.innerHTML = ` ${h}:${m}:${s}`;
        }
    }
    if (message.destinationName == "MN02_durum") {

        var lasercutMN02 = document.getElementById('lasercutMN02');
        var onOffBulbMN02 = document.getElementById('bulbMN02');


        function onPicLas() {
            lasercutMN02.src = "../Content/gifs/laser-cutting.gif"
            onOffBulbMN02.src = "../Content/icons/switch-on.png"
        }

        function offPicLas() {
            lasercutMN02.src = "../Content/gifs/laser-cutting-off.png"
            onOffBulbMN02.src = "../Content/icons/switch-off.png"
        }

        if (message.payloadString == "true") {
            document.getElementById("startTimerMN02").click();
            onPicLas()

        }
        else {
            document.getElementById("pauseTimerMN02").click();
            offPicLas()
        }
    }

    //CMA-03


    if (message.destinationName == "CMA03_sure") {
        sayi = message.payloadString;

        let milliseconds = sayi;
        let totalSeconds = milliseconds / 1000;

        let hours = Math.floor(totalSeconds / 3600);
        totalSeconds %= 3600;
        let minutes = Math.floor(totalSeconds / 60);
        let seconds = totalSeconds % 60;

        //Ana Sayfa Grafik
        cma03 = sayi;

        let timerRefCMA03 = document.querySelector('.timerDisplayCMA03');
        let int = null;

        document.getElementById('startTimerCMA03').addEventListener('click', () => {
            if (int !== null) {
                clearInterval(int);
            }
            int = setInterval(displayTimerCMA03, 10);
        });

        document.getElementById('pauseTimerCMA03').addEventListener('click', () => {
            clearInterval(int);

            showTimerCMA03();
        });


        function displayTimerCMA03() {
            milliseconds += 10;
            if (milliseconds >= 1000) {
                milliseconds = 0;
                seconds++;
                if (seconds >= 60) {
                    seconds = 0;
                    minutes++;
                    if (minutes >= 60) {
                        minutes = 0;
                        hours++;
                    }
                }
            }
            let h = hours < 10 ? "0" + hours : hours;
            let m = minutes < 10 ? "0" + minutes : minutes;
            let s = seconds < 10 ? "0" + seconds : seconds;

            timerRefCMA03.innerHTML = ` ${h}:${m}:${s}`;
        }

        function showTimerCMA03() {

            let h = hours < 10 ? "0" + hours : hours;
            let m = minutes < 10 ? "0" + minutes : minutes;
            let s = seconds < 10 ? "0" + seconds : seconds;

            timerRefCMA03.innerHTML = ` ${h}:${m}:${s}`;
        }
    }
    if (message.destinationName == "CMA03_durum") {

        var lasercutCMA03 = document.getElementById('lasercutCMA03');
        var onOffBulbCMA03 = document.getElementById('bulbCMA03');


        function onPicLas() {
            lasercutCMA03.src = "../Content/gifs/laser-cutting.gif"
            onOffBulbCMA03.src = "../Content/icons/switch-on.png"
        }

        function offPicLas() {
            lasercutCMA03.src = "../Content/gifs/laser-cutting-off.png"
            onOffBulbCMA03.src = "../Content/icons/switch-off.png"
        }

        if (message.payloadString == "true") {
            document.getElementById("startTimerCMA03").click();
            onPicLas()

        }
        else {
            document.getElementById("pauseTimerCMA03").click();
            offPicLas()
        }
    }

    //CMA-04


    if (message.destinationName == "CMA04_sure") {
        sayi = message.payloadString;

        let cma04;
        let milliseconds = sayi;
        let totalSeconds = milliseconds / 1000;

        let hours = Math.floor(totalSeconds / 3600);
        totalSeconds %= 3600;
        let minutes = Math.floor(totalSeconds / 60);
        let seconds = totalSeconds % 60;

        //Ana Sayfa Grafik
        cma04 = sayi;

        let timerRefCMA04 = document.querySelector('.timerDisplayCMA04');
        let int = null;

        document.getElementById('startTimerCMA04').addEventListener('click', () => {
            if (int !== null) {
                clearInterval(int);
            }
            int = setInterval(displayTimerCMA04, 10);
        });

        document.getElementById('pauseTimerCMA04').addEventListener('click', () => {
            clearInterval(int);

            showTimerCMA04();
        });


        function displayTimerCMA04() {
            milliseconds += 10;
            if (milliseconds >= 1000) {
                milliseconds = 0;
                seconds++;
                if (seconds >= 60) {
                    seconds = 0;
                    minutes++;
                    if (minutes >= 60) {
                        minutes = 0;
                        hours++;
                    }
                }
            }
            let h = hours < 10 ? "0" + hours : hours;
            let m = minutes < 10 ? "0" + minutes : minutes;
            let s = seconds < 10 ? "0" + seconds : seconds;

            timerRefCMA04.innerHTML = ` ${h}:${m}:${s}`;
        }

        function showTimerCMA04() {

            let h = hours < 10 ? "0" + hours : hours;
            let m = minutes < 10 ? "0" + minutes : minutes;
            let s = seconds < 10 ? "0" + seconds : seconds;

            timerRefCMA04.innerHTML = ` ${h}:${m}:${s}`;
        }
    }
    if (message.destinationName == "CMA04_durum") {

        var lasercutCMA04 = document.getElementById('lasercutCMA04');
        var onOffBulbCMA04 = document.getElementById('bulbCMA04');


        function onPicLas() {
            lasercutCMA04.src = "../Content/gifs/laser-cutting.gif"
            onOffBulbCMA04.src = "../Content/icons/switch-on.png"
        }

        function offPicLas() {
            lasercutCMA04.src = "../Content/gifs/laser-cutting-off.png"
            onOffBulbCMA04.src = "../Content/icons/switch-off.png"
        }

        if (message.payloadString == "true") {
            document.getElementById("startTimerCMA04").click();
            onPicLas()

        }
        else {
            document.getElementById("pauseTimerCMA04").click();
            offPicLas()
        }
    }


    //CMA-05


    if (message.destinationName == "CMA05_sure") {
        sayi = message.payloadString;

        let milliseconds = sayi;
        let totalSeconds = milliseconds / 1000;

        let hours = Math.floor(totalSeconds / 3600);
        totalSeconds %= 3600;
        let minutes = Math.floor(totalSeconds / 60);
        let seconds = totalSeconds % 60;

        let timerRefCMA05 = document.querySelector('.timerDisplayCMA05');
        let int = null;

        //Ana Sayfa Grafik
        cma05 = sayi;

        document.getElementById('startTimerCMA05').addEventListener('click', () => {
            if (int !== null) {
                clearInterval(int);
            }
            int = setInterval(displayTimerCMA05, 10);
        });

        document.getElementById('pauseTimerCMA05').addEventListener('click', () => {
            clearInterval(int);

            showTimerCMA05();
        });


        function displayTimerCMA05() {
            milliseconds += 10;
            if (milliseconds >= 1000) {
                milliseconds = 0;
                seconds++;
                if (seconds >= 60) {
                    seconds = 0;
                    minutes++;
                    if (minutes >= 60) {
                        minutes = 0;
                        hours++;
                    }
                }
            }
            let h = hours < 10 ? "0" + hours : hours;
            let m = minutes < 10 ? "0" + minutes : minutes;
            let s = seconds < 10 ? "0" + seconds : seconds;

            timerRefCMA05.innerHTML = ` ${h}:${m}:${s}`;
        }

        function showTimerCMA05() {

            let h = hours < 10 ? "0" + hours : hours;
            let m = minutes < 10 ? "0" + minutes : minutes;
            let s = seconds < 10 ? "0" + seconds : seconds;

            timerRefCMA05.innerHTML = ` ${h}:${m}:${s}`;
        }
    }
    if (message.destinationName == "CMA05_durum") {

        var lasercut = document.getElementById('lasercutCMA05');
        var onOffBulb = document.getElementById('bulbCMA05');


        function onPicLas() {
            lasercut.src = "../Content/gifs/laser-cutting.gif"
            onOffBulb.src = "../Content/icons/switch-on.png"
        }

        function offPicLas() {
            lasercut.src = "../Content/gifs/laser-cutting-off.png"
            onOffBulb.src = "../Content/icons/switch-off.png"
        }

        if (message.payloadString == "true") {
            document.getElementById("startTimerCMA05").click();
            onPicLas()

        }
        else {
            document.getElementById("pauseTimerCMA05").click();
            offPicLas()
        }
    }

    //CMA-06


    if (message.destinationName == "CMA06_sure") {
        sayi = message.payloadString;

        let milliseconds = sayi;
        let totalSeconds = milliseconds / 1000;

        let hours = Math.floor(totalSeconds / 3600);
        totalSeconds %= 3600;
        let minutes = Math.floor(totalSeconds / 60);
        let seconds = totalSeconds % 60;

        //Ana Sayfa Grafik
        cma06 = sayi;

        let timerRefCMA06 = document.querySelector('.timerDisplayCMA06');
        let int = null;

        document.getElementById('startTimerCMA06').addEventListener('click', () => {
            if (int !== null) {
                clearInterval(int);
            }
            int = setInterval(displayTimerCMA06, 10);
        });

        document.getElementById('pauseTimerCMA06').addEventListener('click', () => {
            clearInterval(int);

            showTimerCMA06();
        });


        function displayTimerCMA06() {
            milliseconds += 10;
            if (milliseconds >= 1000) {
                milliseconds = 0;
                seconds++;
                if (seconds >= 60) {
                    seconds = 0;
                    minutes++;
                    if (minutes >= 60) {
                        minutes = 0;
                        hours++;
                    }
                }
            }
            let h = hours < 10 ? "0" + hours : hours;
            let m = minutes < 10 ? "0" + minutes : minutes;
            let s = seconds < 10 ? "0" + seconds : seconds;

            timerRefCMA06.innerHTML = ` ${h}:${m}:${s}`;
        }

        function showTimerCMA06() {

            let h = hours < 10 ? "0" + hours : hours;
            let m = minutes < 10 ? "0" + minutes : minutes;
            let s = seconds < 10 ? "0" + seconds : seconds;

            timerRefCMA06.innerHTML = ` ${h}:${m}:${s}`;
        }
    }
    if (message.destinationName == "CMA06_durum") {

        var lasercutCMA06 = document.getElementById('lasercutCMA06');
        var onOffBulbCMA06 = document.getElementById('bulbCMA06');


        function onPicLas() {
            lasercutCMA06.src = "../Content/gifs/laser-cutting.gif"
            onOffBulbCMA06.src = "../Content/icons/switch-on.png"
        }

        function offPicLas() {
            lasercutCMA06.src = "../Content/gifs/laser-cutting-off.png"
            onOffBulbCMA06.src = "../Content/icons/switch-off.png"
        }

        if (message.payloadString == "true") {
            document.getElementById("startTimerCMA06").click();
            onPicLas()

        }
        else {
            document.getElementById("pauseTimerCMA06").click();
            offPicLas()
        }
    }


    //CMA-07

    if (message.destinationName == "CMA07_sure") {


        sayi = message.payloadString;
        //console.log(message.payloadString);

        let milliseconds = sayi;
        let totalSeconds = milliseconds / 1000;

        let hours = Math.floor(totalSeconds / 3600);
        totalSeconds %= 3600;
        let minutes = Math.floor(totalSeconds / 60);
        let seconds = totalSeconds % 60;

        let timerRefCMA07 = document.querySelector('.timerDisplayCMA07');
        let int = null;

        //Ana Sayfa Grafik
        cma07 = sayi;

        document.getElementById('startTimerCMA07').addEventListener('click', () => {
            if (int !== null) {
                clearInterval(int);
            }
            int = setInterval(displayTimerCMA07, 10);
        });

        document.getElementById('pauseTimerCMA07').addEventListener('click', () => {
            clearInterval(int);

            showTimerCMA07();
        });


        function displayTimerCMA07() {
            milliseconds += 10;
            if (milliseconds >= 1000) {
                milliseconds = 0;
                seconds++;
                if (seconds >= 60) {
                    seconds = 0;
                    minutes++;
                    if (minutes >= 60) {
                        minutes = 0;
                        hours++;
                    }
                }
            }
            let h = hours < 10 ? "0" + hours : hours;
            let m = minutes < 10 ? "0" + minutes : minutes;
            let s = seconds < 10 ? "0" + seconds : seconds;

            timerRefCMA07.innerHTML = ` ${h}:${m}:${s}`;
        }

        function showTimerCMA07() {

            let h = hours < 10 ? "0" + hours : hours;
            let m = minutes < 10 ? "0" + minutes : minutes;
            let s = seconds < 10 ? "0" + seconds : seconds;

            timerRefCMA07.innerHTML = ` ${h}:${m}:${s}`;
        }
    }

    if (message.destinationName == "CMA07_durum") {


        var lasercutCMA07 = document.getElementById('lasercutCMA07');
        var onOffBulbCMA07 = document.getElementById('bulbCMA07');


        function onPicLas() {
            lasercutCMA07.src = "../Content/gifs/laser-cutting.gif"
            onOffBulbCMA07.src = "../Content/icons/switch-on.png"
        }

        function offPicLas() {
            lasercutCMA07.src = "../Content/gifs/laser-cutting-off.png"
            onOffBulbCMA07.src = "../Content/icons/switch-off.png"
        }

        if (message.payloadString == "true") {
            document.getElementById("startTimerCMA07").click();
            onPicLas()

        }
        else {
            document.getElementById("pauseTimerCMA07").click();
            offPicLas()

        }
    }

    //MN01 Veriler WebSocket

    if (message.destinationName == "MN01_devir") {

        document.getElementById("MN01Rpm").textContent = message.payloadString + ' rpm';
    }


    if (message.destinationName == "MN01_akim") {



        if (message.payloadString == null) {

            document.getElementById("MN01Watt").textContent = 0;
        }

        else {

            MN01A = message.payloadString;
            MN01Watt = ((MN01A * 400) * 0.8) * Math.sqrt(3);
            MN01Watt = Math.round(MN01Watt, 0);
            document.getElementById("MN01Watt").textContent = MN01Watt + ' W';
        }

    }

    //MN02 Veriler WebSocket

    if (message.destinationName == "MN02_devir") {

        document.getElementById("MN02Rpm").textContent = message.payloadString + ' rpm';
    }


    if (message.destinationName == "MN02_akim") {



        if (message.payloadString == null) {

            document.getElementById("MN02Watt").textContent = 0;
        }

        else {

            MN02A = message.payloadString;
            MN02Watt = ((MN02A * 400) * 0.8) * Math.sqrt(3);
            MN02Watt = Math.round(MN02Watt, 0);
            document.getElementById("MN02Watt").textContent = MN02Watt + ' W';
        }

    }


    //CMA03 Veriler WebSocket

    if (message.destinationName == "CMA03_devir") {

        document.getElementById("CMA03Rpm").textContent = message.payloadString + ' rpm';
    }


    if (message.destinationName == "CMA03_akim") {


        if (message.payloadString == null) {

            document.getElementById("CMA03Watt").textContent = 0;
        }

        else {

            CMA03A = message.payloadString;
            CMA03Watt = ((CMA03A * 400) * 0.8) * Math.sqrt(3);
            CMA03Watt = Math.round(CMA03Watt, 0);
            document.getElementById("CMA03Watt").textContent = CMA03Watt + ' W';
        }

    }

    //CMA04 Veriler WebSocket

    if (message.destinationName == "CMA04_devir") {

        document.getElementById("CMA04Rpm").textContent = message.payloadString + ' rpm';
    }


    if (message.destinationName == "CMA04_akim") {


        if (message.payloadString == null) {

            document.getElementById("CMA04Watt").textContent = 0;
        }

        else {

            CMA04A = message.payloadString;
            CMA04Watt = ((CMA04A * 400) * 0.8) * Math.sqrt(3);
            CMA04Watt = Math.round(CMA04Watt, 0);
            document.getElementById("CMA04Watt").textContent = CMA04Watt + ' W';
        }

    }


    //CMA04 Veriler WebSocket

    if (message.destinationName == "CMA05_devir") {

        document.getElementById("CMA05Rpm").textContent = message.payloadString + ' rpm';
    }


    if (message.destinationName == "CMA05_akim") {


        if (message.payloadString == null) {

            document.getElementById("CMA05Watt").textContent = 0;
        }

        else {

            CMA05A = message.payloadString;
            CMA05Watt = ((CMA05A * 400) * 0.8) * Math.sqrt(3);
            CMA05Watt = Math.round(CMA05Watt, 0);
            document.getElementById("CMA05Watt").textContent = CMA05Watt + ' W';
        }

    }

    //CMA06 Veriler WebSocket

    if (message.destinationName == "CMA06_devir") {

        document.getElementById("CMA06Rpm").textContent = message.payloadString + ' rpm';
    }


    if (message.destinationName == "CMA06_akim") {


        if (message.payloadString == null) {

            document.getElementById("CMA06Watt").textContent = 0;
        }

        else {

            CMA06A = message.payloadString;
            CMA06Watt = ((CMA06A * 400) * 0.8) * Math.sqrt(3);
            CMA06Watt = Math.round(CMA06Watt, 0);
            document.getElementById("CMA06Watt").textContent = CMA06Watt + ' W';
        }

    }


    //CMA07 Veriler WebSocket

    if (message.destinationName == "CMA07_devir") {

        document.getElementById("CMA07Rpm").textContent = message.payloadString + ' rpm';

    }


    if (message.destinationName == "CMA07_akim") {

        if (message.payloadString == null) {

            document.getElementById("CMA07Watt").textContent = 0;
        }

        else {

            CMA07A = message.payloadString;
            CMA07Watt = ((CMA07A * 400) * 0.8) * Math.sqrt(3);
            CMA07Watt = Math.round(CMA07Watt, 0);
            document.getElementById("CMA07Watt").textContent = CMA07Watt + ' W';
        }

    }





    //Olgunluk

    if (message.destinationName == "Test_gonderilen") {

        console.log(message.payloadString);
    }

}


