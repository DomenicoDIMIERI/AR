Imports DMD.Databases
Imports DMD.Sistema

Partial Class Office


    ''' <summary>
    ''' Cursore sulla tabella dei veicoli
    ''' </summary>
    ''' <remarks></remarks>
    Public Class VeicoliCursor
        Inherits DBObjectCursorPO(Of Veicolo)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Tipo As New CCursorFieldObj(Of String)("Tipo")
        Private m_Modello As New CCursorFieldObj(Of String)("Modello")
        Private m_Seriale As New CCursorFieldObj(Of String)("Seriale")
        Private m_Alimentazione As New CCursorFieldObj(Of String)("Alimentazione")
        Private m_KmALitro As New CCursorField(Of Single)("KmALitro")
        Private m_IconURL As New CCursorFieldObj(Of String)("IconURL")
        Private m_DataAcquisto As New CCursorField(Of Date)("DataAcquisto")
        Private m_DataDismissione As New CCursorField(Of Date)("DataDismissione")
        Private m_StatoVeicolo As New CCursorField(Of StatoVeicolo)("StatoVeicolo")
        Private m_Targa As New CCursorFieldObj(Of String)("Targa")
        Private m_DataImmatricolazione As New CCursorField(Of Date)("DataImmatricolazione")
        Private m_ConsumoUrbano As New CCursorField(Of Single)("ConsumoUrbano")
        Private m_ConsumoExtraUrbano As New CCursorField(Of Single)("ConsumoExtraUrbano")
        Private m_ConsumoCombinato As New CCursorField(Of Single)("ConsumoCombinato")
        Private m_Note As New CCursorFieldObj(Of String)("Note")
        Private m_Flags As New CCursorField(Of VeicoloFlags)("Flags")
        Private m_IDProprietario As New CCursorField(Of Integer)("IDProprietario")
        Private m_NomeProprietario As New CCursorFieldObj(Of String)("NomeProprietario")

        Public Sub New()
        End Sub

        Public ReadOnly Property IDProprietario As CCursorField(Of Integer)
            Get
                Return Me.m_IDProprietario
            End Get
        End Property

        Public ReadOnly Property NomeProprietario As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeProprietario
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of VeicoloFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property KmALitro As CCursorField(Of Single)
            Get
                Return Me.m_KmALitro
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Tipo As CCursorFieldObj(Of String)
            Get
                Return Me.m_Tipo
            End Get
        End Property

        Public ReadOnly Property Modello As CCursorFieldObj(Of String)
            Get
                Return Me.m_Modello
            End Get
        End Property

        Public ReadOnly Property Seriale As CCursorFieldObj(Of String)
            Get
                Return Me.m_Seriale
            End Get
        End Property

        Public ReadOnly Property Alimentazione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Alimentazione
            End Get
        End Property

        Public ReadOnly Property IconURL As CCursorFieldObj(Of String)
            Get
                Return Me.m_IconURL
            End Get
        End Property

        Public ReadOnly Property DataAcquisto As CCursorField(Of Date)
            Get
                Return Me.m_DataAcquisto
            End Get
        End Property

        Public ReadOnly Property DataDismissione As CCursorField(Of Date)
            Get
                Return Me.m_DataDismissione
            End Get
        End Property

        Public ReadOnly Property StatoVeicolo As CCursorField(Of StatoVeicolo)
            Get
                Return Me.m_StatoVeicolo
            End Get
        End Property

        Public ReadOnly Property Targa As CCursorFieldObj(Of String)
            Get
                Return Me.m_Targa
            End Get
        End Property

        Public ReadOnly Property DataImmatricolazione As CCursorField(Of Date)
            Get
                Return Me.m_DataImmatricolazione
            End Get
        End Property

        Public ReadOnly Property ConsumoUrbano As CCursorField(Of Single)
            Get
                Return Me.m_ConsumoUrbano
            End Get
        End Property

        Public ReadOnly Property ConsumoExtraUrbano As CCursorField(Of Single)
            Get
                Return Me.m_ConsumoExtraUrbano
            End Get
        End Property

        Public ReadOnly Property ConsumoCombinato As CCursorField(Of Single)
            Get
                Return Me.m_ConsumoCombinato
            End Get
        End Property

        Public ReadOnly Property Note As CCursorFieldObj(Of String)
            Get
                Return Me.m_Note
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return Office.Veicoli.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeVeicoli"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function
    End Class



End Class