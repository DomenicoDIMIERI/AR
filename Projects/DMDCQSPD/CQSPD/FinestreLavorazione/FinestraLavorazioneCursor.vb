Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.CustomerCalls
Imports DMD.Office

Partial Public Class CQSPD

    <Serializable>
    Public Class FinestraLavorazioneCursor
        Inherits DBObjectCursorPO(Of FinestraLavorazione)


        Private m_IDCliente As New CCursorField(Of Integer)("IDCliente")
        Private m_NomeCliente As New CCursorFieldObj(Of String)("NomeCliente")
        Private m_IconaCliente As New CCursorFieldObj(Of String)("IconaCliente")
        Private m_StatoFinestra As New CCursorField(Of StatoFinestraLavorazione)("StatoFinestra")
        Private m_Flags As New CCursorField(Of FinestraLavorazioneFlags)("Flags")
        'Private m_DataInizioLavorabilita As New CCursorField(Of Date)("DataInizioLavorabilita")
        Private m_DataInizioLavorabilita As New CCursorFieldDBDate("DataInizioLavorabilitaStr")
        Private m_DataFineLavorabilita As New CCursorField(Of Date)("DataFineLavorabilita")
        Private m_DataInizioLavorazione As New CCursorField(Of Date)("DataInizioLavorazione")
        Private m_IDRichiestaFinanziamento As New CCursorField(Of Integer)("IDRichiestaF")
        Private m_IDStudioDiFattibilita As New CCursorField(Of Integer)("IDStudioF")
        Private m_IDCQS As New CCursorField(Of Integer)("IDCQS")
        Private m_IDPD As New CCursorField(Of Integer)("IDPD")
        Private m_IDCQSI As New CCursorField(Of Integer)("IDCQSI")
        Private m_IDPDI As New CCursorField(Of Integer)("IDPDI")
        Private m_StatoCQS As New CCursorField(Of StatoOfferteFL)("StatoCQS")
        Private m_StatoPD As New CCursorField(Of StatoOfferteFL)("StatoPD")
        Private m_StatoCQSI As New CCursorField(Of StatoOfferteFL)("StatoCQSI")
        Private m_StatoPDI As New CCursorField(Of StatoOfferteFL)("StatoPDI")
        Private m_DataUltimoAggiornamento As New CCursorField(Of Date)("DataFineLavorazione")
        Private m_DataFineLavorazione As New CCursorField(Of Date)("DataUltimoAggiornamento")
        Private m_QuotaCedibile As New CCursorField(Of Decimal)("QuotaCedibile")
        Private m_IDBustaPaga As New CCursorField(Of Integer)("IDBustaPaga")
        Private m_StatoRichiestaFinanziamento As New CCursorField(Of StatoOfferteFL)("StatoRichiestaF")
        Private m_StatoStudioDiFattibilita As New CCursorField(Of StatoOfferteFL)("StatoSF")
        Private m_IDContatto As New CCursorField(Of Integer)("IDContatto")
        Private m_StatoContatto As New CCursorField(Of StatoOfferteFL)("StatoContatto")
        Private m_DataContatto As New CCursorField(Of Date)("DataContatto")
        'Private m_DataContatto As New CCursorFieldDBDate("DataContattoStr")
        Private m_DataBustaPaga As New CCursorField(Of Date)("DataBustaPaga")
        Private m_StatoBustaPaga As New CCursorField(Of StatoOfferteFL)("StatoBustaPaga")
        Private m_IDRichiestaCertificato As New CCursorField(Of Integer)("IDRichiestaCertificato")
        Private m_DataRichiestaCertificato As New CCursorField(Of Date)("DataRichiestaCertificato")
        Private m_StatoRichiestaCertificato As New CCursorField(Of StatoOfferteFL)("StatoRichiestaCertificato")
        Private m_DataRichiestaFinanziamento As New CCursorField(Of Date)("DataRichiestaFinanziamento")
        Private m_DataStudioDiFattibilita As New CCursorField(Of Date)("DataStudioDiFattibilita")
        Private m_DataCQS As New CCursorField(Of Date)("DataCQS")
        Private m_DataPD As New CCursorField(Of Date)("DataPD")
        Private m_DataCQSI As New CCursorField(Of Date)("DataCQSI")
        Private m_DataPDI As New CCursorField(Of Date)("DataPDI")

        Private m_IDPrimaVisita As New CCursorField(Of Integer)("IDPrimaVisita")
        Private m_StatoPrimaVisita As New CCursorField(Of StatoOfferteFL)("StatoPrimaVisita")
        Private m_DataPrimaVisita As New CCursorField(Of Date)("DataPrivaVisita")
        Private m_DataImportazione As New CCursorField(Of Date)("DataImportazione")
        Private m_DataEsportazioneOk As New CCursorField(Of Date)("DataEsportazioneOk")

        Private m_IDCollaboratore As New CCursorField(Of Integer)("IDCollaboratore")

