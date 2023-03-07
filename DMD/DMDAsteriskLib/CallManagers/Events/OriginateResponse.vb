Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.Asterisk
Imports DMD.CallManagers.Responses

Namespace CallManagers.Events

    Public Class OriginateResponse
        Inherits AsteriskEvent

        Public Sub New()
            MyBase.New("OriginateResponse")
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

        Public ReadOnly Property Exten As String
            Get
                Return Me.GetAttribute("Exten")
            End Get
        End Property

        Public ReadOnly Property Reason As String
            Get
                Return Me.GetAttribute("Reason")
            End Get
        End Property

        Public ReadOnly Property UniqueID As String
            Get
                Return Me.GetAttribute("Uniqueid")
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
         

    End Class

End Namespace