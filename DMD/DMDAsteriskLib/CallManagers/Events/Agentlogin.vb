Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    Public Class Agentlogin
        Inherits AsteriskEvent

        Public Sub New()
            MyBase.New("Agentlogin")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property Agent As String
            Get
                Return Me.GetAttribute("Agent")
            End Get
        End Property

        Public ReadOnly Property UniqueID As String
            Get
                Return Me.GetAttribute("Uniqueid")
            End Get
        End Property

        Public ReadOnly Property Channel As String
            Get
                Return Me.GetAttribute("Channel")
            End Get
        End Property
          
    End Class

End Namespace