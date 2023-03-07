Imports Microsoft.Office.Interop.Excel

Namespace Excel

    Public Class Excel12Interop
        Inherits ExcelInterop

        Public Overrides Sub DeleteWorkSheetFromFile(fileName As String, sheetName As String)
            'Dim excelApp As New Microsoft.Office.Interop.Excel.Application
            'Dim workbook As Microsoft.Office.Interop.Excel._Workbook
            'Dim worksheet As Microsoft.Office.Interop.Excel._Worksheet
            ''Dim sheet As Microsoft.Office.Interop.Excel.Sheets
            'excelApp.DisplayAlerts = False
            'workbook = excelApp.Workbooks.Open(fileName)
            'worksheet = workbook.Worksheets(sheetName)
            'worksheet.Delete()
            'workbook.Save()
            'excelApp.Quit()

        End Sub

        Public Overrides Sub FormatStandardTable(fileName As String, sheetName As String)
            'Dim excelApp As New Microsoft.Office.Interop.Excel.Application
            'Dim workbook As Microsoft.Office.Interop.Excel._Workbook
            'Dim worksheet As Microsoft.Office.Interop.Excel._Worksheet
            ''Dim range As Microsoft.Office.Interop.Excel.Range
            'excelApp.DisplayAlerts = False
            'workbook = excelApp.Workbooks.Open(fileName)
            'worksheet = workbook.Worksheets(sheetName)
            ''range = worksheet.Range(worksheet.PageSetup.PrintArea).Style
            'worksheet.Columns.AutoFit()
            'workbook.Save()
            'excelApp.Quit()
        End Sub

    End Class

End Namespace