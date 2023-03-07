Imports FinSeA
Imports Atom8.Communications.Fax

Namespace FinSeA


    Partial Class FaxGateway

        <Flags> _
        Public Enum FaxFlags As Integer
            None = 0
        End Enum

        Public Enum FaxSendStatus As Integer
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

        Public Enum FaxReceiveStatus As Integer
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


        Public Class FaxDocument
            'Inherits FinSeA.Databases.DBObjectBase

            Private m_ID As Integer
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
            Private m_Flags As FaxFlags                 'Flags
            'Private m_Attributi As CKeyCollection       'Attributi
            Private m_StatoInvio As FaxSendStatus       'Stato di invio
            Private m_DettaglioStatoInvio As String     'Dettaglio che descrive lo stato di invio
            Private m_StatoRicezione As FaxReceiveStatus    'Stato di ricezione
            Private m_DettaglioStatoRicezione As String     'Dettaglio sullo stato di ricezione
            Private m_DataLettura As Nullable(Of Date)              'Data di conferma lettura
            Private m_MessageID As String               'Stringa che identifica univocamente il messaggio nel contesto dell'uilizzatore
            Private m_Attached As String                'Percorso del file allegato (inviato)
            Private m_Parametro1 As String

            Public Sub New()
                Me.m_ID = 0
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
                Me.m_Flags = FaxFlags.None
                ' Me.m_Attributi = Nothing
                Me.m_StatoInvio = FaxSendStatus.NotSend
                Me.m_DettaglioStatoInvio = ""
                Me.m_StatoRicezione = FaxReceiveStatus.NotReceived
                Me.m_DettaglioStatoRicezione = ""
                Me.m_DataLettura = Nothing
                Me.m_MessageID = ""
                Me.m_Attached = ""
                Me.m_Parametro1 = ""
            End Sub

            Public ReadOnly Property ID As Integer
                Get
                    Return Me.m_ID
                End Get
            End Property

            ''' <summary>
            ''' Restituisce o imposta un parametro usato internamente
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Parametro1 As String
                Get
                    Return Me.m_Parametro1
                End Get
                Set(value As String)
                    Dim oldValue As String = Me.m_Parametro1
                    value = Trim(value)
                    If (oldValue = value) Then Exit Property
                    Me.m_Parametro1 = value
                    Me.DoChanged("Parametro1", value, oldValue)
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
                    If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
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
                    If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
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
                    If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
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
                    If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
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
            Public Property Flags As FaxFlags
                Get
                    Return Me.m_Flags
                End Get
                Set(value As FaxFlags)
                    Dim oldValue As FaxFlags = Me.m_Flags
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
            Public Property StatoInvio As FaxSendStatus
                Get
                    Return Me.m_StatoInvio
                End Get
                Set(value As FaxSendStatus)
                    Dim oldValue As FaxSendStatus = Me.m_StatoInvio
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

            Public Property StatoRicezione As FaxReceiveStatus
                Get
                    Return Me.m_StatoRicezione
                End Get
                Set(value As FaxReceiveStatus)
                    Dim oldValue As FaxReceiveStatus = Me.m_StatoRicezione
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
                    If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
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

            ''' <summary>
            ''' Restituisce o imposta la URL relativa del file inviato come Fax
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Attached As String
                Get
                    Return Me.m_Attached
                End Get
                Set(value As String)
                    Dim oldValue As String = Me.m_Attached
                    value = Trim(value)
                    If (oldValue = value) Then Exit Property
                    Me.m_Attached = value
                    Me.DoChanged("Attached", value, oldValue)
                End Set
            End Property

            'Protected Overrides Function GetConnection() As FinSeA.Databases.CDBConnection
            '    Return SMSGateway.Database
            'End Function

            'Public Overrides Function GetModule() As FinSeA.Sistema.CModule
            '    Return Nothing
            'End Function

            'Public Overrides Function GetTableName() As String
            '    Return "tbl_Fax"
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

            Private Sub DoChanged(ByVal propName As String, ByVal newValue As Object, ByVal oldValue As Object)

            End Sub

            Private Function DBStr(ByVal str As String) As String
                If str Is vbNullString Then
                    Return "NULL"
                Else
                    Return "'" & Replace(str, "'", "''") & "'"
                End If
            End Function

            Private Function DBNumber(ByVal value As Object) As String
                If IsDBNull(value) OrElse value Is Nothing Then
                    Return "NULL"
                Else
                    Return Trim(Replace(value, ",", "."))
                End If
            End Function

            Private Function DBBool(ByVal value As Object) As String
                If TypeOf (value) Is DBNull Then Return "NULL"
                Return IIf(CBool(value), "True", "False")
            End Function

            Private Function DBDate(ByVal value As Object) As String
                If (TypeOf (value) Is DBNull OrElse value Is Nothing) Then
                    Return "NULL"
                ElseIf (TypeOf (value) Is Nullable(Of Date)) AndAlso (DirectCast(value, Nullable(Of Date)).HasValue = False) Then
                    Return "NULL"
                Else
                    Dim d As Date = value
                    Return "#" & Month(d) & "/" & Day(d) & "/" & Year(d) & " " & Hour(d) & ":" & Minute(d) & ":" & Second(d) & "#"
                End If
            End Function

            Private Function ToInt(ByVal value As Object, Optional ByVal defValue As Integer = 0) As Integer
                If (TypeOf (value) Is DBNull) Then Return defValue
                Return CInt(value)
            End Function

            Private Function ToStr(ByVal value As Object, Optional ByVal defValue As String = "") As String
                If (TypeOf (value) Is DBNull) Then Return defValue
                Return CStr(value)
            End Function

            Private Function ToDate(ByVal value As Object, Optional ByVal defValue As Date = Nothing) As Date
                If (TypeOf (value) Is DBNull) Then Return defValue
                Return CDate(value)
            End Function

            Private Function ParseDate(ByVal value As Object) As Nullable(Of Date)
                If (TypeOf (value) Is DBNull) Then Return Nothing
                Return CDate(value)
            End Function

            Public Sub Load(ByVal dbRis As System.Data.IDataReader)
                Me.m_ID = ToInt(dbRis("ID"))
                Me.m_Driver = ToStr(dbRis("Driver"))
                Me.m_Modem = ToStr(dbRis("Modem"))
                Me.m_DataInvio = Me.ParseDate(dbRis("DataInvio"))
                Me.m_DataRicezioneServer = Me.ParseDate(dbRis("DataRicezioneServer"))
                Me.m_DataRicezioneServer = Me.ParseDate(dbRis("DataRicezioneServer"))
                Me.m_DataInvioServer = Me.ParseDate(dbRis("DataInvioServer"))
                Me.m_NumeroTentativiInvio = Me.ToInt(dbRis("NumeroTentativiInvio"))
                Me.m_DataRicezione = Me.ParseDate(dbRis("DataRicezione"))
                Me.m_NomeMittente = Me.ToStr(dbRis("NomeMittente"))
                Me.m_NumeroMittente = Me.ToStr(dbRis("NumeroMittente"))
                Me.m_NomeDestinatario = Me.ToStr(dbRis("NomeDestinatario"))
                Me.m_NumeroDestinatario = Me.ToStr(dbRis("NumeroDestinatario"))
                Me.m_Messaggio = Me.ToStr(dbRis("Messaggio"))
                Me.m_Flags = Me.ToInt(dbRis("Flags"))
                ' Attributi, 
                Me.m_StatoInvio = Me.ToInt(dbRis("StatoInvio"))
                Me.m_DettaglioStatoInvio = Me.ToStr(dbRis("DettaglioStatoInvio"))
                Me.m_StatoRicezione = Me.ToInt(dbRis("StatoRicezione"))
                Me.m_DettaglioStatoRicezione = Me.ToStr(dbRis("DettaglioStatoRicezione"))
                Me.m_DataLettura = Me.ParseDate(dbRis("DataLettura"))
                Me.m_MessageID = Me.ToStr(dbRis("MessageID"))
                Me.m_Attached = Me.ToStr(dbRis("Attached"))
                Me.m_Parametro1 = Me.ToStr(dbRis("Parametro1"))
            End Sub

            Public Sub Save()


                Dim dbSQL As String = ""
                If Me.m_ID = 0 Then
                    dbSQL = "INSERT INTO tbl_Fax "
                    dbSQL &= "(Driver, Parametro1, Modem, DataInvio, DataRicezioneServer, DataInvioServer, NumeroTentativiInvio, DataRicezione, NomeMittente, NumeroMittente, NomeDestinatario, NumeroDestinatario, Messaggio, Flags, Attributi, StatoInvio, DettaglioStatoInvio, StatoRicezione, DettaglioStatoRicezione, DataLettura, MessageID, Attached) "
                    dbSQL &= " VALUES "
                    dbSQL &= "(" & DBStr(Me.Driver) & ", " & DBStr(Me.Parametro1) & ", " & DBStr(Me.Modem) & ", " & DBDate(Me.DataInvio) & ", " & DBDate(Me.DataRicezioneServer) & ", " & DBDate(Me.DataInvioServer) & ", " & DBNumber(NumeroTentativiInvio) & ", " & DBDate(DataRicezione) & ", " & DBStr(Me.NomeMittente) & ", " & DBStr(Me.NumeroMittente) & ", " & DBStr(Me.NomeDestinatario) & ", " & DBStr(Me.NumeroDestinatario) & ", " & DBStr(Me.Messaggio) & ", " & DBNumber(Me.Flags) & ", NULL, " & DBNumber(Me.StatoInvio) & "," & DBStr(Me.DettaglioStatoInvio) & ", " & DBNumber(Me.StatoRicezione) & ", " & DBStr(Me.DettaglioStatoRicezione) & ", " & DBDate(Me.DataLettura) & ", " & DBStr(Me.MessageID) & "," & DBStr(Me.Attached) & ")"
                    '  System.Web.HttpContext.Current.Response.Write(dbSQL)
                    ' System.Web.HttpContext.Current.Response.End()

                    FaxGateway.Database.ExecuteCommand(dbSQL)
                    Me.m_ID = FaxGateway.Database.ExecuteScalar("SELECT @@IDENTITY")
                Else
                    dbSQL = "UPDATE tbl_Fax SET "
                    dbSQL &= "Driver = " & DBStr(Me.Driver) & ", "
                    dbSQL &= "Parametro1 = " & DBStr(Me.Parametro1) & ", "
                    dbSQL &= "Modem= " & DBStr(Me.Modem) & ", "
                    dbSQL &= "DataInvio= " & DBDate(Me.DataInvio) & ", "
                    dbSQL &= "DataRicezioneServer= " & DBDate(Me.DataRicezioneServer) & ", "
                    dbSQL &= "DataInvioServer= " & DBDate(Me.DataInvioServer) & ", "
                    dbSQL &= "NumeroTentativiInvio= " & DBNumber(Me.NumeroTentativiInvio) & ", "
                    dbSQL &= "DataRicezione= " & DBDate(Me.DataRicezione) & ", "
                    dbSQL &= "NomeMittente= " & DBStr(Me.NomeMittente) & ", "
                    dbSQL &= "NumeroMittente= " & DBStr(Me.NumeroMittente) & ", "
                    dbSQL &= "NomeDestinatario= " & DBStr(Me.NomeDestinatario) & ", "
                    dbSQL &= "NumeroDestinatario= " & DBStr(Me.NumeroDestinatario) & ", "
                    dbSQL &= "Messaggio= " & DBStr(Me.Messaggio) & ", "
                    dbSQL &= "Flags= " & DBNumber(Me.Flags) & ", "
                    dbSQL &= "Attributi= NULL, "
                    dbSQL &= "StatoInvio= " & DBNumber(Me.StatoInvio) & ", "
                    dbSQL &= "DettaglioStatoInvio= " & DBStr(Me.DettaglioStatoInvio) & ", "
                    dbSQL &= "StatoRicezione= " & DBNumber(Me.StatoRicezione) & ", "
                    dbSQL &= "DettaglioStatoRicezione= " & DBStr(Me.DettaglioStatoRicezione) & ", "
                    dbSQL &= "DataLettura= " & DBDate(Me.DataLettura) & ", "
                    dbSQL &= "Attached=" & DBStr(Me.Attached) & ", "
                    dbSQL &= "MessageID= " & DBStr(Me.MessageID)
                    dbSQL &= " WHERE [ID]=" & Me.m_ID
                    FaxGateway.Database.ExecuteCommand(dbSQL)
                End If

            End Sub

            Sub UpdateStatus()
                Dim status As HylafaxJob = FaxGateway.GetJobStatus(Me.Parametro1)
                If (status Is Nothing) Then Throw New ArgumentException("Fax non trovato")

                Me.Save()
            End Sub

            Sub Send()
                Me.Save()
                Me.Parametro1 = FaxGateway.SendFax(Me.NumeroDestinatario, Me.Attached) '"*k k#s " &
                Me.StatoInvio = FaxSendStatus.Sending
                Me.DettaglioStatoInvio = "Sending"
                Me.Save()
            End Sub

        End Class


    End Class

End Namespace