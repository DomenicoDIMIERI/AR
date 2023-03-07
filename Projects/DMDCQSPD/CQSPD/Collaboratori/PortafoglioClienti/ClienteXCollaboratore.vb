Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Public Class CQSPD

    Public Enum StatoClienteCollaboratore As Integer
        NONATTIVO = 0
        ATTIVO = 10
        CONTATTO = 20
        RICHIESTACARICAMENTO = 25
        CARICATO = 30
        LIQUIDATO = 40
        ANNULLATO = 50
    End Enum

    <Flags>
    Public Enum ClienteCollaboratoreFlags As Integer
        ''' <summary>
        ''' Nessun flags
        ''' </summary>
        None = 0

        ''' <summary>
        ''' Si tratta di un cliente assegnato dall'agenzia
        ''' </summary>
        AssegnatoDaAgenzia = 1
    End Enum

    <Serializable>
    Public Class ClienteXCollaboratore
        Inherits DBObject
        Implements IComparable

        Private m_IDPersona As Integer
        Private m_Persona As CPersonaFisica

        Private m_Nome As String
        Private m_Cognome As String
        Private m_CodiceFiscale As String
        Private m_DataNascita As DateTime?
        Private m_Indirizzo As CIndirizzo

        Private m_IDCollaboratore As Integer
        Private m_Collaboratore As CCollaboratore

        Private m_DataAcquisizione As DateTime?
        Private m_TipoFonte As String
        Private m_IDFonte As Integer
        Private m_Fonte As IFonte
        Private m_NomeFonte As String

        Private m_Flags As ClienteCollaboratoreFlags
        Private m_Parameters As CKeyCollection

        Private m_StatoLavorazione As StatoClienteCollaboratore
        Private m_DettaglioStatoLavorazione As String

        Private m_NomeAmministrazione As String
        Private m_TelefonoCasa As String
        Private m_TelefonoUfficio As String
        Private m_TelefonoCellulare As String
        Private m_Fax As String
        Private m_AltroTelefono As String
        Private m_eMailPersonale As String
        Private m_eMailUfficio As String
        Private m_PEC As String

        Private m_DataRinnovoCQS As DateTime?
        Private m_MotivoRicontatto As String
        Private m_DataRinnovoPD As DateTime?

        Private m_ImportoRichiesto As String
        Private m_MotivoRichiesta As String
        Private m_DataRichiesta As DateTime?

        Private m_IDConsulente As Integer
        Private m_Consulente As CConsulentePratica
        Private m_DataAssegnazione As Date?
        Private m_MotivoAssegnazione As String
        Private m_IDAssegnatoDa As Integer
        <NonSerialized> Private m_AssegnatoDa As CUser

        Public Sub New()
            Me.m_IDPersona = 0
            Me.m_Persona = Nothing
            Me.m_Nome = ""
            Me.m_Cognome = ""
            Me.m_CodiceFiscale = ""
            Me.m_DataNascita = Nothing
            Me.m_Indirizzo = New CIndirizzo("Indirizzo")
            Me.m_IDCollaboratore = 0
            Me.m_Collaboratore = Nothing
            Me.m_DataAcquisizione = Nothing
            Me.m_TipoFonte = ""
            Me.m_IDFonte = 0
            Me.m_Fonte = Nothing
            Me.m_NomeFonte = ""
            Me.m_Flags = ClienteCollaboratoreFlags.None
            Me.m_Parameters = Nothing
            Me.m_StatoLavorazione = StatoClienteCollaboratore.NONATTIVO
            Me.m_DettaglioStatoLavorazione = ""
            Me.m_NomeAmministrazione = ""
            Me.m_TelefonoCasa = ""
            Me.m_TelefonoUfficio = ""
            Me.m_TelefonoCellulare = ""
            Me.m_Fax = ""
            Me.m_AltroTelefono = ""
            Me.m_eMailPersonale = ""
            Me.m_eMailUfficio = ""
            Me.m_PEC = ""
            Me.m_DataRinnovoCQS = Nothing
            Me.m_MotivoRicontatto = ""
            Me.m_DataRinnovoPD = Nothing
            Me.m_ImportoRichiesto = ""
            Me.m_MotivoRichiesta = ""
            Me.m_DataRichiesta = Nothing
            Me.m_IDConsulente = 0
            Me.m_Consulente = Nothing
            Me.m_DataAssegnazione = Nothing
            Me.m_MotivoAssegnazione = ""
            Me.m_IDAssegnatoDa = 0
            Me.m_AssegnatoDa = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID del consulente che segue il cliente per il collaboratore
        ''' </summary>
        ''' <returns></returns>
        Public Property IDConsulente As Integer
            Get
                Return GetID(Me.m_Consulente, Me.m_IDConsulente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDConsulente
                If (oldValue = value) Then Return
                Me.m_IDConsulente = value
                Me.m_Consulente = Nothing
                Me.DoChanged("IDConsulente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il consulente che segue il cliente per il collaboratore
        ''' </summary>
        ''' <returns></returns>
        Public Property Consulente As CConsulentePratica
            Get
                If (Me.m_Consulente Is Nothing) Then Me.m_Consulente = CQSPD.Consulenti.GetItemById(Me.m_IDConsulente)
                Return Me.m_Consulente
            End Get
            Set(value As CConsulentePratica)
                Dim oldValue As CConsulentePratica = Me.Consulente
                If (oldValue Is value) Then Return
                Me.m_Consulente = value
                Me.m_IDConsulente = GetID(value)
                Me.DoChanged("Consulente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Imposta il consulente
        ''' </summary>
        ''' <param name="value"></param>
        Protected Friend Sub SetConsulente(ByVal value As CConsulentePratica)
            Me.m_Consulente = value
            Me.m_IDConsulente = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il motivo del ricontatto fissato dal collaboratore
        ''' </summary>
        ''' <returns></returns>
        Public Property MotivoRicontatto As String
            Get
                Return Me.m_MotivoRicontatto
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_MotivoRicontatto
                Me.m_MotivoRicontatto = value
                Me.DoChanged("MotivoRicontatto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di richiesta caricamento fatta dal collaboratore al gruppo dei consulenti o al teammanager
        ''' </summary>
        ''' <returns></returns>
        Public Property DataRichiesta As DateTime?
            Get
                Return Me.m_DataRichiesta
            End Get
            Set(value As DateTime?)
                Dim oldValue As DateTime? = Me.m_DataRichiesta
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataRichiesta = value
                Me.DoChanged("DataRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'importo richiesto dal cliente al collaboratore
        ''' </summary>
        ''' <returns></returns>
        Public Property ImportoRichiesto As String
            Get
                Return Me.m_ImportoRichiesto
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_ImportoRichiesto
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_ImportoRichiesto = value
                Me.DoChanged("ImportoRichiesto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il motivo della richiesta di finanziamento fatta dal cliente al collaboratore
        ''' </summary>
        ''' <returns></returns>
        Public Property MotivoRichiesta As String
            Get
                Return Me.m_MotivoRichiesta
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_MotivoRichiesta
                If (oldValue = value) Then Return
                Me.m_MotivoRichiesta = value
                Me.DoChanged("MotivoRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisec o imposta la data di ricontatto fissata dal collaboratore
        ''' </summary>
        ''' <returns></returns>
        Public Property DataRinnovoCQS As DateTime?
            Get
                Return Me.m_DataRinnovoCQS
            End Get
            Set(value As DateTime?)
                Dim oldValue As DateTime? = Me.m_DataRinnovoCQS
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataRinnovoCQS = value
                Me.DoChanged("DataRinnovoCQS", value, oldValue)
            End Set
        End Property

        Public Property DataRinnovoPD As DateTime?
            Get
                Return Me.m_DataRinnovoPD
            End Get
            Set(value As DateTime?)
                Dim oldValue As DateTime? = Me.m_DataRinnovoPD
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataRinnovoPD = value
                Me.DoChanged("DataRinnovoPD", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della persona fisica nel database principale associata al cliente gestito dal collaboratore
        ''' </summary>
        ''' <returns></returns>
        Public Property IDPersona As Integer
            Get
                Return GetID(Me.m_Persona, Me.m_IDPersona)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPersona
                If (oldValue = value) Then Return
                Me.m_IDPersona = value
                Me.m_Persona = Nothing
                Me.DoChanged("IDPersona", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la persona fisica nel database principale associata al cliente gestito dal collaboratore
        ''' </summary>
        ''' <returns></returns>
        Public Property Persona As CPersonaFisica
            Get
                If (Me.m_Persona Is Nothing) Then Me.m_Persona = Anagrafica.Persone.GetItemById(Me.m_IDPersona)
                Return Me.m_Persona
            End Get
            Set(value As CPersonaFisica)
                Dim oldValue As CPersonaFisica = Me.m_Persona
                If (oldValue Is value) Then Return
                Me.m_Persona = value
                Me.m_IDPersona = GetID(value)
                Me.DoChanged("Persona", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del cliente registrato dal collaboratore
        ''' </summary>
        ''' <returns></returns>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Nome
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il cognome del cliente registrato dal collaboratore
        ''' </summary>
        ''' <returns></returns>
        Public Property Cognome As String
            Get
                Return Me.m_Cognome
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Cognome
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_Cognome = value
                Me.DoChanged("Cognome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il codice fiscale del cliente registrato dal collaboratore
        ''' </summary>
        ''' <returns></returns>
        Public Property CodiceFiscale As String
            Get
                Return Me.m_CodiceFiscale
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_CodiceFiscale
                value = Formats.ParseCodiceFiscale(value)
                If (oldValue = value) Then Return
                Me.m_CodiceFiscale = value
                Me.DoChanged("CodiceFiscale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di nascita del cliente registrata dal collaboratore
        ''' </summary>
        ''' <returns></returns>
        Public Property DataNascita As DateTime?
            Get
                Return Me.m_DataNascita
            End Get
            Set(value As DateTime?)
                Dim oldValue As DateTime? = Me.m_DataNascita
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataNascita = value
                Me.DoChanged("DataNascita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'indirizzo del cliente registrato dal collaboratore
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Indirizzo As CIndirizzo
            Get
                Return Me.m_Indirizzo
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del collaboratore che ha in gestione il cliente
        ''' </summary>
        ''' <returns></returns>
        Public Property IDCollaboratore As Integer
            Get
                Return GetID(Me.m_Collaboratore, Me.m_IDCollaboratore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCollaboratore
                If (oldValue = value) Then Return
                Me.m_IDCollaboratore = value
                Me.m_Collaboratore = Nothing
                Me.DoChanged("IDCollaboratore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il collaboratore che ha in gestione il cliente
        ''' </summary>
        ''' <returns></returns>
        Public Property Collaboratore As CCollaboratore
            Get
                If (Me.m_Collaboratore Is Nothing) Then Me.m_Collaboratore = CQSPD.Collaboratori.GetItemById(Me.m_IDCollaboratore)
                Return Me.m_Collaboratore
            End Get
            Set(value As CCollaboratore)
                Dim oldValue As CCollaboratore = Me.Collaboratore
                If (oldValue Is value) Then Return
                Me.m_Collaboratore = value
                Me.m_IDCollaboratore = GetID(value)
                Me.DoChanged("Collaboratore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Inizializza i valori di questo oggetto con i valori dei campi corrispondenti dell'oggetto Persona associato
        ''' </summary>
        Public Sub FromPersona()
            Dim p As CPersonaFisica = Me.Persona
            If (p Is Nothing) Then Throw New ArgumentNullException("persona")
            Me.m_Nome = p.Nome
            Me.m_Cognome = p.Cognome
            Me.m_CodiceFiscale = p.CodiceFiscale
            Me.m_DataNascita = p.DataNascita
            Me.m_Indirizzo.CopyFrom(p.ResidenteA)
            Me.m_DettaglioStatoLavorazione = ""
            Me.m_NomeAmministrazione = p.ImpiegoPrincipale().NomeAzienda
            Me.m_TelefonoCasa = p.Telefono
            Me.m_TelefonoCellulare = p.Cellulare
            Me.m_Fax = p.Fax
            Me.m_eMailPersonale = p.eMail
        End Sub

        ''' <summary>
        ''' Assegna il cliente al collaboratore
        ''' </summary>
        Public Sub Assegna(ByVal motivo As String)
            Me.FromPersona()
            'If (Me.StatoLavorazione >= StatoClienteCollaboratore.CONTATTO) Then Throw New InvalidOperationException("Stato non valido")
            'Me.StatoLavorazione = StatoClienteCollaboratore.RICHIESTACARICAMENTO
            Me.MotivoAssegnazione = motivo
            Me.DataAssegnazione = DateUtils.Now
            If (Me.DataRinnovoCQS.HasValue = False) Then
                Me.DataRinnovoCQS = Me.DataAssegnazione
                Me.MotivoRicontatto = Me.MotivoAssegnazione
            End If
            Me.AssegnatoDa = Sistema.Users.CurrentUser
            Me.Flags = Sistema.SetFlag(Me.Flags, ClienteCollaboratoreFlags.AssegnatoDaAgenzia, True)
            Me.Save(True)
            Dim e As New EventDescription("assegnazione", "Assegnazione cliente", Me)
            Me.GetModule.DispatchEvent(e)
        End Sub

        ''' <summary>
        ''' Il collaboratore accetta il cliente
        ''' </summary>
        Public Sub Accetta()
            If (Me.StatoLavorazione < StatoClienteCollaboratore.CONTATTO) Then
                Me.StatoLavorazione = StatoClienteCollaboratore.CONTATTO
                Me.DataAcquisizione = DateUtils.Now
                Me.Save()
            End If
            Me.Save(True)
            Dim e As New EventDescription("accettato", "Accettazione cliente", Me)
            Me.GetModule.DispatchEvent(e)
        End Sub

        ''' <summary>
        ''' Richiede il caricamento dell'anagrafica nel database principale.
        ''' </summary>
        Public Sub RichiediCaricamento()
            If (Me.StatoLavorazione >= StatoClienteCollaboratore.RICHIESTACARICAMENTO) Then Throw New InvalidOperationException("Stato non valido")
            Me.StatoLavorazione = StatoClienteCollaboratore.RICHIESTACARICAMENTO
            Me.Save()
            Dim e As New EventDescription("richiedicaricamento", "Richiesta di caricamento", Me)
            Me.GetModule.DispatchEvent(e)
        End Sub

        ''' <summary>
        ''' Prende in carico il caricamento del cliente nel database principale
        ''' </summary>
        Public Sub PrendiInCarico()
            Dim c As CConsulentePratica = CQSPD.Consulenti.GetItemByUser(Sistema.Users.CurrentUser)
            If (c Is Nothing) Then Throw New InvalidOperationException("L'utente corrente non è un consulente")
            If (Me.Persona Is Nothing) Then Throw New ArgumentNullException("Anagrafica non associata")
            If (Me.StatoLavorazione <> StatoClienteCollaboratore.RICHIESTACARICAMENTO) Then Throw New InvalidOperationException("Stato non valido")
            Me.Consulente = c
            Me.DataAcquisizione = DateUtils.Now
            Me.StatoLavorazione = StatoClienteCollaboratore.CARICATO
            Me.Save()
            Dim e As New EventDescription("presaincarico", "Presa in carico", Me)
            Me.GetModule.DispatchEvent(e)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta la data di presa in carico del cliente
        ''' </summary>
        ''' <returns></returns>
        Public Property DataAcquisizione As DateTime?
            Get
                Return Me.m_DataAcquisizione
            End Get
            Set(value As DateTime?)
                Dim oldValue As DateTime? = Me.m_DataAcquisizione
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataAcquisizione = value
                Me.DoChanged("DataAcquisizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo della fonte registrata dal collaboratore per il cliente
        ''' </summary>
        ''' <returns></returns>
        Public Property TipoFonte As String
            Get
                Return Me.m_TipoFonte
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_TipoFonte
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_TipoFonte = value
                Me.m_Fonte = Nothing
                Me.DoChanged("TipoFonte", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della fonte registrata dal collaboratore per il cliente
        ''' </summary>
        ''' <returns></returns>
        Public Property IDFonte As Integer
            Get
                Return GetID(Me.m_Fonte, Me.m_IDFonte)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDFonte
                If (oldValue = value) Then Return
                Me.m_Fonte = Nothing
                Me.m_IDFonte = value
                Me.DoChanged("IDFonte", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la fonte registrata dal collaboratore per il cliente
        ''' </summary>
        ''' <returns></returns>
        Public Property Fonte As IFonte
            Get
                If (Me.m_Fonte Is Nothing) Then Me.m_Fonte = Anagrafica.Fonti.GetItemById(Me.m_TipoFonte, Me.m_TipoFonte, Me.m_IDFonte)
                Return Me.m_Fonte
            End Get
            Set(value As IFonte)
                Dim oldValue As IFonte = Me.Fonte
                If (oldValue Is value) Then Return
                Me.m_Fonte = value
                Me.m_IDFonte = GetID(value)
                Me.m_NomeFonte = "" : If (value IsNot Nothing) Then Me.m_NomeFonte = value.Nome
                Me.DoChanged("Fonte", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della fonte registrata dal collaboratore per il cliente
        ''' </summary>
        ''' <returns></returns>
        Public Property NomeFonte As String
            Get
                Return Me.m_NomeFonte
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeFonte
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeFonte = value
                Me.DoChanged("NomeFonte", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei flags aggiuntivi
        ''' </summary>
        ''' <returns></returns>
        Public Property Flags As ClienteCollaboratoreFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As ClienteCollaboratoreFlags)
                Dim oldValue As ClienteCollaboratoreFlags = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei parametri aggiuntivi
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Parameters As CKeyCollection
            Get
                If (Me.m_Parameters Is Nothing) Then Me.m_Parameters = New CKeyCollection
                Return Me.m_Parameters
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato di lavorazione del cliente
        ''' </summary>
        ''' <returns></returns>
        Public Property StatoLavorazione As StatoClienteCollaboratore
            Get
                Return Me.m_StatoLavorazione
            End Get
            Set(value As StatoClienteCollaboratore)
                Dim oldValue As StatoClienteCollaboratore = Me.m_StatoLavorazione
                If (oldValue = value) Then Return
                Me.m_StatoLavorazione = value
                Me.DoChanged("StatoLavorazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il dettaglio dello stato di lavorazione del cliente
        ''' </summary>
        ''' <returns></returns>
        Public Property DettaglioStatoLavorazione As String
            Get
                Return Me.m_DettaglioStatoLavorazione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_DettaglioStatoLavorazione
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_DettaglioStatoLavorazione = value
                Me.DoChanged("DettaglioStatoLavorazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'amministrazione presso cui lavora il cliente
        ''' </summary>
        ''' <returns></returns>
        Public Property NomeAmministrazione As String
            Get
                Return Me.m_NomeAmministrazione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeAmministrazione
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeAmministrazione = value
                Me.DoChanged("NomeAmministrazione", value, oldValue)
            End Set
        End Property

        Public Property TelefonoCasa As String
            Get
                Return Me.m_TelefonoCasa
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_TelefonoCasa
                value = Formats.ParsePhoneNumber(value)
                If (oldValue = value) Then Return
                Me.m_TelefonoCasa = value
                Me.DoChanged("TelefonoCasa", value, oldValue)
            End Set
        End Property

        Public Property TelefonoUfficio As String
            Get
                Return Me.m_TelefonoUfficio
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_TelefonoUfficio
                value = Formats.ParsePhoneNumber(value)
                If (oldValue = value) Then Return
                Me.m_TelefonoUfficio = value
                Me.DoChanged("TelefonoUfficio", value, oldValue)
            End Set
        End Property

        Public Property TelefonoCellulare As String
            Get
                Return Me.m_TelefonoCellulare
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_TelefonoCellulare
                value = Formats.ParsePhoneNumber(value)
                If (oldValue = value) Then Return
                Me.m_TelefonoCellulare = value
                Me.DoChanged("TelefonoCellulare", value, oldValue)
            End Set
        End Property

        Public Property Fax As String
            Get
                Return Me.m_Fax
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Fax
                value = Formats.ParsePhoneNumber(value)
                If (oldValue = value) Then Return
                Me.m_Fax = value
                Me.DoChanged("Fax", value, oldValue)
            End Set
        End Property

        Public Property AltroTelefono As String
            Get
                Return Me.m_AltroTelefono
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_AltroTelefono
                value = Formats.ParsePhoneNumber(value)
                If (oldValue = value) Then Return
                Me.m_AltroTelefono = value
                Me.DoChanged("AltroTelefono", value, oldValue)
            End Set
        End Property

        Public Property eMailPersonale As String
            Get
                Return Me.m_eMailPersonale
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_eMailPersonale
                value = Formats.ParseEMailAddress(value)
                If (oldValue = value) Then Return
                Me.m_eMailPersonale = value
                Me.DoChanged("eMailPersonale", value, oldValue)
            End Set
        End Property

        Public Property eMailUfficio As String
            Get
                Return Me.m_eMailUfficio
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_eMailUfficio
                value = Formats.ParseEMailAddress(value)
                If (oldValue = value) Then Return
                Me.m_eMailUfficio = value
                Me.DoChanged("eMailUfficio", value, oldValue)
            End Set
        End Property

        Public Property PEC As String
            Get
                Return Me.m_PEC
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_PEC
                value = Formats.ParseEMailAddress(value)
                If (oldValue = value) Then Return
                Me.m_PEC = value
                Me.DoChanged("PEC", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di assegnazione (per i clienti assegnati direttamente dall'agenzia)
        ''' </summary>
        ''' <returns></returns>
        Public Property DataAssegnazione As Date?
            Get
                Return Me.m_DataAssegnazione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataAssegnazione
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataAssegnazione = value
                Me.DoChanged("DataAssegnazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il motivo dell'assegnazione
        ''' </summary>
        ''' <returns></returns>
        Public Property MotivoAssegnazione As String
            Get
                Return Me.m_MotivoAssegnazione
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_MotivoAssegnazione
                If (oldValue = value) Then Return
                Me.m_MotivoAssegnazione = value
                Me.DoChanged("MotivoAssegnazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'operatore che ha assegnato il cliente al collaboratore
        ''' </summary>
        ''' <returns></returns>
        Public Property IDAssegnatoDa As Integer
            Get
                Return GetID(Me.m_AssegnatoDa, Me.m_IDAssegnatoDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAssegnatoDa
                If (oldValue = value) Then Return
                Me.m_IDAssegnatoDa = value
                Me.m_AssegnatoDa = Nothing
                Me.DoChanged("IDAssegnatoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'operatore che ha assegnato il cliente al collaboratore
        ''' </summary>
        ''' <returns></returns>
        Public Property AssegnatoDa As CUser
            Get
                If (Me.m_AssegnatoDa Is Nothing) Then Me.m_AssegnatoDa = Sistema.Users.GetItemById(Me.m_IDAssegnatoDa)
                Return Me.m_AssegnatoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.AssegnatoDa
                If (oldValue Is value) Then Return
                Me.m_AssegnatoDa = value
                Me.m_IDAssegnatoDa = GetID(value)
                Me.DoChanged("AssegnatoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il nome ed il cognome del cliente
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Nominativo As String
            Get
                Return Strings.Combine(Me.m_Nome, Me.m_Cognome, " ")
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return Me.Nominativo
        End Function

        Public Overrides Function IsChanged() As Boolean
            Return MyBase.IsChanged() OrElse Me.m_Indirizzo.IsChanged
        End Function


        Public Overrides Function GetModule() As CModule
            Return CQSPD.Collaboratori.ClientiXCollaboratori.Module
        End Function

        Public Overrides Sub Save(Optional force As Boolean = False)
            MyBase.Save(force)
            Me.m_Indirizzo.SetChanged(False)
        End Sub


        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDCliXCollab"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_IDPersona = reader.Read("IDPersona", Me.m_IDPersona)
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Cognome = reader.Read("Cognome", Me.m_Cognome)
            Me.m_CodiceFiscale = reader.Read("CodiceFiscale", Me.m_CodiceFiscale)
            Me.m_IDCollaboratore = reader.Read("IDCollaboratore", Me.m_IDCollaboratore)
            Me.m_DataNascita = reader.Read("DataNascita", Me.m_DataNascita)
            Me.m_Indirizzo.Provincia = reader.Read("Indirizzo_Provincia", Me.m_Indirizzo.Provincia)
            Me.m_Indirizzo.Citta = reader.Read("Indirizzo_Citta", Me.m_Indirizzo.Citta)
            Me.m_Indirizzo.CAP = reader.Read("Indirizzo_CAP", Me.m_Indirizzo.CAP)
            Me.m_Indirizzo.ToponimoViaECivico = reader.Read("Indirizzo_Via", Me.m_Indirizzo.ToponimoViaECivico)
            Me.m_Indirizzo.SetChanged(False)
            Me.m_DataAcquisizione = reader.Read("DataAcquisizione", Me.m_DataAcquisizione)
            Me.m_TipoFonte = reader.Read("TipoFonte", Me.m_TipoFonte)
            Me.m_IDFonte = reader.Read("IDFonte", Me.m_IDFonte)
            Me.m_NomeFonte = reader.Read("NomeFonte", Me.m_NomeFonte)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_StatoLavorazione = reader.Read("StatoLavorazione", Me.m_StatoLavorazione)
            Me.m_DettaglioStatoLavorazione = reader.Read("DettaglioStatoLavorazione", Me.m_DettaglioStatoLavorazione)
            Me.m_NomeAmministrazione = reader.Read("NomeAmministrazione", Me.m_NomeAmministrazione)
            Me.m_TelefonoCasa = reader.Read("TelefonoCasa", Me.m_TelefonoCasa)
            Me.m_TelefonoUfficio = reader.Read("TelefonoUfficio", Me.m_TelefonoUfficio)
            Me.m_TelefonoCellulare = reader.Read("TelefonoCellulare", Me.m_TelefonoCellulare)
            Me.m_Fax = reader.Read("Fax", Me.m_Fax)
            Me.m_AltroTelefono = reader.Read("AltroTelefono", Me.m_AltroTelefono)
            Me.m_eMailPersonale = reader.Read("eMailPersonale", Me.m_eMailPersonale)
            Me.m_eMailUfficio = reader.Read("eMailUfficio", Me.m_eMailUfficio)
            Me.m_PEC = reader.Read("PEC", Me.m_PEC)
            Me.m_DataRinnovoCQS = reader.Read("DataRinnovoCQS", Me.m_DataRinnovoCQS)
            Me.m_DataRinnovoPD = reader.Read("DataRinnovoPD", Me.m_DataRinnovoPD)
            Me.m_ImportoRichiesto = reader.Read("ImportoRichiesto", Me.m_ImportoRichiesto)
            Me.m_MotivoRichiesta = reader.Read("MotivoRichiesta", Me.m_MotivoRichiesta)
            Me.m_DataRichiesta = reader.Read("DataRichiesta", Me.m_DataRichiesta)
            Me.m_MotivoRicontatto = reader.Read("MotivoRicontatto", Me.m_MotivoRicontatto)
            Me.m_IDConsulente = reader.Read("IDConsulente", Me.m_IDConsulente)
            Me.m_DataAssegnazione = reader.Read("DataAssegnazione", Me.m_DataAssegnazione)
            Me.m_MotivoAssegnazione = reader.Read("MotivoAssegnazione", Me.m_MotivoAssegnazione)
            Me.m_IDAssegnatoDa = reader.Read("IDAssegnatoDa", Me.m_IDAssegnatoDa)

            Dim tmp As String = ""
            tmp = reader.Read("Parameters", tmp)
            If (tmp <> "") Then Me.m_Parameters = CType(XML.Utils.Serializer.Deserialize(tmp), CKeyCollection)

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("IDPersona", Me.IDPersona)
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Cognome", Me.m_Cognome)
            writer.Write("CodiceFiscale", Me.m_CodiceFiscale)
            writer.Write("DataNascita", Me.m_DataNascita)
            writer.Write("IDCollaboratore", Me.IDCollaboratore)
            writer.Write("Indirizzo_Provincia", Me.m_Indirizzo.Provincia)
            writer.Write("Indirizzo_Citta", Me.m_Indirizzo.Citta)
            writer.Write("Indirizzo_CAP", Me.m_Indirizzo.CAP)
            writer.Write("Indirizzo_Via", Me.m_Indirizzo.ToponimoViaECivico)
            writer.Write("DataAcquisizione", Me.m_DataAcquisizione)
            writer.Write("TipoFonte", Me.m_TipoFonte)
            writer.Write("IDFonte", Me.IDFonte)
            writer.Write("NomeFonte", Me.m_NomeFonte)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("StatoLavorazione", Me.m_StatoLavorazione)
            writer.Write("DettaglioStatoLavorazione", Me.m_DettaglioStatoLavorazione)
            writer.Write("NomeAmministrazione", Me.m_NomeAmministrazione)
            writer.Write("TelefonoCasa", Me.m_TelefonoCasa)
            writer.Write("TelefonoUfficio", Me.m_TelefonoUfficio)
            writer.Write("TelefonoCellulare", Me.m_TelefonoCellulare)
            writer.Write("Fax", Me.m_Fax)
            writer.Write("AltroTelefono", Me.m_AltroTelefono)
            writer.Write("eMailPersonale", Me.m_eMailPersonale)
            writer.Write("eMailUfficio", Me.m_eMailUfficio)
            writer.Write("PEC", Me.m_PEC)
            writer.Write("DataRinnovoCQS", Me.m_DataRinnovoCQS)
            writer.Write("DataRinnovoPD", Me.m_DataRinnovoPD)
            writer.Write("Parameters", XML.Utils.Serializer.Serialize(Me.Parameters))
            writer.Write("ImportoRichiesto", Me.m_ImportoRichiesto)
            writer.Write("MotivoRichiesta", Me.m_MotivoRichiesta)
            writer.Write("DataRichiesta", Me.m_DataRichiesta)
            writer.Write("MotivoRicontatto", Me.m_MotivoRicontatto)
            writer.Write("IDConsulente", Me.IDConsulente)
            writer.Write("DataAssegnazione", Me.m_DataAssegnazione)
            writer.Write("MotivoAssegnazione", Me.m_MotivoAssegnazione)
            writer.Write("IDAssegnatoDa", Me.IDAssegnatoDa)
            Return MyBase.SaveToRecordset(writer)
        End Function


        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDPersona", Me.IDPersona)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Cognome", Me.m_Cognome)
            writer.WriteAttribute("CodiceFiscale", Me.m_CodiceFiscale)
            writer.WriteAttribute("DataNascita", Me.m_DataNascita)
            writer.WriteAttribute("IDCollaboratore", Me.IDCollaboratore)
            writer.WriteAttribute("DataAcquisizione", Me.m_DataAcquisizione)
            writer.WriteAttribute("TipoFonte", Me.m_TipoFonte)
            writer.WriteAttribute("IDFonte", Me.IDFonte)
            writer.WriteAttribute("NomeFonte", Me.m_NomeFonte)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("StatoLavorazione", Me.m_StatoLavorazione)
            writer.WriteAttribute("DettaglioStatoLavorazione", Me.m_DettaglioStatoLavorazione)
            writer.WriteAttribute("NomeAmministrazione", Me.m_NomeAmministrazione)
            writer.WriteAttribute("TelefonoCasa", Me.m_TelefonoCasa)
            writer.WriteAttribute("TelefonoUfficio", Me.m_TelefonoUfficio)
            writer.WriteAttribute("TelefonoCellulare", Me.m_TelefonoCellulare)
            writer.WriteAttribute("Fax", Me.m_Fax)
            writer.WriteAttribute("AltroTelefono", Me.m_AltroTelefono)
            writer.WriteAttribute("eMailPersonale", Me.m_eMailPersonale)
            writer.WriteAttribute("eMailUfficio", Me.m_eMailUfficio)
            writer.WriteAttribute("PEC", Me.m_PEC)
            writer.WriteAttribute("DataRinnovoCQS", Me.m_DataRinnovoCQS)
            writer.WriteAttribute("DataRinnovoPD", Me.m_DataRinnovoPD)
            writer.WriteAttribute("ImportoRichiesto", Me.m_ImportoRichiesto)
            writer.WriteAttribute("MotivoRichiesta", Me.m_MotivoRichiesta)
            writer.WriteAttribute("DataRichiesta", Me.m_DataRichiesta)
            writer.WriteAttribute("MotivoRicontatto", Me.m_MotivoRicontatto)
            writer.WriteAttribute("IDConsulente", Me.IDConsulente)
            writer.WriteAttribute("DataAssegnazione", Me.m_DataAssegnazione)
            writer.WriteAttribute("MotivoAssegnazione", Me.m_MotivoAssegnazione)
            writer.WriteAttribute("IDAssegnatoDa", Me.IDAssegnatoDa)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Indirizzo", Me.m_Indirizzo)
            writer.WriteTag("Parameters", Me.m_Parameters)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDPersona" : Me.m_IDPersona = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Cognome" : Me.m_Cognome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CodiceFiscale" : Me.m_CodiceFiscale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataNascita" : Me.m_DataNascita = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDCollaboratore" : Me.m_IDCollaboratore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataAcquisizione" : Me.m_DataAcquisizione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "TipoFonte" : Me.m_TipoFonte = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDFonte" : Me.m_IDFonte = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeFonte" : Me.m_NomeFonte = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "StatoLavorazione" : Me.m_StatoLavorazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DettaglioStatoLavorazione" : Me.m_DettaglioStatoLavorazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Indirizzo" : Me.m_Indirizzo = CType(XML.Utils.Serializer.ToObject(fieldValue), CIndirizzo)
                Case "Parameters" : Me.m_Parameters = CType(XML.Utils.Serializer.ToObject(fieldValue), CKeyCollection)
                Case "NomeAmministrazione" : Me.m_NomeAmministrazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TelefonoCasa" : Me.m_TelefonoCasa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TelefonoUfficio" : Me.m_TelefonoUfficio = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TelefonoCellulare" : Me.m_TelefonoCellulare = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Fax" : Me.m_Fax = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "AltroTelefono" : Me.m_AltroTelefono = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "eMailPersonale" : Me.m_eMailPersonale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "eMailUfficio" : Me.m_eMailUfficio = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PEC" : Me.m_PEC = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataRinnovoCQS" : Me.m_DataRinnovoCQS = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataRinnovoPD" : Me.m_DataRinnovoPD = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "MotivoRicontatto" : Me.m_MotivoRicontatto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ImportoRichiesto" : Me.m_ImportoRichiesto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MotivoRichiesta" : Me.m_MotivoRichiesta = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataRichiesta" : Me.m_DataRichiesta = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDConsulente" : Me.m_IDConsulente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataAssegnazione" : Me.m_DataAssegnazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDAssegnatoDa" : Me.m_IDAssegnatoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "MotivoAssegnazione" : Me.m_MotivoAssegnazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Function CompareTo(obj As ClienteXCollaboratore) As Integer
            Return Strings.Compare(Me.Nominativo, obj.Nominativo)
        End Function

        Private Function _CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(CType(obj, ClienteXCollaboratore))
        End Function
    End Class

End Class