Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class CQSPD

    ''' <summary>
    ''' Interfaccia implementata dagli oggetti su cui è possibile effettuare dei controlli amministrativi
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IOggettoVerificabile

        ''' <summary>
        ''' Restituisce o imposta l'ultima verifica amministrativa effettuata sull'oggetto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property UltimaVerifica As VerificaAmministrativa

    End Interface

End Class
