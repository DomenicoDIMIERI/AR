Imports System
Imports System.Collections.Generic
Imports System.Net.Sockets
Imports System.Text
Imports System.IO

Namespace Net.Mail.Protocols.POP3

    ''' <summary>
    ''' This command represents a Pop3 USER command.
    ''' </summary>
    Friend NotInheritable Class UserCommand
        Inherits Pop3Command(Of Pop3Response)

        Private _username As String

        ''' <summary>
        ''' Initializes a new instance of the <see cref="UserCommand"/> class.
        ''' </summary>
        ''' <param name="stream">The stream.</param>
        ''' <param name="username">The username.</param>
        Public Sub New(ByVal client As Pop3Client, ByVal stream As Stream, ByVal username As String)
            MyBase.New(client, stream, False, Pop3State.Authorization)
            If (String.IsNullOrEmpty(username)) Then Throw New ArgumentNullException("username")
            Me._username = username
        End Sub

        ''' <summary>
        ''' Creates the USER request message.
        ''' </summary>
        ''' <returns>
        ''' The byte[] containing the USER request message.
        ''' </returns>
        Protected Overrides Function CreateRequestMessage() As Byte()
            Return GetRequestMessage(Pop3Commands.User, _username, Pop3Commands.Crlf)
        End Function

    End Class

End Namespace