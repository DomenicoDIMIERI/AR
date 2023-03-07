Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Imports DMD.GPS

Partial Class Office

    ''' <summary>
    ''' Rappresenta un documento contabile
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class VoceDiPagamentoCursor
        Inherits DBObjectCursorPO(Of VoceDiPagamento)

    
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_Quantita As New CCursorField(Of Decimal)("Quantita")
        Private m_NomeValuta As New CCursorFieldObj(Of String)("NomeValuta")
        Private m_DataOperazione As New CCursorField(Of Date)("DataOperazione")
        Private m_DataEffettiva As New CCursorField(Of Date)("DataEffettiva")
        Private m_SourceType As New CCursorFieldObj(Of String)("SourceType")
        Private m_SourceID As New CCursorField(Of Integer)("SourceID")
        Private m_SourceParams As New CCursorFieldObj(Of String)("SourceParams")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_IDCCOrigine As New CCursorField(Of Integer)("IDCCOrigine")
        Private m_NomeCCOrigine As New CCursorFieldObj(Of String)("NomeCCOrigine")
        Private m_IDCCDestinazione As New CCursorField(Of Integer)("IDCCDestinazione")
        Private m_NomeCCDestinazione As New CCursorFieldObj(Of String)("NomeCCDestinazione")
        Private m_TipoMetodoDiPagamento As New CCursorFieldObj(Of String)("TipoMetodoDiPagamento")
        Private m_IDMetodoDiPagamento As New CCursorField(Of Integer)("IDMetodotoDiPagamento")
        Private m_NomeMetodoDiPagamento As New CCursorFieldObj(Of String)("NomeMetodoDiPagamento")

        Public Sub New()

        End Sub


        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property Quantita As CCursorField(Of Decimal)
            Get
                Return Me.m_Quantita
            End Get
        End Property

        Public ReadOnly Property NomeValuta As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeValuta
            End Get
        End Property

        Public ReadOnly Property DataOperazione As CCursorField(Of Date)
            Get
                Return Me.m_DataOperazione
            End Get
        End Property

        Public ReadOnly Property DataEffettiva As CCursorField(Of Date)
            Get
                Return Me.m_DataEffettiva
            End Get
        End Property

        Public ReadOnly Property SourceType As CCursorFieldObj(Of String)
            Get
                Return Me.m_SourceType
            End Get
        End Property

        Public ReadOnly Property SourceID As CCursorField(Of Integer)
            Get
                Return Me.m_SourceID
            End Get
        End Property

        Public ReadOnly Property SourceParams As CCursorFieldObj(Of String)
            Get
                Return Me.m_SourceParams
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property IDCCOrigine As CCursorField(Of Integer)
            Get
                Return Me.m_IDCCOrigine
            End Get
        End Property

        Public ReadOnly Property NomeCCOrigine As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCCOrigine
            End Get
        End Property

        Public ReadOnly Property IDCCDestinazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDCCDestinazione
            End Get
        End Property

        Public ReadOnly Property NomeCCDestinazione As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCCDestinazione
            End Get
        End Property

        Public ReadOnly Property TipoMetodoDiPagamento As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoMetodoDiPagamento
            End Get
        End Property

        Public ReadOnly Property IDMetodoDiPagamento As CCursorField(Of Integer)
            Get
                Return Me.m_IDMetodoDiPagamento
            End Get
        End Property

        Public ReadOnly Property NomeMetodoDiPagamento As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeMetodoDiPagamento
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function


        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeVociPagamento"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Office.VociDiPagamento.Module
        End Function
    End Class



End Class