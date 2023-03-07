Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

#Const usa_tbl_RicontattiQuick = False

Partial Public Class Anagrafica


    Public Enum StatoRicontatto As Integer
        NONPROGRAMMATO = 0
        PROGRAMMATO = 1
        EFFETTUATO = 2
        RIMANDATO = 3
        ANNULLATO = 4
    End Enum

    <Flags> _
    Public Enum RicontattoFlags As Integer
        ''' <summary>
        ''' Nessun flag
        ''' </summary>
        ''' <remarks></remarks>
        None = 0

        ''' <summary>
        ''' Flag che indica che il ricontatto è relativo all'intera giornata piuttosto che ad un orario specifico
        ''' </summary>
        ''' <remarks></remarks>
        GiornataIntera = 1

        ''' <summary>
        ''' Il ricontatto è riservato per il sistema: può essere visualizzato dagli operatori ma non viene mostrato nei campi di default dedicati al ricontatto nell'interfaccia
        ''' </summary>
        ''' <remarks></remarks>
        Reserved = 2048
    End Enum


    <Serializable> _
    Public Class CRicontatto
        Inherits DBObjectPO

        Private m_TipoAppuntamento As String
        Private m_NumeroOIndirizzo As String
        Private m_DataPrevista As Date '[Date] Data ed ora in cui ricontattare
        Private m_IDAssegnatoA As Integer  '[Int]  ID dell'operatore che deve ricontattare
        <NonSerialized> _
        Private m_AssegnatoA As CUser '[CUser]Utente che deve ricontttare
        Private m_NomeAssegnatoA As String '[Text] Nome dell'operatore che deve ricontattare
        Private m_Note As String '[Text] Promemoria per il ricontatto
        Private m_StatoRicontatto As StatoRicontatto '[Int] Valore che indica lo stato del ricontatto
        Private m_DataRicontatto As Nullable(Of Date) '[Date] Data ed ora del ricontatto
        Private m_IDOperatore As Integer      '[Int] ID dell'operatore che ha preso in carico il ricontatto
        <NonSerialized> _
        Private m_Operatore As CUser '[CUser] Operatore che ha preso in carico il ricontatto
        Private m_NomeOperatore As String '[Text] Nome dell'operatore che ha preso in carico il ricontatto
        Private m_TipoContatto As String '[Text] Stringa che indica il tipo di contatto (telefonata, e-mail, ecc...)
        Private m_IDContatto As Integer   '[Int] ID del contatto
        <NonSerialized> _
        Private m_Contatto As CContatto '[CContatto] Oggetto contatto
        Private m_IDPersona As Integer '[ID] id della persona da contattare
        <NonSerialized> _
        Private m_Persona As CPersona '[CPersona] Persona da ricontattare
        Private m_NomePersona As String '[Text] Nominativo della persona da ricontattare
        Private m_SourceName As String '[Text] Nome del server che ha generato l'oggetto
        Private m_SourceParam As String '[Text] Parametri per il server che ha generato l'oggetto
        Private m_Promemoria As Integer 'Promemoria in minuti
        Private m_Flags As RicontattoFlags
        Private m_Categoria As String
        Private m_DettaglioStato As String
        Private m_DettaglioStato1 As String
        Private m_IDRicontattoPrecedente As Integer
        Private m_RicontattoPrecedente As CRicontatto
        Private m_IDRicontattoSuccessivo As Integer
        Private m_RicontattoSuccessivo As CRicontatto
        Private m_Priorita As Integer

        Public Sub New()
            Me.m_DataPrevista = Nothing
            Me.m_IDAssegnatoA = 0
            Me.m_AssegnatoA = Nothing
            Me.m_NomeAssegnatoA = ""
            Me.m_Note = ""
            Me.m_DataRicontatto = Nothing
            Me.m_StatoRicontatto = StatoRicontatto.PROGRAMMATO
            Me.m_IDOperatore = 0
            Me.m_Operatore = Nothing
            Me.m_NomeOperatore = ""
            Me.m_TipoContatto = ""
            Me.m_IDContatto = 0
            Me.m_Contatto = Nothing
            Me.m_IDPersona = 0
            Me.m_Persona = Nothing
            Me.m_NomePersona = ""
            Me.m_SourceName = ""
            Me.m_SourceParam = ""
            Me.m_Promemoria = 0
            Me.m_Flags = 0
            Me.m_Categoria = "Normale"
            Me.m_DettaglioStato = ""
            Me.m_DettaglioStato1 = ""
            Me.m_TipoAppuntamento = "Telefonata"
            Me.m_NumeroOIndirizzo = ""
            Me.m_IDRicontattoPrecedente = 0
            Me.m_RicontattoPrecedente = Nothing
            Me.m_IDRicontattoSuccessivo = 0
            Me.m_RicontattoSuccessivo = Nothing
            Me.m_Priorita = 0
        End Sub

        Public Property Priorita As Integer
            Get
                Return Me.m_Priorita
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Priorita
                If (oldValue = value) Then Exit Property
                Me.m_Priorita = value
                Me.DoChanged("Priorita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del ricontatto precedente nella catena dei ricontatti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDRicontattoPrecedente As Integer
            Get
                Return GetID(Me.m_RicontattoPrecedente, Me.m_IDRicontattoPrecedente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRicontattoPrecedente
                If (oldValue = value) Then Exit Property
                Me.m_IDRicontattoPrecedente = value
                Me.m_RicontattoPrecedente = Nothing
                Me.DoChanged("IDRicontattoPrecedente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il ricontatto precedente nella catena dei ricontatti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RicontattoPrecedente As CRicontatto
            Get
                If (Me.m_RicontattoPrecedente Is Nothing) Then Me.m_RicontattoPrecedente = Anagrafica.Ricontatti.GetItemById(Me.m_IDRicontattoPrecedente)
                Return Me.m_RicontattoPrecedente
            End Get
            Set(value As CRicontatto)
                Dim oldValue As CRicontatto = Me.m_RicontattoPrecedente
                If (oldValue Is value) Then Exit Property
                Me.m_RicontattoPrecedente = value
                Me.m_IDRicontattoPrecedente = GetID(value)
                Me.DoChanged("RicontattoPrecedente", value, oldValue)
            End Set
        End Property



        ''' <summary>
        ''' Restituisce o imposta l'ID del ricontatto successivo nella catena dei ricontatti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDRicontattoSuccessivo As Integer
            Get
                Return GetID(Me.m_RicontattoSuccessivo, Me.m_IDRicontattoSuccessivo)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRicontattoSuccessivo
                If (oldValue = value) Then Exit Property
                Me.m_IDRicontattoSuccessivo = value
                Me.m_RicontattoSuccessivo = Nothing
                Me.DoChanged("IDRicontattoSuccessivo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il ricontatto Successivo nella catena dei ricontatti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RicontattoSuccessivo As CRicontatto
            Get
                If (Me.m_RicontattoSuccessivo Is Nothing) Then Me.m_RicontattoSuccessivo = Anagrafica.Ricontatti.GetItemById(Me.m_IDRicontattoSuccessivo)
                Return Me.m_RicontattoSuccessivo
            End Get
            Set(value As CRicontatto)
                Dim oldValue As CRicontatto = Me.m_RicontattoSuccessivo
                If (oldValue Is value) Then Exit Property
                Me.m_RicontattoSuccessivo = value
                Me.m_IDRicontattoSuccessivo = GetID(value)
                Me.DoChanged("RicontattoSuccessivo", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta il tipo di appuntamento desiderato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoAppuntamento As String
            Get
                Return Me.m_TipoAppuntamento
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TipoAppuntamento
                If (oldValue = value) Then Exit Property
                Me.m_TipoAppuntamento = value
                Me.DoChanged("TipoAppuntamento", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero o l'indirizzo fissato per l'appuntamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumeroOIndirizzo As String
            Get
                Return Me.m_NumeroOIndirizzo
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NumeroOIndirizzo
                If (oldValue = value) Then Exit Property
                Me.m_NumeroOIndirizzo = value
                Me.DoChanged("NumeroOIndirizzo", value, oldValue)
            End Set
        End Property



        ''' <summary>
        ''' Restituisce o imposta la categoria del ricontatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Categoria As String
            Get
                Return Me.m_Categoria
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Categoria
                If (Me.m_Categoria = value) Then Exit Property
                Me.m_Categoria = value
                Me.DoChanged("Categoria", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta delle informazioni aggiuntive sull'esito del ricontatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DettaglioStato As String
            Get
                Return Me.m_DettaglioStato
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_DettaglioStato
                If (oldValue = value) Then Exit Property
                Me.m_DettaglioStato = value
                Me.DoChanged("DettaglioStato", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta delle informazioni aggiuntive sull'esito del ricontatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DettaglioStato1 As String
            Get
                Return Me.m_DettaglioStato1
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_DettaglioStato1
                If (oldValue = value) Then Exit Property
                Me.m_DettaglioStato1 = value
                Me.DoChanged("DettaglioStato1", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetModule() As CModule
            Return Ricontatti.Module
        End Function



        ''' <summary>
        ''' Restituisce o imposta la data prevista per il ricontatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataPrevista As Date
            Get
                Return Me.m_DataPrevista
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_DataPrevista
                If (oldValue = value) Then Exit Property
                Me.m_DataPrevista = value
                Me.DoChanged("DataPrevista", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il promemoria in minuti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Promemoria As Integer
            Get
                Return Me.m_Promemoria
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Promemoria
                If (oldValue = value) Then Exit Property
                Me.m_Promemoria = value
                Me.DoChanged("Promemoria", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il ricontatto deve essere effettuato ad un determinato orario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GiornataIntera As Boolean
            Get
                Return TestFlag(Me.m_Flags, RicontattoFlags.GiornataIntera)
            End Get
            Set(value As Boolean)
                If (Me.GiornataIntera = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, RicontattoFlags.GiornataIntera, value)
                Me.DoChanged("GiornataIntera", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta dei flags aggiuntivi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Flags As RicontattoFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As RicontattoFlags)
                Dim oldValue As RicontattoFlags = Me.m_Flags
                If (oldValue = value) Then Exit Property
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'operatore che deve effettuare il ricontatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDAssegnatoA As Integer
            Get
                Return GetID(Me.m_AssegnatoA, Me.m_IDAssegnatoA)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAssegnatoA
                If (oldValue = value) Then Exit Property
                Me.m_IDAssegnatoA = value
                Me.m_AssegnatoA = Nothing
                Me.DoChanged("IDAssegnatoA", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'operatore che deve effettuare il ricontatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AssegnatoA As CUser
            Get
                If (Me.m_AssegnatoA Is Nothing) Then Me.m_AssegnatoA = Sistema.Users.GetItemById(Me.m_IDAssegnatoA)
                Return Me.m_AssegnatoA
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_AssegnatoA
                If (oldValue = value) Then Exit Property
                Me.m_AssegnatoA = value
                Me.m_IDAssegnatoA = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeAssegnatoA = value.Nominativo
                Me.DoChanged("AssegnatoA", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'operatore che deve effettuare il ricontatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeAssegnatoA As String
            Get
                Return Me.m_NomeAssegnatoA
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomeAssegnatoA
                If (oldValue = value) Then Exit Property
                Me.m_NomeAssegnatoA = value
                Me.DoChanged("NomeAssegnatoA", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta delle annotazioni
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
        ''' Restituisce o imposta la data in cui è stato effettuato il ricontatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataRicontatto As Nullable(Of Date)
            Get
                Return Me.m_DataRicontatto
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataRicontatto
                If (oldValue = value) Then Exit Property
                Me.m_DataRicontatto = value
                Me.DoChanged("DataRicontatto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato del ricontatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoRicontatto As StatoRicontatto
            Get
                Return Me.m_StatoRicontatto
            End Get
            Set(value As StatoRicontatto)
                Dim oldValue As StatoRicontatto = Me.m_StatoRicontatto
                If (oldValue = value) Then Exit Property
                Me.m_StatoRicontatto = value
                Me.DoChanged("StatoRicontatto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'operatore che ha effettuato il ricontatto
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
                If oldValue = value Then Exit Property
                Me.m_IDOperatore = value
                Me.m_Operatore = Nothing
                Me.DoChanged("IDOperatore", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetPersona(ByVal value As CPersona)
            Me.m_Persona = value
            Me.m_IDPersona = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'operatore che ha effettuato il ricontatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Operatore As CUser
            Get
                If (Me.m_Operatore Is Nothing) Then Me.m_Operatore = Sistema.Users.GetItemById(Me.m_IDOperatore)
                Return Me.m_Operatore
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_Operatore
                If (oldValue = value) Then Exit Property
                Me.m_Operatore = value
                Me.m_IDOperatore = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeOperatore = value.Nominativo
                Me.DoChanged("Operatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'operatore che ha effettuato il ricontatto
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
        ''' Restituisce o imposta il tipo del contatto generato a partire da questo ricontatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoContatto As String
            Get
                Return Me.m_TipoContatto
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_TipoContatto
                If (oldValue = value) Then Exit Property
                Me.m_TipoContatto = value
                Me.DoChanged("TipoContatto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del contatto generato da questo ricontatto
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
                Me.m_IDContatto = value
                Me.m_Contatto = Nothing
                Me.DoChanged("IDContatto", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta l'ID della persona associata
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
                If oldValue = value Then Exit Property
                Me.m_Persona = Nothing
                Me.m_IDPersona = value
                Me.DoChanged("IDPersona", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la persona associata
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
                If (oldValue = value) Then Exit Property
                Me.m_Persona = value
                Me.m_IDPersona = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomePersona = value.Nominativo
                Me.DoChanged("Persona", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nominativo della persona associata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomePersona As String
            Get
                Return Me.m_NomePersona
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_NomePersona
                If (oldValue = value) Then Exit Property
                Me.m_NomePersona = value
                Me.DoChanged("NomePersona", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restitusice o imposta una stringa che indica la sorgente che ha creato questo ricontatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SourceName As String
            Get
                Return Me.m_SourceName
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_SourceName
                If (oldValue = value) Then Exit Property
                Me.m_SourceName = value
                Me.DoChanged("SourceName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restitusice o imposta una stringa che identifica il contesto in cui la sorgente ha creato questo ricontatto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SourceParam As String
            Get
                Return Me.m_SourceParam
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_SourceParam
                If (oldValue = value) Then Exit Property
                Me.m_SourceParam = value
                Me.DoChanged("SourceParam", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_Ricontatti"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_DataPrevista = reader.Read("DataPrevista", Me.m_DataPrevista)
            Me.m_IDAssegnatoA = reader.Read("IDAssegnatoA", Me.m_IDAssegnatoA)
            Me.m_NomeAssegnatoA = reader.Read("NomeAssegnatoA", Me.m_NomeAssegnatoA)
            Me.m_Note = reader.Read("Note", Me.m_Note)
            Me.m_DataRicontatto = reader.Read("DataRicontatto", Me.m_DataRicontatto)
            Me.m_StatoRicontatto = reader.Read("StatoRicontatto", Me.m_StatoRicontatto)
            Me.m_IDOperatore = reader.Read("IDOperatore", Me.m_IDOperatore)
            Me.m_NomeOperatore = reader.Read("NomeOperatore", Me.m_NomeOperatore)
            Me.m_TipoContatto = reader.Read("TipoContatto", Me.m_TipoContatto)
            Me.m_IDContatto = reader.Read("IDContatto", Me.m_IDContatto)
            Me.m_IDPersona = reader.Read("IDPersona", Me.m_IDPersona)
            Me.m_NomePersona = reader.Read("NomePersona", Me.m_NomePersona)
            Me.m_SourceName = reader.Read("SourceName", Me.m_SourceName)
            Me.m_SourceParam = reader.Read("SourceParam", Me.m_SourceParam)
            Me.m_Promemoria = reader.Read("Promemoria", Me.m_Promemoria)
            Me.m_Flags = reader.Read("NFlags", Me.m_Flags)
            Me.m_Categoria = reader.Read("Categoria", Me.m_Categoria)
            Me.m_DettaglioStato = reader.Read("DettaglioStato", Me.m_DettaglioStato)
            Me.m_DettaglioStato1 = reader.Read("DettaglioStato1", Me.m_DettaglioStato1)
            Me.m_TipoAppuntamento = reader.Read("TipoAppuntamento", Me.m_TipoAppuntamento)
            Me.m_NumeroOIndirizzo = reader.Read("NumeroOIndirizzo", Me.m_NumeroOIndirizzo)
            Me.m_Priorita = reader.Read("Priorita", Me.m_Priorita)

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            Select Case Me.Categoria
                Case "Urgente" : Me.Priorita = -10
                Case "Importante" : Me.Priorita = -5
                Case "Normale" : Me.Priorita = 0
                Case "Poco importante" : Me.Priorita = 10
            End Select

            writer.Write("DataPrevista", Me.m_DataPrevista)
            writer.Write("DataPrevistaStr", DBUtils.ToDBDateStr(Me.m_DataPrevista))
            writer.Write("IDAssegnatoA", Me.IDAssegnatoA)
            writer.Write("NomeAssegnatoA", Me.m_NomeAssegnatoA)
            writer.Write("Note", Me.m_Note)
            writer.Write("DataRicontatto", Me.m_DataRicontatto)
            writer.Write("StatoRicontatto", Me.m_StatoRicontatto)
            writer.Write("IDOperatore", Me.IDOperatore)
            writer.Write("NomeOperatore", Me.m_NomeOperatore)
            writer.Write("TipoContatto", Me.m_TipoContatto)
            writer.Write("IDContatto", Me.IDContatto)
            writer.Write("IDPersona", Me.IDPersona)
            writer.Write("NomePersona", Me.m_NomePersona)
            writer.Write("SourceName", Me.m_SourceName)
            writer.Write("SourceParam", Me.m_SourceParam)
            writer.Write("Promemoria", Me.m_Promemoria)
            writer.Write("NFlags", Me.m_Flags)
            writer.Write("Categoria", Me.m_Categoria)
            writer.Write("DettaglioStato", Me.m_DettaglioStato)
            writer.Write("DettaglioStato1", Me.m_DettaglioStato1)
            writer.Write("TipoAppuntamento", Me.m_TipoAppuntamento)
            writer.Write("NumeroOIndirizzo", Me.m_NumeroOIndirizzo)
            writer.Write("IDRicPrec", Me.IDRicontattoPrecedente)
            writer.Write("IDRicSucc", Me.IDRicontattoSuccessivo)
            writer.Write("Priorita", Me.m_Priorita)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return "Ricontattare " & Me.NomePersona & " il " & Me.DataPrevista
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            Me.MirrorMe(dbConn)
            Return ret
        End Function

        Protected Overridable Sub MirrorMe(ByVal dbConn As CDBConnection)
            If (dbConn.IsRemote) Then

            Else
#If usa_tbl_RicontattiQuick Then
                dbConn.ExecuteCommand("DELETE * FROM [tbl_RicontattiQuick] WHERE ID=" & GetID(Me))
                If (Me.Stato = ObjectStatus.OBJECT_VALID) AndAlso (Me.StatoRicontatto = Anagrafica.StatoRicontatto.PROGRAMMATO OrElse Me.StatoRicontatto = Anagrafica.StatoRicontatto.RIMANDATO) Then
                    Dim table As CDBTable = dbConn.Tables("tbl_RicontattiQuick")
                    Dim fieldPart As New System.Text.StringBuilder
                    For Each f As CDBEntityField In table.Fields
                        If (fieldPart.Length > 0) Then fieldPart.Append(",")
                        fieldPart.Append("[")
                        fieldPart.Append(f.Name)
                        fieldPart.Append("]")
                    Next

                    Dim cmd As New System.Text.StringBuilder
                    cmd.Append("INSERT INTO [tbl_RicontattiQuick] (")
                    cmd.Append(fieldPart.ToString)
                    cmd.Append(") SELECT * FROM [tbl_Ricontatti] WHERE [ID]=")
                    cmd.Append(GetID(Me))
                    dbConn.ExecuteCommand(cmd.ToString)
                End If
#End If
            End If
        End Sub

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("DataPrevista", Me.m_DataPrevista)
            writer.WriteAttribute("IDAssegnatoA", Me.IDAssegnatoA)
            writer.WriteAttribute("NomeAssegnatoA", Me.m_NomeAssegnatoA)
            writer.WriteAttribute("DataRicontatto", Me.m_DataRicontatto)
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("NomeOperatore", Me.m_NomeOperatore)
            writer.WriteAttribute("TipoContatto", Me.m_TipoContatto)
            writer.WriteAttribute("IDContatto", Me.IDContatto)
            writer.WriteAttribute("IDPersona", Me.IDPersona)
            writer.WriteAttribute("NomePersona", Me.m_NomePersona)
            writer.WriteAttribute("SourceName", Me.m_SourceName)
            writer.WriteAttribute("SourceParam", Me.m_SourceParam)
            writer.WriteAttribute("Promemoria", Me.m_Promemoria)
            writer.WriteAttribute("NFlags", Me.m_Flags)
            writer.WriteAttribute("Categoria", Me.m_Categoria)
            writer.WriteAttribute("StatoRicontatto", Me.m_StatoRicontatto)
            writer.WriteAttribute("DettaglioStato", Me.m_DettaglioStato)
            writer.WriteAttribute("DettaglioStato1", Me.m_DettaglioStato1)
            writer.WriteAttribute("TipoAppuntamento", Me.m_TipoAppuntamento)
            writer.WriteAttribute("NumeroOIndirizzo", Me.m_NumeroOIndirizzo)
            writer.WriteAttribute("IDRicPrec", Me.IDRicontattoPrecedente)
            writer.WriteAttribute("IDRicSucc", Me.IDRicontattoSuccessivo)
            writer.WriteAttribute("Priorita", Me.m_Priorita)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Note", Me.m_Note)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "DataPrevista" : Me.m_DataPrevista = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDAssegnatoA" : Me.m_IDAssegnatoA = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    'Set m_AssegnatoA = Nothing
                Case "NomeAssegnatoA" : Me.m_NomeAssegnatoA = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Note" : Me.m_Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataRicontatto" : Me.m_DataRicontatto = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "StatoRicontatto" : Me.m_StatoRicontatto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDOperatore" : Me.m_IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatore" : Me.m_NomeOperatore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoContatto" : Me.m_TipoContatto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDContatto" : Me.m_IDContatto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    'Set m_Contatto = Nothing
                Case "IDPersona" : Me.m_IDPersona = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                    'Set m_Persona = Nothing
                Case "NomePersona" : Me.m_NomePersona = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SourceName" : Me.m_SourceName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SourceParam" : Me.m_SourceParam = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Promemoria" : Me.m_Promemoria = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NFlags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Categoria" : Me.m_Categoria = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoRicontatto" : Me.m_StatoRicontatto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DettaglioStato" : Me.m_DettaglioStato = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DettaglioStato1" : Me.m_DettaglioStato1 = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoAppuntamento" : Me.m_TipoAppuntamento = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NumeroOIndirizzo" : Me.m_NumeroOIndirizzo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDRicPrec" : Me.m_IDRicontattoPrecedente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDRicSucc" : Me.m_IDRicontattoSuccessivo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Priorita" : Me.m_Priorita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function



        'Protected Overridable Sub ProgrammaNotifica()
        '    If ( _
        '        Me.Operatore IsNot Nothing AndAlso _
        '        Me.Stato = ObjectStatus.OBJECT_VALID AndAlso _
        '        (Me.StatoRicontatto = Anagrafica.StatoRicontatto.PROGRAMMATO OrElse Me.StatoRicontatto = Anagrafica.StatoRicontatto.RIMANDATO) AndAlso _
        '        Me.Promemoria > 0 _
        '        ) Then
        '        Dim notifiche As CCollection(Of Notifica) = Sistema.Notifiche.GetAlertsBySource(Me, "")
        '        Dim notifica As Notifica
        '        Dim text As String = Me.DescrizioneTipoRicontatto & vbNewLine & "Ora prevista: " & Formats.FormatUserDateTime(Me.DataPrevista) & vbNewLine & "Persona: " & Me.NomePersona & vbNewLine & Me.Note
        '        Dim data As Date = Calendar.DateAdd(DateInterval.Minute, -Me.Promemoria, Me.DataPrevista)
        '        If (notifiche.Count > 0) Then
        '            notifica = notifiche(0)
        '            If (notifica.StatoNotifica <= StatoNotifica.CONSEGNATA) Then
        '                notifica.Descrizione = text
        '                notifica.Data = data
        '                notifica.Target = Me.Operatore
        '                notifica.Save()
        '            End If
        '        Else
        '            notifica = Sistema.Notifiche.ProgramAlert(Me.Operatore, text, data, Me, "Promemoria " & Me.TipoAppuntamento)
        '        End If
        '    Else
        '        Sistema.Notifiche.CancelPendingAlertsBySource(Nothing, Me, "Promemoria " & Me.TipoAppuntamento)
        '    End If
        'End Sub

        Protected Overrides Sub OnCreate(e As SystemEvent)
            MyBase.OnCreate(e)
            Me.NotifyCreated()
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            MyBase.OnDelete(e)
            Me.NotifyDeleted()
        End Sub

        Protected Overrides Sub OnModified(e As SystemEvent)
            MyBase.OnModified(e)
            Me.NotifyModified()
        End Sub

        Protected Overridable Sub NotifyCreated()
            Anagrafica.Ricontatti.doRicontattoCreated(New RicontattoEventArgs(Me))
        End Sub

        Protected Overridable Sub NotifyDeleted()
            Anagrafica.Ricontatti.doRicontattoDeleted(New RicontattoEventArgs(Me))
        End Sub

        Protected Overridable Sub NotifyModified()
            Anagrafica.Ricontatti.doRicontattoModified(New RicontattoEventArgs(Me))
        End Sub


        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            'Me.ProgrammaNotifica()
        End Sub

        Public Overridable ReadOnly Property DescrizioneTipoRicontatto As String
            Get
                Select Case LCase(Me.TipoAppuntamento)
                    Case "telefonata" : Return "Appuntamento Telefonico"
                    Case "appuntamento" : Return "Appuntamento"
                    Case Else : Return "Altro tipo di appuntamento"
                End Select
            End Get
        End Property

    End Class


End Class