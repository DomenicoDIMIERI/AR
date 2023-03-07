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
Imports System.Collections.Specialized
Imports System.IO
Imports DIALTPLib
Imports DMD.Office

Public Class Remote
    Public Const __DMDENTRYSVC As String = "/widgets"

    Public lResolve As Integer = 60 * 1000
    Public lConnect As Integer = 60 * 1000
    Public lSend As Integer = 120 * 1000
    Public lReceive As Integer = 120 * 1000

    Public Shared Event RequireUserLogin(ByVal sender As Object, ByVal e As System.EventArgs)
    Public Shared Event UserLoggedIn(ByVal sender As Object, ByVal e As UserLoginEventArgs)
    Public Shared Event UserLoggedOut(ByVal sender As Object, ByVal e As UserLogoutEventArgs)
    Public Shared Event UploadProgress(ByVal sender As Object, ByVal e As UploadProgressChangedEventArgs)
    Public Shared Event UploadCompleted(ByVal sender As Object, ByVal e As UploadFileCompletedEventArgs)

    ' Public Shared WithEvents client As New WebClient
    Public Shared CurrentUser As CUser = Nothing
    Private Shared m_CurrentSession As DMD.WebSite.CSessionInfo = Nothing
    Private Shared m_AziendaPrincipale As CAzienda = Nothing
    Private Shared logToken As String = ""
    Private Shared uploadCount As Integer = 0

    Public Shared Function CheckUserLogged() As CUser
        If CurrentUser Is Nothing Then
            RaiseEvent RequireUserLogin(Nothing, New System.EventArgs)
        End If
        If (CurrentUser Is Nothing) Then Throw New InvalidOperationException("Utente non connesso")
        Return CurrentUser
    End Function

    Public Shared Property AziendaPrincipale As CAzienda
        Get
            CheckUserLogged()
            If (m_AziendaPrincipale Is Nothing) Then
                Dim url As String = "/widgets/websvcf/dialtp.aspx?_a=GetAziendaPrincipale"
                Dim tmp As String = XML.Utils.Serializer.DeserializeString(RPC.InvokeMethod(getServerName() & url))
                If (tmp <> "") Then m_AziendaPrincipale = DirectCast(XML.Utils.Serializer.Deserialize(tmp), CAzienda)
            End If
            Return m_AziendaPrincipale
        End Get
        Set(value As CAzienda)
            m_AziendaPrincipale = value
        End Set
    End Property


    'Public shared Function InvokeMethod(ByVal methodName As String, ByVal ParamArray params() As Object) As String
    '    Dim buf As New System.Text.StringBuilder
    '    buf.Append(getServerName)
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

    Public Shared Function GetLokToken() As String
        If logToken = "" Then
            Dim url As String = "/widgets/websvcf/dialtp.aspx?_a=GetSessionID"
            logToken = XML.Utils.Serializer.DeserializeString(RPC.InvokeMethod(getServerName() & url))
        End If
        Return logToken
    End Function


    Public Shared Sub Login(ByVal userName As String, ByVal password As String)
        Dim url As String = "/widgets/websvcf/dialtp.aspx?_a=UserLogin"
        RPC.sessionID = GetLokToken()
        Dim tmp As String = RPC.InvokeMethod(DialTPApp.CurrentConfig.ServerName & url, "u", RPC.str2n(userName), "p", RPC.str2n(password))
        CurrentUser = DirectCast(DMD.XML.Utils.Serializer.Deserialize(tmp), CUser)
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

    Public Shared Function FindPersone(ByVal text As String) As CCollection(Of CPersonaInfo)
        CheckUserLogged()
        Dim url As String = getServerName() & "/widgets/websvcf/dialtp.aspx?_a=FindPersona"
        Dim filter As New CRMFinlterI
        filter.text = text
        filter.nMax = 500
        RPC.sessionID = GetLokToken()
        Dim tmp As String = RPC.InvokeMethod(url, "filter", RPC.str2n(XML.Utils.Serializer.Serialize(filter)))
        Dim items As New CCollection(Of CPersonaInfo)
        If (tmp <> "") Then items.AddRange(DirectCast(DMD.XML.Utils.Serializer.Deserialize(tmp), IEnumerable))
        items.Sort()

        Return items
    End Function

    Public Shared Function FindInfoNumbero(ByVal serverName As String, ByVal text As String) As Object
        Dim ret As New CKeyCollection

        serverName = Trim(serverName)
        If Not serverName.EndsWith("/") Then serverName &= "/"
        Dim url As String = serverName & "widgets/websvcf/dialtp.aspx?_a=FindInfoNumbero"

        Dim filter As New CRMFinlterI
        filter.text = text
        filter.nMax = 500
        'RPC.sessionID = GetLokToken()
        Dim tmp As String = RPC.InvokeMethod(url, "filter", RPC.str2n(XML.Utils.Serializer.Serialize(filter)))


        If (tmp <> "") Then ret = CType(XML.Utils.Serializer.Deserialize(tmp), CKeyCollection)

        Return ret
    End Function

    Public Shared Function FindInfoNumberoAsync(ByVal serverName As String, ByVal text As String, ByVal handler As Sistema.IRPCCallHandler) As Sistema.AsyncState
        'If (Remote.CurrentUser Is Nothing) Then
        '    Remote.Login(My.Settings.UserName, My.Settings.Password)
        'End If
        serverName = Trim(serverName)
        If Not serverName.EndsWith("/") Then serverName &= "/"
        Dim url As String = serverName & "widgets/websvcf/dialtp.aspx?_a=FindInfoNumbero"

        Dim filter As New CRMFinlterI
        filter.text = text
        filter.nMax = 500
        'RPC.sessionID = GetLokToken()
        Return RPC.InvokeMethodAsync(url, handler, "filter", RPC.str2n(XML.Utils.Serializer.Serialize(filter)))
    End Function

    Friend Shared Function GetProxyProfile(ByVal serverName As String, ByVal name As String) As ProxyProfile
        Dim url As String = serverName & "/widgets/websvcf/dialtp.aspx?_a=GetProxyProfile"
        Dim tmp As String = RPC.InvokeMethod(url, "name", RPC.str2n(name))
        If (tmp = "") Then Return Nothing
        Return CType(XML.Utils.Serializer.Deserialize(tmp), ProxyProfile)
    End Function

    Public Shared Function FindPersoneAsync(ByVal serverName As String, ByVal text As String, ByVal handler As Sistema.IRPCCallHandler) As Sistema.AsyncState
        'If (Remote.CurrentUser Is Nothing) Then
        '    Remote.Login(My.Settings.UserName, My.Settings.Password)
        'End If
        'getServerName()

        Dim url As String = serverName & "/widgets/websvcf/dialtp.aspx?_a=FindPersona"
        Dim filter As New CRMFinlterI
        filter.text = text
        filter.nMax = 500
        'RPC.sessionID = GetLokToken()
        Return RPC.InvokeMethodAsync(url, handler, "filter", RPC.str2n(XML.Utils.Serializer.Serialize(filter)))
        'Dim items As New CCollection(Of CPersonaInfo)
        'items.Sort()
        'If (tmp <> "") Then items.AddRange(DMD.XML.Utils.Serializer.Deserialize(tmp))

        'Return items
    End Function

    Public Shared Function IsLogged() As Boolean
        Return Databases.GetID(Remote.CurrentUser) <> 0 AndAlso LCase(Remote.CurrentUser.UserName) <> "guest"
    End Function

    Public Shared Sub Logout()
        'Try
        '    Throw New NotImplementedException
        'Catch ex As Exception
        '    DMD.Sistema.Events.NotifyUnhandledException(ex)
        '    MsgBox(ex, MsgBoxStyle.Critical)
        'End Try
        Dim u As CUser = CurrentUser
        RPC.sessionID = GetLokToken()
        RPC.InvokeMethod(getServerName() & "/widgets/websvcf/dialtp.aspx?_a=CurrentUserLogOut")
        CurrentUser = Nothing
        RaiseEvent UserLoggedOut(Nothing, New UserLogoutEventArgs(u, LogOutMethods.LOGOUT_LOGOUT))
    End Sub

    Public Shared Function GetPersonaById(ByVal id As Integer) As CPersona
        If (id = 0) Then Return Nothing
        CheckUserLogged()
        Dim url As String = "/widgets/websvcf/dialtp.aspx?_a=GetPersonaById"
        RPC.sessionID = GetLokToken()
        Dim tmp As String = RPC.InvokeMethod(getServerName() & url, "id", RPC.int2n(id))
        If (tmp <> "") Then
            Return DirectCast(XML.Utils.Serializer.Deserialize(tmp), CPersona)
        Else
            Return Nothing
        End If
    End Function

    Public Shared Function GetRecapitiPersonaById(ByVal p As CPersona) As CCollection(Of CContatto)
        CheckUserLogged()
        Dim ret As New CCollection(Of CContatto)
        Dim url As String = "/widgets/websvcf/dialtp.aspx?_a=LoadContattiPersona"
        RPC.sessionID = GetLokToken()
        Dim tmp As String = RPC.InvokeMethod(getServerName() & url, "pid", RPC.int2n(GetID(p)))

        If (tmp <> "") Then
            ret.AddRange(DirectCast(XML.Utils.Serializer.Deserialize(tmp), IEnumerable))
        End If

        Return ret
    End Function



    Public Shared Function Upload(ByVal fileName As String) As String
        uploadCount += 1
        Dim info As New System.IO.FileInfo(fileName)
        Dim data As Date = info.CreationTime
        Dim serverName As String = DialTPApp.CurrentConfig.UploadServer
        Dim userName As String = DialTPApp.CurrentConfig.UserName
        Dim tmpName As String = System.IO.Path.GetTempFileName
        System.IO.File.Copy(fileName, tmpName, True)
        Dim url As String = serverName & "/widgets/websvc/dialtpuploader.aspx?p=" & My.Computer.Name & "&u=" & userName & "&f=" & fileName & "&d=" & DMD.Sistema.RPC.date2n(data)
        Dim nvc As New NameValueCollection()
        'nvc.Add("id", "TTR")
        nvc.Add("File0", "Upload")
        Dim ret As String = RPC.HttpUploadFile(url, tmpName, "file", "image/jpeg", nvc)
        System.IO.File.Delete(tmpName)
        Return ret
    End Function



    Public Shared Function GetUploadedFileByKey(ByVal key As String) As WebSite.CUploadedFile
        CheckUserLogged()
        Dim url As String = "/widgets/websvcf/dialtp.aspx?_a=GetUoloadByKey"
        RPC.sessionID = GetLokToken()
        Dim tmp As String = RPC.InvokeMethod(getServerName() & url, "rk", RPC.str2n(RPC.sessionID & "_" & GetID(CurrentUser) & "_" & key))
        If (tmp <> "") Then Return DirectCast(XML.Utils.Serializer.Deserialize(tmp), CUploadedFile)
        Return Nothing
    End Function

    Public Shared Function getServerName() As String
        Return DialTPApp.CurrentConfig.ServerName
    End Function

    Public Shared Property CurrentSession() As WebSite.CSessionInfo
        Get
            CheckUserLogged()
            If (m_CurrentSession Is Nothing) Then
                Dim url As String = "/widgets/websvcf/dialtp.aspx?_a=GetCurrentSession"
                Dim tmp As String = XML.Utils.Serializer.DeserializeString(RPC.InvokeMethod(getServerName() & url))
                If (tmp <> "") Then m_CurrentSession = DirectCast(XML.Utils.Serializer.Deserialize(tmp), CSessionInfo)
            End If
            Return m_CurrentSession
        End Get
        Set(value As WebSite.CSessionInfo)
            m_CurrentSession = value
        End Set
    End Property

    Public Shared Function SaveObject(ByVal o As Object) As Object
        CheckUserLogged()
        Dim url As String = "/widgets/websvcf/dialtp.aspx?_a=SaveObject"
        Dim text As String = XML.Utils.Serializer.Serialize(o)
        Dim tmp As String = RPC.InvokeMethod(getServerName() & url, "text", RPC.str2n(text))
        Return XML.Utils.Serializer.Deserialize(tmp)
    End Function

    Public Shared Function SendFax(ByVal doc As FaxDocument) As FaxDocument
        CheckUserLogged()
        Dim url As String = __DMDENTRYSVC & "/websvcf/dialtp.aspx?_a=SendFax"
        Dim text As String = XML.Utils.Serializer.Serialize(doc)
        Dim tmp As String = RPC.InvokeMethod(getServerName() & url, "text", RPC.str2n(text))
        Return DirectCast(XML.Utils.Serializer.Deserialize(tmp), FaxDocument)
    End Function



End Class

