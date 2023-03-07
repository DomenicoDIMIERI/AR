Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Internals

Namespace Internals

    'Classe globale per la gestione delle manutnzioni sulle postazioni di lavoro
    Public Class CVociManutenzioneClass
        Inherits CGeneralClass(Of VoceManutenzione)

        Friend Sub New()
            MyBase.New("modManutenziniVoci", GetType(VociManutenzioneCursor), 0)
        End Sub


    End Class
End Namespace

