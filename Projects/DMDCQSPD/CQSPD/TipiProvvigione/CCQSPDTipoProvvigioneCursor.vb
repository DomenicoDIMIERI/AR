Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    ''' <summary>
    ''' Cursore sulla tabella delle offerte
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CCQSPDTipoProvvigioneCursor
        Inherits DBObjectCursor(Of CCQSPDTipoProvvigione)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_IDGruppoProdotti As New CCursorField(Of Integer)("IDGruppoProdotti")
        Private m_PagataDa As New CCursorField(Of CQSPDTipoSoggetto)("PagataDa")
        Private m_PagataA As New CCursorField(Of CQSPDTipoSoggetto)("PagataA")
        Private m_TipoCalcolo As New CCursorField(Of CQSPDTipoProvvigioneEnum)("TipoCalcolo")
        Private m_Percentuale As New CCursorField(Of Double)("Percentuale")
        Private m_Fisso As New CCursorField(Of Double)("Fisso")
        Private m_ValoreMax As New CCursorField(Of Double)("ValoreMax")
        Private m_Formula As New CCursorFieldObj(Of String)("Formula")
        Private m_Flags As New CCursorField(Of CQSPDTipoProvvigioneFlags)("Flags")


        Public Sub New()
        End Sub

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property IDGruppoProdotti As CCursorField(Of Integer)
            Get
                Return Me.m_IDGruppoProdotti
            End Get
        End Property

        Public ReadOnly Property PagataDa As CCursorField(Of CQSPDTipoSoggetto)
            Get
                Return Me.m_PagataDa
            End Get
        End Property

        Public ReadOnly Property PagataA As CCursorField(Of CQSPDTipoSoggetto)
            Get
                Return Me.m_PagataA
            End Get
        End Property

        Public ReadOnly Property TipoCalcolo As CCursorField(Of CQSPDTipoProvvigioneEnum)
            Get
                Return Me.m_TipoCalcolo
            End Get
        End Property

        Public ReadOnly Property Percentuale As CCursorField(Of Double)
            Get
                Return Me.m_Percentuale
            End Get
        End Property

        Public ReadOnly Property Fisso As CCursorField(Of Double)
            Get
                Return Me.m_Fisso
            End Get
        End Property

        Public ReadOnly Property ValoreMax As CCursorField(Of Double)
            Get
                Return Me.m_ValoreMax
            End Get
        End Property

        Public ReadOnly Property Formula As CCursorFieldObj(Of String)
            Get
                Return Me.m_Formula
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of CQSPDTipoProvvigioneFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CCQSPDTipoProvvigione
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDTipiProvvigione"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function
    End Class


End Class
