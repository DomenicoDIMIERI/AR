Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    Public Enum CQSPDTipoProvvigioneEnum As Integer
        ''' <summary>
        ''' Provvigione fissa espressa nella valuta corrente (Fisso)
        ''' </summary>
        NessunaPercentuale = 0

        ''' <summary>
        ''' Fisso + Percentuale sul montante lordo
        ''' </summary>
        PercentualeSuMontanteLordo = 1

        ''' <summary>
        ''' Fisso + Percentuale sul delta montante
        ''' </summary>
        PercentualeSuDeltaMontante = 2

        ''' <summary>
        ''' Fisso + Valutazione della formula
        ''' </summary>
        Formula = 3
    End Enum

    <Flags>
    Public Enum CQSPDTipoSoggetto As Integer
        Cessionario = 10
        Agenzia = 20
        Collaboratore = 30
        Cliente = 40
    End Enum

    <Flags>
    Public Enum CQSPDTipoProvvigioneFlags As Integer
        None = 0
        Nascosta = 1
        IncludiNelTAN = 2
        IncludiNelTAEG = 4
        IncludiNelTEG = 8
    End Enum


    <Serializable>
    Public Class CCQSPDTipoProvvigione
        Inherits DBObject
        Implements IComparable

        Private m_Nome As String
        Private m_IDGruppoProdotti As Integer
        Private m_GruppoProdotti As CGruppoProdotti
        Private m_PagataDa As CQSPDTipoSoggetto
        Private m_PagataA As CQSPDTipoSoggetto
        Private m_TipoCalcolo As CQSPDTipoProvvigioneEnum
        Private m_Percentuale As Double?
        Private m_Fisso As Double?
        Private m_ValoreMax As Double?
        Private m_Formula As String
        Private m_Flags As CQSPDTipoProvvigioneFlags
        Private m_Vincoli As CCollection(Of CTableConstraint)
        Private m_Parameters As CKeyCollection

        Public Sub New()
            Me.m_Nome = ""
            Me.m_IDGruppoProdotti = 0
            Me.m_GruppoProdotti = Nothing
            Me.m_PagataDa = CQSPDTipoSoggetto.Cessionario
            Me.m_PagataA = CQSPDTipoSoggetto.Agenzia
            Me.m_TipoCalcolo = CQSPDTipoProvvigioneEnum.PercentualeSuMontanteLordo
            Me.m_Percentuale = Nothing
            Me.m_Fisso = Nothing
            Me.m_ValoreMax = Nothing
            Me.m_Formula = ""
            Me.m_Flags = CQSPDTipoProvvigioneFlags.None
            Me.m_Parameters = Nothing
            Me.m_Vincoli = Nothing
        End Sub

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

        Public Property ValoreMax As Double?
            Get
                Return Me.m_ValoreMax
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_ValoreMax
                If (value = oldValue) Then Return
                Me.m_ValoreMax = value
                Me.DoChanged("ValoreMax", value, oldValue)
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
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Return
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del gruppo prodotti per cui è definita la provvigione
        ''' </summary>
        ''' <returns></returns>
        Public Property IDGruppoProdotti As Integer
            Get
                Return GetID(Me.m_GruppoProdotti, Me.m_IDGruppoProdotti)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDGruppoProdotti
                If (oldValue = value) Then Return
                Me.m_IDGruppoProdotti = value
                Me.m_GruppoProdotti = Nothing
                Me.DoChanged("IDGruppoProdotti", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il gruppo prodotti per cui é definita la provvigione
        ''' </summary>
        ''' <returns></returns>
        Public Property GruppoProdotti As CGruppoProdotti
            Get
                If (Me.m_GruppoProdotti Is Nothing) Then Me.m_GruppoProdotti = CQSPD.GruppiProdotto.GetItemById(Me.m_IDGruppoProdotti)
                Return Me.m_GruppoProdotti
            End Get
            Set(value As CGruppoProdotti)
                Dim oldValue As CGruppoProdotti = Me.GruppoProdotti
                If (oldValue Is value) Then Return
                Me.m_GruppoProdotti = value
                Me.m_IDGruppoProdotti = GetID(value)
                Me.DoChanged("GruppoProdotti", value, oldValue)
            End Set
        End Property

        Friend Sub SetGruppoProdotti(ByVal value As CGruppoProdotti)
            Me.m_GruppoProdotti = value
            Me.m_IDGruppoProdotti = GetID(value)
        End Sub

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

        Public Property Flags As CQSPDTipoProvvigioneFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As CQSPDTipoProvvigioneFlags)
                Dim oldValue As CQSPDTipoProvvigioneFlags = Me.m_Flags
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


        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDTipiProvvigione"
        End Function


        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_IDGruppoProdotti = reader.Read("IDGruppoProdotti", Me.m_IDGruppoProdotti)
            Me.m_PagataDa = reader.Read("PagataDa", Me.m_PagataDa)
            Me.m_PagataA = reader.Read("PagataA", Me.m_PagataA)
            Me.m_TipoCalcolo = reader.Read("TipoCalcolo", Me.m_TipoCalcolo)
            Me.m_Percentuale = reader.Read("Percentuale", Me.m_Percentuale)
            Me.m_Fisso = reader.Read("Fisso", Me.m_Fisso)
            Me.m_Formula = reader.Read("Formula", Me.m_Formula)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_ValoreMax = reader.Read("ValoreMax", Me.m_ValoreMax)
            Dim tmp As String = reader.Read("Params", "")
            If (tmp <> "") Then Me.m_Parameters = CType(XML.Utils.Serializer.Deserialize(tmp), CKeyCollection)
            tmp = reader.Read("Vincoli", "")
            If (tmp <> "") Then
                Me.m_Vincoli = New CCollection(Of CTableConstraint)
                Me.m_Vincoli.AddRange(XML.Utils.Serializer.Deserialize(tmp))
            End If
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("IDGruppoProdotti", Me.IDGruppoProdotti)
            writer.Write("PagataDa", Me.m_PagataDa)
            writer.Write("PagataA", Me.m_PagataA)
            writer.Write("TipoCalcolo", Me.m_TipoCalcolo)
            writer.Write("Percentuale", Me.m_Percentuale)
            writer.Write("Fisso", Me.m_Fisso)
            writer.Write("Formula", Me.m_Formula)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Params", XML.Utils.Serializer.Serialize(Me.Parameters))
            writer.Write("Vincoli", XML.Utils.Serializer.Serialize(Me.Vincoli))
            writer.Write("ValoreMax", Me.m_ValoreMax)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("IDGruppoProdotti", Me.IDGruppoProdotti)
            writer.WriteAttribute("PagataDa", Me.m_PagataDa)
            writer.WriteAttribute("PagataA", Me.m_PagataA)
            writer.WriteAttribute("TipoCalcolo", Me.m_TipoCalcolo)
            writer.WriteAttribute("Percentuale", Me.m_Percentuale)
            writer.WriteAttribute("Fisso", Me.m_Fisso)
            writer.WriteAttribute("Formula", Me.m_Formula)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("ValoreMax", Me.m_ValoreMax)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Params", Me.Parameters)
            writer.WriteTag("Vincoli", Me.Vincoli)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDGruppoProdotti" : Me.m_IDGruppoProdotti = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "PagataDa" : Me.m_PagataDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "PagataA" : Me.m_PagataA = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoCalcolo" : Me.m_TipoCalcolo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Percentuale" : Me.m_Percentuale = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Fisso" : Me.m_Fisso = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Formula" : Me.m_Formula = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ValoreMax" : Me.m_ValoreMax = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Params" : Me.m_Parameters = XML.Utils.Serializer.ToObject(fieldValue)
                Case "Vincoli" : Me.m_Vincoli = New CCollection(Of CTableConstraint) : Me.m_Vincoli.AddRange(XML.Utils.Serializer.ToObject(fieldValue))
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Public Overrides Sub Save(Optional force As Boolean = False)
            MyBase.Save(force)
            CQSPD.GruppiProdotto.InvalidateTipiProvvigione()
        End Sub

        Public Function CompareTo(ByVal obj As CCQSPDTipoProvvigione) As Integer
            Return Strings.Compare(Me.m_Nome, obj.m_Nome, CompareMethod.Text)
        End Function

        Private Function _CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(CType(obj, CCQSPDTipoProvvigione))
        End Function
    End Class

End Class
