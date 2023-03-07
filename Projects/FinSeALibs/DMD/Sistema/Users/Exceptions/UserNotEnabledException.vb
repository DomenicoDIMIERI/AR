Imports DMD
Imports DMD.Sistema

Public Class UserNotEnabledException
    Inherits UserLoginException

    Private m_UserStatus As UserStatus

    Public Sub New()
        Me.New(vbNullString, "Utente non abilitato")
    End Sub

    Public Sub New(ByVal userName As String)
        Me.New(userName, "[" & userName & "] Utente non abilitato")
    End Sub

    Public Sub New(ByVal userName As String, ByVal message As String)
        MyBase.New(userName, message)
    End Sub

    Public Sub New(ByVal userName As String, ByVal stato As UserStatus)
        MyBase.New(userName, "[" & userName & "] Utente non abilitato: " & [Enum].GetName(GetType(UserStatus), stato))
        Me.m_UserStatus = stato
    End Sub

    Public ReadOnly Property UserStatus As UserStatus
        Get
            Return Me.m_UserStatus
        End Get
    End Property

End Class

