Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica




Partial Public Class CQSPD

    <Serializable> _
    Public Class CConsulentePratica
        Inherits DBObjectPO
        Implements IComparable

        Private m_Nome As String
        Private m_DataInizio As Nullable(Of Date)
        Private m_DataFine As Nullable(Of Date)
        Private m_IDUser As Integer             'ID dell'utente associato al consulente
        Private m_User As CUser

        Public Sub New()
            Me.m_Nome = ""
            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing
            Me.m_IDUser = 0
            Me.m_User = Nothing
        End Sub

        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Nome
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        Public Property DataInizio As Nullable(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataInizio
                If (oldValue = value) Then Exit Property
                Me.m_DataInizio = value
                Me.DoChanged("DataInizio", value, oldValue)
            End Set
        End Property

        Public Property DataFine As Nullable(Of Date)
            Get
                Return Me.m_DataFine
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataFine
                If (oldValue = value) Then Exit Property
                Me.m_DataFine = value
                Me.DoChanged("DataFine", value, oldValue)
            End Set
        End Property

        Public Function IsValid(ByVal atDate As Date) As Boolean
            Return Calendar.CheckBetween(atDate, Me.m_DataInizio, Me.m_DataFine)
        End Function

        Public Function IsValid() As Boolean
            Return Me.IsValid(Now)
        End Function

        Public Property IDUser As Integer
            Get
                Return GetID(Me.m_User, Me.m_IDUser)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDUser
                If oldValue = value Then Exit Property
                Me.m_IDUser = value
                Me.m_User = Nothing
                Me.DoChanged("IDUsers", value, oldValue)
            End Set
        End Property

        Public Property User As CUser
            Get
                If Me.m_User Is Nothing Then Me.m_User = Sistema.Users.GetItemById(Me.m_IDUser)
                Return Me.m_User
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.User
                If (oldValue = value) Then Exit Property
                Me.m_User = value
                Me.m_IDUser = GetID(value)
                Me.DoChanged("User", value, oldValue)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDConsulenti"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            Dim grp As CGroup = CQSPD.GruppoConsulenti
            If (Me.User IsNot Nothing) AndAlso (Not grp.Members.Contains(Me.User)) Then grp.Members.Add(Me.User)
            Return ret
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Me.m_IDUser = reader.Read("IDUser", Me.m_IDUser)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("IDUser", Me.IDUser)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("IDUser", Me.IDUser)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDUser" : Me.m_IDUser = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function GetModule() As CModule
            Return DMD.CQSPD.Consulenti.Module
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            CQSPD.Consulenti.UpdateCached(Me)
        End Sub

        Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Dim tmp As CConsulentePratica = obj
            Return Strings.Compare(Me.Nome, tmp.Nome, CompareMethod.Text)
        End Function

    End Class


End Class