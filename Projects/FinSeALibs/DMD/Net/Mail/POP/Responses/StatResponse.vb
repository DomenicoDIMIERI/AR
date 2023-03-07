Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace Net.Mail.Protocols.POP3

    ''' <summary>
    ''' This class represents the resulting Pop3 response from a STAT command
    ''' executed against a Pop3 server.
    ''' </summary>
    Friend NotInheritable Class StatResponse
        Inherits Pop3Response

        Private _messageCount As Integer
        ''' <summary>
        ''' Gets the message count.
        ''' </summary>
        ''' <value>The message count.</value>
        Public ReadOnly Property MessageCount As Integer
            Get
                Return Me._messageCount
            End Get
        End Property

        Private _octets As Long
        ''' <summary>
        ''' Gets the octets.
        ''' </summary>
        ''' <value>The octets.</value>
        Public ReadOnly Property Octets As Long
            Get
                Return Me._octets
            End Get
        End Property

        ''' <summary>
        ''' Initializes a new instance of the <see cref="StatResponse"/> class.
        ''' </summary>
        ''' <param name="response">The response.</param>
        ''' <param name="messageCount">The message count.</param>
        ''' <param name="octets">The octets.</param>
        Public Sub New(ByVal response As Pop3Response, ByVal messageCount As Integer, ByVal octets As Long)
            MyBase.New(response.ResponseContents, response.HostMessage, response.StatusIndicator)
            Me._messageCount = messageCount
            Me._octets = octets
        End Sub

    End Class

End Namespace
