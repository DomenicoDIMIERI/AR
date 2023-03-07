Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    
    Public Class DNDState
        Inherits AsteriskEvent

        Public Sub New()
            MyBase.New("DNDState")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub


        Public ReadOnly Property Channel As String
            Get
                Return Me.GetAttribute("Channel")
            End Get
        End Property

        Public ReadOnly Property Status As String
            Get
                Return Me.GetAttribute("Status")
            End Get
        End Property
         
    End Class

End Namespace