<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AllaMedlemmar.ascx.cs" Inherits="BBS.AllaMedlemmar"  EnableViewState="False" %>
<table width="100%" cellpadding="30px">
    <tr>
        <td style="width: 100%">
            <table style="font-family:Verdana; font-size:smaller">
                <tr>
                    <td colspan="2">
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="tillbakaKnapp" runat="server" Text="Tillbaka" 
                                        style="font-family:Verdana; font-size:small" onclick="TillbakaClick"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td height="30px">
                        <h3>Alla medlemmar</h3>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DataGrid runat="server" ID="lista" HeaderStyle="font-size:large" CellSpacing="10">
                            <HeaderStyle BackColor="Silver" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Size="Large" Font-Strikeout="False" Font-Underline="False" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Size="X-Small" Font-Strikeout="False" Font-Underline="False" />
                        </asp:DataGrid>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
