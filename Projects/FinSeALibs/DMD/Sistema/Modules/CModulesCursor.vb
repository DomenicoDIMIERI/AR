Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    Public Class CModulesCursor
        Inherits DBObjectCursor(Of CModule)

        Private m_ParentID As CCursorField(Of Integer)
        Private m_Posizione As CCursorField(Of Integer)
        Private m_ModuleName As CCursorFieldObj(Of String)
        Private m_ClassHandler As CCursorFieldObj(Of String)
        Private m_ConfigClass As CCursorFieldObj(Of String)
        Private m_DisplayName As CCursorFieldObj(Of String)
        Private m_ModulePath As CCursorFieldObj(Of String)
        Private m_IconPath As CCursorFieldObj(Of String)
        Private m_Description As CCursorFieldObj(Of String)
        Private m_Builtin As CCursorField(Of Boolean)
        Private m_Visible As CCursorField(Of Boolean)

        Public Sub New()
            Me.m_ParentID = New CCursorField(Of Integer)("Parent")
            Me.m_Posizione = New CCursorField(Of Integer)("Posizione")
            Me.m_ModuleName = New CCursorFieldObj(Of String)("ModuleName")
            Me.m_ClassHandler = New CCursorFieldObj(Of String)("ClassHandler")
            Me.m_ConfigClass = New CCursorFieldObj(Of String)("ConfigClass")
            Me.m_DisplayName = New CCursorFieldObj(Of String)("DisplayName")
            Me.m_ModulePath = New CCursorFieldObj(Of String)("ModulePath")
            Me.m_IconPath = New CCursorFieldObj(Of String)("IconPath")
            Me.m_Description = New CCursorFieldObj(Of String)("Description")
            Me.m_Builtin = New CCursorField(Of Boolean)("Builtin")
            Me.m_Visible = New CCursorField(Of Boolean)("Visible")
        End Sub
        Public ReadOnly Property ParentID As CCursorField(Of Integer)
            Get
                Return Me.m_ParentID
            End Get
        End Property

        Public ReadOnly Property Posizione As CCursorField(Of Integer)
            Get
                Return Me.m_Posizione
            End Get
        End Property

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

        Public ReadOnly Property ConfigClass As CCursorFieldObj(Of String)
            Get
                Return Me.m_ConfigClass
            End Get
        End Property

        Public ReadOnly Property DisplayName As CCursorFieldObj(Of String)
            Get
                Return Me.m_DisplayName
            End Get
        End Property

        Public ReadOnly Property ModulePath As CCursorFieldObj(Of String)
            Get
                Return Me.m_ModulePath
            End Get
        End Property

        Public ReadOnly Property IconPath As CCursorFieldObj(Of String)
            Get
                Return Me.m_IconPath
            End Get
        End Property

        Public ReadOnly Property Description As CCursorFieldObj(Of String)
            Get
                Return Me.m_Description
            End Get
        End Property

        Public ReadOnly Property Builtin As CCursorField(Of Boolean)
            Get
                Return Me.m_Builtin
            End Get
        End Property

        Public ReadOnly Property Visible As CCursorField(Of Boolean)
            Get
                Return Me.m_Visible
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CModule
        End Function

        'Protected Overrides Function GetCursorFields() As CCursorFieldsCollection
        '    Dim col As CCursorFieldsCollection
        '    col = MyBase.GetCursorFields
        '    Call col.Add(m_ParentID)
        '    Call col.Add(m_Posizione)
        '    Call col.Add(m_ModuleName)
        '    Call col.Add(m_ClassHandler)
        '    Call col.Add(m_ConfigClass)
        '    Call col.Add(m_DisplayName)
        '    Call col.Add(m_ModulePath)
        '    Call col.Add(m_IconPath)
        '    Call col.Add(m_Description)
        '    Call col.Add(m_Builtin)
        '    Call col.Add(m_Visible)
        '    Return col
        'End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Modules"
        End Function


        Protected Overrides Function GetModule() As CModule
            Return Sistema.Modules.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

    End Class


End Class