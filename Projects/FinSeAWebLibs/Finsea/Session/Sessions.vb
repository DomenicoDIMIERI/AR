Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Internals
Imports DMD.WebSite

Namespace Internals

    Public NotInheritable Class CSessionsClass
        Inherits CGeneralClass(Of CSiteSession)

        ''' <summary>
        ''' Evento generato quando viene iniziata una nuova sessione remota
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Public Event SessionStart(ByVal sender As Object, ByVal e As SessionEventArgs)


        ''' <summary>
        ''' Evento generato quando viene terminata una sessione remota
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Public Event SessionEnd(ByVal sender As Object, ByVal e As SessionEventArgs)


        Private m_ActiveSessions As Integer = 0


        Friend Sub New()
            MyBase.New("modSiteSessions", GetType(CSiteSessionsCursor), 0)
        End Sub



        Public Function CountActiveSessions() As Integer
            Return m_ActiveSessions
        End Function

        Public Sub NotifySessionStart(ByVal e As SessionEventArgs)
            m_ActiveSessions += 1
            RaiseEvent SessionStart(Nothing, e)
        End Sub


        Public Sub NotifySessionEnd(ByVal e As SessionEventArgs)
            m_ActiveSessions -= 1
            RaiseEvent SessionEnd(Nothing, e)
        End Sub

    End Class
End Namespace


Partial Class WebSite



    Private Shared m_Sessions As CSessionsClass = Nothing

    Public Shared ReadOnly Property Sessions As CSessionsClass
        Get
            If (m_Sessions Is Nothing) Then m_Sessions = New CSessionsClass
            Return m_Sessions
        End Get
    End Property

End Class
