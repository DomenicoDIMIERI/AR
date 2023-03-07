Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Office
Imports DMD.Internals

Namespace Internals

    Public NotInheritable Class CTicketCategoriesClass
        Inherits CGeneralClass(Of CTicketCategory)

        Private m_UserAllowedCategories As CKeyCollection(Of CCollection(Of CTicketCategory))

        Friend Sub New()
            MyBase.New("modTicketsCategories", GetType(CTicketCategoryCursor), -1)
            Me.m_UserAllowedCategories = Nothing
        End Sub

        Private Function Find(ByVal list As System.Collections.ArrayList, item As String) As Boolean
            For Each s As String In list
                If (Strings.Compare(s, item) = 0) Then Return True
            Next
            Return False
        End Function

        Public Function GetArraySottocategorie(cat As String) As String()
            Dim ret As New System.Collections.ArrayList
            Dim c As CTicketCategory
            cat = Trim(cat & "")
            If (cat = "") Then
                For Each c In Me.LoadAll
                    If c.Categoria = "" AndAlso Not Me.Find(ret, c.Sottocategoria) Then ret.Add(c.Sottocategoria)
                Next
            Else
                For Each c In Me.LoadAll
                    If Strings.Compare(c.Categoria, cat) = 0 AndAlso Not Me.Find(ret, c.Sottocategoria) Then ret.Add(c.Sottocategoria)
                Next
            End If
            Return ret.ToArray(GetType(String))
        End Function

        ''' <summary>
        ''' Restituisce l'oggetto in base a categoria e sottocategoria
        ''' </summary>
        ''' <param name="categoria"></param>
        ''' <param name="sottocategoria"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetItemByName(ByVal categoria As String, ByVal sottocategoria As String) As CTicketCategory
            categoria = Strings.Trim(categoria)
            sottocategoria = Strings.Trim(sottocategoria)
            For Each item As CTicketCategory In Me.LoadAll
                If item.Categoria = categoria AndAlso item.Sottocategoria = sottocategoria Then Return item
            Next
            Return Nothing
        End Function

        ''' <summary>
        ''' Restituisce gli utenti a cui vengono inviati le notifiche per la categoria e la sottocategoria.
        ''' Agli utenti di { categoria, sottocategoria } vengono aggiunti anche gli utenti di { categoria, NULL }
        ''' </summary>
        ''' <param name="categoria"></param>
        ''' <param name="sottocategoria"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetTargetUsers(ByVal categoria As String, ByVal sottocategoria As String) As CCollection(Of CUser)
            Dim targetUser As New CKeyCollection(Of CUser)
            Dim cat As CTicketCategory = TicketCategories.GetItemByName(categoria, "")
            If cat IsNot Nothing Then
                For Each user As CUser In cat.NotifyUsers
                    If Not targetUser.ContainsKey(user.UserName) Then targetUser.Add(user.UserName, user)
                Next
            End If

            cat = TicketCategories.GetItemByName(categoria, sottocategoria)
            If cat IsNot Nothing Then
                For Each user As CUser In cat.NotifyUsers
                    If Not targetUser.ContainsKey(user.UserName) Then targetUser.Add(user.UserName, user)
                Next
            End If

            Return New CCollection(Of CUser)(targetUser)
        End Function

        Public Function GetNotifiedUsers(ByVal categoria As String, ByVal sottocategoria As String) As CCollection(Of CUser)
            Dim targetUser As New CKeyCollection(Of CUser)

            For Each user As CUser In Tickets.GruppoResponsabili.Members
                If Not targetUser.ContainsKey(user.UserName) Then targetUser.Add(user.UserName, user)
            Next

            Dim cat As CTicketCategory = TicketCategories.GetItemByName(categoria, "")
            If cat IsNot Nothing Then
                For Each user As CUser In cat.NotifyUsers
                    If Not targetUser.ContainsKey(user.UserName) Then targetUser.Add(user.UserName, user)
                Next
            End If

            cat = TicketCategories.GetItemByName(categoria, sottocategoria)
            If cat IsNot Nothing Then
                For Each user As CUser In cat.NotifyUsers
                    If Not targetUser.ContainsKey(user.UserName) Then targetUser.Add(user.UserName, user)
                Next
            End If

            For Each user As CUser In Tickets.GruppoEsclusi.Members
                If targetUser.ContainsKey(user.UserName) Then targetUser.RemoveByKey(user.UserName)
            Next

            Return New CCollection(Of CUser)(targetUser)
        End Function

        Private Sub BuildUserCat()
            SyncLock Me
                If (Me.m_UserAllowedCategories Is Nothing) Then
                    Me.m_UserAllowedCategories = New CKeyCollection(Of CCollection(Of CTicketCategory))
                    For Each cat As CTicketCategory In Me.LoadAll
                        For Each user As CUser In cat.NotifyUsers
                            Dim col As CCollection(Of CTicketCategory) = Me.m_UserAllowedCategories.GetItemByKey(user.UserName)
                            If (col Is Nothing) Then
                                col = New CCollection(Of CTicketCategory)
                                Me.m_UserAllowedCategories.Add(user.UserName, col)
                            End If
                            col.Add(cat)
                        Next
                    Next
                End If
                
            End SyncLock
        End Sub

        Public Function GetUserAllowedCategories(ByVal user As CUser) As CCollection(Of CTicketCategory)
            SyncLock Me
                Me.BuildUserCat()
                Dim ret As CCollection(Of CTicketCategory)
                ret = Me.m_UserAllowedCategories.GetItemByKey(user.UserName)
                If (ret Is Nothing) Then ret = New CCollection(Of CTicketCategory)
                Return ret
            End SyncLock
        End Function

        Protected Friend Sub InvalidateUserAllowedCategories()
            SyncLock Me
                Me.m_UserAllowedCategories = Nothing
            End SyncLock
        End Sub


    End Class

End Namespace

Partial Public Class Office

    Private Shared m_TicketCategories As CTicketCategoriesClass = Nothing

    Public Shared ReadOnly Property TicketCategories As CTicketCategoriesClass
        Get
            If (m_TicketCategories Is Nothing) Then m_TicketCategories = New CTicketCategoriesClass
            Return m_TicketCategories
        End Get
    End Property

End Class