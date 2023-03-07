
Namespace Excel

    Public MustInherit Class ExcelInterop

        Public MustOverride Sub DeleteWorkSheetFromFile(ByVal fileName As String, ByVal sheetName As String)
        Public MustOverride Sub FormatStandardTable(ByVal fileName As String, ByVal sheetName As String)



    End Class

End Namespace