Imports DMD.S300.CKT_DLL
Imports DMD.Internals
Imports System.Runtime.InteropServices


Namespace S300

    Public Enum S300ExceptionCodes As Integer
        'Ok = 1
        CKT_ERROR_INVPARAM = -1
        CKT_ERROR_NETDAEMONREADY = -1
        CKT_ERROR_CHECKSUMERR = -2
        CKT_ERROR_MEMORYFULL = -1
        CKT_ERROR_INVFILENAME = -3
        CKT_ERROR_FILECANNOTOPEN = -4
        CKT_ERROR_FILECONTENTBAD = -5
        CKT_ERROR_FILECANNOTCREATED = -2
        CKT_ERROR_NOTHISPERSON = -1
    End Enum

    ''' <summary>
    ''' Classe che rappresenta una eccezione della libreria
    ''' </summary>
    Public Class S300Exception
        Inherits System.Exception

        Private m_Code As S300ExceptionCodes

        Public Sub New()

        End Sub

        Public Sub New(ByVal code As S300ExceptionCodes)
            MyBase.New(GetCodeMsg(code))
            Me.m_Code = code
        End Sub

        Public Sub New(ByVal code As S300ExceptionCodes, ByVal message As String)
            MyBase.New(message)
            Me.m_Code = code
        End Sub



        ''' <summary>
        ''' Restituisce il codice dell'eccezione
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Code As S300ExceptionCodes
            Get
                Return Me.m_Code
            End Get
        End Property

        Private Shared Function GetCodeMsg(ByVal code As S300ExceptionCodes)
            Select Case code
                Case S300ExceptionCodes.CKT_ERROR_CHECKSUMERR : Return "Errore di comunicazione"
                Case S300ExceptionCodes.CKT_ERROR_FILECANNOTCREATED : Return "Impossibile creare il file"
                Case S300ExceptionCodes.CKT_ERROR_FILECANNOTOPEN : Return "Impossibile aprire il file"
                Case S300ExceptionCodes.CKT_ERROR_FILECONTENTBAD : Return "Contenuto del file non valido"
                Case S300ExceptionCodes.CKT_ERROR_INVFILENAME : Return "Nome del file non valido"
                Case S300ExceptionCodes.CKT_ERROR_INVPARAM : Return "Valore del parametro non valido"
                Case S300ExceptionCodes.CKT_ERROR_MEMORYFULL : Return "Memoria piena"
                Case S300ExceptionCodes.CKT_ERROR_NETDAEMONREADY : Return "Demone di rete non pronto"
                Case S300ExceptionCodes.CKT_ERROR_NOTHISPERSON : Return "Utente inesistente"
                Case Else : Return "Sconosciuto"
            End Select
        End Function


    End Class


End Namespace
