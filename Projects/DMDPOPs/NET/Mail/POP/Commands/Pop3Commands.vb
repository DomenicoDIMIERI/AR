Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace Net.Mail.Protocols.POP3

    ''' <summary>
    ''' This class contains a string representation of Pop3 commands
    ''' that can be executed.
    ''' </summary>
    Friend NotInheritable Class Pop3Commands

        ''' <summary>
        ''' The USER command followed by a space.
        ''' </summary>
        Public Const User As String = "USER "

        ''' <summary>
        ''' The CRLF escape sequence.
        ''' </summary>
        Public Const Crlf As String = vbCrLf '"\r\n"

        ''' <summary>
        ''' The QUIT command followed by a CRLF.
        ''' </summary>
        Public Const Quit As String = "QUIT" & vbCrLf '\r\n";

        ''' <summary>
        ''' The STAT command followed by a CRLF.
        ''' </summary>
        Public Const Stat As String = "STAT" & vbCrLf '\r\n";

        ''' <summary>
        ''' The LIST command followed by a space.
        ''' </summary>
        Public Const List As String = "LIST "

        ''' <summary>
        ''' The RETR command followed by a space.
        ''' </summary>
        Public Const Retr As String = "RETR "

        ''' <summary>
        ''' The NOOP command followed by a CRLF.
        ''' </summary>
        Public Const Noop As String = "NOOP" & vbCrLf '\r\n";

        ''' <summary>
        ''' The DELE command followed by a space.
        ''' </summary>
        Public Const Dele As String = "DELE "

        ''' <summary>
        ''' The RSET command followed by a CRLF.
        ''' </summary>
        Public Const Rset As String = "RSET" & vbCrLf '\r\n";

        ''' <summary>
        ''' The PASS command followed by a space.
        ''' </summary>
        Public Const Pass As String = "PASS "

        ''' <summary>
        ''' The TOP command followed by a space.
        ''' </summary>
        Public Const Top As String = "TOP "
    End Class

End Namespace