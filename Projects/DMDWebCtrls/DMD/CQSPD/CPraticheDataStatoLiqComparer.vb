Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms

 
    Public Class CPraticheDataStatoLiqComparer
        Implements IComparer

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function Compare(ByVal a As CRapportino, ByVal b As CRapportino) As Integer
            If a.StatoLiquidata.Data < b.StatoLiquidata.Data Then Return -1
            If a.StatoLiquidata.Data > b.StatoLiquidata.Data Then Return 1
            Return 0
        End Function

        Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
            Return Me.Compare(x, y)
        End Function
    End Class


End Namespace