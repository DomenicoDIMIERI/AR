#Const CaricaDocumentiOnLoad = True

Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.CustomerCalls
Imports DMD.Office

Partial Public Class CQSPD

    Public Enum StatoFinestraLavorazione As Integer
        ''' <summary>
        ''' Indica che la finestra è attiva e che non è ancora stata iniziata la lavorazione prevista per il cliente (a partire dalla data di lavorabilità)
        ''' </summary>
        ''' <remarks></remarks>
        NonAperta = 0

        ''' <summary>
        ''' Indica che la finestra è stata aperta ed il cliente è in lavorazione
        ''' </summary>
        ''' <remarks></remarks>
        Aperta = 1

        ''' <summary>
        ''' Indica che la finestra è attiva e che il cliente è stato contattato
        ''' </summary>
        ''' <remarks></remarks>
        Contattato = 10

        ''' <summary>
        ''' Indica che la finestra è attiva e che il cliente è interessato all'offerta
        ''' </summary>
        ''' <remarks></remarks>
        Interessato = 15

        ''' <summary>
        ''' Indica che al cliente è stata proposta almeno un'offerta
        ''' </summary>
        ''' <remarks></remarks>
        Consulenza = 20

        ''' <summary>
        ''' Indica che è stata caricata almeno una pratica per il cliente
        ''' </summary>
        ''' <remarks></remarks>
        Pratica = 25

        ''' <summary>
        ''' Indica che la finestra è stata chiusa e non + più in lavorazione
        ''' </summary>
        ''' <remarks></remarks>
        Chiusa = 255
    End Enum

    Public Enum StatoOfferteFL As Integer
        ''' <summary>
        ''' Stato sconosciuto
        ''' </summary>
        ''' <remarks></remarks>
        Sconosciuto = 0

        ''' <summary>
        ''' Pratica in lavorazione
        ''' </summary>
        ''' <remarks></remarks>
        InLavorazione = 1

        ''' <summary>
        ''' Pratica liquidata
        ''' </summary>
        ''' <remarks></remarks>
        Liquidata = 2

        ''' <summary>
        ''' Pratica rifiutata dal cliente
        ''' </summary>
        ''' <remarks></remarks>
        RifiutataCliente = 3

        ''' <summary>
        ''' Pratica bocciata dal cessionario
        ''' </summary>
        ''' <remarks></remarks>
        BocciataCessionario = 4

        ''' <summary>
        ''' Pratica bocciata dall'agenzia
        ''' </summary>
        ''' <remarks></remarks>
        BocciataAgenzia = 5

        ''' <summary>
        ''' Pratica non fattibile
        ''' </summary>
        ''' <remarks></remarks>
        NonFattibile = 6
    End Enum

    <Flags>
    Public Enum FinestraLavorazioneFlags As Integer
        None = 0

        ''' <summary>
        ''' Flag che indica che il sistema richiede l'attenzione del cliente
        ''' </summary>
        ''' <remarks></remarks>
        OnCliente = 1

        ''' <summary>
        ''' Flag che indica che il sistema richiede l'attenzione dell'operatore CRM
        ''' </summary>
        ''' <remarks></remarks>
        OnOperatore = 2

        ''' <summary>
        ''' Flag che indica che il sistema richiede l'attenzione del consulente
        ''' </summary>
        ''' <remarks></remarks>
        OnConsulente = 4

        ''' <summary>
        ''' Flag che indica che il sistema richiede l'attenzione del broker
        ''' </summary>
        ''' <remarks></remarks>
        OnSubAgenzia = 8

        ''' <summary>
        ''' Flag che indica che il sistema richiede l'attenzione dell'agenzia
        ''' </summary>
        ''' <remarks></remarks>
        OnAgenzia = 16

        ''' <summary>
        ''' Flag che indica che il sistema richiede l'attenzione del cessionario
        ''' </summary>
        ''' <remarks></remarks>
        OnCessionario = 32

        ''' <summary>
        ''' Indica che nella finestra è possibile proporre una cessione
        ''' </summary>
        ''' <remarks></remarks>
        Disponibile_CQS = 64

        ''' <summary>
        ''' Indica che nella finestra è possibile proporre una delega
        ''' </summary>
        ''' <remarks></remarks>
        Disponibile_PD = 128

        ''' <summary>
        ''' Indica che nella finestra è possibile proporre una integrazione alla cessione
        ''' </summary>
        ''' <remarks></remarks>
        Disponibile_CQSI = 256

        ''' <summary>
        ''' Indica che nella finestra è possibile proporre una integrazione alla delega
        ''' </summary>
        ''' <remarks></remarks>
        Disponibile_PDI = 512

        ''' <summary>
        ''' Vero se è stato richiesto un conteggio estintivo per il cliente prima o durante questa finestra di lavorazione
        ''' </summary>
        ''' <remarks></remarks>
        RichiestoCE = 1024

        ''' <summary>
        ''' Vero se il cliente ha visitato o è stato visitato durante questa finestra di lavorazione
        ''' </summary>
        ''' <remarks></remarks>
        VisitaDurante = 2048

        ''' <summary>
        ''' Flag inserito quando la finestra di lavorazione prevede il rinnovo di una pratica in corso
        ''' </summary>
        ''' <remarks></remarks>
        Rinnovo = 4096

        ''' <summary>
        ''' Flags impostato quando la busta paga è stata ricevuta e l'operatore l'ha segnata come valida
        ''' </summary>
        ''' <remarks></remarks>
        BustaPagaValida = 8192


        Ricalcola = 65536
    End Enum

    <Serializable> _
    Public Class FinestraLavorazione
        Inherits DBObjectPO
        Implements IComparable

        Private m_IDCliente As Integer          'ID del cliente
        Private m_NomeCliente As String         'Nome del cliente
        <NonSerialized> Private m_Cliente As CPersona           'Cliente
        Private m_IconaCliente As String        'Icona del cliente

        Private m_IDConsulente As Integer
        Private m_Consulente As CConsulentePratica

#If UsaDataAttivazione Then
        Private m_DataAttivazione As Date?
        Private m_DettaglioStato As String      'stato del cliente (testo)
        Private m_DettaglioStato1 As String     'dettaglio stato del cliente (testo)

        Private m_IDOperatoreRicontatto As Integer  'ID dell'operatore a cui é assegnato il contatto (CRM)
        Private m_OperatoreRicontatto As CUser      'Operatore a cui é assegnato il contatto (CRM)
        Private m_DataRicontatto As DateTime?   'data di ricontatto
        Private m_MotivoRicontatto As String    'motivo del ricontatto
#End If

        Private m_StatoFinestra As StatoFinestraLavorazione 'Stato della finestra

        Private m_Flags As FinestraLavorazioneFlags               'Flags per questo oggetto

        Private m_DataUltimoAggiornamento As Date?  'Data dell'ultimo aggiornamento

        Private m_DataInizioLavorabilita As Date     'Data di inizio di lavorabilità (es. pratiche rinnovabili)
        Private m_DataFineLavorabilita As Date? 'Data di fine lavorabilità (indica la data oltre la quale non ha più senso lavorare questa finestra)
        Private m_DataInizioLavorazione As Date? 'Data di inizio della lavorazione
        Private m_DataFineLavorazione As Date?  'Data di fine lavorazione

        'Private m_Documenti As CCollection(Of CDocumento)   'Elenco dei documenti richiesti
        Private m_DocumentiRichiestiStr As String
        Private m_DocumentiRichiesti As CCollection(Of CDocumentoXGruppoProdotti)   'Elenco dei documenti caricati
        Private m_MessaggiStr As String
        Private m_Messaggi As CCollection(Of FinestraLavorazioneMsg)

        Private m_IDContatto As Integer                 'ID dell'ultima telefonata fatta
        Private m_Contatto As CContattoUtente           'ultima telefonata fatta
        Private m_StatoContatto As StatoOfferteFL       'esito dell'ultima telefonata fatta
        Private m_DataContatto As Date?                 'data dell'ultima telefonata fatta

        Private m_IDPrimaVisita As Integer              'ID della prima visita fatta
        Private m_PrimaVisita As CVisita                'Prima visita fatta
        Private m_StatoPrimaVisita As StatoOfferteFL    'Esito della prima visita
        Private m_DataPrimaVisita As Date?              'Data della prima visita


        Private m_IDRichiestaFinanziamento As Integer                   'ID della richiesta di finanziamento associata
        Private m_RichiestaFinanziamento As CRichiestaFinanziamento     'Richiesta di finanziamento associata    
        Private m_StatoRichiestaFinanziamento As StatoOfferteFL         'Stato della richiesta di finanziamento associata
        Private m_DataRichiestaFinanziamento As Date?                   'data della richiesta di finanziamento associata

        Private m_IDBustaPaga As Integer                                'ID dell'ultima busta paga caricata
        Private m_BustaPaga As CAttachment                              'ultima busta paga caricata     
        Private m_DataBustaPaga As Date?                                'data dell'ultima busta paga caricata
        Private m_StatoBustaPaga As StatoOfferteFL                      'stato di verifica dell'ultima busta paga

        Private m_IDRichiestaCertificato As Integer                     'id del
        Private m_RichiestaCertificato As RichiestaCERQ
        Private m_DataRichiestaCertificato As Date?
        Private m_StatoRichiestaCertificato As StatoOfferteFL

        Private m_QuotaCedibile As Decimal?                             'Valore della quota cedibile

        Private m_IDStudioDiFattibilita As Integer
        Private m_StudioDiFattibilita As CQSPDConsulenza
        Private m_StatoStudioDiFattibilita As StatoOfferteFL
        Private m_DataStudioDiFattibilita As Date?

        Private m_IDCQS As Integer
        Private m_CQS As CRapportino
        Private m_StatoCQS As StatoOfferteFL
        Private m_DataCQS As Date?

        Private m_IDPD As Integer
        Private m_PD As CRapportino
        Private m_StatoPD As StatoOfferteFL
        Private m_DataPD As Date?

        Private m_IDCQSI As Integer
        Private m_CQSI As CRapportino
        Private m_StatoCQSI As StatoOfferteFL
        Private m_DataCQSI As Date?

        Private m_IDPDI As Integer
        Private m_PDI As CRapportino
        Private m_StatoPDI As StatoOfferteFL
        Private m_DataPDI As Date?




        Private m_AltriPrestiti As CCollection(Of CEstinzione)
        Private m_DataEsportazione As Date?
        Private m_DataEsportazioneOk As Date?
        Private m_TokenEsportazione As String
        Private m_EsportatoVerso As String

        Private m_DataImportazione As Date?

        Private m_IDCollaboratore As Integer
        Private m_Collaboratore As CCollaboratore

        Private m_TipoFonte As String
        Private m_IDFonte As Integer
        Private m_Fonte As IFonte

        Private m_IDConsulenza As Integer
        Private m_Consulenza As CQSPDStudioDiFattibilita

        Private m_Pratiche As PratichePerFinestraLavorazione

        Public Sub New()
            Me.m_IDCliente = 0
            Me.m_NomeCliente = ""
            Me.m_Cliente = Nothing
            Me.m_IconaCliente = ""
            Me.m_StatoFinestra = StatoFinestraLavorazione.NonAperta
            Me.m_Flags = FinestraLavorazioneFlags.None
            Me.m_DataInizioLavorabilita = DateUtils.Now
            Me.m_DataFineLavorabilita = Nothing
            Me.m_DataInizioLavorazione = Nothing
            Me.m_DocumentiRichiesti = Nothing
            Me.m_Messaggi = Nothing

            Me.m_IDPrimaVisita = 0
            Me.m_PrimaVisita = Nothing
            Me.m_StatoPrimaVisita = StatoOfferteFL.Sconosciuto
            Me.m_DataPrimaVisita = Nothing

            Me.m_IDRichiestaFinanziamento = 0
            Me.m_RichiestaFinanziamento = Nothing
            Me.m_IDStudioDiFattibilita = 0
            Me.m_StudioDiFattibilita = Nothing
            Me.m_IDCQS = 0
            Me.m_CQS = Nothing
            Me.m_IDPD = 0
            Me.m_PD = Nothing
            Me.m_IDCQSI = 0
            Me.m_CQSI = Nothing
            Me.m_IDPDI = 0
            Me.m_PDI = Nothing
            Me.m_StatoCQS = StatoOfferteFL.Sconosciuto
            Me.m_StatoPD = StatoOfferteFL.Sconosciuto
            Me.m_StatoCQSI = StatoOfferteFL.Sconosciuto
            Me.m_StatoPDI = StatoOfferteFL.Sconosciuto
            Me.m_DataUltimoAggiornamento = Nothing
            Me.m_DataFineLavorazione = Nothing
            Me.m_QuotaCedibile = Nothing
            Me.m_IDBustaPaga = 0
            Me.m_BustaPaga = Nothing
            Me.m_StatoStudioDiFattibilita = StatoOfferteFL.Sconosciuto
            Me.m_StatoRichiestaFinanziamento = StatoOfferteFL.Sconosciuto
            Me.m_IDContatto = 0
            Me.m_Contatto = Nothing
            Me.m_StatoContatto = StatoOfferteFL.Sconosciuto
            Me.m_DataContatto = Nothing
            Me.m_DataBustaPaga = Nothing
            Me.m_StatoBustaPaga = StatoOfferteFL.Sconosciuto
            Me.m_IDRichiestaCertificato = 0
            Me.m_RichiestaCertificato = Nothing
            Me.m_DataRichiestaCertificato = Nothing
            Me.m_StatoRichiestaCertificato = StatoOfferteFL.Sconosciuto
            Me.m_AltriPrestiti = Nothing
            Me.m_DataEsportazione = Nothing
            Me.m_DataEsportazioneOk = Nothing
            Me.m_TokenEsportazione = ""
            Me.m_EsportatoVerso = ""
            Me.m_DataRichiestaFinanziamento = Nothing
            Me.m_DataStudioDiFattibilita = Nothing
            Me.m_DataCQS = Nothing
            Me.m_DataPD = Nothing
            Me.m_DataCQSI = Nothing
            Me.m_DataPDI = Nothing
            Me.m_DataImportazione = Nothing

