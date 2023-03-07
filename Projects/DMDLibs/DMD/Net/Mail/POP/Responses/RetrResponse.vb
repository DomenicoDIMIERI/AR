Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace Net.Mail.Protocols.POP3

    ''' <summary>
    ''' This class represents a RETR response message resulting
    ''' from a Pop3 RETR command execution against a Pop3 Server.
    ''' </summary>
    Friend NotInheritable Class RetrResponse
        Inherits Pop3Response

        Private _messageLines As String()

        ''' <summary>
        ''' Gets the message lines.
        ''' </summary>
        ''' <value>The Pop3 message lines.</value>
        Public ReadOnly Property MessageLines As String()
            Get
                Return Me._messageLines
            End Get
        End Property

        Private _octects As Long
        Public ReadOnly Property Octets As Long
            Get
                Return Me._octects
            End Get
        End Property

        ''' <summary>
        ''' Initializes a new instance of the <see cref="RetrResponse"/> class.
        ''' </summary>
        ''' <param name="response">The response.</param>
        ''' <param name="messageLines">The message lines.</param>
        Public Sub New(ByVal response As Pop3Response, ByVal messageLines As String())
            MyBase.New(response.ResponseContents, response.HostMessage, response.StatusIndicator)
            If (messageLines Is Nothing) Then Throw New ArgumentNullException("messageLines")
            Dim values As String() = response.HostMessage.Split(" ")
            If (values.Length = 2) Then
                Me._octects = Convert.ToInt64(values(1))
            End If
            Me._messageLines = messageLines
        End Sub

    End Class

End Namespace
