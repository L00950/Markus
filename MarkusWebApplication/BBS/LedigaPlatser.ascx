<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LedigaPlatser.ascx.cs" Inherits="BBS.LedigaPlatser" EnableViewState="false" %>
    <table width="100%" cellpadding="30px">
        <tr>
            <td width="100%" style="font-family:verdana; font-size:small">
                <table style="width: 100%">
                    <tr>
                        <td>
                            Kalender över lediga platser i hamnen. Om det finns lediga platser så kan du trycka på siffran för att se vilka platser som är lediga.
                        </td>
                    </tr>
                    <tr>
                        <td height="30px"></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="tillbakaKnapp" runat="server" Text="Tillbaka" 
                                            style="font-family:Verdana; font-size:small" OnClick="TillbakaKnappClick"/>
                                    </td>
                                    <td style="width: 5px"/>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td height="20px"/>
                    </tr>
                    <tr>
                        <td>
                            <asp:Table ID="tabell" runat="server" width="100%" style="font-family:verdana; font-size:small">
                                <asp:TableRow style="font-size:medium; font-weight:bold">
                                    <asp:TableCell>Maj</asp:TableCell>
                                    <asp:TableCell>Juni</asp:TableCell>
                                    <asp:TableCell>Juli</asp:TableCell>
                                    <asp:TableCell>Augusti</asp:TableCell>
                                    <asp:TableCell>September</asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>