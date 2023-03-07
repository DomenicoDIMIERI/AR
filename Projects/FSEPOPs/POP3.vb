Imports Finsea.Databases

Public NotInheritable Class Networking

    Public Class POP3

        Public Class POP3Exception
            Inherits System.Exception

            Public Sub New()
                MyBase.New("Errore POP3 generico")
            End Sub

            Public Sub New(ByVal message As String)
                MyBase.New(message)
            End Sub

        End Class

        Public Enum ConnectionState As Integer
            Disconnected = 0
            Connecting = 1
            Connected = 2
        End Enum

        Public Class POP3EventArgs
            Inherits System.EventArgs

        End Class

        Public Class POP3ConnectionErrorEventArgs
            Inherits POP3EventArgs

        End Class

        Public Class POP3MessageEventArgs
            Inherits POP3EventArgs

        End Class

        ''' <summary>
        ''' Client POP3
        ''' </summary>
        ''' <remarks></remarks>
        Public Class Client
            Implements IDisposable

            Public Event ConnectionOpened(ByVal sender As Object, ByVal e As POP3EventArgs)
            Public Event ConnectionClosed(ByVal sender As Object, ByVal e As POP3EventArgs)
            Public Event ConnectionError(ByVal sender As Object, ByVal e As POP3ConnectionErrorEventArgs)
            Public Event MessageReceived(ByVal sender As Object, ByVal e As POP3MessageEventArgs)

            'all the vars for use in the classs
            Private TCP As Net.Sockets.TcpClient
            Private POP3Stream As System.IO.Stream
            Private inStream As System.IO.StreamReader
            Private strDataIn, strNumMains(2) As String
            Private intNoEmails As Integer
            Private m_POP3Server As String
            Private m_POP3Port As Integer
            Private m_UserName As String
            Private m_Status As ConnectionState
            Private m_Messages As New CCollection(Of EmailMessage)

            Public Sub New()
                Me.m_Status = ConnectionState.Disconnected
            End Sub

            Protected Overridable Sub OnConnectionOpened(ByVal e As POP3EventArgs)
                RaiseEvent ConnectionOpened(Me, e)
            End Sub

            Protected Overridable Sub OnConnectionClosed(ByVal e As POP3EventArgs)
                RaiseEvent ConnectionClosed(Me, e)
            End Sub

            Protected Overridable Sub OnConnectionError(ByVal e As POP3ConnectionErrorEventArgs)
                RaiseEvent ConnectionError(Me, e)
            End Sub

            Protected Overridable Sub OnMessageReceived(ByVal e As POP3MessageEventArgs)
                RaiseEvent MessageReceived(Me, e)
            End Sub

            ''' <summary>
            ''' Stabilisce la connessione al server POP3
            ''' </summary>
            ''' <param name="strUserName">[in] Nome utente per l'accesso al server POP3</param>
            ''' <param name="strPassword">[in] Password per l'accesso al server POP3</param>
            ''' <param name="strServer">[in] Nome o indirizzo IP del server POP3</param>
            ''' <param name="serverPort">[in opt] Numero della porta a cui connettersi sul server POP3</param>
            ''' <remarks></remarks>
            Public Sub Connect( _
                        ByVal strUserName As String, _
                        ByVal strPassword As String, _
                        ByVal strServer As String, _
                        Optional ByVal serverPort As Integer = 110 _
                                )
                If (Me.m_Status <> ConnectionState.Disconnected) Then
                    Throw New InvalidOperationException("Stato della connessione non valido")
                End If

                Me.m_POP3Server = Trim(strServer)
                Me.m_POP3Port = serverPort
                Me.m_UserName = Trim(strUserName)

                Me.m_Status = ConnectionState.Connecting
                Me.TCP = New Net.Sockets.TcpClient
                Me.TCP.Connect(Me.m_POP3Server, Me.m_POP3Port)
                'create stream into the ip
                Me.POP3Stream = Me.TCP.GetStream
                Me.inStream = New System.IO.StreamReader(Me.POP3Stream)
                'Make sure we get the ok back from the server
                If Me.WaitFor("+OK") = False Then
                    Me.Dispose()
                    Me.OnConnectionError(New POP3ConnectionErrorEventArgs)
                    Throw New POP3Exception("Unexpected Response from mail server when connecting")
                End If
                'send the email down 
                If Me.SendDataAndWait("USER " & Me.m_UserName, "+OK") = False Then
                    Me.Dispose()
                    Me.OnConnectionError(New POP3ConnectionErrorEventArgs)
                    Throw New POP3Exception("Unexpected Response from mail server when sending user")
                End If
                If Me.SendDataAndWait("PASS " & strPassword, "+OK") = False Then
                    Me.Dispose()
                    Me.OnConnectionError(New POP3ConnectionErrorEventArgs)
                    Throw New POP3Exception("Unexpected Response from mail server when sending Password")
                End If
                Me.m_Status = ConnectionState.Connected
                Me.OnConnectionOpened(New POP3EventArgs)
            End Sub

            ''' <summary>
            ''' Function to get the number of mail messages waiting on the server
            ''' </summary>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Function GetMailStat() As Integer
                'send the stat command and make sure it returns as expected
                If Me.SendDataAndWait("STAT", "+OK") = False Then
                    Me.intNoEmails = -1
                    Me.OnConnectionError(New POP3ConnectionErrorEventArgs)
                    Throw New POP3Exception("Unexpected Response from mail server when Getting No of Messages")
                Else
                    'split up the response. It should be +OK Num of emails size of emails
                    strNumMains = Split(strDataIn, " ")
                    Me.intNoEmails = Formats.ToInteger(strNumMains(1))
                    Return Me.intNoEmails
                End If
            End Function

            ''' <summary>
            ''' function to take in what we expect and compare to what we actually get back
            ''' </summary>
            ''' <param name="strTarget"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Protected Friend Overridable Function WaitFor(ByVal strTarget As String) As Boolean
                Me.strDataIn = inStream.ReadLine
                Return InStr(Me.strDataIn, strTarget) > 0
            End Function

            Public Function CheckForMails() As Integer
                Dim num As Integer = Me.GetMailStat
                Me.m_Messages.Clear()
                For i As Integer = 0 To num - 1
                    Dim msg As New EmailMessage(Me, i)
                    msg.Download()
                    Me.m_Messages.Add(msg)
                Next
                Return Me.m_Messages.Count
            End Function

            Protected Friend Overridable Function ReadLine() As String
                Return Me.inStream.ReadLine
            End Function

            


            'Function that will quit the connection to the server (deleting marked mail) and close open readers etc
            Sub CloseConn()

                Try
                    SendData("QUIT")
                    inStream.Close()
                    POP3Stream.Close()
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try

            End Sub

            ''' <summary>
            ''' function to send data down the tcp pipe
            ''' </summary>
            ''' <param name="strCommand">[in] Comando da inviare al server</param>
            ''' <remarks></remarks>
            Protected Friend Overridable Sub SendData(ByVal strCommand As String)
                Dim outBuff As Byte() = ConvertStringToByteArray(strCommand & vbCrLf)
                Me.POP3Stream.Write(outBuff, 0, strCommand.Length + 2)
            End Sub

            ''' <summary>
            ''' function to send data down the tcp pipe
            ''' </summary>
            ''' <param name="strCommand">[in] Comando da inviare al server</param>
            ''' <remarks></remarks>
            Protected Friend Overridable Function SendDataAndWait(ByVal strCommand As String, Optional ByVal waitFor As String = "+OK") As Boolean
                Me.SendData(strCommand)
                Return Me.WaitFor(waitFor)
            End Function

            Public Shared Function ConvertStringToByteArray(ByVal stringToConvert As String) As Byte()
                Return (New System.Text.ASCIIEncoding).GetBytes(stringToConvert)
            End Function

            Public ReadOnly Property Messages As IEnumerable(Of EmailMessage)
                Get
                    Return Me.m_Messages
                End Get
            End Property

#Region "IDisposable Support"
            Private disposedValue As Boolean ' To detect redundant calls

            ' IDisposable
            Protected Overridable Sub Dispose(disposing As Boolean)
                If Not Me.disposedValue Then
                    If disposing Then
                        If Me.inStream IsNot Nothing Then Me.inStream.Dispose()
                        If Me.POP3Stream IsNot Nothing Then Me.POP3Stream.Dispose()
                        If Me.TCP IsNot Nothing Then Me.TCP.Close()
                    End If

                End If
                Me.m_Messages.Clear()
                Me.inStream = Nothing
                Me.POP3Stream = Nothing
                Me.TCP = Nothing
                Me.m_Status = ConnectionState.Disconnected
                Me.disposedValue = True
            End Sub

            ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
            'Protected Overrides Sub Finalize()
            '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            '    Dispose(False)
            '    MyBase.Finalize()
            'End Sub

            ' This code added by Visual Basic to correctly implement the disposable pattern.
            Public Sub Dispose() Implements IDisposable.Dispose
                ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
                Dispose(True)
                GC.SuppressFinalize(Me)
            End Sub
#End Region

        End Class

        ''' <summary>
        ''' Class to extract the elements from raw email text
        ''' </summary>
        ''' <remarks></remarks>
        Public Class EmailMessage
            Private m_Connection As Client
            Private m_Index As Integer
            Private m_MessageSource As String
            Private m_From As String
            Private m_To As String
            Private m_Body As String
            Private m_Subject As String

            Public Sub New()
            End Sub

            Protected Friend Sub New(ByVal client As Client, ByVal index As Integer)
                Me.New()
                Me.m_Connection = client
                Me.m_Index = index
            End Sub

            Public ReadOnly Property Client As Client
                Get
                    Return Me.m_Connection
                End Get
            End Property

            Public ReadOnly Property Index As Integer
                Get
                    Return Me.m_Index
                End Get
            End Property

            Public ReadOnly Property From As String
                Get
                    Return Me.m_From
                End Get
            End Property

            Public ReadOnly Property [To] As String
                Get
                    Return Me.m_To
                End Get
            End Property

            Public ReadOnly Property Body As String
                Get
                    Return Me.m_Body
                End Get
            End Property

            Public ReadOnly Property Subject As String
                Get
                    Return Me.m_Subject
                End Get
            End Property
            
            ''' <summary>
            ''' function that will mark an email for deletion. Delete does not occur until the QUIT is issued to the server
            ''' </summary>
            ''' <param name="intMailItem"></param>
            ''' <remarks></remarks>
            Public Sub MarkForDelete(ByVal intMailItem As Integer)
                If Me.m_Connection.SendDataAndWait("DELE " & Str(intMailItem), "+OK") = False Then
                    Throw New POP3Exception("Unexpected Response Marking email for deletion")
                End If
            End Sub

            Public Sub Download()
                Dim strTemp As String
                Dim strEmailMess As New System.Text.StringBuilder(2048) ' String = ""
                'send the command to the server to return that email back. Command is RETR and the email no ie RETR 1
                If Me.m_Connection.SendDataAndWait("RETR " & Str(Me.m_Index), "+OK") = False Then
                    Throw New POP3Exception("Unexpected Response from mail server getting email body")
                End If
                'Should be ok at this point to read in the tcp stream. We read it in until the end of the email
                'whitch is signified by a line containing only a fullpoint(chr46)
                strTemp = Me.m_Connection.ReadLine
                While (strTemp <> ".")
                    'strEmailMess = strEmailMess & strTemp & vbCrLf
                    strEmailMess.Append(strTemp & vbCrLf)
                    strTemp = Me.m_Connection.ReadLine
                End While
                'call the functions to get the various parts out of the email 
                strTemp = strEmailMess.ToString
                Me.m_From = Me.ParseEmail(strTemp, "From:")
                Me.m_Subject = Me.ParseEmail(strTemp, "Subject:")
                Me.m_To = Me.ParseEmail(strTemp, "To:")
                Me.m_Body = Me.ParseBody()
            End Sub

            'function that will call the main proc with what to bring back for everything but the body text
            Private Function ParseEmail(ByVal strMessage As String, ByVal strType As String) As String
                m_MessageSource = strMessage
                'call the parse routine with the pass filed we want
                ParseEmail = Me.ParseHeader(strType)
            End Function

            'Function to parse each of the header parts out of the email
            Private Function ParseHeader(ByVal strHeader As String) As String

                Dim intLenToStart As Integer
                Dim intLenToLineEnd As Integer
                Dim strTmp As String

                intLenToStart = (InStr(m_MessageSource, strHeader) - 1)
                intLenToLineEnd = InStr(Mid(m_MessageSource, intLenToStart), vbCrLf)
                strTmp = m_MessageSource.Substring(intLenToStart, intLenToLineEnd)

                ParseHeader = Replace(strTmp, vbCrLf, "")

            End Function

            'Funtion to parse out the email body 
            Private Function ParseBody() As String

                'To get the body, everything after the first null line of the message is it (rfc822)
                Dim strTmp As String

                'set the temp var to the message body by getting everything after the null line
                strTmp = m_MessageSource.Substring(m_MessageSource.IndexOf(vbCrLf + vbCrLf))

                'get the encoding of the message out, that way we know if we have to run it through the base64 decode
                'routine or not
                If InStr(m_MessageSource, "Content-Transfer-Encoding: base64") > 0 Then
                    'call the decode routine
                    strTmp = DecodeBase64(strTmp)
                End If

                'if the jobs got html content, remove that from the body
                If InStr(strTmp, "------_=_NextPart_") Then
                    strTmp = strTmp.Substring(1, strTmp.IndexOf(vbCrLf & vbCrLf & vbCrLf & "------_=_NextPart_"))
                End If


                'Strip out the odd hex that apears at the start and the end
                strTmp = Replace(strTmp, Chr(10) & Chr(9), "")
                strTmp = Replace(strTmp, Chr(13), "")

                ParseBody = Trim(strTmp)

            End Function

            ''' <summary>
            ''' Function that will decode base64 encoded email body
            ''' </summary>
            ''' <param name="strBody"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Private Function DecodeBase64(ByVal strBody As String)
                'Try
                Dim encoding As System.Text.Encoding = System.Text.Encoding.Default
                Dim Buffer As Byte() = Convert.FromBase64String(strBody)
                DecodeBase64 = encoding.GetString(Buffer)
                encoding = Nothing
                Buffer = Nothing
                'Catch ex As Exception
                'MsgBox("A problem occured while decoding a base64 email", MsgBoxStyle.Critical)
                'DecodeBase64 = "ERROR"
                'Exit Function
                'End Try
            End Function


        End Class

    End Class

End Class