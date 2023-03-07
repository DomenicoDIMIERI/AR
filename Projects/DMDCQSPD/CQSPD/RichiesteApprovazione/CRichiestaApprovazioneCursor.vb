Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica


Partial Public Class CQSPD

     

    ''' <summary>
    ''' Cursore sulla tabella degli obiettivi
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CRichiestaApprovazioneCursor
        Inherits DBObjectCursorPO(Of CRichiestaApprovazione)

        Private m_Flags As New CCursorField(Of Integer)("Flags")
        Private m_IDOggettoApprovabile As New CCursorField(Of Integer)("IDOggettoApprovabile")
        Private m_TipoOggettoApprovabile As New CCursorFieldObj(Of String)("TipoOggettoApprovabile")
        Private m_DataRichiestaApprovazione As New CCursorField(Of Date)("DataRichiestaApprovazione")
        Private m_IDUtenteRichiestaApprovazione As New CCursorField(Of Integer)("IDUtenteRichiestaApprovazione")
        Private m_NomeUtenteRichiestaApprovazione As New CCursorFieldObj(Of String)("NomeUtenteRichiestaApprovazione")
        Private m_IDMotivoRichiesta As New CCursorField(Of Integer)("IDMotivoRichiesta")
        Private m_NomeMotivoRichiesta As New CCursorFieldObj(Of String)("NomeMotivoRichiesta")
        Private m_DescrizioneRichiesta As New CCursorFieldObj(Of String)("DescrizioneRichiesta")
        Private m_ParametriRichiesta As New CCursorFieldObj(Of String)("ParametriRichiesta")
        Private m_StatoRichiesta As New CCursorField(Of StatoRichiestaApprovazione)("StatoRichiesta")
        Private m_DataPresaInCarico As New CCursorField(Of Date)("DataPresaInCarico")
        Private m_IDPresaInCaricoDa As New CCursorField(Of Integer)("IDPresaInCaricoDa")
        Private m_NomePresaInCaricoDa As New CCursorFieldObj(Of String)("NomePresaInCaricoDa")

        Private m_DataConferma As New CCursorField(Of Date)("DataConferma")
        Private m_IDConfermataDa As New CCursorField(Of Integer)("IDConfermataDa")
        Private m_NomeConfermataDa As New CCursorFieldObj(Of String)("NomeConfermataDa")

        Private m_MotivoConferma As New CCursorFieldObj(Of String)("MotivoConferma")
        Private m_DettaglioConferma As New CCursorFieldObj(Of String)("DettaglioConferma")

        Private m_IDCliente As New CCursorField(Of Integer)("IDCliente")
        Private m_NomeCliente As New CCursorFieldObj(Of String)("NomePersona")

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


        Public ReadOnly Property IDOggettoApprovabile As CCursorField(Of Integer)
            Get
                Return Me.m_IDOggettoApprovabile
            End Get
        End Property

        Public ReadOnly Property TipoOggettoApprovabile As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoOggettoApprovabile
            End Get
        End Property

        Public ReadOnly Property DataRichiestaApprovazione As CCursorField(Of Date)
            Get
                Return Me.m_DataRichiestaApprovazione
            End Get
        End Property

        Public ReadOnly Property IDUtenteRichiestaApprovazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDUtenteRichiestaApprovazione
            End Get
        End Property

        Public ReadOnly Property NomeUtenteRichiestaApprovazione As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeUtenteRichiestaApprovazione
            End Get
        End Property

        Public ReadOnly Property IDMotivoRichiesta As CCursorField(Of Integer)
            Get
                Return Me.m_IDMotivoRichiesta
            End Get
        End Property

        Public ReadOnly Property NomeMotivoRichiesta As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeMotivoRichiesta
            End Get
        End Property

        Public ReadOnly Property DescrizioneRichiesta As CCursorFieldObj(Of String)
            Get
                Return Me.m_DescrizioneRichiesta
            End Get
        End Property

        Public ReadOnly Property ParametriRichiesta As CCursorFieldObj(Of String)
            Get
                Return Me.m_ParametriRichiesta
            End Get
        End Property

        Public ReadOnly Property StatoRichiesta As CCursorField(Of StatoRichiestaApprovazione)
            Get
                Return Me.m_StatoRichiesta
            End Get
        End Property

        Public ReadOnly Property DataPresaInCarico As CCursorField(Of Date)
            Get
                Return Me.m_DataPresaInCarico
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

        Public ReadOnly Property DataConferma As CCursorField(Of Date)
            Get
                Return Me.m_DataConferma
            End Get
        End Property

        Public ReadOnly Property IDConfermataDa As CCursorField(Of Integer)
            Get
                Return Me.m_IDConfermataDa
            End Get
        End Property

        Public ReadOnly Property NomeConfermataDa As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeConfermataDa
            End Get
        End Property


        Public ReadOnly Property Flags As CCursorField(Of Integer)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property MotivoConferma As CCursorFieldObj(Of String)
            Get
                Return Me.m_MotivoConferma
            End Get
        End Property

        Public ReadOnly Property DettaglioConferma As CCursorFieldObj(Of String)
            Get
                Return Me.m_DettaglioConferma
            End Get
        End Property


        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return CQSPD.RichiesteApprovazione.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDRichiesteApprovazione"
        End Function
    End Class




End Class
