﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="VaktLoggDatum.ascx.cs" Inherits="BBS.VaktLoggDatum"  EnableViewState="False" %>
<%@ Import Namespace="MarkusModel" %>
<table width="100%" cellpadding="30px">
    <tr>
        <td style="width: 100%">
            <table style="font-family:Verdana; font-size:smaller">
                <tr>
                    <td>
                        <a href="Default.aspx?sida=medlemssida&id=<%=Request.QueryString["id"]%>"><%=((Medlem)Cache[Request.QueryString["id"]]).Namn%></a> / <a href="Default.aspx?sida=vakt&id=<%=Request.QueryString["id"]%>&month=<%=Request.QueryString["month"]%>">Vaktöversikt</a> / <%=Request.QueryString["datum"]%>
                    </td>
                </tr>
                <tr>
                    <td style="height:30px"/>
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
