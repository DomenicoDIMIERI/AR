Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    ''' <summary>
    ''' Cursore sulla tabella delle offerte
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CCQSPDOfferteCursor
        Inherits DBObjectCursor(Of COffertaCQS)

        Private m_OffertaLibera As New CCursorField(Of Boolean)("OffertaLibera")
        Private m_IDPratica As New CCursorField(Of Integer)("IDPratica")
        Private m_IDCliente As New CCursorField(Of Integer)("IDCliente")
        Private m_NomeCliente As New CCursorFieldObj(Of String)("NomeCliente")
        Private m_StatoOfferta As New CCursorField(Of StatoOfferta)("StatoOfferta")
        Private m_PreventivoID As New CCursorField(Of Integer)("Preventivo")
        Private m_IDCessionario As New CCursorField(Of Integer)("IDCessionario")
        Private m_NomeCessionario As New CCursorFieldObj(Of String)("NomeCessionario")
        Private m_IDProfilo As New CCursorField(Of Integer)("IDProfilo")
        Private m_NomeProfilo As New CCursorFieldObj(Of String)("NomeProfilo")
        Private m_ProdottoID As New CCursorField(Of Integer)("Prodotto")
        Private m_NomeProdotto As New CCursorFieldObj(Of String)("NomeProdotto")
        Private m_Calcolato As New CCursorField(Of Boolean)("Calcolato")
        Private m_Durata As New CCursorField(Of Integer)("Durata")
        Private m_Rata As New CCursorField(Of Decimal)("Rata")
        Private m_Eta As New CCursorField(Of Double)("Eta")
        Private m_Anzianita As New CCursorField(Of Double)("Anzianita")
        Private m_Rappel As New CCursorField(Of Double)("Rappel")
        Private m_TabellaAssicurativaRelID As New CCursorField(Of Integer)("TabellaAssicurativaRel")
        Private m_NomeTabellaAssicurativa As New CCursorFieldObj(Of String)("NomeTabellaAssicurativa")
        Private m_TabellaFinanziariaRelID As New CCursorField(Of Integer)("TabellaFinanziariaRel")
        Private m_NomeTabellaFinanziaria As New CCursorFieldObj(Of String)("NomeTabellaFinanziaria")
        Private m_TabellaSpeseID As New CCursorField(Of Integer)("TabellaSpese")
        Private m_NomeTabellaSpese As New CCursorFieldObj(Of String)("NomeTabellaSpese")
        Private m_ProvvigioneMassima As New CCursorField(Of Double)("MaxProvv")
        Private m_SpreadBase As New CCursorField(Of Double)("SpreadBase")
        Private m_Spread As New CCursorField(Of Double)("Spread")
        Private m_Provvigioni As New CCursorField(Of Double)("Provvigioni")
        Private m_UpFront As New CCursorField(Of Double)("UpFront")
        Private m_Running As New CCursorField(Of Double)("Running")
        Private m_PremioVita As New CCursorField(Of Decimal)("PremioVita")
        Private m_PremioImpiego As New CCursorField(Of Decimal)("PremioImpiego")
        Private m_PremioCredito As New CCursorField(Of Decimal)("PremioCredito")
        Private m_DataNascita As New CCursorField(Of Date)("DataNascita")
        Private m_DataAssunzione As New CCursorField(Of Date)("DataAssunzione")
        Private m_ImpostaSostitutiva As New CCursorField(Of Decimal)("ImpostaSostitutiva")
        Private m_OneriErariali As New CCursorField(Of Decimal)("OneriErariali")
        Private m_NettoRicavo As New CCursorField(Of Decimal)("NettoRicavo")
        Private m_CommissioniBancarie As New CCursorField(Of Decimal)("CommissioniBancarie")
        Private m_Interessi As New CCursorField(Of Decimal)("Interessi")
        Private m_Imposte As New CCursorField(Of Decimal)("Imposte")
        Private m_SpeseConvenzioni As New CCursorField(Of Decimal)("SpeseConvenzioni")
        Private m_AltreSpese As New CCursorField(Of Decimal)("AltreSpese")
        Private m_Rivalsa As New CCursorField(Of Decimal)("Rivalsa")
        Private m_TEG As New CCursorField(Of Double)("TEG")
        Private m_TEG_Max As New CCursorField(Of Double)("TEG_Max")
        Private m_TAEG As New CCursorField(Of Double)("TAEG")
        Private m_TAEG_Max As New CCursorField(Of Double)("TAEG_Max")
        Private m_TAN As New CCursorField(Of Double)("TAN")
        Private m_DataDecorrenza As New CCursorField(Of Date)("DataDecorrenza")
        Private m_Sesso As New CCursorFieldObj(Of String)("Sesso")
        Private m_CaricaAlMassimo As New CCursorField(Of Boolean)("CaricaAlMassimo")
        Private m_TipoCalcoloTEG As New CCursorField(Of TEGCalcFlag)("TipoCalcoloTEG")
        Private m_TipoCalcoloTAEG As New CCursorField(Of TEGCalcFlag)("TipoCalcoloTAEG")
        Private m_ErrorCode As New CCursorField(Of ErrorCodes)("ErrorCode")
        Private m_Messages As New CCursorFieldObj(Of String)("Messages")
        Private m_Flags As New CCursorField(Of OffertaFlags)("Flags")
        Private m_ValoreRiduzioneProvvigionale = New CCursorField(Of Decimal)("ValoreRiduzioneProvv")
        Private m_CapitaleFinanziato As New CCursorField(Of Decimal)("CapitaleFinanziato")

        Public Sub New()
        End Sub

        Public ReadOnly Property CapitaleFinanziato As CCursorField(Of Decimal)
            Get
                Return Me.m_CapitaleFinanziato
            End Get
        End Property

        Public ReadOnly Property ValoreRiduzioneProvvigionale As CCursorField(Of Decimal)
            Get
                Return Me.m_ValoreRiduzioneProvvigionale
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public ReadOnly Property Flags As CCursorField(Of OffertaFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property



        Public ReadOnly Property OffertaLibera As CCursorField(Of Boolean)
            Get
                Return Me.m_OffertaLibera
            End Get
        End Property

        Public ReadOnly Property IDPratica As CCursorField(Of Integer)
            Get
                Return Me.m_IDPratica
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

        Public ReadOnly Property StatoOfferta As CCursorField(Of StatoOfferta)
            Get
                Return Me.m_StatoOfferta
            End Get
        End Property

        Public ReadOnly Property PreventivoID As CCursorField(Of Integer)
            Get
                Return Me.m_PreventivoID
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

        Public ReadOnly Property IDProfilo As CCursorField(Of Integer)
            Get
                Return Me.m_IDProfilo
            End Get
        End Property

        Public ReadOnly Property NomeProfilo As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeProfilo
            End Get
        End Property

        Public ReadOnly Property ProdottoID As CCursorField(Of Integer)
            Get
                Return Me.m_ProdottoID
            End Get
        End Property

        Public ReadOnly Property NomeProdotto As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeProdotto
            End Get
        End Property

        Public ReadOnly Property Calcolato As CCursorField(Of Boolean)
            Get
                Return Me.m_Calcolato
            End Get
        End Property

        Public ReadOnly Property Durata As CCursorField(Of Integer)
            Get
                Return Me.m_Durata
            End Get
        End Property

        Public ReadOnly Property Rata As CCursorField(Of Decimal)
            Get
                Return Me.m_Rata
            End Get
        End Property

        Public ReadOnly Property Eta As CCursorField(Of Double)
            Get
                Return Me.m_Eta
            End Get
        End Property

        Public ReadOnly Property Anzianita As CCursorField(Of Double)
            Get
                Return Me.m_Anzianita
            End Get
        End Property

        Public ReadOnly Property Rappel As CCursorField(Of Double)
            Get
                Return Me.m_Rappel
            End Get
        End Property

        Public ReadOnly Property TabellaAssicurativaRelID As CCursorField(Of Integer)
            Get
                Return Me.m_TabellaAssicurativaRelID
            End Get
        End Property

        Public ReadOnly Property NomeTabellaAssicurativa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeTabellaAssicurativa
            End Get
        End Property

        Public ReadOnly Property TabellaFinanziariaRelID As CCursorField(Of Integer)
            Get
                Return Me.m_TabellaFinanziariaRelID
            End Get
        End Property

        Public ReadOnly Property NomeTabellaFinanziaria As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeTabellaFinanziaria
            End Get
        End Property

        Public ReadOnly Property TabellaSpeseID As CCursorField(Of Integer)
            Get
                Return Me.m_TabellaSpeseID
            End Get
        End Property

        Public ReadOnly Property NomeTabellaSpese As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeTabellaSpese
            End Get
        End Property

        Public ReadOnly Property ProvvigioneMassima As CCursorField(Of Double)
            Get
                Return Me.m_ProvvigioneMassima
            End Get
        End Property

        Public ReadOnly Property SpreadBase As CCursorField(Of Double)
            Get
                Return Me.m_SpreadBase
            End Get
        End Property

        Public ReadOnly Property Spread As CCursorField(Of Double)
            Get
                Return Me.m_Spread
            End Get
        End Property

        Public ReadOnly Property Provvigioni As CCursorField(Of Double)
            Get
                Return Me.m_Provvigioni
            End Get
        End Property

        Public ReadOnly Property UpFront As CCursorField(Of Double)
            Get
                Return Me.m_UpFront
            End Get
        End Property

        Public ReadOnly Property Running As CCursorField(Of Double)
            Get
                Return Me.m_Running
            End Get
        End Property

        Public ReadOnly Property PremioVita As CCursorField(Of Decimal)
            Get
                Return Me.m_PremioVita
            End Get
        End Property

        Public ReadOnly Property PremioImpiego As CCursorField(Of Decimal)
            Get
                Return Me.m_PremioImpiego
            End Get
        End Property

        Public ReadOnly Property PremioCredito As CCursorField(Of Decimal)
            Get
                Return Me.m_PremioCredito
            End Get
        End Property

        Public ReadOnly Property DataNascita As CCursorField(Of Date)
            Get
                Return Me.m_DataNascita
            End Get
        End Property

        Public ReadOnly Property DataAssunzione As CCursorField(Of Date)
            Get
                Return Me.m_DataAssunzione
            End Get
        End Property

        Public ReadOnly Property ImpostaSostitutiva As CCursorField(Of Decimal)
            Get
                Return Me.m_ImpostaSostitutiva
            End Get
        End Property

        Public ReadOnly Property OneriErariali As CCursorField(Of Decimal)
            Get
                Return Me.m_OneriErariali
            End Get
        End Property

        Public ReadOnly Property NettoRicavo As CCursorField(Of Decimal)
            Get
                Return Me.m_NettoRicavo
            End Get
        End Property

        Public ReadOnly Property CommissioniBancarie As CCursorField(Of Decimal)
            Get
                Return Me.m_CommissioniBancarie
            End Get
        End Property

        Public ReadOnly Property Interessi As CCursorField(Of Decimal)
            Get
                Return Me.m_Interessi
            End Get
        End Property

        Public ReadOnly Property Imposte As CCursorField(Of Decimal)
            Get
                Return Me.m_Imposte
            End Get
        End Property

        Public ReadOnly Property SpeseConvenzioni As CCursorField(Of Decimal)
            Get
                Return Me.m_SpeseConvenzioni
            End Get
        End Property

        Public ReadOnly Property AltreSpese As CCursorField(Of Decimal)
            Get
                Return Me.m_AltreSpese
            End Get
        End Property

        Public ReadOnly Property Rivalsa As CCursorField(Of Decimal)
            Get
                Return Me.m_Rivalsa
            End Get
        End Property

        Public ReadOnly Property TEG As CCursorField(Of Double)
            Get
                Return Me.m_TEG
            End Get
        End Property

        Public ReadOnly Property TEG_Max As CCursorField(Of Double)
            Get
                Return Me.m_TEG_Max
            End Get
        End Property

        Public ReadOnly Property TAEG As CCursorField(Of Double)
            Get
                Return Me.m_TAEG
            End Get
        End Property

        Public ReadOnly Property TAEG_Max As CCursorField(Of Double)
            Get
                Return Me.m_TAEG_Max
            End Get
        End Property

        Public ReadOnly Property TAN As CCursorField(Of Double)
            Get
                Return Me.m_TAN
            End Get
        End Property

        Public ReadOnly Property DataDecorrenza As CCursorField(Of Date)
            Get
                Return Me.m_DataDecorrenza
            End Get
        End Property

        Public ReadOnly Property Sesso As CCursorFieldObj(Of String)
            Get
                Return Me.m_Sesso
            End Get
        End Property

        Public ReadOnly Property CaricaAlMassimo As CCursorField(Of Boolean)
            Get
                Return Me.m_CaricaAlMassimo
            End Get
        End Property

        Public ReadOnly Property TipoCalcoloTEG As CCursorField(Of TEGCalcFlag)
            Get
                Return Me.m_TipoCalcoloTEG
            End Get
        End Property

        Public ReadOnly Property TipoCalcoloTAEG As CCursorField(Of TEGCalcFlag)
            Get
                Return Me.m_TipoCalcoloTAEG
            End Get
        End Property

        Public ReadOnly Property ErrorCode As CCursorField(Of ErrorCodes)
            Get
                Return Me.m_ErrorCode
            End Get
        End Property

        Public ReadOnly Property Messages As CCursorFieldObj(Of String)
            Get
                Return Me.m_Messages
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New COffertaCQS
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Preventivi_Offerte"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return DMD.CQSPD.Offerte.Module
        End Function


    End Class


End Class
