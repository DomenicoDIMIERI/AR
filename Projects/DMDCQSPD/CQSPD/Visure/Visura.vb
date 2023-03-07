Imports DMD.Databases

Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Public Class CQSPD

    Public Enum StatoVisura As Integer
        DA_RICHIEDERE = 0
        RICHIESTA = 1
        RITIRATA = 3
        ANNULLATA = 4
        RIFIUTATA = 2
    End Enum



    ''' <summary>
    ''' Rappresenta una richiesta di visura/censimento
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class Visura
        Inherits DBObjectPO

        Private m_Data As Date                      'Data ed ora della richiesta
        Private m_Richiedente As CUser                'Utente che ha effettuato la richiesta
        Private m_IDRichiedente As Integer            'ID del richiedente che ha effettuato la richiesta
        Private m_NomeRichiedente As String           'Nome del richiedente che ha effettuato la richiesta

        Private m_PresaInCaricoDa As CUser
        Private m_IDPresaInCaricoDa As Integer
        Private m_NomePresaInCaricoDa As String

        Private m_DataPresaInCarico As Nullable(Of Date)
        Private m_DataCompletamento As Nullable(Of Date)

        Private m_StatoVisura As StatoVisura

        Private m_ValutazioneAmministrazione As Boolean
        Private m_CensimentoDatoreDiLavoro As Boolean
        Private m_CensimentoSedeOperativa As Boolean
        Private m_VariazioneDenominazione As Boolean
        Private m_Sblocco As Boolean

        Private m_IDAmministrazione As Integer
        Private m_Amministrazione As CAzienda

        Private m_RagioneSociale As String
        Private m_OggettoSociale As String
        Private m_CodiceFiscale As String
        Private m_PartitaIVA As String
        Private m_ResponsabileDaContattare As String
        Private m_Qualifica As String
        Private m_Indirizzo As CIndirizzo
        Private m_Telefono As String
        Private m_Fax As String
        Private m_IndirizzoeMail As String
        Private m_IndirizzoDiNotifica As CIndirizzo
        Private m_TelefonoDiNotifica As String
        Private m_FaxDiNotifica As String
        Private m_ConvenzionePresente As Boolean
        Private m_CodiceODescrizioneConvenzione As String
        Private m_NumeroDipendenti As Nullable(Of Integer)
        Private m_AmministrazioneSottoscriveMODPREST_008 As Boolean
        Private m_NoteOInfoSullaSocieta As String

        Private m_IDBustaPaga As Integer
        Private m_BustaPaga As CAttachment
        Private m_IDMotivoRichiestaSblocco As Integer
        Private m_MotivoRichiestaSblocco As CAttachment
        Private m_AltriAllegati As CAttachments
        Private m_DocumentiProdotti As CAttachments

        Private m_CodiceAmministrazioneCL As String
        Private m_StatoAmministrazioneCL As String

        Private m_CodiceDatoreLavoroCL As String
        Private m_RagioneSocialeSOP As String
        Private m_ResponsabileDaContattareSOP As String
        Private m_QualificaSOP As String
        Private m_IndirizzoSO As CIndirizzo
        Private m_TelefonoSO As String
        Private m_FaxSO As String
        Private m_ConvenzionePresenteSO As Boolean
        Private m_CodiceODescrizioneConvenzioneSO As String
        Private m_IDBustaPagaSO As Integer
        Private m_BustaPagaSO As CAttachment
        Private m_AltriAllegatiSO As CAttachments

        Public Sub New()
            Me.m_Data = Nothing

            Me.m_Richiedente = Nothing
            Me.m_IDRichiedente = 0
            Me.m_NomeRichiedente = vbNullString

            Me.m_PresaInCaricoDa = Nothing
            Me.m_IDPresaInCaricoDa = 0
            Me.m_NomePresaInCaricoDa = vbNullString

            Me.m_DataPresaInCarico = Nothing
            Me.m_DataCompletamento = Nothing

            Me.m_StatoVisura = StatoVisura.DA_RICHIEDERE

            Me.m_ValutazioneAmministrazione = False
            Me.m_CensimentoDatoreDiLavoro = False
            Me.m_CensimentoSedeOperativa = False
            Me.m_VariazioneDenominazione = False
            Me.m_Sblocco = False

            Me.m_IDAmministrazione = 0
            Me.m_Amministrazione = Nothing

            Me.m_RagioneSociale = vbNullString
            Me.m_OggettoSociale = vbNullString
            Me.m_CodiceFiscale = vbNullString
            Me.m_PartitaIVA = vbNullString
            Me.m_ResponsabileDaContattare = vbNullString
            Me.m_Qualifica = vbNullString
            Me.m_Indirizzo = New CIndirizzo()
            Me.m_Telefono = vbNullString
            Me.m_Fax = vbNullString
            Me.m_IndirizzoeMail = vbNullString
            Me.m_IndirizzoDiNotifica = New CIndirizzo
            Me.m_TelefonoDiNotifica = vbNullString
            Me.m_FaxDiNotifica = vbNullString
            Me.m_ConvenzionePresente = False
            Me.m_CodiceODescrizioneConvenzione = vbNullString
            Me.m_NumeroDipendenti = Nothing
            Me.m_AmministrazioneSottoscriveMODPREST_008 = False
            Me.m_NoteOInfoSullaSocieta = vbNullString

            Me.m_IDBustaPaga = 0
            Me.m_BustaPaga = Nothing
            Me.m_IDMotivoRichiestaSblocco = 0
            Me.m_MotivoRichiestaSblocco = Nothing
            Me.m_AltriAllegati = Nothing
            Me.m_DocumentiProdotti = Nothing

            Me.m_CodiceAmministrazioneCL = vbNullString
            Me.m_StatoAmministrazioneCL = vbNullString

            Me.m_CodiceDatoreLavoroCL = vbNullString
            Me.m_RagioneSocialeSOP = vbNullString
            Me.m_ResponsabileDaContattareSOP = vbNullString
            Me.m_QualificaSOP = vbNullString
            Me.m_IndirizzoSO = New CIndirizzo
            Me.m_TelefonoSO = vbNullString
            Me.m_FaxSO = vbNullString
            Me.m_ConvenzionePresenteSO = False
            Me.m_CodiceODescrizioneConvenzioneSO = vbNullString
            Me.m_IDBustaPagaSO = 0
            Me.m_BustaPagaSO = Nothing
            Me.m_AltriAllegatiSO = Nothing
        End Sub

        Public Overrides Sub InitializeFrom(value As Object)
            MyBase.InitializeFrom(value)
            Me.m_Richiedente = Users.CurrentUser
            Me.m_IDRichiedente = GetID(Me.m_Richiedente)
            Me.m_NomeRichiedente = Me.m_Richiedente.Nominativo
            Me.m_StatoVisura = StatoVisura.DA_RICHIEDERE
            Me.m_DataCompletamento = Nothing
            Me.m_DataPresaInCarico = Nothing
            Me.m_PresaInCaricoDa = Nothing
            Me.m_IDPresaInCaricoDa = 0
            Me.m_NomePresaInCaricoDa = vbNullString
            Me.m_AltriAllegati = Nothing
            Me.m_Data = Now
        End Sub

        Public Property CodiceDatoreLavoroCL As String
            Get
                Return Me.m_CodiceDatoreLavoroCL
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_CodiceDatoreLavoroCL
                If (value = value) Then Exit Property
                Me.m_CodiceDatoreLavoroCL = value
                Me.DoChanged("CodiceDatoreLavoroCL", value, oldValue)
            End Set
        End Property

        Public Property RagioneSocialeSOP As String
            Get
                Return Me.m_RagioneSocialeSOP
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_RagioneSocialeSOP
                If (oldValue = value) Then Exit Property
                Me.m_RagioneSocialeSOP = value
                Me.DoChanged("RagioneSocialeSOP", value, oldValue)
            End Set
        End Property

        Public Property ResponsabileDaContattareSOP As String
            Get
                Return Me.m_ResponsabileDaContattareSOP
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ResponsabileDaContattareSOP
                If (oldValue = value) Then Exit Property
                Me.m_ResponsabileDaContattareSOP = value
                Me.DoChanged("ResponsabileDaContattareSOP", value, oldValue)
            End Set
        End Property

        Public Property QualificaSOP As String
            Get
                Return Me.m_QualificaSOP
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_QualificaSOP
                If (oldValue = value) Then Exit Property
                Me.m_QualificaSOP = value
                Me.DoChanged("QualificaSOP", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property IndirizzoSO As CIndirizzo
            Get
                Return Me.m_IndirizzoSO
            End Get
        End Property

        Public Property TelefonoSO As String
            Get
                Return Me.m_TelefonoSO
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TelefonoSO
                If (oldValue = value) Then Exit Property
                Me.m_TelefonoSO = value
                Me.DoChanged("TelefonoSO", value, oldValue)
            End Set
        End Property

        Public Property FaxSO As String
            Get
                Return Me.m_FaxSO
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_FaxSO
                If (oldValue = value) Then Exit Property
                Me.m_FaxSO = value
                Me.DoChanged("FaxSO", value, oldValue)
            End Set
        End Property

        Public Property ConvenzionePresenteSO As Boolean
            Get
                Return Me.m_ConvenzionePresenteSO
            End Get
            Set(value As Boolean)
                If (Me.m_ConvenzionePresenteSO = value) Then Exit Property
                Me.m_ConvenzionePresenteSO = value
                Me.DoChanged("ConvenzionePresenteSO", value, Not value)
            End Set
        End Property

        Public Property CodiceODescrizioneConvenzioneSO As String
            Get
                Return Me.m_CodiceODescrizioneConvenzioneSO
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_CodiceODescrizioneConvenzioneSO
                If (oldValue = value) Then Exit Property
                Me.m_CodiceODescrizioneConvenzioneSO = value
                Me.DoChanged("CodiceODescrizioneConvenzioneSO", value, oldValue)
            End Set
        End Property

        Public Property IDBustaPagaSO As Integer
            Get
                Return GetID(Me.m_BustaPagaSO, Me.m_IDBustaPagaSO)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDBustaPagaSO
                If (oldValue = value) Then Exit Property
                Me.m_IDBustaPagaSO = value
                Me.m_BustaPagaSO = Nothing
                Me.DoChanged("IDBustaPagaSO", value, oldValue)
            End Set
        End Property

        Public Property BustaPagaSO As CAttachment
            Get
                If (Me.m_BustaPagaSO Is Nothing) Then Me.m_BustaPagaSO = Attachments.GetItemById(Me.m_IDBustaPagaSO)
                Return Me.m_BustaPagaSO
            End Get
            Set(value As CAttachment)
                Dim oldValue As CAttachment = Me.m_BustaPagaSO
                If (oldValue Is value) Then Exit Property
                Me.m_BustaPagaSO = value
                Me.m_IDBustaPagaSO = GetID(value)
                Me.DoChanged("IDBustaPagaSO", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property AltriAllegatiSO As CAttachments
            Get
                If (Me.m_AltriAllegatiSO Is Nothing) Then Me.m_AltriAllegatiSO = New CAttachments(Me, "AltriAllegatiSO", 0)
                Return Me.m_AltriAllegatiSO
            End Get
        End Property

        Public Property CodiceAmministrazioneCL As String
            Get
                Return Me.m_CodiceAmministrazioneCL
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_CodiceAmministrazioneCL
                If (oldValue = value) Then Exit Property
                Me.m_CodiceAmministrazioneCL = value
                Me.DoChanged("CodiceAmministrazioneCL", value, oldValue)
            End Set
        End Property

        Public Property StatoAmministrazioneCL As String
            Get
                Return Me.m_StatoAmministrazioneCL
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_StatoAmministrazioneCL
                If (oldValue = value) Then Exit Property
                Me.m_StatoAmministrazioneCL = value
                Me.DoChanged("StatoAmministrazioneCL", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data della richiesta
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
        ''' Restituisce o imposta l'utente che ha effettuato la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Richiedente As CUser
            Get
                If Me.m_Richiedente Is Nothing Then Me.m_Richiedente = Users.GetItemById(Me.m_IDRichiedente)
                Return Me.m_Richiedente
            End Get
            Set(value As CUser)
                Dim oldValue As Object = Me.m_Richiedente
                If (oldValue Is value) Then Exit Property
                Me.m_Richiedente = value
                Me.m_IDRichiedente = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeRichiedente = value.Nominativo
                Me.DoChanged("Richiedente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del richiedente a cui è stata assegnata la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDRichiedente As Integer
            Get
                Return GetID(Me.m_Richiedente, Me.m_IDRichiedente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRichiedente
                If (oldValue = value) Then Exit Property
                Me.m_IDRichiedente = value
                Me.m_Richiedente = Nothing
                Me.DoChanged("IDRichiedente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del richiedente che ha effettuato la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeRichiedente As String
            Get
                Return Me.m_NomeRichiedente
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeRichiedente
                If (oldValue = value) Then Exit Property
                Me.m_NomeRichiedente = value
                Me.DoChanged("NomeRichiedente", value, oldValue)
            End Set
        End Property

        Public Property PresaInCaricoDa As CUser
            Get
                If (Me.m_PresaInCaricoDa Is Nothing) Then Me.m_PresaInCaricoDa = Sistema.Users.GetItemById(Me.m_IDPresaInCaricoDa)
                Return Me.m_PresaInCaricoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_PresaInCaricoDa
                If (oldValue Is value) Then Exit Property
                Me.m_PresaInCaricoDa = value
                Me.m_IDPresaInCaricoDa = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomePresaInCaricoDa = value.Nominativo
                Me.DoChanged("PresaInCaricoDa", value, oldValue)
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

        Public Property NomePresaInCaricoDa As String
            Get
                Return Me.m_NomePresaInCaricoDa
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomePresaInCaricoDa
                If (oldValue = value) Then Exit Property
                Me.m_NomePresaInCaricoDa = value
                Me.DoChanged("NomePresaInCaricoDa", value, oldValue)
            End Set
        End Property

        Public Property DataPresaInCarico As Nullable(Of Date)
            Get
                Return Me.m_DataPresaInCarico
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataPresaInCarico
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataPresaInCarico = value
                Me.DoChanged("DataPresaInCarico", value, oldValue)
            End Set
        End Property

        Public Property DataCompletamento As Nullable(Of Date)
            Get
                Return Me.m_DataCompletamento
            End Get
            Set(value As Nullable(Of Date))
                Dim oldvalue As Nullable(Of Date) = Me.m_DataCompletamento
                If (DateUtils.Compare(value, oldvalue) = 0) Then Return
                Me.m_DataCompletamento = value
                Me.DoChanged("DataCompletamento", value, oldvalue)
            End Set
        End Property

        Public Property StatoVisura As StatoVisura
            Get
                Return Me.m_StatoVisura
            End Get
            Set(value As StatoVisura)
                Dim oldValue As StatoVisura = Me.m_StatoVisura
                If (oldValue = value) Then Exit Property
                Me.m_StatoVisura = value
                Me.DoChanged("StatoVisura", value, oldValue)
            End Set
        End Property

        Public Property ValutazioneAmministrazione As Boolean
            Get
                Return Me.m_ValutazioneAmministrazione
            End Get
            Set(value As Boolean)
                If (Me.m_ValutazioneAmministrazione = value) Then Exit Property
                Me.m_ValutazioneAmministrazione = value
                Me.DoChanged("ValutazioneAmministrazione", value, Not value)
            End Set
        End Property

        Public Property CensimentoDatoreDiLavoro As Boolean
            Get
                Return Me.m_CensimentoDatoreDiLavoro
            End Get
            Set(value As Boolean)
                If (Me.m_CensimentoDatoreDiLavoro = value) Then Exit Property
                Me.m_CensimentoDatoreDiLavoro = value
                Me.DoChanged("CensimentoDatoreDiLavoro", value, Not value)
            End Set
        End Property

        Public Property CensimentoSedeOperativa As Boolean
            Get
                Return Me.m_CensimentoSedeOperativa
            End Get
            Set(value As Boolean)
                If (Me.m_CensimentoSedeOperativa = value) Then Exit Property
                Me.m_CensimentoSedeOperativa = value
                Me.DoChanged("CensimentoSedeOperativa", value, Not value)
            End Set
        End Property

        Public Property VariazioneDenominazione As Boolean
            Get
                Return Me.m_VariazioneDenominazione
            End Get
            Set(value As Boolean)
                If (Me.m_VariazioneDenominazione = value) Then Exit Property
                Me.m_VariazioneDenominazione = value
                Me.DoChanged("VariazioneDenominazione", value, Not value)
            End Set
        End Property

        Public Property Sblocco As Boolean
            Get
                Return Me.m_Sblocco
            End Get
            Set(value As Boolean)
                If (Me.m_Sblocco = value) Then Exit Property
                Me.m_Sblocco = value
                Me.DoChanged("Sblocco", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'amministrazione a cui si è stata inviata la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Amministrazione As CAzienda
            Get
                If (Me.m_Amministrazione Is Nothing) Then Me.m_Amministrazione = Anagrafica.Aziende.GetItemById(Me.m_IDAmministrazione)
                Return Me.m_Amministrazione
            End Get
            Set(value As CAzienda)
                Dim oldValue As CAzienda = Me.m_Amministrazione
                If (oldValue = value) Then Exit Property
                Me.m_Amministrazione = value
                Me.m_IDAmministrazione = GetID(value)
                If (value IsNot Nothing) Then
                    Me.m_RagioneSociale = value.Nominativo

                End If
                Me.DoChanged("Amministrazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'amministrazione a cui è stata fatta la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDAmministrazione As Integer
            Get
                Return GetID(Me.m_Amministrazione, Me.m_IDAmministrazione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAmministrazione
                If (oldValue = value) Then Exit Property
                Me.m_IDAmministrazione = value
                Me.m_Amministrazione = Nothing
                Me.DoChanged("IDAmministrazione", value, oldValue)
            End Set
        End Property

        Public Property RagioneSociale As String
            Get
                Return Me.m_RagioneSociale
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_RagioneSociale
                If (oldValue = value) Then Exit Property
                Me.m_RagioneSociale = value
                Me.DoChanged("RagioneSociale", value, oldValue)
            End Set
        End Property

        Public Property OggettoSociale As String
            Get
                Return Me.m_OggettoSociale
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_OggettoSociale
                If (oldValue = value) Then Exit Property
                Me.m_OggettoSociale = value
                Me.DoChanged("OggettoSociale", value, oldValue)
            End Set
        End Property

        Public Property CodiceFiscale As String
            Get
                Return Me.m_CodiceFiscale
            End Get
            Set(value As String)
                value = Formats.ParseCodiceFiscale(value)
                Dim oldValue As String = Me.m_CodiceFiscale
                If (oldValue = value) Then Exit Property
                Me.m_CodiceFiscale = value
                Me.DoChanged("CodiceFiscale", value, oldValue)
            End Set
        End Property

        Public Property PartitaIVA As String
            Get
                Return Me.m_PartitaIVA
            End Get
            Set(value As String)
                value = Formats.ParsePartitaIVA(value)
                Dim oldValue As String = Me.m_PartitaIVA
                If (oldValue = value) Then Exit Property
                Me.m_PartitaIVA = value
                Me.DoChanged("PartitaIVA", value, oldValue)
            End Set
        End Property

        Public Property ResponsabileDaContattare As String
            Get
                Return Me.m_ResponsabileDaContattare
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ResponsabileDaContattare
                If (oldValue = value) Then Exit Property
                Me.m_ResponsabileDaContattare = value
                Me.DoChanged("ResponsabileDaContattare", value, oldValue)
            End Set
        End Property

        Public Property Qualifica As String
            Get
                Return Me.m_Qualifica
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Qualifica
                If (oldValue = value) Then Exit Property
                Me.m_Qualifica = value
                Me.DoChanged("Qualifica", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Indirizzo As CIndirizzo
            Get
                Return Me.m_Indirizzo
            End Get
        End Property

        Public Property Telefono As String
            Get
                Return Me.m_Telefono
            End Get
            Set(value As String)
                value = Formats.ParsePhoneNumber(value)
                Dim oldValue As String = Me.m_Telefono
                If (oldValue = value) Then Exit Property
                Me.m_Telefono = value
                Me.DoChanged("Telefono", value, oldValue)
            End Set
        End Property

        Public Property Fax As String
            Get
                Return Me.m_Fax
            End Get
            Set(value As String)
                value = Formats.ParsePhoneNumber(value)
                Dim oldValue As String = Me.m_Fax
                If (oldValue = value) Then Exit Property
                Me.m_Fax = value
                Me.DoChanged("Fax", value, oldValue)
            End Set
        End Property

        Public Property IndirizzoeMail As String
            Get
                Return Me.m_IndirizzoeMail
            End Get
            Set(value As String)
                value = Formats.ParseEMailAddress(value)
                Dim oldValue As String = Me.m_IndirizzoeMail
                Me.m_IndirizzoeMail = value
                Me.DoChanged("IndirizzoeMail", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property IndirizzoDiNotifica As CIndirizzo
            Get
                Return Me.m_IndirizzoDiNotifica
            End Get
        End Property

        Public Property TelefonoDiNotifica As String
            Get
                Return Me.m_TelefonoDiNotifica
            End Get
            Set(value As String)
                value = Formats.ParsePhoneNumber(value)
                Dim oldValue As String = Me.m_TelefonoDiNotifica
                Me.m_TelefonoDiNotifica = value
                Me.DoChanged("TelefonoDiNotifica", value, oldValue)
            End Set
        End Property

        Public Property FaxDiNotifica As String
            Get
                Return Me.m_FaxDiNotifica
            End Get
            Set(value As String)
                value = Formats.ParsePhoneNumber(value)
                Dim oldValue As String = Me.m_FaxDiNotifica
                If (oldValue = value) Then Exit Property
                Me.m_FaxDiNotifica = value
                Me.DoChanged("FaxDiNotifica", value, oldValue)
            End Set
        End Property

        Public Property ConvenzionePresente As Boolean
            Get
                Return Me.m_ConvenzionePresente
            End Get
            Set(value As Boolean)
                If (Me.m_ConvenzionePresente = value) Then Exit Property
                Me.m_ConvenzionePresente = value
                Me.DoChanged("ConvenzionePresente", value, Not value)
            End Set
        End Property

        Public Property CodiceODescrizioneConvenzione As String
            Get
                Return Me.m_CodiceODescrizioneConvenzione
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_CodiceODescrizioneConvenzione
                If (oldValue = value) Then Exit Property
                Me.m_CodiceODescrizioneConvenzione = value
                Me.DoChanged("CodiceODescrizioneConvenzione", value, oldValue)
            End Set
        End Property

        Public Property NumeroDipendenti As Nullable(Of Integer)
            Get
                Return Me.m_NumeroDipendenti
            End Get
            Set(value As Nullable(Of Integer))
                Dim oldValue As Nullable(Of Integer) = Me.m_NumeroDipendenti
                If (oldValue = value) Then Exit Property
                Me.m_NumeroDipendenti = value
                Me.DoChanged("NumeroDipendenti", value, oldValue)
            End Set
        End Property

        Public Property AmministrazioneSottoscriveMODPREST_008 As Boolean
            Get
                Return Me.m_AmministrazioneSottoscriveMODPREST_008
            End Get
            Set(value As Boolean)
                If (Me.m_AmministrazioneSottoscriveMODPREST_008 = value) Then Exit Property
                Me.m_AmministrazioneSottoscriveMODPREST_008 = value
                Me.DoChanged("AmministrazioneSottoscriveMODPREST_008", value, Not value)
            End Set
        End Property

        Public Property NoteOInfoSullaSocieta As String
            Get
                Return Me.m_NoteOInfoSullaSocieta
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NoteOInfoSullaSocieta
                If (oldValue = value) Then Exit Property
                Me.m_NoteOInfoSullaSocieta = value
                Me.DoChanged("NoteOInfoSullaSocieta", value, oldValue)
            End Set
        End Property

        Public Property IDBustaPaga As Integer
            Get
                Return GetID(Me.m_BustaPaga, Me.m_IDBustaPaga)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDBustaPaga
                If (oldValue = value) Then Exit Property
                Me.m_IDBustaPaga = value
                Me.m_BustaPaga = Nothing
                Me.DoChanged("IDBustaPaga", value, oldValue)
            End Set
        End Property

        Public Property BustaPaga As CAttachment
            Get
                If (Me.m_BustaPaga Is Nothing) Then Me.m_BustaPaga = Attachments.GetItemById(Me.m_IDBustaPaga)
                Return Me.m_BustaPaga
            End Get
            Set(value As CAttachment)
                Dim oldValue As CAttachment = Me.m_BustaPaga
                If (oldValue Is value) Then Exit Property
                Me.m_BustaPaga = value
                Me.m_IDBustaPaga = GetID(value)
                Me.DoChanged("BustaPaga", value, oldValue)
            End Set
        End Property

        Public Property IDMotivoRichiestaSblocco As Integer
            Get
                Return GetID(Me.m_MotivoRichiestaSblocco, Me.m_IDMotivoRichiestaSblocco)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDMotivoRichiestaSblocco
                If (oldValue = value) Then Exit Property
                Me.m_IDMotivoRichiestaSblocco = value
                Me.m_MotivoRichiestaSblocco = Nothing
                Me.DoChanged("IDMotivoRichiestaSblocco", value, oldValue)
            End Set
        End Property

        Public Property MotivoRichiestaSblocco As CAttachment
            Get
                If (Me.m_MotivoRichiestaSblocco Is Nothing) Then Me.m_MotivoRichiestaSblocco = Attachments.GetItemById(Me.m_IDMotivoRichiestaSblocco)
                Return Me.m_MotivoRichiestaSblocco
            End Get
            Set(value As CAttachment)
                Dim oldValue As CAttachment = Me.m_MotivoRichiestaSblocco
                If (oldValue Is value) Then Exit Property
                Me.m_MotivoRichiestaSblocco = value
                Me.m_IDMotivoRichiestaSblocco = GetID(value)
                Me.DoChanged("MotivoRichiestaSblocco", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property AltriAllegati As CAttachments
            Get
                If (Me.m_AltriAllegati Is Nothing) Then Me.m_AltriAllegati = New CAttachments(Me, "AltriAllegati", 0)
                Return Me.m_AltriAllegati
            End Get
        End Property

        Public ReadOnly Property DocumentiProdotti As CAttachments
            Get
                If (Me.m_DocumentiProdotti Is Nothing) Then Me.m_DocumentiProdotti = New CAttachments(Me, "DocumentiProdotti", 0)
                Return Me.m_DocumentiProdotti
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return DMD.CQSPD.Visure.Module
        End Function

        Public Overrides Function IsChanged() As Boolean
            Return MyBase.IsChanged() OrElse Me.m_Indirizzo.IsChanged OrElse Me.m_IndirizzoDiNotifica.IsChanged OrElse Me.m_IndirizzoSO.IsChanged
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDVisure"
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret) Then
                Me.m_Indirizzo.SetChanged(False)
                Me.m_IndirizzoDiNotifica.SetChanged(False)
                Me.m_IndirizzoSO.SetChanged(False)
            End If
            Return ret
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Data", Me.m_Data)
            writer.Write("IDRichiedente", Me.IDRichiedente)
            writer.Write("NomeRichiedente", Me.m_NomeRichiedente)
            writer.Write("IDPresaInCaricoDa", Me.IDPresaInCaricoDa)
            writer.Write("NomePresaInCaricoDa", Me.m_NomePresaInCaricoDa)
            writer.Write("DataPresaInCarico", Me.m_DataPresaInCarico)
            writer.Write("DataCompletamento", Me.m_DataCompletamento)
            writer.Write("StatoVisura", Me.m_StatoVisura)
            writer.Write("VALAMM", Me.m_ValutazioneAmministrazione)
            writer.Write("CENSDATLAV", Me.m_CensimentoDatoreDiLavoro)
            writer.Write("CENSSEDOP", Me.m_CensimentoSedeOperativa)
            writer.Write("VARIAZDENOM", Me.m_VariazioneDenominazione)
            writer.Write("SBLOCCO", Me.m_Sblocco)
            writer.Write("IDAmministrazione", Me.IDAmministrazione)
            writer.Write("RagioneSociale", Me.m_RagioneSociale)
            writer.Write("OggettoSociale", Me.m_OggettoSociale)
            writer.Write("CodiceFiscale", Me.m_CodiceFiscale)
            writer.Write("PartitaIVA", Me.m_PartitaIVA)
            writer.Write("NomeResponsabile", Me.m_ResponsabileDaContattare)
            writer.Write("QualificaResponsabile", Me.m_Qualifica)
            writer.Write("Indirizzo_Provincia", Me.m_Indirizzo.Provincia)
            writer.Write("Indirizzo_Citta", Me.m_Indirizzo.Citta)
            writer.Write("Indirizzo_CAP", Me.m_Indirizzo.CAP)
            writer.Write("Indirizzo_Via", Me.m_Indirizzo.ToponimoViaECivico)
            writer.Write("Telefono", Me.m_Telefono)
            writer.Write("Fax", Me.m_Fax)
            writer.Write("eMail", Me.m_IndirizzoeMail)
            writer.Write("IndirizzoN_Provincia", Me.m_IndirizzoDiNotifica.Provincia)
            writer.Write("IndirizzoN_Citta", Me.m_IndirizzoDiNotifica.Citta)
            writer.Write("IndirizzoN_CAP", Me.m_IndirizzoDiNotifica.CAP)
            writer.Write("IndirizzoN_Via", Me.m_IndirizzoDiNotifica.ToponimoViaECivico)
            writer.Write("TelefonoN", Me.m_TelefonoDiNotifica)
            writer.Write("FaxN", Me.m_FaxDiNotifica)
            writer.Write("CONVSINO", Me.m_ConvenzionePresente)
            writer.Write("CODCONV", Me.m_CodiceODescrizioneConvenzione)
            writer.Write("NumeroDipendenti", Me.m_NumeroDipendenti)
            writer.Write("AMMMODPRST008", Me.m_AmministrazioneSottoscriveMODPREST_008)
            writer.Write("NoteSocieta", Me.m_NoteOInfoSullaSocieta)
            writer.Write("IDBustaPaga", Me.IDBustaPaga)
            writer.Write("IDMotivoSblocco", Me.IDMotivoRichiestaSblocco)
            writer.Write("CODAMMCL", Me.m_CodiceAmministrazioneCL)
            writer.Write("STATOAMMCL", Me.m_StatoAmministrazioneCL)

            writer.Write("CODDLAVCL", Me.m_CodiceDatoreLavoroCL)
            writer.Write("RAGSOCSOP", Me.m_RagioneSocialeSOP)
            writer.Write("RESPCONTSOP", Me.m_ResponsabileDaContattareSOP)
            writer.Write("QUALIFSOP", Me.m_QualificaSOP)
            writer.Write("IndirizzoSO_Provincia", Me.m_IndirizzoSO.Provincia)
            writer.Write("IndirizzoSO_Citta", Me.m_IndirizzoSO.Citta)
            writer.Write("IndirizzoSO_CAP", Me.m_IndirizzoSO.CAP)
            writer.Write("IndirizzoSO_Via", Me.m_IndirizzoSO.ToponimoViaECivico)
            writer.Write("TelefonoSO", Me.m_TelefonoSO)
            writer.Write("FaxSO", Me.m_FaxSO)
            writer.Write("CONVSINOSO", Me.m_ConvenzionePresenteSO)
            writer.Write("CODCONVSO", Me.m_CodiceODescrizioneConvenzioneSO)
            writer.Write("IDBustaPagaSO", Me.IDBustaPagaSO)

            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Data = reader.Read("Data", Me.m_Data)
            Me.m_IDRichiedente = reader.Read("IDRichiedente", Me.m_IDRichiedente)
            Me.m_NomeRichiedente = reader.Read("NomeRichiedente", Me.m_NomeRichiedente)
            Me.m_IDPresaInCaricoDa = reader.Read("IDPresaInCaricoDa", Me.m_IDPresaInCaricoDa)
            Me.m_NomePresaInCaricoDa = reader.Read("NomePresaInCaricoDa", Me.m_NomePresaInCaricoDa)
            Me.m_DataPresaInCarico = reader.Read("DataPresaInCarico", Me.m_DataPresaInCarico)
            Me.m_DataCompletamento = reader.Read("DataCompletamento", Me.m_DataCompletamento)
            Me.m_StatoVisura = reader.Read("StatoVisura", Me.m_StatoVisura)
            Me.m_ValutazioneAmministrazione = reader.Read("VALAMM", Me.m_ValutazioneAmministrazione)
            Me.m_CensimentoDatoreDiLavoro = reader.Read("CENSDATLAV", Me.m_CensimentoDatoreDiLavoro)
            Me.m_CensimentoSedeOperativa = reader.Read("CENSSEDOP", Me.m_CensimentoSedeOperativa)
            Me.m_VariazioneDenominazione = reader.Read("VARIAZDENOM", Me.m_VariazioneDenominazione)
            Me.m_Sblocco = reader.Read("SBLOCCO", Me.m_Sblocco)
            Me.m_IDAmministrazione = reader.Read("IDAmministrazione", Me.m_IDAmministrazione)
            Me.m_RagioneSociale = reader.Read("RagioneSociale", Me.m_RagioneSociale)
            Me.m_OggettoSociale = reader.Read("OggettoSociale", Me.m_OggettoSociale)
            Me.m_CodiceFiscale = reader.Read("CodiceFiscale", Me.m_CodiceFiscale)
            Me.m_PartitaIVA = reader.Read("PartitaIVA", Me.m_PartitaIVA)
            Me.m_ResponsabileDaContattare = reader.Read("NomeResponsabile", Me.m_ResponsabileDaContattare)
            Me.m_Qualifica = reader.Read("QualificaResponsabile", Me.m_Qualifica)

            Me.m_Indirizzo.Provincia = reader.Read("Indirizzo_Provincia", Me.m_Indirizzo.Provincia)
            Me.m_Indirizzo.Citta = reader.Read("Indirizzo_Citta", Me.m_Indirizzo.Citta)
            Me.m_Indirizzo.CAP = reader.Read("Indirizzo_CAP", Me.m_Indirizzo.CAP)
            Me.m_Indirizzo.ToponimoViaECivico = reader.Read("Indirizzo_Via", Me.m_Indirizzo.ToponimoViaECivico)
            Me.m_Indirizzo.SetChanged(False)

            Me.m_Telefono = reader.Read("Telefono", Me.m_Telefono)
            Me.m_Fax = reader.Read("Fax", Me.m_Fax)
            Me.m_IndirizzoeMail = reader.Read("eMail", Me.m_IndirizzoeMail)

            Me.m_IndirizzoDiNotifica.Provincia = reader.Read("IndirizzoN_Provincia", Me.m_IndirizzoDiNotifica.Provincia)
            Me.m_IndirizzoDiNotifica.Citta = reader.Read("IndirizzoN_Citta", Me.m_IndirizzoDiNotifica.Citta)
            Me.m_IndirizzoDiNotifica.CAP = reader.Read("IndirizzoN_CAP", Me.m_IndirizzoDiNotifica.CAP)
            Me.m_IndirizzoDiNotifica.ToponimoViaECivico = reader.Read("IndirizzoN_Via", Me.m_IndirizzoDiNotifica.ToponimoViaECivico)
            Me.m_IndirizzoDiNotifica.SetChanged(False)

            Me.m_TelefonoDiNotifica = reader.Read("TelefonoN", Me.m_TelefonoDiNotifica)
            Me.m_FaxDiNotifica = reader.Read("FaxN", Me.m_FaxDiNotifica)
            Me.m_ConvenzionePresente = reader.Read("CONVSINO", Me.m_ConvenzionePresente)
            Me.m_CodiceODescrizioneConvenzione = reader.Read("CODCONV", Me.m_CodiceODescrizioneConvenzione)
            Me.m_NumeroDipendenti = reader.Read("NumeroDipendenti", Me.m_NumeroDipendenti)
            Me.m_AmministrazioneSottoscriveMODPREST_008 = reader.Read("AMMMODPRST008", Me.m_AmministrazioneSottoscriveMODPREST_008)
            Me.m_NoteOInfoSullaSocieta = reader.Read("NoteSocieta", Me.m_NoteOInfoSullaSocieta)
            Me.m_IDBustaPaga = reader.Read("IDBustaPaga", Me.m_IDBustaPaga)
            Me.m_IDMotivoRichiestaSblocco = reader.Read("IDMotivoSblocco", Me.m_IDMotivoRichiestaSblocco)
            Me.m_CodiceAmministrazioneCL = reader.Read("CODAMMCL", Me.m_CodiceAmministrazioneCL)
            Me.m_StatoAmministrazioneCL = reader.Read("STATOAMMCL", Me.m_StatoAmministrazioneCL)

            Me.m_CodiceDatoreLavoroCL = reader.Read("CODDLAVCL", Me.m_CodiceDatoreLavoroCL)
            Me.m_RagioneSocialeSOP = reader.Read("RAGSOCSOP", Me.m_RagioneSocialeSOP)
            Me.m_ResponsabileDaContattareSOP = reader.Read("RESPCONTSOP", Me.m_ResponsabileDaContattareSOP)
            Me.m_QualificaSOP = reader.Read("QUALIFSOP", Me.m_QualificaSOP)
            Me.m_IndirizzoSO.Provincia = reader.Read("IndirizzoSO_Provincia", Me.m_IndirizzoSO.Provincia)
            Me.m_IndirizzoSO.Citta = reader.Read("IndirizzoSO_Citta", Me.m_IndirizzoSO.Citta)
            Me.m_IndirizzoSO.CAP = reader.Read("IndirizzoSO_CAP", Me.m_IndirizzoSO.CAP)
            Me.m_IndirizzoSO.ToponimoViaECivico = reader.Read("IndirizzoSO_Via", Me.m_IndirizzoSO.ToponimoViaECivico)
            Me.m_IndirizzoSO.SetChanged(False)

            Me.m_TelefonoSO = reader.Read("TelefonoSO", Me.m_TelefonoSO)
            Me.m_FaxSO = reader.Read("FaxSO", Me.m_FaxSO)
            Me.m_ConvenzionePresenteSO = reader.Read("CONVSINOSO", Me.m_ConvenzionePresenteSO)
            Me.m_CodiceODescrizioneConvenzioneSO = reader.Read("CODCONVSO", Me.m_CodiceODescrizioneConvenzioneSO)
            Me.m_IDBustaPagaSO = reader.Read("IDBustaPagaSO", Me.m_IDBustaPagaSO)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Data", Me.m_Data)
            writer.WriteAttribute("IDRichiedente", Me.IDRichiedente)
            writer.WriteAttribute("NomeRichiedente", Me.m_NomeRichiedente)
            writer.WriteAttribute("IDPresaInCaricoDa", Me.IDPresaInCaricoDa)
            writer.WriteAttribute("NomePresaInCaricoDa", Me.m_NomePresaInCaricoDa)
            writer.WriteAttribute("DataPresaInCarico", Me.m_DataPresaInCarico)
            writer.WriteAttribute("DataCompletamento", Me.m_DataCompletamento)
            writer.WriteAttribute("StatoVisura", Me.m_StatoVisura)
            writer.WriteAttribute("VALAMM", Me.m_ValutazioneAmministrazione)
            writer.WriteAttribute("CENSDATLAV", Me.m_CensimentoDatoreDiLavoro)
            writer.WriteAttribute("CENSSEDOP", Me.m_CensimentoSedeOperativa)
            writer.WriteAttribute("VARIAZDENOM", Me.m_VariazioneDenominazione)
            writer.WriteAttribute("SBLOCCO", Me.m_Sblocco)
            writer.WriteAttribute("IDAmministrazione", Me.IDAmministrazione)
            writer.WriteAttribute("RagioneSociale", Me.m_RagioneSociale)
            writer.WriteAttribute("OggettoSociale", Me.m_OggettoSociale)
            writer.WriteAttribute("CodiceFiscale", Me.m_CodiceFiscale)
            writer.WriteAttribute("PartitaIVA", Me.m_PartitaIVA)
            writer.WriteAttribute("NomeResponsabile", Me.m_ResponsabileDaContattare)
            writer.WriteAttribute("QualificaResponsabile", Me.m_Qualifica)
            writer.WriteAttribute("Telefono", Me.m_Telefono)
            writer.WriteAttribute("Fax", Me.m_Fax)
            writer.WriteAttribute("eMail", Me.m_IndirizzoeMail)
            writer.WriteAttribute("TelefonoN", Me.m_TelefonoDiNotifica)
            writer.WriteAttribute("FaxN", Me.m_FaxDiNotifica)
            writer.WriteAttribute("CONVSINO", Me.m_ConvenzionePresente)
            writer.WriteAttribute("CODCONV", Me.m_CodiceODescrizioneConvenzione)
            writer.WriteAttribute("NumeroDipendenti", Me.m_NumeroDipendenti)
            writer.WriteAttribute("AMMMODPRST008", Me.m_AmministrazioneSottoscriveMODPREST_008)
            writer.WriteAttribute("NoteSocieta", Me.m_NoteOInfoSullaSocieta)
            writer.WriteAttribute("IDBustaPaga", Me.IDBustaPaga)
            writer.WriteAttribute("IDMotivoSblocco", Me.IDMotivoRichiestaSblocco)
            writer.WriteAttribute("CODAMMCL", Me.m_CodiceAmministrazioneCL)
            writer.WriteAttribute("STATOAMMCL", Me.m_StatoAmministrazioneCL)
            writer.WriteAttribute("CODDLAVCL", Me.m_CodiceDatoreLavoroCL)
            writer.WriteAttribute("RAGSOCSOP", Me.m_RagioneSocialeSOP)
            writer.WriteAttribute("RESPCONTSOP", Me.m_ResponsabileDaContattareSOP)
            writer.WriteAttribute("QUALIFSOP", Me.m_QualificaSOP)
            writer.WriteAttribute("TelefonoSO", Me.m_TelefonoSO)
            writer.WriteAttribute("FaxSO", Me.m_FaxSO)
            writer.WriteAttribute("CONVSINOSO", Me.m_ConvenzionePresenteSO)
            writer.WriteAttribute("CODCONVSO", Me.m_CodiceODescrizioneConvenzioneSO)
            writer.WriteAttribute("IDBustaPagaSO", Me.IDBustaPagaSO)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("IndirizzoSO", Me.m_IndirizzoSO)
            writer.WriteTag("Indirizzo", Me.m_Indirizzo)
            writer.WriteTag("IndirizzoN", Me.m_IndirizzoDiNotifica)
            writer.WriteTag("AltriAllegatiSO", Me.AltriAllegatiSO)
            writer.WriteTag("AltriAllegati", Me.AltriAllegati)
            writer.WriteTag("DocumentiProdotti", Me.DocumentiProdotti)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Data" : Me.m_Data = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDRichiedente" : Me.m_IDRichiedente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeRichiedente" : Me.m_NomeRichiedente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDPresaInCaricoDa" : Me.m_IDPresaInCaricoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePresaInCaricoDa" : Me.m_NomePresaInCaricoDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataPresaInCarico" : Me.m_DataPresaInCarico = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataCompletamento" : Me.m_DataCompletamento = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "StatoVisura" : Me.m_StatoVisura = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "VALAMM" : Me.m_ValutazioneAmministrazione = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "CENSDATLAV" : Me.m_CensimentoDatoreDiLavoro = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "CENSSEDOP" : Me.m_CensimentoSedeOperativa = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "VARIAZDENOM" : Me.m_VariazioneDenominazione = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "SBLOCCO" : Me.m_Sblocco = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "IDAmministrazione" : Me.m_IDAmministrazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "RagioneSociale" : Me.m_RagioneSociale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "OggettoSociale" : Me.m_OggettoSociale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CodiceFiscale" : Me.m_CodiceFiscale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PartitaIVA" : Me.m_PartitaIVA = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeResponsabile" : Me.m_ResponsabileDaContattare = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "QualificaResponsabile" : Me.m_Qualifica = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Indirizzo" : Me.m_Indirizzo = fieldValue
                Case "Telefono" : Me.m_Telefono = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Fax" : Me.m_Fax = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "eMail" : Me.m_IndirizzoeMail = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IndirizzoN" : Me.m_IndirizzoDiNotifica = fieldValue
                Case "TelefonoN" : Me.m_TelefonoDiNotifica = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "FaxN" : Me.m_FaxDiNotifica = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CONVSINO" : Me.m_ConvenzionePresente = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "CODCONV" : Me.m_CodiceODescrizioneConvenzione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NumeroDipendenti" : Me.m_NumeroDipendenti = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "AMMMODPRST008" : Me.m_AmministrazioneSottoscriveMODPREST_008 = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "NoteSocieta" : Me.m_NoteOInfoSullaSocieta = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDBustaPaga" : Me.m_IDBustaPaga = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDMotivoSblocco" : Me.m_IDMotivoRichiestaSblocco = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "AltriAllegati" : Me.m_AltriAllegati = fieldValue : Me.m_AltriAllegati.SetOwner(Me) : Me.m_AltriAllegati.SetContesto("AltriAllegati", 0)
                Case "DocumentiProdotti" : Me.m_DocumentiProdotti = fieldValue : Me.m_DocumentiProdotti.SetOwner(Me) : Me.m_DocumentiProdotti.SetContesto("DocumentiProdotti", 0)
                Case "CODAMMCL" : Me.m_CodiceAmministrazioneCL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "STATOAMMCL" : Me.m_StatoAmministrazioneCL = XML.Utils.Serializer.DeserializeString(fieldValue)

                Case "CODDLAVCL" : Me.m_CodiceDatoreLavoroCL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "RAGSOCSOP" : Me.m_RagioneSocialeSOP = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "RESPCONTSOP" : Me.m_ResponsabileDaContattareSOP = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "QUALIFSOP" : Me.m_QualificaSOP = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IndirizzoSO" : Me.m_IndirizzoSO = fieldValue
                Case "TelefonoSO" : Me.m_TelefonoSO = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "FaxSO" : Me.m_FaxSO = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CONVSINOSO" : Me.m_ConvenzionePresenteSO = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "CODCONVSO" : Me.m_CodiceODescrizioneConvenzioneSO = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDBustaPagaSO" : Me.IDBustaPagaSO = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "AltriAllegatiSO" : Me.m_AltriAllegatiSO = fieldValue : Me.m_AltriAllegatiSO.SetOwner(Me) : Me.m_AltriAllegatiSO.SetContesto("AltriAllegatiSO", 0)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return "Visura fatta il " & Formats.FormatUserDate(Me.m_Data) & " da " & Me.m_NomeRichiedente & " per " & Me.m_RagioneSociale
        End Function


    End Class


End Class