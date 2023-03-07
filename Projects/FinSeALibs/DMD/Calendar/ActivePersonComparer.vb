Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica


Partial Class Sistema

    Public Class ActivePersonComparer
        Implements IComparer

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function Compare(x As CActivePerson, y As CActivePerson) As Integer
            Dim ret As Integer = 0
            If (x.PersonID <> y.PersonID) Then
                'Dim la As String = ""
                'Dim lb As String = ""
                'If (x.Ricontatto IsNot Nothing) Then la = x.Ricontatto.NomeLista
                'If (y.Ricontatto IsNot Nothing) Then lb = y.Ricontatto.NomeLista
                'ret = Strings.Compare(la, lb)
                If (ret = 0) Then ret = Strings.Compare(x.Nominativo, y.Nominativo)
                Return ret
            End If
            ret = -Calendar.Compare(x.Data, y.Data)
            If (ret = 0) Then ret = Strings.Compare(x.Nominativo, y.Nominativo, CompareMethod.Text)
            Return ret
        End Function

        Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
            Return Me.Compare(x, y)
        End Function
    End Class


End Class