Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica
     

    Public Class CPersonaComparer
        Implements IComparer

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public Function Compare(ByVal a As CPersona, ByVal b As CPersona) As Integer
            Return Strings.Compare(a.Nominativo, b.Nominativo)
        End Function

        Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
            Return Me.Compare(x, y)
        End Function
    End Class
     

End Class