Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    Public Class Agentcallbacklogoff
        Inherits AsteriskEvent

        Public Sub New()
            MyBase.New("Agentcallbacklogoff")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property Agent As String
            Get
                Return Me.GetAttribute("Agent")
            End Get
        End Property

        Public ReadOnly Property Loginchan As String
            Get
                Return Me.GetAttribute("Loginchan")
            End Get
        End Property

        Public ReadOnly Property UniqueID As String
            Get
                Return Me.GetAttribute("Uniqueid")
            End Get
        End Property

        Public ReadOnly Property LoginTime As String
            Get
                Return Me.GetAttribute("Logintime")
            End Get
        End Property

        Public ReadOnly Property Reason As String
            Get
                Return Me.GetAttribute("Reason")
            End Get
        End Property

         
    End Class

End Namespace