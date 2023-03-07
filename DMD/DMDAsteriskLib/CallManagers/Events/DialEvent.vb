Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers


    Public Class DialEvent
        Inherits AsteriskEvent




        Public Sub New()
        End Sub

        Public Sub New(ByVal server As AsteriskCallManager, ByVal rows() As String)
            MyBase.New(server, rows)
        End Sub

        Public Sub New(ByVal server As AsteriskCallManager, ByVal e As AsteriskEvent)
            MyBase.New(server, e)
        End Sub

        Public ReadOnly Property CallerIDNumber As String
            Get
                Return Me.GetAttribute("CALLERIDNUM")
            End Get
        End Property

        Public ReadOnly Property Channel As String
            Get
                Return Me.GetAttribute("CHANNEL")
            End Get
        End Property

        Public ReadOnly Property ConnectedLineName As String
            Get
                Return Me.GetAttribute("CONNECTEDLINENAME")
            End Get
        End Property

        Public ReadOnly Property ConnectedLineNum As String
            Get
                Return Me.GetAttribute("CONNECTEDLINENUM")
            End Get
        End Property

        Public ReadOnly Property Destination As String
            Get
                Return Me.GetAttribute("DESTINATION")
            End Get
        End Property

        Public ReadOnly Property DestUniqueID As String
            Get
                Return Me.GetAttribute("DESTUNIQUEID")
            End Get
        End Property

        Public ReadOnly Property DialString As String
            Get
                Return Me.GetAttribute("DIALSTRING")
            End Get
        End Property

        Public ReadOnly Property SubEvent As String
            Get
                Return Me.GetAttribute("SUBEVENT")
            End Get
        End Property

        Public ReadOnly Property UniqueID As String
            Get
                Return Me.GetAttribute("UNIQUEID")
            End Get
        End Property

    End Class


End Namespace