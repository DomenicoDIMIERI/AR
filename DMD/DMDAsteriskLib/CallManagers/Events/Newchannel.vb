Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    
    Public Class Newchannel
        Inherits AsteriskEvent

        Public Sub New()
            MyBase.New("Newchannel")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property State As String
            Get
                Return Me.GetAttribute("State")
            End Get
        End Property

        Public ReadOnly Property Channel As String
            Get
                Return Me.GetAttribute("Channel")
            End Get
        End Property

        Public ReadOnly Property CallerID As String
            Get
                Return Me.GetAttribute("CallerID")
            End Get
        End Property

        Public ReadOnly Property CallerIDNum As String
            Get
                Return Me.GetAttribute("CallerIDNum")
            End Get
        End Property

        Public ReadOnly Property CallerIDName As String
            Get
                Return Me.GetAttribute("CallerIDName")
            End Get
        End Property

        Public ReadOnly Property UniqueID As String
            Get
                Return Me.GetAttribute("UniqueID")
            End Get
        End Property
         
    End Class

End Namespace