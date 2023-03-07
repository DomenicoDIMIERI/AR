Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Internals
Imports DMD.Anagrafica
 

Partial Public Class Anagrafica


    Public Interface IMetodoDiPagamento


        ''' <summary>
        ''' Restituisce il nome del metodo di pagamento utilizzato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property NomeMetodo As String

        ''' <summary>
        ''' Restituisce il conto corrente associato al metodo di pagamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property ContoCorrente As ContoCorrente


    End Interface


End Class