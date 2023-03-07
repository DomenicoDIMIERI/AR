Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Sistema

Namespace Forms

    Public Class PrendiInCaricoAnagraficaImportata
        Inherits AzioneEseguibile


        Public Overrides ReadOnly Property Description As String
            Get
                Return "Prende in carico l'anagrafica importata"
            End Get
        End Property

        Protected Overrides Function ExecuteInternal(notifica As Notifica, parameters As String) As String
            If (notifica.SourceName <> "CPersonaFisica") AndAlso (notifica.SourceName <> "CAzienda") Then Throw New ArgumentException("L'azione non è definita sul tipo [" & notifica.SourceName & "]")
            notifica.StatoNotifica = StatoNotifica.LETTA
            Return ""
        End Function

        Public Overrides ReadOnly Property Name As String
            Get
                Return "ANAIMPPRENDIINCARICO"
            End Get
        End Property

        Public Overrides Function Render(notifica As Notifica, context As Object) As Object
            Return Nothing
        End Function

    End Class

End Namespace