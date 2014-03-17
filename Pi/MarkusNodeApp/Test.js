//var mail = require('./markusmail');

//mail.sendmail('markus@linderback.com', 'markus@linderback.com', 'Testmail', 'Innehåll...');
var markusarray = require('./markusarray');
var l = ['B', 'C', 'D'];
var l2 = markusarray.insertFirst(l, 'A');
console.log(l2.length);