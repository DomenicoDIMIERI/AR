Imports System.Web
Imports DMD.Sistema

Partial Class WebSite

    Public Class CCurrentSiteSession
        Inherits CSiteSession

        ' Private m_Contents As CKeyCollection

        Public Sub New()

        End Sub

        'Public ReadOnly Property Contents As CKeyCollection
        '    Get
        '        SyncLock Me
        '            If (Me.m_Contents Is Nothing) Then Me.m_Contents = New CKeyCollection
        '            Return Me.m_Contents
        '        End SyncLock
        '    End Get
        'End Property


        Public Sub NotifyEnd()
            Me.EndTime = Now
            If (DMD.Databases.LOGConn.IsOpen AndAlso WebSite.Instance.Configuration.LogSessions) Then Me.Save(True)
            Dim e As New SessionEventArgs(Me)
            WebSite.Sessions.NotifySessionEnd(e)
        End Sub



        Public Sub NotifyStart(ByVal parameters As CKeyCollection)
            Me.StartTime = Calendar.Now
            For Each k As String In parameters.Keys
                Me.Parameters.SetItemByKey(k, parameters(k))
            Next
            Me.SessionID = parameters.GetItemByKey("SessionID") ' ASP_Session.SessionID
            Me.RemoteIP = parameters.GetItemByKey("RemoteIP") ' WebSite.ASP_Request.ServerVariables("REMOTE_ADDR")
            Me.RemotePort = Formats.ToInteger(parameters.GetItemByKey("RemotePort")) ' WebSite.ASP_Request.ServerVariables("REMOTE_PORT")
            Me.UserAgent = parameters.GetItemByKey("RemoteUserAgent") ' WebSite.ASP_Request.ServerVariables("HTTP_USER_AGENT")
            Me.Cookie = parameters.GetItemByKey("SiteCookie") ' Cookies.GetCookie("SiteCookie")
            Me.InitialReferrer = parameters.GetItemByKey("InitialReferrer") ' CStr(ASP_Session("InitialReferrer"))
            If (DMD.Databases.LOGConn.IsOpen AndAlso WebSite.Instance.Configuration.LogSessions) Then Me.Save(True)
            Dim e As New SessionEventArgs(Me)
            WebSite.Sessions.NotifySessionStart(e)
        End Sub


    End Class

End Class
