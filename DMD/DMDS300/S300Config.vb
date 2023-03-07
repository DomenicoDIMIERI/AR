Imports DMD.S300.CKT_DLL
Imports System.Runtime.InteropServices


Namespace S300

    Public Enum WGOptions As Integer
        Wiegand26 = 0
        EncryptedWiegand = 1
        FixedWiegandAreaCode = 2
    End Enum

    ''' <summary>
    ''' Struttura che racchiude le informazioni sulla configurazione del dispositivo
    ''' </summary>
    Public Structure S300Config

        ''' <summary>
        ''' 
        ''' </summary>
        Public RingAllow As Boolean

        Public RealtimeMode As Boolean

        ''' <summary>
        ''' Valore booleano che indica se il dispositivo può aggiornare in maniera "intelligente" in maniera automatica
        ''' </summary>
        Public AutoUpdateFingerprint As Boolean

        ''' <summary>
        ''' Restituisce o imposta un valore intero compreso tra 0 e 5 che definisce il volume dell'altoparlante
        ''' </summary>
        Public SpeakerVolume As Integer

        ''' <summary>
        ''' Restituisce o imposta il ritardo in secondi per il blocco del rele
        ''' </summary>
        Public DoorLockDelay As Integer

        Public Property LockDelayTime As Integer

        Public Property AdminPassword As String

        Public FixedWiegandAreaCode As Integer


        Public WiegandOption As WGOptions

        ''' <summary>
        ''' Ritardo minimo (in minuti) tra due marcature consecutive
        ''' </summary>
        Public MinDelayInOut As Integer

    End Structure


End Namespace
