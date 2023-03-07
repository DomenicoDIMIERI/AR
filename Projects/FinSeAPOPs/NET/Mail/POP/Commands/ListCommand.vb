Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Net.Sockets
Imports System.Text

Namespace Net.Mail.Protocols.POP3

    ''' <summary>
    ''' This class represents both the multiline and single line Pop3 LIST command.
    ''' </summary>
    Friend NotInheritable Class ListCommand
        Inherits Pop3Command(Of ListResponse)

        ' the id of the message on the server to retrieve.
        Dim _messageId As Integer

        Public Sub New(ByVal client As Pop3Client, ByVal stream As Stream)
            MyBase.New(client, stream, True, Pop3State.Transaction)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ListCommand"/> class.
        ''' </summary>
        ''' <param name="stream">The stream.</param>
        ''' <param name="messageId">The message id.</param>
        Public Sub New(ByVal client As Pop3Client, ByVal stream As Stream, ByVal messageId As Integer)
            MyBase.New(client, stream, False, Pop3State.Unknown)
            If (messageId < 0) Then Throw New ArgumentOutOfRangeException("messageId")
            Me._messageId = messageId
            'MyBase.IsMultiline = False
        End Sub

        ''' <summary>
        ''' Creates the LIST request message.
        ''' </summary>
        ''' <returns>The byte[] containing the LIST request message.</returns>
        Protected Overrides Function CreateRequestMessage() As Byte()
            Dim requestMessage As String = Pop3Commands.List
            If (Not Me.IsMultiline) Then
                requestMessage &= _messageId.ToString()
            End If  ' Append the message id to perform the LIST command for.
            Return GetRequestMessage(requestMessage, Pop3Commands.Crlf)
        End Function

        ''' <summary>
        ''' Creates the response.
        ''' </summary>
        ''' <param name="buffer">The buffer.</param>
        ''' <returns>A <c>ListResponse</c> containing the results of the Pop3 LIST command.</returns>
        Protected Overrides Function CreateResponse(ByVal buffer As Byte()) As ListResponse
            Dim response As Pop3Response = Pop3Response.CreateResponse(buffer)
            Dim items As System.Collections.Generic.List(Of Pop3ListItem)
            If (Me.IsMultiline) Then
                items = New System.Collections.Generic.List(Of Pop3ListItem)
                Dim values As String()
                Dim lines As String() = GetResponseLines(StripPop3HostMessage(buffer, response.HostMessage))

                For Each line As String In lines
                    'each line should consist of 'n m' where n is the message number and m is the number of octets
                    values = line.Split(" ")
                    If (values.Length < 2) Then Throw New Pop3Exception(String.Concat("Invalid line in multiline response:  ", line))

                    items.Add(New Pop3ListItem(Convert.ToInt32(values(0)), Convert.ToInt64(values(1))))
                Next
                'Parse the multiline response.
            Else
                items = New System.Collections.Generic.List(Of Pop3ListItem)(1)
                Dim values As String() = response.HostMessage.Split(" ")

                'should consist of '+OK messageNumber octets'
                If (values.Length < 3) Then
                    Throw New Pop3Exception(String.Concat("Invalid response message: ", response.HostMessage))
                End If
                items.Add(New Pop3ListItem(Convert.ToInt32(values(1)), Convert.ToInt64(values(2))))
            End If  'Parse the single line results.
            Return New ListResponse(response, items)
        End Function

    End Class

End Namespace