Imports DMD
Imports DMD.Sistema

Namespace Drivers

    Public Class DMDFaxConfiguration
        Inherits FaxDriverOptions


        Public Sub New()
        End Sub

        Public Property ServiceAddress As String
            Get
                Return Me.GetValueString("DMDServiceAddress", "faxserver@DMD.net")
            End Get
            Set(value As String)
                Me.SetValueString("DMDServiceAddress", Strings.Trim(value))
            End Set
        End Property


    End Class

    Public Class DMDFaxDriver
        Inherits BaseFaxDriver



        Public Sub New()
        End Sub

        Public Shadows ReadOnly Property Config As DMDFaxConfiguration
            Get
                Return MyBase.Config
            End Get
        End Property

        Protected Overrides Sub CancelJobInternal(jobID As String)
            Throw New NotSupportedException
        End Sub

        Public Overrides ReadOnly Property Description As String
            Get
                Return "Fin.Se.A. Fax Driver"
            End Get
        End Property

        Protected Overrides Function InstantiateNewOptions() As DriverOptions
            Return New DMDFaxConfiguration
        End Function

        Public Overrides Function GetUniqueID() As String
            Return "FSEAFXDRV"
        End Function






        Protected Overrides Sub InternalSend(job As FaxJob)
            Dim toAddress As String = Me.Config.ServiceAddress
            Dim fromAddress As String = DMD.Sistema.EMailer.Config.SMTPUserName
            Dim subject As String = "MSGID:" & job.JobID & "|SENDTO:" & job.Options.TargetNumber
            Dim m As System.Net.Mail.MailMessage = EMailer.PrepareMessage(fromAddress, toAddress, "", "", subject, "", fromAddress, False)
            Dim a As System.Net.Mail.Attachment = EMailer.AddAttachments(m, job.Options.FileName)
            a.ContentDisposition.Inline = False
#If DEBUG Then
            m.Bcc.Add("tecnico@DMD.net")
#End If
            EMailer.SendMessage(m)
            m.Dispose()
            a.Dispose()
        End Sub


        Protected Overrides Sub InternalConnect()
            MyBase.InternalConnect()
            AddHandler EMailer.MessageReceived, AddressOf handleMessageReceived
        End Sub

        Protected Overrides Sub InternalDisconnect()
            RemoveHandler EMailer.MessageReceived, AddressOf handleMessageReceived
        End Sub

        Private Sub handleMessageReceived(ByVal sender As Object, ByVal e As DMD.Sistema.MailMessageEventArgs)
            Dim reader As System.IO.StreamReader = Nothing

            Try
                Dim msg As System.Net.Mail.MailMessage = e.Message
                Dim text As String

                If msg.From Is Nothing OrElse msg.From.Address <> Me.Config.ServiceAddress Then Exit Sub


                If (msg.IsBodyHtml) Then
                    If Me.Parse(msg.Subject, msg.Body) = False Then
                        For Each view As System.Net.Mail.AlternateView In msg.AlternateViews
                            'If (view.ContentType.MediaType = "text/plain" AndAlso view.ContentStream.Length > 0) Then
                            view.ContentStream.Position = 0
                            reader = New System.IO.StreamReader(view.ContentStream)
                            text = reader.ReadToEnd
                            reader.Dispose() : reader = Nothing

                            If Me.Parse(msg.Subject, text) Then Exit For
                            'End If
                        Next
                    End If

                Else
                    text = msg.Body
                    Me.Parse(msg.Subject, text)
                End If

                'msg.Dispose()
            Catch ex As Exception
                Sistema.ApplicationContext.Log("Errore in DMDFaxDriver")
                Sistema.ApplicationContext.Log(e.ToString)
                Sistema.Events.NotifyUnhandledException(ex)
            Finally
                If (reader IsNot Nothing) Then reader.Dispose() : reader = Nothing

            End Try
        End Sub

        Private Function ParseDate(ByVal value As String) As Nullable(Of Date)
            Return Formats.ParseISODate(value)
        End Function

        Protected Overridable Function Parse(ByVal subject As String, ByVal text As String) As Boolean
            subject = Trim(subject)
            If (subject = "") Then Return False

            Dim items() As String = Split(subject, "|")
            Dim msgid As String = ""
            Dim status As String = ""
            Dim errormsg As String = ""
            Dim dlvtime As Nullable(Of Date) = Nothing
            For i As Integer = 0 To Arrays.Len(items) - 1
                Dim item As String = Trim(items(i))
                If (Left(item, Len("MSGID:")) = "MSGID:") Then
                    msgid = Mid(item, Len("MSGID:") + 1)
                ElseIf (Left(item, Len("STATUS:")) = "STATUS:") Then
                    status = Mid(item, Len("STATUS:") + 1)
                ElseIf (Left(item, Len("ERRORMSG:")) = "ERRORMSG:") Then
                    errormsg = Mid(item, Len("ERRORMSG:") + 1)
                ElseIf (Left(item, Len("DLVTIME:")) = "DLVTIME:") Then
                    dlvtime = Me.ParseDate(Mid(item, Len("DLVTIME:") + 1))
                End If
            Next

            Dim j As FaxJob = Nothing

            Select Case (status)
                Case "QUEUED"
                    SyncLock Me.outQueueLock
                        For Each job As FaxJob In Me.OutQueue
                            If job.JobID = msgid Then
                                j = job
                                Exit For
                            End If
                        Next
                    End SyncLock

                    If (j IsNot Nothing) Then
                        ' Me.setQueueDate(j, dlvtime)
                        Me.doFaxChangeStatus(j, FaxJobStatus.QUEUED, "Queued")
                    End If
                    Return True
                Case "OK"
                    SyncLock Me.outQueueLock
                        For Each job As FaxJob In Me.OutQueue
                            If job.JobID = msgid Then
                                j = job
                                Exit For
                            End If
                        Next
                    End SyncLock
                    If (j IsNot Nothing) Then
                        Me.setDeliveryDate(j, dlvtime)
                        Me.doFaxDelivered(j)
                    End If
                    Return True
                Case "ERROR"
                    SyncLock Me.outQueueLock
                        For Each job As FaxJob In Me.OutQueue
                            If job.JobID = msgid Then
                                j = job
                                Exit For
                            End If
                        Next
                    End SyncLock
                    If (j IsNot Nothing) Then
                        Me.setFailDate(j, dlvtime)
                        Me.doFaxError(j, errormsg)
                    End If
                    Return True
                Case Else
                    Return False
            End Select
        End Function

        
    End Class

End Namespace