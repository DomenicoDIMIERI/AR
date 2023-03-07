Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica

Partial Public Class CustomerCalls

    Public Interface ICallManager

        ''' <summary>
        ''' Inizializza il call manager
        ''' </summary>
        ''' <remarks></remarks>
        Sub Start()

        ''' <summary>
        ''' Ferma il call manager
        ''' </summary>
        ''' <remarks></remarks>
        Sub [Stop]()

    End Interface

End Class