#If UsaDataAttivazione Then
        Private m_DataAttivazione As New CCursorField(Of Date)("DataAttivazione")
        Private m_DettaglioStato As New CCursorFieldObj(Of String)("DettaglioStato")
        Private m_DettaglioStato1 As New CCursorFieldObj(Of String)("DettaglioStato1")
        Private m_DataRicontatto As New CCursorField(Of Date)("DataRicontatto")
        Private m_IDOperatoreRicontatto As New CCursorField(Of Integer)("IDOpRicontatto")
        Private m_MotivoRicontatto As New CCursorFieldObj(Of String)("MotivoRicontatto")
#End If
        Private m_IDConsulente As New CCursorField(Of Integer)("IDConsulente")
        Private m_TipoFonte As New CCursorFieldObj(Of String)("TipoFonte")
        Private m_IDFonte As New CCursorField(Of Integer)("IDFonte")

        Private m_IDConsulenza As New CCursorField(Of Integer)("IDConsulenza")


        Public Sub New()

        End Sub

        Public ReadOnly Property TipoFonte As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoFonte
            End Get
        End Property

        Public ReadOnly Property IDFonte As CCursorField(Of Integer)
            Get
                Return Me.m_IDFonte
            End Get
        End Property


        Public ReadOnly Property IDCollaboratore As CCursorField(Of Integer)
            Get
                Return Me.m_IDCollaboratore
            End Get
        End Property

        Public ReadOnly Property IDConsulenza As CCursorField(Of Integer)
            Get
                Return Me.m_IDConsulenza
            End Get
        End Property

#If UsaDataAttivazione Then

        Public ReadOnly Property DataAttivazione As CCursorField(Of Date)
            Get
                Return Me.m_DataAttivazione
            End Get
        End Property

        Public ReadOnly Property DettaglioStato As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioStato
            End Get
        End Property

        Public ReadOnly Property DettaglioStato1 As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioStato1
            End Get
        End Property

        Public ReadOnly Property IDOperatoreRicontatto As CCursorField(Of Integer)
            Get
                Return Me.m_IDOperatoreRicontatto
            End Get
        End Property

        Public ReadOnly Property DataRicontatto As CCursorField(Of Date)
            Get
                Return Me.m_DataRicontatto
            End Get
        End Property

        Public ReadOnly Property MotivoRicontatto As CCursorFieldObj(Of String)
            Get
                Return Me.m_MotivoRicontatto
            End Get
        End Property




