Imports DMD
Imports DMD.Databases
Imports System.Net
Imports DMD.XML

Public Class ItemAllowNegate(Of T)

    Public Item As T

    Public UserAuthID As Integer
    Public UserAllow As Boolean
    Public UserNegate As Boolean
    Public GroupAuthID As Integer
    Public GroupAllow As Boolean
    Public GroupNegate As Boolean

    Public Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
        Me.Item = Nothing
        Me.UserAuthID = 0
        Me.UserAllow = False
        Me.UserNegate = False
        Me.GroupAuthID = 0
        Me.GroupAllow = False
        Me.GroupNegate = False
    End Sub

    Public Function IsAllowed() As Boolean
        Return Me.UserAllow Or Me.GroupAllow
    End Function

    Public Function IsNegated() As Boolean
        Return Me.UserNegate Or Me.GroupNegate
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub


End Class
