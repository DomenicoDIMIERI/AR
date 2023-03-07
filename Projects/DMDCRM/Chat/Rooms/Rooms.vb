Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Internals
Imports DMD.Messenger

Namespace Internals
    ''' <summary>
    ''' Stanze
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CChatRoomsClass
        Inherits CGeneralClass(Of CChatRoom)

        Private m_MainRoom As CChatRoom

        Public Sub New()
            MyBase.New("modChatRooms", GetType(CChatRoomCursor), -1)
        End Sub

        Public Function GetItemByName(ByVal name As String) As CChatRoom
            name = Strings.Trim(name)
            For Each room As CChatRoom In Me.LoadAll
                If (room.Stato = ObjectStatus.OBJECT_VALID AndAlso Strings.Compare(room.Name, name, CompareMethod.Text) = 0) Then Return room
            Next
            Return Nothing
        End Function

        Public ReadOnly Property MainRoom As CChatRoom
            Get
                If (Me.m_MainRoom Is Nothing) Then
                    Me.m_MainRoom = Me.GetItemByName("main")
                End If
                If (Me.m_MainRoom Is Nothing) Then
                    Me.m_MainRoom = New CChatRoom()
                    Me.m_MainRoom.Name = "main"
                    Me.m_MainRoom.Stato = ObjectStatus.OBJECT_VALID
                    Me.m_MainRoom.Save()
                    For Each u As CUser In Sistema.Users.LoadAll
                        If (u.Stato = ObjectStatus.OBJECT_VALID) Then
                            Me.m_MainRoom.AddMember(u)
                        End If
                    Next
                End If
                Return Me.m_MainRoom
            End Get
        End Property

    End Class
End Namespace

Partial Public Class Messenger



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
 