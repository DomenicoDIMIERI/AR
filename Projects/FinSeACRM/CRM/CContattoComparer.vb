Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls

    Public Class CContattoComparer
        Implements IComparer

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function Compare(ByVal a As CContattoUtente, ByVal b As CContattoUtente) As Integer
            Return a.CompareTo(b)
        End Function

        Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
            Return Me.Compare(x, y)
        End Function
    End Class


End Class