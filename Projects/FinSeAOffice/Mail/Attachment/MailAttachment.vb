Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.Net.Mime
Imports DMD.Net.Mime
Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports System.IO

Partial Class Office

    <Serializable>
    Public Class MailAttachment
        Inherits DBObject
        Implements ICloneable

        Private m_Application As MailApplication
        Private m_MessageID As Integer
        Private m_Message As MailMessage
        Private m_Name As String
        Private m_FileName As String
        Private m_ContentID As String
        Private m_ContentType As String
        Private m_ContentDisposition As String
        Private m_FileSize As Integer
        Private m_FileCreationTime As Date?
        Private m_FileLastEditTime As Date?
        Private m_FileLastReadTime As Date?
        Private m_Parameters As CKeyCollection

        Public Sub New()
            Me.m_Application = Nothing
            Me.m_MessageID = 0
            Me.m_Message = Nothing
            Me.m_Name = ""
            Me.m_FileName = ""
            Me.m_ContentID = ""
            Me.m_ContentType = ""
            Me.m_ContentDisposition = ""
            Me.m_FileSize = 0
            Me.m_FileCreationTime = Nothing
            Me.m_FileLastEditTime = Nothing
            Me.m_FileLastReadTime = Nothing
            Me.m_Parameters = Nothing
        End Sub

        Public Sub New(ByVal fileName As String)
            Me.New
            Dim att As New DMD.Net.Mail.AttachmentEx(fileName)
            Me.From(att)
        End Sub

        Public ReadOnly Property Application As MailApplication
            Get
                Return Me.m_Application
            End Get
        End Property

        Protected Friend Sub SetApplication(ByVal value As MailApplication)
            Me.m_Application = value
        End Sub

        Public Property FileCreationTime As Date?
            Get
                Return Me.m_FileCreationTime
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_FileCreationTime
                If (Calendar.Compare(value, oldValue) = 0) Then Return
                Me.m_FileCreationTime = value
                Me.DoChanged("FileCreationTime", value, oldValue)
            End Set
        End Property

        Public Property FileLastEditTime As Date?
            Get
                Return Me.m_FileLastEditTime
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_FileLastEditTime
                If (Calendar.Compare(value, oldValue) = 0) Then Return
                Me.m_FileLastEditTime = value
                Me.DoChanged("FileLastEditTime", value, oldValue)
            End Set
        End Property

        Public Property FileLastReadTime As Date?
            Get
                Return Me.m_FileLastReadTime
            End Get
            Set(value As Date?)
                Dim oldValue As Date? = Me.m_FileLastReadTime
                If (Calendar.Compare(value, oldValue) = 0) Then Return
                Me.m_FileLastReadTime = value
                Me.DoChanged("FileLastReadTime", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Parameters As CKeyCollection
            Get
                If (Me.m_Parameters Is Nothing) Then Me.m_Parameters = New CKeyCollection
                Return Me.m_Parameters
            End Get
        End Property

        Public Property FileSize As Integer
            Get
                Return Me.m_FileSize
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_FileSize
                If (oldValue = value) Then Return
                Me.m_FileSize = value
                Me.DoChanged("FileSize", value, oldValue)
            End Set
        End Property

        Public Property FileName As String
            Get
                Return Me.m_FileName
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_FileName
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_FileName = value
                Me.DoChanged("FileName", value, oldValue)
            End Set
        End Property

        Public Property ContentDisposition As String
            Get
                Return Me.m_ContentDisposition
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_ContentDisposition
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_ContentDisposition = value
                Me.DoChanged("ContentDisposition", value, oldValue)
            End Set
        End Property

        Public Property ContentId As String
            Get
                Return Me.m_ContentID
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_ContentID
                value = Strings.Trim(value)
                If (oldValue = value) Then Exit Property
                Me.m_ContentID = value
                Me.DoChanged("ContentId", value, oldValue)
            End Set
        End Property


        Public Property ContentType As String
            Get
                Return Me.m_ContentType
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_ContentType
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_ContentType = value
                Me.DoChanged("ContentType", value, oldValue)
            End Set
        End Property

        Public Property Name As String
            Get
                Return Me.m_Name
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Name
                value = Strings.Trim(value)
                If (value = oldValue) Then Exit Property
                Me.m_Name = value
                Me.DoChanged("Name", value, oldValue)
            End Set
        End Property

        Public Property MessageID As Integer
            Get
                Return GetID(Me.m_Message, Me.m_MessageID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.MessageID
                If (oldValue = value) Then Return
                Me.m_MessageID = value
                Me.m_Message = Nothing
                Me.DoChanged("MessageID", value, oldValue)
            End Set
        End Property

        Public Property Message As MailMessage
            Get
                If (Me.m_Message Is Nothing) Then Me.m_Message = Office.Mails.GetItemById(Me.m_MessageID)
                Return Me.m_Message
            End Get
            Friend Set(value As MailMessage)
                Dim oldValue As MailMessage = Me.Message
                If (oldValue Is value) Then Return
                Me.m_Message = value
                Me.m_MessageID = GetID(value)
                Me.DoChanged("Message", value, oldValue)
            End Set
        End Property

        Protected Friend Overridable Sub SetMessage(ByVal message As MailMessage)
            Me.m_Message = message
            Me.m_MessageID = GetID(message)
        End Sub

        Public Overrides Function ToString() As String
            Return Me.m_FileName
        End Function



        'Protected Sub SaveContentTo(ByVal fileName As String)
        '    Dim stream As New System.IO.FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None)
        '    Dim buffer() As Byte
        '    ReDim buffer(1024 - 1)
        '    Me.ContentStream.Position = 0
        '    While Me.ContentStream.Position < Me.ContentStream.Length
        '        Me.ContentStream.Read(buffer, 0, 1024)
        '        stream.Write(buffer, 0, 1024)
        '        'Me.ContentStream.Position += 1024
        '    End While
        '    stream.Dispose()
        'End Sub

        'Protected Sub LoadContentFrom(ByVal fileName As String, ByVal name As String, ByVal mediaType As String)
        '    Dim stream As New System.IO.FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)
        '    Dim outStream As New System.IO.MemoryStream(1024)
        '    Dim buffer() As Byte
        '    ReDim buffer(1024 - 1)
        '    While stream.Position < stream.Length
        '        stream.Read(buffer, 0, 1024)
        '        outStream.Write(buffer, 0, 1024)
        '        stream.Position += 1024
        '    End While
        '    stream.Dispose()
        '    Me.BaseObject = New System.Net.Mail.Attachment(outStream, name, mediaType)
        '    outStream.Dispose()
        'End Sub


        '' This code added by Visual Basic to correctly implement the disposable pattern.
        'Public Overridable Sub Dispose() Implements IDisposable.Dispose
        '    If (Me.BaseObject IsNot Nothing) Then Me.BaseObject.Dispose() : Me.BaseObject = Nothing
        '    Me.m_Message = Nothing
        '    Me.m_Name = vbNullString
        '    Me.m_FileName = vbNullString
        '    Me.m_ContentID = vbNullString
        '    Me.m_NameEncoding = Nothing
        '    Me.m_TransferEncoding = Nothing
        'End Sub


        'Private Function GetStr(ByVal enc As System.Text.Encoding) As String
        '    If (enc Is Nothing) Then Return ""
        '    Return enc.WebName
        'End Function

        'Private Function GetEncoding(ByVal str As String) As System.Text.Encoding
        '    str = Trim(str)
        '    If (str = "") Then Return Nothing
        '    Return System.Text.Encoding.GetEncoding(str)
        'End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_eMailAttachments"
        End Function

        'Private Function parseTransferEncoding(ByVal str As String) As System.Net.Mime.TransferEncoding
        '    Try
        '        Return [Enum].Parse(GetType(System.Net.Mime.TransferEncoding), str)
        '    Catch ex As Exception
        '        Return System.Net.Mime.TransferEncoding.Unknown
        '    End Try
        'End Function

        'Private Function formatTransferEncoding(ByVal value As System.Net.Mime.TransferEncoding) As String
        '    Try
        '        Return [Enum].GetName(GetType(System.Net.Mime.TransferEncoding), value)
        '    Catch ex As Exception
        '        Return ""
        '    End Try
        'End Function

        Protected Overrides Function LoadFromRecordset(ByVal reader As DBReader) As Boolean
            Me.m_MessageID = reader.Read("MessageID", Me.m_MessageID)
            Me.m_Name = reader.Read("Name", Me.m_Name)
            Me.m_FileName = reader.Read("FileName", Me.m_FileName)
            Me.m_ContentID = reader.Read("ContentId", Me.m_ContentID)
            Me.m_ContentType = reader.Read("ContentType", Me.m_ContentType)
            Me.m_ContentDisposition = reader.Read("ContentDisposition", Me.m_ContentDisposition)
            Me.m_FileSize = reader.Read("FileSize", Me.m_FileSize)
            Me.m_FileCreationTime = reader.Read("FileCreationTime", Me.m_FileCreationTime)
            Me.m_FileLastEditTime = reader.Read("FileLastEditTime", Me.m_FileLastEditTime)
            Me.m_FileLastReadTime = reader.Read("FileLastReadTime", Me.m_FileLastReadTime)
            Try
                Me.m_Parameters = XML.Utils.Serializer.Deserialize(reader.Read("Params", ""))
            Catch ex As Exception
                Me.m_Parameters = Nothing
            End Try


            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(ByVal writer As DBWriter) As Boolean
            writer.Write("MessageID", Me.MessageID)
            writer.Write("Name", Me.m_Name)
            writer.Write("FileName", Me.m_FileName)
            writer.Write("ContentId", Me.m_ContentID)
            writer.Write("ContentType", Me.m_ContentType)
            writer.Write("ContentDisposition", Me.m_ContentDisposition)
            writer.Write("FileSize", Me.m_FileSize)
            writer.Write("FileCreationTime", Me.m_FileCreationTime)
            writer.Write("FileLastEditTime", Me.m_FileLastEditTime)
            writer.Write("FileLastReadTime", Me.m_FileLastReadTime)
            writer.Write("Params", XML.Utils.Serializer.Serialize(Me.Parameters))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("MessageID", Me.MessageID)
            writer.WriteAttribute("Name", Me.m_Name)
            writer.WriteAttribute("FileName", Me.m_FileName)
            writer.WriteAttribute("ContentId", Me.m_ContentID)
            writer.WriteAttribute("ContentType", Me.m_ContentType)
            writer.WriteAttribute("ContentDisposition", Me.m_ContentDisposition)
            writer.WriteAttribute("FileSize", Me.m_FileSize)
            writer.WriteAttribute("FileCreationTime", Me.m_FileCreationTime)
            writer.WriteAttribute("FileLastEditTime", Me.m_FileLastEditTime)
            writer.WriteAttribute("FileLastReadTime", Me.m_FileLastReadTime)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Params", Me.Parameters)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "MessageID" : Me.MessageID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Name" : Me.m_Name = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "FileName" : Me.m_FileName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ContentId" : Me.m_ContentID = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ContentType" : Me.m_ContentType = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "ContentDisposition" : Me.m_ContentDisposition = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "FileSize" : Me.m_FileSize = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "FileCreationTime" : Me.m_FileCreationTime = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "FileLastEditTime" : Me.m_FileLastEditTime = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "FileLastReadTime" : Me.m_FileLastReadTime = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "Params" : Me.m_Parameters = fieldValue
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function GetModule() As Sistema.CModule
            Return Office.Mails.Attachments.Module
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Mails.Database
        End Function

        Private Function GetFreeFile() As String
            SyncLock Sistema.ApplicationContext
                Me.m_FileName = "mail\attachments\malatt" & ASPSecurity.GetRandomKey(16) & ".dat"
                Dim fileName As String = System.IO.Path.Combine(Sistema.ApplicationContext.SystemDataFolder, Me.m_FileName)
                While System.IO.File.Exists(fileName)
                    Me.m_FileName = "mail\attachments\malatt" & ASPSecurity.GetRandomKey(16) & ".dat"
                End While
                System.IO.File.WriteAllText(fileName, "")
                Return Me.m_FileName
            End SyncLock
        End Function

        Friend Sub From(ByVal at As System.Net.Mail.Attachment)
            Dim stream As System.IO.FileStream = Nothing
#If Not DEBUG Then
            Try
#End If
            If (at.ContentDisposition IsNot Nothing) Then
                Me.m_Name = at.ContentDisposition.FileName
                Me.m_FileSize = at.ContentDisposition.Size
                Me.m_ContentDisposition = at.ContentDisposition.DispositionType
            End If
            If (Me.m_Name = "") Then Me.m_Name = at.Name
            Me.m_ContentID = at.ContentId
            Me.m_ContentType = at.ContentType.ToString

            DMD.Sistema.FileSystem.CreateRecursiveFolder(System.IO.Path.Combine(Sistema.ApplicationContext.SystemDataFolder, "mail\attachments"))
            If (Me.m_FileName = "") Then Me.m_FileName = Me.GetFreeFile

            Dim fileName As String = System.IO.Path.Combine(Sistema.ApplicationContext.SystemDataFolder, Me.m_FileName)

            stream = New System.IO.FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None)
            at.ContentStream.Position = 0
            DMD.Sistema.FileSystem.CopyStream(at.ContentStream, stream)
#If Not DEBUG Then
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
#End If
            If (stream IsNot Nothing) Then stream.Dispose() : stream = Nothing
#If Not DEBUG Then
            End Try
#End If
        End Sub

        Public Function Clone() As Object Implements ICloneable.Clone
            Return Me.MemberwiseClone
        End Function

        '#Region "Static Methods"

        '        Public Shared Function CreateAttachmentFromString(content As String, name As String) As MailAttachment
        '            Dim tmp As System.Net.Mail.Attachment = System.Net.Mail.Attachment.CreateAttachmentFromString(content, name)
        '            Return New MailAttachment(tmp)
        '        End Function

        '        Public Shared Function CreateAttachmentFromString(content As String, name As String, contentEncoding As System.Text.Encoding, mediaType As String) As MailAttachment
        '            Dim tmp As System.Net.Mail.Attachment = System.Net.Mail.Attachment.CreateAttachmentFromString(content, name, contentEncoding, mediaType)
        '            Return New MailAttachment(tmp)
        '        End Function

        '        Public Shared Function CreateAttachmentFromString(content As String, contentType As System.Net.Mime.ContentType) As MailAttachment
        '            Dim tmp As System.Net.Mail.Attachment = System.Net.Mail.Attachment.CreateAttachmentFromString(content, contentType)
        '            Return New MailAttachment(tmp)
        '        End Function

        '        Protected Overrides Sub Finalize()
        '            MyBase.Finalize()
        '            ' DMD.DMDObject.DecreaseCounter(Me)
        '        End Sub

        '#End Region
    End Class

End Class