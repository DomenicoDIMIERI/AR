Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    <Serializable> _
    Public Class CMergePersona
        Inherits DBObject

        Private m_IDPersona1 As Integer
        Private m_Persona1 As CPersona
        Private m_NomePersona1 As String

        Private m_IDPersona2 As Integer
        Private m_Persona2 As CPersona
        Private m_NomePersona2 As String

        Private m_DataOperazione As Date
        Private m_IDOperatore As Integer
        Private m_Operatore As CUser
        Private m_NomeOperatore As String

        Private m_TabelleModificate As CCollection(Of CMergePersonaRecord)

        Public Sub New()
            Me.m_IDPersona1 = 0
            Me.m_Persona1 = Nothing
            Me.m_NomePersona1 = ""

            Me.m_IDPersona2 = 0
            Me.m_Persona2 = Nothing
            Me.m_NomePersona2 = ""

            Me.m_DataOperazione = Nothing
            Me.m_IDOperatore = 0
            Me.m_Operatore = Nothing
            Me.m_NomeOperatore = ""

            Me.m_TabelleModificate = Nothing
        End Sub

        Public Property IDPersona1 As Integer
            Get
                Return GetID(Me.m_Persona1, Me.m_IDPersona1)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPersona1
                If (oldValue = value) Then Exit Property
                Me.m_IDPersona1 = value
                Me.m_Persona1 = Nothing
                Me.DoChanged("IDPersona1", value, oldValue)
            End Set
        End Property

        Public Property Persona1 As CPersona
            Get
                If (Me.m_Persona1 Is Nothing) Then Me.m_Persona1 = Anagrafica.Persone.GetItemById(Me.m_IDPersona1)
                Return Me.m_Persona1
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Persona1
                If (oldValue Is value) Then Exit Property
                Me.m_Persona1 = value
                Me.m_IDPersona1 = GetID(value)
                Me.m_NomePersona1 = ""
                If (value IsNot Nothing) Then Me.m_NomePersona1 = value.Nominativo
                Me.DoChanged("NomePersona1", value, oldValue)
            End Set
        End Property

        Public Property NomePersona1 As String
            Get
                Return Me.m_NomePersona1
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomePersona1
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomePersona1 = value
                Me.DoChanged("NomePersona1", value, oldValue)
            End Set
        End Property

        Public Property IDPersona2 As Integer
            Get
                Return GetID(Me.m_Persona2, Me.m_IDPersona2)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPersona2
                If (oldValue = value) Then Exit Property
                Me.m_IDPersona2 = value
                Me.m_Persona2 = Nothing
                Me.DoChanged("IDPersona2", value, oldValue)
            End Set
        End Property

        Public Property Persona2 As CPersona
            Get
                If (Me.m_Persona2 Is Nothing) Then Me.m_Persona2 = Anagrafica.Persone.GetItemById(Me.m_IDPersona2)
                Return Me.m_Persona2
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Persona2
                If (oldValue Is value) Then Exit Property
                Me.m_Persona2 = value
                Me.m_IDPersona2 = GetID(value)
                Me.m_NomePersona2 = ""
                If (value IsNot Nothing) Then Me.m_NomePersona2 = value.Nominativo
                Me.DoChanged("NomePersona2", value, oldValue)
            End Set
        End Property

        Public Property NomePersona2 As String
            Get
                Return Me.m_NomePersona2
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomePersona2
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomePersona2 = value
                Me.DoChanged("NomePersona2", value, oldValue)
            End Set
        End Property

        
        Public Property DataOperazione As Date
            Get
                Return Me.m_DataOperazione
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_DataOperazione
                If (DateUtils.Compare(oldValue, value) = 0) Then Exit Property
                Me.m_DataOperazione = value
                Me.DoChanged("DataOperazione", value, oldValue)
            End Set
        End Property

        Public Property IDOperatore As Integer
            Get
                Return GetID(Me.m_Operatore, Me.m_IDOperatore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOperatore
                If (oldValue = value) Then Exit Property
                Me.m_IDOperatore = value
                Me.m_Operatore = Nothing
                Me.DoChanged("IDOperatore", value, oldValue)
            End Set
        End Property

        Public Property Operatore As CUser
            Get
                If (Me.m_Operatore Is Nothing) Then Me.m_Operatore = Sistema.Users.GetItemById(Me.m_IDOperatore)
                Return Me.m_Operatore
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Operatore
                If (oldValue Is value) Then Exit Property
                Me.m_IDOperatore = GetID(value)
                Me.m_Operatore = value
                Me.m_NomeOperatore = ""
                If (value IsNot Nothing) Then Me.m_NomeOperatore = value.Nominativo
                Me.DoChanged("Operatore", value, oldValue)
            End Set
        End Property

        Public Property NomeOperatore As String
            Get
                Return Me.m_NomeOperatore
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeOperatore
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeOperatore = value
                Me.DoChanged("NomeOperatore", value, oldValue)
            End Set
        End Property


        Public ReadOnly Property TabelleModificate As CCollection(Of CMergePersonaRecord)
            Get
                If (Me.m_TabelleModificate Is Nothing) Then Me.m_TabelleModificate = New CCollection(Of CMergePersonaRecord)
                Return Me.m_TabelleModificate
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return "Unione tra " & Me.m_NomePersona1 & "[" & Me.m_IDPersona1 & "] e " & Me.m_NomePersona2 & "[" & Me.m_IDPersona2 & "]"
        End Function

        Public Overrides Function GetModule() As CModule
            Return Anagrafica.MergePersone.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_MergePersone"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDPersona1 = reader.Read("IDPersona1", Me.m_IDPersona1)
            Me.m_NomePersona1 = reader.Read("NomePersona1", Me.m_NomePersona1)

            Me.m_IDPersona2 = reader.Read("IDPersona2", Me.m_IDPersona2)
            Me.m_NomePersona2 = reader.Read("NomePersona2", Me.m_NomePersona2)

            Me.m_DataOperazione = reader.Read("DataOperazione", Me.m_DataOperazione)

            Me.m_IDOperatore = reader.Read("IDOperatore", Me.m_IDOperatore)
            Me.m_NomeOperatore = reader.Read("NomeOperatore", Me.m_NomeOperatore)

            Try
                Me.m_TabelleModificate = New CCollection(Of CMergePersonaRecord)
                Me.m_TabelleModificate.AddRange(XML.Utils.Serializer.Deserialize(reader.Read("Tabelle", "")))
            Catch ex As Exception
                Me.m_TabelleModificate = Nothing
            End Try

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDPersona1", Me.IDPersona1)
            writer.Write("NomePersona1", Me.m_NomePersona1)

            writer.Write("IDPersona2", Me.IDPersona2)
            writer.Write("NomePersona2", Me.m_NomePersona2)

            writer.Write("DataOperazione", Me.m_DataOperazione)

            writer.Write("IDOperatore", Me.IDOperatore)
            writer.Write("NomeOperatore", Me.m_NomeOperatore)

            writer.Write("Tabelle", XML.Utils.Serializer.Serialize(Me.TabelleModificate))
             

            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDPersona1", Me.IDPersona1)
            writer.WriteAttribute("NomePersona1", Me.m_NomePersona1)

            writer.WriteAttribute("IDPersona2", Me.IDPersona2)
            writer.WriteAttribute("NomePersona2", Me.m_NomePersona2)

            writer.WriteAttribute("DataOperazione", Me.m_DataOperazione)

            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("NomeOperatore", Me.m_NomeOperatore)


            MyBase.XMLSerialize(writer)

            writer.WriteTag("Tabelle", Me.TabelleModificate)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDPersona1" : Me.m_IDPersona1 = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePersona1" : Me.m_NomePersona1 = XML.Utils.Serializer.DeserializeString(fieldValue)

                Case "IDPersona2" : Me.m_IDPersona2 = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePersona2" : Me.m_NomePersona2 = XML.Utils.Serializer.DeserializeString(fieldValue)

                Case "DataOperazione" : Me.m_DataOperazione = XML.Utils.Serializer.DeserializeDate(fieldValue)

                Case "IDOperatore" : Me.m_IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatore" : Me.m_NomeOperatore = XML.Utils.Serializer.DeserializeString(fieldValue)

                Case "Tabelle"
                    Me.m_TabelleModificate = New CCollection(Of CMergePersonaRecord)
                    Me.m_TabelleModificate.AddRange(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Function GetAffectedRecors(ByVal tableName As String, ByVal fieldName As String) As String
            Dim ret As New System.Text.StringBuilder
            For Each rec As CMergePersonaRecord In Me.TabelleModificate
                If (rec.NomeTabella = tableName AndAlso rec.RecordID <> 0 AndAlso rec.FieldName = fieldName) Then
                    If (ret.Length > 0) Then ret.Append(",")
                    ret.Append(DBUtils.DBNumber(rec.RecordID))
                End If
            Next
            Return ret.ToString
        End Function

        Public Function Add(ByVal tableName As String, ByVal fieldName As String, ByVal recordID As Integer) As CMergePersonaRecord
            Dim rec As New CMergePersonaRecord
            rec.NomeTabella = Trim(tableName)
            rec.FieldName = Trim(fieldName)
            rec.RecordID = recordID
            Me.TabelleModificate.Add(rec)
            Me.SetChanged(True)
            Return rec
        End Function

    End Class


End Class