#If UsaDataAttivazione Then
            Me.m_DataAttivazione = Nothing
            Me.m_DettaglioStato = ""
            Me.m_DettaglioStato1 = ""

            Me.m_IDOperatoreRicontatto = 0
            Me.m_OperatoreRicontatto = Nothing
            Me.m_DataRicontatto = Nothing
            Me.m_MotivoRicontatto = ""
#End If

            Me.m_IDConsulente = 0
            Me.m_Consulente = Nothing

            Me.m_IDCollaboratore = 0
            Me.m_Collaboratore = Nothing


            Me.m_TipoFonte = ""
            Me.m_IDFonte = 0
            Me.m_Fonte = Nothing

            Me.m_IDConsulenza = 0
            Me.m_Consulenza = Nothing
        End Sub

        Public Property Ricalcola As Boolean
            Get
                Return Sistema.TestFlag(Me.m_Flags, FinestraLavorazioneFlags.Ricalcola)
            End Get
            Set(value As Boolean)
                If (Me.Ricalcola = value) Then Return
                Me.m_Flags = Sistema.SetFlag(Me.m_Flags, FinestraLavorazioneFlags.Ricalcola, value)
                Me.DoChanged("Ricalcola", value, Not value)
            End Set
        End Property

        Public Property IDConsulenza As Integer
            Get
                Return GetID(Me.m_Consulenza, Me.m_IDConsulenza)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDConsulenza
                If (oldValue = value) Then Return
                Me.m_IDConsulenza = value
                Me.m_Consulenza = Nothing
                Me.DoChanged("IDConsulenza", value, oldValue)
            End Set
        End Property

        Public Property Consulenza As CQSPDStudioDiFattibilita
            Get
                If (Me.m_Consulenza Is Nothing) Then Me.m_Consulenza = CQSPD.StudiDiFattibilita.GetItemById(Me.m_IDConsulenza)
                Return Me.m_Consulenza
            End Get
            Set(value As CQSPDStudioDiFattibilita)
                Dim oldValue As CQSPDStudioDiFattibilita = Me.m_Consulenza
                If (oldValue Is value) Then Return
                Me.m_Consulenza = value
                Me.m_IDConsulenza = GetID(value)
                Me.DoChanged("Consulenza", value, oldValue)
            End Set
        End Property

        Public Property TipoFonte As String
            Get
                Return Me.m_TipoFonte
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TipoFonte
                If (oldValue = value) Then Return
                Me.m_TipoFonte = value
                Me.m_Fonte = Nothing
                Me.DoChanged("TipoFonte", value, oldValue)
            End Set
        End Property

        Public Property IDFonte As Integer
            Get
                Return GetID(Me.m_Fonte, Me.m_IDFonte)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDFonte
                If (oldValue = value) Then Return
                Me.m_IDFonte = value
                Me.m_Fonte = Nothing
                Me.DoChanged("IDFonte", value, oldValue)
            End Set
        End Property

        Public Property Fonte As IFonte
            Get
                If (Me.m_Fonte Is Nothing) Then Me.m_Fonte = Anagrafica.Fonti.GetItemById(Me.m_TipoFonte, Me.m_TipoFonte, Me.m_IDFonte)
                Return Me.m_Fonte
            End Get
            Set(value As IFonte)
                Dim oldValue As IFonte = Me.Fonte
                If (oldValue Is value) Then Return
                Me.m_IDFonte = GetID(value)
                Me.m_Fonte = value
                Me.DoChanged("Fonte", value, oldValue)
            End Set
        End Property


        Public Property IDCollaboratore As Integer
            Get
                Return GetID(Me.m_Collaboratore, Me.m_IDCollaboratore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCollaboratore
                If (oldValue = value) Then Return
                Me.m_IDCollaboratore = value
                Me.m_Collaboratore = Nothing
                Me.DoChanged("IDCollaboratore", value, oldValue)
            End Set
        End Property

        Public Property Collaboratore As CCollaboratore
            Get
                If (Me.m_Collaboratore Is Nothing) Then Me.m_Collaboratore = CQSPD.Collaboratori.GetItemById(Me.m_IDCollaboratore)
                Return Me.m_Collaboratore
            End Get
            Set(value As CCollaboratore)
                Dim oldValue As CCollaboratore = Me.Collaboratore
                If (oldValue Is value) Then Return
                Me.m_Collaboratore = value
                Me.m_IDCollaboratore = GetID(value)
                Me.DoChanged("IDCollaboratore", value, oldValue)
            End Set
        End Property


#If UsaDataAttivazione Then

        Public Property DataAttivazione As Date?
            Get
                Return Me.m_DataAttivazione
            End Get
            Set(ByVal value As Date?)
                Dim oldValue As Date? = Me.m_DataAttivazione
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataAttivazione = value
                Me.DoChanged("DataAttivazione", value, oldValue)
            End Set
        End Property

        Public Property DettaglioStato As String
            Get
                Return Me.m_DettaglioStato
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_DettaglioStato
                If (oldValue = value) Then Return
                Me.m_DettaglioStato = value
                Me.DoChanged("DettaglioStato", value, oldValue)
            End Set
        End Property

        Public Property DettaglioStato1 As String
            Get
                Return Me.m_DettaglioStato1
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_DettaglioStato1
                If (oldValue = value) Then Return
                Me.m_DettaglioStato1 = value
                Me.DoChanged("DettaglioStato1", value, oldValue)
            End Set
        End Property

        Public Property IDOperatoreRicontatto As Integer  'ID dell'operatore a cui é assegnato il contatto (CRM)
            Get
                Return GetID(Me.m_OperatoreRicontatto, Me.m_IDOperatoreRicontatto)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOperatoreRicontatto
                If (oldValue = value) Then Return
                Me.m_IDOperatoreRicontatto = value
                Me.m_OperatoreRicontatto = Nothing
                Me.DoChanged("IDOperatoreRicontatto", value, oldValue)
            End Set
        End Property

        Public Property OperatoreRicontatto As CUser      'Operatore a cui é assegnato il contatto (CRM)
            Get
                If (Me.m_OperatoreRicontatto Is Nothing) Then Me.m_OperatoreRicontatto = Sistema.Users.GetItemById(Me.m_IDOperatoreRicontatto)
                Return Me.m_OperatoreRicontatto
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.OperatoreRicontatto
                If (oldValue Is value) Then Return
                Me.m_OperatoreRicontatto = value
                Me.m_IDOperatoreRicontatto = GetID(value)
                Me.DoChanged("OperatoreRicontatto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di ricontatto fissata. 
        ''' </summary>
        ''' <returns></returns>
        Public Property DataRicontatto As Date?
            Get
                Return Me.m_DataRicontatto
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataRicontatto
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataRicontatto = value
                Me.DoChanged("DataRicontatto", value, oldValue)
            End Set
        End Property

        Public Property MotivoRicontatto As String
            Get
                Return Me.m_MotivoRicontatto
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_MotivoRicontatto
                If (oldValue = value) Then Return
                Me.m_MotivoRicontatto = value
                Me.DoChanged("MotivoRicontatto", value, oldValue)
            End Set
        End Property



#End If
        Public Property IDConsulente As Integer
            Get
                Return GetID(Me.m_Consulente, Me.m_IDConsulente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDConsulente
                If (oldValue = value) Then Return
                Me.m_Consulente = Nothing
                Me.m_IDConsulente = value
                Me.DoChanged("IDConsulente", value, oldValue)
            End Set
        End Property

        Public Property Consulente As CConsulentePratica
            Get
                If (Me.m_Consulente Is Nothing) Then Me.m_Consulente = CQSPD.Consulenti.GetItemById(Me.m_IDConsulente)
                Return Me.m_Consulente
            End Get
            Set(value As CConsulentePratica)
                Dim oldValue As CConsulentePratica = Me.Consulente
                If (oldValue Is value) Then Return
                Me.m_IDConsulente = GetID(value)
                Me.m_Consulente = value
                Me.DoChanged("Consulente", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property DataProssimaFinestra As Date?
            Get
                Dim d As Date? = DateUtils.Max(Me.DataRinnovoCQS, Me.DataRinnovoPD)
                Dim di As Date? = Me.DataInizioLavorazione : If (di.HasValue = False) Then di = Me.DataInizioLavorabilita
                'If (d.HasValue = False OrElse DateUtils.Compare(d, di) <= 0) Then
                '    Dim minDurata As Integer? = Nothing
                '    For Each p As CEstinzione In Me.AltriPrestiti
                '        If (p.Stato = ObjectStatus.OBJECT_VALID AndAlso p.Durata.HasValue AndAlso p.Durata.Value > 0 AndAlso p.IsInCorsoOFutura(di)) Then
                '            If (minDurata.HasValue) Then
                '                minDurata = Math.Min(p.Durata.Value, minDurata.Value)
                '            Else
                '                minDurata = p.Durata
                '            End If
                '        End If
                '    Next

                '    If (minDurata.HasValue) Then
                '        minDurata = CQSPD.Estinzioni.getMeseRinnovo(minDurata.Value)
                '    End If
                '    If (minDurata.HasValue = False) Then
                '        d = DateUtils.DateAdd(DateInterval.Year, 2, di)
                '    Else
                '        d = DateUtils.DateAdd(DateInterval.Month, minDurata.Value, di)
                '    End If
                'End If
                If (d.HasValue = False) Then d = DateUtils.DateAdd(DateInterval.DayOfYear, 2, di.Value)
                Return DateUtils.GetYearFirstDay(d.Value)
            End Get

        End Property

        Public ReadOnly Property DataRinnovoCQS As Date?
            Get
                Dim ret As Date? = Nothing
                Dim items As CCollection(Of CEstinzione) = Me.AltriPrestiti()
                Dim dMin As Date? = Me.DataInizioLavorazione()
                If (dMin.HasValue = False) Then dMin = Me.DataInizioLavorabilita
                dMin = DateUtils.GetYearFirstDay(dMin.Value)
                dMin = DateUtils.DateAdd(DateInterval.DayOfYear, 1, dMin.Value)
                For Each p As CEstinzione In items
                    If (p.Stato = ObjectStatus.OBJECT_VALID AndAlso
                         (p.Tipo = TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO OrElse
                          p.Tipo = TipoEstinzione.ESTINZIONE_CQP OrElse
                          p.Tipo = TipoEstinzione.ESTINZIONE_PICCOLO_PRESTITO
                         ) AndAlso
                         p.IsInCorsoOFutura(dMin)
                       ) Then
                        Dim dr As Date? = p.DataRinnovo()
                        If (dr.HasValue AndAlso DateUtils.Compare(dMin, dr) <= 0) Then
                            ret = DateUtils.Min(ret, dr)
                        End If
                    End If
                Next

                If (ret.HasValue) Then
                    Return DateUtils.GetMonthFirstDay(ret)
                Else
                    Return Nothing
                End If

            End Get
        End Property

        Public ReadOnly Property DataRinnovoPD As Date?
            Get
                If (Me.Cliente.ImpiegoPrincipale.TipoRapporto() = "H") Then Return Nothing
                Dim ret As Date? = Nothing
                Dim items As CCollection(Of CEstinzione) = Me.AltriPrestiti
                Dim dMin As Date? = Me.DataInizioLavorazione : If (dMin.HasValue = False) Then dMin = Me.DataInizioLavorabilita
                dMin = DateUtils.GetYearFirstDay(dMin.Value)
                dMin = DateUtils.DateAdd(DateInterval.DayOfYear, 1, dMin.Value)
                For Each p As CEstinzione In items
                    If (p.Stato = ObjectStatus.OBJECT_VALID AndAlso p.Tipo = TipoEstinzione.ESTINZIONE_PRESTITODELEGA AndAlso p.IsInCorsoOFutura(dMin)) Then
                        Dim dr As Date? = p.DataRinnovo
                        If (dr.HasValue AndAlso DateUtils.Compare(dMin, dr) <= 0) Then ret = DateUtils.Min(ret, dr)
                    End If
                Next
                If (ret.HasValue = False AndAlso TestFlag(Me.Flags, FinestraLavorazioneFlags.Disponibile_PD)) Then
                    ret = Me.DataRinnovoCQS
                    If (ret.HasValue) Then ret = DateUtils.DateAdd(DateInterval.Month, 6, ret)
                End If

                If (ret.HasValue) Then
                    Return DateUtils.GetMonthFirstDay(ret)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data in cui per la prima volta la finestra è stata importata
        ''' </summary>
        ''' <returns></returns>
        Public Property DataImportazione As Date?
            Get
                Return Me.m_DataImportazione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataImportazione
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataImportazione = value
                Me.DoChanged("DataImportazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della prima visita (ricevuta o effettuata)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPrimaVisita As Integer
            Get
                Return GetID(Me.m_PrimaVisita, Me.m_IDPrimaVisita)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPrimaVisita
                If (oldValue = value) Then Exit Property
                Me.m_IDPrimaVisita = value
                Me.m_PrimaVisita = Nothing
                Me.DoChanged("IDPrimaVisita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la prima visita (effettuata o ricavuta)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PrimaVisita As CVisita
            Get
                If (Me.m_PrimaVisita Is Nothing) Then Me.m_PrimaVisita = CustomerCalls.CRM.GetItemById(Me.m_IDPrimaVisita)
                Return Me.m_PrimaVisita
            End Get
            Set(value As CVisita)
                Dim oldValue As CVisita = Me.m_PrimaVisita
                If (oldValue Is value) Then Exit Property
                Me.m_PrimaVisita = value
                Me.DoChanged("PrimaVisita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato della prima visita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoPrimaVisita As StatoOfferteFL
            Get
                Return Me.m_StatoPrimaVisita
            End Get
            Set(value As StatoOfferteFL)
                Dim oldValue As StatoOfferteFL = Me.m_StatoPrimaVisita
                If (oldValue = value) Then Exit Property
                Me.m_StatoPrimaVisita = value
                Me.DoChanged("StatoPrimaVisita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data della prima visita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataPrimaVisita As Date?
            Get
                Return Me.m_DataPrimaVisita
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataPrimaVisita
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataPrimaVisita = value
                Me.DoChanged("DataPrimaVisita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data della richiesta di finanziamento associata alla finestra
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataRichiestaFinanziamento As Date?
            Get
                Return Me.m_DataRichiestaFinanziamento
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataRichiestaFinanziamento
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataRichiestaFinanziamento = value
                Me.DoChanged("DataRichiestaFinanziamento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data dello studio di fattibilità associato alla finestra
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataStudioDiFattibilita As Date?
            Get
                Return Me.m_DataStudioDiFattibilita
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataStudioDiFattibilita
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataStudioDiFattibilita = value
                Me.DoChanged("DataStudioDiFattibilita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data dello stato attuale della pratica di CQS associata alla finestra
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataCQS As Date?
            Get
                Return Me.m_DataCQS
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataCQS
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataCQS = value
                Me.DoChanged("DataCQS", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data dello stato attuale della pratica di PD associata alla finestra
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataPD As Date?
            Get
                Return Me.m_DataPD
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataPD
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataPD = value
                Me.DoChanged("DataPD", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data dello stato attuale della pratica di CQS (integrazione) associata alla finestra
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataCQSI As Date?
            Get
                Return Me.m_DataCQSI
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataCQSI
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataCQSI = value
                Me.DoChanged("DataCQSI", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data dello stato attuale della pratica di PD (integrazione) associata alla finestra
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataPDI As Date?
            Get
                Return Me.m_DataPDI
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataPDI
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataPDI = value
                Me.DoChanged("DataPDI", value, oldValue)
            End Set
        End Property



        ''' <summary>
        ''' Restituisce o imposta la data e l'ora in cui la finestra di elaborazione è stata esportata per la prima volta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataEsportazione As Date?
            Get
                Return Me.m_DataEsportazione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataEsportazione
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataEsportazione = value
                Me.DoChanged("DataEsportazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data in cui è stata confermata l'esportazione 
        ''' </summary>
        ''' <returns></returns>
        Public Property DataEsportazioneOk As Date?
            Get
                Return Me.m_DataEsportazioneOk
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataEsportazioneOk
                If (DateUtils.Compare(value, oldValue) = 0) Then Return
                Me.m_DataEsportazioneOk = value
                Me.DoChanged("DataEsportazioneOk", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta il nome token utilizzato per esportare l'anagrafica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TokenEsportazione As String
            Get
                Return Me.m_TokenEsportazione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_TokenEsportazione
                If (oldValue = value) Then Exit Property
                Me.m_TokenEsportazione = value
                Me.DoChanged("TokenEsportazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del sito esterno verso cui è stato esportato il token
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EsportatoVerso As String
            Get
                Return Me.m_EsportatoVerso
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_EsportatoVerso
                If (oldValue = value) Then Exit Property
                Me.m_EsportatoVerso = value
                Me.DoChanged("EsportatoVerso", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la collezione dei presti attivi nel periodo di validità della finestra
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property AltriPrestiti As CCollection(Of CEstinzione)
            Get
                SyncLock Me
                    If (Me.m_AltriPrestiti Is Nothing) Then
                        Me.m_AltriPrestiti = New CCollection(Of CEstinzione)
                        Dim items As CCollection(Of CEstinzione) = CQSPD.Estinzioni.GetEstinzioniByPersona(Me.Cliente)
                        Dim di As Date? = Me.DataInizioLavorazione
                        If (di.HasValue = False) Then di = Me.DataInizioLavorabilita
                        For Each p As CEstinzione In items
                            If (p.IsInCorsoOFutura(di)) Then
                                p.SetPersona(Me.Cliente)
                                Me.m_AltriPrestiti.Add(p)
                            End If
                        Next
                    End If
                    Return Me.m_AltriPrestiti
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del clietne
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
        Public Property Cliente As CPersonaFisica
            Get
                If (Me.m_Cliente Is Nothing) Then Me.m_Cliente = Anagrafica.Persone.GetItemById(Me.m_IDCliente)
                Return Me.m_Cliente
            End Get
            Set(value As CPersonaFisica)
                Dim oldValue As CPersonaFisica = Me.m_Cliente
                If (oldValue Is value) Then Exit Property
                Me.m_IDCliente = GetID(value)
                Me.m_Cliente = value
                If (value IsNot Nothing) Then
                    Me.m_NomeCliente = value.Nominativo
                    Me.m_IconaCliente = value.IconURL
                    Me.PuntoOperativo = value.PuntoOperativo
                End If
                Me.DoChanged("Cliente", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetCliente(ByVal value As CPersona)
            Me.m_Cliente = value
            Me.m_IDCliente = GetID(value)
        End Sub

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
                Dim oldValue As String = Me.m_NomeCliente
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeCliente = value
                Me.DoChanged("NomeCliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'icona del cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IconaCliente As String
            Get
                Return Me.m_IconaCliente
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_IconaCliente
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_IconaCliente = value
                Me.DoChanged("IconaCliente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato della fienstra di lavorazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoFinestra As StatoFinestraLavorazione
            Get
                Return Me.m_StatoFinestra
            End Get
            Set(value As StatoFinestraLavorazione)
                Dim oldValue As StatoFinestraLavorazione = Me.m_StatoFinestra
                If (oldValue = value) Then Exit Property
                Me.m_StatoFinestra = value
                Me.DoChanged("StatoFinestra", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o i imposta dei flags aggiuntivi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As FinestraLavorazioneFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As FinestraLavorazioneFlags)
                Dim oldValue As Integer = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data a partire dalla quale sarà possibile proporre una operazione al cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataInizioLavorabilita As Date
            Get
                Return Me.m_DataInizioLavorabilita
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_DataInizioLavorabilita
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataInizioLavorabilita = value
                Me.DoChanged("DataInizioLavorabilita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data oltre la quale non sarà più possibile proporre alcuna operazione al cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataFineLavorabilita As Date?
            Get
                Return Me.m_DataFineLavorabilita
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataFineLavorabilita
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataFineLavorabilita = value
                Me.DoChanged("DataFineLavorabilita", value, oldValue)
            End Set
        End Property




        ''' <summary>
        ''' Restituisce o imposta la data di inizio lavorazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataInizioLavorazione As Date?
            Get
                Return Me.m_DataInizioLavorazione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataInizioLavorazione
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataInizioLavorazione = value
                Me.DoChanged("DataInizioLavorazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di fine lavorazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataFineLavorazione As Date?
            Get
                Return Me.m_DataFineLavorazione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataFineLavorazione
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataFineLavorazione = value
                Me.DoChanged("DataFineLavorazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data dell'ultimo aggiornamento di stato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataUltimoAggiornamento As Date?
            Get
                Return Me.m_DataUltimoAggiornamento
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataUltimoAggiornamento
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataUltimoAggiornamento = value
                Me.DoChanged("DataUltimoAggiornamento", value, oldValue)
            End Set
        End Property

        Public Function GetFlag(ByVal flag As FinestraLavorazioneFlags) As Boolean
            Return TestFlag(Me.m_Flags, flag)
        End Function

        Public Sub SetFlag(ByVal flag As FinestraLavorazioneFlags, ByVal value As Boolean)
            Me.Flags = Sistema.SetFlag(Me.Flags, flag, value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'ultimo contatto 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDContatto As Integer
            Get
                Return GetID(Me.m_Contatto, Me.m_IDContatto)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDContatto
                If (oldValue = value) Then Exit Property
                Me.m_Contatto = Nothing
                Me.m_IDContatto = value
                Me.DoChanged("IDContatto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ultimo contatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Contatto As CContattoUtente
            Get
                If (Me.m_Contatto Is Nothing) Then Me.m_Contatto = CustomerCalls.CRM.GetItemById(Me.m_IDContatto)
                Return Me.m_Contatto
            End Get
            Set(value As CContattoUtente)
                Dim oldValue As CContattoUtente = Me.m_Contatto
                If (oldValue Is value) Then Exit Property
                Me.m_Contatto = value
                Me.m_IDContatto = GetID(value)
                Me.DoChanged("Contatto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data dell'ultimo contatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataContatto As Date?
            Get
                Return Me.m_DataContatto
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataContatto
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataContatto = value
                Me.DoChanged("DataContatto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato del contatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoContatto As StatoOfferteFL
            Get
                Return Me.m_StatoContatto
            End Get
            Set(value As StatoOfferteFL)
                Dim oldValue As StatoOfferteFL = Me.m_StatoContatto
                If (oldValue = value) Then Exit Property
                Me.m_StatoContatto = value
                Me.DoChanged("StatoContatto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la collezione dei documenti che vengono richiesti al cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DocumentiRichiesti As CCollection(Of CDocumentoXGruppoProdotti)
            Get
                SyncLock Me
                    If (Me.m_DocumentiRichiesti Is Nothing) Then
                        If (Me.m_DocumentiRichiestiStr <> vbNullString) Then
                            Me.m_DocumentiRichiesti = Me.GetDocs(Me.m_DocumentiRichiestiStr)
                            Me.m_DocumentiRichiestiStr = vbNullString
                        Else
                            Me.m_DocumentiRichiesti = New CCollection(Of CDocumentoXGruppoProdotti)
                        End If
                    End If
                    Return Me.m_DocumentiRichiesti
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la collezione dei messaggi inviati o ricevuti per questa finestra di lavorazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Messaggi As CCollection(Of FinestraLavorazioneMsg)
            Get
                SyncLock Me
                    If (Me.m_Messaggi Is Nothing) Then
                        If (Me.m_MessaggiStr <> vbNullString) Then
                            Me.m_Messaggi = Me.GetMessages(Me.m_MessaggiStr)
                            Me.m_MessaggiStr = vbNullString
                        Else
                            Me.m_Messaggi = New CCollection(Of FinestraLavorazioneMsg)
                        End If

                    End If
                    Return Me.m_Messaggi
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'ultima richiesta di finanziamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDRichiestaFinanziamento As Integer
            Get
                Return GetID(Me.m_RichiestaFinanziamento, Me.m_IDRichiestaFinanziamento)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRichiestaFinanziamento
                If (oldValue = value) Then Exit Property
                Me.m_IDRichiestaFinanziamento = value
                Me.m_RichiestaFinanziamento = Nothing
                Me.DoChanged("IDRichiestaFinanziamento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ultima richiesta di finanziamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RichiestaFinanziamento As CRichiestaFinanziamento
            Get
                If Me.m_RichiestaFinanziamento Is Nothing Then Me.m_RichiestaFinanziamento = CQSPD.RichiesteFinanziamento.GetItemById(Me.m_IDRichiestaFinanziamento)
                Return Me.m_RichiestaFinanziamento
            End Get
            Set(value As CRichiestaFinanziamento)
                Dim oldValue As CRichiestaFinanziamento = Me.m_RichiestaFinanziamento
                If (oldValue Is value) Then Exit Property
                Me.m_RichiestaFinanziamento = value
                Me.m_IDRichiestaFinanziamento = GetID(value)
                Me.DoChanged("RichiestaFinanziamento", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetRichiestaFinanziamento(ByVal value As CRichiestaFinanziamento)
            Me.m_RichiestaFinanziamento = value
            Me.m_IDRichiestaFinanziamento = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'ultimo studio di fattibilità
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDStudioDiFattibilita As Integer
            Get
                Return GetID(Me.m_StudioDiFattibilita, Me.m_IDStudioDiFattibilita)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDStudioDiFattibilita
                If (oldValue = value) Then Exit Property
                Me.m_IDStudioDiFattibilita = value
                Me.m_StudioDiFattibilita = Nothing
                Me.DoChanged("IDStudioDiFattibilita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ultimo studio di fattibilità
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StudioDiFattibilita As CQSPDConsulenza
            Get
                If (Me.m_StudioDiFattibilita Is Nothing) Then Me.m_StudioDiFattibilita = CQSPD.Consulenze.GetItemById(Me.m_IDStudioDiFattibilita)
                Return Me.m_StudioDiFattibilita
            End Get
            Set(value As CQSPDConsulenza)
                Dim oldValue As CQSPDConsulenza = Me.m_StudioDiFattibilita
                If (oldValue Is value) Then Exit Property
                Me.m_StudioDiFattibilita = value
                Me.m_IDStudioDiFattibilita = GetID(value)
                Me.DoChanged("StudioDiFattibilita", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetStudioDiFattibilita(ByVal value As CQSPDConsulenza)
            Me.m_StudioDiFattibilita = value
            Me.m_IDStudioDiFattibilita = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID della pratica di CQS proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDCQS As Integer
            Get
                Return GetID(Me.m_CQS, Me.m_IDCQS)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCQS
                If (oldValue = value) Then Exit Property
                Me.m_IDCQS = value
                Me.m_CQS = Nothing
                Me.DoChanged("IDCQS", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la CQS proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CQS As CRapportino
            Get
                If (Me.m_CQS Is Nothing) Then Me.m_CQS = CQSPD.Pratiche.GetItemById(Me.m_IDCQS)
                Return Me.m_CQS
            End Get
            Set(value As CRapportino)
                Dim oldValue As CRapportino = Me.m_CQS
                If (oldValue Is value) Then Exit Property
                Me.m_CQS = value
                Me.m_IDCQS = GetID(value)
                Me.DoChanged("CQS", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetCQS(ByVal value As CRapportino)
            Me.m_CQS = value
            Me.m_IDCQS = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID della pratica di delega proposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPD As Integer
            Get
                Return GetID(Me.m_PD, Me.m_IDPD)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPD
                If (oldValue = value) Then Exit Property
                Me.m_IDPD = value
                Me.m_PD = Nothing
                Me.DoChanged("IDPD", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la pratica di delega corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PD As CRapportino
            Get
                If (Me.m_PD Is Nothing) Then Me.m_PD = CQSPD.Pratiche.GetItemById(Me.m_IDPD)
                Return Me.m_PD
            End Get
            Set(value As CRapportino)
                Dim oldValue As CRapportino = Me.m_PD
                If (oldValue Is value) Then Exit Property
                Me.m_PD = value
                Me.m_IDPD = GetID(value)
                Me.DoChanged("PD", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetPD(ByVal value As CRapportino)
            Me.m_PD = value
            Me.m_IDPD = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID della pratica di integrazione alla cessione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDCQSI As Integer
            Get
                Return GetID(Me.m_CQSI, Me.m_IDCQSI)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDCQSI
                If (oldValue = value) Then Exit Property
                Me.m_CQSI = Nothing
                Me.m_IDCQSI = value
                Me.DoChanged("IDCQSI", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la pratica di integrazione alla cessione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CQSI As CRapportino
            Get
                If Me.m_CQSI Is Nothing Then Me.m_CQSI = CQSPD.Pratiche.GetItemById(Me.m_IDCQSI)
                Return Me.m_CQSI
            End Get
            Set(value As CRapportino)
                Dim oldValue As CRapportino = Me.m_CQSI
                If (oldValue Is value) Then Exit Property
                Me.m_CQSI = value
                Me.m_IDCQSI = GetID(value)
                Me.DoChanged("CQSI", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetCQSI(ByVal value As CRapportino)
            Me.m_CQSI = value
            Me.m_IDCQSI = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID della pratica di integrazione alla delega
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPDI As Integer
            Get
                Return GetID(Me.m_PDI, Me.m_IDPDI)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPDI
                If (oldValue = value) Then Exit Property
                Me.m_IDPDI = value
                Me.m_PDI = Nothing
                Me.DoChanged("IDPDI", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la pratica di integrazione alla delega
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PDI As CRapportino
            Get
                If Me.m_PDI Is Nothing Then Me.m_PDI = CQSPD.Pratiche.GetItemById(Me.m_IDPDI)
                Return Me.m_PDI
            End Get
            Set(value As CRapportino)
                Dim oldValue As CRapportino = Me.m_PDI
                If (oldValue Is value) Then Exit Property
                Me.m_PDI = value
                Me.m_IDPDI = GetID(value)
                Me.DoChanged("PDI", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetPDI(ByVal value As CRapportino)
            Me.m_PDI = value
            Me.m_IDPDI = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta lo stato di lavorazione della pratica di cessione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoCQS As StatoOfferteFL
            Get
                Return Me.m_StatoCQS
            End Get
            Set(value As StatoOfferteFL)
                Dim oldValue As StatoOfferteFL = Me.m_StatoCQS
                If (oldValue = value) Then Exit Property
                Me.m_StatoCQS = value
                Me.DoChanged("StatoCQS", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato di lavorazione della pratica di delegazione di pagamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoPD As StatoOfferteFL
            Get
                Return Me.m_StatoPD
            End Get
            Set(value As StatoOfferteFL)
                Dim oldValue As StatoOfferteFL = Me.m_StatoPD
                If (oldValue = value) Then Exit Property
                Me.m_StatoPD = value
                Me.DoChanged("StatoPD", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato di lavorazione della pratica di integrazione alla cessione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoCQSI As StatoOfferteFL
            Get
                Return Me.m_StatoCQSI
            End Get
            Set(value As StatoOfferteFL)
                Dim oldValue As StatoOfferteFL = Me.m_StatoCQSI
                If (oldValue = value) Then Exit Property
                Me.m_StatoCQSI = value
                Me.DoChanged("StatoCQSI", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato di lavorazione della pratica di integrazione alla delega
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoPDI As StatoOfferteFL
            Get
                Return Me.m_StatoPDI
            End Get
            Set(value As StatoOfferteFL)
                Dim oldValue As StatoOfferteFL = Me.m_StatoPDI
                If (oldValue = value) Then Exit Property
                Me.m_StatoPDI = value
                Me.DoChanged("StatoPDI", value, oldValue)
            End Set
        End Property

        Public Property StatoRichiestaFinanziamento As StatoOfferteFL
            Get
                Return Me.m_StatoRichiestaFinanziamento
            End Get
            Set(value As StatoOfferteFL)
                Dim oldValue As StatoOfferteFL = Me.m_StatoRichiestaFinanziamento
                If (oldValue = value) Then Exit Property
                Me.m_StatoRichiestaFinanziamento = value
                Me.DoChanged("StatoRichiestaFinanziamento", value, oldValue)
            End Set
        End Property

        Public Property StatoStudioFattibilita As StatoOfferteFL
            Get
                Return Me.m_StatoStudioDiFattibilita
            End Get
            Set(value As StatoOfferteFL)
                Dim oldValue As StatoOfferteFL = Me.m_StatoStudioDiFattibilita
                If (oldValue = value) Then Exit Property
                Me.m_StatoStudioDiFattibilita = value
                Me.DoChanged("StatoStudioFattibilita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la quota cedibile
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property QuotaCedibile As Decimal?
            Get
                Return Me.m_QuotaCedibile
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_QuotaCedibile
                If (oldValue = value) Then Exit Property
                Me.m_QuotaCedibile = value
                Me.DoChanged("QuotaCedibile", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'ultima busta paga ricevuta durante la finestra di lavorazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDBustaPaga As Integer
            Get
                Return GetID(Me.m_BustaPaga, Me.m_IDBustaPaga)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDBustaPaga
                If (oldValue = value) Then Exit Property
                Me.m_IDBustaPaga = value
                Me.m_BustaPaga = Nothing
                Me.DoChanged("IDBustaPaga", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ultima busta paga ricevuta durante la finestra di lavorazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BustaPaga As CAttachment
            Get
                If (Me.m_BustaPaga Is Nothing) Then Me.m_BustaPaga = Sistema.Attachments.GetItemById(Me.m_IDBustaPaga)
                Return Me.m_BustaPaga
            End Get
            Set(value As CAttachment)
                Dim oldValue As CAttachment = Me.m_BustaPaga
                If (oldValue Is value) Then Exit Property
                Me.m_BustaPaga = value
                Me.m_IDBustaPaga = GetID(value)
                Me.DoChanged("IDBustaPaga", value, oldValue)
            End Set
        End Property

        Public Property DataBustaPaga As Date?
            Get
                Return Me.m_DataBustaPaga
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataBustaPaga
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataBustaPaga = value
                Me.DoChanged("DataBustaPaga", value, oldValue)
            End Set
        End Property

        Public Property StatoBustaPaga As StatoOfferteFL
            Get
                Return Me.m_StatoBustaPaga
            End Get
            Set(value As StatoOfferteFL)
                Dim oldValue As StatoOfferteFL = Me.m_StatoBustaPaga
                If (oldValue = value) Then Exit Property
                Me.m_StatoBustaPaga = value
                Me.DoChanged("StatoBustaPaga", value, oldValue)
            End Set
        End Property

        Public Property IDRichiestaCertificato As Integer
            Get
                Return GetID(Me.m_RichiestaCertificato, Me.m_IDRichiestaCertificato)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRichiestaCertificato
                If (oldValue = value) Then Exit Property
                Me.m_IDRichiestaCertificato = value
                Me.m_RichiestaCertificato = Nothing
                Me.DoChanged("IDRichiestaCertificato", value, oldValue)
            End Set
        End Property

        Public Property RichiestaCertificato As RichiestaCERQ
            Get
                If (Me.m_RichiestaCertificato Is Nothing) Then Me.m_RichiestaCertificato = Office.RichiesteCERQ.GetItemById(Me.m_IDRichiestaCertificato)
                Return Me.m_RichiestaCertificato
            End Get
            Set(value As RichiestaCERQ)
                Dim oldValue As RichiestaCERQ = Me.m_RichiestaCertificato
                If (oldValue Is value) Then Exit Property
                Me.m_RichiestaCertificato = value
                Me.m_IDRichiestaCertificato = GetID(value)
                Me.DoChanged("RichiestaCertificato", value, oldValue)
            End Set
        End Property

        Public Property DataRichiestaCertificato As Date?
            Get
                Return Me.m_DataRichiestaCertificato
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataRichiestaCertificato
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataRichiestaCertificato = value
                Me.DoChanged("DataRichiestaCertificato", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato della richiesta del certificato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoRichiestaCertificato As StatoOfferteFL
            Get
                Return Me.m_StatoRichiestaCertificato
            End Get
            Set(value As StatoOfferteFL)
                Dim oldValue As StatoOfferteFL = Me.m_StatoRichiestaCertificato
                If (oldValue = value) Then Exit Property
                Me.m_StatoRichiestaCertificato = value
                Me.DoChanged("StatoRichiestaCertificato", value, oldValue)
            End Set
        End Property

        Private Function GetDocs(ByVal text As String) As CCollection(Of CDocumentoXGruppoProdotti)
            Dim ret As New CCollection(Of CDocumentoXGruppoProdotti)
            Try
                ret.AddRange(XML.Utils.Serializer.Deserialize(text))
            Catch ex As Exception
                'ret = New CCollection(Of CDocumentoXGruppoProdotti)
            End Try
            Return ret
        End Function

        Private Function GetMessages(ByVal text As String) As CCollection(Of FinestraLavorazioneMsg)
            Dim ret As New CCollection(Of FinestraLavorazioneMsg)
            Try
                ret.AddRange(XML.Utils.Serializer.Deserialize(text))
                ret.Sort()
            Catch ex As Exception
                'ret = New CCollection(Of FinestraLavorazioneMsg)
            End Try
            Return ret
        End Function




        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return CQSPD.FinestreDiLavorazione.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDFinestreLavorazione"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDCliente = reader.Read("IDCliente", Me.m_IDCliente)
            Me.m_NomeCliente = reader.Read("NomeCliente", Me.m_NomeCliente)
            Me.m_IconaCliente = reader.Read("IconaCliente", Me.m_IconaCliente)
            Me.m_StatoFinestra = reader.Read("StatoFinestra", Me.m_StatoFinestra)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_DataInizioLavorabilita = reader.Read("DataInizioLavorabilita", Me.m_DataInizioLavorabilita)
            Me.m_DataFineLavorabilita = reader.Read("DataFineLavorabilita", Me.m_DataFineLavorabilita)
            Me.m_DataInizioLavorazione = reader.Read("DataInizioLavorazione", Me.m_DataInizioLavorazione)
            Me.m_IDRichiestaFinanziamento = reader.Read("IDRichiestaF", Me.m_IDRichiestaFinanziamento)
            Me.m_IDStudioDiFattibilita = reader.Read("IDStudioF", Me.m_IDStudioDiFattibilita)
            Me.m_IDCQS = reader.Read("IDCQS", Me.m_IDCQS)
            Me.m_IDPD = reader.Read("IDPD", Me.m_IDPD)
            Me.m_IDCQSI = reader.Read("IDCQSI", Me.m_IDCQSI)
            Me.m_IDPDI = reader.Read("IDPDI", Me.m_IDPDI)
            Me.m_StatoCQS = reader.Read("StatoCQS", Me.m_StatoCQS)
            Me.m_StatoPD = reader.Read("StatoPD", Me.m_StatoPD)
            Me.m_StatoCQSI = reader.Read("StatoCQSI", Me.m_StatoCQSI)
            Me.m_StatoPDI = reader.Read("StatoPDI", Me.m_StatoPDI)
            Me.m_DataFineLavorazione = reader.Read("DataFineLavorazione", Me.m_DataFineLavorazione)
            Me.m_DataUltimoAggiornamento = reader.Read("DataUltimoAggiornamento", Me.m_DataUltimoAggiornamento)
            Me.m_QuotaCedibile = reader.Read("QuotaCedibile", Me.m_QuotaCedibile)
            Me.m_IDBustaPaga = reader.Read("IDBustaPaga", Me.m_IDBustaPaga)
            Me.m_StatoRichiestaFinanziamento = reader.Read("StatoRichiestaF", Me.m_StatoRichiestaFinanziamento)
            Me.m_StatoStudioDiFattibilita = reader.Read("StatoSF", Me.m_StatoStudioDiFattibilita)
            Me.m_IDContatto = reader.Read("IDContatto", Me.m_IDContatto)
            Me.m_StatoContatto = reader.Read("StatoContatto", Me.m_StatoContatto)
            Me.m_DataContatto = reader.Read("DataContatto", Me.m_DataContatto)
            Me.m_DataBustaPaga = reader.Read("DataBustaPaga", Me.m_DataBustaPaga)
            Me.m_StatoBustaPaga = reader.Read("StatoBustaPaga", Me.m_StatoBustaPaga)
            Me.m_IDRichiestaCertificato = reader.Read("IDRichiestaCertificato", Me.m_IDRichiestaCertificato)
            Me.m_DataRichiestaCertificato = reader.Read("DataRichiestaCertificato", Me.m_DataRichiestaCertificato)
            Me.m_StatoRichiestaCertificato = reader.Read("StatoRichiestaCertificato", Me.m_StatoRichiestaCertificato)
            Me.m_DataEsportazione = reader.Read("DataEsportazione", Me.m_DataEsportazione)
            Me.m_EsportatoVerso = reader.Read("EsportatoVerso", Me.m_EsportatoVerso)
            Me.m_TokenEsportazione = reader.Read("TokenEsportazione", Me.m_TokenEsportazione)

            Me.m_DataRichiestaFinanziamento = reader.Read("DataRichiestaFinanziamento", Me.m_DataRichiestaFinanziamento)
            Me.m_DataStudioDiFattibilita = reader.Read("DataStudioDiFattibilita", Me.m_DataStudioDiFattibilita)
            Me.m_DataCQS = reader.Read("DataCQS", Me.m_DataCQS)
            Me.m_DataPD = reader.Read("DataPD", Me.m_DataPD)
            Me.m_DataCQSI = reader.Read("DataCQSI", Me.m_DataCQSI)
            Me.m_DataPDI = reader.Read("DataPDI", Me.m_DataPDI)

            Me.m_IDPrimaVisita = reader.Read("IDPrimaVisita", Me.m_IDPrimaVisita)
            Me.m_StatoPrimaVisita = reader.Read("StatoPrimaVisita", Me.m_StatoPrimaVisita)
            Me.m_DataPrimaVisita = reader.Read("DataPrivaVisita", Me.m_DataPrimaVisita)

            Me.m_DataImportazione = reader.Read("DataImportazione", Me.m_DataImportazione)
            Me.m_DataEsportazioneOk = reader.Read("DataEsportazioneOk", Me.m_DataEsportazioneOk)
            Me.m_IDCollaboratore = reader.Read("IDCollaboratore", Me.m_IDCollaboratore)

            Me.m_DocumentiRichiesti = Nothing
            Me.m_Messaggi = Nothing
#If CaricaDocumentiOnLoad Then
            'Me.m_DocumentiRichiesti = Me.GetDocs(reader.Read("DocumentiRichiesti", ""))
            Me.m_DocumentiRichiestiStr = reader.Read("DocumentiRichiesti", "")
            Me.m_MessaggiStr = reader.Read("Messaggi", "") 'Me.GetMessages(
#End If

#If UsaDataAttivazione Then
            Me.m_DataAttivazione = reader.Read("DataAttivazione", Me.m_DataAttivazione)
            Me.m_DettaglioStato = reader.Read("DettaglioStato", Me.m_DettaglioStato)
            Me.m_DettaglioStato1 = reader.Read("DettaglioStato1", Me.m_DettaglioStato1)
            Me.m_IDOperatoreRicontatto = reader.Read("IDOpRicontatto", Me.m_IDOperatoreRicontatto)
            Me.m_DataRicontatto = reader.Read("DataRicontatto", Me.m_DataRicontatto)
            Me.m_MotivoRicontatto = reader.Read("MotivoRicontatto", Me.m_MotivoRicontatto)
#End If
            Me.m_IDConsulente = reader.Read("IDConsulente", Me.m_IDConsulente)
            Me.m_TipoFonte = reader.Read("TipoFonte", Me.m_TipoFonte)
            Me.m_IDFonte = reader.Read("IDFonte", Me.m_IDFonte)

            Me.m_IDConsulenza = reader.Read("IDConsulenza", Me.m_IDConsulenza)

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDCliente", Me.IDCliente)
            writer.Write("NomeCliente", Me.m_NomeCliente)
            writer.Write("IconaCliente", Me.m_IconaCliente)
            writer.Write("StatoFinestra", Me.m_StatoFinestra)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("DataInizioLavorabilita", Me.m_DataInizioLavorabilita)
            writer.Write("DataInizioLavorabilitaStr", DBUtils.ToDBDateStr(Me.m_DataInizioLavorabilita))
            writer.Write("DataFineLavorabilita", Me.m_DataFineLavorabilita)
            writer.Write("DataInizioLavorazione", Me.m_DataInizioLavorazione)
#If CaricaDocumentiOnLoad Then
            If (Me.m_DocumentiRichiesti IsNot Nothing) Then
                writer.Write("DocumentiRichiesti", XML.Utils.Serializer.Serialize(Me.DocumentiRichiesti))
            Else
                writer.Write("DocumentiRichiesti", Me.m_DocumentiRichiestiStr)
            End If
            If (Me.m_Messaggi IsNot Nothing) Then
                writer.Write("Messaggi", XML.Utils.Serializer.Serialize(Me.Messaggi))
            Else
                writer.Write("Messaggi", Me.m_MessaggiStr)
            End If
#End If

            writer.Write("IDRichiestaF", Me.IDRichiestaFinanziamento)
            writer.Write("IDStudioF", Me.IDStudioDiFattibilita)
            writer.Write("IDCQS", Me.IDCQS)
            writer.Write("IDPD", Me.IDPD)
            writer.Write("IDCQSI", Me.IDCQSI)
            writer.Write("IDPDI", Me.IDPDI)
            writer.Write("StatoCQS", Me.m_StatoCQS)
            writer.Write("StatoPD", Me.m_StatoPD)
            writer.Write("StatoCQSI", Me.m_StatoCQSI)
            writer.Write("StatoPDI", Me.m_StatoPDI)
            writer.Write("DataFineLavorazione", Me.m_DataFineLavorazione)
            writer.Write("DataUltimoAggiornamento", Me.m_DataUltimoAggiornamento)
            writer.Write("QuotaCedibile", Me.m_QuotaCedibile)
            writer.Write("IDBustaPaga", Me.IDBustaPaga)
            writer.Write("StatoRichiestaF", Me.m_StatoRichiestaFinanziamento)
            writer.Write("StatoSF", Me.m_StatoStudioDiFattibilita)
            writer.Write("IDContatto", Me.IDContatto)
            writer.Write("StatoContatto", Me.m_StatoContatto)
            writer.Write("DataContatto", Me.m_DataContatto)
            writer.Write("DataContattoStr", DBUtils.ToDBDateStr(Me.m_DataContatto))
            writer.Write("DataBustaPaga", Me.m_DataBustaPaga)
            writer.Write("StatoBustaPaga", Me.m_StatoBustaPaga)
            writer.Write("IDRichiestaCertificato", Me.IDRichiestaCertificato)
            writer.Write("DataRichiestaCertificato", Me.m_DataRichiestaCertificato)
            writer.Write("StatoRichiestaCertificato", Me.m_StatoRichiestaCertificato)
            writer.Write("DataEsportazione", Me.m_DataEsportazione)
            writer.Write("EsportatoVerso", Me.m_EsportatoVerso)
            writer.Write("TokenEsportazione", Me.m_TokenEsportazione)
            writer.Write("DataRichiestaFinanziamento", Me.m_DataRichiestaFinanziamento)
            writer.Write("DataStudioDiFattibilita", Me.m_DataStudioDiFattibilita)
            writer.Write("DataCQS", Me.m_DataCQS)
            writer.Write("DataPD", Me.m_DataPD)
            writer.Write("DataCQSI", Me.m_DataCQSI)
            writer.Write("DataPDI", Me.m_DataPDI)
            writer.Write("IDPrimaVisita", Me.IDPrimaVisita)
            writer.Write("StatoPrimaVisita", Me.m_StatoPrimaVisita)
            writer.Write("DataPrivaVisita", Me.m_DataPrimaVisita)
            writer.Write("DataImportazione", Me.m_DataImportazione)
            writer.Write("DataEsportazioneOk", Me.m_DataEsportazioneOk)
            writer.Write("IDCollaboratore", Me.IDCollaboratore)
#If UsaDataAttivazione Then
            writer.Write("DataAttivazione", Me.m_DataAttivazione)
            writer.Write("DettaglioStato", Me.m_DettaglioStato)
            writer.Write("DettaglioStato1", Me.m_DettaglioStato1)
            writer.Write("IDOpRicontatto", Me.IDOperatoreRicontatto)
            writer.Write("DataRicontatto", Me.m_DataRicontatto)
            writer.Write("MotivoRicontatto", Me.m_MotivoRicontatto)
#End If
            writer.Write("IDConsulente", Me.IDConsulente)
            writer.Write("TipoFonte", Me.m_TipoFonte)
            writer.Write("IDFonte", Me.IDFonte)

            writer.Write("IDConsulenza", Me.IDConsulenza)

            Return MyBase.SaveToRecordset(writer)
        End Function


        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDCliente" : Me.m_IDCliente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCliente" : Me.m_NomeCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IconaCliente" : Me.m_IconaCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoFinestra" : Me.m_StatoFinestra = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataInizioLavorabilita" : Me.m_DataInizioLavorabilita = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFineLavorabilita" : Me.m_DataFineLavorabilita = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataInizioLavorazione" : Me.m_DataInizioLavorazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDRichiestaF" : Me.m_IDRichiestaFinanziamento = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDStudioF" : Me.m_IDStudioDiFattibilita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDCQS" : Me.m_IDCQS = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDPD" : Me.m_IDPD = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDCQSI" : Me.m_IDCQSI = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDPDI" : Me.m_IDPDI = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "StatoCQS" : Me.m_StatoCQS = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "StatoPD" : Me.m_StatoPD = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "StatoCQSI" : Me.m_StatoCQSI = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "StatoPDI" : Me.m_StatoPDI = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataFineLavorazione" : Me.m_DataFineLavorazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataUltimoAggiornamento" : Me.m_DataUltimoAggiornamento = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DocumentiRichiesti" : Me.m_DocumentiRichiesti = New CCollection(Of CDocumentoXGruppoProdotti) : Me.m_DocumentiRichiesti.AddRange(fieldValue)
                Case "Messaggi" : Me.m_Messaggi = New CCollection(Of FinestraLavorazioneMsg) : Me.m_Messaggi.AddRange(fieldValue)
                Case "QuotaCedibile" : Me.m_QuotaCedibile = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "IDBustaPaga" : Me.m_IDBustaPaga = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "StatoRichiestaF" : Me.m_StatoRichiestaFinanziamento = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "StatoSF" : Me.m_StatoStudioDiFattibilita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDContatto" : Me.m_IDContatto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "StatoContatto" : Me.m_StatoContatto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataContatto" : Me.m_DataContatto = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataBustaPaga" : Me.m_DataBustaPaga = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "StatoBustaPaga" : Me.m_StatoBustaPaga = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDRichiestaCertificato" : Me.m_IDRichiestaCertificato = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataRichiestaCertificato" : Me.m_DataRichiestaCertificato = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "StatoRichiestaCertificato" : Me.m_StatoRichiestaCertificato = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataEsportazione" : Me.m_DataEsportazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "EsportatoVerso" : Me.m_EsportatoVerso = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TokenEsportazione" : Me.m_TokenEsportazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataRichiestaFinanziamento" : Me.m_DataRichiestaFinanziamento = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataStudioDiFattibilita" : Me.m_DataStudioDiFattibilita = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataCQS" : Me.m_DataCQS = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataPD" : Me.m_DataPD = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataCQSI" : Me.m_DataCQSI = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataPDI" : Me.m_DataPDI = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDPrimaVisita" : Me.m_IDPrimaVisita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "StatoPrimaVisita" : Me.m_StatoPrimaVisita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataPrivaVisita" : Me.m_DataPrimaVisita = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataImportazione" : Me.m_DataImportazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataEsportazioneOk" : Me.m_DataEsportazioneOk = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDCollaboratore" : Me.m_IDCollaboratore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
#If UsaDataAttivazione Then
                Case "DataAttivazione" : Me.m_DataAttivazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DettaglioStato" : Me.m_DettaglioStato = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DettaglioStato1" : Me.m_DettaglioStato1 = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDOpRicontatto" : Me.m_IDOperatoreRicontatto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataRicontatto" : Me.m_DataRicontatto = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "MotivoRicontatto" : Me.m_MotivoRicontatto = XML.Utils.Serializer.DeserializeString(fieldValue)
#End If
                Case "IDConsulente" : Me.m_IDConsulente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoFonte" : Me.m_TipoFonte = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDFonte" : Me.m_IDFonte = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDConsulenza" : Me.m_IDConsulenza = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDCliente", Me.IDCliente)
            writer.WriteAttribute("NomeCliente", Me.m_NomeCliente)
            writer.WriteAttribute("IconaCliente", Me.m_IconaCliente)
            writer.WriteAttribute("StatoFinestra", Me.m_StatoFinestra)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("DataInizioLavorabilita", Me.m_DataInizioLavorabilita)
            writer.WriteAttribute("DataFineLavorabilita", Me.m_DataFineLavorabilita)
            writer.WriteAttribute("DataInizioLavorazione", Me.m_DataInizioLavorazione)
            writer.WriteAttribute("IDRichiestaF", Me.IDRichiestaFinanziamento)
            writer.WriteAttribute("IDStudioF", Me.IDStudioDiFattibilita)
            writer.WriteAttribute("IDCQS", Me.IDCQS)
            writer.WriteAttribute("IDPD", Me.IDPD)
            writer.WriteAttribute("IDCQSI", Me.IDCQSI)
            writer.WriteAttribute("IDPDI", Me.IDPDI)
            writer.WriteAttribute("StatoCQS", Me.m_StatoCQS)
            writer.WriteAttribute("StatoPD", Me.m_StatoPD)
            writer.WriteAttribute("StatoCQSI", Me.m_StatoCQSI)
            writer.WriteAttribute("StatoPDI", Me.m_StatoPDI)
            writer.WriteAttribute("DataFineLavorazione", Me.m_DataFineLavorazione)
            writer.WriteAttribute("DataUltimoAggiornamento", Me.m_DataUltimoAggiornamento)
            writer.WriteAttribute("QuotaCedibile", Me.m_QuotaCedibile)
            writer.WriteAttribute("IDBustaPaga", Me.IDBustaPaga)
            writer.WriteAttribute("StatoRichiestaF", Me.m_StatoRichiestaFinanziamento)
            writer.WriteAttribute("StatoSF", Me.m_StatoStudioDiFattibilita)
            writer.WriteAttribute("IDContatto", Me.IDContatto)
            writer.WriteAttribute("StatoContatto", Me.m_StatoContatto)
            writer.WriteAttribute("DataContatto", Me.m_DataContatto)
            writer.WriteAttribute("DataBustaPaga", Me.m_DataBustaPaga)
            writer.WriteAttribute("StatoBustaPaga", Me.m_StatoBustaPaga)
            writer.WriteAttribute("IDRichiestaCertificato", Me.IDRichiestaCertificato)
            writer.WriteAttribute("DataRichiestaCertificato", Me.m_DataRichiestaCertificato)
            writer.WriteAttribute("StatoRichiestaCertificato", Me.m_StatoRichiestaCertificato)
            writer.WriteAttribute("DataEsportazione", Me.m_DataEsportazione)
            writer.WriteAttribute("EsportatoVerso", Me.m_EsportatoVerso)
            writer.WriteAttribute("TokenEsportazione", Me.m_TokenEsportazione)
            writer.WriteAttribute("DataRichiestaFinanziamento", Me.m_DataRichiestaFinanziamento)
            writer.WriteAttribute("DataStudioDiFattibilita", Me.m_DataStudioDiFattibilita)
            writer.WriteAttribute("DataCQS", Me.m_DataCQS)
            writer.WriteAttribute("DataPD", Me.m_DataPD)
            writer.WriteAttribute("DataCQSI", Me.m_DataCQSI)
            writer.WriteAttribute("DataPDI", Me.m_DataPDI)
            writer.WriteAttribute("IDPrimaVisita", Me.IDPrimaVisita)
            writer.WriteAttribute("StatoPrimaVisita", Me.m_StatoPrimaVisita)
            writer.WriteAttribute("DataPrivaVisita", Me.m_DataPrimaVisita)
            writer.WriteAttribute("DataImportazione", Me.m_DataImportazione)
            writer.WriteAttribute("DataEsportazioneOk", Me.m_DataEsportazioneOk)
            writer.WriteAttribute("IDCollaboratore", Me.IDCollaboratore)
#If UsaDataAttivazione Then
            writer.WriteAttribute("DataAttivazione", Me.m_DataAttivazione)
            writer.WriteAttribute("DettaglioStato", Me.m_DettaglioStato)
            writer.WriteAttribute("DettaglioStato1", Me.m_DettaglioStato1)
            writer.WriteAttribute("IDOpRicontatto", Me.IDOperatoreRicontatto)
            writer.WriteAttribute("DataRicontatto", Me.m_DataRicontatto)
            writer.WriteAttribute("MotivoRicontatto", Me.m_MotivoRicontatto)
#End If
            writer.WriteAttribute("IDConsulente", Me.IDConsulente)
            writer.WriteAttribute("TipoFonte", Me.m_TipoFonte)
            writer.WriteAttribute("IDFonte", Me.IDFonte)

            writer.WriteAttribute("IDConsulenza", Me.IDConsulenza)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("DocumentiRichiesti", Me.DocumentiRichiesti)
            writer.WriteTag("Messaggi", Me.Messaggi)
        End Sub

        Protected Overrides Sub OnCreate(e As SystemEvent)
            MyBase.OnCreate(e)
            CQSPD.FinestreDiLavorazione.doItemCreated(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            MyBase.OnDelete(e)
            CQSPD.FinestreDiLavorazione.doItemDeleted(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnModified(e As SystemEvent)
            MyBase.OnModified(e)
            CQSPD.FinestreDiLavorazione.doItemModified(New ItemEventArgs(Me))
        End Sub

        Private Function IsBocciato(ByVal stato As StatoOfferteFL) As Boolean
            Select Case stato
                Case StatoOfferteFL.BocciataAgenzia, _
                      StatoOfferteFL.BocciataCessionario, _
                       StatoOfferteFL.NonFattibile, _
                        StatoOfferteFL.RifiutataCliente
                    Return True
                Case Else
                    Return False
            End Select
        End Function

        Public Sub NotificaProposta(ByVal p As CQSPDConsulenza)
            If (p Is Nothing) Then Throw New ArgumentNullException("p")
            If (Me.DataInizioLavorazione.HasValue) Then
                If Not Me.CheckBetween(p.DataConsulenza, Me.DataInizioLavorazione, Me.DataFineLavorazione) Then Exit Sub
            Else
                If Not Me.CheckBetween(p.DataConsulenza, Me.DataInizioLavorabilita, Me.DataFineLavorabilita) Then Exit Sub
            End If

            Dim pid As Integer = GetID(p)
            If (Me.IDStudioDiFattibilita = 0 OrElse Me.IsBocciato(Me.StatoStudioFattibilita)) Then
                Me.StudioDiFattibilita = p
            End If
            If (Me.StudioDiFattibilita.Stato = ObjectStatus.OBJECT_VALID) Then
                Select Case StudioDiFattibilita.StatoConsulenza
                    Case StatiConsulenza.ACCETTATA
                        Me.StatoStudioFattibilita = StatoOfferteFL.Liquidata
                        Me.DataStudioDiFattibilita = Me.StudioDiFattibilita.DataConferma

                    Case StatiConsulenza.BOCCIATA
                        Me.StatoStudioFattibilita = StatoOfferteFL.BocciataAgenzia
                        Me.DataStudioDiFattibilita = Me.StudioDiFattibilita.DataAnnullamento

                    Case StatiConsulenza.NONFATTIBILE
                        Me.StatoStudioFattibilita = StatoOfferteFL.NonFattibile
                        Me.DataStudioDiFattibilita = Me.StudioDiFattibilita.DataAnnullamento

                    Case StatiConsulenza.RIFIUTATA
                        Me.StatoStudioFattibilita = StatoOfferteFL.RifiutataCliente
                        Me.DataStudioDiFattibilita = Me.StudioDiFattibilita.DataConferma

                    Case Else
                        Me.StatoStudioFattibilita = StatoOfferteFL.InLavorazione
                        Me.DataStudioDiFattibilita = Me.StudioDiFattibilita.DataConsulenza
                End Select
            Else
                Me.StatoStudioFattibilita = StatoOfferteFL.Sconosciuto
            End If

            Me.m_DataUltimoAggiornamento = DateUtils.Now
            Me.Save(True)
        End Sub


        Public Sub MergeWith(ByVal w As FinestraLavorazione)
            If (w Is Nothing) Then Throw New ArgumentNullException("w")
            If (w Is Me) Then Exit Sub
            If (GetID(w) = GetID(Me)) Then Exit Sub

            If (Me.m_IDCliente = 0) Then Me.m_IDCliente = w.IDCliente
            If (Me.m_NomeCliente = "") Then Me.m_NomeCliente = w.m_NomeCliente
            If (Me.m_Cliente Is Nothing) Then Me.m_Cliente = w.m_Cliente
            If (Me.m_IconaCliente = "") Then Me.m_IconaCliente = w.m_IconaCliente
            If (Me.m_QuotaCedibile.HasValue = False) Then Me.m_QuotaCedibile = w.m_QuotaCedibile

            Me.m_StatoFinestra = Math.Max(Me.m_StatoFinestra, w.m_StatoFinestra)

            Me.m_Flags = Me.m_Flags Or w.m_Flags

            Me.m_DataInizioLavorabilita = DateUtils.Min(Me.m_DataInizioLavorabilita, w.m_DataInizioLavorabilita)
            Me.m_DataFineLavorabilita = DateUtils.Max(Me.m_DataFineLavorabilita, w.m_DataFineLavorabilita)
            Me.m_DataInizioLavorazione = DateUtils.Min(Me.m_DataInizioLavorazione, w.m_DataInizioLavorazione)
            Me.m_DataUltimoAggiornamento = DateUtils.Max(Me.m_DataUltimoAggiornamento, w.m_DataUltimoAggiornamento)
            Me.m_DataFineLavorazione = DateUtils.Max(Me.m_DataFineLavorazione, w.m_DataFineLavorazione)
            Me.m_DataImportazione = DateUtils.Min(Me.m_DataImportazione, w.m_DataImportazione)
            Me.m_DataEsportazioneOk = DateUtils.Min(Me.m_DataEsportazioneOk, w.m_DataEsportazioneOk)

            Me.DocumentiRichiesti.AddRange(w.DocumentiRichiesti)
            Me.DocumentiRichiesti.Sort()

            Me.Messaggi.AddRange(w.Messaggi)
            Me.Messaggi.Sort()

            If (Me.IDContatto = 0 OrElse DateUtils.Compare(Me.m_DataContatto, w.m_DataContatto) > 0) Then
                Me.m_IDContatto = w.m_IDContatto
                Me.m_Contatto = w.m_Contatto
                Me.m_DataContatto = w.m_DataContatto
                Me.m_StatoContatto = w.m_StatoContatto
            End If

            If (Me.IDPrimaVisita = 0 OrElse DateUtils.Compare(Me.m_DataPrimaVisita, w.m_DataPrimaVisita) > 0) Then
                Me.m_IDPrimaVisita = w.m_IDPrimaVisita
                Me.m_PrimaVisita = w.m_PrimaVisita
                Me.m_DataPrimaVisita = w.m_DataPrimaVisita
                Me.m_StatoPrimaVisita = w.m_StatoPrimaVisita
            End If

            If (Me.IDRichiestaFinanziamento = 0) Then
                Me.m_IDRichiestaFinanziamento = w.m_IDRichiestaFinanziamento
                Me.m_RichiestaFinanziamento = w.m_RichiestaFinanziamento
                Me.m_StatoRichiestaFinanziamento = w.m_StatoRichiestaFinanziamento
                Me.m_DataRichiestaFinanziamento = w.m_DataRichiestaFinanziamento

                If (Me.RichiestaFinanziamento IsNot Nothing) Then
                    Me.RichiestaFinanziamento.FinestraLavorazione = Me
                    Me.RichiestaFinanziamento.Save()
                End If
            End If

            If (Me.IDBustaPaga = 0) Then
                Me.m_IDBustaPaga = w.m_IDBustaPaga
                Me.m_BustaPaga = w.m_BustaPaga
                Me.m_DataBustaPaga = w.m_DataBustaPaga
                Me.m_StatoBustaPaga = w.m_StatoBustaPaga
            End If

            If (Me.IDRichiestaCertificato = 0) Then
                Me.m_IDRichiestaCertificato = w.m_IDRichiestaCertificato
                Me.m_RichiestaCertificato = w.m_RichiestaCertificato
                Me.m_DataRichiestaCertificato = w.m_DataRichiestaCertificato
                Me.m_StatoRichiestaCertificato = w.m_StatoRichiestaCertificato
            End If

            If (Me.IDConsulenza = 0) Then
                Me.m_IDConsulenza = w.m_IDConsulenza
                Me.m_Consulenza = w.m_Consulenza
            End If

            If (Me.IDCQS = 0) Then
                Me.m_IDCQS = w.IDCQS
                Me.m_CQS = w.m_CQS
                Me.m_DataCQS = w.m_DataCQS
                Me.m_StatoCQS = w.m_StatoCQS

                If (Me.CQS IsNot Nothing) Then
                    Me.CQS.FinestraLavorazione = Me
                    Me.CQS.Save()
                End If
            End If

            If (Me.IDPD = 0) Then
                Me.m_IDPD = w.IDPD
                Me.m_PD = w.m_PD
                Me.m_StatoPD = w.m_StatoPD
                Me.m_DataPD = w.m_DataPD

                If (Me.PD IsNot Nothing) Then
                    Me.PD.FinestraLavorazione = Me
                    Me.PD.Save()
                End If
            End If

            If (Me.IDCQSI = 0) Then
                Me.m_IDCQSI = w.IDCQSI
                Me.m_CQSI = w.m_CQSI
                Me.m_StatoCQSI = w.m_StatoCQSI
                Me.m_DataCQSI = w.m_DataCQSI

                If (Me.CQSI IsNot Nothing) Then
                    Me.CQSI.FinestraLavorazione = Me
                    Me.CQSI.Save()
                End If
            End If

            If (Me.IDPDI = 0) Then
                Me.m_IDPDI = w.IDPDI
                Me.m_PDI = w.m_PDI
                Me.m_StatoPDI = w.m_StatoPDI
                Me.m_DataPDI = w.m_DataPDI

                If (Me.PDI IsNot Nothing) Then
                    Me.PDI.FinestraLavorazione = Me
                    Me.PDI.Save()
                End If
            End If
#If UsaDataAttivazione Then
            Me.m_DataAttivazione = DateUtils.Min(Me.m_DataAttivazione, w.m_DataAttivazione)
            If (Me.m_DettaglioStato = "") Then
                Me.m_DettaglioStato = w.m_DettaglioStato
                Me.m_DettaglioStato1 = w.m_DettaglioStato1
            End If

            If (Me.IDOperatoreRicontatto = 0) Then
                Me.m_IDOperatoreRicontatto = w.m_IDOperatoreRicontatto
                Me.m_OperatoreRicontatto = w.m_OperatoreRicontatto
            End If

            If (DateUtils.Compare(Me.m_DataRicontatto, w.m_DataRicontatto) > 0) Then
                Me.m_DataRicontatto = w.m_DataRicontatto
                Me.m_MotivoRicontatto = w.m_MotivoRicontatto
            End If
#End If

            If (Me.m_IDConsulente = 0) Then
                Me.m_IDConsulente = w.m_IDConsulente
                Me.m_Consulente = w.m_Consulente
            End If

            If (Me.m_IDCollaboratore = 0) Then
                Me.m_IDCollaboratore = w.m_IDCollaboratore
                Me.m_Collaboratore = w.m_Collaboratore
            End If

            If (Me.Fonte Is Nothing) Then
                Me.m_TipoFonte = w.m_TipoFonte
                Me.m_IDFonte = w.m_IDFonte
                Me.m_Fonte = w.m_Fonte
            End If

            Me.SetChanged(True)
        End Sub

        Public Function CompareTo(ByVal obj As FinestraLavorazione) As Integer
            Dim ret As Integer = Arrays.Compare(Me.StatoFinestra, obj.StatoFinestra)
            Dim d1 As Date? = Me.DataInizioLavorazione : If (d1.HasValue = False) Then d1 = Me.DataInizioLavorabilita
            Dim d2 As Date? = obj.DataInizioLavorazione : If (d2.HasValue = False) Then d2 = obj.DataInizioLavorabilita
            'If (ret = 0) Then ret = DateUtils.Compare(Me.DataInizioLavorabilita, obj.DataInizioLavorabilita)
            If (ret = 0) Then ret = DateUtils.Compare(d1, d2)
            If (ret = 0) Then ret = Strings.Compare(Me.NomeCliente, obj.NomeCliente)
            Return ret
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Public Function MatchDate(ByVal d As Date) As Boolean
            If (Me.DataInizioLavorazione.HasValue) Then
                Return Me.CheckBetween(d, Me.DataInizioLavorazione, Me.DataFineLavorazione)
            Else
                Return Me.CheckBetween(d, Me.DataInizioLavorabilita, Me.DataFineLavorabilita)
            End If
        End Function

        Private Function CheckBetween(ByVal d As Date?, ByVal di As Date?, ByVal df As Date?) As Boolean
            di = DateUtils.GetMonthFirstDay(di)
            df = DateUtils.GetLastSecond(DateUtils.GetLastMonthDay(df))
            Return DateUtils.CheckBetween(d, di, df)
        End Function

        Private Function GetDataCaricamento(ByVal p As CRapportino) As DateTime
            Dim ret As Date? = p.DataDecorrenza
            For Each s As CQSPD.CStatoLavorazionePratica In p.StatiDiLavorazione
                If (s.Data.HasValue AndAlso DateUtils.Compare(s.Data, ret) < 0) Then ret = s.Data
            Next
            If (ret.HasValue = False) Then ret = p.CreatoIl
            Return ret
        End Function

        Sub NotificaPratica(ByVal p As CRapportino)
            If (p Is Nothing) Then Throw New ArgumentNullException("p")
            Dim data As Date = Me.GetDataCaricamento(p)
            If Not Me.MatchDate(data) Then Exit Sub
            Dim pid As Integer = GetID(p)
            Dim statoo As StatoOfferteFL = StatoOfferteFL.InLavorazione
            If (p.StatoAttuale Is CQSPD.StatiPratica.StatoLiquidato OrElse p.StatoAttuale Is CQSPD.StatiPratica.StatoArchiviato) Then
                statoo = StatoOfferteFL.Liquidata
            ElseIf (p.StatoAttuale Is CQSPD.StatiPratica.StatoAnnullato) Then
                Dim rule As CStatoPratRule = p.StatoDiLavorazioneAttuale.RegolaApplicata
                If (rule IsNot Nothing) Then
                    If (TestFlag(rule.Flags, FlagsRegolaStatoPratica.DaCliente)) Then
                        statoo = StatoOfferteFL.RifiutataCliente
                    ElseIf (TestFlag(rule.Flags, FlagsRegolaStatoPratica.Bocciata)) Then
                        statoo = StatoOfferteFL.BocciataAgenzia
                    Else
                        statoo = StatoOfferteFL.NonFattibile
                    End If
                Else
                    statoo = StatoOfferteFL.NonFattibile
                End If
            End If

            If (pid = Me.IDCQS) Then
                Me.StatoCQS = statoo
                Me.DataCQS = data
            ElseIf (pid = Me.IDPD) Then
                Me.StatoPD = statoo
                Me.DataPD = data
            ElseIf (pid = Me.IDCQSI) Then
                Me.StatoCQSI = statoo
                Me.DataCQSI = data
            ElseIf (pid = Me.IDPDI) Then
                Me.StatoPDI = statoo
                Me.DataPDI = data
            End If
            Me.m_DataUltimoAggiornamento = DateUtils.Now()
            ' Me.Save(True)
        End Sub

        Sub NotificaContatto(ByVal c As CContattoUtente)
            If (c Is Nothing) Then Throw New ArgumentNullException("c")
            If (c.Stato <> ObjectStatus.OBJECT_VALID) Then Exit Sub

            If (Me.DataInizioLavorazione.HasValue) Then
                If Not Me.CheckBetween(c.Data, Me.DataInizioLavorazione, Me.DataFineLavorazione) Then Return
            Else
                If Not Me.CheckBetween(c.Data, Me.DataInizioLavorabilita, Me.DataFineLavorabilita) Then Return
            End If



            If Me.StatoFinestra = StatoFinestraLavorazione.NonAperta Then
                If (Me.GetFlag(FinestraLavorazioneFlags.Rinnovo)) Then 'AndAlso Me.CheckBetween(visita.Data, w.DataInizioLavorabilita, w.DataFineLavorabilita)) Then
                    Me.StatoFinestra = StatoFinestraLavorazione.Aperta
                    Me.DataInizioLavorazione = DateUtils.Now
                End If
                'ElseIf w.StatoFinestra <> StatoFinestraLavorazione.Chiusa Then
                '    If (Me.CheckBetween(visita.Data, w.DataInizioLavorazione, w.DataFineLavorazione)) Then
                '        w.DataUltimoAggiornamento = DateUtils.Now
                '        w.Save()
                '    End If
            End If

            If (TypeOf (c) Is CVisita) Then
                If (Me.m_DataPrimaVisita.HasValue = False OrElse DateUtils.Compare(Me.m_DataPrimaVisita, c.Data) > 0) Then
                    Me.PrimaVisita = c
                    Me.DataPrimaVisita = c.Data
                End If
                Select Case Strings.LCase(c.Persona.DettaglioEsito)
                    Case "bocciata" : Me.StatoPrimaVisita = StatoOfferteFL.BocciataAgenzia
                    Case "non fattibile", "irrintracciabile" : Me.StatoPrimaVisita = StatoOfferteFL.NonFattibile
                    Case "non interessato", "rifiutata dal cliente", "non contattare" : Me.StatoPrimaVisita = StatoOfferteFL.RifiutataCliente
                    Case Else : Me.StatoPrimaVisita = StatoOfferteFL.InLavorazione
                End Select
            Else
                If (Me.m_DataContatto.HasValue = False OrElse DateUtils.Compare(Me.m_DataContatto, c.Data) > 0) Then
                    Me.Contatto = c
                    Me.DataContatto = c.Data
                End If
                Select Case Strings.LCase(c.Persona.DettaglioEsito)
                    Case "bocciata" : Me.StatoContatto = StatoOfferteFL.BocciataAgenzia
                    Case "non fattibile", "irrintracciabile" : Me.StatoContatto = StatoOfferteFL.NonFattibile
                    Case "non interessato", "rifiutata dal cliente", "non contattare" : Me.StatoContatto = StatoOfferteFL.RifiutataCliente
                    Case Else : Me.StatoContatto = StatoOfferteFL.InLavorazione
                End Select
            End If

            If (Me.Contatto Is Nothing) Then
                Me.Contatto = Me.PrimaVisita
                Me.DataContatto = Me.DataPrimaVisita
                Me.StatoContatto = Me.StatoPrimaVisita
            End If


            Me.m_DataUltimoAggiornamento = DateUtils.Max(Me.m_DataUltimoAggiornamento, c.Data)

            Me.Save(True)

        End Sub

        Public Sub NotificaRichiesta(ByVal r As CRichiestaFinanziamento)
            If (r Is Nothing) Then Throw New ArgumentNullException("richiesta")
            If (Me.DataInizioLavorazione.HasValue) Then
                If (Not Me.CheckBetween(r.Data, Me.DataInizioLavorazione, Me.DataFineLavorazione)) Then Exit Sub
            Else
                If (Not Me.CheckBetween(r.Data, Me.DataFineLavorabilita, Me.DataFineLavorabilita)) Then Exit Sub
            End If

            'If (GetID(Me) = r.IDFinestraLavorazione) Then Exit Sub
            Dim oldW As FinestraLavorazione = r.FinestraLavorazione

            Me.RichiestaFinanziamento = r
            Me.StatoRichiestaFinanziamento = StatoOfferteFL.Liquidata
            Me.DataUltimoAggiornamento = DateUtils.Now
            Me.DataRichiestaFinanziamento = r.Data

            Me.Save(True)

            If (oldW IsNot Nothing AndAlso GetID(oldW) <> GetID(Me)) Then
                oldW.RichiestaFinanziamento = Nothing
                oldW.StatoRichiestaFinanziamento = StatoOfferteFL.Sconosciuto
                oldW.DataRichiestaFinanziamento = Nothing
                oldW.Save()
            End If


        End Sub

        Private Function GetMaxStatoPratica1(ByVal a As StatoOfferteFL, ByVal b As StatoOfferteFL) As StatoOfferteFL
            Return Math.Max(a, b)
        End Function

        Function GetMaxStatoPratica() As StatoOfferteFL
            Dim ret As StatoOfferteFL = Me.m_StatoCQS
            ret = GetMaxStatoPratica1(ret, Me.m_StatoPD)
            ret = GetMaxStatoPratica1(ret, Me.m_StatoCQSI)
            ret = GetMaxStatoPratica1(ret, Me.m_StatoPDI)
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce la data di rinnovo della cessione con scadenza più recente
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetDataRinnovoCQS() As Date?
            Dim ret As Date? = Nothing
            For Each p As CEstinzione In Me.AltriPrestiti
                If (p.Tipo = TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO OrElse p.Tipo = TipoEstinzione.ESTINZIONE_CQP) AndAlso p.Estinta = False Then
                    Dim dr As Date? = p.DataRinnovo
                    If (dr.HasValue) Then 'AndAlso DateUtils.Compare(Me.DataInizioLavorabilita, dr) > 0) Then
                        ret = DateUtils.Min(ret, dr)
                    End If
                End If
            Next
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce la data di rinnovo della delega con scadenza più recente
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetDataRinnovoPD() As Date?
            Dim ret As Date? = Nothing
            For Each p As CEstinzione In Me.AltriPrestiti
                If p.Tipo = TipoEstinzione.ESTINZIONE_PRESTITODELEGA AndAlso p.Estinta = False Then
                    Dim dr As Date? = p.DataRinnovo
                    If (dr.HasValue) Then 'AndAlso DateUtils.Compare(Me.DataInizioLavorabilita, dr) > 0) Then
                        ret = DateUtils.Min(ret, dr)
                    End If
                End If
            Next
            Return ret
        End Function

        Protected Friend Sub SetAltriPrestiti(ByVal items As CCollection(Of CEstinzione))
            Me.m_AltriPrestiti = items
        End Sub

        Public Function Esporta(ByVal src As CImportExportSource) As CImportExport
            Dim p As CPersona = Me.Cliente

            Dim ie As New CImportExport()
            ie.Esportazione = True
            ie.PersonaEsportata = p
            ie.DataEsportazione = DateUtils.Now()
            ie.EsportataDa = Sistema.Users.CurrentUser
            ie.FinestraLavorazioneEsportata = Me
            ie.AltriPrestiti().AddRange(Me.AltriPrestiti)
            ie.Documenti().AddRange(p.Attachments)
            ie.PuntoOperativo = p.PuntoOperativo
            If (Me.RichiestaFinanziamento IsNot Nothing) Then ie.RichiesteFinanziamento().Add(Me.RichiestaFinanziamento)
            If (Me.StudioDiFattibilita IsNot Nothing) Then ie.Consulenze.Add(Me.StudioDiFattibilita)
            If (Me.CQS IsNot Nothing) Then ie.Pratiche.Add(Me.CQS)
            If (Me.PD IsNot Nothing) Then ie.Pratiche.Add(Me.PD)
            If (Me.CQSI IsNot Nothing) Then ie.Pratiche.Add(Me.CQSI)
            If (Me.PDI IsNot Nothing) Then ie.Pratiche.Add(Me.PDI)
            ie.StatoRemoto = StatoEsportazione.NonEsportato
            ie.Source = src
            ie.Stato = ObjectStatus.OBJECT_VALID
            ie.Esporta()

            If (ie.StatoRemoto <> StatoEsportazione.Esportato) Then Throw New Exception(ie.DettaglioStatoRemoto)

            Me.DataEsportazione = ie.DataEsportazione
            Me.TokenEsportazione = ie.SharedKey
            Me.Save()

            Return ie
        End Function

    End Class




End Class
