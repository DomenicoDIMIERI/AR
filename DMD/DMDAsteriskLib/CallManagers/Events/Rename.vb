Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    
    Public Class Rename
        Inherits AsteriskEvent

        Public Sub New()
            MyBase.New("Rename")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property Oldname As String
            Get
                Return Me.GetAttribute("Oldname")
            End Get
        End Property

        Public ReadOnly Property Newname As String
            Get
                Return Me.GetAttribute("Newname")
            End Get
        End Property
         
        Public ReadOnly Property CallerID As String
            Get
                Return Me.GetAttribute("CallerID")
            End Get
        End Property
         
    End Class

End Namespace