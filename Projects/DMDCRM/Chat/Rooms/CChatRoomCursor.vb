Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.CustomerCalls

Partial Public Class Messenger

    ''' <summary>
    ''' Cursore sulla tabella delle stanze
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CChatRoomCursor
        Inherits DBObjectCursor(Of CChatRoom)

        Private m_Name As New CCursorFieldObj(Of String)("Name")

        Public Sub New()
        End Sub

        Public ReadOnly Property Name As CCursorFieldObj(Of String)
            Get
                Return Me.m_Name
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return DMD.Messenger.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Messenger.Rooms.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_ChatRooms"
        End Function
         

    End Class

End Class
 