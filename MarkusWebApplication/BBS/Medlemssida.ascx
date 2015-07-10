<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Medlemssida.ascx.cs" Inherits="BBS.Medlemssida" EnableViewState="false" %>
<%@ Import Namespace="MarkusModel" %>
<table width="100%" cellpadding="30px">
    <tr>
        <td style="width: 100%">
            <table style="font-family:Verdana; font-size:smaller">
                <tr>
                    <td colspan="2">
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="medlemmar" runat="server" Text="Medlemmar" 
                                        style="font-family:Verdana; font-size:small" OnClick="MedlemmarClick"/>
                                </td>
                                <td style="width: 5px"/>
                                <td>
                                    <asp:Button ID="ledigaPlatserKnapp" runat="server" Text="Lediga Platser" 
                                        style="font-family:Verdana; font-size:small" OnClick="LedigaPlatserKnappClick"/>
                                </td>
                                <td style="width: 5px"/>
                                <td>
                                    <asp:Button ID="vaktöversiktKnapp" runat="server" Text="Vaktgång" 
                                        style="font-family:Verdana; font-size:small" OnClick="vaktöversiktKnapp_Click"/>
                                </td>
                                <td style="width: 5px"/>
                                <td>
                                    <asp:Button ID="loggaUtKnapp" runat="server" Text="Logga ut" 
                                        style="font-family:Verdana; font-size:small" onclick="LoggaUtKnappClick"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td height="30px">
                        <h3>Mina uppgifter</h3>
                    </td>
                    <td style="font-size: x-small">
                        (Ändringar mailas till sekr@bbslidingo.se)
                    </td>
                </tr>
                <tr>
                    <td>
                        Namn:
                    </td>
                    <td>
                        <asp:Label ID="namn" runat="server" Width="200px"><%=((Medlem)Cache[Request.QueryString["id"]]).Namn%></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Adress:
                    </td>
                    <td>
                        <asp:Label ID="adress" runat="server" Width="200px"><%=((Medlem)Cache[Request.QueryString["id"]]).Gata%></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Ort:
                    </td>
                    <td>
                        <asp:Label ID="postnr" runat="server" Width="50px"><%=((Medlem)Cache[Request.QueryString["id"]]).PostNummer%></asp:Label>
                        <asp:Label ID="ort" runat="server" Width="150px"><%=((Medlem)Cache[Request.QueryString["id"]]).Ort%></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        E-post:
                    </td>
                    <td>
                        <asp:Label ID="email" runat="server" Width="200px"><%=((Medlem)Cache[Request.QueryString["id"]]).Email%></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Tel:
                    </td>
                    <td>
                        <asp:Label ID="tel" runat="server" Width="200px"><%=((Medlem)Cache[Request.QueryString["id"]]).Tel%></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Mobil:
                    </td>
                    <td>
                        <asp:Label ID="mobil" runat="server" Width="200px"><%=((Medlem)Cache[Request.QueryString["id"]]).Mobil%></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="height: 10px"></td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 100%"><hr/></td>
                </tr>
                <tr>
                    <td style="height: 10px"><h3>Mina bryggplatser</h3></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Table style="width: 100%" runat="server" ID="bryggplatsLista">
                        </asp:Table>
                    </td>
                </tr>
                <tr>
                    <td style="height: 10px"></td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 100%"><hr/></td>
                </tr>
                <tr>
                    <td style="height: 20px"><h3>Mitt lösenord</h3></td>
                </tr>
                <tr>
                    <td>
                        Nytt lösenord:
                    </td>
                    <td>
                        <asp:TextBox ID="nyttLösenord" runat="server" Width="100px" AutoCompleteType="None"></asp:TextBox>
                        <asp:Label ID="feltext" ForeColor="red" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Repetera:
                    </td>
                    <td>
                        <asp:TextBox ID="nyttLösenordIgen" runat="server" Width="100px" AutoCompleteType="None"></asp:TextBox>
                        <asp:Button ID="sparaKnapp" runat="server" Text="Spara" 
                                        style="font-family:Verdana; font-size:small" OnClick="SparaKnappClick"/>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
