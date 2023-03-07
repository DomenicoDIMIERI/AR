Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

  
    Public NotInheritable Class CTeamManagersClass
        Inherits CGeneralClass(Of CTeamManager)

        Private m_DefaultSetPremi As CSetPremi

        Friend Sub New()
            MyBase.New("modTeamManager", GetType(CTeamManagersCursor), -1)
        End Sub
  

        Public Function GetSetPremiById(ByVal id As Integer) As CSetPremi
            If (id = 0) Then Return Nothing
            Dim cursor As New CSetPremiCursor
            Dim ret As CSetPremi
            cursor.PageSize = 1
            cursor.IgnoreRights = True
            cursor.ID.Value = id
            ret = cursor.Item
            cursor.Dispose()
            Return ret
        End Function

        Public Property DefaultSetPremi As CSetPremi
            Get
                If (m_DefaultSetPremi Is Nothing) Then
                    Dim idSet As Integer = CQSPD.Module.Settings.GetValueInt("TeamManagers_DefSetPremi", 0)
                    m_DefaultSetPremi = TeamManagers.GetSetPremiById(idSet)
                    If m_DefaultSetPremi Is Nothing Then
                        m_DefaultSetPremi = New CSetPremi
                        m_DefaultSetPremi.Stato = ObjectStatus.OBJECT_VALID
                        m_DefaultSetPremi.Save()
                    End If
                End If
                Return m_DefaultSetPremi
            End Get
            Set(value As CSetPremi)
                If (GetID(value) = 0) Then Throw New ArgumentNullException("Il set premi predefinito non può essere Null")
                CQSPD.Module.Settings.SetValueInt("TeamManagers_DefSetPremi", GetID(value))
                m_DefaultSetPremi = value
            End Set
        End Property

        Public Function GetItemByName(ByVal nominativo As String) As CTeamManager
            nominativo = Trim(nominativo)
            If (nominativo = "") Then Return Nothing
            For Each tm As CTeamManager In Me.LoadAll
                If (tm.Stato = ObjectStatus.OBJECT_VALID AndAlso Strings.Compare(tm.Nominativo, nominativo) = 0) Then
                    Return tm
                End If
            Next
            Return Nothing
        End Function

        Public Function GetItemByUser(ByVal user As CUser) As CTeamManager
            If (user Is Nothing) Then Throw New ArgumentNullException
            If (GetID(user) = 0) Then Return Nothing
            For Each tm As CTeamManager In Me.LoadAll
                If (tm.Stato = ObjectStatus.OBJECT_VALID AndAlso tm.UserID = GetID(user)) Then
                    Return tm
                End If
            Next
            Return Nothing
        End Function

        Public Function GetItemByUser(ByVal userID As Integer) As CTeamManager
            If (userID = 0) Then Return Nothing
            Return Me.GetItemByUser(Sistema.Users.GetItemById(userID))
        End Function

        Public Function GetItemByPersona(ByVal personID As Integer) As CTeamManager
            If (personID = 0) Then Return Nothing
            For Each tm As CTeamManager In Me.LoadAll
                If (tm.Stato = ObjectStatus.OBJECT_VALID AndAlso tm.PersonaID = personID) Then
                    Return tm
                End If
            Next
            Return Nothing
        End Function




    End Class

    Private Shared m_TeamManagers As CTeamManagersClass = Nothing

    Public Shared ReadOnly Property TeamManagers As CTeamManagersClass
        Get
            If (m_TeamManagers Is Nothing) Then m_TeamManagers = New CTeamManagersClass
            Return m_TeamManagers
        End Get
    End Property

End Class
