Imports System
Imports System.Collections.Generic
Imports System.Net.Sockets
Imports System.IO
Imports System.Text

Namespace Net.Mail.Protocols.POP3

    ''' <summary>
    ''' This class represents the Pop3 PASS command.
    ''' </summary>
    Friend NotInheritable Class PassCommand
        Inherits Pop3Command(Of Pop3Response)

        Private _password As String

        ''' <summary>
        ''' Initializes a new instance of the <see cref="PassCommand"/> class.
        ''' </summary>
        ''' <param name="stream">The stream.</param>
        ''' <param name="password">The password.</param>
        Public Sub New(ByVal client As Pop3Client, ByVal stream As Stream, ByVal password As String)
            MyBase.New(client, stream, False, Pop3State.Authorization)
            If (String.IsNullOrEmpty(password)) Then Throw New ArgumentNullException("password")
            Me._password = password
        End Sub

        ''' <summary>
        ''' Creates the PASS request message.
        ''' </summary>
        ''' <returns>
        ''' The byte[] containing the PASS request message.
        ''' </returns>
        Protected Overrides Function CreateRequestMessage() As Byte()
            Return GetRequestMessage(Pop3Commands.Pass, _password, Pop3Commands.Crlf)
        End Function

    End Class


End Namespace