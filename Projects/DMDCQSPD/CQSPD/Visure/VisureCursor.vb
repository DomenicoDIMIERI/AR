Imports DMD.Databases

Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Public Class CQSPD


    ''' <summary>
    ''' Cursore sulla tabella delle visure
    ''' </summary>
    ''' <remarks></remarks>
    Public Class VisureCursor
        Inherits DBObjectCursorPO(Of Visura)

        Private m_Data As New CCursorField(Of Date)("Data")
        Private m_IDRichiedente As New CCursorField(Of Integer)("IDRichiedente")
        Private m_NomeRichiedente As New CCursorFieldObj(Of String)("NomeRichiedente")
        Private m_IDPresaInCaricoDa As New CCursorField(Of Integer)("IDPresaInCaricoDa")
        Private m_NomePresaInCaricoDa As New CCursorFieldObj(Of String)("NomePresaInCaricoDa")
        Private m_DataPresaInCarico As New CCursorField(Of Date)("DataPresaInCarico")
        Private m_DataCompletamento As New CCursorField(Of Date)("DataCompletamento")
        Private m_StatoVisura As New CCursorField(Of StatoVisura)("StatoVisura")
        Private m_ValutazioneAmministrazione As New CCursorField(Of Boolean)("VALAMM")
        Private m_CensimentoDatoreDiLavoro As New CCursorField(Of Boolean)("CENSDATLAV")
        Private m_CensimentoSedeOperativa As New CCursorField(Of Boolean)("CENSSEDOP")
        Private m_VariazioneDenominazione As New CCursorField(Of Boolean)("VARIAZDENOM")
        Private m_Sblocco As New CCursorField(Of Boolean)("SBLOCCO")
        Private m_IDAmministrazione As New CCursorField(Of Integer)("IDAmministrazione")
        Private m_RagioneSociale As New CCursorFieldObj(Of String)("RagioneSociale")
        Private m_OggettoSociale As New CCursorFieldObj(Of String)("OggettoSociale")
        Private m_CodiceFiscale As New CCursorFieldObj(Of String)("CodiceFiscale")
        Private m_PartitaIVA As New CCursorFieldObj(Of String)("PartitaIVA")
        Private m_ResponsabileDaContattare As New CCursorFieldObj(Of String)("NomeResponsabile")
        Private m_Qualifica As New CCursorFieldObj(Of String)("QualificaResponsabile")
        Private m_IndirizzoProvincia As New CCursorFieldObj(Of String)("Indirizzo_Provincia")
        Private m_IndirizzoCitta As New CCursorFieldObj(Of String)("Indirizzo_Citta")
        Private m_IndirizzoCAP As New CCursorFieldObj(Of String)("Indirizzo_CAP")
        Private m_IndirizzoToponimoViaECivico As New CCursorFieldObj(Of String)("Indirizzo_Via")
        Private m_Telefono As New CCursorFieldObj(Of String)("Telefono")
        Private m_Fax As New CCursorFieldObj(Of String)("Fax")
        Private m_IndirizzoeMail As New CCursorFieldObj(Of String)("eMail")
        Private m_IndirizzoDiNotificaProvincia As New CCursorFieldObj(Of String)("IndirizzoN_Provincia")
        Private m_IndirizzoDiNotificaCitta As New CCursorFieldObj(Of String)("IndirizzoN_Citta")
        Private m_IndirizzoDiNotificaCAP As New CCursorFieldObj(Of String)("IndirizzoN_CAP")
        Private m_IndirizzoDiNotificaToponimoViaECivico As New CCursorFieldObj(Of String)("IndirizzoN_Via")
        Private m_TelefonoDiNotifica As New CCursorFieldObj(Of String)("TelefonoN")
        Private m_FaxDiNotifica As New CCursorFieldObj(Of String)("FaxN")
        Private m_ConvenzionePresente As New CCursorField(Of Boolean)("CONVSINO")
        Private m_CodiceODescrizioneConvenzione As New CCursorFieldObj(Of String)("CODCONV")
        Private m_NumeroDipendenti As New CCursorField(Of Integer)("NumeroDipendenti")
        Private m_AmministrazioneSottoscriveMODPREST_008 As New CCursorField(Of Boolean)("AMMMODPRST008")
        Private m_NoteOInfoSullaSocieta As New CCursorFieldObj(Of String)("NoteSocieta")
        Private m_IDBustaPaga As New CCursorField(Of Integer)("IDBustaPaga")
        Private m_IDMotivoRichiestaSblocco As New CCursorField(Of Integer)("IDMotivoSblocco")
        Private m_CodiceAmministrazioneCL As New CCursorFieldObj(Of String)("CODAMMCL")
        Private m_StatoAmministrazioneCL As New CCursorFieldObj(Of String)("STATOAMMCL")

        Public Sub New()
        End Sub

        Public ReadOnly Property CodiceAmministrazioneCL As CCursorFieldObj(Of String)
            Get
                Return Me.m_CodiceAmministrazioneCL
            End Get
        End Property

        Public ReadOnly Property StatoAmministrazioneCL As CCursorFieldObj(Of String)
            Get
                Return Me.m_StatoAmministrazioneCL
            End Get
        End Property

        Public ReadOnly Property Data As CCursorField(Of Date)
            Get
                Return Me.m_Data
            End Get
        End Property

        Public ReadOnly Property IDRichiedente As CCursorField(Of Integer)
            Get
                Return Me.m_IDRichiedente
            End Get
        End Property

        Public ReadOnly Property NomeRichiedente As CCursorFieldObj(Of String)
            Get
                Return Me.m_NomeRichiedente
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

        Public ReadOnly Property DataCompletamento As CCursorField(Of Date)
            Get
                Return Me.m_DataCompletamento
            End Get
        End Property

        Public ReadOnly Property StatoVisura As CCursorField(Of StatoVisura)
            Get
                Return Me.m_StatoVisura
            End Get
        End Property

        Public ReadOnly Property ValutazioneAmministrazione As CCursorField(Of Boolean)
            Get
                Return Me.m_ValutazioneAmministrazione
            End Get
        End Property

        Public ReadOnly Property CensimentoDatoreDiLavoro As CCursorField(Of Boolean)
            Get
                Return Me.m_CensimentoDatoreDiLavoro
            End Get
        End Property

        Public ReadOnly Property CensimentoSedeOperativa As CCursorField(Of Boolean)
            Get
                Return Me.m_CensimentoSedeOperativa
            End Get
        End Property

        Public ReadOnly Property VariazioneDenominazione As CCursorField(Of Boolean)
            Get
                Return Me.m_VariazioneDenominazione
            End Get
        End Property

        Public ReadOnly Property Sblocco As CCursorField(Of Boolean)
            Get
                Return Me.m_Sblocco
            End Get
        End Property

        Public ReadOnly Property IDAmministrazione As CCursorField(Of Integer)
            Get
                Return Me.m_IDAmministrazione
            End Get
        End Property

        Public ReadOnly Property RagioneSociale As CCursorFieldObj(Of String)
            Get
                Return Me.m_RagioneSociale
            End Get
        End Property

        Public ReadOnly Property OggettoSociale As CCursorFieldObj(Of String)
            Get
                Return Me.m_OggettoSociale
            End Get
        End Property

        Public ReadOnly Property CodiceFiscale As CCursorFieldObj(Of String)
            Get
                Return Me.m_CodiceFiscale
            End Get
        End Property

        Public ReadOnly Property PartitaIVA As CCursorFieldObj(Of String)
            Get
                Return Me.m_PartitaIVA
            End Get
        End Property

        Public ReadOnly Property ResponsabileDaContattare As CCursorFieldObj(Of String)
            Get
                Return Me.m_ResponsabileDaContattare
            End Get
        End Property

        Public ReadOnly Property Qualifica As CCursorFieldObj(Of String)
            Get
                Return Me.m_Qualifica
            End Get
        End Property

        Public ReadOnly Property IndirizzoProvincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoProvincia
            End Get
        End Property

        Public ReadOnly Property IndirizzoCitta As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoCitta
            End Get
        End Property

        Public ReadOnly Property IndirizzoCAP As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoCAP
            End Get
        End Property

        Public ReadOnly Property IndirizzoToponimoViaECivico As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoToponimoViaECivico
            End Get
        End Property

        Public ReadOnly Property Telefono As CCursorFieldObj(Of String)
            Get
                Return Me.m_Telefono
            End Get
        End Property

        Public ReadOnly Property Fax As CCursorFieldObj(Of String)
            Get
                Return Me.m_Fax
            End Get
        End Property

        Public ReadOnly Property IndirizzoeMail As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoeMail
            End Get
        End Property

        Public ReadOnly Property IndirizzoDiNotificaProvincia As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoDiNotificaProvincia
            End Get
        End Property

        Public ReadOnly Property IndirizzoDiNotificaCitta As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoDiNotificaCitta
            End Get
        End Property

        Public ReadOnly Property IndirizzoDiNotificaCAP As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoDiNotificaCAP
            End Get
        End Property

        Public ReadOnly Property IndirizzoDiNotificaToponimoViaECivico As CCursorFieldObj(Of String)
            Get
                Return Me.m_IndirizzoDiNotificaToponimoViaECivico
            End Get
        End Property

        Public ReadOnly Property TelefonoDiNotifica As CCursorFieldObj(Of String)
            Get
                Return Me.m_TelefonoDiNotifica
            End Get
        End Property

        Public ReadOnly Property FaxDiNotifica As CCursorFieldObj(Of String)
            Get
                Return Me.m_FaxDiNotifica
            End Get
        End Property

        Public ReadOnly Property ConvenzionePresente As CCursorField(Of Boolean)
            Get
                Return Me.m_ConvenzionePresente
            End Get
        End Property

        Public ReadOnly Property CodiceODescrizioneConvenzione As CCursorFieldObj(Of String)
            Get
                Return Me.m_CodiceODescrizioneConvenzione
            End Get
        End Property

        Public ReadOnly Property NumeroDipendenti As CCursorField(Of Integer)
            Get
                Return Me.m_NumeroDipendenti
            End Get
        End Property

        Public ReadOnly Property AmministrazioneSottoscriveMODPREST_008 As CCursorField(Of Boolean)
            Get
                Return Me.m_AmministrazioneSottoscriveMODPREST_008
            End Get
        End Property

        Public ReadOnly Property NoteOInfoSullaSocieta As CCursorFieldObj(Of String)
            Get
                Return Me.m_NoteOInfoSullaSocieta
            End Get
        End Property

        Public ReadOnly Property IDBustaPaga As CCursorField(Of Integer)
            Get
                Return Me.m_IDBustaPaga
            End Get
        End Property

        Public ReadOnly Property IDMotivoRichiestaSblocco As CCursorField(Of Integer)
            Get
                Return Me.m_IDMotivoRichiestaSblocco
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return DMD.CQSPD.Visure.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDVisure"
        End Function

        Public Overrides Function Add() As Object
            Dim ret As Visura = MyBase.Add()
            ret.Data = Now
            ret.Richiedente = Users.CurrentUser
            Return ret
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function
    End Class


End Class