Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Partial Public Class Messenger

    ''' <summary>
    ''' Stanze
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CChatRoomsClass
        Inherits CGeneralClass(Of CChatRoom)

        Public Sub New()
            MyBase.New("modChatRooms", GetType(CChatRoomCursor), -1)
        End Sub


    End Class

    Private Shared m_Rooms As CChatRoomsClass = Nothing

    ''' <summary>
    ''' Accede alle stanze
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property Rooms As CChatRoomsClass
        Get
            If (m_Rooms Is Nothing) Then m_Rooms = New CChatRoomsClass
            Return m_Rooms
        End Get
    End Property

End Class
 