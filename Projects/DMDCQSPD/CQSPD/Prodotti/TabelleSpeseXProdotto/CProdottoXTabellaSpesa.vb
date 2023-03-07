Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD
    ''' <summary>
    ''' Relazione Prodotti x Tabellea Spese ( 1 - molti )
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CProdottoXTabellaSpesa
        Inherits DBObject
        Implements IComparable

        Private m_Nome As String 'Descrizione
        Private m_IDProdotto As Integer 'ID del prodotto associato
        <NonSerialized> Private m_Prodotto As CCQSPDProdotto 'Oggetto Prodotto Associato
        Private m_IDTabellaSpese As Integer 'ID della tablla spese
        <NonSerialized> Private m_TabellaSpese As CTabellaSpese
        Private m_Flags As Integer
        Private m_DataInizio As DateTime?
        Private m_DataFine As DateTime?

        Public Sub New()
            Me.m_Nome = ""
            Me.m_IDProdotto = 0
            Me.m_Prodotto = Nothing
            Me.m_IDTabellaSpese = 0
            Me.m_TabellaSpese = Nothing
            Me.m_Flags = 0
            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' ID del prodotto associato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDProdotto As Integer
            Get
                Return GetID(Me.m_Prodotto, Me.m_IDProdotto)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDProdotto
                If (oldValue = value) Then Exit Property
                Me.m_IDProdotto = value
                Me.m_Prodotto = Nothing
                Me.DoChanged("IDProdotto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Oggetto Prodotto Associato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Prodotto As CCQSPDProdotto
            Get
                If (Me.m_Prodotto Is Nothing) Then Me.m_Prodotto = CQSPD.Prodotti.GetItemById(Me.m_IDProdotto)
                Return Me.m_Prodotto
            End Get
            Set(value As CCQSPDProdotto)
                Dim oldValue As CCQSPDProdotto = Me.Prodotto
                If (oldValue = value) Then Exit Property
                Me.m_Prodotto = value
                Me.m_IDProdotto = GetID(value)
                Me.DoChanged("Prodotto", value, oldValue)
            End Set
        End Property


        Protected Friend Overridable Sub SetProdotto(ByVal value As CCQSPDProdotto)
            Me.m_Prodotto = value
            Me.m_IDProdotto = GetID(value)
        End Sub

        Public Property IDTabellaSpese As Integer
            Get
                Return GetID(Me.m_TabellaSpese, Me.m_IDTabellaSpese)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDTabellaSpese
                If (oldValue = value) Then Exit Property
                Me.m_IDTabellaSpese = value
                Me.m_TabellaSpese = Nothing
                Me.DoChanged("IDTabellaSpese", value, oldValue)
            End Set
        End Property

        Public Property TabellaSpese As CTabellaSpese
            Get
                If (Me.m_TabellaSpese Is Nothing) Then Me.m_TabellaSpese = CQSPD.TabelleSpese.GetItemById(Me.m_IDTabellaSpese)
                Return Me.m_TabellaSpese
            End Get
            Set(value As CTabellaSpese)
                Dim oldValue As CTabellaSpese = Me.TabellaSpese
                If (oldValue = value) Then Exit Property
                Me.m_TabellaSpese = value
                Me.m_IDTabellaSpese = GetID(value)
                Me.DoChanged("TabellaSpese", value, oldValue)
            End Set
        End Property

        Public Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public Property DataInizio As DateTime?
            Get
                Return Me.m_DataInizio
            End Get
            Set(value As DateTime?)
                Dim oldValue As DateTime? = Me.m_DataInizio
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataInizio = value
                Me.DoChanged("DataInizio", value, oldValue)
            End Set
        End Property

        Public Property DataFine As DateTime?
            Get
                Return Me.m_DataFine
            End Get
            Set(value As DateTime?)
                Dim oldValue As DateTime? = Me.m_DataFine
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataFine = value
                Me.DoChanged("DataFine", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_FIN_ProdXTabSpes"
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            For Each p As CCQSPDProdotto In CQSPD.Prodotti.LoadAll
                p.InvalidateTabelleSpese
            Next
        End Sub

        Public Function IsValid() As Boolean
            Return Me.IsValid(DateUtils.Now)
        End Function

        Public Function IsValid(ByVal at As DateTime) As Boolean
            Return DateUtils.CheckBetween(at, Me.m_DataInizio, Me.m_DataFine)
        End Function


        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_IDProdotto = reader.Read("IDProdotto", Me.m_IDProdotto)
            Me.m_IDTabellaSpese = reader.Read("IDTabellaSpese", Me.m_IDTabellaSpese)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("IDProdotto", Me.IDProdotto)
            writer.Write("IDTabellaSpese", Me.IDTabellaSpese)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("IDProdotto", Me.IDProdotto)
            writer.WriteAttribute("IDTabellaSpese", Me.IDTabellaSpese)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDProdotto" : Me.m_IDProdotto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDTabellaSpese" : Me.m_IDTabellaSpese = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)

                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


        Public Function CompareTo(other As CProdottoXTabellaSpesa) As Integer
            Return Strings.Compare(Me.m_Nome, other.m_Nome, CompareMethod.Text)
        End Function

        Public Function _CompareTo(other As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(CType(other, CProdottoXTabellaSpesa))
        End Function

    End Class


End Class