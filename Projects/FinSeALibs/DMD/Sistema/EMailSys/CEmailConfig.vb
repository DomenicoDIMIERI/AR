Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

 

Partial Public Class Sistema

    <Flags> _
    Public Enum POPFlags As Integer
        None = 0

        ''' <summary>
        ''' Se vero indica che il sistema cercherà di scaricare automaticamente le nuove email
        ''' </summary>
        ''' <remarks></remarks>
        Enabled = 1

        ''' <summary>
        ''' Se vero indica che il sistema eliminerà la posta dal server dopo N giorni 
        ''' </summary>
        ''' <remarks></remarks>
        RemoveFromServerAfterDownload = 2
    End Enum

    <Flags> _
    Public Enum SMTPFlags As Integer
        None = 0
    End Enum

    <Serializable> _
    Public Class CEmailConig
        Inherits DBObjectBase

        Private m_DisplayName As String
        Private m_SMTPSendUsing As Integer
        Private m_SMTPServer As String
        Private m_SMTPAuthenticate As Integer
        Private m_SMTPUserName As String
        Private m_SMTPPassword As String
        Private m_SMTPServerPort As Integer
        Private m_SMTPUseSSL As Boolean
        Private m_SMTPConnectionTimeout As Integer
        Private m_POPServer As String
        Private m_POPPort As Integer
        Private m_POPUserName As String
        Private m_POPPassword As String
        Private m_POPUseSSL As Boolean
        Private m_SMTPValidateCertificate As Boolean
        Private m_POPFlags As POPFlags
        Private m_SMTPFlags As SMTPFlags
        Private m_RemoveFromServerAfterNDays As Integer
        Private m_CheckInterval As Integer
        Private m_SMTPLimitOutSent As Integer
        Private m_LastSync As Date?

        Public Sub New()
            Me.m_DisplayName = ""
            Me.m_SMTPSendUsing = 0 'cdoSendUsingPort
            Me.m_SMTPServer = "localhost"
            Me.m_SMTPAuthenticate = 0 ' cdoBasic
            Me.m_SMTPUserName = ""
            Me.m_SMTPPassword = ""
            Me.m_SMTPServerPort = 25
            Me.m_SMTPUseSSL = False
            Me.m_SMTPConnectionTimeout = 60
            Me.m_POPServer = "localhost"
            Me.m_POPPort = 110
            Me.m_POPUserName = ""
            Me.m_POPPassword = ""
            Me.m_POPUseSSL = False
            Me.m_SMTPValidateCertificate = True
            Me.m_POPFlags = Sistema.POPFlags.Enabled
            Me.m_SMTPFlags = Sistema.SMTPFlags.None
            Me.m_RemoveFromServerAfterNDays = 7
            Me.m_CheckInterval = 10
            Me.m_SMTPLimitOutSent = 0
            Me.m_LastSync = Nothing
        End Sub

        ''' <summary>
        ''' Restituisce o imposta la data e l'ora dell'ultima sincronizzazione avvenuta con il server
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LastSync As Date?
            Get
                Return Me.m_LastSync
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_LastSync
                If (Calendar.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_LastSync = value
                Me.DoChanged("LastSync", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il limite al numero di mail inviabili ogni minuto.
        ''' Se si inserise 0 o un vaore negativo non viene usato alcun limitatore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SMTPLimitOutSent As Integer
            Get
                Return Me.m_SMTPLimitOutSent
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_SMTPLimitOutSent
                If (oldValue = value) Then Exit Property
                Me.m_SMTPLimitOutSent = value
                Me.DoChanged("SMTPLimitOutSent", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'intervallo di tempo in minuti entro il quale controllare l'arrivo delle nuove email
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CheckInterval As Integer
            Get
                Return Me.m_CheckInterval
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_CheckInterval
                If (oldValue = value) Then Exit Property
                Me.m_CheckInterval = value
                Me.DoChanged("CheckInterval", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se eliminare i dati dal server dopo averli scaricati
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RemoveFromServerAfterDownload As Boolean
            Get
                Return TestFlag(Me.m_POPFlags, POPFlags.RemoveFromServerAfterDownload)
            End Get
            Set(value As Boolean)
                If (Me.RemoveFromServerAfterDownload = value) Then Exit Property
                Me.m_POPFlags = SetFlag(Me.m_POPFlags, POPFlags.RemoveFromServerAfterDownload, value)
                Me.DoChanged("RemoveFromServerAfterDownload", value, Not value)
            End Set
        End Property

        Public Property RemoveFromServerAfterNDays As Integer
            Get
                Return Me.m_RemoveFromServerAfterNDays
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_RemoveFromServerAfterNDays
                If (oldValue = value) Then Exit Property
                Me.m_RemoveFromServerAfterNDays = value
                Me.DoChanged("RemoveFromServerAfterNDays", value, oldValue)
            End Set
        End Property

        Public Property POPFlags As POPFlags
            Get
                Return Me.m_POPFlags
            End Get
            Set(value As POPFlags)
                Dim oldValue As POPFlags = Me.m_POPFlags
                If (oldValue = value) Then Exit Property
                Me.m_POPFlags = value
                Me.DoChanged("POPFlags", value, oldValue)
            End Set
        End Property

        Public Property SMTPFlags As SMTPFlags
            Get
                Return Me.m_SMTPFlags
            End Get
            Set(value As SMTPFlags)
                Dim oldValue As SMTPFlags = Me.m_SMTPFlags
                If (oldValue = value) Then Exit Property
                Me.m_SMTPFlags = value
                Me.DoChanged("SMTPFlags", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se il sistema deve sincronizzare automaticamente le email
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property POP3Enabled As Boolean
            Get
                Return TestFlag(Me.m_POPFlags, POPFlags.Enabled)
            End Get
            Set(value As Boolean)
                If (Me.POP3Enabled = value) Then Exit Property
                Me.m_POPFlags = SetFlag(Me.m_POPFlags, POPFlags.Enabled, value)
                Me.DoChanged("POP3Enabled", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del server POP
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property POPServer As String
            Get
                Return Me.m_POPServer
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_POPServer
                If (oldValue = value) Then Exit Property
                Me.m_POPServer = value
                Me.DoChanged("POPServer", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la porta del server POP
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property POPPort As Integer
            Get
                Return Me.m_POPPort
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_POPPort
                If (oldValue = value) Then Exit Property
                Me.m_POPPort = value
                Me.DoChanged("POPPort", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la porta del server POP
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property POPUseSSL As Boolean
            Get
                Return Me.m_POPUseSSL
            End Get
            Set(value As Boolean)
                If (Me.m_POPUseSSL = value) Then Exit Property
                Me.m_POPUseSSL = value
                Me.DoChanged("POPUseSSL", value, Not value)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta il nome utente per l'accesso al server POP
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property POPUserName As String
            Get
                Return Me.m_POPUserName
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_POPUserName
                If (oldValue = value) Then Exit Property
                Me.m_POPUserName = value
                Me.DoChanged("POPUserName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la password per l'accesso al server POP
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property POPPassword As String
            Get
                Return Me.m_POPPassword
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_POPPassword
                If (oldValue = value) Then Exit Property
                Me.m_POPPassword = value
                Me.DoChanged("POPPassword", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome visualizzato per le e-mail inviate dal sistema
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SMTPDisplayName As String
            Get
                Return Me.m_DisplayName
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_DisplayName
                If (oldValue = value) Then Exit Property
                Me.m_DisplayName = value
                Me.DoChanged("SMTPDisplayName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore che indica come inviare l'email
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SMTPSendUsing As Integer
            Get
                Return Me.m_SMTPSendUsing
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_SMTPSendUsing
                If (oldValue = value) Then Exit Property
                Me.m_SMTPSendUsing = value
                Me.DoChanged("SMTPSendUsing", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del server SMTP
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SMTPServer As String
            Get
                Return Me.m_SMTPServer
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_SMTPServer
                If (oldValue = value) Then Exit Property
                Me.m_SMTPServer = value
                Me.DoChanged("SMTPServer", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore che indica la modalità di autenticazione sul server SMTP
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SMTPAuthenticate As Integer
            Get
                Return Me.m_SMTPAuthenticate
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_SMTPAuthenticate
                If (oldValue = value) Then Exit Property
                Me.m_SMTPAuthenticate = value
                Me.DoChanged("SMTPAuthenticate", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la username utilizzata per l'accesso al server SMTP
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SMTPUserName As String
            Get
                Return Me.m_SMTPUserName
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_SMTPUserName
                If (oldValue = value) Then Exit Property
                Me.m_SMTPUserName = value
                Me.DoChanged("SMTPUserName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la password usata per l'accesso al server SMTP
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SMTPPassword As String
            Get
                Return Me.m_SMTPPassword
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_SMTPPassword
                If (oldValue = value) Then Exit Property
                Me.m_SMTPPassword = value
                Me.DoChanged("SMTPPassword", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la porta del server SMTP
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SMTPServerPort As Integer
            Get
                Return Me.m_SMTPServerPort
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_SMTPServerPort
                If (oldValue = value) Then Exit Property
                Me.m_SMTPServerPort = value

                Me.DoChanged("SMTPServerPort", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se utilizzare una connessione SSL con il server SMTP
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SMTPUseSSL As Boolean
            Get
                Return Me.m_SMTPUseSSL
            End Get
            Set(value As Boolean)
                If (Me.m_SMTPUseSSL = value) Then Exit Property
                Me.m_SMTPUseSSL = value
                Me.DoChanged("SMTPUseSSL", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore in secondi utilizzato per il timeout delle operazioni sul server SMTP
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SMTPConnectionTimeout As Integer
            Get
                Return Me.m_SMTPConnectionTimeout
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_SMTPConnectionTimeout
                If (oldValue = value) Then Exit Property
                Me.m_SMTPConnectionTimeout = value
                Me.DoChanged("SMTPConnectionTimeout", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se validare o meno il certificato remoto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SMTPValidateCertificate As Boolean
            Get
                Return Me.m_SMTPValidateCertificate
            End Get
            Set(value As Boolean)
                If (Me.m_SMTPValidateCertificate = value) Then Exit Property
                Me.m_SMTPValidateCertificate = value
                Me.DoChanged("SMTPValidateCertificate", value, Not value)
            End Set
        End Property

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_EMailConfig"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_SMTPSendUsing = reader.Read("SMTPSendUsing", Me.m_SMTPSendUsing)
            Me.m_SMTPServer = reader.Read("SMTPServer", Me.m_SMTPServer)
            Me.m_SMTPAuthenticate = reader.Read("SMTPAuthenticate", Me.m_SMTPAuthenticate)
            Me.m_SMTPUserName = reader.Read("SMTPUserName", Me.m_SMTPUserName)
            Me.m_SMTPPassword = reader.Read("SMTPPassword", Me.m_SMTPPassword)
            Me.m_SMTPServerPort = reader.Read("SMTPServerPort", Me.m_SMTPServerPort)
            Me.m_SMTPUseSSL = reader.Read("SMTPUseSSL", Me.m_SMTPUseSSL)
            Me.m_SMTPConnectionTimeout = reader.Read("SMTPConnectionTimeout", Me.m_SMTPConnectionTimeout)
            Me.m_DisplayName = reader.Read("SMTPDisplayName", Me.m_DisplayName)
            Me.m_POPServer = reader.Read("POPServer", Me.m_POPServer)
            Me.m_POPPort = reader.Read("POPPort", Me.m_POPPort)
            Me.m_POPUserName = reader.Read("POPUserName", Me.m_POPUserName)
            Me.m_POPPassword = reader.Read("POPPassword", Me.m_POPPassword)
            Me.m_POPUseSSL = reader.Read("POPUseSSL", Me.m_POPUseSSL)
            Me.m_SMTPValidateCertificate = reader.Read("SMTPValidateCertificate", Me.m_SMTPValidateCertificate)
            Me.m_POPFlags = reader.Read("POPFlags", Me.m_POPFlags)
            Me.m_SMTPFlags = reader.Read("SMTPFlags", Me.m_SMTPFlags)
            Me.m_RemoveFromServerAfterNDays = reader.Read("RemoveFromServerAfterNDays", Me.m_RemoveFromServerAfterNDays)
            Me.m_CheckInterval = reader.Read("CheckInterval", Me.m_CheckInterval)
            Me.m_SMTPLimitOutSent = reader.Read("SMTPLimitOutSent", Me.m_SMTPLimitOutSent)
            Me.m_LastSync = reader.Read("LastSync", Me.m_LastSync)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("SMTPSendUsing", Me.m_SMTPSendUsing)
            writer.Write("SMTPServer", Me.m_SMTPServer)
            writer.Write("SMTPAuthenticate", Me.m_SMTPAuthenticate)
            writer.Write("SMTPUserName", Me.m_SMTPUserName)
            writer.Write("SMTPPassword", Me.m_SMTPPassword)
            writer.Write("SMTPServerPort", Me.m_SMTPServerPort)
            writer.Write("SMTPUseSSL", Me.m_SMTPUseSSL)
            writer.Write("SMTPConnectionTimeout", Me.m_SMTPConnectionTimeout)
            writer.Write("SMTPDisplayName", Me.m_DisplayName)
            writer.Write("POPServer", Me.m_POPServer)
            writer.Write("POPPort", Me.m_POPPort)
            writer.Write("POPUserName", Me.m_POPUserName)
            writer.Write("POPPassword", Me.m_POPPassword)
            writer.Write("SMTPValidateCertificate", Me.m_SMTPValidateCertificate)
            writer.Write("POPUseSSL", Me.m_POPUseSSL)
            writer.Write("POPFlags", Me.m_POPFlags)
            writer.Write("SMTPFlags", Me.m_SMTPFlags)
            writer.Write("RemoveFromServerAfterNDays", Me.m_RemoveFromServerAfterNDays)
            writer.Write("CheckInterval", Me.m_CheckInterval)
            writer.Write("SMTPLimitOutSent", Me.m_SMTPLimitOutSent)
            writer.Write("LastSync", Me.m_LastSync)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "SMTPSendUsing" : Me.m_SMTPSendUsing = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SMTPServer" : Me.m_SMTPServer = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SMTPAuthenticate" : Me.m_SMTPAuthenticate = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SMTPUserName" : Me.m_SMTPUserName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SMTPPassword" : Me.m_SMTPPassword = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SMTPServerPort" : Me.m_SMTPServerPort = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SMTPUseSSL" : Me.m_SMTPUseSSL = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "SMTPConnectionTimeout" : Me.m_SMTPConnectionTimeout = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SMTPDisplayName" : Me.m_DisplayName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "POPServer" : Me.m_POPServer = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "POPPort" : Me.m_POPPort = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "POPUserName" : Me.m_POPUserName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "POPPassword" : Me.m_POPPassword = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SMTPValidateCertificate" : Me.m_SMTPValidateCertificate = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "POPUseSSL" : Me.m_POPUseSSL = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "POPFlags" : Me.m_POPFlags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SMTPFlags" : Me.m_SMTPFlags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "RemoveFromServerAfterNDays" : Me.m_RemoveFromServerAfterNDays = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "CheckInterval" : Me.m_CheckInterval = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SMTPLimitOutSent" : Me.m_SMTPLimitOutSent = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "LastSync" : Me.m_LastSync = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("SMTPSendUsing", Me.m_SMTPSendUsing)
            writer.WriteAttribute("SMTPServer", Me.m_SMTPServer)
            writer.WriteAttribute("SMTPAuthenticate", Me.m_SMTPAuthenticate)
            writer.WriteAttribute("SMTPUserName", Me.m_SMTPUserName)
            writer.WriteAttribute("SMTPPassword", Me.m_SMTPPassword)
            writer.WriteAttribute("SMTPServerPort", Me.m_SMTPServerPort)
            writer.WriteAttribute("SMTPUseSSL", Me.m_SMTPUseSSL)
            writer.WriteAttribute("SMTPConnectionTimeout", Me.m_SMTPConnectionTimeout)
            writer.WriteAttribute("SMTPDisplayName", Me.m_DisplayName)
            writer.WriteAttribute("POPServer", Me.m_POPServer)
            writer.WriteAttribute("POPPort", Me.m_POPPort)
            writer.WriteAttribute("POPUserName", Me.m_POPUserName)
            writer.WriteAttribute("POPPassword", Me.m_POPPassword)
            writer.WriteAttribute("SMTPValidateCertificate", Me.m_SMTPValidateCertificate)
            writer.WriteAttribute("POPUseSSL", Me.m_POPUseSSL)
            writer.WriteAttribute("POPFlags", Me.m_POPFlags)
            writer.WriteAttribute("SMTPFlags", Me.m_SMTPFlags)
            writer.WriteAttribute("RemoveFromServerAfterNDays", Me.m_RemoveFromServerAfterNDays)
            writer.WriteAttribute("CheckInterval", Me.m_CheckInterval)
            writer.WriteAttribute("SMTPLimitOutSent", Me.m_SMTPLimitOutSent)
            writer.WriteAttribute("LastSync", Me.m_LastSync)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Sub Load()
            Dim tbl As DMD.Databases.CDBTable = APPConn.Tables.GetItemByKey(Me.GetTableName)
            Dim col As DMD.Databases.CDBEntityField = tbl.Fields.GetItemByKey("LastSync")
            If (col Is Nothing) Then
                col = tbl.Fields.Add("LastSync", GetType(Date))
                col.Create()
            End If

            Dim reader As DBReader
            Dim dbSQL As String = "SELECT * FROM [" & Me.GetTableName & "] ORDER BY [ID] ASC"
            reader = New DBReader(APPConn.Tables(Me.GetTableName), dbSQL)
            If reader.Read Then
                APPConn.Load(Me, reader)
            End If
            reader.Dispose()
            EMailer.SynchronizeInterval = Me.CheckInterval
            EMailer.AutoSynchronize = Me.POP3Enabled

        End Sub

        Protected Overrides Sub DoChanged(propName As String, Optional newVal As Object = Nothing, Optional oldVal As Object = Nothing)
            MyBase.DoChanged(propName, newVal, oldVal)
            EMailer.doConfigChanged(New System.EventArgs)
        End Sub

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            EMailer.SetConfig(Me)
            EMailer.SynchronizeInterval = Me.CheckInterval
            EMailer.AutoSynchronize = Me.POP3Enabled
        End Sub



    End Class


End Class