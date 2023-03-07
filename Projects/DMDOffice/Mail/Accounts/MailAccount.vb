Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports DMD.Net.Mime
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD

Partial Class Office

    Public Enum SMTPTipoCrittografica As Integer
        Nessuna = 0
        SSL = 1
        TLS = 2
        Automatica = 3
    End Enum

    ''' <summary>
    ''' Rappresenta un account di posta elettronica configurato sul sistema
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable>
    Public Class MailAccount
        Inherits DBObjectPO

        Private Const DELAFTERRECYCLECLEAN = 1
        Private Const DELAFTERRECYCLE = 2
        Private Const DELAFTERNDAYS = 3

        'Parametri per la ricezione
        Private m_AccountName As String
        Private m_DefaultFolderID As Integer
        Private m_DefaultFolder As MailFolder
        Private m_UserName As String
        Private m_Password As String
        Private m_ServerName As String
        Private m_ServerPort As Integer                 'Porta per la ricezione
        Private m_eMailAddress As String                'Indirizzo email del mittente
        Private m_Protocol As String                    'Nome del protocollo (POP3 o IMPAP)
        Private m_UseSSL As Boolean                     'Se vero la connessione avviene su Secure Socket Layer

        Private m_SMTPServerName As String              'Nome del server smtp (per l'invio)
        Private m_SMTPPort As Integer                   'Porta del server smpt (per la ricezione)
        Private m_ReplayTo As String                    'Indirizzo usato per le risposte
        Private m_DisplayName As String                 'Nome del mittente visualizzato dai destinatari
        Private m_SMTPUserName As String                'UserName utilizzato per l'invio
        Private m_SMTPPassword As String                'Password utilizzata per l'invio
        Private m_PopBeforeSMPT As Boolean              'Se vero il programma effettua prima l'accesso al server POP3

        Private m_Flags As Integer
        Private m_DelServerAfterDays As Integer         'Rimuove le email dal server dopo N giorni da quando sono stato scaricate

        Private m_TimeOut As Integer
        Private m_SMTPCrittografia As SMTPTipoCrittografica
        Private m_LastSync As Date?

        Private m_FirmaPerNuoviMessaggi As String
        Private m_FirmaPerRisposte As String

        Private m_ApplicationID As Integer
        Private m_Application As MailApplication

        Public Sub New()
            Me.m_AccountName = ""
            Me.m_DefaultFolder = Nothing
            Me.m_DefaultFolderID = 0
            Me.m_UserName = ""
            Me.m_Password = ""
            Me.m_ServerName = ""
            Me.m_ServerPort = 110
            Me.m_eMailAddress = ""
            Me.m_Protocol = "POP3"
            Me.m_UseSSL = False

            Me.m_SMTPServerName = "127.0.0.1"
            Me.m_SMTPPort = 25
            Me.m_ReplayTo = ""
            Me.m_DisplayName = ""
            Me.m_SMTPUserName = ""
            Me.m_SMTPPassword = ""
            Me.m_PopBeforeSMPT = False

            Me.m_Flags = 0
            Me.m_DelServerAfterDays = 0
            Me.m_TimeOut = 120
            Me.m_SMTPCrittografia = SMTPTipoCrittografica.Nessuna
            Me.m_LastSync = Nothing

            Me.m_FirmaPerNuoviMessaggi = ""
            Me.m_FirmaPerRisposte = ""

            Me.m_ApplicationID = 0
            Me.m_Application = Nothing
        End Sub

        Public Sub New(ByVal host As String, ByVal userName As String, ByVal password As String)
            Me.New()
            Me.m_ServerName = host
            Me.m_UserName = userName
            Me.m_Password = password
        End Sub

        Public Property DefaultFolder As MailFolder
            Get
                If (Me.m_DefaultFolder Is Nothing) Then
                    If (Me.Application IsNot Nothing) Then
                        Me.m_DefaultFolder = Me.Application.GetFolderById(Me.m_DefaultFolderID)
                        If (Me.m_DefaultFolder Is Nothing) Then Me.m_DefaultFolder = Me.Application.Root.Inbox
                    End If
                End If
                Return Me.m_DefaultFolder
            End Get
            Set(value As MailFolder)
                Dim oldValue As MailFolder = Me.DefaultFolder
                If (oldValue Is value) Then Return
                Me.m_DefaultFolder = value
                Me.m_DefaultFolderID = GetID(value)
                Me.DoChanged("DefaultFolder", value, oldValue)
            End Set
        End Property

        Public Property DefaultFolderID As Integer
            Get
                Return GetID(Me.m_DefaultFolder, Me.m_DefaultFolderID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.DefaultFolderID
                If (oldValue = value) Then Return
                Me.m_DefaultFolderID = value
                Me.m_DefaultFolder = Nothing
                Me.DoChanged("DefaultFolderID", value, oldValue)
            End Set
        End Property

        Public Property ApplicationID As Integer
            Get
                Return GetID(Me.m_Application, Me.m_ApplicationID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.ApplicationID
                If (oldValue = value) Then Return
                Me.m_ApplicationID = value
                Me.m_Application = Nothing
                Me.DoChanged("ApplicationID", value, oldValue)
            End Set
        End Property

        Public Property Application As MailApplication
            Get
                If (Me.m_Application Is Nothing) Then Me.m_Application = Office.Mails.Applications.GetItemById(Me.m_ApplicationID)
                Return Me.m_Application
            End Get
            Set(value As MailApplication)
                Dim oldValue As MailApplication = Me.Application
                If (oldValue Is value) Then Return
                Me.m_Application = value
                Me.m_ApplicationID = GetID(value)
                Me.DoChanged("Application", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetApplication(ByVal app As MailApplication)
            Me.m_Application = app
            Me.m_ApplicationID = GetID(app)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta la firma da inserire in coda ai nuovi messaggi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FirmaPerNuoviMessaggi As String
            Get
                Return Me.m_FirmaPerNuoviMessaggi
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_FirmaPerNuoviMessaggi
                If (oldValue = value) Then Exit Property
                Me.m_FirmaPerNuoviMessaggi = value
                Me.DoChanged("FirmaPerNuoviMessaggi", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la firma da inserire in coda ai messaggi inviati come risposta o inoltrati
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FirmaPerRisposte As String
            Get
                Return Me.m_FirmaPerRisposte
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_FirmaPerRisposte
                If (oldValue = value) Then Exit Property
                Me.m_FirmaPerRisposte = value
                Me.DoChanged("FirmaPerRisposte", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta la data dell'ultima sincronizzazione completa avvenuta con il server
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LastStync As Date?
            Get
                Return Me.m_LastSync
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_LastSync
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_LastSync = value
                Me.DoChanged("LastSync", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo di crittografia utilizzato dal server della posta in uscita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SMTPCrittografia As SMTPTipoCrittografica
            Get
                Return Me.m_SMTPCrittografia
            End Get
            Set(value As SMTPTipoCrittografica)
                Dim oldValue As SMTPTipoCrittografica = Me.m_SMTPCrittografia
                If (oldValue = value) Then Exit Property
                Me.m_SMTPCrittografia = value
                Me.DoChanged("SMTPCrittografia", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tempo massimo di attesa per scaricare i messaggi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TimeOut As Integer
            Get
                Return Me.m_TimeOut
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_TimeOut
                If (oldValue = value) Then Exit Property
                Me.m_TimeOut = value
                Me.DoChanged("TimeOut", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome dell'account
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AccountName As String
            Get
                Return Me.m_AccountName
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_AccountName
                If (oldValue = value) Then Exit Property
                Me.m_AccountName = value
                Me.DoChanged("AccountName", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restitusice o imposta la username utilizzata per accedere al server della posta in arrivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UserName As String
            Get
                Return Me.m_UserName
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_UserName
                If (oldValue = value) Then Exit Property
                Me.m_UserName = value
                Me.DoChanged("UserName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la password utilizzata per accedere al server della posta in arrivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Password As String
            Get
                Return Me.m_Password
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Password
                If (oldValue = value) Then Exit Property
                Me.m_Password = value
                Me.DoChanged("Password", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del server della posta in arrivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ServerName As String
            Get
                Return Me.m_ServerName
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_ServerName
                If (oldValue = value) Then Exit Property
                Me.m_ServerName = value
                Me.DoChanged("ServerName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero della porta per la connessione al server della posta in arrivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ServerPort As Integer
            Get
                Return Me.m_ServerPort
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_ServerPort
                If (oldValue = value) Then Exit Property
                Me.m_ServerPort = value
                Me.DoChanged("ServerPort", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo di posta elettronica gestito da questo account
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property eMailAddress As String
            Get
                Return Me.m_eMailAddress
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_eMailAddress
                If (oldValue = value) Then Exit Property
                Me.m_eMailAddress = value
                Me.DoChanged("eMailAddress", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del protocollo utilizzate per ricevere le email (POP3 o IMAP)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Protocol As String
            Get
                Return Me.m_Protocol
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Protocol
                If (oldValue = value) Then Exit Property
                Me.m_Protocol = value
                Me.DoChanged("Protocol", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se la connessione al server della posta in arrivo deve avvenire su SSL
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UseSSL As Boolean
            Get
                Return Me.m_UseSSL
            End Get
            Set(value As Boolean)
                If (Me.m_UseSSL = value) Then Exit Property
                Me.m_UseSSL = value
                Me.DoChanged("UseSSL", value, Not value)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta il nome visualizzato per le email inviate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DisplayName As String
            Get
                Return Me.m_DisplayName
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_DisplayName
                If (oldValue = value) Then Exit Property
                Me.m_DisplayName = value
                Me.DoChanged("DisplayName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome del server per la posta in uscita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SMTPServerName As String
            Get
                Return Me.m_SMTPServerName
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_SMTPServerName
                If (oldValue = value) Then Exit Property
                Me.m_SMTPServerName = value
                Me.DoChanged("SMTPServerName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la porta per la connessione al server della posta in uscita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SMTPPort As Integer
            Get
                Return Me.m_SMTPPort
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_SMTPPort
                If (oldValue = value) Then Exit Property
                Me.m_SMTPPort = value
                Me.DoChanged("SMTPPort", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'indirizzo per le risposte
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ReplayTo As String
            Get
                Return Me.m_ReplayTo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_ReplayTo
                If (oldValue = value) Then Exit Property
                Me.m_ReplayTo = value
                Me.DoChanged("ReplayTo", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome utente per il server della posta in uscita
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
        ''' Restituisce o imposta la password per l'accesso al server della posta in uscita
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SMTPPassword As String
            Get
                Return Me.m_SMTPPassword
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_SMTPPassword
                If (oldValue = value) Then Exit Property
                Me.m_SMTPPassword = value
                Me.DoChanged("STMPPassword", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se accedere al server SMTP solo dopo aver effettuato il login su POP
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property POPBeforeSMTP As Boolean
            Get
                Return Me.m_PopBeforeSMPT
            End Get
            Set(value As Boolean)
                If (Me.m_PopBeforeSMPT = value) Then Exit Property
                Me.m_PopBeforeSMPT = value
                Me.DoChanged("POPBeforeSMTP", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore che indica se le email devono essere eliminate dal server dopo aver svuotato il cestino
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DelServerAfterRecycleClean As Boolean
            Get
                Return (Me.m_Flags And DELAFTERRECYCLECLEAN) = DELAFTERRECYCLECLEAN
            End Get
            Set(value As Boolean)
                If (Me.DelServerAfterRecycleClean = value) Then Exit Property
                If (value) Then
                    Me.m_Flags = Me.m_Flags Or DELAFTERRECYCLECLEAN
                Else
                    Me.m_Flags = Me.m_Flags And Not DELAFTERRECYCLECLEAN
                End If
                Me.DoChanged("DelServerAfterRecycleClean", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore che indica se le email devono essere eliminate dal server dopo averle cestinate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DelServerAfterRecycle As Boolean
            Get
                Return (Me.m_Flags And DELAFTERRECYCLE) = DELAFTERRECYCLE
            End Get
            Set(value As Boolean)
                If (Me.DelServerAfterRecycle = value) Then Exit Property
                If (value) Then
                    Me.m_Flags = Me.m_Flags Or DELAFTERRECYCLE
                Else
                    Me.m_Flags = Me.m_Flags And Not DELAFTERRECYCLE
                End If
                Me.DoChanged("DelServerAfterRecycle", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore che indica se le email devono essere eliminate dopo N giorni
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DelServerAfterNDays As Boolean
            Get
                Return TestFlag(Me.m_Flags, DELAFTERNDAYS)
            End Get
            Set(value As Boolean)
                If (Me.DelServerAfterNDays = value) Then Exit Property
                If (value) Then
                    Me.m_Flags = Me.m_Flags Or DELAFTERNDAYS
                Else
                    Me.m_Flags = Me.m_Flags And Not DELAFTERNDAYS
                End If
                Me.DoChanged("DelServerAfterNDays", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il numero di giorni dalla data di scaricamento per eliminare le email dal server.
        ''' 0 indica che le email non verranno eliminate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DelServerAfterDays As Integer
            Get
                Return Me.m_DelServerAfterDays
            End Get
            Set(value As Integer)
                If (value < 0) Then Throw New ArgumentOutOfRangeException("value")
                Dim oldValue As Integer = Me.m_DelServerAfterDays
                If (oldValue = value) Then Exit Property
                Me.m_DelServerAfterDays = value
                Me.DoChanged("DelServerAfterDays", value, oldValue)
            End Set
        End Property

        Protected Overrides Function SaveToDatabase(dbConn As CDBConnection, force As Boolean) As Boolean
            Dim ret As Boolean = MyBase.SaveToDatabase(dbConn, force)
            If (ret) Then
                Dim app As MailApplication = Me.Application
                If (app IsNot Nothing) Then
                    Dim i As Integer = app.Accounts.IndexOf(Me)
                    If (i >= 0) Then
                        If (Me.Stato = ObjectStatus.OBJECT_VALID) Then
                            If Not (app.Accounts(i) Is Me) Then app.Accounts(i) = Me
                        Else
                            app.Accounts.RemoveAt(i)
                        End If
                    Else
                        If (Me.Stato = ObjectStatus.OBJECT_VALID) Then
                            app.Accounts.Add(Me)
                        End If
                    End If
                End If
            End If
            Return ret
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_EmailAccounts"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_AccountName = reader.Read("AccountName", Me.m_AccountName)
            Me.m_DefaultFolderID = reader.Read("DefaultFolderID", Me.m_DefaultFolderID)
            Me.m_UserName = reader.Read("UserName", Me.m_UserName)
            Me.m_Password = reader.Read("Password", Me.m_Password)
            Me.m_ServerName = reader.Read("ServerName", Me.m_ServerName)
            Me.m_ServerPort = reader.Read("ServerPort", Me.m_ServerPort)
            Me.m_eMailAddress = reader.Read("eMailAddress", Me.m_eMailAddress)
            Me.m_Protocol = reader.Read("Protocol", Me.m_Protocol)
            Me.m_UseSSL = reader.Read("UseSSL", Me.m_UseSSL)
            Me.m_SMTPServerName = reader.Read("SMTPServerName", Me.m_SMTPServerName)
            Me.m_SMTPPort = reader.Read("SMTPPort", Me.m_SMTPPort)
            Me.m_ReplayTo = reader.Read("ReplayTo", Me.m_ReplayTo)
            Me.m_DisplayName = reader.Read("DisplayName", Me.m_DisplayName)
            Me.m_SMTPUserName = reader.Read("SMTPUserName", Me.m_SMTPUserName)
            Me.m_SMTPPassword = reader.Read("SMTPPassword", Me.m_SMTPPassword)
            Me.m_PopBeforeSMPT = reader.Read("PopBeforeSMTP", Me.m_PopBeforeSMPT)
            Me.m_Flags = reader.Read("Flags", Me.m_Flags)
            Me.m_DelServerAfterDays = reader.Read("DelAfterDays", Me.m_DelServerAfterDays)
            Me.m_TimeOut = reader.Read("TimeOut", Me.m_TimeOut)
            Me.m_LastSync = reader.Read("LastSync", Me.m_LastSync)
            Me.m_FirmaPerNuoviMessaggi = reader.Read("FirmaPerNuoviMessaggi", Me.m_FirmaPerNuoviMessaggi)
            Me.m_FirmaPerRisposte = reader.Read("FirmaPerRisposte", Me.m_FirmaPerRisposte)
            Me.m_ApplicationID = reader.Read("ApplicationID", Me.m_ApplicationID)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("AccountName", Me.m_AccountName)
            writer.Write("DefaultFolderID", Me.DefaultFolderID)
            writer.Write("UserName", Me.m_UserName)
            writer.Write("Password", Me.m_Password)
            writer.Write("ServerName", Me.m_ServerName)
            writer.Write("ServerPort", Me.m_ServerPort)
            writer.Write("eMailAddress", Me.m_eMailAddress)
            writer.Write("Protocol", Me.m_Protocol)
            writer.Write("UseSSL", Me.m_UseSSL)
            writer.Write("SMTPServerName", Me.m_SMTPServerName)
            writer.Write("SMTPPort", Me.m_SMTPPort)
            writer.Write("ReplayTo", Me.m_ReplayTo)
            writer.Write("DisplayName", Me.m_DisplayName)
            writer.Write("SMTPUserName", Me.m_SMTPUserName)
            writer.Write("SMTPPassword", Me.m_SMTPPassword)
            writer.Write("PopBeforeSMTP", Me.m_PopBeforeSMPT)
            writer.Write("Flags", Me.m_Flags)
            writer.Write("DelAfterDays", Me.m_DelServerAfterDays)
            writer.Write("TimeOut", Me.m_TimeOut)
            writer.Write("LastSync", Me.m_LastSync)
            writer.Write("FirmaPerNuoviMessaggi", Me.m_FirmaPerNuoviMessaggi)
            writer.Write("FirmaPerRisposte", Me.m_FirmaPerRisposte)
            writer.Write("ApplicationID", Me.ApplicationID)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("AccountName", Me.m_AccountName)
            writer.WriteAttribute("DefaultFolderID", Me.DefaultFolderID)
            writer.WriteAttribute("UserName", Me.m_UserName)
            writer.WriteAttribute("Password", Me.m_Password)
            writer.WriteAttribute("ServerName", Me.m_ServerName)
            writer.WriteAttribute("ServerPort", Me.m_ServerPort)
            writer.WriteAttribute("eMailAddress", Me.m_eMailAddress)
            writer.WriteAttribute("Protocol", Me.m_Protocol)
            writer.WriteAttribute("UseSSL", Me.m_UseSSL)
            writer.WriteAttribute("SMTPServerName", Me.m_SMTPServerName)
            writer.WriteAttribute("SMTPPort", Me.m_SMTPPort)
            writer.WriteAttribute("ReplayTo", Me.m_ReplayTo)
            writer.WriteAttribute("DisplayName", Me.m_DisplayName)
            writer.WriteAttribute("SMTPUserName", Me.m_SMTPUserName)
            writer.WriteAttribute("SMTPPassword", Me.m_SMTPPassword)
            writer.WriteAttribute("PopBeforeSMTP", Me.m_PopBeforeSMPT)
            writer.WriteAttribute("Flags", Me.m_Flags)
            writer.WriteAttribute("DelAfterDays", Me.m_DelServerAfterDays)
            writer.WriteAttribute("TimeOut", Me.m_TimeOut)
            writer.WriteAttribute("LastSync", Me.m_LastSync)
            writer.WriteAttribute("ApplicationID", Me.ApplicationID)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("FirmaPerNuoviMessaggi", Me.m_FirmaPerNuoviMessaggi)
            writer.WriteTag("FirmaPerRisposte", Me.m_FirmaPerRisposte)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "AccountName" : Me.m_AccountName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DefaultFolderID" : Me.m_DefaultFolderID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "UserName" : Me.m_UserName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Password" : Me.m_Password = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ServerName" : Me.m_ServerName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ServerPort" : Me.m_ServerPort = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "eMailAddress" : Me.m_eMailAddress = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Protocol" : Me.m_Protocol = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "UseSSL" : Me.m_UseSSL = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "SMTPServerName" : Me.m_SMTPServerName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SMTPPort" : Me.m_SMTPPort = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ReplayTo" : Me.m_ReplayTo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DisplayName" : Me.m_DisplayName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SMTPUserName" : Me.m_SMTPUserName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SMTPPassword" : Me.m_SMTPPassword = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PopBeforeSMTP" : Me.m_PopBeforeSMPT = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Flags" : Me.m_Flags = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DelAfterDays" : Me.m_DelServerAfterDays = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TimeOut" : Me.m_TimeOut = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SMTPCrittografia" : Me.m_SMTPCrittografia = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "LastSync" : Me.m_LastSync = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "FirmaPerNuoviMessaggi" : Me.m_FirmaPerNuoviMessaggi = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "FirmaPerRisposte" : Me.m_FirmaPerRisposte = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ApplicationID" : Me.m_ApplicationID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Overrides Function GetModule() As CModule
            Return Office.Mails.Accounts.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Mails.Database
        End Function
    End Class

End Class
