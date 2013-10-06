<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Larm.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Larm Barrstigen</title>
</head>
<body style="background-color: skyblue">
    <form id="form1" runat="server">
    <div>
        <table style="width: 100%; font-size: 30px; font-family: verdana">
            <tr>
                <td style="height: 100px; text-align: center; font-size: 40px"><asp:Label runat="server" ID="Meddelandetext" ForeColor="red"></asp:Label></td>
            </tr>
            <tr>
                <td style="text-align: center; width: 100%">
                    <asp:Button runat="server" id="Uppdatera" style="height: 100px; width: 90%; font-size: 30px; text-align: center" Text="Uppdatera" OnClick="Uppdatera_Click"/>
                </td>
            </tr>
            <tr>
                <td style="height: 100px"></td>
            </tr>
            <tr>
                <td style="text-align: center; width: 100%">
                    <asp:Button runat="server" id="Aktivera" style="height: 100px; width: 90%; font-size: 30px; text-align: center" Text="PÅ" OnClick="Aktivera_Click"/>
                </td>
            </tr>
            <tr>
                <td style="height: 40px"></td>
            </tr>
            <tr>
                <td style="text-align: center; width: 100%">
                    <asp:Button runat="server" id="Avaktivera" style="height: 100px; width: 90%; font-size: 30px; text-align: center" Text="AV" OnClick="Avaktivera_Click"/>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
