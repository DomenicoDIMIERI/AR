Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Text
Imports System.Net.Sockets

Namespace Net.Mail.Protocols.POP3

    ''' <summary>
    ''' This class represents a Pop3 response message and
    ''' is intended to be used as a base class for all other
    ''' Pop3Response types.
    ''' </summary>
    Friend Class Pop3Response

        Private _responseContents As Byte()

        ''' <summary>
        ''' Gets the response contents.
        ''' </summary>
        ''' <value>The response contents.</value>
        Friend ReadOnly Property ResponseContents As Byte()
            Get
                Return Me._responseContents
            End Get
        End Property

        Private _statusIndicator As Boolean
        ''' <summary>
        ''' Gets a value indicating whether message was <c>true</c> +OK or <c>false</c> -ERR
        ''' </summary>
        ''' <value><c>true</c> if [status indicator]; otherwise, <c>false</c>.</value>
        Public ReadOnly Property StatusIndicator As Boolean
            Get
                Return Me._statusIndicator
            End Get
        End Property

        Private _hostMessage As String
        ''' <summary>
        ''' Gets the host message.
        ''' </summary>
        ''' <value>The host message.</value>
        Public ReadOnly Property HostMessage As String
            Get
                Return Me._hostMessage
            End Get
        End Property


        ''' <summary>
        ''' Initializes a new instance of the <see cref="Pop3Response"/> class.
        ''' </summary>
        ''' <param name="responseContents">The response contents.</param>
        ''' <param name="hostMessage">The host message.</param>
        ''' <param name="statusIndicator">if set to <c>true</c> [status indicator].</param>
        Public Sub New(ByVal responseContents As Byte(), ByVal hostMessage As String, ByVal statusIndicator As Boolean)
            If (responseContents Is Nothing) Then Throw New ArgumentNullException("responseBuffer")
            If (String.IsNullOrEmpty(hostMessage)) Then Throw New ArgumentNullException("hostMessage")
            Me._responseContents = responseContents
            Me._hostMessage = hostMessage
            Me._statusIndicator = statusIndicator
        End Sub

        ''' <summary>
        ''' Creates the response.
        ''' </summary>
        ''' <param name="responseContents">The response contents.</param>
        ''' <returns></returns>
        Public Shared Function CreateResponse(ByVal responseContents As Byte()) As Pop3Response
            Dim hostMessage As String
            Dim stream As New MemoryStream(responseContents)
            Using reader As New StreamReader(stream)
                hostMessage = reader.ReadLine()
                If (hostMessage Is Nothing) Then
                    Return Nothing
                End If
                Dim success As Boolean = hostMessage.StartsWith(Pop3Responses.Ok)
                Return New Pop3Response(responseContents, hostMessage, success)
            End Using
        End Function

    End Class

End Namespace