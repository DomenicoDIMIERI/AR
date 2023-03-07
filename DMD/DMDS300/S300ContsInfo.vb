Imports DMD.S300.CKT_DLL
Imports System.Runtime.InteropServices


Namespace S300

    ''' <summary>
    ''' Struttura che racchiude le informazioni sui contatori di sistema
    ''' </summary>
    Public Structure S300CountsInfo

        ''' <summary>
        ''' Numero di "persone" definite sul dispositivo
        ''' </summary>
        Public PersonsCount As Integer

        ''' <summary>
        ''' Numero di impronte digitali registrate sul dispositivo
        ''' </summary>
        Public FingerPrintsCount As Integer

        ''' <summary>
        ''' Numero di marcature di ingresso/uscita registrate sul dispositivo
        ''' </summary>
        Public ClockingsCounts As Integer


    End Structure


End Namespace
