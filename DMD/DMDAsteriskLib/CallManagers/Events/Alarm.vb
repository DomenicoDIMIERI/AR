Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    
    Public Class Alarm
        Inherits AsteriskEvent

        Public Sub New()
            MyBase.New("Alarm")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        ''' <summary>
        ''' (Red|Yellow|Blue|No|Unknown) Alarm|Recovering|Loopback|Not Open|None
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Alarm As String
            Get
                Return Me.GetAttribute("Alarm")
            End Get
        End Property

        Public ReadOnly Property Channel As String
            Get
                Return Me.GetAttribute("Channel")
            End Get
        End Property
         
    End Class

End Namespace