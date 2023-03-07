Imports DMD.Databases

Imports DMD.Sistema

Partial Public Class CQSPD

    Public Class CZonaCursor
        Inherits DBObjectCursorPO(Of CZona)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")

        Public Sub New()
        End Sub

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Protected Overrides Function GetModule() As CModule
            Return Zone.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDZone"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

    End Class


End Class