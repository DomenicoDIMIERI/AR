Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Sistema

Partial Public Class CQSPD

    Public Class PrendiInCaricoRichiestaFinanziamento
        Inherits AzioneEseguibile


        Public Overrides ReadOnly Property Description As String
            Get
                Return "Prende in carico la richiesta di finanziamento e programma un ricontatto nel CRM"
            End Get
        End Property

        Protected Overrides Function ExecuteInternal(notifica As Notifica, parameters As String) As String
            If (notifica.SourceName <> "CRichiestaFinanziamento") Then Throw New ArgumentException("L'azione non è definita sul tipo [" & notifica.SourceName & "]")
            Dim richFin As CRichiestaFinanziamento = CQSPD.RichiesteFinanziamento.GetItemById(notifica.SourceID)
            If (richFin Is Nothing) Then Throw New ArgumentNullException("Richiesta di finaniamento")
            richFin.PrendiInCarico()
            Dim ric As CRicontatto = Anagrafica.Ricontatti.ProgrammaRicontatto(richFin.Cliente, DateUtils.Now(), "Ricontatto programmato per la richiesta online: " & richFin.ToString, TypeName(richFin), GetID(richFin), "", richFin.PuntoOperativo, Users.CurrentUser)
            notifica.StatoNotifica = StatoNotifica.LETTA
            Return GetID(ric)
        End Function

        Public Overrides ReadOnly Property Name As String
            Get
                Return "RICHFINPRENDIINCARICO"
            End Get
        End Property

        Public Overrides Function Render(notifica As Notifica, context As Object) As Object
            Return Nothing
        End Function

    End Class

End Class