Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Public partial class Databases

    Public Class CDBTable
        Inherits CDBEntity

        'Private m_InsertCommand As System.Data.IDbCommand
        'Private m_UpdateCommand As System.Data.IDbCommand

        Public Sub New()
            ' Me.m_InsertCommand = Nothing
            ' Me.m_UpdateCommand = Nothing
        End Sub

        Public Sub New(ByVal tableName As String)
            MyBase.New(tableName)
            'Me.m_InsertCommand = Nothing
            'Me.m_UpdateCommand = Nothing
        End Sub

        Public Function GetUpdateCommand(ByVal changedValues As CKeyCollection(Of Boolean)) As System.Data.IDbCommand
            'If (Me.m_UpdateCommand IsNot Nothing) Then Me.m_UpdateCommand.Dispose()
            'Me.m_UpdateCommand = Me.GetNewUpdateCommand(changedValues)
            'Return Me.m_UpdateCommand
            Return Me.GetNewUpdateCommand(changedValues)
        End Function

        Public Function GetInsertCommand() As System.Data.IDbCommand
            'If Me.m_InsertCommand Is Nothing Then Me.m_InsertCommand = Me.GetNewInsertCommand
            'Return Me.m_InsertCommand
            Return Me.GetNewInsertCommand
        End Function

        Private Function GetNewUpdateCommand(ByVal changedValues As CKeyCollection(Of Boolean)) As System.Data.IDbCommand
            Dim names As String
            Dim tableName As String = Me.Name
            Dim idFieldName As String = vbNullString
            names = vbNullString
            For Each dc As CDBEntityField In Me.Fields
                If dc.AutoIncrement = False Then
                    If (changedValues(dc.Name)) Then
                        names = Strings.Combine(names, "[" & dc.Name & "]=@" & dc.Name, ",")
                    End If
                Else
                    idFieldName = dc.Name
                End If
            Next

            Dim cmd As DMDDBCommand = Me.Connection.CreateCommand("UPDATE [" & tableName & "] SET " & names & " WHERE [" & idFieldName & "]=@" & idFieldName)
            Dim param As System.Data.OleDb.OleDbParameter

            For Each dc As CDBEntityField In Me.Fields
                If dc.AutoIncrement = False AndAlso changedValues(dc.Name) Then
                    param = DirectCast(cmd.m_cmd, System.Data.OleDb.OleDbCommand).Parameters.Add("@" & dc.Name, DBUtils.GetOleDbDataType(dc.DataType))
                    'param.Direction = ParameterDirection.Output
                    param.IsNullable = dc.AllowDBNull
                    param.SourceColumn = dc.Name
                End If
            Next

            For Each dc As CDBEntityField In Me.Fields
                If dc.AutoIncrement = True Then
                    param = DirectCast(cmd.m_cmd, System.Data.OleDb.OleDbCommand).Parameters.Add("@" & dc.Name, DBUtils.GetOleDbDataType(dc.DataType))
                    'param.Direction = ParameterDirection.Output
                    param.IsNullable = dc.AllowDBNull
                    param.SourceColumn = dc.Name
                End If
            Next

            Return cmd
        End Function

        Private Function GetNewInsertCommand() As System.Data.IDbCommand
            ' Dim ds As System.Data.DataSet
            'Dim dt As New System.Data.DataTable
            'Dim da As System.Data.IDataAdapter
            Dim names, values As String
            Dim tableName As String = Me.Name
            Dim idFieldName As String

            ' da = Me.Connection.CreateAdapter("SELECT * FROM [" & tableName & "] WHERE (0<>0)")
            'ds = New System.Data.DataSet()
            'da.FillSchema(ds, SchemaType.Source)
            'dt = ds.Tables(0)
            names = vbNullString
            values = vbNullString
            'For Each dc As System.Data.DataColumn In dt.Columns
            For Each dc As CDBEntityField In Me.Fields
                If dc.AutoIncrement = False Then
                    names = Strings.Combine(names, "[" & dc.Name & "]", ",")
                    values = Strings.Combine(values, "@" & dc.Name, ",")
                Else
                    idFieldName = dc.Name
                End If
            Next

            Dim cmd As DMDDBCommand = Me.Connection.CreateCommand("INSERT INTO [" & tableName & "] (" & names & ") VALUES (" & values & ")")
            Dim param As System.Data.OleDb.OleDbParameter
            'For Each dc As System.Data.DataColumn In dt.Columns
            For Each dc As CDBEntityField In Me.Fields
                If dc.AutoIncrement = False Then
                    param = DirectCast(cmd.m_cmd, System.Data.OleDb.OleDbCommand).Parameters.Add("@" & dc.Name, DBUtils.GetOleDbDataType(dc.DataType))

                    'If (dc.ColumnName = idFieldName) Then
                    '    maxID = Me.ExecuteScalar("SELECT 1 + Max([" & idFieldName & "]) FROM [" & tableName & "]")
                    '    param.Value = maxID
                    'Else
                    '    param.Value = dr(dc.ColumnName)
                    'End If
                End If
            Next

            'dt.Dispose()
            'ds.Dispose()

            Return cmd
        End Function



        Protected Overrides Sub CreateInternal1()
            Me.Connection.CreateTable(Me)
        End Sub

        Protected Overrides Sub DropInternal1()
            Me.Connection.DropTable(Me)
            Me.Connection.Tables.RemoveByKey(Me.Name)
        End Sub

        Protected Overrides Sub UpdateInternal1()
            Me.Connection.UpdateTable(Me)
        End Sub

        Protected Overrides Sub RenameItnernal(newName As String)
            Dim buffer As New System.Text.StringBuilder
            buffer.Append("ALTER TABLE ")
            buffer.Append(Me.InternalName)
            buffer.Append(" RENAME TO [")
            buffer.Append(newName)
            Me.Connection.ExecuteCommand(buffer.ToString)
        End Sub

    End Class


End Class


