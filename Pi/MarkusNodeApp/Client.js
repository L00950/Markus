var net = require('net');
var exec = require('child_process').exec;
var os = require('os');

var client = new net.Socket;
var start = Date.now();
client.connect(8089, 'linderback.com', function() {
    client.write('enablevpn:'+ os.hostname());
});

client.on('data', function(data) {
    console.log('enablevpn:(' + data + ')');
    console.log('tid: ' + (Date.now() - start) + 'ms');
    client.destroy();
    if (data == "1")
        exec('ping -c 1 192.168.11.192', function(error, stdout, stderr) {
            if (error !== null)
                exec('sudo pon lidingo', function(errorPon, stdoutPon, stderrPon) {
                    if (errorPon !== null)
                        console.log('error pon ' + errorPon);
                });
            else
                console.log("Available");
        });
    else if (data == "0")
        exec('sudo poff lidingo', function(error, stdout, stderr) {
        });
    else {
        console.log('enablevpn returnerade felaktig status: (' + data + ')');
    }
});

client.on('close', function() {
});
