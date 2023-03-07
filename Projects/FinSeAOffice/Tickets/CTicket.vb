Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Public Class Office

    ''' <summary>
    ''' Valori enumerativi che indicano lo stato di una segnalazione
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum TicketStatus As Integer
        ''' <summary>
        ''' Ticket inserito ma non ancora aperto
        ''' </summary>
        ''' <remarks></remarks>
        INSERITO = 0

        ''' <summary>
        ''' Aperto
        ''' </summary>
        ''' <remarks></remarks>
        APERTO = 1

        ''' <summary>
        ''' Preso in carico
        ''' </summary>
        ''' <remarks></remarks>
        INLAVORAZIONE = 2

        ''' <summary>
        ''' Risolto
        ''' </summary>
        ''' <remarks></remarks>
        RISOLTO = 3

        ''' <summary>
        ''' L'utente ha chiuso il ticket
        ''' </summary>
        ''' <remarks></remarks>
        NONRISOLVIBILE = 4

        ''' <summary>
        ''' Sospeso
        ''' </summary>
        ''' <remarks></remarks>
        SOSPESO = 5

        ''' <summary>
        ''' L'utente ha riaperto il ticket
        ''' </summary>
        ''' <remarks></remarks>
        RIAPERTO = 6




    End Enum

    ''' <summary>
    ''' Rappresenta una richiesta fatta da un utente
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public NotInheritable Class CTicket
        Inherits DBObjectPO
        Implements IIndexable

        Private m_IDApertoDa As Integer
        Private m_ApertoDa As CUser
        Private m_DataRichiesta As Nullable(Of Date)
        Private m_NomeApertoDa As String
        Private m_InCaricoA As CUser                    'Utente cha ha preso in carico la segnalazione
        Private m_IDInCaricoA As Integer                'ID dell'utente che ha preso in carico la segnalazione
        Private m_DataPresaInCarico As Nullable(Of Date)
        Private m_DataChiusura As Nullable(Of Date)
        Private m_NomeInCaricoA As String               'Nome dell'utente che ha preso in carico la segnalazione
        Private m_Categoria As String                   'Categoria del problema
        Private m_Sottocategoria As String              'Sottocategoria
        Private m_Messaggio As String
        Private m_StatoSegnalazione As TicketStatus     'Stato di risoluzione del problema
        Private m_Priorita As PriorityEnum
        Private m_IDSupervisore As Integer
        Private m_Supervisore As CUser
        Private m_NomeSupervisore As String
        Private m_Canale As String
        Private m_Attachments As CCollection(Of CAttachment)
        Private m_IDCliente As Integer
        Private m_Cliente As CPersona
        Private m_NomeCliente As String
        Private m_Messages As CCollection(Of CTicketAnsware)
        Private m_TipoContesto As String
        Private m_IDContesto As Integer
        Private m_IDPostazione As Integer
        Private m_Postazione As CPostazione
        Private m_NomePostazione As String

        Public Sub New()
            Me.m_InCaricoA = Nothing
            Me.m_IDInCaricoA = 0
            Me.m_NomeInCaricoA = ""
            Me.m_Messaggio = vbNullString
            Me.m_Categoria = vbNullString
            Me.m_StatoSegnalazione = TicketStatus.INSERITO
            Me.m_Priorita = PriorityEnum.PRIORITY_NORMAL
            Me.m_IDSupervisore = 0
            Me.m_Supervisore = Nothing
            Me.m_NomeSupervisore = vbNullString
            Me.m_ApertoDa = Nothing
            Me.m_IDApertoDa = 0
            Me.m_NomeApertoDa = vbNullString
            Me.m_DataRichiesta = Nothing
            Me.m_DataPresaInCarico = Nothing
            Me.m_DataChiusura = Nothing
            Me.m_Canale = vbNullString
            Me.m_Attachments = Nothing
            Me.m_IDCliente = 0
            Me.m_Cliente = Nothing
            Me.m_NomeCliente = ""
            Me.m_Messages = Nothing
            Me.m_TipoContesto = ""
            Me.m_IDContesto = 0
            Me.m_IDPostazione = 0
            Me.m_Postazione = Nothing
            Me.m_NomePostazione = ""
        End Sub

        Public Property IDPostazione As Integer
            Get
                Return GetID(Me.m_Postazione, Me.m_IDPostazione)
            End Get
            Set(value As Integer)
                Dim oldvalue As Integer = Me.IDPostazione
                If (oldvalue = value) Then Return
                Me.m_IDPostazione = value
                Me.m_Postazione = Nothing
                Me.DoChanged("idpostazione", value, oldvalue)
            End Set
        End Property

        Public Property Postazione As CPostazione
            Get
                If (Me.m_Postazione Is Nothing) Then Me.m_Postazione = Anagrafica.Postazioni.GetItemById(Me.m_IDPostazione)
                Return Me.m_Postazione
            End Get
            Set(value As CPostazione)
                Dim oldvalue As CPostazione = Me.Postazione
                If (oldvalue Is value) Then Return
                Me.m_Postazione = value
                Me.m_IDPostazione = GetID(value)
                Me.m_NomePostazione = ""
                If (value IsNot Nothing) Then Me.m_NomePostazione = value.Nome
                Me.DoChanged("postazione", value, oldvalue)
            End Set
        End Property

        Public Property NomePostazione As String
            Get
                Return Me.m_NomePostazione
            End Get
            Set(value As String)
                Dim oldvalue As String = Me.m_NomePostazione
                value = Strings.Trim(value)
                If (oldvalue = value) Then Return
                Me.m_NomePostazione = value
                Me.DoChanged("nomepostazione", value, oldvalue)
            End Set
        End Property

        Public Function GetDataUltimoAggiornamento() As Date
            Dim ret As Date? = Me.DataRichiesta
            ret = Calendar.Max(ret, Me.DataPresaInCarico)
            ret = Calendar.Max(ret, Me.DataChiusura)
            For Each m As CTicketAnsware In Me.Messages
                ret = Calendar.Max(ret, m.Data)
            Next
            If (ret.HasValue) Then
                Return ret
            Else
                Return Calendar.Max(Me.CreatoIl, Me.ModificatoIl)
            End If
        End Function

        Public Property TipoContesto As String
            Get
                Return Me.m_TipoContesto
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_TipoContesto
                If (oldValue = value) Then Exit Property
                Me.m_TipoContesto = value
                Me.DoChanged("TipoContesto", value, oldValue)
            End Set
        End Property

        Public Property IDContesto As Integer
            Get
                Return Me.m_IDContesto
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_IDContesto
                If (oldValue = value) Then Exit Property
                Me.m_IDContesto = value
                Me.DoChanged("IDContesto", value, oldValue)
            End Set
        End Property

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

        Public ReadOnly Property Messages As CCollection(Of CTicketAnsware)
            Get
                SyncLock Me
                    If Me.m_Messages Is Nothing Then
                        Me.m_Messages = New CCollection(Of CTicketAnsware)
                        Dim tmp As New CAnnotazioni(Me)
                        For Each a As CAnnotazione In tmp
                            Dim m As New CTicketAnsware
                            m.Data = a.CreatoIl
                            m.Operatore = a.CreatoDa
                            m.StatoTicket = Me.StatoSegnalazione
                            m.Messaggio = a.Valore
                            m.Stato = a.Stato
                            Me.m_Messages.Add(m)
                        Next
                        Me.m_Messages.Sort()
                    End If
                    Return Me.m_Messages
                End SyncLock
            End Get
        End Property

        Public Property Cliente As CPersona
            Get
                SyncLock Me
                    If Me.m_Cliente Is Nothing Then Me.m_Cliente = Anagrafica.Persone.GetItemById(Me.m_IDCliente)
                    Return Me.m_Cliente
                End SyncLock
            End Get
            Set(value As CPersona)
                Dim oldValue As CPersona
                SyncLock Me
                    oldValue = Me.m_Cliente
                    If (oldValue Is value) Then Exit Property
                    Me.m_Cliente = value
                    Me.m_IDCliente = GetID(value)
                    If (value IsNot Nothing) Then Me.m_NomeCliente = value.Nominativo
                End SyncLock
                Me.DoChanged("Cliente", value, oldValue)
            End Set
        End Property

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

        Public ReadOnly Property Attachments As CCollection(Of CAttachment)
            Get
                SyncLock Me
                    If (Me.m_Attachments Is Nothing) Then Me.m_Attachments = New CCollection(Of CAttachment)(New CAttachments(Me))
                    Return Me.m_Attachments
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property NumberEx As String
            Get
                Return RPC.FormatID(Me.ID)
            End Get
        End Property

        Public Property Canale As String
            Get
                Return Me.m_Canale
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Canale
                If (oldValue = value) Then Exit Property
                Me.m_Canale = value
                Me.DoChanged("Canale", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data in cui è stata effettuata la richiesta di assistenza
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataRichiesta As Nullable(Of Date)
            Get
                Return Me.m_DataRichiesta
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataRichiesta
                If (oldValue = value) Then Exit Property
                Me.m_DataRichiesta = value
                Me.DoChanged("DataRichiesta", value, oldValue)
            End Set
        End Property

        Public Property DataPresaInCarico As Nullable(Of Date)
            Get
                Return Me.m_DataPresaInCarico
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataPresaInCarico
                If (oldValue = value) Then Exit Property
                Me.m_DataPresaInCarico = value
                Me.DoChanged("DataPresaInCarico", value, oldValue)
            End Set
        End Property

        Public Property DataChiusura As Nullable(Of Date)
            Get
                Return Me.m_DataChiusura
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DataChiusura
                If (oldValue = value) Then Exit Property
                Me.m_DataChiusura = value
                Me.DoChanged("DataChiusura", value, oldValue)
            End Set
        End Property


        Public Property Priorita As PriorityEnum
            Get
                Return Me.m_Priorita
            End Get
            Set(value As PriorityEnum)
                Dim oldValue As PriorityEnum = Me.m_Priorita
                If (oldValue = value) Then Exit Property
                Me.m_Priorita = value
                Me.DoChanged("Priorita", value, oldValue)
            End Set
        End Property

        Public Property IDSupervisore As Integer
            Get
                Return GetID(Me.m_Supervisore, Me.m_IDSupervisore)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDSupervisore
                If (oldValue = value) Then Exit Property
                Me.m_IDSupervisore = value
                Me.m_Supervisore = Nothing
                Me.DoChanged("IDSupervisore", value, oldValue)
            End Set
        End Property

        Public Property Supervisore As CUser
            Get
                If (Me.m_Supervisore Is Nothing) Then Me.m_Supervisore = Sistema.Users.GetItemById(Me.m_IDSupervisore)
                Return Me.m_Supervisore
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_Supervisore
                If (oldValue Is value) Then Exit Property
                Me.m_Supervisore = Nothing
                Me.m_IDSupervisore = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeSupervisore = value.Nominativo
                Me.DoChanged("Supervisore", value, oldValue)
            End Set
        End Property

        Public Property NomeSupervisore As String
            Get
                Return Me.m_NomeSupervisore
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeSupervisore
                If (oldValue = value) Then Exit Property
                Me.m_NomeSupervisore = value
                Me.DoChanged("NomeSupervisore", value, oldValue)
            End Set
        End Property

        Public Property IDApertoDa As Integer
            Get
                Return GetID(Me.m_ApertoDa, Me.m_IDApertoDa)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDApertoDa
                If (oldValue = value) Then Exit Property
                Me.m_IDApertoDa = value
                Me.m_ApertoDa = Nothing
                Me.DoChanged("ApertoDaID", value, oldValue)
            End Set
        End Property

        Public Property ApertoDa As CUser
            Get
                If (Me.m_ApertoDa Is Nothing) Then Me.m_ApertoDa = Sistema.Users.GetItemById(Me.m_IDApertoDa)
                Return Me.m_ApertoDa
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.m_ApertoDa
                If (oldValue Is value) Then Exit Property
                Me.m_ApertoDa = value
                Me.m_IDApertoDa = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeApertoDa = value.Nominativo
                Me.DoChanged("ApertoDa", value, oldValue)
            End Set
        End Property

        Public Property NomeApertoDa As String
            Get
                Return Me.m_NomeApertoDa
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_NomeApertoDa
                If (oldValue = value) Then Exit Property
                Me.m_NomeApertoDa = value
                Me.DoChanged("NomeApertoDa", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha preso in carico la segnalazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDInCaricoA As Integer
            Get
                Return GetID(Me.m_InCaricoA, Me.m_IDInCaricoA)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDInCaricoA
                If (oldValue = value) Then Exit Property
                Me.m_InCaricoA = Nothing
                Me.m_IDInCaricoA = value
                Me.DoChanged("InCaricoAID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisec o imposta l'Utente che ha preso in carico la segnalazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property InCaricoA As CUser
            Get
                If (Me.m_InCaricoA Is Nothing) Then Me.m_InCaricoA = Users.GetItemById(Me.m_IDInCaricoA)
                Return Me.m_InCaricoA
            End Get
            Set(value As CUser)
                'Dim oldValue As CUser = Me.InCaricoA
                Me.m_InCaricoA = value
                Me.m_IDInCaricoA = GetID(value)
                If (value IsNot Nothing) Then Me.m_NomeInCaricoA = value.Nominativo
                Me.DoChanged("InCaricoA", value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che ha preso in carico la segnalazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomeInCaricoA As String
            Get
                Return Me.m_NomeInCaricoA
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeInCaricoA
                value = Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeInCaricoA = value
                Me.DoChanged("InCaricoANome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la categoria del problema
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
                value = Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Categoria = value
                Me.DoChanged("Categoria", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la sotto-categoria del problema
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Sottocategoria As String
            Get
                Return Me.m_Sottocategoria
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Sottocategoria
                value = Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_Sottocategoria = value
                Me.DoChanged("Sottocategoria", value, oldValue)
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
        ''' Restituisce o imposta lo stato della segnalazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoSegnalazione As TicketStatus
            Get
                Return Me.m_StatoSegnalazione
            End Get
            Set(value As TicketStatus)
                Dim oldValue As TicketStatus = Me.m_StatoSegnalazione
                If (oldValue = value) Then Exit Property
                Me.m_StatoSegnalazione = value
                Me.DoChanged("StatoSegnalazione", value, oldValue)
            End Set
        End Property


        Public Overrides Function ToString() As String
            Return RPC.FormatID(Me.ID)
        End Function


        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Tickets.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_SupportTickets"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Dim str As String
            Dim tmp As CCollection

            Me.m_IDInCaricoA = reader.Read("InCaricoA", Me.m_IDInCaricoA)
            Me.m_NomeInCaricoA = reader.Read("InCaricoANome", Me.m_NomeInCaricoA)
            Me.m_Categoria = reader.Read("Categoria", Me.m_Categoria)
            Me.m_Sottocategoria = reader.Read("Sottocategoria", Me.m_Sottocategoria)
            Me.m_Messaggio = reader.Read("Messaggio", Me.m_Messaggio)
            Me.m_StatoSegnalazione = reader.Read("StatoSegnalazione", Me.m_StatoSegnalazione)
            Me.m_IDApertoDa = reader.Read("ApertoDa", Me.m_IDApertoDa)
            Me.m_NomeApertoDa = reader.Read("NomeApertoDa", Me.m_NomeApertoDa)
            Me.m_Priorita = reader.Read("Priorita", Me.m_Priorita)
            Me.m_IDSupervisore = reader.Read("IDSupervisore", Me.m_IDSupervisore)
            Me.m_NomeSupervisore = reader.Read("NomeSupervisore", Me.m_NomeSupervisore)
            Me.m_DataRichiesta = reader.Read("DataRichiesta", Me.m_DataRichiesta)
            Me.m_DataPresaInCarico = reader.Read("DataPresaInCarico", Me.m_DataPresaInCarico)
            Me.m_DataChiusura = reader.Read("DataChiusura", Me.m_DataChiusura)
            Me.m_Canale = reader.Read("Canale", Me.m_Canale)
            Me.m_IDCliente = reader.Read("IDCliente", Me.m_IDCliente)
            Me.m_NomeCliente = reader.Read("NomeCliente", Me.m_NomeCliente)
            Me.m_TipoContesto = reader.Read("TipoContesto", Me.m_TipoContesto)
            Me.m_IDContesto = reader.Read("IDContesto", Me.m_IDContesto)
            Me.m_IDPostazione = reader.Read("IDPostazione", Me.m_IDPostazione)
            Me.m_NomePostazione = reader.Read("NomePostazione", Me.m_NomePostazione)

            Try
                str = reader.Read("Messages", "")
                If (str <> "") Then
                    tmp = XML.Utils.Serializer.Deserialize(str)
                    Me.m_Messages = New CCollection(Of CTicketAnsware)
                    Me.m_Messages.AddRange(tmp)
                End If
            Catch ex As Exception

            End Try

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

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)

            If (Me.Stato = ObjectStatus.OBJECT_VALID) Then
                Office.Tickets.Index.Index(Me)
            Else
                Office.Tickets.Index.Unindex(Me)
            End If

            Return ret
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("InCaricoA", Me.IDInCaricoA)
            writer.Write("InCaricoANome", Me.m_NomeInCaricoA)
            writer.Write("Categoria", Me.m_Categoria)
            writer.Write("Sottocategoria", Me.m_Sottocategoria)
            writer.Write("Messaggio", Me.m_Messaggio)
            writer.Write("StatoSegnalazione", Me.m_StatoSegnalazione)
            writer.Write("ApertoDa", Me.IDApertoDa)
            writer.Write("NomeApertoDa", Me.m_NomeApertoDa)
            writer.Write("Priorita", Me.m_Priorita)
            writer.Write("IDSupervisore", Me.IDSupervisore)
            writer.Write("NomeSupervisore", Me.m_NomeSupervisore)
            writer.Write("DataRichiesta", Me.m_DataRichiesta)
            writer.Write("DataPresaInCarico", Me.m_DataPresaInCarico)
            writer.Write("DataChiusura", Me.m_DataChiusura)
            writer.Write("Canale", Me.m_Canale)
            writer.Write("IDCliente", Me.IDCliente)
            writer.Write("NomeCliente", Me.m_NomeCliente)
            writer.Write("Messages", XML.Utils.Serializer.Serialize(Me.Messages))
            writer.Write("Attachments", XML.Utils.Serializer.Serialize(Me.Attachments))
            writer.Write("TipoContesto", Me.m_TipoContesto)
            writer.Write("IDContesto", Me.m_IDContesto)
            writer.Write("IDPostazione", Me.IDPostazione)
            writer.Write("NomePostazione", Me.m_NomePostazione)

            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("InCaricoA", Me.IDInCaricoA)
            writer.WriteAttribute("InCaricoANome", Me.m_NomeInCaricoA)
            writer.WriteAttribute("Categoria", Me.m_Categoria)
            writer.WriteAttribute("Sottocategoria", Me.m_Sottocategoria)
            writer.WriteAttribute("StatoSegnalazione", Me.m_StatoSegnalazione)
            writer.WriteAttribute("ApertoDa", Me.IDApertoDa)
            writer.WriteAttribute("NomeApertoDa", Me.m_NomeApertoDa)
            writer.WriteAttribute("IDSupervisore", Me.IDSupervisore)
            writer.WriteAttribute("NomeSupervisore", Me.m_NomeSupervisore)
            writer.WriteAttribute("DataRichiesta", Me.m_DataRichiesta)
            writer.WriteAttribute("DataPresaInCarico", Me.m_DataPresaInCarico)
            writer.WriteAttribute("DataChiusura", Me.m_DataChiusura)
            writer.WriteAttribute("Canale", Me.m_Canale)
            writer.WriteAttribute("Priorita", Me.m_Priorita)
            writer.WriteAttribute("IDCliente", Me.IDCliente)
            writer.WriteAttribute("NomeCliente", Me.m_NomeCliente)
            writer.WriteAttribute("TipoContesto", Me.m_TipoContesto)
            writer.WriteAttribute("IDContesto", Me.m_IDContesto)
            writer.WriteAttribute("IDPostazione", Me.IDPostazione)
            writer.WriteAttribute("NomePostazione", Me.m_NomePostazione)

            MyBase.XMLSerialize(writer)
            writer.WriteTag("Messaggio", Me.m_Messaggio)
            writer.WriteTag("Attachments", Me.Attachments)
            writer.WriteTag("Messages", Me.Messages)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "InCaricoA" : Me.m_IDInCaricoA = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "InCaricoANome" : Me.m_NomeInCaricoA = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Categoria" : Me.m_Categoria = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Sottocategoria" : Me.m_Sottocategoria = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Messaggio" : Me.m_Messaggio = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoSegnalazione" : Me.m_StatoSegnalazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ApertoDa" : Me.m_IDApertoDa = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeApertoDa" : Me.m_NomeApertoDa = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Priorita" : Me.m_Priorita = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDSupervisore" : Me.m_IDSupervisore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeSupervisore" : Me.m_NomeSupervisore = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DataRichiesta" : Me.m_DataRichiesta = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataPresaInCarico" : Me.m_DataPresaInCarico = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataChiusura" : Me.m_DataChiusura = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Canale" : Me.m_Canale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Attachments" : Me.m_Attachments = New CCollection(Of CAttachment) : Me.m_Attachments.AddRange(fieldValue)
                Case "Messages" : Me.m_Messages = New CCollection(Of CTicketAnsware) : Me.m_Messages.AddRange(fieldValue)
                Case "IDCliente" : Me.m_IDCliente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCliente" : Me.m_NomeCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TipoContesto" : Me.m_TipoContesto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDContesto" : Me.m_IDContesto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDPostazione" : Me.m_IDPostazione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePostazione" : Me.m_NomePostazione = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


        ''' <summary>
        ''' Apre il ticket
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Apri()
            If (Me.m_StatoSegnalazione <> TicketStatus.INSERITO) Then Throw New InvalidOperationException("Stato non valido")
            Me.StatoSegnalazione = TicketStatus.APERTO
            Me.DataRichiesta = Now
            Me.ApertoDa = Users.CurrentUser
            Me.Save()
            Me.GetModule.DispatchEvent(New EventDescription("ticket_open", "Apertura del ticket #" & Me.NumberEx, Me))
        End Sub

        ''' <summary>
        ''' Prende in carico
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub PrendiInCarico()
            If (Me.m_StatoSegnalazione <> TicketStatus.APERTO AndAlso Me.m_StatoSegnalazione <> TicketStatus.RIAPERTO) Then Throw New InvalidOperationException("Stato non valido")
            Me.StatoSegnalazione = TicketStatus.INLAVORAZIONE
            Me.DataPresaInCarico = Now
            Me.InCaricoA = Users.CurrentUser
            Me.Save()
            Me.GetModule.DispatchEvent(New EventDescription("ticket_work", "Presa in carico del ticket #" & Me.NumberEx, Me))
        End Sub

        ''' <summary>
        ''' Chiude il ticket come risolto
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Risolto()
            If (Me.m_StatoSegnalazione <> TicketStatus.INLAVORAZIONE AndAlso Me.m_StatoSegnalazione <> TicketStatus.SOSPESO) Then Throw New InvalidOperationException("Stato non valido")
            Me.StatoSegnalazione = TicketStatus.RISOLTO
            Me.DataChiusura = Now
            Me.InCaricoA = Users.CurrentUser
            Me.Save()
            Me.GetModule.DispatchEvent(New EventDescription("ticket_closed", "Chiusura del ticket #" & Me.NumberEx, Me))
        End Sub

        ''' <summary>
        ''' Chiude il ticket come non risolvibile
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Errore()
            If (Me.m_StatoSegnalazione <> TicketStatus.INLAVORAZIONE AndAlso Me.m_StatoSegnalazione <> TicketStatus.SOSPESO) Then Throw New InvalidOperationException("Stato non valido")
            Me.StatoSegnalazione = TicketStatus.NONRISOLVIBILE
            Me.DataChiusura = Now
            Me.InCaricoA = Users.CurrentUser
            Me.Save()
            Me.GetModule.DispatchEvent(New EventDescription("ticket_error", "Chiusura con errore del ticket #" & Me.NumberEx, Me))
        End Sub

        ''' <summary>
        ''' Riapre il ticket
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Riapri()
            If (Me.m_StatoSegnalazione <> TicketStatus.RISOLTO AndAlso Me.m_StatoSegnalazione <> TicketStatus.NONRISOLVIBILE) Then Throw New InvalidOperationException("Stato non valido")
            Me.StatoSegnalazione = TicketStatus.APERTO
            Me.Save()
            Me.GetModule.DispatchEvent(New EventDescription("ticket_resume", "Riapertura del ticket #" & Me.NumberEx, Me))
        End Sub

        ''' <summary>
        ''' Sospende il ticket
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Sospendi()
            If (Me.m_StatoSegnalazione <> TicketStatus.INLAVORAZIONE) Then Throw New InvalidOperationException("Stato non valido")
            Me.StatoSegnalazione = TicketStatus.SOSPESO
            Me.Save()
            Me.GetModule.DispatchEvent(New EventDescription("ticket_suspended", "Sospensione del ticket #" & Me.NumberEx, Me))
        End Sub

        Public Sub AddMessage(ByVal msg As CTicketAnsware)
            Dim oldStato As TicketStatus = Me.StatoSegnalazione
            Me.Messages.Add(msg)
            Me.Messages.Sort()
            Me.StatoSegnalazione = msg.StatoTicket
            Me.Save(True)
            If (oldStato <> Me.StatoSegnalazione) Then
                Select Case Me.StatoSegnalazione
                    Case TicketStatus.APERTO, TicketStatus.RIAPERTO, TicketStatus.INSERITO
                        Me.GetModule.DispatchEvent(New EventDescription("ticket_resume", "Riapertura del ticket #" & Me.NumberEx, Me))
                    Case TicketStatus.INLAVORAZIONE
                        Me.GetModule.DispatchEvent(New EventDescription("ticket_work", "Presa in carico del ticket #" & Me.NumberEx, Me))
                    Case TicketStatus.NONRISOLVIBILE
                        Me.GetModule.DispatchEvent(New EventDescription("ticket_error", "Chiusura con errore del ticket #" & Me.NumberEx, Me))
                    Case TicketStatus.RISOLTO
                        Me.GetModule.DispatchEvent(New EventDescription("ticket_closed", "Chiusura del ticket #" & Me.NumberEx, Me))
                End Select
            Else
                Me.GetModule.DispatchEvent(New EventDescription("ticket_message", "Aggiornamento del ticket #" & Me.NumberEx, Me))
            End If

        End Sub

        Protected Overrides Sub OnCreate(e As SystemEvent)
            MyBase.OnCreate(e)
            Tickets.doItemCreated(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnDelete(e As SystemEvent)
            MyBase.OnDelete(e)
            Tickets.doItemDeleted(New ItemEventArgs(Me))
        End Sub

        Protected Overrides Sub OnModified(e As SystemEvent)
            MyBase.OnModified(e)
            Tickets.doItemModified(New ItemEventArgs(Me))
        End Sub

        Public Function GetIndexedWords() As String() Implements IIndexable.GetIndexedWords
            Dim buffer As New System.Text.StringBuilder
            buffer.Append(Me.NumberEx)
            If (buffer.Length > 0) Then buffer.Append(" ")
            buffer.Append(Me.Categoria)
            If (buffer.Length > 0) Then buffer.Append(" ")
            buffer.Append(Me.Sottocategoria)
            If (buffer.Length > 0) Then buffer.Append(" ")
            buffer.Append(Me.Messaggio)
            If (buffer.Length > 0) Then buffer.Append(" ")
            buffer.Append(Me.NomeCliente)
            For Each m As CTicketAnsware In Me.Messages
                If (buffer.Length > 0) Then buffer.Append(" ")
                buffer.Append(m.Messaggio)
            Next

            Dim ret As New System.Collections.ArrayList
            Dim str As String = buffer.ToString

            str = Replace(str, vbCr, " ")
            str = Replace(str, vbLf, " ")
            str = Replace(str, "  ", " ")

            Dim a() As String = Split(str, " ")
            If (Arrays.Len(a) > 0) Then
                For Each s As String In a
                    If (Strings.Trim(s) <> "") Then ret.Add(s)
                Next
            End If

            Return ret.ToArray(GetType(String))
        End Function

        Public Function GetKeyWords() As String() Implements IIndexable.GetKeyWords
            Return {}
        End Function
    End Class


End Class