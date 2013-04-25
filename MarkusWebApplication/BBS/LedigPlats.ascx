<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LedigPlats.ascx.cs" Inherits="BBS.LedigPlats" %>
<form id="form1">
    <table width="100%" cellpadding="30px">
        <tr>
            <td width="100%" style="font-family:verdana; font-size:small">
                <table>
                    <tr>
                        <td>
                            Följande bryggplatser är lediga <b><%=Convert.ToDateTime(Request.QueryString["datum"]).ToString("yyyy-MM-dd")%></b>.
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
                                    <asp:TableCell>Plats</asp:TableCell>
                                    <asp:TableCell>Namn</asp:TableCell>
                                    <asp:TableCell>Tel</asp:TableCell>
                                    <asp:TableCell>Mobil</asp:TableCell>
                                    <asp:TableCell>E-mail</asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</form>