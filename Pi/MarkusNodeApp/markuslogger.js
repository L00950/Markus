﻿var log4js = require('log4js');
log4js.clearAppenders();
log4js.loadAppender('file');
log4js.addAppender(log4js.appenders.file('markus.log'), 'node');
var logger = log4js.getLogger('node');
logger.setLevel('ALL');

var getLogger = function() {
    return logger;
};

exports.logger = getLogger();
