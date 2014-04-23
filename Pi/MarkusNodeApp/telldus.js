var telldus = require('telldus');
var net = require('net');
var log = require('./markuslogger').logger;


var client = new net.Socket;
var senasteDeviceAction = Date.now();

telldus.addDeviceEventListener(function (device, status) {
    var now = Date.now();
    if (now - senasteDeviceAction < 500) return;
    senasteDeviceAction = now;
    client.connect(8089, 'linderback.com', function() {
        client.write('larm');
    });

    client.on('data', function(data) {
        log.info('larmanrop ok, tid: ' + (Date.now() - now) + 'ms');
        client.destroy();
    });

    client.on('error', function(error) {
        log.error('fel vid anrop: ' + error);
    });

    client.on('close', function() {
    });
});
