Imports DMD
Imports DMD.Databases
Imports DMD.Sistema

Partial Class WebSite

    Public Class CSiteSessionsCursor
        Inherits DBObjectCursorBase(Of CSiteSession)

        Private m_SessionID As New CCursorFieldObj(Of String)("SessionID")
        Private m_StartTime As New CCursorField(Of Date)("StartTime")
        Private m_EndTime As New CCursorField(Of Date)("EndTime")
        Private m_RemoteIP As New CCursorFieldObj(Of String)("RemoteIP")
        Private m_RemotePort As New CCursorField(Of Integer)("RemotePort")
        Private m_UserAgent As New CCursorFieldObj(Of String)("UserAgent")
        Private m_Cookie As New CCursorFieldObj(Of String)("Cookie")
        Private m_InitialReferrer As New CCursorFieldObj(Of String)("InitialReferrer")

        Public Sub New()
        End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return Databases.LOGConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Sessions.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_SiteSessions"
        End Function

        Public ReadOnly Property SessionID As CCursorFieldObj(Of String)
            Get
                Return Me.m_SessionID
            End Get
        End Property

        Public ReadOnly Property StartTime As CCursorField(Of Date)
            Get
                Return Me.m_StartTime
            End Get
        End Property

        Public ReadOnly Property EndTime As CCursorField(Of Date)
            Get
                Return Me.m_EndTime
            End Get
        End Property

        Public ReadOnly Property RemoteIP As CCursorFieldObj(Of String)
            Get
                Return Me.m_RemoteIP
            End Get
        End Property

        Public ReadOnly Property RemotePort As CCursorField(Of Integer)
            Get
                Return Me.m_RemotePort
            End Get
        End Property

        Public ReadOnly Property UserAgent As CCursorFieldObj(Of String)
            Get
                Return Me.m_UserAgent
            End Get
        End Property

        Public ReadOnly Property Cookie As CCursorFieldObj(Of String)
            Get
                Return Me.m_Cookie
            End Get
        End Property

        Public ReadOnly Property InitialReferrer As CCursorFieldObj(Of String)
            Get
                Return Me.m_InitialReferrer
            End Get
        End Property

    End Class


End Class
