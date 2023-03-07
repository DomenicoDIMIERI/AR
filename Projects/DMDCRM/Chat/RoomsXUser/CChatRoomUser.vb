Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls

Partial Public Class Messenger


    Public Enum ChatUserFlags As Integer
        None = 0
        Online = 1
        'Hidden = 2
    End Enum

    ''' <summary>
    ''' Rappresenta le statistiche utente per una singola stanza
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class CChatRoomUser
        Inherits DBObject

        Private m_Flags As ChatUserFlags
        Private m_IDStanza As Integer
        <NonSerialized> Private m_Stanza As CChatRoom
        Private m_UserID As Integer
        <NonSerialized> Private m_User As CUser
        Private m_LastVisit As Nullable(Of Date)
        Private m_FirstVisit As Nullable(Of Date)
        Private m_Params As CKeyCollection

        Public Sub New()
            Me.m_Flags = ChatUserFlags.None
            Me.m_IDStanza = 0
            Me.m_Stanza = Nothing
            Me.m_UserID = 0
            Me.m_User = Nothing
            Me.m_LastVisit = Nothing
            Me.m_FirstVisit = Nothing
            Me.m_Params = Nothing
        End Sub

        Public Property Flags As ChatUserFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As ChatUserFlags)
                Dim oldValue As ChatUserFlags = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        Public Property IDStanza As Integer
            Get
                Return GetID(Me.m_Stanza, Me.m_IDStanza)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.IDStanza
                If (oldValue = value) Then Exit Property
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
                Me.DoChanged("Stanza", value, oldValue)
            End Set
        End Property

        Public Property UserID As Integer
            Get
                Return GetID(Me.m_User, Me.m_UserID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.UserID
                If (oldValue = value) Then Exit Property
                Me.m_UserID = value
                Me.m_User = Nothing
                Me.DoChanged("UserID", value, oldValue)
            End Set
        End Property

        Public Property User As CUser
            Get
                If (Me.m_User Is Nothing) Then Me.m_User = Sistema.Users.GetItemById(Me.m_UserID)
                Return Me.m_User
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.User
                If (oldValue Is value) Then Return
                Me.m_User = value
                Me.m_UserID = GetID(value)
                Me.DoChanged("User", value, oldValue)
            End Set
        End Property

        Public Property FirstVisit As Nullable(Of Date)
            Get
                Return Me.m_FirstVisit
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_FirstVisit
                If DateUtils.Compare(oldValue, value) = 0 Then Exit Property
                Me.m_FirstVisit = value
                Me.DoChanged("FirstVisit", value, oldValue)
            End Set
        End Property

        Public Property LastVisit As Nullable(Of Date)
            Get
                Return Me.m_LastVisit
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_LastVisit
                If DateUtils.Compare(oldValue, value) = 0 Then Exit Property
                Me.m_LastVisit = value
                Me.DoChanged("LastVisit", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Parameters As CKeyCollection
            Get
                If (Me.m_Params Is Nothing) Then Me.m_Params = New CKeyCollection
                Return Me.m_Params
            End Get
        End Property

        Public Function CountUnreadMessages() As Integer
            Dim cursor As New CMessagesCursor
            cursor.TargetID.Value = 0
            cursor.TargetID.IncludeNulls = True
            'cursor.ReadTime.Value = Nothing
            cursor.StatoMessaggio.ValueIn({StatoMessaggio.NonConsegnato, StatoMessaggio.NonLetto})
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDStanza.Value = GetID(Me)
            Dim cnt As Integer = cursor.Count
            cursor.Dispose()
            Return cnt
        End Function

        Public Function GetUnreadMessages() As CCollection(Of CMessage)
            Dim ret As New CCollection(Of CMessage)
            Dim cursor As New CMessagesCursor
            cursor.TargetID.Value = 0
            cursor.TargetID.IncludeNulls = True
            'cursor.ReadTime.Value = Nothing
            cursor.StatoMessaggio.ValueIn({StatoMessaggio.NonConsegnato, StatoMessaggio.NonLetto})
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDStanza.Value = GetID(Me)
            While Not cursor.EOF
                Dim m As CMessage = cursor.Item
                m.SetStanza(Me.Stanza)
                ret.Add(m)
                cursor.MoveNext()
            End While
            cursor.Dispose()
            Return ret
        End Function


        Protected Overrides Function GetConnection() As CDBConnection
            Return DMD.Messenger.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ChatRoomUsers"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_IDStanza = reader.Read("IDStanza", Me.m_IDStanza)
            Me.m_UserID = reader.Read("UserID", Me.m_UserID)
            Me.m_LastVisit = reader.Read("LastVisit", Me.m_LastVisit)
            Me.m_FirstVisit = reader.Read("FirstVisit", Me.m_FirstVisit)
            Dim tmp As String = reader.Read("Parameters", "")
            If (tmp <> "") Then Me.m_Params = CType(XML.Utils.Serializer.Deserialize(tmp), CKeyCollection)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Flags", Me.m_Flags)
            writer.Write("IDStanza", Me.IDStanza)
            writer.Write("UserID", Me.UserID)
            writer.Write("LastVisit", Me.m_LastVisit)
            writer.Write("FirstVisit", Me.m_FirstVisit)
            writer.Write("Parameters", XML.Utils.Serializer.Serialize(Me.Parameters))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("RoomID", Me.IDStanza)
            writer.WriteAttribute("UserID", Me.UserID)
            writer.WriteAttribute("LastVisit", Me.m_LastVisit)
            writer.WriteAttribute("FirstVisit", Me.m_FirstVisit)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Parameters", Me.Parameters)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Flags" : Me.m_Flags = CType(XML.Utils.Serializer.DeserializeInteger(fieldValue), ChatUserFlags)
                Case "IDStanza" : Me.m_IDStanza = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "UserID" : Me.m_UserID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "LastVisit" : Me.m_LastVisit = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "FirstVisit" : Me.m_FirstVisit = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Parameters" : Me.m_Params = CType(XML.Utils.Serializer.ToObject(fieldValue), CKeyCollection)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

    End Class

End Class
 