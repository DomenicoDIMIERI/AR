Imports FinSeA

Namespace FinSeA

     

    Public MustInherit Class DBObjectService
        Protected m_ID As Integer

        Public Sub New()
            Me.m_ID = 0
        End Sub

        Public ReadOnly Property ID As Integer
            Get
                Return Me.m_ID
            End Get
        End Property



        Protected Overridable Sub DoChanged(ByVal propName As String, ByVal newValue As Object, ByVal oldValue As Object)

        End Sub

        Protected Function DBStr(ByVal str As String) As String
            If str Is vbNullString Then
                Return "NULL"
            Else
                Return "'" & Replace(str, "'", "''") & "'"
            End If
        End Function

        Protected Function DBNumber(ByVal value As Object) As String
            If IsDBNull(value) OrElse value Is Nothing Then
                Return "NULL"
            Else
                Return Trim(Replace(value, ",", "."))
            End If
        End Function

        Protected Function DBBool(ByVal value As Object) As String
            If TypeOf (value) Is DBNull Then Return "NULL"
            Return IIf(CBool(value), "True", "False")
        End Function

        Protected Function DBDate(ByVal value As Object) As String
            If (TypeOf (value) Is DBNull OrElse value Is Nothing) Then
                Return "NULL"
            ElseIf (TypeOf (value) Is Nullable(Of Date)) AndAlso (DirectCast(value, Nullable(Of Date)).HasValue = False) Then
                Return "NULL"
            Else
                Dim d As Date = value
                Return "#" & Month(d) & "/" & Day(d) & "/" & Year(d) & " " & Hour(d) & ":" & Minute(d) & ":" & Second(d) & "#"
            End If
        End Function

        Protected Function ToInt(ByVal value As Object, Optional ByVal defValue As Integer = 0) As Integer
            If (TypeOf (value) Is DBNull) Then Return defValue
            Return CInt(value)
        End Function

        Protected Function ToStr(ByVal value As Object, Optional ByVal defValue As String = "") As String
            If (TypeOf (value) Is DBNull) Then Return defValue
            Return CStr(value)
        End Function

        Protected Function ToValuta(ByVal value As Object, Optional ByVal defValue As Decimal = 0.0) As Decimal
            If (TypeOf (value) Is DBNull) Then Return defValue
            Return CDec(value)
        End Function


        Protected Function ToDate(ByVal value As Object, Optional ByVal defValue As Date = Nothing) As Date
            If (TypeOf (value) Is DBNull) Then Return defValue
            Return CDate(value)
        End Function

        Protected Function ParseDate(ByVal value As Object) As Nullable(Of Date)
            If (TypeOf (value) Is DBNull) Then Return Nothing
            Return CDate(value)
        End Function

        Protected Overridable Sub Load(ByVal dbRis As System.Data.IDataReader)
            Me.m_ID = ToInt(dbRis("ID"))
        End Sub

        Protected MustOverride Function GetInsertCommand() As String

        Protected MustOverride Function GetUpdateCommand() As String

        Protected MustOverride Function GetConnection() As DBConnection

        Public Overridable Sub Save()
            Dim dbSQL As String = ""
            If Me.m_ID = 0 Then
                dbSQL = Me.GetInsertCommand
                Me.GetConnection.ExecuteCommand(dbSQL)
                Me.m_ID = Me.GetConnection.ExecuteScalar("SELECT @@IDENTITY")
            Else
                dbSQL = Me.GetUpdateCommand
                Me.GetConnection.ExecuteCommand(dbSQL)
            End If

        End Sub
    End Class
     

End Namespace