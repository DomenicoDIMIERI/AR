Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls


    ''' <summary>
    ''' Propone la creazione di una pratica
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CAzioneCreaPratica
        Inherits CAzioneProposta


        Protected Friend Overrides Sub Initialize(c As CContattoUtente)
            Dim tel As CTelefonata = c
            Call MyBase.Initialize(c)
            MyBase.Descrizione = "Inizia pratica"
            MyBase.SetCommand("/?_m=modRapportini&_a=ContactToPratica&cid=" & Databases.GetID(tel, 0))
            MyBase.SetIsScript(False)
        End Sub

    End Class

End Class