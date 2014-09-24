require('net').createServer(function(socket) {
    console.log('connected');

    socket.on('data', function(data) {
        console.log('Mottaget:' + data);
        if (data.toString().indexOf('enablevpn') > -1) {
            socket.write('1');
        }
    });
}).listen(8089);