Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace Net.Mail.Protocols.POP3

    ''' <summary>
    ''' This class contains the Positive and Negative starting response strings
    ''' that can be returned from a Pop3 server.
    ''' </summary>
    Friend NotInheritable Class Pop3Responses

        ''' <summary>
        ''' The +OK starting of a positive response from the server.
        ''' </summary>
        Friend Const Ok As String = "+OK"

        ''' <summary>
        ''' The -ERR starting of a negative response from the server.
        ''' </summary>
        Friend Const Err As String = "-ERR"
    End Class

End Namespace