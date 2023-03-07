Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    ''' <summary>
    ''' Interfaccia implementata dai gestori di un'azione
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface ModuleActionHandler

        ''' <summary>
        ''' Esegue l'azione specificata 
        ''' </summary>
        ''' <param name="action"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Execute(ByVal action As CModuleAction, ByVal context As Object) As ModuleActionResult


    End Interface


End Class