<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Statistik.aspx.cs" Inherits="Danica.Statistik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Danica Pension - Statistik</title>
    <script>
        var minuter = 5;
        var milliSekunder = minuter * 60 * 1000;
        function init() {
            setTimeout(reLoad, milliSekunder);
        }

        function reLoad() {
            window.location = "statistik.aspx";
        }
    </script>
</head>
<body style="font-family: verdana" onload="init();">
    <form id="form1" runat="server">
        <div>
            <div style="height: 50px; border-top-left-radius: 5px; border-top-right-radius: 5px; background-color: navy; text-align: center; font-size: 40px; color: white">Danica Pension - <%=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") %></div>
            <div style="height: 10px; background-color: lightblue"></div>
            <div style="">
                <table style="width: 100%; height: 100px; font-size: 50px">
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
                                                <td id="depåBar" runat="server" style="text-align: center; background-color: lightgreen"></td>
                                                <td id="fondBar" runat="server" style="text-align: center; background-color: lightskyblue"></td>
                                                <td id="kryssBar" runat="server" style="text-align: center; background-color: burlywood"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <table style="width: 100%; border-spacing: 0">
                                            <tr>
                                                <td id="bankBar" runat="server" style="text-align: center; background-color: lightgreen"></td>
                                                <td id="mäklarBar" runat="server" style="text-align: center; background-color: lightskyblue"></td>
                                                <td id="kryssBar2" runat="server" style="text-align: center; background-color: burlywood"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td></td>
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
