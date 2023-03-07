Imports Microsoft.VisualBasic

Public Class MasterPage
    Inherits System.Web.UI.MasterPage

    Public Sub New()
        DMD.DMDObject.IncreaseCounter(Me)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        DMD.DMDObject.DecreaseCounter(Me)
    End Sub
End Class

