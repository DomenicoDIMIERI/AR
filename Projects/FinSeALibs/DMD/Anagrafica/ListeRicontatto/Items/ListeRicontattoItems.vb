Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Internals
Imports DMD.Anagrafica

Namespace Internals

    Public NotInheritable Class CListeRicontattoItemsClass
        Inherits CGeneralClass(Of ListaRicontattoItem)

        Friend Sub New()
            MyBase.New("modListeRicontattoItem", GetType(ListaRicontattoItemCursor), 0)
        End Sub

        Public Function GetRicontattoBySource(ByVal source As Object) As ListaRicontattoItem
            Throw New NotImplementedException()
        End Function
    End Class

End Namespace

 