Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    ''' <summary>
    ''' Cursore sulla tabella dei moduli definiti per gruppo
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CModuleXGroupCursor
        Inherits DBObjectCursorBase(Of CModuleXGroup)

        Private m_ModuleID As New CCursorField(Of Integer)("Module")
        Private m_GroupID As New CCursorField(Of Integer)("Group")
        Private m_Allow As New CCursorField(Of Boolean)("Allow")
        Private m_Negate As New CCursorField(Of Boolean)("Negate")

        Public Sub New()
        End Sub

        Public ReadOnly Property ModuleID As CCursorField(Of Integer)
            Get
                Return Me.m_ModuleID
            End Get
        End Property

        Public ReadOnly Property GroupID As CCursorField(Of Integer)
            Get
                Return Me.m_GroupID
            End Get
        End Property

        Public ReadOnly Property Allow As CCursorField(Of Boolean)
            Get
                Return Me.m_Allow
            End Get
        End Property

        Public ReadOnly Property Negate As CCursorField(Of Boolean)
            Get
                Return Me.m_Negate
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_ModulesXGroup"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function
    End Class


End Class