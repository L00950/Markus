var fs = require('fs')
    , http = require('http')
    , markusmail = require('./markusmail.js')
    , config = require('./config.json');

module.exports = {
    handleLarm: function (cache, name) {
        if (cache.aktivtlarm == null) {
            if(config.email_vid_alarm == true)
                markusmail.sendmail('markus@linderback.com', 'markus@linderback.com', 'Larm:' + name, '');
            cache.aktivtlarm =
            {
                id: Date.now(),
                expire: (Date.now() + config.tid_larm_ar_aktivt)
            };
            console.log(cache);
            fs.mkdir('larm/' + cache.aktivtlarm.id.toString(), function(err) {
                if (err)
                    console.log('Gick inte skapa mapp larm/' + cache.aktivtlarm.id.toString());
                else {
                    this.tabilderna(cache);
                }
            }.bind(this));
        } else {
            cache.aktivtlarm.expire = Date.now() + this.larmtid;
        }
    },
    tabilderna: function (cache) {
        for (item in config.cameras) {
            if (config.cameras[item].enabled == false) continue;
            try {
                http.get(config.cameras[item].adress, function (res) {
                    var imagedata = '';
                    res.setEncoding('binary');
                    
                    res.on('data', function (chunk) {
                        imagedata += chunk;
                    });
                    
                    res.on('end', function () {
                        if (cache.aktivtlarm != null) {
                            fs.writeFile('larm/' + cache.aktivtlarm.id.toString() + '/' + Date.now().toString() + '.jpg', imagedata, 'binary', function (err) {
                            });
                        }                        ;
                    });
                }).on('error', function() { console.log('Fel vid ta bild'); });
            }
            catch (exp) { }
        }


        if (Date.now() < cache.aktivtlarm.expire)
            setTimeout(function() { this.tabilderna(cache); }.bind(this), config.tid_mellan_larmbilder);
        else
            cache.aktivtlarm = null;
    }
};


