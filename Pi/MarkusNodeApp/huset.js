var fs = require( 'fs' )
    , socketio = require( 'socket.io' )
    , telldus = require( 'telldus' )
    , path = require( 'path' )
    , iphone = require('./iphone.js')
    , server = require( './server.js' )
    , datasource = require( './datasource.js' )
    , sms = require( './sms.js' )
    , eliq = require( './eliq.js' )
    , elspot = require( './elspot.js' )
    , triggers = require( './triggers.js' )
    , markusarray = require( './markusarray.js' )
    , markusmail = require( './markusmail.js' )
    , tabilder = require( './tabilder.js' )
    , exec = require('child_process').exec
    , config = require('./config.json')
    , net = require('net');

function dateToString(datum) {
    var date = new Date(datum);
    return date.getFullYear() + '-' + fill(2, (date.getMonth() + 1)) + '-' + fill(2, date.getDate()) + ' ' + fill(2, date.getHours()) + ':' + fill(2, date.getMinutes()) + ':' + fill(2, date.getSeconds()) + '.' + fill(3, date.getMilliseconds());
}

function fill(antal, text) {
    while (text.toString().length < antal)
        text = '0' + text;
    return text;
}

console.log('Initiating datasource ...');
datasource.Init(function () {

    console.log('Creating cache ...');

    var cache = {
        telldus_devices: {},
        telldus_sensors: {},
        eliq_datanow: null,
        eliq_dataday: null,
        elspot_now: null,
        devicegroups: null,
        larm: { state: 0 },
        larmhistory: [],
        vpn: config.vpn,
        senasthemma: { tid: Date.now() },
        aktivtlarm: null
    };
    console.log(cache);

    var senasteDeviceAction = Date.now();

    console.log('Initiating triggers:');
    triggers.Init(cache);

    if (config.tellstick.enable === 1) {

        console.log('Connecting tellstick events ...');

        console.log('\tTellstick sensor values ...');
        var sql = "SELECT \
          dest.* \
        FROM \
            (SELECT \
              out.id, \
              out.type, \
              max(out.ts) as ots \
             FROM  \
              telldus_sensor_history as out \
             GROUP BY \
              out.id, \
              out.type) as source \
        LEFT OUTER JOIN \
          telldus_sensor_history as dest ON (source.id = dest.id AND source.type=dest.type AND source.ots = dest.ts)";
        datasource.db.each(
          sql,
          // For each row
          function (err, row) {
              if (!err) {

                  // Only track configured devicesb
                  for (item in config.tellstick.sensors) {

                      if (config.tellstick.sensors[item].id == row.id) {

                          // Prepare
                          var idx = 's_' + row.id + '' + row.type;

                          // Inject value_diff and name 
                          row.value_diff = 0;
                          row.name = config.tellstick.sensors[item].name;

                          cache.telldus_sensors[idx] = row;

                          // Get min/max
                          var ts = Math.round((new Date()).getTime() / 1000) - 3600 * 24;
                          var statementInner = datasource.db.prepare("SELECT min(value) as val_min, max(value) as val_max FROM telldus_sensor_history WHERE id=? AND type=? AND ts>" + ts);
                          statementInner.each(
                            [row.id, row.type],
                            function (errInner, rowInner) {
                                if (!err) {
                                    cache.telldus_sensors[idx].min = rowInner.val_min;
                                    cache.telldus_sensors[idx].max = rowInner.val_max;
                                }
                            }
                          ); statementInner.finalize();
                      }

                  }

              } else {
                  console.log(err);
              }
          },
          // When finished
          function (err) {

          }
        );

        console.log('\tTellstick devices ...');
        var devices = telldus.getDevicesSync();
        devices.forEach(function (item) {
            cache.telldus_devices['d_' + item.id] = {
                id: item.id,
                name: item.name,
                status: item.status.name,
                lastaction: Date.now()
            };
        });

        console.log('\tDevice groups ...');
        cache.devicegroups = config.tellstick.devicegroups;

        console.log('\tLarm history...');
        var index = 0;
        datasource.db.each(
            "select * from telldus_device_history order by ts desc limit 15",
            function (err, row) {
                if (!err) {
                    var item = { id: row.id, status: row.status, ts: row.ts, name: cache.telldus_devices['d_' + row.id].name, larm: 0 };
                    cache.larmhistory[index++] = item;
                } else {
                    console.log(err);
                }
            },
            function (err) {
                if (err)
                    console.log(err);
            });
    }

    console.log('Initiating websockets ...');
    if (config.server.live_stream === 1) {

        // Start listening, disable on screen logging
        var io = socketio.listen(server).set('log level', 0);

        // On connection callback
        io.on('connection', function (socket) {

            // Send initial values
            if (config.debug.enabled) console.log('Transmitting initial cache ...');
            io.sockets.emit('message', { msg: 'initial_data', data: cache });

            // On incoming message callback (has currently no use)
            socket.on('message', function (data) {

                // Incoming!!
                console.log(data);
                if (data.msg == 'telldus_device_on') {
                    telldus.turnOn(parseInt(data.id), function () { });
                } else if (data.msg == 'telldus_device_off') {
                    telldus.turnOff(parseInt(data.id), function () { });
                } else if (data.msg == 'larm') {
                    //datasource.db.prepare("update larm set state = ?").run(parseInt(data.state)).finalize();
                    cache.larm.state = parseInt(data.state);
                    console.log(cache.larm.state);
                    io.sockets.emit('message', { msg: 'larm', data: { state: cache.larm.state} });
                }

            });

            // Handle socket.io errors
            socket.on('error', function (err) {
                console.log('Socket.io connection error: ' + err.errno);
            });

        });

        // Handle socket.io errors
        io.on('error', function (err) {
            console.log('Socket.io Error: ' + err.errno);
        });

    }

    if (config.tellstick.enable === 1) {
        console.log('Initiating tellstick module:');
        console.log('\tStarting sensor event listener ...');
        telldus.addSensorEventListener(function (id, protocol, model, type, value, ts) {

            // Filter out crap
            console.log(protocol + " " + model + " " + value + " " + id);
            if (protocol == "temperature" && (model == "fineoffset" || model == "mandolyn") && Number(value) != NaN && Number(type) != NaN && Number(id) != NaN && Number(value) == value) {

                // Only track configured sensors
                for (item in config.tellstick.sensors) {

                    if (config.tellstick.sensors[item].id == id) {

                        // Check if value has changed, to prevent excessive emits
                        var valueChanged = (cache.telldus_sensors['s_' + id + '' + type] == undefined || value != cache.telldus_sensors['s_' + id + '' + type].value) ? true : false;
                        var valueDiff = (cache.telldus_sensors['s_' + id + '' + type] == undefined) ? 0 : value - cache.telldus_sensors['s_' + id + '' + type].value;
                        var valueMin = (cache.telldus_sensors['s_' + id + '' + type] == undefined) ? value : cache.telldus_sensors['s_' + id + '' + type].min;
                        var valueMax = (cache.telldus_sensors['s_' + id + '' + type] == undefined) ? value : cache.telldus_sensors['s_' + id + '' + type].max;
                        valueMin = (value < valueMin) ? value : valueMin;
                        valueMax = (value > valueMax) ? value : valueMax;

                        // Update cache

                        cache.telldus_sensors['s_' + id + '' + type] = {
                            id: id,
                            type: type,
                            name: config.tellstick.sensors[item].name,
                            ts: ts,
                            message: model,
                            protocol: protocol,
                            value: value,
                            value_diff: valueDiff,
                            min: valueMin,
                            max: valueMax
                        };

                        // Notify triggers
                        if (valueChanged)
                            triggers.notifySensorUpdate(id, type);

                        // Insert in database
                        if (config.tellstick.sensor_history === 1 && valueChanged)
                            try {
                                datasource.db.prepare("INSERT INTO telldus_sensor_history (id,message,protocol,type,value,ts) VALUES(?,?,?,?,?,?)").run(id, model, protocol, type, value, ts).finalize();
                            } catch (err) {
                                console.error('DB insert failed: ', err);
                            }

                        // Broadcast sensor value to all clients
                        if (config.tellstick.sensor_emit === 1 && config.server.live_stream === 1 && valueChanged)
                            io.sockets.emit('message', { msg: "tellstick_sensor_update", data: cache.telldus_sensors['s_' + id + '' + type] });
                    }
                }
            } else {
                if (config.debug.enabled) console.info('Invalid sensor data received');
            }

        });
        console.log('\tStarting device event listener ...');
        telldus.addDeviceEventListener(function (device, status) {
            var now = Date.now();
            if (now - senasteDeviceAction < 500) return;
            senasteDeviceAction = now;
            var name = cache.telldus_devices['d_' + device].name;
            if (now - cache.telldus_devices['d_' + device].lastaction < 4000) {
                return;
            }
            cache.telldus_devices['d_' + device] = {
                id: device,
                name: name,
                status: status.name,
                lastaction: now
            };
            var larmenhet = false;
            for (index in config.tellstick.larmdevices) {
                if (config.tellstick.larmdevices[index] == device)
                    larmenhet = true;
            }
            console.log(dateToString(now) + ' Action:' + name + ' status.name:' + status.name);

            if (status.name === 'OFF' && larmenhet) {
                console.log('Status OFF gor vi inget med pa larmenheter');
                return;
            }
            if (larmenhet && cache.larm) {
                console.log(dateToString(now) + ' Tar hand om larm från ' + name);
                tabilder.handleLarm(cache, name);
            }

            // Notify triggers
            //triggers.notifyDeviceUpdate(device);

            // Broadcast device event to all clients
            if (config.tellstick.device_emit === 1 && config.server.live_stream === 1 && larmenhet === false)
                io.sockets.emit('message', { msg: "tellstick_device_update", data: cache.telldus_devices['d_' + device] });

            // Add device event to database, add ts manually
            if (config.tellstick.device_history === 1) {

                if (larmenhet === true) {
                    // Prepare data
                    var lStatusNum = (status.name === 'ON') ? 1 : 0;
                    var ts = now / 1000;
                    

                    // Execute statement
                    try {
                        datasource.db.prepare("INSERT INTO telldus_device_history (id,status,ts) VALUES(?,?,?)").run(device, lStatusNum, ts).finalize();
                    } catch (err) {
                        console.error('DB insert failed: ', err);
                    }
                    cache.larmhistory = markusarray.insertFirst(cache.larmhistory, { id: device, status: lStatusNum, ts: ts, name: cache.telldus_devices['d_' + device].name, larm: larm });
                }
                io.sockets.emit('message', { msg: "larmhistory", data: cache.larmhistory });
            }

        });
        console.log('\tStarting raw device event listener ...');
        telldus.addRawDeviceEventListener(function (controllerId, data) {
            if(config.tellstick.lograwevent === 1)
                console.log(dateToString(Date.now()) + ' Rawdata: ' + data);

            // Notify triggers
//          triggers.notifyRawDeviceUpdate(data);

        });
    }

    if (config.eliq.enable === 1) {
        console.log('Initiating Eliq module:');
        console.log('\tStarting live power consumption listener ...');
        eliq.onDatanowUpdate = function (data) {

            // Check if value has changed
            var valueChanged = (cache.eliq_datanow == null || cache.eliq_datanow.power != data.power) ? true : false;

            // Save to cache
            cache.eliq_datanow = data;

            // Emit update
            if (config.server.live_stream === 1 && valueChanged)
                io.sockets.emit('message', { msg: "eliq_datanow", data: data });

        };
        console.log('\tStarting daily power consumption listener ...');
        eliq.onDatadayUpdate = function (data) {

            // Save to cache
            cache.eliq_dataday = data;

            // Emit update
            if (config.server.live_stream === 1)
                io.sockets.emit('message', { msg: "eliq_dataday", data: data });

        }
        eliq.Start();
    }

    if (config.elspot.enable === 1) {
        console.log('Initiating Electricity price module:');
        console.log('\tStarting live electricity price listener ...');
        elspot.onNowUpdate = function (data) {

            // Check if value has changed
            var valueChanged = (cache.elspot_now == null || cache.elspot_now.full_price != data.full_price) ? true : false;

            // Save to cache
            cache.elspot_now = data;

            // Emit update
            if (config.server.live_stream === 1 && valueChanged)
                io.sockets.emit('message', { msg: "elspot_now", data: data });

        };
        elspot.Start();
    }

    //console.log('Startar att pinga iPhone...');
    //setInterval(iphone.iPhoneTimer(cache, io), config.tid_mellan_ping_till_iphones);

    //var hemmakontrollIntervall = setInterval(function () {
    //    console.log('Kollar om någon är hemma. Larm: ' + larm);
    //    console.log('Senaste tid någon var hemma: ' + dateToString(cache.senasthemma.tid));
    //    if (larm == 0) {
    //        console.log('Larm av');
    //        if (((Date.now() - cache.senasthemma.tid) > (1000 * 60 * 60)) && larm == 0 && config.skicka_mail_om_ingen_hemma_och_larm_av == true) {
    //            console.log('Ingen hemma och larmet av');
    //            markusmail.sendmail('markus@linderback.com', 'markus@linderback.com', 'Larm - Ingen hemma?', 'Starta larmet på http://linderback.com:8081');
    //        }
    //    }
    //}, (1000 * 60 * 60)); // varje timme

    console.log('Startar VPN-tjänst...');
    net.createServer(function (socket) {
        console.log('connected');
        
        socket.on('data', function (data) {
            var meddelande = data.toString();
            console.log('Mottaget:' + data);
            if (meddelande.indexOf('enablevpn') > -1) {
                if (meddelande.indexOf('RYDA') > -1) {
                    console.log('RYDA frågar efter enablevpn');
                    socket.write('0');
                } else {
                    console.log('Annan än RYDA frågar efter enablevpn');
                    socket.write('1');
                }
            }
        });
    }).listen(8089);

});

    // Super sweet errorhandling.. Until someone figures out the ECONNRESET problem
process.on('uncaughtException', function (err) {
    console.log('Unhandled error occurred: ', err);
});
