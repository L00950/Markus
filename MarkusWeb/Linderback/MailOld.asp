<%@LANGUAGE=VBSCRIPT%>
<%
dim obj
set obj = Server.CreateObject("W3Proj.VarmeBO")
if(len(Request.QueryString("send")))then
	obj.SendMail
	Response.Redirect("mailsend.htm")
end if
%>
<html>
<head>
<title>Send mail</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
<STYLE>
table, body
{
    FONT-FAMILY: Verdana;
    FONT-SIZE: 10pt;
}
td
{
	TEXT-ALIGN: left;
}
</STYLE>
<script language="javascript">
function clickSend()
{
	if(form1.namn.value=="")
		alert("Please, enter your name!");
	else if(form1.mail.value=="")
		alert("Please, enter your mail!");
	else if(form1.message.value=="")
		alert("Please, enter your message!");
	else
		form1.submit();
}
</script>
</head>

<body>
<form name="form1" id="form1" action="mail.asp?send=true" method="post" onSubmit="return kollaformularet (this);">
  
<input type="hidden" name="receiver" value="info@linderback.com">
<input type="hidden" name="subject" value="Lilla Huset">
<table width="397">
      <tr> 
        <td>
		  Name:
		</td>
        <td> 
          <input type ="text" size=41 name="namn">
        </td>
      </tr>
      <tr> 
        <td>
		  Address:
		</td>
        <td> 
          <textarea name="address" rows=4 cols=31></textarea>
        </td>
      </tr>
      <tr> 
        <td>
		  Tel:
		</td>
        <td> 
          <input type ="text" size=41 name="phone">
        </td>
      </tr>
      <tr> 
        <td>
		  Email:
		</td>
        <td> 
          <input type ="text" size=41 name="mail">
        </td>
      </tr>
      <tr> 
        <td>
		  Message:
		</td>
        <td> 
          <textarea name="message" rows=4 cols=31></textarea>
        </td>
      </tr>
      <tr> 
        <td></td>
        <td> 
          <input type="button" value="Send" onClick="clickSend();">
          <input type="reset" value="Clear">
        </td>
      </tr>
    </table>
</form>

</body>
</html>
