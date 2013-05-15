<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Danica.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Danica Pension</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 100%">
            <tr>
                <td style="text-align: center">
                    Lösenord
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:TextBox ID="lösenord" runat="server"/>
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:Button ID="okButton" runat="server" Text="OK" OnClick="okButton_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
