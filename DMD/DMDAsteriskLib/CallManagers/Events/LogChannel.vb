Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    
    Public Class LogChannel
        Inherits AsteriskEvent
         

        Public Sub New()
            MyBase.New("LogChannel")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub


        Public ReadOnly Property Channel As String
            Get
                Return Me.GetAttribute("Channel")
            End Get
        End Property

        Public ReadOnly Property Enabled As String
            Get
                Return Me.GetAttribute("Enabled")
            End Get
        End Property

        Public ReadOnly Property Reason As String
            Get
                Return Me.GetAttribute("Reason")
            End Get
        End Property
         
    End Class

End Namespace