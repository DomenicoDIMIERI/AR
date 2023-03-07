Imports FinSeA
Imports FinSeA.Net

Public Class frmMain

    Private WithEvents m_App As New FinSeA.Mail.MailApplication

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        My.Settings.ServerName = Me.txtServer.Text
        My.Settings.UserName = Me.txtUserName.Text
        My.Settings.Password = Me.txtPassword.Text
        My.Settings.Save()

        Try
            Me.m_App.Settings.ArchivePath = "c:\temp\mail"
            Dim account As New FinSeA.Mail.Account(Me.txtServer.Text, Me.txtUserName.Text, Me.txtPassword.Text)
            account.Protocol = "POP3"
            Me.m_App.Accounts.Clear()
            Me.m_App.Accounts.Add(account)
            Me.m_App.DownloadEmails()
        Catch ex As Exception
            MsgBox(ex.ToString, MsgBoxStyle.Critical)
        End Try

#If 0 Then

        'variable declaration
        Dim strMailServer, strUsername, strPassword, strFrom, strSubject, strToo, strBody, strMailContent As String
        Dim popConn As SamplePop3Class.POP3
        Dim mailMess As SamplePop3Class.EmailMessage
        Dim intMessCnt, i As Integer

        'set the variables to the input boxes
        strMailServer = TextBox1.Text
        strUsername = TextBox2.Text
        strPassword = TextBox3.Text

        'make sure some detials have been filled in
        If strMailServer = "" Then
            MsgBox("No Mail server specified!")
            Exit Sub
        End If
        If strUsername = "" Then
            MsgBox("No Username specified!")
            Exit Sub
        End If
        If strPassword = "" Then
            MsgBox("No Password Specified!")
            Exit Sub
        End If

        'Disable the button when proccessing
        Button1.Enabled = False

        'create the objects
        popConn = New SamplePop3Class.POP3
        mailMess = New SamplePop3Class.EmailMessage

        'if we have got to this point, try and connect to the server
        popConn.POPConnect(strMailServer, strUsername, strPassword)

        'now we have a connection, see if there are any mails on the server
        intMessCnt = popConn.GetMailStat()

        'now, see if we have returned any messages
        If intMessCnt > 0 Then

            'clear contents of the list and add the heading
            ListBox1.Items.Clear()

            'if we returned some messages, loop through each one and get the details
            For i = 1 To intMessCnt

                'load the entire content of the mail into a string
                strMailContent = popConn.GetMailMessage(i)

                'call the functions to get the various parts out of the email 
                strFrom = mailMess.ParseEmail(strMailContent, "From:")
                strSubject = mailMess.ParseEmail(strMailContent, "Subject:")
                strToo = mailMess.ParseEmail(strMailContent, "To:")
                strBody = mailMess.ParseBody()

                'add email details to the list box
                ListBox1.Items.Add(strFrom & ", " & strSubject)

                'un-comment to display full details of the email in a message box
                'MsgBox("From: " & strFrom & vbNewLine & "Too: " & strToo & vbNewLine & "Subject: " & strSubject & _
                'vbNewLine & "Body: " & strBody)

                'call close method to delete the emails.
                If CheckBox1.Checked = True Then
                    'WARNING - uncommening the line below WILL remove the message from the mail server
                    'popConn.MarkForDelete(i)
                End If
            Next i
        End If

        'Quit the connection to the server
        popConn.CloseConn()
#End If

        Button1.Enabled = True

    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.txtServer.Text = My.Settings.ServerName
        Me.txtUserName.Text = My.Settings.UserName
        Me.txtPassword.Text = My.Settings.Password
    End Sub

    Private Sub m_App_EmailReceived(sender As Object, e As FinSeA.Mail.EmailEventArg) Handles m_App.EmailReceived
        'add email details to the list box
        ListBox1.Items.Add(e.Message.From.ToString & " -> " & e.Message.Subject)
        System.Windows.Forms.Application.DoEvents()
    End Sub
End Class
