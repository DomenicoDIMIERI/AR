Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    <Flags>
    Public Enum ProvvigioneXOffertaFlags As Integer
        None = 0

        ''' <summary>
        ''' La provvigione é visibile solo agli utenti privileggiati
        ''' </summary>
        Privileged = 1
    End Enum

    <Serializable>
    Public Class CCQSPDProvvigioneXOfferta
        Inherits DBObject
        Implements IComparable

        Private m_Nome As String
        Private m_IDOfferta As Integer
        Private m_Offerta As COffertaCQS
        Private m_IDTipoProvvigione As Integer
        Private m_TipoProvvigione As CCQSPDTipoProvvigione
        Private m_IDCedente As Integer
        Private m_Cedente As CPersona
        Private m_NomeCedente As String
        Private m_IDRicevente As Integer
        Private m_Ricevente As CPersona
        Private m_NomeRicevente As String
        Private m_BaseDiCalcolo As Double?
        Private m_Valore As Double?
        Private m_ValorePagato As Double?
        Private m_DataPagamento As Date?
        Private m_Flags As ProvvigioneXOffertaFlags
        Private m_Parameters As CKeyCollection
        Private m_TipoCalcolo As CQSPDTipoProvvigioneEnum
        Private m_PagataDa As CQSPDTipoSoggetto
        Private m_PagataA As CQSPDTipoSoggetto
        Private m_Percentuale As Double?
        Private m_Fisso As Double?
        Private m_Formula As String
        Private m_Vincoli As CCollection(Of CTableConstraint)
        Private m_IDTrattativaCollaboratore As Integer
        Private m_TrattativaCollaboratore As CTrattativaCollaboratore
        Private m_IDCollaboratore As Integer
        Private m_Collaboratore As CCollaboratore
        Private m_NomeCollaboratore As String

        Public Sub New()
            Me.m_Nome = ""
            Me.m_IDOfferta = 0
            Me.m_Offerta = Nothing
            Me.m_IDTipoProvvigione = 0
            Me.m_TipoProvvigione = Nothing
            Me.m_IDCedente = 0
            Me.m_Cedente = Nothing
            Me.m_NomeCedente = ""
            Me.m_IDRicevente = 0
            Me.m_Ricevente = Nothing
            Me.m_NomeRicevente = ""
            Me.m_BaseDiCalcolo = Nothing
            Me.m_Valore = Nothing
            Me.m_ValorePagato = Nothing
            Me.m_DataPagamento = Nothing
            Me.m_Flags = ProvvigioneXOffertaFlags.None
            Me.m_Parameters = Nothing
            Me.m_TipoCalcolo = CQSPDTipoProvvigioneEnum.PercentualeSuMontanteLordo
            Me.m_PagataDa = CQSPDTipoSoggetto.Cessionario
            Me.m_PagataA = CQSPDTipoSoggetto.Agenzia
            Me.m_Percentuale = Nothing
            Me.m_Fisso = Nothing
            Me.m_Formula = ""
            Me.m_Vincoli = Nothing
            Me.m_IDTrattativaCollaboratore = 0
            Me.m_TrattativaCollaboratore = Nothing
            Me.m_IDCollaboratore = 0
            Me.m_Collaboratore = Nothing
            Me.m_NomeCollaboratore = ""
        End Sub

        Public Function Duplicate() As CCQSPDProvvigioneXOfferta
            Dim ret As CCQSPDProvvigioneXOfferta = Me.MemberwiseClone
            DBUtils.SetID(ret, 0)
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce o imposta l'ID della trattativa con il collaboratore
        ''' </summary>
        ''' <returns></returns>
        Public Property IDTrattativaCollaboratore As Integer
            Get
                Return GetID(Me.m_TrattativaCollaboratore, Me.m_IDTrattativaCollaboratore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDTrattativaCollaboratore
                If (oldValue = value) Then Return
                Me.m_IDTrattativaCollaboratore = value
                Me.m_TrattativaCollaboratore = Nothing
                Me.DoChanged("IDTrattativaCollaboratore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la trattativa con il collaboratore
        ''' </summary>
        ''' <returns></returns>
        Public Property TrattativaCollaboratore As CTrattativaCollaboratore
            Get
                If (Me.m_TrattativaCollaboratore Is Nothing) Then Me.m_TrattativaCollaboratore = CQSPD.TrattativeCollaboratore.GetItemById(Me.m_IDTrattativaCollaboratore)
                Return Me.m_TrattativaCollaboratore
            End Get
            Set(value As CTrattativaCollaboratore)
                Dim oldValue As CTrattativaCollaboratore = Me.m_TrattativaCollaboratore
                If (oldValue Is value) Then Return
                Me.m_TrattativaCollaboratore = value
                Me.m_IDTrattativaCollaboratore = GetID(value)
                Me.DoChanged("TrattativaCollaboratore", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetTrattativaCollaboratore(ByVal value As CTrattativaCollaboratore)
            Me.m_TrattativaCollaboratore = value
            Me.m_IDTrattativaCollaboratore = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID del collaboratore
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
        ''' Restituisce o imposta il collaboratore
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
                Dim oldValue As String = Me.m_NomeCollaboratore
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeCollaboratore = value
                Me.DoChanged("NomeCollaboratore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce una collezione di vincoli che determinano l'applicabilità o meno di questo tipo di provvigione all'offerta:
        ''' Affinché questo tipo di provvigione sia valido tutti i vincoli devono essere rispettati
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Vincoli As CCollection(Of CTableConstraint)
            Get
                If (Me.m_Vincoli Is Nothing) Then Me.m_Vincoli = New CCollection(Of CTableConstraint)
                Return Me.m_Vincoli
            End Get
        End Property

        Public Function RispettaVincoli(ByVal offerta As COffertaCQS) As Boolean
            For Each v As CTableConstraint In Me.Vincoli
                If Not v.Check(offerta) Then Return False
            Next
            Return True
        End Function



        ''' <summary>
        ''' Definisce il soggetto che eroga la provvigione
        ''' </summary>
        ''' <returns></returns>
        Public Property PagataDa As CQSPDTipoSoggetto
            Get
                Return Me.m_PagataDa
            End Get
            Set(value As CQSPDTipoSoggetto)
                Dim oldValue As CQSPDTipoSoggetto = Me.m_PagataDa
                If (oldValue = value) Then Return
                Me.m_PagataDa = value
                Me.DoChanged("PagataDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Definisce il soggetto che riceve la provvigione
        ''' </summary>
        ''' <returns></returns>
        Public Property PagataA As CQSPDTipoSoggetto
            Get
                Return Me.m_PagataA
            End Get
            Set(value As CQSPDTipoSoggetto)
                Dim oldValue As CQSPDTipoSoggetto = Me.m_PagataA
                If (oldValue = value) Then Return
                Me.m_PagataA = value
                Me.DoChanged("PagataA", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo di calcolo usato per calcolare la provvigione
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
        ''' Restituisce o imposta la percentuale calcolata secondo quanto definito in TipoCalcolo
        ''' </summary>
        ''' <returns></returns>
        Public Property Percentuale As Double?
            Get
                Return Me.m_Percentuale
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_Percentuale
                If (oldValue = value) Then Return
                Me.m_Percentuale = value
                Me.DoChanged("Percentuale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il valore fisso aggiunto alla provvigione
        ''' </summary>
        ''' <returns></returns>
        Public Property Fisso As Double?
            Get
                Return Me.m_Fisso
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_Fisso
                If (oldValue = value) Then Return
                Me.m_Fisso = value
                Me.DoChanged("Fisso", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la formula usata nel caso TipoCalcolo sia impostato a Formula
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
        ''' Restituisce o imposta il nome della provvigione
        ''' </summary>
        ''' <returns></returns>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Return
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        Public Property IDOfferta As Integer
            Get
                Return GetID(Me.m_Offerta, Me.m_IDOfferta)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOfferta
                If (oldValue = value) Then Return
                Me.m_IDOfferta = value
                Me.m_Offerta = Nothing
                Me.DoChanged("IDOfferta", value, oldValue)
            End Set
        End Property

        Public Property Offerta As COffertaCQS
            Get
                If (Me.m_Offerta Is Nothing) Then Me.m_Offerta = CQSPD.Offerte.GetItemById(Me.m_IDOfferta)
                Return Me.m_Offerta
            End Get
            Set(value As COffertaCQS)
                Dim oldValue As COffertaCQS = Me.m_Offerta
                If (oldValue Is value) Then Return
                Me.m_Offerta = value
                Me.m_IDOfferta = GetID(value)
                Me.DoChanged("Offerta", value, oldValue)
            End Set
        End Property

        Friend Sub SetOfferta(ByVal value As COffertaCQS)
            Me.m_Offerta = value
            Me.m_IDOfferta = GetID(value)
        End Sub

        Public Property IDTipoProvvigione As Integer
            Get
                Return GetID(Me.m_TipoProvvigione, Me.m_IDTipoProvvigione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDTipoProvvigione
                If (oldValue = value) Then Return
                Me.m_IDTipoProvvigione = value
                Me.m_TipoProvvigione = Nothing
                Me.DoChanged("IDTipoProvvigione", value, oldValue)
            End Set
        End Property

        Public Property TipoProvvigione As CCQSPDTipoProvvigione
            Get
                Return Me.m_TipoProvvigione
            End Get
            Set(value As CCQSPDTipoProvvigione)
                Dim oldValue As CCQSPDTipoProvvigione = Me.m_TipoProvvigione
                If (oldValue Is value) Then Return
                Me.m_TipoProvvigione = value
                Me.m_IDTipoProvvigione = GetID(value)
                Me.DoChanged("TipoProvvigione", value, oldValue)
            End Set
        End Property



        Public Property Flags As ProvvigioneXOffertaFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As ProvvigioneXOffertaFlags)
                Dim oldValue As ProvvigioneXOffertaFlags = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Parameters As CKeyCollection
            Get
                If (Me.m_Parameters Is Nothing) Then Me.m_Parameters = New CKeyCollection
                Return Me.m_Parameters
            End Get
        End Property

        Public Property IDCedente As Integer
            Get
                Return GetID(Me.m_Cedente, Me.m_IDCedente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCedente
                If (oldValue = value) Then Return
                Me.m_IDCedente = value
                Me.m_Cedente = Nothing
                Me.DoChanged("IDCedente", value, oldValue)
            End Set
        End Property

        Public Property Cedente As CPersona
            Get
                If (Me.m_Cedente Is Nothing) Then Me.m_Cedente = Anagrafica.Persone.GetItemById(Me.m_IDCedente)
                Return Me.m_Cedente
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Cedente
                If (oldValue Is value) Then Return
                Me.m_Cedente = value
                Me.m_IDCedente = GetID(value)
                Me.m_NomeCedente = "" : If (value IsNot Nothing) Then Me.m_NomeCedente = value.Nominativo
                Me.DoChanged("Cedente", value, oldValue)
            End Set
        End Property

        Public Property NomeCedente As String
            Get
                Return Me.m_NomeCedente
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeCedente
                If (oldValue = value) Then Return
                Me.m_NomeCedente = value
                Me.DoChanged("NomeCedente", value, oldValue)
            End Set
        End Property


        Public Property IDRicevente As Integer
            Get
                Return GetID(Me.m_Ricevente, Me.m_IDRicevente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRicevente
                If (oldValue = value) Then Return
                Me.m_IDRicevente = value
                Me.m_Ricevente = Nothing
                Me.DoChanged("IDRicevente", value, oldValue)
            End Set
        End Property

        Public Property Ricevente As CPersona
            Get
                If (Me.m_Ricevente Is Nothing) Then Me.m_Ricevente = Anagrafica.Persone.GetItemById(Me.m_IDRicevente)
                Return Me.m_Ricevente
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Ricevente
                If (oldValue Is value) Then Return
                Me.m_Ricevente = value
                Me.m_IDRicevente = GetID(value)
                Me.m_NomeRicevente = "" : If (value IsNot Nothing) Then Me.m_NomeRicevente = value.Nominativo
                Me.DoChanged("Ricevente", value, oldValue)
            End Set
        End Property

        Public Property NomeRicevente As String
            Get
                Return Me.m_NomeRicevente
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeRicevente
                If (oldValue = value) Then Return
                Me.m_NomeRicevente = value
                Me.DoChanged("NomeRicevente", value, oldValue)
            End Set
        End Property

        Public Property BaseDiCalcolo As Double?
            Get
                Return Me.m_BaseDiCalcolo
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_BaseDiCalcolo
                If (oldValue = value) Then Return
                Me.m_BaseDiCalcolo = value
                Me.DoChanged("BaseDiCalcolo", value, oldValue)
            End Set
        End Property

        Public Property Valore As Double?
            Get
                Return Me.m_Valore
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_Valore
                If (oldValue = value) Then Return
                Me.m_Valore = value
                Me.DoChanged("Valore", value, oldValue)
            End Set
        End Property

        Public Property ValorePagato As Double?
            Get
                Return Me.m_ValorePagato
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_ValorePagato
                If (oldValue = value) Then Return
                Me.m_ValorePagato = value
                Me.DoChanged("ValorePagato", value, oldValue)
            End Set
        End Property

        Public Property DataPagamento As Date?
            Get
                Return Me.m_DataPagamento
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataPagamento
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataPagamento = value
                Me.DoChanged("DataPagamento", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDProvvXOfferta"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_IDOfferta = reader.Read("IDOfferta", Me.m_IDOfferta)
            Me.m_IDTipoProvvigione = reader.Read("IDTipoProvvigione", Me.m_IDTipoProvvigione)
            Me.m_IDCedente = reader.Read("IDCedente", Me.m_IDCedente)
            Me.m_NomeCedente = reader.Read("NomeCedente", Me.m_NomeCedente)
            Me.m_IDRicevente = reader.Read("IDRicevente", Me.m_IDRicevente)
            Me.m_NomeRicevente = reader.Read("NomeRicevente", Me.m_NomeRicevente)
            Me.m_BaseDiCalcolo = reader.Read("BaseDiCalcolo", Me.m_BaseDiCalcolo)
            Me.m_Valore = reader.Read("Valore", Me.m_Valore)
            Me.m_ValorePagato = reader.Read("ValorePagato", Me.m_ValorePagato)
            Me.m_DataPagamento = reader.Read("DataPagamento", Me.m_DataPagamento)
            Me.m_PagataDa = reader.Read("PagataDa", Me.m_PagataDa)
            Me.m_PagataA = reader.Read("PagataA", Me.m_PagataA)
            Me.m_TipoCalcolo = reader.Read("TipoCalcolo", Me.m_TipoCalcolo)
            Me.m_Percentuale = reader.Read("Percentuale", Me.m_Percentuale)
            Me.m_Fisso = reader.Read("Fisso", Me.m_Fisso)
            Me.m_Formula = reader.Read("Formula", Me.m_Formula)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Dim tmp As String = reader.Read("Params", "")
            If (tmp <> "") Then Me.m_Parameters = CType(XML.Utils.Serializer.Deserialize(tmp), CKeyCollection)
            tmp = reader.Read("Vincoli", "")
            If (tmp <> "") Then
                Me.m_Vincoli = New CCollection(Of CTableConstraint)
                Me.m_Vincoli.AddRange(XML.Utils.Serializer.Deserialize(tmp))
            End If
            Me.m_IDTrattativaCollaboratore = reader.Read("IDTrattativaCollaboratore", Me.m_IDTrattativaCollaboratore)
            Me.m_IDCollaboratore = reader.Read("IDCollaboratore", Me.m_IDCollaboratore)
            Me.m_NomeCollaboratore = reader.Read("NomeCollaboratore", Me.m_NomeCollaboratore)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("IDOfferta", Me.IDOfferta)
            writer.Write("IDTipoProvvigione", Me.IDTipoProvvigione)
            writer.Write("IDCedente", Me.IDCedente)
            writer.Write("NomeCedente", Me.m_NomeCedente)
            writer.Write("IDRicevente", Me.IDRicevente)
            writer.Write("NomeRicevente", Me.m_NomeRicevente)
            writer.Write("BaseDiCalcolo", Me.m_BaseDiCalcolo)
            writer.Write("Valore", Me.m_Valore)
            writer.Write("ValorePagato", Me.m_ValorePagato)
            writer.Write("DataPagamento", Me.m_DataPagamento)
            writer.Write("PagataDa", Me.m_PagataDa)
            writer.Write("PagataA", Me.m_PagataA)
            writer.Write("TipoCalcolo", Me.m_TipoCalcolo)
            writer.Write("Percentuale", Me.m_Percentuale)
            writer.Write("Fisso", Me.m_Fisso)
            writer.Write("Formula", Me.m_Formula)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Params", XML.Utils.Serializer.Serialize(Me.Parameters))
            writer.Write("Vincoli", XML.Utils.Serializer.Serialize(Me.Vincoli))
            writer.Write("IDTrattativaCollaboratore", Me.IDTrattativaCollaboratore)
            writer.Write("IDCollaboratore", Me.IDCollaboratore)
            writer.Write("NomeCollaboratore", Me.m_NomeCollaboratore)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("IDOfferta", Me.IDOfferta)
            writer.WriteAttribute("IDTipoProvvigione", Me.IDTipoProvvigione)
            writer.WriteAttribute("IDCedente", Me.IDCedente)
            writer.WriteAttribute("NomeCedente", Me.m_NomeCedente)
            writer.WriteAttribute("IDRicevente", Me.IDRicevente)
            writer.WriteAttribute("NomeRicevente", Me.m_NomeRicevente)
            writer.WriteAttribute("BaseDiCalcolo", Me.m_BaseDiCalcolo)
            writer.WriteAttribute("Valore", Me.m_Valore)
            writer.WriteAttribute("ValorePagato", Me.m_ValorePagato)
            writer.WriteAttribute("DataPagamento", Me.m_DataPagamento)
            writer.WriteAttribute("PagataDa", Me.m_PagataDa)
            writer.WriteAttribute("PagataA", Me.m_PagataA)
            writer.WriteAttribute("TipoCalcolo", Me.m_TipoCalcolo)
            writer.WriteAttribute("Percentuale", Me.m_Percentuale)
            writer.WriteAttribute("Fisso", Me.m_Fisso)
            writer.WriteAttribute("Formula", Me.m_Formula)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("IDTrattativaCollaboratore", Me.IDTrattativaCollaboratore)
            writer.WriteAttribute("IDCollaboratore", Me.IDCollaboratore)
            writer.WriteAttribute("NomeCollaboratore", Me.m_NomeCollaboratore)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Params", Me.Parameters)
            writer.WriteTag("Vincoli", Me.Vincoli)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDOfferta" : Me.m_IDOfferta = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDTipoProvvigione" : Me.m_IDTipoProvvigione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDCedente" : Me.m_IDCedente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCedente" : Me.m_NomeCedente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDRicevente" : Me.m_IDRicevente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeRicevente" : Me.m_NomeRicevente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Valore" : Me.m_Valore = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "BaseDiCalcolo" : Me.m_BaseDiCalcolo = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "ValorePagato" : Me.m_ValorePagato = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "DataPagamento" : Me.m_DataPagamento = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "PagataDa" : Me.m_PagataDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "PagataA" : Me.m_PagataA = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoCalcolo" : Me.m_TipoCalcolo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Percentuale" : Me.m_Percentuale = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Fisso" : Me.m_Fisso = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Formula" : Me.m_Formula = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Params" : Me.m_Parameters = XML.Utils.Serializer.ToObject(fieldValue)
                Case "Vincoli" : Me.m_Vincoli = New CCollection(Of CTableConstraint) : Me.m_Vincoli.AddRange(XML.Utils.Serializer.ToObject(fieldValue))
                Case "IDTrattativaCollaboratore" : Me.m_IDTrattativaCollaboratore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDCollaboratore" : Me.m_IDCollaboratore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCollaboratore" : Me.m_NomeCollaboratore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Public Function CompareTo(ByVal obj As CCQSPDProvvigioneXOfferta) As Integer
            Return Strings.Compare(Me.m_Nome, obj.m_Nome, CompareMethod.Text)
        End Function

        Private Function _CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(CType(obj, CCQSPDProvvigioneXOfferta))
        End Function

        Public Sub Aggiorna(ByVal offerta As COffertaCQS, ByVal estinzioni As CCollection(Of EstinzioneXEstintore))
            Dim p As CCQSPDProdotto
            Dim o As Object

            p = offerta.Prodotto
            If (Me.IDTipoProvvigione <> 0) Then
                Dim gp As CGruppoProdotti = Nothing
                Dim tp As CCQSPDTipoProvvigione = Nothing
                If (p IsNot Nothing) Then gp = p.GruppoProdotti
                If (gp Is Nothing) Then Return
                Dim items As CCQSPDTipoProvvigioneCollection = gp.Provvigioni
                'tp = items.GetItemById(this.getIDTipoProvvigione());
                If (tp Is Nothing) Then tp = items.GetItemByName(Me.Nome)
                If (tp Is Nothing) Then Return
                Select Case (Me.TipoCalcolo())
                    Case CQSPDTipoProvvigioneEnum.NessunaPercentuale
                        Me.m_Valore = tp.Fisso
                    Case CQSPDTipoProvvigioneEnum.PercentualeSuMontanteLordo
                        Me.m_Valore = tp.Fisso + offerta.MontanteLordo * tp.Percentuale / 100
                    Case CQSPDTipoProvvigioneEnum.PercentualeSuDeltaMontante
                        Me.m_Valore = tp.Fisso + offerta.CalcolaBaseML(estinzioni) * tp.Percentuale / 100
                    Case CQSPDTipoProvvigioneEnum.Formula
                        o = Sistema.Types.CreateInstance(tp.Formula)
                        Me.m_Valore = o.Evaluate(Me, offerta, estinzioni)
                End Select

                If (tp.ValoreMax.HasValue AndAlso Me.m_Valore.HasValue) Then
                    Me.m_Valore = Math.Min(Me.m_Valore.Value, tp.ValoreMax)
                End If
            ElseIf (Me.IDTrattativaCollaboratore()) Then
                Dim col As CCollaboratore = Me.Collaboratore
                Dim tp As CTrattativaCollaboratore = Nothing
                If (col Is Nothing) Then Return
                Dim items As CTrattativeCollaboratore = col.Trattative
                'tp = items.GetItemById(this.getIDTrattativaCollaboratore());
                If (tp Is Nothing) Then tp = items.GetItemByNameAndProdotto(Me.Nome, p, TestFlag(offerta.Flags, OffertaFlags.DirettaCollaboratore))
                If (tp Is Nothing) Then Return
                Select Case (Me.TipoCalcolo())
                    Case CQSPDTipoProvvigioneEnum.NessunaPercentuale
                        If (tp.ValoreBase.HasValue) Then Me.m_Valore = tp.ValoreBase
                    Case CQSPDTipoProvvigioneEnum.PercentualeSuMontanteLordo
                        If (tp.ValoreBase.HasValue) Then Me.m_Valore = tp.ValoreBase
                        If (tp.SpreadApprovato.HasValue) Then Me.m_Valore += offerta.MontanteLordo * tp.SpreadApprovato / 100
                    Case CQSPDTipoProvvigioneEnum.PercentualeSuDeltaMontante
                        If (tp.ValoreBase.HasValue) Then Me.m_Valore = tp.ValoreBase
                        If (tp.SpreadApprovato.HasValue) Then Me.m_Valore += offerta.CalcolaBaseML(estinzioni) * tp.SpreadApprovato / 100
                    Case CQSPDTipoProvvigioneEnum.Formula
                        o = Sistema.Types.CreateInstance(tp.Formula)
                        Me.m_Valore = o.Evaluate(Me, offerta, estinzioni)
                End Select

                If (tp.ValoreMax.HasValue AndAlso Me.m_Valore.HasValue) Then
                    Me.m_Valore = Math.Min(Me.m_Valore.Value, tp.ValoreMax.Value)
                End If
            End If
            Me.Save(True)
        End Sub


    End Class

End Class
