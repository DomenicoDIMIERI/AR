Imports FinSeA
Imports FinSeA.Sistema
Imports FinSeA.Drivers
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Net.Sockets

Namespace Internals

    Public Class PortechSMSDriver
        Inherits BasicSMSDriver


        Public Sub New()
        End Sub


        Protected Overrides Function GetSMSTextLen(text As String) As Integer
            Return 160
        End Function

        Protected Overrides Function GetStatus(messageID As String) As MessageStatus
            Throw New NotImplementedException
        End Function

        Protected Overrides Function Send(destNumber As String, testo As String, options As SMSDriverOptions) As String
            Dim modem As SMSDriverModem = Me.GetModem(options.ModemName)
            If (modem Is Nothing) Then Throw New ArgumentException("Modem non trovato")

            Dim sock As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP)
            sock.Connect(modem.ServerName, modem.ServerPort)

            Dim mob As New P_MOBILE_PACK
            mob.hSocket = sock

            Me.sMobilePack(0) = mob
            unicode_message(mob, 0, 0, modem.UserName, modem.Password)

            sock.Disconnect(False)

            Return ""
        End Function

        Public Overrides Function SupportaConfermaDiRecapito() As Boolean
            Return False
        End Function

        Public Overrides Function SupportaMessaggiConcatenati() As Boolean
            Return False
        End Function

        Protected Overrides Sub VerifySender(sender As String)

        End Sub

