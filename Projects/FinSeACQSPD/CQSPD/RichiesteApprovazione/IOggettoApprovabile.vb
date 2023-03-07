Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class CQSPD

    Public Interface IOggettoApprovabile
        ''' <summary>
        ''' Evento generato quando vine formulata un'offerta che richiede l'approvazione
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Event RequireApprovation(ByVal sender As Object, ByVal e As ItemEventArgs)

       
        ''' <summary>
        ''' Evento generato quando l'offerta corrente viene approvata
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Event Approvata(ByVal sender As Object, ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando l'offerta viene rifiutata
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Event Rifiutata(ByVal sender As Object, ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Evento generato quando l'offerta viene rifiutata
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Event PresaInCarico(ByVal sender As Object, ByVal e As ItemEventArgs)

        ''' <summary>
        ''' Restitusice o imposta l'ID della richiesta di approvazione corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property IDRichiestaApprovazione As Integer

        ''' <summary>
        ''' Restituisce la richiesta ai approvazione corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property RichiestaApprovazione As CRichiestaApprovazione

        ''' <summary>
        ''' Genera una richiesta di approvazione
        ''' </summary>
        ''' <param name="motivo"></param>
        ''' <param name="dettaglio"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function RichiediApprovazione(ByVal motivo As String, ByVal dettaglio As String, ByVal parametri As String) As CRichiestaApprovazione

        ''' <summary>
        ''' Approva la richiesta corrente
        ''' </summary>
        ''' <param name="motivo"></param>
        ''' <param name="dettaglio"></param>
        ''' <remarks></remarks>
        Function Approva(ByVal motivo As String, ByVal dettaglio As String) As CRichiestaApprovazione


        ''' <summary>
        ''' Nega la richiesta corrente
        ''' </summary>
        ''' <param name="motivo"></param>
        ''' <param name="dettaglio"></param>
        ''' <remarks></remarks>
        Function Nega(ByVal motivo As String, ByVal dettaglio As String) As CRichiestaApprovazione

        ''' <summary>
        ''' Prende in carico la richiesta
        ''' </summary>
        ''' <remarks></remarks>
        Function PrendiInCarico() As CRichiestaApprovazione
    End Interface

End Class