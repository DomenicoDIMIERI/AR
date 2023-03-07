Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports System.IO


Public NotInheritable Class WebSite
    'Inherits CGeneralClass(Of DBObjectBase)

    ''' <summary>
    ''' Evento generato quando viene modificata la configurazione base del sito
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Event ConfigurationChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    Private Const MAXDEBUGITEMS As Integer = 1000


    Public Enum ServeFileMode As Integer
        [default] = 0
        inline = 1
        attachment = 2
    End Enum
 
    Friend Sub New()
        '  MyBase.New("WebSite", Nothing)
    End Sub

    'Private m_TimesIndex As Integer = 0
    Public ReadOnly webSiteLock As New Object
    'Private m_Times As StatsItem() = Arrays.CreateInstance(Of StatsItem)(MAXDEBUGITEMS)


    Private m_Config As SiteConfig
    Private m_AllowedIPs As CAllowedIPs
    Private m_Module As CModule = Nothing

    Public ReadOnly Property [Module] As CModule
        Get
            If (m_Module Is Nothing) Then m_Module = Sistema.Modules.GetItemByName("WebSite")
            Return m_Module
        End Get
    End Property

    Public Function IsLocal() As Boolean
        If (WebSite.ASP_Request IsNot Nothing) Then Return WebSite.ASP_Request.IsLocal
        Return False
    End Function

    'Public Function BeginLog(ByVal page As String) As StatsItem
    '    SyncLock Me.webSiteLock
    '        Dim i As Integer = Me.m_TimesIndex
    '        Me.m_TimesIndex += 1
    '        If (Me.m_TimesIndex >= MAXDEBUGITEMS) Then Me.m_TimesIndex = 0

    '        Dim item As New StatsItem(page)
    '        item.LastRun = Now
    '        Me.m_Times(i) = item
    '        Return Me.m_Times(i)
    '    End SyncLock
    'End Function

    'Public Sub EndLog(ByVal item As StatsItem)
    '    SyncLock Me.webSiteLock
    '        Dim span As TimeSpan = Now - item.LastRun
    '        item.Count += 1
    '        item.ExecTime = span.TotalMilliseconds / 1000
    '        item.MaxExecTime = item.ExecTime
    '    End SyncLock
    'End Sub

    'Public Function GetQueryTimes() As StatsItem() ' KeyValuePair(Of String, Double)()
    '    Return Me.m_Times
    'End Function

    Public ReadOnly Property AllowedIPs As CAllowedIPs
        Get
            If m_AllowedIPs Is Nothing Then
                m_AllowedIPs = New CAllowedIPs
                m_AllowedIPs.Load()
            End If
            Return m_AllowedIPs
        End Get
    End Property

    Public Function GetIPAllowInfo(ByVal value As String) As IPADDRESSinfo
        Return AllowedIPs.GetIPAllowInfo(value)
    End Function

    Public Function IsIPAllowed(ByVal value As String) As Boolean
        Return AllowedIPs.IsIPAllowed(value)
    End Function

    Public Function IsIPNegated(ByVal value As String) As Boolean
        Return AllowedIPs.IsIPNegated(value)
    End Function

    Public Property CurrentUser(ByVal sessionID As String) As CUser
        Get
            Return DirectCast(DMD.Sistema.ApplicationContext, WebApplicationContext).CurrentUser(sessionID)
        End Get
        Set(value As CUser)
            DirectCast(DMD.Sistema.ApplicationContext, WebApplicationContext).CurrentUser(sessionID) = value
        End Set
    End Property

    ''' <summary>
    ''' Restituisce vero se il sito è in manutenzione
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsMaintenance() As Boolean
        Return Sistema.ApplicationContext.IsMaintenance
    End Function

    ''' <summary>
    ''' Imposta il sito in manutenzione
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetMaintenance(ByVal value As Boolean)
        If (value) Then
            Sistema.ApplicationContext.EnterMaintenance()
        Else
            Sistema.ApplicationContext.QuitMaintenance()
        End If
    End Sub

    Public ReadOnly Property CurrentSession As CSiteSession
        Get
            If Sistema.ApplicationContext.CurrentSession Is Nothing AndAlso WebSite.ASP_Session IsNot Nothing Then Sistema.ApplicationContext.CurrentSession = New CCurrentSiteSession
            Return Sistema.ApplicationContext.CurrentSession
        End Get
    End Property

    Public ReadOnly Property AppContext As WebApplicationContext
        Get
            Return Sistema.ApplicationContext
        End Get
    End Property

