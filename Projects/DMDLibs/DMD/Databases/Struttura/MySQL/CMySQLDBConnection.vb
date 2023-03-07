Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Partial Public Class Databases



    Public Class CMySQLDBConnection
        Inherits CDBConnection

        Private m_CharSet As String

        Public Sub New()
            Me.m_CharSet = "utf8"
        End Sub

        Public Property CharSet As String
            Get
                Return Me.m_CharSet
            End Get
            Set(value As String)
                Me.m_CharSet = value
            End Set
        End Property

        Protected Friend Overrides Sub CreateInternal()
            Dim cmd As String
            cmd = "CREATE DATABASE " & Me.Path & " DEFAULT CHARACTER SET " & Me.m_CharSet & " COLLATE utf8_unicode_ci;"

        End Sub


        Public Overridable Function GetConnectionString() As String
            'Dim ret As New System.Text.StringBuilder
            'ret.Append("Provider=" & Me.m_Provider & ";")
            'ret.Append("Data Source=" & Me.Path & ";")
            'If (Me.m_PersistSecurityInfo) Then ret.Append("Persist Security Info=" & Me.m_PersistSecurityInfo & ";")
            'Select Case Me.m_LockingMode
            '    Case OleDbLockMode.RowLevelLock : ret.Append("Jet OLEDB:Database Locking Mode=1")
            '    Case OleDbLockMode.TableLevelLock : ret.Append("Jet OLEDB:Database Locking Mode=0")
            '    Case Else
            'End Select
            'Return ret.ToString
            Throw New NotImplementedException
        End Function


        Public Overrides Function CreateConnection() As IDbConnection
            'Dim builder As New OleDb.OleDbConnectionStringBuilder(GetConnectionString())
            'builder.AsynchronousProcessin= True


            Dim cn As New System.Data.OleDb.OleDbConnection
            cn.ConnectionString = Me.GetConnectionString
            Return cn
        End Function

        Protected Friend Overrides Function CreateAdapterInternal(selectCommand As String) As IDataAdapter
            Return New System.Data.OleDb.OleDbDataAdapter(selectCommand, DirectCast(Me.GetConnection, System.Data.OleDb.OleDbConnection))
        End Function

        Protected Friend Overrides Function CreateCommandInternal(ByVal sql As String) As IDbCommand
            Dim cmd As System.Data.IDbCommand = Me.GetConnection.CreateCommand
            cmd.CommandText = sql
            Return cmd
        End Function

        Protected Overrides Function GetTablesArray() As CDBTable()
            ' We only want user tables, not system tables
            Dim restrictions() As String = {vbNullString, vbNullString, vbNullString, "Table"}
            Dim oleDBConn As System.Data.OleDb.OleDbConnection = Me.GetConnection
            Dim userTables As DataTable = oleDBConn.GetSchema("Tables", restrictions)
            Dim ret As New CCollection(Of CDBTable)

            For Each dr As DataRow In userTables.Rows
                'System.Console.WriteLine(userTables.Rows(i)(2).ToString())
                Dim table As New CDBTable
                table.Catalog = Formats.ToString(dr("TABLE_CATALOG"))
                table.Schema = Formats.ToString(dr("TABLE_SCHEMA"))
                table.Name = Formats.ToString(dr("TABLE_NAME"))
                table.Type = Formats.ToString(dr("TABLE_TYPE"))
                table.Guid = Formats.ToString(dr("TABLE_GUID"))
                table.Description = Formats.ToString(dr("DESCRIPTION"))
                table.PropID = Formats.ToString(dr("TABLE_PROPID"))
                table.DateCreated = Formats.ToDate(dr("DATE_CREATED"))
                table.DateModified = Formats.ToDate(dr("DATE_MODIFIED"))
                table.SetChanged(False)
                table.SetCreated(True)
                table.SetConnection(Me)
                ret.Add(table)
            Next
            Return ret.ToArray
        End Function



        Public Overrides Function GetUpdateCommand(ByVal obj As Object, ByVal idFieldName As String, ByVal idValue As Integer, ByVal dr As System.Data.DataRow, ByVal changedValues As CKeyCollection(Of Boolean)) As System.Data.IDbCommand
            Dim tableName As String = Me.GetSaveTableName(obj)
            Dim table As CDBTable = Me.Tables(tableName)
            Dim cmd As DMDDBCommand = table.GetUpdateCommand(changedValues)
            For Each dc As CDBEntityField In table.Fields
                If (dc.AutoIncrement OrElse changedValues(dc.Name)) Then
                    Dim param As System.Data.OleDb.OleDbParameter
                    param = cmd.Parameters("@" & dc.Name)
                    param.Value = dr(dc.Name)
                End If
            Next

            Return cmd
        End Function

        Public Overrides Function GetInsertCommand(ByVal obj As Object, ByVal idFieldName As String, ByVal dr As System.Data.DataRow, Optional ByRef maxID As Integer = 0) As System.Data.IDbCommand
            'Dim ds As System.Data.DataSet
            'Dim dt As New System.Data.DataTable
            'Dim da As System.Data.IDataAdapter
            'Dim names, values As String
            Dim tableName As String = Me.GetSaveTableName(obj)
            Dim table As CDBTable = Me.Tables(tableName)
            Dim cmd As DMDDBCommand = table.GetInsertCommand

            'da = Me.CreateAdapter("SELECT * FROM [" & tableName & "] WHERE (0<>0)")
            'ds = New System.Data.DataSet()
            'da.FillSchema(ds, SchemaType.Source)
            'dt = ds.Tables(0)
            'names = vbNullString
            'values = vbNullString
            'For Each dc As System.Data.DataColumn In dt.Columns
            '    If dc.AutoIncrement = False Then
            '        names = Strings.Combine(names, "[" & dc.ColumnName & "]", ",")
            '        values = Strings.Combine(values, "@" & dc.ColumnName, ",")
            '    End If
            'Next

            'Dim cmd As System.Data.OleDb.OleDbCommand = Me.CreateCommand("INSERT INTO [" & tableName & "] (" & names & ") VALUES (" & values & ")")
            Dim param As System.Data.OleDb.OleDbParameter
            'For Each dc As System.Data.DataColumn In table.Fields ' dt.Columns
            For Each dc As CDBEntityField In table.Fields ' dt.Columns
                If dc.AutoIncrement = False Then
                    param = cmd.Parameters("@" & dc.Name)
                    If (dc.Name = idFieldName) Then
                        maxID = Formats.ToInteger(Me.ExecuteScalar("SELECT Max([" & idFieldName & "]) FROM [" & tableName & "]"))
                        param.Value = maxID + 1
                    Else
                        param.Value = dr(dc.Name)
                    End If
                End If
            Next


            'dt.Dispose()
            ' ds.Dispose()

            Return cmd
        End Function

        Public Overrides Function GetSqlDataType(field As CDBEntityField) As String
            Select Case Types.GetTypeCode(field.DataType)
                Case TypeCode.Boolean : Return "BIT"
                Case TypeCode.Byte : Return "TINYINT"
                Case TypeCode.Char : Return "CHAR"
                Case TypeCode.DateTime : Return "DATE"
                    'Case TypeCode.DBNull : Return "
                Case TypeCode.Decimal : Return "DECIMAL"
                Case TypeCode.Double : Return "REAL"
                    'Case TypeCode.Empty : Return Nothing
                Case TypeCode.Int16 : Return "SHORT"
                Case TypeCode.Int32 : Return IIf(field.AutoIncrement, "COUNTER", "INT")
                Case TypeCode.Int64 : Return "LONG"
                    'Case TypeCode.Object : Return value
                Case TypeCode.SByte : Return "TINYINT"
                Case TypeCode.Single : Return "SINGLE"
                Case TypeCode.String
                    If (field.MaxLength = 0) Then
                        Return "MEMO"
                    Else
                        Return "TEXT(" & field.MaxLength & ")"
                    End If
                Case TypeCode.UInt16 : Return "SHORT"
                Case TypeCode.UInt32 : Return "INT"
                Case TypeCode.UInt64 : Return "LONG"
                Case TypeCode.Object : Return "IMAGE"
                Case Else
                    Throw New NotSupportedException
            End Select
        End Function

        Public Overrides Function GetInternalTableName(table As CDBEntity) As String
            Return "[" & table.Name & "]"
        End Function

        Public Overrides Function GetFriendlyName(name As String) As String
            name = Trim(name)
            If Left(name, 1) = "[" And Right(name, 1) = "]" Then
                name = Mid(name, 2, Len(name) - 2)
            End If
            Return name
        End Function

        Protected Friend Overrides Sub CreateField(field As CDBEntityField)
            Dim dbSQL As String
            dbSQL = "ALTER TABLE [" & field.Owner.Name & "] ADD COLUMN [" & field.Name & "] " & Me.GetSqlDataType(field)
            Me.ExecuteCommand(dbSQL)
        End Sub

        Protected Friend Overrides Sub CreateTable(table As CDBTable)
            Dim sql As String
            Dim t As Boolean = False
            sql = vbNullString
            sql &= "CREATE TABLE [" & table.Name & "] ("
            For Each field As CDBEntityField In table.Fields
                If (t) Then sql &= ","
                sql &= "[" & field.Name & "] " & Me.GetSqlDataType(field)
                t = True
            Next
            sql &= ")"
            Me.ExecuteCommand(sql)
        End Sub

        Protected Friend Overrides Sub CreateView(view As CDBQuery)
            Throw New NotImplementedException
        End Sub

        Protected Friend Overrides Sub DropField(field As CDBEntityField)
            Dim dbSQL As String
            dbSQL = "ALTER TABLE [" & field.Owner.Name & "] DROP COLUMN [" & field.Name & "]"
            Me.ExecuteCommand(dbSQL)
        End Sub

        Protected Friend Overloads Overrides Sub DropTable(table As CDBTable)
            Dim dbSQL As String
            dbSQL = "DROP TABLE [" & table.Name & "]"
            Me.ExecuteCommand(dbSQL)
        End Sub

        Protected Friend Overrides Sub DropView(view As CDBQuery)
            Throw New NotImplementedException
        End Sub

        Protected Friend Overrides Sub UpdateField(field As CDBEntityField)
            Dim dbSQL As String
            dbSQL = "ALTER TABLE [" & field.Owner.Name & "] CHANGE [" & field.Name & "] " & Me.GetSqlDataType(field)
            Me.ExecuteCommand(dbSQL)
        End Sub

        Protected Friend Overrides Sub UpdateTable(table As CDBTable)
            For Each field As CDBEntityField In table.Fields
                If (field.IsCreated = False) Then
                    field.Create()
                ElseIf (field.IsChanged) Then
                    field.Update()
                End If
            Next
        End Sub

        Protected Friend Overrides Sub UpdateView(vuew As CDBQuery)
            Throw New NotImplementedException
        End Sub

        Protected Friend Overrides Sub CreateFieldConstraint(c As CDBFieldConstraint)
            Throw New NotImplementedException
        End Sub

        Protected Friend Overrides Sub DropFieldConstraint(c As CDBFieldConstraint)
            Throw New NotImplementedException
        End Sub

        Protected Friend Overrides Sub UpdateFieldConstraint(c As CDBFieldConstraint)
            Throw New NotImplementedException
        End Sub

        Protected Friend Overrides Sub CreateTableConstraint(c As CDBTableConstraint)
            Throw New NotImplementedException
        End Sub

        Protected Friend Overrides Sub DropTableConstraint(c As CDBTableConstraint)
            Dim dbSQL As String = "DROP INDEX [" & c.Name & "] ON [" & c.Owner.Name & "] "
            Me.ExecuteCommand(dbSQL)
        End Sub

        Protected Friend Overrides Sub UpdateTableConstraint(c As CDBTableConstraint)
            Throw New NotImplementedException
        End Sub

        Public Overrides Function InvokeMethodArray([module] As CModule, methodName As String, args() As Object) As String
            Throw New NotImplementedException()
        End Function

        Public Overrides Function InvokeMethodArrayAsync([module] As CModule, methodName As String, handler As Object, args() As Object) As AsyncState
            Throw New NotImplementedException()
        End Function
    End Class

End Class


