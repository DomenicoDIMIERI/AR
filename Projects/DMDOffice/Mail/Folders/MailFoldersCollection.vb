Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports DMD.Net.Mime
Imports DMD.Databases
Imports DMD.Sistema

Partial Class Office

    <Serializable> _
    Public Class MailFoldersCollection
        Inherits CCollection(Of MailFolder)

        <NonSerialized> Private m_Application As MailApplication
        <NonSerialized> Private m_User As CUser
        <NonSerialized> Private m_Parent As MailFolder

        Public Sub New()
            Me.m_Application = Nothing
            Me.m_User = Nothing
            Me.m_Parent = Nothing
        End Sub

        Public Sub New(ByVal parent As MailFolder)
            Me.New()
            If (parent Is Nothing) Then Throw New ArgumentNullException("parent")
            Me.Load(parent)
        End Sub

        Public Sub New(ByVal user As CUser)
            Me.New()
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            Me.Load(user)
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Application IsNot Nothing) Then DirectCast(value, MailFolder).SetApplication(Me.m_Application)
            If (Me.m_User IsNot Nothing) Then DirectCast(value, MailFolder).SetUtente(Me.m_User)
            If (Me.m_Parent IsNot Nothing) Then DirectCast(value, MailFolder).SetParent(Me.m_Parent)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Application IsNot Nothing) Then DirectCast(newValue, MailFolder).SetApplication(Me.m_Application)
            If (Me.m_User IsNot Nothing) Then DirectCast(newValue, MailFolder).SetUtente(Me.m_User)
            If (Me.m_Parent IsNot Nothing) Then DirectCast(newValue, MailFolder).SetParent(Me.m_Parent)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Friend Sub Load(ByVal parent As MailFolder)
            Dim cursor As MailFolderCursor = Nothing
            Try
                Me.m_Parent = parent
                Me.m_User = parent.Utente
                Me.SetApplication(parent.Application)
                Me.Clear()
                If (GetID(Me.m_Parent) = 0) Then Exit Sub
                cursor = New MailFolderCursor
                cursor.ParentID.Value = GetID(Me.m_Parent)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = True
                cursor.ID.SortOrder = SortEnum.SORT_ASC
                While Not cursor.EOF
                    MyBase.Add(cursor.Item)
                    cursor.MoveNext()
                End While

            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub

        Protected Friend Sub Load(ByVal user As CUser)
            Dim cursor As MailFolderCursor = Nothing
            Try
                Me.Clear()
                Me.m_User = user
                If (GetID(user) = 0) Then Exit Sub
                cursor = New MailFolderCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = True
                cursor.ParentID.Value = 0
                cursor.ParentID.IncludeNulls = True
                cursor.IDutente.Value = GetID(user)
                While Not cursor.EOF
                    MyBase.Add(cursor.Item)
                    cursor.MoveNext()
                End While
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Sub

        Public Function GetItemByName(ByVal path As String) As MailFolder
            path = Replace(Trim(path), "/", "\")
            If (path = "") Then Return Nothing
            Dim p As Integer = InStr(path, "\")
            Dim name As String = ""
            If (p > 0) Then
                name = Trim(Left(path, p - 1))
                path = Trim(Mid(path, p + 1))
            Else
                name = path
            End If
            If (name = "") Then Return Me.m_Application.Root.Childs.GetItemByName(path)
            If (name = "..") Then
                If (Me.m_Parent Is Nothing) Then Return Nothing
                Return Me.m_Parent.Childs.GetItemByName(path)
            End If
            Dim ret As MailFolder = Nothing
            For Each f As MailFolder In Me
                If (f.Name = name) Then Return f
            Next
            If (ret Is Nothing) Then Return Nothing
            If (path = "") Then Return ret
            Return ret.Childs.GetItemByName(path)
        End Function

        Public Overloads Function Add(ByVal folderName As String) As MailFolder
            folderName = Strings.Trim(folderName)
            If (folderName.IndexOf("/") >= 0) Then Throw New ArgumentException("Il percorso non può contenere caratteri speciali")
            Dim f As New MailFolder
            f.Name = folderName
            f.Stato = ObjectStatus.OBJECT_VALID
            Me.Add(f)
            f.Save()
            Return f
        End Function

        Protected Friend Sub SetUser(ByVal user As CUser)
            Me.m_User = user
            If (user IsNot Nothing) Then
                For Each f As MailFolder In Me
                    f.SetUtente(user)
                Next
            End If
        End Sub

        Protected Friend Sub SetParent(ByVal p As MailFolder)
            Me.m_Parent = p
            If (p IsNot Nothing) Then
                For Each f As MailFolder In Me
                    f.SetParent(p)
                Next
            End If
        End Sub

        Protected Friend Sub SetApplication(ByVal a As MailApplication)
            Me.m_Application = a
            If (a IsNot Nothing) Then
                For Each f As MailFolder In Me
                    f.SetApplication(a)
                Next
            End If
        End Sub

    End Class

End Class