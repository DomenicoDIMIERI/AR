Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

 

Partial Public Class Sistema

    <Serializable> _
    Public Class CEmailAccount
        Inherits DBObject

        Private m_Attivo As Boolean
        Private m_AccountType As String
        Private m_AccountName As String
        Private m_POPServer As String
        Private m_POPPort As Integer
        Private m_POPUserName As String
        Private m_POPPassword As String
        Private m_POPUseSSL As Boolean
        Private m_LastSync As Date?

        Public Sub New()
            Me.m_Attivo = True
            Me.m_AccountName = ""
            Me.m_AccountType = "POP3"
            Me.m_POPServer = "localhost"
            Me.m_POPPort = 110
            Me.m_POPUserName = ""
            Me.m_POPPassword = ""
            Me.m_POPUseSSL = False
            Me.m_LastSync = Nothing
        End Sub

        Public Sub New(ByVal userName As String, ByVal password As String)
            Me.New()
            Me.m_POPUserName = Trim(userName)
            Me.m_POPPassword = password
            Me.m_AccountName = Me.m_POPUserName
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
                If (DateUtils.Compare(value, oldValue) = 0) Then Exit Property
                Me.m_LastSync = value
                Me.DoChanged("LastSync", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se l'account è attivo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Attivo As Boolean
            Get
                Return Me.m_Attivo
            End Get
            Set(value As Boolean)
                If (Me.m_Attivo = value) Then Exit Property
                Me.m_Attivo = value
                Me.DoChanged("Attivo", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il tipo dell'account (POP3 e IMAP)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AccountType As String
            Get
                Return Me.m_AccountType
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_AccountType
                If (oldValue = value) Then Exit Property
                Me.m_AccountType = value
                Me.DoChanged("AccountType", value, oldValue)
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
             

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_EmailAccounts"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_Attivo = reader.Read("Attivo", Me.m_Attivo)
            Me.m_AccountName = reader.Read("AccountName", Me.m_AccountName)
            Me.m_AccountType = reader.Read("AccountType", Me.m_AccountType)
            Me.m_POPServer = reader.Read("POPServer", Me.m_POPServer)
            Me.m_POPPort = reader.Read("POPPort", Me.m_POPPort)
            Me.m_POPUserName = reader.Read("POPUserName", Me.m_POPUserName)
            Me.m_POPPassword = reader.Read("POPPassword", Me.m_POPPassword)
            Me.m_POPUseSSL = reader.Read("POPUseSSL", Me.m_POPUseSSL)
            Me.m_LastSync = reader.Read("LastSync", Me.m_LastSync)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Attivo", Me.m_Attivo)
            writer.Write("AccountName", Me.m_AccountName)
            writer.Write("AccountType", Me.m_AccountType)
            writer.Write("POPServer", Me.m_POPServer)
            writer.Write("POPPort", Me.m_POPPort)
            writer.Write("POPUserName", Me.m_POPUserName)
            writer.Write("POPPassword", Me.m_POPPassword)
            writer.Write("POPUseSSL", Me.m_POPUseSSL)
            writer.Write("LastSync", Me.m_LastSync)

            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Attivo" : Me.m_Attivo = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "AccountName" : Me.m_AccountName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "AccountType" : Me.m_AccountType = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "POPServer" : Me.m_POPServer = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "POPPort" : Me.m_POPPort = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "POPUserName" : Me.m_POPUserName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "POPPassword" : Me.m_POPPassword = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "POPUseSSL" : Me.m_POPUseSSL = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "LastSync" : Me.m_LastSync = XML.Utils.Serializer.DeserializeDate(fieldValue)

                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Attivo", Me.m_Attivo)
            writer.WriteAttribute("AccountName", Me.m_AccountName)
            writer.WriteAttribute("AccountType", Me.m_AccountType)
            writer.WriteAttribute("POPServer", Me.m_POPServer)
            writer.WriteAttribute("POPPort", Me.m_POPPort)
            writer.WriteAttribute("POPUserName", Me.m_POPUserName)
            writer.WriteAttribute("POPPassword", Me.m_POPPassword)
            writer.WriteAttribute("POPUseSSL", Me.m_POPUseSSL)
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
        End Sub

        Protected Overrides Sub DoChanged(propName As String, Optional newVal As Object = Nothing, Optional oldVal As Object = Nothing)
            MyBase.DoChanged(propName, newVal, oldVal)
            EMailer.doConfigChanged(New System.EventArgs)
        End Sub
 


    End Class


End Class