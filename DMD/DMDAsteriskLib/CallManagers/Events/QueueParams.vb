Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    ''' <summary>
    ''' sent following an Action: Queues
    ''' </summary>
    ''' <remarks></remarks>
    Public Class QueueParams
        Inherits AsteriskEvent
         

        Public Sub New()
            MyBase.New("QueueParams")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property Queue As String
            Get
                Return Me.GetAttribute("Queue")
            End Get
        End Property

        Public ReadOnly Property Max As Integer
            Get
                Return Me.GetAttribute("Max", 0)
            End Get
        End Property

        Public ReadOnly Property Calls As Integer
            Get
                Return Me.GetAttribute("Calls", 0)
            End Get
        End Property

        Public ReadOnly Property Holdtime As Integer
            Get
                Return Me.GetAttribute("Holdtime", 0)
            End Get
        End Property

        Public ReadOnly Property Completed As Integer
            Get
                Return Me.GetAttribute("Completed", 0)
            End Get
        End Property

        Public ReadOnly Property Abandoned As Integer
            Get
                Return Me.GetAttribute("Abandoned", 0)
            End Get
        End Property

        Public ReadOnly Property ServiceLevel As Integer
            Get
                Return Me.GetAttribute("ServiceLevel", 0)
            End Get
        End Property

        Public ReadOnly Property ServicelevelPerf As Single
            Get
                Return Me.GetAttribute("ServicelevelPerf", 0.0F)
            End Get
        End Property
         
    End Class

End Namespace