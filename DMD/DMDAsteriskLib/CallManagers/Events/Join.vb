Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    Public Class Join
        Inherits AsteriskEvent

        Public Sub New()
            MyBase.New("Join")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property Channel As String
            Get
                Return Me.GetAttribute("Channel")
            End Get
        End Property

        Public ReadOnly Property Queue As String
            Get
                Return Me.GetAttribute("Queue")
            End Get
        End Property

        Public ReadOnly Property CallerID As String
            Get
                Return Me.GetAttribute("CallerID")
            End Get
        End Property

        Public ReadOnly Property CallerIDName As String
            Get
                Return Me.GetAttribute("CallerIDName")
            End Get
        End Property

        Public ReadOnly Property Position As Integer
            Get
                Return Me.GetAttribute("Position", 0)
            End Get
        End Property

        Public ReadOnly Property Count As Integer
            Get
                Return Me.GetAttribute("Count", 0)
            End Get
        End Property

        Public ReadOnly Property UniqueID As String
            Get
                Return Me.GetAttribute("Uniqueid")
            End Get
        End Property
         
    End Class

End Namespace