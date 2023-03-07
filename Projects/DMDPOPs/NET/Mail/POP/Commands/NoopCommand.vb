Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Net.Sockets

Namespace Net.Mail.Protocols.POP3

    ''' <summary>
    ''' This class represents the Pop3 NOOP command.
    ''' </summary>
    Friend NotInheritable Class NoopCommand
        Inherits Pop3Command(Of Pop3Response)

        ''' <summary>
        ''' Initializes a new instance of the <see cref="NoopCommand"/> class.
        ''' </summary>
        ''' <param name="stream">The stream.</param>
        Public Sub New(ByVal client As Pop3Client, ByVal stream As Stream)
            MyBase.New(client, stream, False, Pop3State.Transaction)
        End Sub

        ''' <summary>
        ''' Creates the NOOP request message.
        ''' </summary>
        ''' <returns>
        ''' The byte[] containing the NOOP request message.
        ''' </returns>
        Protected Overrides Function CreateRequestMessage() As Byte()
            Return GetRequestMessage(Pop3Commands.Noop)
        End Function
    End Class

End Namespace
