Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Partial Class WebSite

    ''' <summary>
    ''' Rappresenta le informazioni registrate nel DB relativamente ad un upload
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CUploadedFile
        Inherits DBObjectBase

        Private m_Key As String
        Private m_UserID As Integer
        Private m_User As CUser
        Private m_SourceFile As String
        Private m_TargetURL As String
        Private m_UploadTime As Date
        Private m_FileSize As Integer
        Private m_FileContent As Byte()


        Public Sub New()
            Me.m_Key = ""
            Me.m_User = Users.CurrentUser
            Me.m_UserID = GetID(Me.m_User)
            Me.m_SourceFile = ""
            Me.m_TargetURL = ""
            Me.m_UploadTime = Nothing
            Me.m_FileSize = 0
            Me.m_FileContent = Nothing
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Uploads.Module
        End Function

        ''' <summary>
        ''' Restituisce o imposta la chiave che identifica univocamente l'upload
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Key As String
            Get
                Return Me.m_Key
            End Get
            Set(value As String)
                value = Strings.Trim(value)
                Dim oldValue As String = Me.m_Key
                If (oldValue = value) Then Exit Property
                Me.m_Key = value
                Me.DoChanged("Key", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'ID dell'utente che ha caricato il file
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
                Me.m_UserID = value
                Me.m_User = Nothing
                Me.DoChanged("UserID", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta l'utente che ha caricato il file
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property User As CUser
            Get
                If (Me.m_User Is Nothing) Then Me.m_User = Users.GetItemById(Me.m_UserID)
                Return Me.m_User
            End Get
            Set(value As CUser)
                If (Me.m_User Is value) Then Exit Property
                Me.m_User = value
                Me.m_UserID = GetID(value)
                Me.DoChanged("User", value)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il percorso sul PC remoto da cui è stato caricato il file
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SourceFile As String
            Get
                Return Me.m_SourceFile
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_SourceFile
                If (oldValue = value) Then Exit Property
                Me.m_SourceFile = value
                Me.DoChanged("SourceFile", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la URL da cui è possibile scaricare il file.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Questo campo è vuoto se si tratta di un Upload effettuato direttamente nel DB. In tal caso fare riferimento al campo FileContent</remarks>
        Public Property TargetURL As String
            Get
                Return Me.m_TargetURL
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_TargetURL
                If (oldValue = value) Then Exit Property
                Me.m_TargetURL = value
                Me.DoChanged("TargetURL", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta la data e l'ora del completamento dell'upload
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UploadTime As Date
            Get
                Return Me.m_UploadTime
            End Get
            Set(value As Date)
                Dim oldValue As Date = Me.m_UploadTime
                If (oldValue = value) Then Exit Property
                Me.m_UploadTime = value
                Me.DoChanged("UploadTime", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta le dimensioni in bytes del file caricato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FileSize As Integer
            Get
                Return Me.m_FileSize
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_FileSize
                If (oldValue = value) Then Exit Property
                Me.m_FileSize = value
                Me.DoChanged("FileSize", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il contenuto del file se questo è stato salvato nel database. In tal caso il camppo TargetURL è nullo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FileContent As Byte()
            Get
                Return Me.m_FileContent
            End Get
            Set(value As Byte())
                Me.m_FileContent = value
                Me.DoChanged("FileContent", value)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_TargetURL
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Uploads"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_Key = reader.Read("Key", Me.m_Key)
            Me.m_UserID = reader.Read("UserID", Me.m_UserID)
            Me.m_SourceFile = reader.Read("SourceFile", Me.m_SourceFile)
            Me.m_TargetURL = reader.Read("TargetURL", Me.m_TargetURL)
            Me.m_UploadTime = reader.Read("UploadTime", Me.m_UploadTime)
            Me.m_FileSize = reader.Read("FileSize", Me.m_FileSize)
            Me.m_FileContent = reader.Read("FileContent", Me.m_FileContent)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Key", Me.m_Key)
            writer.Write("UserID", Me.UserID)
            writer.Write("SourceFile", Me.m_SourceFile)
            writer.Write("TargetURL", Me.m_TargetURL)
            writer.Write("UploadTime", Me.m_UploadTime)
            writer.Write("FileSize", Me.m_FileSize)
            writer.Write("FileContent", Me.m_FileContent)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Key", Me.m_Key)
            writer.WriteAttribute("UserID", Me.UserID)
            writer.WriteAttribute("SourceFile", Me.m_SourceFile)
            writer.WriteAttribute("TargetURL", Me.m_TargetURL)
            writer.WriteAttribute("UploadTime", Me.m_UploadTime)
            writer.WriteAttribute("FileSize", Me.m_FileSize)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("FileContent", Me.m_FileContent)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Key" : Me.m_Key = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "UserID" : Me.m_UserID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SourceFile" : Me.m_SourceFile = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "TargetURL" : Me.m_TargetURL = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "UploadTime" : Me.m_UploadTime = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "FileSize" : Me.m_FileSize = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "FileContent" : Me.m_FileContent = fieldValue
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return APPConn
        End Function
    End Class


End Class