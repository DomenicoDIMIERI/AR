Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    ''' <summary>
    ''' Cursore che consente di recuperare tutte le tabelle dei TEG massimi 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CTabelleTEGMaxCursor
        Inherits DBObjectCursor(Of CTabellaTEGMax)

        Private m_ProdottoID As CCursorField(Of Integer)
        Private m_CessionarioID As CCursorField(Of Integer)
        Private m_Nome As CCursorFieldObj(Of String)
        Private m_NomeCessionario As CCursorFieldObj(Of String)
        Private m_Descrizione As CCursorFieldObj(Of String)
        Private m_Visible As New CCursorField(Of Boolean)("Visible")

        Public Sub New()
            Me.m_ProdottoID = New CCursorField(Of Integer)("Prodotto")
            Me.m_CessionarioID = New CCursorField(Of Integer)("Cessionario")
            Me.m_Nome = New CCursorFieldObj(Of String)("Nome")
            Me.m_NomeCessionario = New CCursorFieldObj(Of String)("NomeCessionario")
            Me.m_Descrizione = New CCursorFieldObj(Of String)("Descrizione")
        End Sub

        Public Overrides Function GetTableName() As String
            Return "tbl_FIN_TEGMax"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return DMD.CQSPD.TabelleTEGMax.Module
        End Function

        Public ReadOnly Property IDProdotto As CCursorField(Of Integer)
            Get
                Return Me.m_ProdottoID
            End Get
        End Property

        Public ReadOnly Property Visible As CCursorField(Of Boolean)
            Get
                Return Me.m_Visible
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property IDCessionario As CCursorField(Of Integer)
            Get
                Return Me.m_CessionarioID
            End Get
        End Property

        Public ReadOnly Property NomeCessionario As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCessionario
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CTabellaTEGMax
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

    End Class


End Class
