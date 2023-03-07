Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls

Partial Public Class Messenger

    ''' <summary>
    ''' Cursore sulla tabella degli utenti x stanza 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CChatRoomUserCursor
        Inherits DBObjectCursor(Of CChatRoomUser)

        Private m_Flags As New CCursorField(Of ChatUserFlags)("Flags")
        Private m_IDStanza As New CCursorField(Of Integer)("IDStanza")
        Private m_UserID As New CCursorField(Of Integer)("UserID")
        Private m_LastVisit As New CCursorField(Of DateTime)("LastVisit")
        Private m_FirstVisit As New CCursorField(Of DateTime)("FirstVisit")

        Public Sub New()
        End Sub

        Public ReadOnly Property Flags As CCursorField(Of ChatUserFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Public ReadOnly Property IDStanza As CCursorField(Of Integer)
            Get
                Return Me.m_IDStanza
            End Get
        End Property

        Public ReadOnly Property UserID As CCursorField(Of Integer)
            Get
                Return Me.m_UserID
            End Get
        End Property

        Public ReadOnly Property FirstVisit As CCursorField(Of DateTime)
            Get
                Return Me.m_FirstVisit
            End Get
        End Property

        Public ReadOnly Property LastVisit As CCursorField(Of DateTime)
            Get
                Return Me.m_LastVisit
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return DMD.Messenger.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ChatRoomUsers"
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CChatRoomUser
        End Function

    End Class

End Class
 