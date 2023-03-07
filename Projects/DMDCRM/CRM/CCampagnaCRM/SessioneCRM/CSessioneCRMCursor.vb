Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls


    Public Class CSessioneCRMCursor
        Inherits DBObjectCursorPO(Of CSessioneCRM)

        Private m_IDCampagnaCRM As New CCursorField(Of Integer)("IDCampagnaCRM")
        Private m_IDUtente As New CCursorField(Of Integer)("IDUtente")
        Private m_NomeUtente As New CCursorFieldObj(Of String)("NomeUtente")
        Private m_Inizio As New CCursorField(Of Date)("Inizio")
        Private m_Fine As New CCursorField(Of Date)("Fine")
        Private m_LastUpdated As New CCursorField(Of Date)("LastUpdated")
        Private m_NumeroTelefonateRisposte As New CCursorField(Of Integer)("NumeroTelefonateRisposte")
        Private m_NumeroTelefonateNonRisposte As New CCursorField(Of Integer)("NumeroTelefonateNonRisposte")
        Private m_MinutiConversazione As New CCursorField(Of Integer)("MinutiConversazione")
        Private m_MinutiAttesa As New CCursorField(Of Integer)("MinutiAttesa")
        Private m_NumeroAppuntamentiFissati As New CCursorField(Of Integer)("NumeroAppuntamentiFissati")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_dmdpage As New CCursorFieldObj(Of String)("dmdpage")
        Private m_IDSupervisore As New CCursorField(Of Integer)("IDSupervisore")
        Private m_NomeSupervisore As New CCursorFieldObj(Of String)("NomeSupervisore")

        Public Sub New()

        End Sub

        Public ReadOnly Property LastUpdated As CCursorField(Of DateTime)
            Get
                Return Me.m_LastUpdated
            End Get
        End Property

        Public ReadOnly Property IDSupervisore As CCursorField(Of Integer)
            Get
                Return Me.m_IDSupervisore
            End Get
        End Property

        Public ReadOnly Property NomeSupervisore As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeSupervisore
            End Get
        End Property


        Public ReadOnly Property dmdpage As CCursorFieldObj(Of String)
            Get
                Return Me.m_dmdpage
            End Get
        End Property

        Public ReadOnly Property IDCampagnaCRM As CCursorField(Of Integer)
            Get
                Return Me.m_IDCampagnaCRM
            End Get
        End Property

        Public ReadOnly Property IDUtente As CCursorField(Of Integer)
            Get
                Return Me.m_IDUtente
            End Get
        End Property

        Public ReadOnly Property NomeUtente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeUtente
            End Get
        End Property

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

        Public ReadOnly Property NumeroTelefonateRisposte As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroTelefonateRisposte
            End Get
        End Property

        Public ReadOnly Property NumeroTelefonateNonRisposte As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroTelefonateNonRisposte
            End Get
        End Property

        Public ReadOnly Property MinutiConversazione As CCursorField(Of Integer)
            Get
                Return Me.m_MinutiConversazione
            End Get
        End Property

        Public ReadOnly Property MinutiAttesa As CCursorField(Of Integer)
            Get
                Return Me.m_MinutiAttesa
            End Get
        End Property

        Public ReadOnly Property NumeroAppuntamentiFissati As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroAppuntamentiFissati
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property


        Protected Overrides Function GetModule() As CModule
            Return CustomerCalls.SessioniCRM.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CustomerCalls.CRM.StatsDB
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_SessioneCRM"
        End Function


    End Class



End Class