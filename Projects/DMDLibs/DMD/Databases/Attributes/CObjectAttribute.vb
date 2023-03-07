Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization
Imports DMD.Anagrafica

Public partial class Databases

    <Serializable> _
    Public Class CObjectAttribute
        Inherits DBObject

        Private m_ObjectID As Integer
        Private m_Object As Object
        Private m_AttributeName As String
        Private m_AttributeValue As String

        Public Sub New()
            m_ObjectID = 0
            m_Object = Nothing
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Sistema.Module
        End Function

        Public Property ObjectID As Integer
            Get
                Return GetID(Me.m_Object, Me.m_ObjectID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.ObjectID
                If oldValue = value Then Exit Property
                Me.m_Object = Nothing
                Me.m_ObjectID = value
                Me.DoChanged("ObjectID", value, oldValue)
            End Set
        End Property

        Public Property [Object] As Object
            Get
                Return Me.m_Object
            End Get
            Set(value As Object)
                Dim oldValue As Object = Me.m_Object
                If (oldValue Is value) Then Exit Property
                Me.m_Object = value
                Me.m_ObjectID = GetID(value)
                Me.DoChanged("Object", value, oldValue)
            End Set
        End Property

        Public Property AttributeName As String
            Get
                Return Me.m_AttributeName
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_AttributeName
                If (oldValue = value) Then Exit Property
                Me.m_AttributeName = value
                Me.DoChanged("AttributeName", value, oldValue)
            End Set
        End Property

        Public Property AttributeValue As String
            Get
                Return Me.m_AttributeValue
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_AttributeValue
                If (oldValue = value) Then Exit Property
                Me.m_AttributeValue = value
                Me.DoChanged("AttributeValue", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_ObjectAttributes"
        End Function

        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            reader.Read("Object", Me.m_ObjectID)
            reader.Read("AttributeName", Me.m_AttributeName)
            reader.Read("AttributeValue", Me.m_AttributeValue)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Object", Me.ObjectID)
            writer.Write("AttributeName", Me.m_AttributeName)
            writer.Write("AttributeValue", Me.m_AttributeValue)
            writer.Write("AttributeType", TypeName(Me.m_AttributeValue))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Object", Me.ObjectID)
            writer.WriteAttribute("AttributeName", Me.m_AttributeName)
            writer.WriteAttribute("AttributeType", TypeName(Me.m_AttributeValue))
            MyBase.XMLSerialize(writer)
            writer.WriteTag("AttributeValue", Me.m_AttributeValue)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Object" : Me.m_ObjectID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "AttributeName" : Me.m_AttributeName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "AttributeType"
                Case "AttributeValue"
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Overrides Function ToString() As String
            Dim ret As String
            ret = "[" & AttributeName & "="
            ret &= m_AttributeValue
            ret &= "]"
            Return ret
        End Function

    End Class



End Class

