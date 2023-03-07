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
    Public Class CCQSPDProvvigioneXOffertaCursor
        Inherits DBObjectCursor(Of CCQSPDProvvigioneXOfferta)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_IDOfferta As New CCursorField(Of Integer)("IDOfferta")
        Private m_IDTipoProvvigione As New CCursorField(Of Integer)("IDTipoProvvigione")
        Private m_PagataDa As New CCursorField(Of CQSPDTipoSoggetto)("PagataDa")
        Private m_PagataA As New CCursorField(Of CQSPDTipoSoggetto)("PagataA")
        Private m_TipoCalcolo As New CCursorField(Of CQSPDTipoProvvigioneEnum)("TipoCalcolo")
        Private m_Percentuale As New CCursorField(Of Double)("Percentuale")
        Private m_Fisso As New CCursorField(Of Double)("Fisso")
        Private m_Formula As New CCursorFieldObj(Of String)("Formula")
        Private m_Flags As New CCursorField(Of ProvvigioneXOffertaFlags)("Flags")
        Private m_IDCedente As New CCursorField(Of Integer)("IDCedente")
        Private m_NomeCedente As New CCursorFieldObj(Of String)("NomeCedente")
        Private m_IDRicevente As New CCursorField(Of Integer)("IDRicevente")
        Private m_NomeRicevente As New CCursorFieldObj(Of String)("NomeRicevente")
        Private m_BaseDiCalcolo As New CCursorField(Of Double)("BaseDiCalcolo")
        Private m_Valore As New CCursorField(Of Double)("Valore")
        Private m_ValorePagato As New CCursorField(Of Double)("ValorePagato")
        Private m_DataPagamento As New CCursorField(Of Date)("DataPagamento")



        Public Sub New()
        End Sub

        Public ReadOnly Property IDCedente As CCursorField(Of Integer)
            Get
                Return Me.m_IDCedente
            End Get
        End Property

        Public ReadOnly Property NomeCedente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCedente
            End Get
        End Property

        Public ReadOnly Property IDRicevente As CCursorField(Of Integer)
            Get
                Return Me.m_IDRicevente
            End Get
        End Property

        Public ReadOnly Property NomeRicevente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeRicevente
            End Get
        End Property

        Public ReadOnly Property BaseDiCalcolo As CCursorField(Of Double)
            Get
                Return Me.m_BaseDiCalcolo
            End Get
        End Property

        Public ReadOnly Property Valore As CCursorField(Of Double)
            Get
                Return Me.m_Valore
            End Get
        End Property

        Public ReadOnly Property ValorePagato As CCursorField(Of Double)
            Get
                Return Me.m_ValorePagato
            End Get
        End Property

        Public ReadOnly Property DataPagamento As CCursorField(Of Date)
            Get
                Return Me.m_DataPagamento
            End Get
        End Property

        Public ReadOnly Property IDTipoProvvigione As CCursorField(Of Integer)
            Get
                Return Me.m_IDTipoProvvigione
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property IDOfferta As CCursorField(Of Integer)
            Get
                Return Me.m_IDOfferta
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

        Public ReadOnly Property Formula As CCursorFieldObj(Of String)
            Get
                Return Me.m_Formula
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of ProvvigioneXOffertaFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CCQSPDProvvigioneXOfferta
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDProvvXOfferta"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function
    End Class


End Class
