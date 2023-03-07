Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Drivers
 
Partial Public Class Sistema

    Public Class SMSDriverOptions
        Inherits DriverOptions

        Public Sub New()
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il numero o il nome del mittente visualizzato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Mittente As String
            Get
                Return Me.GetValueString("Mittente", "")
            End Get
            Set(value As String)
                Me.SetValueString("Mittente", Strings.Trim(value))
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o importa un valore booleano che indica se è richiesta la conferma di lettura
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RichiediConfermaDiLettura As Boolean
            Get
                Return Me.GetValueBool("RichiediConfermaDiLettura", False)
            End Get
            Set(value As Boolean)
                Me.SetValueBool("RichiediConfermaDiLettura", value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del modem utilizzato per l'invio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ModemName As String
            Get
                Return Me.GetValueString("ModemName", "")
            End Get
            Set(value As String)
                Me.SetValueString("ModemName", Strings.Trim(value))
            End Set
        End Property
    End Class


 
End Class