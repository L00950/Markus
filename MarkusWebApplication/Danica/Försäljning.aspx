<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Försäljning.aspx.cs" Inherits="Danica_Försäljning" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(drawChart);
        function drawChart()
        {
            var data = google.visualization.arrayToDataTable([
              ['Year', '2012', '2013'],
              ['Jan', 1000, 400],
              ['Feb', 1170, 460],
              ['Mars', 660, 1120],
              ['April', 1030, 540],
              ['Maj', 1030, 540],
              ['Juni', 1030, 540],
              ['Juli', 1030, 540],
              ['Augusti', 1030, 540],
              ['September', 1030, 540],
              ['Oktober', 1030, 540],
              ['November', 1030, 540],
              ['December', 1030, 1030]
            ]);

            var options = {
                title: 'Premier'
            };

            var chart = new google.visualization.LineChart(document.getElementById('chart_div'));
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
