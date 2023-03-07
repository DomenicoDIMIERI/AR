Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Text

Namespace Net.Mail.Protocols.POP3

    Friend NotInheritable Class ConnectResponse
        Inherits Pop3Response

        Private _networkStream As Stream

        Public ReadOnly Property NetworkStream As Stream
            Get
                Return Me._networkStream
            End Get
        End Property

        Public Sub New(ByVal response As Pop3Response, ByVal networkStream As Stream)
            MyBase.New(response.ResponseContents, response.HostMessage, response.StatusIndicator)
            If (networkStream Is Nothing) Then Throw New ArgumentNullException("networkStream")
            Me._networkStream = networkStream
        End Sub

    End Class

End Namespace