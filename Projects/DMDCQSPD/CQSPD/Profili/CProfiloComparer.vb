Partial Public Class CQSPD

    Public Class CProfiloComparer
        Implements IComparer

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
            Dim a As CProfilo = x
            Dim b As CProfilo = y
            If (a.NomeCessionario < b.NomeCessionario) Then Return -1
            If (a.NomeCessionario > b.NomeCessionario) Then Return 1
            If (a.Nome < b.Nome) Then Return -1
            If (a.Nome > b.Nome) Then Return 1
            If (a.Profilo < b.Profilo) Then Return -1
            If (a.Profilo > b.Profilo) Then Return 1
            Return 0
        End Function

    End Class
End Class
