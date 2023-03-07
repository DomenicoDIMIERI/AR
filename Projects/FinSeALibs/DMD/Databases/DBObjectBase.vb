Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization
Imports System.Data.OleDb
Imports System.Threading

Partial Public Class Databases



    <Serializable>
    Public MustInherit Class DBObjectBase
        Inherits DMDObject
        Implements IDBObjectBase, XML.IDMDXMLSerializable

        Public Event PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs)

        Private m_ID As Integer
        <NonSerialized> Private m_OldID As Integer
        <NonSerialized> Private m_Changes As New CCollection(Of PropertyChangedEventArgs)
        <NonSerialized> Private m_Changed As Boolean

        Public Sub New()
            Me.m_ID = 0
            Me.m_Changed = True
        End Sub

        ''' <summary>
        ''' Restituisce l'ID univoco che identifica l'oggetto nella tabella del database in cui è salvato
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ID As Integer Implements IDBObjectBase.ID
            Get
                Return Me.m_ID
            End Get
        End Property


        ''' <summary>
        ''' Quando sottoposto ad override in una classe derivata restituisce il modulo che gestisce l'oggetto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function GetModule() As DMD.Sistema.CModule

        ''' <summary>
        ''' Genera l'evento PropertyChanged
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Overridable Sub OnPropertyChanged(ByVal e As PropertyChangedEventArgs)
            RaiseEvent PropertyChanged(Me, e)
        End Sub

        ''' <summary>
        ''' Informa l'oggetto che la proprietà specificata è stata modificata
        ''' </summary>
        ''' <param name="propName"></param>
        ''' <param name="newVal"></param>
        ''' <param name="oldVal"></param>
        ''' <remarks></remarks>
        Protected Overridable Sub DoChanged(ByVal propName As String, Optional ByVal newVal As Object = Nothing, Optional ByVal oldVal As Object = Nothing)
            Dim e As New PropertyChangedEventArgs(propName, newVal, oldVal)
            Me.m_Changes.Add(e)
            Me.m_Changed = True
            Me.OnPropertyChanged(e)
        End Sub

        ''' <summary>
        ''' Restituisce vero se l'oggetto è stato modificato
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function IsChanged() As Boolean
            Return Me.m_Changed
        End Function

        ''' <summary>
        ''' Imposta il valore IsChanged
        ''' </summary>
        ''' <param name="value"></param>
        ''' <remarks></remarks>
        Public Sub SetChanged(ByVal value As Boolean)
            Me.m_Changed = value
            If (Not value) Then Me.m_Changes.Clear()
        End Sub

        'Public Sub Save()
        '    Me.GetConnection.Save(Me)
        'End Sub

        'Private Shared sem As New ManualResetEvent(True)

        Public Overridable Sub Save(Optional ByVal force As Boolean = False)
            Dim e As New DMD.SystemEvent
            Me.OnBeforeSave(e)
            If (force OrElse Not (Me.IsChanged() = False AndAlso Me.ID() <> 0)) Then
                'sem.WaitOne()
                'Try
                Me.GetConnection().SaveObject(Me, force)
                'Me.SaveToDatabase(Me.GetConnection, force)
                'Catch ex As Exception
                '   Throw
                'Finally
                '   sem.Set()
                'End Try
            End If
            Me.OnAfterSave(e)
        End Sub


        Protected Overridable Function SaveToDatabase(ByVal dbConn As CDBConnection, ByVal force As Boolean) As Boolean Implements IDBObjectBase.SaveToDatabase

            ' Dim lockObj As Object = Me.GetTable
            ' If (lockObj Is Nothing) Then lockObj = dbConn



            'SyncLock lockObj
            If (dbConn Is Nothing) Then Throw New ArgumentNullException("dbConn")
                If (dbConn.IsOpen = False) Then Throw New InvalidOperationException("La connessione [" & dbConn.Path & "] non è aperta")

                If ((Me.m_ID = 0) OrElse force OrElse Me.IsChanged) Then
                    Dim ds As System.Data.DataSet = Nothing
                    Dim dt As System.Data.DataTable = Nothing
                    Dim da As System.Data.IDataAdapter = Nothing
                    Dim dr As System.Data.DataRow = Nothing
                    Dim cmd As System.Data.IDbCommand = Nothing
                    Dim writer As DBWriter = Nothing
                    Dim changedValues As CKeyCollection(Of Boolean) = Nothing
                    Dim hasChanges, tmp As Boolean
                    Dim keys() As String = Nothing
                    Dim originalValues As CKeyCollection(Of Object) = Nothing

