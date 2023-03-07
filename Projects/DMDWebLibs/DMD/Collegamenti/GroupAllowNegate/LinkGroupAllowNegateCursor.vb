Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Partial Public Class WebSite

    Public Class LinkGroupAllowNegateCursor
        Inherits GroupAllowNegateCursor(Of CCollegamento)

        Public Sub New()
        End Sub

        Protected Overrides Function GetItemFieldName() As String
            Return "Collegamento"
        End Function

        Protected Overrides Function GetGroupFieldName() As String
            Return "Gruppo"
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CollegamentiXGruppo"
        End Function

        Protected Overrides Function GetConnection() As CDBConnection
            Return Databases.APPConn
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function InstantiateNew(dbRis As DBReader) As Object
            Return New LinkGroupAllowNegate
        End Function
    End Class

End Class

