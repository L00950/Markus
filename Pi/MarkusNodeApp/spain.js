var telldus = require('telldus');
var net = require('net');

var client = new net.Socket;
var senasteDeviceAction = Date.now();


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
        client.connect(8089, 'linderback.com', function() {
            console.log(dateToString(Date.now()) + ' Larmar till min server');
            client.write('larm:spain');
        });

        client.on('data', function(data) {
        });

        client.on('close', function() {
        });
    }
});

require('net').createServer(function (socket) {
    socket.on('data', function (data) {
    });
}).listen(8089);
