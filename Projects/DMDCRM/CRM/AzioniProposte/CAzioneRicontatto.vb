Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls


    ''' <summary>
    ''' Propone un ricontatto
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CAzioneRicontatto
        Inherits CAzioneProposta

        Public Sub New()
        End Sub

        Protected Friend Overrides Sub Initialize(c As CContattoUtente)
            Dim tel As CTelefonata = c
            MyBase.Initialize(tel)
            'MyBase.Descrizione = "Ricontattare " & tel.NomePersona & " al N°" & tel.Numero & " dal " & Formats.FormatUserDate(tel.DataRicontatto)
            MyBase.SetCommand("try { CustomerCalls.ReCall(" & Databases.GetID(Me.Contatto, 0) & ", '" & Replace(tel.NumeroOIndirizzo, "'", "\'") & "'); } catch (ex) { alert(ex); }")
            MyBase.SetIsScript(True)
        End Sub

    End Class

End Class