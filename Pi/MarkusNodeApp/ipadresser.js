var exec = require('child_process').exec,
    mail = require('./markusmail.js'),
    config = require('./ipadresser.json');
var log = require('./markuslogger').logger;

var meddelande = "Status nätverket:<br/><br/>";

for (var index in config.ipadresser) {
    adress = config.ipadresser[index];
    pingaAdress(adress);
};


var timer = setTimeout(function () {
    var str = "Status nätverket:<br/><br/>";
    var finnsAdressSomArOffline = false;
    for (var index in config.ipadresser) {
        if (config.ipadresser[index].ok === false) {
            finnsAdressSomArOffline = true;
            str += (config.ipadresser[index].namn + "(" + config.ipadresser[index].ip + ") svarar <b>inte</b><br/>");
        }
    }
    if (finnsAdressSomArOffline == true) {
        mail.sendmail('markus@linderback.com', 'markus@linderback.com', 'Status nätverk', str);
        log.info('Alla servrar svarar inte på ping, mail skickat');
    } else {
        log.info('Alla servrar svarar på ping');
    }
}, 10000);


function pingaAdress(adress) {
    console.log('Kontrollerar ' + adress.namn);
    exec('ping -c 1 ' + adress.ip, function (error, stdout, stderr) {
        if (error === null) {
            adress.ok = true;
            console.log(adress.namn + ' svarar');
            meddelande += (adress.namn + "(" + adress.ip + ") svarar<br/>");
        } else {
            adress.ok = false;
            console.log(adress.namn + ' svarar inte');
            meddelande += (adress.namn + "(" + adress.ip + ") svarar <b>inte</b><br/>");
        }
    });
}
