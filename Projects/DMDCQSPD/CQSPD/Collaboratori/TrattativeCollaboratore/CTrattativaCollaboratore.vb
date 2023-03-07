Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.XML

Partial Public Class CQSPD


    Public Enum StatoTrattativa As Integer
        ''' <summary>
        ''' 'L'operatore non ha proposto il prodotto
        ''' </summary>
        ''' <remarks></remarks>
        STATO_NONPROPOSTO = 0

        ''' <summary>
        ''' L'operatore ha proposto il prodotto ed il produttore ha formulato le sue richieste			
        ''' </summary>
        ''' <remarks></remarks>
        STATO_PROPOSTA = 1

        ''' <summary>
        ''' L'operatore ha proposto il prodotto e si sta attendendo l'esito della valutazione  			
        ''' </summary>
        ''' <remarks></remarks>
        STATO_ATTESAAPPROVAZIONE = 3

        ''' <summary>
        ''' Il supervisore ha approvato le richieste del produttore
        ''' </summary>
        ''' <remarks></remarks>
        STATO_APPROVATO = 5

        ''' <summary>
        ''' Il supervisore non ha approvato le richieste del produttore
        ''' </summary>
        ''' <remarks></remarks>
        STATO_NONAPPROVATO = 7

        ''' <summary>
        ''' Il produttore ha accettato l'offerta così come è stata approvata dal supervisore
        ''' </summary>
        ''' <remarks></remarks>
        STATO_ACCETTATO = 9

        ''' <summary>
        ''' Il produttore non ha accettato l'ultima offerta
        ''' </summary>
        ''' <remarks></remarks>
        STATO_NONACCETTATO = 11
    End Enum

    <Flags>
    Public Enum TrattativaCollaboratoreFlags As Integer
        None = 0

        ''' <summary>
        ''' La provvigione é visibile solo agli utenti privilegiati
        ''' </summary>
        Nascosta = 1

        ''' <summary>
        ''' La provvigione si applica solo in caso di produzione diretta del collaboratore
        ''' </summary>
        SoloDirettaCollaboratore = 2
    End Enum

    <Serializable>
    Public Class CTrattativaCollaboratore
        Inherits DBObject

        Private m_Nome As String
        Private m_Richiesto As Boolean
        <NonSerialized> Private m_Collaboratore As CCollaboratore
        Private m_IDCollaboratore As Integer
        Private m_NomeCollaboratore As String
        Private m_IDCessionario As Integer
        <NonSerialized> Private m_Cessionario As CCQSPDCessionarioClass
        Private m_NomeCessionario As String
        Private m_IDProdotto As Integer
        <NonSerialized> Private m_Prodotto As CCQSPDProdotto
        Private m_NomeProdotto As String
        Private m_StatoTrattativa As StatoTrattativa
        <NonSerialized> Private m_PropostoDa As CUser
        Private m_IDPropostoDa As Integer
        Private m_PropostoIl As Nullable(Of Date)
        Private m_SpreadProposto As Double?
        Private m_SpreadRichiesto As Double?
        Private m_SpreadApprovato As Double?
        Private m_SpreadFidelizzazione As Double?
        Private m_ValoreBase As Decimal?
        Private m_ValoreMax As Decimal?
        Private m_Formula As String
        Private m_TipoCalcolo As CQSPDTipoProvvigioneEnum
        Private m_IDApprovatoDa As Integer
        <NonSerialized> Private m_ApprovatoDa As CUser
        Private m_ApprovatoIl As Nullable(Of Date)
        Private m_Note As String
        Private m_Flags As TrattativaCollaboratoreFlags
        Private m_Attributi As CKeyCollection

        Public Sub New()
            Me.m_Nome = ""
            Me.m_Richiesto = False
            Me.m_Collaboratore = Nothing
            Me.m_IDCollaboratore = 0
            Me.m_NomeCollaboratore = ""
            Me.m_IDCessionario = 0
            Me.m_Cessionario = Nothing
            Me.m_NomeCessionario = ""
            Me.m_IDProdotto = 0
            Me.m_Prodotto = Nothing
            Me.m_NomeProdotto = ""
            Me.m_StatoTrattativa = StatoTrattativa.STATO_ACCETTATO
            Me.m_PropostoDa = Nothing
            Me.m_IDPropostoDa = 0
            Me.m_PropostoIl = Nothing
            Me.m_SpreadProposto = Nothing
            Me.m_SpreadRichiesto = Nothing
            Me.m_SpreadApprovato = Nothing
            Me.m_SpreadFidelizzazione = Nothing
            Me.m_ValoreBase = Nothing
            Me.m_ValoreMax = Nothing
            Me.m_Formula = ""
            Me.m_TipoCalcolo = CQSPDTipoProvvigioneEnum.PercentualeSuMontanteLordo
            Me.m_IDApprovatoDa = 0
            Me.m_ApprovatoDa = Nothing
            Me.m_ApprovatoIl = Nothing
            Me.m_Note = ""
            Me.m_Flags = TrattativaCollaboratoreFlags.None
            Me.m_Attributi = Nothing
        End Sub

        Public Overrides Function GetModule() As CModule
            Return CQSPD.TrattativeCollaboratore.Module
        End Function

        ''' <summary>
        ''' Restituisce o imposta il nome della trattativa
        ''' </summary>
        ''' <returns></returns>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Return
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una percentuale di fidelizzazione 
        ''' </summary>
        ''' <returns></returns>
        Public Property SpreadFidelizzazione As Double?
            Get
                Return Me.m_SpreadFidelizzazione
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_SpreadFidelizzazione
                If (oldValue = value) Then Return
                Me.m_SpreadFidelizzazione = value
                Me.DoChanged("SpreadFidelizzazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la provvigione massima in euro
        ''' </summary>
        ''' <returns></returns>
        Public Property ValoreMax As Decimal?
            Get
                Return Me.m_ValoreMax
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_ValoreMax
                If (oldValue = value) Then Return
                Me.m_ValoreMax = value
                Me.DoChanged("ValoreMax", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il valore base riconosciuto per il prodotto
        ''' </summary>
        ''' <returns></returns>
        Public Property ValoreBase As Decimal?
            Get
                Return Me.m_ValoreBase
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_ValoreBase
                If (value = oldValue) Then Return
                Me.m_ValoreBase = value
                Me.DoChanged("ValoreBase", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la formula con cui viene calcolata la provvigione
        ''' </summary>
        ''' <returns></returns>
        Public Property Formula As String
            Get
                Return Me.m_Formula
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Formula
                If (oldValue = value) Then Return
                Me.m_Formula = value
                Me.DoChanged("Formula", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo di calcolo utilizzato per il prodotto
        ''' </summary>
        ''' <returns></returns>
        Public Property TipoCalcolo As CQSPDTipoProvvigioneEnum
            Get
                Return Me.m_TipoCalcolo
            End Get
            Set(value As CQSPDTipoProvvigioneEnum)
                Dim oldValue As CQSPDTipoProvvigioneEnum = Me.m_TipoCalcolo
                If (oldValue = value) Then Return
                Me.m_TipoCalcolo = value
                Me.DoChanged("TipoCalcolo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei flags aggiuntivi
        ''' </summary>
        ''' <returns></returns>
        Public Property Flags As TrattativaCollaboratoreFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As TrattativaCollaboratoreFlags)
                Dim oldValue As TrattativaCollaboratoreFlags = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce dei parametri aggiuntivi
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Attributi As CKeyCollection
            Get
                If (Me.m_Attributi Is Nothing) Then Me.m_Attributi = New CKeyCollection
                Return Me.m_Attributi
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore che indica se il prodotto 
        ''' </summary>
        ''' <returns></returns>
        Public Property Richiesto As Boolean
            Get
                Return Me.m_Richiesto
            End Get
            Set(value As Boolean)
                If (Me.m_Richiesto = value) Then Return
                Me.m_Richiesto = value
                Me.DoChanged("Richiesto", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del collaboratore associato
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
        ''' Restituosce o imposta il collaboratore associato
        ''' </summary>
        ''' <returns></returns>
        Public Property Collaboratore As CCollaboratore
            Get
                If (Me.m_Collaboratore Is Nothing) Then Me.m_Collaboratore = CQSPD.Collaboratori.GetItemById(Me.m_IDCollaboratore)
                Return Me.m_Collaboratore
            End Get
            Set(value As CCollaboratore)
                Dim oldValue As CCollaboratore = Me.Collaboratore
                If (oldValue = value) Then Return
                Me.m_Collaboratore = value
                Me.m_IDCollaboratore = GetID(value)
                Me.m_NomeCollaboratore = ""
                If (value IsNot Nothing) Then Me.m_NomeCollaboratore = value.NomePersona
                Me.DoChanged("Collaboratore", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetCollaboratore(ByVal value As CCollaboratore)
            Me.m_Collaboratore = value
            Me.m_IDCollaboratore = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome del collaboratore
        ''' </summary>
        ''' <returns></returns>
        Public Property NomeCollaboratore As String
            Get
                Return Me.m_NomeCollaboratore
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeCollaboratore
                If (oldValue = value) Then Return
                Me.m_NomeCollaboratore = value
                Me.DoChanged("NomeCollaboratore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del prodotto associato
        ''' </summary>
        ''' <returns></returns>
        Public Property IDProdotto As Integer
            Get
                Return GetID(Me.m_Prodotto, Me.m_IDProdotto)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDProdotto
                If oldValue = value Then Return
                Me.m_IDProdotto = value
                Me.m_Prodotto = Nothing
                Me.DoChanged("IDProdotto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il prodotto associato
        ''' </summary>
        ''' <returns></returns>
        Public Property Prodotto As CCQSPDProdotto
            Get
                If (Me.m_Prodotto Is Nothing) Then Me.m_Prodotto = CQSPD.Prodotti.GetItemById(Me.m_IDProdotto)
                Return Me.m_Prodotto
            End Get
            Set(value As CCQSPDProdotto)
                Dim oldValue As CCQSPDProdotto = Me.Prodotto
                If (oldValue = value) Then Return
                Me.m_Prodotto = value
                Me.m_IDProdotto = GetID(value)
                Me.m_NomeProdotto = ""
                If (value IsNot Nothing) Then Me.m_NomeProdotto = value.Descrizione
                Me.DoChanged("Prodotto", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetProdotto(ByVal value As CCQSPDProdotto)
            Me.m_Prodotto = value
            Me.m_IDProdotto = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome del prodotto
        ''' </summary>
        ''' <returns></returns>
        Public Property NomeProdotto As String
            Get
                Return Me.m_NomeProdotto
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeProdotto
                If (oldValue = value) Then Return
                Me.m_NomeProdotto = value
                Me.DoChanged("NomeProdotto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta delle note aggiuntive
        ''' </summary>
        ''' <returns></returns>
        Public Property Note As String
            Get
                Return Me.m_Note
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Note
                If (oldValue = value) Then Return
                Me.m_Note = value
                Me.DoChanged("Note", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del cessionario
        ''' </summary>
        ''' <returns></returns>
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

        ''' <summary>
        ''' Restituisce o imposta il cessionario 
        ''' </summary>
        ''' <returns></returns>
        Public Property Cessionario As CCQSPDCessionarioClass
            Get
                If (Me.m_Cessionario Is Nothing) Then Me.m_Cessionario = CQSPD.Cessionari.GetItemById(Me.m_IDCessionario)
                Return Me.m_Cessionario
            End Get
            Set(value As CCQSPDCessionarioClass)
                Dim oldValue As CCQSPDCessionarioClass = Me.Cessionario
                If (oldValue = value) Then Return
                Me.m_Cessionario = value
                Me.m_IDCessionario = GetID(value)
                Me.m_NomeCessionario = ""
                If (value IsNot Nothing) Then Me.m_NomeCessionario = value.Nome
                Me.DoChanged("Cessionario", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetCessionario(ByVal value As CCQSPDCessionarioClass)
            Me.m_Cessionario = value
            Me.m_IDCessionario = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome del cessionario
        ''' </summary>
        ''' <returns></returns>
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

        ''' <summary>
        ''' Restituisce o imposta la data in cui il prodotto é stato proposto/richiesto
        ''' </summary>
        ''' <returns></returns>
        Public Property PropostoIl As Nullable(Of Date)
            Get
                Return Me.m_PropostoIl
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_PropostoIl
                If (DateUtils.Compare(oldValue, value) = 0) Then Return
                Me.m_PropostoIl = value
                Me.DoChanged("PropostoIl", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha proposto il prodotto o che ne ha registrato la richiesta
        ''' </summary>
        ''' <returns></returns>
        Public Property IDPropostoDa As Integer
            Get
                Return GetID(Me.m_PropostoDa, Me.m_IDPropostoDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPropostoDa
                If oldValue = value Then Return
                Me.m_PropostoDa = Nothing
                Me.m_IDPropostoDa = value
                Me.DoChanged("IDPropostoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha proposto il prodotto o che ne ha registrato la richiesta
        ''' </summary>
        ''' <returns></returns>
        Public Property PropostoDa As CUser
            Get
                If (Me.m_PropostoDa Is Nothing) Then Me.m_PropostoDa = Sistema.Users.GetItemById(Me.m_IDPropostoDa)
                Return Me.m_PropostoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.PropostoDa
                If (oldValue = value) Then Return
                Me.m_PropostoDa = value
                Me.m_IDPropostoDa = GetID(value)
                Me.DoChanged("PropostoDa", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetPropostoDa(ByVal value As CUser)
            Me.m_PropostoDa = value
            Me.m_IDPropostoDa = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce il nome dell'utente che ha proposto il prodotto o che ne ha registrato la richiesta
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property NomePropostoDa() As String
            Get
                If Me.PropostoDa Is Nothing Then Return "UserID: " & Me.IDPropostoDa
                Return Me.PropostoDa.Nominativo
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato della trattativa pre il prodotto
        ''' </summary>
        ''' <returns></returns>
        Public Property StatoTrattativa As StatoTrattativa
            Get
                Return Me.m_StatoTrattativa
            End Get
            Set(value As StatoTrattativa)
                Dim oldValue As StatoTrattativa = Me.m_StatoTrattativa
                If (oldValue = value) Then Return
                Me.m_StatoTrattativa = value
                Me.DoChanged("StatoTrattativa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che indica lo stato della trattativa per il prodotto
        ''' </summary>
        ''' <returns></returns>
        Public Property StatoTrattativaEx As String
            Get
                Return CQSPD.TrattativeCollaboratore.FormatStato(Me.StatoTrattativa)
            End Get
            Set(value As String)
                Me.StatoTrattativa = CQSPD.TrattativeCollaboratore.ParseStato(value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la provvigione max in percentuale (proposta)
        ''' </summary>
        ''' <returns></returns>
        Public Property SpreadProposto As Double?
            Get
                Return Me.m_SpreadProposto
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_SpreadProposto
                If (oldValue = value) Then Return
                Me.m_SpreadProposto = value
                Me.DoChanged("SpreadProposto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la provvigione max in percentuale (richiesta)
        ''' </summary>
        ''' <returns></returns>
        Public Property SpreadRichiesto As Double?
            Get
                Return Me.m_SpreadRichiesto
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_SpreadRichiesto
                If (oldValue = value) Then Return
                Me.m_SpreadRichiesto = value
                Me.DoChanged("SpreadRichiesto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la provvigione max in % (concordata)
        ''' </summary>
        ''' <returns></returns>
        Public Property SpreadApprovato As Double?
            Get
                Return Me.m_SpreadApprovato
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_SpreadApprovato
                If (oldValue = value) Then Return
                Me.m_SpreadApprovato = value
                Me.DoChanged("SpreadApprovato", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha convalidato la proposta
        ''' </summary>
        ''' <returns></returns>
        Public Property ApprovatoDa As CUser
            Get
                If (Me.m_ApprovatoDa Is Nothing) Then Me.m_ApprovatoDa = Sistema.Users.GetItemById(Me.m_IDApprovatoDa)
                Return Me.m_ApprovatoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.ApprovatoDa
                If (oldValue = value) Then Return
                Me.m_ApprovatoDa = value
                Me.m_IDApprovatoDa = GetID(value)
                Me.DoChanged("ApprovatoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha convalidato la proposta
        ''' </summary>
        ''' <returns></returns>
        Public Property IDApprovatoDa As Integer
            Get
                Return GetID(Me.m_ApprovatoDa, Me.m_IDApprovatoDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDApprovatoDa
                If (oldValue = value) Then Return
                Me.m_IDApprovatoDa = value
                Me.m_ApprovatoDa = Nothing
                Me.DoChanged("IDApprovatoDa", value, oldValue)
            End Set
        End Property


        Public ReadOnly Property NomeApprovatoDa() As String
            Get
                If Me.ApprovatoDa Is Nothing Then Return "UserID: " & Me.m_IDApprovatoDa
                Return Me.ApprovatoDa.Nominativo
            End Get
        End Property


        ''' <summary>
        ''' Restituisce o imposta la data in cui il prodotto é stato assegnato o negato al collaboratore
        ''' </summary>
        ''' <returns></returns>
        Public Property ApprovatoIl As Nullable(Of Date)
            Get
                Return Me.m_ApprovatoIl
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_ApprovatoIl
                If (DateUtils.Compare(oldValue, value) = 0) Then Return
                Me.m_ApprovatoIl = value
                Me.DoChanged("ApprovatoIl", value, oldValue)
            End Set
        End Property


        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Sub Save(Optional force As Boolean = False)
            MyBase.Save(force)
            CQSPD.Collaboratori.InvalidateTrattative
        End Sub

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDCollabBUI"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Richiesto = reader.Read("Richiesto", Me.m_Richiesto)
            Me.m_IDCollaboratore = reader.Read("IDCollaboratore", Me.m_IDCollaboratore)
            Me.m_NomeCollaboratore = reader.Read("NomeCollaboratore", Me.m_NomeCollaboratore)
            Me.m_IDCessionario = reader.Read("IDCessionario", Me.m_IDCessionario)
            Me.m_NomeCessionario = reader.Read("NomeCessionario", Me.m_NomeCessionario)
            Me.m_IDProdotto = reader.Read("IDProdotto", Me.m_IDProdotto)
            Me.m_NomeProdotto = reader.Read("NomeProdotto", Me.m_NomeProdotto)
            Me.m_StatoTrattativa = reader.Read("StatoTrattativa", Me.m_StatoTrattativa)
            Me.m_SpreadProposto = reader.Read("SpreadProposto", Me.m_SpreadProposto)
            Me.m_SpreadRichiesto = reader.Read("SpreadRichiesto", Me.m_SpreadRichiesto)
            Me.m_SpreadApprovato = reader.Read("SpreadApprovato", Me.m_SpreadApprovato)
            Me.m_SpreadFidelizzazione = reader.Read("SpreadFidelizzazione", Me.m_SpreadFidelizzazione)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_ValoreBase = reader.Read("ValoreBase", Me.m_ValoreBase)
            Me.m_ValoreMax = reader.Read("ValoreMax", Me.m_ValoreMax)
            Me.m_Formula = reader.Read("Formula", Me.m_Formula)
            Dim tmp As String = reader.Read("Attributi", "")
            If (tmp <> "") Then Me.m_Attributi = XML.Utils.Serializer.Deserialize(tmp)
            Me.m_IDPropostoDa = reader.Read("IDPropostoDa", Me.m_IDPropostoDa)
            Me.m_PropostoIl = reader.Read("PropostoIl", Me.m_PropostoIl)
            Me.m_IDApprovatoDa = reader.Read("IDApprovatoDa", Me.m_IDApprovatoDa)
            Me.m_ApprovatoIl = reader.Read("ApprovatoIl", Me.m_ApprovatoIl)
            Me.m_Note = reader.Read("Note", Me.m_Note)
            Me.m_TipoCalcolo = reader.Read("TipoCalcolo", Me.m_TipoCalcolo)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Richiesto", Me.m_Richiesto)
            writer.Write("IDCollaboratore", Me.IDCollaboratore)
            writer.Write("NomeCollaboratore", Me.m_NomeCollaboratore)
            writer.Write("IDCessionario", Me.IDCessionario)
            writer.Write("NomeCessionario", Me.m_NomeCessionario)
            writer.Write("IDProdotto", Me.IDProdotto)
            writer.Write("NomeProdotto", Me.m_NomeProdotto)
            writer.Write("StatoTrattativa", Me.m_StatoTrattativa)
            writer.Write("SpreadProposto", Me.m_SpreadProposto)
            writer.Write("SpreadRichiesto", Me.m_SpreadRichiesto)
            writer.Write("SpreadApprovato", Me.m_SpreadApprovato)
            writer.Write("SpreadFidelizzazione", Me.m_SpreadFidelizzazione)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("ValoreBase", Me.m_ValoreBase)
            writer.Write("ValoreMax", Me.m_ValoreMax)
            writer.Write("Formula", Me.m_Formula)
            writer.Write("Attributi", XML.Utils.Serializer.Serialize(Me.Attributi))
            writer.Write("IDPropostoDa", Me.IDPropostoDa)
            writer.Write("PropostoIl", Me.m_PropostoIl)
            writer.Write("IDApprovatoDa", Me.IDApprovatoDa)
            writer.Write("ApprovatoIl", Me.m_ApprovatoIl)
            writer.Write("TipoCalcolo", Me.m_TipoCalcolo)
            writer.Write("Note", Me.m_Note)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Richiesto", Me.m_Richiesto)
            writer.WriteAttribute("IDCollaboratore", Me.IDCollaboratore)
            writer.WriteAttribute("NomeCollaboratore", Me.m_NomeCollaboratore)
            writer.WriteAttribute("IDCessionario", Me.IDCessionario)
            writer.WriteAttribute("NomeCessionario", Me.m_NomeCessionario)
            writer.WriteAttribute("IDProdotto", Me.IDProdotto)
            writer.WriteAttribute("NomeProdotto", Me.m_NomeProdotto)
            writer.WriteAttribute("StatoTrattativa", Me.m_StatoTrattativa)
            writer.WriteAttribute("SpreadProposto", Me.m_SpreadProposto)
            writer.WriteAttribute("SpreadRichiesto", Me.m_SpreadRichiesto)
            writer.WriteAttribute("SpreadApprovato", Me.m_SpreadApprovato)
            writer.WriteAttribute("SpreadFidelizzazione", Me.m_SpreadFidelizzazione)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("ValoreBase", Me.m_ValoreBase)
            writer.WriteAttribute("ValoreMax", Me.m_ValoreMax)
            writer.WriteAttribute("IDPropostoDa", Me.IDPropostoDa)
            writer.WriteAttribute("PropostoIl", Me.m_PropostoIl)
            writer.WriteAttribute("IDApprovatoDa", Me.IDApprovatoDa)
            writer.WriteAttribute("ApprovatoIl", Me.m_ApprovatoIl)
            writer.WriteAttribute("TipoCalcolo", Me.m_TipoCalcolo)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Attributi", Me.Attributi)
            writer.WriteTag("Formula", Me.m_Formula)
            writer.WriteTag("Note", Me.m_Note)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Richiesto" : Me.m_Richiesto = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "IDCollaboratore" : Me.m_IDCollaboratore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCollaboratore" : Me.m_NomeCollaboratore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDCessionario" : Me.m_IDCessionario = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCessionario" : Me.m_NomeCessionario = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDProdotto" : Me.m_IDProdotto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeProdotto" : Me.m_NomeProdotto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoTrattativa" : Me.m_StatoTrattativa = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "SpreadProposto" : Me.m_SpreadProposto = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "SpreadRichiesto" : Me.m_SpreadRichiesto = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "SpreadApprovato" : Me.m_SpreadApprovato = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "SpreadFidelizzazione" : Me.m_SpreadFidelizzazione = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ValoreBase" : Me.m_ValoreBase = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "ValoreMax" : Me.m_ValoreMax = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Formula" : Me.m_Formula = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDPropostoDa" : Me.m_IDPropostoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "PropostoIl" : Me.m_PropostoIl = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDApprovatoDa" : Me.m_IDApprovatoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ApprovatoIl" : Me.m_ApprovatoIl = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Note" : Me.m_Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Attributi" : Me.m_Attributi = CType(fieldValue, CKeyCollection)
                Case "TipoCalcolo" : Me.m_TipoCalcolo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Overrides Function ToString() As String
            Dim ret As New System.Text.StringBuilder
            ret.Append("{ ")
            ret.Append(Me.NomeCollaboratore)
            ret.Append(", ")
            ret.Append(Me.NomeProdotto)
            ret.Append(" }")
            Return ret.ToString
        End Function

    End Class

End Class
