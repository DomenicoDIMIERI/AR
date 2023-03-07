Imports DMD.Sistema
Imports DMD.Databases

Partial Public Class CQSPD


    ''' <summary>
    ''' Cursore che consente di recuperare tutte le tabelle finanziarie associate ad un prodotto
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CRigheTEGMaxCursor
        Inherits DBObjectCursor(Of CRigaTEGMax)

        Private m_TabellaID As CCursorField(Of Integer)
        Private m_ValoreSoglia As CCursorField(Of Double)

        Public Sub New()
            Me.m_TabellaID = New CCursorField(Of Integer)("Tabella")
            Me.m_ValoreSoglia = New CCursorField(Of Double)("ValoreSoglia")
        End Sub

        Protected Overrides Function GetModule() As CModule
            Return DMD.CQSPD.TabelleTEGMax.Module 'modCQSPDTblTEGMax
        End Function

        Public ReadOnly Property TabellaID As CCursorField(Of Integer)
            Get
                Return Me.m_TabellaID
            End Get
        End Property

        Public ReadOnly Property ValoreSoglia As CCursorField(Of Double)
            Get
                Return Me.m_ValoreSoglia
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_FIN_TEGMaxI"
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CRigaTEGMax
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

    End Class


End Class
