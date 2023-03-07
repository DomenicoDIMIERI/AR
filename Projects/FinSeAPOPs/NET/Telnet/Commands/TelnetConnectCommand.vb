Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Net.Sockets
Imports System.Net.Security
Imports System.Security.Authentication
Imports FinSeA.Net.Mime
Imports FinSeA.Net.GenericClient
Imports FinSeA.Net.GenericClient.Commands


Namespace Net.Telnet.Commands

    Public Class TelnetConnectCommand
        Inherits ServerCommand

        Public Sub New()
        End Sub

        Protected Overrides Function CreateRequestMessage() As Byte()
            Return Nothing
        End Function

        Protected Overrides Function CreateResponse() As ServerResponse
            Return New TelnetConnectResponse
        End Function
    End Class

End Namespace