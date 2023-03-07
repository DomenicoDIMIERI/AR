Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    Public Class Shutdown
        Inherits AsteriskEvent

        Public Sub New()
            MyBase.New("Shutdown")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property Shutdown As String
            Get
                Return Me.GetAttribute("Shutdown")
            End Get
        End Property

        Public ReadOnly Property Restart As String
            Get
                Return Me.GetAttribute("Restart")
            End Get
        End Property
         
    End Class

End Namespace