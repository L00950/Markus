<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Danica.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Danica</title>
    <meta name="viewport" content="width=device-width"/>
    <meta name="apple-mobile-web-app-capable" content="yes"/>
    <link rel="apple-touch-icon" href="img/touch-icon-ipad.png"/>
    <link rel="apple-touch-icon" sizes="76x76" href="img/touch-icon-ipad.png"/>
    <link rel="apple-touch-icon" sizes="120x120" href="img/touch-icon-iphone-retina.png"/>
    <link rel="apple-touch-icon" sizes="152x152" href="img/touch-icon-ipad-retina.png"/>

    <link rel="stylesheet" type="text/css" href="css/iphone.css" />
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script>
        var minuter = 5;
        var milliSekunder = minuter * 60 * 1000;
        function init() {
            setTimeout(reLoad, milliSekunder);
            if (screen.width < 1000) {
                var t = document.all["huvudTabell"];
                //t.style.cssText = t.style.cssText.replace("40px", "30px");
            }
        }

        function reLoad() {
            window.location = "http://linderback.com/markuswebapplication/danica/";
        }

        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(drawChart);
        var colors = ['blue', 'indigo', 'darkblue'];
        var w = (window.orientation == 0 ? window.screen.width : window.screen.height) - 30;
        

        function drawChart() {
            var data = google.visualization.arrayToDataTable([
                ['Kanal', 'Procent'],
                ['Mäklare', <%=MäklarePie%>],
                ['Bank', <%=BankPie%>],
                ['Kryss', <%=KryssPie%>]
            ]);

            var options = {
                title: 'Kanal',
                colors: colors,
                width: w
            };

            var chart = new google.visualization.PieChart(document.getElementById('chart_div'));
            chart.draw(data, options);

            var data2 = google.visualization.arrayToDataTable([
                ['Produkt', 'Procent'],
                ['Fond', <%=FondPie%>],
                ['Depå', <%=DepåPie%>],
                ['Kryss', <%=KryssPie%>]
            ]);

            var options2 = {
                title: 'Produkt',
                colors: colors,
                width: w
            };

            var chart2 = new google.visualization.PieChart(document.getElementById('chart_div2'));
            chart2.draw(data2, options2);
        }

    </script>
</head>
<body style="font-family: Danske" onload="init();">
    <form id="form1" runat="server">
        <div style="width: 100%">
<%--            <div style="height: 50px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: #123757; text-align: left; font-size: 45px; color: white"><div style="text-align: left; float: left">Premier</div><div style="float: right; width: 10px"></div><div style="float: right"></div></div>
            <div style="height: 10px; background-color: #337BAD"></div>--%>
            <div style="">
                <table id="huvudTabell" style="width: 95%; height: 100px; font-size: 10px">
                    <tr>
                        <td>
                            <table style="background-color: #123757; width: 100%; border-collapse: collapse; border-spacing: 0; padding: 0">
                                <tr>
                                    <td>
                                        <table style="width: 100%; border-collapse: collapse; border-spacing: 0; padding: 0">
                                            <tr>
                                                <td style="height: 50px; text-align: left; font-size: 35px; color: white; background-color: #123757">Premier</td>
                                                <td style="height: 50px; text-align: right; background-color: #123757">
                                                    <img src="img/logga.png" width="120px" alt="logga" /></td>
                                                <td style="height: 50px; width: 10px; background-color: #123757" />
                                            </tr>
                                            <tr style="background-color: #337BAD">
                                                <td colspan="3" style="height: 10px" />
                                            </tr>
<%--                                            <tr>
                                                <td colspan="3" style="height: 10px; background-color: white; color: red; text-align: center; font-size: 25px">Grattis Claes 60år!</td>
                                            </tr>--%>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; vertical-align: central">
                            <table style="width: 100%; text-align: right">
                                <tr>
                                    <td />
                                    <td>Depå</td>
                                    <td>Fond</td>
                                    <td>Summa</td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">Bank</td>
                                    <td>
                                        <asp:Label runat="server" ID="bankDepå"></asp:Label></td>
                                    <td>
                                        <asp:Label runat="server" ID="bankFond"></asp:Label></td>
                                    <td>
                                        <asp:Label runat="server" ID="summaBank"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">Mäklare</td>
                                    <td>
                                        <asp:Label runat="server" ID="mäklareDepå"></asp:Label></td>
                                    <td>
                                        <asp:Label runat="server" ID="mäklareFond"></asp:Label></td>
                                    <td>
                                        <asp:Label runat="server" ID="summaMäklare"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">Kryss</td>
                                    <td/>
                                    <td>
                                        <asp:Label runat="server" ID="kryss"></asp:Label></td>
                                    <td>
                                        <asp:Label runat="server" ID="summaKryss"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">Summa</td>
                                    <td>
                                        <asp:Label runat="server" ID="depå"></asp:Label></td>
                                    <td>
                                        <asp:Label runat="server" ID="fond"></asp:Label></td>
                                    <td>
                                        <asp:Label runat="server" ID="summa"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="height: 30px"></td>
                                </tr>
                                <tr style="font-weight: bold">
                                    <td colspan="3" style="text-align: left">Estimat</td>
                                    <td>
                                        <asp:Label runat="server" ID="estimat"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4"><hr/></td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <table style="width: 100%; border-spacing: 0">
                                            <tr>
                                                <td style="width: 100%">
                                                    <div id="chart_div" style="width: 100%; height: 200px;"></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <table style="width: 100%; border-spacing: 0">
                                            <tr>
                                                <td style="width: 100%">
                                                    <div id="chart_div2" style="width: 100%; height: 200px;"></div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4"><hr/></td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="font-size: 16px; text-align: left" id="stekarrubrik" runat="server">Dagens stekar</td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <table style="width: 100%; border-spacing: 0" id="stekar" runat="server"></table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" id="stekarlinje" runat="server"><hr/></td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="font-size: 14px"><%=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") %></td>
                                </tr>
                                <tr style="visibility: hidden">
                                    <td colspan="4">
                                        <table style="width: 100%; border-spacing: 0">
                                            <tr>
                                                <td id="volymDepå" runat="server" style="text-align: center; background-color: lightgreen"></td>
                                                <td id="volymFond" runat="server" style="text-align: center; background-color: lightskyblue"></td>
                                                <td id="volymKryss" runat="server" style="text-align: center; background-color: burlywood"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
