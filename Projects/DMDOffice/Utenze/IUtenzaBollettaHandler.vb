Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica


Partial Class Office

    ''' <summary>
    ''' Interfaccia implementata dalle classi utilizzabili per generare una bolletta a partire da un'utenza
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IUtenzaBollettaHandler

        ''' <summary>
        ''' Genera la prossima bolletta 
        ''' </summary>
        ''' <param name="utenza"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GenerateNext(ByVal utenza As Utenza) As DMD.Office.DocumentoContabile


    End Interface



End Class