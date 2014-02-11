<%@LANGUAGE=VBSCRIPT%>
<%
dim strServerName, strRemoteAddress, strServerPort
strServerName = Request.ServerVariables("SERVER_NAME")
strRemoteAddress = Request.ServerVariables("REMOTE_ADDR")
strServerPort = Request.ServerVariables("SERVER_PORT")
%>
<HTML>
<HEAD>
<META NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<meta http-equiv="pragma" content="no-cache">
<META NAME="Description" CONTENT="Hus i Ryda">
<META NAME="Keywords" CONTENT="stuga, fisk, fiske, ryda, hus, östergötland">
<TITLE>
<%
if(InStr(1, strServerName, "linderback.com", vbTextCompare)>0) then
	Response.Write("www.linderback.com")
else
	Response.Write("www.lyxbo.com")
end if
%>
</TITLE>

<script language=javascript>
</script>

</HEAD>

<%
if(InStr(1, strServerName, "varmt.com", vbTextCompare)>0) then
	Response.Write( _
	"<frameset rows=""0,*"" border=0 noresize=0 scroll=0>" & _
	"<frame src=""/varme/blankwhite.htm"" scrolling=no>" & _
	"<frame src=""/varme/default.htm"" scrolling=no>" & _
	"</frameset>")
elseif(InStr(1, strServerName, "batterier.se", vbTextCompare)>0) then
	Response.Write( _
	"<frameset rows=""0,*"" border=0 noresize=0 scroll=0>" & _
	"<frame src=""/batterier/blankwhite.htm"" scrolling=no>" & _
	"<frame src=""/batterier/default.htm"" scrolling=no>" & _
	"</frameset>")
elseif(InStr(1, strServerName, "linderback.com", vbTextCompare)>0) then
    Response.Redirect("linderback/default.htm")
elseif(InStr(1, strServerName, "l.com", vbTextCompare)>0) then
	Response.Write( _
	"<frameset rows=""0,*"" border=0 noresize=0 scroll=0>" & _
	"<frame src=""/varme/blankwhite.htm"" scrolling=no>" & _
	"<frame src=""/linderback/uthyrning.asp"" scrolling=no>" & _
	"</frameset>")
elseif(InStr(1, strServerName, "lyxbo.com", vbTextCompare)>0) then
	Response.Write( _
	"<frameset rows=""0,*"" border=0 noresize=0 scroll=0>" & _
	"<frame src=""/varme/blankwhite.htm"" scrolling=no>" & _
	"<frame src=""lyxbo/uthyrning.asp"" scrolling=no>" & _
	"</frameset>")
else
	Response.Write("<BODY bgcolor=white>" & _
	"<b>Välkommen till Olssons Server</b><br><br>" & _
	"Din IP-adress: " + strRemoteAddress + "<br>" & _
	"Servern du efterfrågar: " + strServerName + "<br>" & _
	"Port: " + strServerPort + "<br>" & _
	"Tid: " + CStr(Now()) + "</BODY>")
end if
%>

</HTML>
