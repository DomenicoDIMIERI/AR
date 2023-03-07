Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class CQSPD

    Public Class CAllowedRemoteIPsCursor
        Inherits DBObjectCursor(Of CAllowedRemoteIPs)

        Private m_Name As CCursorFieldObj(Of String)
        Private m_RemoteIP As CCursorFieldObj(Of String)
        Private m_Negate As CCursorField(Of Boolean)

        Public Sub New()
            Me.m_Name = New CCursorFieldObj(Of String)("Name")
            Me.m_RemoteIP = New CCursorFieldObj(Of String)("RemoteIP")
            Me.m_Negate = New CCursorField(Of Boolean)("Negate")
        End Sub

        Public ReadOnly Property Name As CCursorFieldObj(Of String)
            Get
                Return Me.m_Name
            End Get
        End Property

        Public ReadOnly Property RemoteIP As CCursorFieldObj(Of String)
            Get
                Return Me.m_RemoteIP
            End Get
        End Property

        Public ReadOnly Property Negate As CCursorField(Of Boolean)
            Get
                Return Me.m_Negate
            End Get
        End Property

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CAllowedRemoteIPs
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_Rapportini_Allow"
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function
    End Class


End Class
