Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica



Partial Public Class CustomerCalls

    ''' <summary>
    ''' Interfaccia implementata dagli oggetti che si installano nel CRM per essere visualizzati nello storico del cliente
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IStoricoHandler

        ''' <summary>
        ''' Metodo richiamato per aggiungere gli elementi allo storico
        ''' </summary>
        ''' <param name="items"></param>
        ''' <param name="filter"></param>
        ''' <remarks></remarks>
        Sub Aggiungi(ByVal items As CCollection(Of StoricoAction), ByVal filter As CRMFindFilter)

        ''' <summary>
        ''' Restituisce la collezione dei tipi gestiti.
        ''' Le chiavi della collezione indicano il nome della classe mentre il valore indica il nome visibile
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetHandledTypes() As CKeyCollection(Of String)


    End Interface


End Class