Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports System.Threading
Imports System.Web.UI.HtmlControls
Imports System.IO

Partial Class WebSite



    Public Class UploadCalcelledException
        Inherits System.Exception

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal message As String)
            MyBase.New(message)
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class