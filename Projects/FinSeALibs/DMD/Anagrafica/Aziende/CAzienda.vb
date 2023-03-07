Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    <Serializable> _
    Public Class CAzienda
        Inherits CPersona

        Private m_RagioneSociale As String
        Private m_FormaGiuridica As String
        Private m_Categoria As String
        Private m_CapitaleSociale As Nullable(Of Decimal)
        Private m_Fatturato As Nullable(Of Decimal)
        Private m_NumeroDipendenti As Nullable(Of Integer)
        Private m_Tipologia As String

        <NonSerialized> _
        Private m_Impiegati As CImpiegati
        Private m_IDEntePagante As Integer
        Private m_NomeEntePagante As String
        <NonSerialized> _
        Private m_EntePagante As CAzienda
        Private m_IDReferente As Integer
        <NonSerialized> _
        Private m_Referente As CImpiegato
        Private m_ValutazioneGARF As Integer
        Private m_TipoRapporto As String

        Private m_Uffici As CAziendaUffici

        Public Sub New()
            Me.m_RagioneSociale = ""
            Me.m_FormaGiuridica = ""
            Me.m_Categoria = ""
            Me.m_CapitaleSociale = Nothing
            Me.m_Fatturato = Nothing
            Me.m_NumeroDipendenti = Nothing
            Me.m_Tipologia = ""
            Me.m_Impiegati = Nothing
            Me.m_IDEntePagante = 0
            Me.m_NomeEntePagante = ""
            Me.m_EntePagante = Nothing
            Me.ResidenteA.Nome = "Sede legale"
            Me.DomiciliatoA.Nome = "Sede operativa"
            Me.m_ValutazioneGARF = 1
            Me.m_TipoRapporto = ""
            Me.m_Uffici = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce l'elenco degli uffici associati all'azienda
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Uffici As CAziendaUffici
            Get
                SyncLock Me
                    If (Me.m_Uffici Is Nothing) Then Me.m_Uffici = New CAziendaUffici(Me)
                    Return Me.m_Uffici
                End SyncLock
            End Get
        End Property

        Public Overrides Function GetModule() As CModule
            Return Anagrafica.Aziende.Module
        End Function

        Public Shadows Sub MergeWith(ByVal persona As CAzienda)
            With persona
                If (Me.m_RagioneSociale = "") Then Me.m_RagioneSociale = .RagioneSociale
                If (Me.m_FormaGiuridica = "") Then Me.m_FormaGiuridica = .FormaGiuridica
                If (Me.m_Categoria = "") Then Me.m_Categoria = .Categoria
                If (Me.m_CapitaleSociale.HasValue = False) Then Me.m_CapitaleSociale = .CapitaleSociale
                If (Me.m_Fatturato.HasValue = False) Then Me.m_Fatturato = .Fatturato
                If (Me.m_NumeroDipendenti.HasValue = False) Then Me.m_NumeroDipendenti = .NumeroDipendenti
                If (Me.m_Tipologia = "") Then Me.m_Tipologia = .Tipologia
                If (Me.EntePagante Is Nothing) Then Me.EntePagante = .EntePagante
                If (Me.m_NomeEntePagante = "") Then Me.m_NomeEntePagante = .m_NomeEntePagante
                If (Me.IDReferente = 0) Then Me.m_IDReferente = .IDReferente
                If (Me.m_ValutazioneGARF < .m_ValutazioneGARF) Then Me.m_ValutazioneGARF = .m_ValutazioneGARF
                If (Me.m_TipoRapporto = "") Then Me.m_TipoRapporto = .m_TipoRapporto
                Me.m_Uffici = Nothing
            End With
            MyBase.MergeWith(persona)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il tipo rapporto predefinito per l'azienda
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoRapporto As String
            Get
                Return Me.m_TipoRapporto
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TipoRapporto
                If (oldValue = value) Then Exit Property
                Me.m_TipoRapporto = value
                Me.DoChanged("TipoRapporto", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta la valutazione GARF dell'azienda
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ValutazioneGARF As Integer
            Get
                Return Me.m_ValutazioneGARF
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_ValutazioneGARF
                If (oldValue = value) Then Exit Property
                Me.m_ValutazioneGARF = value
                Me.DoChanged("ValutazioneGARF", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'impiegato a cui fare riferimento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDReferente As Integer
            Get
                Return GetID(Me.m_Referente, Me.m_IDReferente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDReferente
                If (oldValue = value) Then Exit Property
                Me.m_IDReferente = value
                Me.m_Referente = Nothing
                Me.DoChanged("IDReferente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce  o imposta l'impiegato a cui fare riferimento.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Referente As CImpiegato
            Get
                If (Me.m_Referente Is Nothing) Then Me.m_Referente = Me.Impiegati.GetItemById(Me.m_IDReferente)
                Return Me.m_Referente
            End Get
            Set(value As CImpiegato)
                Dim oldValue As Integer = Me.IDReferente
                If (oldValue = GetID(value)) Then Exit Property
                If Me.Impiegati.GetItemById(GetID(value)) Is Nothing Then
                    Throw New ArgumentOutOfRangeException("Il referente non è presente nell'elenco degli impiegati")
                End If
                Me.m_Referente = value
                Me.m_IDReferente = GetID(value)
                Me.DoChanged("Referente", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce una collezione di tutte le relazioni lavorative tra l'azienda e le persone fisiche assunte 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Impiegati As CImpiegati
            Get
                If Me.m_Impiegati Is Nothing Then Me.m_Impiegati = New CImpiegati(Me)
                Return Me.m_Impiegati
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo di azienda
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FormaGiuridica As String
            Get
                Return Me.m_FormaGiuridica
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_FormaGiuridica
                If (oldValue = value) Then Exit Property
                Me.m_FormaGiuridica = value
                Me.DoChanged("FormaGiuridica", value, oldValue)
            End Set
        End Property

        Public Property Categoria As String
            Get
                Return Me.m_Categoria
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Categoria
                If (oldValue = value) Then Exit Property
                Me.m_Categoria = value
                Me.DoChanged("Categoria", value, oldValue)
            End Set
        End Property

        Public Property Tipologia As String
            Get
                Return Me.m_Tipologia
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Tipologia
                If (oldValue = value) Then Exit Property
                Me.m_Tipologia = value
                Me.DoChanged("Tipologia", value, oldValue)
            End Set
        End Property

        Public Property CapitaleSociale As Nullable(Of Decimal)
            Get
                Return Me.m_CapitaleSociale
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_CapitaleSociale
                If (oldValue = value) Then Exit Property
                Me.m_CapitaleSociale = value
                Me.DoChanged("CapitaleSociale", value, oldValue)
            End Set
        End Property

        Public Property Fatturato As Nullable(Of Decimal)
            Get
                Return Me.m_Fatturato
            End Get
            Set(value As Nullable(Of Decimal))
                Dim oldValue As Nullable(Of Decimal) = Me.m_Fatturato
                If (oldValue = value) Then Exit Property
                Me.m_Fatturato = value
                Me.DoChanged("Fatturato", value, oldValue)
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

        Public Property IDEntePagante As Integer
            Get
                Return GetID(Me.m_EntePagante, Me.m_IDEntePagante)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDEntePagante
                If oldValue = value Then Exit Property
                Me.m_IDEntePagante = value
                Me.m_EntePagante = Nothing
                Me.DoChanged("IDEntePagante", value, oldValue)
            End Set
        End Property

        Public Property EntePagante As CAzienda
            Get
                If Me.m_EntePagante Is Nothing Then Me.m_EntePagante = Anagrafica.Aziende.GetItemById(Me.m_IDEntePagante)
                Return Me.m_EntePagante
            End Get
            Set(value As CAzienda)
                Dim oldValue As CAzienda = Me.EntePagante
                If (oldValue = value) Then Exit Property
                Me.m_EntePagante = value
                Me.m_IDEntePagante = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeEntePagante = value.Nominativo
                Me.DoChanged("EntePagante", value, oldValue)
            End Set
        End Property

        Public Property NomeEntePagante As String
            Get
                Return Me.m_NomeEntePagante
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeEntePagante
                If (oldValue = value) Then Exit Property
                Me.m_NomeEntePagante = value
                Me.DoChanged("NomeEntePagante", value, oldValue)
            End Set
        End Property

        Public Overrides ReadOnly Property TipoPersona As TipoPersona
            Get
                Return TipoPersona.PERSONA_GIURIDICA
            End Get
        End Property

        Public Property RagioneSociale As String
            Get
                Return Me.m_RagioneSociale
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_RagioneSociale
                If (oldValue = value) Then Exit Property
                Me.m_RagioneSociale = value
                Me.DoChanged("RagioneSociale", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_Persone"
        End Function

        Public Overrides Function IsChanged() As Boolean
            Dim ret As Boolean = MyBase.IsChanged()
            If (Not ret AndAlso Me.m_Impiegati IsNot Nothing) Then ret = DBUtils.IsChanged(Me.m_Impiegati)
            Return ret
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If Not (Me.m_Impiegati Is Nothing) Then Me.m_Impiegati.Save(force)
            Return ret
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Anagrafica.Aziende.UpdateCached(Me)
        End Sub

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_RagioneSociale = reader.Read("Cognome", Me.m_RagioneSociale)
            Me.m_FormaGiuridica = reader.Read("TipoAzienda", Me.m_FormaGiuridica)
            Me.m_CapitaleSociale = reader.Read("CapitaleSociale", Me.m_CapitaleSociale)
            Me.m_Fatturato = reader.Read("Fatturato", Me.m_Fatturato)
            Me.m_NumeroDipendenti = reader.Read("NumeroDipendenti", Me.m_NumeroDipendenti)
            Me.m_Categoria = reader.Read("Categoria", Me.m_Categoria)
            Me.m_Tipologia = reader.Read("Tipologia", Me.m_Tipologia)
            Me.m_IDEntePagante = reader.Read("IDEntePagante", Me.m_IDEntePagante)
            Me.m_NomeEntePagante = reader.Read("NomeEntePagante", Me.m_NomeEntePagante)
            Me.m_IDReferente = reader.Read("IDImpiego", Me.m_IDReferente)
            Me.m_ValutazioneGARF = reader.Read("GARF", Me.m_ValutazioneGARF)
            Me.m_TipoRapporto = reader.Read("TipoRapporto", Me.m_TipoRapporto)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Cognome", Me.m_RagioneSociale)
            writer.Write("TipoAzienda", Me.m_FormaGiuridica)
            writer.Write("CapitaleSociale", Me.m_CapitaleSociale)
            writer.Write("Fatturato", Me.m_Fatturato)
            writer.Write("NumeroDipendenti", Me.m_NumeroDipendenti)
            writer.Write("Categoria", Me.m_Categoria)
            writer.Write("Tipologia", Me.m_Tipologia)
            writer.Write("IDEntePagante", Me.m_IDEntePagante)
            writer.Write("NomeEntePagante", Me.m_NomeEntePagante)
            writer.Write("IDImpiego", Me.IDReferente)
            writer.Write("GARF", Me.m_ValutazioneGARF)
            writer.Write("TipoRapporto", Me.m_TipoRapporto)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("RagioneSociale", Me.m_RagioneSociale)
            writer.WriteAttribute("FormaGiuridica", Me.m_FormaGiuridica)
            writer.WriteAttribute("CapitaleSociale", Me.m_CapitaleSociale)
            writer.WriteAttribute("Fatturato", Me.m_Fatturato)
            writer.WriteAttribute("NumeroDipendenti", Me.m_NumeroDipendenti)
            writer.WriteAttribute("Categoria", Me.m_Categoria)
            writer.WriteAttribute("Tipologia", Me.m_Tipologia)
            writer.WriteAttribute("IDEntePagante", Me.IDEntePagante)
            writer.WriteAttribute("NomeEntePagante", Me.m_NomeEntePagante)
            writer.WriteAttribute("IDReferente", Me.IDReferente)
            writer.WriteAttribute("GARF", Me.m_ValutazioneGARF)
            writer.WriteAttribute("TipoRapporto", Me.m_TipoRapporto)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "RagioneSociale" : Me.m_RagioneSociale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "FormaGiuridica" : Me.m_FormaGiuridica = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Categoria" : Me.m_Categoria = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CapitaleSociale" : Me.m_CapitaleSociale = XML.Utils.Serializer.DeserializeNumber(fieldValue)
                Case "Fatturato" : Me.m_Fatturato = XML.Utils.Serializer.DeserializeNumber(fieldValue)
                Case "NumeroDipendenti" : Me.m_NumeroDipendenti = XML.Utils.Serializer.DeserializeNumber(fieldValue)
                Case "Tipologia" : Me.m_Tipologia = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDEntePagante" : Me.m_IDEntePagante = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDReferente" : Me.m_IDReferente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeEntePagante" : Me.m_NomeEntePagante = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "GARF" : Me.m_ValutazioneGARF = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoRapporto" : Me.m_TipoRapporto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : Call MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides ReadOnly Property Nominativo As String
            Get
                Return Me.m_RagioneSociale
            End Get
        End Property

        Protected Overrides Function DropFromDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            If (GetID(Anagrafica.Aziende.AziendaPrincipale) = Me.ID AndAlso Not force) Then Throw New InvalidOperationException("Non è possibile eliminare l'azienda principale")
            Return MyBase.DropFromDatabase(dbConn, force)
        End Function

        Friend Sub InternalAddUfficio(ByVal value As CUfficio)
            SyncLock Me
                If (Me.m_Uffici IsNot Nothing) Then
                    value = Me.m_Uffici.GetItemById(GetID(value))
                    If (value Is Nothing) Then Me.m_Uffici.Add(value)
                    Me.m_Uffici.Sort()
                End If
            End SyncLock
        End Sub

        Friend Sub InternalRemoveUfficio(ByVal value As CUfficio)
            SyncLock Me
                If (Me.m_Uffici IsNot Nothing) Then
                    value = Me.m_Uffici.GetItemById(GetID(value))
                    If (value IsNot Nothing) Then Me.m_Uffici.Remove(value)
                End If
            End SyncLock
        End Sub

        Friend Sub InternalUpdateUfficio(ByVal value As CUfficio)
            SyncLock Me
                If (Me.m_Uffici IsNot Nothing) Then
                    Dim oldValue As CUfficio = Me.m_Uffici.GetItemById(GetID(value))
                    If (oldValue IsNot Nothing) Then Me.m_Uffici.Remove(oldValue)
                    If (value.Stato = ObjectStatus.OBJECT_VALID) Then
                        Me.m_Uffici.Add(value)
                        Me.Uffici.Sort()
                    End If

                End If
            End SyncLock
        End Sub


    End Class



End Class