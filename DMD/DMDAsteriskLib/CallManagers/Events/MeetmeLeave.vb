Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    
    Public Class MeetmeLeave
        Inherits AsteriskEvent

        Public Sub New()
            MyBase.New("MeetmeLeave")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property Channel As String
            Get
                Return Me.GetAttribute("Channel")
            End Get
        End Property

        Public ReadOnly Property Meetme As String
            Get
                Return Me.GetAttribute("Meetme")
            End Get
        End Property

        Public ReadOnly Property Usernum As String
            Get
                Return Me.GetAttribute("Usernum")
            End Get
        End Property

        Public ReadOnly Property UniqueID As String
            Get
                Return Me.GetAttribute("UniqueID")
            End Get
        End Property
         
    End Class

End Namespace