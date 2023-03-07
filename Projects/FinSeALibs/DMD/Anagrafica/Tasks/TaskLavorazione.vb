Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    ''' <summary>
    ''' Rappresenta uno stato di lavorazione
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class TaskLavorazione
        Inherits DBObjectPO

        Private m_IDCliente As Integer
        Private m_Cliente As CPersona
        Private m_NomeCliente As String

        Private m_IDStatoAttuale As Integer
        Private m_StatoAttuale As StatoTaskLavorazione

        Private m_DataAssegnazione As Date
        Private m_IDAssegnatoDa As Integer
        Private m_AssegnatoDa As CUser
        
        Private m_DataPrevista As Nullable(Of Date)
        Private m_IDAssegnatoA As Integer
        Private m_AssegnatoA As CUser
        Private m_Note As String

        Private m_IDAzioneEseguita As Integer
        'Private m_AzioneEseguita As AzioneTaskLavorazione

        Private m_Categoria As String       'Nome della categoria a cui appartiene il task
        Private m_Priorita As Integer       'Priorità (crescente più importante)

        Private m_IDSorgente As Integer     'ID dell'oggetto da cui è partito il task
        Private m_TipoSorgente As String    'Tipo dell'oggetto da cui è partito il task
        Private m_Sorgente As Object        'Oggetto da cui è partito il task

        Private m_IDContesto As Integer     'ID del contesto in cui è stato generato il task
        Private m_TipoContesto As String    'Tipo del contesto in cui è stato generato il task

        Private m_IDRegolaEseguita As Integer 'ID della regola la cui esecuzione ha portato allo stato corrente
        Private m_RegolaEseguita As RegolaTaskLavorazione   'Regola la cui esecuzione ha portato allo stato corrente
        Private m_ParametriAzione As String
        Private m_RisultatoAzione As String
        Private m_DataInizioEsecuzione As Nullable(Of Date)
        Private m_DataFineEsecuzione As Nullable(Of Date)
        Private m_IDTaskSorgente As Integer
        Private m_TaskSorgente As TaskLavorazione
        Private m_IDTaskDestinazione As Integer
        Private m_TaskDestinazione As TaskLavorazione

        Public Sub New()
            Me.m_IDCliente = 0
            Me.m_Cliente = Nothing
            Me.m_NomeCliente = ""

            Me.m_IDStatoAttuale = 0
            Me.m_StatoAttuale = Nothing

            Me.m_DataAssegnazione = Calendar.Now
            Me.m_IDAssegnatoDa = 0
            Me.m_AssegnatoDa = Nothing

            Me.m_DataPrevista = Nothing
            Me.m_IDAssegnatoA = 0
            Me.m_AssegnatoA = Nothing
            Me.m_Note = ""

            Me.m_IDAzioneEseguita = 0
            'Me.m_AzioneEseguita = Nothing

            Me.m_Categoria = ""
            Me.m_Priorita = 0

            Me.m_IDSorgente = 0
            Me.m_TipoSorgente = ""
            Me.m_Sorgente = Nothing

            Me.m_IDContesto = 0
            Me.m_TipoContesto = ""

            Me.m_IDRegolaEseguita = 0
            Me.m_RegolaEseguita = Nothing

            Me.m_ParametriAzione = ""
            Me.m_RisultatoAzione = ""
            Me.m_DataInizioEsecuzione = Nothing
            Me.m_DataFineEsecuzione = Nothing
            Me.m_IDTaskDestinazione = 0
            Me.m_IDTaskSorgente = 0
        End Sub

        ''' <summary>
        ''' Restituisce o imposta i parametri passati all'azione per eseguire il task
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ParametriAzione As String
            Get
                Return Me.m_ParametriAzione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_ParametriAzione
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_ParametriAzione = value
                Me.DoChanged("ParametriAzione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il risultato dell'esecuzione dell'azione sul task
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RisultatoAzione As String
            Get
                Return Me.m_RisultatoAzione
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_RisultatoAzione
                If (oldValue = value) Then Exit Property
                Me.m_RisultatoAzione = value
                Me.DoChanged("RisultatoAzione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di esecuzione dell'azione sul task
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataInizioEsecuzione As Date?
            Get
                Return Me.m_DataInizioEsecuzione
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_DataInizioEsecuzione
                If Calendar.Compare(oldValue, value) = 0 Then Exit Property
                Me.m_DataInizioEsecuzione = value
                Me.DoChanged("DataInizioEsecuzione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di fine esecuzione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataFineEsecuzione As Nullable(Of Date)
            Get
                Return Me.m_DataFineEsecuzione
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataFineEsecuzione
                If Calendar.Compare(value, oldValue) = 0 Then Exit Property
                Me.m_DataFineEsecuzione = value
                Me.DoChanged("DataFineEsecuzione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del task di provenienza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDTaskSorgente As Integer
            Get
                Return GetID(Me.m_TaskSorgente, Me.m_IDTaskSorgente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDTaskSorgente
                If oldValue = value Then Exit Property
                Me.m_IDTaskSorgente = value
                Me.m_TaskSorgente = Nothing
                Me.DoChanged("IDTaskSorgente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il task di provenienza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TaskSorgente As TaskLavorazione
            Get
                If (Me.m_TaskSorgente Is Nothing) Then Me.m_TaskSorgente = Anagrafica.TasksDiLavorazione.GetItemById(Me.m_IDTaskSorgente)
                Return Me.m_TaskSorgente
            End Get
            Set(value As TaskLavorazione)
                Dim oldValue As TaskLavorazione = Me.TaskSorgente
                If (oldValue Is value) Then Exit Property
                Me.m_TaskSorgente = value
                Me.m_IDTaskSorgente = GetID(value)
                Me.DoChanged("TaskSorgente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del task di destinazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDTaskDestinazione As Integer
            Get
                Return GetID(Me.m_TaskDestinazione, Me.m_IDTaskDestinazione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDTaskDestinazione
                If oldValue = value Then Exit Property
                Me.m_IDTaskDestinazione = value
                Me.m_TaskDestinazione = Nothing
                Me.DoChanged("TaskDestinazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il task di destinazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TaskDestinazione As TaskLavorazione
            Get
                If (Me.m_TaskDestinazione Is Nothing) Then Me.m_TaskDestinazione = Anagrafica.TasksDiLavorazione.GetItemById(Me.m_IDTaskSorgente)
                Return Me.m_TaskDestinazione
            End Get
            Set(value As TaskLavorazione)
                Dim oldValue As TaskLavorazione = Me.TaskDestinazione
                If (oldValue Is value) Then Exit Property
                Me.m_TaskDestinazione = value
                Me.m_IDTaskDestinazione = GetID(value)
                Me.DoChanged("TaskDestinazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID della regola la cui esecuzione ha portato allo stato corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDRegolaEseguita As Integer
            Get
                Return GetID(Me.m_RegolaEseguita, Me.m_IDRegolaEseguita)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRegolaEseguita
                If (oldValue = value) Then Exit Property
                Me.m_RegolaEseguita = Nothing
                Me.m_IDRegolaEseguita = value
                Me.DoChanged("IDRegolaEseguita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la regola la cui esecuzione ha portato allo stato corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RegolaEseguita As RegolaTaskLavorazione
            Get
                If (Me.m_RegolaEseguita Is Nothing) Then Me.m_RegolaEseguita = Anagrafica.RegoleTasksLavorazione.GetItemById(Me.m_IDRegolaEseguita)
                Return Me.m_RegolaEseguita
            End Get
            Set(value As RegolaTaskLavorazione)
                Dim oldValue As RegolaTaskLavorazione = Me.RegolaEseguita
                If (oldValue Is value) Then Exit Property
                Me.m_RegolaEseguita = value
                Me.m_IDRegolaEseguita = GetID(value)
                Me.DoChanged("RegolaEseguita", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la categoria a cui appartiene il task 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Categoria As String
            Get
                Return Me.m_Categoria
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Categoria
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Categoria = value
                Me.DoChanged("Categoria", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un numero che indica la priorità crescente assegnata al task
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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
        ''' Restituisce o imposta l'ID dell'oggetto da cui è partito il task
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDSorgente As Integer
            Get
                Return GetID(Me.m_Sorgente, Me.m_IDSorgente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDSorgente
                If (oldValue = value) Then Exit Property
                Me.m_IDSorgente = value
                Me.m_Sorgente = Nothing
                Me.DoChanged("IDSorgente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo dell'oggetto che ha generato il task
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoSorgente As String
            Get
                If (Me.m_Sorgente IsNot Nothing) Then Return TypeName(Me.m_Sorgente)
                Return Me.m_TipoSorgente
            End Get
            Set(value As String)
                Dim oldValue As String = Me.TipoSorgente
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_TipoSorgente = value
                Me.DoChanged("TipoSorgente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'oggetto che ha generato il task
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Sorgente As Object
            Get
                If (Me.m_Sorgente Is Nothing) Then Me.m_Sorgente = Sistema.Types.GetItemByTypeAndId(Me.m_TipoSorgente, Me.m_IDSorgente)
                Return Me.m_Sorgente
            End Get
            Set(value As Object)
                Dim oldValue As Object = Me.Sorgente
                If (Object.ReferenceEquals(value, oldValue)) Then Exit Property
                Me.m_Sorgente = value
                Me.m_IDSorgente = GetID(value)
                If (value Is Nothing) Then
                    Me.m_TipoSorgente = ""
                Else
                    Me.m_TipoSorgente = TypeName(value)
                End If
                Me.DoChanged("Sorgente", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del contesto in cui è stato creato il task
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDContesto As Integer
            Get
                Return Me.m_IDContesto
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDContesto
                If (oldValue = value) Then Exit Property
                Me.m_IDContesto = value
                Me.DoChanged("IDContesto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo del contesto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoContesto As String
            Get
                Return Me.m_TipoContesto
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_TipoContesto
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_TipoContesto = value
                Me.DoChanged("TipoContesto", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID del cliente in lavorazione
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
        ''' Restituisce o imposta il cliente in lavorazione
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
                Dim oldValue As CPersona = Me.Cliente
                If (oldValue Is value) Then Exit Property
                Me.m_Cliente = value
                Me.m_IDCliente = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeCliente = value.Nominativo
                Me.DoChanged("Cliente", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetCliente(ByVal value As CPersona)
            Me.m_Cliente = value
            Me.m_IDCliente = GetID(value)
        End Sub

        ''' <summary>
        ''' 
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

        Public Property IDStatoAttuale As Integer
            Get
                Return GetID(Me.m_StatoAttuale, Me.m_IDStatoAttuale)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDStatoAttuale
                If (oldValue = value) Then Exit Property
                Me.m_IDStatoAttuale = value
                Me.m_StatoAttuale = Nothing
                Me.DoChanged("IDStatoAttuale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato attuale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoAttuale As StatoTaskLavorazione
            Get
                If (Me.m_StatoAttuale Is Nothing) Then Me.m_StatoAttuale = Anagrafica.StatiTasksLavorazione.GetItemById(Me.m_IDStatoAttuale)
                Return Me.m_StatoAttuale
            End Get
            Set(value As StatoTaskLavorazione)
                Dim oldValue As StatoTaskLavorazione = Me.StatoAttuale
                If (oldValue Is value) Then Exit Property
                Me.m_IDStatoAttuale = GetID(value)
                Me.m_StatoAttuale = value
                Me.DoChanged("StatoAttuale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data di assegnazione del task
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataAssegnazione As Date
            Get
                Return Me.m_DataAssegnazione
            End Get
            Set(value As Date)
                Dim oldValeu As Date = Me.m_DataAssegnazione
                If (Calendar.Compare(value, oldValeu) = 0) Then Exit Property
                Me.m_DataAssegnazione = value
                Me.DoChanged("DataAssegnazione", value, oldValeu)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha creato il task
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDAssegnatoDa As Integer
            Get
                Return GetID(Me.m_AssegnatoDa, Me.m_IDAssegnatoDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAssegnatoDa
                If (oldValue = value) Then Exit Property
                Me.m_IDAssegnatoDa = value
                Me.m_AssegnatoDa = Nothing
                Me.DoChanged("IDAssegnatoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha creato il task
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AssegnatoDa As CUser
            Get
                If (Me.m_AssegnatoDa Is Nothing) Then Me.m_AssegnatoDa = Sistema.Users.GetItemById(Me.m_IDAssegnatoDa)
                Return Me.m_AssegnatoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.AssegnatoDa
                If (oldValue Is value) Then Exit Property
                Me.m_AssegnatoDa = value
                Me.m_IDAssegnatoDa = GetID(value)
                Me.DoChanged("AssegnatoDa", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data prevista per il task
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
        ''' Restituisce o imposta l'ID dell'utente a cui è stato assegnato il task
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
        ''' Restituisce o imposta l'utente a cui è stato assegnato il task
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AssegnatoA As CUser
            Get
                If Me.m_AssegnatoA Is Nothing Then Me.m_AssegnatoA = Sistema.Users.GetItemById(Me.m_IDAssegnatoA)
                Return Me.m_AssegnatoA
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.AssegnatoA
                If (oldValue Is value) Then Exit Property
                Me.m_AssegnatoA = value
                Me.m_IDAssegnatoA = GetID(value)
                Me.DoChanged("AssegnatoA", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta delle note inserite in fase di assegnazione
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
        ''' Restituisce o imposta l'ID dell'azione eseguita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDAzioneEseguita As Integer
            Get
                Return Me.m_IDAzioneEseguita ' Return GetID(Me.m_AzioneEseguita, Me.m_IDAzioneEseguita)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDAzioneEseguita
                If (oldValue = value) Then Exit Property
                Me.m_IDAzioneEseguita = value
                ' Me.m_AzioneEseguita = Nothing
                Me.DoChanged("IDAzioneEseguita", value, oldValue)
            End Set
        End Property

        ' ''' <summary>
        ' ''' Restituisce o imposta l'azione eseguita per il task di lavorazione
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Property AzioneEseguita As AzioneTaskLavorazione
        '    Get
        '        If Me.m_AzioneEseguita Is Nothing Then Me.m_AzioneEseguita = Anagrafica.AzioniTasksLavorazione.GetItemById(Me.m_IDAzioneEseguita)
        '        Return Me.m_AzioneEseguita
        '    End Get
        '    Set(value As AzioneTaskLavorazione)
        '        Dim oldValue As AzioneTaskLavorazione = Me.AzioneEseguita
        '        If (oldValue Is value) Then Exit Property
        '        Me.m_AzioneEseguita = value
        '        Me.m_IDAzioneEseguita = GetID(value)
        '        Me.DoChanged("AzioneEseguita", value, oldValue)
        '    End Set
        'End Property

        Public Overrides Function GetModule() As CModule
            Return Anagrafica.TasksDiLavorazione.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_TaskLavorazione"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDCliente = reader.Read("IDCliente", Me.m_IDCliente)
            Me.m_NomeCliente = reader.Read("NomeCliente", Me.m_NomeCliente)

            Me.m_IDStatoAttuale = reader.Read("IDStatoAttuale", Me.m_IDStatoAttuale)
            
            Me.m_DataAssegnazione = reader.Read("DataAssegnazione", Me.m_DataAssegnazione)
            Me.m_IDAssegnatoDa = reader.Read("IDAssegnatoDa", Me.m_IDAssegnatoDa)
            
            Me.m_DataPrevista = reader.Read("DataPrevista", Me.m_DataPrevista)
            Me.m_IDAssegnatoA = reader.Read("IDAssegnatoA", Me.m_IDAssegnatoA)
            Me.m_Note = reader.Read("Note", Me.m_Note)

            Me.m_IDAzioneEseguita = reader.Read("IDAzioneEseguita", Me.m_IDAzioneEseguita)

            Me.m_Categoria = reader.Read("Categoria", Me.m_Categoria)
            Me.m_Priorita = reader.Read("Priorita", Me.m_Priorita)
            Me.m_IDSorgente = reader.Read("IDSorgente", Me.m_IDSorgente)
            Me.m_TipoSorgente = reader.Read("TipoSorgente", Me.m_TipoSorgente)
            Me.m_IDContesto = reader.Read("IDContesto", Me.m_IDContesto)
            Me.m_TipoContesto = reader.Read("TipoContesto", Me.m_TipoContesto)

            Me.m_IDRegolaEseguita = reader.Read("IDRegolaEseguita", Me.m_IDRegolaEseguita)
            Me.m_ParametriAzione = reader.Read("ParametriAzione", Me.m_ParametriAzione)
            Me.m_RisultatoAzione = reader.Read("RisultatoAzione", Me.m_RisultatoAzione)
            Me.m_DataInizioEsecuzione = reader.Read("DataEsecuzione", Me.m_DataInizioEsecuzione)
            Me.m_DataFineEsecuzione = reader.Read("DataFineEsecuzione", Me.m_DataFineEsecuzione)
            Me.m_IDTaskDestinazione = reader.Read("IDTaskDestinazione", Me.m_IDTaskDestinazione)
            Me.m_IDTaskSorgente = reader.Read("IDTaskSorgente", Me.m_IDTaskSorgente)

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDCliente", Me.IDCliente)
            writer.Write("NomeCliente", Me.m_NomeCliente)

            writer.Write("IDStatoAttuale", Me.IDStatoAttuale)

            writer.Write("DataAssegnazione", Me.m_DataAssegnazione)
            writer.Write("IDAssegnatoDa", Me.IDAssegnatoDa)

            writer.Write("DataPrevista", Me.m_DataPrevista)
            writer.Write("IDAssegnatoA", Me.IDAssegnatoA)
            writer.Write("Note", Me.m_Note)

            writer.Write("IDAzioneEseguita", Me.IDAzioneEseguita)


            writer.Write("Categoria", Me.m_Categoria)
            writer.Write("Priorita", Me.m_Priorita)
            writer.Write("IDSorgente", Me.IDSorgente)
            writer.Write("TipoSorgente", Me.TipoSorgente)
            writer.Write("IDContesto", Me.m_IDContesto)
            writer.Write("TipoContesto", Me.m_TipoContesto)

            writer.Write("IDRegolaEseguita", Me.IDRegolaEseguita)
            writer.Write("ParametriAzione", Me.m_ParametriAzione)
            writer.Write("RisultatoAzione", Me.m_RisultatoAzione)
            writer.Write("DataEsecuzione", Me.m_DataInizioEsecuzione)
            writer.Write("DataFineEsecuzione", Me.m_DataFineEsecuzione)
            writer.Write("IDTaskDestinazione", Me.IDTaskDestinazione)
            writer.Write("IDTaskSorgente", Me.IDTaskSorgente)

            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDCliente", Me.IDCliente)
            writer.WriteAttribute("NomeCliente", Me.m_NomeCliente)

            writer.WriteAttribute("IDStatoAttuale", Me.IDStatoAttuale)

            writer.WriteAttribute("DataAssegnazione", Me.m_DataAssegnazione)
            writer.WriteAttribute("IDAssegnatoDa", Me.IDAssegnatoDa)

            writer.WriteAttribute("DataPrevista", Me.m_DataPrevista)
            writer.WriteAttribute("IDAssegnatoA", Me.IDAssegnatoA)
            writer.WriteAttribute("Note", Me.m_Note)

            writer.WriteAttribute("IDAzioneEseguita", Me.IDAzioneEseguita)

            writer.WriteAttribute("Categoria", Me.m_Categoria)
            writer.WriteAttribute("Priorita", Me.m_Priorita)
            writer.WriteAttribute("IDSorgente", Me.IDSorgente)
            writer.WriteAttribute("TipoSorgente", Me.TipoSorgente)
            writer.WriteAttribute("IDContesto", Me.m_IDContesto)
            writer.WriteAttribute("TipoContesto", Me.m_TipoContesto)

            writer.WriteAttribute("IDRegolaEseguita", Me.IDRegolaEseguita)
            writer.WriteAttribute("ParametriAzione", Me.m_ParametriAzione)
            writer.WriteAttribute("RisultatoAzione", Me.m_RisultatoAzione)
            writer.WriteAttribute("DataEsecuzione", Me.m_DataInizioEsecuzione)
            writer.WriteAttribute("DataFineEsecuzione", Me.m_DataFineEsecuzione)
            writer.WriteAttribute("IDTaskDestinazione", Me.IDTaskDestinazione)
            writer.WriteAttribute("IDTaskSorgente", Me.IDTaskSorgente)

            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDCliente" : Me.m_IDCliente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCliente" : Me.m_NomeCliente = XML.Utils.Serializer.DeserializeString(fieldValue)

                Case "IDStatoAttuale" : Me.m_IDStatoAttuale = XML.Utils.Serializer.DeserializeInteger(fieldValue)

                Case "DataAssegnazione" : Me.m_DataAssegnazione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDAssegnatoDa" : Me.m_IDAssegnatoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)

                Case "DataPrevista" : Me.m_DataPrevista = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDAssegnatoA" : Me.m_IDAssegnatoA = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Note" : Me.m_Note = XML.Utils.Serializer.DeserializeString(fieldValue)

                Case "IDAzioneEseguita" : Me.m_IDAzioneEseguita = XML.Utils.Serializer.DeserializeInteger(fieldValue)

                Case "Categoria" : Me.m_Categoria = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Priorita" : Me.m_Priorita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDSorgente" : Me.m_IDSorgente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoSorgente" : Me.m_TipoSorgente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDContesto" : Me.m_IDContesto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoContesto" : Me.m_TipoContesto = XML.Utils.Serializer.DeserializeString(fieldValue)

                Case "IDRegolaEseguita" : Me.m_IDRegolaEseguita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ParametriAzione" : Me.m_ParametriAzione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "RisultatoAzione" : Me.m_RisultatoAzione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataEsecuzione" : Me.m_DataInizioEsecuzione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataFineEsecuzione" : Me.m_DataFineEsecuzione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDTaskDestinazione" : Me.m_IDTaskDestinazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDTaskSorgente" : Me.m_IDTaskSorgente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Anagrafica.TasksDiLavorazione.UpdateCached(Me)
        End Sub

    End Class


End Class