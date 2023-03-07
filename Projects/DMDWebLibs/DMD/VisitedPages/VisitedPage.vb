Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Partial Class WebSite

    <Serializable> _
    Public Class VisitedPage
        Inherits DBObjectBase

        Private m_SessionID As Integer
        <NonSerialized> _
        Private m_Session As CSiteSession
        Private m_UserID As Integer
        <NonSerialized> _
        Private m_User As CUser
        Private m_UserName As String
        Private m_Data As Date
        Private m_Secure As Boolean
        Private m_Protocol As String
        Private m_SiteName As String
        Private m_PageName As String
        Private m_QueryString As String
        Private m_PostedData As String
        Private m_ExecTime As Single    'Tempo in secondi che ha impiegato la pagina per essere eseguita
        Private m_StatusCode As String
        Private m_StatusDescription As String
        Private m_Referrer As String
        Private m_IDAnnuncio As String      'Campo di utilità usato per collegare la pagina ad una fonte (per le statistiche)
        'Private m_ReferrerDomain As String  'Restituisce solo la parte relativa al dominio principale del referrer

        Public Sub New()
            Me.m_SessionID = 0
            Me.m_Session = Nothing
            Me.m_UserID = 0
            Me.m_User = Nothing
            Me.m_UserName = ""
            Me.m_Data = Now
            Me.m_Secure = False
            Me.m_Protocol = ""
            Me.m_SiteName = ""
            Me.m_PageName = ""
            Me.m_QueryString = ""
            Me.m_PostedData = ""
            Me.m_Referrer = ""
            Me.m_ExecTime = 0.0
            Me.m_StatusCode = ""
            Me.m_StatusDescription = ""
            Me.m_IDAnnuncio = ""
        End Sub

        ''' <summary>
        ''' Restituisce o imposta il valore di un campo di utilità utilizzabile per collegare la visita alla pagina ad un annuncio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IDAnnuncio As String
            Get
                Return Me.m_IDAnnuncio
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_IDAnnuncio
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_IDAnnuncio = value
                Me.DoChanged("IDAnnuncio", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il codice che indica lo stato della richiesta (200 = "OK", 400, 500, ...)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatusCode As String
            Get
                Return Me.m_StatusCode
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_StatusCode
                If (oldValue = value) Then Exit Property
                Me.m_StatusCode = value
                Me.DoChanged("StatusCode", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta una stringa che descrive lo stato della richiesta in maniera pià dettagliata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatusDescription As String
            Get
                Return Me.m_StatusDescription
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_StatusDescription
                If (oldValue = value) Then Exit Property
                Me.m_StatusDescription = value
                Me.DoChanged("StatusDescription", value, oldValue)
            End Set
        End Property


        ''' <summary>
        ''' Restituisce o imposta il tempo di esecuzione della richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ExecTime As Single
            Get
                Return Me.m_ExecTime
            End Get
            Set(value As Single)
                Dim oldValue As Double = Me.m_ExecTime
                If (oldValue = value) Then Exit Property
                Me.m_ExecTime = value
                Me.DoChanged("ExecTime", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la URL completa della pagina
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Location As String
            Get
                Dim qs, ret As String
                ret = Me.PageName
                qs = Me.QueryString
                If (qs <> vbNullString) Then ret = ret & "?" & qs
                Return ret
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta un valore booleano che indica se la pagina è stata servita su connessione sicura (HTTPS)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Secure As Boolean
            Get
                Return Me.m_Secure
            End Get
            Set(value As Boolean)
                If (Me.m_Secure = value) Then Exit Property
                Me.m_Secure = value
                Me.DoChanged("Secure", value, Not value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il protocollo di accesso alla pagina (HTTP)
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
        ''' Restituisce o imposta il nome del server che ha servito la pagina (dominio principale)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SiteName As String
            Get
                Return Me.m_SiteName
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_SiteName
                If (oldValue = value) Then Exit Property
                Me.m_SiteName = value
                Me.DoChanged("SiteName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della pagina (percorso relativo)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PageName As String
            Get
                Return Me.m_PageName
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_PageName
                If (oldValue = value) Then Exit Property
                Me.m_PageName = value
                Me.DoChanged("PageName", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta le informazioni inviate tramite il metodo GET
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property QueryString As String
            Get
                Return Me.m_QueryString
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_QueryString
                If (oldValue = value) Then Exit Property
                Me.m_QueryString = value
                Me.DoChanged("QueryString", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce un singolo parametro della querystring
        ''' </summary>
        ''' <param name="name"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetQueryStringValue(ByVal name As String) As String
            Return VisitedPages.GetQueryStringValue(Me.QueryString, name)
        End Function

        ''' <summary>
        ''' Restituisce o imposta le informazioni inviate tramite il metodo POST
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PostedData As String
            Get
                Return Me.m_PostedData
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_PostedData
                If (oldValue = value) Then Exit Property
                Me.m_PostedData = value
                Me.DoChanged("PostedData", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la URL completa della pagina da cui è avvenuto l'accesso a questa pagina
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Referrer As String
            Get
                Return Me.m_Referrer
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Referrer
                If (oldValue = value) Then Exit Property
                Me.m_Referrer = value
                Me.DoChanged("Referrer", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il dominio principale del referrer
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ReferrerDomain As String
            Get
                Dim page As String = Me.m_Referrer
                If (page = "") Then Return ""

                Dim i As Integer = InStr(page, "://")
                If (i > 0) Then page = Trim(Mid(page, i + 3))

                i = InStr(page, "?")
                If (i = 1) Then page = ""
                If (i > 1) Then page = Trim(Left(page, i - 1))

                i = InStr(page, "/")
                If (i = 1) Then page = ""
                If (i > 1) Then page = Trim(Left(page, i - 1))

                i = InStrRev(page, ".")
                If (i > 0) Then
                    i = InStrRev(page, ".", i - 1)
                    If (i > 0) Then page = Trim(Mid(page, i + 1))
                End If

                Return page
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data e l'ora in cui la pagina è stata richiesta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Data As Date
            Get
                Return Me.m_Data
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_Data
                If (oldValue = value) Then Exit Property
                Me.m_Data = value
                Me.DoChanged("Data", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce l'ID della sessione (nel LOG) in cui è stata richiesta la pagina
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SessionID As Integer
            Get
                Return GetID(Me.m_Session, Me.m_SessionID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.SessionID
                If (oldValue = value) Then Exit Property
                Me.m_SessionID = value
                Me.m_Session = Nothing
                Me.DoChanged("SessionID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la sessione in cui è stata richiesta la pagina
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Session As CSiteSession
            Get
                If (Me.m_Session Is Nothing) Then Me.m_Session = Sessions.GetItemById(Me.m_SessionID)
                Return Me.m_Session
            End Get
            Set(value As CSiteSession)
                Dim oldValue As CSiteSession = Me.Session
                If (oldValue = value) Then Exit Property
                Me.m_Session = value
                Me.m_SessionID = GetID(value)
                Me.DoChanged("Session", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetSession(ByVal value As CSiteSession)
            Me.m_Session = value
            Me.m_SessionID = GetID(value)
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha richiesto la pagina
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property User As CUser
            Get
                If Me.m_User Is Nothing Then Me.m_User = Sistema.Users.GetItemById(Me.m_UserID)
                Return Me.m_User
            End Get
            Set(value As CUser)
                Dim oldValue As CUser = Me.User
                If (oldValue = value) Then Exit Property
                Me.m_User = value
                Me.m_UserID = GetID(value)
                If (value IsNot Nothing) Then Me.m_UserName = value.Nominativo
                Me.DoChanged("User", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetUser(ByVal value As CUser)
            Me.m_User = value
            Me.m_UserID = GetID(value)
            Me.m_UserName = vbNullString
            If (value IsNot Nothing) Then Me.m_UserName = value.Nominativo
        End Sub

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha richiesto la pagina
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UserID As Integer
            Get
                Return GetID(Me.m_User, Me.m_UserID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.UserID
                If (oldValue = value) Then Exit Property
                Me.m_User = Nothing
                Me.m_UserID = value
                Me.DoChanged("UserID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nominativo dell'utente che ha richiesto la pagina
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
        ''' Restituisce la data dell'ultima modifica effettuata al file. Il nome del file deve essere un percorso mappato e non una URL
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetFileLastModified() As Date
            Return FileSystem.GetLastAccessTime(Me.PageName)
        End Function

        Public Overrides Function GetModule() As CModule
            Return VisitedPages.Module
        End Function

        Public Function CountVisits() As Integer
            Return Formats.ToInteger(Me.GetConnection.ExecuteScalar("SELECT Count(*) FROM [" & Me.GetTableName & "] WHERE [ScriptName]=" & DBUtils.DBString(Me.PageName)))
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_VisitedPages"
        End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_SessionID = reader.Read("Session", Me.m_SessionID)
            Me.m_UserID = reader.Read("UserID", Me.m_UserID)
            Me.m_UserName = reader.Read("UserName", Me.m_UserName)
            Me.m_Data = reader.Read("Data", Me.m_Data)
            Me.m_Secure = reader.Read("Secure", Me.m_Secure)
            Me.m_Protocol = reader.Read("Protocol", Me.m_Protocol)
            Me.m_SiteName = reader.Read("SiteName", Me.m_SiteName)
            Me.m_PageName = reader.Read("ScriptName", Me.m_PageName)
            Me.m_QueryString = reader.Read("QueryString", Me.m_QueryString)
            Me.m_PostedData = reader.Read("PostedData", Me.m_PostedData)
            Me.m_Referrer = reader.Read("Referrer1", Me.m_Referrer)
            If (Me.m_Referrer = "") Then Me.m_Referrer = reader.Read("Referrer", Me.m_Referrer)
            Me.m_ExecTime = reader.Read("ExecTime", Me.m_ExecTime)
            Me.m_StatusDescription = reader.Read("PageStatus", Me.m_StatusDescription)
            Me.m_StatusCode = reader.Read("PageStatusCode", Me.m_StatusCode)
            Me.m_IDAnnuncio = reader.Read("IDAnnunction", Me.m_IDAnnuncio)

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("Session", Me.SessionID)
            writer.Write("UserID", Me.UserID)
            writer.Write("UserName", Me.m_UserName)
            writer.Write("Data", Me.m_Data)
            writer.Write("Secure", Me.m_Secure)
            writer.Write("Protocol", Me.m_Protocol)
            writer.Write("SiteName", Me.m_SiteName)
            writer.Write("ScriptName", Me.m_PageName)
            writer.Write("QueryString", Me.m_QueryString)
            writer.Write("PostedData", Me.m_PostedData)
            writer.Write("Referrer", Left(Me.m_Referrer, 255))
            writer.Write("Referrer1", Me.m_Referrer)
            writer.Write("ExecTime", Me.m_ExecTime)
            writer.Write("PageStatus", Me.m_StatusDescription)
            writer.Write("PageStatusCode", Me.m_StatusCode)
            writer.Write("IDAnnunction", Me.m_IDAnnuncio)
            writer.Write("ReferrerDomain", Me.ReferrerDomain)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Session", Me.SessionID)
            writer.WriteAttribute("UserID", Me.UserID)
            writer.WriteAttribute("UserName", Me.m_UserName)
            writer.WriteAttribute("Data", Me.m_Data)
            writer.WriteAttribute("Secure", Me.m_Secure)
            writer.WriteAttribute("Protocol", Me.m_Protocol)
            writer.WriteAttribute("SiteName", Me.m_SiteName)
            writer.WriteAttribute("ScriptName", Me.m_PageName)
            writer.WriteAttribute("ExecTime", Me.m_ExecTime)
            writer.WriteAttribute("Referrer", Me.m_Referrer)
            writer.WriteAttribute("PageStatus", Me.m_PageName)
            writer.WriteAttribute("PageStatusCode", Me.m_StatusCode)
            writer.WriteAttribute("IDAnnuncio", Me.m_IDAnnuncio)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("QueryString", Me.m_QueryString)
            writer.WriteTag("PostedData", Me.m_PostedData)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Session" : Me.m_SessionID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "UserID" : Me.m_UserID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "UserName" : Me.m_UserName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Data" : Me.m_Data = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Secure" : Me.m_Secure = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Protocol" : Me.m_Protocol = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "SiteName" : Me.m_SiteName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ScriptName" : Me.m_PageName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "QueryString" : Me.m_QueryString = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PostedData" : Me.m_PostedData = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Referrer" : Me.m_Referrer = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ExecTime" : Me.m_ExecTime = XML.Utils.Serializer.DeserializeFloat(fieldValue)
                Case "PageStatus" : Me.m_StatusDescription = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "PageStatusCode" : Me.m_StatusCode = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDAnnuncio" : Me.m_IDAnnuncio = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return Databases.LOGConn
        End Function

        Public Overrides Function ToString() As String
            Return Me.Location
        End Function

    End Class

End Class