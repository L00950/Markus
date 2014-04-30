var telldus = require('telldus');


telldus.addDeviceEventListener(function(device, status) {
    console.log('DeviceEvent:' + device + ' Status:' + status);
});

telldus.addRawDeviceEventListener(function(id, data) {
    console.log('RawDeviceEvent:' + data);
});

telldus.addSensorEventListener(function(id, protocol, model, type, value, ts) {
    console.log('SensorEvent: ' + protocol + " " + model + " " + value + " " + id + " " + ts);
});

console.log('Lyssnar...');
