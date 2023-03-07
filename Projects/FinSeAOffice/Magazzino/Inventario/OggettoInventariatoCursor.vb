Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class Office
 
    Public Class OggettoInventariatoCursor
        Inherits DBObjectCursorPO(Of OggettoInventariato)

        Private m_IDArticolo As New CCursorField(Of Integer)("IDArticolo")
        Private m_Codice As New CCursorFieldObj(Of String)("Codice")
        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_NomeArticolo As New CCursorFieldObj(Of String)("NomeArticolo")
        Private m_Categoria As New CCursorFieldObj(Of String)("Categoria")
        Private m_Marca As New CCursorFieldObj(Of String)("Marca")
        Private m_Modello As New CCursorFieldObj(Of String)("Modello")
        Private m_Seriale As New CCursorFieldObj(Of String)("Seriale")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_IconURL As New CCursorFieldObj(Of String)("IconURL")
        Private m_StatoAttuale As New CCursorField(Of StatoOggettoInventariato)("StatoAttuale")
        Private m_DettaglioStatoAttuale As New CCursorFieldObj(Of String)("DettaglioStatoAttuale")
        Private m_ValoreStimato As New CCursorField(Of Decimal)("ValoreStimato")
        Private m_DataValutazione As New CCursorField(Of Date)("DataValutazione")
        Private m_TipoInUsoDa As New CCursorFieldObj(Of String)("TipoInUsoDa")
        Private m_IDInUsoDa As New CCursorField(Of Integer)("InUsoDaID")
        Private m_NomeInUsoDa As New CCursorFieldObj(Of String)("NomeInUsoDa")

        Private m_DataProduzione As New CCursorField(Of Date)("DataProduzione")
        Private m_DataAcquisto As New CCursorField(Of Date)("DataAscquisto")
        Private m_TipoDocumentoAcquisto As New CCursorFieldObj(Of String)("TipoDocumentoAcquisto")
        Private m_NumeroDocumentoAcquisto As New CCursorFieldObj(Of String)("NumeroDocumentoAcquisto")
        Private m_StatoAcquisto As New CCursorField(Of StatoAcquistoOggettoInventariato)("StatoAcquisto")
        Private m_DettaglioStatoAcquisto As New CCursorFieldObj(Of String)("DettaglioStatoAcquisto")
        Private m_AcquistatoDaID As New CCursorField(Of Integer)("AcquistatoDaID")
        Private m_NomeAcquistatoDa As New CCursorFieldObj(Of String)("NomeAcquistatoDa")
        Private m_PrezzoAcquisto As New CCursorField(Of Decimal)("PrezzoAcquisto")
        Private m_AliquotaIVA As New CCursorField(Of Single)("AliquotaIVA")

        Private m_IDUfficioOriginale As New CCursorField(Of Integer)("IDUfficioOriginale")
        Private m_NomeUfficioOriginale As New CCursorFieldObj(Of String)("NomeUfficioOriginale")

        Private m_CodiceScaffale As New CCursorFieldObj(Of String)("CodiceScaffale")
        Private m_CodiceReparto As New CCursorFieldObj(Of String)("CodiceReparto")

        Private m_DataDismissione As New CCursorField(Of Date)("DataDismissione")
        Private m_DismessoDaID As New CCursorField(Of Integer)("DismessoDaID")
        Private m_NomeDismessoDa As New CCursorFieldObj(Of String)("NomeDismessoDa")
        Private m_MotivoDismissione As New CCursorFieldObj(Of String)("MotivoDismissione")
        Private m_DettaglioDismissione As New CCursorFieldObj(Of String)("DettaglioDismissione")
        Private m_ValoreDismissione As New CCursorField(Of Decimal)("ValoreDismissione")
        Private m_AliquotaIVADismissione As New CCursorField(Of Single)("AliquotaIVADismissione")
        Private m_Flags As New CCursorField(Of FlagsOggettoInventariato)("Flags")
        Private m_IDOrdineAcquisto As New CCursorField(Of Integer)("IDOrdineAcquisto")
        Private m_IDDocumentoAcquisto As New CCursorField(Of Integer)("IDDocumentoAcquisto")
        Private m_IDSpedizione As New CCursorField(Of Integer)("IDSpedizione")
        Private m_CodiceRFID As New CCursorFieldObj(Of String)("CodiceRFID")

        Public Sub New()
        End Sub

        Public ReadOnly Property CodiceRFID As CCursorFieldObj(Of String)
            Get
                Return Me.m_CodiceRFID
            End Get
        End Property

        Public ReadOnly Property IDOrdineAcquisto As CCursorField(Of Integer)
            Get
                Return Me.m_IDOrdineAcquisto
            End Get
        End Property

        Public ReadOnly Property IDDocumentoAcquisto As CCursorField(Of Integer)
            Get
                Return Me.m_IDDocumentoAcquisto
            End Get
        End Property

        Public ReadOnly Property IDSpedizione As CCursorField(Of Integer)
            Get
                Return Me.m_IDSpedizione
            End Get
        End Property

        Public ReadOnly Property IDArticolo As CCursorField(Of Integer)
            Get
                Return Me.m_IDArticolo
            End Get
        End Property

        Public ReadOnly Property Codice As CCursorFieldObj(Of String)
            Get
                Return Me.m_Codice
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property NomeArticolo As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeArticolo
            End Get
        End Property

        Public ReadOnly Property Categoria As CCursorFieldObj(Of String)
            Get
                Return Me.m_Categoria
            End Get
        End Property

        Public ReadOnly Property Marca As CCursorFieldObj(Of String)
            Get
                Return Me.m_Marca
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

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property IconURL As CCursorFieldObj(Of String)
            Get
                Return Me.m_IconURL
            End Get
        End Property

        Public ReadOnly Property StatoAttuale As CCursorField(Of StatoOggettoInventariato)
            Get
                Return Me.m_StatoAttuale
            End Get
        End Property

        Public ReadOnly Property DettaglioStatoAttuale As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioStatoAttuale
            End Get
        End Property

        Public ReadOnly Property ValoreStimato As CCursorField(Of Decimal)
            Get
                Return Me.m_ValoreStimato
            End Get
        End Property

        Public ReadOnly Property DataValutazione As CCursorField(Of Date)
            Get
                Return Me.m_DataValutazione
            End Get
        End Property

        Public ReadOnly Property TipoInUsoDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoInUsoDa
            End Get
        End Property

        Public ReadOnly Property IDInUsoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDInUsoDa
            End Get
        End Property

        Public ReadOnly Property NomeInUsoDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeInUsoDa
            End Get
        End Property

        Public ReadOnly Property DataProduzione As CCursorField(Of Date)
            Get
                Return Me.m_DataProduzione
            End Get
        End Property

        Public ReadOnly Property DataAcquisto As CCursorField(Of Date)
            Get
                Return Me.m_DataAcquisto
            End Get
        End Property

        Public ReadOnly Property TipoDocumentoAcquisto As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoDocumentoAcquisto
            End Get
        End Property

        Public ReadOnly Property NumeroDocumentoAcquisto As CCursorFieldObj(Of String)
            Get
                Return Me.m_NumeroDocumentoAcquisto
            End Get
        End Property

        Public ReadOnly Property StatoAcquisto As CCursorField(Of StatoAcquistoOggettoInventariato)
            Get
                Return Me.m_StatoAcquisto
            End Get
        End Property

        Public ReadOnly Property DettaglioStatoAcquisto As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioStatoAcquisto
            End Get
        End Property

        Public ReadOnly Property AcquistatoDaID As CCursorField(Of Integer)
            Get
                Return Me.m_AcquistatoDaID
            End Get
        End Property

        Public ReadOnly Property NomeAcquistatoDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeAcquistatoDa
            End Get
        End Property

        Public ReadOnly Property PrezzoAcquisto As CCursorField(Of Decimal)
            Get
                Return Me.m_PrezzoAcquisto
            End Get
        End Property

        Public ReadOnly Property AliquotaIVA As CCursorField(Of Single)
            Get
                Return Me.m_AliquotaIVA
            End Get
        End Property

        Public ReadOnly Property IDUfficioOriginale As CCursorField(Of Integer)
            Get
                Return Me.m_IDUfficioOriginale
            End Get
        End Property

        Public ReadOnly Property NomeUfficioOriginale As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeUfficioOriginale
            End Get
        End Property

        Public ReadOnly Property CodiceScaffale As CCursorFieldObj(Of String)
            Get
                Return Me.m_CodiceScaffale
            End Get
        End Property

        Public ReadOnly Property CodiceReparto As CCursorFieldObj(Of String)
            Get
                Return Me.m_CodiceReparto
            End Get
        End Property

        Public ReadOnly Property DataDismissione As CCursorField(Of Date)
            Get
                Return Me.m_DataDismissione
            End Get
        End Property

        Public ReadOnly Property DismessoDaID As CCursorField(Of Integer)
            Get
                Return Me.m_DismessoDaID
            End Get
        End Property

        Public ReadOnly Property NomeDismessoDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeDismessoDa
            End Get
        End Property

        Public ReadOnly Property MotivoDismissione As CCursorFieldObj(Of String)
            Get
                Return Me.m_MotivoDismissione
            End Get
        End Property

        Public ReadOnly Property DettaglioDismissione As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioDismissione
            End Get
        End Property

        Public ReadOnly Property ValoreDismissione As CCursorField(Of Decimal)
            Get
                Return Me.m_ValoreDismissione
            End Get
        End Property

        Public ReadOnly Property AliquotaIVADismissione As CCursorField(Of Single)
            Get
                Return Me.m_AliquotaIVADismissione
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of FlagsOggettoInventariato)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Office.OggettiInventariati.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeOggettiInventariati"
        End Function


    End Class

End Class


