Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    ''' <summary>
    ''' Cursore sulla tabella delle autorizzazione utente
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CUserAuthorizationCursor
        Inherits DBObjectCursorBase(Of CUserAuthorization)

        Private m_ActionID As New CCursorField(Of Integer)("Action")
        Private m_UserID As New CCursorField(Of Integer)("User")
        Private m_Allow As New CCursorField(Of Boolean)("Allow")
        Private m_Negate As New CCursorField(Of Boolean)("Negate")

        Public Sub New()
        End Sub



        Public ReadOnly Property ActionID As CCursorField(Of Integer)
            Get
                Return Me.m_ActionID
            End Get
        End Property

        Public ReadOnly Property UserID As CCursorField(Of Integer)
            Get
                Return Me.m_UserID
            End Get
        End Property

        Public ReadOnly Property Allow As CCursorField(Of Boolean)
            Get
                Return Me.m_Allow
            End Get
        End Property

        Public ReadOnly Property Negate As CCursorField(Of Boolean)
            Get
                Return Me.m_Negate
            End Get
        End Property

        Public Overrides Function GetTableName() As String
            Return "tbl_UserAuthorizations"
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function
    End Class


End Class