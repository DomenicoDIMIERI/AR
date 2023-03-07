Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls

Partial Public Class Messenger

    ''' <summary>
    ''' Rappresenta una stanza della chat
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CChatRoom
        Inherits DBObject
        Implements IComparable

        Private m_Name As String
        Private m_Parameters As CKeyCollection

        Public Sub New()
            Me.m_Name = ""
            Me.m_Parameters = Nothing
        End Sub

        Public Property Name As String
            Get
                Return Me.m_Name
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Name
                If (oldValue = value) Then Exit Property
                Me.m_Name = value
                Me.DoChanged("Name", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Parameters As CKeyCollection
            Get
                If (Me.m_Parameters Is Nothing) Then Me.m_Parameters = New CKeyCollection
                Return Me.m_Parameters
            End Get
        End Property

        Public Function GetMembers() As CCollection(Of CChatRoomUser)
            Dim ret As New CCollection(Of CChatRoomUser)
            Dim cursor As New CChatRoomUserCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDStanza.Value = GetID(Me)
            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
            cursor.Dispose()

            Return ret
        End Function

        Public Function GetMember(ByVal u As CUser) As CChatRoomUser
            If (u Is Nothing) Then Throw New ArgumentNullException("user")
            Dim cursor As New CChatRoomUserCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDStanza.Value = GetID(Me)
            cursor.UserID.Value = GetID(u)
            cursor.IgnoreRights = True
            cursor.PageSize = 1
            Dim ret As CChatRoomUser = cursor.Item
            cursor.Dispose()
            Return ret
        End Function

        Public Function HasMember(ByVal u As CUser) As Boolean
            Return (Me.GetMember(u) IsNot Nothing)
        End Function

        Public Function AddMember(u As CUser) As CChatRoomUser
            If (Me.HasMember(u)) Then Throw New ArgumentException("utente già nella stanza")
            Dim item As New CChatRoomUser
            item.Stanza = Me
            item.User = u
            item.Stato = ObjectStatus.OBJECT_VALID
            item.Save()
            Return item
        End Function

        Public Function RemoveMember(u As CUser) As CChatRoomUser
            Dim item As CChatRoomUser = Me.GetMember(u)
            If (item Is Nothing) Then Throw New ArgumentException("utente non nella stanza")
            item.Delete()
            Return item
        End Function

        Public Overrides Function ToString() As String
            Return Me.Name
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return DMD.Messenger.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Messenger.Rooms.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ChatRooms"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Name = reader.Read("Name", Me.m_Name)
            Dim tmp As String = reader.Read("Parameters", "")
            If (tmp <> "") Then Me.m_Parameters = CType(XML.Utils.Serializer.Deserialize(tmp), CKeyCollection)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Name", Me.m_Name)
            writer.Write("Parameters", XML.Utils.Serializer.Serialize(Me.Parameters))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Name", Me.m_Name)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Parameters", Me.Parameters)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Name" : Me.m_Name = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Parameters" : Me.m_Parameters = CType(XML.Utils.Serializer.ToObject(fieldValue), CKeyCollection)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Function CompareTo(ByVal obj As CChatRoom) As Integer
            Return Strings.Compare(Me.Name, obj.Name, CompareMethod.Text)
        End Function

        Private Function _CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(CType(obj, CChatRoom))
        End Function

    End Class

End Class
 