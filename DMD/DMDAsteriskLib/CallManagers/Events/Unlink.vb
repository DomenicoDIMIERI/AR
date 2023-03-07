Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    Public Class Unlink
        Inherits AsteriskEvent
         
        Public Sub New()
            MyBase.New("Unlink")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property Channel1 As String
            Get
                Return Me.GetAttribute("Channel1")
            End Get
        End Property

        Public ReadOnly Property Channel2 As String
            Get
                Return Me.GetAttribute("Channel2")
            End Get
        End Property

        Public ReadOnly Property UniqueID1 As String
            Get
                Return Me.GetAttribute("UniqueID1")
            End Get
        End Property

        Public ReadOnly Property UniqueID2 As String
            Get
                Return Me.GetAttribute("UniqueID2")
            End Get
        End Property

        Public ReadOnly Property CallerID1 As String
            Get
                Return Me.GetAttribute("CallerID1")
            End Get
        End Property

        Public ReadOnly Property CallerID2 As String
            Get
                Return Me.GetAttribute("CallerID2")
            End Get
        End Property

         
    End Class

End Namespace