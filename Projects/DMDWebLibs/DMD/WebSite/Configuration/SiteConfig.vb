Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Partial Class WebSite

    <Flags> _
    Public Enum SiteFlags As Integer
        NONE = 0
        COMPRESS_RESPONSE = 1
        LOG_SESSIONS = 2
        LOG_PAGES = 4
        LOG_QUERYSTRINGS = 8
        LOG_POSTDATA = 16
        NOTIFY_UNHAUTORIZED = 32
        VERIFY_REMOTEIP = 64
        VERIFY_TIMERESTRICTIONS = 256
        VERIFY_CLIENTCERTIFICATE = 512
        LOG_DBQUERIES = 1024
    End Enum

    <Serializable> _
    Public Class SiteConfig
        Inherits DBObjectBase

        

        Private m_SiteName As String
        Private m_SiteDescription As String
        Private m_SiteURL As String
        Private m_InfoEMail As String
        Private m_PartitaIVA As String
        Private m_CodiceFiscale As String
        Private m_Telefono As String
        Private m_LogoURL As String
        Private m_Note As String
        Private m_SupportEMail As String
        Private m_PublicURL As String
        Private m_SimboloValuta As String
        Private m_DecimaliPerValuta As Integer
        Private m_DecimaliPerPercentuale As Integer
        Private m_Flags As SiteFlags
        Private m_NumberOfUploadsLimit As Integer
        Private m_UploadSpeedLimit As Integer
        Private m_UploadTimeOut As Integer
        Private m_ShortTimeOut As Integer
        Private m_LongTimeOut As Integer
        Private m_KeyWords As String
        Private m_UploadBufferSize As Integer
        Private m_CRMMaxCacheSize As Integer
        Private m_CRMUnloadFactor As Single
        Private m_DeleteLogFilesAfterNDays As Integer

        Public Sub New()
            Me.m_SiteName = ""
            Me.m_SiteDescription = ""
            Me.m_SiteURL = ""
            Me.m_InfoEMail = ""
            Me.m_PartitaIVA = ""
            Me.m_CodiceFiscale = ""
            Me.m_Telefono = ""
            Me.m_LogoURL = ""
            Me.m_Note = ""
            Me.m_SupportEMail = ""
            Me.m_PublicURL = ""
            Me.m_SimboloValuta = ""
            Me.m_DecimaliPerValuta = 2
            Me.m_DecimaliPerPercentuale = 2
            Me.m_Flags = SiteFlags.NONE
            Me.m_NumberOfUploadsLimit = -1
            Me.m_UploadSpeedLimit = 0
            Me.m_UploadTimeOut = 10 * 60
            Me.m_ShortTimeOut = 15
            Me.m_LongTimeOut = 120
            Me.m_KeyWords = ""
            Me.m_UploadBufferSize = 1024 * 10
            Me.m_CRMMaxCacheSize = 100
            Me.m_CRMUnloadFactor = 0.25
            Me.m_DeleteLogFilesAfterNDays = 30
        End Sub



        Public Property SiteName As String
            Get
                Return Me.m_SiteName
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_SiteName
                If (oldValue = value) Then Return
                Me.m_SiteName = value
                Me.DoChanged("SiteName", value, oldValue)
            End Set
        End Property

        Public Property SiteDescription As String
            Get
                Return Me.m_SiteDescription
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_SiteDescription
                If (oldValue = value) Then Return
                Me.m_SiteDescription = value
                Me.DoChanged("SiteDescription", value, oldValue)
            End Set
        End Property

        Public Property SiteURL As String
            Get
                Return Me.m_SiteURL
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_SiteURL
                If (oldValue = value) Then Return
                Me.m_SiteURL = value
                Me.DoChanged("SiteURL", value, oldValue)
            End Set
        End Property

        Public Property InfoEMail As String
            Get
                Return Me.m_InfoEMail
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_InfoEMail
                If (oldValue = value) Then Return
                Me.m_InfoEMail = value
                Me.DoChanged("InfoEMail", value, oldValue)
            End Set
        End Property

        Public Property PartitaIVA As String
            Get
                Return Me.m_PartitaIVA
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_PartitaIVA
                If (oldValue = value) Then Return
                Me.m_PartitaIVA = value
                Me.DoChanged("PartitaIVA", value, oldValue)
            End Set
        End Property

        Public Property CodiceFiscale As String
            Get
                Return Me.m_CodiceFiscale
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_CodiceFiscale
                If (value = oldValue) Then Return
                Me.m_CodiceFiscale = value
                Me.DoChanged("CodiceFiscale", value, oldValue)
            End Set
        End Property

        Public Property Telefono As String
            Get
                Return Me.m_Telefono
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Telefono
                If (oldValue = value) Then Return
                Me.m_Telefono = value
                Me.DoChanged("Telefono", value, oldValue)
            End Set
        End Property

        Public Property LogoURL As String
            Get
                Return Me.m_LogoURL
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_LogoURL
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_LogoURL = value
                Me.DoChanged("LogoURL", value, oldValue)
            End Set
        End Property

        Public Property Note As String
            Get
                Return Me.m_Note
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Note
                If (oldValue = value) Then Return
                Me.m_Note = value
                Me.DoChanged("Note", value, oldValue)
            End Set
        End Property

        Public Property SupportEMail As String
            Get
                Return Me.m_SupportEMail
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_SupportEMail
                If (oldValue = value) Then Return
                Me.DoChanged("SupportEMail", value, oldValue)
            End Set
        End Property

        Public Property Flags As SiteFlags
            Get
                Return Me.m_Flags
            End Get
            Set(value As SiteFlags)
                Dim oldValue As SiteFlags = Me.m_Flags
                If (oldValue = value) Then Return
                Me.m_Flags = value
                Me.DoChanged("Flags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di giorni per cui i files di log vengono mantenuti.
        ''' Se viene impostato un valore negativo i files di log non vengono eliminati
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DeleteLogFilesAfterNDays As Integer
            Get
                Return Me.m_DeleteLogFilesAfterNDays
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_DeleteLogFilesAfterNDays
                If (oldValue = value) Then Exit Property
                Me.m_DeleteLogFilesAfterNDays = value
                Me.DoChanged("DeleteLogFilesAfterNDays", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la dimensione massima dell'indice delle anagrafiche delle persone (mantenuto in memoria)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CRMMaxCacheSize As Integer
            Get
                Return Me.m_CRMMaxCacheSize
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_CRMMaxCacheSize
                If (oldValue = value) Then Exit Property
                Me.m_CRMMaxCacheSize = value
                Me.DoChanged("CRMMaxCacheSize", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la percentuale della dell'indice delle anagrafiche (in memoria) che viene scaricata quando si raggiunge la dimensione massima
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CRMUnloadFactor As Single
            Get
                Return Me.m_CRMUnloadFactor
            End Get
            Set(value As Single)
                Dim oldValue As Single = Me.m_CRMUnloadFactor
                If (value = oldValue) Then Exit Property
                Me.m_CRMUnloadFactor = value
                Me.DoChanged("CRMUnloadFactor", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UploadBufferSize As Integer
            Get
                Return Me.m_UploadBufferSize
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_UploadBufferSize
                If (oldValue = value) Then Exit Property
                Me.m_UploadBufferSize = value
                Me.DoChanged("UploadBufferSize", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisc o imposta il timeout in secondi per le richieste "veloci"
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ShortTimeOut As Integer
            Get
                Return Me.m_ShortTimeOut
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_ShortTimeOut
                If (oldValue = value) Then Exit Property
                Me.m_ShortTimeOut = value
                Me.DoChanged("ShortTimeOut", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il timeout in secondi per le richieste "lente"
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LongTimeOut As Integer
            Get
                Return Me.m_LongTimeOut
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_LongTimeOut
                If (oldValue = value) Then Exit Property
                Me.m_LongTimeOut = value
                Me.DoChanged("LongTimeOut", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o impostai il tempo, in scondi, entro cui considerare fallito un upload
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UploadTimeOut As Integer
            Get
                Return Me.m_UploadTimeOut
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_UploadTimeOut
                If (oldValue = value) Then Exit Property
                Me.m_UploadTimeOut = value
                Me.DoChanged("UploadTimeOut ", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o impostai l numero massimo di upload contemporanei concessi per il server
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NumberOfUploadsLimit As Integer
            Get
                Return Me.m_NumberOfUploadsLimit
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_NumberOfUploadsLimit
                If (oldValue = value) Then Exit Property
                Me.m_NumberOfUploadsLimit = value
                Me.DoChanged("NumberOfUploadsLimit", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il limite massimo di bytes al secondo inviabili per un singolo upload
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UploadSpeedLimit As Integer
            Get
                Return Me.m_UploadSpeedLimit
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_UploadSpeedLimit
                If (oldValue = value) Then Exit Property
                Me.m_UploadSpeedLimit = value
                Me.DoChanged("UploadSpeedLimit", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'elenco delle parole chiave inserite nella pagina
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property KeyWords As String
            Get
                Return Me.m_KeyWords
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_KeyWords
                If (oldValue = value) Then Exit Property
                Me.m_KeyWords = value
                Me.DoChanged("KeyWords", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il simbolo usato per la valuta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SimboloValuta As String
            Get
                Return Me.m_SimboloValuta
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_SimboloValuta
                If (oldValue = value) Then Exit Property
                Me.m_SimboloValuta = value
                Me.DoChanged("SimboloValuta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di decimali usati per la valuta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DecimaliPerValuta As Integer
            Get
                Return Me.m_DecimaliPerValuta
            End Get
            Set(value As Integer)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("DecimaliPerValuta non può essere un valore negativo")
                Dim oldValue As Integer = Me.m_DecimaliPerValuta
                Me.m_DecimaliPerValuta = value
                Me.DoChanged("DecimaliPerValuta", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di decimali usati per formattare i valori decimali
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DecimaliPerPercentuale As Integer
            Get
                Return Me.m_DecimaliPerPercentuale
            End Get
            Set(value As Integer)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("DecimaliPerPercentuale non può essere un valore negativo")
                Dim oldValue As Integer = Me.m_DecimaliPerPercentuale
                If (oldValue = value) Then Exit Property
                Me.m_DecimaliPerPercentuale = value
                Me.DoChanged("DecimaliPerPercentuale", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetModule() As CModule
            Return WebSite.Instance.Module
        End Function

        ''' <summary>
        ''' Restituisce la URL del percorso temporaneo predefinito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property TempFolder As String
            Get
                Return Me.PublicURL & "Temp/"
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta la url del percorso utilizzabile come cartella pubblica sul sito
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PublicURL As String
            Get
                Return Me.m_PublicURL
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                If (Right(value, 1) <> "/") Then value &= "/"
                Dim oldValue As String = Me.m_PublicURL
                If (oldValue = value) Then Exit Property
                Me.m_PublicURL = value
                Me.DoChanged("PublicURL", value, oldValue)
            End Set
        End Property
 

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se comprimere i dati inviati
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CompressResponse As Boolean
            Get
                Return TestFlag(Me.m_Flags, SiteFlags.COMPRESS_RESPONSE)
            End Get
            Set(value As Boolean)
                Dim oldValue As Boolean = Me.CompressResponse
                If (oldValue = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, SiteFlags.COMPRESS_RESPONSE, value)
                Me.DoChanged("CompressResponse", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il sistema deve effettuare il salvataggio di ogni sessione nel db di log
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LogSessions As Boolean
            Get
                Return TestFlag(Me.m_Flags, SiteFlags.LOG_SESSIONS)
            End Get
            Set(value As Boolean)
                Dim oldValue As Boolean = Me.LogSessions
                If (oldValue = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, SiteFlags.LOG_SESSIONS, value)
                Me.DoChanged("LogSessions", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se tutti i comandi inviati ai vari database devono essere registrati nel file di log
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LogDBCommands() As Boolean
            Get
                Return TestFlag(Me.m_Flags, SiteFlags.LOG_DBQUERIES)
            End Get
            Set(value As Boolean)
                Dim oldValue As Boolean = Me.LogDBCommands
                If (oldValue = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, SiteFlags.LOG_DBQUERIES, value)
                Me.DoChanged("LogDBCommands", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il sistema deve effettuare il salvataggio di ogni pagina nel db di log
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LogPages As Boolean
            Get
                Return TestFlag(Me.m_Flags, SiteFlags.LOG_PAGES)
            End Get
            Set(value As Boolean)
                Dim oldValue As Boolean = Me.LogPages
                If (oldValue = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, SiteFlags.LOG_PAGES, value)
                Me.DoChanged("LogPages", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il sistema deve memorizzare la URL comprensiva dai dati GET quando effettua il log delle pagine
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LogQueryStrings As Boolean
            Get
                Return TestFlag(Me.m_Flags, SiteFlags.LOG_QUERYSTRINGS)
            End Get
            Set(value As Boolean)
                Dim oldValue As Boolean = Me.LogQueryStrings
                If (oldValue = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, SiteFlags.LOG_QUERYSTRINGS, value)
                Me.DoChanged("LogQueryStrings", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il sistema deve memorizzare i dati di tipo POST quando effettua il log delle pagine
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LogPostData As Boolean
            Get
                Return TestFlag(Me.m_Flags, SiteFlags.LOG_POSTDATA)
            End Get
            Set(value As Boolean)
                Dim oldValue As Boolean = Me.LogPostData
                If (oldValue = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, SiteFlags.LOG_POSTDATA, value)
                Me.DoChanged("LogPostData", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il sistema deve memorizzare i tentativi di accesso non validi 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NotifyUnhautorized As Boolean
            Get
                Return TestFlag(Me.m_Flags, SiteFlags.NOTIFY_UNHAUTORIZED)
            End Get
            Set(value As Boolean)
                Dim oldValue As Boolean = Me.NotifyUnhautorized
                If (oldValue = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, SiteFlags.NOTIFY_UNHAUTORIZED, value)
                Me.DoChanged("NotifyUnhautorized", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il sistema deve verificare gli accessi in base all'IP del client 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property VerifyRemoteIP As Boolean
            Get
                Return TestFlag(Me.m_Flags, SiteFlags.VERIFY_REMOTEIP)
            End Get
            Set(value As Boolean)
                Dim oldValue As Boolean = Me.VerifyRemoteIP
                If (oldValue = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, SiteFlags.VERIFY_REMOTEIP, value)
                Me.DoChanged("VerifyRemoteIP", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il sistema deve verificare gli accessi in base agli orari prestabiliti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property VerifyTimeRestrictions As Boolean
            Get
                Return TestFlag(Me.m_Flags, SiteFlags.VERIFY_TIMERESTRICTIONS)
            End Get
            Set(value As Boolean)
                Dim oldValue As Boolean = Me.VerifyTimeRestrictions
                If (oldValue = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, SiteFlags.VERIFY_TIMERESTRICTIONS, value)
                Me.DoChanged("VerifyTimeRestrictions", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il sistema deve verificare il certificato client
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property VerifyClientCertificate As Boolean
            Get
                Return TestFlag(Me.m_Flags, SiteFlags.VERIFY_CLIENTCERTIFICATE)
            End Get
            Set(value As Boolean)
                Dim oldValue As Boolean = Me.VerifyClientCertificate
                If (oldValue = value) Then Exit Property
                Me.m_Flags = SetFlag(Me.m_Flags, SiteFlags.VERIFY_CLIENTCERTIFICATE, value)
                Me.DoChanged("VerifyClientCertificate", value, oldValue)
            End Set
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_SiteConfiguration"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_SiteName = reader.Read("SiteName", Me.m_SiteName)
            Me.m_SiteDescription = reader.Read("SiteDescription", Me.m_SiteDescription)
            Me.m_SiteURL = reader.Read("SiteURL", Me.m_SiteURL)
            Me.m_InfoEMail = reader.Read("InfoEMail", Me.m_InfoEMail)
            Me.m_PartitaIVA = reader.Read("PartitaIVA", Me.m_PartitaIVA)
            Me.m_CodiceFiscale = reader.Read("CodiceFiscale", Me.m_CodiceFiscale)
            Me.m_Telefono = reader.Read("Telefono", Me.m_Telefono)
            Me.m_LogoURL = reader.Read("LogoURL", Me.m_LogoURL)
            Me.m_Note = reader.Read("Note", Me.m_Note)
            Me.m_PublicURL = reader.Read("PublicFolder", Me.m_PublicURL)
            'm_ExternalDB = d bRis("ExternalDB") 
            Me.m_SupportEMail = reader.Read("SupportEMail", Me.m_SupportEMail)
            Me.m_SimboloValuta = reader.Read("SimboloValuta", Me.m_SimboloValuta)
            Me.m_DecimaliPerValuta = reader.Read("DecimaliPerValuta", Me.m_DecimaliPerValuta)
            Me.m_DecimaliPerPercentuale = reader.Read("DecimaliPerPercentuale", Me.m_DecimaliPerPercentuale)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_UploadSpeedLimit = reader.Read("UploadSpeedLimit", Me.m_UploadSpeedLimit)
            Me.m_NumberOfUploadsLimit = reader.Read("NumberOfUploadsLimit", Me.m_NumberOfUploadsLimit)
            Me.m_UploadTimeOut = reader.Read("UploadTimeOut", Me.m_UploadTimeOut)
            Me.m_ShortTimeOut = reader.Read("ShortTimeOut", Me.m_ShortTimeOut)
            Me.m_LongTimeOut = reader.Read("LongTimeOut", Me.m_LongTimeOut)
            Me.m_KeyWords = reader.Read("KeyWords", Me.m_KeyWords)
            Me.m_UploadBufferSize = reader.Read("UploadBufferSize", Me.m_UploadBufferSize)
            Me.m_CRMMaxCacheSize = reader.Read("CRMMaxCacheSize", Me.m_CRMMaxCacheSize)
            Me.m_CRMUnloadFactor = reader.Read("CRMUnloadFactor", Me.m_CRMUnloadFactor)
            Me.m_DeleteLogFilesAfterNDays = reader.Read("DelLogNDays", Me.m_DeleteLogFilesAfterNDays)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("SiteName", Me.m_SiteName)
            writer.Write("SiteDescription", Me.m_SiteDescription)
            writer.Write("SiteURL", Me.m_SiteURL)
            writer.Write("InfoEMail", Me.m_InfoEMail)
            writer.Write("PartitaIVA", Me.m_PartitaIVA)
            writer.Write("CodiceFiscale", Me.m_CodiceFiscale)
            writer.Write("Telefono", Me.m_Telefono)
            writer.Write("LogoURL", Me.m_LogoURL)
            writer.Write("Note", Me.m_Note)
            'dbRis("ExternalDB") = m_ExternalDB
            writer.Write("SupportEMail", Me.m_SupportEMail)
            writer.Write("PublicFolder", Me.m_PublicURL)
            writer.Write("SimboloValuta", Me.m_SimboloValuta)
            writer.Write("DecimaliPerValuta", Me.m_DecimaliPerValuta)
            writer.Write("DecimaliPerPercentuale", Me.m_DecimaliPerPercentuale)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("UploadSpeedLimit", Me.m_UploadSpeedLimit)
            writer.Write("NumberOfUploadsLimit", Me.m_NumberOfUploadsLimit)
            writer.Write("UploadTimeOut", Me.m_UploadTimeOut)
            writer.Write("ShortTimeOut", Me.m_ShortTimeOut)
            writer.Write("LongTimeOut", Me.m_LongTimeOut)
            writer.Write("KeyWords", Me.m_KeyWords)
            writer.Write("UploadBufferSize", Me.m_UploadBufferSize)
            writer.Write("CRMMaxCacheSize", Me.m_CRMMaxCacheSize)
            writer.Write("CRMUnloadFactor", Me.m_CRMUnloadFactor)
            writer.Write("DelLogNDays", Me.m_DeleteLogFilesAfterNDays)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("SiteURL", Me.m_SiteURL)
            writer.WriteAttribute("InfoEMail", Me.m_InfoEMail)
            writer.WriteAttribute("PartitaIVA", Me.m_PartitaIVA)
            writer.WriteAttribute("CodiceFiscale", Me.m_CodiceFiscale)
            writer.WriteAttribute("Telefono", Me.m_Telefono)
            writer.WriteAttribute("LogoURL", Me.m_LogoURL)
            writer.WriteAttribute("SupportEMail", Me.m_SupportEMail)
            writer.WriteAttribute("PublicFolder", Me.m_PublicURL)
            writer.WriteAttribute("SimboloValuta", Me.m_SimboloValuta)
            writer.WriteAttribute("DecimaliPerValuta", Me.m_DecimaliPerValuta)
            writer.WriteAttribute("DecimaliPerPercentuale", Me.m_DecimaliPerPercentuale)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("SiteName", Me.m_SiteName)
            writer.WriteAttribute("UploadSpeedLimit", Me.m_UploadSpeedLimit)
            writer.WriteAttribute("NumberOfUploadsLimit", Me.m_NumberOfUploadsLimit)
            writer.WriteAttribute("UploadTimeOut", Me.m_UploadTimeOut)
            writer.WriteAttribute("ShortTimeOut", Me.m_ShortTimeOut)
            writer.WriteAttribute("LongTimeOut", Me.m_LongTimeOut)
            writer.WriteAttribute("UploadBufferSize", Me.m_UploadBufferSize)
            writer.WriteAttribute("CRMMaxCacheSize", Me.m_CRMMaxCacheSize)
            writer.WriteAttribute("CRMUnloadFactor", Me.m_CRMUnloadFactor)
            writer.WriteAttribute("DelLogNDays", Me.m_DeleteLogFilesAfterNDays)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("SiteDescription", Me.m_SiteDescription)
            writer.WriteTag("Note", Me.m_Note)
            writer.WriteTag("KeyWords", Me.m_KeyWords)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "SiteName" : Me.m_SiteName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SiteDescription" : Me.m_SiteDescription = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SiteURL" : Me.m_SiteURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "InfoEMail" : Me.m_InfoEMail = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PartitaIVA" : Me.m_PartitaIVA = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "CodiceFiscale" : Me.m_CodiceFiscale = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Telefono" : Me.m_Telefono = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "LogoURL" : Me.m_LogoURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Note" : Me.m_Note = XML.Utils.Serializer.DeserializeString(fieldValue)
                    'dbRis("ExternalDB") = m_ExternalDB
                Case "SupportEMail" : Me.m_SupportEMail = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PublicFolder" : Me.m_PublicURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SimboloValuta" : Me.m_SimboloValuta = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DecimaliPerValuta" : Me.m_DecimaliPerValuta = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DecimaliPerPercentuale" : Me.m_DecimaliPerPercentuale = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "UploadSpeedLimit" : Me.m_UploadSpeedLimit = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NumberOfUploadsLimit" : Me.m_NumberOfUploadsLimit = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "UploadTimeOut" : Me.m_UploadTimeOut = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ShortTimeOut" : Me.m_ShortTimeOut = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "LongTimeOut" : Me.m_LongTimeOut = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "KeyWords" : Me.m_KeyWords = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "UploadBufferSize" : Me.m_UploadBufferSize = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "CRMMaxCacheSize" : Me.m_CRMMaxCacheSize = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "CRMUnloadFactor" : Me.m_CRMUnloadFactor = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "DelLogNDays" : Me.m_DeleteLogFilesAfterNDays = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Function Load() As Boolean
            If APPConn Is Nothing OrElse Not APPConn.IsOpen Then Return False

            Dim dbSQL As String = "SELECT * FROM [tbl_SiteConfiguration] ORDER BY [ID] ASC"
            Dim reader As New DBReader(APPConn.Tables("tbl_SiteConfiguration"), dbSQL)
            If reader.Read Then APPConn.Load(Me, reader)
            reader.Dispose()

            Return True
        End Function

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret) Then
                WebSite.Instance.SetConfiguration(Me)
            End If
            Return ret
        End Function


        Protected Overrides Function GetConnection() As CDBConnection
            Return Databases.APPConn
        End Function

        'Function LoadFromCollection(col, baseName)
        '    m_ID = Formats.ToInteger("&H" & col(baseName & "ID"))
        '    m_SiteName = "" & col(baseName & "SiteName")
        '    m_SiteDescription = "" & col(baseName & "SiteDescription")
        '    m_SiteURL = "" & col(baseName & "SiteURL")
        '    m_InfoEMail = "" & col(baseName & "InfoEMail")
        '    m_PartitaIVA = "" & col(baseName & "PartitaIVA")
        '    m_CodiceFiscale = "" & col(baseName & "CodiceFiscale")
        '    m_Telefono = "" & col(baseName & "Telefono")
        '    m_LogoURL = "" & col(baseName & "LogoURL")
        '    m_Note = "" & col(baseName & "Note")
        '    m_ExternalDB = "" & col(baseName & "ExternalDB")
        '    m_SupportEMail = "" & col(baseName & "SupportEMail")
        '    LoadFromCollection = True
        'End Function

        'Function SaveToCollection(col, baseName)
        '    col(baseName & "ID") = Hex(Formats.ToInteger(m_ID))
        '    col(baseName & "SiteName") = "" & m_SiteName
        '    col(baseName & "SiteDescription") = "" & m_SiteDescription
        '    col(baseName & "SiteURL") = "" & m_SiteURL
        '    col(baseName & "InfoEMail") = "" & m_InfoEMail
        '    col(baseName & "PartitaIVA") = "" & m_PartitaIVA
        '    col(baseName & "CodiceFiscale") = "" & m_CodiceFiscale
        '    col(baseName & "Telefono") = "" & m_Telefono
        '    col(baseName & "LogoURL") = "" & m_LogoURL
        '    col(baseName & "Note") = "" & m_Note
        '    col(baseName & "ExternalDB") = "" & m_ExternalDB
        '    col(baseName & "SupportEMail") = "" & m_SupportEMail
        '    SaveToCollection = True
        'End Function

        Protected Overrides Sub DoChanged(propName As String, Optional newVal As Object = Nothing, Optional oldVal As Object = Nothing)
            MyBase.DoChanged(propName, newVal, oldVal)
            WebSite.Instance.doConfigChanged(New System.EventArgs)
        End Sub





    End Class

 


End Class