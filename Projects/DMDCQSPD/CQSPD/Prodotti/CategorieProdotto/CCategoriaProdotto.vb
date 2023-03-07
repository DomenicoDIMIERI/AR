Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    ''' <summary>
    ''' Relazione tra un prodotto ed una convenzione
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CCategoriaProdotto
        Inherits DBObject
        Implements IComparable

        Private m_Nome As String 'Descrizione
        Private m_NomeGruppo As String 'Supercategoria
        Private m_Flags As Integer
        Private m_DataInizio As DateTime?
        Private m_DataFine As DateTime?
        Private m_Attributi As CKeyCollection

        Public Sub New()
            Me.m_Nome = ""
            Me.m_Flags = 0
            Me.m_NomeGruppo = ""
            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing
            Me.m_Attributi = New CKeyCollection
        End Sub

        Public Overrides Function GetModule() As CModule
            Return CQSPD.CategorieProdotto.Module
        End Function

        ''' <summary>
        ''' Restituisce o imposta il nome del gruppo a cui appartiene la categoria di prodotti
        ''' Questo valore viene usato per raggruppare ulteriormente i prodotti nei reports
        ''' </summary>
        ''' <returns></returns>
        Public Property NomeGruppo As String
            Get
                Return Me.m_NomeGruppo
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeGruppo
                If (oldValue = value) Then Return
                Me.m_NomeGruppo = value
                Me.DoChanged("NomeGruppo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della categoria di prodotti
        ''' </summary>
        ''' <returns></returns>
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

        Public ReadOnly Property Attributi As CKeyCollection
            Get
                If (Me.m_Attributi Is Nothing) Then Me.m_Attributi = New CKeyCollection
                Return Me.m_Attributi
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDCatProd"
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            CQSPD.CategorieProdotto.UpdateCached(Me)
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
            Me.m_NomeGruppo = reader.Read("NomeGruppo", Me.m_NomeGruppo)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Dim tmp As String = reader.Read("Attributi", "")
            If (tmp <> "") Then Me.m_Attributi = XML.Utils.Serializer.Deserialize(tmp)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("NomeGruppo", Me.m_NomeGruppo)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("Attributi", XML.Utils.Serializer.Serialize(Me.Attributi))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("NomeGruppo", Me.m_NomeGruppo)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Attributi", Me.Attributi)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeGruppo" : Me.m_NomeGruppo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Attributi" : Me.m_Attributi = CType(fieldValue, CKeyCollection)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


        Public Function CompareTo(other As CCategoriaProdotto) As Integer
            Return Strings.Compare(Me.m_Nome, other.m_Nome, CompareMethod.Text)
        End Function

        Public Function _CompareTo(other As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(CType(other, CCategoriaProdotto))
        End Function

        Public Overrides Sub InitializeFrom(value As Object)
            MyBase.InitializeFrom(value)
        End Sub

    End Class


End Class