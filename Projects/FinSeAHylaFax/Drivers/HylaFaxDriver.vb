Imports DMD.Sistema
Imports Atom8.Communications.Fax
Imports DMD.Net.Mail

Namespace Drivers

    'Public Class HylafaxDriverAddress
    '    Implements DMD.XML.IDMDXMLSerializable

    '    Public Indirizzo As String
    '    Public NomePuntoOperativo As String



    '    Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal

    '    End Sub

    '    Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize

    '    End Sub
    'End Class

    Public Class HylafaxDriverConfiguration
        Inherits FaxDriverOptions

        Public Property HostName As String
            Get
                Return Me.GetValueString("HostName", "")
            End Get
            Set(value As String)
                Me.SetValueString("HostName", Strings.Trim(value))
            End Set
        End Property

        Public Property HostPort As Integer
            Get
                Return Me.GetValueInt("HostPort", 4559)
            End Get
            Set(value As Integer)
                Me.SetValueInt("HostPort", value)
            End Set
        End Property

        Public Property UserName As String
            Get
                Return Me.GetValueString("UserName", "")
            End Get
            Set(value As String)
                Me.SetValueString("UserName", Strings.Trim(value))
            End Set
        End Property

        Public Property Password As String
            Get
                Return Me.GetValueString("Password", "")
            End Get
            Set(value As String)
                Me.SetValueString("Password", value)
            End Set
        End Property

        Public Property MonitorAddressesList As String
            Get
                Return Me.GetValueString("MonitorAddressesList", "")
            End Get
            Set(value As String)
                Me.SetValueString("MonitorAddressesList", Strings.Trim(value))
            End Set
        End Property

    End Class

    Public Class HylaFaxDriver
        Inherits BaseFaxDriver


        '   Private m_ObjHFax As Hylafax
        

        'Private WithEvents m_Timer As System.Timers.Timer
        ' Private m_Accounts As New CCollection(Of CEmailAccount)
        Private m_Connections As New CKeyCollection


        Public Sub New()

        End Sub

        Public Shadows ReadOnly Property Config As HylafaxDriverConfiguration
            Get
                Return MyBase.Config
            End Get
        End Property

        Protected Overrides Sub InternalConnect()
            MyBase.InternalConnect()

            'Dim addressess As String() = Split(Me.Config.MonitorAddressesList, ";")
            'For i As Integer = 0 To Arrays.Len(addressess) - 1
            'Dim addr As String = Strings.Trim(addressess(i))
            'If (EMailer.IsValidAddress(addr)) Then Me.Accounts.Add(New CEmailAccount(addr, ""))
            'Next
            

            ' Me.m_ObjHFax.Request =
            'Me.m_Timer = New System.Timers.Timer
            'Me.m_Timer.Interval = 1000 '* MAILCHECK_INTERVAL
            'Me.m_Timer.Enabled = False

            'For Each account As CEmailAccount In Me.Accounts
            '    Sistema.EMailer.MailAccounts.Add(account)
            'Next

#If 0 Then
            'Sincronizza la coda dei messaggi ricevuti
            SyncLock Me.inqueueLock
                For Each item As HylafaxIncomingQueueItem In Me.m_ObjHFax.IncomingQueue
                    'Dim job As FaxJob = Nothing
                    'For Each job1 As FaxJob In Me.IncomingQueue
                    '    If job1.Options.GetValueString("RemoteID", "") = item.
                    'Next Then

                    '        job.InternalStatus = item
                    '        Me.SetDriver(job, Me)
                    '        Me.SetStatus(job, FaxJobStatus.COMPLETED)
                    '        Me.SetJobID(job, item.Sender)
                    '        Me.IncomingQueue.Add(item.Sender & "_" & item.ReceivedDate, job)
                Next
            End SyncLock

            SyncLock Me.outQueueLock
                For Each item As HylafaxOutgoingQueueItem In Me.m_ObjHFax.OutgoingQueue
                    Dim job As FaxJob = Nothing
                    For Each fax As FaxJob In Me.OutQueue
                        If item.JobID = fax.Options.GetValueString("RemoteID", "") Then
                            job = fax
                            Exit For
                        End If
                    Next
                    If (job Is Nothing) Then
                        job = Me.NewJob
                        Me.SetJobID(job, item.Submitter)
                        Me.OutQueue.Add(job.JobID, job)
                    End If
                    job.InternalStatus = item
                    Me.SetDriver(job, Me)
                Next
            End SyncLock