#If Not DEBUG Then
                    Try
#End If
                    Dim selectCommand As String
                    If (Me.ID = 0) Then
                        selectCommand = "SELECT * FROM " & Me.GetTable.InternalName & " WHERE (0<>0)"
                    Else
                        selectCommand = "SELECT * FROM " & Me.GetTable.InternalName & " WHERE ([" & Me.GetIDFieldName & "]=" & Me.ID & ")"
                    End If

                    da = dbConn.CreateAdapter(selectCommand)
                    ds = New System.Data.DataSet(Me.GetTableName)

                    da.Fill(ds)
                    dt = ds.Tables(0)

                    If (Me.ID = 0) Then
                        dr = dt.Rows.Add()
                    Else
                        dr = dt.Rows(0)
                        originalValues = New CKeyCollection(Of Object)
                        For i As Integer = 0 To dt.Columns.Count - 1
                            originalValues.Add(dt.Columns(i).ColumnName, dr(dt.Columns(i).ColumnName))
                        Next
                    End If

                    dt.Dispose()
                    dt = Nothing

                    ds.Dispose()
                    ds = Nothing


                    writer = New DBWriter(Me.GetTable, dr)
                    Me.SaveToRecordset(writer)

                    If (Me.ID = 0) Then
                        cmd = dbConn.GetInsertCommand(Me, Me.GetIDFieldName, dr, Me.m_ID)
                        cmd.ExecuteNonQuery()
                        Me.SetID(dbConn.ExecuteScalar("SELECT @@IDENTITY"))
                        'cmd = dbConn.GetInsertCommand(Me, Me.GetIDFieldName, dr, Me.m_ID)
                        'DirectCast(da, System.Data.OleDb.OleDbDataAdapter).InsertCommand = DirectCast(cmd, DMDDBCommand).m_cmd
                        'Dim up As New RowUpdater(dbConn, da, dr)
                        'up.Run()
                        'Me.SetID(up.ID)
                    Else
                        Debug.Print("UPDATING: " & Me.m_ID)

                        changedValues = New CKeyCollection(Of Boolean)
                        keys = originalValues.Keys
                        For i As Integer = 0 To UBound(keys)
                            tmp = Arrays.Compare(dr(keys(i)), originalValues(keys(i))) <> 0
                            hasChanges = hasChanges OrElse tmp
                            changedValues.Add(keys(i), tmp)
                        Next
                        If (hasChanges) Then
                            cmd = dbConn.GetUpdateCommand(Me, Me.GetIDFieldName, Me.ID, dr, changedValues)
                            cmd.ExecuteNonQuery()
                        End If
                    End If

                    Me.m_Changes.Clear()
#If Not DEBUG Then
                    Catch ex As Exception
                        Sistema.Events.NotifyUnhandledException(ex)
                        Throw
                    Finally
#End If
                    If (cmd IsNot Nothing) Then cmd.Dispose() : cmd = Nothing
                    If (dt IsNot Nothing) Then dt.Dispose() : dt = Nothing
                    If (ds IsNot Nothing) Then ds.Dispose() : ds = Nothing
                    If (writer IsNot Nothing) Then writer.Dispose() : writer = Nothing

                    If (keys IsNot Nothing) Then Erase keys
                    changedValues = Nothing
                    originalValues = Nothing
#If Not DEBUG Then
                    End Try
