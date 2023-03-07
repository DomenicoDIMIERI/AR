Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Public MustInherit Class UserLogEventArgs
    Inherits UserEventArgs

    Private m_Params As String

    Public Sub New()
    End Sub

    Public Sub New(ByVal user As CUser, Optional ByVal params As String = vbNullString)
        MyBase.New(user)
        Me.m_Params = params
    End Sub

    Public ReadOnly Property Params As String
        Get
            Return Me.m_Params
        End Get
    End Property



End Class
