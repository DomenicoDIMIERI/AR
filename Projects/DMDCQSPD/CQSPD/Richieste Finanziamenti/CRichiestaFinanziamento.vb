Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    Public Enum StatoRichiestaFinanziamento As Integer
        INSERITA = 0
        EVASA = 1
    End Enum

    ''' <summary>
    ''' Valore che indica il tipo di specifica sull'importo della richiesta di finanziamento
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum TipoRichiestaFinanziamento As Integer
        ''' <summary>
        ''' Il cliente ha chiesto di avere il massimo possibile 
        ''' </summary>
        ''' <remarks></remarks>
        MASSIMO_POSSIBILE = 0

        ''' <summary>
        ''' Il cliente ha in mente un valore preciso pari a ImportoRichiesto
        ''' </summary>
        ''' <remarks></remarks>
        UGUALEA = 1

        ''' <summary>
        ''' Al cliente servono almeno ImportoRichiesto 
        ''' </summary>
        ''' <remarks></remarks>
        ALMENO = 2

        ''' <summary>
        ''' Il cliente vuole totalizzare una somma compresa tra ImportoRichiesto e ImportoRichiesto1
        ''' </summary>
        ''' <remarks></remarks>
        TRA = 3

        ''' <summary>
        ''' Il cliente non ha specificato alcun valore per la richiesta
        ''' </summary>
        ''' <remarks></remarks>
        NONSPECIFICATO = 4

        ''' <summary>
        ''' Indica un consolidamento prestiti.
        ''' 2060315 DMD Valore richiesto da K COPPOLA su richiesta di Carrano.
        '''         Io ritengo che sia sbagliato inserire qui questo campo poiché non indica il tipo di richiesta ma il motivo della richiesta 
        '''         che andrebbe specificato nelle note o al massimo aggiungendo un ulteriore campo.
        '''         Per evitare di iniziare un'altra discussione inutile in cui Carrano vuole per forza avere ragione solo perché è il capo procedo con la sua richiesta anche se la ritengo errata.
        ''' </summary>
        ''' <remarks></remarks>
        CONSOLIDAMENTO = 5
    End Enum

    ''' <summary>
    ''' Flags che specificano alcune proprietà della richiesta di finanziamento
    ''' </summary>
    ''' <remarks></remarks>
    <Flags> _
    Public Enum RichiestaFinanziamentoFlags As Integer
        None = 0

        ''' <summary>
        ''' Il cliente ha effettuato la stessa richiesta presso un'altra finanziaria
        ''' </summary>
        ''' <remarks></remarks>
        ClienteHaChiestoAdAltri = 1

        ''' <summary>
        ''' L'operatore ha verificato l'eventuale presenza di altre richieste
        ''' </summary>
        ''' <remarks></remarks>
        VerificatoAltreRichieste = 2

        ''' <summary>
        ''' Il cliente ha effettuato una richiesta di conteggio estintivo
        ''' </summary>
        ''' <remarks></remarks>
        HaRichieste = 4

        ''' <summary>
        ''' Un amministratore ha soppresso i messaggi generati da questa richiesta
        ''' </summary>
        ''' <remarks></remarks>
        Soppressa = 8
    End Enum

    
    <Serializable> _
    Public Class CRichiestaFinanziamento
        Inherits DBObjectPO
        Implements IComparable

        Private m_Data As Date
        Private m_TipoFonte As String
        Private m_IDFonte As Integer
        Private m_Fonte As IFonte
        Private m_NomeFonte As String
        Private m_Referrer As String
        Private m_ImportoRichiesto As Nullable(Of Decimal)
        Private m_ImportoRichiesto1 As Nullable(Of Decimal)
        Private m_RataMassima As Nullable(Of Decimal)
        Private m_DurataMassima As Nullable(Of Integer)
        Private m_IDCliente As Integer
        Private m_Cliente As CPersonaFisica
        Private m_NomeCliente As String
        Private m_Note As String
        Private m_IDAssegnatoA As Integer
        Private m_AssegnatoA As CUser
        Private m_NomeAssegnatoA As String
        Private m_IDPresaInCaricoDa As Integer
        Private m_PresaInCaricoDa As CUser
        Private m_NomePresaInCarocoDa As String
        Private m_IDFonteStr As String
        Private m_IDCampagnaStr As String
        Private m_IDAnnuncioStr As String
        Private m_IDKeyWordStr As String
        Private m_IDPratica As Integer
        Private m_Pratica As CRapportino
        Private m_Canale As CCanale
        Private m_IDCanale As Integer
        Private m_NomeCanale As String

        Private m_Canale1 As CCanale
        Private m_IDCanale1 As Integer
        Private m_NomeCanale1 As String

        Private m_StatoRichiesta As StatoRichiestaFinanziamento
        Private m_TipoRichiesta As TipoRichiestaFinanziamento
        Private m_IDContesto As Integer
        Private m_TipoContesto As String
        Private m_Durata As Double
        Private m_IDPrivacy As Integer
        Private m_Privacy As CAttachment
        Private m_IDModulo As Integer
        Private m_Modulo As CAttachment
        Private m_Flags As RichiestaFinanziamentoFlags
        Private m_Conteggi As CRichiesteConteggiXRichiesta
        Private m_AltriPreventivi As CAltriPreventiviXRichiesta

        Private m_IDFinestraLavorazione As Integer
        Private m_FinestraLavorazione As FinestraLavorazione


        Private m_Scopo As String

        Private m_IDCollaboratore As Integer
        Private m_Collaboratore As CCollaboratore
        Private m_NomeCollaboratore As String

        Public Sub New()
            Me.m_Data = Now()
            Me.m_IDFonte = 0
            Me.m_Fonte = Nothing
            Me.m_NomeFonte = vbNullString
            Me.m_Referrer = vbNullString
            Me.m_ImportoRichiesto = Nothing
            Me.m_RataMassima = Nothing
            Me.m_DurataMassima = Nothing
            Me.m_IDCliente = 0
            Me.m_Cliente = Nothing
            Me.m_NomeCliente = vbNullString
            Me.m_Note = vbNullString
            Me.m_IDAssegnatoA = 0
            Me.m_AssegnatoA = Nothing
            Me.m_NomeAssegnatoA = vbNullString
            Me.m_IDPresaInCaricoDa = 0
            Me.m_PresaInCaricoDa = Nothing
            Me.m_NomePresaInCarocoDa = vbNullString
            Me.m_IDFonteStr = vbNullString
            Me.m_IDCampagnaStr = vbNullString
            Me.m_IDAnnuncioStr = vbNullString
            Me.m_IDKeyWordStr = vbNullString
            Me.m_IDPratica = 0
            Me.m_Pratica = Nothing

            Me.m_Canale = Nothing
            Me.m_IDCanale = 0
            Me.m_NomeCanale = vbNullString

            Me.m_Canale1 = Nothing
            Me.m_IDCanale1 = 0
            Me.m_NomeCanale1 = vbNullString

            Me.m_StatoRichiesta = StatoRichiestaFinanziamento.INSERITA
            Me.m_TipoRichiesta = TipoRichiestaFinanziamento.MASSIMO_POSSIBILE
            Me.m_ImportoRichiesto1 = Nothing
            Me.m_IDContesto = 0
            Me.m_TipoContesto = vbNullString
            Me.m_Durata = 0
            Me.m_IDPrivacy = 0
            Me.m_Privacy = Nothing
            Me.m_Flags = RichiestaFinanziamentoFlags.None
            Me.m_IDModulo = 0
            Me.m_Modulo = Nothing
            Me.m_Conteggi = Nothing
            Me.m_AltriPreventivi = Nothing

            Me.m_IDFinestraLavorazione = 0
            Me.m_FinestraLavorazione = Nothing

            Me.m_Scopo = ""

            Me.m_IDCollaboratore = 0
            Me.m_Collaboratore = Nothing
            Me.m_NomeCollaboratore = ""
        End Sub

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

        Public Property Collaboratore As CCollaboratore
            Get
                If (Me.m_Collaboratore Is Nothing) Then Me.m_Collaboratore = CQSPD.Collaboratori.GetItemById(Me.m_IDCollaboratore)
                Return Me.m_Collaboratore
            End Get
            Set(value As CCollaboratore)
                Dim oldValue As CCollaboratore = Me.Collaboratore
                If (oldValue Is value) Then Return
                Me.m_IDCollaboratore = GetID(value)
                Me.m_Collaboratore = value
                Me.m_NomeCollaboratore = "" : If (value IsNot Nothing) Then Me.m_NomeCollaboratore = value.NomePersona
                Me.DoChanged("Collaboratore", value, oldValue)
            End Set
        End Property

        Public Property NomeCollaboratore As String
            Get
                Return Me.m_NomeCollaboratore
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeCollaboratore
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeCollaboratore = value
                Me.DoChanged("NomeCollaboratore", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta l'ID della finestra di lavorazione in cui è stata effettuata la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDFinestraLavorazione As Integer
            Get
                Return GetID(Me.m_FinestraLavorazione, Me.m_IDFinestraLavorazione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDFinestraLavorazione
                If (oldValue = value) Then Exit Property
                Me.m_IDFinestraLavorazione = value
                Me.m_FinestraLavorazione = Nothing
                Me.DoChanged("IDFinestraLavorazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la finestra di lavorazione in cui è stata effettuata la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FinestraLavorazione As FinestraLavorazione
            Get
                If (Me.m_FinestraLavorazione Is Nothing) Then Me.m_FinestraLavorazione = CQSPD.FinestreDiLavorazione.GetItemById(Me.m_IDFinestraLavorazione)
                Return Me.m_FinestraLavorazione
            End Get
            Set(value As FinestraLavorazione)
                Dim oldValue As FinestraLavorazione = Me.m_FinestraLavorazione
                If (oldValue Is value) Then Exit Property
                Me.m_FinestraLavorazione = value
                Me.m_IDFinestraLavorazione = GetID(value)
                Me.DoChanged("FinestraLavorazione", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetFinestraLavorazione(ByVal value As FinestraLavorazione)
            Me.m_FinestraLavorazione = value
            Me.m_IDFinestraLavorazione = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce la collezione dei conteggi estintivi collegati a questa richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Conteggi As CRichiesteConteggiXRichiesta
            Get
                If (Me.m_Conteggi Is Nothing) Then Me.m_Conteggi = New CRichiesteConteggiXRichiesta(Me)
                Return Me.m_Conteggi
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la collezione dei preventivi ricevuti dalla concorrenza collegati a questa richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property AltriPreventivi As CAltriPreventiviXRichiesta
            Get
                If (Me.m_AltriPreventivi Is Nothing) Then Me.m_AltriPreventivi = New CAltriPreventiviXRichiesta(Me)
                Return Me.m_AltriPreventivi
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei flags aggiuntivi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As RichiestaFinanziamentoFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As RichiestaFinanziamentoFlags)
                Dim oldValue As RichiestaFinanziamentoFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo scopo della richiesta di finanziamento (Es. Consolidamento, Liquidità, Rinnovo)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Scopo As String
            Get
                Return Me.m_Scopo
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Scopo
                If (oldValue = value) Then Exit Property
                Me.m_Scopo = value
                Me.DoChanged("Scopo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'allegato caricato come informativa sulla privacy
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPrivacy As Integer
            Get
                Return GetID(Me.m_Privacy, Me.m_IDPrivacy)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPrivacy
                If (oldValue = value) Then Exit Property
                Me.m_IDPrivacy = value
                Me.m_Privacy = Nothing
                Me.DoChanged("IDPrivacy", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'allegato Privacy
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Privacy As CAttachment
            Get
                If (Me.m_Privacy Is Nothing) Then Me.m_Privacy = Sistema.Attachments.GetItemById(Me.m_IDPrivacy)
                Return Me.m_Privacy
            End Get
            Set(value As CAttachment)
                Dim oldValue As CAttachment = Me.m_Privacy
                If (oldValue Is value) Then Exit Property
                Me.m_IDPrivacy = GetID(value)
                Me.m_Privacy = value
                Me.DoChanged("Privacy", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'allegato caricato contenente il modulo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDModulo As Integer
            Get
                Return GetID(Me.m_Modulo, Me.m_IDModulo)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDModulo
                If (oldValue = value) Then Exit Property
                Me.m_IDModulo = value
                Me.m_Modulo = Nothing
                Me.DoChanged("IDModulo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se la richiesta può generare messaggi e e-mail
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Soppressa As Boolean
            Get
                Return TestFlag(Me.m_Flags, RichiestaFinanziamentoFlags.Soppressa)
            End Get
            Set(value As Boolean)
                If (Me.Soppressa = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, RichiestaFinanziamentoFlags.Soppressa, value)
                Me.DoChanged("Soppressa", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'allegato del modulo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Modulo As CAttachment
            Get
                If (Me.m_Modulo Is Nothing) Then Me.m_Modulo = Sistema.Attachments.GetItemById(Me.m_IDModulo)
                Return Me.m_Modulo
            End Get
            Set(value As CAttachment)
                Dim oldValue As CAttachment = Me.m_Modulo
                If (oldValue Is value) Then Exit Property
                Me.m_IDModulo = GetID(value)
                Me.m_Modulo = value
                Me.DoChanged("Modulo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la durata in secondi della consulenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Durata As Double
            Get
                Return Me.m_Durata
            End Get
            Set(value As Double)
                Dim oldValue As Double = Me.m_Durata
                If (Arrays.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_Durata = value
                Me.DoChanged("Durata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del contesto in cui è stata creata la consulenza
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
                Me.DoChanged("IDContesto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo del contesto in cui è stato creato l'oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoContesto As String
            Get
                Return Me.m_TipoContesto
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TipoContesto
                If (oldValue = value) Then Exit Property
                Me.m_TipoContesto = value
                Me.DoChanged("TipoContesto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo della richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoRichiesta As TipoRichiestaFinanziamento
            Get
                Return Me.m_TipoRichiesta
            End Get
            Set(value As TipoRichiestaFinanziamento)
                Dim oldValue As TipoRichiestaFinanziamento = Me.m_TipoRichiesta
                If (oldValue = value) Then Exit Property
                Me.m_TipoRichiesta = value
                Me.DoChanged("TipoRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'estremo superiore dell'importo richiesto nel caso di TipoRichieste (TRA)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImportoRichiesto1 As Nullable(Of Decimal)
            Get
                Return Me.m_ImportoRichiesto1
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_ImportoRichiesto1
                If (oldValue = value) Then Exit Property
                Me.m_ImportoRichiesto1 = value
                Me.DoChanged("ImportoRichiesto1", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato della richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoRichiesta As StatoRichiestaFinanziamento
            Get
                Return Me.m_StatoRichiesta
            End Get
            Set(value As StatoRichiestaFinanziamento)
                Dim oldValue As StatoRichiestaFinanziamento = Me.m_StatoRichiesta
                If (oldValue = value) Then Exit Property
                Me.m_StatoRichiesta = value
                Me.DoChanged("StatoRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il canale attraverso cui è pervenuto questo contatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Canale As CCanale
            Get
                If (Me.m_Canale Is Nothing) Then Me.m_Canale = Anagrafica.Canali.GetItemById(Me.m_IDCanale)
                Return Me.m_Canale
            End Get
            Set(value As CCanale)
                Dim oldValue As CCanale = Me.Canale
                If (oldValue Is value) Then Exit Property
                Me.m_Canale = value
                Me.m_IDCanale = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeCanale = value.Nome
                Me.DoChanged("Canale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del canale 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDCanale As Integer
            Get
                Return GetID(Me.m_Canale, Me.m_IDCanale)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCanale
                If (oldValue = value) Then Exit Property
                Me.m_IDCanale = value
                Me.m_Canale = Nothing
                Me.DoChanged("IDCanale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del canale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeCanale As String
            Get
                Return Me.m_NomeCanale
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeCanale
                If (oldValue = value) Then Exit Property
                Me.m_NomeCanale = value
                Me.DoChanged("NomeCanale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il canale attraverso cui è pervenuto questo contatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Canale1 As CCanale
            Get
                If (Me.m_Canale1 Is Nothing) Then Me.m_Canale1 = Anagrafica.Canali.GetItemById(Me.m_IDCanale1)
                Return Me.m_Canale1
            End Get
            Set(value As CCanale)
                Dim oldValue As CCanale = Me.Canale1
                If (oldValue Is value) Then Exit Property
                Me.m_Canale1 = value
                Me.m_IDCanale1 = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeCanale1 = value.Nome
                Me.DoChanged("Canale1", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del canale 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDCanale1 As Integer
            Get
                Return GetID(Me.m_Canale1, Me.m_IDCanale1)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCanale1
                If (oldValue = value) Then Exit Property
                Me.m_IDCanale1 = value
                Me.m_Canale1 = Nothing
                Me.DoChanged("IDCanale1", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del canale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeCanale1 As String
            Get
                Return Me.m_NomeCanale1
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeCanale1
                If (oldValue = value) Then Exit Property
                Me.m_NomeCanale1 = value
                Me.DoChanged("NomeCanale1", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data 
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

        Public Property TipoFonte As String
            Get
                Return Me.m_TipoFonte
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_TipoFonte
                If (oldValue = value) Then Exit Property
                Me.m_TipoFonte = value
                Me.DoChanged("TipoFonte", value, oldValue)
            End Set
        End Property

        Public Property IDFonte As Integer
            Get
                Return GetID(Me.m_Fonte, Me.m_IDFonte)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDFonte
                If (oldValue = value) Then Exit Property
                Me.m_IDFonte = value
                Me.m_Fonte = Nothing
                Me.DoChanged("IDFonte", value, oldValue)
            End Set
        End Property

        Public Property Fonte As IFonte
            Get
                If Me.m_Fonte Is Nothing Then Me.m_Fonte = Anagrafica.Fonti.GetItemById(Me.m_IDFonte)
                Return Me.m_Fonte
            End Get
            Set(value As IFonte)
                If (GetID(value) = Me.IDFonte) Then Exit Property
                Dim oldValue As IFonte = Me.Fonte
                Me.m_Fonte = value
                Me.m_IDFonte = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeFonte = value.Nome
                Me.DoChanged("Fonte", value, oldValue)
            End Set
        End Property

        Public Property NomeFonte As String
            Get
                Return Me.m_NomeFonte
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeFonte
                If (oldValue = value) Then Exit Property
                Me.m_NomeFonte = value
                Me.DoChanged("NomeFonte", value, oldValue)
            End Set
        End Property

        Public Property Referrer As String
            Get
                Return Me.m_Referrer
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Referrer
                If (oldValue = value) Then Exit Property
                Me.m_Referrer = value
                Me.DoChanged("Referrer", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'importo richiesto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <exception cref="ArgumentOutOfRangeException">Se il valore specificato non è NULL esso deve essere positivo</exception>
        ''' <remarks></remarks>
        Public Property ImportoRichiesto As Nullable(Of Decimal)
            Get
                Return Me.m_ImportoRichiesto
            End Get
            Set(value As Nullable(Of Decimal))
                If (value.HasValue AndAlso value.Value <= 0) Then Throw New ArgumentOutOfRangeException("ImportoRichiesto")
                Dim oldValue As Nullable(Of Decimal) = Me.m_ImportoRichiesto
                If (oldValue.Equals(value)) Then Exit Property
                Me.m_ImportoRichiesto = value
                Me.DoChanged("ImportoRichiesto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la massima rata che il cliente desidera pagare.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <exception cref="ArgumentOutOfRangeException">Se il valore specificato non è NULL esso deve essere positivo</exception>
        ''' <remarks></remarks>
        Public Property RataMassima As Nullable(Of Decimal)
            Get
                Return Me.m_RataMassima
            End Get
            Set(value As Nullable(Of Decimal))
                If (value.HasValue AndAlso value.Value <= 0) Then Throw New ArgumentOutOfRangeException("RataMassima")
                Dim oldValue As Nullable(Of Decimal) = Me.m_RataMassima
                If (oldValue.Equals(value)) Then Exit Property
                Me.m_RataMassima = value
                Me.DoChanged("RataMassima", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la durata massima desiderata per il finanziamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <exception cref="ArgumentOutOfRangeException">Se il valore specificato non è NULL esso deve essere positivo</exception>
        ''' <remarks></remarks>
        Public Property DurataMassima As Nullable(Of Integer)
            Get
                Return Me.m_DurataMassima
            End Get
            Set(value As Nullable(Of Integer))
                If (value.HasValue AndAlso value.Value <= 0) Then Throw New ArgumentOutOfRangeException("DurataMassima")
                Dim oldValue As Integer? = Me.m_DurataMassima
                If (oldValue = value) Then Exit Property
                Me.m_DurataMassima = value
                Me.DoChanged("DurataMassima", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del cliente che ha effettuato la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDCliente As Integer
            Get
                Return GetID(Me.m_Cliente, Me.m_IDCliente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCliente
                If (oldValue = value) Then Exit Property
                Me.m_IDCliente = value
                Me.m_Cliente = Nothing
                Me.DoChanged("IDCliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'anagrafica del cliente che ha effettuato la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Cliente As CPersonaFisica
            Get
                If (Me.m_Cliente Is Nothing) Then Me.m_Cliente = Anagrafica.Persone.GetItemById(Me.m_IDCliente)
                Return Me.m_Cliente
            End Get
            Set(value As CPersonaFisica)
                'If (GetID(value) = Me.IDCliente) Then Exit Property
                Dim oldValue As CPersonaFisica = Me.Cliente
                Me.m_IDCliente = GetID(value)
                Me.m_Cliente = value
                If (value IsNot Nothing) Then Me.m_NomeCliente = value.Nominativo
            End Set
        End Property

        Protected Friend Overridable Sub SetCliente(ByVal value As CPersonaFisica)
            Me.m_Cliente = value
            Me.m_IDCliente = GetID(value)
        End Sub

        Public Property NomeCliente As String
            Get
                Return Me.m_NomeCliente
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeCliente
                Me.m_NomeCliente = value
                Me.DoChanged("NomeCliente", value, oldValue)
            End Set
        End Property



        ''' <summary>
        ''' Restituisce o imposta delle annotazioni testuali
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
        ''' Restituisce o imposta l'ID dell'operatore a cui è assegnata la richiesta.
        ''' Se questo valore è 0 la richiesta è associata al punto operativo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDAssegnatoA As Integer
            Get
                Return GetID(Me.m_AssegnatoA, Me.m_IDAssegnatoA)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAssegnatoA
                If (oldValue = value) Then Exit Property
                Me.m_IDAssegnatoA = value
                Me.m_AssegnatoA = Nothing
                Me.DoChanged("IDAssegnatoA", value, oldValue)
            End Set
        End Property

        Public Property AssegnatoA As CUser
            Get
                If (Me.m_AssegnatoA Is Nothing) Then Me.m_AssegnatoA = Sistema.Users.GetItemById(Me.m_IDAssegnatoA)
                Return Me.m_AssegnatoA
            End Get
            Set(value As CUser)
                If (GetID(value) = Me.IDAssegnatoA) Then Exit Property
                Dim oldValue As CUser = Me.AssegnatoA
                Me.m_AssegnatoA = value
                Me.m_IDAssegnatoA = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeAssegnatoA = value.Nominativo
                Me.DoChanged("AssegnatoA", value, oldValue)
            End Set
        End Property

        Public Property NomeAssegnatoA As String
            Get
                Return Me.m_NomeAssegnatoA
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeAssegnatoA
                If (oldValue = value) Then Exit Property
                Me.m_NomeAssegnatoA = value
                Me.DoChanged("NomeAssegnatoA", value, oldValue)
            End Set
        End Property

        Public Property IDPresaInCaricoDa As Integer
            Get
                Return GetID(Me.m_PresaInCaricoDa, Me.m_IDPresaInCaricoDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPresaInCaricoDa
                If (oldValue = value) Then Exit Property
                Me.m_IDPresaInCaricoDa = value
                Me.m_PresaInCaricoDa = Nothing
                Me.DoChanged("IDPresaInCaricoDa", value, oldValue)
            End Set
        End Property

        Public Property PresaInCaricoDa As CUser
            Get
                If (Me.m_PresaInCaricoDa Is Nothing) Then Me.m_PresaInCaricoDa = Sistema.Users.GetItemById(Me.m_IDPresaInCaricoDa)
                Return Me.m_PresaInCaricoDa
            End Get
            Set(value As CUser)
                If (GetID(value) = Me.IDPresaInCaricoDa) Then Exit Property
                Dim oldValue As CUser = Me.PresaInCaricoDa
                Me.m_PresaInCaricoDa = value
                Me.m_IDPresaInCaricoDa = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomePresaInCarocoDa = value.Nominativo
                Me.DoChanged("PresaInCaricoDa", value, oldValue)
            End Set
        End Property

        Public Property NomePresaInCaricoDa As String
            Get
                Return Me.m_NomePresaInCarocoDa
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomePresaInCarocoDa
                If (oldValue = value) Then Exit Property
                Me.m_NomePresaInCarocoDa = value
                Me.DoChanged("NomePresaInCaricoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che identifica la fonte
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDFonteStr As String
            Get
                Return Me.m_IDFonteStr
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_IDFonteStr
                If (oldValue = value) Then Exit Property
                Me.m_IDFonteStr = value
                Me.DoChanged("IDFonteStr", value, oldValue)
            End Set
        End Property
        ''' <summary>
        ''' Restituisce o imposta una stringa che indica 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDCampagnaStr As String
            Get
                Return Me.m_IDCampagnaStr
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_IDCampagnaStr
                If (oldValue = value) Then Exit Property
                Me.m_IDCampagnaStr = value
                Me.DoChanged("IDCampagnaStr", value, oldValue)
            End Set
        End Property


        Public Property IDAnnuncioStr As String
            Get
                Return Me.m_IDAnnuncioStr
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValeu As String = Me.m_IDAnnuncioStr
                If (oldValeu = value) Then Exit Property
                Me.m_IDAnnuncioStr = value
                Me.DoChanged("IDAnnuncioStr", value, oldValeu)
            End Set
        End Property

        Public Property IDKeyWordStr As String
            Get
                Return Me.m_IDKeyWordStr
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_IDKeyWordStr
                If (oldValue = value) Then Exit Property
                Me.m_IDKeyWordStr = value
                Me.DoChanged("IDKeyWordStr", value, oldValue)
            End Set
        End Property

        Public Property IDPratica As Integer
            Get
                Return GetID(Me.m_Pratica, Me.m_IDPratica)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPratica
                If (oldValue = value) Then Exit Property
                Me.m_IDPratica = value
                Me.m_Pratica = Nothing
                Me.DoChanged("IDPRatica", value, oldValue)
            End Set
        End Property

        Public Property Pratica As CRapportino
            Get
                If Me.m_Pratica Is Nothing Then Me.m_Pratica = DMD.CQSPD.Pratiche.GetItemById(Me.m_IDPratica)
                Return Me.m_Pratica
            End Get
            Set(value As CRapportino)
                Dim oldValue As CRapportino = Me.Pratica
                If (oldValue Is value) Then Exit Property
                Me.m_IDPratica = GetID(value)
                Me.m_Pratica = value
                Me.DoChanged("Pratica", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property StatoEx As String
            Get
                If (Me.m_IDPratica <> 0) Then Return "Generata la pratica N°" & Strings.Hex(Me.m_IDPratica)
                If (Me.m_IDPresaInCaricoDa <> 0) Then Return "Presa in carico da " & Me.NomePresaInCaricoDa
                If (Me.m_IDAssegnatoA <> 0) Then Return "Assegnata a " & Me.NomeAssegnatoA
                Return "Non assegnata"
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return DMD.CQSPD.RichiesteFinanziamento.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_RichiesteFinanziamenti"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Data = reader.Read("Data", Me.m_Data)
            Me.m_IDFonte = reader.Read("IDFonte", Me.m_IDFonte)
            Me.m_NomeFonte = reader.Read("NomeFonte", Me.m_NomeFonte)
            Me.m_Referrer = reader.Read("Referrer", Me.m_Referrer)
            Me.m_ImportoRichiesto = reader.Read("ImportoRichiesto", Me.m_ImportoRichiesto)
            Me.m_RataMassima = reader.Read("RataMassimima", Me.m_RataMassima)
            Me.m_DurataMassima = reader.Read("DurataMassima", Me.m_DurataMassima)
            Me.m_IDCliente = reader.Read("IDCliente", Me.m_IDCliente)
            Me.m_NomeCliente = reader.Read("NomeCliente", Me.m_NomeCliente)
            Me.m_Note = reader.Read("Note", Me.m_Note)
            Me.m_IDAssegnatoA = reader.Read("IDAssegnatoA", Me.m_IDAssegnatoA)
            Me.m_NomeAssegnatoA = reader.Read("NomeAssegnatoA", Me.m_NomeAssegnatoA)
            Me.m_IDPresaInCaricoDa = reader.Read("IDPresaInCaricoDa", Me.m_IDPresaInCaricoDa)
            Me.m_NomePresaInCarocoDa = reader.Read("NomePresaInCaricoDa", Me.m_NomePresaInCarocoDa)
            Me.m_IDFonteStr = reader.Read("IDFonteStr", Me.m_IDFonteStr)
            Me.m_IDCampagnaStr = reader.Read("IDCampagnaStr", Me.m_IDCampagnaStr)
            Me.m_IDAnnuncioStr = reader.Read("IDAnnuncioStr", Me.m_IDAnnuncioStr)
            Me.m_IDKeyWordStr = reader.Read("IDKeyWordStr", Me.m_IDKeyWordStr)
            Me.m_IDPratica = reader.Read("IDPratica", Me.m_IDPratica)
            Me.m_TipoFonte = reader.Read("TipoFonte", Me.m_TipoFonte)
            Me.m_IDCanale = reader.Read("IDCanale", Me.m_IDCanale)
            Me.m_NomeCanale = reader.Read("NomeCanale", Me.m_NomeCanale)
            Me.m_IDCanale1 = reader.Read("IDCanale1", Me.m_IDCanale1)
            Me.m_NomeCanale1 = reader.Read("NomeCanale1", Me.m_NomeCanale1)
            Me.m_StatoRichiesta = reader.Read("StatoRichiesta", Me.m_StatoRichiesta)
            Me.m_TipoRichiesta = reader.Read("TipoRichiesta", Me.m_TipoRichiesta)
            Me.m_ImportoRichiesto1 = reader.Read("ImportoRichiesto1", Me.m_ImportoRichiesto1)
            Me.m_IDContesto = reader.Read("IDContesto", Me.m_IDContesto)
            Me.m_TipoContesto = reader.Read("TipoContesto", Me.m_TipoContesto)
            Me.m_Durata = reader.Read("Durata", Me.m_Durata)
            Me.m_IDPrivacy = reader.Read("IDPrivacy", Me.m_IDPrivacy)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_IDModulo = reader.Read("IDModulo", Me.m_IDModulo)
            Me.m_IDFinestraLavorazione = reader.Read("IDFinestraLavorazione", Me.m_IDFinestraLavorazione)
            Me.m_Scopo = reader.Read("Scopo", Me.m_Scopo)
            Me.m_IDCollaboratore = reader.Read("IDCollaboratore", Me.m_IDCollaboratore)
            Me.m_NomeCollaboratore = reader.Read("NomeCollaboratore", Me.m_NomeCollaboratore)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Data", Me.m_Data)
            writer.Write("IDFonte", Me.IDFonte)
            writer.Write("NomeFonte", Me.m_NomeFonte)
            writer.Write("Referrer", Me.m_Referrer)
            writer.Write("ImportoRichiesto", Me.m_ImportoRichiesto)
            writer.Write("RataMassimima", Me.m_RataMassima)
            writer.Write("DurataMassima", Me.m_DurataMassima)
            writer.Write("IDCliente", Me.IDCliente)
            writer.Write("NomeCliente", Me.m_NomeCliente)
            writer.Write("Note", Me.m_Note)
            writer.Write("IDAssegnatoA", Me.IDAssegnatoA)
            writer.Write("NomeAssegnatoA", Me.m_NomeAssegnatoA)
            writer.Write("IDPresaInCaricoDa", Me.IDPresaInCaricoDa)
            writer.Write("NomePresaInCaricoDa", Me.m_NomePresaInCarocoDa)
            writer.Write("IDFonteStr", Me.m_IDFonteStr)
            writer.Write("IDCampagnaStr", Me.m_IDCampagnaStr)
            writer.Write("IDAnnuncioStr", Me.m_IDAnnuncioStr)
            writer.Write("IDKeyWordStr", Me.m_IDKeyWordStr)
            writer.Write("IDPratica", Me.IDPratica)
            writer.Write("TipoFonte", Me.m_TipoFonte)
            writer.Write("IDCanale", Me.IDCanale)
            writer.Write("NomeCanale", Me.m_NomeCanale)
            writer.Write("IDCanale1", Me.IDCanale1)
            writer.Write("NomeCanale1", Me.m_NomeCanale1)
            writer.Write("StatoRichiesta", Me.m_StatoRichiesta)
            writer.Write("TipoRichiesta", Me.m_TipoRichiesta)
            writer.Write("ImportoRichiesto1", Me.m_ImportoRichiesto1)
            writer.Write("IDContesto", Me.m_IDContesto)
            writer.Write("TipoContesto", Me.m_TipoContesto)
            writer.Write("Durata", Me.m_Durata)
            writer.Write("IDPrivacy", Me.IDPrivacy)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("IDModulo", Me.IDModulo)
            writer.Write("IDFinestraLavorazione", Me.IDFinestraLavorazione)
            writer.Write("Scopo", Me.m_Scopo)
            writer.Write("IDCollaboratore", Me.IDCollaboratore)
            writer.Write("NomeCollaboratore", Me.m_NomeCollaboratore)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Data", Me.m_Data)
            writer.WriteAttribute("IDFonte", Me.IDFonte)
            writer.WriteAttribute("NomeFonte", Me.m_NomeFonte)
            writer.WriteAttribute("Referrer", Me.m_Referrer)
            writer.WriteAttribute("ImportoRichiesto", Me.m_ImportoRichiesto)
            writer.WriteAttribute("RataMassimima", Me.m_RataMassima)
            writer.WriteAttribute("DurataMassima", Me.m_DurataMassima)
            writer.WriteAttribute("IDCliente", Me.IDCliente)
            writer.WriteAttribute("NomeCliente", Me.m_NomeCliente)
            writer.WriteAttribute("IDAssegnatoA", Me.IDAssegnatoA)
            writer.WriteAttribute("NomeAssegnatoA", Me.m_NomeAssegnatoA)
            writer.WriteAttribute("IDPresaInCaricoDa", Me.IDPresaInCaricoDa)
            writer.WriteAttribute("NomePresaInCaricoDa", Me.m_NomePresaInCarocoDa)
            writer.WriteAttribute("IDFonteStr", Me.m_IDFonteStr)
            writer.WriteAttribute("IDCampagnaStr", Me.m_IDCampagnaStr)
            writer.WriteAttribute("IDAnnuncioStr", Me.m_IDAnnuncioStr)
            writer.WriteAttribute("IDKeyWordStr", Me.m_IDKeyWordStr)
            writer.WriteAttribute("IDPratica", Me.IDPratica)
            writer.WriteAttribute("TipoFonte", Me.m_TipoFonte)
            writer.WriteAttribute("IDCanale", Me.IDCanale)
            writer.WriteAttribute("NomeCanale", Me.m_NomeCanale)
            writer.WriteAttribute("IDCanale1", Me.IDCanale1)
            writer.WriteAttribute("NomeCanale1", Me.m_NomeCanale1)
            writer.WriteAttribute("StatoRichiesta", Me.m_StatoRichiesta)
            writer.WriteAttribute("TipoRichiesta", Me.m_TipoRichiesta)
            writer.WriteAttribute("ImportoRichiesto1", Me.m_ImportoRichiesto1)
            writer.WriteAttribute("IDContesto", Me.m_IDContesto)
            writer.WriteAttribute("TipoContesto", Me.m_TipoContesto)
            writer.WriteAttribute("Durata", Me.m_Durata)
            writer.WriteAttribute("IDPrivacy", Me.IDPrivacy)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("IDModulo", Me.IDModulo)
            writer.WriteAttribute("IDFinestraLavorazione", Me.IDFinestraLavorazione)
            writer.WriteAttribute("Scopo", Me.m_Scopo)
            writer.WriteAttribute("IDCollaboratore", Me.IDCollaboratore)
            writer.WriteAttribute("NomeCollaboratore", Me.m_NomeCollaboratore)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Note", Me.m_Note)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Data" : Me.m_Data = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDFonte" : Me.m_IDFonte = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeFonte" : Me.m_NomeFonte = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Referrer" : Me.m_Referrer = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ImportoRichiesto" : Me.m_ImportoRichiesto = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "RataMassimima" : Me.m_RataMassima = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "DurataMassima" : Me.m_DurataMassima = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "IDCliente" : Me.m_IDCliente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCliente" : Me.m_NomeCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Note" : Me.m_Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDAssegnatoA" : Me.m_IDAssegnatoA = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAssegnatoA" : Me.m_NomeAssegnatoA = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDPresaInCaricoDa" : Me.m_IDPresaInCaricoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePresaInCaricoDa" : Me.m_NomePresaInCarocoDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDFonteStr" : Me.m_IDFonteStr = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDCampagnaStr" : Me.m_IDCampagnaStr = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDAnnuncioStr" : Me.m_IDAnnuncioStr = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDKeyWordStr" : Me.m_IDKeyWordStr = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDPratica" : Me.m_IDPratica = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoFonte" : Me.m_TipoFonte = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDCanale" : Me.m_IDCanale = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCanale" : Me.m_NomeCanale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDCanale1" : Me.m_IDCanale1 = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCanale1" : Me.m_NomeCanale1 = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoRichiesta" : Me.m_StatoRichiesta = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoRichiesta" : Me.m_TipoRichiesta = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ImportoRichiesto1" : Me.m_ImportoRichiesto1 = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "IDContesto" : Me.m_IDContesto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoContesto" : Me.m_TipoContesto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Durata" : Me.m_Durata = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "IDPrivacy" : Me.m_IDPrivacy = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDModulo" : Me.m_IDModulo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDFinestraLavorazione" : Me.m_IDFinestraLavorazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Scopo" : Me.m_Scopo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeCollaboratore" : Me.m_NomeCollaboratore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDCollaboratore" : Me.m_IDCollaboratore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Dim ret As New System.Text.StringBuilder
            If (Me.Cliente Is Nothing) Then
                ret.Append("Il cliente (" & Me.IDCliente & ")")
            Else
                ret.Append(IIf(Me.Cliente.Sesso = "F", "La Sig.ra ", "Il Sig. "))
                ret.Append(Me.Cliente.Nominativo)
            End If
            ret.Append(" ha richiesto un finanziamento ")
            Select Case Me.m_TipoRichiesta
                Case TipoRichiestaFinanziamento.ALMENO : ret.Append("di almeno " & Formats.FormatValuta(Me.ImportoRichiesto))
                Case TipoRichiestaFinanziamento.MASSIMO_POSSIBILE : ret.Append(" pari al massimo possibile")
                Case TipoRichiestaFinanziamento.TRA : ret.Append("tra " & Formats.FormatValuta(Me.ImportoRichiesto) & " e " & Formats.FormatValuta(Me.m_ImportoRichiesto1))
                Case TipoRichiestaFinanziamento.UGUALEA : ret.Append("di " & Formats.FormatValuta(Me.ImportoRichiesto))
            End Select
            ret.Append(" in data " & Formats.FormatUserDateTime(Me.Data))

            Return ret.ToString
        End Function

        ''' <summary>
        ''' Prende in carico la richiesta
        ''' </summary>
        ''' <remarks></remarks>
        Sub PrendiInCarico()
            If (Me.PresaInCaricoDa IsNot Nothing) Then Throw New InvalidOperationException("La richiesta è già stata presa in carico da " & Me.NomePresaInCaricoDa)
            Me.PresaInCaricoDa = Users.CurrentUser
            Me.StatoRichiesta = StatoRichiestaFinanziamento.EVASA
            Me.Save()
        End Sub

        Protected Overrides Sub OnCreate(e As SystemEvent)
            MyBase.OnCreate(e)
            CQSPD.RichiesteFinanziamento.doNuovaRichiesta(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            MyBase.OnDelete(e)
            CQSPD.RichiesteFinanziamento.doRichiestaEliminata(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnModified(e As SystemEvent)
            MyBase.OnModified(e)
            CQSPD.RichiesteFinanziamento.doRichiestaModificata(New ItemEventArgs(Me))
        End Sub

        Public Function CompareTo(ByVal obj As CRichiestaFinanziamento) As Integer
            Dim ret As Integer = DateUtils.Compare(Me.Data, obj.Data)
            If (ret = 0) Then ret = Me.ID.CompareTo(obj.ID)
            Return ret
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function
    End Class


End Class
