var telldus = require('telldus');

console.log('Alla enheter:');
var devices = telldus.getDevicesSync();
devices.forEach(function(item) {
    console.log(item.id + ' ' + item.name + ' ' + item.status.name);
});

telldus.addDeviceEventListener(function(device, status) {
    console.log('DeviceEvent:' + device + ' Status:' + status);
});

telldus.addRawDeviceEventListener(function(id, data) {
    console.log('RawDeviceEvent:' + data);
});

telldus.addSensorEventListener(function(id, protocol, model, type, value, ts) {
    console.log('SensorEvent: ' + protocol + " " + model + " " + value + " " + id + " " + ts);
});

console.log('Tryck en tangent för att avbryta...');

process.stdin.setRawMode(true);
process.stdin.resume();
process.stdin.on('data', process.exit.bind(process, 0));
