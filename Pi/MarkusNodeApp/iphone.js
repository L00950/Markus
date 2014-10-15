var config = require('./config.json'),
    exec = require('child_process').exec;

module.exports = {
    iPhoneTimer: function(cache, io) {
        for (item in config.iphones) {
            (function(ipadress) {
                exec('ping -c 1 ' + ipadress, function(error, stdout, stderr) {
                    if (error === null) {
                        cache.aktivtlarm = null;
                        cache.senasthemma.tid = Date.now();
                        cache.larm.state = 0;
                        io.sockets.emit('message', { msg: 'larm', data: { state: cache.larm.state } });
                        io.sockets.emit('message', { msg: "senasthemma", data: cache.senasthemma });
                    }
                });
            })(config.iphones[item].ip);
        }
    }
}



