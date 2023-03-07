Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    
    Public Class Newexten
        Inherits AsteriskEvent

        Public Sub New()
            MyBase.New("Newexten")
        End Sub
         
        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property Channel As String
            Get
                Return Me.GetAttribute("Channel")
            End Get
        End Property

        Public ReadOnly Property Context As String
            Get
                Return Me.GetAttribute("Context")
            End Get
        End Property

        Public ReadOnly Property Extension As String
            Get
                Return Me.GetAttribute("Extension")
            End Get
        End Property

        Public ReadOnly Property Priority As Integer
            Get
                Return Me.GetAttribute("Priority", 0)
            End Get
        End Property

        Public ReadOnly Property Application As String
            Get
                Return Me.GetAttribute("Application")
            End Get
        End Property

        Public ReadOnly Property AppData As String
            Get
                Return Me.GetAttribute("AppData")
            End Get
        End Property

        Public ReadOnly Property UniqueID As String
            Get
                Return Me.GetAttribute("UniqueID")
            End Get
        End Property
         
    End Class

End Namespace