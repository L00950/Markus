var net = require('net');
var exec = require('child_process').exec;

var client = new net.Socket();
var start = Date.now();
client.connect(8089, 'linderback.com', function() {
    client.write('enablevpn');
});

client.on('data', function(data) {
    console.log('enablevpn: ' + data);
    console.log('tid: ' + (Date.now() - start) + 'ms');
    client.destroy();
    if (data === '1')
        exec('pon lidingo', function(error, stdout, stderr) {
        });
    else if (data === '0')
        exec('poff lidingo', function(error, stdout, stderr) {
        });
});

client.on('close', function() {
});
