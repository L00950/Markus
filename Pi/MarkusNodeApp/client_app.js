var io = require('socket.io-client');


console.log('Startar...');
var socket = io.connect('http://192.168.1.89:8081');

socket.on('connect', function() {
    console.log('connected to server...');
    socket.emit('message', { msg: 'larm', data: {} });
});

// Handle socket.io errors
socket.on('error', function (err) {
    console.log('Socket.io connection error: ' + err.errno);
});