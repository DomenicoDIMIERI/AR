Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    <Serializable> _
    Public Class CTabellaAssicurativa
        Inherits DBObject

        Private m_Nome As String
        Private m_Descrizione As String
        Private m_Dividendo As Integer
        Private m_IDAssicurazione As Integer
        Private m_Assicurazione As CAssicurazione
        Private m_NomeAssicurazione As String
        Private m_MeseScatto As Integer
        Private m_DataInizio As Nullable(Of Date)
        Private m_DataFine As Nullable(Of Date)
        Private m_Coefficienti As CCoefficientiAssicurativiCollection

        Public Sub New()
            Me.m_Nome = vbNullString
            Me.m_Descrizione = vbNullString
            Me.m_Dividendo = 1000
            Me.m_DataInizio = Nothing
            Me.m_DataFine = Nothing
            Me.m_IDAssicurazione = 0
            Me.m_Assicurazione = Nothing
            Me.m_NomeAssicurazione = ""
            Me.m_MeseScatto = -1
            Me.m_Coefficienti = Nothing
        End Sub

        Public Overrides Function GetModule() As CModule
            Return CQSPD.TabelleAssicurative.Module
        End Function

        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Descrizione
                If (oldValue = value) Then Exit Property
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
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

        Public Property Dividendo As Integer
            Get
                Return Me.m_Dividendo
            End Get
            Set(value As Integer)
                If (value <= 0) Then Throw New ArgumentOutOfRangeException("Dividendo deve essere un valore positivo")
                Dim oldValue As Integer = Me.m_Dividendo
                If (oldValue = value) Then Exit Property
                Me.m_Dividendo = value
                Me.DoChanged("Dividendo", value, oldValue)
            End Set
        End Property

        Public Function IsValid() As Boolean
            Return Me.IsValid(Now)
        End Function

        Public Function IsValid(ByVal at As Date) As Boolean
            Return DateUtils.CheckBetween(at, Me.m_DataInizio, Me.m_DataFine)
        End Function

        Public Property IDAssicurazione As Integer
            Get
                Return GetID(Me.m_Assicurazione, Me.m_IDAssicurazione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAssicurazione
                If (oldValue = value) Then Exit Property
                Me.m_IDAssicurazione = value
                Me.m_Assicurazione = Nothing
                Me.DoChanged("IDAssicurazione", value, oldValue)
            End Set
        End Property

        Public Property Assicurazione As CAssicurazione
            Get
                If (Me.m_Assicurazione Is Nothing) Then Me.m_Assicurazione = CQSPD.Assicurazioni.GetItemById(Me.m_IDAssicurazione)
                Return Me.m_Assicurazione
            End Get
            Set(value As CAssicurazione)
                Dim oldValue As CAssicurazione = Me.Assicurazione
                If (oldValue Is value) Then Exit Property
                Me.m_Assicurazione = value
                Me.m_IDAssicurazione = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeAssicurazione = value.Nome
                Me.DoChanged("Assicurazione", value, oldValue)
            End Set
        End Property

        Public Property NomeAssicurazione As String
            Get
                Return Me.m_NomeAssicurazione
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeAssicurazione
                If (oldValue = value) Then Exit Property
                Me.m_NomeAssicurazione = value
                Me.DoChanged("NomeAssicurazione", value, oldValue)
            End Set
        End Property

        Public Property MeseScatto As Integer
            Get
                Return Me.m_MeseScatto
            End Get
            Set(value As Integer)
                If (value < -1 Or value > 12) Then Throw New ArgumentOutOfRangeException("MeseScatto")
                Dim oldValue As Integer = Me.m_MeseScatto
                Me.m_MeseScatto = value
                Me.DoChanged("MeseScatto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce un oggetto CCoefficientiAssicurativiCollection relativo ai coefficienti di questa tabella
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Coefficienti As CCoefficientiAssicurativiCollection
            Get
                If (Me.m_Coefficienti Is Nothing) Then Me.m_Coefficienti = New CCoefficientiAssicurativiCollection(Me)
                Return Me.m_Coefficienti
            End Get
        End Property

        Public Function GetCoefficiente(ByVal Sesso As String, ByVal Anni As Integer, ByVal Durata As Integer) As Nullable(Of Double)
            Dim p As Nullable(Of Double)
            If (Me.m_Coefficienti IsNot Nothing) Then
                p = Me.m_Coefficienti.GetCoefficiente(Sesso, Anni, Durata)
            Else
                p = Me.GetConnection.ExecuteScalar("SELECT [C" & Durata & "] FROM [tbl_CoefficientiAssicurativi] WHERE ([Tabella]=" & GetID(Me) & ") AND ([Sesso]=" & DBUtils.DBString(Sesso) & " Or [Sesso]='U') AND ([anni]=" & Anni & ")")
            End If
            If p.HasValue Then p = p.Value / Me.m_Dividendo
            Return p
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (Me.m_Coefficienti IsNot Nothing) Then Me.m_Coefficienti.Save(force)
            Return ret
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_TabelleAssicurative"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_Dividendo = reader.Read("Dividendo", Me.m_Dividendo)
            Me.m_DataInizio = reader.Read("DataInizio", Me.m_DataInizio)
            Me.m_DataFine = reader.Read("DataFine", Me.m_DataFine)
            Me.m_IDAssicurazione = reader.Read("IDAssicurazione", Me.m_IDAssicurazione)
            Me.m_NomeAssicurazione = reader.Read("NomeAssicurazione", Me.m_NomeAssicurazione)
            Me.m_MeseScatto = reader.Read("MeseScatto", Me.m_MeseScatto)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("Dividendo", Me.m_Dividendo)
            writer.Write("DataInizio", Me.m_DataInizio)
            writer.Write("DataFine", Me.m_DataFine)
            writer.Write("IDAssicurazione", Me.IDAssicurazione)
            writer.Write("NomeAssicurazione", Me.m_NomeAssicurazione)
            writer.Write("MeseScatto", Me.m_MeseScatto)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("Dividendo", Me.m_Dividendo)
            writer.WriteAttribute("DataInizio", Me.m_DataInizio)
            writer.WriteAttribute("DataFine", Me.m_DataFine)
            writer.WriteAttribute("IDAssicurazione", Me.IDAssicurazione)
            writer.WriteAttribute("NomeAssicurazione", Me.m_NomeAssicurazione)
            writer.WriteAttribute("MeseScatto", Me.m_MeseScatto)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Dividendo" : Me.m_Dividendo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataInizio" : Me.m_DataInizio = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFine" : Me.m_DataFine = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDAssicurazione" : Me.m_IDAssicurazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAssicurazione" : Me.m_NomeAssicurazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MeseScatto" : Me.m_MeseScatto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            CQSPD.TabelleAssicurative.UpdateCached(Me)
        End Sub

    End Class


End Class