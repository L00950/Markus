require('net').createServer(function(socket) {
    console.log('connected');

    socket.on('data', function (data) {
        var meddelande = data.toString();
        console.log('Mottaget:' + data);
        if (meddelande.indexOf('enablevpn') > -1) {
            if (meddelande.indexOf('RYDA') > -1) {
                socket.write('0');
            } else {
                socket.write('1');
            }
        }
        else if (meddelande.indexOf('larm') > -1) {
            console.log('larm GC');
        }
    });
}).listen(8089);
