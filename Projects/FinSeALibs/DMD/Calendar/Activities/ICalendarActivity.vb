Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Sistema

    Public Enum StatoAttivita As Integer
        INATTESA = 0
        INCORSO = 1
        CONCLUSA = 2
    End Enum

    <Flags> _
    Public Enum CalendarActivityFlags As Integer
        None = 0
        CanEdit = 1
        CanDelete = 2
        IsAction = 4
    End Enum



    ''' <summary>
    ''' Interfaccia implementata da un'oggetto che sfrutta il calendario per la gestione degli eventi
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface ICalendarActivity
        Inherits IComparable

        Property Flags As CalendarActivityFlags

        ''' <summary>
        ''' Restituisce o imposta la categoria
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property Categoria As String

        ''' <summary>
        ''' Stato dell'attività
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property StatoAttivita As StatoAttivita

        ''' <summary>
        ''' Descrizione dell'attività
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property Descrizione As String

        ''' <summary>
        ''' Luogo in cui si svolge l'attività
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property Luogo As String

        ''' <summary>
        ''' Note aggiuntive
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property Note As String

        ''' <summary>
        ''' Vero se l'attività è relativa all'intera giornata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property GiornataIntera As Boolean

        ''' <summary>
        ''' Promemoria in minuti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property Promemoria As Integer

        ''' <summary>
        ''' Numero di ripetizioni dell'attività a partire dalla data di inizio. 
        ''' Se 0 l'attività si ripete fino a DataFine o all'infinito se DataFine non è valorizzata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property Ripetizione As Integer

        ''' <summary>
        ''' Data di inizio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property DataInizio As Date

        ''' <summary>
        ''' Data finale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property DataFine As Date?

        ''' <summary>
        ''' ID dell'operatore a cui è assegnata l'attività
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property IDOperatore As Integer

        ''' <summary>
        ''' Operatore a cui è assegnata l'attività
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property Operatore As CUser

        Property NomeOperatore As String


        Property IDAssegnatoA As Integer

        Property AssegnatoA As CUser

        Property NomeAssegnatoA As String

        ''' <summary>
        ''' Un campo aggiuntivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property Tag As Object

        ''' <summary>
        ''' Restituisce  imposta un'icona per l'attività
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property IconURL As String

        ''' <summary>
        ''' Restituisce un numero intero che indica la priorità (crescente) dell'evento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property Priorita As Integer

        Property IDPersona As Integer

        Property Persona As CPersona

        Property NomePersona As String

        ReadOnly Property ProviderName As String

        Sub SetProvider(ByVal p As ICalendarProvider)

        ReadOnly Property Provider As ICalendarProvider

    End Interface


End Class