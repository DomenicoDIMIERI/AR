Imports FinSeA.Databases
Imports FinSeA.Sistema

Partial Public Class CQSPD

    ''' <summary>
    ''' Cursore sulla tabella dei documenti caricabili per prodotto
    ''' </summary>
    ''' <remarks></remarks>
    Public Class VincoloXGruppoProdottiCursor
        Inherits DBObjectCursor(Of VincoloXGruppoProdotti)

        Private m_IDGruppoProdotti As New CCursorField(Of Integer)("GruppoProdotti")
        Private m_IDDocumento As New CCursorField(Of Integer)("Documento")
        Private m_Disposizione As New CCursorField(Of DocumentoXProdottoDisposition)("Disposizione")
        Private m_Richiesto As New CCursorField(Of Boolean)("Richiesto")
        Private m_IDStatoPratica As New CCursorField(Of Integer)("IDStatoPratica")
        Private m_Progressivo As New CCursorField(Of Integer)("Progressivo")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")

        Public Sub New()
        End Sub

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property Progressivo As CCursorField(Of Integer)
            Get
                Return Me.m_Progressivo
            End Get
        End Property

        Public ReadOnly Property IDGruppoProdotti As CCursorField(Of Integer)
            Get
                Return Me.m_IDGruppoProdotti
            End Get
        End Property

        Public ReadOnly Property IDDocumento As CCursorField(Of Integer)
            Get
                Return Me.m_IDDocumento
            End Get
        End Property

        Public ReadOnly Property Disposizione As CCursorField(Of DocumentoXProdottoDisposition)
            Get
                Return Me.m_Disposizione
            End Get
        End Property

        Public ReadOnly Property Richiesto As CCursorField(Of Boolean)
            Get
                Return Me.m_Richiesto
            End Get
        End Property

        Public ReadOnly Property IDStatoPratica As CCursorField(Of Integer)
            Get
                Return Me.m_IDStatoPratica
            End Get
        End Property



        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_DocumentiXGruppoProdotti"
        End Function
    End Class


End Class