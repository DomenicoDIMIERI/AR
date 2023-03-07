Imports System.Threading
Imports DMD.Databases
Imports System.IO.Ports
Imports DMD.Nokia
Imports DMD
Imports DMD.Sistema

'Namespace DMD

Public Class SMSGateway
    'Public Const NokiaDeiveSN As String = "354208031228498"
    'Public Const COMPortName As String = "COM8"


    Private Shared lock As New Object
    Private Shared sem As New ManualResetEvent(False)
    Private Shared m_Database As CDBConnection = Nothing
    
    Private Delegate Sub sendHandlerDelegate()

    ' Private Shared m_sendHandler As sendHandlerDelegate = AddressOf sendHandler

    Private Sub New()
        '  m_sendHandler.BeginInvoke(Nothing, Nothing)
    End Sub

    Shared Sub New()
       
       

    End Sub



   

    Public Shared Property Database As CDBConnection
        Get
            Return m_Database
        End Get
        Set(value As CDBConnection)
            m_Database = value
        End Set
    End Property

    Public Shared Function Send(ByVal accountName As String, ByVal userName As String, ByVal password As String, ByVal targetNumber As String, ByVal message As String, Optional ByVal messageID As String = "", Optional ByVal falgs As Integer = 0) As SMSMessage
        'SyncLock lock
        Dim svc As OutService = OutServices.GetItemByName(accountName)
        If (svc Is Nothing) Then Throw New Exception("Nome del servizio non valido")
        If svc.UserName <> userName OrElse svc.Password <> password Then Throw New PermissionDeniedException("Credenziali non valide")

        Dim sms As New SMSMessage
        sms.NomeMittente = accountName
        sms.MessageID = messageID
        sms.NumeroDestinatario = targetNumber
        sms.Messaggio = message
        sms.StatoInvio = SMSSendStatus.Sending
        sms.NumeroTentativiInvio = 1
        sms.DataInvioServer = Now
        sms.Save()
        Try
            svc.Send(sms)
            sms.StatoInvio = SMSSendStatus.Sent
            sms.DettaglioStatoInvio = "Inviato"
            sms.Save()
        Catch ex As Exception
            sms.StatoInvio = SMSSendStatus.Error
            sms.DettaglioStatoInvio = ex.Message
        End Try

        Return sms
        ' End SyncLock
    End Function

    Public Shared Function GetMessageByMsgID(ByVal msgID As String) As SMSMessage
        msgID = Trim(msgID)
        If (msgID = "") Then Return Nothing
        Dim cursor As New SMSMessageCursor
        cursor.MessageID.Value = msgID
        cursor.IgnoreRights = True
        'cursor.Modem.Value = Me.Nome
        Dim ret As SMSMessage = cursor.Item
        cursor.Dispose()
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


'End Namespace