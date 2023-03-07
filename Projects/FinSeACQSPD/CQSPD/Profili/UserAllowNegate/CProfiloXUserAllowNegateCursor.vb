Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Partial Public Class CQSPD

    Public Class CProfiloXUserAllowNegateCursor
        Inherits UserAllowNegateCursor(Of CProfilo)

        Public Sub New()
        End Sub

        Protected Overrides Function GetItemFieldName() As String
            Return "Preventivatore"
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_PreventivatoriXUser"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New CProfiloXUserAllowNegate
        End Function

    End Class

End Class

