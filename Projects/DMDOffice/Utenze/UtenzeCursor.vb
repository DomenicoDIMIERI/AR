Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Class Office

    ''' <summary>
    ''' Cursore sulla tabella delle utenze
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class UtenzeCursor
        Inherits DBObjectCursorPO(Of Utenza)


        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_IDFornitore As New CCursorField(Of Integer)("IDFornitore")
        Private m_NomeFornitore As New CCursorFieldObj(Of String)("NomeFornitore")
        Private m_IDCliente As New CCursorField(Of Integer)("IDCliente")
        Private m_NomeCliente As New CCursorFieldObj(Of String)("NomeCliente")
        Private m_NumeroContratto As New CCursorFieldObj(Of String)("NumeroContratto")
        Private m_CodiceCliente As New CCursorFieldObj(Of String)("CodiceCliente")
        Private m_CodiceUtenza As New CCursorFieldObj(Of String)("CodiceUtenza")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_TipoPeriodicita As New CCursorField(Of PeriodicitaUtenza)("TipoPeriodicita")
        Private m_IntervalloPeriodicita As New CCursorField(Of Integer)("IntervalloPeriodicita")
        Private m_DataSottoscrizione As New CCursorField(Of Date)("DataSottoscrizione")
        Private m_DataInizioPeriodo As New CCursorField(Of Date)("DataInizioPeriodo")
        Private m_DataFinePeriodo As New CCursorField(Of Date)("DataFinePeriodo")
        Private m_UnitaMisura As New CCursorFieldObj(Of String)("UnitaMisura")
        Private m_CostoUnitario As New CCursorField(Of Decimal)("CostoUnitario")
        Private m_CostiFissi As New CCursorField(Of Decimal)("CostiFissi")
        Private m_NomeValuta As New CCursorFieldObj(Of String)("NomeValuta")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_TipoMetodoDiPagamento As New CCursorFieldObj(Of String)("TipoMetodoDiPagamento")
        Private m_IDMetodoDiPagamento As New CCursorField(Of Integer)("IDMetodotoDiPagamento")
        Private m_NomeMetodoDiPagamento As New CCursorFieldObj(Of String)("NomeMetodoDiPagamento")
        Private m_TipoUtenza As New CCursorFieldObj(Of String)("TipoUtenza")
        Private m_StimatoreBolletta As New CCursorFieldObj(Of String)("StimatoreBolletta")
    
        Public Sub New()

        End Sub

        Public ReadOnly Property StimatoreBolletta As CCursorFieldObj(Of String)
            Get
                Return Me.m_StimatoreBolletta
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property IDFornitore As CCursorField(Of Integer)
            Get
                Return Me.m_IDFornitore
            End Get
        End Property

        Public ReadOnly Property NomeFornitore As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeFornitore
            End Get
        End Property

        Public ReadOnly Property IDCliente As CCursorField(Of Integer)
            Get
                Return Me.m_IDCliente
            End Get
        End Property

        Public ReadOnly Property NomeCliente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCliente
            End Get
        End Property

        Public ReadOnly Property NumeroContratto As CCursorFieldObj(Of String)
            Get
                Return Me.m_NumeroContratto
            End Get
        End Property

        Public ReadOnly Property CodiceCliente As CCursorFieldObj(Of String)
            Get
                Return Me.m_CodiceCliente
            End Get
        End Property

        Public ReadOnly Property CodiceUtenza As CCursorFieldObj(Of String)
            Get
                Return Me.m_CodiceUtenza
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property


        Public ReadOnly Property TipoPeriodicita As CCursorField(Of PeriodicitaUtenza)
            Get
                Return Me.m_TipoPeriodicita
            End Get
        End Property

        Public ReadOnly Property IntervalloPeriodicita As CCursorField(Of Integer)
            Get
                Return Me.m_IntervalloPeriodicita
            End Get
        End Property

        Public ReadOnly Property DataSottoscrizione As CCursorField(Of Date)
            Get
                Return Me.m_DataSottoscrizione
            End Get
        End Property

        Public ReadOnly Property DataInizioPeriodo As CCursorField(Of Date)
            Get
                Return Me.m_DataInizioPeriodo
            End Get
        End Property

        Public ReadOnly Property DataFinePeriodo As CCursorField(Of Date)
            Get
                Return Me.m_DataFinePeriodo
            End Get
        End Property

        Public ReadOnly Property UnitaMisura As CCursorFieldObj(Of String)
            Get
                Return Me.m_UnitaMisura
            End Get
        End Property

        Public ReadOnly Property CostoUnitario As CCursorField(Of Decimal)
            Get
                Return Me.m_CostoUnitario
            End Get
        End Property

        Public ReadOnly Property CostiFissi As CCursorField(Of Decimal)
            Get
                Return Me.m_CostiFissi
            End Get
        End Property

        Public ReadOnly Property NomeValuta As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeValuta
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property TipoMetodoDiPagamento As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoMetodoDiPagamento
            End Get
        End Property

        Public ReadOnly Property IDMetodoDiPagamento As CCursorField(Of Integer)
            Get
                Return Me.m_IDMetodoDiPagamento
            End Get
        End Property

        Public ReadOnly Property NomeMetodoDiPagamento As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeMetodoDiPagamento
            End Get
        End Property

        Public ReadOnly Property TipoUtenza As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoUtenza
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function


        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeUtenze"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Office.Utenze.Module
        End Function
    End Class



End Class