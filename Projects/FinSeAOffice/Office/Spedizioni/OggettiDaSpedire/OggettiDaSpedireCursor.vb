Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Imports DMD.GPS

Partial Class Office
   
  
    ''' <summary>
    ''' Cursore sulla tabella degli oggetti da spedire
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class OggettiDaSpedireCursor
        Inherits DBObjectCursorPO(Of OggettoDaSpedire)


        Private m_AspettoBeni As New CCursorFieldObj(Of String)("AspettoBeni")
        Private m_IDCliente As New CCursorField(Of Integer)("IDCliente")
        Private m_NomeCliente As New CCursorFieldObj(Of String)("NomeCliente")
        Private m_IDDestinatario As New CCursorField(Of Integer)("IDDestinatario")
        Private m_NomeDestinatario As New CCursorFieldObj(Of String)("NomeDestinatario")
        Private m_INDMITT_CAP As New CCursorFieldObj(Of String)("INDMITT_CAP")
        Private m_INDMITT_Citta As New CCursorFieldObj(Of String)("INDMITT_CITTA")
        Private m_INDMITT_Provincia As New CCursorFieldObj(Of String)("INDMITT_PROV")
        Private m_INDMITT_ToponimoViaECivico As New CCursorFieldObj(Of String)("INDMITT_VIA")
        Private m_INDDEST_CAP As New CCursorFieldObj(Of String)("INDDEST_CAP")
        Private m_INDDEST_Citta As New CCursorFieldObj(Of String)("INDDEST_CITTA")
        Private m_INDDEST_Provincia As New CCursorFieldObj(Of String)("INDDEST_PROV")
        Private m_INDDEST_ToponimoViaECivico As New CCursorFieldObj(Of String)("INDDEST_VIA")
        Private m_NumeroColli As New CCursorField(Of Integer)("NumeroColli")
        Private m_Peso As New CCursorField(Of Double)("Peso")
        Private m_Altezza As New CCursorField(Of Double)("Altezza")
        Private m_Larghezza As New CCursorField(Of Double)("Larghezza")
        Private m_Profondita As New CCursorField(Of Double)("Profondita")

        Private m_IDRichiestaDa As New CCursorField(Of Integer)("IDRichiestaDa")
        Private m_NomeRichiestaDa As New CCursorFieldObj(Of String)("NomeRichiestaDa")
        Private m_DataRichiesta As New CCursorField(Of Date)("DataRichiesta")

        Private m_IDPresaInCaricoDa As New CCursorField(Of Integer)("IDPresaInCaricoDa")
        Private m_NomePresaInCaricoDa As New CCursorFieldObj(Of String)("NomePresaInCaricoDa")
        Private m_DataPresaInCarico As New CCursorField(Of Date)("DataPresaInCarico")

        Private m_IDConfermatoDa As New CCursorField(Of Integer)("IDConfermatoDa")
        Private m_NomeConfermatoDa As New CCursorFieldObj(Of String)("NomeConfermatoDa")
        Private m_DataConferma As New CCursorField(Of Date)("DataConferma")

        Private m_DescrizioneSpedizione As New CCursorFieldObj(Of String)("DescrizioneSpedizione")

        Private m_NotePerIlCorriere As New CCursorFieldObj(Of String)("NotePerIlCorriere")
        Private m_NotePerIlDestinatario As New CCursorFieldObj(Of String)("NotePerIlDestinatario")

        Private m_StatoOggetto As New CCursorField(Of StatoOggettoDaSpedire)("StatoOggetto")
        Private m_Flags As New CCursorField(Of OggettoDaSpedireFlags)("Flags")
        Private m_DettaglioStato As New CCursorFieldObj(Of String)("DettaglioStato")

        Private m_DataInizioSpedizione As New CCursorField(Of Date)("DataInizioSpedizione")
        Private m_DataConsegna As New CCursorField(Of Date)("DataConsegna")

        Private m_CategoriaContenuto As New CCursorFieldObj(Of String)("CategoriaContenuto")
        Private m_DescrizioneContenuto As New CCursorFieldObj(Of String)("DescrizioneContenuto")

        Public Sub New()

        End Sub

        Public ReadOnly Property DescrizioneContenuto As CCursorFieldObj(Of String)
            Get
                Return Me.m_DescrizioneContenuto
            End Get
        End Property

        Public ReadOnly Property CategoriaContenuto As CCursorFieldObj(Of String)
            Get
                Return Me.m_CategoriaContenuto
            End Get
        End Property

        Public ReadOnly Property AspettoBeni As CCursorFieldObj(Of String)
            Get
                Return Me.m_AspettoBeni
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

        Public ReadOnly Property IDDestinatario As CCursorField(Of Integer)
            Get
                Return Me.m_IDDestinatario
            End Get
        End Property

        Public ReadOnly Property NomeDestinatario As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeDestinatario
            End Get
        End Property

        Public ReadOnly Property INDMITT_CAP As CCursorFieldObj(Of String)
            Get
                Return Me.m_INDMITT_CAP
            End Get
        End Property

        Public ReadOnly Property INDMITT_Citta As CCursorFieldObj(Of String)
            Get
                Return Me.m_INDMITT_Citta
            End Get
        End Property

        Public ReadOnly Property INDMITT_Provincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_INDMITT_Provincia
            End Get
        End Property

        Public ReadOnly Property INDMITT_ToponimoViaECivico As CCursorFieldObj(Of String)
            Get
                Return Me.m_INDMITT_ToponimoViaECivico
            End Get
        End Property

        Public ReadOnly Property INDDEST_CAP As CCursorFieldObj(Of String)
            Get
                Return Me.m_INDDEST_CAP
            End Get
        End Property

        Public ReadOnly Property INDDEST_Citta As CCursorFieldObj(Of String)
            Get
                Return Me.m_INDDEST_Citta
            End Get
        End Property

        Public ReadOnly Property INDDEST_Provincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_INDDEST_Provincia
            End Get
        End Property

        Public ReadOnly Property INDDEST_ToponimoViaECivico As CCursorFieldObj(Of String)
            Get
                Return Me.m_INDDEST_ToponimoViaECivico
            End Get
        End Property

        Public ReadOnly Property NumeroColli As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroColli
            End Get
        End Property

        Public ReadOnly Property Peso As CCursorField(Of Double)
            Get
                Return Me.m_Peso
            End Get
        End Property

        Public ReadOnly Property Altezza As CCursorField(Of Double)
            Get
                Return Me.m_Altezza
            End Get
        End Property

        Public ReadOnly Property Larghezza As CCursorField(Of Double)
            Get
                Return Me.m_Larghezza
            End Get
        End Property

        Public ReadOnly Property Profondita As CCursorField(Of Double)
            Get
                Return Me.m_Profondita
            End Get
        End Property

        Public ReadOnly Property IDRichiestaDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDRichiestaDa
            End Get
        End Property

        Public ReadOnly Property NomeRichiestaDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeRichiestaDa
            End Get
        End Property

        Public ReadOnly Property DataRichiesta As CCursorField(Of Date)
            Get
                Return Me.m_DataRichiesta
            End Get
        End Property

        Public ReadOnly Property IDPresaInCaricoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDPresaInCaricoDa
            End Get
        End Property

        Public ReadOnly Property NomePresaInCaricoDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomePresaInCaricoDa
            End Get
        End Property

        Public ReadOnly Property DataPresaInCarico As CCursorField(Of Date)
            Get
                Return Me.m_DataPresaInCarico
            End Get
        End Property

        Public ReadOnly Property IDConfermatoDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDConfermatoDa
            End Get
        End Property

        Public ReadOnly Property NomeConfermatoDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeConfermatoDa
            End Get
        End Property

        Public ReadOnly Property DataConferma As CCursorField(Of Date)
            Get
                Return Me.m_DataConferma
            End Get
        End Property

        Public ReadOnly Property DescrizioneSpedizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_DescrizioneSpedizione
            End Get
        End Property

        Public ReadOnly Property NotePerIlCorriere As CCursorFieldObj(Of String)
            Get
                Return Me.m_NotePerIlCorriere
            End Get
        End Property

        Public ReadOnly Property NotePerIlDestinatario As CCursorFieldObj(Of String)
            Get
                Return Me.m_NotePerIlDestinatario
            End Get
        End Property

        Public ReadOnly Property StatoOggetto As CCursorField(Of StatoOggettoDaSpedire)
            Get
                Return Me.m_StatoOggetto
            End Get
        End Property

        Public ReadOnly Property Flags As CCursorField(Of OggettoDaSpedireFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property DettaglioStato As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioStato
            End Get
        End Property

        Public ReadOnly Property DataInizioSpedizione As CCursorField(Of Date)
            Get
                Return Me.m_DataInizioSpedizione
            End Get
        End Property

        Public ReadOnly Property DataConsegna As CCursorField(Of Date)
            Get
                Return Me.m_DataConsegna
            End Get
        End Property


        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As Sistema.CModule
            Return Office.OggettiDaSpedire.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeOggettiDaSpedire"
        End Function





    End Class



End Class