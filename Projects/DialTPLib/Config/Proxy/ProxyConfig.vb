Imports System.IO
Imports System.Xml.Serialization
Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.XML

<Flags>
Public Enum ProxyConfigFlags As Integer
    None = 0

    Enabled = 1

    UseUpProxy = 2
End Enum

Public NotInheritable Class ProxyConfig
    Inherits DBObjectBase

    Private m_ProfileName As String
    Private m_ProxyIP As String
    Private m_ProxyPort As Integer
    Private m_Flags As ProxyConfigFlags

    Public Sub New()
        Me.m_ProfileName = ""
        Me.m_ProxyIP = ""
        Me.m_ProxyPort = 8080
        Me.m_Flags = ProxyConfigFlags.Enabled
    End Sub

    Public Property ProfileName As String
        Get
            Return Me.m_ProfileName
        End Get
        Set(value As String)
            Dim oldValue As String = Me.m_ProfileName
            value = Strings.Trim(value)
            If (oldValue = value) Then Return
            Me.m_ProfileName = value
            Me.DoChanged("ProfileName", value, oldValue)
        End Set
    End Property

    Public Property ProxyIP As String
        Get
            Return Me.m_ProxyIP
        End Get
        Set(value As String)
            Dim oldValue As String = Me.m_ProxyIP
            value = Strings.Trim(value)
            If (oldValue = value) Then Return
            Me.m_ProxyIP = value
            Me.DoChanged("ProxyIP", value, oldValue)
        End Set
    End Property

    Public Property ProxyPort As Integer
        Get
            Return Me.m_ProxyPort
        End Get
        Set(value As Integer)
            Dim oldValue As Integer = Me.m_ProxyPort
            If (oldValue = value) Then Return
            Me.m_ProxyPort = value
            Me.DoChanged("ProxyPort", value, oldValue)
        End Set
    End Property

    Public Property Flags As ProxyConfigFlags
        Get
            Return Me.m_Flags
        End Get
        Set(value As ProxyConfigFlags)
            Dim oldValue As ProxyConfigFlags = Me.m_Flags
            If (oldValue = value) Then Return
            Me.m_Flags = value
            Me.DoChanged("Flags", value, oldValue)
        End Set
    End Property



    Protected Overrides Function GetConnection() As CDBConnection
        Return DialTPApp.Database
    End Function

    Public Overrides Function GetModule() As DMD.Sistema.CModule
        Return Nothing
    End Function

    Public Overrides Function GetTableName() As String
        Return ""
    End Function

    Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
        writer.WriteAttribute("ProfileName", Me.m_ProfileName)
        writer.WriteAttribute("ProxyIP", Me.m_ProxyIP)
        writer.WriteAttribute("ProxyPort", Me.m_ProxyPort)
        writer.WriteAttribute("Flags", Me.m_Flags)
        MyBase.XMLSerialize(writer)
    End Sub

    Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
        Select Case fieldName
            Case "ProfileName" : Me.m_ProfileName = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "ProxyIP" : Me.m_ProxyIP = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "ProxyPort" : Me.m_ProxyPort = CInt(XML.Utils.Serializer.DeserializeInteger(fieldValue))
            Case "Flags" : Me.m_Flags = CType(XML.Utils.Serializer.DeserializeInteger(fieldValue), ProxyConfigFlags)
            Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
        End Select
    End Sub



End Class
