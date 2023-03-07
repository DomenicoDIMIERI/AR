Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

      
    ''' <summary>
    ''' Rappresenta una richiesta di conteggio estintivo già presente sul gestionale esterno
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CRichiestaConteggio
        Inherits DBObjectPO

        Public Event Segnalata(ByVal sender As Object, ByVal e As ItemEventArgs)

        Public Event PresaInCarico(ByVal sender As Object, ByVal e As ItemEventArgs)


        Private m_IDRichiestaDiFinanziamento As Integer
        <NonSerialized> _
        Private m_RichiestaDiFinanziamento As CRichiestaFinanziamento

      
        Private m_DataRichiesta As Nullable(Of Date)
        Private m_IDIstituto As Integer
        <NonSerialized> _
        Private m_Istituto As CAzienda
        Private m_NomeIstituto As String
        Private m_IDAgenziaRichiedente As Integer
        <NonSerialized> _
        Private m_AgenziaRichiedente As CAzienda
        Private m_NomeAgenziaRichiedente As String
        Private m_IDAgente As Integer
        <NonSerialized> _
        Private m_Agente As CPersonaFisica
        Private m_NomeAgente As String
        Private m_DataEvasione As Nullable(Of Date)

        Private m_IDPratica As Integer
        Private m_Pratica As CRapportino
        Private m_NumeroPratica As String

        Private m_Descrizione As String

        Private m_IDAllegato As Integer
        Private m_Allegato As CAttachment

        Private m_PresaInCaricoDaID As Integer
        Private m_PresaInCaricoDaNome As String
        Private m_PresaInCaricoDa As CUser
        Private m_DataPresaInCarico As Date?

        Private m_Cliente As CPersona
        Private m_IDCliente As Integer
        Private m_NomeCliente As String

        Private m_ImportoCE As Nullable(Of Decimal)

        Private m_DataSegnalazione As Nullable(Of Date)

        Private m_InviatoDa As CPersona
        Private m_InviatoDaID As Integer
        Private m_InviatoDaNome As String
        Private m_InviatoIl As Date?

        Private m_RicevutoDa As CUser
        Private m_RicevutoDaID As Integer
        Private m_RicevutoDaNome As String
        Private m_RicevutoIl As Date?

        Private m_MezzoDiInvio As String


        Private m_IDFinestraLavorazione As Integer
        Private m_FinestraLavorazione As FinestraLavorazione

        Private m_Esito As String

        Private m_IDCessionario As Integer
        Private m_Cessionario As CCQSPDCessionarioClass
        Private m_NomeCessionario As String
        Private m_DurataMesi As Integer?
        Private m_ImportoRata As Decimal?
        Private m_TAN As Double?
        Private m_TAEG As Double?
        Private m_DataDecorrenzaPratica As DateTime?
        Private m_UltimaScadenza As DateTime?
        Private m_ATCPIVA As String
        Private m_ATCDescrizione As String


        Public Sub New()
            Me.m_IDRichiestaDiFinanziamento = 0
            Me.m_RichiestaDiFinanziamento = Nothing
            Me.m_DataRichiesta = Nothing
            Me.m_IDIstituto = 0
            Me.m_Istituto = Nothing
            Me.m_NomeIstituto = ""
            Me.m_IDAgenziaRichiedente = 0
            Me.m_AgenziaRichiedente = Nothing
            Me.m_NomeAgenziaRichiedente = ""
            Me.m_IDAgente = 0
            Me.m_Agente = Nothing
            Me.m_NomeAgente = ""
            Me.m_DataEvasione = Nothing
            Me.m_IDPratica = 0
            Me.m_Pratica = Nothing
            Me.m_NumeroPratica = ""
            Me.m_Descrizione = ""
            Me.m_IDAllegato = 0
            Me.m_Allegato = Nothing
            Me.m_PresaInCaricoDa = Nothing
            Me.m_PresaInCaricoDaID = 0
            Me.m_PresaInCaricoDaNome = ""
            Me.m_DataPresaInCarico = Nothing
            Me.m_Cliente = Nothing
            Me.m_IDCliente = 0
            Me.m_NomeCliente = ""
            Me.m_ImportoCE = Nothing
            Me.m_DataSegnalazione = Nothing
            Me.m_InviatoDa = Nothing
            Me.m_InviatoDaID = 0
            Me.m_InviatoDaNome = ""
            Me.m_InviatoIl = Nothing
            Me.m_RicevutoDa = Nothing
            Me.m_RicevutoDaID = 0
            Me.m_RicevutoDaNome = ""
            Me.m_RicevutoIl = Nothing
            Me.m_MezzoDiInvio = ""
            Me.m_IDFinestraLavorazione = 0
            Me.m_FinestraLavorazione = Nothing
            Me.m_Esito = ""

            Me.m_IDCessionario = 0
            Me.m_Cessionario = Nothing
            Me.m_NomeCessionario = ""
            Me.m_DurataMesi = Nothing
            Me.m_ImportoRata = Nothing
            Me.m_TAN = Nothing
            Me.m_TAEG = Nothing
            Me.m_DataDecorrenzaPratica = Nothing
            Me.m_UltimaScadenza = Nothing
            Me.m_ATCPIVA = ""
            Me.m_ATCDescrizione = ""
        End Sub

        Public Property IDCessionario As Integer
            Get
                Return GetID(Me.m_Cessionario, Me.m_IDCessionario)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCessionario
                If (oldValue = value) Then Return
                Me.m_IDCessionario = value
                Me.m_Cessionario = Nothing
                Me.DoChanged("IDCessionario", value, oldValue)
            End Set
        End Property

        Public Property Cessionario As CCQSPDCessionarioClass
            Get
                If (Me.m_Cessionario Is Nothing) Then Me.m_Cessionario = CQSPD.Cessionari.GetItemById(Me.m_IDCessionario)
                Return Me.m_Cessionario
            End Get
            Set(value As CCQSPDCessionarioClass)
                Dim oldValue As CCQSPDCessionarioClass = Me.Cessionario
                If (oldValue Is value) Then Return
                Me.m_Cessionario = value
                Me.m_IDCessionario = GetID(value)
                Me.m_NomeCessionario = "" : If (value IsNot Nothing) Then Me.m_NomeCessionario = value.Nome
                Me.DoChanged("Cessionario", value, oldValue)
            End Set
        End Property

        Public Property NomeCessionario As String
            Get
                Return Me.m_NomeCessionario
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeCessionario
                If (oldValue = value) Then Return
                Me.m_NomeCessionario = value
                Me.DoChanged("NomeCessionario", value, oldValue)
            End Set
        End Property

        Public Property DurataMesi As Integer?
            Get
                Return Me.m_DurataMesi
            End Get
            Set(value As Integer?)
                Dim oldValue As Integer? = Me.m_DurataMesi
                If (oldValue = value) Then Return
                Me.m_DurataMesi = value
                Me.DoChanged("DurataMesi", value, oldValue)
            End Set
        End Property

        Public Property ImportoRata As Decimal?
            Get
                Return Me.m_ImportoRata
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_ImportoRata
                If (oldValue = value) Then Return
                Me.m_ImportoRata = value
                Me.DoChanged("ImportoRata", value, oldValue)
            End Set
        End Property

        Public Property TAN As Double?
            Get
                Return Me.m_TAN
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_TAN
                If (oldValue = value) Then Return
                Me.m_TAN = value
                Me.DoChanged("TAN", value, oldValue)
            End Set
        End Property

        Public Property TAEG As Double?
            Get
                Return Me.m_TAEG
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_TAEG
                If (oldValue = value) Then Return
                Me.m_TAEG = value
                Me.DoChanged("TAEG", value, oldValue)
            End Set
        End Property

        Public Property DataDecorrenzaPratica As DateTime?
            Get
                Return Me.m_DataDecorrenzaPratica
            End Get
            Set(value As DateTime?)
                Dim oldValue As DateTime? = Me.m_DataDecorrenzaPratica
                If (Calendar.Compare(oldValue, value) = 0) Then Return
                Me.m_DataDecorrenzaPratica = value
                Me.DoChanged("DataDecorrenzaPratica", value, oldValue)
            End Set
        End Property

        Public Property UltimaScadenza As DateTime?
            Get
                Return Me.m_UltimaScadenza
            End Get
            Set(value As DateTime?)
                Dim oldValue As DateTime? = Me.m_UltimaScadenza
                If (Calendar.Compare(value, oldValue) = 0) Then Return
                Me.m_UltimaScadenza = value
                Me.DoChanged("UltimaScadenza", value, oldValue)
            End Set
        End Property

        Public Property ATCPIVA As String
            Get
                Return Me.m_ATCPIVA
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_ATCPIVA
                value = Strings.Trim(value)
                If (value = oldValue) Then Return
                Me.m_ATCPIVA = value
                Me.DoChanged("ATCPIVA", value, oldValue)
            End Set
        End Property

        Public Property ATCDescrizione As String
            Get
                Return Me.m_ATCDescrizione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_ATCDescrizione
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_ATCDescrizione = value
                Me.DoChanged("ATCDescrizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che descrive l'esito del conteggio
        ''' </summary>
        ''' <returns></returns>
        Public Property Esito As String
            Get
                Return Me.m_Esito
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Esito
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_Esito = value
                Me.DoChanged("Esito", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della finestra di lavorazione in cui è avvenuta la richiesta
        ''' </summary>
        ''' <returns></returns>
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
        ''' Restituisce o imposta la persona che ha inviato il modulo di richiesta all'agenzia (es. da  Prestitalia a Fin.Se.A.)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property InviatoDa As CPersona
            Get
                If (Me.m_InviatoDa Is Nothing) Then Me.m_InviatoDa = Anagrafica.Persone.GetItemById(Me.m_InviatoDaID)
                Return Me.m_InviatoDa
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_InviatoDa
                If (oldValue Is value) Then Exit Property
                Me.m_InviatoDa = value
                Me.m_InviatoDaID = GetID(value)
                If (value IsNot Nothing) Then Me.m_InviatoDaNome = value.Nominativo
                Me.DoChanged("InviatoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della persona che ha inviato il modulo all'agenzia (Es. da Prestitalia a Fin.Se.A)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property InviatoDaID As Integer
            Get
                Return GetID(Me.m_InviatoDa, Me.m_InviatoDaID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.InviatoDaID
                If (oldValue = value) Then Exit Property
                Me.m_InviatoDa = Nothing
                Me.m_InviatoDaID = value
                Me.DoChanged("InviatoDaID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della persoan che ha inviato il modulo (Es. da Prestitalia a Fin.Se.A )
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property InviatoDaNome As String
            Get
                Return Me.m_InviatoDaNome
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_InviatoDaNome
                If (oldValue = value) Then Exit Property
                Me.m_InviatoDaNome = value
                Me.DoChanged("InviatoDaNome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di invio del modulo (da Prestitalia a Fin.Se.A)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property InviatoIl As Date?
            Get
                Return Me.m_InviatoIl
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_InviatoIl
                If Calendar.Compare(value, oldValue) = 0 Then Exit Property
                Me.m_InviatoIl = value
                Me.DoChanged("InviatoIl", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha registrato il modulo nel sistema
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RicevutoDa As CUser
            Get
                If (Me.m_RicevutoDa Is Nothing) Then Me.m_RicevutoDa = Sistema.Users.GetItemById(Me.m_RicevutoDaID)
                Return Me.m_RicevutoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.RicevutoDa
                If (oldValue Is value) Then Exit Property
                Me.m_RicevutoDa = value
                Me.m_RicevutoDaID = GetID(value)
                If (value IsNot Nothing) Then Me.m_RicevutoDaNome = value.Nominativo
                Me.DoChanged("RicevutoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha registrato il modulo nel sistema
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RicevutoDaID As Integer
            Get
                Return GetID(Me.m_RicevutoDa, Me.m_RicevutoDaID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.RicevutoDaID
                If (oldValue = value) Then Exit Property
                Me.m_RicevutoDa = Nothing
                Me.m_RicevutoDaID = value
                Me.DoChanged("RicevutoDaID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che ha registrato il modulo nel sistema
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RicevutoDaNome As String
            Get
                Return Me.m_RicevutoDaNome
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_RicevutoDaNome
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_RicevutoDaNome = value
                Me.DoChanged("RicevutoDaNome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di registrazione del modulo nel sistema
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RicevutoIl As Date?
            Get
                Return Me.m_RicevutoIl
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_RicevutoIl
                If Calendar.Compare(value, oldValue) = 0 Then Exit Property
                Me.m_RicevutoIl = value
                Me.DoChanged("RicevutoIl", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del mezzo utilizzato per inviare il modulo (da Prestitalia all'agenzia, es. e-mail)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MezzoDiInvio As String
            Get
                Return Me.m_MezzoDiInvio
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_MezzoDiInvio
                If (oldValue = value) Then Exit Property
                Me.m_MezzoDiInvio = value
                Me.DoChanged("MezzoDiInvio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il valore da estinguere alla data di rilascio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ImportoCE As Nullable(Of Decimal)
            Get
                Return Me.m_ImportoCE
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_ImportoCE
                If (oldValue = value) Then Exit Property
                Me.m_ImportoCE = value
                Me.DoChanged("ImportoCE", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la persona per cui viene fatta la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Cliente As CPersona
            Get
                If (Me.m_Cliente Is Nothing) Then Me.m_Cliente = Anagrafica.Persone.GetItemById(Me.m_IDCliente)
                Return Me.m_Cliente
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Cliente
                If (oldValue Is value) Then Exit Property
                Me.m_Cliente = value
                Me.m_IDCliente = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeCliente = value.Nominativo
                Me.DoChanged("Cliente", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetCliente(ByVal value As CPersona)
            Me.m_Cliente = value
            Me.m_IDCliente = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID della persona per cui viene fatta la richiesta
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
        ''' Restituisce o imposta il nome del cliente che ha effettuato la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeCliente As String
            Get
                Return Me.m_NomeCliente
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeCliente
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeCliente = value
                Me.DoChanged("NomeCliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della pratica (interna) per cui è stato richiesto il conteggio estintivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPratica As Integer
            Get
                Return GetID(Me.m_Pratica, Me.m_IDPratica)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPratica
                If (oldValue = value) Then Exit Property
                Me.m_IDPratica = value
                Me.m_Pratica = Nothing
                Me.DoChanged("IDPratica", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la pratica per cui è stato chiesto il conteggio estintivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Pratica As CRapportino
            Get
                If (Me.m_Pratica Is Nothing) Then Me.m_Pratica = CQSPD.Pratiche.GetItemById(Me.m_IDPratica)
                Return Me.m_Pratica
            End Get
            Set(value As CRapportino)
                Dim oldValue As CRapportino = Me.m_Pratica
                If (oldValue Is value) Then Exit Property
                Me.m_Pratica = value
                Me.m_IDPratica = GetID(value)
                Me.DoChanged("Pratica", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero esterno della pratica per cui è stato richiesto il conteggio estintivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroPratica As String
            Get
                Return Me.m_NumeroPratica
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NumeroPratica
                If (oldValue = value) Then Exit Property
                Me.m_NumeroPratica = value
                Me.DoChanged("NumeroPratica", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che descrive la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Descrizione
                If (oldValue = value) Then Exit Property
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'allegato contenente il modulo di richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDAllegato As Integer
            Get
                Return GetID(Me.m_Allegato, Me.m_IDAllegato)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAllegato
                If (oldValue = value) Then Exit Property
                Me.m_IDAllegato = value
                Me.m_Allegato = Nothing
                Me.DoChanged("IDAllegato", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'allegato contenente il modulo di richiesta del conteggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Allegato As CAttachment
            Get
                If (Me.m_Allegato Is Nothing) Then Me.m_Allegato = Sistema.Attachments.GetItemById(Me.m_IDAllegato)
                Return Me.m_Allegato
            End Get
            Set(value As CAttachment)
                Dim oldValue As CAttachment = Me.m_Allegato
                If (oldValue Is value) Then Exit Property
                Me.m_Allegato = value
                Me.m_IDAllegato = GetID(value)
                Me.DoChanged("Allegato", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha preso in carico la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PresaInCaricoDa As CUser
            Get
                If (Me.m_PresaInCaricoDa Is Nothing) Then Me.m_PresaInCaricoDa = Sistema.Users.GetItemById(Me.m_PresaInCaricoDaID)
                Return Me.m_PresaInCaricoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.PresaInCaricoDa
                If (oldValue Is value) Then Exit Property
                Me.m_PresaInCaricoDa = value
                Me.m_PresaInCaricoDaID = GetID(value)
                If (value IsNot Nothing) Then Me.m_PresaInCaricoDaNome = value.Nominativo
                Me.DoChanged("PresaInCaricoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta L'ID dell'operatore che ha preso in carico la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PresaInCaricoDaID As Integer
            Get
                Return GetID(Me.m_PresaInCaricoDa, Me.m_PresaInCaricoDaID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.PresaInCaricoDaID
                If (oldValue = value) Then Exit Property
                Me.m_PresaInCaricoDaID = value
                Me.m_PresaInCaricoDa = Nothing
                Me.DoChanged("PresaInCaricoDaID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che ha preso in carico la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PresaInCaricoDaNome As String
            Get
                Return Me.m_PresaInCaricoDaNome
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_PresaInCaricoDaNome
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_PresaInCaricoDaNome = value
                Me.DoChanged("PresaInCaricoDaNome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data in cui la richiesta è stata presa in carico
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataPresaInCarico As Date?
            Get
                Return Me.m_DataPresaInCarico
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataPresaInCarico
                If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataPresaInCarico = value
                Me.DoChanged("DataPresaInCarico", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisdce o imposta l'ID della richiesta di finanziamento in cui è stato registrato questo conteggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDRichiestaDiFinanziamento As Integer
            Get
                Return GetID(Me.m_RichiestaDiFinanziamento, Me.m_IDRichiestaDiFinanziamento)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRichiestaDiFinanziamento
                If (oldValue = value) Then Exit Property
                Me.m_IDRichiestaDiFinanziamento = value
                Me.m_RichiestaDiFinanziamento = Nothing
                Me.DoChanged("IDRichiestaDiFinanziamento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la richiesta di finanziamento in cui è stato registrato questo conteggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RichiestaDiFinanziamento As CRichiestaFinanziamento
            Get
                If (Me.m_RichiestaDiFinanziamento Is Nothing) Then Me.m_RichiestaDiFinanziamento = CQSPD.RichiesteFinanziamento.GetItemById(Me.m_IDRichiestaDiFinanziamento)
                Return Me.m_RichiestaDiFinanziamento
            End Get
            Set(value As CRichiestaFinanziamento)
                Dim oldValue As CRichiestaFinanziamento = Me.m_RichiestaDiFinanziamento
                If (oldValue Is value) Then Exit Property
                Me.m_RichiestaDiFinanziamento = value
                Me.m_IDRichiestaDiFinanziamento = GetID(value)
                Me.DoChanged("RichiestaDiFinanziamento", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetRichiestaDiFinanziamento(ByVal value As CRichiestaFinanziamento)
            Me.m_RichiestaDiFinanziamento = value
            Me.m_IDRichiestaDiFinanziamento = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta la data e l'ora della richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataRichiesta As Nullable(Of Date)
            Get
                Return Me.m_DataRichiesta
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataRichiesta
                If (oldValue = value) Then Exit Property
                Me.m_DataRichiesta = value
                Me.DoChanged("DataRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta L'ID dell'istituto presso cui è stata effettuata la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDIstituto As Integer
            Get
                Return GetID(Me.m_Istituto, Me.m_IDIstituto)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDIstituto
                If (oldValue = value) Then Exit Property
                Me.m_IDIstituto = value
                Me.m_Istituto = Nothing
                Me.DoChanged("IDIstituto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'istituto presso cui è stato richiesto questo conteggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Istituto As CAzienda
            Get
                If (Me.m_Istituto Is Nothing) Then Me.m_Istituto = Anagrafica.Persone.GetItemById(Me.m_IDIstituto)
                Return Me.m_Istituto
            End Get
            Set(value As CAzienda)
                Dim oldValue As CAzienda = Me.m_Istituto
                If (oldValue Is value) Then Exit Property
                Me.m_Istituto = value
                Me.m_IDIstituto = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeIstituto = value.Nominativo
                Me.DoChanged("Istituto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'istituto presso cui è stato richiesto questo conteggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeIstituto As String
            Get
                Return Me.m_NomeIstituto
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeIstituto
                If (oldValue = value) Then Exit Property
                Me.m_NomeIstituto = value
                Me.DoChanged("NomeIstituto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'agenzia che ha richiesto il conteggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDAgenziaRichiedente As Integer
            Get
                Return GetID(Me.m_AgenziaRichiedente, Me.m_IDAgenziaRichiedente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAgenziaRichiedente
                If (oldValue = value) Then Exit Property
                Me.m_IDAgenziaRichiedente = value
                Me.m_AgenziaRichiedente = Nothing
                Me.DoChanged("IDAgenziaRichiedente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restitusce o imposta l'agenzia che ha richiesto questo conteggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AgenziaRichiedente As CAzienda
            Get
                If (Me.m_AgenziaRichiedente Is Nothing) Then Me.m_AgenziaRichiedente = Anagrafica.Aziende.GetItemById(Me.m_IDAgenziaRichiedente)
                Return Me.m_AgenziaRichiedente
            End Get
            Set(value As CAzienda)
                Dim oldValue As CAzienda = Me.m_AgenziaRichiedente
                If (oldValue Is value) Then Exit Property
                Me.m_AgenziaRichiedente = value
                Me.m_IDAgenziaRichiedente = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeAgenziaRichiedente = value.Nominativo
                Me.DoChanged("AgenziaRichiedente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'agenzia richiedente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeAgenziaRichiedente As String
            Get
                Return Me.m_NomeAgenziaRichiedente
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeAgenziaRichiedente
                If (oldValue = value) Then Exit Property
                Me.m_NomeAgenziaRichiedente = value
                Me.DoChanged("NomeAgenziaRichiedente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'agente che ha richiesto il conteggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDAgente As Integer
            Get
                Return GetID(Me.m_Agente, Me.m_IDAgente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAgente
                If (oldValue = value) Then Exit Property
                Me.m_IDAgente = value
                Me.m_Agente = Nothing
                Me.DoChanged("IDAgente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Agente As CPersonaFisica
            Get
                If (Me.m_Agente Is Nothing) Then Me.m_Agente = Anagrafica.Persone.GetItemById(Me.m_IDAgente)
                Return Me.m_Agente
            End Get
            Set(value As CPersonaFisica)
                Dim oldValue As CPersonaFisica = Me.m_Agente
                If (oldValue Is value) Then Exit Property
                Me.m_Agente = value
                Me.m_IDAgente = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeAgente = value.Nominativo
                Me.DoChanged("Agente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'agente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeAgente As String
            Get
                Return Me.m_NomeAgente
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeAgente
                If (oldValue = value) Then Exit Property
                Me.m_NomeAgente = value
                Me.DoChanged("NomeAgente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di evasione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataEvasione As Nullable(Of Date)
            Get
                Return Me.m_DataEvasione
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataEvasione
                If (Calendar.Compare(oldValue, value) = 0) Then Exit Property
                Me.m_DataEvasione = value
                Me.DoChanged("DataEvasione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data in cui questo oggetto è stato segnalato agli operatori
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataSegnalazione As Nullable(Of Date)
            Get
                Return Me.m_DataSegnalazione
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataSegnalazione
                If Calendar.Compare(oldValue, value) = 0 Then Exit Property
                Me.m_DataSegnalazione = value
                Me.DoChanged("DataSegnalazione", value, oldValue)
            End Set
        End Property

        Public Sub Segnala()
            Me.m_DataSegnalazione = Calendar.Now
            Me.Save(True)
            Dim e As New ItemEventArgs(Me)
            Me.doOnSegnalata(e)
        End Sub

        Private Sub doOnSegnalata(ByVal e As ItemEventArgs)
            Me.OnSegnalata(e)
            CQSPD.RichiesteConteggi.doOnSegnalata(e)
        End Sub

        Protected Overridable Sub OnSegnalata(ByVal e As ItemEventArgs)
            RaiseEvent Segnalata(Me, e)
        End Sub

        Public Sub PrendiInCarico()
            Me.PresaInCaricoDa = Sistema.Users.CurrentUser
            Me.DataPresaInCarico = Calendar.Now
            Me.Save(True)
            Dim e As New ItemEventArgs(Me)
            Me.doOnPresaInCarico(e)
        End Sub

        Private Sub doOnPresaInCarico(ByVal e As ItemEventArgs)
            Me.OnPresaInCarico(e)
            CQSPD.RichiesteConteggi.doOnPresaInCarico(e)
        End Sub

        Protected Overridable Sub OnPresaInCarico(ByVal e As ItemEventArgs)
            RaiseEvent PresaInCarico(Me, e)
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return CQSPD.RichiesteConteggi.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_RichiesteFinanziamentiC"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDRichiestaDiFinanziamento = reader.Read("IDRichiestaF", Me.m_IDRichiestaDiFinanziamento)
            Me.m_DataRichiesta = reader.Read("DataRichiesta", Me.m_DataRichiesta)
            Me.m_IDIstituto = reader.Read("IDIstituto", Me.m_IDIstituto)
            Me.m_NomeIstituto = reader.Read("NomeIstituto", Me.m_NomeIstituto)
            Me.m_IDAgenziaRichiedente = reader.Read("IDAgenziaR", Me.m_IDAgenziaRichiedente)
            Me.m_NomeAgenziaRichiedente = reader.Read("NomeAgenziaR", Me.m_NomeAgenziaRichiedente)
            Me.m_IDAgente = reader.Read("IDAgente", Me.m_IDAgente)
            Me.m_NomeAgente = reader.Read("NomeAgente", Me.m_NomeAgente)
            Me.m_DataEvasione = reader.Read("DataEvasione", Me.m_DataEvasione)
            Me.m_ImportoCE = reader.Read("ImportoCE", Me.m_ImportoCE)

            Me.m_IDPratica = reader.Read("IDPratica", Me.m_IDPratica)
            Me.m_NumeroPratica = reader.Read("NumeroPratica", Me.m_NumeroPratica)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_IDAllegato = reader.Read("IDAllegato", Me.m_IDAllegato)
            Me.m_PresaInCaricoDaID = reader.Read("PresaInCaricoDaID", Me.m_PresaInCaricoDaID)
            Me.m_PresaInCaricoDaNome = reader.Read("PresaInCaricoDaNome", Me.m_PresaInCaricoDaNome)
            Me.m_DataPresaInCarico = reader.Read("DataPresaInCarico", Me.m_DataPresaInCarico)
            Me.m_IDCliente = reader.Read("IDCliente", Me.m_IDCliente)
            Me.m_NomeCliente = reader.Read("NomeCliente", Me.m_NomeCliente)

            Me.m_DataSegnalazione = reader.Read("DataSegnalazione", Me.m_DataSegnalazione)

            Me.m_InviatoDaID = reader.Read("InviatoDaID", Me.m_InviatoDaID)
            Me.m_InviatoDaNome = reader.Read("InviatoDaNome", Me.m_InviatoDaNome)
            Me.m_InviatoIl = reader.Read("InviatoIl", Me.m_InviatoIl)
            Me.m_RicevutoDaID = reader.Read("RicevutoDaID", Me.m_RicevutoDaID)
            Me.m_RicevutoDaNome = reader.Read("RicevutoDaNome", Me.m_RicevutoDaNome)
            Me.m_RicevutoIl = reader.Read("RicevutoIl", Me.m_RicevutoIl)
            Me.m_MezzoDiInvio = reader.Read("MezzoDiInvio", Me.m_MezzoDiInvio)
            Me.m_IDFinestraLavorazione = reader.Read("IDFinestraLavorazione", Me.m_IDFinestraLavorazione)

            Me.m_Esito = reader.Read("Esito", Me.m_Esito)

            Me.m_IDCessionario = reader.Read("IDCessionario", Me.m_IDCessionario)
            Me.m_NomeCessionario = reader.Read("NomeCessionario", Me.m_NomeCessionario)
            Me.m_DurataMesi = reader.Read("DurataMesi", Me.m_DurataMesi)
            Me.m_ImportoRata = reader.Read("ImportoRata", Me.m_ImportoRata)
            Me.m_TAN = reader.Read("TAN", Me.m_TAN)
            Me.m_TAEG = reader.Read("TAEG", Me.m_TAEG)
            Me.m_DataDecorrenzaPratica = reader.Read("DataDecorrenzaPratica", Me.m_DataDecorrenzaPratica)
            Me.m_UltimaScadenza = reader.Read("UltimaScadenza", Me.m_UltimaScadenza)
            Me.m_ATCPIVA = reader.Read("ATCPIVA", Me.m_ATCPIVA)
            Me.m_ATCDescrizione = reader.Read("ATCDescrizione", Me.m_ATCDescrizione)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDRichiestaF", Me.IDRichiestaDiFinanziamento)
            writer.Write("DataRichiesta", Me.m_DataRichiesta)
            writer.Write("IDIstituto", Me.IDIstituto)
            writer.Write("NomeIstituto", Me.m_NomeIstituto)
            writer.Write("IDAgenziaR", Me.IDAgenziaRichiedente)
            writer.Write("NomeAgenziaR", Me.m_NomeAgenziaRichiedente)
            writer.Write("IDAgente", Me.IDAgente)
            writer.Write("NomeAgente", Me.m_NomeAgente)
            writer.Write("DataEvasione", Me.m_DataEvasione)
            writer.Write("ImportoCE", Me.m_ImportoCE)
            writer.Write("IDPratica", Me.IDPratica)
            writer.Write("NumeroPratica", Me.m_NumeroPratica)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("IDAllegato", Me.IDAllegato)
            writer.Write("PresaInCaricoDaID", Me.PresaInCaricoDaID)
            writer.Write("PresaInCaricoDaNome", Me.m_PresaInCaricoDaNome)
            writer.Write("DataPresaInCarico", Me.m_DataPresaInCarico)
            writer.Write("IDCliente", Me.IDCliente)
            writer.Write("NomeCliente", Me.m_NomeCliente)
            writer.Write("DataSegnalazione", Me.m_DataSegnalazione)
            writer.Write("InviatoDaID", Me.InviatoDaID)
            writer.Write("InviatoDaNome", Me.m_InviatoDaNome)
            writer.Write("InviatoIl", Me.m_InviatoIl)
            writer.Write("RicevutoDaID", Me.RicevutoDaID)
            writer.Write("RicevutoDaNome", Me.m_RicevutoDaNome)
            writer.Write("RicevutoIl", Me.m_RicevutoIl)
            writer.Write("MezzoDiInvio", Me.m_MezzoDiInvio)
            writer.Write("IDFinestraLavorazione", Me.IDFinestraLavorazione)
            writer.Write("Esito", Me.m_Esito)
            writer.Write("IDCessionario", Me.IDCessionario)
            writer.Write("NomeCessionario", Me.m_NomeCessionario)
            writer.Write("DurataMesi", Me.m_DurataMesi)
            writer.Write("ImportoRata", Me.m_ImportoRata)
            writer.Write("TAN", Me.m_TAN)
            writer.Write("TAEG", Me.m_TAEG)
            writer.Write("DataDecorrenzaPratica", Me.m_DataDecorrenzaPratica)
            writer.Write("UltimaScadenza", Me.m_UltimaScadenza)
            writer.Write("ATCPIVA", Me.m_ATCPIVA)
            writer.Write("ATCDescrizione", Me.m_ATCDescrizione)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDRichiestaF", Me.IDRichiestaDiFinanziamento)
            writer.WriteAttribute("DataRichiesta", Me.m_DataRichiesta)
            writer.WriteAttribute("IDIstituto", Me.IDIstituto)
            writer.WriteAttribute("NomeIstituto", Me.m_NomeIstituto)
            writer.WriteAttribute("IDAgenziaR", Me.IDAgenziaRichiedente)
            writer.WriteAttribute("NomeAgenziaR", Me.m_NomeAgenziaRichiedente)
            writer.WriteAttribute("IDAgente", Me.IDAgente)
            writer.WriteAttribute("NomeAgente", Me.m_NomeAgente)
            writer.WriteAttribute("DataEvasione", Me.m_DataEvasione)
            writer.WriteAttribute("ImportoCE", Me.m_ImportoCE)
            writer.WriteAttribute("IDPratica", Me.IDPratica)
            writer.WriteAttribute("NumeroPratica", Me.m_NumeroPratica)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("IDAllegato", Me.IDAllegato)
            writer.WriteAttribute("PresaInCaricoDaID", Me.PresaInCaricoDaID)
            writer.WriteAttribute("PresaInCaricoDaNome", Me.m_PresaInCaricoDaNome)
            writer.WriteAttribute("DataPresaInCarico", Me.m_DataPresaInCarico)
            writer.WriteAttribute("IDCliente", Me.IDCliente)
            writer.WriteAttribute("NomeCliente", Me.m_NomeCliente)
            writer.WriteAttribute("DataSegnalazione", Me.m_DataSegnalazione)
            writer.WriteAttribute("InviatoDaID", Me.InviatoDaID)
            writer.WriteAttribute("InviatoDaNome", Me.m_InviatoDaNome)
            writer.WriteAttribute("InviatoIl", Me.m_InviatoIl)
            writer.WriteAttribute("RicevutoDaID", Me.RicevutoDaID)
            writer.WriteAttribute("RicevutoDaNome", Me.m_RicevutoDaNome)
            writer.WriteAttribute("RicevutoIl", Me.m_RicevutoIl)
            writer.WriteAttribute("MezzoDiInvio", Me.m_MezzoDiInvio)
            writer.WriteAttribute("IDFinestraLavorazione", Me.IDFinestraLavorazione)
            writer.WriteAttribute("Esito", Me.m_Esito)
            writer.WriteAttribute("IDCessionario", Me.IDCessionario)
            writer.WriteAttribute("NomeCessionario", Me.m_NomeCessionario)
            writer.WriteAttribute("DurataMesi", Me.m_DurataMesi)
            writer.WriteAttribute("ImportoRata", Me.m_ImportoRata)
            writer.WriteAttribute("TAN", Me.m_TAN)
            writer.WriteAttribute("TAEG", Me.m_TAEG)
            writer.WriteAttribute("DataDecorrenzaPratica", Me.m_DataDecorrenzaPratica)
            writer.WriteAttribute("UltimaScadenza", Me.m_UltimaScadenza)
            writer.WriteAttribute("ATCPIVA", Me.m_ATCPIVA)
            writer.WriteAttribute("ATCDescrizione", Me.m_ATCDescrizione)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDRichiestaF" : Me.m_IDRichiestaDiFinanziamento = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataRichiesta" : Me.m_DataRichiesta = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDIstituto" : Me.m_IDIstituto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeIstituto" : Me.m_NomeIstituto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDAgenziaR" : Me.m_IDAgenziaRichiedente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAgenziaR" : Me.m_NomeAgenziaRichiedente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDAgente" : Me.m_IDAgente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAgente" : Me.m_NomeAgente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataEvasione" : Me.m_DataEvasione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDPratica" : Me.m_IDPratica = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NumeroPratica" : Me.m_NumeroPratica = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDAllegato" : Me.m_IDAllegato = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "PresaInCaricoDaID" : Me.m_PresaInCaricoDaID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "PresaInCaricoDaNome" : Me.m_PresaInCaricoDaNome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataPresaInCarico" : Me.m_DataPresaInCarico = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDCliente" : Me.m_IDCliente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCliente" : Me.m_NomeCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ImportoCE" : Me.m_ImportoCE = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "DataSegnalazione" : Me.m_DataSegnalazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "InviatoDaID" : Me.m_InviatoDaID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "InviatoDaNome" : Me.m_InviatoDaNome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "InviatoIl" : Me.m_InviatoIl = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "RicevutoDaID" : Me.m_RicevutoDaID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "RicevutoDaNome" : Me.m_RicevutoDaNome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "RicevutoIl" : Me.m_RicevutoIl = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "MezzoDiInvio" : Me.m_MezzoDiInvio = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDFinestraLavorazione" : Me.m_IDFinestraLavorazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Esito" : Me.m_Esito = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDCessionario" : Me.m_IDCessionario = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCessionario" : Me.m_NomeCessionario = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DurataMesi" : Me.m_DurataMesi = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ImportoRata" : Me.m_ImportoRata = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TAN" : Me.m_TAN = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TAEG" : Me.m_TAEG = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "DataDecorrenzaPratica" : Me.m_DataDecorrenzaPratica = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "UltimaScadenza" : Me.m_UltimaScadenza = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ATCPIVA" : Me.m_ATCPIVA = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ATCDescrizione" : Me.m_ATCDescrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return "Richiesta di Conteggio Estintivo, Cliente: " & Me.m_NomeCliente & ", Pratica: " & Me.m_NumeroPratica
        End Function

        Protected Overrides Sub OnCreate(e As SystemEvent)
            MyBase.OnCreate(e)
            CQSPD.RichiesteConteggi.doItemCreated(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            MyBase.OnDelete(e)
            CQSPD.RichiesteConteggi.doItemDeleted(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnModified(e As SystemEvent)
            MyBase.OnModified(e)
            CQSPD.RichiesteConteggi.doItemModified(New ItemEventArgs(Me))
        End Sub
    End Class


End Class
