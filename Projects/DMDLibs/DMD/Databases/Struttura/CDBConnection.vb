Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Partial Public Class Databases

    Public Class DMDDBCObj
        Implements IDisposable

        Public Conn As CDBConnection
        Public ReadOnly CreationTime As Date
        Public CompletionTime As Date? = Nothing

        Private Shared m_Timer As New System.Timers.Timer(5000)
        Private Shared m_List As New System.Collections.ArrayList

        Shared Sub New()
            AddHandler m_Timer.Elapsed, AddressOf timerClick
            m_Timer.Enabled = True
        End Sub



        Private Shared Sub timerClick(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim i As Integer = 0
            Do
                Dim o As DMDDBCObj = Nothing
                SyncLock m_List
                    If (i < m_List.Count) Then
                        o = m_List(i)
                    End If
                End SyncLock

                If (o Is Nothing) Then Exit Do

                If (o.ExeTimeMilliseconds > 30000) Then
                    SyncLock m_List
                        m_List.Remove(o)
                    End SyncLock
                    Debug.Print("DB Lock? " & o.ToString)
                    Sistema.Events.NotifyUnhandledException(New DBException("DB Lock? " & o.ToString))
                Else
                    i += 1
                End If
            Loop
        End Sub

        Public Shared Function GetList() As System.Collections.ArrayList
            SyncLock m_List
                Return m_List.Clone
            End SyncLock
        End Function

        Public Sub New(ByVal conn As CDBConnection)
            Me.Conn = conn
            Me.CreationTime = Now
            'Me.UserName = Sistema.Users.usern
            SyncLock m_List
                m_List.Add(Me)
            End SyncLock
        End Sub

        Public Overridable Sub Create()

        End Sub

        Public ReadOnly Property ExeTimeMilliseconds As Double
            Get
                If (Me.CompletionTime.HasValue) Then
                    Return (Me.CompletionTime.Value - Me.CreationTime).TotalMilliseconds
                Else
                    Return (Now - Me.CreationTime).TotalMilliseconds
                End If

            End Get
        End Property

        ' Questo codice viene aggiunto da Visual Basic per implementare in modo corretto il criterio Disposable.
        Public Overridable Sub Dispose() Implements IDisposable.Dispose
            SyncLock m_List
                m_List.Remove(Me)
            End SyncLock
            Me.Conn = Nothing
            Me.CompletionTime = Nothing
        End Sub

    End Class

    Public Class DMDDBCommand
        Inherits DMDDBCObj
        Implements System.Data.IDbCommand

        Public Text As String
        Public m_cmd As System.Data.IDbCommand

        Public Sub New(ByVal conn As CDBConnection, ByVal text As String)
            MyBase.New(conn)
            Me.Text = text
        End Sub

        Public Overrides Sub Create()
            Me.m_cmd = Me.Conn.CreateCommandInternal(Me.Text)
        End Sub

        Public Property CommandText As String Implements IDbCommand.CommandText
            Get
                Return Me.m_cmd.CommandText
            End Get
            Set(value As String)
                Me.m_cmd.CommandText = value
            End Set
        End Property

        Public Property CommandTimeout As Integer Implements IDbCommand.CommandTimeout
            Get
                Return Me.m_cmd.CommandTimeout
            End Get
            Set(value As Integer)
                Me.m_cmd.CommandTimeout = value
            End Set
        End Property

        Public Property CommandType As CommandType Implements IDbCommand.CommandType
            Get
                Return Me.m_cmd.CommandType
            End Get
            Set(value As CommandType)
                Me.m_cmd.CommandType = value
            End Set
        End Property

        Public Property Connection As IDbConnection Implements IDbCommand.Connection
            Get
                Return Me.m_cmd.Connection
            End Get
            Set(value As IDbConnection)
                Me.m_cmd.Connection = value
            End Set
        End Property

        Public ReadOnly Property Parameters As IDataParameterCollection Implements IDbCommand.Parameters
            Get
                Return Me.m_cmd.Parameters
            End Get
        End Property

        Public Property Transaction As IDbTransaction Implements IDbCommand.Transaction
            Get
                Return Me.m_cmd.Transaction
            End Get
            Set(value As IDbTransaction)
                Me.m_cmd.Transaction = value
            End Set
        End Property

        Public Property UpdatedRowSource As UpdateRowSource Implements IDbCommand.UpdatedRowSource
            Get
                Return Me.m_cmd.UpdatedRowSource
            End Get
            Set(value As UpdateRowSource)
                Me.m_cmd.UpdatedRowSource = value
            End Set
        End Property

        Public Sub Cancel() Implements IDbCommand.Cancel
            Me.m_cmd.Cancel()
        End Sub

        Public Sub Prepare() Implements IDbCommand.Prepare
            Me.m_cmd.Prepare()
        End Sub

        Public Function CreateParameter() As IDbDataParameter Implements IDbCommand.CreateParameter
            Return Me.m_cmd.CreateParameter
        End Function

        Private Sub WarnQuery(ByVal sql As String, ByVal time As Double)
            Dim buffer As New System.Text.StringBuilder
            buffer.Append("Query lenta: (")
            buffer.Append(Formats.FormatDurata(time / 1000))
            buffer.Append(")")
            buffer.Append(vbNewLine)
            buffer.Append(sql)
            buffer.Append(vbNewLine)

            Sistema.ApplicationContext.Log(buffer.ToString)
        End Sub

        Public Function ExecuteNonQuery() As Integer Implements IDbCommand.ExecuteNonQuery
            Dim t As Date = Now
            Dim ret As Integer = Me.m_cmd.ExecuteNonQuery
            Dim t1 As DateTime = Now
            Dim exeTime As Double = (t1 - t).TotalMilliseconds
            If (exeTime >= 1000 AndAlso Sistema.ApplicationContext IsNot Nothing) Then
                Me.WarnQuery(Me.m_cmd.CommandText, exeTime)
            End If
            Return ret
        End Function

        Public Function ExecuteReader() As IDataReader Implements IDbCommand.ExecuteReader
            Dim t As DateTime = Now
            Dim ret As New DMDDBReader(Me.Conn, Me.m_cmd, Me.m_cmd.CommandText)
            ret.Create()
            Dim t1 As DateTime = Now
            Dim exeTime As Double = (t1 - t).TotalMilliseconds
            If (exeTime >= 1000 AndAlso Sistema.ApplicationContext IsNot Nothing) Then
                Me.WarnQuery(Me.m_cmd.CommandText, exeTime)
            End If
            Return ret
        End Function

        Public Function ExecuteReader(behavior As CommandBehavior) As IDataReader Implements IDbCommand.ExecuteReader
            Dim t As Date = Now
            Dim ret As New DMDDBReader(Me.Conn, Me.m_cmd, Me.m_cmd.CommandText)
            ret.Create(behavior)
            Dim t1 As Date = Now
            Dim exeTime As Double = (t1 - t).TotalMilliseconds
            If (exeTime >= 1000 AndAlso Sistema.ApplicationContext IsNot Nothing) Then
                Me.WarnQuery(Me.m_cmd.CommandText, exeTime)
            End If
            Return ret
        End Function

        Public Function ExecuteScalar() As Object Implements IDbCommand.ExecuteScalar
            Dim t As Date = Now
            Dim ret As Object = Me.m_cmd.ExecuteScalar
            Dim t1 As DateTime = Now
            Dim exeTime As Double = (t1 - t).TotalMilliseconds
            If (exeTime >= 1000 AndAlso Sistema.ApplicationContext IsNot Nothing) Then
                Me.WarnQuery(Me.m_cmd.CommandText, exeTime)
            End If
            Return ret
        End Function


        ' Questo codice viene aggiunto da Visual Basic per implementare in modo corretto il criterio Disposable.
        Public Overrides Sub Dispose() 'Implements IDisposable.Dispose
            If (Me.m_cmd IsNot Nothing) Then Me.m_cmd.Dispose()
            Me.m_cmd = Nothing
            MyBase.Dispose()
        End Sub

        Public Overrides Function ToString() As String
            Dim ret As New System.Text.StringBuilder
            If (Me.m_cmd IsNot Nothing) Then
                ret.Append(" - ")
                ret.Append(Me.m_cmd.CommandText)
            End If
            Return ret.ToString
        End Function

    End Class

    Public Class DMDDBReader
        Inherits DMDDBCObj
        Implements System.Data.IDataReader

        Private m_Text As String
        Public m_Cmd As System.Data.IDbCommand
        Public m_Reader As System.Data.IDataReader

        Public Sub New(ByVal conn As CDBConnection, ByVal cmd As System.Data.IDbCommand, ByVal text As String)
            MyBase.New(conn)
            Me.m_Cmd = cmd
            Me.m_Text = text
        End Sub

        Public Overrides Sub Create()
            Me.m_Reader = Me.m_Cmd.ExecuteReader
        End Sub

        Public Overloads Sub Create(ByVal b As CommandBehavior)
            Me.m_Reader = Me.m_Cmd.ExecuteReader(b)
        End Sub

        Public ReadOnly Property Depth As Integer Implements IDataReader.Depth
            Get
                Return Me.m_Reader.Depth
            End Get
        End Property

        Public ReadOnly Property FieldCount As Integer Implements IDataRecord.FieldCount
            Get
                Return Me.m_Reader.FieldCount
            End Get
        End Property

        Public ReadOnly Property IsClosed As Boolean Implements IDataReader.IsClosed
            Get
                Return Me.m_Reader.IsClosed
            End Get
        End Property

        Default Public ReadOnly Property Item(name As String) As Object Implements IDataRecord.Item
            Get
                Return Me.m_Reader.Item(name)
            End Get
        End Property

        Default Public ReadOnly Property Item(i As Integer) As Object Implements IDataRecord.Item
            Get
                Return Me.m_Reader.Item(i)
            End Get
        End Property

        Public ReadOnly Property RecordsAffected As Integer Implements IDataReader.RecordsAffected
            Get
                Return Me.m_Reader.RecordsAffected
            End Get
        End Property

        Public Sub Close() Implements IDataReader.Close
            If (Me.m_Reader IsNot Nothing) Then
                Me.m_Reader.Close()
                Me.m_Reader.Dispose()
            End If
            Me.m_Reader = Nothing
        End Sub

        Public Function GetBoolean(i As Integer) As Boolean Implements IDataRecord.GetBoolean
            Return Me.m_Reader.GetBoolean(i)
        End Function

        Public Function GetByte(i As Integer) As Byte Implements IDataRecord.GetByte
            Return Me.m_Reader.GetByte(i)
        End Function

        Public Function GetBytes(i As Integer, fieldOffset As Long, buffer() As Byte, bufferoffset As Integer, length As Integer) As Long Implements IDataRecord.GetBytes
            Return Me.m_Reader.GetBytes(i, fieldOffset, buffer, bufferoffset, length)
        End Function

        Public Function GetChar(i As Integer) As Char Implements IDataRecord.GetChar
            Return Me.m_Reader.GetChar(i)
        End Function

        Public Function GetChars(i As Integer, fieldoffset As Long, buffer() As Char, bufferoffset As Integer, length As Integer) As Long Implements IDataRecord.GetChars
            Return Me.m_Reader.GetChars(i, fieldoffset, buffer, bufferoffset, length)
        End Function

        Public Function GetData(i As Integer) As IDataReader Implements IDataRecord.GetData
            Return Me.m_Reader.GetData(i)
        End Function

        Public Function GetDataTypeName(i As Integer) As String Implements IDataRecord.GetDataTypeName
            Return Me.m_Reader.GetDataTypeName(i)
        End Function

        Public Function GetDateTime(i As Integer) As Date Implements IDataRecord.GetDateTime
            Return Me.m_Reader.GetDateTime(i)
        End Function

        Public Function GetDecimal(i As Integer) As Decimal Implements IDataRecord.GetDecimal
            Return Me.m_Reader.GetDecimal(i)
        End Function

        Public Function GetDouble(i As Integer) As Double Implements IDataRecord.GetDouble
            Return Me.m_Reader.GetDouble(i)
        End Function

        Public Function GetFieldType(i As Integer) As Type Implements IDataRecord.GetFieldType
            Return Me.m_Reader.GetFieldType(i)
        End Function

        Public Function GetFloat(i As Integer) As Single Implements IDataRecord.GetFloat
            Return Me.m_Reader.GetFloat(i)
        End Function

        Public Function GetGuid(i As Integer) As Guid Implements IDataRecord.GetGuid
            Return Me.m_Reader.GetGuid(i)
        End Function

        Public Function GetInt16(i As Integer) As Short Implements IDataRecord.GetInt16
            Return Me.m_Reader.GetInt16(i)
        End Function

        Public Function GetInt32(i As Integer) As Integer Implements IDataRecord.GetInt32
            Return Me.m_Reader.GetInt32(i)
        End Function

        Public Function GetInt64(i As Integer) As Long Implements IDataRecord.GetInt64
            Return Me.m_Reader.GetInt64(i)
        End Function

        Public Function GetName(i As Integer) As String Implements IDataRecord.GetName
            Return Me.m_Reader.GetName(i)
        End Function

        Public Function GetOrdinal(name As String) As Integer Implements IDataRecord.GetOrdinal
            Return Me.m_Reader.GetOrdinal(name)
        End Function

        Public Function GetSchemaTable() As DataTable Implements IDataReader.GetSchemaTable
            Return Me.m_Reader.GetSchemaTable
        End Function

        Public Function GetString(i As Integer) As String Implements IDataRecord.GetString
            Return Me.m_Reader.GetString(i)
        End Function

        Public Function GetValue(i As Integer) As Object Implements IDataRecord.GetValue
            Return Me.m_Reader.GetValue(i)
        End Function

        Public Function GetValues(values() As Object) As Integer Implements IDataRecord.GetValues
            Return Me.m_Reader.GetValues(values)
        End Function

        Public Function IsDBNull(i As Integer) As Boolean Implements IDataRecord.IsDBNull
            Return Me.m_Reader.IsDBNull(i)
        End Function

        Public Function NextResult() As Boolean Implements IDataReader.NextResult
            Return Me.m_Reader.NextResult
        End Function

        Public Function Read() As Boolean Implements IDataReader.Read
            Return Me.m_Reader.Read
        End Function


        Public Overrides Sub Dispose() 'Implements IDisposable.Dispose
            If (Me.m_Reader IsNot Nothing) Then Me.m_Reader.Dispose()
            Me.m_Reader = Nothing
            MyBase.Dispose()
        End Sub

        Public Overrides Function ToString() As String
            Dim ret As New System.Text.StringBuilder
            If (Me.m_Text <> "") Then
                ret.Append(" - ")
                ret.Append(Me.m_Text)
            End If
            Return ret.ToString
        End Function

    End Class

    'Public Class DMDDBAdapter
    '    Inherits DMDDBCObj
    '    Implements System.Data.IDataAdapter

    '    Private m_Text As String
    '    Private m_Conn As CDBConnection
    '    Public m_A As System.Data.IDataAdapter

    '    Public Sub New(ByVal text As String, ByVal conn As CDBConnection)
    '        Me.m_Text = text
    '        Me.m_Conn = conn
    '    End Sub

    '    Public Sub Create()
    '        Me.m_A = Me.m_Conn.CreateAdapterInternal(Me.m_Text)
    '    End Sub

    '    Public Property MissingMappingAction As MissingMappingAction Implements IDataAdapter.MissingMappingAction
    '        Get
    '            Return Me.m_A.MissingMappingAction
    '        End Get
    '        Set(value As MissingMappingAction)
    '            Me.m_A.MissingMappingAction = value
    '        End Set
    '    End Property

    '    Public Property MissingSchemaAction As MissingSchemaAction Implements IDataAdapter.MissingSchemaAction
    '        Get
    '            Return Me.m_A.MissingSchemaAction
    '        End Get
    '        Set(value As MissingSchemaAction)
    '            Me.m_A.MissingSchemaAction = value
    '        End Set
    '    End Property

    '    Public ReadOnly Property TableMappings As ITableMappingCollection Implements IDataAdapter.TableMappings
    '        Get
    '            Return Me.m_A.TableMappings
    '        End Get
    '    End Property

    '    Public Overrides Function ToString() As String
    '        Dim ret As New System.Text.StringBuilder
    '        ret.Append(Me.UserName)
    '        If (Me.m_Text <> "") Then
    '            ret.Append(" - ")
    '            ret.Append(Me.m_Text)
    '        End If
    '        Return ret.ToString
    '    End Function

    '    Public Function FillSchema(dataSet As DataSet, schemaType As SchemaType) As DataTable() Implements IDataAdapter.FillSchema
    '        Return Me.m_A.FillSchema(dataSet, schemaType)
    '    End Function

    '    Public Function Fill(dataSet As DataSet) As Integer Implements IDataAdapter.Fill
    '        Return Me.m_A.Fill(dataSet)
    '    End Function

    '    Public Function GetFillParameters() As IDataParameter() Implements IDataAdapter.GetFillParameters
    '        Return Me.m_A.GetFillParameters
    '    End Function

    '    Public Function Update(dataSet As DataSet) As Integer Implements IDataAdapter.Update
    '        Return Me.m_A.Update(dataSet)
    '    End Function

    '    Public Overrides Sub Dispose()
    '        Me.m_A = Nothing
    '        Me.m_Conn = Nothing
    '        MyBase.Dispose()
    '    End Sub
    'End Class

    ''' <summary>
    ''' Connessione generica ad un database
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class CDBConnection

        Implements IDisposable, DMD.XML.IDMDXMLSerializable

        Private Const QUERYMAXEXECTIMELOG As Integer = 250
        Private Shared queryCNT As Integer = 0

        Private Shared Function GetUniqueQueryID() As Integer
            If (queryCNT = Integer.MaxValue) Then
                queryCNT += Integer.MinValue
            Else
                queryCNT += 1
            End If
            Return queryCNT
        End Function


        Private Const MAXDEBUGITEMS As Integer = 500

        Public Event ConnectionOpened(ByVal sender As Object, ByVal e As System.EventArgs)
        Public Event ConnectionClosed(ByVal sender As Object, ByVal e As System.EventArgs)
        Public Event ConnectionError(ByVal sender As Object, ByVal e As System.EventArgs)
        Public Event QueryBegin(ByVal sender As Object, ByVal e As DBQueryEventArgs)
        Public Event QueryCompleted(ByVal sender As Object, ByVal e As DBQueryCompletedEventArgs)

        Private m_Path As String
        Private m_UserName As String
        Private m_Password As String
        Private m_dbConn As System.Data.IDbConnection
        Private m_Tables As CDBTablesCollection
        Private m_Queries As CDBQueriesCollection
        Private ReadOnly m_DBLock As New Object

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Path = ""
            Me.m_dbConn = Nothing
        End Sub

        Protected Friend MustOverride Sub CreateInternal()


        ''' <summary>
        ''' Crea il database
        ''' </summary>
        Public Sub Create()
            Me.CreateInternal()
        End Sub

        'Public Class CTimesComparer
        '    Implements IComparer


        '    Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
        '        Dim t1 As KeyValuePair(Of String, Double) = x
        '        Dim t2 As KeyValuePair(Of String, Double) = y
        '        If t2.Value < t1.Value Then
        '            Return -1
        '        ElseIf t2.Value > t1.Value Then
        '            Return 1
        '        Else
        '            Return 0
        '        End If
        '    End Function
        'End Class

        Protected Friend MustOverride Function CreateAdapterInternal(ByVal selectCommand As String) As System.Data.IDataAdapter

        Public Function CreateAdapter(ByVal selectCommand As String) As System.Data.IDataAdapter
            'Dim ret As New DMDDBAdapter(selectCommand, Me)
            'ret.Create
            'Return ret
            Return Me.CreateAdapterInternal(selectCommand)
        End Function

        Public MustOverride Function CreateConnection() As System.Data.IDbConnection

        Public Function CreateCommand(ByVal sql As String) As System.Data.IDbCommand
            Dim ret As New DMDDBCommand(Me, sql)
            ret.Create()
            Return ret
        End Function

        Protected Friend MustOverride Function CreateCommandInternal(ByVal sql As String) As System.Data.IDbCommand

        Public MustOverride Function GetUpdateCommand(ByVal obj As Object, ByVal idFieldName As String, ByVal idValue As Integer, ByVal dr As System.Data.DataRow, ByVal changedValues As CKeyCollection(Of Boolean)) As System.Data.IDbCommand
        Public MustOverride Function GetInsertCommand(ByVal obj As Object, ByVal idFieldName As String, ByVal dr As System.Data.DataRow, Optional ByRef maxID As Integer = 0) As System.Data.IDbCommand
        Public MustOverride Function GetSqlDataType(ByVal field As CDBEntityField) As String
        Protected Friend MustOverride Sub CreateTable(ByVal table As CDBTable)
        Protected Friend MustOverride Sub UpdateTable(ByVal table As CDBTable)
        Protected Friend MustOverride Sub DropTable(ByVal table As CDBTable)
        Protected Friend MustOverride Sub CreateView(ByVal view As CDBQuery)
        Protected Friend MustOverride Sub UpdateView(ByVal vuew As CDBQuery)
        Protected Friend MustOverride Sub DropView(ByVal view As CDBQuery)
        Protected Friend MustOverride Sub CreateField(ByVal field As CDBEntityField)
        Protected Friend MustOverride Sub DropField(ByVal field As CDBEntityField)
        Protected Friend MustOverride Sub UpdateField(ByVal field As CDBEntityField)
        Protected Friend MustOverride Sub CreateFieldConstraint(ByVal c As CDBFieldConstraint)
        Protected Friend MustOverride Sub UpdateFieldConstraint(ByVal c As CDBFieldConstraint)
        Protected Friend MustOverride Sub DropFieldConstraint(ByVal c As CDBFieldConstraint)
        Protected Friend MustOverride Sub CreateTableConstraint(ByVal c As CDBTableConstraint)
        Protected Friend MustOverride Sub UpdateTableConstraint(ByVal c As CDBTableConstraint)
        Protected Friend MustOverride Sub DropTableConstraint(ByVal c As CDBTableConstraint)
        ''' <summary>
        ''' Restituisce o imposta la stringa che identifica la risorsa database
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property Path As String
            Get
                Return Me.m_Path
            End Get
            Set(value As String)
                value = Trim(value)
                If (Me.m_Path = value) Then Exit Property
                Me.m_Path = value
            End Set
        End Property

        Public Sub SetCredentials(ByVal userName As String, ByVal password As String)
            Me.m_UserName = userName
            Me.m_Password = password
        End Sub

        ''' <summary>
        ''' Restituisce la collezione delle tabelle contenute nel database
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Tables As CDBTablesCollection
            Get
                SyncLock Me.m_DBLock
                    If (Me.m_Tables Is Nothing) Then
                        If (Me.IsOpen = False) Then Throw New DBException("Il database non è aperto: " & Me.Path)

                        Me.m_Tables = New CDBTablesCollection(Me)
                        Dim arr() As CDBTable = Me.GetTablesArray
                        For i As Integer = 0 To Arrays.Len(arr) - 1
                            Dim item As CDBTable = arr(i)
                            Me.m_Tables.Add(item)
                            item.SetChanged(False)
                            item.SetCreated(True)
                            item.SetConnection(Me)
                        Next
                    End If
                    Return Me.m_Tables
                End SyncLock
            End Get
        End Property

        Protected MustOverride Function GetTablesArray() As CDBTable()

        ''' <summary>
        ''' Restituisce la collezione delle queries contenute nel database
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Queries As CDBQueriesCollection
            Get
                SyncLock Me.m_DBLock
                    If (Me.IsOpen = False) Then Throw New DBException("Il database non è aperto:  " & Me.Path)

                    If (Me.m_Queries Is Nothing) Then Me.m_Queries = New CDBQueriesCollection(Me)
                    Return Me.m_Queries
                End SyncLock
            End Get
        End Property

        Public Overridable Function ToDB(Of T)(ByVal value As T) As Object
            If value Is Nothing Then Return DBNull.Value
            If value Is Nothing Then Return DBNull.Value
            Return value
        End Function

        Public Overridable Function ToDB(ByVal value As Object) As Object
            If TypeOf (value) Is DBNull Then Return DBNull.Value
            If value Is Nothing Then Return DBNull.Value
            Return value
        End Function


        'adStateClosed      0           The object is closed
        'adStateOpen        1           The object is open
        'adStateConnecting  2       The object is connecting
        'adStateExecuting   4       The object is executing a command
        'adStateFetching    8       The rows of the object are being retrieved

        ''' <summary>
        ''' Restituisce vero se la connessione è aperta
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsOpen() As Boolean
            If (Me.m_dbConn Is Nothing) Then Return False
            Return (Me.m_dbConn.State <> 0)
        End Function

        ''' <summary>
        ''' Apre la connessione
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function OpenDB() As Boolean
            SyncLock Me.m_DBLock
                If (Me.IsOpen) Then Throw New DBException("Il database è già aperto: " & Me.Path)
                If Me.m_dbConn Is Nothing Then Me.m_dbConn = Me.CreateConnection
                Me.m_dbConn.Open()
                Dim e As New DBEventArgs(Me)
                Me.OnConnectionOpened(e)
                DBUtils.doConnectionOpened(e)
                Return True
            End SyncLock
        End Function

        Protected Overridable Sub OnConnectionOpened(ByVal e As System.EventArgs)
            RaiseEvent ConnectionOpened(Me, e)
        End Sub

        ''' <summary>
        ''' Chiude la connessione
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CloseDB() As Boolean
            SyncLock Me.m_DBLock
                If (Me.IsOpen = False) Then Throw New DBException("Il database non è aperto: " & Me.Path)
                Me.m_dbConn.Close()
                Me.m_dbConn = Nothing
                Me.m_Tables = Nothing
                Me.m_Queries = Nothing
                Dim e As New DBEventArgs(Me)
                Me.OnConnectionClosed(e)
                DBUtils.doConnectionClosed(e)
                Return True
            End SyncLock
        End Function

        Protected Overridable Sub OnConnectionClosed(ByVal e As System.EventArgs)
            RaiseEvent ConnectionClosed(Me, e)
        End Sub

        ''' <summary>
        ''' Restituisce l'oggetto IDBConnection sottostante
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetConnection() As System.Data.IDbConnection
            Return Me.m_dbConn
        End Function

        'Private Class MyParams
        '    Public Conn As CDBConnection
        '    Public SQL As String
        '    Public Result As System.Data.IDataReader
        '    Public manualResetEvent As New System.Threading.ManualResetEvent(False)

        '    Public Sub New(ByVal conn As CDBConnection, ByVal sql As String)
        '        Me.Conn = conn
        '        Me.SQL = sql
        '    End Sub
        'End Class

        '        Private Sub ThreadWork(ByVal params As MyParams)
        '            Dim dbCmd As System.Data.IDbCommand = Nothing
        '#If Not Debug Then
        '            Try
        '#End If
        '            dbCmd = params.Conn.GetConnection.CreateCommand
        '            dbCmd.CommandText = params.SQL
        '            params.Result = dbCmd.ExecuteReader
        '#If Not Debug Then
        '            Catch ex As Exception
        '                Throw New DBException("Errore nell'esecuzione di ExecuteReader: " & ex.Message, params.SQL, ex)
        '            Finally
        '#End If
        '            dbCmd.Dispose()
        '#If Not Debug Then
        '            End Try
        '#End If
        '            params.manualResetEvent.Set()
        '        End Sub


        ' ''' <summary>
        ' ''' Restituisce un data reader
        ' ''' </summary>
        ' ''' <param name="sql"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function ExecuteReader2(ByVal sql As String) As System.Data.IDataReader
        '    If (Me.IsOpen = False) Then Throw New DBException("Il DB [" & Me.Path & "] non è aperto")
        '    sql = Trim(sql)
        '    If (sql = "") Then Throw New ArgumentNullException("sql")

        '    Dim thread As New System.Threading.Thread(AddressOf ThreadWork)
        '    Dim params As New MyParams(Me, sql)
        '    thread.IsBackground = True
        '    thread.Start(params)
        '    params.manualResetEvent.WaitOne()
        '    params.manualResetEvent.Dispose()

        '    Return params.Result
        'End Function


        ''' <summary>
        ''' Restituisce un data reader
        ''' </summary>
        ''' <param name="sql"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExecuteReader(ByVal sql As String) As System.Data.IDataReader
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim dbCmd As System.Data.IDbCommand = Nothing
            Try
                If (Me.IsOpen = False) Then Throw New DBException("Il DB [" & Me.Path & "] non è aperto")
                sql = Trim(sql)

#If TRACKCOMMANDS Then
            Dim item As New StatsItem(Me.Name & " - " & sql, 1)
            item.LastRun = DateUtils.Now
            'item.User = Sistema.Users.CurrentUser
            SyncLock DBUtils.PendingQueries
                DBUtils.PendingQueries.Add(item)
            End SyncLock
#End If

                'Me.OnQueryBegin(e)


                dbCmd = Me.CreateCommand(sql)
                dbRis = dbCmd.ExecuteReader

#If TRACKCOMMANDS Then
            item.ExecTime = (DateUtils.Now - item.LastRun).TotalMilliseconds
            If (item.ExecTime < QUERYMAXEXECTIMELOG) Then
                SyncLock DBUtils.PendingQueries
                    DBUtils.PendingQueries.Remove(item)
                End SyncLock
            Else
                Debug.Print(item.ExecTime & " - " & sql)
            End If
#End If
                dbCmd.Dispose()
                dbCmd = Nothing

                Return dbRis
            Catch ex As Exception
                'Me.OnQueryCompleted(New DBQueryCompletedEventArgs(e.QueryID, e.Connection, e.SQL, e.StartTime, Now, ex))
                Try
                    Dim text As String = Formats.GetTimeStamp
                    If (Sistema.Users.CurrentUser IsNot Nothing) Then text &= " - " & Sistema.Users.CurrentUser.UserName
                    Sistema.ApplicationContext.Log(text & " - " & sql)
                Catch ex1 As Exception
                    Debug.Print(ex1.Message)
                End Try
                Throw New DBException("Errore nell'esecuzione di ExecuteReader: " & ex.Message, sql, ex)
            Finally

                If (dbCmd IsNot Nothing) Then dbCmd.Dispose() : dbCmd = Nothing
            End Try
            ' Me.OnQueryCompleted(New DBQueryCompletedEventArgs(e.QueryID, e.Connection, e.SQL, e.StartTime, Now))

            ' If (item IsNot Nothing) Then item.End()

        End Function

        Protected Overridable Sub OnQueryBegin(ByVal e As DBQueryEventArgs)
            If (DBUtils.StopStatistics) Then Exit Sub
            RaiseEvent QueryBegin(Me, e)
        End Sub

        Protected Overridable Sub OnQueryCompleted(ByVal e As DBQueryCompletedEventArgs)
            If (DBUtils.StopStatistics) Then Exit Sub
            RaiseEvent QueryCompleted(Me, e)
        End Sub

        ''' <summary>
        ''' Restituisce il valore di una query scalare
        ''' </summary>
        ''' <param name="dbSQL"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExecuteScalar(ByVal dbSQL As String) As Object
            SyncLock Me.m_DBLock
                Dim dbCmd As System.Data.IDbCommand = Nothing
                '#If Not Debug Then
                Try
                    '#End If
                    If (Me.IsOpen = False) Then Throw New DBException("Il DB [" & Me.Path & "] non è aperto")

                    dbSQL = Trim(dbSQL)

#If TRACKCOMMANDS Then
                Dim item As New StatsItem(Me.Name & " - " & dbSQL, 1)
                item.LastRun = DateUtils.Now
                'item.User = Sistema.Users.CurrentUser
                SyncLock DBUtils.PendingQueries
                    DBUtils.PendingQueries.Add(item)
                End SyncLock
#End If

                    dbCmd = Me.CreateCommand(dbSQL)

                    Dim ret As Object = dbCmd.ExecuteScalar

#If TRACKCOMMANDS Then
                item.ExecTime = (DateUtils.Now - item.LastRun).TotalMilliseconds
                If (item.ExecTime < QUERYMAXEXECTIMELOG) Then
                    SyncLock DBUtils.PendingQueries
                        DBUtils.PendingQueries.Remove(item)
                    End SyncLock
                End If
#End If

                    Return ret
                    '#If Not Debug Then
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                    Throw New DBException("Errore nell'esecuzione di ExecuteScalar: " & ex.Message, dbSQL, ex)
                Finally
                    '#End If
                    If (dbCmd IsNot Nothing) Then dbCmd.Dispose()
                    '#If Not Debug Then
                End Try
                '#End If

            End SyncLock
        End Function

        ''' <summary>
        ''' Esegue il comando
        ''' </summary>
        ''' <param name="sql"></param>
        ''' <remarks></remarks>
        Public Sub ExecuteCommand(ByVal sql As String)
            Dim cmd As System.Data.IDbCommand = Nothing
            Try
                If (Me.IsOpen = False) Then Throw New DBException("Il DB [" & Me.Path & "] non è aperto")

                sql = Trim(sql)

#If TRACKCOMMANDS Then
                Dim item As New StatsItem(Me.Name & " - " & sql, 1)
                item.LastRun = DateUtils.Now
                'item.User = Sistema.Users.CurrentUser
                SyncLock DBUtils.PendingQueries
                    DBUtils.PendingQueries.Add(item)
                End SyncLock
#End If

                cmd = Me.CreateCommand(sql)
                cmd.ExecuteNonQuery()

#If TRACKCOMMANDS Then
                item.ExecTime = (DateUtils.Now - item.LastRun).TotalMilliseconds
                If (item.ExecTime < QUERYMAXEXECTIMELOG) Then
                    SyncLock DBUtils.PendingQueries
                        DBUtils.PendingQueries.Remove(item)
                    End SyncLock
                End If
#End If
            Catch ex As Exception
                Throw New DBException("Errore nell'esecuzione di ExecuteCommand: " & ex.Message, sql, ex)
            Finally
                If (cmd IsNot Nothing) Then cmd.Dispose()
            End Try
        End Sub

        ''' <summary>
        ''' Carica l'oggetto dal recordset
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <param name="dbRis"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Load(ByVal obj As Object, ByVal dbRis As DBReader) As Boolean
            SyncLock Me.m_DBLock
                If (Me.IsOpen = False) Then Throw New DBException("Il DB [" & Me.Path & "] non è aperto")

                If TypeOf (obj) Is IDBObjectBase Then
                    Return DirectCast(obj, IDBObjectBase).LoadFromRecordset(dbRis)
                Else
                    Throw New ArgumentException("L'oggetto non implementa alcuna interfaccia valida")
                    Return False
                End If
            End SyncLock
        End Function

        Public Function Load(ByVal obj As Object, ByVal dbRis As IDataReader) As Boolean
            SyncLock Me.m_DBLock
                If (Me.IsOpen = False) Then Throw New DBException("Il DB [" & Me.Path & "] non è aperto")

                If TypeOf (obj) Is IDBObjectBase Then
                    Dim reader As New DBReader(dbRis)
                    Return DirectCast(obj, IDBObjectBase).LoadFromRecordset(reader)
                Else
                    Throw New ArgumentException("L'oggetto non implementa alcuna interfaccia valida")
                    Return False
                End If
            End SyncLock
        End Function

        Public ReadOnly Property Name As String
            Get
                Return DMD.Sistema.FileSystem.GetFileName(Me.m_Path)
            End Get
        End Property



        Public Overridable Function GetFriendlyName(ByVal name As String) As String
            Return name
        End Function

        Public Function GetSaveTableName(ByVal obj As Object) As String
            Return DirectCast(obj, IDBObjectBase).GetTableName
        End Function

        Public Overridable Function GetInternalTableName(ByVal table As CDBEntity) As String
            Return table.Name
        End Function


        'Public Function Delete(ByVal item As Object, Optional ByVal force As Boolean = False) As Boolean
        '    Return DirectCast(item, IDBMinObject).DropFromDatabase(Me, force)
        'End Function

        Public Function Drop(ByVal constraint As CDBTableConstraint) As Boolean
            SyncLock Me.m_DBLock
                If (Me.IsOpen = False) Then Throw New DBException("Il DB [" & Me.Path & "] non è aperto")

                Dim table As CDBEntity = constraint.Owner
                Me.ExecuteCommand("DROP INDEX [" & constraint.Name & "] ON [" & table.Name & "]")
                table.Constraints.Remove(constraint)
                Return True
            End SyncLock
        End Function

        ''' <summary>
        ''' Elimina una tabella dal database
        ''' </summary>
        ''' <param name="tableName"></param>
        ''' <remarks></remarks>
        Public Overridable Sub DropTable(ByVal tableName As String)
            SyncLock Me.m_DBLock
                If (Me.IsOpen = False) Then Throw New DBException("Il DB [" & Me.Path & "] non è aperto")

                tableName = Replace(Trim(tableName), "'", "''")
                Me.ExecuteCommand("DROP TABLE " & tableName & "")
                If (Me.m_Tables IsNot Nothing) Then Me.m_Tables.RemoveByKey(tableName)
            End SyncLock
        End Sub

        Public Function TableExists(ByVal tableName As String) As Boolean
            SyncLock Me.m_DBLock
                tableName = Trim(tableName)
                Return Me.Tables.ContainsKey(tableName)
            End SyncLock
        End Function


        Public Function FillDataTable(ByVal selectCommand As String) As System.Data.DataTable
            Dim da As System.Data.IDataAdapter = Nothing
            Dim ds As System.Data.DataSet = Nothing
            Dim dt As System.Data.DataTable = Nothing
            Try
                da = Me.CreateAdapter(selectCommand)
                ds = New System.Data.DataSet()
                da.Fill(ds)
                Return ds.Tables(0)
            Catch ex As Exception
                Throw
            Finally
                If (ds IsNot Nothing) Then ds.Dispose()
            End Try
        End Function

        Public Overridable Sub Dispose() Implements IDisposable.Dispose
            If (Me.IsOpen) Then Me.CloseDB()
            Me.m_dbConn = Nothing
            Me.m_Password = vbNullString
            Me.m_Path = vbNullString
            Me.m_Queries = Nothing
            Me.m_Tables = Nothing
            Me.m_UserName = vbNullString
        End Sub


        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Path" : Me.m_Path = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "UserName" : Me.m_UserName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Password" : Me.m_Password = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Tables" : Me.m_Tables = fieldValue : Me.m_Tables.SetOwner(Me)
                Case "Queries" : Me.m_Queries = fieldValue : Me.m_Queries.SetOwner(Me)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Path", Me.m_Path)
            writer.WriteAttribute("UserName", Me.m_UserName)
            writer.WriteAttribute("Password", Me.m_Password)
            'm_dbConn As System.Data.IDbConnection
            writer.WriteTag("Tables", Me.Tables)
            writer.WriteTag("Queries", Me.Queries)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Public Overridable Sub SaveObject(ByVal obj As Object, ByVal force As Boolean)
            If (obj Is Nothing) Then Throw New ArgumentNullException("obj")
            DirectCast(obj, IDBMinObject).SaveToDatabase(Me, force)
        End Sub

        Public Overridable Sub DeleteObject(ByVal obj As Object, ByVal force As Boolean)
            If (obj Is Nothing) Then Throw New ArgumentNullException("obj")
            DirectCast(obj, IDBMinObject).DropFromDatabase(Me, force)
        End Sub

        Public Overridable Function GetItemById(ByVal m As CModule, ByVal id As Integer) As Object
            Dim cursor As DBObjectCursorBase = Nothing
            If (id = 0) Then Return Nothing
            Try
                Dim h As IModuleHandler = m.CreateHandler(Nothing)
                If (h Is Nothing) Then Throw New ArgumentNullException("Module Handler")
                cursor = h.CreateCursor
                If (cursor Is Nothing) Then Throw New ArgumentNullException("cursor")
                cursor.ID.Value = id
                cursor.IgnoreRights = True
                Return cursor.Item
            Catch ex As Exception
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try

            'Dim dbRis As System.Data.IDataReader = Nothing

            'Try
            '    If (id = 0) Then Return Nothing

            '    Dim item As Object = Sistema.Types.CreateInstance(GetType(T))

            '    Dim conn As CDBConnection = DBUtils.GetConnection(item)

            '    Dim dbSQL As New System.Text.StringBuilder
            '    dbSQL.Append("SELECT * FROM [")
            '    dbSQL.Append(DBUtils.GetTableName(item))
            '    dbSQL.Append("] WHERE [ID]=")
            '    dbSQL.Append(DBUtils.DBNumber(id))

            '    dbRis = conn.ExecuteReader(dbSQL.ToString)

            '    If (dbRis.Read) Then
            '        conn.Load(item, dbRis)
            '    Else
            '        If (TypeOf (item) Is IDisposable) Then DirectCast(item, IDisposable).Dispose()
            '        item = Nothing
            '    End If

            '    Return item
            'Catch ex As Exception
            '    Sistema.Events.NotifyUnhandledException(ex)
            '    Throw
            'Finally
            '    If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            'End Try

        End Function


        Public Overridable Function IsRemote() As Boolean
            Return False
        End Function


        Public MustOverride Function InvokeMethodArray([module] As CModule, ByVal methodName As String, ByVal args() As Object) As String

        Public Function InvokeMethod([module] As CModule, ByVal methodName As String, ParamArray ByVal args() As Object) As String
            Return Me.InvokeMethodArray([module], methodName, args)
        End Function

        Public MustOverride Function InvokeMethodArrayAsync([module] As CModule, ByVal methodName As String, ByVal handler As Object, ByVal args() As Object) As AsyncState


        Public Function InvokeMethodAsync([module] As CModule, ByVal methodName As String, ByVal handler As Object, ParamArray ByVal args() As Object) As AsyncState
            Return Me.InvokeMethodArrayAsync([module], methodName, handler, args)
        End Function



    End Class


End Class


