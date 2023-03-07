Imports DMD
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class CustomerCalls

    Public Class VisitaInAttesaAction
        Inherits AzioneEseguibile


        Public Overrides ReadOnly Property Description As String
            Get
                Return "Cliente in attesa"
            End Get
        End Property

        Protected Overrides Function ExecuteInternal(notifica As Notifica, parameters As String) As String
            If (notifica.SourceName <> "CVisita") Then Throw New ArgumentException("L'azione non è definita sul tipo [" & notifica.SourceName & "]")
            Dim visita As CVisita = CustomerCalls.Visite.GetItemById(notifica.SourceID)
            If (visita Is Nothing) Then Throw New ArgumentNullException("Visita")
            Return Nothing
        End Function

        Public Overrides ReadOnly Property Name As String
            Get
                Return "VISITAINATTESAACT"
            End Get
        End Property

        Public Overrides Function Render(notifica As Notifica, context As Object) As Object
            Return Nothing
        End Function

    End Class

End Class