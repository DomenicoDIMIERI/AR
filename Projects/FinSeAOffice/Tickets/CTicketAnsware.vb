Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Public Class Office

     
    ''' <summary>
    ''' Rappresenta una richiesta fatta da un utente
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public NotInheritable Class CTicketAnsware
        Inherits DBObject
        Implements IComparable

        Private m_IDTicket As Integer
        <NonSerialized> Private m_Ticket As CTicket
        Private m_IDOperatore As Integer
        <NonSerialized> Private m_Operatore As CUser
        Private m_NomeOperatore As String
        Private m_Data As Date
        Private m_StatoTicket As TicketStatus
        Private m_Messaggio As String
        Private m_Attachments As CCollection(Of CAttachment)

        Public Sub New()
            Me.m_IDTicket = 0
            Me.m_Ticket = Nothing
            Me.m_IDOperatore = 0
            Me.m_Operatore = Nothing
            Me.m_NomeOperatore = ""
            Me.m_Data = Calendar.Now
            Me.m_Messaggio = ""
            Me.m_Attachments = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID del ticket a cui appartiene il messaggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDTicket As Integer
            Get
                Return GetID(Me.m_Ticket, Me.m_IDTicket)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDTicket
                If (oldValue = value) Then Exit Property
                Me.m_IDTicket = value
                Me.m_Ticket = Nothing
                Me.DoChanged("Ticket", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta il ticket a cui appartiene il messaggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Ticket As CTicket
            Get
                SyncLock Me
                    If (Me.m_Ticket Is Nothing) Then Me.m_Ticket = Tickets.GetItemById(Me.m_IDTicket)
                    Return Me.m_Ticket
                End SyncLock
            End Get
            Set(value As CTicket)
                Dim oldValue As CTicket
                SyncLock Me
                    oldValue = Me.m_Ticket
                    If (oldValue Is value) Then Exit Property
                    Me.m_Ticket = value
                    Me.m_IDTicket = GetID(value)
                End SyncLock
                Me.DoChanged("Ticket", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'operatore che ha inserito il messaggio
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
        ''' Restituisce o imposta l'operatore che ha inserito il messaggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Operatore As CUser
            Get
                SyncLock Me
                    If Me.m_Operatore Is Nothing Then Me.m_Operatore = Sistema.Users.GetItemById(Me.m_IDOperatore)
                    Return Me.m_Operatore
                End SyncLock
            End Get
            Set(value As CUser)
                Dim oldValue As CUser
                SyncLock Me
                    oldValue = Me.Operatore
                    If (oldValue Is value) Then Exit Property
                    Me.m_Operatore = value
                    Me.m_IDOperatore = GetID(value)
                    If (value IsNot Nothing) Then Me.m_NomeOperatore = value.Nominativo
                End SyncLock
                Me.DoChanged("Operatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'operatore che ha inserito il messaggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeOperatore As String
            Get
                Return Me.m_NomeOperatore
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeOperatore
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeOperatore = value
                Me.DoChanged("NomeOperatore", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data in cui è stato inserito il messaggio
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
                If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_Data = value
                Me.DoChanged("Data", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il messaggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Messaggio As String
            Get
                Return Me.m_Messaggio
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Messaggio
                If (oldValue = value) Then Exit Property
                Me.m_Messaggio = value
                Me.DoChanged("Messaggio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta lo stato in cui viene portato il ticket in seguito a questa risposta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoTicket As TicketStatus
            Get
                Return Me.m_StatoTicket
            End Get
            Set(value As TicketStatus)
                Dim oldValue As TicketStatus = Me.m_StatoTicket
                If (oldValue = value) Then Exit Property
                Me.m_StatoTicket = value
                Me.DoChanged("StatoTicket", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Attachments As CCollection(Of CAttachment)
            Get
                SyncLock Me
                    If Me.m_Attachments Is Nothing Then Me.m_Attachments = New CCollection(Of CAttachment)
                    Return Me.m_Attachments
                End SyncLock
            End Get
        End Property


        Public Overrides Function ToString() As String
            Return Me.m_Messaggio
        End Function


        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_SupportTicketsAnswares"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Dim str As String
            Dim tmp As CCollection

            Me.m_IDTicket = reader.Read("IDTicket", Me.m_IDTicket)
            Me.m_IDOperatore = reader.Read("IDOperatore", Me.m_IDOperatore)
            Me.m_NomeOperatore = reader.Read("NomeOperatore", Me.m_NomeOperatore)
            Me.m_Data = reader.Read("Data", Me.m_Data)
            Me.m_Messaggio = reader.Read("Messaggio", Me.m_Messaggio)
            Me.m_StatoTicket = reader.Read("StatoTicket", Me.m_StatoTicket)
            Me.m_Messaggio = reader.Read("Messaggio", Me.m_Messaggio)
            Try
                str = reader.Read("Attachments", "")
                If (str <> "") Then
                    tmp = XML.Utils.Serializer.Deserialize(str)
                    Me.m_Attachments = New CCollection(Of CAttachment)
                    Me.m_Attachments.AddRange(tmp)
                End If
            Catch ex As Exception

            End Try
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDTicket", Me.IDTicket)
            writer.Write("IDOperatore", Me.IDOperatore)
            writer.Write("NomeOperatore", Me.m_NomeOperatore)
            writer.Write("Data", Me.m_Data)
            writer.Write("Messaggio", Me.m_Messaggio)
            writer.Write("StatoTicket", Me.m_StatoTicket)
            writer.Write("Messaggio", Me.m_Messaggio)
            writer.Write("Attachments", XML.Utils.Serializer.Serialize(Me.Attachments))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDTicket", Me.IDTicket)
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("NomeOperatore", Me.m_NomeOperatore)
            writer.WriteAttribute("Data", Me.m_Data)
            writer.WriteAttribute("Messaggio", Me.m_Messaggio)
            writer.WriteAttribute("StatoTicket", Me.m_StatoTicket)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Messaggio", Me.m_Messaggio)
            writer.WriteTag("Attachments", Me.Attachments)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDTicket" : Me.m_IDTicket = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDOperatore" : Me.m_IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeOperatore" : Me.m_NomeOperatore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Data" : Me.m_Data = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "StatoTicket" : Me.m_StatoTicket = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Messaggio" : Me.m_Messaggio = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Attachments" : Me.m_Attachments = New CCollection(Of CAttachment) : Me.m_Attachments.AddRange(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Function CompareTo(ByVal other As CTicketAnsware) As Integer
            Return Calendar.Compare(Me.m_Data, other.m_Data)
        End Function
 
        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function
    End Class


End Class