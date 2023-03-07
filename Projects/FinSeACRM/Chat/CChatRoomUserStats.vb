Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls

Partial Public Class Messenger

    ''' <summary>
    ''' Rappresenta le statistiche utente per una singola stanza
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CChatRoomUserStats
        Inherits DBObjectBase

        Private m_RoomName As String
        Private m_UserID As Integer
        Private m_LastVisit As Nullable(Of Date)
        Private m_FirstVisit As Nullable(Of Date)
        Private m_UnreadMessages As Integer

        Public Sub New()
            Me.m_RoomName = ""
            Me.m_UserID = 0
            Me.m_LastVisit = Nothing
            Me.m_FirstVisit = Nothing
            Me.m_UnreadMessages = 0
        End Sub

        Public Property RoomName As String
            Get
                Return Me.m_RoomName
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_RoomName
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_RoomName = value
                Me.DoChanged("RoomName", value, oldValue)
            End Set
        End Property

        Public Property UserID As Integer
            Get
                Return Me.m_UserID
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_UserID
                If (oldValue = value) Then Exit Property
                Me.m_UserID = value
                Me.DoChanged("UserID", value, oldValue)
            End Set
        End Property

        Public Property FirstVisit As Nullable(Of Date)
            Get
                Return Me.m_FirstVisit
            End Get
            Set(value As Nullable(Of Date))
                Dim oldValue As Nullable(Of Date) = Me.m_FirstVisit
                If (oldValue = value) Then Exit Property
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
                If (oldValue = value) Then Exit Property
                Me.m_LastVisit = value
                Me.DoChanged("LastVisit", value, oldValue)
            End Set
        End Property

        Public Property UnreadMessages As Integer
            Get
                Return Me.m_UnreadMessages
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_UnreadMessages
                If (oldValue = value) Then Exit Property
                Me.m_UnreadMessages = value
                Me.DoChanged("UnreadMessages", value, oldValue)
            End Set
        End Property

         
        Protected Overrides Function GetConnection() As CDBConnection
            Return CRM.Database
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ChatRoomUserStats"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

    End Class

End Class
 