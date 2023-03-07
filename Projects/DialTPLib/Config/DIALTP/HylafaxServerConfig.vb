Imports System.IO
Imports System.Xml.Serialization
Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.XML

Public NotInheritable Class HylafaxServerConfig
    Inherits DBObjectBase

    Private m_HylafaxServer As String
    Private m_HylafaxPort As Integer
    Private m_HylafaxUserName As String
    Private m_HylafaxPassword As String
    Private m_HylafaxDialPrefix As String
    Private m_HylafaxUploadTo As String

    Public Sub New()
        Me.m_HylafaxServer = "localhost"
        Me.m_HylafaxPort = 4559
        Me.m_HylafaxUserName = "root"
        Me.m_HylafaxPassword = ""
        Me.m_HylafaxDialPrefix = "9"
        Me.m_HylafaxUploadTo = ""
    End Sub

    Public Property HylafaxServer As String
        Get
            Return Me.m_HylafaxServer
        End Get
        Set(value As String)
            Dim oldValue As String = Me.m_HylafaxServer
            value = Strings.Trim(value)
            If (oldValue = value) Then Return
            Me.m_HylafaxServer = value
            Me.DoChanged("HylafaxServer", value, oldValue)
        End Set
    End Property

    Public Property HylafaxPort As Integer
        Get
            Return Me.m_HylafaxPort
        End Get
        Set(value As Integer)
            Dim oldValue As Integer = Me.m_HylafaxPort
            If (oldValue = value) Then Return
            Me.m_HylafaxPort = value
            Me.DoChanged("HylafaxPort", value, oldValue)
        End Set
    End Property

    Public Property HylafaxUserName As String
        Get
            Return Me.m_HylafaxUserName
        End Get
        Set(value As String)
            Dim oldValue As String = Me.m_HylafaxUserName
            value = Strings.Trim(value)
            If (oldValue = value) Then Return
            Me.m_HylafaxUserName = value
            Me.DoChanged("HylafaxUserName", value, oldValue)
        End Set
    End Property

    Public Property HylafaxPassword As String
        Get
            Return Me.m_HylafaxPassword
        End Get
        Set(value As String)
            Dim oldValue As String = Me.m_HylafaxPassword
            If (oldValue = value) Then Return
            Me.m_HylafaxPassword = value
            Me.DoChanged("HylafaxPassword", value, oldValue)
        End Set
    End Property

    Public Property HylafaxDialPrefix As String
        Get
            Return Me.m_HylafaxDialPrefix
        End Get
        Set(value As String)
            Dim oldValue As String = Me.m_HylafaxDialPrefix
            value = Strings.Trim(value)
            If (oldValue = value) Then Return
            Me.m_HylafaxDialPrefix = value
            Me.DoChanged("HylafaxDialPrefix", value, oldValue)
        End Set
    End Property

    Public Property HylafaxUploadTo As String
        Get
            Return Me.m_HylafaxUploadTo
        End Get
        Set(value As String)
            Dim oldValue As String = Me.m_HylafaxUploadTo
            value = Strings.Trim(value)
            If (oldValue = value) Then Return
            Me.m_HylafaxUploadTo = value
            Me.DoChanged("HylafaxUploadTo", value, oldValue)
        End Set
    End Property

    Protected Overrides Function GetConnection() As CDBConnection
        Return APPConn
    End Function

    Public Overrides Function GetModule() As DMD.Sistema.CModule
        Return Nothing
    End Function

    Public Overrides Function GetTableName() As String
        Return ""
    End Function

    Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
        writer.WriteAttribute("HylafaxServer", Me.m_HylafaxServer)
        writer.WriteAttribute("HylafaxPort", Me.m_HylafaxPort)
        writer.WriteAttribute("HylafaxUserName", Me.m_HylafaxUserName)
        writer.WriteAttribute("HylafaxPassword", Me.m_HylafaxPassword)
        writer.WriteAttribute("HylafaxDialPrefix", Me.m_HylafaxDialPrefix)
        writer.WriteAttribute("HylafaxUploadTo", Me.m_HylafaxUploadTo)
        MyBase.XMLSerialize(writer)
    End Sub

    Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
        Select Case fieldName
            Case "HylafaxServer" : Me.m_HylafaxServer = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "HylafaxPort" : Me.m_HylafaxPort = CInt(XML.Utils.Serializer.DeserializeInteger(fieldValue))
            Case "HylafaxUserName" : Me.m_HylafaxUserName = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "HylafaxPassword" : Me.m_HylafaxPassword = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "HylafaxDialPrefix" : Me.m_HylafaxDialPrefix = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "HylafaxUploadTo" : Me.m_HylafaxUploadTo = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
        End Select
    End Sub



End Class
