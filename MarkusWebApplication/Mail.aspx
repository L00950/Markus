<%@ Page language="c#" Inherits="MarkusWebApplication.Mail" CodeFile="Mail.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Send mail</title>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<STYLE> 
			TABLE { FONT-SIZE: 10pt; FONT-FAMILY: Verdana } 
			BODY { FONT-SIZE: 10pt; FONT-FAMILY: Verdana } 
			TD { TEXT-ALIGN: left } 
			</STYLE>
	</HEAD>
	<body>
		<form id="form1" runat="server">
		    <input type="hidden" name="receiver" value="info@linderback.com" id="receiver" runat="server"/>
		    <input type="hidden" name="subject" value="Lilla Huset" id="subject" runat="server"/>
			<table width="397">
				<tr>
					<td>
						Name:
					</td>
					<td>
					    <input type="text" size="41" name="namn" id="name" runat="server"/>
					</td>
				</tr>
				<tr>
					<td>
						Address:
					</td>
					<td>
						<textarea name="address" rows="4" cols="31" id="address" runat="server"></textarea>
					</td>
				</tr>
				<tr>
					<td>
						Tel:
					</td>
					<td>
					    <input type="text" size="41" name="phone" id="phone" runat="server"/>
					</td>
				</tr>
				<tr>
					<td>
						Email:
					</td>
					<td>
					    <input type="text" size="41" name="mail" id="email" runat="server"/>
					</td>
				</tr>
				<tr>
					<td>
						Message:
					</td>
					<td>
						<textarea name="message" rows="4" cols="31" id="message" runat="server"></textarea>
					</td>
				</tr>
				<tr>
					<td></td>
					<td>
					    <input type="button" value="Send" id="sendbutton" name="sendbutton" runat="server" onserverclick="sendbutton_ServerClick"/>
					    <input type="reset" value="Clear"/>
					</td>
				</tr>
			</table>
			<asp:Label ID="errormessage" Runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
		</form>
	</body>
</HTML>
