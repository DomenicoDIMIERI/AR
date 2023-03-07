Namespace Excel

    Public Class ExcelUtils
        Private Shared m_Interop As ExcelInterop

        Shared Sub New()
        End Sub

        Friend Shared Function GetInterop() As ExcelInterop
            If m_Interop Is Nothing Then m_Interop = Initialize
            Return m_Interop
        End Function

        Private Shared Function Initialize() As ExcelInterop
            Return New Excel12Interop
        End Function

        Public Shared Sub DeleteWorkSheetFromFile(ByVal fileName As String, ByVal sheetName As String)
            GetInterop.DeleteWorkSheetFromFile(fileName, sheetName)
        End Sub

        Public Shared Sub FormatStandardTable(ByVal fileName As String, ByVal sheetName As String)
            GetInterop.FormatStandardTable(fileName, sheetName)
        End Sub

    End Class

End Namespace