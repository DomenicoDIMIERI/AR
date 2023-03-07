Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica
     
    Public Interface ITaskHandler

        ''' <summary>
        ''' Esegue la regola specificata sul task sorgente e restituisce il task generato
        ''' </summary>
        ''' <param name="taskSorgente"></param>
        ''' <param name="regola"></param>
        ''' <param name="parametri"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Handle(ByVal taskSorgente As TaskLavorazione, ByVal regola As RegolaTaskLavorazione, ByVal parametri As String) As TaskLavorazione

    End Interface


End Class