#End If

        Public ReadOnly Property IDConsulente As CCursorField(Of Integer)
            Get
                Return Me.m_IDConsulente
            End Get
        End Property

        Public ReadOnly Property DataEsportazioneOk As CCursorField(Of Date)
            Get
                Return Me.m_DataEsportazioneOk
            End Get
        End Property

        Public ReadOnly Property DataImportazione As CCursorField(Of Date)
            Get
                Return Me.m_DataImportazione
            End Get
        End Property

        Public ReadOnly Property IDPrimaVisita As CCursorField(Of Integer)
            Get
                Return Me.m_IDPrimaVisita
            End Get
        End Property

        Public ReadOnly Property StatoPrimaVisita As CCursorField(Of StatoOfferteFL)
            Get
                Return Me.m_StatoPrimaVisita
            End Get
        End Property

        Public ReadOnly Property DataPrimaVisita As CCursorField(Of Date)
            Get
                Return Me.m_DataPrimaVisita
            End Get
        End Property


        Public ReadOnly Property DataRichiestaFinanziamento As CCursorField(Of Date)
            Get
                Return Me.m_DataRichiestaFinanziamento
            End Get
        End Property

        Public ReadOnly Property DataStudioDiFattibilita As CCursorField(Of Date)
            Get
                Return Me.m_DataStudioDiFattibilita
            End Get
        End Property

        Public ReadOnly Property DataCQS As CCursorField(Of Date)
            Get
                Return Me.m_DataCQS
            End Get
        End Property

        Public ReadOnly Property DataPD As CCursorField(Of Date)
            Get
                Return Me.m_DataPD
            End Get
        End Property

        Public ReadOnly Property DataCQSI As CCursorField(Of Date)
            Get
                Return Me.m_DataCQSI
            End Get
        End Property

        Public ReadOnly Property DataPDI As CCursorField(Of Date)
            Get
                Return Me.m_DataPDI
            End Get
        End Property

        Public ReadOnly Property IDContatto As CCursorField(Of Integer)
            Get
                Return Me.m_IDContatto
            End Get
        End Property

        Public ReadOnly Property StatoContatto As CCursorField(Of StatoOfferteFL)
            Get
                Return Me.m_StatoContatto
            End Get
        End Property

        Public ReadOnly Property DataContatto As CCursorField(Of Date)
            Get
                Return Me.m_DataContatto
            End Get
        End Property

        'Public ReadOnly Property DataContatto As CCursorFieldDBDate
        '    Get
        '        Return Me.m_DataContatto
        '    End Get
        'End Property


        Public ReadOnly Property StatoRichiestaFinanziamento As CCursorField(Of StatoOfferteFL)
            Get
                Return Me.m_StatoRichiestaFinanziamento
            End Get
        End Property

        Public ReadOnly Property StatoStudioDiFattibilita As CCursorField(Of StatoOfferteFL)
            Get
                Return Me.m_StatoStudioDiFattibilita
            End Get
        End Property


        Public ReadOnly Property IDBustaPaga As CCursorField(Of Integer)
            Get
                Return Me.m_IDBustaPaga
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

        Public ReadOnly Property IconaCliente As CCursorFieldObj(Of String)
            Get
                Return Me.m_IconaCliente
            End Get
        End Property

        Public ReadOnly Property StatoFinestra As CCursorField(Of StatoFinestraLavorazione)
            Get
                Return Me.m_StatoFinestra
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of FinestraLavorazioneFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        'Public ReadOnly Property DataInizioLavorabilita As CCursorField(Of Date)
        '    Get
        '        Return Me.m_DataInizioLavorabilita
        '    End Get
        'End Property

        Public ReadOnly Property DataInizioLavorabilita As CCursorFieldDBDate
            Get
                Return Me.m_DataInizioLavorabilita
            End Get
        End Property

        Public ReadOnly Property DataFineLavorabilita As CCursorField(Of Date)
            Get
                Return Me.m_DataFineLavorabilita
            End Get
        End Property

        Public ReadOnly Property DataInizioLavorazione As CCursorField(Of Date)
            Get
                Return Me.m_DataInizioLavorazione
            End Get
        End Property

        Public ReadOnly Property IDRichiestaFinanziamento As CCursorField(Of Integer)
            Get
                Return Me.m_IDRichiestaFinanziamento
            End Get
        End Property

        Public ReadOnly Property IDStudioDiFattibilita As CCursorField(Of Integer)
            Get
                Return Me.m_IDStudioDiFattibilita
            End Get
        End Property

        Public ReadOnly Property IDCQS As CCursorField(Of Integer)
            Get
                Return Me.m_IDCQS
            End Get
        End Property

        Public ReadOnly Property IDPD As CCursorField(Of Integer)
            Get
                Return Me.m_IDPD
            End Get
        End Property

        Public ReadOnly Property IDCQSI As CCursorField(Of Integer)
            Get
                Return Me.m_IDCQSI
            End Get
        End Property

        Public ReadOnly Property IDPDI As CCursorField(Of Integer)
            Get
                Return Me.m_IDPDI
            End Get
        End Property

        Public ReadOnly Property StatoCQS As CCursorField(Of StatoOfferteFL)
            Get
                Return Me.m_StatoCQS
            End Get
        End Property

        Public ReadOnly Property StatoPD As CCursorField(Of StatoOfferteFL)
            Get
                Return Me.m_StatoPD
            End Get
        End Property

        Public ReadOnly Property StatoCQSI As CCursorField(Of StatoOfferteFL)
            Get
                Return Me.m_StatoCQSI
            End Get
        End Property

        Public ReadOnly Property StatoPDI As CCursorField(Of StatoOfferteFL)
            Get
                Return Me.m_StatoPDI
            End Get
        End Property

        Public ReadOnly Property DataUltimoAggiornamento As CCursorField(Of Date)
            Get
                Return Me.m_DataUltimoAggiornamento
            End Get
        End Property

        Public ReadOnly Property DataFineLavorazione As CCursorField(Of Date)
            Get
                Return Me.m_DataFineLavorazione
            End Get
        End Property

        Public ReadOnly Property QuotaCedibile As CCursorField(Of Decimal)
            Get
                Return Me.m_QuotaCedibile
            End Get
        End Property

        Public ReadOnly Property DataBustaPaga As CCursorField(Of Date)
            Get
                Return Me.m_DataBustaPaga
            End Get
        End Property

        Public ReadOnly Property StatoBustaPaga As CCursorField(Of StatoOfferteFL)
            Get
                Return Me.m_StatoBustaPaga
            End Get
        End Property

        Public ReadOnly Property IDRichiestaCertificato As CCursorField(Of Integer)
            Get
                Return Me.m_IDRichiestaCertificato
            End Get
        End Property

        Public ReadOnly Property DataRichiestaCertificato As CCursorField(Of Date)
            Get
                Return Me.m_DataRichiestaCertificato
            End Get
        End Property

        Public ReadOnly Property StatoRichiestaCertificato As CCursorField(Of StatoOfferteFL)
            Get
                Return Me.m_StatoRichiestaCertificato
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return CQSPD.FinestreDiLavorazione.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDFinestreLavorazione"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New FinestraLavorazione
        End Function

    End Class






End Class
