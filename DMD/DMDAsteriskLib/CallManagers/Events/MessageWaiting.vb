Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    
    Public Class MessageWaiting
        Inherits AsteriskEvent

        
        Public Sub New()
            MyBase.New("MessageWaiting")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property Mailbox As String
            Get
                Return Me.GetAttribute("Mailbox")
            End Get
        End Property

        Public ReadOnly Property Waiting As Integer
            Get
                Return Me.GetAttribute("Waiting", 0)
            End Get
        End Property

        Public ReadOnly Property [New] As Integer
            Get
                Return Me.GetAttribute("New", 0)
            End Get
        End Property

        Public ReadOnly Property Old As Integer
            Get
                Return Me.GetAttribute("Old", 0)
            End Get
        End Property
         
    End Class

End Namespace