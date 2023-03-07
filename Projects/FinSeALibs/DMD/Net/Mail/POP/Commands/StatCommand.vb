Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Net.Sockets
Imports System.Text

Namespace Net.Mail.Protocols.POP3

    ''' <summary>
    ''' This class represents the Pop3 STAT command.
    ''' </summary>
    Friend NotInheritable Class StatCommand
        Inherits Pop3Command(Of StatResponse)

        ''' <summary>
        ''' Initializes a new instance of the <see cref="StatCommand"/> class.
        ''' </summary>
        ''' <param name="stream">The stream.</param>
        Public Sub New(ByVal client As Pop3Client, ByVal stream As Stream)
            MyBase.New(client, stream, False, Pop3State.Transaction)
        End Sub

        ''' <summary>
        ''' Creates the STAT request message.
        ''' </summary>
        ''' <returns>
        ''' The byte[] containing the STAT request message.
        ''' </returns>
        Protected Overrides Function CreateRequestMessage() As Byte()
            Return GetRequestMessage(Pop3Commands.Stat)
        End Function

        ''' <summary>
        ''' Creates the response.
        ''' </summary>
        ''' <param name="buffer">The buffer.</param>
        ''' <returns>
        ''' The <c>Pop3Response</c> containing the results of the
        ''' Pop3 command execution.
        ''' </returns>
        Protected Overrides Function CreateResponse(ByVal buffer As Byte()) As StatResponse
            Dim response As Pop3Response = Pop3Response.CreateResponse(buffer)
            Dim values As String() = response.HostMessage.Split(" ")

            'should consist of '+OK', 'messagecount', 'octets'
            If (values.Length < 3) Then Throw New Pop3Exception(String.Concat("Invalid response message: ", response.HostMessage))

            Dim messageCount As Integer = Convert.ToInt32(values(1))
            Dim octets As Long = Convert.ToInt64(values(2))

            Return New StatResponse(response, messageCount, octets)
        End Function

    End Class


End Namespace