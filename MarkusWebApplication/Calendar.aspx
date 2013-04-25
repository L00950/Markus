<%@ Page language="c#" Inherits="MarkusWebApplication.WebForm1" CodeFile="Calendar.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Administrera bokningar</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link rel="stylesheet" type="text/css" href="StyleSheet.css" />
		<script type="text/javascript">
		function OPEN_WIN(url,name,width,height,settings)
		{
			win_setup ="toolbar=no,location=no,titlebar=no,directories=no,status=no,menubar=no";
			win_setup+=",resizable=yes,copyhistory=no,";
			win_setup+=settings;
			win_setup+=",width="+width;
			win_setup+=",height="+height;
			return window.open(url, name, win_setup);
		}
        </script>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<table width="95%">
				<tr id="loginrow" runat="server" Visible="true">
					<td>
						<P><span style="FONT-SIZE: 14pt; FONT-FAMILY: Verdana">Login</span><br>
							<table height="2" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<tr>
									<td bgColor="#333399"></td>
								</tr>
							</table>
						</P>
						<table>
							<tr>
								<td>Username:
								</td>
							    <td><input id="username" style="WIDTH: 60px" type="text" runat="server" autocomplete="off"/>
									<DIV id="loginmessage" style="DISPLAY: inline; FONT-WEIGHT: bold; FONT-SIZE: 10pt; WIDTH: 400px; COLOR: red; FONT-FAMILY: Verdana; HEIGHT: 16px"
										runat="server"></DIV>
									&nbsp;
								</td>
							</tr>
							<tr>
								<td>Password:
								</td>
								<td><input id="password" style="WIDTH: 60px" type="password" runat="server" />
								</td>
							</tr>
							<tr>
								<td><input id="login" type="button" value="Login" runat="server" onserverclick="LoginServerClick" />
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr id="overviewrow" runat="server" Visible="false">
					<td>
						<P><span style="FONT-SIZE: 14pt; FONT-FAMILY: Verdana">Översikt</span><br>
							<table height="2" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<tr>
									<td bgColor="#333399"></td>
								</tr>
							</table>
						</P>
						<P><input id="lillahuset" type="button" value="Lilla Huset" runat="server" onserverclick="LillahusetServerClick"/>&nbsp;<input id="ettvaan" type="button" value="Ett Tvåan" runat="server" onserverclick="EttvaanServerClick"/>&nbsp;<input id="hebbes" type="button" value="Hebbes" runat="server" onserverclick="HebbesServerClick"/>&nbsp;
						<input id="MonteRojo" type="button" value="Monte Rojo" runat="server" onserverclick="MonterojoServerClick"/>&nbsp;&nbsp;<input id="säkerhetskopiera" type="button" value="Säkerhetskopiera" runat="server" onserverclick="SäkerhetskopieraServerClick"/>&nbsp;&nbsp;<input id="logout" type="button" value="Logga ut" runat="server" onserverclick="LogoutServerClick"/>
						</P>
                        <p>
                            Antal bokningar:<asp:Label ID="antalBokningar" runat="server"></asp:Label><br />
                            Datafil:<asp:Label ID="dataFil" runat="server"></asp:Label><br />
                            Storlek:<asp:Label ID="filStorlek" runat="server"></asp:Label><br />
                            Senast sparad:<asp:Label ID="senastSparad" runat="server"></asp:Label><br />
                            <asp:Label ID="statusText" runat="server"></asp:Label>
                        </p>
					</td>
				</tr>
				<tr id="newbookingrow" runat="server" Visible="false">
					<td>
						<p><span style="FONT-SIZE: 14pt; FONT-FAMILY: Verdana"><asp:Label ID="newbookinglabel" Runat="server">Ny bokning</asp:Label></span><br>
							<table height="2" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<tr>
									<td bgColor="#333399"></td>
								</tr>
							</table>
						</p>
						<table>
							<tr>
								<td>ID:
								</td>
								<td><input id="id" style="WIDTH: 40px" disabled type="text" runat="server" />
								</td>
							</tr>
							<tr>
								<td>Namn:
								</td>
								<td><input id="person" style="WIDTH: 200px" type="text" name="person" runat="server" />
								</td>
							</tr>
							<tr>
								<td>Anmärkning:
								</td>
								<td><input id="note" style="WIDTH: 200px" type="text" name="note" runat="server" />
								</td>
							</tr>
							<tr>
								<td>Period:
								</td>
								<td>&nbsp; Från
									<asp:calendar id="CalendarStart" runat="server" Font-Names="Verdana" 
                                        Font-Size="9pt" Height="200px"
										Width="230px" NextPrevFormat="ShortMonth" ForeColor="Black" BorderColor="Black" BorderStyle="Solid"
										CellSpacing="1" BackColor="White" onselectionchanged="CalendarStartSelectionChanged">
										<TodayDayStyle ForeColor="White" BackColor="#999999"></TodayDayStyle>
										<DayStyle BackColor="#CCCCCC"></DayStyle>
										<NextPrevStyle Font-Size="8pt" Font-Bold="True" ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Size="8pt" Font-Bold="True" Height="8pt" ForeColor="#333333"></DayHeaderStyle>
										<SelectedDayStyle ForeColor="White" BackColor="#333399"></SelectedDayStyle>
										<TitleStyle Font-Size="12pt" Font-Bold="True" Height="12pt" ForeColor="White" BackColor="#333399"></TitleStyle>
										<OtherMonthDayStyle ForeColor="#999999"></OtherMonthDayStyle>
									</asp:calendar></td>
								<td>&nbsp; Till
									<asp:calendar id="CalendarEnd" runat="server" Font-Names="Verdana" 
                                        Font-Size="9pt" Height="200px"
										Width="230px" NextPrevFormat="ShortMonth" ForeColor="Black" BorderColor="Black" BorderStyle="Solid"
										CellSpacing="1" BackColor="White" onselectionchanged="CalendarEndSelectionChanged">
										<TodayDayStyle ForeColor="White" BackColor="#999999"></TodayDayStyle>
										<DayStyle BackColor="#CCCCCC"></DayStyle>
										<NextPrevStyle Font-Size="8pt" Font-Bold="True" ForeColor="White"></NextPrevStyle>
										<DayHeaderStyle Font-Size="8pt" Font-Bold="True" Height="8pt" ForeColor="#333333"></DayHeaderStyle>
										<SelectedDayStyle ForeColor="White" BackColor="#333399"></SelectedDayStyle>
										<TitleStyle Font-Size="12pt" Font-Bold="True" Height="12pt" ForeColor="White" BackColor="#333399"></TitleStyle>
										<OtherMonthDayStyle ForeColor="#999999"></OtherMonthDayStyle>
									</asp:calendar></td>
							</tr>
						</table>
						<P>
                        <input id="savebutton" type="button" value="Spara" runat="server" onserverclick="SavebuttonServerClick"/>
                        <asp:Button id="deletebutton" Text="Ta Bort" runat="server" OnClick="DeletebuttonServerClick" />
                        <input id="cancelbutton" type="button" value="Avbryt" runat="server" onserverclick="CancelbuttonServerClick"/>
                        <br/>
						</P>
					</td>
				</tr>
				<tr id="bookingsrow" runat="server" Visibe="false">
					<td>
						<P><span id="bokningar" style="FONT-SIZE: 14pt; FONT-FAMILY: Verdana" runat="server">Bokningar</span><br>
							<table height="2" cellSpacing="0" cellPadding="0" width="100%" border="0">
								<tr>
									<td bgColor="#333399"></td>
								</tr>
							</table>
						</P>
					    <P><input id="newbutton" type="button" value="Ny bokning" runat="server" onserverclick="NewbuttonServerClick"/><input id="see2006" type="button" value="Se 2013" runat="server"/><input id="see2007" type="button" value="Se 2014" runat="server"/><input id="goback" type="button" value="Översikt" runat="server" onserverclick="GobackServerClick"/></P>
						<asp:datagrid id="bookings" runat="server" BorderColor="White" BorderStyle="Ridge" CellSpacing="1"
							BackColor="White" GridLines="None" CellPadding="3" BorderWidth="2px" AutoGenerateColumns="False">
							<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#9471DE"></SelectedItemStyle>
							<ItemStyle ForeColor="Black" BackColor="#DEDFDE"></ItemStyle>
							<HeaderStyle Font-Bold="True" ForeColor="White" BackColor="#333399"></HeaderStyle>
							<FooterStyle ForeColor="Black" BackColor="#C6C3C6"></FooterStyle>
							<Columns>
								<asp:BoundColumn DataField="id" HeaderText="ID"></asp:BoundColumn>
								<asp:BoundColumn DataField="ankomst" HeaderText="Fr&#229;n" 
                                    DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
								<asp:BoundColumn DataField="avresa" HeaderText="Till" 
                                    DataFormatString="{0:yyyy-MM-dd}"></asp:BoundColumn>
								<asp:BoundColumn DataField="Person" HeaderText="Namn"></asp:BoundColumn>
								<asp:BoundColumn DataField="kommentar" HeaderText="Anm&#228;rkning"></asp:BoundColumn>
								<asp:EditCommandColumn ButtonType="PushButton" EditText="Ändra" UpdateText="Update" 
                                    CancelText="Cancel" HeaderStyle-HorizontalAlign="Center">
                                </asp:EditCommandColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Right" ForeColor="Black" BackColor="#C6C3C6"></PagerStyle>
						</asp:datagrid>
					</td>
				</tr>
				<tr>
					<td>
					    <span style="FONT-SIZE: 12px">Powered by Markus Linderbäck</span>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
