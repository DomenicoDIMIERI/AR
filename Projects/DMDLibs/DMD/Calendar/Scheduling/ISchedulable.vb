Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Sistema

    Public Interface ISchedulable


        ReadOnly Property Programmazione As MultipleScheduleCollection

        ''' <summary>
        ''' Notifica all'oggetto che implementa la classe che la programmazione è stata cambiata esternamente e deve essere ricaricata
        ''' </summary>
        Sub InvalidateProgrammazione()

        Sub NotifySchedule(ByVal s As CalendarSchedule)

    End Interface
End Class