Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Partial Class WebSite

    Public Class CUserAgent
        Inherits DBObjectBase

        Private m_UserAgent As String
        Private m_DeviceType As String
        Private m_Device As String
        Private m_Bits As Integer
        Private m_SistemaOperativo As String
        Private m_VersioneSistemaOperativo As String
        Private m_Browser As String
        Private m_VersioneBrowser As String

        Public Sub New()
            Me.m_UserAgent = ""
            Me.m_DeviceType = ""
            Me.m_Device = ""
            Me.m_Bits = 0
            Me.m_SistemaOperativo = ""
            Me.m_VersioneSistemaOperativo = ""
            Me.m_Browser = ""
            Me.m_VersioneBrowser = ""
        End Sub

        Public Property UserAgent As String
            Get
                Return Me.m_UserAgent
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_UserAgent
                If (oldValue = value) Then Exit Property
                Me.m_UserAgent = value
                Me.DoChanged("UserAgent", value, oldValue)
            End Set
        End Property

        Public Property DeviceType As String
            Get
                Return Me.m_DeviceType
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_DeviceType
                If (oldValue = value) Then Exit Property
                Me.m_DeviceType = value
                Me.DoChanged("DeviceType", value, oldValue)
            End Set
        End Property

        Public Property Device As String
            Get
                Return Me.m_Device
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Device
                If (oldValue = value) Then Exit Property
                Me.m_Device = value
                Me.DoChanged("Device", value, oldValue)
            End Set
        End Property

        Public Property Bits As Integer
            Get
                Return Me.m_Bits
            End Get
            Set(value As Integer)
                Dim oldValue As Integer = Me.m_Bits
                If (oldValue = value) Then Exit Property
                Me.m_Bits = value
                Me.DoChanged("Bits", value, oldValue)
            End Set
        End Property

        Public Property SistemaOperativo As String
            Get
                Return Me.m_SistemaOperativo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_SistemaOperativo
                If (oldValue = value) Then Exit Property
                Me.m_SistemaOperativo = value
                Me.DoChanged("SistemaOperativo", value, oldValue)
            End Set
        End Property

        Public Property VersioneSistemaOperativo As String
            Get
                Return Me.m_VersioneSistemaOperativo
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_VersioneSistemaOperativo
                If (oldValue = value) Then Exit Property
                Me.m_VersioneSistemaOperativo = value
                Me.DoChanged("VersioneSistemaOperativo", value, oldValue)
            End Set
        End Property

        Public Property Browser As String
            Get
                Return Me.m_Browser
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Browser
                If (oldValue = value) Then Exit Property
                Me.m_Browser = value
                Me.DoChanged("Browser", value, oldValue)
            End Set
        End Property

        Public Property VersioneBrowser As String
            Get
                Return Me.m_VersioneBrowser
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_VersioneBrowser
                If (oldValue = value) Then Exit Property
                Me.m_VersioneBrowser = value
                Me.DoChanged("VersioneBrowser", value, oldValue)
            End Set
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return LOGConn
        End Function

        Public Overrides Function GetModule() As CModule
            Return UserAgents.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_UserAgents"
        End Function

        Public Overrides Function ToString() As String
            Return Me.m_UserAgent
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            reader.Read("UserAgent", Me.m_UserAgent)
            reader.Read("DeviceType", Me.m_DeviceType)
            reader.Read("Device", Me.m_Device)
            reader.Read("Bits", Me.m_Bits)
            reader.Read("SistemaOperativo", Me.m_SistemaOperativo)
            reader.Read("VersioneSistemaOperativo", Me.m_VersioneSistemaOperativo)
            reader.Read("Browser", Me.m_Browser)
            reader.Read("VersioneBrowser", Me.m_VersioneBrowser)
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("UserAgent", Me.m_UserAgent)
            writer.Write("Device", Me.m_Device)
            writer.Write("DeviceType", Me.m_DeviceType)
            writer.Write("Bits", Me.m_Bits)
            writer.Write("SistemaOperativo", Me.m_SistemaOperativo)
            writer.Write("VersioneSistemaOperativo", Me.m_VersioneSistemaOperativo)
            writer.Write("Browser", Me.m_Browser)
            writer.Write("VersioneBrowser", Me.m_VersioneBrowser)
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("UserAgent", Me.m_UserAgent)
            writer.WriteTag("Device", Me.m_Device)
            writer.WriteTag("DeviceType", Me.m_DeviceType)
            writer.WriteTag("Bits", Me.m_Bits)
            writer.WriteTag("SistemaOperativo", Me.m_SistemaOperativo)
            writer.WriteTag("VersioneSistemaOperativo", Me.m_VersioneSistemaOperativo)
            writer.WriteTag("Browser", Me.m_Browser)
            writer.WriteTag("VersioneBrowser", Me.m_VersioneBrowser)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "UserAgent" : Me.m_UserAgent = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Device" : Me.m_Device = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "DeviceType" : Me.m_DeviceType = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Bits" : Me.m_Bits = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "SistemaOperativo" : Me.m_SistemaOperativo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "VersioneSistemaOperativo" : Me.m_VersioneSistemaOperativo = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Browser" : Me.m_Browser = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "VersioneBrowser" : Me.m_VersioneBrowser = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub


    End Class


End Class
