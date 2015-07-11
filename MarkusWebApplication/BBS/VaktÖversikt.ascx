<%@ Control Language="C#" AutoEventWireup="true" CodeFile="VaktÖversikt.ascx.cs" Inherits="BBS.VaktÖversikt" EnableViewState="false" %>
<%@ Import Namespace="MarkusModel" %>
<table width="100%" cellpadding="30px">
    <tr>
        <td width="100%" style="font-family:verdana; font-size:small">
            <table style="width: 100%">
                <tr>
                    <td>
                        <a href="Default.aspx?sida=medlemssida&id=<%=Request.QueryString["id"]%>"><%=((Medlem)Cache[Request.QueryString["id"]]).Namn%></a> / Vaktöversikt
                    </td>
                </tr>
                <tr>
                    <td height="30px"></td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList runat="server" ID="månad" AutoPostBack="True" OnSelectedIndexChanged="månad_SelectedIndexChanged">
                            <asp:ListItem Text="April" Value="4"/>
                            <asp:ListItem Text="Maj" Value="5"/>
                            <asp:ListItem Text="Juni" Value="6"/>
                            <asp:ListItem Text="Juli" Value="7"/>
                            <asp:ListItem Text="Augusti" Value="8"/>
                            <asp:ListItem Text="September" Value="9"/>
                            <asp:ListItem Text="Oktober" Value="10"/>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td height="20px"/>
                </tr>
                <tr>
                    <td>
                        <asp:Table ID="tabell" runat="server" width="100%" style="font-family:verdana; font-size:small"/>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
