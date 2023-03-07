Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.CustomerCalls

Partial Public Class CQSPD
 
    Public Class ClientiLavoratiStatsItemCursor
        Inherits DBObjectCursorPO(Of ClientiLavoratiStatsItem)

        Private m_IDCliente As New CCursorField(Of Integer)("IDCliente")
        Private m_NomeCliente As New CCursorFieldObj(Of String)("NomeCliente")
        Private m_IconaCliente As New CCursorFieldObj(Of String)("IconaCliente")

        Private m_StatoLavorazione As New CCursorField(Of StatoLavorazione)("StatoLavorazione")
        Private m_SottostatoLavorazione As New CCursorField(Of SottostatoLavorazione)("SottostatoLavorazione")
        Private m_DataInizioLavorazione As New CCursorField(Of Date)("DataInizioLavorazione")
        Private m_DataFineLavorazione As New CCursorField(Of Date)("DataFineLavorazione")
        Private m_DataUltimoAggiornamento As New CCursorField(Of Date)("DataUltimoAggiornamento")

        Private m_IDOperatore As New CCursorField(Of Integer)("IDOperatore")
        Private m_NomeOperatore As New CCursorFieldObj(Of String)("NomeOperatore")
        Private m_NumeroBustePaga As New CCursorField(Of Integer)("NumeroBustePaga")
        Private m_NumeroVisite As New CCursorField(Of Integer)("NumeroVisite")
        'Private m_NumeroVisiteConsulenza As Integer 'Numero di visite con scopo consulenza (prima o successiva) durante il periodo della lavorazione
        Private m_NumeroRichiesteFinanziamento As New CCursorField(Of Integer)("NumeroRichiesteFinanziamento")
        Private m_NumeroRichiesteConteggiEstintivi As New CCursorField(Of Integer)("NumeroRichiesteConteggiEstintivi")
        Private m_NumeroStudiDiFattibilita As New CCursorField(Of Integer)("NumeroStudiDiFattibilita")
        Private m_NumeroOfferteProposte As New CCursorField(Of Integer)("NumeroOfferteProposte")
        'Private m_NumeroOfferteAccettate As Integer 'Numero di studi di fattibilità accettati dal cliente durante il periodo della lavorazione
        Private m_NumeroOfferteRifiutate As New CCursorField(Of Integer)("NumeroOfferteRifiutate")
        Private m_NumeroOfferteNonFattibili As New CCursorField(Of Integer)("NumeroOfferteNonFattibili")
        Private m_NumeroOfferteBocciate As New CCursorField(Of Integer)("NumeroOfferteBocciate")
        Private m_NumeroPratiche As New CCursorField(Of Integer)("NumeroPratiche")
        Private m_NumeroPraticheLiquidate As New CCursorField(Of Integer)("NumeroPraticheLiquidate")
        Private m_NumeroPraticheAnnullate As New CCursorField(Of Integer)("NumeroPraticheAnnullate")
        Private m_NumeroPraticheRifiutate As New CCursorField(Of Integer)("NumeroPraticheRifiutate")
        Private m_NumeroPraticheNonFattibili As New CCursorField(Of Integer)("NumeroPraticheNonFattibili")
        Private m_NumeroPraticheBocciate As New CCursorField(Of Integer)("NumeroPraticheBocciate")
        Private m_Flags As New CCursorField(Of Integer)("Flags")

     

        Public Sub New()
        End Sub

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

        Public ReadOnly Property IconaCliente As CCursorFieldObj(Of String)
            Get
                Return Me.m_IconaCliente
            End Get
        End Property

        Public ReadOnly Property StatoLavorazione As CCursorField(Of StatoLavorazione)
            Get
                Return Me.m_StatoLavorazione
            End Get
        End Property

        Public ReadOnly Property SottostatoLavorazione As CCursorField(Of SottostatoLavorazione)
            Get
                Return Me.m_SottostatoLavorazione
            End Get
        End Property

        Public ReadOnly Property DataInizioLavorazione As CCursorField(Of Date)
            Get
                Return Me.m_DataInizioLavorazione
            End Get
        End Property

        Public ReadOnly Property DataFineLavorazione As CCursorField(Of Date)
            Get
                Return Me.m_DataFineLavorazione
            End Get
        End Property

        Public ReadOnly Property DataUltimoAggiornamento As CCursorField(Of Date)
            Get
                Return Me.m_DataUltimoAggiornamento
            End Get
        End Property

        Public ReadOnly Property IDOperatore As CCursorField(Of Integer)
            Get
                Return Me.m_IDOperatore
            End Get
        End Property

        Public ReadOnly Property NomeOperatore As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeOperatore
            End Get
        End Property

        Public ReadOnly Property NumeroBustePaga As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroBustePaga
            End Get
        End Property

        Public ReadOnly Property NumeroVisite As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroVisite
            End Get
        End Property

        'Private m_NumeroVisiteConsulenza As Integer 'Numero di visite con scopo consulenza (prima o successiva) durante il periodo della lavorazione
        Public ReadOnly Property NumeroRichiesteFinanziamento As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroRichiesteFinanziamento
            End Get
        End Property

        Public ReadOnly Property NumeroRichiesteConteggiEstintivi As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroRichiesteConteggiEstintivi
            End Get
        End Property

        Public ReadOnly Property NumeroStudiDiFattibilita As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroStudiDiFattibilita
            End Get
        End Property

        Public ReadOnly Property NumeroOfferteProposte As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroOfferteProposte
            End Get
        End Property

        'Private m_NumeroOfferteAccettate As Integer 'Numero di studi di fattibilità accettati dal cliente durante il periodo della lavorazione
        Public ReadOnly Property NumeroOfferteRifiutate As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroOfferteRifiutate
            End Get
        End Property

        Public ReadOnly Property NumeroOfferteNonFattibili As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroOfferteNonFattibili
            End Get
        End Property

        Public ReadOnly Property NumeroOfferteBocciate As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroOfferteBocciate
            End Get
        End Property

        Public ReadOnly Property NumeroPratiche As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroPratiche
            End Get
        End Property

        Public ReadOnly Property NumeroPraticheLiquidate As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroPraticheLiquidate
            End Get
        End Property

        Public ReadOnly Property NumeroPraticheAnnullate As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroPraticheAnnullate
            End Get
        End Property

        Public ReadOnly Property NumeroPraticheRifiutate As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroPraticheRifiutate
            End Get
        End Property

        Public ReadOnly Property NumeroPraticheNonFattibili As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroPraticheNonFattibili
            End Get
        End Property

        Public ReadOnly Property NumeroPraticheBocciate As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroPraticheBocciate
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return CQSPD.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDClientiLavorati"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function
    End Class




End Class
