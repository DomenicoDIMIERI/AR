Imports Microsoft.VisualBasic
Imports System.Web
Imports System.Web.UI
Imports System.Data
Imports System.Text.RegularExpressions
Imports DMD
Imports DMD.Sistema
Imports DMD.WebSite

Partial Class WebSite

    Public Class WebServiceEx
        Inherits WebPageEx

        'Private m_FunName As String
        ''Public Shared WebServicesStats As New CKeyCollection(Of StatsItem)
        'Private m_SInfo As StatsItem

        Public Sub New()
        End Sub

        'Protected Overrides Sub StartStatsInfo()
        '    Me.m_FunName = Trim(Me.CurrentPage.PageName & "." & Me.GetParameter("_m", "")) & "." & Trim(Me.GetParameter("_a", ""))
        '    m_SInfo = Me.AC.GetCurrentSessionInfo.BeginWebService(Me.m_FunName)
        '    'Me.m_StartTime = Now
        '    'SyncLock pagesLock
        '    '    Dim item As StatsItem = WebServicesStats.GetItemByKey(Me.m_FunName)
        '    '    If (item Is Nothing) Then
        '    '        item = New StatsItem
        '    '        item.Name = Me.m_FunName
        '    '        WebServicesStats.Add(Me.m_FunName, item)
        '    '    End If
        '    '    item.ActiveCount += 1
        '    '    item.Count += 1
        '    'End SyncLock

        'End Sub

        Protected Overrides Sub OnPreInit(e As EventArgs)
            'Me.Response.ContentEncoding = System.Text.Encoding.ASCII
            MyBase.OnPreInit(e)
        End Sub

        Protected Overrides Sub OnLoad(e As EventArgs)

            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            MyBase.OnLoad(e)
        End Sub

        'Protected Overrides Sub EndStatsInfo()
        '    'Dim item As StatsItem
        '    'SyncLock pagesLock
        '    '    item = WebServicesStats.GetItemByKey(Me.m_FunName)
        '    'End SyncLock
        '    'If (item IsNot Nothing) Then
        '    '    Dim exeTime As Double = (Now - Me.m_StartTime).TotalMilliseconds
        '    '    item.ActiveCount -= 1
        '    '    item.MaxExecTime = Math.Max(item.MaxExecTime, exeTime)
        '    '    item.ExecTime += exeTime
        '    'End If
        '    Me.AC.GetCurrentSessionInfo.EndWebService(Me.m_SInfo)
        'End Sub

        Protected Overrides Sub OnUnload(e As EventArgs)
            MyBase.OnUnload(e)
            'SyncLock FunctionStats
            '    Dim item As StatsItem = FunctionStats.GetItemByKey(Me.m_FunName)
            '    If (item IsNot Nothing) Then
            '        Dim exeTime As Double = (Now - Me.m_StartTime).TotalMilliseconds
            '        item.ActiveCount -= 1
            '        item.MaxExecTime = Math.Max(item.MaxExecTime, exeTime)
            '        item.ExecTime += exeTime
            '    End If
            'End SyncLock
            
        End Sub

      

        'I webservice hanno tempi di esecuzione più brevi
        Protected Overrides Function GetDefaultTimeOut() As Integer
            Dim ret As Integer = WebSite.Instance.Configuration.ShortTimeOut
            If (ret <= 1) Then ret = 30
            Return ret
        End Function



        Protected Overrides Sub ReadCookies()
            'MyBase.ReadCookies()

        End Sub

        Protected Overrides Sub WriteCookies()
            'MyBase.WriteCookies()
        End Sub

        
        Protected Overrides Sub SecurityCheckMaintenance()
            If WebSite.Instance.IsMaintenance() OrElse DMD.Sistema.FileSystem.FileExists(Server.MapPath("/maintenance.html")) Then
                Throw New InvalidOperationException("Sistema in manutenzione")
            End If
        End Sub



        Protected Overrides Sub InternalRender(writer As HtmlTextWriter)
            Dim ret As New System.Text.StringBuilder
            If Me.AC.IsDebug Then
                ret.Append(Me.ExecuteAction(Me.RequestedAction, Me))
                Me.SanitarizeResponse(ret)
                writer.Write("00")
                writer.Write(ret.ToString)
            Else
                Try
                    ret.Append(Me.ExecuteAction(Me.RequestedAction, Me))
                    Me.SanitarizeResponse(ret)
                    writer.Write("00")
                    writer.Write(ret.ToString)
                Catch ex As Exception
                    If (TypeOf (ex) Is System.Reflection.TargetInvocationException) AndAlso (ex.InnerException IsNot Nothing) Then ex = ex.InnerException
                    ret.Append (TypeName(ex) )
                    ret.Append(vbNewLine)
                    ret.Append(ex.Message)
                    Me.SanitarizeResponse(ret) '& vbNewLine & ex.StackTrace)
                    writer.Write("01")
                    writer.Write(ret.ToString)
                End Try
            End If
        End Sub

        Protected Overridable Function SanitarizeResponse(ByVal text As System.Text.StringBuilder) As System.Text.StringBuilder
            text.Replace(ChrW(65535), "?")
            Return text
        End Function

        ''' <summary>
        ''' Metodo richiamato per i WebServices
        ''' </summary>
        ''' <param name="actionName"></param>
        ''' <param name="context"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function ExecuteAction(ByVal actionName As String, ByVal context As Object) As String
            If (Me.CurrentModule Is Nothing) Then Throw New ArgumentNullException("Modulo")
            Me.ValidateActionBeforeRun(actionName, context)
            Return Me.CurrentModule.ExecuteAction(actionName, context)
        End Function

       
 

        Public Overrides Function IsLogEnabled() As Boolean
            Return False  ' MyBase.IsLogEnabled()
        End Function

        Public Function GetWebServicesStats() As String
            Dim col As New CCollection(Of StatsItem)
            'SyncLock Me.AC.applicationLock
            '    For Each Session As CSessionInfo In Me.AC.GetAllSessions
            '        For Each info As StatsItem In Session.WebServicesInfo
            '            col.Add(info)
            '        Next
            '    Next
            'End SyncLock
            Dim items As StatsItem() = col.ToArray
            If (Arrays.Len(items) > 0) Then
                Return XML.Utils.Serializer.Serialize(items)
            Else
                Return ""
            End If
        End Function

        Public Function GetPageStats() As String
            Dim col As New CCollection(Of StatsItem)
            'SyncLock Me.AC.applicationLock
            '    For Each Session As CSessionInfo In Me.AC.GetAllSessions
            '        For Each info As StatsItem In Session.PagesInfo
            '            col.Add(info)
            '        Next
            '    Next
            'End SyncLock
            Dim items As StatsItem() = col.ToArray
            If (Arrays.Len(items) > 0) Then
                Return XML.Utils.Serializer.Serialize(items)
            Else
                Return ""
            End If
        End Function

    End Class

     

End Class