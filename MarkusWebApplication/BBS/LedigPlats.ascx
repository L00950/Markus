<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LedigPlats.ascx.cs" Inherits="BBS.LedigPlats" %>
<%@ Import Namespace="MarkusModel" %>
<table width="100%" cellpadding="30px">
        <tr>
            <td width="100%" style="font-family:verdana; font-size:small">
                <table>
                    <tr>
                        <td>
                            <a href="Default.aspx?sida=medlemssida&id=<%=Request.QueryString["id"]%>"><%=((Medlem)Cache[Request.QueryString["id"]]).Namn%></a> / <a href="Default.aspx?sida=ledigaplatser&id=<%=Request.QueryString["id"]%>">Lediga platser</a> / <%=Request.QueryString["datum"]%>
                        </td>
                    </tr>
                    <tr>
                        <td style="height:30px"/>
                    </tr>
                    <tr>
                        <td>
                            Följande bryggplatser är lediga <b><%=Convert.ToDateTime(Request.QueryString["datum"]).ToString("yyyy-MM-dd")%></b>.<br />
                            Kontakta gärna platsägaren för att dubbelkolla för säkerhets skull.
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