#End If

                End If

            'End SyncLock




            Return True
        End Function

        Private Class RowUpdater
            Public da As System.Data.OleDb.OleDbDataAdapter
            Public conn As CDBConnection
            Public dr As System.Data.DataRow
            Public ID As Integer
            Public sem As New ManualResetEvent(False)

            Public Sub New(ByVal conn As CDBConnection, ByVal da As System.Data.OleDb.OleDbDataAdapter, ByVal dr As System.Data.DataRow)
                Me.conn = conn
                Me.da = da
                Me.dr = dr
            End Sub

            Public Sub Run()
                AddHandler da.RowUpdated, AddressOf rowUpdated
                Me.da.Update({Me.dr})
                sem.WaitOne()
                Me.sem.Dispose()
                RemoveHandler da.RowUpdated, AddressOf rowUpdated
            End Sub

            Private Sub rowUpdated(ByVal sender As Object, ByVal e As OleDbRowUpdatedEventArgs)
                If e.Status = UpdateStatus.Continue AndAlso e.StatementType = StatementType.Insert Then
                    ' Get the Identity column value
                    Me.ID = Formats.ToInteger(Me.conn.ExecuteScalar("SELECT @@IDENTITY"))
                    e.Row("ID") = Me.ID
                    e.Row.AcceptChanges()
                    sem.Set()
                End If
            End Sub

        End Class



        Protected Overridable Function GetIDFieldName() As String
            Return "ID"
        End Function

        Protected Overridable Function LoadFromRecordset(ByVal reader As DBReader) As Boolean Implements IDBObjectBase.LoadFromRecordset
            Me.m_ID = reader.Read(Me.GetIDFieldName, Me.m_ID)
            Me.m_Changed = False
            Me.m_Changes.Clear()
            Return True
        End Function

        Protected Overridable Function SaveToRecordset(ByVal writer As DBWriter) As Boolean Implements IDBObjectBase.SaveToRecordset
            Return True
        End Function


        Protected Friend MustOverride Function GetConnection() As CDBConnection

        Public MustOverride Function GetTableName() As String Implements IDBObjectBase.GetTableName

        <NonSerialized> _
        Private m_Table As CDBTable

        Protected Overridable Function GetTable() As CDBTable
            If Me.m_Table Is Nothing Then Me.m_Table = Me.GetConnection.Tables(Me.GetTableName)
            Return Me.m_Table
        End Function


        Public Overrides Function ToString() As String
            Return TypeName(Me) & "[" & Me.ID & "]"
        End Function

        Public Overridable Sub Delete(Optional ByVal force As Boolean = False)
            Me.GetConnection.DeleteObject(Me, force)
        End Sub

        Protected Overridable Function DropFromDatabase(dbConn As CDBConnection, ByVal force As Boolean) As Boolean Implements IDBObjectBase.DropFromDatabase
            dbConn.ExecuteCommand("DELETE * FROM [" & Me.GetTableName & "] WHERE [" & Me.GetIDFieldName & "]=" & Me.ID)
            Me.OnDelete(New DMD.SystemEvent)
            Return True
        End Function

        Protected Overridable Sub ResetID() Implements IDBObjectBase.ResetID
            Me.m_ID = 0
        End Sub

        Protected Overridable Sub SetID(newID As Integer) Implements IDBObjectBase.SetID
            '#If DEBUG Then
            '            If (newID <> 0 AndAlso Me.GetConnection IsNot Nothing AndAlso Me.GetConnection.IsOpen) Then
            '                Dim dbRis As System.Data.IDataReader = Me.GetConnection.ExecuteReader("SELECT * FROM [" & Me.GetTableName & "] WHERE [" & Me.GetIDFieldName & "]=" & newID)
            '                If (dbRis.Read = False) Then
            '                    Debug.Print("OPPS")
            '                End If
            '                dbRis.Dispose()
            '            End If
            '#End If

            Me.m_ID = newID
        End Sub

        Protected Overridable Sub OnCreate(ByVal e As DMD.SystemEvent)

        End Sub

        Protected Overridable Sub OnDelete(ByVal e As DMD.SystemEvent)

        End Sub

        Protected Overridable Sub OnModified(ByVal e As DMD.SystemEvent)

        End Sub

        Protected Overridable Sub OnBeforeSave(ByVal e As DMD.SystemEvent)

        End Sub

        Protected Overridable Sub OnAfterSave(ByVal e As DMD.SystemEvent)
            If Me.m_OldID = 0 And Me.m_ID <> 0 Then
                Me.OnCreate(e)
            Else
                Me.OnModified(e)
            End If
        End Sub

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "m_ID" : Me.SetID(XML.Utils.Serializer.DeserializeInteger(fieldValue))
                Case "m_Changed" : Me.m_Changed = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "Changes"
                    Me.m_Changes.Clear()
                    If (TypeOf (fieldValue) Is IEnumerable) Then
                        Me.m_Changes.AddRange(fieldValue)
                    End If
                Case Else 'Throw New ArgumentOutOfRangeException(fieldName)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("m_ID", Me.m_ID)
            writer.WriteAttribute("m_Changed", Me.m_Changed)
            ' writer.WriteTag("Changes", Me.m_Changes)
        End Sub

        Public Overrides Function Equals(obj As Object) As Boolean
            'Return MyBase.Equals(obj)
            Dim t1 As System.Type = Me.GetType
            Dim t2 As System.Type = obj.GetType
            Return (t1 Is t2) AndAlso (GetID(Me) = GetID(obj))
        End Function

        Public Shared Operator =(ByVal a As DBObjectBase, ByVal b As DBObjectBase) As Boolean
            If (a Is Nothing) Then
                If (b Is Nothing) Then
                    Return True
                Else
                    Return False
                End If
            Else
                If (b Is Nothing) Then
                    Return False
                Else
                    Return a.Equals(b)
                End If
            End If
        End Operator

        Public Shared Operator <>(ByVal a As DBObjectBase, ByVal b As DBObjectBase) As Boolean
            If (a Is Nothing) Then
                If (b Is Nothing) Then
                    Return False
                Else
                    Return True
                End If
            Else
                If (b Is Nothing) Then
                    Return True
                Else
                    Return Not a.Equals(b)
                End If
            End If
        End Operator

        ''' <summary>
        ''' Copia tutti i valori delle proprietà compresi quelle definite nelle gerarchie sottostanti
        ''' </summary>
        ''' <param name="value"></param>
        ''' <remarks></remarks>
        Public Overrides Sub InitializeFrom(value As Object)
            MyBase.InitializeFrom(value)
        End Sub

        ''' <summary>
        ''' Copia tutti i valori delle proprietà tranne quelli definiti a partire da questo oggetto in poi
        ''' </summary>
        ''' <param name="value"></param>
        ''' <remarks></remarks>
        Public Overrides Sub CopyFrom(value As Object)
            Dim f1() As System.Reflection.FieldInfo = Types.GetAllFields(value.GetType)
            Dim f2() As System.Reflection.FieldInfo = Types.GetAllFields(GetType(DBObject))
            Dim f3() As System.Reflection.FieldInfo = Types.GetAllFields(GetType(DBObjectBase))
            For Each f As System.Reflection.FieldInfo In f1
                If Not f.IsInitOnly AndAlso Arrays.IndexOf(f2, f) < 0 AndAlso Arrays.IndexOf(f3, f) < 0 Then
                    f.SetValue(Me, f.GetValue(value))
                End If
            Next
        End Sub

        Friend Function IsFieldChanged(ByVal fieldName As String) As Boolean
            For i As Integer = 0 To Me.m_Changes.Count - 1
                Dim p As PropertyChangedEventArgs = Me.m_Changes(i)
                If (p.PropertyName = fieldName) Then Return True
            Next
            Return False
        End Function

        Friend Function GetOriginalFieldValue(fieldName As String) As Object
            For i As Integer = 0 To Me.m_Changes.Count - 1
                Dim p As PropertyChangedEventArgs = Me.m_Changes(i)
                If (p.PropertyName = fieldName) Then Return p.OldValue
            Next
            Return Nothing
        End Function
    End Class

End Class

