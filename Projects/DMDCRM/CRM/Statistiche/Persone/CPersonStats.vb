Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica



Partial Public Class CustomerCalls

    <Flags> _
    Public Enum PersonFlags As Integer
        None = 0

        ''' <summary>
        ''' Questo flag indica che la riga è relativa ad una persona fisica
        ''' </summary>
        ''' <remarks></remarks>
        PersonaFisica = 1

        ''' <summary>
        ''' Questo flag indica che la riga è relativa ad una persona da ricontattabile
        ''' </summary>
        ''' <remarks></remarks>
        Ricontattabile = 2

        ''' <summary>
        ''' Questo flag indica che la persona è deceduta
        ''' </summary>
        ''' <remarks></remarks>
        Deceduto = 4

        ''' <summary>
        ''' Flag che indica che si tratta di un cliente non ancora acquisito
        ''' </summary>
        ''' <remarks></remarks>
        ClienteInAquisizione = 8

        ''' <summary>
        ''' Flag che indica che si tratta di un cliente acquisito (almeno una pratica)
        ''' </summary>
        ''' <remarks></remarks>
        ClienteAcquisito = 16
    End Enum

    <Serializable>
    Public Class CPersonStats
        Inherits DBObjectBase

        Private m_IDPersona As Integer
        <NonSerialized> Private m_Persona As CPersona
        Private m_NomePersona As String

        Private m_DettaglioEsito As String
        Private m_DettaglioEsito1 As String

        Private m_ConteggioVisiteRicevute As Integer
        Private m_ConteggioVisiteEffettuate As Integer
        Private m_IDUltimaVisita As Integer
        <NonSerialized> Private m_UltimaVisita As CVisita
        Private m_DataUltimaVisita As Date?

        <NonSerialized> Private m_UltimoContattoNo As CContattoUtente
        Private m_UltimoContattoNoID As Integer
        Private m_DataUltimoContattoNo As Date?
        Private m_ConteggioNoRispo As Integer

        <NonSerialized> Private m_UltimoContattoOk As CContattoUtente
        Private m_UltimoContattoOkID As Integer
        Private m_DataUltimoContattoOk As Nullable(Of Date)
        Private m_ConteggioRisp As Integer

        Private m_Note As String
        Private m_Flags As PersonFlags

        'Private m_ProssimoRicontatto As CRicontatto
        'Private m_ProssimoRicontattoID As Integer
        Private m_DataProssimoRicontatto As Nullable(Of Date)
        Private m_MotivoProssimoRicontatto As String

        Private m_IDPuntoOperativo As Integer
        Private m_NomePuntoOperativo As String
        <NonSerialized> Private m_PuntoOperativo As CUfficio

        Private m_DescrizioneStato As String

        Private m_IconURL As String

        Private m_DataAggiornamento As Date?

        Private m_DataUltimaOperazione As Date?

        Private m_CondizioniAttenzione As CCollection(Of CPersonWatchCond)

        Public Sub New()
            Me.m_IDPersona = 0
            Me.m_Persona = Nothing
            Me.m_NomePersona = ""
            Me.m_DettaglioEsito = ""
            Me.m_DettaglioEsito1 = ""

            Me.m_UltimoContattoNo = Nothing
            Me.m_UltimoContattoNoID = 0
            Me.m_DataUltimoContattoNo = Nothing

            Me.m_IDUltimaVisita = 0
            Me.m_UltimaVisita = Nothing
            Me.m_DataUltimaVisita = Nothing

            Me.m_UltimoContattoOk = Nothing
            Me.m_UltimoContattoOkID = 0
            Me.m_DataUltimoContattoOk = Nothing
            Me.m_ConteggioRisp = 0
            Me.m_ConteggioNoRispo = 0
            Me.m_Flags = PersonFlags.None
            Me.m_NomePersona = ""
            Me.m_DataProssimoRicontatto = Nothing
            Me.m_MotivoProssimoRicontatto = ""

            Me.m_IDPuntoOperativo = 0
            Me.m_NomePuntoOperativo = ""
            Me.m_PuntoOperativo = Nothing

            Me.m_DescrizioneStato = ""

            Me.m_IconURL = ""

            Me.m_DataAggiornamento = Nothing

            Me.m_DataUltimaOperazione = Nothing

            Me.m_ConteggioVisiteRicevute = 0
            Me.m_ConteggioVisiteEffettuate = 0
        End Sub

        ''' <summary>
        ''' Restituisce o imposta una stringa che riporta il valore omonimo impostato per l'oggetto CPersona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DettaglioEsito As String
            Get
                Return Me.m_DettaglioEsito
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_DettaglioEsito
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_DettaglioEsito = value
                Me.DoChanged("DettaglioEsito", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che riporta il valore omonimo impostato per l'oggetto CPersona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DettaglioEsito1 As String
            Get
                Return Me.m_DettaglioEsito1
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_DettaglioEsito1
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_DettaglioEsito1 = value
                Me.DoChanged("DettaglioEsito1", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'ultima visita (effettuata o ricevuta) registrata per questa persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDUltimaVisita As Integer
            Get
                Return GetID(Me.m_UltimaVisita, Me.m_IDUltimaVisita)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDUltimaVisita
                If (oldValue = value) Then Exit Property
                Me.m_IDUltimaVisita = value
                Me.m_UltimaVisita = Nothing
                Me.DoChanged("IDUltimaVisita", value, oldValue)
            End Set
        End Property

        Public Property UltimaVisita As CVisita
            Get
                If (Me.m_UltimaVisita Is Nothing) Then Me.m_UltimaVisita = CustomerCalls.Visite.GetItemById(Me.m_IDUltimaVisita)
                Return Me.m_UltimaVisita
            End Get
            Set(value As CVisita)
                Dim oldValue As CVisita = Me.UltimaVisita
                If (oldValue Is value) Then Exit Property
                Me.m_UltimaVisita = value
                Me.m_IDUltimaVisita = GetID(value)
                If (value IsNot Nothing) Then Me.m_DataUltimaVisita = value.Data
                Me.DoChanged("UltimaVisita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data e l'ora dell'ultima visita ricevuta o effettuata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataUltimaVisita As Date?
            Get
                Return Me.m_DataUltimaVisita
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataUltimaVisita
                If DateUtils.Compare(value, oldValue) = 0 Then Return
                Me.m_DataUltimaVisita = value
                Me.DoChanged("DataUltimaVisita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restitusice la collezione delle condizioni di attenzione registrate per questa anagrafica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CondizioniAttenzione As CCollection(Of CPersonWatchCond)
            Get
                If (Me.m_CondizioniAttenzione Is Nothing) Then Me.m_CondizioniAttenzione = New CCollection(Of CPersonWatchCond)
                Return Me.m_CondizioniAttenzione
            End Get
        End Property

        Public Function GetAttenzione(ByVal source As Object, ByVal tag As String) As CPersonWatchCond
            Dim sType As String = ""
            Dim sID As Integer = 0
            If (source IsNot Nothing) Then
                sType = TypeName(source)
                sID = GetID(source)
            End If
            For Each c As CPersonWatchCond In Me.CondizioniAttenzione
                If c.SourceType = sType AndAlso c.SourceID = sID AndAlso c.Tag = tag Then
                    Return c
                End If
            Next
            Return Nothing
        End Function

        Public Function AggiungiAttenzione(ByVal source As Object, ByVal descrizione As String, ByVal tag As String) As CPersonWatchCond
            Dim c As CPersonWatchCond = Me.GetAttenzione(source, tag)
            Dim hasChanged As Boolean = False
            If (c Is Nothing) Then
                c = New CPersonWatchCond
                c.Source = source
                c.Data = Now
                c.Tag = tag
                Me.CondizioniAttenzione.Add(c)
                hasChanged = True
            End If
            If (c.Descrizione <> Strings.Trim(descrizione)) Then
                hasChanged = True
                c.Descrizione = descrizione
            End If
            If (hasChanged) Then
                Me.DoChanged("CondizioniAttenzione")
            End If
            Return c
        End Function

        Public Function RimuoviAttenzione(ByVal source As Object, ByVal tag As String) As CPersonWatchCond
            Dim sType As String = ""
            Dim sID As Integer = 0
            If (source IsNot Nothing) Then
                sType = TypeName(source)
                sID = GetID(source)
            End If
            Dim c As CPersonWatchCond = Nothing
            Dim i As Integer = 0
            While (i < Me.CondizioniAttenzione.Count)
                c = Me.CondizioniAttenzione(i)
                If c.SourceType = sType AndAlso c.SourceID = sID AndAlso c.Tag = tag Then
                    Me.CondizioniAttenzione.RemoveAt(i)
                    Me.DoChanged("CondizioniAttenzione")
                    Exit While
                End If
                i += 1
            End While
            Return c
        End Function

        ''' <summary>
        ''' Restituisce il valore della gravita maggiore tra le condizioni di attenzione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Gravita As Integer
            Get
                Dim ret As Integer = 0
                For Each c As CPersonWatchCond In Me.CondizioniAttenzione
                    If c.Gravita > ret Then ret = c.Gravita
                Next
                Return ret
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data dell'ultimo aggiornamento di stato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DataUltimaOperazione As Date?
            Get
                Return Me.m_DataUltimaOperazione
            End Get
            'Set(value As Date?)
            '    Dim oldValue As Date? = Me.m_DataUltimaOperazione
            '    If DateUtils.Compare(value, oldValue) = 0 Then Exit Property
            '    Me.m_DataUltimaOperazione = value
            '    Me.DoChanged("DataUltimaOperazione", value, oldValue)
            'End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data dell'ultimo aggiornamento di questo record
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataAggiornamento As Date?
            Get
                Return Me.m_DataAggiornamento
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataAggiornamento
                If DateUtils.Compare(value, oldValue) = 0 Then Exit Property
                Me.m_DataAggiornamento = value
                Me.DoChanged("DataAggiornamento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il percorso dell'immagine associata alla persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IconURL As String
            Get
                Return Me.m_IconURL
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_IconURL
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_IconURL = value
                Me.DoChanged("IconURL", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la prossima data di ricontatto impostata per la persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataProssimoRicontatto As Date?
            Get
                Return Me.m_DataProssimoRicontatto
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataProssimoRicontatto
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_DataProssimoRicontatto = value
                Me.DoChanged("DataProssimoRicontatto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il motivo del prossimo ricontattol
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MotivoProssimoRicontatto As String
            Get
                Return Me.m_MotivoProssimoRicontatto
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_MotivoProssimoRicontatto
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_MotivoProssimoRicontatto = value
                Me.DoChanged("MotivoProssimoRicontatto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta L'ID del punto operativo di appartenenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPuntoOperativo As Integer
            Get
                Return GetID(Me.m_PuntoOperativo, Me.m_IDPuntoOperativo)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPuntoOperativo
                If (oldValue = value) Then Exit Property
                Me.m_IDPuntoOperativo = value
                Me.m_PuntoOperativo = Nothing
                Me.DoChanged("IDPuntoOperativo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il punto operativo di appartenenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PuntoOperativo As CUfficio
            Get
                If (Me.m_PuntoOperativo Is Nothing) Then Me.m_PuntoOperativo = Anagrafica.Uffici.GetItemById(Me.m_IDPuntoOperativo)
                Return Me.m_PuntoOperativo
            End Get
            Set(value As CUfficio)
                Dim oldValue As CUfficio = Me.PuntoOperativo
                If (oldValue Is value) Then Exit Property
                Me.m_PuntoOperativo = value
                Me.m_IDPuntoOperativo = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomePuntoOperativo = value.Nome
                Me.DoChanged("PuntoOperativo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del punto operativo di appartenenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomePuntoOperativo As String
            Get
                Return Me.m_NomePuntoOperativo
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomePuntoOperativo
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomePuntoOperativo = value
                Me.DoChanged("NomePuntoOperativo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomePersona As String
            Get
                Return Me.m_NomePersona
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomePersona
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomePersona = value
                Me.DoChanged("NomePersona", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'elenco dei flags
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As PersonFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As PersonFlags)
                Dim oldValue As PersonFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se la riga è relativa ad una persona fisica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property isPersonaFisica As Boolean
            Get
                Return TestFlag(Me.m_Flags, PersonFlags.PersonaFisica)
            End Get
            Set(value As Boolean)
                If (Me.isPersonaFisica = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, PersonFlags.PersonaFisica, value)
                Me.DoChanged("isPersonaFisica", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se la persona è ricontattabile (non deceduto e non ok ricontatti)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property isRicontattabile As Boolean
            Get
                Return TestFlag(Me.m_Flags, PersonFlags.Ricontattabile)
            End Get
            Set(value As Boolean)
                If (Me.isRicontattabile = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, PersonFlags.Ricontattabile, value)
                Me.DoChanged("isRicontattabile", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se la persona è deceduta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property isDeceduto As Boolean
            Get
                Return TestFlag(Me.m_Flags, PersonFlags.Deceduto)
            End Get
            Set(value As Boolean)
                If (Me.isDeceduto = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, PersonFlags.Deceduto, value)
                Me.DoChanged("isDeceduta", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica che il contatto rappresenta un cliente in fase di acquisizione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property isClienteInAcquisizione As Boolean
            Get
                Return TestFlag(Me.m_Flags, PersonFlags.ClienteInAquisizione)
            End Get
            Set(value As Boolean)
                If (Me.isClienteInAcquisizione = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, PersonFlags.ClienteInAquisizione, value)
                Me.DoChanged("isClienteInAcquisizione", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica che il cliente è acquisito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property isClienteAcquisito As Boolean
            Get
                Return TestFlag(Me.m_Flags, PersonFlags.ClienteAcquisito)
            End Get
            Set(value As Boolean)
                If (Me.isClienteAcquisito = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, PersonFlags.ClienteAcquisito, value)
                Me.DoChanged("isClienteAcquisito", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della persona a cui fa riferimento questo oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDPersona As Integer
            Get
                Return GetID(Me.m_Persona, Me.m_IDPersona)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDPersona
                If (oldValue = value) Then Exit Property
                Me.m_IDPersona = value
                Me.m_Persona = Nothing
                Me.DoChanged("IDPersona", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la persona a cui fa riferimento questo oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Persona As CPersona
            Get
                If (Me.m_Persona Is Nothing) Then Me.m_Persona = Anagrafica.Persone.GetItemById(Me.m_IDPersona)
                Return Me.m_Persona
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona = Me.m_Persona
                If (oldValue Is value) Then Exit Property
                Me.m_Persona = value
                Me.m_IDPersona = GetID(value)
                If (value IsNot Nothing) Then
                    Me.m_NomePersona = value.Nominativo
                    Me.m_IconURL = value.IconURL
                End If
                Me.DoChanged("Persona", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetPersona(ByVal value As CPersona)
            Me.m_Persona = value
            Me.m_IDPersona = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ultimo contatto avuto con la persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UltimoContattoNo As CContattoUtente
            Get
                If (Me.m_UltimoContattoNo Is Nothing) Then Me.m_UltimoContattoNo = CRM.GetItemById(Me.m_UltimoContattoNoID)
                Return Me.m_UltimoContattoNo
            End Get
            Set(value As CContattoUtente)
                Dim oldValue As CContattoUtente = Me.m_UltimoContattoNo
                If (oldValue Is value) Then Exit Property
                Me.m_UltimoContattoNo = value
                Me.m_UltimoContattoNoID = GetID(value)
                Me.DoChanged("UltimoContattoNo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'ultimo contatto avuto con la persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UltimoContattoNoID As Integer
            Get
                Return GetID(Me.m_UltimoContattoNo, Me.m_UltimoContattoNoID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.UltimoContattoNoID
                If (oldValue = value) Then Exit Property
                Me.m_UltimoContattoNo = Nothing
                Me.m_UltimoContattoNoID = value
                Me.DoChanged("UltimoContattoNoID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ultimo contatto con esito positivo avuto con la persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UltimoContattoOk As CContattoUtente
            Get
                If (Me.m_UltimoContattoOk Is Nothing) Then Me.m_UltimoContattoOk = CRM.GetItemById(Me.m_UltimoContattoOkID)
                Return Me.m_UltimoContattoOk
            End Get
            Set(value As CContattoUtente)
                Dim oldValue As CContattoUtente = Me.m_UltimoContattoOk
                If (oldValue Is value) Then Exit Property
                Me.m_UltimoContattoOk = value
                Me.m_UltimoContattoOkID = GetID(value)
                Me.DoChanged("UltimoContattoOk", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'ultimo contatto con esito positivo con la persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UltimoContattoOkID As Integer
            Get
                Return GetID(Me.m_UltimoContattoOk, Me.m_UltimoContattoOkID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.UltimoContattoOkID
                If (oldValue = value) Then Exit Property
                Me.m_UltimoContattoOk = Nothing
                Me.m_UltimoContattoOkID = value
                Me.DoChanged("UltimoContattoOkID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restitusice o imposta la data dell'ultimo contatto con esito positivo avuto con la persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DataUltimoContattoOk As Date?
            Get
                Return Me.m_DataUltimoContattoOk
            End Get
            'Set(value As Date?)
            '    Dim oldValue As Date? = Me.m_DataUltimoContattoOk
            '    If (DateUtils.Compare(oldValue, value) = 0) Then Exit Property
            '    Me.m_DataUltimoContattoOk = value
            '    Me.DoChanged("DataUltimoContattoOk", value, oldValue)
            'End Set
        End Property

        Public ReadOnly Property DataUltimoContattoNo As Date?
            Get
                Return Me.m_DataUltimoContattoNo
            End Get
            'Set(value As Date?)
            '    Dim oldValue As Date? = Me.m_DataUltimoContattoNo
            '    If (DateUtils.Compare(oldValue, value) = 0) Then Exit Property
            '    Me.m_DataUltimoContattoNo = value
            '    Me.DoChanged("DataUltimoContattoNo", value, oldValue)
            'End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data dell'ultimo contatto avuto con la persona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DataUltimoContatto As Date?
            Get
                Return DateUtils.Max(Me.DataUltimoContattoOk, Me.DataUltimoContattoNo)
            End Get
        End Property


        ''' <summary>
        ''' Restituisce o imposta il conteggio dei contatti con esito positivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConteggioRisp As Integer
            Get
                Return Me.m_ConteggioRisp
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_ConteggioRisp
                If (oldValue = value) Then Exit Property
                Me.m_ConteggioRisp = value
                Me.DoChanged("ConteggioRisp", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il conteggio dei contatti con esito non positivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConteggioNoRisp As Integer
            Get
                Return Me.m_ConteggioNoRispo
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_ConteggioNoRispo
                If (oldValue = value) Then Exit Property
                Me.m_ConteggioNoRispo = value
                Me.DoChanged("ConteggioNoRisp", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta delle note aggiuntive
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
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Note = value
                Me.DoChanged("Note", value, oldValue)
            End Set
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return CRM.StatsDB
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_UltimaChiamata"
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Flags", Me.m_Flags)
            writer.Write("IDPersona", Me.IDPersona)
            writer.Write("IDTelefonata", Me.UltimoContattoNoID)
            writer.Write("IDTelefonataOk", Me.UltimoContattoOkID)
            writer.Write("DataUltimaTelefonataOk", Me.m_DataUltimoContattoOk)
            writer.Write("DataUltimaTelefonata", Me.m_DataUltimoContattoNo)
            writer.Write("ConteggioRisp", Me.m_ConteggioRisp)
            writer.Write("ConteggioNoRispo", Me.m_ConteggioNoRispo)
            writer.Write("Note", Me.m_Note)
            writer.Write("NomePersona", Me.m_NomePersona)
            writer.Write("IDPuntoOperativo", Me.IDPuntoOperativo)
            writer.Write("MotivoProssimoRicontatto", Me.m_MotivoProssimoRicontatto)
            writer.Write("DataProssimoRicontatto", Me.m_DataProssimoRicontatto)
            writer.Write("IconURL", Me.m_IconURL)
            writer.Write("DataAggiornamento", Me.m_DataAggiornamento)
            writer.Write("DataUltimaOperazione", Me.m_DataUltimaOperazione)
            writer.Write("Gravita", Me.Gravita)
            writer.Write("Condizioni", XML.Utils.Serializer.Serialize(Me.CondizioniAttenzione))
            writer.Write("ContaCondizioni", Me.CondizioniAttenzione.Count)
            writer.Write("IDUltimaVisita", Me.IDUltimaVisita)
            writer.Write("DataUltimaVisita", Me.m_DataUltimaVisita)
            writer.Write("DataAggiornamento1", DBUtils.ToDBDateStr(Me.m_DataAggiornamento))
            writer.Write("DettaglioEsito", Me.m_DettaglioEsito)
            writer.Write("DettaglioEsito1", Me.m_DettaglioEsito1)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDPersona = reader.Read("IDPersona", Me.m_IDPersona)
            Me.m_UltimoContattoNoID = reader.Read("IDTelefonata", Me.m_UltimoContattoNoID)
            Me.m_UltimoContattoOkID = reader.Read("IDTelefonataOk", Me.m_UltimoContattoOkID)
            Me.m_DataUltimoContattoOk = reader.Read("DataUltimaTelefonataOk", Me.m_DataUltimoContattoOk)
            Me.m_DataUltimoContattoNo = reader.Read("DataUltimaTelefonata", Me.m_DataUltimoContattoNo)
            Me.m_ConteggioRisp = reader.Read("ConteggioRisp", Me.m_ConteggioRisp)
            Me.m_ConteggioNoRispo = reader.Read("ConteggioNoRispo", Me.m_ConteggioNoRispo)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_Note = reader.Read("Note", Me.m_Note)
            Me.m_NomePersona = reader.Read("NomePersona", Me.m_NomePersona)
            Me.m_IDPuntoOperativo = reader.Read("IDPuntoOperativo", Me.m_IDPuntoOperativo)
            Me.m_NomePuntoOperativo = reader.Read("NomePuntoOperativo", Me.m_NomePuntoOperativo)
            Me.m_MotivoProssimoRicontatto = reader.Read("MotivoProssimoRicontatto", Me.m_MotivoProssimoRicontatto)
            Me.m_DataProssimoRicontatto = reader.Read("DataProssimoRicontatto", Me.m_DataProssimoRicontatto)
            Me.m_IconURL = reader.Read("IconURL", Me.m_IconURL)
            Me.m_DataAggiornamento = reader.Read("DataAggiornamento", Me.m_DataAggiornamento)
            Me.m_DataUltimaOperazione = reader.Read("DataUltimaOperazione", Me.m_DataUltimaOperazione)
            Me.m_DataUltimaVisita = reader.Read("DataUltimaVisita", Me.m_DataUltimaVisita)
            Me.m_IDUltimaVisita = reader.Read("IDUltimaVisita", Me.m_IDUltimaVisita)
            Me.m_DettaglioEsito = reader.Read("DettaglioEsito", Me.m_DettaglioEsito)
            Me.m_DettaglioEsito1 = reader.Read("DettaglioEsito1", Me.m_DettaglioEsito1)
            Me.m_CondizioniAttenzione = New CCollection(Of CPersonWatchCond)
            Dim tmp As String = ""
            tmp = reader.Read("Condizioni", tmp)
            If (tmp <> "") Then Me.m_CondizioniAttenzione.AddRange(XML.Utils.Serializer.Deserialize(tmp))

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDPersona", Me.IDPersona)
            writer.WriteAttribute("IDContattoNo", Me.UltimoContattoNoID)
            writer.WriteAttribute("IDContattoOk", Me.UltimoContattoOkID)
            writer.WriteAttribute("DataContattoOk", Me.m_DataUltimoContattoOk)
            writer.WriteAttribute("DataContattoNo", Me.m_DataUltimoContattoNo)
            writer.WriteAttribute("ConteggioRisp", Me.m_ConteggioRisp)
            writer.WriteAttribute("ConteggioNoRispo", Me.m_ConteggioNoRispo)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("NomePersona", Me.m_NomePersona)
            writer.WriteAttribute("IDPuntoOperativo", Me.IDPuntoOperativo)
            writer.WriteAttribute("NomePuntoOperativo", Me.m_NomePuntoOperativo)
            writer.WriteAttribute("MotivoProssimoRicontatto", Me.m_MotivoProssimoRicontatto)
            writer.WriteAttribute("DataProssimoRicontatto", Me.m_DataProssimoRicontatto)
            writer.WriteAttribute("IconURL", Me.m_IconURL)
            writer.WriteAttribute("DataAggiornamento", Me.m_DataAggiornamento)
            writer.WriteAttribute("DataUltimaOperazione", Me.m_DataUltimaOperazione)
            writer.WriteAttribute("IDUltimaVisita", Me.IDUltimaVisita)
            writer.WriteAttribute("DataUltimaVisita", Me.m_DataUltimaVisita)
            writer.WriteAttribute("DettaglioEsito", Me.m_DettaglioEsito)
            writer.WriteAttribute("DettaglioEsito1", Me.m_DettaglioEsito1)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Note", Me.m_Note)
            If writer.Settings.GetValueBool("SerializeContatti") Then
                writer.WriteTag("ContattoOk", Me.UltimoContattoOk)
                writer.WriteTag("ContattoNo", Me.UltimoContattoNo)
            End If
            writer.WriteTag("Condizioni", Me.CondizioniAttenzione)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDPersona" : Me.m_IDPersona = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDContattoNo" : Me.m_UltimoContattoNoID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDContattoOk" : Me.m_UltimoContattoOkID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataContattoOk" : Me.m_DataUltimoContattoOk = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataContattoNo" : Me.m_DataUltimoContattoNo = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ConteggioRisp" : Me.m_ConteggioRisp = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ConteggioNoRispo" : Me.m_ConteggioNoRispo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Note" : Me.m_Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomePersona" : Me.m_NomePersona = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDPuntoOperativo" : Me.m_IDPuntoOperativo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePuntoOperativo" : Me.m_NomePuntoOperativo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "MotivoProssimoRicontatto" : Me.m_MotivoProssimoRicontatto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataProssimoRicontatto" : Me.m_DataProssimoRicontatto = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IconURL" : Me.m_IconURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataAggiornamento" : Me.m_DataAggiornamento = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ContattoOk" : Me.m_UltimoContattoOk = XML.Utils.Serializer.ToObject(fieldValue)
                Case "ContattoNo" : Me.m_UltimoContattoNo = XML.Utils.Serializer.ToObject(fieldValue)
                Case "DataUltimaOperazione" : Me.m_DataUltimaOperazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDUltimaVisita" : Me.m_IDUltimaVisita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataUltimaVisita" : Me.m_DataUltimaVisita = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Condizioni" : Me.m_CondizioniAttenzione = XML.Utils.Serializer.ToObject(fieldValue)
                Case "DettaglioEsito" : Me.m_DettaglioEsito = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DettaglioEsito1" : Me.m_DettaglioEsito1 = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        ''' <summary>
        ''' Aggiorna le informazioni sulla base della persona 
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AggiornaPersona()
            If (Me.Persona Is Nothing) Then Throw New ArgumentNullException("persona")

            Me.PuntoOperativo = Me.Persona.PuntoOperativo
            Me.NomePersona = Me.Persona.Nominativo
            Me.isDeceduto = Me.Persona.Deceduto
            Me.isRicontattabile = Not Me.isDeceduto
            Me.isPersonaFisica = Me.Persona.TipoPersona = TipoPersona.PERSONA_FISICA
            Me.IconURL = Me.Persona.IconURL
            Me.DettaglioEsito = Me.Persona.DettaglioEsito
            Me.DettaglioEsito1 = Me.Persona.DettaglioEsito1

            If Me.isRicontattabile AndAlso Me.Persona.GetFlag(PFlags.CF_CONSENSOADV).HasValue Then
                Me.isRicontattabile = Me.Persona.GetFlag(PFlags.CF_CONSENSOADV).Value
            End If

            'If (Me.Persona.GetFlag(PFlags.Cliente).HasValue AndAlso Me.Persona.GetFlag(PFlags.Cliente).Value = True) Then
            '    Me.isClienteInAcquisizione = False
            '    Me.isClienteAcquisito = True
            'Else

            'End If


        End Sub

        Public Sub AggiornaProssimoRicontatto()
            If (Me.Persona Is Nothing) Then Throw New ArgumentNullException("persona")
            Dim ric As CRicontatto = Anagrafica.Ricontatti.GetProssimoRicontatto(Me.Persona) ', ""
            If (ric Is Nothing) Then
                Me.m_MotivoProssimoRicontatto = ""
                Me.m_DataProssimoRicontatto = Nothing
            Else
                Me.m_MotivoProssimoRicontatto = ric.Note
                Me.m_DataProssimoRicontatto = ric.DataPrevista
            End If
            Me.Save(True)
        End Sub

        Public Sub Ricalcola()
            If (Me.Persona Is Nothing) Then Throw New ArgumentNullException("persona")

            Me.ConteggioNoRisp = 0
            Me.ConteggioRisp = 0
            Me.m_DataUltimoContattoNo = Nothing
            Me.m_DataUltimoContattoOk = Nothing
            Me.UltimoContattoNo = Nothing
            Me.UltimoContattoOk = Nothing
            Me.m_DataProssimoRicontatto = Nothing
            Me.m_MotivoProssimoRicontatto = ""

            Me.AggiornaPersona()

            Dim ric As CRicontatto = Me.GetProssimoRicontatto(Me.Persona)
            If (ric Is Nothing) Then
                Me.m_MotivoProssimoRicontatto = ""
                Me.m_DataProssimoRicontatto = Nothing
            Else
                Me.m_MotivoProssimoRicontatto = ric.Note
                Me.m_DataProssimoRicontatto = ric.DataPrevista
            End If

            Dim dbRis As System.Data.IDataReader
            dbRis = CRM.TelDB.ExecuteReader("SELECT Count(*) As [Cnt], Max([Data]) As [MaxData] FROM [tbl_Telefonate] WHERE [IDPersona]=" & DBUtils.DBNumber(Me.IDPersona) & " AND [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [Esito]=" & DBUtils.DBNumber(EsitoChiamata.RIPOSTA) & " AND ([ClassName]='CTelefonata')")
            If (dbRis.Read) Then
                Me.ConteggioRisp = Formats.ToInteger(dbRis("Cnt"))
                Me.m_DataUltimoContattoOk = Formats.ParseDate(dbRis("MaxData"))
            End If
            dbRis.Dispose()

            dbRis = CRM.TelDB.ExecuteReader("SELECT Count(*) As [Cnt], Max([Data]) As [MaxData] FROM [tbl_Telefonate] WHERE [IDPersona]=" & DBUtils.DBNumber(Me.IDPersona) & " AND [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [Esito]<>" & DBUtils.DBNumber(EsitoChiamata.RIPOSTA) & " AND ([ClassName]='CTelefonata')")
            If (dbRis.Read) Then
                Me.ConteggioNoRisp = Formats.ToInteger(dbRis("Cnt"))
                Me.m_DataUltimoContattoNo = Formats.ParseDate(dbRis("MaxData"))
            End If
            dbRis.Dispose()

            dbRis = CRM.TelDB.ExecuteReader("SELECT [ID] FROM [tbl_Telefonate] WHERE [IDPersona]=" & DBUtils.DBNumber(Me.IDPersona) & " AND [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [Esito]=" & DBUtils.DBNumber(EsitoChiamata.RIPOSTA) & " AND ([ClassName]='CTelefonata') ORDER BY [Data] DESC")
            If (dbRis.Read) Then
                Me.UltimoContattoOkID = Formats.ToInteger(dbRis("ID"))
            End If
            dbRis.Dispose()

            dbRis = CRM.TelDB.ExecuteReader("SELECT [ID] FROM [tbl_Telefonate] WHERE [IDPersona]=" & DBUtils.DBNumber(Me.IDPersona) & " AND [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [Esito]<>" & DBUtils.DBNumber(EsitoChiamata.RIPOSTA) & " AND ([ClassName]='CTelefonata') ORDER BY [Data] DESC")
            If (dbRis.Read) Then
                Me.UltimoContattoNoID = Formats.ToInteger(dbRis("ID"))
            End If
            dbRis.Dispose()

            Me.UltimaVisita = CustomerCalls.Visite.GetUltimaVisita(Me.Persona)

          
            Me.Save(True)
        End Sub

        Public Sub AggiornaOperazione(ByVal source As Object, ByVal descrizione As String)
            Me.m_DataAggiornamento = DateUtils.Now
            Me.m_DataUltimaOperazione = DateUtils.Now
            Me.m_Note = descrizione
            Me.Save(True)
        End Sub

        Public Sub AggiornaContatto(ByVal contatto As CContattoUtente)
            Me.m_DataAggiornamento = DateUtils.Now
            If (TypeOf (contatto) Is CVisita) Then
                Me.UltimaVisita = contatto
            ElseIf (TypeOf (contatto) Is CTelefonata) Then
                Select Case contatto.Esito
                    Case EsitoChiamata.RIPOSTA
                        Me.m_UltimoContattoOk = contatto
                        Me.m_UltimoContattoOkID = GetID(contatto)
                        Me.m_DataUltimoContattoOk = contatto.Data
                        Me.ConteggioRisp += 1
                    Case Else
                        Me.m_UltimoContattoNo = contatto
                        Me.m_UltimoContattoNoID = GetID(contatto)
                        Me.m_DataUltimoContattoNo = contatto.Data
                        Me.ConteggioNoRisp += 1
                End Select
            End If
            
            Me.Save(True)
        End Sub

        Private Function GetProssimoRicontatto(ByVal p As CPersona) As CRicontatto
            Dim cursor As New CRicontattiCursor
            Dim ret As CRicontatto
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.StatoRicontatto.Value = StatoRicontatto.PROGRAMMATO
            cursor.IDPersona.Value = GetID(p)
            cursor.DataPrevista.SortOrder = SortEnum.SORT_ASC
            'cursor.NomeLista.Value = ""
            'cursor.NomeLista.IncludeNulls = True
            cursor.IgnoreRights = True
            ret = cursor.Item
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return ret
        End Function

    End Class



End Class