var net = require('net');
var exec = require('child_process').exec;

var client = new net.Socket();
client.connect(8089, 'linderback.com', function() {
    client.write('enablevpn');
});

client.on('data', function(data) {
    console.log('enablevpn: ' + data);
    client.destroy();
    if (data === '1')
        exec('pon lidingo', function(error, stdout, stderr) {
        });
    else if (data === '0')
        exec('poff lidingo', function(error, stdout, stderr) {
        });
});

client.on('close', function() {
    console.log('Connection closed');
});
