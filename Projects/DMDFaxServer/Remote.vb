Imports System.Runtime.InteropServices
Imports System.Reflection
Imports System.Drawing
Imports System.Threading
Imports System.Net.Mail
Imports DMD.Sistema
Imports DMD
Imports DMD.Anagrafica
Imports DMD.Databases
Imports System.Net
Imports DMD.WebSite
Imports DMD.CustomerCalls

Module Remote
    Public lResolve As Integer = 60 * 1000
    Public lConnect As Integer = 60 * 1000
    Public lSend As Integer = 120 * 1000
    Public lReceive As Integer = 120 * 1000

    Public Event UserLoggedIn(ByVal sender As Object, ByVal e As UserLoginEventArgs)
    Public Event UserLoggedOut(ByVal sender As Object, ByVal e As UserLogoutEventArgs)
    Public Event UploadProgress(ByVal sender As Object, ByVal e As UploadProgressChangedEventArgs)
    Public Event UploadCompleted(ByVal sender As Object, ByVal e As UploadFileCompletedEventArgs)

    Public WithEvents client As New WebClient
    Public CurrentUser As CUser = Nothing
    Private m_CurrentSession As DMD.WebSite.CSessionInfo = Nothing
    Private m_AziendaPrincipale As CAzienda = Nothing
    Private logToken As String = ""
    Private uploadCount As Integer = 0

    Public Property AziendaPrincipale As CAzienda
        Get
            If (Remote.CurrentUser Is Nothing) Then
                Remote.Login(My.Settings.UserName, My.Settings.Password)
            End If
            If (m_AziendaPrincipale Is Nothing) Then
                Dim url As String = "/widgets/websvcf/dialtp.aspx?_a=GetAziendaPrincipale"
                Dim tmp As String = XML.Utils.Serializer.DeserializeString(RPC.InvokeMethod(My.Settings.ServerName & url))
                If (tmp <> "") Then m_AziendaPrincipale = XML.Utils.Serializer.Deserialize(tmp)
            End If
            Return m_AziendaPrincipale
        End Get
        Set(value As CAzienda)
            m_AziendaPrincipale = value
        End Set
    End Property


    'Public Function InvokeMethod(ByVal methodName As String, ByVal ParamArray params() As Object) As String
    '    Dim buf As New System.Text.StringBuilder
    '    buf.Append(My.Settings.ServerName)
    '    buf.Append(methodName)

    '    If (params IsNot Nothing AndAlso params.Length > 0) Then
    '        If (InStr(methodName, "?") <= 0) Then
    '            buf.Append("?")
    '        Else
    '            buf.Append("&")
    '        End If
    '        For i As Integer = 0 To UBound(params) Step 2
    '            If (i > 0) Then buf.Append("&")
    '            buf.Append(params(i))
    '            buf.Append("=")
    '            buf.Append(Strings.URLEncode(params(i + 1)))
    '        Next
    '    End If

    '    Dim ret As String = client.DownloadString(buf.ToString)

    '    If (Left(ret, 2) = "00") Then
    '        ret = Mid(ret, 3)
    '    Else
    '        Throw New Exception("RPC: Error 0x" & Left(ret, 2) & vbCrLf & Mid(ret, 3))
    '    End If
    '    Return ret
    'End Function

    Public Function GetLokToken() As String
        If logToken = "" Then
            Dim url As String = "/widgets/websvcf/dialtp.aspx?_a=GetSessionID"
            logToken = XML.Utils.Serializer.DeserializeString(RPC.InvokeMethod(My.Settings.ServerName & url))
        End If
        Return logToken
    End Function


    Public Sub Login(ByVal userName As String, ByVal password As String)
        Dim url As String = "/widgets/websvcf/dialtp.aspx?_a=UserLogin"
        RPC.sessionID = GetLokToken()
        Dim tmp As String = RPC.InvokeMethod(My.Settings.ServerName & url, "u", RPC.str2n(userName), "p", RPC.str2n(password))
        CurrentUser = DMD.XML.Utils.Serializer.Deserialize(tmp)
        If CurrentUser IsNot Nothing Then
            Select Case LCase(CurrentUser.UserName)
                Case "system", "guest"
                    CurrentUser = Nothing
                    Throw New InvalidOperationException("Utente non ammesso")
                Case Else
                    RaiseEvent UserLoggedIn(Nothing, New UserLoginEventArgs(CurrentUser))
            End Select
        End If
    End Sub

    Public Function FindPersone(ByVal text As String) As CCollection(Of CPersonaInfo)
        If (Remote.CurrentUser Is Nothing) Then
            Remote.Login(My.Settings.UserName, My.Settings.Password)
        End If
        Dim url As String = My.Settings.ServerName & "/widgets/websvcf/dialtp.aspx?_a=FindPersona"
        Dim filter As New CRMFinlterI
        filter.text = text
        filter.nMax = 500
        RPC.sessionID = GetLokToken()
        Dim tmp As String = RPC.InvokeMethod(url, "filter", RPC.str2n(XML.Utils.Serializer.Serialize(filter)))
        Dim items As New CCollection(Of CPersonaInfo)
        items.Sort()
        If (tmp <> "") Then items.AddRange(DMD.XML.Utils.Serializer.Deserialize(tmp))

        Return items
    End Function

    Public Sub Logout()
        'Try
        '    Throw New NotImplementedException
        'Catch ex As Exception
        '    DMD.Sistema.Events.NotifyUnhandledException(ex)
        '    MsgBox(ex, MsgBoxStyle.Critical)
        'End Try
        Dim u As CUser = CurrentUser
        RPC.sessionID = GetLokToken()
        RPC.InvokeMethod(My.Settings.ServerName & "/widgets/websvcf/dialtp.aspx?_a=CurrentUserLogOut")
        CurrentUser = Nothing
        RaiseEvent UserLoggedOut(Nothing, New UserLogoutEventArgs(u, LogOutMethods.LOGOUT_LOGOUT))
    End Sub

    Function GetPersonaById(ByVal id As Integer) As CPersona
        If (id = 0) Then Return Nothing

        If (Remote.CurrentUser Is Nothing) Then
            Remote.Login(My.Settings.UserName, My.Settings.Password)
        End If

        Dim url As String = "/widgets/websvcf/dialtp.aspx?_a=GetPersonaById"
        RPC.sessionID = GetLokToken()
        Dim tmp As String = RPC.InvokeMethod(My.Settings.ServerName & url, "id", RPC.int2n(id))
        If (tmp <> "") Then
            Return XML.Utils.Serializer.Deserialize(tmp)
        Else
            Return Nothing
        End If
    End Function

    Function GetRecapitiPersonaById(ByVal p As CPersona) As CCollection(Of CContatto)
        Dim ret As New CCollection(Of CContatto)

        If (Remote.CurrentUser Is Nothing) Then
            Remote.Login(My.Settings.UserName, My.Settings.Password)
        End If

        Dim url As String = "/widgets/websvcf/dialtp.aspx?_a=LoadContattiPersona"
        RPC.sessionID = GetLokToken()
        Dim tmp As String = RPC.InvokeMethod(My.Settings.ServerName & url, "pid", RPC.int2n(GetID(p)))

        If (tmp <> "") Then
            ret.AddRange(XML.Utils.Serializer.Deserialize(tmp))
        End If

        Return ret
    End Function

    Public Function Upload(ByVal fileName As String) As String
        If (client Is Nothing) Then client = New WebClient
        Dim key As String = uploadCount
        uploadCount += 1
        Dim uri As New Uri(My.Settings.ServerName & "/widgets/websvc/HttpUtil1.aspx?token=" & RPC.sessionID & "&u=" & GetID(CurrentUser) & "&rk=" & key)
        'client.Headers.Add("ASP.NET_SessionId", RPC.sessionID)
        'Dim header As String = RPC.GetAllResponseHaders
        'Dim lines As String() = Split(header, vbCrLf)
        'For Each line As String In lines
        '    line = Trim(line)
        '    Dim name As String = ""
        '    Dim value As String = ""

        '    If (line <> "") Then
        '        Dim p As Integer = InStr(line, ":")
        '        If (p > 0) Then
        '            name = Trim(Left(line, p - 1))
        '            value = Trim(Mid(line, p + 1))

        '        Else
        '            name = line
        '        End If
        '    End If

        '    Select Case LCase(name)
        '        Case "content-length", "content-type", "", "expires", "server"
        '        Case Else
        '            client.Headers.Add(name, value)
        '    End Select
        'Next
        client.UploadFileAsync(uri, fileName)
        'client.UploadFile(uri, fileName)

        Return key
    End Function

    Private Sub client_UploadFileCompleted(sender As Object, e As UploadFileCompletedEventArgs) Handles client.UploadFileCompleted
        RaiseEvent UploadCompleted(sender, e)
    End Sub

    Private Sub client_UploadProgressChanged(sender As Object, e As UploadProgressChangedEventArgs) Handles client.UploadProgressChanged
        RaiseEvent UploadProgress(sender, e)
    End Sub

    Function GetUploadedFileByKey(ByVal key As String) As WebSite.CUploadedFile
        If (Remote.CurrentUser Is Nothing) Then
            Remote.Login(My.Settings.UserName, My.Settings.Password)
        End If

        Dim url As String = "/widgets/websvcf/dialtp.aspx?_a=GetUoloadByKey"
        RPC.sessionID = GetLokToken()
        Dim tmp As String = RPC.InvokeMethod(My.Settings.ServerName & url, "rk", RPC.str2n(RPC.sessionID & "_" & GetID(CurrentUser) & "_" & key))
        If (tmp <> "") Then Return XML.Utils.Serializer.Deserialize(tmp)
        Return Nothing
    End Function

    Public Property CurrentSession() As WebSite.CSessionInfo
        Get
            If (Remote.CurrentUser Is Nothing) Then
                Remote.Login(My.Settings.UserName, My.Settings.Password)
            End If
            If (m_CurrentSession Is Nothing) Then
                Dim url As String = "/widgets/websvcf/dialtp.aspx?_a=GetCurrentSession"
                Dim tmp As String = XML.Utils.Serializer.DeserializeString(RPC.InvokeMethod(My.Settings.ServerName & url))
                If (tmp <> "") Then m_CurrentSession = XML.Utils.Serializer.Deserialize(tmp)
            End If
            Return m_CurrentSession
        End Get
        Set(value As WebSite.CSessionInfo)
            m_CurrentSession = value
        End Set
    End Property

    Public Function SaveObject(ByVal o As Object) As Object
        If (Remote.CurrentUser Is Nothing) Then
            Remote.Login(My.Settings.UserName, My.Settings.Password)
        End If

        Dim url As String = "/widgets/websvcf/dialtp.aspx?_a=SaveObject"
        Dim text As String = XML.Utils.Serializer.Serialize(o)
        Dim tmp As String = RPC.InvokeMethod(My.Settings.ServerName & url, "text", RPC.str2n(text))
        Return XML.Utils.Serializer.Deserialize(tmp)
    End Function

    Public Function SendFax(ByVal doc As FaxDocument) As FaxDocument
        If (Remote.CurrentUser Is Nothing) Then
            Remote.Login(My.Settings.UserName, My.Settings.Password)
        End If

        Dim url As String = "/widgets/websvcf/dialtp.aspx?_a=SendFax"
        Dim text As String = XML.Utils.Serializer.Serialize(doc)
        Dim tmp As String = RPC.InvokeMethod(My.Settings.ServerName & url, "text", RPC.str2n(text))
        Return XML.Utils.Serializer.Deserialize(tmp)
    End Function

End Module

