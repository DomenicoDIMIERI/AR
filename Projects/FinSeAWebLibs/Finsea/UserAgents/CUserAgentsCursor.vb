Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
 
Partial Class WebSite
 
    Public Class CUserAgentsCursor
        Inherits DBObjectCursorBase(Of CUserAgent)

        Private m_UserAgent As New CCursorFieldObj(Of String)("UserAgent")
        Private m_Device As New CCursorFieldObj(Of String)("Device")
        Private m_DeviceType As New CCursorFieldObj(Of String)("DeviceType")
        Private m_Bits As New CCursorField(Of Integer)("Bits")
        Private m_SistemaOperativo As New CCursorFieldObj(Of String)("SistemaOperativo")
        Private m_VersioneSistemaOperativo As New CCursorFieldObj(Of String)("VersioneSistemaOperativo")
        Private m_Browser As New CCursorFieldObj(Of String)("Browser")
        Private m_VersioneBrowser As New CCursorFieldObj(Of String)("VersioneBrowser")

        Public Sub New()
        End Sub

        Public ReadOnly Property UserAgent As CCursorFieldObj(Of String)
            Get
                Return Me.m_UserAgent
            End Get
        End Property

        Public ReadOnly Property Device As CCursorFieldObj(Of String)
            Get
                Return Me.m_Device
            End Get
        End Property

        Public ReadOnly Property DeviceType As CCursorFieldObj(Of String)
            Get
                Return Me.m_DeviceType
            End Get
        End Property

        Public ReadOnly Property Bits As CCursorField(Of Integer)
            Get
                Return Me.m_Bits
            End Get
        End Property

        Public ReadOnly Property SistemaOperativo As CCursorFieldObj(Of String)
            Get
                Return Me.m_SistemaOperativo
            End Get
        End Property

        Public ReadOnly Property VersioneSistemaOperativo As CCursorFieldObj(Of String)
            Get
                Return Me.m_VersioneSistemaOperativo
            End Get
        End Property

        Public ReadOnly Property Browser As CCursorFieldObj(Of String)
            Get
                Return Me.m_Browser
            End Get
        End Property

        Public ReadOnly Property VersioneBrowser As CCursorFieldObj(Of String)
            Get
                Return Me.m_VersioneBrowser
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return LOGConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return UserAgents.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_UserAgents"
        End Function
    End Class


End Class
