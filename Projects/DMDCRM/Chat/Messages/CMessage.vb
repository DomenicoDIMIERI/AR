Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls

Partial Public Class Messenger

    Public Enum StatoMessaggio As Integer
        NonConsegnato = 0
        NonLetto = 1
        Letto = 2
    End Enum

    <Serializable> _
    Public Class CMessage
        Inherits DBObject
        Implements IComparable

        Private m_Time As Date           '[Date] Data e ora di invio del messaggio
        Private m_SourceID As Integer
        Private m_Source As CUser
        Private m_SourceName As String     '[Text] Nome del mittente
        Private m_SourceDescription As String '[Text] Nominativo del mittente
        Private m_TargetID As Integer
        Private m_Target As CUser
        Private m_TargetName As String '[Text] Nome del destinatario
        Private m_Message As String '[Text] Messaggio
        Private m_Visible As Boolean '[Boolean]
        Private m_DeliveryTime As Nullable(Of Date) '[Date] Data ed ora di consegna
        Private m_ReadTime As Nullable(Of Date) '[Date] Data ed ora di lettura/chiusura
        Private m_SourceSession As Integer '[Int] ID della sessione di invio
        Private m_TargetSession As Integer '[Int] ID della sessione di ricezione
        Private m_IDStanza As Integer
        Private m_Stanza As CChatRoom
        Private m_NomeStanza As String
        Private m_StatoMessaggio As StatoMessaggio

        Public Sub New()
            Me.m_Time = Nothing
            Me.m_SourceID = 0
            Me.m_Source = Nothing
            Me.m_SourceName = ""
            Me.m_SourceDescription = ""
            Me.m_TargetID = 0
            Me.m_Target = Nothing
            Me.m_TargetName = ""
            Me.m_Message = ""
            Me.m_Visible = True
            Me.m_DeliveryTime = Nothing
            Me.m_ReadTime = Nothing
            Me.m_SourceSession = 0
            Me.m_TargetSession = 0
            Me.m_IDStanza = 0
            Me.m_Stanza = Nothing
            Me.m_NomeStanza = ""
            Me.m_StatoMessaggio = StatoMessaggio.NonConsegnato
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Messages.Module
        End Function

        ''' <summary>
        ''' Restituisce o imposta lo stato del messaggio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatoMessaggio As StatoMessaggio
            Get
                Return Me.m_StatoMessaggio
            End Get
            Set(value As StatoMessaggio)
                Dim oldValue As StatoMessaggio = Me.m_StatoMessaggio
                If (oldValue = value) Then Exit Property
                Me.m_StatoMessaggio = value
                Me.DoChanged("StatoMessaggio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data e l'ora di invio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Time As Date
            Get
                Return Me.m_Time
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_Time
                If (DateUtils.Compare(oldValue, value) = 0) Then Exit Property
                Me.m_Time = value
                Me.DoChanged("Time", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente sorgente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SourceID As Integer
            Get
                Return GetID(Me.m_Source, Me.m_SourceID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.SourceID
                If (oldValue = value) Then Exit Property
                Me.m_SourceID = value
                Me.m_Source = Nothing
                Me.DoChanged("SourceID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o impostal 'utente sorgente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Source As CUser
            Get
                If (Me.m_Source Is Nothing) Then Me.m_Source = Sistema.Users.GetItemById(Me.m_SourceID)
                Return Me.m_Source
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Source
                If (oldValue Is value) Then Exit Property
                Me.m_Source = value
                Me.m_SourceID = GetID(value)
                Me.m_SourceName = ""
                Me.m_SourceDescription = ""
                If (value IsNot Nothing) Then
                    Me.m_SourceName = value.UserName
                    Me.m_SourceDescription = value.Nominativo
                End If
                Me.DoChanged("Source", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che ha inviato la richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SourceName As String
            Get
                Return Me.m_SourceName
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_SourceName
                If (oldValue = value) Then Exit Property
                Me.m_SourceName = value
                Me.DoChanged("SourceName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome lungo dell'utente sorgente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SourceDescription As String
            Get
                Return Me.m_SourceDescription
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_SourceDescription
                If (oldValue = value) Then Exit Property
                Me.m_SourceDescription = value
                Me.DoChanged("SourceDescription", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente destinatario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Target As CUser
            Get
                If (Me.m_Target Is Nothing) Then Me.m_Target = Sistema.Users.GetItemById(Me.m_TargetID)
                Return Me.m_Target
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.Target
                If (oldValue Is value) Then Exit Property
                Me.m_Target = value
                Me.m_TargetID = GetID(value)
                Me.m_TargetName = ""
                If (value IsNot Nothing) Then Me.m_TargetName = value.UserName
                Me.DoChanged("Target", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente destinazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TargetID As Integer
            Get
                Return GetID(Me.m_Target, Me.m_TargetID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.TargetID
                If (oldValue = value) Then Exit Property
                Me.m_TargetID = value
                Me.m_Target = Nothing
                Me.DoChanged("TargetID", value, oldValue)
            End Set
        End Property

        Public Property TargetName As String
            Get
                Return Me.m_TargetName
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_TargetName
                If (oldValue = value) Then Exit Property
                Me.m_TargetName = value
                Me.DoChanged("TargetName", value, oldValue)
            End Set
        End Property

        Public Property Message As String
            Get
                Return Me.m_Message
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Message
                If (oldValue = value) Then Exit Property
                Me.m_Message = value
                Me.DoChanged("Message", value, oldValue)
            End Set
        End Property

        Public Property DeliveryTime As Nullable(Of Date)
            Get
                Return Me.m_DeliveryTime
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_DeliveryTime
                If (DateUtils.Compare(oldValue, value) = 0) Then Exit Property
                Me.m_DeliveryTime = value
                Me.DoChanged("DeliveryTime", value, oldValue)
            End Set
        End Property

        Public Property ReadTime As Nullable(Of Date)
            Get
                Return Me.m_ReadTime
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_ReadTime
                If (DateUtils.Compare(oldValue, value) = 0) Then Exit Property
                Me.m_ReadTime = value
                Me.DoChanged("ReadTime", value, oldValue)
            End Set
        End Property

        Public Property SourceSession As Integer
            Get
                Return Me.m_SourceSession
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_SourceSession
                If (oldValue = value) Then Exit Property
                Me.m_SourceSession = value
                Me.DoChanged("SourceSession", value, oldValue)
            End Set
        End Property

        Public Property TargetSession As Integer
            Get
                Return Me.m_TargetSession
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_TargetSession
                If (oldValue = value) Then Exit Property
                Me.m_TargetSession = value
                Me.DoChanged("TargetSession", value, oldValue)
            End Set
        End Property

        Public Property IDStanza As Integer
            Get
                Return GetID(Me.m_Stanza, Me.m_IDStanza)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDStanza
                If (oldValue = value) Then Return
                Me.m_IDStanza = value
                Me.m_Stanza = Nothing
                Me.DoChanged("IDStanza", value, oldValue)
            End Set
        End Property

        Public Property Stanza As CChatRoom
            Get
                If (Me.m_Stanza Is Nothing) Then Me.m_Stanza = Messenger.Rooms.GetItemById(Me.m_IDStanza)
                Return Me.m_Stanza
            End Get
            Set(value As CChatRoom)
                Dim oldValue As CChatRoom = Me.Stanza
                If (oldValue Is value) Then Return
                Me.m_Stanza = value
                Me.m_IDStanza = GetID(value)
                Me.m_NomeStanza = ""
                If (value IsNot Nothing) Then Me.m_NomeStanza = value.Name
                Me.DoChanged("Stanza", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetStanza(ByVal value As CChatRoom)
            Me.m_Stanza = value
            Me.m_IDStanza = GetID(value)
        End Sub

        Public Property NomeStanza As String
            Get
                Return Me.m_NomeStanza
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_NomeStanza
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_NomeStanza = value
                Me.DoChanged("NomeStanza", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_Messenger"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return DMD.Messenger.Database
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Time = reader.Read("Time", Me.m_Time)
            Me.m_SourceID = reader.Read("SourceID", Me.m_SourceID)
            Me.m_SourceName = reader.Read("SourceName", Me.m_SourceName)
            Me.m_SourceDescription = reader.Read("SourceDescription", Me.m_SourceDescription)
            Me.m_TargetID = reader.Read("TargetID", Me.m_TargetID)
            Me.m_TargetName = reader.Read("TargetName", Me.m_TargetName)
            Me.m_Message = reader.Read("Message", Me.m_Message)
            Me.m_DeliveryTime = reader.Read("DeliveryTime", Me.m_DeliveryTime)
            Me.m_ReadTime = reader.Read("ReadTime", Me.m_ReadTime)
            Me.m_SourceSession = reader.Read("SourceSession", Me.m_SourceSession)
            Me.m_TargetSession = reader.Read("TargetSession", Me.m_TargetSession)
            Me.m_IDStanza = reader.Read("IDStanza", Me.m_IDStanza)
            Me.m_NomeStanza = reader.Read("Stanza", Me.m_NomeStanza)
            Me.m_StatoMessaggio = reader.Read("StatoMessaggio", Me.m_StatoMessaggio)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Time", Me.m_Time)
            writer.Write("TimeStr", DBUtils.ToDBDateStr(Me.m_Time))
            writer.Write("SourceID", Me.SourceID)
            writer.Write("SourceName", Me.m_SourceName)
            writer.Write("SourceDescription", Me.m_SourceDescription)
            writer.Write("TargetID", Me.TargetID)
            writer.Write("TargetName", Me.m_TargetName)
            writer.Write("Message", Me.m_Message)
            writer.Write("DeliveryTime", Me.m_DeliveryTime)
            writer.Write("ReadTime", Me.m_ReadTime)
            writer.Write("SourceSession", Me.m_SourceSession)
            writer.Write("TargetSession", Me.m_TargetSession)
            writer.Write("IDStanza", Me.IDStanza)
            writer.Write("Stanza", Me.m_Stanza)
            writer.Write("StatoMessaggio", Me.m_StatoMessaggio)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_Message
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            writer.WriteAttribute("Time", Me.m_Time)
            writer.WriteAttribute("SourceID", Me.SourceID)
            writer.WriteAttribute("SourceName", Me.m_SourceName)
            writer.WriteAttribute("SourceDescription", Me.m_SourceDescription)
            writer.WriteAttribute("TargetID", Me.TargetID)
            writer.WriteAttribute("TargetName", Me.m_TargetName)
            writer.WriteAttribute("DeliveryTime", Me.m_DeliveryTime)
            writer.WriteAttribute("ReadTime", Me.m_ReadTime)
            writer.WriteAttribute("SourceSession", Me.m_SourceSession)
            writer.WriteAttribute("TargetSession", Me.m_TargetSession)
            writer.WriteAttribute("IDStanza", Me.IDStanza)
            writer.WriteAttribute("NomeStanza", Me.m_NomeStanza)
            writer.WriteAttribute("StatoMessaggio", Me.m_StatoMessaggio)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Message", Me.m_Message)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case fieldName
                Case "Time" : Me.m_Time = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "SourceID" : Me.m_SourceID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SourceName" : Me.m_SourceName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SourceDescription" : Me.m_SourceDescription = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TargetID" : Me.m_TargetID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TargetName" : Me.m_TargetName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Message" : Me.m_Message = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DeliveryTime" : Me.m_DeliveryTime = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ReadTime" : Me.m_ReadTime = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "SourceSession" : Me.m_SourceSession = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TargetSession" : Me.m_TargetSession = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDStanza" : Me.m_IDStanza = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeStanza" : Me.m_NomeStanza = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "StatoMessaggio" : Me.m_StatoMessaggio = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : Call MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Function CompareTo(ByVal obj As CMessage) As Integer
            Return DateUtils.Compare(Me.Time, obj.Time)
        End Function

        Private Function _CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(CType(obj, CMessage))
        End Function

    End Class

End Class
 