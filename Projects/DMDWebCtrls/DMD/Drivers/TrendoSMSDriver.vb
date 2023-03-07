Imports Microsoft.VisualBasic
Imports DMD
Imports DMD.Sistema

Namespace Drivers

    Public Class TrendoSMSDriverOptions
        Inherits SMSDriverOptions

        ''' <summary>
        ''' Restituisce o imposta la url della pagina che riceve il risultato di invio dell'SMS
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SMSFeedBackURL As String
            Get
                Return Me.GetValueString("SMSFeedBackURL", "")
            End Get
            Set(value As String)
                Me.SetValueString("SMSFeedBackURL", DMD.Sistema.Strings.Trim(value))
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome utente per l'accesso al servizio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UserName As String
            Get
                Return Me.GetValueString("UserName", "")
            End Get
            Set(value As String)
                Me.SetValueString("UserName", DMD.Sistema.Strings.Trim(value))
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la password per l'accesso al servizio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Password As String
            Get
                Return Me.GetValueString("Password", "")
            End Get
            Set(value As String)
                Me.SetValueString("Password", value)
            End Set
        End Property
    End Class

    Public Class TrendoSMSDriver
        Inherits BasicSMSDriver

        Public Enum ServiceTypeEnum As Integer
            Silver = 0
            Gold = 1
            GoldPlus = 2
        End Enum



        Private m_DefaultAreaCode As String = "+39"
        Private m_ServiceType As Nullable(Of ServiceTypeEnum) = Nothing
        Private m_UserName As String = vbNullString
        Private m_Password As String = vbNullString
        Private m_ScheduledTime As Nullable(Of Date) = Nothing


        Public Sub New()
        End Sub


        ''' <summary>
        ''' Restituisce o imposta il codice internazionale utilizzato per completare eventuali numeri per cui non è impostato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DefaultAreaCode As String
            Get
                Return m_DefaultAreaCode
            End Get
            Set(value As String)
                value = m_DefaultAreaCode
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo di servizio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ServiceType As Nullable(Of ServiceTypeEnum)
            Get
                Return Me.m_ServiceType
            End Get
            Set(value As Nullable(Of ServiceTypeEnum))
                Me.m_ServiceType = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la username usata per l'accesso al servizio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UserName As String
            Get
                Return Me.m_UserName
            End Get
            Set(value As String)
                Me.m_UserName = Trim(value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la password usata per l'accesso al servizio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Password As String
            Get
                Return Me.m_Password
            End Get
            Set(value As String)
                Me.m_Password = value
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta la data e l'ora di invio posticipato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ScheduledTiem As Nullable(Of Date)
            Get
                Return Me.m_ScheduledTime
            End Get
            Set(value As Nullable(Of Date))
                Me.m_ScheduledTime = value
            End Set
        End Property

        Public Overrides ReadOnly Property Description As String
            Get
                Return "Trendoo SMS"
            End Get
        End Property

        Public Overrides Function GetUniqueID() As String
            Return "Trendoo SMS 1.00"
        End Function



        Protected Overrides Function Send(ByVal destNumber As String, testo As String, ByVal options As SMSDriverOptions) As String
            Dim st As ServiceTypeEnum
            If (Me.m_ScheduledTime.HasValue) Then
                st = Me.m_ServiceType
            Else
                st = IIf(options.RichiediConfermaDiLettura, ServiceTypeEnum.Gold, ServiceTypeEnum.Silver)
                If (options.Mittente <> "") Then st = ServiceTypeEnum.GoldPlus
            End If

            Dim modem As SMSDriverModem = Me.GetModem(options.ModemName)
            If (modem Is Nothing) Then Throw New InvalidOperationException("Nessun modem installato")
            If modem.SendEnabled = False Then Throw New InvalidOperationException("Il modem non consente l'invio di SMS")


            Dim ret As String = SMSSEND(modem.UserName, modem.Password, New String() {modem.DialPrefix & destNumber}, testo, st, options.Mittente, Me.ScheduledTiem, "")

            Dim results() As String = Split(ret, "|")
            If (results(0) <> "OK") Then Throw New Exception("Errore di invio SMS:" & ret)

            Return results(1)   'Restituisce l'order_id
        End Function

        Private Function ParseMessageStatus(ByVal value As String) As MessageStatusEnum
            Select Case UCase(Trim(value))
                Case "SCHEDULED" : Return MessageStatusEnum.Scheduled
                Case "SENT" : Return MessageStatusEnum.Sent
                Case "DLVRD" : Return MessageStatusEnum.Delivered
                Case "ERROR" : Return MessageStatusEnum.Error
                Case "TIMEOUT" : Return MessageStatusEnum.Timeout
                Case "TOOM4NUM" : Return MessageStatusEnum.Error 'Troppi SMS per lo stesso destinatario nelle ultime 24 ore
                Case "TOOM4USER" : Return MessageStatusEnum.Error 'Troppi SMS inviati dall'utente nelle ultime 24 ore
                Case "UNKNPFX" : Return MessageStatusEnum.BadNumber  'Prefisso SMS non valido o sconosciuto
                Case "UNKNRCPT" : Return MessageStatusEnum.BadNumber  'Numero di telefono del destinatario non valido o sconosciuto
                Case "WAIT4DLVR" : Return MessageStatusEnum.Waiting    'Numero di telefono del destinatario non valido o sconosciuto
                Case "WAITING" : Return MessageStatusEnum.Waiting 'Numero di telefono del destinatario non valido o sconosciuto
                    'Case "UNKNOWN"
                Case Else : Return MessageStatusEnum.Unknown
            End Select
        End Function

        Public Overrides Function SupportaConfermaDiRecapito() As Boolean
            Return True
        End Function

        Protected Overrides Function CountRequiredSMS(text As String) As Integer
            Dim maxl As Integer = Me.GetMaxSMSLen
            Dim msgl As Integer = Me.GetSMSTextLen(text)
            If (msgl > maxl) Then
                Return Math.Ceiling(msgl / 153)
            Else
                Return 1
            End If
        End Function

        Protected Overrides Function GetSMSTextLen(text As String) As Integer
            Dim cnt As Integer = 0
            For i As Integer = 0 To Len(text)
                Dim ch As String = Mid(text, i, 1)
                Select Case ch
                    Case "^", "{", "}", "\", "[", "~", "]", "|", "€" : cnt += 2
                    Case Else : cnt += 1
                End Select
            Next
            Return cnt
        End Function

        Public Overrides Function SupportaMessaggiConcatenati() As Boolean
            Return True
        End Function




        Protected Overrides Function GetStatus(ByVal order_id As String) As MessageStatus
            order_id = Trim(order_id)
            If (order_id = "") Then Throw New ArgumentNullException("order_id")

            Dim ret As String = SMSSTATUS(Me.UserName, Me.Password, order_id)
            If (Left(ret, 2) <> "OK") Then
                Throw New Exception("Formato della risposta non valido: " & ret)
            End If
            ret = Mid(ret, 4)

            Dim rows() As String = Split(ret, ";")
            Dim result As New MessageStatus
            'For i As Integer = 0 To UBound(rows)
            Dim i As Integer = 0
            Dim nibbles() As String = Split(rows(i), "|")
            result.MessageID = order_id
            If (Arrays.Len(nibbles) > 0) Then result.TargetNumber = nibbles(0)
            If (Arrays.Len(nibbles) > 1) Then
                result.MessageStatus = Me.ParseMessageStatus(nibbles(1))
                result.MessageStatusEx = nibbles(1)
            Else
                result.MessageStatus = MessageStatusEnum.Unknown
                result.MessageStatusEx = ""
            End If
            If (Arrays.Len(nibbles) > 2 AndAlso result.MessageStatus = MessageStatusEnum.Delivered) Then
                result.DeliveryTime = ParseDriverDate(nibbles(2))
            End If
            'Next

            Select Case result.MessageStatus
                Case MessageStatusEnum.BadNumber : result.MessageStatusEx = "Messaggio non inviato: Numero Errato"
                Case MessageStatusEnum.Delivered : result.MessageStatusEx = "Messaggio Ricevuto"
                Case MessageStatusEnum.Error : result.MessageStatusEx = "Errore generico"
                Case MessageStatusEnum.Scheduled : result.MessageStatusEx = "Messaggio predisposto per l'invio ritardato"
                Case MessageStatusEnum.Sent : result.MessageStatusEx = "Messaggio Inviato"
                Case MessageStatusEnum.Timeout : result.MessageStatusEx = "Errore di Timeout"
                Case MessageStatusEnum.Unknown : result.MessageStatusEx = "Stato di invio Sconosciuto"
                Case MessageStatusEnum.Waiting : result.MessageStatusEx = "Messaggio in attesa di essere inviato"
            End Select

            Return result
        End Function

        Public Sub RemoveDelayed(ByVal order_id As String)
            order_id = Trim(order_id)
            If (order_id = "") Then Throw New ArgumentNullException("order_id")

            Dim ret As String = REMOVE_DELAYED(Me.UserName, Me.Password, order_id)
            If (Left(ret, 2) <> "OK") Then
                Throw New Exception("Formato della risposta non valido: " & ret)
            End If
        End Sub

        ''' <summary>
        ''' Restituisce la lista degli SMS inviati nell'intervallo data specificato
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSMSHistory(ByVal from As Date, ByVal [to] As Date) As CCollection
            Dim col As New CCollection
            Dim ret As String = SMSHISTORY(Me.UserName, Me.Password, from, [to])
            If (Left(ret, 2) <> "OK") Then
                Throw New Exception("Formato della risposta non valido: " & ret)
            End If
            Return col
        End Function

        ''' <summary>
        ''' Restituisce la disponibilità di credito
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetCredit() As Decimal
            Dim ret As String = GetCredit(Me.UserName, Me.Password)
            If (Left(ret, 2) <> "OK") Then
                Throw New Exception("Formato della risposta non valido: " & ret)
            Else
                Return Formats.ParseValuta(Mid(ret, 3))
            End If
        End Function

        Protected Overrides Sub VerifySender(sender As String)
            CheckSender(sender)
        End Sub

#Region "Driver Specific"

        Private Sub CheckSender(ByVal value As String)
            Dim numbers As String = "0123456789+"
            Dim isAlfa As Boolean = False
            For i As Integer = 1 To Len(numbers)
                If (InStr(value, Mid(numbers, i, 1)) > 0) Then
                    isAlfa = True
                    Exit For
                End If
            Next
            If (isAlfa) Then
                If (Len(value) > 11) Then Throw New ArgumentException("Il nome del mittente non può superare 11 caratteri")
            Else
                If (Len(value) > 16) Then Throw New ArgumentException("Il numero del mittente non può superare 16 cifre")
            End If
        End Sub


        Private Function FormatDriverDate(ByVal value As Nullable(Of Date)) As String
            If (Not value.HasValue) Then Return vbNullString
            Return Formats.Format(value, "yyyyMMddHHmmss")
        End Function

        Private Function ParseDriverDate(ByVal value As String) As Nullable(Of Date)
            value = Trim(value)
            If (value = "") Then Return Nothing
            Dim yyyy As Integer = Formats.ToInteger(Mid(value, 1, 4))
            Dim MM As Integer = Formats.ToInteger(Mid(value, 5, 2))
            Dim dd As Integer = Formats.ToInteger(Mid(value, 7, 2))
            Dim HH As Integer = Formats.ToInteger(Mid(value, 9, 2))
            Dim m As Integer = Formats.ToInteger(Mid(value, 11, 2))
            Dim ss As Integer = Formats.ToInteger(Mid(value, 13, 2))
            Return DateUtils.MakeDate(yyyy, MM, dd, HH, m, ss)
        End Function

        Public Function FormatTargetNumber(ByVal value As String) As String
            Dim ret As String = Formats.ParsePhoneNumber(value)
            If ((Left(ret, 1) <> "+") OrElse (Left(ret, 2) <> "00")) Then ret = m_DefaultAreaCode & ret
            Return ret
        End Function

        Private Function URLEncode(ByVal value As String) As String
            Return Sistema.Strings.URLEncode(value)
        End Function

        Private Function PrepareRecipients(ByVal numbers() As String) As String
            Dim ret As New System.Text.StringBuilder
            If (numbers IsNot Nothing AndAlso numbers.Length > 0) Then
                For i As Integer = 0 To UBound(numbers)
                    Dim n As String = numbers(i)
                    Dim p As String = Formats.ParsePhoneNumber(n)
                    If (p <> "") Then
                        If (ret.Length > 0) Then ret.Append(",")
                        ret.Append(p)
                    End If
                Next
            End If
            Return ret.ToString
        End Function


        Private Function FormatMessageType(ByVal value As ServiceTypeEnum) As String
            Select Case value
                Case ServiceTypeEnum.Silver : Return "SI"
                Case ServiceTypeEnum.Gold : Return "GS"
                Case ServiceTypeEnum.GoldPlus : Return "GP"
                Case Else : Throw New ArgumentOutOfRangeException("message type")
            End Select
        End Function

        Private Function HttpRequest(ByVal address As String) As String
#If 0 Then
            Dim req As New MSXML2.ServerXMLHTTP
            Dim ret As String = ""
            'req.setTimeouts(lResolve, lConnect, lSend, lReceive)
            req.setOption(MSXML2.SERVERXMLHTTP_OPTION.SXH_OPTION_IGNORE_SERVER_SSL_CERT_ERROR_FLAGS, 13056)
            'req.setOption(MSXML2.SERVERXMLHTTP_OPTION.SXH_OPTION_SELECT_CLIENT_SSL_CERT, False)
            req.open("GET", address, False)
            req.send("")
            If (req.readyState = 4) Then
                ret = req.responseText
            Else
                Throw New Exception("Errore di comunicazione con il sito: " & address & " (" & req.readyState & ")")
            End If
            req = Nothing
            Return ret
#Else
            Dim req As New System.Net.WebClient
            Dim ret As String = req.DownloadString(address)
            req.Dispose()
            Return ret
#End If
        End Function

        Public Function SMSSEND(ByVal username As String, ByVal password As String, ByVal destNumbers As String(), testo As String, ByVal messageType As ServiceTypeEnum, ByVal sender As String, ByVal scheduledTime As Nullable(Of Date), ByVal orderID As String) As String
            username = Trim(username) : If (username = "") Then Throw New ArgumentNullException("username")
            password = Trim(password) : If (password = "") Then Throw New ArgumentNullException("password")
            Dim recips As String = PrepareRecipients(destNumbers) : If (recips = "") Then Throw New ArgumentNullException("destNumbers")
            testo = Trim(testo) : If (testo = "") Then Throw New ArgumentNullException("testo")
            sender = Trim(sender) : If (sender <> "") Then CheckSender(sender)
            orderID = Trim(orderID)

            Dim buffer As New System.Text.StringBuilder
            buffer.Append("https://api.trendoo.it/Trend/SENDSMS?")
            buffer.Append("login=" & URLEncode(username))
            buffer.Append("&password=" & URLEncode(password))
            buffer.Append("&message_type=" & FormatMessageType(messageType))
            buffer.Append("&recipient=" & recips)
            buffer.Append("&message=" & URLEncode(Sistema.Strings.RemoveHTMLTags(testo)))
            If (sender <> vbNullString) Then buffer.Append("&sender=" & URLEncode(sender))
            If (scheduledTime.HasValue) Then buffer.Append("&scheduled_delivery_time=" & FormatDriverDate(scheduledTime.Value))
            If (orderID <> vbNullString) Then buffer.Append("&order_id=" & URLEncode(orderID))

            Return HttpRequest(buffer.ToString)
        End Function

        Public Overloads Function SMSSTATUS(ByVal userName As String, ByVal password As String, ByVal order_id As String) As String
            userName = Trim(userName) : If (userName = "") Then Throw New ArgumentNullException("username")
            password = Trim(password) : If (password = "") Then Throw New ArgumentNullException("password")
            order_id = Trim(order_id) : If (order_id = "") Then Throw New ArgumentNullException("order_id")

            Dim buffer As New System.Text.StringBuilder
            buffer.Append("https://api.trendoo.it/Trend/SMSSTATUS?")
            buffer.Append("login=" & URLEncode(userName))
            buffer.Append("&password=" & URLEncode(password))
            buffer.Append("&order_id=" & URLEncode(order_id))

            Return HttpRequest(buffer.ToString)
        End Function

        Public Function REMOVE_DELAYED(ByVal userName As String, ByVal password As String, ByVal order_id As String) As String
            userName = Trim(userName) : If (userName = "") Then Throw New ArgumentNullException("username")
            password = Trim(password) : If (password = "") Then Throw New ArgumentNullException("password")
            order_id = Trim(order_id) : If (order_id = "") Then Throw New ArgumentNullException("order_id")

            Dim buffer As New System.Text.StringBuilder
            buffer.Append("https://api.trendoo.it/Trend/REMOVE_DELAYED?")
            buffer.Append("login=" & URLEncode(userName))
            buffer.Append("&password=" & URLEncode(password))
            buffer.Append("&order_id=" & URLEncode(order_id))

            Return HttpRequest(buffer.ToString)
        End Function

        ''' <summary>
        ''' Restituisce la lista degli SMS inviati nell'intervallo data specificato
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SMSHISTORY(ByVal userName As String, ByVal password As String, ByVal from As Date, ByVal [to] As Date) As String
            userName = Trim(userName) : If (userName = "") Then Throw New ArgumentNullException("username")
            password = Trim(password) : If (password = "") Then Throw New ArgumentNullException("password")

            Dim buffer As New System.Text.StringBuilder
            buffer.Append("https://api.trendoo.it/Trend/SMSHISTORY?")
            buffer.Append("login=" & URLEncode(userName))
            buffer.Append("&password=" & URLEncode(password))
            buffer.Append("&from=" & FormatDriverDate(from))
            buffer.Append("&to=" & FormatDriverDate([to]))
            Return HttpRequest(buffer.ToString)
        End Function

        ''' <summary>
        ''' Restituisce la disponibilità di credito
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetCredit(ByVal userName As String, ByVal password As String) As String
            userName = Trim(userName) : If (userName = "") Then Throw New ArgumentNullException("username")
            password = Trim(password) : If (password = "") Then Throw New ArgumentNullException("password")

            Dim buffer As New System.Text.StringBuilder
            Dim col As New CCollection
            buffer.Append("https://api.trendoo.it/Trend/CREDITS?")
            buffer.Append("login=" & URLEncode(userName))
            buffer.Append("&password=" & URLEncode(password))
            Return HttpRequest(buffer.ToString)
        End Function

#End Region
        Protected Overrides Function InstantiateNewOptions() As DriverOptions
            Return New TrendoSMSDriverOptions
        End Function

        Protected Overrides Sub InternalConnect()
            'Il driver non richiede la connessione ma username e password
            With DirectCast(Me.GetDefaultOptions, TrendoSMSDriverOptions)
                Me.UserName = .UserName
                Me.Password = .Password
            End With
        End Sub

        Protected Overrides Sub InternalDisconnect()
            'Il driver non richiede la connessione
        End Sub
    End Class

End Namespace