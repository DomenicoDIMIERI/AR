Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Sistema


Partial Class Office


    Public Class RichiestaPermessoFerieCursor
        Inherits DBObjectCursorPO(Of RichiestaPermessoFerie)

        Private m_DataRichiesta As New CCursorField(Of Date)("DataRichiesta")
        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_MotivoRichiesta As New CCursorFieldObj(Of String)("MotivoRichiesta")
        Private m_NoteRichiesta As New CCursorFieldObj(Of String)("NoteRichiesta")

        Private m_TipoRichiesta As New CCursorField(Of TipoRichiestaPermessoFerie)("TipoRichiesta")

        Private m_IDRichiedente As New CCursorField(Of Integer)("IDRichiedente")
        Private m_NomeRichiedente As New CCursorFieldObj(Of String)("NomeRichiedente")

        Private m_DataPresaInCarico As New CCursorField(Of Date)("DataPresaInCarico")
        Private m_IDInCaricoA As New CCursorField(Of Integer)("IDInCaricoA")
        Private m_NomeInCaricoA As New CCursorFieldObj(Of String)("NomeInCaricoA")
        Private m_EsitoRichiesta As New CCursorField(Of EsitoRichiestaPermessoFerie)("EsitoRichiesta")
        Private m_DettaglioEsitoRichiesta As New CCursorFieldObj(Of String)("DettaglioEsitoRichiesta")
        Private m_NotaPrvSupervisore As New CCursorFieldObj(Of String)("NotaPrvSupervisore")

        Private m_Flags As New CCursorField(Of Integer)("Flags")

        Public Sub New()
        End Sub

        Public ReadOnly Property DataRichiesta As CCursorField(Of Date)
            Get
                Return Me.m_DataRichiesta
            End Get
        End Property

        Public ReadOnly Property DataInizio As CCursorField(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
        End Property

        Public ReadOnly Property NotaPrvSupervisore As CCursorFieldObj(Of String)
            Get
                Return Me.m_NotaPrvSupervisore
            End Get
        End Property


        Public ReadOnly Property DataFine As CCursorField(Of Date)
            Get
                Return Me.m_DataFine
            End Get
        End Property

        Public ReadOnly Property MotivoRichiesta As CCursorFieldObj(Of String)
            Get
                Return Me.m_MotivoRichiesta
            End Get
        End Property

        Public ReadOnly Property NoteRichiesta As CCursorFieldObj(Of String)
            Get
                Return Me.m_NoteRichiesta
            End Get
        End Property

        Public ReadOnly Property TipoRichiesta As CCursorField(Of TipoRichiestaPermessoFerie)
            Get
                Return Me.m_TipoRichiesta
            End Get
        End Property

        Public ReadOnly Property IDRichiedente As CCursorField(Of Integer)
            Get
                Return Me.m_IDRichiedente
            End Get
        End Property

        Public ReadOnly Property NomeRichiedente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeRichiedente
            End Get
        End Property

        Public ReadOnly Property DataPresaInCarico As CCursorField(Of Date)
            Get
                Return Me.m_DataPresaInCarico
            End Get
        End Property

        Public ReadOnly Property IDInCaricoA As CCursorField(Of Integer)
            Get
                Return Me.m_IDInCaricoA
            End Get
        End Property

        Public ReadOnly Property NomeInCaricoA As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeInCaricoA
            End Get
        End Property

        Public ReadOnly Property EsitoRichiesta As CCursorField(Of EsitoRichiestaPermessoFerie)
            Get
                Return Me.m_EsitoRichiesta
            End Get
        End Property

        Public ReadOnly Property DettaglioEsitoRichiesta As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioEsitoRichiesta
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeRichiestePermF"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Office.RichiestePermessiFerie.Module
        End Function

    End Class


End Class