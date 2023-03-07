Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class Office

    Public Enum StatoOggettoInventariato As Integer
        Sconosciuto = 0
        Funzionante = 1
        ConQualcheDifetto = 2
        DaRiparare = 3
        ValutazioneRiparazione = 4
        NonRiparabile = 5
        Dismesso = 10
    End Enum

    Public Enum StatoAcquistoOggettoInventariato As Integer
        Sconosciuto = 0
        Nuovo = 1
        Usato = 2
        Ricondizionato = 3
        Usurato = 4
        GuastoRiparabile = 5
        GuastoNonRiparabile = 6
    End Enum

    Public Enum FlagsOggettoInventariato As Integer
        None = 0

        ''' <summary>
        ''' Indica che l'oggetto è in uso
        ''' </summary>
        ''' <remarks></remarks>
        InUso = 1
    End Enum

    <Serializable> _
    Public Class OggettoInventariato
        Inherits DBObjectPO

        Private m_Codice As String          'Codice di inventario
        Private m_IDArticolo As Integer     'ID dell'articolo associato
        <NonSerialized> Private m_Articolo As Articolo      'Articolo associato
        Private m_NomeArticolo As String            'Nome dell'articolo
        Private m_Nome As String            'Nome che identifica questo oggetto
        Private m_Categoria As String       'Categoria dell'articolo
        Private m_Marca As String           'Marca dell'articolo
        Private m_Modello As String         'Modello dell'articolo
        Private m_Seriale As String         'Restituisce il numero seriale dell'articolo 
        Private m_Descrizione As String     'Descrizione dell'articolo
        Private m_IconURL As String         'URL dell'immagine predefinita associata all'articolo
        Private m_StatoAttuale As StatoOggettoInventariato
        Private m_DettaglioStatoAttuale As String
        Private m_ValoreStimato As Decimal?
        Private m_DataValutazione As Date?
        Private m_TipoInUsoDa As String
        Private m_IDInUsoDa As Integer
        Private m_InUsoDa As Object
        Private m_NomeInUsoDa As String

        Private m_DataProduzione As Date?       'Data di produzione di questo oggetto
        Private m_DataAcquisto As Date?     'Data di acquisto
        Private m_TipoDocumentoAcquisto As String   'Tipo del documento che attesta l'acquisto
        Private m_NumeroDocumentoAcquisto As String 'Numero del documento che attesta l'acquisto
        Private m_StatoAcquisto As StatoAcquistoOggettoInventariato
        Private m_DettaglioStatoAcquisto As String
        Private m_AcquistatoDaID As Integer
        <NonSerialized> Private m_AcquistatoDa As CUser
        Private m_NomeAcquistatoDa As String
        Private m_PrezzoAcquisto As Decimal?
        Private m_AliquotaIVA As Single?

        Private m_IDUfficioOriginale As Integer     'ID dell'ufficio per cui è stato originariamente acquistato l'articolo
        <NonSerialized> Private m_UfficioOriginale As CUfficio      'Ufficio per cui è stato originariamente acquistato l'articolo
        Private m_NomeUfficioOriginale As String    'Nome dell'ufficio per cui è stato originariamente acquistato l'articolo

        Private m_CodiceScaffale As String          'Codice dello scaffale
        Private m_CodiceReparto As String           'Codice del reparto

        Private m_DataDismissione As Date?          'Data e ora di dismissione
        Private m_DismessoDaID As Integer           'Utente che ha dismesso l'articolo
        <NonSerialized> Private m_DismessoDa As CUser               'Utente che ha dismesso l'articolo
        Private m_NomeDismessoDa As String          'Nome dell'utente che ha dismesso l'articolo
        Private m_MotivoDismissione As String       'Motivo dismissione
        Private m_DettaglioDismissione As String    'Dettaglio Dismissione
        Private m_ValoreDismissione As Decimal?
        Private m_AliquotaIVADismissione As Single?

        Private m_Attributi As AttributiOggettoCollection
        Private m_Relazioni As OggettiCollegatiCollection
        Private m_Flags As FlagsOggettoInventariato

        Private m_IDOrdineAcquisto As Integer
        Private m_OrdineAcquisto As DocumentoContabile

        Private m_IDDocumentoAcquisto As Integer
        Private m_DocumentoAcquisto As DocumentoContabile

        Private m_IDSpedizione As Integer
        Private m_Spedizione As Spedizione

        Private m_CodiceRFID As String
        Private m_PosizioneGPS As GPSRecord
        Private m_IsPosizioneGPSRelativa As Boolean

        'Private m_CodiceInterno As String


        Public Sub New()
            Me.m_Codice = ""
            Me.m_Nome = ""
            Me.m_NomeArticolo = ""
            Me.m_Categoria = ""
            Me.m_Marca = ""
            Me.m_Modello = ""
            Me.m_Seriale = ""
            Me.m_Descrizione = ""
            Me.m_IconURL = ""
            Me.m_StatoAttuale = StatoOggettoInventariato.Sconosciuto
            Me.m_DettaglioStatoAttuale = ""
            Me.m_ValoreStimato = Nothing
            Me.m_DataValutazione = Nothing
            Me.m_TipoInUsoDa = ""
            Me.m_IDInUsoDa = 0
            Me.m_InUsoDa = Nothing
            Me.m_NomeInUsoDa = ""

            Me.m_DataProduzione = Nothing
            Me.m_DataAcquisto = Nothing
            Me.m_TipoDocumentoAcquisto = ""
            Me.m_NumeroDocumentoAcquisto = ""
            Me.m_StatoAcquisto = StatoAcquistoOggettoInventariato.Sconosciuto
            Me.m_DettaglioStatoAcquisto = ""
            Me.m_AcquistatoDaID = 0
            Me.m_AcquistatoDa = Nothing
            Me.m_NomeAcquistatoDa = ""
            Me.m_PrezzoAcquisto = Nothing
            Me.m_AliquotaIVA = Nothing

            Me.m_IDUfficioOriginale = 0
            Me.m_UfficioOriginale = Nothing
            Me.m_NomeUfficioOriginale = ""

            Me.m_CodiceReparto = ""
            Me.m_CodiceScaffale = ""

            Me.m_DataDismissione = Nothing
            Me.m_DismessoDaID = 0
            Me.m_DismessoDa = Nothing
            Me.m_NomeDismessoDa = ""
            Me.m_MotivoDismissione = ""
            Me.m_DettaglioDismissione = ""
            Me.m_ValoreDismissione = Nothing
            Me.m_AliquotaIVADismissione = Nothing

            Me.m_Attributi = Nothing
            Me.m_Relazioni = Nothing
            Me.m_Flags = FlagsOggettoInventariato.None

            Me.m_IDOrdineAcquisto = 0
            Me.m_OrdineAcquisto = Nothing
            Me.m_IDDocumentoAcquisto = 0
            Me.m_DocumentoAcquisto = Nothing
            Me.m_IDSpedizione = 0
            Me.m_Spedizione = Nothing

            Me.m_CodiceRFID = ""
            Me.m_PosizioneGPS = New GPSRecord
            Me.m_IsPosizioneGPSRelativa = True
            'Me.m_CodiceInterno = ""
        End Sub

        ' ''' <summary>
        ' ''' Restituisce o imposta il codice interno
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Property CodiceInterno As String
        '    Get
        '        Return Me.m_CodiceInterno
        '    End Get
        '    Set(value As String)
        '        Dim oldValue As String = Me.m_CodiceInterno
        '        value = Strings.Trim(value)
        '        If (oldValue = value) Then Exit Property
        '        Me.m_CodiceInterno = value
        '        Me.DoChanged("CodiceInterno", value, oldValue)
        '    End Set
        'End Property

        ''' <summary>
        ''' Restituisce o imposta il codice RFID associato all'oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CodiceRFID As String
            Get
                Return Me.m_CodiceRFID
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_CodiceRFID
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_CodiceRFID = value
                Me.DoChanged("CodiceRFID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta le coordinate GPS che determinano la posizione dell'oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PosizioneGPS As GPSRecord
            Get
                Return Me.m_PosizioneGPS
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se la posizione GPS è relativa all'ufficio di appartenenza o se è assoluta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IsPosizioneGPSRelativa As Boolean
            Get
                Return Me.m_IsPosizioneGPSRelativa
            End Get
            Set(value As Boolean)
                If (Me.m_IsPosizioneGPSRelativa = value) Then Exit Property
                Me.m_IsPosizioneGPSRelativa = value
                Me.DoChanged("IsPosizioneGPSRelativa", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del documento di ordine di acquisto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDOrdineAcquisto As Integer
            Get
                Return GetID(Me.m_OrdineAcquisto, Me.m_IDOrdineAcquisto)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDOrdineAcquisto
                If (oldValue = value) Then Exit Property
                Me.m_IDOrdineAcquisto = value
                Me.m_OrdineAcquisto = Nothing
                Me.DoChanged("IDOrdineAcquisto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ordine di acquisto  
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OrdineAcquisto As DocumentoContabile
            Get
                If (Me.m_OrdineAcquisto Is Nothing) Then Me.m_OrdineAcquisto = Office.DocumentiContabili.GetItemById(Me.m_IDOrdineAcquisto)
                Return Me.m_OrdineAcquisto
            End Get
            Set(value As DocumentoContabile)
                Dim oldValue As DocumentoContabile = Me.m_OrdineAcquisto
                If (oldValue Is value) Then Exit Property
                Me.m_DocumentoAcquisto = value
                Me.m_IDDocumentoAcquisto = GetID(value)
                Me.DoChanged("OrdineAcquisto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del documento che convalida l'acquisto dell'oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDDocumentoAcquisto As Integer
            Get
                Return GetID(Me.m_DocumentoAcquisto, Me.m_IDDocumentoAcquisto)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDDocumentoAcquisto
                If (oldValue = value) Then Exit Property
                Me.m_IDDocumentoAcquisto = value
                Me.m_DocumentoAcquisto = Nothing
                Me.DoChanged("IDDocumentoAcquisto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il documento di acquisto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DocumentoAcquisto As DocumentoContabile
            Get
                If (Me.m_DocumentoAcquisto Is Nothing) Then Me.m_DocumentoAcquisto = Office.DocumentiContabili.GetItemById(Me.m_IDDocumentoAcquisto)
                Return Me.m_DocumentoAcquisto
            End Get
            Set(value As DocumentoContabile)
                Dim oldValue As DocumentoContabile = Me.m_DocumentoAcquisto
                If (oldValue Is value) Then Exit Property
                Me.m_DocumentoAcquisto = value
                Me.m_IDDocumentoAcquisto = GetID(value)
                Me.DoChanged("DocumentoAcquisto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della spedizione con cui è stato ricevuto l'oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDSpedizione As Integer
            Get
                Return GetID(Me.m_Spedizione, Me.m_IDSpedizione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDSpedizione
                If (oldValue = value) Then Exit Property
                Me.m_IDSpedizione = value
                Me.m_Spedizione = Nothing
                Me.DoChanged("IDSpedizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la spedizione con cui è stato ricevuto l'oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Spedizione As Spedizione
            Get
                If (Me.m_Spedizione Is Nothing) Then Me.m_Spedizione = Office.Spedizioni.GetItemById(Me.m_IDSpedizione)
                Return Me.m_Spedizione
            End Get
            Set(value As Spedizione)
                Dim oldValue As Spedizione = Me.m_Spedizione
                If (oldValue Is value) Then Exit Property
                Me.m_Spedizione = value
                Me.m_IDSpedizione = GetID(value)
                Me.DoChanged("Spedizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la collezione delle relazione con gli altri oggetti inventariati
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Relazioni As OggettiCollegatiCollection
            Get
                SyncLock Me
                    If (Me.m_Relazioni Is Nothing) Then Me.m_Relazioni = New OggettiCollegatiCollection(Me)
                    Return Me.m_Relazioni
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di produzione del lotto a cui appartiene questo oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataProduzione As Date?
            Get
                Return Me.m_DataProduzione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataProduzione
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataProduzione = value
                Me.DoChanged("DataProduzione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'articolo associato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDArticolo As Integer
            Get
                Return GetID(Me.m_Articolo, Me.m_IDArticolo)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDArticolo
                If (oldValue = value) Then Exit Property
                Me.m_IDArticolo = value
                Me.m_Articolo = Nothing
                Me.DoChanged("IDArticolo", value, oldValue)
            End Set
        End Property

        Public Property Articolo As Articolo
            Get
                If (Me.m_Articolo Is Nothing) Then Me.m_Articolo = Office.Articoli.GetItemById(Me.m_IDArticolo)
                Return Me.m_Articolo
            End Get
            Set(value As Articolo)
                Dim oldValue As Articolo = Me.m_Articolo
                If (oldValue Is value) Then Exit Property
                Me.m_Articolo = value
                Me.m_IDArticolo = GetID(value)
                If (value Is Nothing) Then
                    Me.m_NomeArticolo = ""
                Else
                    Me.m_NomeArticolo = value.Nome
                End If
                Me.DoChanged("Articolo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Nome Articolo associato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeArticolo As String
            Get
                Return Me.m_NomeArticolo
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeArticolo
                If (oldValue = value) Then Exit Property
                Me.m_NomeArticolo = value
                Me.DoChanged("NomeArticolo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il codice di inventario dell'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Codice As String
            Get
                Return Me.m_Codice
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Codice
                If (oldValue = value) Then Exit Property
                Me.m_Codice = value
                Me.DoChanged("Codice", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        Public Property Categoria As String
            Get
                Return Me.m_Categoria
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Categoria
                If (oldValue = value) Then Exit Property
                Me.m_Categoria = value
                Me.DoChanged("Categoria", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la marca dell'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Marca As String
            Get
                Return Me.m_Marca
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Marca
                If (oldValue = value) Then Exit Property
                Me.m_Marca = value
                Me.DoChanged("Marca", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il modello dell'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Modello As String
            Get
                Return Me.m_Modello
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Modello
                If (oldValue = value) Then Exit Property
                Me.m_Modello = value
                Me.DoChanged("Modello", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restitusice una collezione di proprietà aggiuntive
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Attributi As AttributiOggettoCollection
            Get
                SyncLock Me
                    If (Me.m_Attributi Is Nothing) Then Me.m_Attributi = New AttributiOggettoCollection(Me)
                    Return Me.m_Attributi
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che descrive in dettaglio l'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Descrizione As String
            Get
                Return Me.m_Descrizione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Descrizione
                If (oldValue = value) Then Exit Property
                Me.m_Descrizione = value
                Me.DoChanged("Descrizione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la url dell'immagine principale dell'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IconURL As String
            Get
                Return Me.m_IconURL
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_IconURL
                If (oldValue = value) Then Exit Property
                Me.m_IconURL = value
                Me.DoChanged("IconURL", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato attuale dell'oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoAttuale As StatoOggettoInventariato
            Get
                Return Me.m_StatoAttuale
            End Get
            Set(value As StatoOggettoInventariato)
                Dim oldValue As StatoOggettoInventariato = Me.m_StatoAttuale
                If (oldValue = value) Then Exit Property
                Me.m_StatoAttuale = value
                Me.DoChanged("StatoAttuale", value, oldValue)
            End Set
        End Property

        Public Property DettaglioStatoAttuale As String
            Get
                Return Me.m_DettaglioStatoAttuale
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_DettaglioStatoAttuale
                If (oldValue = value) Then Exit Property
                Me.m_DettaglioStatoAttuale = value
                Me.DoChanged("DettaglioStatoAttuale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo di oggetto che utilizza l'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoInUsoDa As String
            Get
                Return Me.m_TipoInUsoDa
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TipoInUsoDa
                If (oldValue = value) Then Exit Property
                If (value Is Nothing) Then
                    Me.m_TipoInUsoDa = ""
                Else
                    Me.m_TipoInUsoDa = TypeName(value)
                End If
                Me.m_TipoInUsoDa = value
                Me.m_InUsoDa = Nothing
                Me.DoChanged("TipoInUsoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restitusice o imposta l'ID dell'oggetto che utilizza questo articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDInUsoDa As Integer
            Get
                Return GetID(Me.m_InUsoDa, Me.m_IDInUsoDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDInUsoDa
                If (oldValue = value) Then Exit Property
                Me.m_IDInUsoDa = value
                Me.m_InUsoDa = Nothing
                Me.DoChanged("IDInUsoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'oggetto che utilizza questo artcillo 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property InUsoDa As Object
            Get
                If (Me.m_InUsoDa Is Nothing) Then Me.m_InUsoDa = Sistema.Types.GetItemByTypeAndId(Me.m_TipoInUsoDa, Me.m_IDInUsoDa)
                Return Me.m_InUsoDa
            End Get
            Set(value As Object)
                Dim oldValue As Object = Me.InUsoDa
                If (oldValue Is value) Then Exit Property
                Me.m_InUsoDa = value
                Me.m_IDInUsoDa = GetID(value)
                Me.DoChanged("InUsoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'oggetto che utilizza questo articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeInUsoDa As String
            Get
                Return Me.m_NomeInUsoDa
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeInUsoDa
                If (oldValue = value) Then Exit Property
                Me.m_NomeInUsoDa = value
                Me.DoChanged("NomeInUsoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il valore stimato dell'articolo alla data memorizzata in DataValutazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ValoreStimato As Decimal?
            Get
                Return Me.m_ValoreStimato
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_ValoreStimato
                If (oldValue = value) Then Exit Property
                Me.m_ValoreStimato = value
                Me.DoChanged("ValoreStimato", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di valutazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataValutazione As Date?
            Get
                Return Me.m_DataValutazione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataValutazione
                If DateUtils.Compare(value, oldValue) = 0 Then Exit Property
                Me.m_DataValutazione = value
                Me.DoChanged("DataValutazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di acquisto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataAcquisto As Date?
            Get
                Return Me.m_DataAcquisto
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataAcquisto
                If DateUtils.Compare(value, oldValue) = 0 Then Exit Property
                Me.m_DataAcquisto = value
                Me.DoChanged("DataAcquisto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo del documento di acquisto (Fattura, Scontrino, ...)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoDocumentoAcquisto As String
            Get
                Return Me.m_TipoDocumentoAcquisto
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TipoDocumentoAcquisto
                If (oldValue = value) Then Exit Property
                Me.m_TipoDocumentoAcquisto = value
                Me.DoChanged("TipoDocumentoAcquisto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero del documento di acquisto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroDocumentoAcquisto As String
            Get
                Return Me.m_NumeroDocumentoAcquisto
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NumeroDocumentoAcquisto
                If (oldValue = value) Then Exit Property
                Me.m_NumeroDocumentoAcquisto = value
                Me.DoChanged("NumeroDocumentoAcquisto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato dell'oggetto al momento dell'acquisto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoAcquisto As StatoAcquistoOggettoInventariato
            Get
                Return Me.m_StatoAcquisto
            End Get
            Set(value As StatoAcquistoOggettoInventariato)
                Dim oldValue As StatoAcquistoOggettoInventariato = Me.m_StatoAcquisto
                If (oldValue = value) Then Exit Property
                Me.m_StatoAcquisto = value
                Me.DoChanged("StatoAcquisto", value, oldValue)
            End Set
        End Property

        Public Property DettaglioStatoAcquisto As String
            Get
                Return Me.m_DettaglioStatoAcquisto
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_DettaglioStatoAcquisto
                If (oldValue = value) Then Exit Property
                Me.m_DettaglioStatoAcquisto = value
                Me.DoChanged("DettaglioStatoAcquisto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restitusice o imposta l'ID dell'utente che ha acquistato l'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AcquistatoDaID As Integer
            Get
                Return GetID(Me.m_AcquistatoDa, Me.m_AcquistatoDaID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.AcquistatoDaID
                If (oldValue = value) Then Exit Property
                Me.m_AcquistatoDaID = value
                Me.m_AcquistatoDa = Nothing
                Me.DoChanged("AcquistatoDaID", value, oldValue)
            End Set
        End Property

        Public Property AcquistatoDa As CUser
            Get
                SyncLock Me
                    If (Me.m_AcquistatoDa Is Nothing) Then Me.m_AcquistatoDa = Sistema.Users.GetItemById(Me.m_AcquistatoDaID)
                    Return Me.m_AcquistatoDa
                End SyncLock
            End Get
            Set(value As CUser)
                Dim oldValue As CUser
                SyncLock Me
                    oldValue = Me.AcquistatoDa
                    If (oldValue Is value) Then Exit Property
                    Me.m_AcquistatoDa = value
                    Me.m_AcquistatoDaID = GetID(value)
                    If (value IsNot Nothing) Then Me.m_NomeAcquistatoDa = value.Nominativo
                End SyncLock
                Me.DoChanged("AcquistatoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che ha acquistato l'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeAcquistatoDa As String
            Get
                Return Me.m_NomeAcquistatoDa
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeAcquistatoDa
                If (oldValue = value) Then Exit Property
                Me.m_NomeAcquistatoDa = value
                Me.DoChanged("NomeAcquistatoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il prezzo (imponibile) di acquisto dell'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PrezzoAcquisto As Decimal?
            Get
                Return Me.m_PrezzoAcquisto
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_PrezzoAcquisto
                If (oldValue = value) Then Exit Property
                Me.m_PrezzoAcquisto = value
                Me.DoChanged("PrezzoAcquisto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restitusice o imposta l'aliquota iva espressa in % sul prezzo imponibile di acquisto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AliquotaIVA As Single?
            Get
                Return Me.m_AliquotaIVA
            End Get
            Set(value As Single?)
                Dim oldValue As Single? = Me.m_AliquotaIVA
                If (oldValue = value) Then Exit Property
                Me.m_AliquotaIVA = value
                Me.DoChanged("AliquotaIVA", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del punto di operativo per cui è stato originariamente acquistato l'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDUfficioOriginale As Integer
            Get
                Return GetID(Me.m_UfficioOriginale, Me.m_IDUfficioOriginale)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDUfficioOriginale
                If (oldValue = value) Then Exit Property
                Me.m_IDUfficioOriginale = value
                Me.m_UfficioOriginale = Nothing
                Me.DoChanged("IDUfficioOriginale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ufficio per cui è stato originariamente acquistato l'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UfficioOriginale As CUfficio
            Get
                If (Me.m_UfficioOriginale Is Nothing) Then Me.m_UfficioOriginale = Anagrafica.Uffici.GetItemById(Me.m_IDUfficioOriginale)
                Return Me.m_UfficioOriginale
            End Get
            Set(value As CUfficio)
                Dim oldValue As CUfficio = Me.UfficioOriginale
                If (oldValue Is value) Then Exit Property
                Me.m_UfficioOriginale = value
                Me.m_IDUfficioOriginale = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeUfficioOriginale = value.Nome
                Me.DoChanged("UfficioOriginale", value, oldValue)
            End Set
        End Property

        Public Property NomeUfficioOriginale As String
            Get
                Return Me.m_NomeUfficioOriginale
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeUfficioOriginale
                If (oldValue = value) Then Exit Property
                Me.m_NomeUfficioOriginale = value
                Me.DoChanged("NomeUfficioOriginale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il codice del reparto in cui è stoccato l'articolo se non in uso
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CodiceReparto As String
            Get
                Return Me.m_CodiceReparto
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_CodiceReparto
                If (oldValue = value) Then Exit Property
                Me.m_CodiceReparto = value
                Me.DoChanged("CodiceReparto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il codice dello scaffale in cui è stoccato l'articolo se non in uso
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CodiceScaffale As String
            Get
                Return Me.m_CodiceScaffale
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_CodiceScaffale
                If (oldValue = value) Then Exit Property
                Me.m_CodiceScaffale = value
                Me.DoChanged("CodiceScaffale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di dismissione dell'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataDismissione As Date?
            Get
                Return Me.m_DataDismissione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataDismissione
                If (oldValue = value) Then Exit Property
                Me.m_DataDismissione = value
                Me.DoChanged("DataDismissione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha dismesso l'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DismessoDaID As Integer
            Get
                Return GetID(Me.m_DismessoDa, Me.m_DismessoDaID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.DismessoDaID
                If (oldValue = value) Then Exit Property
                Me.m_DismessoDa = Nothing
                Me.m_DismessoDaID = value
                Me.DoChanged("DismessoDaID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha dismesso l'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DismessoDa As CUser
            Get
                If (Me.m_DismessoDa Is Nothing) Then Me.m_DismessoDa = Sistema.Users.GetItemById(Me.m_DismessoDaID)
                Return Me.m_DismessoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.DismessoDa
                If (oldValue Is value) Then Exit Property
                Me.m_DismessoDa = value
                Me.m_DismessoDaID = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeDismessoDa = value.Nominativo
                Me.DoChanged("DismessoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che ha dismesso l'articolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeDismessoDa As String
            Get
                Return Me.m_NomeDismessoDa
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeDismessoDa
                If (oldValue = value) Then Exit Property
                Me.m_NomeDismessoDa = value
                Me.DoChanged("NomeDismessoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restitusice o imposta il motivo della dismissione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MotivoDismissione As String
            Get
                Return Me.m_MotivoDismissione
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_MotivoDismissione
                If (oldValue = value) Then Exit Property
                Me.m_MotivoDismissione = value
                Me.DoChanged("MotivoDismissione", value, oldValue)
            End Set
        End Property

        Public Property DettaglioDismissione As String
            Get
                Return Me.m_DettaglioDismissione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_DettaglioDismissione
                If (oldValue = value) Then Exit Property
                Me.m_DettaglioDismissione = value
                Me.DoChanged("DettaglioDismissione", value, oldValue)
            End Set
        End Property

        Public Property ValoreDismissione As Decimal?
            Get
                Return Me.m_ValoreDismissione
            End Get
            Set(value As Decimal?)
                Dim oldValue As Decimal? = Me.m_ValoreDismissione
                If (oldValue = value) Then Exit Property
                Me.m_ValoreDismissione = value
                Me.DoChanged("ValoreDismissione", value, oldValue)
            End Set
        End Property

        Public Property AliquotaIVADismissione As Single?
            Get
                Return Me.m_AliquotaIVADismissione
            End Get
            Set(value As Single?)
                Dim oldValue As Single? = Me.m_AliquotaIVADismissione
                If (oldValue = value) Then Exit Property
                Me.m_AliquotaIVADismissione = value
                Me.DoChanged("AliquotaIVADismissione", value, oldValue)
            End Set
        End Property


        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Codice", Me.m_Codice)
            writer.WriteAttribute("IDArticolo", Me.IDArticolo)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("NomeArticolo", Me.m_NomeArticolo)
            writer.WriteAttribute("Categoria", Me.m_Categoria)
            writer.WriteAttribute("Marca", Me.m_Marca)
            writer.WriteAttribute("Modello", Me.m_Modello)
            writer.WriteAttribute("Seriale", Me.m_Seriale)
            writer.WriteAttribute("IconURL", Me.m_IconURL)
            writer.WriteAttribute("StatoAttuale", Me.m_StatoAttuale)
            writer.WriteAttribute("DettaglioStatoAttuale", Me.m_DettaglioStatoAttuale)
            writer.WriteAttribute("ValoreStimato", Me.m_ValoreStimato)
            writer.WriteAttribute("DataValutazione", Me.m_DataValutazione)
            writer.WriteAttribute("TipoInUsoDa", Me.TipoInUsoDa)
            writer.WriteAttribute("IDInUsoDa", Me.IDInUsoDa)
            writer.WriteAttribute("NomeInUsoDa", Me.NomeInUsoDa)
            writer.WriteAttribute("DataProduzione", Me.m_DataProduzione)
            writer.WriteAttribute("DataAcquisto", Me.m_DataAcquisto)
            writer.WriteAttribute("TipoDocumentoAcquisto", Me.m_TipoDocumentoAcquisto)
            writer.WriteAttribute("NumeroDocumentoAcquisto", Me.m_NumeroDocumentoAcquisto)
            writer.WriteAttribute("StatoAcquisto", Me.m_StatoAcquisto)
            writer.WriteAttribute("DettaglioStatoAcquisto", Me.m_DettaglioStatoAcquisto)
            writer.WriteAttribute("AcquistatoDaID", Me.AcquistatoDaID)
            writer.WriteAttribute("NomeAcquistatoDa", Me.m_NomeAcquistatoDa)
            writer.WriteAttribute("PrezzoAcquisto", Me.m_PrezzoAcquisto)
            writer.WriteAttribute("AliquotaIVA", Me.m_AliquotaIVA)
            writer.WriteAttribute("IDUfficioOriginale", Me.IDUfficioOriginale)
            writer.WriteAttribute("NomeUfficioOriginale", Me.m_NomeUfficioOriginale)
            writer.WriteAttribute("CodiceScaffale", Me.m_CodiceScaffale)
            writer.WriteAttribute("CodiceReparto", Me.m_CodiceReparto)
            writer.WriteAttribute("DataDismissione", Me.m_DataDismissione)
            writer.WriteAttribute("DismessoDaID", Me.DismessoDaID)
            writer.WriteAttribute("NomeDismessoDa", Me.m_NomeDismessoDa)
            writer.WriteAttribute("MotivoDismissione", Me.m_MotivoDismissione)
            writer.WriteAttribute("DettaglioDismissione", Me.m_DettaglioDismissione)
            writer.WriteAttribute("ValoreDismissione", Me.m_ValoreDismissione)
            writer.WriteAttribute("AliquotaIVADismissione", Me.m_AliquotaIVADismissione)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("IDOrdineAcquisto", Me.IDOrdineAcquisto)
            writer.WriteAttribute("IDDocumentoAcquisto", Me.IDDocumentoAcquisto)
            writer.WriteAttribute("IDSpedizione", Me.IDSpedizione)
            writer.WriteAttribute("CodiceRFID", Me.m_CodiceRFID)
            writer.WriteAttribute("GSP_REL", Me.m_IsPosizioneGPSRelativa)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Descrizione", Me.m_Descrizione)
            writer.WriteTag("Attributi", Me.Attributi)
            writer.WriteTag("Relazioni", Me.Relazioni)
            writer.WriteTag("PosizioneGPS", Me.m_PosizioneGPS)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Codice" : Me.m_Codice = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeArticolo" : Me.m_NomeArticolo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Categoria" : Me.m_Categoria = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Marca" : Me.m_Marca = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Modello" : Me.m_Modello = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Seriale" : Me.m_Seriale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IconURL" : Me.m_IconURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoAttuale" : Me.m_StatoAttuale = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DettaglioStatoAttuale" : Me.m_DettaglioStatoAttuale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ValoreStimato" : Me.m_ValoreStimato = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "DataValutazione" : Me.m_DataValutazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "TipoInUsoDa" : Me.m_TipoInUsoDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDInUsoDa" : Me.m_IDInUsoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeInUsoDa" : Me.NomeInUsoDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataProduzione" : Me.m_DataProduzione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataAcquisto" : Me.m_DataAcquisto = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "TipoDocumentoAcquisto" : Me.m_TipoDocumentoAcquisto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NumeroDocumentoAcquisto" : Me.m_NumeroDocumentoAcquisto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoAcquisto" : Me.m_StatoAcquisto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DettaglioStatoAcquisto" : Me.m_DettaglioStatoAcquisto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "AcquistatoDaID" : Me.m_AcquistatoDaID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeAcquistatoDa" : Me.m_NomeAcquistatoDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PrezzoAcquisto" : Me.m_PrezzoAcquisto = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "AliquotaIVA" : Me.m_AliquotaIVA = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "IDUfficioOriginale" : Me.m_IDUfficioOriginale = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeUfficioOriginale" : Me.m_NomeUfficioOriginale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CodiceScaffale" : Me.m_CodiceScaffale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CodiceReparto" : Me.m_CodiceReparto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataDismissione" : Me.m_DataDismissione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DismessoDaID" : Me.m_DismessoDaID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeDismessoDa" : Me.m_NomeDismessoDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MotivoDismissione" : Me.m_MotivoDismissione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DettaglioDismissione" : Me.m_DettaglioDismissione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ValoreDismissione" : Me.m_ValoreDismissione = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "AliquotaIVADismissione" : Me.m_AliquotaIVADismissione = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDArticolo" : Me.m_IDArticolo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDOrdineAcquisto" : Me.m_IDOrdineAcquisto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDDocumentoAcquisto" : Me.m_IDDocumentoAcquisto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDSpedizione" : Me.m_IDSpedizione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "CodiceRFID" : Me.m_CodiceRFID = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "GSP_REL" : Me.m_IsPosizioneGPSRelativa = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "PosizioneGPS" : Me.m_PosizioneGPS = CType(fieldValue, GPSRecord)
                Case "Attributi"
                    Me.m_Attributi = fieldValue
                    Me.m_Attributi.SetOggetto(Me)
                Case "Relazioni"
                    Me.m_Relazioni = New OggettiCollegatiCollection
                    Me.m_Relazioni.SetOwner(Me)
                    For Each item As OggettoCollegato In DirectCast(fieldValue, IEnumerable)
                        Me.m_Relazioni.Add(item)
                        If (item.IDOggetto1 = GetID(Me)) Then item.SetOggetto1(Me)
                        If (item.IDOggetto2 = GetID(Me)) Then item.SetOggetto2(Me)
                    Next
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeOggettiInventariati"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Office.OggettiInventariati.Module
        End Function

        Public Overrides Function IsChanged() As Boolean
            Return MyBase.IsChanged() OrElse (Me.m_Attributi IsNot Nothing AndAlso Me.m_Attributi.IsChanged) OrElse (Me.m_PosizioneGPS.IsChanged)
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (Me.m_Attributi IsNot Nothing) Then
                Me.m_Attributi.Save(force)
                MyBase.SaveToDatabase(dbConn, True)
            End If
            Me.m_PosizioneGPS.SetChanged(False)
            Return ret
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Codice = reader.Read("Codice", Me.m_Codice)
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_NomeArticolo = reader.Read("NomeArticolo", Me.m_NomeArticolo)
            Me.m_IDArticolo = reader.Read("IDArticolo", Me.m_IDArticolo)
            Me.m_Categoria = reader.Read("Categoria", Me.m_Categoria)
            Me.m_Marca = reader.Read("Marca", Me.m_Marca)
            Me.m_Modello = reader.Read("Modello", Me.m_Modello)
            Me.m_Seriale = reader.Read("Seriale", Me.m_Seriale)
            Me.m_IconURL = reader.Read("IconURL", Me.m_IconURL)
            Me.m_StatoAttuale = reader.Read("StatoAttuale", Me.m_StatoAttuale)
            Me.m_DettaglioStatoAttuale = reader.Read("DettaglioStatoAttuale", Me.m_DettaglioStatoAttuale)
            Me.m_ValoreStimato = reader.Read("ValoreStimato", Me.m_ValoreStimato)
            Me.m_DataValutazione = reader.Read("DataValutazione", Me.m_DataValutazione)
            Me.m_TipoInUsoDa = reader.Read("TipoInUsoDa", Me.m_TipoInUsoDa)
            Me.m_IDInUsoDa = reader.Read("IDInUsoDa", Me.m_IDInUsoDa)
            Me.m_NomeInUsoDa = reader.Read("NomeInUsoDa", Me.m_NomeInUsoDa)
            Me.m_DataProduzione = reader.Read("DataProduzione", Me.m_DataProduzione)
            Me.m_DataAcquisto = reader.Read("DataAcquisto", Me.m_DataAcquisto)
            Me.m_TipoDocumentoAcquisto = reader.Read("TipoDocumentoAcquisto", Me.m_TipoDocumentoAcquisto)
            Me.m_NumeroDocumentoAcquisto = reader.Read("NumeroDocumentoAcquisto", Me.m_NumeroDocumentoAcquisto)
            Me.m_StatoAcquisto = reader.Read("StatoAcquisto", Me.m_StatoAcquisto)
            Me.m_DettaglioStatoAcquisto = reader.Read("DettaglioStatoAcquisto", Me.m_DettaglioStatoAcquisto)
            Me.m_AcquistatoDaID = reader.Read("AcquistatoDaID", Me.m_AcquistatoDaID)
            Me.m_NomeAcquistatoDa = reader.Read("NomeAcquistatoDa", Me.m_NomeAcquistatoDa)
            Me.m_PrezzoAcquisto = reader.Read("PrezzoAcquisto", Me.m_PrezzoAcquisto)
            Me.m_AliquotaIVA = reader.Read("AliquotaIVA", Me.m_AliquotaIVA)
            Me.m_IDUfficioOriginale = reader.Read("IDUfficioOriginale", Me.m_IDUfficioOriginale)
            Me.m_NomeUfficioOriginale = reader.Read("NomeUfficioOriginale", Me.m_NomeUfficioOriginale)
            Me.m_CodiceScaffale = reader.Read("CodiceScaffale", Me.m_CodiceScaffale)
            Me.m_CodiceReparto = reader.Read("CodiceReparto", Me.m_CodiceReparto)
            Me.m_DataDismissione = reader.Read("DataDismissione", Me.m_DataDismissione)
            Me.m_DismessoDaID = reader.Read("DismessoDaID", Me.m_DismessoDaID)
            Me.m_NomeDismessoDa = reader.Read("NomeDismessoDa", Me.m_NomeDismessoDa)
            Me.m_MotivoDismissione = reader.Read("MotivoDismissione", Me.m_MotivoDismissione)
            Me.m_DettaglioDismissione = reader.Read("DettaglioDismissione", Me.m_DettaglioDismissione)
            Me.m_ValoreDismissione = reader.Read("ValoreDismissione", Me.m_ValoreDismissione)
            Me.m_AliquotaIVADismissione = reader.Read("AliquotaIVADismissione", Me.m_AliquotaIVADismissione)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_IDOrdineAcquisto = reader.Read("IDOrdineAcquisto", Me.m_IDOrdineAcquisto)
            Me.m_IDDocumentoAcquisto = reader.Read("IDDocumentoAcquisto", Me.m_IDDocumentoAcquisto)
            Me.m_IDSpedizione = reader.Read("IDSpedizione", Me.m_IDSpedizione)
            Me.m_CodiceRFID = reader.Read("CodiceRFID", Me.m_CodiceRFID)
            With Me.m_PosizioneGPS
                .Longitudine = reader.Read("GPS_LON", .Longitudine)
                .Latitudine = reader.Read("GPS_LAT", .Latitudine)
                .Altitudine = reader.Read("GPS_ALT", .Altitudine)
                .SetChanged(False)
            End With
            Me.m_IsPosizioneGPSRelativa = reader.Read("GSP_REL", Me.m_IsPosizioneGPSRelativa)

            Dim tmp As String = reader.Read("Attributi", "")
            Try
                Me.m_Attributi = XML.Utils.Serializer.Deserialize(tmp)
                Me.m_Attributi.SetOggetto(Me)
            Catch ex As Exception
                Me.m_Attributi = Nothing
            End Try

            'Me.m_CodiceInterno = reader.Read("CodiceInterno", Me.m_CodiceInterno)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Codice", Me.m_Codice)
            writer.Write("Nome", Me.m_Nome)
            writer.Write("NomeArticolo", Me.m_NomeArticolo)
            writer.Write("IDArticolo", Me.IDArticolo)
            writer.Write("Categoria", Me.m_Categoria)
            writer.Write("Marca", Me.m_Marca)
            writer.Write("Modello", Me.m_Modello)
            writer.Write("Seriale", Me.m_Seriale)
            writer.Write("IconURL", Me.m_IconURL)
            writer.Write("StatoAttuale", Me.m_StatoAttuale)
            writer.Write("DettaglioStatoAttuale", Me.m_DettaglioStatoAttuale)
            writer.Write("ValoreStimato", Me.m_ValoreStimato)
            writer.Write("DataValutazione", Me.m_DataValutazione)
            writer.Write("TipoInUsoDa", Me.TipoInUsoDa)
            writer.Write("IDInUsoDa", Me.IDInUsoDa)
            writer.Write("NomeInUsoDa", Me.NomeInUsoDa)
            writer.Write("DataProduzione", Me.m_DataProduzione)
            writer.Write("DataAcquisto", Me.m_DataAcquisto)
            writer.Write("TipoDocumentoAcquisto", Me.m_TipoDocumentoAcquisto)
            writer.Write("NumeroDocumentoAcquisto", Me.m_NumeroDocumentoAcquisto)
            writer.Write("StatoAcquisto", Me.m_StatoAcquisto)
            writer.Write("DettaglioStatoAcquisto", Me.m_DettaglioStatoAcquisto)
            writer.Write("AcquistatoDaID", Me.AcquistatoDaID)
            writer.Write("NomeAcquistatoDa", Me.m_NomeAcquistatoDa)
            writer.Write("PrezzoAcquisto", Me.m_PrezzoAcquisto)
            writer.Write("AliquotaIVA", Me.m_AliquotaIVA)
            writer.Write("IDUfficioOriginale", Me.IDUfficioOriginale)
            writer.Write("NomeUfficioOriginale", Me.m_NomeUfficioOriginale)
            writer.Write("CodiceScaffale", Me.m_CodiceScaffale)
            writer.Write("CodiceReparto", Me.m_CodiceReparto)
            writer.Write("DataDismissione", Me.m_DataDismissione)
            writer.Write("DismessoDaID", Me.DismessoDaID)
            writer.Write("NomeDismessoDa", Me.m_NomeDismessoDa)
            writer.Write("MotivoDismissione", Me.m_MotivoDismissione)
            writer.Write("DettaglioDismissione", Me.m_DettaglioDismissione)
            writer.Write("ValoreDismissione", Me.m_ValoreDismissione)
            writer.Write("AliquotaIVADismissione", Me.m_AliquotaIVADismissione)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("Attributi", XML.Utils.Serializer.Serialize(Me.Attributi))
            writer.Write("IDOrdineAcquisto", Me.IDOrdineAcquisto)
            writer.Write("IDDocumentoAcquisto", Me.IDDocumentoAcquisto)
            writer.Write("IDSpedizione", Me.IDSpedizione)
            writer.Write("CodiceRFID", Me.m_CodiceRFID)

            With Me.m_PosizioneGPS
                writer.Write("GPS_LON", .Longitudine)
                writer.Write("GPS_LAT", .Latitudine)
                writer.Write("GPS_ALT", .Altitudine)
            End With
            writer.Write("GSP_REL", Me.m_IsPosizioneGPSRelativa)

            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Nome
        End Function

    End Class

End Class


