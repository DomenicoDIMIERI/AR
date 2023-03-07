Imports System
Imports System.Collections.Generic
'Imports System.Net.Sockets
Imports System.Text
Imports System.IO

Namespace Net.Mail.Protocols.POP3

    ''' <summary>
    ''' This class represents the Pop3 DELE command.
    ''' </summary>
    Friend NotInheritable Class DeleCommand
        Inherits Pop3Command(Of Pop3Response)

        Dim _messageId As Integer = Integer.MinValue

        ''' <summary>
        ''' Initializes a new instance of the <see cref="DeleCommand"/> class.
        ''' </summary>
        ''' <param name="stream">The stream.</param>
        ''' <param name="messageId">The message id.</param>
        Public Sub New(ByVal client As Pop3Client, ByVal stream As Stream, ByVal messageId As Integer)
            MyBase.New(client, stream, False, Pop3State.Transaction)
            If (messageId < 0) Then Throw New ArgumentOutOfRangeException("_messageId")
            Me._messageId = messageId
        End Sub

        ''' <summary>
        ''' Creates the DELE request message.
        ''' </summary>
        ''' <returns>
        ''' The byte[] containing the DELE request message.
        ''' </returns>
        Protected Overrides Function CreateRequestMessage() As Byte()
            Return GetRequestMessage(String.Concat(Pop3Commands.Dele, _messageId.ToString(), Pop3Commands.Crlf))
        End Function

    End Class

End Namespace