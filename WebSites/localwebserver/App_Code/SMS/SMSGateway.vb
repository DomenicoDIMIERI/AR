Imports System.Threading

Namespace FinSeA


    Public Class SMSGateway

        Public Enum ErrorCode As Integer
            None = 0
            LoginError = 1

        End Enum

        Public Class Account
            Public UserName As String
            Public Password As String

            Public Sub New()
            End Sub

            Public Sub New(ByVal userName As String, ByVal password As String)
                Me.UserName = userName
                Me.Password = password
            End Sub
        End Class

        Public Shared lock As New Object
        Private Shared sem As New ManualResetEvent(False)
        Private Shared m_Database As DBConnection = Nothing
        'Private Shared m_Queue As New CCollection(Of SMSMessage)
        Private Shared m_Modem As SMSGSMModem = Nothing

        Private Delegate Sub sendHandlerDelegate()

        ' Private Shared m_sendHandler As sendHandlerDelegate = AddressOf sendHandler

        Private Sub New()
            '  m_sendHandler.BeginInvoke(Nothing, Nothing)
        End Sub

        Public Shared Sub Initialize()
            Dim db As New DBConnection
            db.Path = System.Web.HttpContext.Current.Server.MapPath("/App_Data/db/smslog.mdb")
            db.Open()
            Database = db

            Dim modem As New SMSGSMModem
            modem.COMPort.PortName = "COM4"
            modem.Open()

            modem = modem
        End Sub

        Public Shared Sub CheckInit()
            If Database Is Nothing Then Initialize()
        End Sub

        Public Shared Property Database As DBConnection
            Get
                Return m_Database
            End Get
            Set(value As DBConnection)
                m_Database = value
            End Set
        End Property

        Public Shared Property Modem As SMSGSMModem
            Get
                Return m_Modem
            End Get
            Set(value As SMSGSMModem)
                m_Modem = value
            End Set
        End Property

        'Public Shared Sub SendMessage(ByVal msg As SMSMessage)
        '    SyncLock lock
        '        m_Queue.Add(msg)
        '        sem.Set()
        '    End SyncLock
        'End Sub

        Shared Function GetMessageByMsgID(ByVal msgID As String) As SMSMessage
            CheckInit()

            msgID = Trim(msgID)
            If (msgID = "") Then Return Nothing
            Dim ret As SMSMessage = Nothing
            Dim dbSQL As String = "SELECT * FROM tbl_Messaggi WHERE [MessageID]='" & Replace(msgID, "'", "''") & "'"
            'System.Web.HttpContext.Current.Response.Write(dbSQL)
            'System.Web.HttpContext.Current.Response.End()
            'Exit Function
            Dim dbRis As System.Data.IDataReader = Database.ExecuteReader(dbSQL)
            If (dbRis.Read) Then
                ret = New SMSMessage
                ret.Load(dbRis)
            End If
            dbRis.Dispose()
            Return ret
        End Function



        'Private Shared Sub sendHandler()
        '    sem.WaitOne(1000)
        '    Dim sms As SMSMessage = Nothing
        '    SyncLock lock
        '        If m_Queue.Count > 0 Then
        '            sms = m_Queue(0)
        '            m_Queue.RemoveAt(0)
        '        End If
        '    End SyncLock
        '    If (sms IsNot Nothing) Then
        '        SyncLock m_Modem
        '            Try
        '                m_Modem.SendSMS(sms.NumeroDestinatario, sms.Messaggio)
        '                sms.StatoInvio = SMSSendStatus.Sent
        '                sms.DettaglioStatoInvio = "Inviato"
        '            Catch ex As Exception
        '                sms.StatoInvio = SMSSendStatus.Error
        '                sms.DettaglioStatoInvio = ex.Message
        '            End Try
        '        End SyncLock
        '        sms.Save()
        '    End If
        '    m_sendHandler.BeginInvoke(Nothing, Nothing)
        'End Sub



    End Class


End Namespace