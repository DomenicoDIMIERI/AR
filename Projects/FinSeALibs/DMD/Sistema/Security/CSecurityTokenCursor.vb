Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases

Partial Public Class Sistema

    Public NotInheritable Class CSecurityTokenCursor
        Inherits DBObjectCursorBase(Of CSecurityToken)

        Private m_TokenID As New CCursorFieldObj(Of String)("Token")
        Private m_TokenName As New CCursorFieldObj(Of String)("TokenName")
        Private m_Valore As New CCursorFieldObj(Of String)("Valore")
        Private m_Session As New CCursorFieldObj(Of String)("Session")
        Private m_CreatoDaID As New CCursorField(Of Integer)("CreatoDa")
        Private m_CreatoIl As New CCursorField(Of Date)("CreatoIl")
        Private m_UsatoDaID As New CCursorField(Of Integer)("UsatoDa")
        Private m_UsatoIl As New CCursorField(Of Date)("UsatoIl")
        Private m_Dettaglio As New CCursorFieldObj(Of String)("Dettaglio")
        Private m_ExpireTime As New CCursorField(Of Date)("ExpireTime")
        Private m_ExpireCount As New CCursorField(Of Integer)("ExpireCount")
        Private m_UseCount As New CCursorField(Of Integer)("UseCount")
        Private m_TokenSourceName As New CCursorFieldObj(Of String)("TokenSourceName")
        Private m_TokenSourceID As New CCursorField(Of Integer)("TokenSourceID")
        Private m_TokenClass As New CCursorFieldObj(Of String)("TokenClass")

        Public Sub New()
        End Sub

        Public ReadOnly Property TokenID As CCursorFieldObj(Of String)
            Get
                Return Me.m_TokenID
            End Get
        End Property

        Public ReadOnly Property TokenClass As CCursorFieldObj(Of String)
            Get
                Return Me.m_TokenClass
            End Get
        End Property

        Public ReadOnly Property TokenSourceName As CCursorFieldObj(Of String)
            Get
                Return Me.m_TokenSourceName
            End Get
        End Property

        Public ReadOnly Property TokenSourceID As CCursorField(Of Integer)
            Get
                Return Me.m_TokenSourceID
            End Get
        End Property

        Public ReadOnly Property TokenName As CCursorFieldObj(Of String)
            Get
                Return Me.m_TokenName
            End Get
        End Property

        Public ReadOnly Property Valore As CCursorFieldObj(Of String)
            Get
                Return Me.m_Valore
            End Get
        End Property

        Public ReadOnly Property Session As CCursorFieldObj(Of String)
            Get
                Return Me.m_Session
            End Get
        End Property

        Public ReadOnly Property CreatoDaID As CCursorField(Of Integer)
            Get
                Return Me.m_CreatoDaID
            End Get
        End Property

        Public ReadOnly Property CreatoIl As CCursorField(Of Date)
            Get
                Return Me.m_CreatoIl
            End Get
        End Property

        Public ReadOnly Property UsatoDaID As CCursorField(Of Integer)
            Get
                Return Me.m_UsatoDaID
            End Get
        End Property

        Public ReadOnly Property UsatoIl As CCursorField(Of Date)
            Get
                Return Me.m_UsatoIl
            End Get
        End Property

        Public ReadOnly Property Dettaglio As CCursorFieldObj(Of String)
            Get
                Return Me.m_Dettaglio
            End Get
        End Property

        Public ReadOnly Property ExpireTime As CCursorField(Of Date)
            Get
                Return Me.m_ExpireTime
            End Get
        End Property

        Public ReadOnly Property ExpireCount As CCursorField(Of Integer)
            Get
                Return Me.m_ExpireCount
            End Get
        End Property

        Public ReadOnly Property UseCount As CCursorField(Of Integer)
            Get
                Return Me.m_UseCount
            End Get
        End Property

        Protected Friend Overrides Function GetConnection() As CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_SecurityTokens"
        End Function

    End Class

End Class

