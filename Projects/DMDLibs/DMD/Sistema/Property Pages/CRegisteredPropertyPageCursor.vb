Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    Public Class CRegisteredPropertyPageCursor
        Inherits DBObjectCursorBase(Of CRegisteredPropertyPage)

        Private m_ClassName As CCursorFieldObj(Of String)
        Private m_TabPageClass As CCursorFieldObj(Of String)
        Private m_Context As CCursorFieldObj(Of String)
        Private m_Priority As CCursorField(Of Integer)

        Public Sub New()
            Me.m_ClassName = New CCursorFieldObj(Of String)("ClassName")
            Me.m_TabPageClass = New CCursorFieldObj(Of String)("TabPageClass")
            Me.m_Context = New CCursorFieldObj(Of String)("Context")
            Me.m_Priority = New CCursorField(Of Integer)("Priority")
        End Sub

        Public ReadOnly Property ClassName As CCursorFieldObj(Of String)
            Get
                Return Me.m_ClassName
            End Get
        End Property

        Public ReadOnly Property TabPageClass As CCursorFieldObj(Of String)
            Get
                Return Me.m_TabPageClass
            End Get
        End Property

        Public ReadOnly Property Context As CCursorFieldObj(Of String)
            Get
                Return Me.m_Context
            End Get
        End Property

        Public ReadOnly Property Priority As CCursorField(Of Integer)
            Get
                Return Me.m_Priority
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CRegisteredPropertyPage
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_RegisteredTabPages"
        End Function

        'Protected Overrides Function GetCursorFields() As CCursorFieldsCollection
        '    Dim col As CCursorFieldsCollection
        '    col = MyBase.GetCursorFields
        '    col.Add(m_ClassName)
        '    col.Add(m_TabPageClass)
        '    col.Add(m_Context)
        '    col.Add(m_Priority)
        '    Return col
        'End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function
    End Class


End Class