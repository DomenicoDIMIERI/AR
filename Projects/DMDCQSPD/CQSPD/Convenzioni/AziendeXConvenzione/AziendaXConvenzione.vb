Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    ''' <summary>
    ''' Relazione tra un'azienda ed una convenzione
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class AziendaXConvenzione
        Inherits DBObject
        Implements IComparable

        Private m_Nome As String 'Descrizione
        Private m_IDAzienda As Integer 'ID del Azienda associato
        <NonSerialized> Private m_Azienda As CAzienda 'Oggetto Azienda Associato
        Private m_IDConvenzione As Integer 'ID della convenzione
        <NonSerialized> Private m_Convenzione As CQSPDConvenzione
        Private m_Flags As Integer
        Private m_DataInizio As DateTime?
        Private m_DataFine As DateTime?

        Public Sub New()
            Me.m_Nome = ""
            Me.m_IDAzienda = 0
            Me.m_Azienda = Nothing
            Me.m_IDConvenzione = 0
            Me.m_Convenzione = Nothing
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
        ''' ID del Azienda associato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDAzienda As Integer
            Get
                Return GetID(Me.m_Azienda, Me.m_IDAzienda)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAzienda
                If (oldValue = value) Then Exit Property
                Me.m_IDAzienda = value
                Me.m_Azienda = Nothing
                Me.DoChanged("IDAzienda", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Oggetto Azienda Associato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Azienda As CAzienda
            Get
                If (Me.m_Azienda Is Nothing) Then Me.m_Azienda = Anagrafica.Aziende.GetItemById(Me.m_IDAzienda)
                Return Me.m_Azienda
            End Get
            Set(value As CAzienda)
                Dim oldValue As CAzienda = Me.Azienda
                If (oldValue = value) Then Exit Property
                Me.m_Azienda = value
                Me.m_IDAzienda = GetID(value)
                Me.DoChanged("Azienda", value, oldValue)
            End Set
        End Property


        Protected Friend Overridable Sub SetAzienda(ByVal value As CAzienda)
            Me.m_Azienda = value
            Me.m_IDAzienda = GetID(value)
        End Sub

        Public Property IDConvenzione As Integer
            Get
                Return GetID(Me.m_Convenzione, Me.m_IDConvenzione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDConvenzione
                If (oldValue = value) Then Exit Property
                Me.m_IDConvenzione = value
                Me.m_Convenzione = Nothing
                Me.DoChanged("IDConvenzione", value, oldValue)
            End Set
        End Property

        Public Property Convenzione As CQSPDConvenzione
            Get
                If (Me.m_Convenzione Is Nothing) Then Me.m_Convenzione = CQSPD.Convenzioni.GetItemById(Me.m_IDConvenzione)
                Return Me.m_Convenzione
            End Get
            Set(value As CQSPDConvenzione)
                Dim oldValue As CQSPDConvenzione = Me.Convenzione
                If (oldValue = value) Then Exit Property
                Me.m_Convenzione = value
                Me.m_IDConvenzione = GetID(value)
                Me.DoChanged("Convenzione", value, oldValue)
            End Set
        End Property

        Friend Sub SetConvenzione(ByVal value As CQSPDConvenzione)
            Me.m_Convenzione = value
            Me.m_IDConvenzione = GetID(value)
        End Sub

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
            Return "tbl_FIN_AzieXConvenzioni"
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            For Each p As CQSPDConvenzione In CQSPD.Convenzioni.LoadAll
                p.InvalidateAziende()
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
            Me.m_IDAzienda = reader.Read("IDAzienda", Me.m_IDAzienda)
            Me.m_IDConvenzione = reader.Read("IDConvenzione", Me.m_IDConvenzione)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("IDAzienda", Me.IDAzienda)
            writer.Write("IDConvenzione", Me.IDConvenzione)
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
            writer.WriteAttribute("IDAzienda", Me.IDAzienda)
            writer.WriteAttribute("IDConvenzione", Me.IDConvenzione)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDAzienda" : Me.m_IDAzienda = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDConvenzione" : Me.m_IDConvenzione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)

                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


        Public Function CompareTo(other As AziendaXConvenzione) As Integer
            Return Strings.Compare(Me.m_Nome, other.m_Nome, CompareMethod.Text)
        End Function

        Public Function _CompareTo(other As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(CType(other, AziendaXConvenzione))
        End Function

    End Class


End Class