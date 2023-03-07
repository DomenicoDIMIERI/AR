Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    Public Class HangupEvent
        Inherits AsteriskEvent

        Public Sub New()
            MyBase.New("Hangup")
        End Sub

        Public Sub New(ByVal m As AsteriskCallManager, ByVal e As AsteriskEvent)
            MyBase.New(m, e)
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property Channel As String
            Get
                Return Me.GetAttribute("Channel")
            End Get
        End Property

        Public ReadOnly Property Uniqueid As String
            Get
                Return Me.GetAttribute("Uniqueid")
            End Get
        End Property

        Public ReadOnly Property Cause As HangupCauses
            Get
                Return Me.GetAttribute("Cause", 0)
            End Get
        End Property

        Public ReadOnly Property CauseTxt As String
            Get
                Return Me.GetAttribute("Cause-txt")
            End Get
        End Property

        Public ReadOnly Property CauseEx As String
            Get
                Return [Enum].GetName(GetType(HangupCauses), Me.Cause)
            End Get
        End Property


        Public Shared Function GetCauseName(ByVal cause As HangupCauses) As String
            Return [Enum].GetName(GetType(HangupCauses), cause)
        End Function
    End Class

End Namespace