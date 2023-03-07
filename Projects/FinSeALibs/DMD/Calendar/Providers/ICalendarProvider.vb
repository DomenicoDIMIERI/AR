Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Sistema

    ''' <summary>
    ''' Classe base del provider di attività del calendario
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface ICalendarProvider

        ''' <summary>
        ''' Crea un nuovo oggetto 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function InstantiateNewItem() As Object


        Function GetCreateCommand() As String

        ''' <summary>
        ''' Restituisce un testo breve che descrive il tipo di provider
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetShortDescription() As String

        ''' <summary>
        ''' Restituisce un array di tipi di oggetto supportati da questo provider
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetSupportedTypes() As System.Type()

        ''' <summary>
        ''' Restituisce le persone con eventi gestiti da questo provider nell'intervallo di date specificato
        ''' </summary>
        ''' <param name="fromDate"></param>
        ''' <param name="toDate"></param>
        ''' <param name="ufficio"></param>
        ''' <param name="operatore"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetActivePersons(ByVal nomeLista As String, ByVal fromDate As Nullable(Of Date), ByVal toDate As Nullable(Of Date), Optional ByVal ufficio As Integer = 0, Optional ByVal operatore As Integer = 0) As CCollection(Of CActivePerson)

        ''' <summary>
        ''' Restituisce le scadenza gestite da questo provider
        ''' </summary>
        ''' <param name="fromDate"></param>
        ''' <param name="toDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetScadenze(ByVal fromDate As Nullable(Of Date), ByVal toDate As Nullable(Of Date)) As CCollection(Of ICalendarActivity)

        ''' <summary>
        ''' Restituisce le aziende pendenti
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetPendingActivities() As CCollection(Of ICalendarActivity)

        ''' <summary>
        ''' Restituisce l'elenco delle cose da fare per l'utente specificato
        ''' </summary>
        ''' <param name="user"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetToDoList(ByVal user As CUser) As CCollection(Of ICalendarActivity)

        ''' <summary>
        ''' Restituisce il nome che identifica univocamente il provider all'interno del sistema
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property UniqueName As String


        Sub SaveActivity(ByVal item As ICalendarActivity, Optional ByVal force As Boolean = False)

        Sub DeleteActivity(ByVal item As ICalendarActivity, Optional ByVal force As Boolean = False)

    End Interface


End Class