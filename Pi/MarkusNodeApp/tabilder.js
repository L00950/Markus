var fs = require('fs')
    , http = require('http')
    , markusmail = require('./markusmail.js');

var larmtid = 1000 * 10 * 1;
var sendmail = false;

module.exports = {
    handleLarm: function (cache, name) {
        if (cache.aktivtlarm == null) {
            if(sendmail)
                markusmail.sendmail('markus@linderback.com', 'markus@linderback.com', 'Larm:' + name, '');
            cache.aktivtlarm =
            {
                id: Date.now(),
                expire: (Date.now() + 10000)
            };
            console.log(cache);
            fs.mkdir('larm/' + cache.aktivtlarm.id.toString(), function(err) {
                if (err)
                    console.log('Gick inte skapa mapp larm/' + cache.aktivtlarm.id.toString());
                else {
                    console.log('Innan tabilder');
                    this.tabilderna(cache);
                    console.log('Efter tabilder');
                }
            }.bind(this));
        } else {
            cache.aktivtlarm.expire = Date.now() + this.larmtid;
        }
    },
    tabilderna: function (cache) {
        console.log(Date.now() + ' ' + cache.aktivtlarm.expire);

        var options = {
            host: 'images.webcams.travel',
            port: 80,
            path: '/webcam/1228154082-Väder-Maspalomas-Beach-"-La-Charca"-Meloneras.jpg'
        };

        var request = http.get( options, function ( res ) {
            var imagedata = '';
            res.setEncoding( 'binary' );

            res.on( 'data', function ( chunk ) {
                imagedata += chunk;
            });

            res.on( 'end', function () {
                fs.writeFile( 'larm/'+ cache.aktivtlarm.id.toString() +'/' + Date.now().toString() + '.jpg', imagedata, 'binary', function ( err ) {
                    if ( err ) throw err;
                    console.log('Fil sparad');
                });
            });

        });


        if (Date.now() < cache.aktivtlarm.expire)
            setTimeout(function() { this.tabilderna(cache); }.bind(this), 500);
        else
            cache.aktivtlarm = null;
    }
};


