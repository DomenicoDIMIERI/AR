Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Imports DMD.Sistema
Imports System.Net.Mail

Public NotInheritable Class ADV

    Public Shared Event DatabaseChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    Private Shared m_Database As CDBConnection = Nothing

    Private Sub New()
    End Sub

    Public Shared Sub Initialize()
        Dim h As HandlerTipoCampagna = Campagne.GetHandler(TipoCampagnaPubblicitaria.eMail)
        Campagne.Initialize()
    End Sub

    Public Shared Sub Terminate()
        Campagne.Terminate()
    End Sub

    Public Shared Property Database As CDBConnection
        Get
            If (m_Database Is Nothing) Then Return APPConn
            Return m_Database
        End Get
        Set(value As CDBConnection)
            If (Database Is value) Then Exit Property
            Terminate()
            m_Database = value
            Initialize()
            RaiseEvent DatabaseChanged(Nothing, New System.EventArgs)
        End Set
    End Property

End Class
