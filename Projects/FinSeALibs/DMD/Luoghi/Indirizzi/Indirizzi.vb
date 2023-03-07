Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Anagrafica

    Public NotInheritable Class CIndirizziClass
        Inherits CGeneralClass(Of CIndirizzo)

        Friend Sub New()
            MyBase.New("Indirizzi", GetType(CIndirizziCursor))
        End Sub
         

    End Class


End Class