Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    Public Enum QueueStatusFlags As Integer
        NotInUse = 1
        InUse = 2
        Busy = 3
        FLAG4 = 4
        Unavailable = 5
        Ringing = 6
    End Enum

    Public Class QueueMember
        Inherits AsteriskEvent
         
        Public Sub New()
            MyBase.New("QueueMember")
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

        Public ReadOnly Property Membership As String
            Get
                Return Me.GetAttribute("Membership")
            End Get
        End Property

        Public ReadOnly Property Penalty As Integer
            Get
                Return Me.GetAttribute("Penalty", 0)
            End Get
        End Property

        Public ReadOnly Property CallsTaken As Integer
            Get
                Return Me.GetAttribute("CallsTaken", 0)
            End Get
        End Property

        Public ReadOnly Property LastCall As Integer
            Get
                Return Me.GetAttribute("LastCall", 0)
            End Get
        End Property

        Public ReadOnly Property Status As QueueStatusFlags
            Get
                Return Me.GetAttribute("Status", 0)
            End Get
        End Property

        Public ReadOnly Property Paused As Integer
            Get
                Return Me.GetAttribute("Paused", 0)
            End Get
        End Property
         
    End Class

End Namespace