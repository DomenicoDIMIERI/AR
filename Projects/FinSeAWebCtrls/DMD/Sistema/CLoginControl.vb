Imports DMD
Imports DMD.Sistema
Imports DMD.Forms
Imports DMD.WebSite
Imports DMD.Databases
Imports DMD.Forms.Utils


Imports DMD.Anagrafica

Namespace Forms
 
 

    '-----------------------------------------------------
    ' CLoginControl
    '-----------------------------------------------------
    Public Class CLoginControl
        Inherits WebControl

        Public Sub New()
            Me.BackColor = "#E6E6E6"
            ' Me.Dock = DockType.DOCK_FILL
        End Sub

        Private Sub GetLoginHTML(ByVal writer As System.Web.UI.HtmlTextWriter)

            writer.Write("<script language=""javascript"" type=""text/javascript"">")
            writer.Write(vbNewLine)
            writer.Write("var _")
            writer.Write(Me.Name)
            writer.Write(" = new CLoginControl(""")
            writer.Write(Me.Name)
            writer.Write(""");")
            writer.Write(vbNewLine)
            writer.Write("Window.addListener(""onload"", new Function(""setTimeout('_")
            writer.Write(Me.Name & ".init();', 100)""));")
            writer.Write(vbNewLine)
            writer.Write("</script>")

        End Sub

        'Public Sub GetPwdForgot(ByVal writer As HTMLWriter)
        '    Select Case LCase(Trim(Me.GetParameter(renderer, "_s", vbNullString)))
        '        Case "1"
        '            Try
        '                SystemUtils.SendPasswordsToEMail(Me.GetParameter(renderer, "txtEMail", vbNullString))
        '                writer.WriteRowData("<h1>Procedura di recupero password</h1>")
        '                writer.WriteRowData("</br>")
        '                writer.WriteRowData("Le tue password sono state inviate all'indirizzo specificato.<br/>")
        '            Catch ex As Exception
        '                writer.WriteRowData("<h1>Procedura di recupero password</h1>")
        '                writer.WriteRowData("</br>")
        '                writer.WriteRowData("Si è verificato un errore durante l'elaborazione della richiesta:<br/>")
        '                writer.WriteRowData("<i>" & Strings.HtmlEncode(ex.Message) & "</i><br/>")
        '                writer.WriteRowData("<span class=""font8pt"" style=""color:gray;"">" & Strings.HtmlEncode(ex.StackTrace) & "</span><br/>")
        '            End Try

        '            writer.WriteRowData("<a href='/'>Torna indietro</a>")
        '        Case Else
        '            writer.WriteRowData("<h1>Procedura di recupero password</h1>")
        '            writer.WriteRowData("</br>")
        '            writer.WriteRowData("<table border=""0"" cellspacing=""1"" cellpadding=""0"" style=""border-collapse:collapse;"">")
        '            writer.WriteRowData("<tr>")
        '            writer.WriteRowData("<td colspan=""2""><br />Inserisci la tua e-mail e clicca su <b>Inviami i miei dati...</b></td>")
        '            writer.WriteRowData("</tr>")
        '            writer.WriteRowData("<tr>")
        '            writer.WriteRowData("<td colspan=""2"">&nbsp;</td>")
        '            writer.WriteRowData("</tr>")
        '            writer.WriteRowData("<tr>")
        '            writer.WriteRowData("<td style=""width:80px;"">e-mail:</td>")
        '            writer.WriteRowData("<td style=""width:300px;""><input type=""text"" name=""txtEMail"" id=""txtEMail"" class=""textfield"" style=""width:100%;"" /></td>")
        '            writer.WriteRowData("</tr>")
        '            writer.WriteRowData("<tr>")
        '            writer.WriteRowData("<td colspan=""2"" style=""text-align:right;""><input type=""button"" name=""btnSubmit"" value=""INVIAMI I MIEI DATI..."" class=""btnfield"" onclick=""_" & Me.Name & ".doSendEMail();"" /></td>")
        '            writer.WriteRowData("</tr>")
        '            writer.WriteRowData("</table>")
        '    End Select
        'End Sub


        Public Overrides Sub GetInnerHTML(ByVal writer As System.Web.UI.HtmlTextWriter)
            If Users.CurrentUser.IsLogged = False Then
                'Select Case LCase(Trim(Me.GetParameter(renderer, "_a", vbNullString)))
                'Case "pwd_forgot" : Me.GetPwdForgot(writer)
                'Case Else
                Me.GetLoginHTML(writer)
                'End Select
            Else
            End If
        End Sub

        'Public Overrides Function GetScriptHTML() As String
        '    Dim writer As New HtmlWriter
        '    html = ""
        '    writer.WriteRowData ("function CLoginControl_Validate() {"
        '    writer.WriteRowData ("var frm = document.forms[0];"
        '    writer.WriteRowData ("if (frm.username.value == '') {"
        '    writer.WriteRowData ("alert(""Nome utente non valido!"");"
        '    writer.WriteRowData ("frm.username.focus();"
        '    writer.WriteRowData ("return false;"
        '    writer.WriteRowData ("}"
        '    writer.WriteRowData ("if (frm.password.value == '') {"
        '    writer.WriteRowData ("alert(""Password non valida!"");"
        '    writer.WriteRowData ("frm.password.focus();"
        '    writer.WriteRowData ("return false;"
        '    writer.WriteRowData ("}"
        '    writer.WriteRowData ("var winW = 0, winH = 0;"
        '    writer.WriteRowData ("if (document.body && document.body.offsetWidth) {"
        '    writer.WriteRowData ("winW = document.body.offsetWidth;"
        '    writer.WriteRowData ("winH = document.body.offsetHeight;"
        '    writer.WriteRowData ("}"
        '    writer.WriteRowData ("if (document.compatMode == 'CSS1Compat' && document.documentElement && document.documentElement.offsetWidth) {"
        '    writer.WriteRowData ("winW = document.documentElement.offsetWidth;"
        '    writer.WriteRowData ("winH = document.documentElement.offsetHeight;"
        '    writer.WriteRowData ("}"
        '    writer.WriteRowData ("if (window.innerWidth && window.innerHeight) {"
        '    writer.WriteRowData ("winW = window.innerWidth;"
        '    writer.WriteRowData ("winH = window.innerHeight;"
        '    writer.WriteRowData ("}"
        '    writer.WriteRowData ("frm.w.value = winW;"
        '    writer.WriteRowData ("frm.h.value = winH;"
        '    writer.WriteRowData ("return true;"
        '    writer.WriteRowData ("}"
        '    Return html
        'End Function

    End Class


End Namespace