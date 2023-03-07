Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms


 
  

    '--------------------------------
    Public Class CPraticheDataStatoContattoComparer
        Implements IComparer

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function Compare(ByVal a As CRapportino, ByVal b As CRapportino) As Integer
            Dim d1 As DateTime? = Nothing
            Dim d2 As DateTime? = Nothing
            If (a.StatoContatto IsNot Nothing) Then d1 = a.StatoContatto.Data
            If (b.StatoContatto IsNot Nothing) Then d2 = b.StatoContatto.Data
            Return DateUtils.Compare(d1, d2)
        End Function

        Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
            Return Me.Compare(x, y)
        End Function
    End Class


End Namespace