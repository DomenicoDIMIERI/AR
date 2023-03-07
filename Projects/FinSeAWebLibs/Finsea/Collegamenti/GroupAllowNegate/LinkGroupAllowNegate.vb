Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Partial Public Class WebSite

    <Serializable> _
    Public Class LinkGroupAllowNegate
        Inherits GroupAllowNegate(Of CCollegamento)

        Public Sub New()
        End Sub

        Protected Overrides Function GetItemFieldName() As String
            Return "Collegamento"
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CollegamentiXGruppo"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function


    End Class

End Class

