Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    Public Class ExtensionStatus
        Inherits AsteriskEvent

        Public Sub New()
            MyBase.New("ExtensionStatus")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property Exten As String
            Get
                Return Me.GetAttribute("Exten")
            End Get
        End Property

        Public ReadOnly Property Context As String
            Get
                Return Me.GetAttribute("Context")
            End Get
        End Property

        Public ReadOnly Property Status As Integer
            Get
                Return Me.GetAttribute("Status")
            End Get
        End Property
         
         
    End Class

End Namespace