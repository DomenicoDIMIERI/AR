Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers

    ''' <summary>
    ''' Informazioni sull'accesso ad un callmanager
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ManagerLogoutEventArgs
        Inherits System.EventArgs

        Private m_UserName As String
        Private m_Status As String

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_UserName = ""
        End Sub

        Public Sub New(ByVal userName As String, ByVal status As String)
            Me.New
            Me.m_UserName = userName
            Me.m_Status = status
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public ReadOnly Property UserName As String
            Get
                Return Me.m_UserName
            End Get
        End Property

        Public ReadOnly Property Status As String
            Get
                Return Me.m_Status
            End Get
        End Property



    End Class

End Namespace