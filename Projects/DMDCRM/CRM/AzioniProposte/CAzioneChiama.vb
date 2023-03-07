Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls


    ''' <summary>
    ''' Propone un nuovo contatto
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CAzioneChiama
        Inherits CAzioneProposta

        Public Sub New()
        End Sub

        Public Sub New(ByVal c As CTelefonata)
            MyBase.New(c)
        End Sub

        Protected Friend Overrides Sub Initialize(c As CContattoUtente)
            Dim tel As CTelefonata = c
            ' MyBase.Descrizione = "Nuova telefonata a " & tel.NomePersona & " al N°" & tel.Numero & " dal " & Formats.FormatUserDate(tel.DataRicontatto)
            MyBase.SetCommand("try { CustomerCalls.PlaceCall(" & Me.Contatto.IDPersona & ", '" & Replace(tel.NumeroOIndirizzo, "'", "\'") & "'); } catch (ex) { alert(ex); }")
            MyBase.SetIsScript(True)
            MyBase.Initialize(c)
        End Sub


    End Class

    




End Class