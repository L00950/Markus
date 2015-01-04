var telldus = require('telldus');
var net = require('net');

var senasteDeviceAction = Date.now();
var senasteTemp = 0;
var senasteHumidity = 0;


function dateToString(datum) {
    var date = new Date(datum);
    return date.getFullYear() + '-' + fill(2, (date.getMonth() + 1)) + '-' + fill(2, date.getDate()) + ' ' + fill(2, date.getHours()) + ':' + fill(2, date.getMinutes()) + ':' + fill(2, date.getSeconds()) + '.' + fill(3, date.getMilliseconds());
}

function fill(antal, text) {
    while (text.toString().length < antal)
        text = '0' + text;
    return text;
}

telldus.addDeviceEventListener(function (device, status) {
    var now = Date.now();
    if (now - senasteDeviceAction < 1000) return;
    senasteDeviceAction = now;

    console.log(dateToString(Date.now()) + ' event id:' + device + ' state:' + status.name);
    if (device == 1 && status.name === 'ON') {
        var client = new net.Socket;
        client.connect(8089, 'linderback.com', function() {
            console.log(dateToString(Date.now()) + ' Larmar till min server');
            client.write('larm:spain');
            client.end();
        });

        client.on('data', function(data) {
        });

        client.on('close', function() {
        });
    }
});

telldus.addRawDeviceEventListener(function(controllerId, data) {
    var id = 0;
    var temp = 0;
    var humidity = 0;
    var pairs = data.split(";");
    for (var index in pairs) {
        var pair = pairs[index];
        if (pair.split(":")[0] == "id") {
            id = pair.split(":")[1];
        }
        if (pair.split(":")[0] == "temp") {
            temp = pair.split(":")[1];
        }
        if (pair.split(":")[0] == "humidity") {
            humidity = pair.split(":")[1];
        }
    }

    if (Number(id) == 170 && (temp != senasteTemp || humidity != senasteHumidity)) {
        senasteTemp = temp;
        senasteHumidity = humidity;
        var client = new net.Socket;
        client.connect(8089, 'linderback.com', function () {
            client.write('place:ryda;temp:' + temp + ';humidity:' + humidity + ';');
            client.end();
        });
        
        client.on('data', function (returnData) {
            client.destroy();
        });
        
        client.on('close', function () {
        });
     }
});


    require('net').createServer(function (socket) {
    socket.on('data', function (data) {
    });
}).listen(8089);

process.on('uncaughtException', function (err) {
    console.log(dateToString(Date.now()) + ' Ohanterat exception: ' + err);
});
