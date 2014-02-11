    // Include node.js modules
var fs = require('fs')
  , socketio = require('socket.io')
  , telldus = require('telldus')
  , path = require('path')

  // Include huset.js modules
  , server = require('./server.js')
  , datasource = require('./datasource.js')
  , sms = require('./sms.js')
  , eliq = require('./eliq.js')
  , elspot = require('./elspot.js')
  , triggers = require('./triggers.js')

  // Include configuration
  , config = require('./config.json');

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
    var cache = { telldus_devices: {}, telldus_sensors: {}, eliq_datanow: null, eliq_dataday: null, elspot_now: null, devicegroups: null, larm: {}, larmhistory: {} };
    var larm = 0;
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
                  for (sensor in config.tellstick.sensors) {

                      if (config.tellstick.sensors[sensor].id == row.id) {

                          // Prepare
                          var idx = 's_' + row.id + '' + row.type

                          // Inject value_diff and name 
                          row.value_diff = 0;
                          row.name = config.tellstick.sensors[sensor].name;

                          cache.telldus_sensors[idx] = row;

                          // Get min/max
                          var ts = Math.round((new Date()).getTime() / 1000) - 3600 * 24;
                          var statement_inner = datasource.db.prepare("SELECT min(value) as val_min, max(value) as val_max FROM telldus_sensor_history WHERE id=? AND type=? AND ts>" + ts);
                          statement_inner.each(
                            [row.id, row.type],
                            function (err_inner, row_inner) {
                                if (!err) {
                                    cache.telldus_sensors[idx].min = row_inner.val_min;
                                    cache.telldus_sensors[idx].max = row_inner.val_max;
                                }
                            }
                          ); statement_inner.finalize();
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

        console.log('\tLarm status...');
        cache.larm = { state: larm }; // av vid startup
        //sql = "select state from larm";
        //datasource.db.each(
        //    sql,
        //    function (err, row) {
        //        if (!err) {
        //            cache.larm = { state: row.state };
        //            console.log(row.state);
        //        } else {
        //            console.log(err);
        //        }
        //    },
        //    function (err) {
        //        console.log(err);
        //    }
        //    );

    }

    console.log('Initiating websockets ...');
    if (config.server.live_stream === 1) {

        // Start listening, disable on screen logging
        var io = socketio.listen(server).set('log level', 0);

        // On connection callback
        io.on('connection', function (socket) {

            // Send initial values
            if (config.debug.enabled) console.log('Transmitting initial cache ...');
            // uppdatera cache med status på larm
            cache.larm = { state: larm };
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
                    larm = parseInt(data.state);
                    console.log(larm);
                    io.sockets.emit('message', { msg: 'larm', data: { state: larm } });
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
                for (sensor in config.tellstick.sensors) {

                    if (config.tellstick.sensors[sensor].id == id) {

                        // Check if value has changed, to prevent excessive emits
                        var value_changed = (cache.telldus_sensors['s_' + id + '' + type] == undefined || value != cache.telldus_sensors['s_' + id + '' + type].value) ? true : false;
                        var value_diff = (cache.telldus_sensors['s_' + id + '' + type] == undefined) ? 0 : value - cache.telldus_sensors['s_' + id + '' + type].value;
                        var value_min = (cache.telldus_sensors['s_' + id + '' + type] == undefined) ? value : cache.telldus_sensors['s_' + id + '' + type].min;
                        var value_max = (cache.telldus_sensors['s_' + id + '' + type] == undefined) ? value : cache.telldus_sensors['s_' + id + '' + type].max;
                        value_min = (value < value_min) ? value : value_min;
                        value_max = (value > value_max) ? value : value_max;

                        // Update cache

                        cache.telldus_sensors['s_' + id + '' + type] = {
                            id: id,
                            type: type,
                            name: config.tellstick.sensors[sensor].name,
                            ts: ts,
                            message: model,
                            protocol: protocol,
                            value: value,
                            value_diff: value_diff,
                            min: value_min,
                            max: value_max
                        };

                        // Notify triggers
                        if (value_changed)
                            triggers.notifySensorUpdate(id, type);

                        // Insert in database
                        if (config.tellstick.sensor_history === 1 && value_changed)
                            try {
                                datasource.db.prepare("INSERT INTO telldus_sensor_history (id,message,protocol,type,value,ts) VALUES(?,?,?,?,?,?)").run(id, model, protocol, type, value, ts).finalize();
                            } catch (err) {
                                console.error('DB insert failed: ', err);
                            }

                        // Broadcast sensor value to all clients
                        if (config.tellstick.sensor_emit === 1 && config.server.live_stream === 1 && value_changed)
                            io.sockets.emit('message', { msg: "tellstick_sensor_update", data: cache.telldus_sensors['s_' + id + '' + type] });
                    }
                }
            } else {
                if (config.debug.enabled) console.info('Invalid sensor data received');
            }

        });
        console.log('\tStarting device event listener ...');
        telldus.addDeviceEventListener(function (device, status) {
            // Update cache (preserve name)
            var now = Date.now();
            if (now - senasteDeviceAction < 500) return;
            senasteDeviceAction = now;
            var name = cache.telldus_devices['d_' + device].name;
            if (now - cache.telldus_devices['d_' + device].lastaction < 4000) {
                //console.log('Dubbel action:' + name + ' Status:' + status.name);
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
                console.log('Index:' + index);
                console.log('Device:' + device);
                if (config.tellstick.larmdevices[index] == device)
                    larmenhet = true;
            }
            console.log('Larmenhet:' + larmenhet);
            console.log(dateToString(now) + ' Action:' + name + ' status.name:' + status.name + 'status:' + status);

            if (status.name === 'OFF' && larmenhet) {
                console.log('Status OFF gor vi inget med pa larmenheter');
                return;
            }
            console.log(dateToString(now) + ' Tar hand om larm...');

            // Notify triggers
            //triggers.notifyDeviceUpdate(device);

            // Broadcast device event to all clients
            if (config.tellstick.device_emit === 1 && config.server.live_stream === 1 && larmenhet === false)
                io.sockets.emit('message', { msg: "tellstick_device_update", data: cache.telldus_devices['d_' + device] });

            // Add device event to database, add ts manually
            if (config.tellstick.device_history === 1) {

                if (larmenhet === true) {
                    // Prepare data
                    var l_status_num = (status.name === 'ON') ? 1 : 0;
                    var ts = now / 1000;
                    

                    // Execute statement
                    try {
                        datasource.db.prepare("INSERT INTO telldus_device_history (id,status,ts) VALUES(?,?,?)").run(device, l_status_num, ts).finalize();
                    } catch (err) {
                        console.error('DB insert failed: ', err);
                    }
                    //var larmhistoryIndex = 0;
                    //datasource.db.each(
                    //  "select * from telldus_device_history order by ts desc limit 10",
                    //  function (err, row) {
                    //      if (!err) {
                    //          var item = { id: row.id, status: row.status, ts: row.ts, name: cache.telldus_devices['d_'+row.id].name };
                    //          console.log(larmhistoryIndex + ':' + item.name);
                    //          cache.larmhistory[larmhistoryIndex++] = item;
                    //      } else {
                    //          console.log(err);
                    //      }
                    //  },
                    //  function (err) {
                    //      if(err)
                    //        console.log(err);
                    //  });

                    cache.larmhistory[14] = cache.larmhistory[13];
                    cache.larmhistory[13] = cache.larmhistory[12];
                    cache.larmhistory[12] = cache.larmhistory[11];
                    cache.larmhistory[11] = cache.larmhistory[10];
                    cache.larmhistory[10] = cache.larmhistory[9];
                    cache.larmhistory[9] = cache.larmhistory[8];
                    cache.larmhistory[8] = cache.larmhistory[7];
                    cache.larmhistory[7] = cache.larmhistory[6];
                    cache.larmhistory[6] = cache.larmhistory[5];
                    cache.larmhistory[5] = cache.larmhistory[4];
                    cache.larmhistory[4] = cache.larmhistory[3];
                    cache.larmhistory[3] = cache.larmhistory[2];
                    cache.larmhistory[2] = cache.larmhistory[1];
                    cache.larmhistory[1] = cache.larmhistory[0];
                    cache.larmhistory[0] = { id: device, status: l_status_num, ts: ts, name: cache.telldus_devices['d_' + device].name, larm: larm };
                }
                io.sockets.emit('message', { msg: "larmhistory", data: cache.larmhistory });
            }

        });
        console.log('\tStarting raw device event listener ...');
        telldus.addRawDeviceEventListener(function (controllerId, data) {
            console.log('Rawdata: ' + data)

          // Notify triggers
//          triggers.notifyRawDeviceUpdate(data);

        });
    }

    if (config.eliq.enable === 1) {
        console.log('Initiating Eliq module:');
        console.log('\tStarting live power consumption listener ...');
        eliq.onDatanowUpdate = function (data) {

            // Check if value has changed
            var value_changed = (cache.eliq_datanow == null || cache.eliq_datanow.power != data.power) ? true : false;

            // Save to cache
            cache.eliq_datanow = data;

            // Emit update
            if (config.server.live_stream === 1 && value_changed)
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
            var value_changed = (cache.elspot_now == null || cache.elspot_now.full_price != data.full_price) ? true : false;

            // Save to cache
            cache.elspot_now = data;

            // Emit update
            if (config.server.live_stream === 1 && value_changed)
                io.sockets.emit('message', { msg: "elspot_now", data: data });

        };
        elspot.Start();
    }
});

    // Super sweet errorhandling.. Until someone figures out the ECONNRESET problem
process.on('uncaughtException', function (err) {
    console.log('Unhandled error occurred: ', err);
});
