Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Net.Sockets
Imports System.IO

Namespace Net.Mail.Protocols.POP3

    ''' <summary>
    ''' This class represents the Pop3 RETR command.
    ''' </summary>
    Friend NotInheritable Class RetrCommand
        Inherits Pop3Command(Of RetrResponse)

        Private _message As Integer

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RetrCommand"/> class.
        ''' </summary>
        ''' <param name="stream">The stream.</param>
        ''' <param name="message">The message.</param>
        Public Sub New(ByVal client As Pop3Client, ByVal stream As Stream, ByVal message As Integer)
            MyBase.New(client, stream, True, Pop3State.Transaction)
            If (message < 0) Then Throw New ArgumentOutOfRangeException("message")
            Me._message = message
        End Sub

        ''' <summary>
        ''' Creates the RETR request message.
        ''' </summary>
        ''' <returns>
        ''' The byte[] containing the RETR request message.
        ''' </returns>
        Protected Overrides Function CreateRequestMessage() As Byte()
            Return GetRequestMessage(Pop3Commands.Retr, _message.ToString(), Pop3Commands.Crlf)
        End Function

        ''' <summary>
        ''' Creates the response.
        ''' </summary>
        ''' <param name="buffer">The buffer.</param>
        ''' <returns>
        ''' The <c>Pop3Response</c> containing the results of the
        ''' Pop3 command execution.
        ''' </returns>
        Protected Overrides Function CreateResponse(ByVal buffer As Byte()) As RetrResponse
            Dim response As Pop3Response = Pop3Response.CreateResponse(buffer)
            Dim messageLines As String() = GetResponseLines(StripPop3HostMessage(buffer, response.HostMessage))
            Return New RetrResponse(response, messageLines)
        End Function

    End Class

End Namespace