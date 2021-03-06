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
    , net = require('net')
    , json = require('json-serialize');

function dateToString(datum) {
    var date = new Date(datum);
    return date.getFullYear() + '-' + fill(2, (date.getMonth() + 1)) + '-' + fill(2, date.getDate()) + ' ' + fill(2, date.getHours()) + ':' + fill(2, date.getMinutes()) + ':' + fill(2, date.getSeconds()) + '.' + fill(3, date.getMilliseconds());
}

function fill(antal, text) {
    while (text.toString().length < antal)
        text = '0' + text;
    return text;
}

function HämtaNamnPåSensor(config, id) {
    var retval = '';
    config.tellstick.sensors.forEach(function(element) {
        if (element.id === id) retval = element.name;
    }, this);
    return retval;
}

console.log(dateToString(Date.now()) + ' Initiating datasource ...');
datasource.Init(function () {

    console.log(dateToString(Date.now()) + ' Creating cache ...');

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
        aktivtlarm: null,
        senastetemp: { temp: 0, humidity: 0},
        robban: { status: 'Klipper', batteri: 59}
    };
    console.log(cache);

    var senasteDeviceAction = Date.now();

    console.log(dateToString(Date.now()) + ' Initiating triggers:');
    triggers.Init(cache);

    if (config.tellstick.enable === 1) {

        console.log(dateToString(Date.now()) + ' Connecting tellstick events ...');

        console.log(dateToString(Date.now()) + ' \tTellstick sensor values ...');
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

        console.log(dateToString(Date.now()) + ' \tTellstick devices ...');
        var devices = telldus.getDevicesSync();
        devices.forEach(function (item) {
            cache.telldus_devices['d_' + item.id] = {
                id: item.id,
                name: item.name,
                status: item.status.name,
                lastaction: Date.now()
            };
        });

        console.log(dateToString(Date.now()) + ' \tDevice groups ...');
        cache.devicegroups = config.tellstick.devicegroups;

        console.log(dateToString(Date.now()) + ' \tLarm history...');
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

    console.log(dateToString(Date.now()) + ' Initiating websockets ...');
    if (config.server.live_stream === 1) {

        // Start listening, disable on screen logging
        var io = socketio.listen(server).set('log level', 0);

        // On connection callback
        io.on('connection', function (socket) {

            // Send initial values
            if (config.debug.enabled) console.log(dateToString(Date.now()) + ' Transmitting initial cache ...');
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
                } else if (data.msg == 'vpn') {
                    for (item in cache.vpn) {
                        if (cache.vpn[item].name == data.name) {
                            cache.vpn[item].enabled = data.enabled;
                            io.sockets.emit('message', {msg: 'vpn', data: cache.vpn[item]});
                        }
                    }
                }

            });

            // Handle socket.io errors
            socket.on('error', function (err) {
                console.log(dateToString(Date.now()) + ' Socket.io connection error: ' + err.errno);
            });

        });

        // Handle socket.io errors
        io.on('error', function (err) {
            console.log(dateToString(Date.now()) + ' Socket.io Error: ' + err.errno);
        });

    }

    if (config.tellstick.enable === 1) {
        console.log(dateToString(Date.now()) + ' Initiating tellstick module:');
        console.log(dateToString(Date.now()) + ' \tStarting sensor event listener ...');
        telldus.addSensorEventListener(function (id, protocol, model, type, value, ts) {

            // Filter out crap
            console.log(dateToString(Date.now()) +  " Sensor event Protocol:" +  protocol + " Model:" + model + " Value:" + value + " Id:" + id + " Type:" + type);
            if ((protocol == "temperature" || protocol == "temperaturehumidity") && (model == "fineoffset" || model == "mandolyn") && Number(value) != NaN && Number(type) != NaN && Number(id) != NaN && Number(value) == value) {

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
                if (config.debug.enabled) console.info(dateToString(Date.now()) + ' Invalid sensor data received');
            }

        });
        console.log(dateToString(Date.now()) + ' \tStarting device event listener ...');
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
                console.log(dateToString(Date.now()) + ' Status OFF gor vi inget med pa larmenheter');
                return;
            }
            if (larmenhet && cache.larm.state == 1) {
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
                    cache.larmhistory = markusarray.insertFirst(cache.larmhistory, { id: device, status: lStatusNum, ts: ts, name: cache.telldus_devices['d_' + device].name, larm: cache.larm.state });
                }
                io.sockets.emit('message', { msg: "larmhistory", data: cache.larmhistory });
            }

        });
        console.log(dateToString(Date.now()) + ' \tStarting raw device event listener ...');
        telldus.addRawDeviceEventListener(function (controllerId, data) {
            if(config.tellstick.lograwevent === 1)
                console.log(dateToString(Date.now()) + ' Rawdata: ' + data);

            if (data.toString().indexOf('temperaturehumidity') > -1) {
                var id = 0;
                var temp = 0;
                var humidity = 0;
                var pairs = data.split(";");
                for (var pairIndex in pairs) {
                    var pair = pairs[pairIndex];
                    if (pair.split(":")[0] == "id") {
                        id = pair.split(":")[1];
                    }
                    if (pair.split(":")[0] == "temp") {
                        temp = pair.split(":")[1];
                    }
                    if (pair.split(":")[0] == "humidity") {
                        humidity = pair.split(":")[1];
                    }
                }

                if (Number(id) == 135 && (temp != cache.senastetemp.temp || humidity != cache.senastetemp.humidity)) {
                    cache.senastetemp.temp = temp;
                    cache.senastetemp.humidity = humidity;


                }
            }
            // Notify triggers
//          triggers.notifyRawDeviceUpdate(data);

        });
    }

    if (config.eliq.enable === 1) {
        console.log(dateToString(Date.now()) + ' Initiating Eliq module:');
        console.log(dateToString(Date.now()) + ' \tStarting live power consumption listener ...');
        eliq.onDatanowUpdate = function (data) {

            // Check if value has changed
            var valueChanged = (cache.eliq_datanow == null || cache.eliq_datanow.power != data.power) ? true : false;

            // Save to cache
            cache.eliq_datanow = data;

            // Emit update
            if (config.server.live_stream === 1 && valueChanged)
                io.sockets.emit('message', { msg: "eliq_datanow", data: data });

        };
        console.log(dateToString(Date.now()) + ' \tStarting daily power consumption listener ...');
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
        console.log(dateToString(Date.now()) + ' Initiating Electricity price module:');
        console.log(dateToString(Date.now()) + ' \tStarting live electricity price listener ...');
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

    console.log(dateToString(Date.now()) + ' Startar att pinga iPhone...');
    setInterval(function() { iphone.iPhoneTimer(cache, io); }, config.tid_mellan_ping_till_iphones);

    var hemmakontrollIntervall = setInterval(function () {
        console.log(dateToString(Date.now()) + ' Kollar om någon är hemma. Larm: ' + cache.larm.state);
        console.log(dateToString(Date.now()) + ' Senaste tid någon var hemma: ' + dateToString(cache.senasthemma.tid));
        if (cache.larm.state == 0) {
            console.log(dateToString(Date.now()) + ' Larm av');
            if (((Date.now() - cache.senasthemma.tid) > (1000 * 60 * 60)) && cache.larm.state == 0 && config.skicka_mail_om_ingen_hemma_och_larm_av == true) {
                console.log(dateToString(Date.now()) + ' Ingen hemma och larmet av');
                markusmail.sendmail('markus@linderback.com', 'markus@linderback.com', 'Larm - Ingen hemma?', 'Starta larmet på http://linderback.com:8081');
            }
        }
    }, (1000 * 60 * 60)); // varje timme

    console.log(dateToString(Date.now()) + ' Startar VPN-tjänst...');
    net.createServer(function (socket) {
        console.log(dateToString(Date.now()) + ' connected');

        socket.on('end', function() {
            console.log(dateToString(Date.now()) + ' disconnected');
        });
        
        socket.on('timeout', function () {
            console.log(dateToString(Date.now()) + ' timeout');
        });
        
        socket.on('close', function (hadError) {
            console.log(dateToString(Date.now()) + ' close, hadError=' + hadError);
        });

        socket.on('data', function (data) {
            var meddelande = data.toString();
            console.log(dateToString(Date.now()) + ' Mottaget:' + data);
            if (meddelande.indexOf('enablevpn') > -1) {
                if (meddelande.indexOf('RYDA') > -1) {
                    console.log(dateToString(Date.now()) + ' RYDA frågar efter enablevpn');
                    socket.write('1');
                } else {
                    console.log(dateToString(Date.now()) + ' Annan än RYDA frågar efter enablevpn');
                    socket.write('1');
                }
            } else if (meddelande.indexOf('larm') > -1) {
                console.log(dateToString(Date.now()) + ' LARM Spanien');
                cache.larmhistory = markusarray.insertFirst(cache.larmhistory, { id: 99, status: 1, ts: Date.now()/1000, name: 'Spanien', larm: 1 });
                io.sockets.emit('message', { msg: "larmhistory", data: cache.larmhistory });
                //markusmail.sendmail('markus@linderback.com', 'markus@linderback.com', 'Larm spanien', '');
            } else if (meddelande.indexOf('temp') > -1) {
                var pairs = meddelande.split(";");
                console.log(dateToString(Date.now()) + ' Temp kommer från Spanien');
                var place = '';
                var temp = 0;
                var humidity = 0;
                for (var i in pairs) {
                    console.log(dateToString(Date.now()) + ' ' + pair);
                    var pair = pairs[i];
                    if (pair.split(":")[0] == "place") {
                        place = pair.split(":")[1];
                    }
                    if (pair.split(":")[0] == "temp") {
                        temp = pair.split(":")[1];
                    }
                    if (pair.split(":")[0] == "humidity") {
                        humidity = pair.split(":")[1];
                    }
                }
                if (place.indexOf('spain') !== -1) {
                    var id = place === 'spain_outside' ? 1000 : 1001;
                    var namn = HämtaNamnPåSensor(config, id);
                    console.log(dateToString(Date.now()) + ' Temp ' + namn + ' ' + temp + ' fuktighet ' + humidity);
                    var type = 1;
                    cache.telldus_sensors['s_' + id + '' + type] = {
                        id: id,
                        type: type,
                        name: namn,
                        ts: Date.now(),
                        message: '',
                        protocol: '',
                        value: temp,
                        value_diff: cache.telldus_sensors['s_' + id + '' + type] == undefined ? 0 : temp - cache.telldus_sensors['s_' + id + '' + type].value,
                        min: cache.telldus_sensors['s_' + id + '' + type] == undefined ? temp : Math.min(cache.telldus_sensors['s_' + id + '' + type].min, temp),
                        max: cache.telldus_sensors['s_' + id + '' + type] == undefined ? temp : Math.max(cache.telldus_sensors['s_' + id + '' + type].max, temp)
                    };
                    io.sockets.emit('message', { msg: "tellstick_sensor_update", data: cache.telldus_sensors['s_' + id + '' + type] });

                    // Insert in database
                    try {
                        datasource.db.prepare("INSERT INTO telldus_sensor_history (id,message,protocol,type,value,ts) VALUES(?,?,?,?,?,?)").run(id, '', '', type, temp, Date.now()).finalize();
                    } catch (err) {
                        console.error(dateToString(Date.now()) + 'DB insert failed: ', err);
                    }
                    
                    type = 2;
                    cache.telldus_sensors['s_' + id + '' + 2] = {
                        id: id,
                        type: type,
                        name: namn,
                        ts: Date.now(),
                        message: '',
                        protocol: '',
                        value: humidity,
                        value_diff: 0,
                        min: humidity,
                        max: humidity
                    };
                    io.sockets.emit('message', { msg: "tellstick_sensor_update", data: cache.telldus_sensors['s_' + id + '' + type] });
                } else if (place == 'ryda') {
                    console.log(dateToString(Date.now()) + ' Temp Ryda ' + temp + ' fuktighet ' + humidity);
                }
            } else if (meddelande.indexOf('cache') > -1) {
                socket.write(JSON.stringify(cache));
            } else {
                socket.write('unknown command: ' + meddelande);
            }
        });
    }).listen(8089);

});

process.on('uncaughtException', function (err) {
    var msg = dateToString(Date.now()) + ' Ohanterat exception: ' + err;
    console.log(msg);
    markusmail.sendmail('markus@linderback.com', 'markus@linderback.com', 'Exception på raspen', msg);
});
