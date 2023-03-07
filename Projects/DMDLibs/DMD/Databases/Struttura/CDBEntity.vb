Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Public partial class Databases

 
    Public MustInherit Class CDBEntity
        Inherits CDBObject

        Private m_Catalog As String
        Private m_Schema As String
        Private m_Type As String
        Private m_Guid As String
        Private m_PropID As String
        Private m_Description As String
        Private m_DateCreated As Nullable(Of Date)
        Private m_DateModified As Nullable(Of Date)
        Private m_Fields As CDBEntityFields
        Private m_Constraints As CDBConstraintsCollection
        Private m_IsHidden As Boolean
        Private m_TrackChanges As Boolean

        Public Sub New()
            Me.m_Catalog = vbNullString
            Me.m_Schema = vbNullString
            Me.m_Type = vbNullString
            Me.m_Guid = vbNullString
            Me.m_PropID = vbNullString
            Me.m_Description = vbNullString
            Me.m_DateCreated = Nothing
            Me.m_DateModified = Nothing
            Me.m_Fields = Nothing
            Me.m_Constraints = Nothing
            Me.m_IsHidden = False
            Me.m_TrackChanges = False
        End Sub

        Public Sub New(ByVal name As String)
            Me.New
            Me.SetName(name)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che impone al sistema di tenere traccia di tutte le modifiche fatte ai campi 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TrackChanges As Boolean
            Get
                Return Me.m_TrackChanges
            End Get
            Set(value As Boolean)
                Me.m_TrackChanges = value
            End Set
        End Property



        Public ReadOnly Property Fields As CDBEntityFields
            Get
                SyncLock Me
                    If (Me.m_Fields Is Nothing) Then Me.m_Fields = New CDBEntityFields(Me)
                    Return Me.m_Fields
                End SyncLock
            End Get
        End Property

        Public Function CreateAdapter() As System.Data.IDataAdapter
            Dim ret As New System.Text.StringBuilder
            ret.Append("SELECT * FROM [")
            ret.Append(Me.Name)
            ret.Append("]")
            Return Me.Connection.CreateAdapter(ret.ToString)
        End Function

        Public Property Description As String
            Get
                Return Me.m_Description
            End Get
            Set(value As String)
                Me.m_Description = value
            End Set
        End Property

        Public Property Schema As String
            Get
                Return Me.m_Schema
            End Get
            Set(value As String)
                Me.m_Schema = Trim(value)
            End Set
        End Property

        Public Property Guid As String
            Get
                Return Me.m_Guid
            End Get
            Set(value As String)
                Me.m_Guid = Trim(value)
            End Set
        End Property

        Public Property PropID As String
            Get
                Return Me.m_PropID
            End Get
            Set(value As String)
                Me.m_PropID = Trim(value)
            End Set
        End Property

        Public Property Type As String
            Get
                Return Me.m_Type
            End Get
            Set(value As String)
                Me.m_Type = Trim(value)
            End Set
        End Property

        Public Property Catalog As String
            Get
                Return Me.m_Catalog
            End Get
            Set(value As String)
                Me.m_Catalog = Trim(value)
            End Set
        End Property

        Public Property DateCreated As Nullable(Of Date)
            Get
                Return Me.m_DateCreated
            End Get
            Set(value As Nullable(Of Date))
                Me.m_DateCreated = value
            End Set
        End Property

        Public Property DateModified As Nullable(Of Date)
            Get
                Return Me.m_DateModified
            End Get
            Set(value As Nullable(Of Date))
                Me.m_DateModified = value
            End Set
        End Property

        Public Property IsHidden As Boolean
            Get
                Return Me.m_IsHidden
            End Get
            Set(value As Boolean)
                Me.m_IsHidden = value
            End Set
        End Property

        Public ReadOnly Property Constraints As CDBConstraintsCollection
            Get
                If (Me.m_Constraints Is Nothing) Then Me.m_Constraints = New CDBConstraintsCollection(Me)
                Return Me.m_Constraints
            End Get
        End Property

        Protected Overrides Sub OnConnectionClosed(sender As Object, e As EventArgs)
            'Me.Dispose()
        End Sub

        Public Overrides Function IsChanged() As Boolean
            If MyBase.IsChanged Then Return True
            If (Me.m_Fields Is Nothing) Then Return False
            For Each field As CDBEntityField In Me.m_Fields
                If field.IsChanged Then Return True
            Next
            Return False
        End Function

        Public ReadOnly Property InternalName As String
            Get
                If (Me.Connection IsNot Nothing) Then Return Me.Connection.GetInternalTableName(Me)
                Return Me.Name
            End Get
        End Property

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Catalog", Me.m_Catalog)
            writer.WriteAttribute("Schema", Me.m_Schema)
            writer.WriteAttribute("Type", Me.m_Type)
            writer.WriteAttribute("Guid", Me.m_Guid)
            writer.WriteAttribute("PropID", Me.m_PropID)
            writer.WriteAttribute("Description", Me.m_Description)
            writer.WriteAttribute("DateCreated", Me.m_DateCreated)
            writer.WriteAttribute("DateModified", Me.m_DateModified)
            writer.WriteAttribute("IsHidden", Me.m_IsHidden)
            writer.WriteAttribute("TrackChanged", Me.m_TrackChanges)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Fields", Me.Fields)
            writer.WriteTag("Constraints", Me.Constraints)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Catalog" : Me.m_Catalog = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Schema" : Me.m_Schema = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Type" : Me.m_Type = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Guid" : Me.m_Guid = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PropID" : Me.m_PropID = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Description" : Me.m_Description = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DateCreated" : Me.m_DateCreated = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DateModified" : Me.m_DateModified = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IsHidden" : Me.m_IsHidden = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "TrackChanged" : Me.m_TrackChanges = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Fields" : Me.m_Fields = fieldValue : Me.m_Fields.SetOwner(Me)
                Case "Constraints" : Me.m_Constraints = fieldValue : Me.m_Constraints.SetOwner(Me)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

    End Class

End Class


