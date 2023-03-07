Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls


    Public Class CPausaCRMCursor
        Inherits DBObjectCursorPO(Of CPausaCRM)

        Private m_IDSessioneCRM As New CCursorField(Of Integer)("IDSessioneCRM")
        Private m_IDUtente As New CCursorField(Of Integer)("IDUtente")
        Private m_NomeUtente As New CCursorFieldObj(Of String)("NomeUtente")
        Private m_OraRichiesta As New CCursorField(Of Date)("OraRichiesta")
        Private m_OraInizioValutazione As New CCursorField(Of Date)("OraInizioValutazione")
        Private m_OraFineValutazione As New CCursorField(Of Date)("OraFineValutazione")
        Private m_OraPrevista As New CCursorField(Of Date)("OraPrevista")
        Private m_Inizio As New CCursorField(Of Date)("Inizio")
        Private m_Fine As New CCursorField(Of Date)("Fine")
        Private m_Motivo As New CCursorFieldObj(Of String)("Motivo")
        Private m_DurataPrevista As New CCursorField(Of Integer)("DurataPrevista")
        Private m_DettaglioMotivo As New CCursorFieldObj(Of String)("DettaglioMotivo")
        Private m_IDSupervisore As New CCursorField(Of Integer)("IDSupervisore")
        Private m_NomeSupervisore As New CCursorFieldObj(Of String)("NomeSupervisore")
        Private m_EsitoSupervisione As New CCursorFieldObj(Of String)("EsitoSupervisione")
        Private m_NoteAmministrative As New CCursorFieldObj(Of String)("NoteAmministrative")
        Private m_StatoRichiesta As New CCursorField(Of StatoPausaCRM)("StatoRichiesta")
        Private m_Flags As New CCursorField(Of Integer)("Flags")

        Public Sub New()

        End Sub

        Public ReadOnly Property IDSessioneCRM As CCursorField(Of Integer)
            Get
                Return Me.m_IDSessioneCRM
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

        Public ReadOnly Property OraRichiesta As CCursorField(Of Date)
            Get
                Return Me.m_OraRichiesta
            End Get
        End Property

        Public ReadOnly Property OraInizioValutazione As CCursorField(Of Date)
            Get
                Return Me.m_OraInizioValutazione
            End Get
        End Property

        Public ReadOnly Property OraFineValutazione As CCursorField(Of Date)
            Get
                Return Me.m_OraFineValutazione
            End Get
        End Property

        Public ReadOnly Property OraPrevista As CCursorField(Of Date)
            Get
                Return Me.m_OraPrevista
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

        Public ReadOnly Property Motivo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Motivo
            End Get
        End Property

        Public ReadOnly Property DurataPrevista As CCursorField(Of Integer)
            Get
                Return Me.m_DurataPrevista
            End Get
        End Property

        Public ReadOnly Property DettaglioMotivo As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioMotivo
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

        Public ReadOnly Property EsitoSupervisione As CCursorFieldObj(Of String)
            Get
                Return Me.m_EsitoSupervisione
            End Get
        End Property

        Public ReadOnly Property NoteAmministrative As CCursorFieldObj(Of String)
            Get
                Return Me.m_NoteAmministrative
            End Get
        End Property

        Public ReadOnly Property StatoRichiesta As CCursorField(Of StatoPausaCRM)
            Get
                Return Me.m_StatoRichiesta
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return CustomerCalls.PauseCRM.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CustomerCalls.CRM.StatsDB
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PausaCRM"
        End Function


    End Class



End Class