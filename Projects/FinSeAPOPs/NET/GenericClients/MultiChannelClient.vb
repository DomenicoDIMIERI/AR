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
Imports FinSeA.Net.GenericClient.Commands

Namespace Net.GenericClient

 

    ''' <summary>
    ''' Client su cui si basano i client Telnet, POP3, IMAP, ecc
    ''' </summary>
    Public MustInherit Class MultichannelClient
        Inherits GenericClient

    End Class

End Namespace
