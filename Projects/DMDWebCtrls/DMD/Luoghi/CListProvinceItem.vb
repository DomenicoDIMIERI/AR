Imports DMD
Imports DMD.Sistema
Imports DMD.WebSite

Imports DMD.Forms
Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Forms.Utils

Namespace Forms
  

    Public Class CListProvinceItem
        Public Value As String
        Public UserAuthID As Integer
        Public UserAllow As Boolean
        Public UserNegate As Boolean
        Public GroupAuthID As Integer
        Public GroupAllow As Boolean
        Public GroupNegate As Boolean

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Value = ""
            UserAuthID = 0
            UserAllow = False
            UserNegate = False
            GroupAuthID = 0
            GroupAllow = False
            GroupNegate = False
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class





End Namespace