Imports DMD.Databases
Imports DMD.Sistema

Partial Class Office


    ''' <summary>
    ''' Cursore sulla tabella dei centralini
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PBXCursor
        Inherits DBObjectCursorPO(Of PBX)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Tipo As New CCursorFieldObj(Of String)("Tipo")
        Private m_Versione As New CCursorFieldObj(Of String)("Versione")
        Private m_DataInstallazione As New CCursorField(Of Date)("DataInstallazione")
        Private m_DataDismissione As New CCursorField(Of Date)("DataDismissione")
        Private m_Flags As New CCursorField(Of PBXFlags)("Flags")

        Public Sub New()
        End Sub

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Tipo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Tipo
            End Get
        End Property

        Public ReadOnly Property Versione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Versione
            End Get
        End Property

        Public ReadOnly Property DataInstallazione As CCursorField(Of Date)
            Get
                Return Me.m_DataInstallazione
            End Get
        End Property

        Public ReadOnly Property DataDismissione As CCursorField(Of Date)
            Get
                Return Me.m_DataDismissione
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of PBXFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return Office.PBXs.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficePBX"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.PBXs.Database
        End Function

    End Class



End Class