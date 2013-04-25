<%@LANGUAGE=VBSCRIPT%>
<%
dim obj
set obj = Server.CreateObject("W3Proj.VarmeBO")
%>
<HTML>
<HEAD>
<META NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
<meta http-equiv="pragma" content="no-cache">
<TITLE>Statistik linderback.com</TITLE>
</HEAD>
<link rel="STYLESHEET" type="text/css" href="bodyWhite.css">
<BODY style="FONT-FAMILY:century gothic;FONT-SIZE: 12px;">
<b>Alla nya träffar</b>
<%
obj.ListHits
%>
<p>
<b>Alla träffar</b>
<%
obj.ListAllHits
%>
</BODY>
</HTML>
