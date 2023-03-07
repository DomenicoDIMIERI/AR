Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    Public Class QueueMemberPaused
        Inherits AsteriskEvent

        Public Sub New()
            MyBase.New("QueueMemberPaused")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property Queue As String
            Get
                Return Me.GetAttribute("Queue")
            End Get
        End Property

        Public ReadOnly Property Location As String
            Get
                Return Me.GetAttribute("Location")
            End Get
        End Property

        Public ReadOnly Property MemberName As String
            Get
                Return Me.GetAttribute("MemberName")
            End Get
        End Property

        Public ReadOnly Property Paused As Integer
            Get
                Return Me.GetAttribute("Paused", 0)
            End Get
        End Property
         
    End Class

End Namespace