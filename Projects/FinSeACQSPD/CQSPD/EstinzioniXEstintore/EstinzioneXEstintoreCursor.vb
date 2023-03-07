Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    ''' <summary>
    ''' Cursore sulla tabella delle estinzioni
    ''' </summary>
    ''' <remarks></remarks>
    Public Class EstinzioneXEstintoreCursor
        Inherits DBObjectCursor(Of EstinzioneXEstintore)

        Private m_Selezionata As New CCursorField(Of Boolean)("Selezionata")
        Private m_IDEstinzione As New CCursorField(Of Integer)("IDEstinzione")
        Private m_IDEstintore As New CCursorField(Of Integer)("IDEstintore")
        Private m_TipoEstintore As New CCursorFieldObj(Of String)("TipoEstintore")
        Private m_DataDecorrenza As New CCursorField(Of Date)("DataDecorrenza")
        Private m_NumeroQuoteInsolute As New CCursorField(Of Integer)("NQI")
        Private m_NumeroQuoteScadute As New CCursorField(Of Integer)("NQS")
        Private m_NumeroQuoteResidue As New CCursorField(Of Integer)("NQR")
        Private m_Parametro As New CCursorFieldObj(Of String)("Parametro")
        Private m_Correzione As New CCursorField(Of Decimal)("Correzione")

        Public Sub New()
        End Sub

        Public ReadOnly Property Selezionata As CCursorField(Of Boolean)
            Get
                Return Me.m_Selezionata
            End Get
        End Property

        Public ReadOnly Property Parametro As CCursorFieldObj(Of String)
            Get
                Return Me.m_Parametro
            End Get
        End Property

        Public ReadOnly Property IDEstinzione As CCursorField(Of Integer)
            Get
                Return Me.m_IDEstinzione
            End Get
        End Property

        Public ReadOnly Property IDEstintore As CCursorField(Of Integer)
            Get
                Return Me.m_IDEstintore
            End Get
        End Property

        Public ReadOnly Property TipoEstintore As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoEstintore
            End Get
        End Property

        Public ReadOnly Property DataDecorrenza As CCursorField(Of Date)
            Get
                Return Me.m_DataDecorrenza
            End Get
        End Property

        Public ReadOnly Property NumeroQuoteInsolute As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroQuoteInsolute
            End Get
        End Property

        Public ReadOnly Property NumeroQuoteScadute As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroQuoteScadute
            End Get
        End Property

        Public ReadOnly Property NumeroQuoteResidue As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroQuoteResidue
            End Get
        End Property

        Public ReadOnly Property Correzione As CCursorField(Of Decimal)
            Get
                Return Me.m_Correzione
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New EstinzioneXEstintore
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_EstinzioniXEstintore"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function
    End Class


End Class
