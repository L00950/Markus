var tabilder = require('./tabilder.js');

var cache = { aktivtlarm: null };

tabilder.handleLarm(cache, 'Entre');



//var options = {
//    host: 'images.webcams.travel',
//    port: 80,
//    path: '/webcam/1228154082-Väder-Maspalomas-Beach-"-La-Charca"-Meloneras.jpg'
//};

//var request = http.get(options, function(res) {
//    var imagedata = '';
//    res.setEncoding('binary');

//    res.on('data', function(chunk) {
//        imagedata += chunk;
//    });

//    res.on('end', function() {
//        fs.writeFile('client/larm/gc.jpg', imagedata, 'binary', function(err) {
//            if (err) throw err;
//            console.log('File saved.');
//        });
//    });

//});


