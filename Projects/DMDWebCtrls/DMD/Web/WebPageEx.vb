Imports Microsoft.VisualBasic
Imports System.Web
Imports System.Web.UI
Imports System.Data
Imports System.Text.RegularExpressions
Imports DMD
Imports DMD.Forms

Namespace Web

    Public Class WebPageEx
        Inherits WebSite.WebPageEx


        Public Const ADRESSBAR_HEIGHT = 40
        Public Const FOOTER_HEIGHT = 20
        Public Const ONMAINTENANCE = False
        Public Const SESSIONTIMEOUT = 20


        Private m_dmdpage As String = ""
        Private m_WebControls As DMD.Forms.CWebControlsCollection
        Private m_IncludedScripts As New CCollection(Of String)
        Private m_IncludedCSS As New CCollection(Of String)

        Shared Sub New()
            'DMD.ADV.Initialize()
        End Sub

        ''' <summary>
        ''' Restituisce vero se la pagina viene generata tramite codice e non tramite HTML
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function CustomRendering() As Boolean
            Return False
        End Function

        Public Sub New()
            Me.SetupInitialScripts()
        End Sub

        Public ReadOnly Property dmdpage As String
            Get
                If (Me.m_dmdpage = "") Then Me.m_dmdpage = Trim(Request.Form("_dmdpage"))
                If (Me.m_dmdpage = "") Then Me.m_dmdpage = Sistema.ASPSecurity.GetRandomKey(25)
                Return Me.m_dmdpage
            End Get
        End Property

        Protected Overridable Function GetResourceVersion(ByVal url As String) As String
            Try
                Dim path As String = Me.AC.MapPath(url)
                Dim f As New System.IO.FileInfo(path)
                Return Hex(f.Length) & "." & f.LastWriteTimeUtc
            Catch ex As Exception
                Return "err"
            End Try
        End Function

        Protected Overridable Function AddJSResourceWithVersion(ByVal url As String) As String
            Dim version As String = Me.GetResourceVersion(url)
            If (InStr(url, "?") > 0) Then
                url = url & "&scrv=" & version
            Else
                url = url & "?scrv=" & version
            End If
            Me.IncludedScripts.Add(url)
            Return url
        End Function

        Protected Overridable Function AddCSSResourceWithVersion(ByVal url As String) As String
            Dim version As String = Me.GetResourceVersion(url)
            If (InStr(url, "?") > 0) Then
                url = url & "&scrv=" & version
            Else
                url = url & "?scrv=" & version
            End If
            Me.IncludedCSS.Add(url)
            Return url
        End Function

        Protected Overridable Sub SetupInitialScripts()
            Me.AddJSResourceWithVersion("/ckeditor/ckeditor.js")
            Me.AddJSResourceWithVersion("/ckeditor/config.js")

            Me.AddJSResourceWithVersion("/widgets/common.js")
            Me.AddJSResourceWithVersion("/widgets/Forms.js")

            Me.AddJSResourceWithVersion("/widgets/System.js")
            Me.AddJSResourceWithVersion("/widgets/WebSite.js")
            Me.AddJSResourceWithVersion("/widgets/CRM.js")
            Me.AddJSResourceWithVersion("/widgets/GPSLib.js")
            Me.AddJSResourceWithVersion("/widgets/Office.js")
            Me.AddJSResourceWithVersion("/widgets/CQSPD.js")
            Me.AddJSResourceWithVersion("/widgets/Tickets.js")
            Me.AddJSResourceWithVersion("/widgets/System_Controls.js")
            Me.AddJSResourceWithVersion("/widgets/WebSite_Controls.js")
            Me.AddJSResourceWithVersion("/widgets/CRM_Controls.js")
            Me.AddJSResourceWithVersion("/widgets/Office_Controls.js")
            Me.AddJSResourceWithVersion("/widgets/CQSPD_Controls.js")
            Me.AddJSResourceWithVersion("/widgets/Tickets_Controls.js")

            Me.AddCSSResourceWithVersion("/widgets/style.css")
        End Sub

        Protected Overridable Sub IncludeGoogleMapsScript(ByVal writer As System.Web.UI.HtmlTextWriter, ByVal apiKey As String)
            'writer.Write("<script src=""https://maps.googleapis.com/maps/api/js?key=" & apiKey & "&callback=initMap"" async defer></script>")
            'writer.Write("<script  src=""https://maps.googleapis.com/maps/api/js?key=" & apiKey & "&callback=initMap""  type=""text/javascript""></script>")

            writer.Write("<script src=""https://maps.google.com/maps/api/js?sensor=false&force=lite"" async defer type=""text/javascript""></script>")

        End Sub

        Protected Overridable Function GetGoogleMapsApiKey() As String
            Return ""
        End Function

        ''' <summary>
        ''' Restituisce la collezione degli script che vengono inclusi nella pagina sul client
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IncludedScripts As CCollection(Of String)
            Get
                Return Me.m_IncludedScripts
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la collezione dei fogli di stile che vengono inclusi nella pagina sul client
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IncludedCSS As CCollection(Of String)
            Get
                Return Me.m_IncludedCSS
            End Get
        End Property


        'Public Function WORKINGAREA_WIDTH() As Integer
        '    Dim ret As Integer
        '    If Me.GetParameter(renderer, "interface", "0") = "0" Then
        '        ret = Me.MAINWINDOW_WIDTH - 5
        '    Else
        '        ret = Me.MAINWINDOW_WIDTH
        '    End If

        '    Return Math.Max(ret, 1)
        'End Function

        'Public Function WORKINGAREA_HEIGHT() As Integer
        '    Dim ret As Integer
        '    If Me.GetParameter(renderer, "interface", "0") = "0" Then
        '        ret = Me.MAINWINDOW_HEIGHT - 5
        '    Else
        '        ret = Me.MAINWINDOW_HEIGHT - 60
        '    End If
        '    Return Math.Max(ret, 1)
        'End Function

        'Public Function DEFAULT_LISTITEMS() As Integer
        '    Return 1 + (WORKINGAREA_HEIGHT() - 80) \ 15
        'End Function

        'Public Property MAINWINDOW_WIDTH As Integer
        '    Get
        '        Dim ret As Integer = 0
        '        Try
        '            If Request.QueryString("mww") <> vbNullString Then
        '                ret = Formats.ToInteger(Request.QueryString("mww"))
        '            ElseIf Request.Form("mww") <> vbNullString Then
        '                ret = Formats.ToInteger(Request.Form("mww"))
        '            ElseIf Cookies.IsCookieSet("window_w") Then
        '                Return Formats.ToInteger(Cookies.GetCookie("window_w", 800))
        '            Else
        '                ret = Formats.ToInteger(Session.Contents("MAINWINDOW_WIDTH"))
        '            End If
        '        Catch ex As Exception

        '        End Try
        '        'If ret <= 0 Then Return 800
        '        Return ret
        '    End Get
        '    Set(value As Integer)
        '        Session.Contents("MAINWINDOW_WIDTH") = value
        '    End Set
        'End Property

        'Public Property MAINWINDOW_HEIGHT As Integer
        '    Get
        '        Dim ret As Integer = 0
        '        Try
        '            If Request.QueryString("mwh") <> vbNullString Then
        '                ret = Formats.ToInteger(Request.QueryString("mwh"))
        '            ElseIf Request.Form("mwh") <> vbNullString Then
        '                ret = Formats.ToInteger(Request.Form("mwh"))
        '            ElseIf Cookies.IsCookieSet("window_h") Then
        '                ret = Formats.ToInteger(Cookies.GetCookie("window_h", 600))
        '            Else
        '                ret = Formats.ToInteger(Session.Contents("MAINWINDOW_HEIGHT"))
        '            End If
        '        Catch ex As Exception

        '        End Try
        '        'If ret <= 0 Then Return 600
        '        Return ret
        '    End Get
        '    Set(value As Integer)
        '        Session.Contents("MAINWINDOW_HEIGHT") = value
        '    End Set
        'End Property

        Protected Overridable Sub CreateHeaderHTML(ByVal writer As System.Web.UI.HtmlTextWriter)
            writer.Write("<title>" & ApplicationContext.Title)
            If Not (Me.CurrentModule Is Nothing) Then
                writer.Write(" : " & Me.CurrentModule.Description)
            End If
            writer.Write("</title>")
            For i As Integer = 0 To Me.IncludedScripts.Count - 1
                Dim script As String = Me.IncludedScripts(i)
                writer.Write("<script type=""text/javascript"" src=""" & script & """></script>")
            Next
            If Me.GetGoogleMapsApiKey <> "" Then
                IncludeGoogleMapsScript(writer, Me.GetGoogleMapsApiKey)
            End If
            For i As Integer = 0 To Me.IncludedCSS.Count - 1
                Dim css As String = Me.IncludedCSS(i)
                writer.Write("<link rel=""stylesheet"" href=""" & css & """ type=""text/css"">")
            Next
            writer.Write("<script language=""javascript"" type=""text/javascript"">")
            If (ApplicationContext.IsDebug) Then writer.Write("__DEBUG = true;")
            writer.Write("__BASENAME = '" & Me.ApplicationContext.BaseName & "';")

            Dim str As String = XML.Utils.Serializer.Serialize(Sistema.Users.CurrentUser)
            str = Replace(str, vbCr, "")
            str = Replace(str, vbLf, "")
            str = Replace(str, Chr(34), "\" & Chr(34))
            writer.Write("Sistema_Users_currentUserXML = """ & str & """;")

            str = XML.Utils.Serializer.Serialize(Sistema.Users.CurrentUser.Settings)
            str = Replace(str, vbCr, "")
            str = Replace(str, vbLf, "")
            str = Replace(str, Chr(34), "\" & Chr(34))

            writer.Write("Sistema_Users_currentUserSettingXML = """ & str & """;")
            writer.Write("</script>")


        End Sub

        Public ReadOnly Property WebControls As DMD.Forms.CWebControlsCollection
            Get
                If Me.m_WebControls Is Nothing Then Me.m_WebControls = New DMD.Forms.CWebControlsCollection
                Return Me.m_WebControls
            End Get
        End Property


        Protected Overridable Sub GetInnerHTML(ByVal writer As System.Web.UI.HtmlTextWriter)
            Dim i As Integer
            For i = 0 To Me.WebControls.Count - 1
                Me.WebControls.Item(i).CreateHTML(writer)
            Next
        End Sub


        Public Overridable Sub GetLoginHTML(ByVal writer As System.Web.UI.HtmlTextWriter)
            Dim ctrlLogin As CLoginControl

            If Not Me.IsUserLogged Then
                ctrlLogin = New CLoginControl
                ctrlLogin.Name = "login"
                ctrlLogin.CreateHTML(writer)
            End If
        End Sub

        Protected Overridable Sub CreateBodyHTML(ByVal writer As System.Web.UI.HtmlTextWriter)
            writer.Write("<div class=""mainForm"" id=""mainForm""  >")
            writer.Write("<form name=""frmMain"" id=""frmMain"" action=""" & Me.CurrentPage.PageName & """ method=""post"" onsubmit=""return false;"">")
            If (Not Me.RequiresLogin) OrElse (Me.IsUserLogged) Then
                Me.GetInnerHTML(writer)
            Else
                Me.GetLoginHTML(writer)
            End If

            writer.Write("<input type=""hidden"" name=""_m"" id=""_m"" value=""" & DMD.Databases.GetID(Me.CurrentModule) & """ />")
            writer.Write("<input type=""hidden"" name=""_a"" id=""_a"" value=""" & Sistema.Strings.HtmlEncode(Me.GetParameter("_a", vbNullString)) & """ />")
            writer.Write("<input type=""hidden"" name=""_s"" id=""_s"" value=''/>")

            writer.Write("</form>")
            writer.Write("</div>")
            'writer.Write("<iframe name=""frmHidden"" id=""frmHidden"" style=""display:none;"" src=""/blank.html""></iframe>")
            'writer.Write("<iframe name=""" & Me.ApplicationContext.BaseName & """ id=""" & Me.ApplicationContext.BaseName & """ style=""display:none;"" src=""/blank.html""></iframe>")
            writer.Write("<script language=""javascript"" type=""text/javascript"">")
            writer.Write("var __dmdpage = """ & Me.dmdpage & """;" & vbNewLine)
            writer.Write("function ShowMessanger() { Messenger.ShowSendMessage(); return false; }")
            If Me.GetParameter("interface", "0") <> "0" Then
                writer.Write("Cookies.setCookie(""window_w"", Window.getWidth());")
                writer.Write("Cookies.setCookie(""window_h"", Window.getHeight());")
            End If
            'writer.Write ("if (typeof(window.onload) == ""function"") Window.addListener(""onload"", window.onload);"
            'writer.Write ("window.onload = new Function(""Window.dispatchEvent('onload')"");"
            If DMD.Sistema.Formats.ToString(Session("Error_Message")) <> "" Then
                writer.Write("alert('" & Sistema.Strings.ToJS(Session("Error_Message")) & "');")
                Session("Error_Message") = ""
            End If


            writer.Write("</script>")
        End Sub

        Protected Overrides Sub OnInit(e As EventArgs)
            MyBase.OnInit(e)
            Me.Response.Buffer = False
            If (Not Me.CustomRendering) Then
                For i As Integer = 0 To Me.IncludedScripts.Count - 1
                    Dim item As String = Me.IncludedScripts(i)
                    Me.ClientScript.RegisterClientScriptInclude(Me.GetType, "resScript" & i, item)
                Next
            End If
        End Sub



        Protected Overridable Sub CreateHTML(ByVal writer As System.Web.UI.HtmlTextWriter)
            writer.Write("<!DOCTYPE html>")
            'writer.Write ("<html xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""it"" lang=""it"">"
            writer.Write("<html>")
            writer.Write("<head>")
            Me.CreateHeaderHTML(writer)
            writer.Write("</head>")
            writer.Write("<body>")
            Me.CreateBodyHTML(writer)
            writer.Write("</body>")
            writer.Write("</html>")

        End Sub




        Protected Overrides Sub InternalRender(writer As HtmlTextWriter)
            If Me.CustomRendering Then
                Me.CreateHTML(writer)
            Else
                MyBase.InternalRender(writer)
            End If
        End Sub



        Public Overrides Sub Dispose()
            If (Me.m_WebControls IsNot Nothing) Then
                For Each c As WebControl In Me.m_WebControls
                    If (TypeOf (c) Is IDisposable) Then DirectCast(c, IDisposable).Dispose()
                Next
                Me.m_WebControls = Nothing
            End If
            Me.m_IncludedScripts = Nothing
            Me.m_IncludedCSS = Nothing
            MyBase.Dispose()
        End Sub


    End Class

End Namespace