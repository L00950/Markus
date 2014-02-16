var markusarray = require('./MarkusArray.js');

var cache2 = { larmhistory: [] };
//cache2.larmhistory = ['B', 'C', 'D'];
cache2.larmhistory[0] = 'B';
cache2.larmhistory[1] = 'C';
cache2.larmhistory[2] = 'D';
console.log(cache2.larmhistory.length);
cache2.larmhistory = markusarray.insertFirst(cache2.larmhistory, 'A');
console.log(cache2.larmhistory[0]);
