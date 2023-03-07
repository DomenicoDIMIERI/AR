Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    ''' <summary>
    ''' Fired when the "RELOAD" console command is executed.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    Public Class Reload
        Inherits AsteriskEvent
         
        Public Sub New()
            MyBase.New("Reload")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property Message As String
            Get
                Return Me.GetAttribute("Message")
            End Get
        End Property
          
    End Class

End Namespace