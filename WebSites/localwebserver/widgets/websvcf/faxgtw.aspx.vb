Imports FinSeA
Imports FinSeA.FaxGateway

Partial Class widgets_websvcf_smsgtw
    Inherits System.Web.UI.Page

    Public Enum ErrorCode As Integer
        None = 0
        LoginError = 1
        BadNumber = 2
        BadDocument = 3
    End Enum

    Private Const username As String = "dmdfxgtw1"
    Private Const password As String = "232232mdsff22"
    Private Shared lock As New Object

    Private Function GetParameter(ByVal name As String) As String
        Dim ret As String = Request.Form(name)
        If (ret <> "") Then Return ret
        Return Request.QueryString(name)
    End Function

    Private Function getURL(ByVal sender As String, ByVal u As String) As String
        Select Case sender
            'Case "ar" : Return "http://areariservata.finsea.net/widgets/websvc/download.aspx?_a=get&t=attachment&id=" & u
            'Case "pdd" : Return "http://www.prestitidonato.it/widgets/websvc/download.aspx?_a=get&t=attachment&id=" & u
            Case "ar" : Return "http://areariservata.finsea.net" & u
            Case "pdd" : Return "http://www.prestitidonato.it" & u
            Case "local" : Return "http://localhost:13423/" & u
            Case Else : Return ""
        End Select
    End Function

    Private Function SendFax(writer As HtmlTextWriter) As Integer
        Dim login As String = Trim(Me.GetParameter("un"))
        Dim password As String = Trim(Me.GetParameter("pw"))
        Dim message_type As String = Trim(Me.GetParameter("t"))
        Dim recipient As String = Trim(Me.GetParameter("num"))
        Dim u As String = Trim(Me.GetParameter("u"))
        Dim sender As String = Trim(Me.GetParameter("sender"))
        Dim scheduled_delivery_time As String = Trim(Me.GetParameter("scheduled_delivery_time"))
        Dim order_id As String = Trim(Me.GetParameter("id"))

        'If (login <> username OrElse password <> password) Then Return Me.Terminate(writer, 1, "LOGIN ERROR")

        If (recipient = "") Then Return Me.Terminate(writer, ErrorCode.BadNumber, "Specifica il numero del destinatario")
        u = Me.getURL(sender, u)
        If (u = "") Then Return Me.Terminate(writer, ErrorCode.BadDocument, "Il file da inviare è vuoto")

        'Return Me.Terminate(writer, ErrorCode.BadDocument, u)

        'Scarichiamo il file
        Dim docFile As String = ""
        Try
            docFile = Server.MapPath("/App_Data/Fax/Sent/" & Formats.GetTimeStamp & ".pdf")
            My.Computer.Network.DownloadFile(u, docFile)
        Catch ex As Exception
            Return Me.Terminate(writer, ErrorCode.BadDocument, u & "<br>" & ex.Message)
        End Try
        
        Dim fax As New FaxDocument
        Try
            fax.NomeMittente = sender
            fax.MessageID = order_id
            fax.Modem = login
            fax.NumeroDestinatario = recipient
            fax.Attached = docFile
            fax.StatoInvio = FaxSendStatus.NotSend
            fax.NumeroTentativiInvio = 1
            'sms.Attributi.Add("message_type", message_type)
            'sms.Attributi.Add("scheduled_delivery_time", scheduled_delivery_time)
            fax.Send()
            Return Me.Terminate(writer, ErrorCode.None, "OK|" & fax.Messaggio)
        Catch ex As Exception
            'fax.StatoInvio = FaxSendStatus.Error
            'fax.DettaglioStatoInvio = ex.Message
            'fax.Save()
            Return Me.Terminate(writer, 255, ex.Message & FaxGateway.Config.ServerName & ":" & FaxGateway.Config.ServerPort)
        End Try
    End Function

    Private Function GetFaxStatus(writer As HtmlTextWriter) As Integer
        Dim login As String = Trim(Me.GetParameter("u"))
        Dim password As String = Trim(Me.GetParameter("p"))
        Dim order_id As String = Trim(Me.GetParameter("id"))
        'If (login <> username OrElse password <> password) Then Return Me.Terminate(writer, 1, "LOGIN ERROR")

        Dim fax As FaxDocument = FaxGateway.GetFax(order_id)
        If (fax Is Nothing) Then Return Me.Terminate(writer, 255, "Fax non trovato")

        fax.UpdateStatus()

        Return Me.Terminate(writer, 0, "OK|" & fax.StatoInvio & "|" & fax.DettaglioStatoInvio & "|" & fax.StatoRicezione & "|" & fax.DettaglioStatoRicezione & "|")
    End Function

    Protected Overrides Sub Render(writer As HtmlTextWriter)
        SyncLock lock
            If FaxGateway.Database Is Nothing Then
                Dim db As New DBConnection
                db.Path = Server.MapPath("/App_Data/db/faxlog.mdb")
                db.Open()

                FaxGateway.Database = db

               
            End If

            FaxGateway.Config.DialPrefix = "9"
            FaxGateway.Config.NotifyEMail = "tegg.fax@finsea.net"
            FaxGateway.Config.ServerName = "192.168.70.254"
            'FaxGateway.Config.ServerPort = 4559
            'FaxGateway.Config.UserName = "root"

            Select Case Request.QueryString("_a")
                Case "SendFax" : Me.SendFax(writer)
                Case "GetFaxStatus" : Me.GetFaxStatus(writer)
            End Select
        End SyncLock
    End Sub

    Private Function Terminate(ByVal writer As HtmlTextWriter, ByVal code As ErrorCode, ByVal message As String) As ErrorCode
        writer.Write(Right("00" & Hex(code), 2) & "|" & message)
        Return code
    End Function

End Class
