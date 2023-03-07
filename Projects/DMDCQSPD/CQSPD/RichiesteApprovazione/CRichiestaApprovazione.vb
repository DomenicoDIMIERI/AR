Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica


Partial Public Class CQSPD

    Public Enum StatoRichiestaApprovazione As Integer
        ''' <summary>
        ''' La richiesta non è stata ancora inoltrata
        ''' </summary>
        ''' <remarks></remarks>
        NONCHIESTA = -1

        ''' <summary>
        ''' Nessun supervisore ha ancora preso in carico la richiesta
        ''' </summary>
        ''' <remarks></remarks>
        ATTESA = 0

        ''' <summary>
        ''' Un supervisore ha preso in carico la richiesta
        ''' </summary>
        ''' <remarks></remarks>
        PRESAINCARICO = 1

        ''' <summary>
        ''' Il supervisore ha approvato la richiesta
        ''' </summary>
        ''' <remarks></remarks>
        APPROVATA = 2

        ''' <summary>
        ''' Il supervisore ha negato la richiesta
        ''' </summary>
        ''' <remarks></remarks>
        NEGATA = 3

        ''' <summary>
        ''' La richiesta è stata annullata perché l'offerta è stata modificata
        ''' </summary>
        ''' <remarks></remarks>
        ANNULLATA = 4
    End Enum

    ''' <summary>
    ''' Rappresenta la specifica di un obiettivo per un ufficio
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CRichiestaApprovazione
        Inherits DBObjectPO
        Implements IComparable

        Private m_IDCausa As Integer                    'ID del vincolo non rispettato
        Private m_Causa As CDocumentoPraticaCaricato    'Vincolo non rispettato
        Private m_DescrizioneCausa As String            'Descrizione d

        Private m_IDOggettoApprovabile As Integer
        Private m_TipoOggettoApprovabile As String
        Private m_OggettoApprovabile As Object
        Private m_DataRichiestaApprovazione As Date
        Private m_IDUtenteRichiestaApprovazione As Integer
        Private m_NomeUtenteRichiestaApprovazione As String
        Private m_UtenteRichiestaApprovazione As CUser
        Private m_IDMotivoRichiesta As Integer
        Private m_MotivoRichiesta As CMotivoScontoPratica
        Private m_NomeMotivoRichiesta As String
        Private m_DescrizioneRichiesta As String
        Private m_ParametriRichiesta As String
        Private m_StatoRichiesta As StatoRichiestaApprovazione

        Private m_DataPresaInCarico As Date?
        Private m_IDPresaInCaricoDa As Integer
        Private m_NomePresaInCaricoDa As String
        Private m_PresaInCaricoDa As CUser

        Private m_DataConferma As Date?
        Private m_IDConfermataDa As Integer
        Private m_NomeConfermataDa As String
        Private m_ConfermataDa As CUser

        Private m_Flags As Integer

        Private m_MotivoConferma As String
        Private m_DettaglioConferma As String

        Private m_IDCliente As Integer
        Private m_NomeCliente As String
        Private m_Cliente As CPersona

        Public Sub New()
            Me.m_IDOggettoApprovabile = 0
            Me.m_TipoOggettoApprovabile = ""
            Me.m_OggettoApprovabile = Nothing
            Me.m_DataRichiestaApprovazione = Now
            Me.m_IDUtenteRichiestaApprovazione = 0
            Me.m_NomeUtenteRichiestaApprovazione = ""
            Me.m_UtenteRichiestaApprovazione = Nothing
            Me.m_IDMotivoRichiesta = 0
            Me.m_MotivoRichiesta = Nothing
            Me.m_NomeMotivoRichiesta = ""
            Me.m_DescrizioneRichiesta = ""
            Me.m_ParametriRichiesta = ""
            Me.m_StatoRichiesta = StatoRichiestaApprovazione.NONCHIESTA

            Me.m_DataPresaInCarico = Nothing
            Me.m_IDPresaInCaricoDa = 0
            Me.m_NomePresaInCaricoDa = ""
            Me.m_PresaInCaricoDa = Nothing

            Me.m_DataConferma = Nothing
            Me.m_IDConfermataDa = 0
            Me.m_NomeConfermataDa = ""
            Me.m_ConfermataDa = Nothing

            Me.m_MotivoConferma = ""
            Me.m_DettaglioConferma = ""

            Me.m_Flags = 0

            Me.m_IDCliente = 0
            Me.m_NomeCliente = ""
            Me.m_Cliente = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDCliente As Integer
            Get
                Return GetID(Me.m_Cliente, Me.m_IDCliente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCliente
                If (oldValue = value) Then Exit Property
                Me.m_IDCliente = value
                Me.m_Cliente = Nothing
                Me.DoChanged("IDCliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Cliente As CPersona
            Get
                If (Me.m_Cliente Is Nothing) Then Me.m_Cliente = Anagrafica.Persone.GetItemById(Me.m_IDCliente)
                Return Me.m_Cliente
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Cliente
                If (oldValue Is value) Then Exit Property
                Me.m_Cliente = value
                Me.m_IDCliente = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeCliente = value.Nominativo
                Me.DoChanged("Cliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeCliente As String
            Get
                Return Me.m_NomeCliente
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeCliente
                If (oldValue = value) Then Exit Property
                Me.m_NomeCliente = value
                Me.DoChanged("NomeCliente", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta una stringa che specifica il motivo della conferma o della negazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MotivoConferma As String
            Get
                Return Me.m_MotivoConferma
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_MotivoConferma
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_MotivoConferma = value
                Me.DoChanged("MotivoConferma", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che specifica meglio il motivo della conferma o della negazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DettaglioConferma As String
            Get
                Return Me.m_DettaglioConferma
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_DettaglioConferma
                If (oldValue = value) Then Exit Property
                Me.m_DettaglioConferma = value
                Me.DoChanged("DettaglioConferma", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'oggetto approvabile
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDOggettoApprovabile As Integer
            Get
                Return GetID(Me.m_OggettoApprovabile, Me.m_IDOggettoApprovabile)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOggettoApprovabile
                If (oldValue = value) Then Exit Property
                Me.m_IDOggettoApprovabile = value
                Me.m_OggettoApprovabile = Nothing
                Me.DoChanged("IDOggettoApprovabile", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo dell'oggetto approvabile
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoOggettoApprovabile As String
            Get
                If (Me.m_OggettoApprovabile IsNot Nothing) Then Return TypeName(Me.m_OggettoApprovabile)
                Return Me.m_TipoOggettoApprovabile
            End Get
            Set(value As String)
                Dim oldValue As String = Me.TipoOggettoApprovabile
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_TipoOggettoApprovabile = value
                Me.m_OggettoApprovabile = Nothing
                Me.DoChanged("TipoOggettoApprovabile", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'oggetto approvabile
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OggettoApprovabile As Object
            Get
                If (Me.m_OggettoApprovabile Is Nothing) Then Me.m_OggettoApprovabile = Sistema.Types.GetItemByTypeAndId(Me.m_TipoOggettoApprovabile, Me.m_IDOggettoApprovabile)
                Return Me.m_OggettoApprovabile
            End Get
            Set(value As Object)
                Dim oldValue As Object = Me.OggettoApprovabile
                If (oldValue Is value) Then Exit Property
                Me.SetOggettoApprovabile(value)
                Me.DoChanged("OggettoApprovabile", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetOggettoApprovabile(ByVal value As Object)
            Me.m_OggettoApprovabile = value
            Me.m_IDOggettoApprovabile = GetID(value)
            If (value IsNot Nothing) Then
                Me.m_TipoOggettoApprovabile = TypeName(value)
            Else
                Me.m_TipoOggettoApprovabile = ""
            End If
        End Sub

        ''' <summary>
        ''' Restituisce o imposta la data e l'ora della richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataRichiestaApprovazione As Date
            Get
                Return Me.m_DataRichiestaApprovazione
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_DataRichiestaApprovazione
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataRichiestaApprovazione = value
                Me.DoChanged("DataRichiestaApprovazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha effettuato la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDUtenteRichiestaApprovazione As Integer
            Get
                Return GetID(Me.m_UtenteRichiestaApprovazione, Me.m_IDUtenteRichiestaApprovazione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDUtenteRichiestaApprovazione
                If (oldValue = value) Then Exit Property
                Me.m_IDUtenteRichiestaApprovazione = value
                Me.m_UtenteRichiestaApprovazione = Nothing
                Me.DoChanged("IDUtenteRichiestaApprovazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che ha effettuato la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeUtenteRichiestaApprovazione As String
            Get
                Return Me.m_NomeUtenteRichiestaApprovazione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeUtenteRichiestaApprovazione
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeUtenteRichiestaApprovazione = value
                Me.DoChanged("NomeUtenteRichiestaApprovazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha effettuato la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UtenteRichiestaApprovazione As CUser
            Get
                If (Me.m_UtenteRichiestaApprovazione Is Nothing) Then Me.m_UtenteRichiestaApprovazione = Sistema.Users.GetItemById(Me.m_IDUtenteRichiestaApprovazione)
                Return Me.m_UtenteRichiestaApprovazione
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.UtenteRichiestaApprovazione
                If (oldValue Is value) Then Exit Property
                Me.m_IDUtenteRichiestaApprovazione = GetID(value)
                Me.m_UtenteRichiestaApprovazione = value
                If (value IsNot Nothing) Then Me.m_NomeUtenteRichiestaApprovazione = value.Nominativo
                Me.DoChanged("UtenteRichiestaApprovazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del motivo della richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDMotivoRichiesta As Integer
            Get
                Return GetID(Me.m_MotivoRichiesta, Me.m_IDMotivoRichiesta)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDMotivoRichiesta
                If (oldValue = value) Then Exit Property
                Me.m_MotivoRichiesta = Nothing
                Me.m_IDMotivoRichiesta = value
                Me.DoChanged("IDMotivoRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il motivo della richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MotivoRichiesta As CMotivoScontoPratica
            Get
                If (Me.m_MotivoRichiesta Is Nothing) Then Me.m_MotivoRichiesta = CQSPD.MotiviSconto.GetItemById(Me.m_IDMotivoRichiesta)
                Return Me.m_MotivoRichiesta
            End Get
            Set(value As CMotivoScontoPratica)
                Dim oldValue As CMotivoScontoPratica = Me.MotivoRichiesta
                If (oldValue Is value) Then Exit Property
                Me.m_MotivoRichiesta = value
                Me.m_IDMotivoRichiesta = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeMotivoRichiesta = value.Nome
                Me.DoChanged("MotivoRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del motivo della richiesta di approvazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeMotivoRichiesta As String
            Get
                Return Me.m_NomeMotivoRichiesta
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeMotivoRichiesta
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeMotivoRichiesta = value
                Me.DoChanged("NomeMotivoRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che descrive la richiesta 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DescrizioneRichiesta As String
            Get
                Return Me.m_DescrizioneRichiesta
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_DescrizioneRichiesta
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_DescrizioneRichiesta = value
                Me.DoChanged("DescrizioneRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei parametri aggiuntivi per la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ParametriRichiesta As String
            Get
                Return Me.m_ParametriRichiesta
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_ParametriRichiesta
                If (oldValue = value) Then Exit Property
                Me.m_ParametriRichiesta = value
                Me.DoChanged("ParametriRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato della richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoRichiesta As StatoRichiestaApprovazione
            Get
                Return Me.m_StatoRichiesta
            End Get
            Set(value As StatoRichiestaApprovazione)
                Dim oldValue As StatoRichiestaApprovazione = Me.m_StatoRichiesta
                If (oldValue = value) Then Exit Property
                Me.m_StatoRichiesta = value
                Me.DoChanged("StatoRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data della prima presa in carico della richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataPresaInCarico As Date?
            Get
                Return Me.m_DataPresaInCarico
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataPresaInCarico
                If (oldValue = value) Then Exit Property
                Me.m_DataPresaInCarico = value
                Me.DoChanged("DataPresaInCarico", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del primo utente che ha preso in carico la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPresaInCaricoDa As Integer
            Get
                Return GetID(Me.m_PresaInCaricoDa, Me.m_IDPresaInCaricoDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPresaInCaricoDa
                If (oldValue = value) Then Exit Property
                Me.m_IDPresaInCaricoDa = value
                Me.m_PresaInCaricoDa = Nothing
                Me.DoChanged("IDPresaInCaricoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del primo utente che ha preso in carico la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomePresaInCaricoDa As String
            Get
                Return Me.m_NomePresaInCaricoDa
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomePresaInCaricoDa
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomePresaInCaricoDa = value
                Me.DoChanged("NomePresaInCaricoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il primo utente che ha preso in carico la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PresaInCaricoDa As CUser
            Get
                If (Me.m_PresaInCaricoDa Is Nothing) Then Me.m_PresaInCaricoDa = Sistema.Users.GetItemById(Me.m_IDPresaInCaricoDa)
                Return Me.m_PresaInCaricoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.PresaInCaricoDa
                If (oldValue Is value) Then Exit Property
                Me.m_PresaInCaricoDa = value
                Me.m_IDPresaInCaricoDa = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomePresaInCaricoDa = value.Nominativo
                Me.DoChanged("PresaInCaricoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di approvazione o negazione della richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataConferma As Date?
            Get
                Return Me.m_DataConferma
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataConferma
                If (oldValue = value) Then Exit Property
                Me.m_DataConferma = value
                Me.DoChanged("DataConferma", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha approvato o negato la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDConfermataDa As Integer
            Get
                Return GetID(Me.m_ConfermataDa, Me.m_IDConfermataDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDConfermataDa
                If (oldValue = value) Then Exit Property
                Me.m_IDConfermataDa = value
                Me.m_ConfermataDa = Nothing
                Me.DoChanged("IDConfermataDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che ha approvato o negato la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeConfermataDa As String
            Get
                Return Me.m_NomeConfermataDa
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeConfermataDa
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeConfermataDa = value
                Me.DoChanged("NomeConfermataDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha approvato o negato la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConfermataDa As CUser
            Get
                If (Me.m_ConfermataDa Is Nothing) Then Me.m_ConfermataDa = Sistema.Users.GetItemById(Me.m_IDConfermataDa)
                Return Me.m_ConfermataDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.ConfermataDa
                If (oldValue Is value) Then Exit Property
                Me.m_ConfermataDa = value
                Me.m_IDConfermataDa = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeConfermataDa = value.Nominativo
                Me.DoChanged("ConfermataDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta i flags
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property



        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return CQSPD.RichiesteApprovazione.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDRichiesteApprovazione"
        End Function



        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDOggettoApprovabile = reader.Read("IDOggettoApprovabile", Me.m_IDOggettoApprovabile)
            Me.m_TipoOggettoApprovabile = reader.Read("TipoOggettoApprovabile", Me.m_TipoOggettoApprovabile)
            Me.m_DataRichiestaApprovazione = reader.Read("DataRichiestaApprovazione", Me.m_DataRichiestaApprovazione)
            Me.m_IDUtenteRichiestaApprovazione = reader.Read("IDUtenteRichiestaApprovazione", Me.m_IDUtenteRichiestaApprovazione)
            Me.m_NomeUtenteRichiestaApprovazione = reader.Read("NomeUtenteRichiestaApprovazione", Me.m_NomeUtenteRichiestaApprovazione)
            Me.m_IDMotivoRichiesta = reader.Read("IDMotivoRichiesta", Me.m_IDMotivoRichiesta)
            Me.m_NomeMotivoRichiesta = reader.Read("NomeMotivoRichiesta", Me.m_NomeMotivoRichiesta)
            Me.m_DescrizioneRichiesta = reader.Read("DescrizioneRichiesta", Me.m_DescrizioneRichiesta)
            Me.m_ParametriRichiesta = reader.Read("ParametriRichiesta", Me.m_ParametriRichiesta)
            Me.m_StatoRichiesta = reader.Read("StatoRichiesta", Me.m_StatoRichiesta)

            Me.m_DataPresaInCarico = reader.Read("DataPresaInCarico", Me.m_DataPresaInCarico)
            Me.m_IDPresaInCaricoDa = reader.Read("IDPresaInCaricoDa", Me.m_IDPresaInCaricoDa)
            Me.m_NomePresaInCaricoDa = reader.Read("NomePresaInCaricoDa", Me.m_NomePresaInCaricoDa)
            
            Me.m_DataConferma = reader.Read("DataConferma", Me.m_DataConferma)
            Me.m_IDConfermataDa = reader.Read("IDConfermataDa", Me.m_IDConfermataDa)
            Me.m_NomeConfermataDa = reader.Read("NomeConfermataDa", Me.m_NomeConfermataDa)
            
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)

            Me.m_MotivoConferma = reader.Read("MotivoConferma", Me.m_MotivoConferma)
            Me.m_DettaglioConferma = reader.Read("DettaglioConferma", Me.m_DettaglioConferma)

            Me.m_IDCliente = reader.Read("IDCliente", Me.m_IDCliente)
            Me.m_NomeCliente = reader.Read("NomePersona", Me.m_NomeCliente)

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDOggettoApprovabile", Me.IDOggettoApprovabile)
            writer.Write("TipoOggettoApprovabile", Me.TipoOggettoApprovabile)
            writer.Write("DataRichiestaApprovazione", Me.m_DataRichiestaApprovazione)
            writer.Write("IDUtenteRichiestaApprovazione", Me.IDUtenteRichiestaApprovazione)
            writer.Write("NomeUtenteRichiestaApprovazione", Me.m_NomeUtenteRichiestaApprovazione)
            writer.Write("IDMotivoRichiesta", Me.IDMotivoRichiesta)
            writer.Write("NomeMotivoRichiesta", Me.m_NomeMotivoRichiesta)
            writer.Write("DescrizioneRichiesta", Me.m_DescrizioneRichiesta)
            writer.Write("ParametriRichiesta", Me.m_ParametriRichiesta)
            writer.Write("StatoRichiesta", Me.m_StatoRichiesta)

            writer.Write("DataPresaInCarico", Me.m_DataPresaInCarico)
            writer.Write("IDPresaInCaricoDa", Me.IDPresaInCaricoDa)
            writer.Write("NomePresaInCaricoDa", Me.m_NomePresaInCaricoDa)

            writer.Write("DataConferma", Me.m_DataConferma)
            writer.Write("IDConfermataDa", Me.IDConfermataDa)
            writer.Write("NomeConfermataDa", Me.m_NomeConfermataDa)

            writer.Write("Flags", Me.m_Flags)

            writer.Write("MotivoConferma", Me.m_MotivoConferma)
            writer.Write("DettaglioConferma", Me.m_DettaglioConferma)

            writer.Write("IDCliente", Me.IDCliente)
            writer.Write("NomePersona", Me.m_NomeCliente)


            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDOggettoApprovabile", Me.IDOggettoApprovabile)
            writer.WriteAttribute("TipoOggettoApprovabile", Me.TipoOggettoApprovabile)
            writer.WriteAttribute("DataRichiestaApprovazione", Me.m_DataRichiestaApprovazione)
            writer.WriteAttribute("IDUtenteRichiestaApprovazione", Me.IDUtenteRichiestaApprovazione)
            writer.WriteAttribute("NomeUtenteRichiestaApprovazione", Me.m_NomeUtenteRichiestaApprovazione)
            writer.WriteAttribute("IDMotivoRichiesta", Me.IDMotivoRichiesta)
            writer.WriteAttribute("NomeMotivoRichiesta", Me.m_NomeMotivoRichiesta)
            writer.WriteAttribute("StatoRichiesta", Me.m_StatoRichiesta)

            writer.WriteAttribute("DataPresaInCarico", Me.m_DataPresaInCarico)
            writer.WriteAttribute("IDPresaInCaricoDa", Me.IDPresaInCaricoDa)
            writer.WriteAttribute("NomePresaInCaricoDa", Me.m_NomePresaInCaricoDa)

            writer.WriteAttribute("DataConferma", Me.m_DataConferma)
            writer.WriteAttribute("IDConfermataDa", Me.IDConfermataDa)
            writer.WriteAttribute("NomeConfermataDa", Me.m_NomeConfermataDa)
            writer.WriteAttribute("MotivoConferma", Me.m_MotivoConferma)
            


            writer.WriteAttribute("Flags", Me.m_Flags)

            writer.WriteAttribute("IDCliente", Me.IDCliente)
            writer.WriteAttribute("NomeCliente", Me.m_NomeCliente)

            MyBase.XMLSerialize(writer)

            writer.WriteTag("DescrizioneRichiesta", Me.m_DescrizioneRichiesta)
            writer.WriteTag("ParametriRichiesta", Me.m_ParametriRichiesta)
            writer.WriteTag("DettaglioConferma", Me.m_DettaglioConferma)

        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDOggettoApprovabile" : Me.m_IDOggettoApprovabile = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoOggettoApprovabile" : Me.m_TipoOggettoApprovabile = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataRichiestaApprovazione" : Me.m_DataRichiestaApprovazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDUtenteRichiestaApprovazione" : Me.m_IDUtenteRichiestaApprovazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeUtenteRichiestaApprovazione" : Me.m_NomeUtenteRichiestaApprovazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDMotivoRichiesta" : Me.m_IDMotivoRichiesta = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeMotivoRichiesta" : Me.m_NomeMotivoRichiesta = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoRichiesta" : Me.m_StatoRichiesta = XML.Utils.Serializer.DeserializeInteger(fieldValue)

                Case "DataPresaInCarico" : Me.m_DataPresaInCarico = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDPresaInCaricoDa" : Me.m_IDPresaInCaricoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePresaInCaricoDa" : Me.m_NomePresaInCaricoDa = XML.Utils.Serializer.DeserializeString(fieldValue)

                Case "DataConferma" : Me.m_DataConferma = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDConfermataDa" : Me.m_IDConfermataDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeConfermataDa" : Me.m_NomeConfermataDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)

                Case "DescrizioneRichiesta" : Me.m_DescrizioneRichiesta = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ParametriRichiesta" : Me.m_ParametriRichiesta = XML.Utils.Serializer.DeserializeString(fieldValue)

                Case "MotivoConferma" : Me.m_MotivoConferma = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DettaglioConferma" : Me.m_DettaglioConferma = XML.Utils.Serializer.DeserializeString(fieldValue)

                Case "IDCliente" : Me.m_IDCliente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCliente" : Me.m_NomeCliente = XML.Utils.Serializer.DeserializeString(fieldValue)

                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Function CompareTo(ByVal obj As CRichiestaApprovazione) As Integer
            Return DateUtils.Compare(Me.m_DataRichiestaApprovazione, obj.m_DataRichiestaApprovazione)
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Protected Overrides Sub OnCreate(e As SystemEvent)
            MyBase.OnCreate(e)
            CQSPD.RichiesteApprovazione.doItemCreated(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            MyBase.OnDelete(e)
            CQSPD.RichiesteApprovazione.doItemDeleted(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnModified(e As SystemEvent)
            MyBase.OnModified(e)
            CQSPD.RichiesteApprovazione.doItemModified(New ItemEventArgs(Me))
        End Sub

    End Class




End Class
