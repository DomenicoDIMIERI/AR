Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Partial Public Class WebSite

    <Serializable> _
    Public Class LinkUserAllowNegate
        Inherits UserAllowNegate(Of CCollegamento)

        Public Sub New()
        End Sub

        Protected Overrides Function GetItemFieldName() As String
            Return "Collegamento"
        End Function

        Protected Overrides Function GetUserFieldName() As String
            Return "Utente"
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CollegamentiXUtente"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Databases.APPConn
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

    End Class

End Class

