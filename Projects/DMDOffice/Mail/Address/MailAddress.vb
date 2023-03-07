Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Office
Imports DMD.Internals
Imports DMD.XML

Partial Class Office

    <Serializable>
    Public Class MailAddress
        Inherits DBObjectBase

        <NonSerialized> Private m_Application As MailApplication
        Private m_MessageID As Integer
        <NonSerialized> Private m_Message As MailMessage
        Private m_FieldName As String
        Private m_Address As String
        Private m_DisplayName As String

        Public Sub New()
            Me.m_Application = Nothing
            Me.m_MessageID = 0
            Me.m_Message = Nothing
            Me.m_FieldName = ""
            Me.m_Address = ""
            Me.m_DisplayName = ""
        End Sub

        Public Sub New(ByVal address As String, ByVal displayName As String)
            Me.New
            Me.m_Address = Strings.Trim(address)
            Me.m_DisplayName = Strings.Trim(displayName)
        End Sub


        Public Sub New(ByVal msg As MailMessage, ByVal fieldName As String)
            If (msg Is Nothing) Then Throw New ArgumentNullException("msg")
            fieldName = LCase(Trim(fieldName))
            If (fieldName = "") Then Throw New ArgumentNullException("fieldName")
            Me.m_Application = Nothing
            Me.m_MessageID = GetID(msg)
            Me.m_Message = msg
            Me.m_FieldName = fieldName
            Me.m_Address = ""
            Me.m_DisplayName = ""
        End Sub

        Public ReadOnly Property Application As MailApplication
            Get
                Return Me.m_Application
            End Get
        End Property

        Protected Friend Sub SetApplication(ByVal value As MailApplication)
            Me.m_Application = value
        End Sub

        Public Property MessageID As Integer
            Get
                Return GetID(Me.m_Message, Me.m_MessageID)
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.MessageID
                If (oldValue = value) Then Return
                Me.m_Message = Nothing
                Me.m_MessageID = value
                Me.DoChanged("MessageID", value, oldValue)
            End Set
        End Property


        Public Property Message As MailMessage
            Get
                If (Me.m_Message Is Nothing) Then Me.m_Message = Me.Application.GetMessageById(Me.m_MessageID)
                Return Me.m_Message
            End Get
            Set(value As MailMessage)
                Dim oldValue As MailMessage = Me.m_Message
                If (oldValue Is value) Then Return
                Me.m_Message = value
                Me.m_MessageID = GetID(value)
                Me.DoChanged("Message", value, oldValue)
            End Set
        End Property

        Protected Friend Sub SetMessage(ByVal value As MailMessage)
            Me.m_Message = value
            Me.m_MessageID = GetID(value)
        End Sub

        Public Property FieldName As String
            Get
                Return Me.m_FieldName
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_FieldName
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_FieldName = value
                Me.DoChanged("FieldName", value, oldValue)
            End Set
        End Property

        Public Property Address As String
            Get
                Return Me.m_Address
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_Address
                value = Formats.ParseEMailAddress(value)
                If (oldValue = value) Then Return
                Me.m_Address = value
                Me.DoChanged("Address", value, oldValue)
            End Set
        End Property

        Public Property DisplayName As String
            Get
                Return Me.m_DisplayName
            End Get
            Set(value As String)
                Dim oldValue As String = Me.m_DisplayName
                value = Strings.Trim(value)
                If (oldValue = value) Then Return
                Me.m_DisplayName = value
                Me.DoChanged("DisplayName", value, oldValue)
            End Set
        End Property

        Public ReadOnly Property Host As String
            Get
                Dim str As String = Me.Address
                Dim p As Integer = str.LastIndexOf("@")
                If (p > 0) Then Return str.Substring(p)
                Return vbNullString
            End Get
        End Property

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return DMD.Office.Mails.Database
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_MailAddresses"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.m_MessageID = reader.Read("MessageID", Me.m_MessageID)
            Me.m_FieldName = reader.Read("FieldName", Me.m_FieldName)
            Me.m_Address = reader.Read("Address", Me.m_Address)
            Me.m_DisplayName = reader.Read("DisplayName", Me.m_DisplayName)

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean

            writer.Write("MessageID", Me.MessageID)
            writer.Write("FieldName", Me.m_FieldName)
            writer.Write("Address", Me.m_Address)
            writer.Write("DisplayName", Me.m_DisplayName)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XMLWriter)
            writer.WriteAttribute("MessageID", Me.MessageID)
            writer.WriteAttribute("FieldName", Me.m_FieldName)
            writer.WriteAttribute("Address", Me.m_Address)
            writer.WriteAttribute("DisplayName", Me.m_DisplayName)
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "MessageID" : Me.m_MessageID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "FieldName" : Me.m_FieldName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Address" : Me.m_Address = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DisplayName" : Me.m_DisplayName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function ToString() As String
            Dim ret As New System.Text.StringBuilder
            If (Me.m_DisplayName <> vbNullString) Then
                ret.Append(Me.m_DisplayName)
                ret.Append(" <")
                ret.Append(Me.m_Address)
                ret.Append(">")
            Else
                ret.Append(Me.m_Address)
            End If
            Return ret.ToString
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            If (Not TypeOf (obj) Is MailAddress) OrElse (obj Is Nothing) Then Return False
            With DirectCast(obj, MailAddress)
                Return Strings.Compare(Me.m_FieldName, .m_FieldName, CompareMethod.Text) = 0 AndAlso
                       Strings.Compare(Me.m_Address, .m_Address, CompareMethod.Text) = 0 AndAlso
                       Strings.Compare(Me.m_DisplayName, .m_DisplayName, CompareMethod.Text) = 0
            End With
        End Function


    End Class

End Class
