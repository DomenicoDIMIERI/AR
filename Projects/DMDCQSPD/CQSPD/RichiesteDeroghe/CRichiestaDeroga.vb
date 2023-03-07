Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports System.Deployment



Partial Public Class CQSPD

    Public Enum StatoRichiestaDeroga As Integer
        ''' <summary>
        ''' La richiesta è in fase di preparazione
        ''' </summary>
        Nessuno = 0

        ''' <summary>
        ''' La richiesta è pronta per essere inviata
        ''' </summary>
        DaInviare = 1

        ''' <summary>
        ''' La richiesta è stata inviata
        ''' </summary>
        Inviata = 2

        ''' <summary>
        ''' La richiesta è stata ricevuta dal destinatario
        ''' </summary>
        Ricevuta = 3

        ''' <summary>
        ''' La richiesta è stata accettata
        ''' </summary>
        Accettata = 4

        ''' <summary>
        ''' La richiesta è stata rifiutata 
        ''' </summary>
        Rifiutata = 5
    End Enum

    ''' <summary>
    ''' Rappresenta una richiesta di deroga fatta al cessionario per ottenere delle condizioni migliorative
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CRichiestaDeroga
        Inherits DBObjectPO

        Private m_IDCliente As Integer                  'ID del cliente
        Private m_Cliente As CPersonaFisica             'Cliente
        Private m_NomeCliente As String                 'Nome del cliente

        Private m_StatoRichiesta As StatoRichiestaDeroga 'Stato della richiesta
        Private m_DataRichiesta As Date?                'Data in cui è stata effettuata la richiesta
        Private m_IDRichiedente As Integer              'ID dell'operatore che ha richiesto la deroga
        Private m_Richiedente As CUser                  'Operatore che ha richiesto la deroga
        Private m_NomeRichiedente As String             'Nome dell'operatore che ha richiesto la deroga
        Private m_MotivoRichiesta As String             'Messaggio da inviare per spiegare la richiesta

        Private m_IDAgenziaConcorrente As Integer       'ID dell'agenzia concorrente
        Private m_AgenziaConcorrente As CPersona        'Concorrente
        Private m_NomeAgenziaConcorrente As String      'Nome dell'agenzia concorrente
        Private m_NomeProdottoConcorrente As String                'Nome del prodotto offerto dalla concorrenza
        Private m_NumeroPreventivoConcorrente As String            'Numero del preventivo offerto dalla concorrenza
        Private m_RataConcorrente As Decimal?                      'Rata dell'offerta concorrente
        Private m_DurataConcorrente As Integer?                     'Durata dell'offerta concorrente
        Private m_NettoRicavoConcorrente As Decimal?                'Netto ricavo concorrente
        Private m_TANConcorrente As Double?                         'TAN dell'offerta concorrente
        Private m_TAEGConcorrente As Double?                         'TAN dell'offerta concorrente

        Private m_IDOffertaIniziale As Integer          'ID dell'offerta che si è richiesto di valutare
        Private m_OffertaIniziale As COffertaCQS        'Offerta che si è richiesto di valutare

        Private m_InviatoA As String                    'Indirizzo di invio (a cui è stata fatta la richiesta)
        Private m_InviatoACC As String                  'Indirizzi in copia carbone
        Private m_MezzoDiInvio As String                'Tipo del mezzo di invio (e-mail)
        Private m_SendSubject As String                 'Oggetto della mail inviata
        Private m_SendMessange As String                'Corpo della mail inviata
        Private m_SendDate As Date?                     'Data di invio della mail
        Private m_Attachments As CCollection(Of CAttachment)   'Documenti allegati alla richiesta (inviati come allegato della e-mail)


        Private m_RicevutoIl As Date?                    'Data di ricezione del messaggio   
        'Private m_RicevutoDa As CUser
        'Private m_RicevutoDaID As Integer
        'Private m_RicevutoDaNome As String


        Private m_RispostoIl As Date?                   'Data di ricezione del messaggio di risposta
        Private m_RispostoDa As String                  'Indirizzo di provenienza della risposta
        Private m_RispostoAMezzo As String              'Mezzo di provenienza della risposta
        Private m_RispostoSubject As String             'Oggetto della mail di risposta
        Private m_RispostoMessage As String             'Corpo della mail di risposta

        Private m_IDOffertaCorrente As Integer          'Nell'iter di lavorazione l'offerta può essere modificata    
        Private m_OffertaCorrente As COffertaCQS

        Private m_Flags As Integer
        Private m_Parameters As CKeyCollection

        Private m_IDFinestraLavorazione As Integer
        Private m_FinestraLavorazione As FinestraLavorazione



        Public Sub New()
            Me.m_IDCliente = 0
            Me.m_Cliente = Nothing
            Me.m_NomeCliente = ""

            Me.m_StatoRichiesta = StatoRichiestaDeroga.Nessuno
            Me.m_DataRichiesta = Nothing
            Me.m_IDRichiedente = 0
            Me.m_Richiedente = Nothing
            Me.m_NomeRichiedente = ""
            Me.m_MotivoRichiesta = ""

            Me.m_IDAgenziaConcorrente = 0
            Me.m_AgenziaConcorrente = Nothing
            Me.m_NomeAgenziaConcorrente = ""
            Me.m_NomeProdottoConcorrente = ""
            Me.m_NumeroPreventivoConcorrente = ""
            Me.m_RataConcorrente = Nothing
            Me.m_DurataConcorrente = Nothing
            Me.m_NettoRicavoConcorrente = Nothing
            Me.m_TANConcorrente = Nothing
            Me.m_TAEGConcorrente = Nothing

            Me.m_IDOffertaIniziale = 0
            Me.m_OffertaIniziale = Nothing

            Me.m_InviatoA = ""
            Me.m_InviatoACC = ""
            Me.m_MezzoDiInvio = ""
            Me.m_SendSubject = ""
            Me.m_SendMessange = ""
            Me.m_SendDate = Nothing
            Me.m_Attachments = Nothing 'CCollection(Of CAttachment)   'Documenti allegati alla richiesta (inviati come allegato della e-mail)


            Me.m_RicevutoIl = Nothing
            Me.m_RispostoIl = Nothing
            Me.m_RispostoDa = ""
            Me.m_RispostoAMezzo = ""
            Me.m_RispostoSubject = ""
            Me.m_RispostoMessage = ""

            Me.m_IDOffertaCorrente = 0
            Me.m_OffertaCorrente = Nothing

            Me.m_Flags = 0
            Me.m_Parameters = Nothing 'CKeyCollection

            Me.m_IDFinestraLavorazione = 0
            Me.m_FinestraLavorazione = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID del cliente per cui è stata chiesta la deroga
        ''' </summary>
        ''' <returns></returns>
        Public Property IDCliente As Integer
            Get
                Return GetID(Me.m_Cliente, Me.m_IDCliente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCliente
                If (oldValue = value) Then Return
                Me.m_IDCliente = value
                Me.m_Cliente = Nothing
                Me.DoChanged("IDCliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il cliente per cui è stata chiesta la deroga
        ''' </summary>
        ''' <returns></returns>
        Public Property Cliente As CPersonaFisica
            Get
                If (Me.m_Cliente Is Nothing) Then Me.m_Cliente = Anagrafica.Persone.GetItemById(Me.m_IDCliente)
                Return Me.m_Cliente
            End Get
            Set(value As CPersonaFisica)
                Dim oldValue As CPersonaFisica = Me.m_Cliente
                If (oldValue Is value) Then Return
                Me.m_Cliente = value
                Me.m_IDCliente = GetID(value)
                Me.m_NomeCliente = ""
                If (value IsNot Nothing) Then Me.m_NomeCliente = value.Nominativo
                Me.DoChanged("Cliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del cliente per cui è stata chiesta la deroga
        ''' </summary>
        ''' <returns></returns>
        Public Property NomeCliente As String
            Get
                Return Me.m_NomeCliente
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeCliente
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeCliente = value
                Me.DoChanged("NomeCliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato della richiesta
        ''' </summary>
        ''' <returns></returns>
        Public Property StatoRichiesta As StatoRichiestaDeroga
            Get
                Return Me.m_StatoRichiesta
            End Get
            Set(value As StatoRichiestaDeroga)
                Dim oldValue As StatoRichiestaDeroga = Me.m_StatoRichiesta
                If (oldValue = value) Then Return
                Me.m_StatoRichiesta = value
                Me.DoChanged("StatoRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data in cui è stata creata la richiesta
        ''' </summary>
        ''' <returns></returns>
        Public Property DataRichiesta As Date?
            Get
                Return Me.m_DataRichiesta
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataRichiesta
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataRichiesta = value
                Me.DoChanged("DataRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha creato la richiestaz
        ''' </summary>
        ''' <returns></returns>
        Public Property IDRichiedente As Integer
            Get
                Return GetID(Me.m_Richiedente, Me.m_IDRichiedente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRichiedente
                If (oldValue = value) Then Return
                Me.m_IDRichiedente = value
                Me.m_Richiedente = Nothing
                Me.DoChanged("IDRichiedente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha creato la richiesta
        ''' </summary>
        ''' <returns></returns>
        Public Property Richiedente As CUser
            Get
                If (Me.m_Richiedente Is Nothing) Then Me.m_Richiedente = Sistema.Users.GetItemById(Me.m_IDRichiedente)
                Return Me.m_Richiedente
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Richiedente
                If (oldValue Is value) Then Return
                Me.m_Richiedente = value
                Me.m_IDRichiedente = GetID(value)
                Me.m_NomeRichiedente = ""
                If (value IsNot Nothing) Then Me.m_NomeRichiedente = value.Nominativo
                Me.DoChanged("Richiedente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del richiedente
        ''' </summary>
        ''' <returns></returns>
        Public Property NomeRichiedente As String
            Get
                Return Me.m_NomeRichiedente
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeRichiedente
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeRichiedente = value
                Me.DoChanged("NomeRichiedente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il messaggio composto dal richiedente per giustificare la richiesta
        ''' </summary>
        ''' <returns></returns>
        Public Property MotivoRichiesta As String
            Get
                Return Me.m_MotivoRichiesta
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_MotivoRichiesta
                If (oldValue = value) Then Return
                Me.m_MotivoRichiesta = value
                Me.DoChanged("MotivoRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della persona/azienda che ha formulato l'offerta concorrente
        ''' </summary>
        ''' <returns></returns>
        Public Property IDAgenziaConcorrente As Integer
            Get
                Return GetID(Me.m_AgenziaConcorrente, Me.m_IDAgenziaConcorrente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAgenziaConcorrente
                If (oldValue = value) Then Return
                Me.m_IDAgenziaConcorrente = value
                Me.m_AgenziaConcorrente = Nothing
                Me.DoChanged("IDAgenziaConcorrente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la persona/azienda che ha formulato l'offerta concorrente
        ''' </summary>
        ''' <returns></returns>
        Public Property AgenziaConcorrente As CPersona
            Get
                If (Me.m_AgenziaConcorrente Is Nothing) Then Me.m_AgenziaConcorrente = Anagrafica.Persone.GetItemById(Me.m_IDAgenziaConcorrente)
                Return Me.m_AgenziaConcorrente
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_AgenziaConcorrente
                If (oldValue Is value) Then Return
                Me.m_AgenziaConcorrente = value
                Me.m_IDAgenziaConcorrente = GetID(value)
                Me.m_NomeAgenziaConcorrente = ""
                If (value IsNot Nothing) Then Me.m_NomeAgenziaConcorrente = value.Nominativo
                Me.DoChanged("AgenziaConcorrente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della persona/agenzia che ha formulato l'offerta concorrente
        ''' </summary>
        ''' <returns></returns>
        Public Property NomeAgenziaConcorrente As String
            Get
                Return Me.m_NomeAgenziaConcorrente
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeAgenziaConcorrente
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeAgenziaConcorrente = value
                Me.DoChanged("NomeAgenziaConcorrente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del prodotto offerto dalla concorrenza
        ''' </summary>
        ''' <returns></returns>
        Public Property NomeProdottoConcorrente As String
            Get
                Return Me.m_NomeProdottoConcorrente
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeProdottoConcorrente
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NomeProdottoConcorrente = value
                Me.DoChanged("NomeProdottoConcorrente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero del preventivo formulato dalla concorrenza
        ''' </summary>
        ''' <returns></returns>
        Public Property NumeroPreventivoConcorrente As String
            Get
                Return Me.m_NumeroPreventivoConcorrente
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NumeroPreventivoConcorrente
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_NumeroPreventivoConcorrente = value
                Me.DoChanged("NumeroPreventivoConcorrente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la rata del preventivo formulato dalla concorrenza
        ''' </summary>
        ''' <returns></returns>
        Public Property RataConcorrente As Decimal?
            Get
                Return Me.m_RataConcorrente
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_RataConcorrente
                If (oldValue = value) Then Return
                Me.m_RataConcorrente = value
                Me.DoChanged("RataConcorrente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la durata del preventivo formulato dalla concorrenza
        ''' </summary>
        ''' <returns></returns>
        Public Property DurataConcorrente As Integer?
            Get
                Return Me.m_DurataConcorrente
            End Get
            Set(value As Integer?)
                Dim oldValue As Integer? = Me.m_DurataConcorrente
                If (oldValue = value) Then Return
                Me.m_DurataConcorrente = value
                Me.DoChanged("DurataConcorrente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il netto ricavo del preventivo formulato dalla concorrenza
        ''' </summary>
        ''' <returns></returns>
        Public Property NettoRicavoConcorrente As Decimal?
            Get
                Return Me.m_NettoRicavoConcorrente
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_NettoRicavoConcorrente
                If (oldValue = value) Then Return
                Me.m_NettoRicavoConcorrente = value
                Me.DoChanged("NettoRicavoConcorrente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il TAN del preventivo formulato dalla concorrenza
        ''' </summary>
        ''' <returns></returns>
        Public Property TANConcorrente As Double?
            Get
                Return Me.m_TANConcorrente
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_TANConcorrente
                If (oldValue = value) Then Return
                Me.m_TANConcorrente = value
                Me.DoChanged("TANConcorrente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il TAEG del preventivo formulato dalla concorrenza
        ''' </summary>
        ''' <returns></returns>
        Public Property TAEGConcorrente As Double?
            Get
                Return Me.m_TAEGConcorrente
            End Get
            Set(value As Double?)
                Dim oldValue As Double? = Me.m_TAEGConcorrente
                If (oldValue = value) Then Return
                Me.m_TAEGConcorrente = value
                Me.DoChanged("TAEGConcorrente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'offerta iniziale (inviata in approvazione) 
        ''' </summary>
        ''' <returns></returns>
        Public Property IDOffertaIniziale As Integer
            Get
                Return GetID(Me.m_OffertaIniziale, Me.m_IDOffertaIniziale)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOffertaIniziale
                If (oldValue = value) Then Return
                Me.m_IDOffertaIniziale = value
                Me.m_OffertaIniziale = Nothing
                Me.DoChanged("IDOffertaIniziale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'offerta iniziale (inviata in approvazione)
        ''' </summary>
        ''' <returns></returns>
        Public Property OffertaIniziale As COffertaCQS
            Get
                If (Me.m_OffertaIniziale Is Nothing) Then Me.m_OffertaIniziale = CQSPD.Offerte.GetItemById(Me.m_IDOffertaIniziale)
                Return Me.m_OffertaIniziale
            End Get
            Set(value As COffertaCQS)
                Dim oldValue As COffertaCQS = Me.m_OffertaIniziale
                If (oldValue Is value) Then Return
                Me.m_OffertaIniziale = value
                Me.m_IDOffertaIniziale = GetID(value)
                Me.DoChanged("OffertaIniziale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo a cui è stata inviata la richiesta
        ''' </summary>
        ''' <returns></returns>
        Public Property InviatoA As String
            Get
                Return Me.m_InviatoA
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_InviatoA
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_InviatoA = value
                Me.DoChanged("InviatoA", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una
        ''' </summary>
        ''' <returns></returns>
        Public Property InviatoACC As String
            Get
                Return Me.m_InviatoACC
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_InviatoACC
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_InviatoACC = value
                Me.DoChanged("InviatoACC", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restitusice o imposta il mezzo utilizzato per inviare la richiesta (e-mail, ecc)
        ''' </summary>
        ''' <returns></returns>
        Public Property MezzoDiInvio As String
            Get
                Return Me.m_MezzoDiInvio
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_MezzoDiInvio
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_MezzoDiInvio = value
                Me.DoChanged("MezzoDiInvio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'oggetto della mail inviata per la richiesta
        ''' </summary>
        ''' <returns></returns>
        Public Property SendSubject As String
            Get
                Return Me.m_SendSubject
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_SendSubject
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_SendSubject = value
                Me.DoChanged("SendSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il corpo del messaggio inviato per la richiesta
        ''' </summary>
        ''' <returns></returns>
        Public Property SendMessage As String
            Get
                Return Me.m_SendMessange
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_SendMessange
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_SendMessange = value
                Me.DoChanged("SendMessange", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di invio
        ''' </summary>
        ''' <returns></returns>
        Public Property SendDate As Date?
            Get
                Return Me.m_SendDate
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_SendDate
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_SendDate = value
                Me.DoChanged("SendDate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'elenco degli allegati inviati
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Attachments As CCollection(Of CAttachment)
            Get
                If (Me.m_Attachments Is Nothing) Then Me.m_Attachments = New CCollection(Of CAttachment)
                Return Me.m_Attachments
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di ricezione (da parte del cessionario)
        ''' </summary>
        ''' <returns></returns>
        Public Property RicevutoIl As Date?
            Get
                Return Me.m_RicevutoIl
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_RicevutoIl
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_RicevutoIl = value
                Me.DoChanged("RicevutoIl", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di ricezione del messaggio di risposta
        ''' </summary>
        ''' <returns></returns>
        Public Property RispostoIl As Date?
            Get
                Return Me.m_RispostoIl
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_RispostoIl
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_RispostoIl = value
                Me.DoChanged("RispostoIl", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo del mittente della risposta
        ''' </summary>
        ''' <returns></returns>
        Public Property RispostoDa As String
            Get
                Return Me.m_RispostoDa
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_RispostoDa
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_RispostoDa = value
                Me.DoChanged("RispostoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del mezzo della comunicazione di risposta
        ''' </summary>
        ''' <returns></returns>
        Public Property RispostoAMezzo As String
            Get
                Return Me.m_RispostoAMezzo
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_RispostoAMezzo
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_RispostoAMezzo = value
                Me.DoChanged("RispostoAMezzo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'oggetto della mail inviata come risposta
        ''' </summary>
        ''' <returns></returns>
        Public Property RispostoSubject As String
            Get
                Return Me.m_RispostoSubject
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_RispostoSubject
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_RispostoSubject = value
                Me.DoChanged("RispostoSubject", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il messaggio inviato come risposta
        ''' </summary>
        ''' <returns></returns>
        Public Property RispostoMessage As String
            Get
                Return Me.m_RispostoMessage
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_RispostoMessage
                If (oldValue = value) Then Return
                Me.m_RispostoMessage = value
                Me.DoChanged("RispostoMessage", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'offerta accettata dal cessionario
        ''' </summary>
        ''' <returns></returns>
        Public Property IDOffertaCorrente As Integer
            Get
                Return GetID(Me.m_OffertaCorrente, Me.m_IDOffertaCorrente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOffertaCorrente
                If (oldValue = value) Then Return
                Me.m_IDOffertaCorrente = value
                Me.m_OffertaCorrente = Nothing
                Me.DoChanged("IDOffertaCorrente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'offerta accettata dal cessionario
        ''' </summary>
        ''' <returns></returns>
        Public Property OffertaCorrente As COffertaCQS
            Get
                If (Me.m_OffertaCorrente Is Nothing) Then Me.m_OffertaCorrente = CQSPD.Offerte.GetItemById(Me.m_IDOffertaCorrente)
                Return Me.m_OffertaCorrente
            End Get
            Set(value As COffertaCQS)
                Dim oldValue As COffertaCQS = Me.m_OffertaCorrente
                If (oldValue Is value) Then Return
                Me.m_OffertaCorrente = value
                Me.m_IDOffertaCorrente = GetID(value)
                Me.DoChanged("OffertaCorrente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei flags aggiuntivi
        ''' </summary>
        ''' <returns></returns>
        Public Property Flags As Integer
            Get
                Return Me.m_Flags
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei parametri aggiuntivi
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Parameters As CKeyCollection
            Get
                If (Me.m_Parameters Is Nothing) Then Me.m_Parameters = New CKeyCollection
                Return Me.m_Parameters
            End Get
        End Property


        ''' <summary>
        ''' Restituisce o imposta l'ID della finestra di lavorazione in cui è avvenuta la richiesta
        ''' </summary>
        ''' <returns></returns>
        Public Property IDFinestraLavorazione As Integer
            Get
                Return GetID(Me.m_FinestraLavorazione, Me.m_IDFinestraLavorazione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDFinestraLavorazione
                If (oldValue = value) Then Exit Property
                Me.m_IDFinestraLavorazione = value
                Me.m_FinestraLavorazione = Nothing
                Me.DoChanged("IDFinestraLavorazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la finestra di lavorazione
        ''' </summary>
        ''' <returns></returns>
        Public Property FinestraLavorazione As FinestraLavorazione
            Get
                If (Me.m_FinestraLavorazione Is Nothing) Then Me.m_FinestraLavorazione = CQSPD.FinestreDiLavorazione.GetItemById(Me.m_IDFinestraLavorazione)
                Return Me.m_FinestraLavorazione
            End Get
            Set(value As FinestraLavorazione)
                Dim oldValue As FinestraLavorazione = Me.m_FinestraLavorazione
                If (oldValue Is value) Then Exit Property
                Me.m_FinestraLavorazione = value
                Me.m_IDFinestraLavorazione = GetID(value)
                Me.DoChanged("FinestraLavorazione", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetFinestraLavorazione(ByVal value As FinestraLavorazione)
            Me.m_FinestraLavorazione = value
            Me.m_IDFinestraLavorazione = GetID(value)
        End Sub






        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return CQSPD.RichiesteDeroghe.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_RichiesteDeroghe"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDCliente = reader.Read("IDCliente", Me.m_IDCliente)
            Me.m_NomeCliente = reader.Read("NomeCliente", Me.m_NomeCliente)

            Me.m_StatoRichiesta = reader.Read("StatoRichiesta", Me.m_StatoRichiesta)
            Me.m_DataRichiesta = reader.Read("DataRichiesta", Me.m_DataRichiesta)
            Me.m_IDRichiedente = reader.Read("IDRichiedente", Me.m_IDRichiedente)
            Me.m_NomeRichiedente = reader.Read("NomeRichiedente", Me.m_NomeRichiedente)
            Me.m_MotivoRichiesta = reader.Read("MotivoRichiesta", Me.m_MotivoRichiesta)
            Me.m_IDAgenziaConcorrente = reader.Read("IDAgenziaConcorrente", Me.m_IDAgenziaConcorrente)
            Me.m_NomeAgenziaConcorrente = reader.Read("NomeAgenziaConcorrente", Me.m_NomeAgenziaConcorrente)
            Me.m_NomeProdottoConcorrente = reader.Read("NomeProdottoConcorrente", Me.m_NomeProdottoConcorrente)
            Me.m_NumeroPreventivoConcorrente = reader.Read("NumeroPreventivoConcorrente", Me.m_NumeroPreventivoConcorrente)
            Me.m_RataConcorrente = reader.Read("RataConcorrente", Me.m_RataConcorrente)
            Me.m_DurataConcorrente = reader.Read("DurataConcorrente", Me.m_DurataConcorrente)
            Me.m_NettoRicavoConcorrente = reader.Read("NettoRicavoConcorrente", Me.m_NettoRicavoConcorrente)
            Me.m_TANConcorrente = reader.Read("TANConcorrente", Me.m_TANConcorrente)
            Me.m_TAEGConcorrente = reader.Read("TAEGConcorrente", Me.m_TAEGConcorrente)
            Me.m_IDOffertaIniziale = reader.Read("IDOffertaIniziale", Me.m_IDOffertaIniziale)
            Me.m_InviatoA = reader.Read("InviatoA", Me.m_InviatoA)
            Me.m_InviatoACC = reader.Read("InviatoACC", Me.m_InviatoACC)
            Me.m_MezzoDiInvio = reader.Read("MezzoDiInvio", Me.m_MezzoDiInvio)
            Me.m_SendSubject = reader.Read("SendSubject", Me.m_SendSubject)
            Me.m_SendMessange = reader.Read("SendMessange", Me.m_SendMessange)
            Me.m_SendDate = reader.Read("SendDate", Me.m_SendDate)
            Try
                Me.m_Attachments = New CCollection(Of CAttachment)
                Me.m_Attachments.AddRange(XML.Utils.Serializer.Deserialize(reader.Read("Attachments", "")))
            Catch ex As Exception
                Me.m_Attachments = Nothing
            End Try



            Me.m_RicevutoIl = reader.Read("RicevutoIl", Me.m_RicevutoIl)

            Me.m_RispostoIl = reader.Read("RispostoIl", Me.m_RispostoIl)
            Me.m_RispostoDa = reader.Read("RispostoDa", Me.m_RispostoDa)
            Me.m_RispostoAMezzo = reader.Read("RispostoAMezzo", Me.m_RispostoAMezzo)
            Me.m_RispostoSubject = reader.Read("RispostoSubject", Me.m_RispostoSubject)
            Me.m_RispostoMessage = reader.Read("RispostoMessage", Me.m_RispostoMessage)

            Me.m_IDOffertaCorrente = reader.Read("IDOffertaCorrente", Me.m_IDOffertaCorrente)

            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Try
                Me.m_Parameters = XML.Utils.Serializer.Deserialize(reader.Read("Parameters", ""))
            Catch ex As Exception
                Me.m_Parameters = Nothing
            End Try

            Me.m_IDFinestraLavorazione = reader.Read("IDFinestraLavorazione", Me.m_IDFinestraLavorazione)


            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDCliente", Me.IDCliente)
            writer.Write("NomeCliente", Me.m_NomeCliente)

            writer.Write("StatoRichiesta", Me.m_StatoRichiesta)
            writer.Write("DataRichiesta", Me.m_DataRichiesta)
            writer.Write("IDRichiedente", Me.IDRichiedente)
            writer.Write("NomeRichiedente", Me.m_NomeRichiedente)
            writer.Write("MotivoRichiesta", Me.m_MotivoRichiesta)
            writer.Write("IDAgenziaConcorrente", Me.IDAgenziaConcorrente)
            writer.Write("NomeAgenziaConcorrente", Me.m_NomeAgenziaConcorrente)
            writer.Write("NomeProdottoConcorrente", Me.m_NomeProdottoConcorrente)
            writer.Write("NumeroPreventivoConcorrente", Me.m_NumeroPreventivoConcorrente)
            writer.Write("RataConcorrente", Me.m_RataConcorrente)
            writer.Write("DurataConcorrente", Me.m_DurataConcorrente)
            writer.Write("NettoRicavoConcorrente", Me.m_NettoRicavoConcorrente)
            writer.Write("TANConcorrente", Me.m_TANConcorrente)
            writer.Write("TAEGConcorrente", Me.m_TAEGConcorrente)
            writer.Write("IDOffertaIniziale", Me.IDOffertaIniziale)
            writer.Write("InviatoA", Me.m_InviatoA)
            writer.Write("InviatoACC", Me.m_InviatoACC)
            writer.Write("MezzoDiInvio", Me.m_MezzoDiInvio)
            writer.Write("SendSubject", Me.m_SendSubject)
            writer.Write("SendMessange", Me.m_SendMessange)
            writer.Write("SendDate", Me.m_SendDate)
            writer.Write("Attachments", XML.Utils.Serializer.Serialize(Me.Attachments))
            writer.Write("RicevutoIl", Me.m_RicevutoIl)
            writer.Write("RispostoIl", Me.m_RispostoIl)
            writer.Write("RispostoDa", Me.m_RispostoDa)
            writer.Write("RispostoAMezzo", Me.m_RispostoAMezzo)
            writer.Write("RispostoSubject", Me.m_RispostoSubject)
            writer.Write("RispostoMessage", Me.m_RispostoMessage)
            writer.Write("IDOffertaCorrente", Me.IDOffertaCorrente)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Parameters", XML.Utils.Serializer.Serialize(Me.Parameters))
            writer.Write("IDFinestraLavorazione", Me.IDFinestraLavorazione)


            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDCliente", Me.IDCliente)
            writer.WriteAttribute("NomeCliente", Me.m_NomeCliente)

            writer.WriteAttribute("StatoRichiesta", Me.m_StatoRichiesta)
            writer.WriteAttribute("DataRichiesta", Me.m_DataRichiesta)
            writer.WriteAttribute("IDRichiedente", Me.IDRichiedente)
            writer.WriteAttribute("NomeRichiedente", Me.m_NomeRichiedente)
            writer.WriteAttribute("IDAgenziaConcorrente", Me.IDAgenziaConcorrente)
            writer.WriteAttribute("NomeAgenziaConcorrente", Me.m_NomeAgenziaConcorrente)
            writer.WriteAttribute("NomeProdottoConcorrente", Me.m_NomeProdottoConcorrente)
            writer.WriteAttribute("NumeroPreventivoConcorrente", Me.m_NumeroPreventivoConcorrente)
            writer.WriteAttribute("RataConcorrente", Me.m_RataConcorrente)
            writer.WriteAttribute("DurataConcorrente", Me.m_DurataConcorrente)
            writer.WriteAttribute("NettoRicavoConcorrente", Me.m_NettoRicavoConcorrente)
            writer.WriteAttribute("TANConcorrente", Me.m_TANConcorrente)
            writer.WriteAttribute("TAEGConcorrente", Me.m_TAEGConcorrente)
            writer.WriteAttribute("IDOffertaIniziale", Me.IDOffertaIniziale)
            writer.WriteAttribute("InviatoA", Me.m_InviatoA)
            writer.WriteAttribute("InviatoACC", Me.m_InviatoACC)
            writer.WriteAttribute("MezzoDiInvio", Me.m_MezzoDiInvio)
            writer.WriteAttribute("SendSubject", Me.m_SendSubject)
            writer.WriteAttribute("SendDate", Me.m_SendDate)
            writer.WriteAttribute("RicevutoIl", Me.m_RicevutoIl)
            writer.WriteAttribute("RispostoIl", Me.m_RispostoIl)
            writer.WriteAttribute("RispostoDa", Me.m_RispostoDa)
            writer.WriteAttribute("RispostoAMezzo", Me.m_RispostoAMezzo)
            writer.WriteAttribute("RispostoSubject", Me.m_RispostoSubject)
            writer.WriteAttribute("IDOffertaCorrente", Me.IDOffertaCorrente)
            writer.WriteAttribute("Flags", Me.m_Flags)

            writer.WriteAttribute("IDFinestraLavorazione", Me.IDFinestraLavorazione)

            MyBase.XMLSerialize(writer)

            writer.WriteTag("MotivoRichiesta", Me.m_MotivoRichiesta)
            writer.WriteTag("SendMessange", Me.m_SendMessange)
            writer.WriteTag("Attachments", Me.Attachments)
            writer.WriteTag("RispostoMessage", Me.m_RispostoMessage)
            writer.WriteTag("Parameters", Me.Parameters)

        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDCliente" : Me.m_IDCliente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCliente" : Me.m_NomeCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoRichiesta" : Me.m_StatoRichiesta = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataRichiesta" : Me.m_DataRichiesta = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDRichiedente" : Me.m_IDRichiedente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeRichiedente" : Me.m_NomeRichiedente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDAgenziaConcorrente" : Me.m_IDAgenziaConcorrente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAgenziaConcorrente" : Me.m_NomeAgenziaConcorrente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeProdottoConcorrente" : Me.m_NomeProdottoConcorrente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NumeroPreventivoConcorrente" : Me.m_NumeroPreventivoConcorrente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "RataConcorrente" : Me.m_RataConcorrente = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "DurataConcorrente" : Me.m_DurataConcorrente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NettoRicavoConcorrente" : Me.m_NettoRicavoConcorrente = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TANConcorrente" : Me.m_TANConcorrente = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TAEGConcorrente" : Me.m_TAEGConcorrente = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "IDOffertaIniziale" : Me.m_IDOffertaIniziale = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "InviatoA" : Me.m_InviatoA = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "InviatoACC" : Me.m_InviatoACC = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MezzoDiInvio" : Me.m_MezzoDiInvio = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SendSubject" : Me.m_SendSubject = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SendDate" : Me.m_SendDate = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "RicevutoIl" : Me.m_RicevutoIl = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "RispostoIl" : Me.m_RispostoIl = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "RispostoDa" : Me.m_RispostoDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "RispostoAMezzo" : Me.m_RispostoAMezzo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "RispostoSubject" : Me.m_RispostoSubject = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDOffertaCorrente" : Me.m_IDOffertaCorrente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDFinestraLavorazione" : Me.m_IDFinestraLavorazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "MotivoRichiesta" : Me.m_MotivoRichiesta = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SendMessange" : Me.m_SendMessange = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Attachments"
                    Me.m_Attachments = New CCollection(Of CAttachment)
                    Me.m_Attachments.AddRange(fieldValue)
                Case "RispostoMessage" : Me.m_RispostoMessage = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Parameters" : Me.m_Parameters = CType(fieldValue, CKeyCollection)

                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return "Richiesta Deroga, Cliente: " & Me.m_NomeCliente & " del : " & Formats.FormatUserDateTime(Me.m_DataRichiesta)
        End Function

        Protected Overrides Sub OnCreate(e As SystemEvent)
            MyBase.OnCreate(e)
            CQSPD.RichiesteDeroghe.doItemCreated(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            MyBase.OnDelete(e)
            CQSPD.RichiesteDeroghe.doItemDeleted(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnModified(e As SystemEvent)
            MyBase.OnModified(e)
            CQSPD.RichiesteDeroghe.doItemModified(New ItemEventArgs(Me))
        End Sub

        Public Sub Invia()
            Me.Stato = ObjectStatus.OBJECT_VALID
            Me.Save()
            Me.OnInviata(New System.EventArgs)
        End Sub

        Protected Overridable Sub OnInviata(ByVal e As System.EventArgs)
            CQSPD.RichiesteDeroghe.doOnInviata(New ItemEventArgs(Me))
        End Sub

        Public Sub Ricevi()
            Me.Save()
            Me.OnRicevuta(New System.EventArgs)
        End Sub

        Protected Overridable Sub OnRicevuta(ByVal e As System.EventArgs)
            CQSPD.RichiesteDeroghe.doOnRicevuta(New ItemEventArgs(Me))
        End Sub

    End Class


End Class
