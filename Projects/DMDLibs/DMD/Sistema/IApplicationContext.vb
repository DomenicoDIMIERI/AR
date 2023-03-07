Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    ''' <summary>
    ''' Interfaccia implementata dall'applicazione e necessaria per interfacciarsi alla libreria
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IApplicationContext
        ''' <summary>
        ''' Restituisce o imposta la collezione delle impostazioni di sistema
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Settings As CKeyCollection


        ReadOnly Property RemoteIP As String
        ReadOnly Property RemotePort As Integer

        ''' <summary>
        ''' Restituisce la cartella di avvio del programma
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property StartupFloder As String

        ''' <summary>
        ''' Restituisce la cartella temporanea predefinita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property TmporaryFolder As String

        ''' <summary>
        ''' Restituisce la cartella in cui l'utente corrente può salvare i dati
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property UserDataFolder As String

        ''' <summary>
        ''' Restituisce la cartella dati di sistema
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property SystemDataFolder As String

        ' ''' <summary>
        ' ''' Restituisce il percorso predefinito dell'utente corrente
        ' ''' </summary>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'ReadOnly Property UserFolder As String


        ''' <summary>
        ''' Restituisce l'ID dell'applicazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property InstanceID As String

        ''' <summary>
        ''' Restituisce la descrizione dell'applicazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Description As String

        ''' <summary>
        ''' Restituisce l'utente corrente (nella sessione)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CurrentUser As CUser

        ''' <summary>
        ''' Restituisce la sessione corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CurrentSession As Object

        ''' <summary>
        ''' Restituisce l'assembly principale
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetEntryAssembly() As System.Reflection.Assembly

        ''' <summary>
        ''' Restituisce un parametro letto dall'applicazione
        ''' </summary>
        ''' <param name="paramName"></param>
        ''' <param name="defValue"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetParameter(ByVal paramName As String, Optional ByVal defValue As String = vbNullString) As String

        ''' <summary>
        ''' Restituisce un parametro letto dall'applicazione
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="paramName"></param>
        ''' <param name="defValue"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetParameter(Of T)(ByVal paramName As String, Optional ByVal defValue As Object = Nothing) As T

        ''' <summary>
        ''' Effettua il mapping del percorso 
        ''' </summary>
        ''' <param name="path"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function MapPath(ByVal path As String) As String

        ''' <summary>
        ''' Effettua l'unmapping del percorso
        ''' </summary>
        ''' <param name="path"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function UnMapPath(ByVal path As String) As String

        ''' <summary>
        ''' Restituisce vero se l'utente è connesso
        ''' </summary>
        ''' <param name="user"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function IsUserLogged(ByVal user As CUser) As Boolean

        ''' <summary>
        ''' Restituisce vero se l'applicazione è in modalità di debug
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function IsDebug() As Boolean

        ''' <summary>
        ''' Restituisce o imposta l'azienda di lavoro principale 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property IDAziendaPrincipale As Integer

        ReadOnly Property SupportEMail As String

        ''' <summary>
        ''' Restituisec la url del percorso di lavoro
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property BaseURL As String

        ''' <summary>
        ''' Restituisce una descrizione breve dell'applicazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Title As String

        ''' <summary>
        ''' Ferma l'applicazione e rilascia tutte le risorse
        ''' </summary>
        ''' <remarks></remarks>
        Sub [Stop]()

        ''' <summary>
        ''' Inizializza l'applicazione ed acquisice le risorse
        ''' </summary>
        ''' <remarks></remarks>
        Sub Start()

        ''' <summary>
        ''' Restituisce l'ufficio di lavoro corrente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property CurrentUfficio As CUfficio


        Property CurrentLogin As CLoginHistory

        ''' <summary>
        ''' Restituisce una proprietà dell'applicazione in base al nome
        ''' </summary>
        ''' <param name="name"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function GetProperty(ByVal name As String) As Object

        ''' <summary>
        ''' Restituisce vero se l'applicazione è stata messa in stato manutenzione
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function IsMaintenance() As Boolean

        ''' <summary>
        ''' Mette l'applicazione in stato manutenzione
        ''' </summary>
        ''' <remarks></remarks>
        Sub EnterMaintenance()

        ''' <summary>
        ''' Esce dallo stato manutenzione
        ''' </summary>
        ''' <remarks></remarks>
        Sub QuitMaintenance()

        Sub Log(ByVal message As String)




    End Interface


End Class

