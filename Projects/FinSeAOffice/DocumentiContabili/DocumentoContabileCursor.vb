Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Imports DMD.GPS

Partial Class Office

    ''' <summary>
    ''' Rappresenta un documento contabile
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class DocumentoContabileCursor
        Inherits DBObjectCursorPO(Of DocumentoContabile)

        Private m_TipoDocumento As New CCursorFieldObj(Of String)("TipoDocumento")
        Private m_NumeroDocumento As New CCursorFieldObj(Of String)("NumeroDocumento")
        Private m_DataRegistrazione As New CCursorField(Of Date)("DataRegistrazione")
        Private m_DataEmissione As New CCursorField(Of Date)("DataEmissione")
        Private m_DataEvasione As New CCursorField(Of Date)("DataEvasione")
        Private m_StatoDocumento As New CCursorField(Of StatoDocumentoContabile)("StatoDocumento")
        Private m_TotaleImponibile As New CCursorField(Of Decimal)("TotaleImponibile")
        Private m_TotaleIvato As New CCursorField(Of Decimal)("TotaleIvato")
        Private m_IDCliente As New CCursorField(Of Integer)("IDCliente")
        Private m_NomeCliente As New CCursorFieldObj(Of String)("NomeCliente")
        Private m_IndirizzoCliente_Nome As New CCursorFieldObj(Of String)("INDCLT_LABEL")
        Private m_IndirizzoCliente_ToponimoViaECivico As New CCursorFieldObj(Of String)("INDCLT_VIA")
        Private m_IndirizzoCliente_CAP As New CCursorFieldObj(Of String)("INDCLT_CAP")
        Private m_IndirizzoCliente_Citta As New CCursorFieldObj(Of String)("INDCLT_CITTA")
        Private m_IndirizzoCliente_Provincia As New CCursorFieldObj(Of String)("INDCLT_PROV")
        Private m_CodiceFiscaleCliente As New CCursorFieldObj(Of String)("CodiceFiscaleCliente")
        Private m_PartitaIVACliente As New CCursorFieldObj(Of String)("PartitaIVACliente")
        Private m_IDFornitore As New CCursorField(Of Integer)("IDFornitore")
        Private m_NomeFornitore As New CCursorFieldObj(Of String)("NomeFornitore")
        Private m_IndirizzoFornitore_Nome As New CCursorFieldObj(Of String)("INDFNT_LABEL")
        Private m_IndirizzoFornitore_ToponimoViaECivico As New CCursorFieldObj(Of String)("INDFNT_VIA")
        Private m_IndirizzoFornitore_CAP As New CCursorFieldObj(Of String)("INDFNT_CAP")
        Private m_IndirizzoFornitore_Citta As New CCursorFieldObj(Of String)("INDFNT_CITTA")
        Private m_IndirizzoFornitore_Provincia As New CCursorFieldObj(Of String)("INDFNT_PROV")
        Private m_CodiceFiscaleFornitore As New CCursorFieldObj(Of String)("CodiceFiscaleFornitore")
        Private m_PartitaIVAFornitore As New CCursorFieldObj(Of String)("PartitaIVAFornitore")
        Private m_IndirizzoSpedizione_Nome As New CCursorFieldObj(Of String)("INDSPD_LABEL")
        Private m_IndirizzoSpedizione_ToponimoViaECivico As New CCursorFieldObj(Of String)("INDSPD_VIA")
        Private m_IndirizzoSpedizione_CAP As New CCursorFieldObj(Of String)("INDSPD_CAP")
        Private m_IndirizzoSpedizione_Citta As New CCursorFieldObj(Of String)("INDSPD_CITTA")
        Private m_IndirizzoSpedizione_Provincia As New CCursorFieldObj(Of String)("INDSPD_PROV")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_SourceType As New CCursorFieldObj(Of String)("SourceType")
        Private m_SourceID As New CCursorField(Of Integer)("SourceID")
        Private m_SourceParams As New CCursorFieldObj(Of String)("SourceParams")

        Public Sub New()

        End Sub

        Public ReadOnly Property TipoDocumento As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoDocumento
            End Get
        End Property

        Public ReadOnly Property NumeroDocumento As CCursorFieldObj(Of String)
            Get
                Return Me.m_NumeroDocumento
            End Get
        End Property

        Public ReadOnly Property DataRegistrazione As CCursorField(Of Date)
            Get
                Return Me.m_DataRegistrazione
            End Get
        End Property

        Public ReadOnly Property DataEmissione As CCursorField(Of Date)
            Get
                Return Me.m_DataEmissione
            End Get
        End Property

        Public ReadOnly Property DataEvasione As CCursorField(Of Date)
            Get
                Return Me.m_DataEvasione
            End Get
        End Property

        Public ReadOnly Property StatoDocumento As CCursorField(Of StatoDocumentoContabile)
            Get
                Return Me.m_StatoDocumento
            End Get
        End Property

        Public ReadOnly Property TotaleImponibile As CCursorField(Of Decimal)
            Get
                Return Me.m_TotaleImponibile
            End Get
        End Property

        Public ReadOnly Property TotaleIvato As CCursorField(Of Decimal)
            Get
                Return Me.m_TotaleIvato
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

        Public ReadOnly Property IndirizzoCliente_Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoCliente_Nome
            End Get
        End Property

        Public ReadOnly Property IndirizzoCliente_ToponimoViaECivico As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoCliente_ToponimoViaECivico
            End Get
        End Property

        Public ReadOnly Property IndirizzoCliente_CAP As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoCliente_CAP
            End Get
        End Property

        Public ReadOnly Property IndirizzoCliente_Citta As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoCliente_Citta
            End Get
        End Property

        Public ReadOnly Property IndirizzoCliente_Provincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoCliente_Provincia
            End Get
        End Property

        Public ReadOnly Property CodiceFiscaleCliente As CCursorFieldObj(Of String)
            Get
                Return Me.m_CodiceFiscaleCliente
            End Get
        End Property

        Public ReadOnly Property PartitaIVACliente As CCursorFieldObj(Of String)
            Get
                Return Me.m_PartitaIVACliente
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

        Public ReadOnly Property IndirizzoFornitore_Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoFornitore_Nome
            End Get
        End Property

        Public ReadOnly Property IndirizzoFornitore_ToponimoViaECivico As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoFornitore_ToponimoViaECivico
            End Get
        End Property

        Public ReadOnly Property IndirizzoFornitore_CAP As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoFornitore_CAP
            End Get
        End Property

        Public ReadOnly Property IndirizzoFornitore_Citta As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoFornitore_Citta
            End Get
        End Property

        Public ReadOnly Property IndirizzoFornitore_Provincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoFornitore_Provincia
            End Get
        End Property

        Public ReadOnly Property CodiceFiscaleFornitore As CCursorFieldObj(Of String)
            Get
                Return Me.m_CodiceFiscaleFornitore
            End Get
        End Property

        Public ReadOnly Property PartitaIVAFornitore As CCursorFieldObj(Of String)
            Get
                Return Me.m_PartitaIVAFornitore
            End Get
        End Property

        Public ReadOnly Property IndirizzoSpedizione_Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoSpedizione_Nome
            End Get
        End Property

        Public ReadOnly Property IndirizzoSpedizione_ToponimoViaECivico As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoSpedizione_ToponimoViaECivico
            End Get
        End Property

        Public ReadOnly Property IndirizzoSpedizione_CAP As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoSpedizione_CAP
            End Get
        End Property

        Public ReadOnly Property IndirizzoSpedizione_Citta As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoSpedizione_Citta
            End Get
        End Property

        Public ReadOnly Property IndirizzoSpedizione_Provincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoSpedizione_Provincia
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function


        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeDocumentiContabili"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Office.DocumentiContabili.Module
        End Function
    End Class



End Class