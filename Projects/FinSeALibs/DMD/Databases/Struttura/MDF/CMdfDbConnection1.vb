Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization
Imports System.Data.SqlClient

Public partial class Databases

    Public Class CMdfDbConnection
        Inherits CDBConnection

        'Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\temp\WK_FINSAB2_data.mdf;Integrated Security=True;Connect Timeout=30
        Private m_UseLocalDB As Boolean = True
        Private m_DataSource As String = ".\SQLEXPRESS"
        Private m_ConnectionTimeout As Integer = 30
        Private m_IntegratedSecurity As Boolean = True

        ''' <summary>
        ''' Restituisce o imposta il nome dell'istanza di SQL Server a cui connettersi
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataSouce As String
            Get
                Return Me.m_DataSource
            End Get
            Set(value As String)
                Me.m_DataSource = value
            End Set
        End Property

        ''' <summary>
        ''' Restituisce o imposta il valore in secondi del timeout per il tentativo di connessione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ConnectionTimeOut As Integer
            Get
                Return Me.m_ConnectionTimeout
            End Get
            Set(value As Integer)
                Me.m_ConnectionTimeout = value
            End Set
        End Property

        Public Property IntegratedSecurity As Boolean
            Get
                Return Me.m_IntegratedSecurity
            End Get
            Set(value As Boolean)
                Me.m_IntegratedSecurity = value
            End Set
        End Property

        Public Property UseLocalDB As Boolean
            Get
                Return Me.m_UseLocalDB
            End Get
            Set(value As Boolean)
                Me.m_UseLocalDB = value
            End Set
        End Property

        Public Overridable Function GetConnectionString() As String
            Dim str As New System.Text.StringBuilder
            If (Me.m_UseLocalDB) Then
                str.Append("Server=(LocalDB)\v11.0;")
            Else
                str.Append("Data Source=" & Me.m_DataSource & ";")
                str.Append("Database='" & FileSystem.GetBaseName(Me.Path) & "';")
                str.Append("Connect Timeout=" & Me.m_ConnectionTimeout & ";")
            End If
            str.Append("AttachDbFilename=" & Me.Path & ";")
            str.Append("Integrated Security=" & CStr(IIf(Me.m_IntegratedSecurity, "True", "False")) & ";")
            Return str.ToString
        End Function


        Protected Friend Overrides Function CreateAdapterInternal(selectCommand As String) As IDataAdapter
            Return New SqlDataAdapter(selectCommand, DirectCast(Me.GetConnection, SqlConnection))
        End Function

        Protected Friend Overrides Function CreateCommandInternal(sql As String) As IDbCommand
            Dim cmd As System.Data.IDbCommand = Me.GetConnection.CreateCommand
            cmd.CommandText = sql
            Return cmd
        End Function

        Public Overrides Function CreateConnection() As IDbConnection
            Dim cn As New SqlConnection
            cn.ConnectionString = Me.GetConnectionString
            Return cn
        End Function

        Protected Friend Overrides Sub CreateField(field As CDBEntityField)
            Dim dbSQL As String
            dbSQL = "ALTER TABLE [" & field.Owner.Name & "] ADD COLUMN [" & field.Name & "] " & Me.GetSqlDataType(field)
            Me.ExecuteCommand(dbSQL)
        End Sub

        Protected Friend Overrides Sub CreateFieldConstraint(c As CDBFieldConstraint)
            Throw New NotImplementedException
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

        Protected Friend Overrides Sub CreateTableConstraint(c As CDBTableConstraint)
            Throw New NotImplementedException
        End Sub

        Protected Friend Overrides Sub CreateView(view As CDBQuery)
            Throw New NotImplementedException
        End Sub

        Protected Friend Overrides Sub DropField(field As CDBEntityField)
            Dim dbSQL As New System.Text.StringBuilder
            dbSQL.Append("ALTER TABLE [")
            dbSQL.Append(field.Owner.Name)
            dbSQL.Append("] DROP COLUMN [")
            dbSQL.Append(field.Name)
            dbSQL.Append("]")
            Me.ExecuteCommand(dbSQL.ToString)
        End Sub

        Protected Friend Overrides Sub DropFieldConstraint(c As CDBFieldConstraint)
            Throw New NotImplementedException
        End Sub

        Protected Friend Overloads Overrides Sub DropTable(table As CDBTable)
            Dim dbSQL As String
            dbSQL = "DROP TABLE [" & table.Name & "]"
            Me.ExecuteCommand(dbSQL)
        End Sub

        Protected Friend Overrides Sub DropTableConstraint(c As CDBTableConstraint)
            Throw New NotImplementedException
        End Sub

        Protected Friend Overrides Sub DropView(view As CDBQuery)
            Throw New NotImplementedException
        End Sub

        Public Overrides Function GetInsertCommand(obj As Object, idFieldName As String, dr As DataRow, Optional ByRef maxID As Integer = 0) As IDbCommand
            Dim tableName As String = Me.GetSaveTableName(obj)
            Dim table As CDBTable = Me.Tables(tableName)
            Dim cmd As SqlCommand = table.GetInsertCommand

            Dim param As SqlParameter
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
                Case TypeCode.Int32 : Return IIf(field.AutoIncrement, "Autonumber", "INT")
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
                Case Else
                    Throw New NotSupportedException
            End Select
        End Function

        Protected Overrides Function GetTablesArray() As CDBTable()
            ' We only want user tables, not system tables
            Dim restrictions() As String = {vbNullString, vbNullString, vbNullString, "Table"}
            Dim oleDBConn As SqlConnection = Me.GetConnection
            Dim userTables As DataTable = oleDBConn.GetSchema("Tables") ', restrictions)
            Dim ret As New CCollection(Of CDBTable)

            For Each dr As DataRow In userTables.Rows
                'System.Console.WriteLine(userTables.Rows(i)(2).ToString())
                Dim table As New CDBTable
                table.Catalog = Formats.ToString(Me.GetRowField(dr, "TABLE_CATALOG"))
                table.Schema = Formats.ToString(Me.GetRowField(dr, "TABLE_SCHEMA"))
                table.Name = Formats.ToString(Me.GetRowField(dr, "TABLE_NAME"))
                table.Type = Formats.ToString(Me.GetRowField(dr, "TABLE_TYPE"))
                table.Guid = Formats.ToString(Me.GetRowField(dr, "TABLE_GUID"))
                table.Description = Formats.ToString(Me.GetRowField(dr, "DESCRIPTION"))
                table.PropID = Formats.ToString(Me.GetRowField(dr, "TABLE_PROPID"))
                table.DateCreated = Formats.ToDate(Me.GetRowField(dr, "DATE_CREATED"))
                table.DateModified = Formats.ToDate(Me.GetRowField(dr, "DATE_MODIFIED"))
                table.SetChanged(False)
                table.SetCreated(True)
                table.SetConnection(Me)
                ret.Add(table)
            Next
            Return ret.ToArray
        End Function

        Private Function GetRowField(ByVal dr As DataRow, ByVal fieldName As String) As Object
            Try
                Return dr(fieldName)
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        Public Overrides Function GetUpdateCommand(obj As Object, idFieldName As String, idValue As Integer, dr As DataRow, changedValues As CKeyCollection(Of Boolean)) As IDbCommand
            Dim tableName As String = Me.GetSaveTableName(obj)
            Dim table As CDBTable = Me.Tables(tableName)
            Dim cmd As SqlCommand = table.GetUpdateCommand(changedValues)
            For Each dc As CDBEntityField In table.Fields
                If (dc.AutoIncrement OrElse changedValues(dc.Name)) Then
                    Dim param As SqlParameter
                    param = cmd.Parameters("@" & dc.Name)
                    param.Value = dr(dc.Name)
                End If
            Next

            Return cmd
        End Function

        Protected Friend Overrides Sub UpdateField(field As CDBEntityField)
            Dim dbSQL As String
            dbSQL = "ALTER TABLE [" & field.Owner.Name & "] CHANGE [" & field.Name & "] " & Me.GetSqlDataType(field)
            Me.ExecuteCommand(dbSQL)
        End Sub

        Protected Friend Overrides Sub UpdateFieldConstraint(c As CDBFieldConstraint)
            Throw New NotImplementedException
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

        Protected Friend Overrides Sub UpdateTableConstraint(c As CDBTableConstraint)
            Throw New NotImplementedException
        End Sub

        Protected Friend Overrides Sub UpdateView(vuew As CDBQuery)
            Throw New NotImplementedException
        End Sub

        Protected Friend Overrides Sub CreateInternal()
            Throw New NotImplementedException()
        End Sub

        Public Overrides Function InvokeMethodArray([module] As CModule, methodName As String, args() As Object) As String
            Throw New NotImplementedException()
        End Function

        Public Overrides Function InvokeMethodArrayAsync([module] As CModule, methodName As String, handler As Object, args() As Object) As AsyncState
            Throw New NotImplementedException()
        End Function
    End Class

End Class