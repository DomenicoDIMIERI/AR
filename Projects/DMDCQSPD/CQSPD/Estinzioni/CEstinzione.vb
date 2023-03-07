Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD



    <Serializable>
    Public Class CEstinzione
        Inherits DBObjectPO
        Implements ICloneable

        Private m_Tipo As TipoEstinzione '[INT]      Un valore intero che indica la tipologia di estinzione
        Private m_IDIstituto As Integer '[INT]      ID dell'istituto con cui il cliente ha stipulato il contratto da estinguere
        <NonSerialized>
        Private m_Istituto As CCQSPDCessionarioClass '[Object]   Oggetto istituto con cui il cliente ha stipulato il contratto
        Private m_NomeIstituto As String '[TEXT]     Nome dell'istituto
        Private m_NomeAgenzia As String
        Private m_NomeFiliale As String
        Private m_Numero As String
        Private m_DataInizio As Date? '[Date]     Data di inizio del prestito
        Private m_Scadenza As Date? '[Date]     Data di scadenza del prestito
        Private m_Rata As Decimal? '[Double]   
        Private m_Durata As Integer? '[INT]      Durata in numero di rate
        Private m_DataEstinzione As Date?
        Private m_TAN As Double? '[Double]    
        Private m_TAEG As Double? 'TAEG

        Private m_NumeroRateInsolute As Integer? '[int] Numero di rate insolute
        Private m_NumeroRateDaPagare As Integer?
        Private m_NumeroRatePagate As Integer?
        Private m_AbbuonoInteressi As Decimal
        Private m_PenaleEstinzione As Double?
        Private m_SpeseAccessorie As Decimal?
        Private m_DecorrenzaPratica As Date?
        Private m_Estinta As Boolean   '[Boolean]  Se vero indica che estingue questo prestito
        Private m_Penale As Double? '[Double] Percentuale da corrispondere come penale per estinzione anticipata

        Private m_IDPratica As Integer          '[INT] ID della pratica che estingue questo prestito (eventuale)
        <NonSerialized>
        Private m_Pratica As CRapportino        '[CRapportino] Pratica che estingue questo prestito (eventuale)
        Private m_Calculated As Boolean

        Private m_IDPersona As Integer
        <NonSerialized>
        Private m_Persona As CPersona
        Private m_NomePersona As String

        Private m_IDEstintoDa As Integer
        <NonSerialized>
        Private m_EstintoDa As CEstinzione

        Private m_DettaglioStato As String

        Private m_SourceType As String  'Tipo dell'oggetto che ha generato questo record
        Private m_SourceID As Integer   'ID dell'oggetto che ha generato questo record

        Private m_Note As String

        Private m_TipoFonte As String
        Private m_IDFonte As Integer
        Private m_Fonte As IFonte
        Private m_NomeFonte As String
        Private m_DataAcquisizione As Date?
        Private m_Validato As Boolean     'Se vero indica che il prestito in corso è stato validato da un operatore
        Private m_ValidatoIl As Date?     'Data e ora di validazione
        Private m_ValidatoDa As CUser     'Utente che ha validato l'operazione
        Private m_IDValidatoDa As Integer 'ID dell'utente che ha validato l'operazione
        Private m_NomeValidatoDa As String 'Nome dell'utente che ha validato l'operazione
        Private m_NomeSorgenteValidazione As String 'Nome del mezzo di validazione
        Private m_TipoSorgenteValidazione As String 'Tipo
        Private m_IDSorgenteValidazione As Integer 'ID 


        Private m_IDClienteXCollaboratore As Integer
        Private m_ClienteXCollaboratore As ClienteXCollaboratore


        Public Sub New()
            Me.m_Tipo = TipoEstinzione.ESTINZIONE_NO
            Me.m_IDIstituto = 0
            Me.m_Istituto = Nothing
            Me.m_NomeIstituto = ""
            Me.m_NomeAgenzia = ""
            Me.m_NomeFiliale = ""

            Me.m_DataInizio = Nothing
            Me.m_Scadenza = Nothing
            Me.m_Rata = Nothing
            Me.m_Durata = Nothing
            Me.m_TAN = Nothing
            Me.m_TAEG = Nothing
            Me.m_Estinta = False
            Me.m_DataEstinzione = Nothing
            Me.m_IDPratica = 0
            Me.m_Pratica = Nothing
            Me.m_NumeroRatePagate = Nothing
            Me.m_Calculated = False
            Me.m_AbbuonoInteressi = Nothing
            Me.m_PenaleEstinzione = Nothing
            Me.m_SpeseAccessorie = Nothing
            Me.m_DecorrenzaPratica = Nothing
            Me.m_IDPersona = 0
            Me.m_Persona = Nothing
            Me.m_NomePersona = ""
            Me.m_EstintoDa = Nothing
            Me.m_IDEstintoDa = 0
            Me.m_DettaglioStato = ""
            Me.m_SourceType = ""
            Me.m_SourceID = 0
            Me.m_Numero = ""
            Me.m_Note = ""

            Me.m_TipoFonte = ""
            Me.m_IDFonte = 0
            Me.m_Fonte = Nothing
            Me.m_NomeFonte = ""
            Me.m_DataAcquisizione = Nothing

            Me.m_Validato = False
            Me.m_ValidatoIl = Nothing
            Me.m_ValidatoDa = Nothing
            Me.m_IDValidatoDa = 0
            Me.m_NomeValidatoDa = ""
            Me.m_NomeSorgenteValidazione = ""
            Me.m_TipoSorgenteValidazione = ""
            Me.m_IDSorgenteValidazione = 0

            Me.m_IDClienteXCollaboratore = 0
            Me.m_ClienteXCollaboratore = Nothing
        End Sub

        Public Property IDClienteXCollaboratore As Integer
            Get
                Return GetID(Me.m_ClienteXCollaboratore, Me.m_IDClienteXCollaboratore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDClienteXCollaboratore
                If (oldValue = value) Then Return
                Me.m_IDClienteXCollaboratore = value
                Me.m_ClienteXCollaboratore = Nothing
                Me.DoChanged("IDClienteXCollaboratore", value, oldValue)
            End Set
        End Property

        Public Property ClienteXCollaboratore As ClienteXCollaboratore
            Get
                If (Me.m_ClienteXCollaboratore Is Nothing) Then Me.m_ClienteXCollaboratore = CQSPD.Collaboratori.ClientiXCollaboratori.GetItemById(Me.m_IDClienteXCollaboratore)
                Return Me.m_ClienteXCollaboratore
            End Get
            Set(value As ClienteXCollaboratore)
                Dim oldValue As ClienteXCollaboratore = Me.m_ClienteXCollaboratore
                If (oldValue Is value) Then Return
                Me.m_ClienteXCollaboratore = value
                Me.m_IDClienteXCollaboratore = GetID(value)
                Me.DoChanged("ClienteXCollaboratore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il prestito è stato validato
        ''' </summary>
        ''' <returns></returns>
        Public Property Validato As Boolean
            Get
                Return Me.m_Validato
            End Get
            Set(value As Boolean)
                If (Me.m_Validato = value) Then Return
                Me.m_Validato = value
                Me.DoChanged("Validato", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di validazione
        ''' </summary>
        ''' <returns></returns>
        Public Property ValidatoIl As Date?
            Get
                Return Me.m_ValidatoIl
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_ValidatoIl
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_ValidatoIl = value
                Me.DoChanged("ValidatoIl", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha validato l'operazione
        ''' </summary>
        ''' <returns></returns>
        Public Property ValidatoDa As CUser
            Get
                If (Me.m_ValidatoDa Is Nothing) Then Me.m_ValidatoDa = Sistema.Users.GetItemById(Me.m_IDValidatoDa)
                Return Me.m_ValidatoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.ValidatoDa
                If (oldValue Is value) Then Return
                Me.m_ValidatoDa = value
                Me.m_IDValidatoDa = GetID(value)
                Me.m_NomeValidatoDa = ""
                If (value IsNot Nothing) Then Me.m_NomeValidatoDa = value.Nominativo
                Me.DoChanged("ValidatoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha validato l'operazione
        ''' </summary>
        ''' <returns></returns>
        Public Property IDValidatoDa As Integer
            Get
                Return GetID(Me.m_ValidatoDa, Me.m_IDValidatoDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDValidatoDa
                If (oldValue = value) Then Return
                Me.m_ValidatoDa = Nothing
                Me.m_IDValidatoDa = value
                Me.DoChanged("IDValidatoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che ha validato l'operazione in corso
        ''' </summary>
        ''' <returns></returns>
        Public Property NomeValidatoDa As String
            Get
                Return Me.m_NomeValidatoDa
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeValidatoDa
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeValidatoDa = value
                Me.DoChanged("NomeValidatoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della sorgente da cui sono stati presi i dati per validare l'oggetto
        ''' </summary>
        ''' <returns></returns>
        Public Property NomeSorgenteValidazione As String
            Get
                Return Me.m_NomeSorgenteValidazione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeSorgenteValidazione
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeSorgenteValidazione = value
                Me.DoChanged("NomeSorgenteValidazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo della sorgente da cui sono stati validati i dati
        ''' </summary>
        ''' <returns></returns>
        Public Property TipoSorgenteValidazione As String
            Get
                Return Me.m_TipoSorgenteValidazione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_TipoSorgenteValidazione
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_TipoSorgenteValidazione = value
                Me.DoChanged("TipoSorgenteValidazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della sorgente di validazione
        ''' </summary>
        ''' <returns></returns>
        Public Property IDSorgenteValidazione As Integer
            Get
                Return Me.m_IDSorgenteValidazione
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_IDSorgenteValidazione
                If (oldValue = value) Then Return
                Me.m_IDSorgenteValidazione = value
                Me.DoChanged("IDSorgenteValidazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo della fonte da cui si sono presi i dati del prestito in corso
        ''' </summary>
        ''' <returns></returns>
        Public Property TipoFonte As String
            Get
                Return Me.m_TipoFonte
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TipoFonte
                If (oldValue = value) Then Return
                Me.m_TipoFonte = value
                Me.DoChanged("TipoFonte", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della fonte da cui si sono presi i dati del prestito in corso
        ''' </summary>
        ''' <returns></returns>
        Public Property IDFonte As Integer
            Get
                Return GetID(Me.m_Fonte, Me.m_IDFonte)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDFonte
                If (oldValue = value) Then Return
                Me.m_IDFonte = value
                Me.m_Fonte = Nothing
                Me.DoChanged("IDFonte", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la fonte
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
                Me.m_IDFonte = GetID(value)
                Me.m_Fonte = value
                Me.m_NomeFonte = ""
                If (value IsNot Nothing) Then Me.m_NomeFonte = value.Nome
                Me.DoChanged("Fonte", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della fonte
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
        ''' Restituisce o imposta la data di acquistizione dei dati dalla fonte specificata
        ''' </summary>
        ''' <returns></returns>
        Public Property DataAcquisizione As Date?
            Get
                Return Me.m_DataAcquisizione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataAcquisizione
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataAcquisizione = value
                Me.DoChanged("DataAcquisizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta delle note aggiuntive sull'estinzione
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
        ''' Restituisce o imposta il codice identificativo del prestito presso l'istituto erogante
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Numero As String
            Get
                Return Me.m_Numero
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Numero
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Numero = value
                Me.DoChanged("Numero", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo dell'oggetto che ha generato questo record
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SourceType As String
            Get
                Return Me.m_SourceType
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_SourceType
                If (oldValue = value) Then Exit Property
                Me.m_SourceType = value
                Me.DoChanged("SourceType", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'oggetto che ha generato questo record
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SourceID As Integer
            Get
                Return Me.m_SourceID
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_SourceID
                If (oldValue = value) Then Exit Property
                Me.m_SourceID = value
                Me.DoChanged("SourceID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che descrive lo stato dell'operazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DettaglioStato As String
            Get
                Return Me.m_DettaglioStato
            End Get
            Set(value As String)
                value = Strings.Left(Strings.Trim(value), 255)
                Dim oldValue As String = Me.m_DettaglioStato
                If (oldValue = value) Then Exit Property
                Me.m_DettaglioStato = value
                Me.DoChanged("DettaglioStato", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce l'ID dell'altro prestito che eventualmente estingue questo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDEstintoDa As Integer
            Get
                Return GetID(Me.m_EstintoDa, Me.m_IDEstintoDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDEstintoDa
                If (oldValue = value) Then Exit Property
                Me.m_IDEstintoDa = value
                Me.m_EstintoDa = Nothing
                Me.DoChanged("EstintoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'altro prestito che eventualmente estingue questo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EstintoDa As CEstinzione
            Get
                If (Me.m_EstintoDa Is Nothing) Then Me.m_EstintoDa = DMD.CQSPD.Estinzioni.GetItemById(Me.m_IDEstintoDa)
                Return Me.m_EstintoDa
            End Get
            Set(value As CEstinzione)
                Dim oldValue As CEstinzione = Me.m_EstintoDa
                If (oldValue Is value) Then Exit Property
                Me.m_EstintoDa = value
                Me.m_IDEstintoDa = GetID(value)
                Me.DoChanged("EstintoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la data in cui è possibile rinnovare il prestito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DataRinnovo As Date?
            Get
                Dim ret As Date? = Nothing
                If (Me.Tipo = TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO OrElse Me.Tipo = TipoEstinzione.ESTINZIONE_PRESTITODELEGA OrElse Me.Tipo = TipoEstinzione.ESTINZIONE_CQP) Then
                    'Dim percDurata As Double = DMD.CQSPD.Configuration.PercCompletamentoRifn
                    If (Me.m_DataInizio.HasValue AndAlso Me.m_Durata.HasValue) Then
                        Dim mesiRinnovo As Integer? = CQSPD.Estinzioni.getMeseRinnovo(Me.m_Durata) 'Math.round(Me.m_Durata.Value * percDurata / 100)
                        If (mesiRinnovo.HasValue = False) Then mesiRinnovo = Math.round(Me.m_Durata.Value * CQSPD.Configuration.PercCompletamentoRifn / 100)
                        ret = DateUtils.DateAdd(DateInterval.Month, mesiRinnovo.Value, DateUtils.GetMonthFirstDay(Me.m_DataInizio))
                    End If
                End If
                Return ret
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la data in cui è possibile rinnovare il prestito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DataRicontatto As Date?
            Get
                Dim ret As Date? = Me.DataRinnovo
                If (ret.HasValue) Then
                    Dim giorniAnticipo As Integer = DMD.CQSPD.Configuration.GiorniAnticipoRifin
                    ret = DateUtils.DateAdd(DateInterval.Day, -giorniAnticipo, ret)
                End If
                Return ret
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della persona a cui appartiene questo oggetto
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
                If (oldValue = value) Then Exit Property
                Me.m_IDPersona = value
                Me.m_Persona = Nothing
                Me.DoChanged("IDPersona", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la persona a cui appartiene questo oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Persona As CPersona
            Get
                If (Me.m_Persona Is Nothing) Then Me.m_Persona = Anagrafica.Persone.GetItemById(Me.m_IDPersona)
                Return Me.m_Persona
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Persona
                If (oldValue Is value) Then Exit Property
                Me.m_Persona = value
                Me.m_IDPersona = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomePersona = value.Nominativo
                Me.DoChanged("Persona", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta il nome della persona a cui appartiene l'oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomePersona As String
            Get
                Return Me.m_NomePersona
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomePersona
                If (oldValue = value) Then Exit Property
                Me.m_NomePersona = value
                Me.DoChanged("NomePersona", value, oldValue)
            End Set
        End Property

        Public Sub Validate()
            If Me.m_Calculated = False Then Me.Calculate()
        End Sub

        Public Sub Invalidate()
            Me.m_Calculated = False
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Estinzioni.Module
        End Function

        Public Property DecorrenzaPratica As Date?
            Get
                Return Me.m_DecorrenzaPratica
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DecorrenzaPratica
                If (oldValue = value) Then Exit Property
                Me.m_DecorrenzaPratica = value
                Me.DoChanged("DecorrenzaPratica", value, oldValue)
            End Set
        End Property

        Public Property PenaleEstinzione As Double?
            Get
                Return Me.m_PenaleEstinzione
            End Get
            Set(value As Double?)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("La penale di estinzione non può assumere un valore negativo")
                Dim oldValue As Double? = Me.m_PenaleEstinzione
                If oldValue = value Then Exit Property
                Me.m_PenaleEstinzione = value
                Me.Invalidate()
                Me.DoChanged("PenaleEstinzione", value, oldValue)
            End Set
        End Property

        Public Property SpeseAccessorie As Decimal?
            Get
                Return Me.m_SpeseAccessorie
            End Get
            Set(value As Decimal?)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("Le spese accessorie non possono assumere un valore negativo")
                Dim oldValue As Decimal? = Me.m_SpeseAccessorie
                If oldValue = value Then Exit Property
                Me.m_SpeseAccessorie = value
                Me.Invalidate()
                Me.DoChanged("SpeseAccessorie", value, oldValue)
            End Set
        End Property

        Public Property NumeroRatePagate As Integer?
            Get
                Return Me.m_NumeroRatePagate
            End Get
            Set(value As Integer?)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("Il numero di rate pagate non può essere negativo")
                Dim oldValue As Integer? = Me.m_NumeroRatePagate
                If oldValue = value Then Exit Property
                Me.m_NumeroRatePagate = value
                Me.Invalidate()
                Me.DoChanged("NumeroRatePagate", value, oldValue)
            End Set
        End Property

        Public Property NumeroRateResidue As Integer?
            Get
                Return Me.m_NumeroRateDaPagare
            End Get
            Set(value As Integer?)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("Il numero di rate residue non può essere negativo")
                Dim oldValue As Integer? = Me.m_NumeroRateDaPagare
                If oldValue = value Then Exit Property
                Me.m_NumeroRateDaPagare = value
                Me.Invalidate()
                Me.DoChanged("NumeroRateResidue", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di rate già maturate ma non  ancora pagate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroRateInsolute As Integer?
            Get
                Return Me.m_NumeroRateInsolute
            End Get
            Set(value As Integer?)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("Il numero di rate insolute non può essere negativo")
                Dim oldValue As Integer? = Me.m_NumeroRateInsolute
                If (oldValue = value) Then Exit Property
                Me.m_NumeroRateInsolute = value
                Me.DoChanged("NumeroRateInsolute", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della pratica che estinge il prestito
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
                If oldValue = value Then Exit Property
                Me.m_Pratica = Nothing
                Me.m_IDPratica = value
                Me.DoChanged("IDPratica", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la pratica che estingue questo prestito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Pratica As CRapportino
            Get
                If Me.m_Pratica Is Nothing Then Me.m_Pratica = DMD.CQSPD.Pratiche.GetItemById(Me.m_IDPratica)
                Return Me.m_Pratica
            End Get
            Set(value As CRapportino)
                Dim oldValue As CRapportino = Me.Pratica
                If (oldValue = value) Then Exit Property
                Me.m_Pratica = value
                Me.m_IDPratica = GetID(value)
                Me.DoChanged("Pratica", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetPratica(ByVal value As CRapportino)
            Me.m_Pratica = value
            Me.m_IDPratica = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il tipo del prestito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Tipo As TipoEstinzione
            Get
                Return Me.m_Tipo
            End Get
            Set(value As TipoEstinzione)
                Dim oldValue As TipoEstinzione = Me.m_Tipo
                If (oldValue = value) Then Exit Property
                Me.m_Tipo = value
                Me.DoChanged("Tipo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'istituto che ha erogato il prestito
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
                If oldValue = value Then Exit Property
                Me.m_IDIstituto = value
                Me.m_Istituto = Nothing
                Me.DoChanged("IDIstituto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'istituto che ha erogato il prestito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Istituto As CCQSPDCessionarioClass
            Get
                If (Me.m_Istituto Is Nothing) Then Me.m_Istituto = DMD.CQSPD.Cessionari.GetItemById(Me.m_IDIstituto)
                Return Me.m_Istituto
            End Get
            Set(value As CCQSPDCessionarioClass)
                Dim oldValue As CCQSPDCessionarioClass = Me.Istituto
                If (oldValue = value) Then Exit Property
                Me.m_Istituto = value
                Me.m_IDIstituto = GetID(value)
                If Not (value Is Nothing) Then Me.m_NomeIstituto = value.Nome
                Me.DoChanged("Istituto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'istituto che ha erogato il prestito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeIstituto As String
            Get
                Return Me.m_NomeIstituto
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeIstituto
                If (oldValue = value) Then Exit Property
                Me.m_NomeIstituto = value
                Me.DoChanged("NomeIstituto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'agenzia
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeAgenzia As String
            Get
                Return Me.m_NomeAgenzia
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeAgenzia
                If (oldValue = value) Then Exit Property
                Me.m_NomeAgenzia = value
                Me.DoChanged("NomeAgenzia", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della filiale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeFiliale As String
            Get
                Return Me.m_NomeFiliale
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeFiliale
                If (oldValue = value) Then Exit Property
                Me.m_NomeFiliale = value
                Me.DoChanged("NomeFiliale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la data di decorrenza dell'altro prestito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataInizio As Date?
            Get
                Return Me.m_DataInizio
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataInizio
                If oldValue = value Then Exit Property
                Me.m_DataInizio = value
                Me.Invalidate()
                Me.DoChanged("DataInizio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data dell'ultima rata dell'altro prestito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Scadenza As Date?
            Get
                Return Me.m_Scadenza
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_Scadenza
                If (oldValue = value) Then Exit Property
                If Me.m_Scadenza = value Then Exit Property
                Me.m_Scadenza = value
                Me.Invalidate()
                Me.DoChanged("Scadenza", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il valore della rata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Rata As Decimal?
            Get
                Return Me.m_Rata
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_Rata
                If oldValue = value Then Exit Property
                Me.m_Rata = value
                Me.Invalidate()
                Me.DoChanged("Rata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la durata in mesi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Durata As Integer?
            Get
                Return Me.m_Durata
            End Get
            Set(value As Integer?)
                Dim oldValue As Integer? = Me.m_Durata
                If oldValue = value Then Exit Property
                Me.m_Durata = value
                Me.Invalidate()
                Me.DoChanged("Durata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il TAN
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TAN As Double?
            Get
                Return Me.m_TAN
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_TAN
                If oldValue = value Then Exit Property
                Me.m_TAN = value
                Me.Invalidate()
                Me.DoChanged("TAN", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il TAEG
        ''' </summary>
        ''' <returns></returns>
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

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il prestito è estinto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Estinta As Boolean
            Get
                Return Me.m_Estinta
            End Get
            Set(value As Boolean)
                If (Me.m_Estinta = value) Then Exit Property
                Me.m_Estinta = value
                Me.DoChanged("Estinta", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data in cui questo prestito è stato eventualmente estinto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataEstinzione As Date?
            Get
                Return Me.m_DataEstinzione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataEstinzione
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataEstinzione = value
                Me.DoChanged("DataEstinzione", value, oldValue)
            End Set
        End Property




        Public ReadOnly Property DebitoIniziale As Decimal?
            Get
                If Me.m_Rata.HasValue Then Return Me.m_Rata.Value * Me.m_Durata
                Return Nothing
            End Get
            'Set(value As Decimal?)
            '    Dim oldValue As Decimal? = Me.DebitoIniziale
            '    If (oldValue = value) Then Exit Property
            '    Me.m_Rata = value / Me.m_Durata
            '    Me.DoChanged("DebitoIniziale", value, oldValue)
            'End Set
        End Property

        Public ReadOnly Property DebitoResiduo As Decimal?
            Get
                If (Me.m_NumeroRatePagate.HasValue) Then
                    Return Me.DebitoResiduo(Me.m_NumeroRatePagate)
                Else
                    Return Nothing
                End If
            End Get
            'Set(value As Decimal?)
            '    Dim oldValue As Decimal? = Me.DebitoResiduo
            '    If (oldValue = value) Then Exit Property
            '    Me.m_NumeroRatePagate = (Me.DebitoIniziale - value) / Me.m_Rata.Value
            '    Me.DoChanged("DebitoResiduo", value, oldValue)
            'End Set
        End Property

        Public ReadOnly Property DebitoResiduo(ByVal numeroRatePagate As Integer) As Decimal?
            Get
                Dim ret As Decimal? = Me.DebitoIniziale
                If (ret.HasValue AndAlso Me.m_Rata.HasValue AndAlso Me.m_NumeroRatePagate.HasValue) Then
                    ret = ret.Value - Me.m_Rata.Value * Me.m_NumeroRatePagate.Value
                End If
                Return ret
            End Get
            'Set(value As Decimal?)
            '    Dim oldValue As Decimal? = Me.DebitoResiduo
            '    If (oldValue = value) Then Exit Property
            '    Me.m_NumeroRatePagate = (Me.DebitoIniziale - value) / Me.m_Rata.Value
            '    Me.DoChanged("DebitoResiduo", value, oldValue)
            'End Set
        End Property

        Public ReadOnly Property AbbuonoInteressi As Decimal
            Get
                Me.Validate()
                Return Me.m_AbbuonoInteressi
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il capitale da rimborsare 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CapitaleDaRimborsare As Decimal?
            Get
                Dim ret As Decimal? = Me.DebitoResiduo
                If (ret.HasValue) Then ret = ret.Value - Me.m_AbbuonoInteressi
                Return ret
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il valore della penale di estinzione calcolata sul capitale da rimborsare
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ValorePenale As Decimal?
            Get
                Dim ret As Decimal? = Me.CapitaleDaRimborsare
                If (ret.HasValue AndAlso Me.m_PenaleEstinzione.HasValue) Then ret = ret * Me.m_PenaleEstinzione.Value / 100
                Return ret
            End Get
        End Property

        Public ReadOnly Property TotaleDaRimborsare() As Decimal?
            Get
                Return Math.Sum({Me.CapitaleDaRimborsare, Me.ValorePenale, Me.SpeseAccessorie, Me.ValoreQuoteInsolute})
            End Get
        End Property

        Public ReadOnly Property TotaleDaRimborsare(ByVal numeroQuoteScadute As Integer) As Decimal?
            Get
                Return Math.Sum({Me.CapitaleDaRimborsare, Me.ValorePenale, Me.SpeseAccessorie, Me.ValoreQuoteInsolute})
            End Get
        End Property

        Public ReadOnly Property TotaleDaRimborsare(ByVal numeroQuoteScadute As Integer, ByVal numeroQuoteInsolute As Integer) As Decimal?
            Get
                Return Math.Sum({Me.CapitaleDaRimborsare, Me.ValorePenale, Me.SpeseAccessorie, Me.ValoreQuoteInsolute})
            End Get
        End Property

        Public ReadOnly Property ValoreQuoteInsolute As Decimal?
            Get
                Me.Validate()
                If (Me.m_Rata.HasValue AndAlso Me.m_NumeroRateInsolute.HasValue) Then Return Me.m_NumeroRateInsolute * Me.m_Rata.Value
                Return Nothing
            End Get
        End Property


        Public Function Calculate() As Boolean
            'Dim Dk As Decimal   'Debito residuo per k=1..n-1
            Dim Ik As Decimal 'Interesse in ciascun periodo k=1...n
            Dim k As Integer
            Dim i As Double

            'Me.m_DebitoIniziale = Me.m_Rata.Value * Me.m_Durata
            'Me.m_DebitoResiduo = Me.m_DebitoIniziale - Me.m_Quota * Me.m_NumeroRatePagate

            i = (Me.m_TAN / 100) / 12

            Me.m_AbbuonoInteressi = 0
            If (Me.m_NumeroRateDaPagare.HasValue AndAlso Me.m_Rata.HasValue) Then
                For k = 1 To Me.m_NumeroRateDaPagare.Value ' Me.m_NumeroRatePagate + 1 To Me.m_Durata
                    Ik = Me.m_Rata.Value * (1 - 1 / ((1 + i) ^ k)) ' (Me.m_Durata - k + 1)))
                    Me.m_AbbuonoInteressi = Me.m_AbbuonoInteressi + Ik
                Next
            End If

            Me.m_Calculated = True

            Return True
        End Function


        '---------------------------------------------------------------
        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("m_Tipo", Me.m_Tipo)
            writer.WriteAttribute("m_IDIstituto", Me.IDIstituto)
            writer.WriteAttribute("m_NomeIstituto", Me.m_NomeIstituto)
            writer.WriteAttribute("NomeFiliale", Me.m_NomeFiliale)
            writer.WriteAttribute("m_DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("m_Scadenza", Me.m_Scadenza)
            writer.WriteAttribute("m_Rata", Me.m_Rata)
            writer.WriteAttribute("m_Durata", Me.m_Durata)
            writer.WriteAttribute("m_TAN", Me.m_TAN)
            writer.WriteAttribute("TAEG", Me.m_TAEG)
            writer.WriteAttribute("Estinta", Me.m_Estinta)
            writer.WriteAttribute("IDPratica", Me.IDPratica)
            writer.WriteAttribute("NumeroRatePagate", Me.m_NumeroRatePagate)
            writer.WriteAttribute("NumeroRateResidue", Me.m_NumeroRateDaPagare)
            writer.WriteAttribute("NumeroRateInsolute", Me.m_NumeroRateInsolute)
            writer.WriteAttribute("Calculated", Me.m_Calculated)
            writer.WriteAttribute("AbbuonoInteressi", Me.m_AbbuonoInteressi)
            writer.WriteAttribute("PenaleEstinzione", Me.m_PenaleEstinzione)
            writer.WriteAttribute("SpeseAccessorie", Me.m_SpeseAccessorie)
            writer.WriteAttribute("DecorrenzaPratica", Me.m_DecorrenzaPratica)
            writer.WriteAttribute("IDPersona", Me.IDPersona)
            writer.WriteAttribute("NomePersona", Me.m_NomePersona)
            writer.WriteAttribute("IDEstintoDa", Me.IDEstintoDa)
            writer.WriteAttribute("DettaglioStato", Me.m_DettaglioStato)
            writer.WriteAttribute("SourceType", Me.m_SourceType)
            writer.WriteAttribute("SourceID", Me.m_SourceID)
            writer.WriteAttribute("Numero", Me.m_Numero)
            writer.WriteAttribute("DataEstinzione", Me.m_DataEstinzione)
            writer.WriteAttribute("NomeAgenzia", Me.m_NomeAgenzia)
            writer.WriteAttribute("TipoFonte", Me.m_TipoFonte)
            writer.WriteAttribute("IDFonte", Me.IDFonte)
            writer.WriteAttribute("NomeFonte", Me.m_NomeFonte)
            writer.WriteAttribute("DataAcquisizione", Me.m_DataAcquisizione)

            writer.WriteAttribute("Validato", Me.m_Validato)
            writer.WriteAttribute("ValidatoIl", Me.m_ValidatoIl)
            writer.WriteAttribute("IDValidatoDa", Me.m_IDValidatoDa)
            writer.WriteAttribute("NomeValidatoDa", Me.m_NomeValidatoDa)
            writer.WriteAttribute("NomeSorgenteValidazione", Me.m_NomeSorgenteValidazione)
            writer.WriteAttribute("TipoSorgenteValidazione", Me.m_TipoSorgenteValidazione)
            writer.WriteAttribute("IDSorgenteValidazione", Me.IDSorgenteValidazione)

            writer.WriteAttribute("IDClienteXCollaboratore", Me.IDClienteXCollaboratore)

            MyBase.XMLSerialize(writer)
            writer.WriteTag("Note", Me.m_Note)
        End Sub


        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "m_Tipo" : Me.m_Tipo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "m_IDIstituto" : Me.m_IDIstituto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "m_NomeIstituto" : Me.m_NomeIstituto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeFiliale" : Me.m_NomeFiliale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "m_Scadenza" : m_Scadenza = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "m_Rata" : Me.m_Rata = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "m_Durata" : Me.m_Durata = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "m_TAN" : Me.m_TAN = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "TAEG" : Me.m_TAEG = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Estinta" : Me.m_Estinta = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "DataEstinzione" : Me.m_DataEstinzione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDPratica" : Me.m_IDPratica = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NumeroRateInsolute" : Me.m_NumeroRateInsolute = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NumeroRatePagate" : Me.m_NumeroRatePagate = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Calculated" : Me.m_Calculated = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "AbbuonoInteressi" : Me.m_AbbuonoInteressi = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "PenaleEstinzione" : Me.m_PenaleEstinzione = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "SpeseAccessorie" : Me.m_SpeseAccessorie = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "DecorrenzaPratica" : Me.m_DecorrenzaPratica = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDPersona" : Me.m_IDPersona = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePersona" : Me.m_NomePersona = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDEstintoDa" : Me.m_IDEstintoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NumeroRateResidue" : Me.m_NumeroRateDaPagare = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DettaglioStato" : Me.m_DettaglioStato = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SourceType" : Me.m_SourceType = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SourceID" : Me.m_SourceID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Numero" : Me.m_Numero = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeAgenzia" : Me.m_NomeAgenzia = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Note" : Me.m_Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoFonte" : Me.m_TipoFonte = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDFonte" : Me.m_IDFonte = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeFonte" : Me.m_NomeFonte = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataAcquisizione" : Me.m_DataAcquisizione = XML.Utils.Serializer.DeserializeDate(fieldValue)

                Case "Validato" : Me.m_Validato = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "ValidatoIl" : Me.m_ValidatoIl = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDValidatoDa" : Me.m_IDValidatoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeValidatoDa" : Me.m_NomeValidatoDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeSorgenteValidazione" : Me.m_NomeSorgenteValidazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoSorgenteValidazione" : Me.m_TipoSorgenteValidazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDSorgenteValidazione" : Me.m_IDSorgenteValidazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDClienteXCollaboratore" : Me.m_IDClienteXCollaboratore = XML.Utils.Serializer.DeserializeInteger(fieldValue)

                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function GetTableName() As String
            Return "tbl_Estinzioni"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Tipo = reader.Read("Tipo", Me.m_Tipo)
            Me.m_IDIstituto = reader.Read("Istituto", Me.m_IDIstituto)
            Me.m_NomeIstituto = reader.Read("NomeIstituto", Me.m_NomeIstituto)
            Me.m_NomeFiliale = reader.Read("NomeFiliale", Me.m_NomeFiliale)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataEstinzione = reader.Read("DataEstinzione", Me.m_DataEstinzione)
            Me.m_Scadenza = reader.Read("Scadenza", Me.m_Scadenza)
            Me.m_Rata = reader.Read("Rata", Me.m_Rata)
            Me.m_Durata = reader.Read("Durata", Me.m_Durata)
            Me.m_TAN = reader.Read("TAN", Me.m_TAN)
            Me.m_TAEG = reader.Read("TAEG", Me.m_TAEG)
            Me.m_Estinta = reader.Read("Estingue", Me.m_Estinta)
            Me.m_IDPratica = reader.Read("IDPratica", Me.m_IDPratica)
            Me.m_NumeroRatePagate = reader.Read("NumeroRatePagate", Me.m_NumeroRatePagate)
            Me.m_NumeroRateInsolute = reader.Read("NumeroRateResidue", Me.m_NumeroRateInsolute)
            Me.m_Calculated = reader.Read("Calculated", Me.m_Calculated)
            Me.m_AbbuonoInteressi = reader.Read("AbbuonoInteressi", Me.m_AbbuonoInteressi)
            Me.m_PenaleEstinzione = reader.Read("PenaleEstinzione", Me.m_PenaleEstinzione)
            Me.m_SpeseAccessorie = reader.Read("SpeseAccessorie", Me.m_SpeseAccessorie)
            Me.m_DecorrenzaPratica = reader.Read("DecorrenzaPratica", Me.m_DecorrenzaPratica)
            Me.m_IDPersona = reader.Read("IDPersona", Me.m_IDPersona)
            Me.m_NomePersona = reader.Read("NomePersona", Me.m_NomePersona)
            Me.m_IDEstintoDa = reader.Read("IDEstintoDa", Me.m_IDEstintoDa)
            Me.m_NumeroRateDaPagare = reader.Read("NumeroRateDaPagare", Me.m_NumeroRateDaPagare)
            Me.m_DettaglioStato = reader.Read("DettaglioStato", Me.m_DettaglioStato)
            Me.m_SourceType = reader.Read("SourceType", Me.m_SourceType)
            Me.m_SourceID = reader.Read("SourceID", Me.m_SourceID)
            Me.m_Numero = reader.Read("Numero", Me.m_Numero)
            Me.m_NomeAgenzia = reader.Read("NomeAgenzia", Me.m_NomeAgenzia)
            Me.m_Note = reader.Read("Note", Me.m_Note)

            Me.m_TipoFonte = reader.Read("TipoFonte", Me.m_TipoFonte)
            Me.m_IDFonte = reader.Read("IDFonte", Me.m_IDFonte)
            Me.m_NomeFonte = reader.Read("NomeFonte", Me.m_NomeFonte)
            Me.m_DataAcquisizione = reader.Read("DataAcquisizione", Me.m_DataAcquisizione)

            Me.m_Validato = reader.Read("Validato", Me.m_Validato)
            Me.m_ValidatoIl = reader.Read("ValidatoIl", Me.m_ValidatoIl)
            Me.m_IDValidatoDa = reader.Read("IDValidatoDa", Me.m_IDValidatoDa)
            Me.m_NomeValidatoDa = reader.Read("NomeValidatoDa", Me.m_NomeValidatoDa)
            Me.m_NomeSorgenteValidazione = reader.Read("NomeSorgenteValidazione", Me.m_NomeSorgenteValidazione)
            Me.m_TipoSorgenteValidazione = reader.Read("TipoSorgenteValidazione", Me.m_TipoSorgenteValidazione)
            Me.m_IDSorgenteValidazione = reader.Read("IDSorgenteValidazione", Me.m_IDSorgenteValidazione)

            Me.m_IDClienteXCollaboratore = reader.Read("IDClienteXCollaboratore", Me.m_IDClienteXCollaboratore)

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Tipo", Me.m_Tipo)
            writer.Write("Istituto", Me.IDIstituto)
            writer.Write("NomeIstituto", Me.m_NomeIstituto)
            writer.Write("NomeFiliale", Me.m_NomeFiliale)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("Scadenza", Me.m_Scadenza)
            writer.Write("Rata", Me.m_Rata)
            writer.Write("Durata", Me.m_Durata)
            writer.Write("TAN", Me.m_TAN)
            writer.Write("TAEG", Me.m_TAEG)
            writer.Write("Estingue", Me.m_Estinta)
            writer.Write("DataEstinzione", Me.m_DataEstinzione)
            writer.Write("IDPratica", Me.IDPratica)
            writer.Write("NumeroRatePagate", Me.m_NumeroRatePagate)
            writer.Write("NumeroRateResidue", Me.m_NumeroRateInsolute)
            writer.Write("Calculated", Me.m_Calculated)
            writer.Write("AbbuonoInteressi", Me.m_AbbuonoInteressi)
            writer.Write("PenaleEstinzione", Me.m_PenaleEstinzione)
            writer.Write("SpeseAccessorie", Me.m_SpeseAccessorie)
            writer.Write("DecorrenzaPratica", Me.m_DecorrenzaPratica)
            writer.Write("IDPersona", Me.IDPersona)
            writer.Write("NomePersona", Me.m_NomePersona)
            writer.Write("IDEstintoDa", Me.IDEstintoDa)
            writer.Write("NumeroRateDaPagare", Me.m_NumeroRateDaPagare)
            writer.Write("DettaglioStato", Me.m_DettaglioStato)
            writer.Write("SourceType", Me.m_SourceType)
            writer.Write("SourceID", Me.m_SourceID)
            writer.Write("Numero", Me.m_Numero)
            writer.Write("NomeAgenzia", Me.m_NomeAgenzia)
            writer.Write("Note", Me.m_Note)
            writer.Write("DataRinnovo", Me.DataRinnovo)
            writer.Write("DataRicontatto", Me.DataRicontatto)

            writer.Write("TipoFonte", Me.m_TipoFonte)
            writer.Write("IDFonte", Me.IDFonte)
            writer.Write("NomeFonte", Me.m_NomeFonte)
            writer.Write("DataAcquisizione", Me.m_DataAcquisizione)

            writer.Write("Validato", Me.m_Validato)
            writer.Write("ValidatoIl", Me.m_ValidatoIl)
            writer.Write("IDValidatoDa", Me.m_IDValidatoDa)
            writer.Write("NomeValidatoDa", Me.m_NomeValidatoDa)
            writer.Write("NomeSorgenteValidazione", Me.m_NomeSorgenteValidazione)
            writer.Write("TipoSorgenteValidazione", Me.m_TipoSorgenteValidazione)
            writer.Write("IDSorgenteValidazione", Me.IDSorgenteValidazione)

            writer.Write("IDClienteXCollaboratore", Me.IDClienteXCollaboratore)

            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return DMD.CQSPD.Estinzioni.FormatTipo(Me.Tipo) & ", Rata: " & Formats.FormatValuta(Me.Rata) & ", Rate residue: " & Me.NumeroRateResidue
        End Function

        Public Function InCorso() As Boolean
            Return Not Me.m_Estinta AndAlso Me.InCorso(DateUtils.Now)
        End Function

        ''' <summary>
        ''' Restituisce vero se il questo prestito risultava in corso alla data indicata (cioè se la data è compresa tra l'inizio e la fine o, nel caso di prestiti estinti, alla data di scadenza
        ''' </summary>
        ''' <param name="allaData"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function InCorso(ByVal allaData As Date) As Boolean
            If (Me.m_Estinta AndAlso Me.m_DataEstinzione.HasValue) Then
                Return DateUtils.CheckBetween(allaData, Me.m_DataInizio, Me.m_DataEstinzione)
            Else
                Return DateUtils.CheckBetween(allaData, Me.m_DataInizio, Me.m_Scadenza)
            End If
        End Function

        Protected Friend Sub SetPersona(ByVal value As CPersona)
            Me.m_Persona = value
            Me.m_IDPersona = GetID(value)
            Me.m_NomePersona = value.Nominativo
        End Sub

        Protected Overrides Sub OnCreate(e As SystemEvent)
            'Me.ProgrammaRicontatto()
            MyBase.OnCreate(e)
            CQSPD.Estinzioni.doItemCreated(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnModified(e As SystemEvent)
            'Me.ProgrammaRicontatto()
            MyBase.OnModified(e)
            CQSPD.Estinzioni.doItemModified(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            MyBase.OnDelete(e)
            'Ricontatti.AnnullaRicontattoBySource(Me)
            CQSPD.Estinzioni.doItemDeleted(New ItemEventArgs(Me))
        End Sub

        ''' <summary>
        ''' Programma il ricontatto opportuno
        ''' </summary>
        ''' <remarks></remarks>
        Public Function ProgrammaRicontatto() As CRicontatto
            Dim percRinnovo As Single
            Dim giorniAnticipo As Integer
            Dim dataRicontatto As Date? = Me.DataRicontatto

            Dim r As CRicontatto = Ricontatti.GetRicontattoBySource(Me)
            If (r IsNot Nothing AndAlso r.StatoRicontatto = StatoRicontatto.EFFETTUATO) Then Return r

            If (Me.IDPersona = 0 OrElse Me.Estinta) Then
                If (r IsNot Nothing) Then
                    r.DettaglioStato = "Annullato perché estinto da " & Me.IDEstintoDa
                    r.StatoRicontatto = StatoRicontatto.ANNULLATO
                    r.Save()
                End If
            ElseIf (Me.NumeroRateResidue <= 0) OrElse (Me.Scadenza.HasValue AndAlso Me.Scadenza.Value < DateUtils.ToDay) Then
                If (r IsNot Nothing) Then
                    r.DettaglioStato = "Annullato perché scaduto il " & Formats.FormatUserDate(Me.Scadenza)
                    r.StatoRicontatto = StatoRicontatto.ANNULLATO
                    r.Save()
                End If
            ElseIf dataRicontatto.HasValue Then
                percRinnovo = CQSPD.Configuration.PercCompletamentoRifn
                giorniAnticipo = CQSPD.Configuration.GiorniAnticipoRifin
                'motivo = Formats.FormatInteger(percRinnovo) & "% del prestito " & CQSPD.Estinzioni.FormatTipo(Me.m_Tipo) & " di " & Me.m_NomePersona
                If (r IsNot Nothing) Then
                    r.DettaglioStato = "Riprogrammato in seguito alla modifica del " & Formats.FormatUserDateTime(Now)
                ElseIf (Me.Persona IsNot Nothing) Then
                    'r = Ricontatti.ProgrammaRicontatto(Me.Persona, dataRicontatto, motivo, TypeName(Me), GetID(Me), nomeLista, Me.Persona.PuntoOperativo, Nothing)
                    r = New CRicontatto
                    'item.PuntoOperativo = persona.PuntoOperativo
                    r.DettaglioStato = "Programmato"
                End If
                r.DataPrevista = dataRicontatto
                r.Persona = Me.Persona
                r.SourceName = TypeName(Me)
                r.SourceParam = GetID(Me)
                r.Stato = ObjectStatus.OBJECT_VALID
                r.StatoRicontatto = StatoRicontatto.PROGRAMMATO
                'r.NomeLista = ""
                r.GiornataIntera = False
                r.Flags = SetFlag(r.Flags, RicontattoFlags.Reserved, True)
                If (Me.Persona IsNot Nothing) Then r.PuntoOperativo = Me.Persona.PuntoOperativo
                r.Operatore = Sistema.Users.CurrentUser
                r.AssegnatoA = Nothing
                'r.Produttore = Anagrafica.Aziende.AziendaPrincipale
                If (Me.m_Tipo = TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO OrElse Me.m_Tipo = TipoEstinzione.ESTINZIONE_CQP) Then
                    r.Note = "RINNOVO CQS"
                ElseIf (Me.m_Tipo = TipoEstinzione.ESTINZIONE_PRESTITODELEGA) Then
                    r.Note = "RINNOVO PD"
                End If
                r.Save()
            Else
                If (r IsNot Nothing) Then
                    r.DettaglioStato = "Annullato perché non si tratta di un prestito rinnovabile"
                    r.StatoRicontatto = StatoRicontatto.ANNULLATO
                    r.Save()
                End If
            End If

            Return r
        End Function

        Public Function IsInCorso(ByVal allaData As Date) As Boolean ' = function () {
            If (Me.m_DataEstinzione.HasValue) Then
                Return DateUtils.CheckBetween(allaData, Me.m_DataInizio, Me.m_DataEstinzione)
            Else
                Return DateUtils.CheckBetween(allaData, Me.m_DataInizio, Me.m_Scadenza)
            End If
        End Function

        Public Function IsInCorso() As Boolean ' = function () {
            Return Me.IsInCorso(DateUtils.Now)
        End Function

        Public Function IsInCorsoOFutura() As Boolean
            Return Me.IsInCorsoOFutura(DateUtils.Now)
        End Function

        Public Function IsInCorsoOFutura(ByVal allaData As Date) As Boolean
            Const TOLLERANZA_ESTINZIONE As Integer = 1 'anno
            Dim di As Date? = Me.m_DataInizio
            Dim df As Date? = Me.m_Scadenza
            If (df.HasValue = False AndAlso di.HasValue) Then df = DateUtils.GetLastMonthDay(DateUtils.DateAdd(DateInterval.Month, 120, di))
            If (di.HasValue) Then di = DateUtils.DateAdd(DateInterval.Year, -TOLLERANZA_ESTINZIONE, di)
            If (df.HasValue) Then df = DateUtils.DateAdd(DateInterval.Year, +TOLLERANZA_ESTINZIONE, df)
            If (Me.m_Estinta AndAlso Me.m_DataEstinzione.HasValue) Then df = Me.m_DataEstinzione
            Dim ret As Boolean = DateUtils.CheckBetween(allaData, di, df)
            Return ret
        End Function

        Public Function GetNumeroRateResidue() As Integer?
            If (Me.m_Estinta) Then
                If (Me.m_DataEstinzione.HasValue) Then
                    Return Me.GetNumeroRateResidue(Me.m_DataEstinzione)
                Else
                    Return Nothing
                End If
            Else
                Return Me.GetNumeroRateResidue(DateUtils.Now)
            End If
        End Function

        Public Function GetNumeroRateResidue(ByVal al As Date) As Integer?
            If (Me.m_Scadenza.HasValue) Then
                Return Math.Max(0, DateUtils.DateDiff("M", al, Me.m_Scadenza.Value))
            Else
                Return Nothing
            End If
        End Function


        Public Overrides Function Equals(obj As Object) As Boolean
            If Not (TypeOf (obj) Is CEstinzione) Then Return False
            Dim e As CEstinzione = obj
            Return (Me.IDIstituto = e.IDIstituto) AndAlso _
                   (DateUtils.Compare(Me.DataInizio, e.DataInizio) = 0) AndAlso _
                   (Me.Durata = e.Durata) AndAlso _
                   (Me.IDPersona = e.IDPersona) AndAlso _
                   (Me.Tipo = e.Tipo) AndAlso _
                   (Me.Rata = e.Rata)
        End Function

        Public Function Clone() As Object Implements ICloneable.Clone
            Return Me.MemberwiseClone
        End Function
    End Class





End Class