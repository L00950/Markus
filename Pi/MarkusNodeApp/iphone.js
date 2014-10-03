var config = require('./config.json');

module.exports = {
    iPhoneTimer: function(cache, io) {
        console.log('Pingar iPhones...');
        for (item in config.iphones) {
            exec('ping -c 1 ' + config.iphones[item].ip, function(error, stdout, stderr) {
                if (error === null) {
                    console.log(config.iphones[item].ip + ' svarar');
                    cache.aktivtlarm = null;
                    cache.senasthemma.tid = Date.now();
                    cache.larm.state = 0;
                    io.sockets.emit('message', { msg: 'larm', data: { state: cache.larm.state } });
                    io.sockets.emit('message', { msg: "senasthemma", data: cache.senasthemma });
                } else {
                    console.log(config.iphones[item].ip + ' svarar inte');
                }
            });
        }
    }
}



