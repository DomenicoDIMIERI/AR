Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net
Imports DMD.Net.Mime
Imports DMD.Databases
Imports DMD.Net.Mail
Imports DMD.Sistema
Imports DMD.Internals

Partial Class Office

    '    Class DownloadWorker
    '        Inherits System.ComponentModel.BackgroundWorker

    '        Private owner As MailApplication
    '        Private nItems As Integer
    '        Private ret As CCollection(Of MailMessage)


    '        Public Sub New(ByVal owner As MailApplication, ByVal nItems As Integer)
    '            Me.owner = owner
    '            Me.nItems = nItems
    '            Me.ret = New CCollection(Of MailMessage)
    '        End Sub

    '        Protected Overrides Sub OnDoWork(e As ComponentModel.DoWorkEventArgs)
    '            If (Me.nItems = 0) Then Return
    '            Dim iAccount As Integer = 0
    '            Do
    '                Dim account As MailAccount = Nothing
    '                SyncLock owner.accountsLock
    '                    If (iAccount < owner.Accounts.Count) Then
    '                        account = owner.Accounts(iAccount)
    '                        iAccount += 1
    '                    End If
    '                End SyncLock
    '                If (account Is Nothing) Then Exit Do

    '                Dim client As DMD.Net.Mail.Pop3Client = Nothing
    '#If Not DEBUG Then
    '                    Try
    '#End If
    '                Select Case account.Protocol
    '                    Case "POP3"
    '                        client = New DMD.Net.Mail.Pop3Client(account.UserName, account.Password, account.ServerName, account.ServerPort, account.UseSSL)
    '                        client.TimeOut = IIf(account.TimeOut <= 100, 100, account.TimeOut)

    '                        Dim folder As MailFolder = Nothing
    '                        If (account.DefaultFolderName <> "") Then folder = Me.owner.GetFolderByName(account.DefaultFolderName)
    '                        If (folder Is Nothing) Then folder = Me.owner.Folders.Inbox
    '                        'Dim ret As New CCollection(Of MailMessage)
    '                        Dim dataInizio As Date = DateUtils.Now
    '                        client.Connect()
    '                        client.Authenticate()
    '                        client.Stat()

    '                        'Recuperiamo la data dell'ultimo messaggio scaricato tramite questo account
    '                        Dim ultimaData As Date? = account.LastStync

    '                        'Scarichiamo i messaggi successivi
    '                        Dim list As System.Collections.Generic.List(Of Protocols.POP3.Pop3ListItem) = client.List
    '                        'Dim toDelete As New CCollection

    '                        Dim item As DMD.Net.Mail.Protocols.POP3.Pop3ListItem
    '                        Dim message As DMD.Net.Mail.MailMessageEx
    '                        Dim msg As MailMessage
    '                        For i As Integer = list.Count - 1 To 0 Step -1
    '                            item = list(i)
    '                            message = client.Top(item.MessageId, 5)
    '                            msg = New MailMessage(Me.owner)
    '                            msg.Account = account
    '                            msg.Folder = folder
    '                            msg.Process(message) ' = New MailMessage(Me, message, account.UserName, folder)
    '                            'If (Not Me.IsDownloaded(msg)) Then
    '                            'If (ultimaData.HasValue = True AndAlso msg.DeliveryDate >= ultimaData.Value) OrElse _
    '                            '   ((ultimaData.HasValue = False OrElse (ultimaData.HasValue AndAlso ultimaData = msg.DeliveryDate)) AndAlso Not Me.IsDownloaded(msg)) Then
    '                            If (Not Me.owner.IsDownloaded(msg)) Then
    '                                message = client.RetrMailMessageEx(item)
    '                                msg.Process(message)
    '                                msg.Stato = ObjectStatus.OBJECT_VALID
    '                                msg.SetFlag(MailFlags.Unread, True)
    '                                msg.Save()
    '                                ret.Add(msg)
    '                                Me.owner.doEmailReceived(New EmailEventArg(msg))
    '                            End If

    '                            If account.DelServerAfterNDays Then
    '                                If msg.DeliveryDate.HasValue AndAlso DateUtils.DateDiff(DateInterval.Day, msg.DeliveryDate.Value, DateUtils.Now) >= account.DelServerAfterDays Then
    '                                    'toDelete.Add(item)
    '                                    client.Dele(item)
    '                                End If
    '                            End If
    '                            Me.OnProgressChanged(New System.ComponentModel.ProgressChangedEventArgs(CDbl(i) * 100 / CDbl(list.Count), Nothing))
    '                            msg.Dispose()
    '                            If (nItems > 0 AndAlso ret.Count >= nItems) Then Exit For
    '                        Next

    '                        'For i As Integer = toDelete.Count - 1 To 0 Step -1
    '                        '    item = toDelete(i)
    '                        '    client.Dele(item)
    '                        'Next

    '                        client.Disconnect()
    '                        client.Dispose()

    '                        account.LastStync = dataInizio
    '                        If (account.ID <> 0) Then account.Save()
    '                    Case Else
    '                        Throw New NotSupportedException("Protocollo e-mail non supportato: [" & account.Protocol & "]")
    '                End Select
    '#If Not DEBUG Then
    '                    Catch ex As Exception
    '                        Me.owner.doDownloadError(New DownloadErrorEventArgs(Me.owner, account, ex, ex.Message))
    '                    Finally
    '#End If
    '                If client IsNot Nothing Then
    '                    client.Dispose()
    '                    client = Nothing
    '                End If
    '#If Not DEBUG Then
    '                End Try
    '#End If
    '            Loop
    '            'Return ret

    '            MyBase.OnDoWork(e)
    '        End Sub

    '    End Class


End Class