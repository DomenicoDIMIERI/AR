Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports System.IO.Ports
Imports DMD.Nokia

'Namespace DMD


Partial Class SMSGateway



    Public Class OutService
        Inherits DBObjectBase ' DBObjectService


        Private m_Nome As String                                'Nome del servizio
        Private m_ComPort As String                             'Porta COM usata dal modem per inviare gli SMS
        Private m_DeviceSerialNumber As String                  'Numero seriale del telefono associato
        Private m_Credito As Decimal                            'Credito residuo
        Private m_SogliaCredito As Decimal                      'Soglia credito
        Private m_NotificaA As String                           'Notifica (sotto la soglia)
        Private m_CostoSMS As Decimal                           'Costo per ogni SMS inviato
        Private m_ScadenzaCredito As Nullable(Of Date)          'Data di scadenza dell'intero credito
        Private m_Note As String                                'Note 
        Private m_Flags As Integer                              'Flags
        Private m_UserName As String                            'Nome utente utilizzato per l'accesso al servizio
        Private m_Password As String                            'Password utilizzata per l'accesso al servizio
        Private m_Port As SerialPort
        Private m_Device As DMD.Nokia.NokiaDevice
        Private m_Modem As SMSGSMModem

        Public Sub New()
            Me.m_Nome = ""
            Me.m_Credito = 0.0
            Me.m_SogliaCredito = 0.0
            Me.m_NotificaA = ""
            Me.m_CostoSMS = 0.0
            Me.m_ScadenzaCredito = Nothing
            Me.m_Note = ""
            Me.m_Flags = 0
            Me.m_ComPort = "COM2"
            Me.m_DeviceSerialNumber = ""
            Me.m_UserName = ""
            Me.m_Password = ""
            Me.m_Port = Nothing
            Me.m_Device = Nothing

        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome della porta COM associata al modem per inviare i messaggi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ComPort As String
            Get
                Return Me.m_ComPort
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ComPort
                If (oldValue = value) Then Exit Property
                Me.m_ComPort = value
                Me.DoChanged("ComPort", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero seriale del dispositivo associato 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DeviceSerialNumber As String
            Get
                Return Me.m_DeviceSerialNumber
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_DeviceSerialNumber
                If (oldValue = value) Then Exit Property
                Me.m_DeviceSerialNumber = value
                Me.DoChanged("DeviceSerialNumber", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del servizio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il credito residuo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CreditoResiduo As Decimal
            Get
                Return Me.m_Credito
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_Credito
                If (oldValue = value) Then Exit Property
                Me.m_Credito = value
                Me.DoChanged("CreditoResiduo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il costo per il singolo SMS inviato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CostoSMS As Decimal
            Get
                Return Me.m_CostoSMS
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_CostoSMS
                If (oldValue = value) Then Exit Property
                Me.m_CostoSMS = value
                Me.DoChanged("CostoSMS", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di scadenza del credito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ScadenzaCredito As Nullable(Of Date)
            Get
                Return Me.m_ScadenzaCredito
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_ScadenzaCredito
                If (oldValue.Equals(value)) Then Exit Property
                Me.m_ScadenzaCredito = value
                Me.DoChanged("ScadenzaCredito", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta delle note aggiuntive
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Note As String
            Get
                Return Me.m_Note
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Note
                If (oldValue = value) Then Exit Property
                Me.m_Note = value
                Me.DoChanged("Note", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la soglia del credito al di sotto della quale viene inviato un messaggio email all'indirizzo specificato da NotificaA
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SogliaCredito As Decimal
            Get
                Return Me.m_SogliaCredito
            End Get
            Set(value As Decimal)
                Dim oldValue As Decimal = Me.m_SogliaCredito
                If (oldValue = value) Then Exit Property
                Me.m_SogliaCredito = value
                Me.DoChanged("SogliaCredito", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo e-mail a cui viene inviata la mail di notifica sotto la soglia credito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NotificaA As String
            Get
                Return Me.m_NotificaA
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NotificaA
                If (oldValue = value) Then Exit Property
                Me.m_NotificaA = value
                Me.DoChanged("NotificaA", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei flags aggiuntivi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la username utilizzata per l'accesso al servizio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UserName As String
            Get
                Return Me.m_UserName
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_UserName
                If (oldValue = value) Then Exit Property
                Me.m_UserName = value
                Me.DoChanged("UserName", value, oldValue)
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
                Return Me.m_Password
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Password
                If (oldValue = value) Then Exit Property
                Me.m_Password = value
                Me.DoChanged("Password", value, oldValue)
            End Set
        End Property


        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_ComPort = reader.Read("ComPort", Me.m_ComPort)
            Me.m_DeviceSerialNumber = reader.Read("DeviceSerialNumber", Me.m_DeviceSerialNumber)
            Me.m_UserName = reader.Read("UserName", Me.m_UserName)
            Me.m_Password = reader.Read("Password", Me.m_Password)
            Me.m_Credito = reader.Read("Credito", Me.m_Credito)
            Me.m_CostoSMS = reader.Read("CostoSMS", Me.m_CostoSMS)
            Me.m_ScadenzaCredito = reader.Read("ScadenzaCredito", Me.m_ScadenzaCredito)
            Me.m_Note = reader.Read("Note", Me.m_Note)
            Me.m_SogliaCredito = reader.Read("SogliaCredito", Me.m_SogliaCredito)
            Me.m_NotificaA = reader.Read("NotificaA", Me.m_NotificaA)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("ComPort", Me.m_ComPort)
            writer.Write("DeviceSerialNumber", Me.m_DeviceSerialNumber)
            writer.Write("UserName", Me.m_UserName)
            writer.Write("Password", Me.m_Password)
            writer.Write("Credito", Me.m_Credito)
            writer.Write("CostoSMS", Me.m_CostoSMS)
            writer.Write("ScadenzaCredito", Me.m_ScadenzaCredito)
            writer.Write("Note", Me.m_Note)
            writer.Write("SogliaCredito", Me.m_SogliaCredito)
            writer.Write("NotificaA", Me.m_NotificaA)
            writer.Write("Flags", Me.m_Flags)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("ComPort", Me.m_ComPort)
            writer.WriteAttribute("DeviceSerialNumber", Me.m_DeviceSerialNumber)
            writer.WriteAttribute("UserName", Me.m_UserName)
            writer.WriteAttribute("Password", Me.m_Password)
            writer.WriteAttribute("Credito", Me.m_Credito)
            writer.WriteAttribute("CostoSMS", Me.m_CostoSMS)
            writer.WriteAttribute("ScadenzaCredito", Me.m_ScadenzaCredito)
            writer.WriteAttribute("Note", Me.m_Note)
            writer.WriteAttribute("SogliaCredito", Me.m_SogliaCredito)
            writer.WriteAttribute("NotificaA", Me.m_NotificaA)
            writer.WriteAttribute("Flags", Me.m_Flags)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ComPort" : Me.m_ComPort = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DeviceSerialNumber" : Me.m_DeviceSerialNumber = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "UserName" : Me.m_UserName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Password" : Me.m_Password = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Credito" : Me.m_Credito = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "CostoSMS" : Me.m_CostoSMS = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "ScadenzaCredito" : Me.m_ScadenzaCredito = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Note" : Me.m_Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SogliaCredito" : Me.m_SogliaCredito = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "NotificaA" : Me.m_NotificaA = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return SMSGateway.Database
        End Function


        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OutServices"
        End Function

        Public Function GetComPort() As SerialPort
            If (Me.m_Port Is Nothing) Then
                Me.m_Port = New SerialPort
                With Me.m_Port
                    .PortName = Me.ComPort
                    .BaudRate = 19200
                    .Parity = Parity.None
                    .DataBits = 8
                    .StopBits = StopBits.One
                    .Handshake = Handshake.RequestToSend
                    .DtrEnable = True
                    .RtsEnable = True
                    .NewLine = vbCr
                End With
                'Me.m_Port.Open()
            End If
            Return Me.m_Port
        End Function

        Public Function GetDevice() As DMD.Nokia.NokiaDevice
            If (Me.m_Device Is Nothing) Then
                Me.m_Device = DMD.Nokia.Devices.GetItemBySerialNumber(Me.m_DeviceSerialNumber)
            End If
            Return Me.m_Device
        End Function

        Protected Overrides Sub DoChanged(propName As String, Optional newVal As Object = Nothing, Optional oldVal As Object = Nothing)
            If (Me.m_Modem IsNot Nothing) Then
                Me.m_Modem.Close()
            End If
            Me.m_Modem = Nothing
            Me.m_Port = Nothing
            Me.m_Device = Nothing
            MyBase.DoChanged(propName, newVal, oldVal)
        End Sub

        Public Function GetModem() As SMSGSMModem
            If (Me.m_Modem Is Nothing) Then
                Me.m_Modem = New SMSGSMModem
                Me.m_Modem.COMPort = Me.GetComPort
                Me.GetModem.Open()
            End If
            Return Me.m_Modem
        End Function


        Public Sub Send(ByVal sms As SMSMessage)
            SyncLock Me
                'Try
                'Catch ex As Exception
                'End Try

                Try
                    If Not Me.GetModem.COMPort.IsOpen Then Me.GetModem.Open()

                    sms.Cartella = "outbox"
                    sms.StatoInvio = SMSSendStatus.Sending
                    sms.Modem = Me.Nome
                    sms.Save()
                    Me.GetModem.SendSMS(sms.NumeroDestinatario, sms.Messaggio)
                    sms.Cartella = "sent"
                    sms.StatoInvio = SMSSendStatus.Sent
                    sms.DettaglioStatoInvio = "Inviato"

                    If (Me.CostoSMS > 0) Then
                        Me.CreditoResiduo -= Me.CostoSMS
                        Me.Save()
                        If Me.SogliaCredito > 0 AndAlso Me.CreditoResiduo <= Me.SogliaCredito Then
                            Me.NotifyCreditoSottoSoglia()
                        End If
                    End If


                Catch ex As Exception
                    sms.DettaglioStatoInvio = ex.Message
                    sms.StatoInvio = SMSSendStatus.Error
                End Try
                sms.Save()
                Me.GetModem.Close()
            End SyncLock
        End Sub


        Friend Overridable Function FindMessages(ByVal m As CSMSMessage) As CCollection(Of SMSMessage)
            Dim ret As New CCollection(Of SMSMessage)
            Dim numbers() As String = m.Addresses.ToArray(GetType(String))
            If (numbers.Length <= 0) Then Return ret

            DMD.Sistema.Arrays.Sort(numbers)

            Dim cursor As New SMSMessageCursor
            cursor.Cartella.Value = "inbox"
            cursor.NumeroMittente.ValueIn(numbers)
            cursor.DataRicezione.Value = m.Date
            'cursor.StatoRicezione.ValueIn({SMSReceiveStatus.NotReceived, SMSReceiveStatus.Received})
            cursor.Modem.Value = Me.Nome
            cursor.IgnoreRights = True
            Dim item As SMSMessage = Nothing
            While Not cursor.EOF
                ret.Add(cursor.Item)
                Dim i As Integer = DMD.Sistema.Arrays.BinarySearch(numbers, cursor.Item.NumeroMittente)
                If (i >= 0) Then numbers = DMD.Sistema.Arrays.RemoveAt(numbers, i)
                cursor.MoveNext()
            End While
            cursor.Dispose()

            If Arrays.Len(numbers) > 0 Then
                For Each number As String In numbers
                    item = New SMSMessage
                    item.Cartella = "inbox"
                    item.StatoRicezione = SMSReceiveStatus.NotReceived
                    item.DataRicezione = m.Date
                    item.NumeroMittente = number
                    item.Modem = Me.Nome
                    item.Messaggio = m.Message
                    ret.Add(item)
                Next
            End If

            Return ret
        End Function

     

        Protected Friend Overridable Sub NotifyNewMessage(ByVal m As SMSMessage)
            RPC.InvokeMethod(Me.NotificaA & "?_a=NotifyMessageReceived", "svc", RPC.str2n(Me.Nome), "un", RPC.str2n(Me.UserName), "pw", RPC.str2n(Me.Password), "snd", RPC.str2n(m.NumeroMittente), "dt", RPC.date2n(m.DataRicezione), "msg", RPC.str2n(m.Messaggio), "msgid", GetID(m))
        End Sub

        Protected Friend Overridable Sub NotifyCreditoSottoSoglia()
            RPC.InvokeMethod(Me.NotificaA & "?_a=NotifySogliaCredito", "svc", RPC.str2n(Me.Nome), "un", RPC.str2n(Me.UserName), "pw", RPC.str2n(Me.Password), "cd", RPC.float2n(Me.CreditoResiduo))
        End Sub




       

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)

        End Sub
    End Class


End Class

'End Namespace