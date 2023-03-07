Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    
    Public Class ParkedCall
        Inherits AsteriskEvent

        Public Sub New()
            MyBase.New("ParkedCall")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property Channel As String
            Get
                Return Me.GetAttribute("Channel")
            End Get
        End Property

        Public ReadOnly Property Exten As String
            Get
                Return Me.GetAttribute("Exten")
            End Get
        End Property

        Public ReadOnly Property From As String
            Get
                Return Me.GetAttribute("From")
            End Get
        End Property

        Public ReadOnly Property Timeout As Integer
            Get
                Return Me.GetAttribute("Timeout", 0)
            End Get
        End Property

        Public ReadOnly Property CallerID As String
            Get
                Return Me.GetAttribute("CallerID")
            End Get
        End Property
          
    End Class

End Namespace