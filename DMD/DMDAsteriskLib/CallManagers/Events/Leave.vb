Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    Public Class Leave
        Inherits AsteriskEvent

        Public Sub New()
            MyBase.New("Leave")
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
         
        Public ReadOnly Property Count As Integer
            Get
                Return Me.GetAttribute("Count", 0)
            End Get
        End Property
         
    End Class

End Namespace