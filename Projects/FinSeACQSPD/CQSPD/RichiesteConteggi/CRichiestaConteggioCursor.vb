Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD


    Public Class CRichiestaConteggioCursor
        Inherits DBObjectCursorPO(Of CRichiestaConteggio)

        Private m_IDRichiestaDiFinanziamento As New CCursorField(Of Integer)("IDRichiestaF")
        Private m_DataRichiesta As New CCursorField(Of Date)("DataRichiesta")
        Private m_IDIstituto As New CCursorField(Of Integer)("IDIstituto")
        Private m_NomeIstituto As New CCursorFieldObj(Of String)("NomeIstituto")
        Private m_IDAgenziaRichiedente As New CCursorField(Of Integer)("IDAgenziaR")
        Private m_NomeAgenziaRichiedente As New CCursorFieldObj(Of String)("NomeAgenziaR")
        Private m_IDAgente As New CCursorField(Of Integer)("IDAgente")
        Private m_NomeAgente As New CCursorFieldObj(Of String)("NomeAgente")
        Private m_DataEvasione As New CCursorField(Of Date)("DataEvasione")
        Private m_IDPratica As New CCursorField(Of Integer)("IDPratica")
        Private m_NumeroPratica As New CCursorFieldObj(Of String)("NumeroPratica")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_IDAllegato As New CCursorField(Of Integer)("IDAllegato")
        Private m_PresaInCaricoDaID As New CCursorField(Of Integer)("PresaInCaricoDaID")
        Private m_PresaInCaricoDaNome As New CCursorField(Of Integer)("PresaInCaricoDaNome")
        Private m_DataPresaInCarico As New CCursorField(Of Date)("DataPresaInCarico")
        Private m_DataSegnalazione As New CCursorField(Of Date)("DataSegnalazione")
        Private m_IDCliente As New CCursorField(Of Integer)("IDCliente")
        Private m_NomeCliente As New CCursorFieldObj(Of String)("NomeCliente")
        Private m_ImportoCE As New CCursorField(Of Decimal)("ImportoCE")
        Private m_InviatoDaID As New CCursorField(Of Integer)("InviatoDaID")
        Private m_InviatoDaNome As New CCursorFieldObj(Of String)("InviatoDaNome")
        Private m_InviatoIl As New CCursorField(Of Date)("InviatoIl")
        Private m_RicevutoDaID As New CCursorField(Of Integer)("RicevutoDaID")
        Private m_RicevutoDaNome As New CCursorFieldObj(Of String)("RicevutoDaNome")
        Private m_RicevutoIl As New CCursorField(Of Date)("RicevutoIl")
        Private m_MezzoDiInvio As New CCursorFieldObj(Of String)("MezzoDiInvio")
        Private m_IDFinestraLavorazione As New CCursorField(Of Integer)("IDFinestraLavorazione")
        Private m_Esito As New CCursorFieldObj(Of String)("Esito")
        Private m_IDCessionario As New CCursorField(Of Integer)("IDCessionario")
        Private m_NomeCessionario As New CCursorFieldObj(Of String)("NomeCessionario")
        Private m_DurataMesi As New CCursorField(Of Integer)("DurataMesi")
        Private m_ImportoRata As New CCursorField(Of Decimal)("ImportoRata")
        Private m_TAN As New CCursorField(Of Double)("TAN")
        Private m_TAEG As New CCursorField(Of Double)("TAEG")
        Private m_DataDecorrenzaPratica As New CCursorField(Of DateTime)("DataDecorrenzaPratica")
        Private m_UltimaScadenza As New CCursorField(Of DateTime)("UltimaScadenza")
        Private m_ATCPIVA As New CCursorFieldObj(Of String)("ATCPIVA")
        Private m_ATCDescrizione As New CCursorFieldObj(Of String)("ATCDescrizione")

        Public Sub New()
        End Sub

        Public ReadOnly Property Esito As CCursorFieldObj(Of String)
            Get
                Return Me.m_Esito
            End Get
        End Property

        Public ReadOnly Property IDFinestraLavorazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDFinestraLavorazione
            End Get
        End Property


        Public ReadOnly Property InviatoDaID As CCursorField(Of Integer)
            Get
                Return Me.m_InviatoDaID
            End Get
        End Property

        Public ReadOnly Property InviatoDaNome As CCursorFieldObj(Of String)
            Get
                Return Me.m_InviatoDaNome
            End Get
        End Property

        Public ReadOnly Property InviatoIl As CCursorField(Of Date)
            Get
                Return Me.m_InviatoIl
            End Get
        End Property

        Public ReadOnly Property RicevutoDaID As CCursorField(Of Integer)
            Get
                Return Me.m_RicevutoDaID
            End Get
        End Property

        Public ReadOnly Property RicevutoDaNome As CCursorFieldObj(Of String)
            Get
                Return Me.m_RicevutoDaNome
            End Get
        End Property

        Public ReadOnly Property RicevutoIl As CCursorField(Of Date)
            Get
                Return Me.m_RicevutoIl
            End Get
        End Property

        Public ReadOnly Property MezzoDiInvio As CCursorFieldObj(Of String)
            Get
                Return Me.m_MezzoDiInvio
            End Get
        End Property


        Public ReadOnly Property ImportoCE As CCursorField(Of Decimal)
            Get
                Return Me.m_ImportoCE
            End Get
        End Property

        Public ReadOnly Property IDPratica As CCursorField(Of Integer)
            Get
                Return Me.m_IDPratica
            End Get
        End Property

        Public ReadOnly Property NumeroPratica As CCursorFieldObj(Of String)
            Get
                Return Me.m_NumeroPratica
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property IDAllegato As CCursorField(Of Integer)
            Get
                Return Me.m_IDAllegato
            End Get
        End Property

        Public ReadOnly Property PresaInCaricoDaID As CCursorField(Of Integer)
            Get
                Return Me.m_PresaInCaricoDaID
            End Get
        End Property

        Public ReadOnly Property PresaInCaricoDaNome As CCursorField(Of Integer)
            Get
                Return Me.m_PresaInCaricoDaNome
            End Get
        End Property

        Public ReadOnly Property DataPresaInCarico As CCursorField(Of Date)
            Get
                Return Me.m_DataPresaInCarico
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

        Public ReadOnly Property IDRichiestaDiFinanziamento As CCursorField(Of Integer)
            Get
                Return Me.m_IDRichiestaDiFinanziamento
            End Get
        End Property

        Public ReadOnly Property DataRichiesta As CCursorField(Of Date)
            Get
                Return Me.m_DataRichiesta
            End Get
        End Property

        Public ReadOnly Property DataSegnalazione As CCursorField(Of Date)
            Get
                Return Me.m_DataSegnalazione
            End Get
        End Property

        Public ReadOnly Property IDIstituto As CCursorField(Of Integer)
            Get
                Return Me.m_IDIstituto
            End Get
        End Property

        Public ReadOnly Property NomeIstituto As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeIstituto
            End Get
        End Property

        Public ReadOnly Property IDAgenziaRichiedente As CCursorField(Of Integer)
            Get
                Return Me.m_IDAgenziaRichiedente
            End Get
        End Property

        Public ReadOnly Property NomeAgenziaRichiedente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAgenziaRichiedente
            End Get
        End Property

        Public ReadOnly Property IDAgente As CCursorField(Of Integer)
            Get
                Return Me.m_IDAgente
            End Get
        End Property

        Public ReadOnly Property NomeAgente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAgente
            End Get
        End Property

        Public ReadOnly Property DataEvasione As CCursorField(Of Date)
            Get
                Return Me.m_DataEvasione
            End Get
        End Property

        Public ReadOnly Property IDCessionario As CCursorField(Of Integer)
            Get
                Return Me.m_IDCessionario
            End Get
        End Property

        Public ReadOnly Property NomeCessionario As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeCessionario
            End Get
        End Property

        Public ReadOnly Property DurataMesi As CCursorField(Of Integer)
            Get
                Return Me.m_DurataMesi
            End Get
        End Property

        Public ReadOnly Property ImportoRata As CCursorField(Of Decimal)
            Get
                Return Me.m_ImportoRata
            End Get
        End Property

        Public ReadOnly Property TAN As CCursorField(Of Double)
            Get
                Return Me.m_TAN
            End Get
        End Property

        Public ReadOnly Property TAEG As CCursorField(Of Double)
            Get
                Return Me.m_TAEG
            End Get
        End Property

        Public ReadOnly Property DataDecorrenzaPratica As CCursorField(Of DateTime)
            Get
                Return Me.m_DataDecorrenzaPratica
            End Get
        End Property

        Public ReadOnly Property UltimaScadenza As CCursorField(Of DateTime)
            Get
                Return Me.m_UltimaScadenza
            End Get
        End Property

        Public ReadOnly Property ATCPIVA As CCursorFieldObj(Of String)
            Get
                Return Me.m_ATCPIVA
            End Get
        End Property

        Public ReadOnly Property ATCDescrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_ATCDescrizione
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return CQSPD.RichiesteConteggi.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_RichiesteFinanziamentiC"
        End Function
    End Class


End Class
