Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Internals

Namespace Internals

    'Classe globale per l'accesso agli uffici
    Public Class CValoriRegistriClass
        Inherits CGeneralClass(Of ValoreRegistroContatore)

        Friend Sub New()
            MyBase.New("modPostazioniRegistri", GetType(ValoreRegistroCursor), 0)
        End Sub


    End Class
End Namespace
