Imports DMD.Sistema
Imports DMD.Databases

Partial Public Class CQSPD

    ''' <summary>
    ''' Cursore sulla tabella delle relazioni tra profili e prodotti
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CProdottoProfiloCursor
        Inherits DBObjectCursor(Of CProdottoProfilo)

        Private m_IDProfilo As CCursorField(Of Integer)
        Private m_IDProdotto As CCursorField(Of Integer)
        Private m_Azione As CCursorField(Of IncludeModes)
        Private m_Spread As CCursorField(Of Double)

        Public Sub New()
            Me.m_IDProdotto = New CCursorField(Of Integer)("Prodotto")
            Me.m_IDProfilo = New CCursorField(Of Integer)("Preventivatore")
            Me.m_Azione = New CCursorField(Of IncludeModes)("Azione")
            Me.m_Spread = New CCursorField(Of Double)("Spread")
        End Sub

        ''' <summary>
        ''' ID del profilo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IDProfilo As CCursorField(Of Integer)
            Get
                Return Me.m_IDProfilo
            End Get
        End Property

        ''' <summary>
        ''' ID del prodotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IDProdotto As CCursorField(Of Integer)
            Get
                Return Me.m_IDProdotto
            End Get
        End Property

        ''' <summary>
        ''' Relazione tra profilo e prodotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Azione As CCursorField(Of IncludeModes)
            Get
                Return Me.m_Azione
            End Get
        End Property

        ''' <summary>
        ''' Spread rispetto al genitore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Spread As CCursorField(Of Double)
            Get
                Return Me.m_Spread
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CProdottoProfilo
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PreventivatoriXProdotto"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function
    End Class


End Class
