var net = require('net');
var log = require('./markuslogger').logger;

var timer = setInterval(function() {
    var start = Date.now();
    var client = new net.Socket();
    client.connect(8089, 'linderback.com', function() {
        client.write('test');
    });

    client.on('data', function(data) {
        log.info('anrop ok, tid: ' + (Date.now() - start) + 'ms');
        client.destroy();
    });

    client.on('error', function(error) {
        log.error('fel vid anrop: ' + error);
    });

    client.on('close', function() {
        });
    }, 1000*60*10);
