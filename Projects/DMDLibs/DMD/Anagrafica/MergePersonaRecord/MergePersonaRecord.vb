Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    <Serializable> _
    Public Class CMergePersonaRecord
        Inherits DBObjectBase

        Public NomeTabella As String
        Public RecordID As Integer
        Public FieldName As String
        Public params As CKeyCollection

        Public Sub New()
            Me.NomeTabella = ""
            Me.RecordID = 0
            Me.FieldName = ""
            Me.params = New CKeyCollection
        End Sub

        Public Overrides Function ToString() As String
            Return Me.NomeTabella & "[" & Me.RecordID & "]"
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_MergePersonaRecords"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.NomeTabella = reader.Read("NomeTabella", Me.NomeTabella)
            Me.RecordID = reader.Read("RecordID", Me.RecordID)
            Me.FieldName = reader.Read("FieldName", Me.FieldName)
            Try
                Me.params = XML.Utils.Serializer.Deserialize(reader.Read("params", ""))
            Catch ex As Exception
                Me.params = New CKeyCollection
            End Try
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("NomeTabella", Me.NomeTabella)
            writer.Write("RecordID", Me.RecordID)
            writer.Write("FieldName", Me.FieldName)
            writer.Write("params", XML.Utils.Serializer.Serialize(Me.params))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("NomeTabella", Me.NomeTabella)
            writer.WriteAttribute("RecordID", Me.RecordID)
            writer.WriteAttribute("FieldName", Me.FieldName)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("params", Me.params)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "NomeTabella" : Me.NomeTabella = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "FieldName" : Me.FieldName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "RecordID" : Me.RecordID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "params" : Me.params = CType(fieldValue, CKeyCollection)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub



    End Class


End Class