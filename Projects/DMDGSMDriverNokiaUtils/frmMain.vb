Public Class frmMain

    Private m_Busy As Boolean = False
    Private m_FirstRun As Boolean = True
    Private m_LogBuffer As New System.Text.StringBuilder

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try
           
            Me.SetDB(My.Settings.DBPath)

            Me.Timer1.Interval = 1000
            Me.Timer1.Enabled = True

        Catch ex As Exception
            Me.Log(ex.Message & vbNewLine & ex.StackTrace)
        End Try

    End Sub

    Private Sub btnFindDB_Click(sender As Object, e As EventArgs) Handles btnFindDB.Click
        Dim ofn As New OpenFileDialog
        ofn.Title = "Cerca DB GSM"
        ofn.FileName = Me.txtDB.Text
        If ofn.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Me.SetDB(ofn.FileName)
        End If
        ofn.Dispose()
    End Sub

    Public Sub SetDB(ByVal path As String)
        Try
            Dim conn As New DMD.Databases.CMdbDBConnection
            conn.Path = path
            conn.OpenDB()

            DMD.SMSGateway.Database = conn

            Me.txtDB.Text = path

            My.Settings.DBPath = path
            My.Settings.Save()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Me.Scarica()
    End Sub

    Private Sub Log(ByVal text As String)
        m_LogBuffer.Append(text & vbNewLine)
        Me.txtLog.Text = Me.m_LogBuffer.ToString
    End Sub

    Private Sub txtLog_TextChanged(sender As Object, e As EventArgs) Handles txtLog.TextChanged

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Scarica()
    End Sub

    Public Sub Scarica()
        If (Me.m_Busy) Then Return
        Me.m_Busy = True

        Me.Timer1.Enabled = False
        Me.Log("------------------------------------------------------------")
        Me.Log("Nokia API " & DMD.Nokia.GetInstalledVersion.ToString)
        Me.Log("Ci sono " & DMD.Nokia.Devices.Count & " dispositivi Nokia")

        Try

            'If Not Me.m_FirstRun Then
            '    For Each device As DMD.Nokia.NokiaDevice In DMD.Nokia.Devices
            '        device.SMS.InBox.Messages.Refresh()
            '    Next
            'End If


            For Each dev As DMD.Nokia.NokiaDevice In DMD.Nokia.Devices
                Me.Log("-------------------------")
                Me.Log(" Dispositivo: " & dev.ToString)
                For Each m As DMD.Nokia.CSMSMessage In dev.SMS.InBox.Messages
                    Me.Log(" Messaggio -> " & m.ToString)
                Next
            Next

            DMD.Nokia.Terminate()

            Application.DoEvents()

            DMD.SMSGateway.OutServices.CheckNewMessages()

            Me.m_FirstRun = False

            ' Application.Exit()
        Catch ex As Exception
            Me.Log(ex.Message & vbNewLine & ex.StackTrace)
        End Try

        Me.m_Busy = True = False
        Me.Timer1.Interval = 60 * 1000 * 5
        Me.Timer1.Enabled = True
        Me.Close()

    End Sub
End Class
