function temperature() {};
temperature.prototype.name = "Temperature";
temperature.prototype.run = function() {

  // Prepare DOM-elements
    var container_temp = $('<div class="container container-400" id="sensor_container">');
  container_temp.append('<div class="box box-w-400 box-h-500 bg-dark" id="larmhistory_container">');
  //container_temp.append('<div class="box box-w-400 box-h-500 bg-dark" id="cam_container">');
  //container_temp.append('<div class="box box-w-400 box-h-200 bg-dark" id="eliq_chart_dataday">');
  //container_temp.append('<div class="box box-w-200 box-h-100 bg-dark" id="spot_1">');
  //container_temp.append('<div class="box box-w-200 box-h-100 bg-dark" id="eliq_datanow">');
  //container_temp.append('<div class="box box-w-200 box-h-100 bg-dark" id="eliq_dataday">');

  // .. and put them in the document
  $("body").append(container_temp);
  
};

// Activate this plugin
huset.plugins.push(new temperature());
