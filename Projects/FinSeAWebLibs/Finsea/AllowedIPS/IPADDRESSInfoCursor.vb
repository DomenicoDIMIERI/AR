Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports System.IO


Partial Public Class WebSite


    Public Class IPADDRESSInfoCursor
        Inherits DBObjectCursor(Of IPADDRESSinfo)

        Private m_IP As New CCursorFieldObj(Of String)("IP")
        Private m_NetMask As New CCursorFieldObj(Of String)("NetMask")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_Allow As New CCursorField(Of Boolean)("Allow")
        Private m_Negate As New CCursorField(Of Boolean)("Negate")
        Private m_Interno As New CCursorField(Of Boolean)("Interno")
        Private m_AssociaUfficio As New CCursorFieldObj(Of String)("AssociaUfficio")

        Public Sub New()
        End Sub

        Public ReadOnly Property IP As CCursorFieldObj(Of String)
            Get
                Return Me.m_IP
            End Get
        End Property

        Public ReadOnly Property NetMask As CCursorFieldObj(Of String)
            Get
                Return Me.m_NetMask
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
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

        Public ReadOnly Property Interno As CCursorField(Of Boolean)
            Get
                Return Me.m_Interno
            End Get
        End Property

        Public ReadOnly Property AssociaUfficio As CCursorFieldObj(Of String)
            Get
                Return Me.m_AssociaUfficio
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return WebSite.Instance.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_AllowedIPs"
        End Function
    End Class




End Class
 