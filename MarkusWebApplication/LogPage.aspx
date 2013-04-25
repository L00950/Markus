<%@ Page language="c#" Inherits="MarkusWebApplication.LogPage" CodeFile="LogPage.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>LogPage</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body style='background:#FBF5EC'>
		<form id="Form1" method="post" runat="server">
			<table width="95%" style="FONT-SIZE: 10pt; FONT-FAMILY: Verdana">
				<tr>
					<td width="100%">
						<asp:TextBox ID="log" Rows="20" Width="400" AutoPostBack="False" ReadOnly="True" TextMode="MultiLine"
							Runat="server" style="FONT-SIZE: 10pt; FONT-FAMILY: Verdana"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td height="20" width="50">
					</td>
				</tr>
				<tr>
					<td>
						Meddelande<br>
						<asp:TextBox ID="messsage" rows="3" Width="400" Runat="server" TextMode="MultiLine" style="FONT-SIZE: 10pt; FONT-FAMILY: Verdana"></asp:TextBox>
					</td>
				</tr>
				<TR>
					<TD>Namn<br>
						<asp:TextBox ID="name" rows="3" Width="150" Runat="server" style="FONT-SIZE: 10pt; FONT-FAMILY: Verdana"></asp:TextBox>
						<asp:Button ID="send" Text="Skicka" Runat="server" style="FONT-SIZE: 10pt; FONT-FAMILY: Verdana" onclick="send_Click"></asp:Button></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
