Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls


    Public Class CCampagnaCRMCursor
        Inherits DBObjectCursorPO(Of CCampagnaCRM)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Inizio As New CCursorField(Of Date)("Inizio")
        Private m_Fine As New CCursorField(Of Date)("Fine")
        Private m_TipoAssegnazione As New CCursorField(Of TipoCampagnaCRM)("TipoAssegnazione")
        Private m_TipoCampagna As New CCursorFieldObj(Of String)("TipoCampagna")
        Private m_Flags As New CCursorField(Of CampagnaCRMFlag)("Flags")

        Public Sub New()

        End Sub

        Public ReadOnly Property Inizio As CCursorField(Of Date)
            Get
                Return Me.m_Inizio
            End Get
        End Property

        Public ReadOnly Property Fine As CCursorField(Of Date)
            Get
                Return Me.m_Fine
            End Get
        End Property

        Public ReadOnly Property TipoAssegnazione As CCursorField(Of TipoCampagnaCRM)
            Get
                Return Me.m_TipoAssegnazione
            End Get
        End Property

        Public ReadOnly Property TipoCampagna As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoCampagna
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of CampagnaCRMFlag)
            Get
                Return Me.m_Flags
            End Get
        End Property


        Protected Overrides Function GetModule() As CModule
            Return CustomerCalls.CampagneCRM.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CustomerCalls.CRM.StatsDB
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CampagnaCRM"
        End Function


    End Class



End Class