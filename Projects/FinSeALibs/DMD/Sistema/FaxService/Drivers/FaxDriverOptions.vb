Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Drivers
 
Partial Public Class Sistema

    Public Enum FaxResolution As Integer
        Normal = 0
        High = 1
    End Enum

    Public Class FaxDriverOptions
        Inherits DriverOptions

        Public Sub New()
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il nome del mittente visualizzato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SenderName As String
            Get
                Return Me.GetValueString("NomeMittente", "")
            End Get
            Set(value As String)
                Me.SetValueString("NomeMittente", Strings.Trim(value))
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero del mittente 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SenderNumber As String
            Get
                Return Me.GetValueString("NumeroMittente", "")
            End Get
            Set(value As String)
                Me.SetValueString("NumeroMittente", Strings.Trim(value))
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'oggetto del documento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Subject As String
            Get
                Return Me.GetValueString("Subject", "")
            End Get
            Set(value As String)
                Me.SetValueString("Subject", Strings.Trim(value))
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del destinatario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TargetName As String
            Get
                Return Me.GetValueString("TargetName", "")
            End Get
            Set(value As String)
                Me.SetValueString("TargetName", Strings.Trim(value))
            End Set
        End Property

        ''' <summary>
        ''' Restuitusce o imposta il nome del file da utilizzare come copertina
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CoverPage As String
            Get
                Return Me.GetValueString("CoverPage", "")
            End Get
            Set(value As String)
                Me.SetValueString("CoverPage", Strings.Trim(value))
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta una stringa descrittiva
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Comments As String
            Get
                Return Me.GetValueString("Comments", "")
            End Get
            Set(value As String)
                Me.SetValueString("Comments", Strings.Trim(value))
            End Set
        End Property

        Public Property DocumentName As String
            Get
                Return Me.GetValueString("DocumentName", "")
            End Get
            Set(value As String)
                Me.SetValueString("DocumentName", Strings.Trim(value))
            End Set
        End Property

        Public Property RecipientName As String
            Get
                Return Me.GetValueString("RecipientName", "")
            End Get
            Set(value As String)
                Me.SetValueString("RecipientName", Strings.Trim(value))
            End Set
        End Property


        Public Property Body As String
            Get
                Return Me.GetValueString("Body", "")
            End Get
            Set(value As String)
                Me.SetValueString("Body", Strings.Trim(value))
            End Set
        End Property

        Public Property AttachFaxToRecipient As Boolean
            Get
                Return Me.GetValueBool("AttachFaxToRecipient", True)
            End Get
            Set(value As Boolean)
                Me.SetValueBool("AttachFaxToRecipient", value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'indicazione geografica del luogo da cui è stato inviato il fax
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FromLocation As String
            Get
                Return Me.GetValueString("FromLocation", "")
            End Get
            Set(value As String)
                Me.SetValueString("FromLocation", Strings.Trim(value))
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente che ha inviato il fax
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FromUser As String
            Get
                Return Me.GetValueString("FromUser", "")
            End Get
            Set(value As String)
                Me.SetValueString("FromUser", Strings.Trim(value))
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di telefono per le comunicazioni vocali con l'utente che ha inviato il fax
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FromVoice As String
            Get
                Return Me.GetValueString("FromVoice", "")
            End Get
            Set(value As String)
                Me.SetValueString("FromVoice", Strings.Trim(value))
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo e-mail a cui notificare l'invio del fax
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NotifyEmailAddress As String
            Get
                Return Me.GetValueString("NotifyEmailAddress", "")
            End Get
            Set(value As String)
                Me.SetValueString("NotifyEmailAddress", Strings.Trim(value))
            End Set
        End Property



        ''' <summary>
        ''' Restituisce o imposta il numero massimo di telefonate prima di generare un errore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MaximumDials As Integer
            Get
                Return Me.GetValueInt("MaximumDials", 3)
            End Get
            Set(value As Integer)
                If (value <= 0) Then Throw New ArgumentOutOfRangeException("MaximumDials")
                Me.SetValueInt("MaximumDials", value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero massimo di tentativi per inviare un fax
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MaximumTries As Integer
            Get
                Return Me.GetValueInt("MaximumTries", 12)
            End Get
            Set(value As Integer)
                If (value <= 0) Then Throw New ArgumentOutOfRangeException("MaximumTries")
                Me.SetValueInt("MaximumTries", value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se notificare il completamento del lavoro
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NotifyWhenDone As Boolean
            Get
                Return Me.GetValueBool("NotifyWhenDone", True)
            End Get
            Set(value As Boolean)
                Me.SetValueBool("NotifyWhenDone", value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se notificare l'immissione in coda del fax
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NotifyWhenRequeued As Boolean
            Get
                Return Me.GetValueBool("NotifyWhenRequeued", True)
            End Get
            Set(value As Boolean)
                Me.SetValueBool("NotifyWhenRequeued", value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero massimo di tentativi per inviare un fax
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumberOfDials As Integer
            Get
                Return Me.GetValueInt("NumberOfDials", 3)
            End Get
            Set(value As Integer)
                If (value <= 0) Then Throw New ArgumentOutOfRangeException("NumberOfDials")
                Me.SetValueInt("NumberOfDials", value)
            End Set
        End Property

        ''' <summary>
        ''' Numero massimo di pagine inviabili in un solo fax
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumberOfPages As Integer
            Get
                Return Me.GetValueInt("NumberOfPages", 0)
            End Get
            Set(value As Integer)
                If (value <= 0) Then Throw New ArgumentOutOfRangeException("NumberOfPages")
                Me.SetValueInt("NumberOfPages", value)
            End Set
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PageLength As Integer
            Get
                Return Me.GetValueInt("PageLength", 0)
            End Get
            Set(value As Integer)
                If (value <= 0) Then Throw New ArgumentOutOfRangeException("PageLength")
                Me.SetValueInt("PageLength", value)
            End Set
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PageWidth As Integer
            Get
                Return Me.GetValueInt("PageWidth", 0)
            End Get
            Set(value As Integer)
                If (value <= 0) Then Throw New ArgumentOutOfRangeException("PageWidth")
                Me.SetValueInt("PageWidth", value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ora di invio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SendTime As Nullable(Of Date)
            Get
                Return Me.GetValueDate("SendTime")
            End Get
            Set(value As Nullable(Of Date))
                Me.SetValueDate("SendTime", value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del modem utilizzato per inviare il fax o da cui si è ricevuto il fax
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

        ''' <summary>
        ''' Restituisce o imposta il nome del punto operativo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NomePuntoOperativo As String
            Get
                Return Me.GetValueString("NomePuntoOperativo", "")
            End Get
            Set(value As String)
                Me.SetValueString("NomePuntoOperativo", Strings.Trim(value))
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la posizione geografica del destinatario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ToLocation As String
            Get
                Return Me.GetValueString("ToLocation", "")
            End Get
            Set(value As String)
                Me.SetValueString("ToLocation", Strings.Trim(value))
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'utente destinatario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ToUser As String
            Get
                Return Me.GetValueString("ToUser", "")
            End Get
            Set(value As String)
                Me.SetValueString("ToUser", Strings.Trim(value))
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero per le comunicazioni vocali con il destinatario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ToVoice As String
            Get
                Return Me.GetValueString("ToVoice", "")
            End Get
            Set(value As String)
                Me.SetValueString("ToVoice", Strings.Trim(value))
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del file da inviare come fax
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FileName As String
            Get
                Return Me.GetValueString("FileName", "")
            End Get
            Set(value As String)
                Me.SetValueString("FileName", Strings.Trim(value))
            End Set
        End Property

        Public Property Resolution As FaxResolution
            Get
                Return Me.GetValueInt("Resolution", FaxResolution.High)
            End Get
            Set(value As FaxResolution)
                Me.SetValueInt("Resolution", CInt(value))
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero del destinatario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TargetNumber As String
            Get
                Return Me.GetValueString("TargetNumber", "")
            End Get
            Set(value As String)
                Me.SetValueString("TargetNumber", Strings.Trim(value))
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero da anteporre per un eventuale accesso alla linea del centralino
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DialPrefix As String
            Get
                Return Me.GetValueString("DialPrefix", "")
            End Get
            Set(value As String)
                Me.SetValueString("DialPrefix", Strings.Trim(value))
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero ti tentativi di invio finora effettuati
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Tries As Integer
            Get
                Return Me.GetValueInt("Tries", "")
            End Get
            Set(value As Integer)
                Me.SetValueInt("Tries", value)
            End Set
        End Property

    End Class


 End Class
