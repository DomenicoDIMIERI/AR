Imports DMD
Imports DMD.Sistema
Imports DMD.Forms
Imports DMD.WebSite
Imports DMD.Databases
Imports DMD.Forms.Utils


Imports DMD.Anagrafica

Namespace Forms

 
  
 
 

    '---------------------------------------------------------------------------------------------
    Public Class CListAutorizzazioniItem
        Public Azione As CModuleAction
        Public UserAuthID As Integer
        Public UserAllow As Boolean
        Public UserNegate As Boolean
        Public GroupAuthID As Integer
        Public GroupAllow As Boolean
        Public GroupNegate As Boolean

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.Azione = Nothing
            Me.UserAuthID = 0
            Me.UserAllow = False
            Me.UserNegate = False
            Me.GroupAuthID = 0
            Me.GroupAllow = False
            Me.GroupNegate = False
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class



End Namespace