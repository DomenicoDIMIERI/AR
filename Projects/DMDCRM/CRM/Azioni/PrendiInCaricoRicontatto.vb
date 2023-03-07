Imports DMD
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class CustomerCalls

    Public Class PrendiInCaricoRicontatto
        Inherits AzioneEseguibile


        Public Overrides ReadOnly Property Description As String
            Get
                Return "Prende in carico la richiesta di finanziamento e programma un ricontatto nel CRM"
            End Get
        End Property

        Protected Overrides Function ExecuteInternal(notifica As Notifica, parameters As String) As String
            If (notifica.SourceName <> "CRicontatto") Then Throw New ArgumentException("L'azione non è definita sul tipo [" & notifica.SourceName & "]")
            Dim richFin As CRicontatto = Anagrafica.Ricontatti.GetItemById(notifica.SourceID)
            If (richFin Is Nothing) Then Throw New ArgumentNullException("Richiesta di finaniamento")
            Return Nothing
        End Function

        Public Overrides ReadOnly Property Name As String
            Get
                Return "RICONTTPRENDIINCARICO"
            End Get
        End Property

        Public Overrides Function Render(notifica As Notifica, context As Object) As Object
            Return Nothing
        End Function

    End Class

End Class