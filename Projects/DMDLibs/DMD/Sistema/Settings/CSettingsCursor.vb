Imports DMD.Databases

Partial Public Class Sistema

    Public Class CSettingsCursor
        Inherits DBObjectCursorBase(Of CSetting)

        Private m_OwnerID As New CCursorField(Of Integer)("OwnerID")
        Private m_OwnerType As New CCursorFieldObj(Of String)("OwnerType")
        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Valore As New CCursorFieldObj(Of String)("Valore")
        Private m_TipoValore As New CCursorField(Of TypeCode)("TipoValore")

        Public Sub New()
        End Sub

        Public ReadOnly Property OwnerID As CCursorField(Of Integer)
            Get
                Return Me.m_OwnerID
            End Get
        End Property

        Public ReadOnly Property OwnerType As CCursorFieldObj(Of String)
            Get
                Return Me.m_OwnerType
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property Valore As CCursorFieldObj(Of String)
            Get
                Return Me.m_Valore
            End Get
        End Property

        Public ReadOnly Property TipoValore As CCursorField(Of TypeCode)
            Get
                Return Me.m_TipoValore
            End Get
        End Property


        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function


        Public Overrides Function GetTableName() As String
            Return "tbl_Settings"
        End Function

    End Class


End Class
