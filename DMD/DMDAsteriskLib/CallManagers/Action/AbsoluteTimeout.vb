Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports DMD.CallManagers.Responses

Namespace CallManagers.Actions

    ''' <summary>
    ''' This command will request Asterisk to hangup a given channel after the specified number of seconds, thereby effectively ending the active call.
    ''' If the channel is linked with another channel (an active connected call is in progress), the other channel will continue it's path through the dialplan (if any further steps remains).
    ''' </summary>
    ''' <remarks></remarks>
    Public Class AbsoluteTimeout
        Inherits Action

        Private m_Channel As String
        Private m_Timeout As Integer
        
        Public Sub New()
            MyBase.New("AbsoluteTimeout")
        End Sub

        ''' <summary>
        ''' Inizializza un nuovo oggetto
        ''' </summary>
        ''' <param name="channel">[in] Which channel to hangup, e.g. SIP/123-1c20</param>
        ''' <param name="timeout">[in] The number of seconds until the channel should hangup</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal channel As String, ByVal timeout As Integer)
            Me.New()
            Me.m_Channel = Trim(channel)
            Me.m_Timeout = timeout
        End Sub

        ''' <summary>
        ''' Which channel to hangup, e.g. SIP/123-1c20
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Channel As String
            Get
                Return Me.m_Channel
            End Get
        End Property

        ''' <summary>
        ''' The number of seconds until the channel should hangup
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Timeout As Integer
            Get
                Return Me.m_Timeout
            End Get
        End Property

        Protected Overrides Function GetCommandText() As String
            Return MyBase.GetCommandText() & "Channel: " & Me.Channel & vbCrLf & "Timeout: " & Me.Timeout & vbCrLf
        End Function

       

        Protected Overrides Function AllocateResponse(rows() As String) As ActionResponse
            Return New AbsoluteTimeoutResponse(Me)
        End Function

    End Class

End Namespace