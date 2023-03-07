Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica


Partial Public Class CQSPD

     

    ''' <summary>
    ''' Cursore sulla tabella degli obiettivi
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CObiettivoPraticaCursor
        Inherits DBObjectCursorPO(Of CObiettivoPratica)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_TipoObiettivo As New CCursorField(Of TipoObiettivo)("TipoObiettivo")
        Private m_PeriodicitaObiettivo As New CCursorField(Of PeriodicitaObiettivo)("PeriodicitaObiettivo")
        'Private m_ValoreObiettivo As New CCursorField(Of Double)("ValoreObiettivo")
        Private m_MontanteLordoLiq As New CCursorField(Of Decimal)("MontanteLordoLiq")
        Private m_NumeroPraticheLiq As New CCursorField(Of Integer)("NumeroPraticheLiq")
        Private m_ValoreSpreadLiq As New CCursorField(Of Decimal)("ValoreSpreadLiq")
        Private m_SpreadLiq As New CCursorField(Of Single)("SpreadLiq")
        Private m_ValoreUpFrontLiq As New CCursorField(Of Decimal)("ValoreUpFrontLiq")
        Private m_UpFrontLiq As New CCursorField(Of Single)("UpFrontLiq")
        Private m_DataInizio As New CCursorField(Of Date)("DataInizio")
        Private m_DataFine As New CCursorField(Of Date)("DataFine")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_ValoreScontoLiq As New CCursorField(Of Decimal)("ValoreScontoLiq")
        Private m_ScontoLiq As New CCursorField(Of Single)("ScontoLiq")
        Private m_Livello As New CCursorField(Of Integer)("Livello")
        Private m_CostoStruttura As New CCursorField(Of Decimal)("CostoStruttura")

        Public Sub New()
        End Sub

        Public ReadOnly Property CostoStruttura As CCursorField(Of Decimal)
            Get
                Return Me.m_CostoStruttura
            End Get
        End Property

        Public ReadOnly Property Livello As CCursorField(Of Integer)
            Get
                Return Me.m_Livello
            End Get
        End Property

        Public ReadOnly Property ValoreScontoLiq As CCursorField(Of Decimal)
            Get
                Return Me.m_ValoreScontoLiq
            End Get
        End Property

        Public ReadOnly Property ScontoLiq As CCursorField(Of Single)
            Get
                Return Me.m_ScontoLiq
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property TipoObiettivo As CCursorField(Of TipoObiettivo)
            Get
                Return Me.m_TipoObiettivo
            End Get
        End Property

        Public ReadOnly Property PeriodicitaObiettivo As CCursorField(Of PeriodicitaObiettivo)
            Get
                Return Me.m_PeriodicitaObiettivo
            End Get
        End Property

        'Public ReadOnly Property ValoreObiettivo As CCursorField(Of Double)
        '    Get
        '        Return Me.m_ValoreObiettivo
        '    End Get
        'End Property

        Public ReadOnly Property MontanteLordoLiq As CCursorField(Of Decimal)
            Get
                Return Me.m_MontanteLordoLiq
            End Get
        End Property

        Public ReadOnly Property NumeroPraticheLiq As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroPraticheLiq
            End Get
        End Property

        Public ReadOnly Property ValoreSpreadLiq As CCursorField(Of Decimal)
            Get
                Return Me.m_ValoreSpreadLiq
            End Get
        End Property

        Public ReadOnly Property SpreadLiq As CCursorField(Of Single)
            Get
                Return Me.m_SpreadLiq
            End Get
        End Property

        Public ReadOnly Property ValoreUpFrontLiq As CCursorField(Of Decimal)
            Get
                Return Me.m_ValoreUpFrontLiq
            End Get
        End Property

        Public ReadOnly Property UpFrontLiq As CCursorField(Of Single)
            Get
                Return Me.m_UpFrontLiq
            End Get
        End Property

        Public ReadOnly Property DataInizio As CCursorField(Of Date)
            Get
                Return Me.m_DataInizio
            End Get
        End Property

        Public ReadOnly Property DataFine As CCursorField(Of Date)
            Get
                Return Me.m_DataFine
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        
        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return CQSPD.Obiettivi.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDObiettivi"
        End Function
    End Class




End Class
