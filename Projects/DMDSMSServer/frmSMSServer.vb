Imports FinSeA
Imports FinSeA.Sistema
Imports FinSeASMSServer .
Imports FinSeA.Databases

Public Class frmSMSServer



    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        Me.Visible = True
    End Sub

    Private Sub frmSMSServer_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

    End Sub

    Public Sub Attiva()
        Dim path As String = Sistema.ApplicationContext.SystemDataFolder & "\log.mdb"
        If (Not Sistema.FileSystem.FileExists(path)) Then
            Sistema.FileSystem.CopyFile(My.Application.Info.DirectoryPath & "\db\log.mdb", path)
        End If
        Dim db As New CMdbDBConnection()
        db.Path = path
        db.OpenDB()
        SMSGateway.Database = db

        Dim modem As New SMSGSMModem
        modem.COMPort.PortName = "COM8"
        modem.Open()

        SMSGateway.Modem = modem
    End Sub

    Private Sub frmSMSServer_Load(sender As Object, e As EventArgs) Handles Me.Load
        Sistema.SetApplicationContext(New ApplicationContext)
        Me.Attiva()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim col As CCollection(Of SMSMessage) = Remote.GetMessagesToSend
        For Each sms As SMSMessage In col
            Dim m As SMSMessage = SMSGateway.GetMessageByMsgID(sms.MessageID)
            If (m Is Nothing) Then
                sms.DataRicezioneServer = Calendar.Now
                Me.log("Nuovo messaggio a: " & Formats.FormatPhoneNumber(sms.NumeroDestinatario) & ": " & sms.Messaggio)
                SMSGateway.SendMessage(sms)
            End If
        Next
    End Sub

    Private Sub Log(ByVal text As String)
        Me.TextBox1.Text = Me.TextBox1.Text & FinSeA.Sistema.Formats.FormatUserDateTime(Calendar.Now) & vbTab & text & vbNewLine
    End Sub
End Class
