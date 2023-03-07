Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Imports System.Drawing

Partial Class Office


    <Serializable>
    Public Class DocumentTemplateCursor
        Inherits DBObjectCursorPO(Of DocumentTemplate)

        Private m_Name As New CCursorFieldObj(Of String)("Name")
        Private m_SourceFile As New CCursorFieldObj(Of String)("SourceFile")
        Private m_ContextType As New CCursorFieldObj(Of String)("ContextType")
        Private m_Description As New CCursorFieldObj(Of String)("Description")
        Private m_PageFormatName As New CCursorFieldObj(Of String)("PageFormatName")
        Private m_PageFormatWidth As New CCursorField(Of Single)("PageFormatWidth")
        Private m_PageFormatHeight As New CCursorField(Of Single)("PageFormatHeight")

        Public Sub New()
        End Sub

        Public ReadOnly Property ContextType As CCursorFieldObj(Of String)
            Get
                Return Me.m_ContextType
            End Get
        End Property

        Public ReadOnly Property Name As CCursorFieldObj(Of String)
            Get
                Return Me.m_Name
            End Get
        End Property

        Public ReadOnly Property SourceFile As CCursorFieldObj(Of String)
            Get
                Return Me.m_SourceFile
            End Get
        End Property

        Public ReadOnly Property Description As CCursorFieldObj(Of String)
            Get
                Return Me.m_Description
            End Get
        End Property

        Public ReadOnly Property PageFormatName As CCursorFieldObj(Of String)
            Get
                Return Me.m_PageFormatName
            End Get
        End Property

        Public ReadOnly Property PageFormatWidth As CCursorField(Of Single)
            Get
                Return Me.m_PageFormatWidth
            End Get
        End Property

        Public ReadOnly Property PageFormatHeight As CCursorField(Of Single)
            Get
                Return Me.m_PageFormatHeight
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Office.Templates.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_DocumentiTemplates"
        End Function


    End Class

End Class