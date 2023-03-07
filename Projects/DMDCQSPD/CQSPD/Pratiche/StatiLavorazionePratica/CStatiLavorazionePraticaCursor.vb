Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    ''' <summary>
    ''' Cursore sulla tabella degli stati di lavorazione delle pratiche
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CStatiLavorazionePraticaCursor
        Inherits DBObjectCursor(Of CStatoLavorazionePratica)


        Private m_MacroStato As New CCursorField(Of StatoPraticaEnum)("MacroStato")
        Private m_IDPratica As New CCursorField(Of Integer)("IDPratica")
        Private m_IDFromStato As New CCursorField(Of Integer)("IDFromStato")
        Private m_IDToStato As New CCursorField(Of Integer)("IDToStato")
        Private m_Data As New CCursorField(Of Date)("Data")
        Private m_IDOperatore As New CCursorField(Of Integer)("IDOperatore")
        Private m_NomeOperatore As New CCursorFieldObj(Of String)("NomeOperatore")
        Private m_Note As New CCursorFieldObj(Of String)("Note")
        Private m_Params As New CCursorFieldObj(Of String)("Parameters")
        Private m_Forzato As New CCursorField(Of Boolean)("Forzato")
        Private m_IDOfferta As New CCursorField(Of Integer)("IDOfferta")
        Private m_DescrizioneStato As New CCursorFieldObj(Of String)("DescrizioneStato")
        Private m_IDRegolaApplicata As New CCursorField(Of Integer)("IDRegolaApplicata")

        Public Sub New()
        End Sub

        Public ReadOnly Property MacroStato As CCursorField(Of StatoPraticaEnum)
            Get
                Return Me.m_MacroStato
            End Get
        End Property

        Public ReadOnly Property IDPratica As CCursorField(Of Integer)
            Get
                Return Me.m_IDPratica
            End Get
        End Property

        Public ReadOnly Property IDFromStato As CCursorField(Of Integer)
            Get
                Return Me.m_IDFromStato
            End Get
        End Property

        Public ReadOnly Property IDToStato As CCursorField(Of Integer)
            Get
                Return Me.m_IDToStato
            End Get
        End Property

        Public ReadOnly Property Data As CCursorField(Of Date)
            Get
                Return Me.m_Data
            End Get
        End Property

        Public ReadOnly Property IDOperatore As CCursorField(Of Integer)
            Get
                Return Me.m_IDOperatore
            End Get
        End Property

        Public ReadOnly Property NomeOperatore As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeOperatore
            End Get
        End Property

        Public ReadOnly Property Note As CCursorFieldObj(Of String)
            Get
                Return Me.m_Note
            End Get
        End Property

        Public ReadOnly Property Params As CCursorFieldObj(Of String)
            Get
                Return Me.m_Params
            End Get
        End Property

        Public ReadOnly Property Forzato As CCursorField(Of Boolean)
            Get
                Return Me.m_Forzato
            End Get
        End Property

        Public ReadOnly Property IDOfferta As CCursorField(Of Integer)
            Get
                Return Me.m_IDOfferta
            End Get
        End Property

        Public ReadOnly Property DescrizioneStato As CCursorFieldObj(Of String)
            Get
                Return Me.m_DescrizioneStato
            End Get
        End Property

        Public ReadOnly Property IDRegolaApplicata As CCursorField(Of Integer)
            Get
                Return Me.m_IDRegolaApplicata
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return StatiLavorazionePratica.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PraticheSTL"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

    End Class

End Class