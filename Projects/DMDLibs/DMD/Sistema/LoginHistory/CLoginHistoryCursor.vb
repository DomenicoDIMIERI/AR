Imports DMD.Databases

Partial Public Class Sistema

    ''' <summary>
    ''' Cursore sulla tabella dei log di accesso
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CLoginHistoryCursor
        Inherits DBObjectCursorBase(Of CLoginHistory)

        Private m_UserID As CCursorField(Of Integer)
        Private m_LoginTime As CCursorField(Of Date)
        Private m_LogoutTime As CCursorField(Of Date)
        Private m_LogoutMethod As CCursorField(Of LogOutMethods)
        Private m_RemoteIP As CCursorFieldObj(Of String)
        Private m_RemotePort As CCursorField(Of Integer)
        Private m_UserAgent As CCursorFieldObj(Of String)

        Public Sub New()
            Me.m_UserID = New CCursorField(Of Integer)("User")
            Me.m_LoginTime = New CCursorField(Of Date)("LogInTime")
            Me.m_LogoutTime = New CCursorField(Of Date)("LogOutTime")
            Me.m_LogoutMethod = New CCursorField(Of LogOutMethods)("LogoutMethod")
            Me.m_RemoteIP = New CCursorFieldObj(Of String)("RemoteIP")
            Me.m_RemotePort = New CCursorField(Of Integer)("RemotePort")
            Me.m_UserAgent = New CCursorFieldObj(Of String)("UserAgent")
        End Sub



        Public ReadOnly Property UserID As CCursorField(Of Integer)
            Get
                Return Me.m_UserID
            End Get
        End Property

        Public ReadOnly Property LoginTime As CCursorField(Of Date)
            Get
                Return Me.m_LoginTime
            End Get
        End Property

        Public ReadOnly Property LogoutTime As CCursorField(Of Date)
            Get
                Return Me.m_LogoutTime
            End Get
        End Property

        Public ReadOnly Property LogoutMethod As CCursorField(Of LogOutMethods)
            Get
                Return Me.m_LogoutMethod
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

        Public Overrides Function GetTableName() As String
            Return "tbl_LoginHistory"
        End Function

        'Protected Overrides Function GetCursorFields() As CCursorFieldsCollection
        '    Dim col As CCursorFieldsCollection
        '    col = MyBase.GetCursorFields
        '    col.Add(m_UserID)
        '    col.Add(m_LoginTime)
        '    col.Add(m_LogoutTime)
        '    col.Add(m_LogoutMethod)
        '    col.Add(m_RemoteIP)
        '    col.Add(m_RemotePort)
        '    col.Add(m_UserAgent)
        '    Return col
        'End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CLoginHistory
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return LOGConn
        End Function
    End Class

End Class
