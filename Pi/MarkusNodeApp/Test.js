//var mail = require('./markusmail');

//mail.sendmail('markus@linderback.com', 'markus@linderback.com', 'Testmail', 'Innehåll...');
var countdown = 5;
var fs = require('fs');
http = require('http');

fs.exists('client/larm', function(exists) {
    if (exists) {
        console.log('Folder finns');
    } else {
        fs.mkdir('client/larm', function(err) {
            if (err)
                console.log('Error:' + err);
            else
                console.log('Success!');
        });
    }
});
var intervallobj = setInterval(function() {
    console.log(countdown);
    countdown--;
}, 1000);

var timerobj = setTimeout(function() {
    clearInterval(intervallobj);
    console.log('Intervall stoppat');
    console.log('klart');
}, 4000);


var options = {
    host: 'images.webcams.travel',
    port: 80,
    path: '/webcam/1228154082-Väder-Maspalomas-Beach-"-La-Charca"-Meloneras.jpg'
};

var request = http.get(options, function(res) {
    var imagedata = '';
    res.setEncoding('binary');

    res.on('data', function(chunk) {
        imagedata += chunk;
    });

    res.on('end', function() {
        fs.writeFile('client/larm/gc.jpg', imagedata, 'binary', function(err) {
            if (err) throw err;
            console.log('File saved.');
        });
    });

});


