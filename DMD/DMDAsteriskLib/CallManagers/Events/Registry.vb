Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    ''' <summary>
    ''' Fired when Asterisk registers with a peer
    ''' </summary>
    ''' <remarks>
    ''' For an entry like: register => username:password:authname@sip.domain:port/local_contact
    ''' Domain would reflect the value of sip.domain
    ''' </remarks>
    Public Class Registry
        Inherits AsteriskEvent

        Public Sub New()
            MyBase.New("Registry")
        End Sub

        Public Sub New(ByVal rows() As String)
            MyBase.New(rows)
        End Sub

        Public ReadOnly Property Channel As String
            Get
                Return Me.GetAttribute("Channel")
            End Get
        End Property

        Public ReadOnly Property Domain As String
            Get
                Return Me.GetAttribute("Domain")
            End Get
        End Property


        Public ReadOnly Property Status As String
            Get
                Return Me.GetAttribute("Status")
            End Get
        End Property

        Public ReadOnly Property ChannelDriver As String
            Get
                Return Me.GetAttribute("ChannelDriver")
            End Get
        End Property
          
    End Class

End Namespace