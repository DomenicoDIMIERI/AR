Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Text

Namespace Net.Mail.Protocols.POP3

    Friend NotInheritable Class TopCommand
        Inherits Pop3Command(Of RetrResponse)

        Private _messageNumber As Integer
        Private _lineCount As Integer

        ''' <summary>
        ''' Initializes a new instance of the <see cref="TopCommand"/> class.
        ''' </summary>
        ''' <param name="stream">The stream.</param>
        ''' <param name="messageNumber">The message number.</param>
        ''' <param name="lineCount">The line count.</param>
        Friend Sub New(ByVal client As Pop3Client, ByVal stream As Stream, ByVal messageNumber As Integer, ByVal lineCount As Integer)
            MyBase.New(client, stream, True, Pop3State.Transaction)
            If (messageNumber < 1) Then Throw New ArgumentOutOfRangeException("messageNumber")
            If (lineCount < 0) Then Throw New ArgumentOutOfRangeException("lineCount")
            Me._messageNumber = messageNumber
            Me._lineCount = lineCount
        End Sub

        ''' <summary>
        ''' Abstract method intended for inheritors to
        ''' build out the byte[] request message for
        ''' the specific command.
        ''' </summary>
        ''' <returns>
        ''' The byte[] containing the request message.
        ''' </returns>
        Protected Overrides Function CreateRequestMessage() As Byte()
            Return GetRequestMessage(Pop3Commands.Top, _messageNumber.ToString(), " ", _lineCount.ToString(), Pop3Commands.Crlf)
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
            If (response Is Nothing) Then Return Nothing
            Dim messageLines As String() = GetResponseLines(StripPop3HostMessage(buffer, response.HostMessage))
            Return New RetrResponse(response, messageLines)
        End Function

    End Class

End Namespace
