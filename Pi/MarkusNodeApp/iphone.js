//var mail = require('./markusmail');

//mail.sendmail('markus@linderback.com', 'markus@linderback.com', 'Testmail', 'Innehåll...');

//var log = require('./markuslogger').logger;
var exec = require('child_process').exec;
var senasthemma = Date.now();


var intervallobj = setInterval(function() {
    exec('ping -c 1 192.168.1.75', function(error, stdout, stderr) {
        if (error === null) {
            console.log("Markus hemma");
            senasthemma = Date.now();
        }
    });
    exec('ping -c 1 192.168.1.79', function(error, stdout, stderr) {
        if (error === null) {
            console.log("Amanda hemma");
            senasthemma = Date.now();
        }
        });
    exec('ping -c 1 192.168.1.81', function(error, stdout, stderr) {
        if (error === null) {
            console.log("Camilla hemma");
            senasthemma = Date.now();
        }
        });
    console.log(dateToString(senasthemma));
}, 1000*10);


function dateToString(datum) {
    var date = new Date(datum);
    return date.getFullYear() + '-' + fill(2, (date.getMonth() + 1)) + '-' + fill(2, date.getDate()) + ' ' + fill(2, date.getHours()) + ':' + fill(2, date.getMinutes()) + ':' + fill(2, date.getSeconds()) + '.' + fill(3, date.getMilliseconds());
}
function fill(antal, text) {
    while (text.toString().length < antal)
        text = '0' + text;
    return text;
}




