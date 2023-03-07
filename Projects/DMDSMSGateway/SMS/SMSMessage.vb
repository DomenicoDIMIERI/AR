Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

'Namespace DMD

Partial Class SMSGateway

    <Flags> _
    Public Enum SMSFlags As Integer
        None = 0
    End Enum

    Public Enum SMSSendStatus As Integer
        ''' <summary>
        ''' Messaggio da inviare
        ''' </summary>
        ''' <remarks></remarks>
        NotSend = 0

        ''' <summary>
        ''' Messaggio in fase di invio
        ''' </summary>
        ''' <remarks></remarks>
        Sending = 1

        ''' <summary>
        ''' Messaggio inviato correttamente
        ''' </summary>
        ''' <remarks></remarks>
        Sent = 2

        ''' <summary>
        ''' Messaggio da inviare nuovamente a causa di un errore
        ''' </summary>
        ''' <remarks></remarks>
        Retry = 3

        ''' <summary>
        ''' Messaggio non inviato a causa di un errore
        ''' </summary>
        ''' <remarks></remarks>
        [Error] = 4
    End Enum

    Public Enum SMSReceiveStatus As Integer
        ''' <summary>
        ''' Il messaggio non è stato ricevuto dal destinatario
        ''' </summary>
        ''' <remarks></remarks>
        NotReceived = 0

        ''' <summary>
        ''' Il messaggio è stato ricevuto  
        ''' </summary>
        ''' <remarks></remarks>
        Received = 1

        ''' <summary>
        ''' Il messaggio è stato letto
        ''' </summary>
        ''' <remarks></remarks>
        Read = 2

        ''' <summary>
        ''' Il messaggio è stato eliminato senza leggerlo
        ''' </summary>
        ''' <remarks></remarks>
        Deleted = 3
    End Enum

    Public Class SMSMessage
        Inherits DBObjectBase

        'Private m_ID As Integer
        Private m_Driver As String                  'Nome del driver utilizzato
        Private m_Modem As String                   'Nome del modem utilizzato
        Private m_DataInvio As Nullable(Of Date)                'Data in cui il mittente ha inviato il messaggio
        Private m_DataRicezioneServer As Nullable(Of Date)      'Data in cui il server di invio ha ricevuto il messaggio da inviare
        Private m_DataInvioServer As Nullable(Of Date)          'Data in cui il server ha inviato il messaggio
        Private m_NumeroTentativiInvio As Integer   'Numero di tentativi di invio effettuati
        Private m_DataRicezione As Nullable(Of Date)            'Data di ricezione del messaggio da parte del destinatario
        Private m_NumeroMittente As String          'Numero del mittente
        Private m_NomeMittente As String            'Nome del mittente
        Private m_NumeroDestinatario As String      'Numero del destinatario
        Private m_NomeDestinatario As String        'Nome del destinatario
        Private m_Messaggio As String               'Messaggio
        Private m_Flags As SMSFlags                 'Flags
        'Private m_Attributi As CKeyCollection       'Attributi
        Private m_StatoInvio As SMSSendStatus       'Stato di invio
        Private m_DettaglioStatoInvio As String     'Dettaglio che descrive lo stato di invio
        Private m_StatoRicezione As SMSReceiveStatus    'Stato di ricezione
        Private m_DettaglioStatoRicezione As String     'Dettaglio sullo stato di ricezione
        Private m_DataLettura As Nullable(Of Date)              'Data di conferma lettura
        Private m_MessageID As String               'Stringa che identifica univocamente il messaggio nel contesto dell'uilizzatore
        Private m_Cartella As String

        Public Sub New()
            'Me.m_ID = 0
            Me.m_Driver = ""
            Me.m_Modem = ""
            Me.m_DataInvio = Nothing
            Me.m_DataRicezioneServer = Nothing
            Me.m_DataInvioServer = Nothing
            Me.m_NumeroTentativiInvio = 0
            Me.m_DataRicezione = Nothing
            Me.m_NumeroMittente = ""
            Me.m_NomeMittente = ""
            Me.m_NumeroDestinatario = ""
            Me.m_NomeDestinatario = ""
            Me.m_Messaggio = ""
            Me.m_Flags = SMSFlags.None
            ' Me.m_Attributi = Nothing
            Me.m_StatoInvio = SMSSendStatus.NotSend
            Me.m_DettaglioStatoInvio = ""
            Me.m_StatoRicezione = SMSReceiveStatus.NotReceived
            Me.m_DettaglioStatoRicezione = ""
            Me.m_DataLettura = Nothing
            Me.m_MessageID = ""
            Me.m_Cartella = ""
        End Sub

        Public Property Cartella As String
            Get
                Return Me.m_Cartella
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Cartella
                If (oldValue = value) Then Exit Property
                Me.m_Cartella = value
                Me.DoChanged("Cartella", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restitusice o imposta il nome del driver utilizzato dal server per inviare il messaggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Driver As String
            Get
                Return Me.m_Driver
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Driver
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Driver = value
                Me.DoChanged("Driver", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del modem utilizzato dal server per inviare il messaggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Modem As String
            Get
                Return Me.m_Modem
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Modem
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Modem = value
                Me.DoChanged("Modem", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data in cui l'utente ha richiesto l'invio del messaggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataInvio As Nullable(Of Date)
            Get
                Return Me.m_DataInvio
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataInvio
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataInvio = value
                Me.DoChanged("DataInvio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data in cui il server ha ricevuto la richiesta di invio del messaggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataRicezioneServer As Nullable(Of Date)
            Get
                Return Me.m_DataRicezioneServer
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataRicezioneServer
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataRicezioneServer = value
                Me.DoChanged("DataRicezioneServer", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data in cui 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataInvioServer As Nullable(Of Date)
            Get
                Return Me.m_DataInvioServer
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataInvioServer
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataInvioServer = value
                Me.DoChanged("DataInvioServer", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di tentativi di invio effettuati dal server prima di rinunciare ad inviare il messaggio (in caso di errore)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroTentativiInvio As Integer
            Get
                Return Me.m_NumeroTentativiInvio
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_NumeroTentativiInvio
                If (oldValue = value) Then Exit Property
                Me.m_NumeroTentativiInvio = value
                Me.DoChanged("NumeroTentativiInvio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di ricezione del messaggio da parte del destinatario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataRicezione As Nullable(Of Date)
            Get
                Return Me.m_DataRicezione
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataRicezione
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataRicezione = value
                Me.DoChanged("DataRicezione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero del mittente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroMittente As String
            Get
                Return Me.m_NumeroMittente
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NumeroMittente
                value = Formats.ParsePhoneNumber(value)
                If (value = oldValue) Then Exit Property
                Me.m_NumeroMittente = value
                Me.DoChanged("NumeroMittente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del mittente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeMittente As String
            Get
                Return Me.m_NomeMittente
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeMittente
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeMittente = value
                Me.DoChanged("NomeMittente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del destinatario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroDestinatario As String
            Get
                Return Me.m_NumeroDestinatario
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NumeroDestinatario
                value = Formats.ParsePhoneNumber(value)
                If (oldValue = value) Then Exit Property
                Me.m_NumeroDestinatario = value
                Me.DoChanged("NumeroDestinatario", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del destinatario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeDestinatario As String
            Get
                Return Me.m_NomeDestinatario
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeDestinatario
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeDestinatario = value
                Me.DoChanged("NomeDestinatario", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il contenuto del messaggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Messaggio As String
            Get
                Return Me.m_Messaggio
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Messaggio
                If (oldValue = value) Then Exit Property
                Me.m_Messaggio = value
                Me.DoChanged("Messaggio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei flags
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As SMSFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As SMSFlags)
                Dim oldValue As SMSFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ' ''' <summary>
        ' ''' Restituisce una collezione di attributi
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public ReadOnly Property Attributi As CKeyCollection
        '    Get
        '        If (Me.m_Attributi Is Nothing) Then Me.m_Attributi = New CKeyCollection
        '        Return Me.m_Attributi
        '    End Get
        'End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato di invio del messaggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoInvio As SMSSendStatus
            Get
                Return Me.m_StatoInvio
            End Get
            Set(value As SMSSendStatus)
                Dim oldValue As SMSSendStatus = Me.m_StatoInvio
                If (oldValue = value) Then Exit Property
                Me.m_StatoInvio = value
                Me.DoChanged("StatoInvio", value, oldValue)
            End Set
        End Property

        Public Property DettaglioStatoInvio As String
            Get
                Return Me.m_DettaglioStatoInvio
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_DettaglioStatoInvio
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_DettaglioStatoInvio = value
                Me.DoChanged("DettaglioStatoInvio", value, oldValue)
            End Set
        End Property

        Public Property StatoRicezione As SMSReceiveStatus
            Get
                Return Me.m_StatoRicezione
            End Get
            Set(value As SMSReceiveStatus)
                Dim oldValue As SMSReceiveStatus = Me.m_StatoRicezione
                If (oldValue = value) Then Exit Property
                Me.m_StatoRicezione = value
                Me.DoChanged("StatoRicezione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il dettaglio sullo stato di ricezione del messaggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DettaglioStatoRicezione As String
            Get
                Return Me.m_DettaglioStatoRicezione
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_DettaglioStatoRicezione
                If (oldValue = value) Then Exit Property
                Me.m_DettaglioStatoRicezione = value
                Me.DoChanged("DettaglioStatoRicezione", value, oldValue)
            End Set
        End Property

        Public Property DataLettura As Nullable(Of Date)
            Get
                Return Me.m_DataLettura
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataLettura
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataLettura = value
                Me.DoChanged("DataLettura", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'ID del messaggio definito dall'applicazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MessageID As String
            Get
                Return Me.m_MessageID
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_MessageID
                If (oldValue = value) Then Exit Property
                Me.m_MessageID = value
                Me.DoChanged("MessageID", value, oldValue)
            End Set
        End Property


        'Protected Overrides Function GetConnection() As DMD.Databases.CDBConnection
        '    Return SMSGateway.Database
        'End Function

        'Public Overrides Function GetModule() As DMD.Sistema.CModule
        '    Return Nothing
        'End Function

        'Public Overrides Function GetTableName() As String
        '    Return "tbl_Messaggi"
        'End Function

        'Private Function strToAttr(ByVal str As String) As CKeyCollection
        '    str = Strings.Trim(str)
        '    If (str = "") Then Return New CKeyCollection
        '    Try
        '        Return XML.Utils.Serializer.Deserialize(str)
        '    Catch ex As Exception
        '        Return New CKeyCollection
        '    End Try
        'End Function

        'Private Function attrToStr(ByVal attr As CKeyCollection) As String
        '    Return XML.Utils.Serializer.Serialize(attr)
        'End Function

        'Protected Overrides Function LoadFromRecordset(reader As Databases.DBReader) As Boolean
        '    Me.m_Driver = reader.Read("Driver", Me.m_Driver)
        '    Me.m_Modem = reader.Read("Modem", Me.m_Modem)
        '    Me.m_DataInvio = reader.Read("DataInvio", Me.m_DataInvio)
        '    Me.m_DataRicezioneServer = reader.Read("DataRicezioneServer", Me.m_DataRicezioneServer)
        '    Me.m_DataInvioServer = reader.Read("DataInvioServer", Me.m_DataInvioServer)
        '    Me.m_NumeroTentativiInvio = reader.Read("NumeroTentativiInvio", Me.m_NumeroTentativiInvio)
        '    Me.m_DataRicezione = reader.Read("DataRicezione", Me.m_DataRicezione)
        '    Me.m_NumeroMittente = reader.Read("NumeroMittente", Me.m_NumeroMittente)
        '    Me.m_NomeMittente = reader.Read("NomeMittente", Me.m_NomeMittente)
        '    Me.m_NumeroDestinatario = reader.Read("NumeroDestinatario", Me.m_NumeroDestinatario)
        '    Me.m_NomeDestinatario = reader.Read("NomeDestinatario", Me.m_NomeDestinatario)
        '    Me.m_Messaggio = reader.Read("Messaggio", Me.m_Messaggio)
        '    Me.m_Flags = reader.Read("Flags", Me.m_Flags)
        '    Me.m_Attributi = Me.strToAttr(reader.Read("Attributi", ""))
        '    Me.m_StatoInvio = reader.Read("StatoInvio", Me.m_StatoInvio)
        '    Me.m_DettaglioStatoInvio = reader.Read("DettaglioStatoInvio", Me.m_DettaglioStatoInvio)
        '    Me.m_StatoRicezione = reader.Read("StatoRicezione", Me.m_StatoRicezione)
        '    Me.m_DettaglioStatoRicezione = reader.Read("DettaglioStatoRicezione", Me.m_DettaglioStatoRicezione)
        '    Me.m_DataLettura = reader.Read("DataLettura", Me.m_DataLettura)
        '    Me.m_MessageID = reader.Read("MessageID", Me.m_MessageID)
        '    Return MyBase.LoadFromRecordset(reader)
        'End Function

        'Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
        '    writer.Write("Driver", Me.m_Driver)
        '    writer.Write("Modem", Me.m_Modem)
        '    writer.Write("DataInvio", Me.m_DataInvio)
        '    writer.Write("DataRicezioneServer", Me.m_DataRicezioneServer)
        '    writer.Write("DataInvioServer", Me.m_DataInvioServer)
        '    writer.Write("NumeroTentativiInvio", Me.m_NumeroTentativiInvio)
        '    writer.Write("DataRicezione", Me.m_DataRicezione)
        '    writer.Write("NumeroMittente", Me.m_NumeroMittente)
        '    writer.Write("NomeMittente", Me.m_NomeMittente)
        '    writer.Write("NumeroDestinatario", Me.m_NumeroDestinatario)
        '    writer.Write("NomeDestinatario", Me.m_NomeDestinatario)
        '    writer.Write("Messaggio", Me.m_Messaggio)
        '    writer.Write("Flags", Me.m_Flags)
        '    'writer.Write("Attributi", Me.attrToStr(Me.Attributi))
        '    writer.Write("StatoInvio", Me.m_StatoInvio)
        '    writer.Write("DettaglioStatoInvio", Me.m_DettaglioStatoInvio)
        '    writer.Write("StatoRicezione", Me.m_StatoRicezione)
        '    writer.Write("DettaglioStatoRicezione", Me.m_DettaglioStatoRicezione)
        '    writer.Write("DataLettura", Me.m_DataLettura)
        '    writer.Write("MessageID", Me.m_MessageID)
        '    'Return MyBase.SaveToRecordset(writer)
        'End Function

        'Protected Overrides Sub XMLSerialize(writer As XMLWriter)
        '    writer.WriteAttribute("Driver", Me.m_Driver)
        '    writer.WriteAttribute("Modem", Me.m_Modem)
        '    writer.WriteAttribute("DataInvio", Me.m_DataInvio)
        '    writer.WriteAttribute("DataRicezioneServer", Me.m_DataRicezioneServer)
        '    writer.WriteAttribute("DataInvioServer", Me.m_DataInvioServer)
        '    writer.WriteAttribute("NumeroTentativiInvio", Me.m_NumeroTentativiInvio)
        '    writer.WriteAttribute("DataRicezione", Me.m_DataRicezione)
        '    writer.WriteAttribute("NumeroMittente", Me.m_NumeroMittente)
        '    writer.WriteAttribute("NomeMittente", Me.m_NomeMittente)
        '    writer.WriteAttribute("NumeroDestinatario", Me.m_NumeroDestinatario)
        '    writer.WriteAttribute("NomeDestinatario", Me.m_NomeDestinatario)
        '    writer.WriteAttribute("Flags", Me.m_Flags)
        '    writer.WriteAttribute("StatoInvio", Me.m_StatoInvio)
        '    writer.WriteAttribute("DettaglioStatoInvio", Me.m_DettaglioStatoInvio)
        '    writer.WriteAttribute("StatoRicezione", Me.m_StatoRicezione)
        '    writer.WriteAttribute("DettaglioStatoRicezione", Me.m_DettaglioStatoRicezione)
        '    writer.WriteAttribute("DataLettura", Me.m_DataLettura)
        '    writer.WriteAttribute("MessageID", Me.m_MessageID)
        '    MyBase.XMLSerialize(writer)
        '    writer.WriteTag("Messaggio", Me.m_Messaggio)
        '    writer.WriteTag("Attributi", Me.attrToStr(Me.Attributi))
        'End Sub

        'Private Sub DoChanged(ByVal propName As String, ByVal newValue As Object, ByVal oldValue As Object)

        'End Sub

        'Private Function DBStr(ByVal str As String) As String
        '    If str Is vbNullString Then
        '        Return "NULL"
        '    Else
        '        Return "'" & Replace(str, "'", "''") & "'"
        '    End If
        'End Function

        'Private Function DBNumber(ByVal value As Object) As String
        '    If IsDBNull(value) OrElse value Is Nothing Then
        '        Return "NULL"
        '    Else
        '        Return Trim(Replace(value, ",", "."))
        '    End If
        'End Function

        'Private Function DBBool(ByVal value As Object) As String
        '    If TypeOf (value) Is DBNull Then Return "NULL"
        '    Return IIf(CBool(value), "True", "False")
        'End Function

        'Private Function DBDate(ByVal value As Object) As String
        '    If (TypeOf (value) Is DBNull OrElse value Is Nothing) Then
        '        Return "NULL"
        '    ElseIf (TypeOf (value) Is Nullable(Of Date)) AndAlso (DirectCast(value, Nullable(Of Date)).HasValue = False) Then
        '        Return "NULL"
        '    Else
        '        Dim d As Date = value
        '        Return "#" & Month(d) & "/" & Day(d) & "/" & Year(d) & " " & Hour(d) & ":" & Minute(d) & ":" & Second(d) & "#"
        '    End If
        'End Function

        'Private Function ToInt(ByVal value As Object, Optional ByVal defValue As Integer = 0) As Integer
        '    If (TypeOf (value) Is DBNull) Then Return defValue
        '    Return CInt(value)
        'End Function

        'Private Function ToStr(ByVal value As Object, Optional ByVal defValue As String = "") As String
        '    If (TypeOf (value) Is DBNull) Then Return defValue
        '    Return CStr(value)
        'End Function

        'Private Function ToDate(ByVal value As Object, Optional ByVal defValue As Date = Nothing) As Date
        '    If (TypeOf (value) Is DBNull) Then Return defValue
        '    Return CDate(value)
        'End Function

        'Private Function ParseDate(ByVal value As Object) As Nullable(Of Date)
        '    If (TypeOf (value) Is DBNull) Then Return Nothing
        '    Return CDate(value)
        'End Function

        'Public Sub Load(ByVal dbRis As System.Data.IDataReader)
        '    Me.m_ID = ToInt(dbRis("ID"))
        '    Me.m_Driver = ToStr(dbRis("Driver"))
        '    Me.m_Modem = ToStr(dbRis("Modem"))
        '    Me.m_DataInvio = Me.ParseDate(dbRis("DataInvio"))
        '    Me.m_DataRicezioneServer = Me.ParseDate(dbRis("DataRicezioneServer"))
        '    Me.m_DataRicezioneServer = Me.ParseDate(dbRis("DataRicezioneServer"))
        '    Me.m_DataInvioServer = Me.ParseDate(dbRis("DataInvioServer"))
        '    Me.m_NumeroTentativiInvio = Me.ToInt(dbRis("NumeroTentativiInvio"))
        '    Me.m_DataRicezione = Me.ParseDate(dbRis("DataRicezione"))
        '    Me.m_NomeMittente = Me.ToStr(dbRis("NomeMittente"))
        '    Me.m_NumeroMittente = Me.ToStr(dbRis("NumeroMittente"))
        '    Me.m_NomeDestinatario = Me.ToStr(dbRis("NomeDestinatario"))
        '    Me.m_NumeroDestinatario = Me.ToStr(dbRis("NumeroDestinatario"))
        '    Me.m_Messaggio = Me.ToStr(dbRis("Messaggio"))
        '    Me.m_Flags = Me.ToInt(dbRis("Flags"))
        '    ' Attributi, 
        '    Me.m_StatoInvio = Me.ToInt(dbRis("StatoInvio"))
        '    Me.m_DettaglioStatoInvio = Me.ToStr(dbRis("DettaglioStatoInvio"))
        '    Me.m_StatoRicezione = Me.ToInt(dbRis("StatoRicezione"))
        '    Me.m_DettaglioStatoRicezione = Me.ToStr(dbRis("DettaglioStatoRicezione"))
        '    Me.m_DataLettura = Me.ParseDate(dbRis("DataLettura"))
        '    Me.m_MessageID = Me.ToStr(dbRis("MessageID"))
        'End Sub

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Driver = reader.Read("Driver", Me.m_Driver)
            Me.m_Modem = reader.Read("Modem", Me.m_Modem)
            Me.m_DataInvio = reader.Read("DataInvio", Me.m_DataInvio)
            Me.m_DataRicezioneServer = reader.Read("DataRicezioneServer", Me.m_DataRicezioneServer)
            Me.m_DataInvioServer = reader.Read("DataInvioServer", Me.m_DataInvioServer)
            Me.m_NumeroTentativiInvio = reader.Read("NumeroTentativiInvio", Me.m_NumeroTentativiInvio)
            Me.m_DataRicezione = reader.Read("DataRicezione", Me.m_DataRicezione)
            Me.m_NomeMittente = reader.Read("NomeMittente", Me.m_NomeMittente)
            Me.m_NumeroMittente = reader.Read("NumeroMittente", Me.m_NumeroMittente)
            Me.m_NomeDestinatario = reader.Read("NomeDestinatario", Me.m_NomeDestinatario)
            Me.m_NumeroDestinatario = reader.Read("NumeroDestinatario", Me.m_NumeroDestinatario)
            Me.m_Messaggio = reader.Read("Messaggio", Me.m_Messaggio)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_Cartella = reader.Read("Cartella", Me.m_Cartella)
            ' Attributi, 
            Me.m_StatoInvio = reader.Read("StatoInvio", Me.m_StatoInvio)
            Me.m_DettaglioStatoInvio = reader.Read("DettaglioStatoInvio", Me.m_DettaglioStatoInvio)
            Me.m_StatoRicezione = reader.Read("StatoRicezione", Me.m_StatoRicezione)
            Me.m_DettaglioStatoRicezione = reader.Read("DettaglioStatoRicezione", Me.m_DettaglioStatoRicezione)
            Me.m_DataLettura = reader.Read("DataLettura", Me.m_DataLettura)
            Me.m_MessageID = reader.Read("MessageID", Me.m_MessageID)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Driver", Me.m_Driver)
            writer.Write("Modem", Me.m_Modem)
            writer.Write("DataInvio", Me.m_DataInvio)
            writer.Write("DataRicezioneServer", Me.m_DataRicezioneServer)
            writer.Write("DataInvioServer", Me.m_DataInvioServer)
            writer.Write("NumeroTentativiInvio", Me.m_NumeroTentativiInvio)
            writer.Write("DataRicezione", Me.m_DataRicezione)
            writer.Write("NomeMittente", Me.m_NomeMittente)
            writer.Write("NumeroMittente", Me.m_NumeroMittente)
            writer.Write("NomeDestinatario", Me.m_NomeDestinatario)
            writer.Write("NumeroDestinatario", Me.m_NumeroDestinatario)
            writer.Write("Messaggio", Me.m_Messaggio)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Cartella", Me.m_Cartella)
            ' Attributi, 
            writer.Write("StatoInvio", Me.m_StatoInvio)
            writer.Write("DettaglioStatoInvio", Me.m_DettaglioStatoInvio)
            writer.Write("StatoRicezione", Me.m_StatoRicezione)
            writer.Write("DettaglioStatoRicezione", Me.m_DettaglioStatoRicezione)
            writer.Write("DataLettura", Me.m_DataLettura)
            writer.Write("MessageID", Me.m_MessageID)
            Return MyBase.SaveToRecordset(writer)
        End Function

        'Public Sub Save()


        '    Dim dbSQL As String = ""
        '    If Me.m_ID = 0 Then
        '        dbSQL = "INSERT INTO tbl_Messaggi "
        '        dbSQL &= "(Driver, Modem, DataInvio, DataRicezioneServer, DataInvioServer, NumeroTentativiInvio, DataRicezione, NomeMittente, NumeroMittente, NomeDestinatario, NumeroDestinatario, Messaggio, Flags, Attributi, StatoInvio, DettaglioStatoInvio, StatoRicezione, DettaglioStatoRicezione, DataLettura, MessageID) "
        '        dbSQL &= " VALUES "
        '        dbSQL &= "(" & DBStr(Me.Driver) & ", " & DBStr(Me.Modem) & ", " & DBDate(Me.DataInvio) & ", " & DBDate(Me.DataRicezioneServer) & ", " & DBDate(Me.DataInvioServer) & ", " & DBNumber(NumeroTentativiInvio) & ", " & DBDate(DataRicezione) & ", " & DBStr(Me.NomeMittente) & ", " & DBStr(Me.NumeroMittente) & ", " & DBStr(Me.NomeDestinatario) & ", " & DBStr(Me.NumeroDestinatario) & ", " & DBStr(Me.Messaggio) & ", " & DBNumber(Me.Flags) & ", NULL, " & DBNumber(Me.StatoInvio) & "," & DBStr(Me.DettaglioStatoInvio) & ", " & DBNumber(Me.StatoRicezione) & ", " & DBStr(Me.DettaglioStatoRicezione) & ", " & DBDate(Me.DataLettura) & ", " & DBStr(Me.MessageID) & ")"
        '        '  System.Web.HttpContext.Current.Response.Write(dbSQL)
        '        ' System.Web.HttpContext.Current.Response.End()

        '        SMSGateway.Database.ExecuteCommand(dbSQL)
        '        Me.m_ID = SMSGateway.Database.ExecuteScalar("SELECT @@IDENTITY")
        '    Else
        '        dbSQL = "UPDATE tbl_Messaggi SET "
        '        dbSQL &= "Driver = " & DBStr(Me.Driver) & ", "
        '        dbSQL &= "Modem= " & DBStr(Me.Modem) & ", "
        '        dbSQL &= "DataInvio= " & DBDate(Me.DataInvio) & ", "
        '        dbSQL &= "DataRicezioneServer= " & DBDate(Me.DataRicezioneServer) & ", "
        '        dbSQL &= "DataInvioServer= " & DBDate(Me.DataInvioServer) & ", "
        '        dbSQL &= "NumeroTentativiInvio= " & DBNumber(Me.NumeroTentativiInvio) & ", "
        '        dbSQL &= "DataRicezione= " & DBDate(Me.DataRicezione) & ", "
        '        dbSQL &= "NomeMittente= " & DBStr(Me.NomeMittente) & ", "
        '        dbSQL &= "NumeroMittente= " & DBStr(Me.NumeroMittente) & ", "
        '        dbSQL &= "NomeDestinatario= " & DBStr(Me.NomeDestinatario) & ", "
        '        dbSQL &= "NumeroDestinatario= " & DBStr(Me.NumeroDestinatario) & ", "
        '        dbSQL &= "Messaggio= " & DBStr(Me.Messaggio) & ", "
        '        dbSQL &= "Flags= " & DBNumber(Me.Flags) & ", "
        '        dbSQL &= "Attributi= NULL, "
        '        dbSQL &= "StatoInvio= " & DBNumber(Me.StatoInvio) & ", "
        '        dbSQL &= "DettaglioStatoInvio= " & DBStr(Me.DettaglioStatoInvio) & ", "
        '        dbSQL &= "StatoRicezione= " & DBNumber(Me.StatoRicezione) & ", "
        '        dbSQL &= "DettaglioStatoRicezione= " & DBStr(Me.DettaglioStatoRicezione) & ", "
        '        dbSQL &= "DataLettura= " & DBDate(Me.DataLettura) & ", "
        '        dbSQL &= "MessageID= " & DBStr(Me.MessageID)
        '        dbSQL &= " WHERE [ID]=" & Me.m_ID
        '        SMSGateway.Database.ExecuteCommand(dbSQL)
        '    End If

        'End Sub

        Public Overrides Function GetTableName() As String
            Return "tbl_Messaggi"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return SMSGateway.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function
    End Class

End Class

'End Namespace