#End If

            AddHandler Sistema.EMailer.MessageReceived, AddressOf Me.handleMailReceived
        End Sub

        Private Sub Parse(ByVal text As String, ByRef senderName As String, ByRef senderNumber As String)
            text = Replace(text, vbTab, " ")
            text = Replace(text, vbCr, " ")
            text = Replace(text, vbLf, " ")

            senderName = ""
            senderNumber = ""

            Dim items As String() = Strings.Split(text, ".")
            For Each item As String In items
                item = Strings.Trim(item)
                If (Strings.Left(item, Len("Fax sent from ")) = "Fax sent from ") Then
                    senderName = Strings.Mid(item, Len("Fax sent from ") + 1)
                ElseIf (Strings.Left(item, Len("The phone number is ")) = "The phone number is ") Then
                    senderNumber = Strings.Mid(item, Len("The phone number is ") + 1)
                End If
            Next


        End Sub

        Private Sub ParseSent(ByVal text As String, ByRef stato As String)
            text = Replace(text, vbTab, " ")
            text = Replace(text, vbCr, " ")
            text = Replace(text, vbLf, " ")

            stato = ""

            Const STSTR As String = "Final status of fax job:"
            Dim i As Integer = InStr(text, STSTR)
            If (i > 0) Then
                Dim j As Integer = InStr(i, text, vbCr)
                If (j <= 0) Then j = Len(text)
                stato = Trim(Mid(text, i + Len(STSTR) + 1, j - Len(STSTR) - i))
            End If
        End Sub

        Private Sub handleMailReceived(ByVal sender As Object, ByVal e As MailMessageEventArgs)
            'Dim account As CEmailAccount = Nothing
            
            Dim msg As MailMessageEx = e.Message
            'For Each acc As CEmailAccount In Me.Accounts
            '    For Each ma As MailAddressEx In msg.To
            '        If (acc.POPUserName = ma.Address) Then
            '            account = acc
            '            Exit For
            '        End If
            '    Next
            'Next
            'If (account Is Nothing) Then Exit Sub
            Dim targets As New CCollection(Of MailAddressEx)
            For Each ma As MailAddressEx In msg.To
                targets.Add(ma)
            Next
            For Each ma As MailAddressEx In msg.CC
                targets.Add(ma)
            Next
            For Each ma As MailAddressEx In msg.Bcc
                targets.Add(ma)
            Next

            Dim modem As FaxDriverModem = Nothing
            For Each modem1 As FaxDriverModem In Me.Modems
                For Each ma As MailAddressEx In targets
                    If modem1.eMailRicezione = ma.Address Then
                        modem = modem1
                        Exit For
                    End If
                Next
                If (modem IsNot Nothing) Then Exit For
            Next
            If (modem Is Nothing) Then Exit Sub


            Dim faxID As String = ""
            Dim senderName As String = ""
            Dim senderNumber As String = ""
            Dim jobStatus As String = ""
            Dim i As Integer
            Dim subject As String = msg.Subject
            Const strFind As String = "Fax attached (ID: fax"
            Const strFindDOC As String = "Fax attached (ID: doc"

            If (Strings.Left(subject, Len(strFind)) = strFind) Then
                faxID = Strings.Mid(subject, Len(strFind) + 1, Len(subject) - Len(strFind) - 1)
                Dim text As String = ""

                'If (msg.IsBodyHtml) Then
                '    For Each view As System.Net.Mail.AlternateView In msg.AlternateViews
                '        'If (view.ContentType.MediaType = "text/plain" AndAlso view.ContentStream.Length > 0) Then
                '        view.ContentStream.Position = 0
                '        Dim reader As New System.IO.StreamReader(view.ContentStream)
                '        text = reader.ReadToEnd
                '        reader.Dispose()
                '        Me.Parse(text, senderName, senderNumber)
                '        If (senderNumber <> "") Then Exit For
                '        'End If
                '    Next
                'Else
                text = msg.Body
                Me.Parse(text, senderName, senderNumber)
                'End If
                'Fax sent from "0825867361". The phone number is 0825867361. This email has a fax attached with ID fax000000044. Final status of fax job: done 
                If (senderNumber = "") Then Exit Sub


                Dim fullPath As String = ""
                For Each att As AttachmentEx In msg.Attachments
                    If att.ContentStream.Length > 0 Then
                        Dim fName As String = att.Name
                        Dim folder As String = System.IO.Path.Combine(ApplicationContext.SystemDataFolder, "Received Fax Documents")
                        FileSystem.CreateRecursiveFolder(folder)

                        Dim baseName As String = FileSystem.GetBaseName(fName)
                        Dim extension As String = FileSystem.GetExtensionName(fName)
                        i = 1
                        fullPath = System.IO.Path.Combine(folder, fName)
                        While (FileSystem.FileExists(fullPath))
                            fullPath = System.IO.Path.Combine(folder, baseName & "_" & i & "." & extension)
                            i += 1
                        End While
                        att.SaveToFile(fullPath)
                    End If
                Next



                Dim f As New FaxJob
                Me.SetDriver(f, Me)
                Me.SetOptions(f, Me.GetDefaultOptions)
                Me.SetDate(f, msg.DeliveryDate)

                f.Options.RecipientName = modem.eMailRicezione ' account.POPUserName
                f.Options.SenderName = senderName
                f.Options.SenderNumber = senderNumber
                f.Options.FileName = fullPath
                f.Tag = modem
                f.Options.ModemName = modem.Name

                Me.doFaxReceived(f)
            ElseIf (Strings.Left(subject, Len(strFindDOC)) = strFindDOC) Then
                faxID = Strings.Mid(subject, Len(strFindDOC) + 1) ', Len(subject) - Len(strFindDOC) - 4)
                If faxID.EndsWith(")") Then faxID = faxID.Substring(0, faxID.Length - 1)
                i = faxID.IndexOf(".")
                If (i > 0) Then faxID = faxID.Substring(0, i)

                Dim text As String = ""
                Dim stato As String = ""

                'If (msg.IsBodyHtml) Then
                '    For Each view As System.Net.Mail.AlternateView In msg.AlternateViews
                '        'If (view.ContentType.MediaType = "text/plain" AndAlso view.ContentStream.Length > 0) Then
                '        view.ContentStream.Position = 0
                '        Dim reader As New System.IO.StreamReader(view.ContentStream)
                '        text = reader.ReadToEnd
                '        reader.Dispose()
                '        Me.ParseSent(text, stato)
                '        If (stato <> "") Then Exit For
                '        'End If
                '    Next
                'Else
                text = msg.Body
                Me.ParseSent(text, stato)
                'End If

                Dim f As FaxJob = Nothing
                SyncLock Me.outQueueLock
                    For Each job As FaxJob In Me.OutQueue
                        If job.Options.GetValueString("RemoteID") = faxID Then
                            f = job
                            Exit For
                        End If
                    Next
                End SyncLock

                If (f Is Nothing) Then Exit Sub

                Select Case LCase(Trim(stato))
                    Case ""
                    Case "blocked by another job", "no carrier detected"
                        If (f.JobStatus = FaxJobStatus.QUEUED) Then
                            f.Options.Tries += 1
                            If (f.Options.MaximumTries >= f.Options.MaximumTries) Then
                                Me.doFaxError(f, stato)
                            Else
                                Me.doFaxChangeStatus(f, FaxJobStatus.QUEUED, stato)
                            End If
                        End If
                    Case "done"
                        Me.doFaxDelivered(f)
                    Case Else
                        'If (Left(stato, Len("requeued:")) = "requeued:") Then
                        'Me.doFaxChangeStatus(f, FaxJobStatus.QUEUED, stato)
                        'Else
                        If (f.JobStatus = FaxJobStatus.QUEUED) Then Me.doFaxError(f, stato)
                        'End If

                End Select

            End If

        End Sub

        Protected Overrides Sub InternalDisconnect()
            RemoveHandler Sistema.EMailer.MessageReceived, AddressOf Me.handleMailReceived
            'Me.m_Timer.Dispose()
            'Me.m_Timer = Nothing
            'For Each account As CEmailAccount In Me.Accounts
            '    Sistema.EMailer.MailAccounts.Remove(account)
            'Next
        End Sub

        'Public ReadOnly Property Accounts As CCollection(Of CEmailAccount)
        '    Get
        '        Return Me.m_Accounts
        '    End Get
        'End Property

        Public Overrides Function IsConnected() As Boolean
            Return MyBase.IsConnected 'AndAlso (Me.m_ObjHFax IsNot Nothing) AndAlso (Me.m_ObjHFax.IsConnected)
        End Function


        'Public ReadOnly Property CompletedQueue As CKeyCollection(Of FaxJob)
        '    Get
        '        If (Me.m_CompletedQueue Is Nothing) Then
        '            Me.m_CompletedQueue = New CKeyCollection(Of FaxJob)
        '            For Each item As HylafaxCompletedQueueItem In Me.m_ObjHFax.ArchiveQueue
        '                Dim job As FaxJob = Me.NewJob
        '                job.InternalStatus = item
        '                Me.SetJobID(job, item.JobID)
        '                If (item.FaxStatus = "Failed") Then
        '                    Select Case job.JobStatus
        '                        Case FaxJobStatus.QUEUED, FaxJobStatus.SENDING, FaxJobStatus.DIALLING, FaxJobStatus.WAITRETRY
        '                            Me.SetStatus(job, FaxJobStatus.ERROR)
        '                        Case Else
        '                    End Select
        '                ElseIf (item.FaxStatus = "Success") Then
        '                    Select Case job.JobStatus
        '                        Case FaxJobStatus.QUEUED, FaxJobStatus.SENDING, FaxJobStatus.DIALLING, FaxJobStatus.WAITRETRY
        '                            Me.SetStatus(job, FaxJobStatus.COMPLETED)
        '                        Case Else
        '                    End Select
        '                Else
        '                    Debug.Print("Job" & item.FaxStatus)
        '                End If
        '                Me.m_CompletedQueue.Add("Job_" & job.JobID, job)
        '            Next

        '            For Each item As HylafaxCompletedQueueItem In Me.m_ObjHFax.CompletedQueue
        '                Dim job As FaxJob = Me.NewJob
        '                job.InternalStatus = item
        '                Me.SetJobID(job, item.JobID)
        '                If (item.FaxStatus = "Failed") Then
        '                    Select Case job.JobStatus
        '                        Case FaxJobStatus.QUEUED, FaxJobStatus.SENDING, FaxJobStatus.DIALLING, FaxJobStatus.WAITRETRY
        '                            Me.SetStatus(job, FaxJobStatus.ERROR)
        '                        Case Else
        '                    End Select
        '                ElseIf (item.FaxStatus = "Success") Then
        '                    Select Case job.JobStatus
        '                        Case FaxJobStatus.QUEUED, FaxJobStatus.SENDING, FaxJobStatus.DIALLING, FaxJobStatus.WAITRETRY
        '                            Me.SetStatus(job, FaxJobStatus.COMPLETED)
        '                        Case Else
        '                    End Select
        '                Else
        '                    Debug.Print("Job" & item.FaxStatus)
        '                End If
        '                Me.m_CompletedQueue.Add("Job_" & job.JobID, job)
        '            Next
        '        End If
        '        Return Me.m_CompletedQueue
        '    End Get
        'End Property

        Public Overrides ReadOnly Property Description As String
            Get
                Return "HylaFax Server on " & Me.Config.HostName & ":" & Me.Config.HostPort
            End Get
        End Property



        Private m_DefaultOptions As HylafaxDriverConfiguration = Nothing



        Protected Overrides Function InstantiateNewOptions() As DriverOptions
            Return New HylafaxDriverConfiguration
        End Function

        Public Overrides Function GetUniqueID() As String
            Return "HLFXDRV" ' & Me.m_HostName & ":" & Me.m_Port
        End Function

        Protected Overrides Sub InternalSend(job As FaxJob)
            Dim hf As Hylafax
            Dim modem As FaxDriverModem = Me.GetModem(job.Options.ModemName)
            If (modem Is Nothing AndAlso Me.Modems.Count > 0) Then modem = Me.Modems(0)
            If (modem Is Nothing) Then Throw New ArgumentNullException("Modem non installato")
            If (Not modem.SendEnabled) Then Throw New PermissionDeniedException("Il modem " & modem.Name & " non è abilitato all'invio")
            'With Me.Config
            'hf = New Hylafax("VOTT-FOFS-SESN-TETH", .HostName, .HostPort, .UserName, .Password)
            'End With
            With modem
                hf = New Hylafax("VOTT-FOFS-SESN-TETH", .ServerName, .ServerPort, .UserName, .Password)
                'hf.IsPassive = False
            End With

            'Me.m_ObjHFax .SendFax (
            Dim options As FaxDriverOptions = job.Options
            Dim objHFJS As New HylafaxJobSettings()
            'objHFJS.Comments = options.Comments
            objHFJS.FaxNumber = modem.DialPrefix & options.TargetNumber
            'objHFJS.FromCompany = options.SenderName
            'objHFJS.FromLocation = options.FromLocation
            'objHFJS.FromUser = options.FromUser
            'objHFJS.FromVoice = options.FromVoice
            'objHFJS.JobGroupID
            objHFJS.JobInfo = job.JobID
            'objHFJS.JobPriority()
            'objHFJS.KillTime
            'objHFJS.MaximumDials = options.MaximumDials
            'objHFJS.MaximumTries = options.MaximumTries
            'objHFJS.Modem = options.GetValueString("HylaFaxModemGroup", "any")
            If (options.NotifyEmailAddress <> "") Then objHFJS.NotifyEmailAddress = options.NotifyEmailAddress
            'objHFJS.NotifyWhenDone = options.NotifyWhenDone
            'objHFJS.NotifyWhenRequeued = options.NotifyWhenRequeued
            'objHFJS.NumberOfDials = options.NumberOfDials
            'objHFJS.NumberOfPages = options.NumberOfPages
            'objHFJS.NumberOfTries = options.NumberOfTries
            'objHFJS.PageLength = options.PageLength
            'objHFJS.PageWidth = options.PageWidth
            'If (options.SendTime.HasValue) Then
            'objHFJS.SendTime = options.SendTime.Value
            'Else
            ''objHFJS.SendTime = Now
            'End If
            'objHFJS.ToLocation = options.ToLocation
            'objHFJS.ToUser = options.ToUser
            'objHFJS.ToVoice = options.ToVoice
            'objHFJS.ViewResolutions = options.Resolution
            'objHFJS.ToCompany = options.TargetName


            job.InternalStatus = objHFJS
            'any other settings.... 

            'submit the job to be faxed...


            Dim strFaxID As String = hf.SendFax(options.FileName, "", objHFJS)
            If (Left(strFaxID, Len("Success:")) = "Success:") Then
                Dim p As Integer = InStr(strFaxID, "=")
                strFaxID = Trim(Mid(strFaxID, p + 1))
                job.Options.SetValueString("RemoteID", strFaxID)
                'Me.m_Timer.Enabled = True
                hf = Nothing
            Else
                hf = Nothing
                Throw New Exception("Errore: " & strFaxID)
            End If
        End Sub

        Protected Overrides Sub CancelJobInternal(jobID As String)
            SyncLock Me.outQueueLock
                Dim job As FaxJob = Me.OutQueue.GetItemByKey(jobID)
                Dim remoteID As Integer = Formats.ToInteger(job.Options.GetValueString("RemoteID", "0"))

                Dim hf As Hylafax
                With Me.Config
                    hf = New Hylafax("VOTT-FOFS-SESN-TETH", .HostName, .HostPort, .UserName, .Password)
                End With
                hf.CancelFax(remoteID)
                hf = Nothing
            End SyncLock
        End Sub

        'Public Function GetModems() As CCollection(Of HylafaxModem)
        '    'Me.m_ObjHFax.Modems
        '    Return Nothing
        'End Function
        Protected Sub CheckStatus()
            'Debug.Print("Completed: " & Me.m_ObjHFax.CompletedQueueCount)
            ''If (Me.m_ObjHFax.CompletedQueue.Count > 0) Then
            ''    Debug.Print("ID: " & Me.m_ObjHFax.CompletedQueue.First().JobID)
            ''End If
            'Debug.Print("Outgoind: " & Me.m_ObjHFax.OutgoingQueue.Count)
            'If (Me.m_ObjHFax.OutgoingQueue.Count > 0) Then
            '    Debug.Print("ID: " & Me.m_ObjHFax.OutgoingQueue.First().JobID)
            'End If
            'Debug.Print("Incoming: " & Me.m_ObjHFax.IncomingQueue.Count)
            'If (Me.m_ObjHFax.IncomingQueue.Count > 0) Then
            '    Debug.Print("ID: " & Me.m_ObjHFax.IncomingQueue.First.Sender)
            'End If

            'For Each job As FaxJob In Me.m_PendingJobs

            'Next



            'For Each item As HylafaxCompletedQueueItem In Me.m_ObjHFax.ArchiveQueue
            '    Dim job As New FaxJob
            '    Debug.Print("FaxStatus: " & item.FaxStatus)
            'Next

            'For Each item As HylafaxOutgoingQueueItem In Me.m_ObjHFax.OutgoingQueue
            '    Dim job As New FaxJob
            '    Debug.Print("FaxStatus: " & item.CurrentStatus)
            'Next

            'For Each item As HylafaxIncomingQueueItem In Me.m_ObjHFax.IncomingQueue
            '    Dim job As New FaxJob
            '    Debug.Print("FaxStatus: " & item.Sender)
            'Next
        End Sub

        'Private Sub m_Timer_Elapsed(sender As Object, e As Timers.ElapsedEventArgs) Handles m_Timer.Elapsed
        '    ' Me.CheckStatus()
        '    'Sistema.EMailer.CheckMailsAsync()
        'End Sub


    End Class

End Namespace