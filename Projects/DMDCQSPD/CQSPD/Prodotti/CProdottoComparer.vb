Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    Public Class CProdottoComparer
        Implements IComparer

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
            Dim a As CCQSPDProdotto = x
            Dim b As CCQSPDProdotto = y
            If (a.Descrizione < b.Descrizione) Then Return -1
            If (a.Descrizione > b.Descrizione) Then Return 1
            Return 0
        End Function

    End Class



End Class
