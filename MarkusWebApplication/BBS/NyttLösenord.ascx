<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NyttLösenord.ascx.cs" Inherits="BBS.NyttLösenord" %>
<table width="100%">
    <tr>
        <td width="100%" height="300px" align="center" valign="middle">
            <table style="font-family:Verdana; font-size:smaller">
                <tr>
                    <td colspan="2" style="height: 30px;text-align: center">Beställ nytt lösenord</td>
                </tr>
                <tr>
                    <td>
                        E-mail:
                    </td>
                    <td>
                        <asp:TextBox ID="email" runat="server" Width="200px" AutoCompleteType="None"></asp:TextBox>
                        <asp:Label ID="infoText" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td height="30px"></td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center">
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: right">
                                    <asp:Button ID="skickaKnapp" runat="server" Text="Skicka" 
                                        style="font-family:Verdana; font-size:small" onclick="SkickaClick" />
                                </td>
                                <td style="width: 5px"/>
                                <td style="text-align: left">
                                    <asp:Button ID="tillbakaKnapp" runat="server" Text="Tillbaka" 
                                        style="font-family:Verdana; font-size:small" OnClick="TillbakaKnappClick" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
