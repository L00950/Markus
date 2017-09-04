// var huset = require('./huset.js');

function HämtaNamnPåSensor(cache, id) {
    cache.forEach(function(element) {
        if (element.id == id) return element.name;
    }, this);
}


var options = {
    url: 'http://admin:8999@192.168.11.250/jsondata.cgi',
    json:true
}
require("request")(options, function(error, response, body){
    if(error) console.log(error);
    else {
        console.log('Batteri: ' + body.perc_batt);
        console.log('Status: ' + body.state);
        console.log('Understatus: ' + body.workReq);
        console.log('Meddelande: ' + (body.message == 'none' ? '' : body.message));
        console.log('Laddarstatus: ' + body.batteryChargerState);
        console.log('Distans:' + body.distance);
        console.log('Firmware:' + body.versione_fw);
    }
});

var namn = HämtaNamnPåSensor([{id: "1", name:"ettan"}, {id: "2", name:"tvåan"}], 2)


// var cache = { aktivtlarm: null };

// setInterval(function() { tabilder.iPhoneTimer(cache, null); }, 2000);

//tabilder.handleLarm(cache, 'Entre');



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


