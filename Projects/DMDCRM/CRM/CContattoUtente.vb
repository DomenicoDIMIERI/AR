Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls

    Public Enum EsitoChiamata As Integer
        NESSUNA_RISPOSTA = 0
        RIPOSTA = 1
        ALTRO = 255
    End Enum

    Public Enum StatoConversazione As Integer
        INATTESA = 0
        INCORSO = 1
        CONCLUSO = 2
    End Enum
 
    <Serializable()> _
    Public MustInherit Class CContattoUtente
        Inherits DBObjectPO
        Implements IComparable, IComparable(Of CContattoUtente), IIndexable

        Private m_IDAzienda As Integer
        Private m_Azienda = Nothing
        Private m_IDPersona As Integer '[int] ID della persona associata
        Private m_NomePersona As String
        Private m_Persona As CPersona '[CPersona] Oggetto CPersona associato
        Private m_IDContesto As Integer    '[int]  ID del contesto
        Private m_Contesto As String '[Text] Tipo del contesto
        Private m_Ricevuta As Boolean '[Bool] Se vero indica che si tratta di una chiamata ricevuta. Se falso si tratta di una chiamata effettuata
        Private m_Scopo As String '[Text] Scopo della chiamata
        Private m_Data As Date '[Date] Data e ora della chiamata
        Private m_IDOperatore As Integer '[Int]  ID dell'operatore che ha effettuato la chiamata o che ha risposto
        Private m_Operatore As CUser '[CUser] Oggetto CUser che rappresenta l'utenza dell'operatore che ha gestito la chiamata
        Private m_NomeOperatore As String '[text] Nome dell'operatore
        Private m_Note As String '[text] Campo generico
        Private m_Esito As EsitoChiamata '[INT] 0 nessuna risposta/non trovato, 1 Risposto/trovato, 255 altro (vedi dettaglio)
        Private m_DettaglioEsito As String '[TEXT] Stringa che aggiunge informazioni sull'esito
        Private m_Durata As Single 'Durata del contatto in secondi
        Private m_MessageID As String
        Private m_Options As CSettings
        Private m_NumeroOIndirizzo As String
        Private m_NomeIndirizzo As String
        Private m_Attesa As Single
        Private m_StatoConversazione As StatoConversazione
        Private m_OldStatoConversazione As StatoConversazione
        Private m_IDAccoltoDa As Integer
        Private m_NomeAccoltoDa As String
        Private m_AccoltoDa As CUser
        Private m_DataRicezione As Nullable(Of Date)
        Private m_AttachmentID As Integer
        Private m_Attachment As CAttachment
        Private m_IDPerContoDi As Integer
        Private m_PerContoDi As CPersona
        Private m_NomePerContoDi As String
        Private m_Trasferimenti As CCollection(Of TrasferimentoContatto)
        Private m_Costo As Decimal?

        Public Sub New()
            Me.m_IDPersona = 0
            Me.m_Persona = Nothing
            Me.m_NomePersona = ""
            Me.m_IDContesto = 0
            Me.m_Contesto = Nothing
            Me.m_Ricevuta = True
            Me.m_Scopo = ""
            Me.m_Data = DateUtils.ToDay
            Me.m_IDOperatore = 0
            Me.m_Operatore = Nothing
            Me.m_NomeOperatore = ""
            Me.m_Note = ""
            Me.m_Esito = 0
            Me.m_DettaglioEsito = ""
            Me.m_Durata = 0
            Me.m_MessageID = vbNullString
            Me.m_Options = Nothing
            Me.m_NumeroOIndirizzo = vbNullString
            Me.m_NomeIndirizzo = vbNullString
            Me.m_IDAzienda = 0
            Me.m_Azienda = Nothing
            Me.m_Attesa = 0
            Me.m_StatoConversazione = StatoConversazione.INATTESA
            Me.m_OldStatoConversazione = CustomerCalls.StatoConversazione.INATTESA
            Me.m_IDAccoltoDa = 0
            Me.m_NomeAccoltoDa = ""
            Me.m_AccoltoDa = Nothing
            Me.m_DataRicezione = Nothing
            Me.m_Attachment = Nothing
            Me.m_AttachmentID = 0
            Me.m_IDPerContoDi = 0
            Me.m_PerContoDi = Nothing
            Me.m_NomePerContoDi = ""
            Me.m_Trasferimenti = Nothing
            Me.m_Costo = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il costo totale delle telefonata, sms ecc..
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Costo As Decimal?
            Get
                Return Me.m_Costo
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_Costo
                If (oldValue = value) Then Exit Property
                Me.m_Costo = value
                Me.DoChanged("Costo", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Trasferimenti As CCollection(Of TrasferimentoContatto)
            Get
                SyncLock Me
                    If (Me.m_Trasferimenti Is Nothing) Then
                        Try
                            Dim tmp As String = Me.Options.GetValueString("Trasferimenti", "")
                            Me.m_Trasferimenti = XML.Utils.Serializer.Deserialize(tmp)
                        Catch ex As Exception
                            Me.m_Trasferimenti = New CCollection(Of TrasferimentoContatto)
                        End Try
                    End If
                    Return Me.m_Trasferimenti
                End SyncLock
            End Get
        End Property

        Public Function Trasferisci(ByVal trasferisciA As CUser, ByVal messaggio As String) As TrasferimentoContatto
            If (trasferisciA Is Nothing) Then Throw New ArgumentNullException("trasferisciA")

            Me.StatoConversazione = CustomerCalls.StatoConversazione.INATTESA

            Dim t As New TrasferimentoContatto
            t.Contatto = Me
            t.Operatore = Sistema.Users.CurrentUser
            t.DataTrasferimento = DateUtils.Now
            t.TrasferitoA = trasferisciA
            t.Messaggio = messaggio
            Me.Trasferimenti.Add(t)
            Me.SetChanged(True)
            Me.Save()

            Return t
        End Function

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'allegato 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AttachmentID As Integer
            Get
                Return GetID(Me.m_Attachment, Me.m_AttachmentID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.AttachmentID
                If (oldValue = value) Then Exit Property
                Me.m_AttachmentID = value
                Me.m_Attachment = Nothing
                Me.DoChanged("AttachmentID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'allegato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Attachment As CAttachment
            Get
                If (Me.m_Attachment Is Nothing) Then Me.m_Attachment = Sistema.Attachments.GetItemById(Me.m_AttachmentID)
                Return Me.m_Attachment
            End Get
            Set(value As CAttachment)
                Dim oldValue As CAttachment = Me.m_Attachment
                If (oldValue Is value) Then Exit Property
                Me.m_Attachment = value
                Me.m_AttachmentID = GetID(value)
                Me.DoChanged("Attachment", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha ricevuto per la prima volta il contatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDAccoltoDa As Integer
            Get
                Return GetID(Me.m_AccoltoDa, Me.m_IDAccoltoDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAccoltoDa
                If (oldValue = value) Then Exit Property
                Me.m_IDAccoltoDa = value
                Me.m_AccoltoDa = Nothing
                Me.DoChanged("IDAccoltoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha ricevuto per la prima volta il contatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AccoltoDa As CUser
            Get
                If (Me.m_AccoltoDa Is Nothing) Then Me.m_AccoltoDa = Sistema.Users.GetItemById(Me.m_IDAccoltoDa)
                Return Me.m_AccoltoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_AccoltoDa
                If (oldValue Is value) Then Exit Property
                Me.m_AccoltoDa = value
                Me.m_IDAccoltoDa = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeAccoltoDa = value.Nominativo
                Me.DoChanged("AccoltoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che ha ricevuto per la prima volta il contatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeAccoltoDa As String
            Get
                Return Me.m_NomeAccoltoDa
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeAccoltoDa
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeAccoltoDa = value
                Me.DoChanged("NomeAccoltoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituiscec o imposta la data di ricezione del cliente (senza l'attesa)
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
        ''' Restituisce o imposta lo stato della conversazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoConversazione As StatoConversazione
            Get
                Return Me.m_StatoConversazione
            End Get
            Set(value As StatoConversazione)
                Dim oldValue As StatoConversazione = Me.m_StatoConversazione
                If (oldValue = value) Then Exit Property
                Me.m_StatoConversazione = value
                Me.DoChanged("StatoConversazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'azienda che ha registrato la pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDAzienda As Integer
            Get
                Return GetID(Me.m_Azienda, Me.m_IDAzienda)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAzienda
                If (oldValue = value) Then Exit Property
                Me.m_IDAzienda = value
                Me.m_Azienda = Nothing
                Me.DoChanged("IDAzienda", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'azienda che ha registrato la pratica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Azienda As CAzienda
            Get
                If (Me.m_Azienda Is Nothing) Then Me.m_Azienda = Anagrafica.Aziende.GetItemById(Me.m_IDAzienda)
                Return Me.m_Azienda
            End Get
            Set(value As CAzienda)
                Dim oldValue As CAzienda = Me.m_Azienda
                If (oldValue Is value) Then Exit Property
                Me.m_Azienda = value
                Me.m_IDAzienda = GetID(value)
                Me.DoChanged("Azienda", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetAzienda(ByVal value As CAzienda)
            Me.m_Azienda = value
            Me.m_IDAzienda = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo utilizzato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Property NumeroOIndirizzo As String
            Get
                Return Me.m_NumeroOIndirizzo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NumeroOIndirizzo
                If (oldValue = value) Then Exit Property
                Me.m_NumeroOIndirizzo = value
                Me.DoChanged("NumeroOIndirizzo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'etichetta dell'indirizzo utilizzato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeIndirizzo As String
            Get
                Return Me.m_NomeIndirizzo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeIndirizzo
                If (oldValue = value) Then Exit Property
                Me.m_NomeIndirizzo = value
                Me.DoChanged("NomeIndirizzo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce una collezione di parametri aggiuntivi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Options As CSettings
            Get
                If (Me.m_Options Is Nothing) Then Me.m_Options = New CSettings
                Return Me.m_Options
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'identificativo del messaggio in un sistema esterno
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MessageID As String
            Get
                Return Me.m_MessageID
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_MessageID
                If (oldValue = value) Then Exit Property
                Me.m_MessageID = value
                Me.DoChanged("MessageID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la durata (in secondi) della comunicazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Durata As Single
            Get
                Return Me.m_Durata
            End Get
            Set(value As Single)
                Dim oldValue As Single = Me.m_Durata
                If (oldValue = value) Then Exit Property
                Me.m_Durata = value
                Me.DoChanged("Durata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'attesa (tempo prima della risposta in caso di una telefonata o tempo in sala di attesa per una visita)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Attesa As Single
            Get
                Return Me.m_Attesa
            End Get
            Set(value As Single)
                Dim oldValue As Single = Me.m_Attesa
                If (oldValue = value) Then Exit Property
                Me.m_Attesa = value
                Me.DoChanged("Attesa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della persona con cui è avvenuta la comunicazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPersona As Integer
            Get
                Return GetID(Me.m_Persona, Me.m_IDPersona)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPersona
                If oldValue = value Then Exit Property
                Me.m_IDPersona = value
                Me.m_Persona = Nothing
                Me.DoChanged("IDPersona", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' 'Restituisce o imposta la persona con cui è avvenuta la comunicazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Persona As CPersona
            Get
                If Me.m_Persona Is Nothing Then Me.m_Persona = Anagrafica.Persone.GetItemById(Me.m_IDPersona)
                Return Me.m_Persona
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Persona
                If (oldValue = value) Then Exit Property
                Me.m_Persona = value
                Me.m_IDPersona = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomePersona = value.Nominativo
                Me.DoChanged("Persona", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetPersona(ByVal value As CPersona)
            Me.m_Persona = value
            Me.m_IDPersona = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome della persona con cui è avvenuta la comunicazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomePersona As String
            Get
                Return Me.m_NomePersona
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomePersona
                If (oldValue = value) Then Exit Property
                Me.m_NomePersona = value
                Me.DoChanged("NomePersona", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della persona per cui è stata fatta la chiamata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPerContoDi As Integer
            Get
                Return GetID(Me.m_PerContoDi, Me.m_IDPerContoDi)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPerContoDi
                If oldValue = value Then Exit Property
                Me.m_IDPerContoDi = value
                Me.m_PerContoDi = Nothing
                Me.DoChanged("IDPerContoDi", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' 'Restituisce o imposta la persona per cui è stata fatta la chiamata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PerContoDi As CPersona
            Get
                If Me.m_PerContoDi Is Nothing Then Me.m_PerContoDi = Anagrafica.Persone.GetItemById(Me.m_IDPerContoDi)
                Return Me.m_PerContoDi
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.PerContoDi
                If (oldValue = value) Then Exit Property
                Me.m_PerContoDi = value
                Me.m_IDPerContoDi = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomePerContoDi = value.Nominativo
                Me.DoChanged("PerContoDi", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetPerContoDi(ByVal value As CPersona)
            Me.m_PerContoDi = value
            Me.m_IDPerContoDi = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome della persona per cui è stata fatta la chiamata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomePerContoDi As String
            Get
                Return Me.m_NomePerContoDi
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomePerContoDi
                If (oldValue = value) Then Exit Property
                Me.m_NomePerContoDi = value
                Me.DoChanged("NomePerContoDi", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o i mposta l'ID del contesto in cui è avvenuta la comunicazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDContesto As Integer
            Get
                Return Me.m_IDContesto
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_IDContesto
                If (oldValue = value) Then Exit Property
                Me.m_IDContesto = value
                Me.DoChanged("IDConstesto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del contesto in cui è avvenuta la comunicazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Contesto As String
            Get
                Return Me.m_Contesto
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Contesto
                If (oldValue = value) Then Exit Property
                Me.m_Contesto = value
                Me.DoChanged("Contesto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce vero se la comunicazione è in ingresso
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Ricevuta As Boolean
            Get
                Return Me.m_Ricevuta
            End Get
            Set(value As Boolean)
                If (Me.m_Ricevuta = value) Then Exit Property
                Me.m_Ricevuta = value
                Me.DoChanged("Ricevuta", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che indica lo scopo della comunicazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Scopo As String
            Get
                Return Me.m_Scopo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Scopo
                If (oldValue = value) Then Exit Property
                Me.m_Scopo = value
                Me.DoChanged("Scopo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data della comunicazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Data As Date
            Get
                Return Me.m_Data
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_Data
                If (oldValue = value) Then Exit Property
                Me.m_Data = value
                Me.DoChanged("Data", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'operatore che ha seguito la conversazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDOperatore As Integer
            Get
                Return GetID(Me.m_Operatore, Me.m_IDOperatore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOperatore
                If oldValue = value Then Exit Property
                Me.m_IDOperatore = value
                Me.m_Operatore = Nothing
                Me.DoChanged("IDOperatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'operatore che ha seguito la conversazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeOperatore As String
            Get
                Return Me.m_NomeOperatore
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeOperatore
                If (oldValue = value) Then Exit Property
                Me.m_NomeOperatore = value
                Me.DoChanged("NomeOperatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'operatore che ha seguito la conversazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Operatore As CUser
            Get
                If Me.m_Operatore Is Nothing Then Me.m_Operatore = Sistema.Users.GetItemById(Me.m_IDOperatore)
                Return Me.m_Operatore
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Operatore
                If (oldValue = value) Then Exit Property
                Me.m_Operatore = value
                Me.m_IDOperatore = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeOperatore = value.Nominativo
                Me.DoChanged("Operatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il testo della conversazione
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
        ''' Restituisce o imposta l'esito della conversazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Esito As EsitoChiamata
            Get
                Return Me.m_Esito
            End Get
            Set(value As EsitoChiamata)
                Dim oldValue As EsitoChiamata = Me.m_Esito
                If (oldValue = value) Then Exit Property
                Me.m_Esito = value
                Me.DoChanged("Esito", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che specifica l'esito della conversazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DettaglioEsito As String
            Get
                Return Me.m_DettaglioEsito
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_DettaglioEsito
                If (oldValue = value) Then Exit Property
                Me.m_DettaglioEsito = value
                Me.DoChanged("DettaglioEsito", value, oldValue)
            End Set
        End Property



        Public Overridable ReadOnly Property DescrizioneAttivita As String
            Get
                Return "Contatto utente"
            End Get
        End Property

        ''' <summary>
        ''' Restituisce un oggetto CCollection di CAzioneProposta contenente le azioni proposte
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable ReadOnly Property AzioniProposte As CCollection(Of CAzioneProposta)
            Get
                Dim ret As New CCollection
                Return ret
            End Get
        End Property

        Private Function CompareTo1(ByVal obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Public Function CompareTo(ByVal obj As CContattoUtente) As Integer Implements IComparable(Of CContattoUtente).CompareTo
            If Me.Data < obj.Data Then Return -1
            If Me.Data > obj.Data Then Return 1
            Return Strings.Compare(Me.NomePersona, obj.NomePersona, CompareMethod.Text)
        End Function



        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("IDPersona", Me.IDPersona)
            writer.WriteAttribute("NomePersona", Me.m_NomePersona)
            writer.WriteAttribute("IDContesto", Me.IDContesto)
            writer.WriteAttribute("Contesto", Me.m_Contesto)
            writer.WriteAttribute("Ricevuta", Me.m_Ricevuta)
            writer.WriteAttribute("Scopo", Me.m_Scopo)
            writer.WriteAttribute("Data", Me.m_Data)
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("NomeOperatore", Me.m_NomeOperatore)
            writer.WriteAttribute("Esito", Me.m_Esito)
            writer.WriteAttribute("DettaglioEsito", Me.m_DettaglioEsito)
            writer.WriteAttribute("Durata", Me.m_Durata)
            writer.WriteAttribute("MessageID", Me.m_MessageID)
            writer.WriteAttribute("NumeroOIndirizzo", Me.m_NumeroOIndirizzo)
            writer.WriteAttribute("NomeIndirizzo", Me.m_NomeIndirizzo)
            writer.WriteAttribute("IDAzienda", Me.IDAzienda)
            writer.WriteAttribute("Attesa", Me.m_Attesa)
            writer.WriteAttribute("StatoConversazione", Me.m_StatoConversazione)
            writer.WriteAttribute("OldStatoConversazione", Me.m_OldStatoConversazione)
            writer.WriteAttribute("IDAccoltoDa", Me.IDAccoltoDa)
            writer.WriteAttribute("NomeAccoltoDa", Me.m_NomeAccoltoDa)
            writer.WriteAttribute("DataRicezione", Me.m_DataRicezione)
            writer.WriteAttribute("AttachmentID", Me.AttachmentID)
            writer.WriteAttribute("IDPerContoDi", Me.IDPerContoDi)
            writer.WriteAttribute("NomePerContoDi", Me.m_NomePerContoDi)
            writer.WriteAttribute("Costo", Me.m_Costo)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Note", Me.m_Note)
            writer.WriteTag("Options", Me.Options)
        End Sub


        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "IDPersona" : Me.m_IDPersona = XML.Utils.Serializer.DeserializeNumber(fieldValue)
                Case "NomePersona" : Me.m_NomePersona = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDContesto" : Me.m_IDContesto = XML.Utils.Serializer.DeserializeNumber(fieldValue)
                Case "Contesto" : Me.m_Contesto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Ricevuta" : Me.m_Ricevuta = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Scopo" : Me.m_Scopo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Data" : Me.m_Data = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDOperatore" : Me.m_IDOperatore = XML.Utils.Serializer.DeserializeNumber(fieldValue)
                Case "NomeOperatore" : Me.m_NomeOperatore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Note" : Me.m_Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Esito" : Me.m_Esito = XML.Utils.Serializer.DeserializeNumber(fieldValue)
                Case "DettaglioEsito" : Me.m_DettaglioEsito = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Durata" : Me.m_Durata = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "MessageID" : Me.m_MessageID = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NumeroOIndirizzo" : Me.m_NumeroOIndirizzo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeIndirizzo" : Me.m_NomeIndirizzo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDAzienda" : Me.m_IDAzienda = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Attesa" : Me.m_Attesa = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "StatoConversazione" : Me.m_StatoConversazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "OldStatoConversazione" : Me.m_OldStatoConversazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "AttachmentID" : Me.m_AttachmentID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Costo" : Me.m_Costo = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Options"
                    If (TypeOf (fieldValue) Is CSettings) Then
                        Me.m_Options = fieldValue
                    Else
                        Me.m_Options = New CSettings
                    End If
                Case "IDAccoltoDa" : Me.m_IDAccoltoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAccoltoDa" : Me.m_NomeAccoltoDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataRicezione" : Me.m_DataRicezione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDPerContoDi" : Me.m_IDPerContoDi = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePerContoDi" : Me.m_NomePerContoDi = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function GetTableName() As String
            Return "tbl_Telefonate"
        End Function

        Public MustOverride Function GetNomeTipoOggetto() As String

        Protected Overridable Sub OnConcluso(ByVal e As ContattoEventArgs)
            CustomerCalls.CRM.onNuovoContatto(e)
            CustomerCalls.CRM.Module.DispatchEvent(New EventDescription("Create", Me.GetNomeTipoOggetto & " " & CStr(IIf(Me.Ricevuta, "ricevuta", "effettuata")) & vbCrLf & "Cliente: " & Me.NomePersona & vbCrLf & "Scopo: " & Me.Scopo, Me))
        End Sub

        Protected Overrides Sub OnCreate(e As SystemEvent)
            MyBase.OnCreate(e)
        End Sub

        Protected Overrides Sub OnModified(e As SystemEvent)
            CustomerCalls.CRM.onContattoModificato(New ContattoEventArgs(Me))
            CustomerCalls.CRM.Module.DispatchEvent(New EventDescription("Edit", "Modifica alla " & Me.GetNomeTipoOggetto & " " & CStr(IIf(Me.Ricevuta, "ricevuta", "effettuata")) & vbCrLf & "Cliente: " & Me.NomePersona & vbCrLf & "Scopo: " & Me.Scopo, Me))
            MyBase.OnModified(e)
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            CustomerCalls.CRM.onContattoEliminato(New ContattoEventArgs(Me))
            CustomerCalls.CRM.Module.DispatchEvent(New EventDescription("Delete", Me.GetNomeTipoOggetto & " " & CStr(IIf(Me.Ricevuta, "ricevuta", "effettuata")) & vbCrLf & "Cliente: " & Me.NomePersona & vbCrLf & "Scopo: " & Me.Scopo, Me))
            MyBase.OnDelete(e)
        End Sub

        'Protected Overridable Sub HandleSave(ByVal isNew As Boolean)
        '    With Me
        '        If (isNew) Then
        '            Events.DispatchEvent(CustomerCalls.CRM.Module, "Create", Me.GetNomeTipoOggetto & " " & CStr(IIf(.Ricevuta, "ricevuta", "effettuata")) & vbCrLf & "Cliente: " & .NomePersona & vbCrLf & "Scopo: " & .Scopo, Me)
        '        Else
        '            Events.DispatchEvent(CustomerCalls.CRM.Module, "Edit", "Modifica alla " & Me.GetNomeTipoOggetto & " " & CStr(IIf(.Ricevuta, "ricevuta", "effettuata")) & vbCrLf & "Cliente: " & .NomePersona & vbCrLf & "Scopo: " & .Scopo, Me)
        '        End If
        '    End With
        'End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return CRM.TelDB
        End Function

        Public Overrides Function IsChanged() As Boolean
            If (MyBase.IsChanged() OrElse Options.IsChanged) Then Return True
            If (Me.m_Trasferimenti IsNot Nothing) Then
                For Each t As TrasferimentoContatto In Me.m_Trasferimenti
                    If t.IsChanged Then Return True
                Next
            End If
            Return False
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            Dim isNew As Boolean = (Me.m_OldStatoConversazione <> CustomerCalls.StatoConversazione.CONCLUSO And Me.m_StatoConversazione = CustomerCalls.StatoConversazione.CONCLUSO)
            Me.m_OldStatoConversazione = Me.m_StatoConversazione


            If (Me.m_Trasferimenti IsNot Nothing) Then
                Dim tmp As String = XML.Utils.Serializer.Serialize(Me.m_Trasferimenti)
                Me.Options.SetValueString("Trasferimenti", tmp)
            End If

            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)

            If (Strings.Trim(Sistema.ApplicationContext.Settings.GetItemByKey("CRM.SaveNotesToDB")) <> "1") Then
                System.IO.File.WriteAllText(Sistema.ApplicationContext.SystemDataFolder & "\telefonate\note" & RPC.FormatID(GetID(Me)) & ".dat", Me.m_Note)
                System.IO.File.WriteAllText(Sistema.ApplicationContext.SystemDataFolder & "\telefonate\opt" & RPC.FormatID(GetID(Me)) & ".dat", XML.Utils.Serializer.Serialize(Me.Options))
            End If

            If (ret) Then
                Me.Options.SetChanged(False)
                If (Me.m_Trasferimenti IsNot Nothing) Then
                    For Each t As TrasferimentoContatto In Me.m_Trasferimenti
                        t.SetChanged(False)
                    Next
                End If
            End If



            If (isNew AndAlso Me.Stato = ObjectStatus.OBJECT_VALID) Then Me.OnConcluso(New ContattoEventArgs(Me))

            'CRM.TelDB.ExecuteCommand("DELETE * FROM [tbl_TelefonateQuick] WHERE ID=" & GetID(Me))
            'If (Me.Stato = ObjectStatus.OBJECT_VALID AndAlso Year(Me.Data) >= Year(Now)) Then
            '    Dim table As CDBTable = CRM.TelDB.Tables("tbl_TelefonateQuick")
            '    Dim fieldPart As String = ""
            '    For Each f As CDBEntityField In table.Fields
            '        If (fieldPart <> "") Then fieldPart &= ","
            '        fieldPart &= "[" & f.Name & "]"
            '    Next

            '    CustomerCalls.CRM.TelDB.ExecuteCommand("INSERT INTO [tbl_TelefonateQuick] (" & fieldPart & ") SELECT * FROM [tbl_Telefonate] WHERE [ID]=" & GetID(Me))
            'End If

            Return ret
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)

            If (Me.Stato = ObjectStatus.OBJECT_VALID) Then
                CustomerCalls.CRM.Index.Index(Me)
            Else
                CustomerCalls.CRM.Index.Unindex(Me)
            End If
        End Sub

        Public Overrides Sub Save(Optional force As Boolean = False)
            MyBase.Save(force)
        End Sub

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_IDPersona = reader.Read("IDPersona", Me.m_IDPersona)
            Me.m_NomePersona = reader.Read("NomePersona", Me.m_NomePersona)
            Me.m_IDContesto = reader.Read("IDContesto", Me.m_IDContesto)
            Me.m_Contesto = reader.Read("TipoContesto", Me.m_Contesto)
            Me.m_Ricevuta = reader.Read("Ricevuta", Me.m_Ricevuta)
            Me.m_Scopo = reader.Read("Scopo", Me.m_Scopo)
            Me.m_Data = reader.Read("Data", Me.m_Data)
            Me.m_IDOperatore = reader.Read("IDOperatore", Me.m_IDOperatore)
            Me.m_NomeOperatore = reader.Read("NomeOperatore", Me.m_NomeOperatore)
            Me.m_Esito = reader.Read("Esito", Me.m_Esito)
            Me.m_DettaglioEsito = reader.Read("DettaglioEsito", Me.m_DettaglioEsito)
            Me.m_Durata = reader.Read("Durata", Me.m_Durata)
            Me.m_MessageID = reader.Read("MessageID", Me.m_MessageID)
            Me.m_NumeroOIndirizzo = reader.Read("Numero", Me.m_NumeroOIndirizzo)
            Me.m_NomeIndirizzo = reader.Read("Indirizzo_Nome", Me.m_NomeIndirizzo)
            Me.m_IDAzienda = reader.Read("IDAzienda", Me.m_IDAzienda)
            Me.m_Attesa = reader.Read("Attesa", Me.m_Attesa)
            Me.m_StatoConversazione = reader.Read("StatoConversazione", Me.m_StatoConversazione)
            Me.m_OldStatoConversazione = Me.m_StatoConversazione
            Me.m_IDAccoltoDa = reader.Read("IDAccoltoDa", Me.m_IDAccoltoDa)
            Me.m_NomeAccoltoDa = reader.Read("NomeAccoltoDa", Me.m_NomeAccoltoDa)
            Me.m_DataRicezione = reader.Read("DataRicezione", Me.m_DataRicezione)
            Me.m_AttachmentID = reader.Read("IDAttachment", Me.m_AttachmentID)
            Me.m_IDPerContoDi = reader.Read("IDPerContoDi", Me.m_IDPerContoDi)
            Me.m_NomePerContoDi = reader.Read("NomePerContoDi", Me.m_NomePerContoDi)
            Me.m_Costo = reader.Read("Costo", Me.m_Costo)


            If (Strings.Trim(Sistema.ApplicationContext.Settings.GetItemByKey("CRM.SaveNotesToDB")) = "1") Then
                Me.m_Note = reader.Read("Note", Me.m_Note)
                Dim opstr As String = ""
                opstr = reader.Read("Options", opstr)
                Try
                    If (opstr <> "") Then
                        Me.m_Options = XML.Utils.Serializer.Deserialize(opstr)
                    Else
                        Me.m_Options = Nothing
                    End If
                Catch ex As Exception
                    Me.m_Options = Nothing
                End Try

            End If

            Dim ret As Boolean = MyBase.LoadFromRecordset(reader)

            If (Strings.Trim(Sistema.ApplicationContext.Settings.GetItemByKey("CRM.SaveNotesToDB")) <> "1") Then
                Dim fName As String = Sistema.ApplicationContext.SystemDataFolder & "\telefonate\note" & RPC.FormatID(GetID(Me)) & ".dat"
                If (System.IO.File.Exists(fName)) Then Me.m_Note = System.IO.File.ReadAllText(fName)
                fName = Sistema.ApplicationContext.SystemDataFolder & "\telefonate\opt" & RPC.FormatID(GetID(Me)) & ".dat"
                Dim opstr As String = ""
                If (System.IO.File.Exists(fName)) Then opstr = System.IO.File.ReadAllText(fName)
                If (opstr <> "") Then
                    Me.m_Options = XML.Utils.Serializer.Deserialize(opstr)
                Else
                    Me.m_Options = Nothing
                End If
            End If

            Return ret
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("ClassName", TypeName(Me))
            writer.Write("Ricevuta", Me.m_Ricevuta)
            writer.Write("Scopo", Me.m_Scopo)
            writer.Write("Data", Me.m_Data)
            writer.Write("DataStr", DBUtils.ToDBDateStr(Me.m_Data))
            writer.Write("IDOperatore", Me.IDOperatore)
            writer.Write("NomeOperatore", Me.m_NomeOperatore)
            writer.Write("IDPersona", Me.IDPersona)
            writer.Write("NomePersona", Me.m_NomePersona)
            writer.Write("IDContesto", Me.IDContesto)
            writer.Write("TipoContesto", Me.m_Contesto)
            writer.Write("Esito", Me.m_Esito)
            writer.Write("DettaglioEsito", Me.m_DettaglioEsito)
            writer.Write("IDPuntoOperativo", Me.IDPuntoOperativo)
            writer.Write("Durata", Me.m_Durata)
            writer.Write("MessageID", Me.m_MessageID)
            writer.Write("Numero", Me.m_NumeroOIndirizzo)
            writer.Write("Indirizzo_Nome", Me.m_NomeIndirizzo)
            writer.Write("IDAzienda", Me.IDAzienda)
            writer.Write("Attesa", Me.m_Attesa)
            writer.Write("StatoConversazione", Me.m_StatoConversazione)
            writer.Write("IDAccoltoDa", Me.IDAccoltoDa)
            writer.Write("NomeAccoltoDa", Me.m_NomeAccoltoDa)
            writer.Write("DataRicezione", Me.m_DataRicezione)
            writer.Write("IDAttachment", Me.AttachmentID)
            writer.Write("IDPerContoDi", Me.IDPerContoDi)
            writer.Write("NomePerContoDi", Me.m_NomePerContoDi)
            writer.Write("Costo", Me.m_Costo)


            If (Strings.Trim(Sistema.ApplicationContext.Settings.GetItemByKey("CRM.SaveNotesToDB")) = "1") Then
                writer.Write("Note", Me.m_Note)
                writer.Write("Options", XML.Utils.Serializer.Serialize(Me.Options))
            Else
                writer.Write("Note", "")
                writer.Write("Options", "")
            End If

            Return MyBase.SaveToRecordset(writer)
        End Function



        Public Overrides Function ToString() As String
            Return Me.DescrizioneAttivita
        End Function

        Public Function GetIndexedWords() As String() Implements IIndexable.GetIndexedWords
            Dim ret As New System.Collections.ArrayList
            Dim str As String = Replace(Me.NomePersona & " " & Me.Scopo & " " & Me.NumeroOIndirizzo & " " & Me.Note, vbCrLf, " ")
            str = Replace(str, vbCr, " ")
            str = Replace(str, vbLf, " ")
            str = Replace(str, "  ", " ")

            Dim a() As String = Split(str, " ")
            If (Arrays.Len(a) > 0) Then ret.AddRange(a)

            Return ret.ToArray(GetType(String))
        End Function

        Public Function GetKeyWords() As String() Implements IIndexable.GetKeyWords
            Dim ret As New System.Collections.ArrayList
            ret.Add(Me.NomePersona)

            'Dim str As String = Replace(Me.Scopo, vbCrLf, " ")
            'str = Replace(str, vbCr, " ")
            'str = Replace(str, vbLf, " ")
            'str = Replace(str, "  ", " ")

            'Dim a() As String = Split(str, " ")
            'If (Arrays.Len(a) > 0) Then ret.AddRange(a)

            'str = Replace(Me.NumeroOIndirizzo, vbCrLf, " ")
            'str = Replace(str, vbCr, " ")
            'str = Replace(str, vbLf, " ")
            'str = Replace(str, "  ", " ")

            'a = Split(str, " ")
            'If (Arrays.Len(a) > 0) Then ret.AddRange(a)

            Return ret.ToArray(GetType(String))
        End Function
    End Class



End Class