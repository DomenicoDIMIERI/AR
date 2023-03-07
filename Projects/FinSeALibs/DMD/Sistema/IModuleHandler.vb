Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Public Interface IModuleHandler
    ''' <summary>
    ''' Restituisce il modulo gestito
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ReadOnly Property [Module] As CModule

    ''' <summary>
    ''' Imposta il modulo gestito
    ''' </summary>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Sub SetModule(ByVal value As CModule)

    ''' <summary>
    ''' Esegue l'azione specificata
    ''' </summary>
    ''' <param name="actionName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ExecuteAction(ByVal renderer As Object, ByVal actionName As String) As String

    ' ''' <summary>
    ' ''' Esegue l'azione specificata
    ' ''' </summary>
    ' ''' <param name="actionName"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Function ExecuteAction1(ByVal renderer As Object, ByVal actionName As String) As MethodResults


    ''' <summary>
    ''' Crea un cursore per l'accesso agli oggetti gestiti dal modulo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function CreateCursor() As DBObjectCursorBase

    ''' <summary>
    ''' Restituisce o imposta un valore booleano che indica se l'inizializzazione avviene sul server o sul client (vero indica il client)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property UseLocal As Boolean

    ''' <summary>
    ''' Restituisce vero se l'utente corrente puo eseguire il comando list sul modulo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function CanList() As Boolean

    Function CanCreate() As Boolean

    Function CanEdit(ByVal item As Object) As Boolean

    Function CanDelete(ByVal item As Object) As Boolean

    ''' <summary>
    ''' Restituisce vero se l'utente corrente può eseguire la configurazione del modulo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function CanConfigure() As Boolean

End Interface


 