#Region "LIB"

        Private Const MAX_PASSWORD As Integer = (64 - 1)
        Private Const MAX_PHONE_NUMBER As Integer = (64 - 1)
        Private Const MAX_SMS_TEXT As Integer = (512 - 1)
        Private Const MAX_SMS As Integer = 30000
        Private Const MAX_MOBILE As Integer = 256
        Private Const MAX_AT_STRING As Integer = 1024
        Private ReadOnly CTRL_Z As Byte() = {26} ';//Â²°T¿é¤Jµ²§ô
        Private ReadOnly CTRL_X As Byte() = {24} ';//Â÷¶}Â²°T¥\¯à

        Private sSmsPack As P_SMS_PACK() = Arrays.CreateInstance(Of P_SMS_PACK)(MAX_SMS) ' S_SMS_PACK () [MAX_SMS];
        Private sMobilePack As P_MOBILE_PACK() = Arrays.CreateInstance(Of P_MOBILE_PACK)(MAX_MOBILE) 'S_MOBILE_PACK  
        Private g_total_sms As Integer = 0
        Private g_total_mobile As Integer = 0

        Private MsgEnc As System.Text.Encoding = System.Text.Encoding.Default

        Public Class P_SMS_PACK '_SMS_PACK
            Public ok_message As Integer
            Public nTry As Integer
            Public nErr As Integer
            Public nTryCount As Integer
            Public bSending As Integer

            Public nType As Integer 'ucs2, 7bit or 8 bit ..

            Public phone_str As Byte() 'char [MAX_PHONE_NUMBER+1];

            Public wsms As Byte() 'WCHAR [MAX_SMS_TEXT+1];

            Public Sub New()
                Me.phone_str = Arrays.CreateInstance(Of Byte)(MAX_PHONE_NUMBER + 1)
                Me.wsms = Arrays.CreateInstance(Of Byte)(MAX_SMS_TEXT + 1)
            End Sub
        End Class 'S_SMS_PACK, *P_SMS_PACK;


        Public Class P_MOBILE_PACK
            Public hSocket As Socket

            Public bMv372 As Integer

            Public nPort As Integer

            Public tx_flag As Integer
            Public rx_flag As Integer

            Public connect_ok As Integer
            Public module_flag As Integer
            'int send_flag;
            Public iSms As Integer
            Public bCompOK As Integer


            Public szWaitStr As Byte() ' '[MAX_SMS_TEXT+1];

            Public Sub New()
                Me.szWaitStr = Arrays.CreateInstance(Of Byte)(MAX_SMS_TEXT + 1)
            End Sub
        End Class 'S_MOBILE_PACK, *P_MOBILE_PACK;


        Private Sub get_time(ByVal iSMS As Integer, ByVal iModile As Integer)
            '    char showbuf[256];
            'Dim stime As SYSTEMTIME

            'P_MOBILE_PACK mob = psMobilePack(txopt);
            'Dim sms As P_SMS_PACK = psSmsPack(iSMS)

            'GetLocalTime(&stime)

            'M_LOG( "SMS[%d] %s %04d/%02d/%02d %02d:%02d:%02d.\r\n",
            '          iSMS+1, sms->phone_str, //iModile+1,
            '          stime.wYear, stime.wMonth, stime.wDay,
            '          stime.wHour , stime.wMinute , stime.wSecond );
        End Sub

        Private Function psSmsPack(ByVal i As Integer) As P_SMS_PACK
            Return sSmsPack(i)
        End Function

        Private Function psMobilePack(ByVal i As Integer) As P_MOBILE_PACK
            Return sMobilePack(i)
        End Function


        Private Function is_all_7bitChar(ByVal pi As Byte(), ByVal max As Integer) As Boolean
            Dim wc As Byte 'WCHAR 
            Dim p As Integer = 0
            While (max > 0)
                max -= 1
                wc = pi(p)
                If (wc = 0) Then
                    Exit While 'break;
                End If

                If (wc > 127) Then Return False
                p += 1
            End While

            Return 1
        End Function

        Private Sub M_LOG(ByVal text As String, ByVal ParamArray params() As Object)
            Debug.Print(text)
        End Sub

        Private Function strcpy_s(ByVal buffer As Byte(), ByVal bufferSize As Integer, ByVal src As String) As Byte()
            Dim tmp() As Byte = MsgEnc.GetBytes(src)
            For i = 0 To UBound(tmp)
                buffer(i) = tmp(i)
            Next
            Return buffer
        End Function

        Private Function strcpy_s(ByVal buffer As Byte(), ByVal bufferSize As Integer, ByVal src As Byte()) As Byte()
            For i = 0 To UBound(src)
                buffer(i) = src(i)
            Next
            Return buffer
        End Function

        Private Function strcpy_s(ByVal buffer As Byte(), ByVal startIndex As Integer, ByVal bufferSize As Integer, ByVal src As String) As Byte()
            Dim tmp() As Byte = MsgEnc.GetBytes(src)
            For i = 0 To UBound(tmp)
                buffer(i + startIndex) = tmp(i)
            Next
            Return buffer
        End Function

        Private Function strcpy_s(ByVal buffer As Byte(), ByVal startIndex As Integer, ByVal bufferSize As Integer, ByVal src As Byte()) As Byte()
            For i = 0 To UBound(src)
                buffer(i + startIndex) = src(i)
            Next
            Return buffer
        End Function

        Private Function strlen(ByVal str As Byte()) As Integer
            For i As Integer = 0 To UBound(str)
                If (str(i) = 0) Then Return i
            Next
            Return 0
        End Function

        'Private Function sprintf_s(ByVal buffer As Byte(), ByVal startIndex As Integer, ByVal bufferSize As Integer, ByVal str As String, ByVal ParamArray params() As Object) As Byte()
        '    Dim src As String = ""
        '    Dim pos As Integer = 0
        '    Do
        '        Dim i As Integer = InStr(pos, str, "%")
        '        If (i < 0) Then Exit Do
        '        Dim flag As String = Mid(str, i, 1) : i += 1
        '        Dim larghezza As String = ""
        '        Dim tipo As String = ""
        '        Select Case flag
        '            Case "+" ' : Printf denota sempre il segno '+' o '-' di un numero (il default è omettere il segno per i numeri positivi). Solo per variabili numeriche.
        '            Case "-" ' Printf allinea a sinistra l'output di questo segnaposto (il default è a destra).
        '            Case "#" ': Forma alternata: con 'g' e 'G', gli zeri finali non sono rimossi; con 'f', 'F', 'e', 'E', 'g', 'G', l'output contiene sempre un punto decimale; con 'o', 'x', e 'X', vengono rispettivamente prefissi uno 0, 0x, o 0Xai numeri diversi da zero.
        '            Case " " ' Fa inserire a printf spazi sulla sinistra dell'output fino al raggiungimento della lunghezza richiesta. Se combinato con '0' (vedi sotto), farà diventare il segno uno spazio per i numeri positivi, ma gli altri caratteri saranno riempiti da zeri.
        '            Case "0" ' Fa usare a printf dei caratteri '0' (anziché spazi) a sinistra fino al raggiungimento di una certa lunghezza. Per esempio, assumendo i = 3, printf("%2d", i) risulta in " 3", mentre printf("%02d", i) resulta in "03"
        '            Case Else
        '                larghezza = flag
        '        End Select
        '        While Char.IsNumber(Mid(str, i, 1))
        '            larghezza &= Mid(str, i, 1)
        '            i += 1
        '        End While
        '        If Mid(str, i, 1) = "." Then

        '        End If
        '        Select Case tipo
        '            Case "d" 'numero decimale con segno
        '            Case "u" 'numero decimale senza segno
        '            Case "f", "F" 'valore reale come numero con virgola
        '            Case "e", "E" 'valore reale  reale nella forma esponenziale standard ([-]d.ddd e[+/-]ddd); 'e' usa la 'e' minuscola, 'E' usa la maiuscola
        '            Case "g", "G" 'Stampa un valore reale con notazione reale o esponenziale, scegliendo quella più adatta alla sua dimensione. 'g' usa lettere minuscole, 'G' usa lettere maiuscole. Questa notazione è diversa dalla notazione con virgola perché gli zeri superflui alla destra del punto decimale non sono inclusi. Il punto decimale non è incluso nei numeri interi;
        '            Case "x", "X" 'Stampa un intero senza segno come numero esadecimale. 'x' usa lettere minuscole e 'X' usa maiuscole;
        '            Case "o" 'Stampa un intero senza segno in base ottale;
        '            Case "s" 'Stampa una stringa;
        '            Case "c" 'Stampa un carattere;
        '            Case "p" ' Stampa un puntatore a void, in un formato definito dall'implementazione;
        '            Case "n" ' Scrivi il numero di caratteri finora scritti correttamente in un parametro puntatore a intero;
        '            Case "%" 'stampa un carattere '%' (non accetta flag, precisione, lunghezza o larghezza).


        '        End Select

        '    Loop While (pos < Len(str))
        '    'Select Case Format()
        '    '    Case "%02X"
        '    '        src = Strings.Hex(value, 2)
        '    '    Case Else
        '    '        Throw New FormatException
        '    'End Select

        '    Return strcpy_s(buffer, startIndex, src)
        'End Function

        Private Function sprintf_s(ByVal buffer As Byte(), ByVal startIndex As Integer, ByVal bufferSize As Integer, ByVal message As Byte()) As Byte()
            Return strcpy_s(buffer, bufferSize, startIndex, message)
        End Function

        Private Function sprintf_s(ByVal buffer As Byte(), ByVal startIndex As Integer, ByVal bufferSize As Integer, ByVal message As String) As Byte()
            Return strcpy_s(buffer, bufferSize, startIndex, message)
        End Function

        Private Function sprintf_s(ByVal buffer As Byte(), ByVal bufferSize As Integer, ByVal message As String) As Byte()
            Return strcpy_s(buffer, bufferSize, 0, message)
        End Function

        Private Function sprintf_s(ByVal buffer As Byte(), ByVal message As String) As Byte()
            Return strcpy_s(buffer, Arrays.Len(buffer), 0, message)
        End Function

        ' =======================================================================
        Private Function char_uc2_to_ascii7(ByVal wc As Byte) As Byte
            Dim c As Byte = wc And &H7F
            If (c >= Asc(" ")) Then Return c
            Select Case Chr(c)
                Case vbNullChar, vbCr, vbLf
                    Return c
            End Select
            Return Asc(" ")
        End Function

        ' =======================================================================
        Private Function make_pdu(ByVal pd As Byte(), ByVal iMax As Integer, ByVal da As Byte(), ByVal msg As Byte()) As Integer 'size_t //, int nType)
            Dim l, i As Integer

            ' 00 ªA°È¤¤¤ß¸¹½X (SMSC ¦a§})
            ' 11
            ' 00 TP-MR(TP-Message-Reference)
            strcpy_s(pd, iMax, "001100")

            l = strlen(da)
            sprintf_s(pd, 6, iMax, Strings.Hex(l, 2)) '"%02X", l) ' 0D = strlen(TP-DA);

            If (da(0) = vbNullChar) Then
                strcpy_s(pd, 8, iMax, "81") ' 81 = unknown code
            Else
                strcpy_s(pd, 8, iMax, "91") ' 91 =  international  code
            End If
            ' 683176116125F0 TP-DA ¦¬«H¤H¤â¾÷¸¹½X
            da(l) = Asc("F"c) ' //
            For i = 0 To l - 1 Step 2 '; i+=2)
                pd(10 + i) = da(i + 1)
                pd(11 + i) = da(i)
            Next
            da(l) = 0 ' restore

            ' 00 TP-PID (TP-Protocol-Identifier)
            ' 08 TP-DCS (TP-Data-Coding-Scheme)
            ' 01 TP-VP (TP-Validy-Period)
            strcpy_s(pd, 10 + i, iMax, "000801")

            l = strlen(msg)
            sprintf_s(pd, 16 + i, iMax, Strings.Hex(l / 2, 2)) ' "%02X", l / 2) '06 TP-UDL µu«Hªø«×
            sprintf_s(pd, 18 + i, iMax, msg)

            Return (l + i + 18) / 2 - 1
        End Function



        Private Function str_uc2_to_ascii7(ByVal po As Byte(), ByVal pi As Byte()) As Integer
            Dim i As Integer = 0
            Dim c As Byte

            While (i < 512)
                c = char_uc2_to_ascii7(pi(i))
                po(i) = Asc(c)
                i += 1
                If (c = vbNullChar) Then Exit While
            End While

            '/*
            '        If (i > 512) Then
            '    {
            '        LIST_PRINT("str_uc2_to_ascii7() overflow!\r\n");
            '    }
            '    */

            po(i) = 0
            Return i
        End Function

        Private Function str_uc2_to_hex16(ByVal po() As Byte, ByVal pi() As Byte) As Integer
            Dim i As Integer = 0

            While (pi(i * 2) OrElse pi(i * 2 + 1))
                char_uc2_to_hex16(po, i * 4, pi, i * 2)
                i += 1
                If (i > 512) Then
                    ' LIST_PRINT("str_uc2_to_hex() overflow!\r\n")
                    Return 0
                End If
            End While
            Return i
        End Function

        '=======================================================================
        Private Function char_uc2_to_hex16(ByVal po() As Byte, ByVal poI As Integer, ByVal pi As Byte(), ByVal piI As Integer) As UInteger
            Dim b2htab As String = "0123456789ABCDEF"
            Dim l As Integer = 0

            po(poI + 2) = Asc(b2htab.Chars((pi(piI) >> 4) And &HF))
            po(poI + 3) = Asc(b2htab.Chars(pi(piI + 0) And &HF))

            po(poI + 0) = Asc(b2htab.Chars((pi(piI + 1) >> 4) And &HF))
            po(poI + 1) = Asc(b2htab.Chars(pi(piI + 1) And &HF))

            po(poI + 4) = 0
            Return l
        End Function

        Private Function _send(ByVal h As Socket, ByVal buffer As Byte(), ByVal len As Integer, ByVal flags As Integer) As Integer
            Return h.Send(buffer, len, SocketFlags.None)
        End Function

        Private Function szSend(ByVal h As Socket, ByVal s As Byte())
            Return _send(h, s, strlen(s), 0)
        End Function

        Private Function szSend(ByVal h As Socket, ByVal s As String)
            Return szSend(h, MsgEnc.GetBytes(s))
        End Function

        Private Function waitStr(ByVal pi As String, ByVal it As Integer, ByVal nMobile As Integer) As Boolean
            Dim mob As P_MOBILE_PACK = psMobilePack(nMobile)
            Dim t As Integer = it ' * 10;
            Dim zkey As Byte() = mob.szWaitStr

            strcpy_s(mob.szWaitStr, MAX_AT_STRING, pi)

            While (t)
                t -= 1
                If (mob.bCompOK < 0) Then Exit While 'break;

                If (mob.bCompOK) Then
                    zkey(0) = 0
                    mob.bCompOK = 0
                    'Sleep(100);
                    Return 1
                End If

                Thread.Sleep(100)
            End While

            zkey(0) = 0
            mob.bCompOK = 0
            Return 0
        End Function

        Private Function unicode_message(ByRef mob As P_MOBILE_PACK, ByVal txopt As Integer, ByVal mgid As Integer, ByVal username As String, ByVal password As String) As Integer
            Dim sms As P_SMS_PACK = psSmsPack(mgid)

            Dim nTry As Integer = 3
            Dim sck As Socket = mob.hSocket
            Dim hr As Integer
            Dim i As Integer = 0
            Dim l As Integer 'size_t 

            Dim uTmp() As Byte = Arrays.CreateInstance(Of Byte)(1024)
            Dim ssTmp() As Byte = Arrays.CreateInstance(Of Byte)(1024)
            Dim sz() As Byte = Arrays.CreateInstance(Of Byte)(1024)

            nTry = 3
            Do
                hr = waitStr("username:", 20, txopt)
                nTry -= 1
            Loop While (hr < 1 AndAlso nTry)

            If (hr < 1) Then GoTo _err

            sck = mob.hSocket

            szSend(sck, username)

            hr = waitStr("password:", 20, txopt)
            If (hr < 1) Then GoTo _err

            szSend(sck, password)

            hr = waitStr("command:", 20, txopt)
            If (hr < 1) Then GoTo _err

            sprintf_s(sz, "state" & (mob.nPort + 1) & vbCr) 'sprintf_s(sz, "state%d" & vbCr, mob.nPort + 1)
            szSend(sck, sz)

            hr = waitStr("free", 10, txopt)
            If (hr < 1) Then
                mob.nPort = mob.nPort Xor 1
                sprintf_s(sz, "state%d" & vbCr, mob.nPort + 1)
                szSend(sck, sz)

                hr = waitStr("free", 10, txopt)
                If (hr < 1) Then GoTo _err
            End If

            sprintf_s(sz, "module%d" & vbCr, mob.nPort + 1)
            szSend(sck, sz)

            hr = waitStr("getting", 10, txopt)
            If (hr < 1) Then GoTo _err

            If (sms.nTry) Then
                M_LOG("Send SMS[%d] %s @ Mobile[%d/%d], retry=%d. " & vbCrLf, mgid + 1, sms.phone_str, mob.nPort, txopt + 1, sms.nTry)
            Else
                M_LOG("Send SMS[%d] %s @ Mobile[%d/%d], last try." & vbCrLf, mgid + 1, sms.phone_str, mob.nPort, txopt + 1)
            End If

            szSend(sck, "ATE1" & vbCr)
            hr = waitStr("0" & vbCr, 20, txopt)
            If (hr < 1) Then GoTo _err
            Thread.Sleep(10)

            szSend(sck, "AT" & vbCr)
            hr = waitStr("0" & vbCr, 20, txopt)
            If (hr < 1) Then GoTo _err

            '  list_print("wsms[%d]\r\n",wcslen(wsms[mgid]));
            sms.wsms(160) = vbNullChar
            If (is_all_7bitChar(sms.wsms, 160)) Then
                sms.nType = 7

                szSend(sck, "AT+CMGF=1" & vbCr)
                hr = waitStr("0\r", 20, txopt)
                If (hr < 1) Then GoTo _err

                szSend(sck, "AT+CMGS=" & Chr(34))
                szSend(sck, sms.phone_str)
                szSend(sck, Chr(34) & vbCr)

                'hr = waitStr(">", 20, txopt);
                'if (hr < 1) goto _err;

                str_uc2_to_ascii7(ssTmp, sms.wsms)
            Else
                sms.wsms(70) = vbNullChar
                str_uc2_to_hex16(uTmp, sms.wsms)
                sms.nType = 16

                szSend(sck, "AT+CMGF=0" & vbCr)
                hr = waitStr("0" & vbCr, 20, txopt)
                If (hr < 1) Then GoTo _err

                l = make_pdu(ssTmp, 510, sms.phone_str, uTmp) ', sms->nType);
                sprintf_s(sz, 510, "AT+CMGS=%d" & vbCr, l)
                szSend(sck, sz)
            End If

            hr = waitStr(">", 20, txopt)
            If (hr < 1) Then GoTo _err

            szSend(sck, ssTmp)
            Thread.Sleep(10)
            _send(sck, CTRL_Z, 1, 0)

            hr = waitStr("+CMGS:", 150, txopt)
            If (hr > 0) Then sms.ok_message = 1

