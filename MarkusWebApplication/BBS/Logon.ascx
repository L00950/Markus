<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Logon.ascx.cs" Inherits="BBS.Logon" %>
<table width="100%">
    <tr>
        <td width="100%" height="300px" align="center" valign="middle">
            <table style="font-family:Verdana; font-size:smaller">
                <tr>
                    <td>
                        E-post:
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox ID="användare" runat="server" Width="200px" AutoCompleteType="None"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Lösenord:
                    </td>
                    <td>
                        <asp:TextBox ID="lösenord" TextMode="Password" Width="200px" runat="server" AutoCompleteType="None"></asp:TextBox>
                        <asp:Button ID="nyttLösenKnapp" runat="server" Text="Glömt lösen" 
                            style="font-family:Verdana; font-size:small" OnClick="NyttLösenKnappClick" />
                    </td>
                </tr>
                <tr>
                    <td height="30px"></td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="loggaInKnapp" runat="server" Text="Logga in" 
                            style="font-family:Verdana; font-size:small" onclick="LoggaInKnappClick" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
