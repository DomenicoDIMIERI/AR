Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Partial Public Class WebSite

    Public Class LinkUserAllowNegateCursor
        Inherits UserAllowNegateCursor(Of CCollegamento)

        Public Sub New()
        End Sub

        Protected Overrides Function GetItemFieldName() As String
            Return "Collegamento"
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CollegamentiXUtente"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New LinkUserAllowNegate
        End Function

    End Class

End Class

