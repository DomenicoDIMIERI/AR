Imports DMD.Databases
Imports DMD.Sistema

Partial Class Office


    ''' <summary>
    ''' Cursore sulla tabella dei motivi delle richieste
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IRichiestaHandler

        Sub validate(ByVal richiesta As RichiestaCERQ, ByVal motivo As MotivoRichiesta, ByVal toStato As StatoRichiestaCERQ)

        Sub execute(ByVal richiesta As RichiestaCERQ, ByVal motivo As MotivoRichiesta, ByVal toStato As StatoRichiestaCERQ)

    End Interface

End Class