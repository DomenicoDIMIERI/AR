Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    Public NotInheritable Class CKnownUsersClass
        Private ReadOnly m_Lock As New Object
        Private m_GlobalAdmin As CUser
        Private m_SystemUser As CUser
        Private m_Guest As CUser

        Friend Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        ''' <summary>
        ''' Restituisce l'utente amministrativo globale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GlobalAdmin As CUser
            Get
                SyncLock m_Lock
                    If (m_GlobalAdmin Is Nothing) Then m_GlobalAdmin = Sistema.Users.GetItemByName("admin")
                    If (m_GlobalAdmin Is Nothing) Then
                        m_GlobalAdmin = Sistema.Users.CreateUser("admin")
                        m_GlobalAdmin.ForceUser(SystemUser)
                        m_GlobalAdmin.ForcePassword("admin")
                        m_GlobalAdmin.Stato = ObjectStatus.OBJECT_VALID
                        m_GlobalAdmin.Visible = True
                        m_GlobalAdmin.Save()
                    End If
                    Return m_GlobalAdmin
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce di sistema predefinito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property SystemUser As CUser
            Get
                SyncLock m_Lock
                    If (m_SystemUser Is Nothing) Then m_SystemUser = Sistema.Users.GetItemByName("SYSTEM")
                    If (m_SystemUser Is Nothing) Then
                        m_SystemUser = New CUser("SYSTEM")
                        m_SystemUser.ForceUser(m_SystemUser)
                        m_SystemUser.ForcePassword(ASPSecurity.GetRandomKey(25))
                        m_SystemUser.Stato = ObjectStatus.OBJECT_VALID
                        m_SystemUser.UserStato = UserStatus.USER_DISABLED
                        m_SystemUser.Visible = False
                        m_SystemUser.Save()
                    End If
                    Return m_SystemUser
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Restituisce l'utente predefinito per le sessioni non autenticate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GuestUser As CUser
            Get
                SyncLock m_Lock
                    If (m_Guest Is Nothing) Then m_Guest = Sistema.Users.GetItemByName("Guest")
                    If (m_Guest Is Nothing) Then
                        m_Guest = New CUser("Guest")
                        m_Guest.ForceUser(SystemUser)
                        m_Guest.ForcePassword("")
                        m_Guest.Stato = ObjectStatus.OBJECT_VALID
                        m_Guest.UserStato = UserStatus.USER_DISABLED
                        m_Guest.Visible = False
                        m_Guest.Save()
                    End If
                    Return m_Guest
                End SyncLock
            End Get
        End Property


        Public Sub Reset()
            SyncLock m_Lock
                m_GlobalAdmin = Nothing
                m_Guest = Nothing
                m_Module = Nothing
                m_SystemUser = Nothing
            End SyncLock
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


    



End Class
