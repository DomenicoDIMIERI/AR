Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Internals
Imports DMD.Sistema

Namespace Internals


    Public NotInheritable Class CGroupsClass
        Inherits CGeneralClass(Of CGroup)

        Friend Sub New()
            MyBase.New("Gruppi", GetType(CGroupCursor), -1)
        End Sub


        Public Function GetItemByName(ByVal value As String) As CGroup
            value = Trim(value)
            If (value = "") Then Return Nothing
            For Each grp As CGroup In Me.LoadAll
                If Strings.Compare(grp.GroupName, value) = 0 Then Return grp
            Next
            Return Nothing
        End Function

        Private Function ContainsKey(ByVal value As String) As Boolean
            value = Trim(value)
            If (value = "") Then Return False
            For Each item As CGroup In Me.LoadAll
                If Strings.Compare(item.GroupName, value) = 0 Then Return True
            Next
            Return False
        End Function

        Public Function GetFirstAvailableGroupName(ByVal baseName As String) As String
            SyncLock Me
                baseName = Trim(baseName)
                Dim nome As String = baseName
                Dim i As Integer
                Dim t As Boolean
                t = Me.ContainsKey(nome)
                i = 0
                While (t)
                    i = i + 1
                    nome = baseName & " (" & i & ")"
                    t = Me.ContainsKey(nome)
                End While
                Return nome
            End SyncLock
        End Function

        Public Function GetFirstAvailableGroupName() As String
            Return GetFirstAvailableGroupName("Gruppo")
        End Function

        Public NotInheritable Class CKnownGroupsClass
            Private m_Administrators As CGroup
            Private m_Users As CGroup
            Private m_Guests As CGroup


            Friend Sub New()
            End Sub

            ''' <summary>
            ''' Restituisce il gruppo predefinito Administrators
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property Administrators As CGroup
                Get
                    If m_Administrators Is Nothing Then
                        m_Administrators = Sistema.Groups.GetItemByName("Administrators")
                        If (m_Administrators Is Nothing) Then
                            m_Administrators = New CGroup("Administrators")
                            m_Administrators.IsBuiltIn = True
                            m_Administrators.Stato = ObjectStatus.OBJECT_VALID
                            m_Administrators.ForceUser(Sistema.Users.KnownUsers.SystemUser)
                            m_Administrators.Save()
                        End If
                    End If
                    Return m_Administrators
                End Get
            End Property

            ''' <summary>
            ''' Restituisce il gruppo predefinito degli utenti non connessi
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property Guests As CGroup
                Get
                    If m_Guests Is Nothing Then
                        m_Guests = Sistema.Groups.GetItemByName("Guests")
                        If (m_Guests Is Nothing) Then
                            m_Guests = New CGroup("Guests")
                            m_Guests.IsBuiltIn = True
                            m_Guests.Stato = ObjectStatus.OBJECT_VALID
                            m_Guests.ForceUser(Sistema.Users.KnownUsers.SystemUser)
                            m_Guests.Save()
                        End If
                    End If
                    Return m_Guests
                End Get
            End Property

            ''' <summary>
            ''' Restituisce il gruppo predefinito degli utenti definiti
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property Users As CGroup
                Get
                    If m_Users Is Nothing Then
                        m_Users = Sistema.Groups.GetItemByName("Users")
                        If (m_Users Is Nothing) Then
                            m_Users = New CGroup("Users")
                            m_Users.IsBuiltIn = True
                            m_Users.Stato = ObjectStatus.OBJECT_VALID
                            m_Users.ForceUser(Sistema.Users.KnownUsers.SystemUser)
                            m_Users.Save()
                        End If
                    End If
                    Return m_Users
                End Get
            End Property

        End Class

        Private m_KnownGroups As CKnownGroupsClass = Nothing

        Public ReadOnly Property KnownGroups As CKnownGroupsClass
            Get
                If (m_KnownGroups Is Nothing) Then m_KnownGroups = New CKnownGroupsClass
                Return Me.m_KnownGroups
            End Get
        End Property
    End Class

End Namespace

Partial Public Class Sistema


    Private Shared m_Groups As CGroupsClass = Nothing

    Public Shared ReadOnly Property Groups As CGroupsClass
        Get
            SyncLock Sistema.lock
                If (m_Groups Is Nothing) Then m_Groups = New CGroupsClass
                Return m_Groups
            End SyncLock
        End Get
    End Property

End Class

