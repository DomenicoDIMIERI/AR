Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Public partial class Databases

    Public Class CCvsDBConnection
        Inherits CXlsDBConnection

       
        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal fileName As String, Optional ByVal useHeaders As Boolean = True)
            MyBase.New(fileName, "", useHeaders)
        End Sub


        Public Overrides Function GetSqlDataType(field As CDBEntityField) As String
            If field.DataType Is GetType(String) Then
                Return IIf(field.MaxLength > 0, "TEXT(255)", "MEMO")
            Else
                Return MyBase.GetSqlDataType(field)
            End If
        End Function

        Public Overrides Function GetFriendlyName(name As String) As String
            name = MyBase.GetFriendlyName(name)
            If Right(name, 1) = "$" Or Right(name, 1) = "_" Then
                Return Left(name, Len(name) - 1)
            Else
                Return name
            End If
        End Function

        Public Overrides Function GetInternalTableName(table As CDBEntity) As String
            Return Replace(table.Name, "#c", ".csv")
        End Function

        Public Overrides Function GetConnectionString() As String
            Const excelObject As String = "Provider=Microsoft.{0}.OLEDB.{1};Data Source={2};Extended Properties=""{3};HDR=YES;{4}"""
            Return String.Format(excelObject, "Jet", "4.0", Me.Path, "text", "FMT=Delimited")
        End Function

        Protected Overrides Function GetTablesArray() As CDBTable()
            Dim ret As CDBTable() = MyBase.GetTablesArray()
            For i As Integer = 0 To UBound(ret)
                Dim table As CDBTable = ret(i)
                table.IsHidden = Right(table.Name, 1) = "_"
                table.Name = Left(table.Name, Len(table.Name) - 1)
            Next
            Return ret
        End Function

        

    End Class


End Class


