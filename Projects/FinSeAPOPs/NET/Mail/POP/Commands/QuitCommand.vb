Imports System
Imports System.Collections.Generic
Imports System.Net.Sockets
Imports System.IO
Imports System.Text

Namespace Net.Mail.Protocols.POP3

    ''' <summary>
    ''' This class represents the Pop3 QUIT command.
    ''' </summary>
    Friend NotInheritable Class QuitCommand
        Inherits Pop3Command(Of Pop3Response)

        ''' <summary>
        ''' Initializes a new instance of the <see cref="QuitCommand"/> class.
        ''' </summary>
        ''' <param name="stream">The stream.</param>
        Public Sub New(ByVal client As Pop3Client, ByVal stream As Stream)
            MyBase.New(client, stream, False, Pop3State.Transaction Or Pop3State.Authorization)
        End Sub

        ''' <summary>
        ''' Creates the Quit request message.
        ''' </summary>
        ''' <returns>
        ''' The byte[] containing the QUIT request message.
        ''' </returns>
        Protected Overrides Function CreateRequestMessage() As Byte()
            Return GetRequestMessage(Pop3Commands.Quit)
        End Function

    End Class

End Namespace