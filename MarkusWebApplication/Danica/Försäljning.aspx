﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Försäljning.aspx.cs" Inherits="Danica_Försäljning" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="apple-mobile-web-app-capable" content="yes"/>
    <link rel="apple-touch-icon" href="img/touch-icon-ipad.png"/>
    <link rel="apple-touch-icon" sizes="76x76" href="img/touch-icon-ipad.png"/>
    <link rel="apple-touch-icon" sizes="120x120" href="img/touch-icon-iphone-retina.png"/>
    <link rel="apple-touch-icon" sizes="152x152" href="img/touch-icon-ipad-retina.png"/>

    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(drawChart);
        function drawChart()
        {
            var data = google.visualization.arrayToDataTable([
              ['Kanal', 'Procent'],
              ['Mäklare', 1000],
              ['Bank', 1170],
              ['Kryss', 660]
            ]);

            var options = {
                title: 'Premier per kanal'
            };

            var chart = new google.visualization.PieChart(document.getElementById('chart_div'));
            chart.draw(data, options);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="chart_div" style="width: 900px; height: 500px;"></div>
        <script type="text/javascript">document.write(screen.width)</script>
    </form>
</body>
</html>
