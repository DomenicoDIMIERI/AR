Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    Public Class CUserXGroupCursor
        Inherits DBObjectCursorBase(Of CUserXGroup)

        Private m_UserID As CCursorField(Of Integer)
        Private m_GroupID As CCursorField(Of Integer)

        Public Sub New()
            Me.m_UserID = New CCursorField(Of Integer)("User")
            Me.m_GroupID = New CCursorField(Of Integer)("Group")
        End Sub

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_UsersXGroup"
        End Function

        Public ReadOnly Property UserID As CCursorField(Of Integer)
            Get
                Return Me.m_UserID
            End Get
        End Property

        Public ReadOnly Property GroupID As CCursorField(Of Integer)
            Get
                Return Me.m_GroupID
            End Get
        End Property

    End Class

End Class
