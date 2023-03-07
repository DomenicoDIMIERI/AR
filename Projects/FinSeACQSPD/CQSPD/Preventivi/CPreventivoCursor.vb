Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    Public Class CPreventivoCursor
        Inherits DBObjectCursorPO(Of CPreventivo)

        Private m_ProfiloID As New CCursorField(Of Integer)("Profilo")
        Private m_NomeProfilo As New CCursorFieldObj(Of String)("NomeProfilo")
        Private m_DataNascita As New CCursorField(Of Date)("DataNascita")
        Private m_DataAssunzione As New CCursorField(Of Date)("DataAssunzione")
        Private m_DataDecorrenza As New CCursorField(Of Date)("DataDecorrenza")
        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Cognome As New CCursorFieldObj(Of String)("Cognome")
        Private m_Nominativo As New CCursorFieldObj(Of String)("(Trim([Nome] & ' ' & [Cognome]))")
        Private m_TipoContratto As New CCursorFieldObj(Of String)("TipoContratto")
        Private m_TipoRapporto As New CCursorFieldObj(Of String)("TipoRapporto")
        Private m_Sesso As New CCursorFieldObj(Of String)("Sesso")
        Private m_Durata As New CCursorField(Of Integer)("Durata")
        Private m_Rata As New CCursorField(Of Decimal)("Rata")
        'Private m_NumeroOfferta As CCursorFieldObj(Of String)
        Private m_Provvigionale As New CCursorField(Of Double)("Provvigionale")
        Private m_CaricaAlMassimo As New CCursorField(Of Boolean)("CaricaAlMassimo")
        Private m_SortBy As New CCursorField(Of SortPreventivoBy)("SortBy")
        Private m_StipendioLordo As New CCursorField(Of Decimal)("StipendioLordo")
        Private m_StipendioNetto As New CCursorField(Of Decimal)("StipendioNetto")
        Private m_TFR As New CCursorField(Of Decimal)("TFR")
        Private m_NumeroMensilita As New CCursorField(Of Integer)("NumeroMensilita")
        Private m_Amministrazione As New CCursorField(Of Integer)("Amministrazione")

        Public Sub New()
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public ReadOnly Property TipoContratto As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoContratto
            End Get
        End Property

        Public ReadOnly Property TipoRapporto As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoRapporto
            End Get
        End Property

        Public ReadOnly Property Durata As CCursorField(Of Integer)
            Get
                Return Me.m_Durata
            End Get
        End Property

        Public ReadOnly Property Rata As CCursorField(Of Decimal)
            Get
                Return Me.m_Rata
            End Get
        End Property

        Public ReadOnly Property Provvigionale As CCursorField(Of Double)
            Get
                Return Me.m_Provvigionale
            End Get
        End Property

        Public ReadOnly Property CaricaAlMassimo As CCursorField(Of Boolean)
            Get
                Return Me.m_CaricaAlMassimo
            End Get
        End Property

        Public ReadOnly Property SortBy As CCursorField(Of SortPreventivoBy)
            Get
                Return Me.m_SortBy
            End Get
        End Property

        Public ReadOnly Property StipendioLordo As CCursorField(Of Decimal)
            Get
                Return Me.m_StipendioLordo
            End Get
        End Property

        Public ReadOnly Property StipendioNetto As CCursorField(Of Decimal)
            Get
                Return Me.m_StipendioNetto
            End Get
        End Property

        Public ReadOnly Property TFR As CCursorField(Of Decimal)
            Get
                Return Me.m_TFR
            End Get
        End Property

        Public ReadOnly Property NumeroMensilita As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroMensilita
            End Get
        End Property

        Public ReadOnly Property Amministrazione As CCursorField(Of Integer)
            Get
                Return Me.m_Amministrazione
            End Get
        End Property


        Public ReadOnly Property Nominativo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nominativo
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Cognome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Cognome
            End Get
        End Property

        Public ReadOnly Property ProfiloID As CCursorField(Of Integer)
            Get
                Return Me.m_ProfiloID
            End Get
        End Property

        Public ReadOnly Property NomeProfilo As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeProfilo
            End Get
        End Property

        'Public ReadOnly Property NumeroOfferta As CCursorFieldObj(Of String)
        '    Get
        '        Return Me.m_NumeroOfferta
        '    End Get
        'End Property

        Public ReadOnly Property DataNascita As CCursorField(Of Date)
            Get
                Return Me.m_DataNascita
            End Get
        End Property

        Public ReadOnly Property DataAssunzione As CCursorField(Of Date)
            Get
                Return Me.m_DataAssunzione
            End Get
        End Property

        Public ReadOnly Property DataDecorrenza As CCursorField(Of Date)
            Get
                Return Me.m_DataDecorrenza
            End Get
        End Property

        Public ReadOnly Property Sesso As CCursorFieldObj(Of String)
            Get
                Return Me.m_Sesso
            End Get
        End Property


        Protected Overrides Function GetModule() As CModule
            Return DMD.CQSPD.Preventivi.Module '"modPreventivatori"
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Preventivi"
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CPreventivo
        End Function

    End Class


End Class
