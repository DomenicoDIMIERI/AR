Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization
Imports DMD.Anagrafica

Public partial class Databases

    Public Class CObjectAttributeCursor
        Inherits DBObjectCursor(Of CObjectAttribute)

        Private m_ObjectID As CCursorField(Of Integer)
        Private m_ObjectType As CCursorFieldObj(Of String)
        Private m_AttributeName As CCursorFieldObj(Of String)
        Private m_AttributeType As CCursorField(Of Integer)
        Private m_AttributeValue As CCursorFieldObj(Of String)

        Public Sub New()
            Me.m_ObjectID = New CCursorField(Of Integer)("IDObject")
            Me.m_ObjectType = New CCursorFieldObj(Of String)("ObjectType")
            Me.m_AttributeName = New CCursorFieldObj(Of String)("AttributeName")
            Me.m_AttributeType = New CCursorField(Of Integer)("AttributeType")
            Me.m_AttributeValue = New CCursorFieldObj(Of String)("AttributeValue")
        End Sub



        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ObjectAttributes"
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CObjectAttribute
        End Function

        Public ReadOnly Property ObjectID As CCursorField(Of Integer)
            Get
                Return Me.m_ObjectID
            End Get
        End Property

        Public ReadOnly Property ObjectType As CCursorFieldObj(Of String)
            Get
                Return Me.m_ObjectType
            End Get
        End Property

        Public ReadOnly Property AttributeName As CCursorFieldObj(Of String)
            Get
                Return Me.m_AttributeName
            End Get
        End Property

        Public ReadOnly Property AttributeType As CCursorField(Of Integer)
            Get
                Return Me.m_AttributeType
            End Get
        End Property


        Public ReadOnly Property AttributeValue As CCursorFieldObj(Of String)
            Get
                Return Me.m_AttributeValue
            End Get
        End Property


        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function
    End Class

End Class

