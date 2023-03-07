Imports System.Net.Sockets
Imports System.Threading
Imports System.Net

Namespace CallManagers.Events

    ''' <summary>
    ''' Fired when two voice channels are linked together and voice data exchange commences.
    ''' </summary>
    ''' <remarks>Several Link events may be seen for a single call. This can occur when Asterisk fails to setup a native bridge for the call. As far as I can tell, this is when Asterisk must sit between two telephones and perform CODEC conversion on their behalf.</remarks>
    Public Class Link
        Inherits AsteriskEvent
         

        Public Sub New()
            MyBase.New("Link")
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


        Public ReadOnly Property BridgeState As String
            Get
                Return Me.GetAttribute("Bridgestate")
            End Get
        End Property

        Public ReadOnly Property BridgeType As String
            Get
                Return Me.GetAttribute("Bridgetype")
            End Get
        End Property

        Public ReadOnly Property TimeStamp As String
            Get
                Return Me.GetAttribute("Timestamp")
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