_err:
            _send(sck, CTRL_X, 1, 0)
            hr = waitStr("release", 10, txopt)

            Thread.Sleep(5)

            If (hr) Then
                szSend(sck, "logout" & vbCr)
            Else
                szSend(sck, vbCr & "logout" & vbCr)
            End If
            waitStr("exit", 10, txopt)

            Thread.Sleep(5)

            If (Not sms.ok_message) Then
                If (sms.nTry) Then
                    M_LOG("Error: SMS[%d] %s send fail! retry=%d." & vbCrLf, mgid + 1, sms.phone_str, sms.nTry)
                Else
                    M_LOG("Error: SMS[%d] %s send fail! give up!" & vbCrLf, mgid + 1, sms.phone_str) ' sms->nTry);
                End If
                Return 0
            End If
            get_time(mgid, txopt)
            Return 1
        End Function


        Private Function get_unsend_message(ByVal iStart As Integer, ByVal nTotal As Integer) As Integer
            Dim sms As P_SMS_PACK
            Dim i As Integer = iStart
            Dim n As Integer = nTotal

            While (n)
                n -= 1
                If (i >= nTotal) Then i = 0
                sms = psSmsPack(i)
                If (Not sms.ok_message AndAlso sms.nTry) Then
                    Return i
                End If

                i += 1
            End While

            Return -1
        End Function

        Private Function get_free_mobile(ByVal iStart As Integer, ByVal nTotal As Integer) As Integer
            Dim mob As P_MOBILE_PACK
            Dim i As Integer = iStart

            While (nTotal)
                nTotal -= 1
                If (i >= nTotal) Then i = 0

                mob = psMobilePack(i)

                If (Not mob.tx_flag AndAlso Not mob.module_flag) Then
                    If (mob.bMv372) Then
                        mob.nPort = mob.nPort Xor 1
                    Else
                        mob.nPort = 0
                    End If

                    Return i
                End If

                i += 1
            End While

            Return -1
        End Function


        Private Function is_all_Mobile_free() As Boolean
            Dim mob As P_MOBILE_PACK
            Dim i As Integer

            For i = 0 To g_total_mobile - 1
                mob = psMobilePack(i)
                If (mob.tx_flag) Then Return False
            Next
            Return True
        End Function


        Private Sub smstask()
            Dim mob As P_MOBILE_PACK = Nothing
            Dim sms As P_SMS_PACK

            Dim iSms As Integer = -1
            Dim iMobile As Integer = -1

            '   int i, n;

            While (1)
                Thread.Sleep(10)

                iSms = get_unsend_message(iSms + 1, g_total_sms)
                If (iSms < 0) Then
                    Exit While
                End If

                sms = psSmsPack(iSms)

                If (sms.bSending) Then
                    Continue While

                    Do
                        iMobile = get_free_mobile(iMobile + 1, g_total_mobile)
                        Thread.Sleep(10)
                    Loop While (iMobile < 0)


                    sms.bSending = 1
                    sms.nTry -= 1

                    mob = psMobilePack(iMobile)
                    mob.iSms = iSms
                    mob.tx_flag = 1 ' start tx task
                End If
            End While

            While (Not is_all_Mobile_free())
                Thread.Sleep(10)
            End While

            Thread.Sleep(10)
        End Sub

#End Region

        Public Overrides ReadOnly Property Description As String
            Get
                Return "Portech SMS Driver"
            End Get
        End Property

        Public Overrides Function GetUniqueID() As String
            Return "PTCHSMSDRV"
        End Function

        Protected Overrides Sub InternalConnect()

        End Sub

        Protected Overrides Sub InternalDisconnect()

        End Sub
    End Class

End Namespace