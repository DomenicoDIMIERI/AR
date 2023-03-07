Imports FinSeA
Imports FinSeA.SMSGateway

Partial Class widgets_websvcf_smsgtw
    Inherits System.Web.UI.Page

    

    Private Const username As String = "dmdsmgtw1"
    Private Const password As String = "232232mdsff22"
    Private Shared lock As New Object

    Private Function GetParameter(ByVal name As String) As String
        Dim ret As String = Request.Form(name)
        If (ret <> "") Then Return ret
        Return Request.QueryString(name)
    End Function

    Private Function SendSMS(writer As HtmlTextWriter) As Integer
        Dim login As String = Trim(Me.GetParameter("u"))
        Dim password As String = Trim(Me.GetParameter("p"))
        Dim message_type As String = Trim(Me.GetParameter("t"))
        Dim recipient As String = Trim(Me.GetParameter("num"))
        Dim message As String = Trim(Me.GetParameter("msg"))
        Dim sender As String = Trim(Me.GetParameter("sender"))
        Dim scheduled_delivery_time As String = Trim(Me.GetParameter("scheduled_delivery_time"))
        Dim order_id As String = Trim(Me.GetParameter("id"))
        'If (login <> username OrElse password <> password) Then Return Me.Terminate(writer, 1, "LOGIN ERROR")

        Dim sms As New SMSMessage
        ' Try
        sms.NomeMittente = sender
        sms.MessageID = order_id
        sms.Modem = login
        sms.NumeroDestinatario = recipient
        sms.Messaggio = message
        sms.StatoInvio = SMSSendStatus.NotSend
        sms.NumeroTentativiInvio = 1
        sms.DataInvioServer = Now
        'sms.Attributi.Add("message_type", message_type)
        'sms.Attributi.Add("scheduled_delivery_time", scheduled_delivery_time)
        sms.Save()
        Try
            SMSGateway.Modem.Open()
        Catch ex As Exception

        End Try

        SMSGateway.Modem.SendSMS(recipient, message) '"*k k#s " &
        SMSGateway.Modem.Close()
        sms.StatoInvio = SMSSendStatus.Sent
        sms.DettaglioStatoInvio = "Inviato"
        sms.Save()
        Return Me.Terminate(writer, ErrorCode.None, "OK")
        'Catch ex As Exception
        '    ' sms.StatoInvio = SMSSendStatus.Error
        '    ' sms.DettaglioStatoInvio = ex.Message
        '    ' sms.Save()
        '    Return Me.Terminate(writer, 255, ex.Message)
        'End Try
    End Function

    Private Function GetSMSStatus(writer As HtmlTextWriter) As Integer
        Dim login As String = Trim(Me.GetParameter("u"))
        Dim password As String = Trim(Me.GetParameter("p"))
        Dim order_id As String = Trim(Me.GetParameter("id"))
        'If (login <> username OrElse password <> password) Then Return Me.Terminate(writer, 1, "LOGIN ERROR")

        Dim sms As SMSMessage = SMSGateway.GetMessageByMsgID(order_id)
        If (sms Is Nothing) Then Return Me.Terminate(writer, 255, "Messaggio non trovato")

        Return Me.Terminate(writer, 0, "OK|" & sms.StatoInvio & "|" & sms.DettaglioStatoInvio & "|" & sms.StatoRicezione & "|" & sms.DettaglioStatoRicezione & "|")
    End Function

    Protected Overrides Sub Render(writer As HtmlTextWriter)
        SyncLock lock
            'If SMSGateway.Modem Is Nothing Then
            '    Dim db As New DBConnection
            '    db.Path = Server.MapPath("/App_Data/db/smslog.mdb")
            '    db.Open()
            '    SMSGateway.Database = db

            '    Dim modem As New SMSGSMModem
            '    modem.COMPort.PortName = "COM4"
            '    modem.Open()

            '    SMSGateway.Modem = modem
            'End If
            SMSGateway.CheckInit()

            Select Case Request.QueryString("_a")
                Case "SendSMS" : Me.SendSMS(writer)
                Case "GetSMSStatus" : Me.GetSMSStatus(writer)
            End Select
        End SyncLock
    End Sub

    Private Function Terminate(ByVal writer As HtmlTextWriter, ByVal code As ErrorCode, ByVal message As String) As ErrorCode
        writer.Write(Right("00" & Hex(code), 2) & "|" & message)
        Return code
    End Function

End Class