#Region "Oggetti ASP"

    Public Shared ReadOnly Property ASP_Server As System.Web.HttpServerUtility
        Get
            If System.Web.HttpContext.Current Is Nothing Then Return Nothing
            Return System.Web.HttpContext.Current.Server
        End Get
    End Property

    ''' <summary>
    ''' Restituisce l'indirizzo IP della macchina che ha effettuato la richiesta remota (bypassando l'eventuale netmask)
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetRemoteMachineIP() As String
        Dim VisitorsIPAddr As String = String.Empty
        If (System.Web.HttpContext.Current IsNot Nothing) Then
            If (System.Web.HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR") IsNot Nothing) Then
                VisitorsIPAddr = System.Web.HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR").ToString '.ToString();
            ElseIf (System.Web.HttpContext.Current.Request.UserHostAddress.Length > 0) Then
                VisitorsIPAddr = System.Web.HttpContext.Current.Request.UserHostAddress
            End If
        End If
        Return VisitorsIPAddr
    End Function

    Public Shared ReadOnly Property ASP_Request As System.Web.HttpRequest
        Get
            If System.Web.HttpContext.Current Is Nothing Then Return Nothing
            Return System.Web.HttpContext.Current.Request
        End Get
    End Property

    Public Shared ReadOnly Property ASP_Response As System.Web.HttpResponse
        Get
            If System.Web.HttpContext.Current Is Nothing Then Return Nothing
            Return System.Web.HttpContext.Current.Response
        End Get
    End Property

    Public Shared ReadOnly Property ASP_Session As System.Web.SessionState.HttpSessionState
        Get
            If System.Web.HttpContext.Current Is Nothing Then Return Nothing
            Return System.Web.HttpContext.Current.Session
        End Get
    End Property

    Public Shared ReadOnly Property Application As System.Web.HttpApplicationState
        Get
            If System.Web.HttpContext.Current Is Nothing Then Return Nothing
            Return System.Web.HttpContext.Current.Application
        End Get
    End Property

#End Region

#Region "Configurazione"

    Public ReadOnly Property Configuration As SiteConfig
        Get
            SyncLock Me.webSiteLock
                If m_Config Is Nothing Then
                    m_Config = New SiteConfig
                    m_Config.Load()
                End If
                Return m_Config
            End SyncLock
        End Get
    End Property

    Protected Friend Sub SetConfiguration(ByVal value As SiteConfig)
        SyncLock Me.webSiteLock
            Me.m_Config = value

            Sistema.IndexingService.MaxCacheSize = IIf(value.CRMMaxCacheSize > 1, value.CRMMaxCacheSize, 1)
            Sistema.IndexingService.UnloadFactor = IIf(value.CRMUnloadFactor > 0, value.CRMUnloadFactor, 0.25)
            DMD.Databases.DBUtils.StopStatistics = Not value.LogDBCommands

        End SyncLock
        Me.doConfigChanged(New System.EventArgs)
    End Sub

    Protected Friend Sub doConfigChanged(ByVal e As System.EventArgs)
        Me.OnConfigurationChanged(New System.EventArgs)
    End Sub

    Protected Sub OnConfigurationChanged(ByVal e As System.EventArgs)
        RaiseEvent ConfigurationChanged(Me, e)
    End Sub

#End Region



    Public Sub ServeFile(ByVal fileName As String)
        Dim fileType As String = FileSystem.GetExtensionName(fileName)
        Dim displayName As String = FileSystem.GetFileName(fileName)
        Me.ServeFile(fileName, fileType, displayName, ServeFileMode.default, 1024)
    End Sub

    Public Sub ServeFile(ByVal fileName As String, ByVal fileType As String)
        Dim displayName As String = FileSystem.GetFileName(fileName)
        Me.ServeFile(fileName, fileType, displayName, ServeFileMode.default, 1024)
    End Sub

    Public Sub ServeFile(ByVal fileName As String, ByVal fileType As String, ByVal displayName As String)
        Me.ServeFile(fileName, fileType, displayName, ServeFileMode.default, 1024)
    End Sub

    Public Sub ServeFile(ByVal fileName As String, ByVal fileType As String, ByVal displayName As String, ByVal mode As ServeFileMode)
        Me.ServeFile(fileName, fileType, displayName, mode, 1024)
    End Sub

    Public Sub ServeFile(ByVal fileName As String, ByVal fileType As String, ByVal displayName As String, ByVal mode As ServeFileMode, ByVal bufferSize As Integer)
        Dim objStream As System.IO.FileStream = Nothing
        Dim fileSize As Long
        Dim mimeType As String
        Dim contentDisposition As String
        Dim buffer() As Byte = Nothing

        Try
            objStream = New System.IO.FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            fileSize = objStream.Length
            mimeType = DMD.Net.Mime.MIME.GetMimeTypeFromExtension(fileType)
            Select Case mode
                Case ServeFileMode.attachment : contentDisposition = "attachment"
                Case ServeFileMode.default : contentDisposition = DMD.Net.Mime.MIME.GetDefaultContentDispositionExtension(fileType)
                Case ServeFileMode.inline : contentDisposition = "inline"
                Case Else : Throw New ArgumentOutOfRangeException
            End Select
            'If displayName = vbNullString Then displayName = "download" & ASPSecurity.GetRandomKey(6)

            ASP_Response.Clear()
            ASP_Response.AddHeader("content-disposition", contentDisposition & "; filename= """ & displayName & """")
            ASP_Response.AddHeader("content-length", fileSize)
            ASP_Response.ContentType = mimeType
            ASP_Response.Charset = "utf-8" ' "windows-1252"

            ReDim buffer(bufferSize - 1)
            While (fileSize >= bufferSize)
                objStream.Read(buffer, 0, bufferSize)
                ASP_Response.BinaryWrite(buffer)
                fileSize -= bufferSize
            End While
            If (fileSize > 0) Then
                ReDim buffer(fileSize - 1)
                objStream.Read(buffer, 0, fileSize)
                ASP_Response.BinaryWrite(buffer)
            End If
            ASP_Response.Flush()
            'objStream.Close()

        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
            Throw
        Finally
            If (objStream IsNot Nothing) Then objStream.Dispose() : objStream = Nothing
            If (buffer IsNot Nothing) Then Erase buffer : buffer = Nothing
        End Try
    End Sub

    Public Function GetAbsolutePath(ByVal path As String) As String
        Dim ret As String = ApplicationContext.BaseURL
        If (Right(ret, 1) <> "/") Then ret &= "/"
        path = Trim(path)
        If (Left(path, 1) = "/") Then path = Mid(path, 2)
        Return ret & path
    End Function

    Public ReadOnly Property CurrentPage As CCurrentPage
        Get
            Return New CCurrentPage
        End Get
    End Property

    Private Shared m_Instance As WebSite = Nothing

    Public Shared ReadOnly Property Instance As WebSite
        Get
            If (m_Instance Is Nothing) Then m_Instance = New WebSite
            Return m_Instance
        End Get
    End Property


End Class
 