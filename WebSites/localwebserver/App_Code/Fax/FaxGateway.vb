Imports Atom8.Communications.Fax
Imports System.Net.Mail

Namespace FinSeA


    Public Class FaxGateway

        Public Class Config
            Private Shared m_ServerName As String = "127.0.0.1"
            Private Shared m_ServerPort As Integer = 4559
            Private Shared m_UserName As String = "root"
            Private Shared m_Password As String = ""
            Private Shared m_DialPrefix As String = "9"
            Private Shared m_NotifyEMail As String = ""
            Private Shared m_FaxFolder As String = ""

            Public Shared Property ServerName As String
                Get
                    Return m_ServerName
                End Get
                Set(value As String)
                    m_ServerName = value
                End Set
            End Property

            Public Shared Property ServerPort As Integer
                Get
                    Return m_ServerPort
                End Get
                Set(value As Integer)
                    m_ServerPort = value
                End Set
            End Property

            Public Shared Property UserName As String
                Get
                    Return m_UserName
                End Get
                Set(value As String)
                    m_UserName = value
                End Set
            End Property

            Public Shared Property Password As String
                Get
                    Return m_Password
                End Get
                Set(value As String)
                    m_Password = value
                End Set
            End Property

            Public Shared Property DialPrefix As String
                Get
                    Return m_DialPrefix
                End Get
                Set(value As String)
                    m_DialPrefix = value
                End Set
            End Property

            Public Shared Property NotifyEMail As String
                Get
                    Return m_NotifyEMail
                End Get
                Set(value As String)
                    m_NotifyEMail = value
                End Set
            End Property
        End Class

        Private Shared m_Database As DBConnection = Nothing


        Private Sub New()
        End Sub

        Shared Sub New()

        End Sub


        Public Shared Property Database As DBConnection
            Get
                Return m_Database
            End Get
            Set(value As DBConnection)
                m_Database = value
            End Set
        End Property

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

        Private Shared Sub handleMailReceived(ByVal sender As Object, ByVal e As MailMessageEventArgs)
            'Dim account As CEmailAccount = Nothing

            Dim msg As MailMessage = e.Message
            'For Each acc As CEmailAccount In Me.Accounts
            '    For Each ma As MailAddressEx In msg.To
            '        If (acc.POPUserName = ma.Address) Then
            '            account = acc
            '            Exit For
            '        End If
            '    Next
            'Next
            'If (account Is Nothing) Then Exit Sub
            Dim targets As New System.Collections.ArrayList
            For Each ma As MailAddress In msg.To
                targets.Add(ma)
            Next
            For Each ma As MailAddress In msg.CC
                targets.Add(ma)
            Next
            For Each ma As MailAddress In msg.Bcc
                targets.Add(ma)
            Next

            Dim t As Boolean = False
            For Each ma As MailAddress In targets
                If Config.NotifyEMail = ma.Address Then
                    t = True
                    Exit For
                End If
            Next

            If (t = False) Then Exit Sub


            Dim faxID As String = ""
            Dim senderName As String = ""
            Dim senderNumber As String = ""
            Dim jobStatus As String = ""
            Dim i As Integer
            Dim subject As String = msg.Subject
            Const strFind As String = "Fax attached (ID: fax"
            Const strFindDOC As String = "Fax attached (ID: doc"

            If (Strings.Left(subject, Len(strFind)) = strFind) Then
                'faxID = Strings.Mid(subject, Len(strFind) + 1, Len(subject) - Len(strFind) - 1)
                'Dim text As String = ""

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
                '    text = msg.Body
                '    Me.Parse(text, senderName, senderNumber)
                'End If
                ''Fax sent from "0825867361". The phone number is 0825867361. This email has a fax attached with ID fax000000044. Final status of fax job: done 
                'If (senderNumber = "") Then Exit Sub


                'Dim fullPath As String = ""
                'For Each att As Attachment In msg.Attachments
                '    If att.ContentStream.Length > 0 Then
                '        Dim fName As String = att.Name
                '        Dim folder As String = System.IO.Path.Combine(ApplicationContext.SystemDataFolder, "Received Fax Documents")
                '        FileSystem.CreateRecursiveFolder(folder)

                '        Dim baseName As String = FileSystem.GetBaseName(fName)
                '        Dim extension As String = FileSystem.GetExtensionName(fName)
                '        i = 1
                '        fullPath = System.IO.Path.Combine(folder, fName)
                '        While (FileSystem.FileExists(fullPath))
                '            fullPath = System.IO.Path.Combine(folder, baseName & "_" & i & "." & extension)
                '            i += 1
                '        End While
                '        att.SaveToFile(fullPath)
                '    End If
                'Next



                'Dim f As New FaxJob
                'Me.SetDriver(f, Me)
                'Me.SetOptions(f, Me.GetDefaultOptions)
                'Me.SetDate(f, msg.DeliveryDate)

                'f.Options.RecipientName = modem.eMailRicezione ' account.POPUserName
                'f.Options.SenderName = senderName
                'f.Options.SenderNumber = senderNumber
                'f.Options.FileName = fullPath
                'f.Tag = modem
                'f.Options.ModemName = modem.Name

                'Me.doFaxReceived(f)
            ElseIf (Strings.Left(subject, Len(strFindDOC)) = strFindDOC) Then
                'faxID = Strings.Mid(subject, Len(strFindDOC) + 1) ', Len(subject) - Len(strFindDOC) - 4)
                'If faxID.EndsWith(")") Then faxID = faxID.Substring(0, faxID.Length - 1)
                'i = faxID.IndexOf(".")
                'If (i > 0) Then faxID = faxID.Substring(0, i)

                'Dim text As String = ""
                'Dim stato As String = ""

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
                '    text = msg.Body
                '    Me.ParseSent(text, stato)
                'End If

                'Dim f As FaxJob = Nothing
                'SyncLock Me.outQueueLock
                '    For Each job As FaxJob In Me.OutQueue
                '        If job.Options.GetValueString("RemoteID") = faxID Then
                '            f = job
                '            Exit For
                '        End If
                '    Next
                'End SyncLock

                'If (f Is Nothing) Then Exit Sub

                'Select Case stato
                '    Case ""
                '    Case "done"
                '        Me.doFaxDelivered(f)
                '    Case Else
                '        'If (Left(stato, Len("requeued:")) = "requeued:") Then
                '        'Me.doFaxChangeStatus(f, FaxJobStatus.QUEUED, stato)
                '        'Else
                '        If (f.JobStatus = FaxJobStatus.QUEUED) Then Me.doFaxError(f, stato)
                '        'End If

                'End Select

            End If

        End Sub


        Public Shared Function GetFax(ByVal order_id As String) As FaxDocument
            Dim ret As FaxDocument = Nothing
            Dim dbSQL As String = "SELECT * FROM [tbl_Fax] WHERE [MessageID]='" & Replace(order_id, "'", "''") & "'"
            Dim dbRis As System.Data.IDataReader = Database.ExecuteReader(dbSQL)
            If (dbRis.Read) Then
                ret = New FaxDocument
                ret.Load(dbRis)
            End If
            dbRis.Dispose()
            Return ret
        End Function




        Public Shared Function SendFax(ByVal targetNumber As String, ByVal docFile As String, Optional ByVal jobInfo As String = "") As String
            Dim hf As Hylafax
            hf = New Hylafax("VOTT-FOFS-SESN-TETH", Config.ServerName, Config.ServerPort, Config.UserName, Config.Password)
            ' hf.IsPassive = False

            Dim objHFJS As New HylafaxJobSettings()
            'objHFJS.Comments = options.Comments
            objHFJS.FaxNumber = Config.DialPrefix & targetNumber
            'objHFJS.FromCompany = options.SenderName
            'objHFJS.FromLocation = options.FromLocation
            'objHFJS.FromUser = options.FromUser
            'objHFJS.FromVoice = options.FromVoice
            'objHFJS.JobGroupID
            objHFJS.JobInfo = jobInfo
            'objHFJS.JobPriority()
            'objHFJS.KillTime
            'objHFJS.MaximumDials = options.MaximumDials
            'objHFJS.MaximumTries = options.MaximumTries
            'objHFJS.Modem = options.GetValueString("HylaFaxModemGroup", "any")
            If (Config.NotifyEMail <> "") Then objHFJS.NotifyEmailAddress = Config.NotifyEMail
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


            'any other settings.... 

            'submit the job to be faxed...
            Dim strFaxID As String = hf.SendFax(docFile, "", objHFJS)
            If (Left(strFaxID, Len("Success:")) = "Success:") Then
                Dim p As Integer = InStr(strFaxID, "=")
                strFaxID = Trim(Mid(strFaxID, p + 1))
                hf = Nothing
            Else
                hf = Nothing
                Throw New Exception("Errore: " & strFaxID)
            End If

            Return strFaxID
        End Function

        Public Shared Sub CancelJob(jobID As String)
            Dim hf As Hylafax
            hf = New Hylafax("VOTT-FOFS-SESN-TETH", Config.ServerName, Config.ServerPort, Config.UserName, Config.Password)
            hf.CancelFax(jobID)
            hf = Nothing
        End Sub

        Public Shared Function GetJobStatus(jobID As String) As HylafaxJob
            Dim hf As Hylafax
            hf = New Hylafax("VOTT-FOFS-SESN-TETH", Config.ServerName, Config.ServerPort, Config.UserName, Config.Password)
            Dim job As HylafaxJob = hf.GetFaxJob(jobID)
            hf = Nothing
            Return job
        End Function


    End Class

End Namespace