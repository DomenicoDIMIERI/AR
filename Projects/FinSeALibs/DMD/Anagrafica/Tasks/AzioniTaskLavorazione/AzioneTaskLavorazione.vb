Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    ''' <summary>
    ''' Rappresenta un'azione eseguita su un task
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class AzioneTaskLavorazione
        Inherits DBObjectPO

        Private m_IDTaskLavorazione As Integer
        Private m_TaskLavorazione As TaskLavorazione
        Private m_DataEsecuzione As Date
        Private m_DataFineEsecuzione As Nullable(Of Date)
        Private m_ParametriAzione As String
        Private m_RisultatoAzione As String
        Private m_IDRegolaApplicata As Integer
        Private m_RegolaApplicata As RegolaTaskLavorazione
        Private m_Flags As Integer
        Private m_NomeHandler As String
        
        Public Sub New()
            Me.m_IDTaskLavorazione = 0
            Me.m_TaskLavorazione = Nothing
            Me.m_DataEsecuzione = Calendar.Now
            Me.m_ParametriAzione = ""
            Me.m_RisultatoAzione = ""
            Me.m_IDRegolaApplicata = 0
            Me.m_RegolaApplicata = Nothing
            Me.m_DataFineEsecuzione = Nothing
            Me.m_Flags = 0
            Me.m_NomeHandler = ""
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID del task su cui è stata eseguita l'azione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDTaskLavorazione As Integer
            Get
                Return GetID(Me.m_TaskLavorazione, Me.m_IDTaskLavorazione)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDTaskLavorazione
                If (oldValue = value) Then Exit Property
                Me.m_IDTaskLavorazione = value
                Me.m_TaskLavorazione = Nothing
                Me.DoChanged("IDTaskLavorazione", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il task su cui è stata eseguita l'azione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TaskLavorazione As TaskLavorazione
            Get
                If Me.m_TaskLavorazione Is Nothing Then Me.m_TaskLavorazione = Anagrafica.TasksDiLavorazione.GetItemById(Me.m_IDTaskLavorazione)
                Return Me.m_TaskLavorazione
            End Get
            Set(value As TaskLavorazione)
                Dim oldValue As TaskLavorazione = Me.TaskLavorazione
                If (oldValue Is value) Then Exit Property
                Me.m_TaskLavorazione = value
                Me.m_IDTaskLavorazione = GetID(value)
                Me.DoChanged("TaskLavorazione", value, oldValue)
            End Set
        End Property

        

        ''' <summary>
        ''' Restituisce o imposta il nome dell'handler che ha eseguito l'azione
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
        Public Property DataEsecuzione As Date
            Get
                Return Me.m_DataEsecuzione
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_DataEsecuzione
                If Calendar.Compare(oldValue, value) = 0 Then Exit Property
                Me.m_DataEsecuzione = value
                Me.DoChanged("DataEsecuzione", value, oldValue)
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
        ''' Restituisce o imposta l'ID della regola applicata al task
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDRegolaApplicata As Integer
            Get
                Return GetID(Me.m_RegolaApplicata, Me.m_IDRegolaApplicata)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDRegolaApplicata
                If (oldValue = value) Then Exit Property
                Me.m_IDRegolaApplicata = value
                Me.m_RegolaApplicata = Nothing
                Me.DoChanged("IDRegolaApplicata", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la regola applicata al task
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RegolaApplicata As RegolaTaskLavorazione
            Get
                If Me.m_RegolaApplicata Is Nothing Then Me.m_RegolaApplicata = Anagrafica.RegoleTasksLavorazione.GetItemById(Me.m_IDRegolaApplicata)
                Return Me.m_RegolaApplicata
            End Get
            Set(value As RegolaTaskLavorazione)
                Dim oldValue As RegolaTaskLavorazione = Me.RegolaApplicata
                If (oldValue Is value) Then Exit Property
                Me.m_RegolaApplicata = value
                Me.m_IDRegolaApplicata = GetID(value)
                Me.DoChanged("RegolaApplicata", value, oldValue)
            End Set
        End Property

        
        Public Overrides Function GetModule() As CModule
            Return Anagrafica.AzioniTasksLavorazione.Module
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_TaskLavorazioneAzioni"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_IDTaskLavorazione = reader.Read("IDTaskLavorazione", Me.m_IDTaskLavorazione)
            Me.m_DataEsecuzione = reader.Read("DataEsecuzione", Me.m_DataEsecuzione)
            Me.m_ParametriAzione = reader.Read("ParametriAzione", Me.m_ParametriAzione)
            Me.m_RisultatoAzione = reader.Read("RisultatoAzione", Me.m_RisultatoAzione)
            Me.m_IDRegolaApplicata = reader.Read("IDRegolaApplicata", Me.m_IDRegolaApplicata)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_NomeHandler = reader.Read("NomeHandler", Me.m_NomeHandler)
            Me.m_DataFineEsecuzione = reader.Read("DataFineEsecuzione", Me.m_DataFineEsecuzione)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDTaskLavorazione", Me.IDTaskLavorazione)
            writer.Write("DataEsecuzione", Me.m_DataEsecuzione)
            writer.Write("ParametriAzione", Me.m_ParametriAzione)
            writer.Write("RisultatoAzione", Me.m_RisultatoAzione)
            writer.Write("IDRegolaApplicata", Me.IDRegolaApplicata)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("NomeHandler", Me.m_NomeHandler)
            writer.Write("DataFineEsecuzione", Me.m_DataFineEsecuzione)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDTaskLavorazione", Me.IDTaskLavorazione)
            writer.WriteAttribute("DataEsecuzione", Me.m_DataEsecuzione)
            writer.WriteAttribute("IDRegolaApplicata", Me.IDRegolaApplicata)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("NomeHandler", Me.m_NomeHandler)
            writer.WriteAttribute("DataFineEsecuzione", Me.m_DataFineEsecuzione)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("ParametriAzione", Me.m_ParametriAzione)
            writer.WriteTag("RisultatoAzione", Me.m_RisultatoAzione)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "ParametriAzione" : Me.m_ParametriAzione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "RisultatoAzione" : Me.m_RisultatoAzione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDTaskLavorazione" : Me.m_IDTaskLavorazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataEsecuzione" : Me.m_DataEsecuzione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "IDRegolaApplicata" : Me.m_IDRegolaApplicata = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeHandler" : Me.m_NomeHandler = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataFineEsecuzione" : Me.m_DataFineEsecuzione = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            Anagrafica.AzioniTasksLavorazione.UpdateCached(Me)
        End Sub

    End Class


End Class