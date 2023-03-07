Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Public partial class Databases


    <Serializable>
    Public Class CDBEntityField
        Inherits CDBObject

        <NonSerialized> Private m_Owner As CDBEntity
        Private m_IsAutoIncrement As Boolean
        Private m_DataType As System.Type
        Private m_Length As Integer
        Private m_AutoIncrementSeed As Integer
        Private m_AutoIncrementStep As Integer
        Private m_AllowDBNull As Boolean
        Private m_Caption As String
        Private m_Ordinal As Integer
        Private m_Namespace As String
        Private m_Expression As String
        Private m_DefaultValue As Object
        Private m_ColumnMapping As String
        Private m_Prefix As String
        Private m_ReadOnly As Boolean
        Private m_Unique As Boolean

        Public Sub New()
            Me.m_DefaultValue = DBNull.Value
            Me.m_Owner = Nothing
            Me.m_IsAutoIncrement = False
            Me.m_DataType = Nothing
            Me.m_Length = 0
            Me.m_AutoIncrementSeed = 0
            Me.m_AutoIncrementStep = 1
            Me.m_AllowDBNull = True
            Me.m_Caption = vbNullString
            Me.m_Ordinal = 0
            Me.m_Namespace = vbNullString
            Me.m_Expression = vbNullString
            Me.m_ColumnMapping = vbNullString
            Me.m_Prefix = vbNullString
            Me.m_ReadOnly = False
            Me.m_Unique = False
        End Sub

        Public Sub New(ByVal name As String, ByVal dataType As System.Type)
            Me.New()
            Me.SetName(name)
            Me.m_DataType = dataType
        End Sub

        Public Sub New(ByVal name As String, ByVal dataType As System.TypeCode)
            Me.New(name, Types.GetTypeFromCode(dataType))
        End Sub

        Public ReadOnly Property Owner As CDBEntity
            Get
                Return Me.m_Owner
            End Get
        End Property
        Protected Friend Sub SetOwner(ByVal value As CDBEntity)
            Me.m_Owner = value
            If (value IsNot Nothing) Then Me.SetConnection(value.Connection)
        End Sub

        Public Property MaxLength As Integer
            Get
                Return Me.m_Length
            End Get
            Set(value As Integer)
                Me.m_Length = value
            End Set
        End Property

        Public Property DataType As System.Type
            Get
                Return Me.m_DataType
            End Get
            Set(value As System.Type)
                Me.m_DataType = value
            End Set
        End Property


        Public Property AutoIncrement As Boolean
            Get
                Return Me.m_IsAutoIncrement
            End Get
            Set(value As Boolean)
                Me.m_IsAutoIncrement = value
            End Set
        End Property

        Public Property AutoIncrementSeed As Integer
            Get
                Return Me.m_AutoIncrementSeed
            End Get
            Set(value As Integer)
                Me.m_AutoIncrementSeed = value
            End Set
        End Property

        Public Property AutoIncrementStep As Integer
            Get
                Return Me.m_AutoIncrementStep
            End Get
            Set(value As Integer)
                Me.m_AutoIncrementStep = value
            End Set
        End Property

        Public Property AllowDBNull As Boolean
            Get
                Return Me.m_AllowDBNull
            End Get
            Set(value As Boolean)
                Me.m_AllowDBNull = value
            End Set
        End Property

        Public ReadOnly Property Ordinal As Integer
            Get
                Return Me.m_Ordinal
            End Get
        End Property
        Public Sub SetOrdinal(ByVal value As Integer)
            Me.m_Ordinal = value
        End Sub

        Public Property Prefix As String
            Get
                Return Me.m_Prefix
            End Get
            Set(value As String)
                Me.m_Prefix = Trim(value)
            End Set
        End Property

        Public Property [ReadOnly] As Boolean
            Get
                Return Me.m_ReadOnly
            End Get
            Set(value As Boolean)
                Me.m_ReadOnly = value
            End Set
        End Property

        Public Property Unique As Boolean
            Get
                Return Me.m_Unique
            End Get
            Set(value As Boolean)
                Me.m_Unique = value
            End Set
        End Property

        Public Property [Namespace] As String
            Get
                Return Me.m_Namespace
            End Get
            Set(value As String)
                Me.m_Namespace = Trim(value)
            End Set
        End Property

        Public Property Expression As String
            Get
                Return Me.m_Expression
            End Get
            Set(value As String)
                Me.m_Expression = Trim(value)
            End Set
        End Property

        Public Property DefaultValue As Object
            Get
                Return Me.m_DefaultValue
            End Get
            Set(value As Object)
                Me.m_DefaultValue = value
            End Set
        End Property

        Public Property ColumnMapping As String
            Get
                Return Me.m_ColumnMapping
            End Get
            Set(value As String)
                Me.m_ColumnMapping = Trim(value)
            End Set
        End Property

        Public Property Caption As String
            Get
                Return Me.m_Caption
            End Get
            Set(value As String)
                Me.m_Caption = value
            End Set
        End Property


        Protected Overrides Sub CreateInternal1()
            Me.Connection.CreateField(Me)
        End Sub

        Protected Overrides Sub DropInternal1()
            Me.Connection.DropField(Me)
        End Sub

        Protected Overrides Sub UpdateInternal1()
            Me.Connection.UpdateField(Me)
        End Sub

        Protected Overrides Sub RenameItnernal(newName As String)
            Throw New NotImplementedException
        End Sub

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IsAutoIncrement", Me.m_IsAutoIncrement)
            writer.WriteAttribute("DataType", Me.m_DataType.FullName)
            writer.WriteAttribute("Length", Me.m_Length)
            writer.WriteAttribute("AutoIncrementSeed", Me.m_AutoIncrementSeed)
            writer.WriteAttribute("AutoIncrementStep", Me.m_AutoIncrementStep)
            writer.WriteAttribute("AllowDBNull", Me.m_AllowDBNull)
            writer.WriteAttribute("Caption", Me.m_Caption)
            writer.WriteAttribute("Ordinal", Me.m_Ordinal)
            writer.WriteAttribute("Namespace", Me.m_Namespace)
            writer.WriteAttribute("Expression", Me.m_Expression)
            writer.WriteAttribute("DefaultValue", Sistema.Formats.ToString(Me.m_DefaultValue))
            writer.WriteAttribute("ColumnMapping", Me.m_ColumnMapping)
            writer.WriteAttribute("Prefix", Me.m_Prefix)
            writer.WriteAttribute("ReadOnly", Me.m_ReadOnly)
            writer.WriteAttribute("Unique", Me.m_Unique)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IsAutoIncrement" : Me.m_IsAutoIncrement = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "DataType" : Me.m_DataType = DMD.Sistema.Types.GetType(XML.Utils.Serializer.DeserializeString(fieldValue))
                Case "Length" : Me.m_Length = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "AutoIncrementSeed" : Me.m_AutoIncrementSeed = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "AutoIncrementStep" : Me.m_AutoIncrementStep = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "AllowDBNull" : Me.m_AllowDBNull = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Caption" : Me.m_Caption = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Ordinal" : Me.m_Ordinal = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Namespace" : Me.m_Namespace = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Expression" : Me.m_Expression = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DefaultValue" : Me.m_DefaultValue = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ColumnMapping" : Me.m_ColumnMapping = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Prefix" : Me.m_Prefix = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ReadOnly" : Me.m_ReadOnly = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Unique" : Me.m_Unique = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

    End Class

End Class



