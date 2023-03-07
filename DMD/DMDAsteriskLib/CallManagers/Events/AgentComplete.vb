Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    Public Class AgentComplete
        Inherits AsteriskEvent

        Public Sub New()
            MyBase.New("AgentComplete")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property Queue As String
            Get
                Return Me.GetAttribute("Queue")
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

        Public ReadOnly Property Member As String
            Get
                Return Me.GetAttribute("Member")
            End Get
        End Property

        Public ReadOnly Property MemberName As String
            Get
                Return Me.GetAttribute("MemberName")
            End Get
        End Property

        Public ReadOnly Property HoldTime As String
            Get
                Return Me.GetAttribute("HoldTime")
            End Get
        End Property

        Public ReadOnly Property TalkTime As String
            Get
                Return Me.GetAttribute("TalkTime")
            End Get
        End Property

        Public ReadOnly Property Reason As String
            Get
                Return Me.GetAttribute("Reason")
            End Get
        End Property
         
    End Class

End Namespace