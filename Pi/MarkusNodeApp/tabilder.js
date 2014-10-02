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
                    //(function () { this.tabilder(cache); }.bind(this));
                    console.log('Efter tabilder');
                }
            }.bind(this));
        } else {
            cache.aktivtlarm.expire = Date.now() + this.larmtid;
        }
    },
    tabilderna: function (cache) {
        console.log(Date.now() + ' ' + cache.aktivtlarm.expire);
        if (Date.now() < cache.aktivtlarm.expire)
            setTimeout(function() { this.tabilderna(cache); }.bind(this), 500);
        else
            cache.aktivtlarm = null;
    }
};


