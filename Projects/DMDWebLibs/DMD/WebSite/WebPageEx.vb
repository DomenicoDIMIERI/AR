#Const DEBUGDELAY = 0
Imports Microsoft.VisualBasic
Imports System.Web
Imports System.Web.UI
Imports System.Data
Imports System.Text.RegularExpressions
Imports DMD
Imports DMD.Sistema
Imports DMD.WebSite
Imports System.Threading
Imports System.ComponentModel
Imports System.IO
Imports System.Timers

Partial Class WebSite

    Public Class WebPageEx
        Inherits System.Web.UI.Page

        Public req As reqInfo

        'Protected WebLock As New Object
        Public Shared DELAYTONOTIFY As Integer = 5000

        Private m_CurrentPage As CCurrentPage
        Private m_CurrentModule As DMD.Sistema.CModule
        Private Shared requestCount As Integer = 0

        Private m_a As String
        Private m_FunName As String
        'Private m_GCInfo As StatsItem

        'Public Shared pagesLock As New Object
        'Public Shared PageStats As New CKeyCollection(Of StatsItem)


        'Public Shared Function GetRequestID() As Integer
        '    If (requestCount = Integer.MaxValue) Then
        '        requestCount = Integer.MinValue
        '    Else
        '        requestCount += 1
        '    End If
        '    Return requestCount
        'End Function



        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.CheckWebSite()

        End Sub


        ''' <summary>
        ''' Restituisce il contesto dell'applicazione "Sistema.ApplicationContext" convertito nel tipo WebApplicationContext
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ApplicationContext As WebApplicationContext
            Get
                Return DirectCast(Sistema.ApplicationContext, WebApplicationContext)
            End Get
        End Property



        ''' <summary>
        ''' Effettua i controlli sul sito
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Sub CheckWebSite()
        End Sub


        ''' <summary>
        ''' Restituisce vero se l'accesso alla pagina genera un log
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function IsLogEnabled() As Boolean
            Return WebSite.Instance.Configuration.LogPages
        End Function

        ''' <summary>
        ''' Restituisce vero se l'accesso alla pagina richiede che l'utente effettui il login
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function RequiresLogin() As Boolean
            Return False
        End Function




        ''' <summary>
        ''' Restituisce o imposta il modulo corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable ReadOnly Property CurrentModule As CModule
            Get
                If (Me.m_CurrentModule Is Nothing) Then
                    Dim m As String = Me.GetParameter("_m", "")
                    If IsNumeric(m) Then
                        Me.m_CurrentModule = Modules.GetItemById(CInt(m))
                    Else
                        Me.m_CurrentModule = Modules.GetItemByName(m)
                    End If
                End If
                Return Me.m_CurrentModule
            End Get
        End Property

        Public Overridable ReadOnly Property RequestedAction As String
            Get
                If (Me.m_a = "") Then Me.m_a = DMD.Sistema.Strings.Trim(Me.GetParameter("_a", ""))
                Return Me.m_a
            End Get
        End Property


        Public Function IsUserLogged() As Boolean
            Dim sessionID As String = Me.CurrentSession.SessionID
            If (sessionID = "") Then Return False
            Dim user As CUser = DirectCast(DMD.Sistema.ApplicationContext, WebApplicationContext).CurrentUser(sessionID)
            Select Case LCase(user.UserName)
                Case "system", "guest" : Return False
                Case Else : Return True
            End Select
        End Function



        Public Overridable Function IsAllowedIP(ByVal value As String) As Boolean
            Dim ret As Boolean = False
            Dim lastUserIP As String = Trim(Session("LastUserIP"))
            If (lastUserIP <> value) Then
                'Prima convalida dell'IP
                Dim info As IPADDRESSinfo = WebSite.Instance.GetIPAllowInfo(value)
                If (info Is Nothing) Then
                    ret = False
                Else
                    ret = info.Allow AndAlso Not info.Negate
                End If
                Session("LastUserIP_Res") = ret
                Session("LastUserIP") = value
            ElseIf (lastUserIP = value) Then
                'L'IP è già stato convalidato in questa sessione
                ret = CBool(Session("LastUserIP_Res"))
            End If
            Return ret
        End Function

        ''' <summary>
        ''' Questa funzione viene richiamanta per indicare un tentativo di accesso anomalo al sistema
        ''' da un IP che è cambiato durante la sessione
        ''' </summary>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function SegnalaIPAnomalo(ByVal value As String, ByVal consentito As Boolean) As Boolean
            Return False
        End Function

        Public Overridable Function IsNegateIP(ByVal value As String) As Boolean
            Return WebSite.Instance.IsIPNegated(value)
        End Function

        Public Overridable Function IsAllowedCertificate(ByVal value As HttpClientCertificate) As Boolean
            If value.IsPresent = False Then Return False
            If (value.ValidFrom > Now OrElse value.ValidUntil < Now) Then Return False
            Return True
        End Function



        'Restituisce vero se l'orario di sistema indica 
        Public Function IsWorkTime() As Boolean
            'Dim days, intervals, i
            'Dim wd, hh, hh1, hh2, t

            'days = Split(WORKDAYS, ", ")
            'wd = WeekDay(Now, 2)
            'hh = CDbl(Hour(Now)) + CDbl(Minute(Now)) / 60

            't = False
            'i = 0
            'While (i <= UBound(days)) And Not t
            '    t = (wd = CInt(days(i)))
            '    i = i + 1
            'End While
            'If t Then
            '    intervals = Split(WORKHOURS, ", ")
            '    t = False
            '    i = 0
            '    While (i <= UBound(intervals)) And Not t
            '        Dim times, nibbles
            '        times = Split(intervals(i), ": ")
            '        nibbles = Split(times(0), ".")
            '        hh1 = CDbl(nibbles(0)) + CDbl(nibbles(1)) / 60
            '        nibbles = Split(times(1), ".")
            '        hh2 = CDbl(nibbles(0)) + CDbl(nibbles(1)) / 60
            '        t = (hh >= hh1) And (hh <= hh2)
            '        i = i + 1
            '    End While
            'End If

            'IsWorkTime = t
            Return True
        End Function

        ''' <summary>
        ''' Verifica il protocollo di connessione (tipo HTTP o HTTPs)
        ''' </summary>
        Protected Overridable Sub SecurityCheckProtocol()

        End Sub

        ''' <summary>
        ''' Effettua i controlli per l'esecuzione della pagina
        ''' </summary>
        Protected Overridable Sub PerformSecurityChecks()
            Me.SecurityCheckProtocol()
            Me.SecurityCheckMaintenance()
            Me.SecurityCheckTimeRestrictions()
            Me.SecurityCheckRemoteIP()
            Me.SecurityCheckValidUser()
        End Sub

        ''' <summary>
        ''' Verifica che l'utente che ha effettuato la richiesta remota sia valido (SystemUser non può effettuare richieste remote)
        ''' </summary>
        Protected Overridable Sub SecurityCheckValidUser()
            If Users.CurrentUser.UserName = Users.KnownUsers.SystemUser.UserName Then
                Me.NotifyUnhautorized("Metodo di accesso non valido")
            End If
        End Sub

        Protected Overridable Sub SecurityCheckMaintenance()
            If WebSite.Instance.IsMaintenance() OrElse DMD.Sistema.FileSystem.FileExists(Server.MapPath("/maintenance.html")) Then
                Me.CurrentPage.TransferredTo("/maintenance.html")
                Server.Transfer("/maintenance.html")
                Response.End()
            End If
        End Sub

        Protected Overridable Sub SecurityCheckTimeRestrictions()
            If WebSite.Instance.Configuration.VerifyTimeRestrictions AndAlso Not IsWorkTime() Then
                Me.NotifyUnhautorized("Accesso non consentito fuori gli orari prestabiliti")
            End If
        End Sub

        Protected Overridable Sub SecurityCheckRemoteIP()
            If Me.AC.IsDebug Then Return

            Dim remoteIP As String = Request.ServerVariables("REMOTE_ADDR")
            'If (Len(remoteIP) < 5) Then remoteIP = "0.0.0.0"

            If (WebSite.Instance.Configuration.VerifyRemoteIP AndAlso Not Me.IsAllowedIP(remoteIP)) Then
                Me.NotifyUnhautorized("Tentativo di accesso da un IP non consentito: " & remoteIP)
            End If
        End Sub

        Protected Overridable Sub SecurityCheckClientCertificate()
            If (WebSite.Instance.Configuration.VerifyClientCertificate AndAlso Not Me.IsAllowedCertificate(Request.ClientCertificate) _
                AndAlso Not IsNegateIP(Request.ServerVariables("REMOTE_ADDR"))) Then
                Me.NotifyUnhautorized("Certificato client non valido o IP bloccato esplicitamente")
            End If
        End Sub

        Public Sub NotifyUnhautorized(ByVal msg As String)
            'Me.CurrentPage.EndExecution("200.403", msg)
            Me.CurrentPage.NotifyUnhautorized(msg)

            'Dim last_msg_unhauth As String = Session("last_msg_unhauth")
            'If (last_msg_unhauth = msg) Then
            '    Response.End()
            '    Return
            'End If
            'Session("last_msg_unhauth") = msg
            Response.Write("FF ")
            Response.Write(msg)
            Response.End()
        End Sub

        Protected Overridable Sub ReadCookies()
            'If Me.GetParameter("interface") <> "0" Then
            'End If
        End Sub

        Protected Overridable Sub WriteCookies()
            Cookies.SetCookie("_SVRTIME", XML.Utils.Serializer.SerializeDate(Now))
        End Sub

        Protected Overrides Sub OnInit(e As EventArgs)
            SyncLock pendingRequests
                req = New reqInfo(Me, Me.GetDefaultTimeOut * 1000)
                pendingRequests.Add(req)
            End SyncLock
            Me.ReadCookies()
            MyBase.OnInit(e)
        End Sub

        ''' <summary>
        ''' Restituisce il timeout predefinito per la pagina (in secondi)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function GetDefaultTimeOut() As Integer
            Dim ret As Integer = WebSite.Instance.Configuration.LongTimeOut
            If (ret <= 1) Then ret = 30
            Return ret
        End Function

        Public ReadOnly Property AC As WebApplicationContext
            Get
                Return Sistema.ApplicationContext
            End Get
        End Property

        'Private requestID As Integer

        'Protected Overridable Sub StartStatsInfo()
        '    Me.m_FunName = Trim(Me.CurrentPage.PageName & "." & Me.GetParameter("_m", "")) & "." & Trim(Me.GetParameter("_a", ""))
        '    Me.m_GCInfo = Me.AC.GetCurrentSessionInfo.BeginPage(Me.m_FunName)
        'End Sub


        Protected Overrides Sub OnPreInit(e As EventArgs)
            'Me.requestID = GetRequestID()
            'Me.StartStatsInfo()
            Dim page As VisitedPage = Me.CurrentPage
            If Me.CheckGZIP Then Me.GZipEncodePage()

            Me.StartExecution()
            If (Me.IsLogEnabled) Then Me.SavePageLogInfo()
            MyBase.OnPreInit(e)
            Me.PerformSecurityChecks()
            Me.WriteCookies()
        End Sub

        Protected Overrides Sub OnLoad(e As EventArgs)
            Me.AC.UpdateCurrentSession()
            MyBase.OnLoad(e)
        End Sub

        ''' <summary>
        ''' Funziona richiata dal metodo OnLoad (se IsLogEnabled restituisce true) allo scopo di salvare le informazioni sulla pagina
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Sub SavePageLogInfo()
            Me.CurrentPage.SaveLog()
        End Sub

        'Protected Overridable Sub EndStatsInfo()
        '    'Try
        '    Me.AC.GetCurrentSessionInfo.EndPage(Me.m_GCInfo)
        '    'Catch ex As Exception
        '    'Throw
        '    'End Try
        'End Sub

        Protected Overrides Sub OnUnload(e As EventArgs)

            If (Me.m_CurrentPage IsNot Nothing) Then
                If (Me.IsLogEnabled) Then Me.SavePageLogInfo()
                If Me.CurrentPage.StatusCode = "" Then
                    Me.EndExecution("200", "")
                Else
                    Me.EndExecution(Me.CurrentPage.StatusCode, Me.CurrentPage.StatusDescription)
                End If
            Else
                Me.AC.Log(Me.ClientQueryString.ToString)
            End If

            SyncLock pendingRequests
                If (Me.req IsNot Nothing) Then pendingRequests.Remove(req)
            End SyncLock

            MyBase.OnUnload(e)
        End Sub

        Protected Overrides Sub OnError(e As EventArgs)
            Try
                SyncLock pendingRequests
                    If (Me.req IsNot Nothing) Then pendingRequests.Remove(req)
                End SyncLock
            Catch ex As Exception

            End Try
            MyBase.OnError(e)
        End Sub


        ''' <summary>
        ''' Restituisce vero se la pagina
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function CheckGZIP() As Boolean
            Return WebSite.Instance.Configuration.CompressResponse
        End Function


        Public NotInheritable Class reqInfo
            'Implements IDisposable

            Public UserName As String
            Public Name As String
            Public page As WebPageEx
            Public Timeout As Integer
            Public StartTime As Date
            Public requetThread As Thread

            Public Sub New(ByVal page As WebPageEx, ByVal timeout As Integer)
                Me.UserName = Sistema.Users.CurrentUser.Nominativo
                Me.Name = page.CurrentPage.Location
                Me.page = page
                Me.Timeout = timeout
                Me.StartTime = Now
                Me.requetThread = System.Threading.Thread.CurrentThread
            End Sub

            Public ReadOnly Property ExecTime As Integer
                Get
                    Return CInt(Math.Floor((Now - Me.StartTime).TotalMilliseconds))
                End Get
            End Property

        End Class

        Private Shared pendingRequests As New System.Collections.ArrayList
        Private Shared WithEvents timer As System.Timers.Timer = InitTimer()


        Private Shared Function InitTimer() As System.Timers.Timer
            Dim ret As New System.Timers.Timer
            ret.Interval = 500
#If Not DEBUG Then
            ret.Enabled = False
#Else
            ret.Enabled = True
#End If

            Return ret
        End Function


        Public Shared Function GetPendingRequests() As reqInfo()
            SyncLock pendingRequests
                Return pendingRequests.ToArray(GetType(reqInfo))
            End SyncLock
        End Function

        Private Shared Sub timer_Elapsed(sender As Object, e As ElapsedEventArgs) Handles timer.Elapsed
            SyncLock pendingRequests
                If Sistema.ApplicationContext.IsDebug Then Return


                Dim i As Integer = 0
                While (i < pendingRequests.Count)
                    Dim req As reqInfo = pendingRequests(i)
                    'req.ExecTime += Me.timer.Interval
                    If (req.ExecTime > req.Timeout) Then
                        pendingRequests.RemoveAt(i)

                        Sistema.ApplicationContext.Log(Formats.FormatUserDateTime(Now) & " - " & req.UserName & " - Timeout dello script: " & req.Name & " (" & req.Timeout & ")")
                        Try
                            req.page.Response.Clear()
                            req.page.Response.Write("0F - TimeoutException" & vbCrLf & "Timeout dello script:  " & req.Name & " (" & req.Timeout & ")")
                            'req.page.Response.Flush()
                            req.page.Response.End()
                        Catch ex As Exception

                        End Try

                        Try
                            req.requetThread.Abort()

                        Catch ex As Exception

                        End Try
                        'req.Dispose()
                        req = Nothing
                    Else
                        i += 1
                    End If
                End While
            End SyncLock
        End Sub



        Protected Overrides Sub Render(writer As HtmlTextWriter)
            Dim startTime As Date = Now


            If Me.AC.IsDebug Then
                Me.InternalRender(writer)
            Else
                Try
                    Me.InternalRender(writer)
                Catch ex As Exception
                    Throw
                Finally

                End Try
            End If




            'req.Dispose
            req = Nothing
            Dim durata As TimeSpan = (Now - startTime)
            If durata.TotalMilliseconds > DELAYTONOTIFY Then
                Me.AC.Log(Formats.FormatUserDateTime(Now) & " - " & Sistema.Users.CurrentUser.UserName & " - Ritardo Script: " & Me.CurrentPage.Location & " (" & Formats.FormatUserDateTime(startTime) & " - " & Formats.FormatDurata(durata.TotalMilliseconds / 1000) & ")")
            End If
        End Sub



        Protected Overridable Sub InternalRender(ByVal writer As HtmlTextWriter)
            MyBase.Render(writer)
        End Sub


        'Public Function GetParameter1(ByVal paramName As String) As String
        '    Return Me.GetParameter(paramName)
        'End Function

        Public Function GetParameter(Of T As Structure)(ByVal paramName As String, Optional ByVal defValue As Object = Nothing) As Nullable(Of T)
            If (Not IsParameterSet(paramName)) Then Return defValue
            Dim tp As System.Type = GetType(T)
            Dim param As String = Me.GetParameter(paramName, "")
            Dim ret As Object = Nothing
            If (tp) Is GetType(String) Then ret = Formats.ToString(param)
            If (tp) Is GetType(Boolean) Then ret = Formats.ParseBool(param)
            If (tp) Is GetType(Integer) Then ret = Formats.ParseInteger(param)
            If (tp) Is GetType(Double) Then ret = Formats.ParseDouble(param)
            If (tp) Is GetType(Decimal) Then ret = Formats.ParseValuta(param)
            If (tp) Is GetType(Date) Then ret = Formats.ParseDate(param)
            Return ret
        End Function

        Public Function GetParameter(ByVal paramName As String, ByVal defValue As String) As String
            Dim keys As Specialized.NameObjectCollectionBase.KeysCollection = Nothing
            If Request.QueryString.HasKeys Then
                keys = Request.QueryString.Keys
                For Each key As String In keys
                    If key = paramName Then Return Request.QueryString(paramName)
                Next
            End If

            If Request.Form.HasKeys Then
                keys = Request.Form.Keys
                For Each key As String In keys
                    If key = paramName Then Return Request.Form(paramName)
                Next
            End If
            Return defValue
        End Function


        Public Function IsParameterSet(ByVal paramName As String) As Boolean
            Dim keys As Specialized.NameObjectCollectionBase.KeysCollection = Nothing
            If Request.QueryString.HasKeys Then
                keys = Request.QueryString.Keys
                For Each key As String In keys
                    If key = paramName Then Return True
                Next
            End If
            If Request.Form.HasKeys Then
                keys = Request.Form.Keys
                For Each key As String In keys
                    If key = paramName Then Return True
                Next
            End If
            Return False
        End Function



        ''' <summary>
        ''' Restituisce vero se la richiesta proviene da un dispositivo mobile
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsMobileDevice() As Boolean
            If (IsIPad() Or IsIPhone()) Then Return True
            Dim u As String = Trim(Request.ServerVariables("HTTP_USER_AGENT"))
            Dim b As New Regex("(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|iphone|ipdad|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase)
            Dim v As New Regex("1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase)
            Return b.IsMatch(u) Or v.IsMatch(Left(u, 4))
        End Function

        ''' <summary>
        ''' Restituisce vero se il dispositivo remoto viene riconosciuto come un iPad
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsIPad() As Boolean
            Dim str As String = Trim(Request.ServerVariables("HTTP_USER_AGENT"))
            Dim uA As CUserAgent = UserAgents.GetItemByString(str)
            If (uA Is Nothing) Then
                Return InStr(str, "ipad", CompareMethod.Text) > 0
            Else
                Return LCase(uA.Device) = "ipad"
            End If
        End Function

        ''' <summary>
        ''' Restituisce vero se il dispositivo remoto viene riconosciuto come un iPhone
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsIPhone() As Boolean
            Dim str As String = Trim(Request.ServerVariables("HTTP_USER_AGENT"))
            Dim uA As CUserAgent = UserAgents.GetItemByString(str)
            If (uA Is Nothing) Then
                Return InStr(str, "iphone", CompareMethod.Text) > 0
            Else
                Return LCase(uA.Device) = "iphone"
            End If
        End Function

        ''' <summary>
        ''' Determines if GZip is supported
        ''' </summary>
        ''' <returns></returns>
        Public Function IsGZipSupported() As Boolean
            Dim AcceptEncoding As String = HttpContext.Current.Request.Headers("Accept-Encoding")
            Return (Not String.IsNullOrEmpty(AcceptEncoding) AndAlso (AcceptEncoding.Contains("gzip") Or AcceptEncoding.Contains("deflate")))
        End Function

        ''' <summary>
        ''' Sets up the current page or handler to use GZip through a Response.Filter
        ''' IMPORTANT:  
        ''' You have to call this method before any output is generated!
        ''' </summary>
        Public Sub GZipEncodePage()
            If (IsGZipSupported()) Then
                Dim Response As HttpResponse = HttpContext.Current.Response
                Dim AcceptEncoding As String = HttpContext.Current.Request.Headers("Accept-Encoding")
                If (AcceptEncoding.Contains("gzip")) Then
                    Response.Filter = New System.IO.Compression.GZipStream(Response.Filter, System.IO.Compression.CompressionMode.Compress)
                    Response.AppendHeader("Content-Encoding", "gzip")
                Else
                    Response.Filter = New System.IO.Compression.DeflateStream(Response.Filter, System.IO.Compression.CompressionMode.Compress)
                    Response.AppendHeader("Content-Encoding", "deflate")
                End If
            End If
        End Sub


        ''' <summary>
        ''' Restituisce un oggetto che descrive la pagina corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CurrentPage As CCurrentPage
            Get
                If Me.m_CurrentPage Is Nothing Then
                    Me.m_CurrentPage = New CCurrentPage
                    Me.m_CurrentPage.Initialize(Me)
                End If
                Return Me.m_CurrentPage
            End Get
        End Property

        Public ReadOnly Property CurrentSession As CCurrentSiteSession
            Get
                Return WebSite.Instance.CurrentSession
            End Get
        End Property

        'Private m_TimeStart As Date
        'Private m_GCStart As Long

        Public Overridable Sub StartExecution()
            'Me.m_TimeStart = Now
            'Me.m_GCStart = GC.GetTotalMemory(False)
            Me.CurrentPage.StartExecution()
        End Sub

        Public Overridable Sub EndExecution(ByVal status As String, ByVal message As String)

            Me.CurrentPage.EndExecution(status, message)
            Me.m_CurrentPage = Nothing
            ' Me.m_CurrentModule = Nothing


        End Sub

        ''' <summary>
        ''' Questo metodo viene richiamato prima di eseguire un'azione e può essere usato per log o per convalidare
        ''' </summary>
        ''' <param name="actionName"></param>
        ''' <param name="context"></param>
        ''' <remarks></remarks>
        Protected Overridable Sub ValidateActionBeforeRun(ByVal actionName As String, ByVal context As Object)

        End Sub

        Public Overrides Sub Dispose()
            MyBase.Dispose()
            Me.m_a = vbNullString
            Me.m_CurrentModule = Nothing
            Me.m_CurrentPage = Nothing

            Me.m_FunName = vbNullString

            Me.Finalize()
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class



End Class