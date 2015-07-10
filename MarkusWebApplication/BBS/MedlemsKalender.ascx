<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MedlemsKalender.ascx.cs" Inherits="BBS.MedlemsKalender" %>
    <table width="100%" cellpadding="30px">
        <tr>
            <td width="100%" style="font-family:verdana; font-size:small">
                <table>
                    <tr>
                        <td>
                            Här kan du markera när din bryggplats <b><%=BryggplatsId()%></b> är ledig. Med ledig anses från kl 12 den markerade dagen till klockan 12 dagen efter. Ex 1 juli avser perioden 1 juli kl 12:00 till 2 juli kl 12:00.
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
                                        <asp:Button ID="sparaKnapp" runat="server" Text="Spara" 
                                            style="font-family:Verdana; font-size:small" OnClick="SparaKnappClick"/>
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