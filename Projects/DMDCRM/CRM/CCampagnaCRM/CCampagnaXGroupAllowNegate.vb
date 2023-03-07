Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica
Imports DMD.XML

Partial Public Class CustomerCalls


    <Serializable>
    Public Class CCampagnaXGroupAllowNegate
        Inherits GroupAllowNegate(Of CCampagnaCRM)


        Public Sub New()
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return Nothing
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Nothing
        End Function

        Protected Overrides Function GetItemFieldName() As String
            Return ""
        End Function
    End Class



End Class