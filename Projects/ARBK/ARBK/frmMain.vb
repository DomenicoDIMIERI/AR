Public Class frmMain

    Private m_Loaded As Boolean = False
    Private m_FTP As New FinSeA.Net.FTP.FTPclient

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load

        Try
            Me.txtFTPServer.Text = My.Settings.FTPServer
            Me.txtFTPPort.Text = My.Settings.FTPPort
            Me.txtUserName.Text = My.Settings.FTPUserName
            Me.txtPassword.Text = My.Settings.FTPPassword
            Me.txtLOGPath.Text = My.Settings.BKFolder
            Me.chkAuto.Checked = My.Settings.BKAuto
            Me.m_Loaded = True
        Catch ex As Exception
            MsgBox("Load: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub fieldChangedHandler(sender As Object, e As EventArgs) Handles txtFTPServer.TextChanged, txtFTPPort.TextChanged, txtUserName.TextChanged, txtPassword.TextChanged, txtLOGPath.TextChanged
        Try
            If Not Me.m_Loaded Then Return

            Me.m_FTP.Logout()

            My.Settings.FTPServer = Me.txtFTPServer.Text
            My.Settings.FTPPort = Me.txtFTPPort.Text
            My.Settings.FTPUserName = Me.txtUserName.Text
            My.Settings.FTPPassword = Me.txtPassword.Text
            My.Settings.BKFolder = Me.txtLOGPath.Text
            My.Settings.BKAuto = Me.chkAuto.Checked
            My.Settings.Save()
        Catch ex As Exception
            MsgBox("FieldChanged: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnScarica_Click(sender As Object, e As EventArgs) Handles btnScarica.Click
        Try


            Dim files As String() = {"sitedb.mdb", "teldb.mdb", "cqspddb.mdb", "officedb.mdb", "annotazioni.mdb", "indexdb.mdb", "log.mdb", "sysdb.mdb", "storico.mdb", "teldbsts.mdb"}

            Me.m_FTP.ServerName = Me.txtFTPServer.Text
            Me.m_FTP.ServerPort = Me.txtFTPPort.Text
            Me.m_FTP.Username = Me.txtUserName.Text
            Me.m_FTP.Password = Me.txtPassword.Text
            Me.m_FTP.Login()

            Me.m_FTP.CurrentDirectory = "/areariservata.finsea.net_Backup_Giornaliero/mdb-database"

            Me.Log("Inizio procedura")

            For Each file As String In files
                Me.Log("Download di " & file)
                Me.m_FTP.Download("sysdb.mdb", System.IO.Path.Combine(Me.txtLOGPath.Text, file), True)
            Next

            Me.Log("Procedura terminata")

        Catch ex As Exception
            MsgBox("btnDownload: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Public Sub Log(ByVal text As String)
        Me.txtLog.Text &= Now.ToShortTimeString & " " & text & vbNewLine
        Application.DoEvents()
    End Sub

    Private Sub btnBK_Click(sender As Object, e As EventArgs) Handles btnBK.Click
        With New FolderBrowserDialog
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                Me.txtLOGPath.Text = .SelectedPath
            End If
        End With
    End Sub

    
End Class
