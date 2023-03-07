Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD



    ''' <summary>
    ''' Rappresenta una collezione di estinzioni
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CEstinzioniCollection
        Inherits CCollection(Of CEstinzione)

        Public Sub New()
        End Sub

        Public Overloads Function Add(ByVal tipo As TipoEstinzione, ByVal istituto As CCQSPDCessionarioClass) As CEstinzione
            Dim item As New CEstinzione
            item.Tipo = tipo
            item.Istituto = istituto
            MyBase.Add(item)
            Return item
        End Function



    End Class

End Class