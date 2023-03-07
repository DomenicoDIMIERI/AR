Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    <Serializable> _
    Public Class CGroupMembersCollection
        Inherits CCollection(Of CUser)

        <NonSerialized> _
        Private m_Group As CGroup

        Public Sub New()
            Me.m_Group = Nothing
            MyBase.Comparer = New CUserComparer
        End Sub

        Public Sub New(ByVal group As CGroup)
            Me.New()
            Me.Initialize(group)
        End Sub

        Public ReadOnly Property Group As CGroup
            Get
                Return Me.m_Group
            End Get
        End Property

        Friend Function Initialize(ByVal group As CGroup) As Boolean
            If (group Is Nothing) Then Throw New ArgumentNullException("group")
            MyBase.Clear()
            Me.m_Group = group
            If (GetID(group) <> 0) Then
                Dim dbRis As System.Data.IDataReader
                Dim dbSQL As String
                Dim Item As CUser
                dbSQL = "SELECT [User] FROM [tbl_UsersXGroup] WHERE [Group]=" & GetID(group)
                dbRis = APPConn.ExecuteReader(dbSQL)
                While dbRis.Read
                    Item = Sistema.Users.GetItemById(dbRis("User"))
                    If Not (Item Is Nothing) Then Call MyBase.Add(Item)
                End While
                dbRis.Dispose()
                dbRis = Nothing
                Me.Sort()
            End If
            Return True
        End Function

        ''' <summary>
        ''' Restituisce l'indice dell'utente con l'ID specificato
        ''' </summary>
        ''' <param name="userID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function IndexOf(ByVal userID As Integer) As Integer
            For i As Integer = 0 To Me.Count - 1
                If GetID(Me(i)) = userID Then Return i
            Next
            Return -1
        End Function

        ''' <summary>
        ''' Restituisce un valore booleano che indica se l'utente appartiene a questo gruppo
        ''' </summary>
        ''' <param name="userID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shadows Function Contains(ByVal userID As Integer) As Boolean
            Return Me.IndexOf(userID) >= 0
        End Function

        Public Shadows Function Contains(ByVal user As CUser) As Boolean
            Return Me.IndexOf(user) >= 0
        End Function

        ''' <summary>
        ''' Rimuove l'utente specificato da questo gruppo
        ''' </summary>
        ''' <param name="userID"></param>
        ''' <remarks></remarks>
        Public Shadows Sub Remove(ByVal userID As Integer)
            Me.Remove(Sistema.Users.GetItemById(userID))
        End Sub

        Public Shadows Sub Remove(ByVal user As CUser)
            SyncLock user
                Dim i As Integer = Me.IndexOf(user)
                MyBase.RemoveAt(i)
                APPConn.ExecuteCommand("DELETE * FROM [tbl_UsersXGroup] WHERE [Group]=" & GetID(Me.m_Group) & " AND [User]=" & GetID(user))
                If (user.Groups.Contains(Me.Group)) Then user.Groups.Remove(Me.Group)
            End SyncLock
        End Sub

        ''' <summary>
        ''' Aggiunte l'utente specificato da questo gruppo
        ''' </summary>
        ''' <param name="value"></param>
        ''' <remarks></remarks>
        Public Shadows Sub Add(ByVal value As CUser)
            SyncLock value
                Dim dbSQL As String
                Dim userID As Integer = GetID(value)
                dbSQL = "INSERT INTO [tbl_UsersXGroup] ([Group], [User]) VALUES (" & GetID(Me.m_Group) & ", " & userID & ")"
                APPConn.ExecuteCommand(dbSQL)
                MyBase.Add(value)
                If Not value.Groups.Contains(Me.Group) Then value.Groups.Add(Me.Group)
            End SyncLock
        End Sub

        Public Shadows Sub Add(ByVal UserID As Integer)
            Me.Add(Sistema.Users.GetItemById(UserID))
        End Sub


    End Class


End Class

