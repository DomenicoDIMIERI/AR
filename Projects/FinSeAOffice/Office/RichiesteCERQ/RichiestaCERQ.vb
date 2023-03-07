Imports DMD.Databases

Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Office


    Public Enum StatoRichiestaCERQ As Integer
        ''' <summary>
        ''' La quota non è ancora stata richiesta
        ''' </summary>
        ''' <remarks></remarks>
        DA_RICHIEDERE = 0

        ''' <summary>
        ''' La quota è stata richiesta
        ''' </summary>
        ''' <remarks></remarks>
        RICHIESTA = 1

        ''' <summary>
        ''' La quota è stata ritirata
        ''' </summary>
        ''' <remarks></remarks>
        RITIRATA = 3

        ''' <summary>
        ''' La richiesta è stata annullata dall'operatore
        ''' </summary>
        ''' <remarks></remarks>
        ANNULLATA = 4

        ''' <summary>
        ''' La richiesta è stata rifiutata dall'amministrazione
        ''' </summary>
        ''' <remarks></remarks>
        RIFIUTATA = 2
    End Enum

    ''' <summary>
    ''' Rappresenta una richiesta di conteggio estintivo/rimborso quote
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class RichiestaCERQ
        Inherits DBObjectPO

        ''' <summary>
        ''' Evento generato quando la richiesta viene inviata
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event RichiestaInviata(ByVal sender As Object, ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando la richiesta viene conclusa completandola
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event RichiestaRitirata(ByVal sender As Object, ByVal e As ItemEventArgs)


        ''' <summary>
        ''' Evento generato quando la richiesta viene annullata da un operatore interno
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event RichiestaAnnullata(ByVal sender As Object, ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando la richiesta viene annullata da un operatore esterno
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event RichiestaRifiutata(ByVal sender As Object, ByVal e As ItemEventArgs)

        Private m_Data As Date                      'Data ed ora della richiesta
        <NonSerialized> _
        Private m_Operatore As CUser                'Utente che ha effettuato la richiesta
        Private m_IDOperatore As Integer            'ID dell'operatore che ha effettuato la richiesta
        Private m_NomeOperatore As String           'Nome dell'operatore che ha effettuato la richiesta
        <NonSerialized> _
        Private m_Cliente As CPersonaFisica         'Persona fisica per cui si è richiesto il conteggio/quota
        Private m_IDCliente As Integer              'ID della persona fisica per cui si è richiesto il conteggio/quota
        Private m_NomeCliente As String             'Nome della persona fisica per cui si è richiesto il conteggio/quota
        Private m_TipoRichiesta As String           'Quota cedibile o conteggio estintivo
        <NonSerialized> _
        Private m_AziendaRichiedente As CAzienda    'Azienda che ha fatto la richiesta
        Private m_IDAziendaRichiedente As Integer   'ID dell'azienda che ha effettuato la richiesta
        Private m_NomeAziendaRichiedente As String  'Nome dell'azienda che ha effettuato la richiesta
        <NonSerialized> _
        Private m_Amministrazione As CAzienda       'Amministrazione a cui si è fatta la richiesta
        Private m_IDAmministrazione As Integer      'ID dell'amministrazione a cui si è fatta la richiesta
        Private m_NomeAmministrazione As String     'Nome dell'amministrazione a cui si è fatta la richiesta
        Private m_RichiestaAMezzo As String         'Nome del mezzo usato per la richiesta (telfono, fax, email)
        Private m_RichiestaAIndirizzo As String     'Indirizzo usato per inviare la richiesta
        Private m_Note As String                    'Dettagli aggiunti alla richieta
        Private m_StatoOperazione As StatoRichiestaCERQ     'Stato dell'operazione
        Private m_DataPrevista As Nullable(Of Date) 'Data in cui si prevede di ritirare la richiesta
        Private m_DataEffettiva As Nullable(Of Date)   'Data del ritiro
        Private m_OperatoreEffettivo As CUser          'Utente che ha ritirato la quota
        Private m_IDOperatoreEffettivo As Integer      'ID dell'utente che ha ritirato la quota
        Private m_NomeOperatoreEffettivo As String     'Nome dell'operatore che ha ritirato la quota
        <NonSerialized> _
        Private m_Commissione As Commissione             'Commissione per cui è stata effettuata la richiesta
        Private m_IDCommissione As Integer              'ID della pratica per cui è stata effettuata la richiesta
        ' Private m_DescrizioneCommissione As String      'Stringa descrittiva della pratica
        Private m_ContextType As String             'Contesto che ha generato la richiesta (es. una pratica)
        Private m_ContextID As Integer              'ID del contesto che ha generato la richiesta
        <NonSerialized> _
        Private m_DocumentoProdotto As DBObjectBase
        Private m_IDDocumentoProdotto As Integer
        Private m_TipoDocumentoProdotto As String

        Public Sub New()
            Me.m_Data = Nothing
            Me.m_Operatore = Nothing
            Me.m_IDOperatore = 0
            Me.m_Cliente = Nothing
            Me.m_IDCliente = 0
            Me.m_NomeCliente = vbNullString
            Me.m_TipoRichiesta = vbNullString
            Me.m_Amministrazione = Nothing
            Me.m_IDAmministrazione = 0
            Me.m_NomeAmministrazione = vbNullString
            Me.m_RichiestaAMezzo = vbNullString
            Me.m_RichiestaAIndirizzo = vbNullString
            Me.m_Note = vbNullString
            Me.m_StatoOperazione = StatoRichiestaCERQ.DA_RICHIEDERE
            Me.m_DataPrevista = Nothing
            Me.m_DataEffettiva = Nothing
            Me.m_OperatoreEffettivo = Nothing
            Me.m_IDOperatoreEffettivo = 0
            Me.m_NomeOperatoreEffettivo = vbNullString
            Me.m_Commissione = Nothing
            Me.m_IDCommissione = 0
            'Me.m_DescrizioneCommissione = vbNullString
            Me.m_ContextType = vbNullString
            Me.m_ContextID = 0
            Me.m_AziendaRichiedente = Nothing
            Me.m_IDAziendaRichiedente = 0
            Me.m_NomeAziendaRichiedente = vbNullString
            Me.m_DocumentoProdotto = Nothing
            Me.m_IDDocumentoProdotto = 0
            Me.m_TipoDocumentoProdotto = vbNullString
        End Sub

        Public Overrides Sub InitializeFrom(value As Object)
            MyBase.InitializeFrom(value)
            Me.m_Operatore = Users.CurrentUser
            Me.m_IDOperatore = GetID(Me.m_Operatore)
            Me.m_NomeOperatore = Me.m_Operatore.Nominativo
            Me.m_Data = Now
            Me.m_StatoOperazione = StatoRichiestaCERQ.DA_RICHIEDERE
            Me.m_OperatoreEffettivo = Nothing
            Me.m_IDOperatoreEffettivo = Nothing
            Me.m_NomeOperatoreEffettivo = vbNullString
            Me.m_DataEffettiva = Nothing
            Me.m_DataPrevista = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il documento prodotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DocumentoProdotto As DBObjectBase
            Get
                If (Me.m_DocumentoProdotto Is Nothing) Then Me.m_DocumentoProdotto = DMD.Sistema.Types.GetItemByTypeAndId(Me.m_TipoDocumentoProdotto, Me.m_IDDocumentoProdotto)
                Return Me.m_DocumentoProdotto
            End Get
            Set(value As DBObjectBase)
                Dim oldValue As DBObjectBase = Me.m_DocumentoProdotto
                If (oldValue Is value) Then Exit Property
                Me.m_DocumentoProdotto = value
                If (value IsNot Nothing) Then
                    Me.m_TipoDocumentoProdotto = TypeName(value)
                    Me.m_IDDocumentoProdotto = GetID(value)
                Else
                    Me.m_TipoDocumentoProdotto = vbNullString
                    Me.m_IDDocumentoProdotto = 0
                End If
                Me.DoChanged("DocumentoProdotto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del documento prodotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDDocumentoProdotto As Integer
            Get
                Return GetID(Me.m_DocumentoProdotto, Me.m_IDDocumentoProdotto)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDDocumentoProdotto
                If (oldValue = value) Then Exit Property
                Me.m_IDDocumentoProdotto = value
                Me.m_DocumentoProdotto = Nothing
                Me.DoChanged("IDDocumentoProdotto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del tipo del documento prodotto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoDocumentoProdotto As String
            Get
                Return Me.m_TipoDocumentoProdotto
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TipoDocumentoProdotto
                If (oldValue = value) Then Exit Property
                Me.m_TipoDocumentoProdotto = value
                Me.DoChanged("TipoDocumentoProdotto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o importa l'azienda che ha effettuato la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AziendaRichiedente As CAzienda
            Get
                If (Me.m_AziendaRichiedente Is Nothing) Then Me.m_AziendaRichiedente = Anagrafica.Aziende.GetItemById(Me.m_IDAziendaRichiedente)
                Return Me.m_AziendaRichiedente
            End Get
            Set(value As CAzienda)
                Dim oldValue As CAzienda = Me.m_AziendaRichiedente
                If (oldValue Is value) Then Exit Property
                Me.m_AziendaRichiedente = value
                Me.m_IDAziendaRichiedente = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeAziendaRichiedente = value.Nominativo
                Me.DoChanged("AziendaRichiedente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'azienda richiedente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDAziendaRichiedente As Integer
            Get
                Return GetID(Me.m_AziendaRichiedente, Me.m_IDAziendaRichiedente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAziendaRichiedente
                If (oldValue = value) Then Exit Property
                Me.m_IDAziendaRichiedente = value
                Me.m_AziendaRichiedente = Nothing
                Me.DoChanged("IDAziendaRichiedente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'azienda che ha effettuato la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeAziendaRichiedente As String
            Get
                Return Me.m_NomeAziendaRichiedente
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeAziendaRichiedente
                If (oldValue = value) Then Exit Property
                Me.m_NomeAziendaRichiedente = value
                Me.DoChanged("NomeAziendaRichiedente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data della richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Data As Date
            Get
                Return Me.m_Data
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_Data
                If (oldValue = value) Then Exit Property
                Me.m_Data = value
                Me.DoChanged("Data", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha effettuato la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Operatore As CUser
            Get
                If Me.m_Operatore Is Nothing Then Me.m_Operatore = Users.GetItemById(Me.m_IDOperatore)
                Return Me.m_Operatore
            End Get
            Set(value As CUser)
                Dim oldValue As Object = Me.m_Operatore
                If (oldValue Is value) Then Exit Property
                Me.m_Operatore = value
                Me.m_IDOperatore = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeOperatore = value.Nominativo
                Me.DoChanged("Operatore", value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'operatore a cui è stata assegnata la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDOperatore As Integer
            Get
                Return GetID(Me.m_Operatore, Me.m_IDOperatore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOperatore
                If (oldValue = value) Then Exit Property
                Me.m_IDOperatore = value
                Me.m_Operatore = Nothing
                Me.DoChanged("IDOperatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'operatore che ha effettuato la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeOperatore As String
            Get
                Return Me.m_NomeOperatore
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeOperatore
                If (oldValue = value) Then Exit Property
                Me.m_NomeOperatore = value
                Me.DoChanged("NomeOperatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la persona per cui si è fatta la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Cliente As CPersonaFisica
            Get
                If Me.m_Cliente Is Nothing Then Me.m_Cliente = Anagrafica.Persone.GetItemById(Me.m_IDCliente)
                Return Me.m_Cliente
            End Get
            Set(value As CPersonaFisica)
                Dim oldValue As CPersonaFisica = Me.m_Cliente
                If (oldValue = value) Then Exit Property
                Me.m_Cliente = value
                Me.m_IDCliente = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeCliente = value.Nominativo
                Me.DoChanged("Cliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del cliente per cui si è fatta la richiesta
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
        ''' Restituisce o imposta il nominativo del cliente per cui si è fatta la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeCliente As String
            Get
                Return Me.m_NomeCliente
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeCliente
                If (oldValue = value) Then Exit Property
                Me.m_NomeCliente = value
                Me.DoChanged("NomeCliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo della richiesta (Richiesta conteggio estintivo, Rimborso quote, ...)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoRichiesta As String
            Get
                Return Me.m_TipoRichiesta
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_TipoRichiesta
                If (oldValue = value) Then Exit Property
                Me.m_TipoRichiesta = value
                Me.DoChanged("TipoRichiesta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'amministrazione a cui si è stata inviata la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Amministrazione As CAzienda
            Get
                If (Me.m_Amministrazione Is Nothing) Then Me.m_Amministrazione = Anagrafica.Aziende.GetItemById(Me.m_IDAmministrazione)
                Return Me.m_Amministrazione
            End Get
            Set(value As CAzienda)
                Dim oldValue As CAzienda = Me.m_Amministrazione
                If (oldValue = value) Then Exit Property
                Me.m_Amministrazione = value
                Me.m_IDAmministrazione = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeAmministrazione = value.Nominativo
                Me.DoChanged("Amministrazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'amministrazione a cui è stata fatta la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDAmministrazione As Integer
            Get
                Return GetID(Me.m_Amministrazione, Me.m_IDAmministrazione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAmministrazione
                If (oldValue = value) Then Exit Property
                Me.m_IDAmministrazione = value
                Me.m_Amministrazione = Nothing
                Me.DoChanged("IDAmministrazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'amministrazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeAmministrazione As String
            Get
                Return Me.m_NomeAmministrazione
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeAmministrazione
                If (oldValue = value) Then Exit Property
                Me.m_NomeAmministrazione = value
                Me.DoChanged("NomeAmministrazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il mezzo usato per la richiesta (e-mail, fax, telefono)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RichiestaAMezzo As String
            Get
                Return Me.m_RichiestaAMezzo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_RichiestaAMezzo
                If (oldValue = value) Then Exit Property
                Me.m_RichiestaAMezzo = value
                Me.DoChanged("RichiestaAMezzo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo usato per la richiesta (numero di telefono o di fax, indirizzo email, indirizzo postale)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RichiestaAIndirizzo As String
            Get
                Return Me.m_RichiestaAIndirizzo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_RichiestaAIndirizzo
                If (oldValue = value) Then Exit Property
                Me.m_RichiestaAIndirizzo = value
                Me.DoChanged("RichiestaAIndirizzo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa per informazioni aggiuntive
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Note As String
            Get
                Return Me.m_Note
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Note
                If (oldValue = value) Then Exit Property
                Me.m_Note = value
                Me.DoChanged("Note", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato dell'operazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoOperazione As StatoRichiestaCERQ
            Get
                Return Me.m_StatoOperazione
            End Get
            Set(value As StatoRichiestaCERQ)
                Dim oldValue As StatoRichiestaCERQ = Me.m_StatoOperazione
                If (oldValue = value) Then Exit Property
                Me.m_StatoOperazione = value
                Me.DoChanged("StatoOperazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data prevista per il ritiro
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataPrevista As Nullable(Of Date)
            Get
                Return Me.m_DataPrevista
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataPrevista
                If (oldValue = value) Then Exit Property
                Me.m_DataPrevista = value
                Me.DoChanged("DataPrevista", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di ritiro (o di annullamento/rifiuto)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataEffettiva As Nullable(Of Date)
            Get
                Return Me.m_DataEffettiva
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataEffettiva
                If (oldValue = value) Then Exit Property
                Me.m_DataEffettiva = value
                Me.DoChanged("DataEffettiva", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'operatore che ha ritirato (annullato o memorizzato il rifiuto)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OperatoreEffettivo As CUser
            Get
                If (Me.m_OperatoreEffettivo Is Nothing) Then Me.m_Operatore = Users.GetItemById(Me.m_IDOperatoreEffettivo)
                Return Me.m_OperatoreEffettivo
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_OperatoreEffettivo
                If (oldValue Is value) Then Exit Property
                Me.m_OperatoreEffettivo = value
                Me.m_IDOperatoreEffettivo = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeOperatoreEffettivo = value.Nominativo
                Me.DoChanged("OperatoreEffettivo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'operatore che ha ritirato (annulato o memorizzato il rifiuto)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDOperatoreEffettivo As Integer
            Get
                Return GetID(Me.m_OperatoreEffettivo, Me.m_IDOperatoreEffettivo)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_IDOperatoreEffettivo
                If (oldValue = value) Then Exit Property
                Me.m_IDOperatoreEffettivo = value
                Me.m_OperatoreEffettivo = Nothing
                Me.DoChanged("IDOperatoreEffettivo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'operatore che ha ritirato (annullato o memorizzato il rifiuto)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeOperatoreEffettivo As String
            Get
                Return Me.m_NomeOperatoreEffettivo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeOperatoreEffettivo
                If (oldValue = value) Then Exit Property
                Me.m_NomeOperatoreEffettivo = value
                Me.DoChanged("NomeOperatoreEffettivo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la pratica per cui è stata effettuata la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Commissione As Commissione
            Get
                If (Me.m_Commissione Is Nothing) Then Me.m_Commissione = Commissioni.GetItemById(Me.m_IDCommissione)
                Return Me.m_Commissione
            End Get
            Set(value As Commissione)
                Dim oldValue As Commissione = Me.m_Commissione
                If (oldValue Is value) Then Exit Property
                Me.m_Commissione = value
                Me.m_IDCommissione = GetID(value)
                Me.DoChanged("Commissione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della pratica per cui è stata fatta la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDCommissione As Integer
            Get
                Return GetID(Me.m_Commissione, Me.m_IDCommissione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCommissione
                If (oldValue = value) Then Exit Property
                Me.m_IDCommissione = value
                Me.m_Commissione = Nothing
                Me.DoChanged("IDCommissione", value, oldValue)
            End Set
        End Property

        ' ''' <summary>
        ' ''' Restituisce o imposta una stringa descrittiva per la pratica
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Property DescrizioneCommissione As String
        '    Get
        '        Return Me.m_DescrizioneCommissione
        '    End Get
        '    Set(value As String)
        '        value = Trim(value)
        '        Dim oldValue As String = Me.m_DescrizioneCommissione
        '        If (oldValue = value) Then Exit Property
        '        Me.m_DescrizioneCommissione = value
        '        Me.DoChanged("DescrizioneCommissione", value, oldValue)
        '    End Set
        'End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo del contesto a cui è associata la richiesta (es. una pratica)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ContextType As String
            Get
                Return Me.m_ContextType
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_ContextType
                If (oldValue = value) Then Exit Property
                Me.m_ContextType = value
                Me.DoChanged("ContextType", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del contesto che ha generato la richiesta (es. una pratica)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ContextID As Integer
            Get
                Return Me.m_ContextID
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_ContextID
                If (oldValue = value) Then Exit Property
                Me.m_ContextID = value
                Me.DoChanged("ContextID", value, oldValue)
            End Set
        End Property

        Public Sub Richiedi()
            If Me.StatoOperazione <> StatoRichiestaCERQ.DA_RICHIEDERE Then Throw New InvalidOperationException("La richiesta è già stata inoltrata")
            Me.StatoOperazione = StatoRichiestaCERQ.RICHIESTA
            Me.Operatore = Users.CurrentUser
            Me.Data = Calendar.Now
            Me.Save()
            Dim e As New ItemEventArgs(Me)
            Me.OnRichiestaInviata(e)
            Office.RichiesteCERQ.doRichiestaInviata(e)
        End Sub

        Protected Overridable Sub OnRichiestaInviata(ByVal e As ItemEventArgs)
            RaiseEvent RichiestaInviata(Me, e)
        End Sub

        Public Sub Ritira()
            If Me.StatoOperazione <> StatoRichiestaCERQ.RICHIESTA Then Throw New InvalidOperationException("La richiesta non è in stato Richiesta")
            Me.StatoOperazione = StatoRichiestaCERQ.RITIRATA
            Me.OperatoreEffettivo = Users.CurrentUser
            Me.DataEffettiva = Calendar.Now()
            Me.Save()
            Dim e As New ItemEventArgs(Me)
            Me.OnRichiestaRitirata(e)
            Office.RichiesteCERQ.doRichiestaRitirata(e)
        End Sub

        Protected Overridable Sub OnRichiestaRitirata(ByVal e As ItemEventArgs)
            RaiseEvent RichiestaRitirata(Me, e)
        End Sub

        Public Sub Annulla()
            If Me.StatoOperazione > StatoRichiestaCERQ.RICHIESTA Then Throw New InvalidOperationException("La richiesta non è in stato Richiesta")
            Me.StatoOperazione = StatoRichiestaCERQ.ANNULLATA
            Me.OperatoreEffettivo = Users.CurrentUser
            Me.DataEffettiva = Calendar.Now()
            Me.Save()
            Dim e As New ItemEventArgs(Me)
            Me.OnRichiestaAnnullata(e)
            Office.RichiesteCERQ.doRichiestaAnnullata(e)
        End Sub

        Protected Overridable Sub OnRichiestaAnnullata(ByVal e As ItemEventArgs)
            RaiseEvent RichiestaAnnullata(Me, e)
        End Sub

        Public Sub Rifiuta()
            If Me.StatoOperazione <> StatoRichiestaCERQ.RICHIESTA Then Throw New InvalidOperationException("La richiesta non è in stato Richiesta")
            Me.StatoOperazione = StatoRichiestaCERQ.RIFIUTATA
            Me.OperatoreEffettivo = Users.CurrentUser
            Me.DataEffettiva = Calendar.Now()
            Me.Save()
            Dim e As New ItemEventArgs(Me)
            Me.OnRichiestaRifiutata(e)
            Office.RichiesteCERQ.doRichiestaRifiutata(e)
        End Sub

        Protected Overridable Sub OnRichiestaRifiutata(ByVal e As ItemEventArgs)
            RaiseEvent RichiestaRifiutata(Me, e)
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return RichiesteCERQ.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDRichCERQ"
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Data", Me.m_Data)
            writer.Write("IDOperatore", Me.IDOperatore)
            writer.Write("NomeOperatore", Me.m_NomeOperatore)
            writer.Write("IDCliente", Me.IDCliente)
            writer.Write("NomeCliente", Me.m_NomeCliente)
            writer.Write("TipoRichiesta", Me.m_TipoRichiesta)
            writer.Write("IDAmministrazione", Me.IDAmministrazione)
            writer.Write("NomeAmministrazione", Me.m_NomeAmministrazione)
            writer.Write("RichiestaAMezzo", Me.m_RichiestaAMezzo)
            writer.Write("RichiestaAIndirizzo", Me.m_RichiestaAIndirizzo)
            writer.Write("Note", Me.m_Note)
            writer.Write("StatoOperazione", Me.m_StatoOperazione)
            writer.Write("DataPrevista", Me.m_DataPrevista)
            writer.Write("DataEffettiva", Me.m_DataEffettiva)
            writer.Write("IDOperatoreEffettivo", Me.IDOperatoreEffettivo)
            writer.Write("NomeOperatoreEffettivo", Me.m_NomeOperatoreEffettivo)
            writer.Write("IDCommissione", Me.IDCommissione)
            writer.Write("ContextType", Me.m_ContextType)
            writer.Write("ContextID", Me.m_ContextID)
            writer.Write("IDAziendaRichiedente", Me.IDAziendaRichiedente)
            writer.Write("NomeAziendaRichiedente", Me.m_NomeAziendaRichiedente)
            writer.Write("TipoOggettoProdotto", Me.m_TipoDocumentoProdotto)
            writer.Write("IDOggettoProdotto", Me.IDDocumentoProdotto)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Data = reader.Read("Data", Me.m_Data)
            Me.m_IDOperatore = reader.Read("IDOperatore", Me.m_IDOperatore)
            Me.m_NomeOperatore = reader.Read("NomeOperatore", Me.m_NomeOperatore)
            Me.m_IDCliente = reader.Read("IDCliente", Me.m_IDCliente)
            Me.m_NomeCliente = reader.Read("NomeCliente", Me.m_NomeCliente)
            Me.m_TipoRichiesta = reader.Read("TipoRichiesta", Me.m_TipoRichiesta)
            Me.m_IDAmministrazione = reader.Read("IDAmministrazione", Me.m_IDAmministrazione)
            Me.m_NomeAmministrazione = reader.Read("NomeAmministrazione", Me.m_NomeAmministrazione)
            Me.m_RichiestaAMezzo = reader.Read("RichiestaAMezzo", Me.m_RichiestaAMezzo)
            Me.m_RichiestaAIndirizzo = reader.Read("RichiestaAIndirizzo", Me.m_RichiestaAIndirizzo)
            Me.m_Note = reader.Read("Note", Me.m_Note)
            Me.m_StatoOperazione = reader.Read("StatoOperazione", Me.m_StatoOperazione)
            Me.m_DataPrevista = reader.Read("DataPrevista", Me.m_DataPrevista)
            Me.m_DataEffettiva = reader.Read("DataEffettiva", Me.m_DataEffettiva)
            Me.m_IDOperatoreEffettivo = reader.Read("IDOperatoreEffettivo", Me.m_IDOperatoreEffettivo)
            Me.m_NomeOperatoreEffettivo = reader.Read("NomeOperatoreEffettivo", Me.m_NomeOperatoreEffettivo)
            Me.m_IDCommissione = reader.Read("IDCommissione", Me.m_IDCommissione)
            Me.m_ContextType = reader.Read("ContextType", Me.m_ContextType)
            Me.m_ContextID = reader.Read("ContextID", Me.m_ContextID)
            Me.m_IDAziendaRichiedente = reader.Read("IDAziendaRichiedente", Me.m_IDAziendaRichiedente)
            Me.m_NomeAziendaRichiedente = reader.Read("NomeAziendaRichiedente", Me.m_NomeAziendaRichiedente)
            Me.m_TipoDocumentoProdotto = reader.Read("TipoOggettoProdotto", Me.m_TipoDocumentoProdotto)
            Me.m_IDDocumentoProdotto = reader.Read("IDOggettoProdotto", Me.m_IDDocumentoProdotto)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Data", Me.m_Data)
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("NomeOperatore", Me.m_NomeOperatore)
            writer.WriteAttribute("IDCliente", Me.IDCliente)
            writer.WriteAttribute("NomeCliente", Me.m_NomeCliente)
            writer.WriteAttribute("TipoRichiesta", Me.m_TipoRichiesta)
            writer.WriteAttribute("IDAmministrazione", Me.IDAmministrazione)
            writer.WriteAttribute("NomeAmministrazione", Me.m_NomeAmministrazione)
            writer.WriteAttribute("RichiestaAMezzo", Me.m_RichiestaAMezzo)
            writer.WriteAttribute("RichiestaAIndirizzo", Me.m_RichiestaAIndirizzo)
            writer.WriteAttribute("StatoOperazione", Me.m_StatoOperazione)
            writer.WriteAttribute("DataPrevista", Me.m_DataPrevista)
            writer.WriteAttribute("DataEffettiva", Me.m_DataEffettiva)
            writer.WriteAttribute("IDOperatoreEffettivo", Me.IDOperatoreEffettivo)
            writer.WriteAttribute("NomeOperatoreEffettivo", Me.m_NomeOperatoreEffettivo)
            writer.WriteAttribute("IDCommissione", Me.IDCommissione)
            writer.WriteAttribute("ContextType", Me.m_ContextType)
            writer.WriteAttribute("ContextID", Me.m_ContextID)
            writer.WriteAttribute("IDAziendaRichiedente", Me.IDAziendaRichiedente)
            writer.WriteAttribute("NomeAziendaRichiedente", Me.m_NomeAziendaRichiedente)
            writer.WriteAttribute("TipoOggettoProdotto", Me.m_TipoDocumentoProdotto)
            writer.WriteAttribute("IDOggettoProdotto", Me.IDDocumentoProdotto)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Note", Me.m_Note)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Data" : Me.m_Data = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDOperatore" : Me.m_IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatore" : Me.m_NomeOperatore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDCliente" : Me.m_IDCliente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCliente" : Me.m_NomeCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoRichiesta" : Me.m_TipoRichiesta = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDAmministrazione" : Me.m_IDAmministrazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAmministrazione" : Me.m_NomeAmministrazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "RichiestaAMezzo" : Me.m_RichiestaAMezzo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "RichiestaAIndirizzo" : Me.m_RichiestaAIndirizzo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Note" : Me.m_Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoOperazione" : Me.m_StatoOperazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataPrevista" : Me.m_DataPrevista = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataEffettiva" : Me.m_DataEffettiva = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDOperatoreEffettivo" : Me.m_IDOperatoreEffettivo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatoreEffettivo" : Me.m_NomeOperatoreEffettivo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDCommissione" : Me.m_IDCommissione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ContextType" : Me.m_ContextType = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ContextID" : Me.m_ContextID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDAziendaRichiedente" : Me.m_IDAziendaRichiedente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAziendaRichiedente" : Me.m_NomeAziendaRichiedente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoOggettoProdotto" : Me.m_TipoDocumentoProdotto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDOggettoProdotto" : Me.m_IDDocumentoProdotto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_TipoRichiesta & " fatta il " & Formats.FormatUserDate(Me.m_Data) & " da " & Me.m_NomeOperatore & " per " & Me.m_NomeCliente & " presso " & Me.m_NomeAmministrazione
        End Function

        'Public Function ProgrammaAlert() As Notifica
        '    Dim notifica As Notifica = Nothing

        '    If (Me.Stato <> ObjectStatus.OBJECT_VALID) Then
        '        'Se l'oggetto è stato eliminato o non è stato confermato eliminiamo eventuali notifiche associate
        '        Sistema.Notifiche.CancelPendingAlertsBySource(Nothing, Me, "Richiesta CERQ")
        '    Else
        '        'l'oggetto è in stato valido
        '        If (Me.m_DataPrevista.HasValue AndAlso Me.m_StatoOperazione <= StatoRichiestaCERQ.RICHIESTA) Then
        '            'Se è stata inserita una data di ritiro prevista programmiamo la notifica
        '            Dim descr As String = "Ritirare " & Me.m_TipoRichiesta & vbNewLine & " presso " & Me.m_NomeAmministrazione & vbNewLine & " di " & Me.m_NomeCliente
        '            Dim notifiche As CCollection(Of Notifica) = Sistema.Notifiche.GetPendingAlertsBySource(Me.Operatore, Me)
        '            If (notifiche.Count > 0) Then
        '                notifica = notifiche(0)
        '                notifica.Descrizione = descr
        '                notifica.Target = Me.Operatore
        '                notifica.Data = Me.m_DataPrevista
        '                For i = 1 To notifiche.Count - 1
        '                    notifiche(i).StatoNotifica = StatoNotifica.ANNULLATA
        '                    notifiche(i).Save()
        '                Next
        '            Else
        '                notifica = Sistema.Notifiche.ProgramAlert(Me.Operatore, descr, Me.m_DataPrevista, Me, "Richiesta CERQ")
        '            End If
        '            notifica.Save()
        '        Else
        '            'Se non è stata inserita una data prevista eliminiamo eventuali nofifiche programmate
        '            Sistema.Notifiche.CancelPendingAlertsBySource(Nothing, Me, "Richiesta CERQ")
        '        End If
        '    End If

        '    Return notifica
        'End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret AndAlso Me.m_Commissione IsNot Nothing) Then Me.m_Commissione.Save(force)
            Return ret
        End Function

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            ' Me.ProgrammaAlert()
            MyBase.OnAfterSave(e)
        End Sub

        Protected Overrides Sub OnCreate(e As SystemEvent)
            MyBase.OnCreate(e)
            Office.RichiesteCERQ.doItemCreated(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            MyBase.OnDelete(e)
            Office.RichiesteCERQ.doItemDeleted(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnModified(e As SystemEvent)
            MyBase.OnModified(e)
            Office.RichiesteCERQ.doItemModified(New ItemEventArgs(Me))
        End Sub

        'Protected Overrides Function SaveToDatabase(dbConn As CDBConnection) As Boolean
        '    Dim statoOld As ObjectStatus = Databases.GetOriginalValue(Me, "Stato")
        '    Dim statoRichOld As StatoRichiestaCERQ = Databases.GetOriginalValue(Me, "StatoOperazione")

        '    Dim ret As Boolean = MyBase.SaveToDatabase(dbConn)
        '    If (ret AndAlso Me.Stato = ObjectStatus.OBJECT_VALID) Then
        '        If (statoOld <> ObjectStatus.OBJECT_VALID) Then
        '            RichiesteCERQ.doRichiestaCreata(New RichiesteCERQ.RichiestaCERQEventArgs(Me))
        '        End If
        '        If (statoRichOld = StatoRichiestaCERQ.DA_RICHIEDERE AndAlso Me.StatoOperazione = StatoRichiestaCERQ.RICHIESTA) Then
        '            RichiesteCERQ.doRichiestaEffettuata(New RichiesteCERQ.RichiestaCERQEventArgs(Me))
        '        ElseIf (statoRichOld = StatoRichiestaCERQ.RICHIESTA) Then
        '            Select Case Me.StatoOperazione
        '                Case StatoRichiestaCERQ.RIFIUTATA
        '                    RichiesteCERQ.doRichiestaRifiutata(New RichiesteCERQ.RichiestaCERQEventArgs(Me))
        '                Case StatoRichiestaCERQ.RITIRATA
        '                    RichiesteCERQ.doRichiestaRitirata(New RichiesteCERQ.RichiestaCERQEventArgs(Me))
        '                Case StatoRichiestaCERQ.ANNULLATA
        '                    RichiesteCERQ.doRichiestaAnnullata(New RichiesteCERQ.RichiestaCERQEventArgs(Me))
        '                Case StatoRichiestaCERQ.DA_RICHIEDERE
        '                    Debug.Print("La richiesta è stata retrocessa")
        '                Case Else

        '            End Select
        '        End If
        '    End If
        '    Return ret
        'End Function

       

    End Class



End Class