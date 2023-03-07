Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    ''' <summary>
    ''' Cursore che consente di recuperare tutte le tabelle finanziarie associate ad un prodotto
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CProdottoXTabellaAssCursor
        Inherits DBObjectCursor(Of CProdottoXTabellaAss)

        Private m_ProdottoID As CCursorField(Of Integer)
        Private m_Descrizione As CCursorFieldObj(Of String)
        Private m_RischioVitaID As CCursorField(Of Integer)
        Private m_RischioImpiegoID As CCursorField(Of Integer)
        Private m_RischioCreditoID As CCursorField(Of Integer)

        Public Sub New()
            Me.m_ProdottoID = New CCursorField(Of Integer)("Prodotto")
            Me.m_Descrizione = New CCursorFieldObj(Of String)("Descrizione")
            Me.m_RischioVitaID = New CCursorField(Of Integer)("RischioVita")
            Me.m_RischioImpiegoID = New CCursorField(Of Integer)("RischioImpiego")
            Me.m_RischioCreditoID = New CCursorField(Of Integer)("RischioCredito")
        End Sub

        Public ReadOnly Property ProdottoID As CCursorField(Of Integer)
            Get
                Return Me.m_ProdottoID
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property RischioVitaID As CCursorField(Of Integer)
            Get
                Return Me.m_RischioVitaID
            End Get
        End Property

        Public ReadOnly Property RischioImpiegoID As CCursorField(Of Integer)
            Get
                Return Me.m_RischioImpiegoID
            End Get
        End Property

        Public ReadOnly Property RischioCreditoID As CCursorField(Of Integer)
            Get
                Return Me.m_RischioCreditoID
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CProdottoXTabellaAss
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_FIN_ProdXTabAss"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

    End Class


End Class