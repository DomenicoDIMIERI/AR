Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    ''' <summary>
    ''' Rappresenta una possibile regola applicabile ad un task di lavorazione
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class RegolaTaskLavorazione
        Inherits DBObject

        Private Const FLAG_ATTIVO As Integer = 1

        Private m_Nome As String                                'Nome della regola
        Private m_Descrizione As String                         'Descrizione della regola
        Private m_IDStatoSorgente As Integer                    'ID dello stato sorgente
        Private m_StatoSorgente As StatoTaskLavorazione         'stato destinazione
        Private m_IDStatoDestinazione As Integer                'ID dello stato destinazione
        Private m_StatoDestinazione As StatoTaskLavorazione     'Stato destinazione
        Private m_NomeHandler As String                         'handler da chiamare per l'esecuzione della regola
        Private m_ContextType As String                         'Contesto in cui é applicabile la regola
        Private m_Flags As Integer                              'flags

        Public Sub New()
            Me.m_Nome = ""
            Me.m_Descrizione = ""
            Me.m_IDStatoSorgente = 0
            Me.m_StatoSorgente = Nothing
            Me.m_IDStatoDestinazione = 0
            Me.m_StatoDestinazione = Nothing
            Me.m_Flags = FLAG_ATTIVO
            Me.m_NomeHandler = ""
            Me.m_ContextType = ""
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il contesto in cui si applica la regola (es. CTelefonata, CVisita, ec...)
        ''' </summary>
        ''' <returns></returns>
        Public Property ContextType As String
            Get
                Return Me.m_ContextType
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_ContextType
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_ContextType = value
                Me.DoChanged("ContextType", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dello stato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Nome
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il nome della classe che implementa l'interfaccia ITaskHandler il cui metodo Handle verrà lanciato all'esecuzione dell'azione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeHandler As String
            Get
                Return Me.m_NomeHandler
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeHandler
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeHandler = value
                Me.DoChanged("NomeHandler", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la descrizione dello stato
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
        ''' Restituisce o imposta un itero utilizzabile per i flags
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

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se lo stato è attivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Attivo As Boolean
            Get
                Return TestFlag(Me.m_Flags, FLAG_ATTIVO)
            End Get
            Set(value As Boolean)
                If (Me.Attivo = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, FLAG_ATTIVO, value)
                Me.DoChanged("Attivo", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dello stato sorgente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDStatoSorgente As Integer
            Get
                Return GetID(Me.m_StatoSorgente, Me.m_IDStatoSorgente)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDStatoSorgente
                If (oldValue = value) Then Exit Property
                Dim oldStato As StatoTaskLavorazione = Me.StatoSorgente
                Me.m_IDStatoSorgente = value
                Me.m_StatoSorgente = Nothing
                Me.DoChanged("IDStatoSorgente", value, oldValue)
                If (oldStato IsNot Nothing) Then oldStato.NotifyRegolaChanged(Me)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o impsota lo stato origine
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoSorgente As StatoTaskLavorazione
            Get
                If (Me.m_StatoSorgente Is Nothing) Then Me.m_StatoSorgente = Anagrafica.StatiTasksLavorazione.GetItemById(Me.m_IDStatoSorgente)
                Return Me.m_StatoSorgente
            End Get
            Set(value As StatoTaskLavorazione)
                Dim oldValue As StatoTaskLavorazione = Me.StatoSorgente
                If (oldValue Is value) Then Exit Property
                Me.m_StatoSorgente = value
                Me.m_IDStatoSorgente = GetID(value)
                Me.DoChanged("StatoSorgente", value, oldValue)
                If (oldValue IsNot Nothing) Then oldValue.NotifyRegolaChanged(Me)
            End Set
        End Property

        Protected Friend Sub SetStatoSorgente(ByVal value As StatoTaskLavorazione)
            Me.m_StatoSorgente = value
            Me.m_IDStatoSorgente = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID dello stato destinazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDStatoDestinazione As Integer
            Get
                Return GetID(Me.m_StatoDestinazione, Me.m_IDStatoDestinazione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDStatoDestinazione
                If (oldValue = value) Then Exit Property
                Me.m_IDStatoDestinazione = value
                Me.m_StatoDestinazione = Nothing
                Me.DoChanged("IDStatoDestinazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato destinazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoDestinazione As StatoTaskLavorazione
            Get
                If (Me.m_StatoDestinazione Is Nothing) Then Me.m_StatoDestinazione = Anagrafica.StatiTasksLavorazione.GetItemById(Me.m_IDStatoDestinazione)
                Return Me.m_StatoDestinazione
            End Get
            Set(value As StatoTaskLavorazione)
                Dim oldValue As StatoTaskLavorazione = Me.StatoDestinazione
                If (oldValue Is value) Then Exit Property
                Me.m_StatoDestinazione = value
                Me.m_IDStatoDestinazione = GetID(value)
                Me.DoChanged("StatoDestinazione", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetModule() As CModule
            Return Anagrafica.RegoleTasksLavorazione.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_TaskLavorazioneRegole"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Nome = reader.Read("Nome", Me.m_Nome)
            Me.m_Descrizione = reader.Read("Descrizione", Me.m_Descrizione)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_IDStatoSorgente = reader.Read("IDStatoSorgente", Me.m_IDStatoSorgente)
            Me.m_IDStatoDestinazione = reader.Read("IDStatoDestinazione", Me.m_IDStatoDestinazione)
            Me.m_NomeHandler = reader.Read("NomeHandler", Me.m_NomeHandler)
            Me.m_ContextType = reader.Read("ContextType", Me.m_ContextType)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Descrizione", Me.m_Descrizione)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("IDStatoSorgente", Me.IDStatoSorgente)
            writer.Write("IDStatoDestinazione", Me.IDStatoDestinazione)
            writer.Write("NomeHandler", Me.m_NomeHandler)
            writer.Write("ContextType", Me.m_ContextType)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Descrizione", Me.m_Descrizione)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("IDStatoSorgente", Me.IDStatoSorgente)
            writer.WriteAttribute("IDStatoDestinazione", Me.IDStatoDestinazione)
            writer.WriteAttribute("NomeHandler", Me.m_NomeHandler)
            writer.WriteAttribute("ContextType", Me.m_ContextType)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Descrizione" : Me.m_Descrizione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDStatoSorgente" : Me.m_IDStatoSorgente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDStatoDestinazione" : Me.m_IDStatoDestinazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeHandler" : Me.m_NomeHandler = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ContextType" : Me.m_ContextType = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Anagrafica.RegoleTasksLavorazione.UpdateCached(Me)
            If (Me.StatoSorgente IsNot Nothing) Then Me.StatoSorgente.NotifyRegolaChanged(Me)
        End Sub

        Public Function Execute(ByVal taskSorgente As TaskLavorazione, ByVal parametri As String) As Object
            Dim handler As ITaskHandler = Sistema.Types.CreateInstance(Me.NomeHandler)
            Return handler.Handle(taskSorgente, Me, parametri)
        End Function

        Public Function NewTask(ByVal cliente As CPersona, ByVal parametri As String) As TaskLavorazione
            Dim taskSorgente As New TaskLavorazione()
            taskSorgente.AssegnatoDa = Sistema.Users.CurrentUser
            taskSorgente.AssegnatoA = Sistema.Users.CurrentUser
            taskSorgente.Categoria = Me.StatoSorgente.Categoria
            taskSorgente.StatoAttuale = Me.StatoSorgente
            taskSorgente.DataInizioEsecuzione = Calendar.Now()
            taskSorgente.Cliente = cliente
            taskSorgente.PuntoOperativo = cliente.PuntoOperativo
            taskSorgente.Stato = ObjectStatus.OBJECT_VALID
            taskSorgente.Save()
            Dim handler As ITaskHandler = Sistema.Types.CreateInstance(Me.NomeHandler)
            Return handler.Handle(taskSorgente, Me, parametri)
        End Function



    End Class


End Class