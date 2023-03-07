Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    ''' <summary>
    ''' Cursore sulla tabella delle Categorie Prodotto
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CCategorieProdottoCursor
        Inherits DBObjectCursor(Of CCategoriaProdotto)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_NomeGruppo As New CCursorFieldObj(Of String)("NomeGruppo")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_DataInizio As New CCursorField(Of DateTime)("DataInizio")
        Private m_DataFine As New CCursorField(Of DateTime)("DataFine")

        Public Sub New()

        End Sub


        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property NomeGruppo As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeGruppo
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property DataInizio As CCursorField(Of DateTime)
            Get
                Return Me.m_DataInizio
            End Get
        End Property

        Public ReadOnly Property DataFine As CCursorField(Of DateTime)
            Get
                Return Me.m_DataFine
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CCategoriaProdotto
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDCatProd"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return CQSPD.CategorieProdotto.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

    End Class


End Class