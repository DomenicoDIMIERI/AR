Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    Public Class CModuleActionsCursor
        Inherits DBObjectCursorBase(Of CModuleAction)

        Private m_ModuleName As CCursorFieldObj(Of String)
        Private m_AuthorizationName As CCursorFieldObj(Of String)
        Private m_AuthorizationDescription As CCursorFieldObj(Of String)
        Private m_Visible As CCursorField(Of Boolean)
        Private m_ClassHandler As CCursorFieldObj(Of String)

        Public Sub New()
            Me.m_ModuleName = New CCursorFieldObj(Of String)("Modulo")
            Me.m_AuthorizationName = New CCursorFieldObj(Of String)("AuthorizationName")
            Me.m_AuthorizationDescription = New CCursorFieldObj(Of String)("AuthorizationDescription")
            Me.m_Visible = New CCursorField(Of Boolean)("Visible")
            Me.m_ClassHandler = New CCursorFieldObj(Of String)("ClassHandler")
        End Sub

        Public ReadOnly Property ModuleName As CCursorFieldObj(Of String)
            Get
                Return Me.m_ModuleName
            End Get
        End Property

        Public ReadOnly Property ClassHandler As CCursorFieldObj(Of String)
            Get
                Return Me.m_ClassHandler
            End Get
        End Property

        Public ReadOnly Property AuthorizationName As CCursorFieldObj(Of String)
            Get
                Return Me.m_AuthorizationName
            End Get
        End Property

        Public ReadOnly Property AuthorizationDescription As CCursorFieldObj(Of String)
            Get
                Return Me.m_AuthorizationDescription
            End Get
        End Property

        Public ReadOnly Property Visible As CCursorField(Of Boolean)
            Get
                Return Me.m_Visible
            End Get
        End Property
        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CModuleAction
        End Function

        'Protected Overrides Function GetCursorFields() As CCursorFieldsCollection
        '    Dim col As CCursorFieldsCollection
        '    col = MyBase.GetCursorFields
        '    Call col.Add(m_ModuleName)
        '    Call col.Add(m_AuthorizationName)
        '    Call col.Add(m_AuthorizationDescription)
        '    Call col.Add(m_Visible)
        '    Call col.Add(m_ClassHandler)
        '    Return col
        'End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_DefinedAuthorizations"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function
    End Class


End Class