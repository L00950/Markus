var net = require('net');

var client = new net.Socket();
client.connect(8089, 'linderback.com', function() {
    console.log('CONNECTED...');
    client.write('enablevpn');
});

client.on('data', function(data) {
    console.log('DATA: ' + data);
    client.destroy();
});

client.on('close', function() {
    console.log('Connection closed');
});
