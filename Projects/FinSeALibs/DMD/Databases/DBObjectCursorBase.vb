Imports DMD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports System.Xml.Serialization

Public partial class Databases

    Public Enum DBCursorStatus As Integer
        CLOSED = 0
        NORMAL = 1
        MODIFIED = 2
        DELETED = 3
        NEWROW = 4
    End Enum

    Public MustInherit Class DBObjectCursorBase
        Implements IComparer, IDisposable, XML.IDMDXMLSerializable

        Private m_Token As String
        Private m_PageNum As Integer
        Private m_Index As Integer
        Private m_ID As CCursorField(Of Integer)
        Private m_Module As CModule
        Private m_NumItems As Integer
        Private m_IgnoreRights As Boolean
        Private WithEvents m_dbConn As CDBConnection
        Private m_reader As DBReader
        Private m_Items() As Object
        Private m_Item As Object
        Private m_Fields As CCursorFieldsCollection
        Private m_CursorStatus As DBCursorStatus
        Private m_PageSize As Integer
        Private m_WhereClauses As New CCollection(Of String)
        Private m_WhereFields As CKeyCollection(Of CCursorField)
        Private m_SortFields As CKeyCollection(Of CCursorField)
        Private m_Async As Boolean

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)

            Me.m_Token = ""
            Me.m_ID = New CCursorField(Of Integer)(Me.GetIDFieldName)
            Me.m_PageNum = -1
            Me.m_Index = -1
            Me.m_dbConn = Nothing
            'Me.m_dbRis = Nothing
            Me.m_reader = Nothing
            Me.m_IgnoreRights = False
            Me.m_NumItems = -1
            Me.m_Fields = Nothing
            Me.m_CursorStatus = 0
            Me.m_PageSize = DBCURSORPAGESIZE
            ReDim m_Items(Me.m_PageSize - 1)
            Me.m_Module = Nothing
            'Me.m_Init = False
            Me.m_Item = Nothing
            Me.m_CursorStatus = DBCursorStatus.CLOSED
            Me.m_WhereFields = Nothing
            Me.m_SortFields = Nothing
            Me.m_Async = False
        End Sub

        Protected MustOverride Function GetModule() As CModule

        Public ReadOnly Property [Module] As CModule
            Get
                If (Me.m_Module Is Nothing) Then Me.m_Module = Me.GetModule
                Return Me.m_Module
            End Get
        End Property

        Public ReadOnly Property WhereClauses As CCollection(Of String)
            Get
                If Me.m_WhereClauses Is Nothing Then Me.m_WhereClauses = New CCollection(Of String)
                Return Me.m_WhereClauses
            End Get
        End Property

        Public ReadOnly Property Fields As CCursorFieldsCollection
            Get
                If (Me.m_Fields Is Nothing) Then Me.m_Fields = Me.GetCursorFields
                Return Me.m_Fields
            End Get
        End Property

        ''' <summary>
        ''' Elimina tutti i vincoli impostati per i campi e per le clausole where poi resetta il cursore
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Clear()
            For Each field As CCursorField In Me.Fields
                field.Clear()
            Next
            Me.WhereClauses.Clear()
            Me.Reset1()
        End Sub

        ''' <summary>
        ''' Restituisce o impostau na stringa che identifica univocamente il cursore
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Token As String
            Get
                Return Me.m_Token
            End Get
            Set(value As String)
                value = Trim(value)
                Me.m_Token = value
            End Set
        End Property

        Protected Overridable Function GetIDFieldName() As String
            Return "ID"
        End Function

        Public Property IgnoreRights As Boolean
            Get
                Return Me.m_IgnoreRights
            End Get
            Set(value As Boolean)
                Me.m_IgnoreRights = value
            End Set
        End Property

        Public ReadOnly Property ID As CCursorField(Of Integer)
            Get
                Return Me.m_ID
            End Get
        End Property

        Public ReadOnly Property Connection As CDBConnection
            Get
                If Me.m_dbConn Is Nothing Then Me.m_dbConn = Me.GetConnection
                Return Me.m_dbConn
            End Get
        End Property

        ''' <summary>
        ''' Restituisce la connessione al database predefinito per questo tipo di oggetto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Friend MustOverride Function GetConnection() As CDBConnection


        Public Overridable Function Open() As Boolean
            Dim conn As CDBConnection = Me.Connection
            If (conn.IsRemote) Then
                If (Me.m_Token <> "") Then Throw New InvalidOperationException("Cursore già inizializzato")
                Dim params As String = XML.Utils.Serializer.Serialize(Me)
                Dim tn As String = Types.vbTypeName(Me)
                If (Me.m_Async) Then
                    RPC.InvokeMethodAsync("/websvc/?_m=Sistema&_a=CreateNewCursor", Me, "tn", RPC.str2n(tn), "params", RPC.str2n(params))
                Else
                    Dim ret As String = RPC.InvokeMethod("/websvc/?_m=Sistema&_a=CreateNewCursor", "tn", RPC.str2n(tn), "params", RPC.str2n(params))
                    Me.doInitComplete(ret)
                End If
                Return True
            Else
                If (Me.m_CursorStatus <> DBCursorStatus.CLOSED) Then Throw New InvalidOperationException("Stato del cursore non valido")
#If Not DEBUG Then
            Try
#End If
                If (Me.GetDBSchema IsNot Nothing) Then
                    Dim strUnindexed As String = ""
                    For Each f As CCursorField In Me.GetWhereFields
                        If f.IsSet AndAlso Not f.IsExpression(f.FieldName) Then
                            Dim c As CDBEntityField = Me.GetDBSchema.Fields.GetItemByKey(f.FieldName)
                            Dim isIndexed As Boolean = False
                            If (c IsNot Nothing) Then
                                For Each s As CDBTableConstraint In Me.GetDBSchema.Constraints
                                    For Each ts As CDBFieldConstraint In s.Columns
                                        If ts.Column IsNot Nothing AndAlso ts.Column.Name = f.FieldName Then
                                            isIndexed = True
                                            Exit For
                                        End If
                                    Next
                                    If (isIndexed) Then Exit For
                                Next

                            End If

                            If Not isIndexed Then strUnindexed = Strings.Combine(strUnindexed, f.FieldName, ", ")
                        End If
                    Next
                    If (strUnindexed <> "") Then
                        Dim str As String = Formats.GetTimeStamp & " - Query su campo non indicizzato: " & Sistema.vbTypeName(Me) & " [" & Me.GetDBSchema.Name & "] " & strUnindexed
                        Sistema.ApplicationContext.Log(str)
                    End If
                End If
                Me.m_PageNum = 0
                Me.m_Index = 0
                Dim dbSQL As String = Me.GetSQL
                Me.m_NumItems = Me.CountRecords(dbSQL)
                dbSQL = Me.GetFullSQL
                Me.m_reader = New DBReader(Me.GetDBSchema(), dbSQL)
                Me.m_CursorStatus = DBCursorStatus.NORMAL
                ReDim m_Items(Me.m_PageSize - 1)

                Me.LoadNextPage()
#If Not DEBUG Then
            Catch ex As Exception
                Me.m_NumItems = -1
                Me.m_PageNum = 0
                Me.m_Index = -1
                If (Me.m_reader IsNot Nothing) Then Me.m_reader.Dispose()
                Me.m_reader = Nothing
                Me.m_CursorStatus = DBCursorStatus.CLOSED
                For i As Integer = 0 To UBound(Me.m_Items)
                    Me.m_Items(i) = Nothing
                Next

                Throw
            End Try
#End If

                Dim e As New DBCursorEventArgs(Me)
                DBUtils.doCursorOpened(e)

                Return True
            End If
        End Function


        Protected Overridable Sub doInitComplete(ByVal ret As String)
            If (ret = "") Then Throw New Exception("Cursor Token error")
            Dim tn As String = Types.vbTypeName(Me)
            Dim tmp As DBObjectCursorBase = XML.Utils.Serializer.Deserialize(ret, tn)
            Me.m_Token = tmp.m_Token
            Me.m_Index = tmp.m_Index
            Me.m_Items = tmp.m_Items
            Me.m_NumItems = tmp.m_NumItems
            Me.m_PageNum = tmp.m_PageNum
            Me.m_CursorStatus = tmp.m_CursorStatus
            Me.OnOpen(Me)
        End Sub

        Protected Overridable Sub OnOpen(ByVal cursor As DBObjectCursorBase)


        End Sub

        Private Function CountRecords(ByVal sql As String) As Integer
            'Me.m_NumItems = Me.m_reader.CountResults  '0 Me.Connection.ExecuteScalar("SELECT @@ROWCOUNT")
            Dim j As Integer = InStr(sql, " from ", CompareMethod.Text)
            Dim cntSQL As String = Strings.JoinW("SELECT Count(*) FROM ", Mid(sql, j + 6))
            Return Formats.ToInteger(Me.Connection.ExecuteScalar(cntSQL), 0)
        End Function

        Public Function GetDBSchema() As CDBEntity
            Dim entity As CDBEntity = Me.GetConnection.Tables.GetItemByKey(Me.GetTableName)
            If (entity Is Nothing) Then entity = Me.GetConnection.Queries.GetItemByKey(Me.GetTableName)
            Return entity
        End Function

        Public ReadOnly Property CursorStatus() As DBCursorStatus
            Get
                Return Me.m_CursorStatus
            End Get
        End Property

        Public Property PageSize As Integer
            Get
                Return Me.m_PageSize
            End Get
            Set(value As Integer)
                Me.m_PageSize = value
                ReDim Preserve m_Items(Me.m_PageSize - 1)
            End Set
        End Property

        Public Sub Reset1()
            Dim conn As CDBConnection = Me.GetConnection
            If (conn.IsRemote) Then
                Dim tn As String = Types.vbTypeName(Me)
                If (Me.m_Token <> "") Then
                    Dim ret As String = RPC.InvokeMethod("/websvc/?_m=Sistema&_a=DestroyCursor", "tn", RPC.str2n(tn), "tk", RPC.str2n(Me.m_Token))
                    'var ret = RPC.invokeMethod("/websvc/?_m=Sistema&_a=ResetCursor", "tn", RPC.str2n(tn), "tk", RPC.str2n(this.m_Token));
                End If
                Me.m_CursorStatus = DBCursorStatus.CLOSED
                Me.m_Token = ""
                Me.m_Index = -1
                ReDim Me.m_Items(Me.m_PageSize - 1)
                Me.m_NumItems = -1
                Me.m_PageNum = -1
            Else
                If (Me.CursorStatus = DBCursorStatus.CLOSED) Then Exit Sub
                If (Me.m_reader IsNot Nothing) Then Me.m_reader.Dispose()
                Me.m_reader = Nothing
                Me.m_NumItems = -1
                Me.m_PageNum = -1
                Me.m_Index = -1
                Me.m_CursorStatus = DBCursorStatus.CLOSED
                Me.m_Item = Nothing
                For i As Integer = 0 To UBound(Me.m_Items)
                    Me.m_Items(i) = Nothing
                Next
                '            Me.m_Init = False

                Dim e As New DBCursorEventArgs(Me)
                DBUtils.doCursorClosed(e)
            End If

        End Sub

        Protected Sub CheckInit()
            If Me.CursorStatus = DBCursorStatus.CLOSED Then Me.Open()
        End Sub

        Public Function Count() As Integer
            Me.CheckInit()
            Return Me.m_NumItems
        End Function

        Public ReadOnly Property Index As Integer
            Get
                Return Me.m_Index
            End Get
        End Property

        Public Function EOF() As Boolean
            Me.CheckInit()
            Return (Me.m_Index >= Me.m_NumItems)
        End Function

        Public Function MoveTo(ByVal index As Integer) As Boolean
            Dim p As Integer

            Me.CheckInit()
            If (index < 0) Then Return False
            If (index > Me.m_NumItems) Then index = Me.m_NumItems
            p = Fix(index / Me.m_PageSize)
            If (p = Me.m_PageNum) Then
                Me.m_Index = index
            Else
                Dim conn As CDBConnection = Me.GetConnection
                If (conn.IsRemote) Then
                    Me.m_PageNum = p
                    Me.m_Index = index
                    Me.LoadNextPage()
                Else
                    If (p < Me.m_PageNum) Then
                        If (Me.m_reader IsNot Nothing) Then Me.m_reader.Dispose()
                        Me.m_reader = New DBReader(Me.GetDBSchema, Me.GetFullSQL)
                        Me.m_PageNum = 0
                    End If
                    Dim itemsToRead As Integer = (p - 1 - Me.m_PageNum) * Me.m_PageSize
                    'While (itemsToRead > 0) AndAlso Me.m_dbRis.Read
                    While (itemsToRead > 0) AndAlso Me.m_reader.Read
                        itemsToRead -= 1
                    End While
                    Me.m_PageNum = p
                    Me.m_Index = index

                    Me.LoadNextPage()
                End If
            End If

            Return True
        End Function

        Public Function MovePrev() As Boolean
            Return Me.MoveTo(Me.m_Index - 1)
        End Function

        Public Function MoveNext() As Boolean
            Return Me.MoveTo(Me.m_Index + 1)
        End Function

        Public Function MoveFirst() As Boolean
            Me.CheckInit()
            Dim conn As CDBConnection = Me.GetConnection
            If (conn.IsRemote) Then
                Me.m_PageNum = 0
                Me.m_Index = 0
                Me.LoadNextPage()
            Else
                If Me.m_PageNum > 0 Then
                    If Not (Me.m_reader Is Nothing) Then Me.m_reader.Dispose()
                    Me.m_reader = New DBReader(Me.GetDBSchema, Me.GetFullSQL)
                    Me.LoadNextPage()
                End If
                Me.m_PageNum = 0
                Me.m_Index = 0
            End If
            Return (Me.m_Index < Me.m_NumItems)
        End Function

        Public Function MoveLast() As Boolean
            Return Me.MoveTo(Me.Count - 1)
        End Function

        Private Sub LoadNextPage()
            Dim conn As CDBConnection = Me.Connection
            If (conn.IsRemote) Then
                Me.CheckInit()
                Dim tn As String = Types.vbTypeName(Me)
                Dim ret As String = RPC.InvokeMethod("/websvc/?_m=Sistema&_a=LoadNextCursorPage", "tn", RPC.str2n(tn), "tk", RPC.str2n(Me.m_Token), "idx", RPC.int2n(Me.m_Index))
                If (ret <> "") Then
                    Me.m_Items = XML.Utils.Serializer.Deserialize(ret) ', Types.vbTypeName(this.InstantiateNew(null)));
                Else
                    'Me.m_Items = Arrays.CreateInstance(Of (this.baseType, this.m_PageSize);
                    ReDim Me.m_Items(Me.m_PageSize - 1)
                End If
            Else
                Me.CheckInit()

                Dim i As Integer = 0
                While (i < Me.m_PageSize) AndAlso Me.m_reader.Read
                    Me.m_Items(i) = Me.InstantiateNew(Me.m_reader)
                    If Me.Connection.Load(Me.m_Items(i), Me.m_reader) = False Then Me.m_Items(i) = Nothing
                    i += 1
                End While

                While (i < Me.m_PageSize)
                    Me.m_Items(i) = Nothing
                    i += 1
                End While

                Me.SyncPage()
            End If

        End Sub

        Public MustOverride Function InstantiateNew(ByVal reader As DBReader) As Object

        Public MustOverride Function GetTableName() As String

        Protected Function GetCursorFields() As CCursorFieldsCollection
            Dim ret As New CCursorFieldsCollection
            'ret.Add(Me.m_ID)
            Dim items() As System.Reflection.PropertyInfo = Me.GetType.GetProperties(Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)
            For Each p As System.Reflection.PropertyInfo In items
                If GetType(CCursorField).IsAssignableFrom(p.PropertyType) Then
                    Dim f As CCursorField = p.GetValue(Me, Nothing)
                    If (f IsNot Nothing) Then ret.Add(f)
                End If
            Next
            Return ret
        End Function

        
        Protected ReadOnly Property WhereFields As CKeyCollection(Of CCursorField)
            Get
                If (Me.m_WhereFields Is Nothing) Then Me.m_WhereFields = Me.GetWhereFields
                Return Me.m_WhereFields
            End Get
        End Property

        Protected ReadOnly Property SortFields As CKeyCollection(Of CCursorField)
            Get
                If (Me.m_SortFields Is Nothing) Then Me.m_SortFields = Me.GetSortFields
                Return Me.m_SortFields
            End Get
        End Property

        Protected Overridable Function GetSortFields() As CKeyCollection(Of CCursorField)
            Return New CKeyCollection(Of CCursorField)(Me.Fields)
        End Function


        Protected Overridable Function GetWhereFields() As CKeyCollection(Of CCursorField)
            Return New CKeyCollection(Of CCursorField)(Me.Fields)
        End Function

        Public Overridable Function GetWherePart() As String
            Dim fields As CKeyCollection(Of CCursorField) = Me.WhereFields
            Dim wherePart As New System.Text.StringBuilder

            For Each field As CCursorField In fields
                If field.IsSet Then
                    If (wherePart.Length > 0) Then wherePart.Append(" AND ")
                    wherePart.Append("(")
                    wherePart.Append(field.GetSQL)
                    wherePart.Append(")")
                End If
            Next

            If (Me.IgnoreRights = False) AndAlso (Me.Module IsNot Nothing) Then
                'If (Users.CurrentUser IsNot Users.KnownUsers.SystemUser) Then
                Dim tmpSQL As String = Me.GetWherePartLimit
                If tmpSQL <> vbNullString Then
                    If (wherePart.Length > 0) Then wherePart.Append(" AND ")
                    wherePart.Append("(")
                    wherePart.Append(tmpSQL)
                    wherePart.Append(")")
                End If

            End If
            For Each clause As String In Me.WhereClauses
                If clause <> "" Then
                    If (wherePart.Length > 0) Then wherePart.Append(" AND ")
                    wherePart.Append("(")
                    wherePart.Append(clause)
                    wherePart.Append(")")
                End If
            Next

            Return wherePart.ToString
        End Function

        Public Overridable Function GetWherePartLimit() As String
            If Not Me.Module.UserCanDoAction("list") Then Return "(0<>0)"
            Return vbNullString
        End Function

        Public Function GetItemsArray() As Object
            Return Me.m_Items
        End Function

        Protected Overridable Function GetSortPart(ByVal field As CCursorField) As String
            Dim ret As New System.Text.StringBuilder
            If (InStr(field.FieldName, "[") <= 0 AndAlso InStr(field.FieldName, "(") <= 0) Then
                ret.Append("[")
                ret.Append(field.FieldName)
                ret.Append("]")
            Else
                ret.Append(field.FieldName)
            End If

            Select Case field.SortOrder
                Case SortEnum.SORT_ASC : ret.Append(" ASC")
                Case SortEnum.SORT_DESC : ret.Append(" DESC")
                Case Else
            End Select

            Return ret.ToString
        End Function

        Public Overridable Function GetSortPart() As String
            Dim fields As CKeyCollection(Of CCursorField) = Me.SortFields
            Dim items As New CCollection
            Dim field As CCursorField
            For Each field In fields
                If field.SortOrder <> SortEnum.SORT_NOTSET Then items.Add(field)
            Next
            items.Comparer = Me
            items.Sort()

            Dim orderPart As String = ""
            For Each field In items
                Dim fieldName As String = Me.GetSortPart(field)
                If (fieldName <> "") Then orderPart = Strings.Combine(orderPart, fieldName, ",")
            Next

            Return Trim(orderPart)
        End Function

        Protected Function Compare(ByVal f1 As Object, ByVal f2 As Object) As Integer Implements IComparer.Compare
            Return -Arrays.Compare(DirectCast(f1, CCursorField).SortPriority, DirectCast(f2, CCursorField).SortPriority)
        End Function

        ''' <summary>
        ''' Restituisce l'elenco dei campi della query da restituire (default: *)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overridable Function GetFieldsProjection() As String
            Return "*"
        End Function

        Public Overridable Function GetSQL() As String
            Dim dbSQL As New System.Text.StringBuilder
            dbSQL.Append("SELECT ")
            dbSQL.Append(Me.GetFieldsProjection)
            dbSQL.Append(" FROM [")
            dbSQL.Append(Me.GetTableName)
            dbSQL.Append("]")

            Dim wherePart As String = Me.GetWherePart

            If (wherePart <> "") Then
                dbSQL.Append(" WHERE ")
                dbSQL.Append(wherePart)
            End If

            Return dbSQL.ToString
        End Function

        Public Overridable Function GetFullSQL() As String
            Dim dbSQL As New System.Text.StringBuilder
            dbSQL.Append(Me.GetSQL)

            Dim sortPart As String = Me.GetSortPart
            If sortPart <> vbNullString Then
                dbSQL.Append(" ORDER BY ")
                dbSQL.Append(sortPart)
            End If

            Return dbSQL.ToString
        End Function

        Public Property Item As Object
            Get
                Me.CheckInit()
                If Me.m_CursorStatus = DBCursorStatus.NEWROW Then
                    Return Me.m_Item
                Else
                    If Not Me.EOF Then
                        Return Me.m_Items(Me.m_Index Mod Me.m_PageSize)
                    Else
                        Return Nothing
                    End If
                End If
            End Get
            Set(value As Object)
                Me.CheckInit()
                If (Me.m_CursorStatus = DBCursorStatus.NEWROW) Then
                    Me.m_Item = value
                Else
                    Me.m_Items(Me.m_Index Mod Me.m_PageSize) = value
                End If
            End Set
        End Property

        Public Function Drop() As Boolean
            Select Case Me.m_CursorStatus
                Case DBCursorStatus.NORMAL
                    DirectCast(Me.Item, DBObjectBase).Delete()
                    Me.m_CursorStatus = DBCursorStatus.DELETED
                    Return True
                Case Else
                    Throw New InvalidOperationException("Stato del cursore non valido")
                    Return False
            End Select
        End Function

        Public Function Update() As Boolean
            Select Case Me.m_CursorStatus
                Case DBCursorStatus.NEWROW, DBCursorStatus.MODIFIED
                    DirectCast(Me.Item, DBObjectBase).Save()
                    Me.m_CursorStatus = DBCursorStatus.NORMAL
                    Return True
                Case DBCursorStatus.DELETED
                    DirectCast(Me.Item, DBObjectBase).Delete()
                    Me.m_CursorStatus = DBCursorStatus.NORMAL
                    Return True
                Case Else
                    DirectCast(Me.Item, DBObjectBase).Save()
                    Me.m_CursorStatus = DBCursorStatus.NORMAL
                    Return True
            End Select
        End Function

        Private Sub handlePropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs)
            Me.m_CursorStatus = DBCursorStatus.MODIFIED
        End Sub

        Public Overridable Function Add() As Object
            Me.CheckInit()
            If (Me.m_CursorStatus <> DBCursorStatus.NORMAL) Then
                Throw New InvalidOperationException("DBObjectCursorBase: Stato del cursore non valido")
            End If
            Me.m_CursorStatus = DBCursorStatus.NEWROW
            Me.m_Item = Me.InstantiateNew(Nothing)
            Me.OnInitialize(Me.m_Item)
            Return Me.m_Item
        End Function

        ''' <summary>
        ''' Metodo chiamato per inizializzare l'oggetto in seguito alla chiamata al metodo Add 
        ''' </summary>
        ''' <param name="item"></param>
        ''' <remarks></remarks>
        Protected Overridable Sub OnInitialize(ByVal item As Object)

        End Sub

       

        Public Overridable Sub InitFrom(ByVal cursor As DBObjectCursorBase)
            Me.Reset1()
            For Each sourceField As CCursorField In cursor.Fields
                Me.Fields(sourceField.FieldName).CopyFrom(sourceField)
            Next
            Me.m_PageSize = cursor.PageSize
        End Sub

        Public Sub Destroy()
            Me.Reset1()
            Dim conn As CDBConnection = Me.GetConnection
            If (conn.IsRemote) Then
                If (Me.m_Token <> "") Then
                    Dim tn As String = Types.vbTypeName(Me)
                    Dim ret As String = RPC.InvokeMethod("/websvc/?_m=Sistema&_a=DestroyCursor", "tn", RPC.str2n(tn), "tk", RPC.str2n(Me.m_Token))
                End If
            Else
                If (Me.m_reader IsNot Nothing) Then Me.m_reader.Dispose() : Me.m_reader = Nothing
            End If

            For Each field As CCursorField In Me.Fields
                field.Clear()
            Next

            Me.m_Token = vbNullString
            Me.m_PageNum = -1
            Me.m_Index = -1
            Me.m_ID = Nothing
            Me.m_Module = Nothing
            Me.m_NumItems = -1
            Me.m_IgnoreRights = False
            Me.m_dbConn = Nothing
            If (Me.m_Items IsNot Nothing) Then Erase Me.m_Items : Me.m_Items = Nothing
            Me.m_Item = Nothing
            Me.m_Fields = Nothing
            Me.m_CursorStatus = DBCursorStatus.CLOSED
            Me.m_PageSize = 0
            Me.m_WhereClauses = Nothing
            Me.m_WhereFields = Nothing
            Me.m_SortFields = Nothing
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Overridable Sub Dispose() Implements IDisposable.Dispose
#If Not DEBUG Then
            Try
#End If
            Me.Destroy()
#If Not DEBUG Then
            Catch ex As Exception
                Debug.Print(ex.StackTrace)
            End Try
#End If

        End Sub


        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case (fieldName)
                Case "m_Token" : Me.m_Token = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "m_CursorStatus" : Me.m_CursorStatus = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "m_PageNum" : Me.m_PageNum = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "m_Index" : Me.m_Index = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "m_NumItems" : Me.m_NumItems = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "m_IgnoreRights" : Me.m_IgnoreRights = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
                Case "m_PageSize" : Me.m_PageSize = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "m_Fields"
                    If Not (TypeOf (fieldValue) Is String) AndAlso (fieldValue IsNot Nothing) Then
                        'If (fieldValue IsNot Nothing) Then
                        For Each fld As CCursorField In Arrays.Convert(Of CCursorField)(fieldValue)
                            Dim target As CCursorField = Me.Fields.GetItemByKey(fld.FieldName)
                            If (target Is Nothing) Then
                                Throw New ArgumentOutOfRangeException(Strings.JoinW("Il cursore [", TypeName(Me), "] non contenete il campo [", fld.FieldName, "]"))
                            End If
                            target.CopyFrom(fld)
                        Next
                    End If
                Case "m_Items"
                    Me.m_Items = Arrays.Convert(Of Object)(fieldValue)
            End Select
        End Sub

        Private Function getChangedFields() As CCollection(Of CCursorField)
            Dim ret As New CCollection(Of CCursorField)
            For Each f As CCursorField In Me.Fields
                If (f.IsChanged) Then ret.Add(f)
            Next
            Return ret
        End Function

        Protected Overridable Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("m_Token", Me.m_Token)
            writer.WriteAttribute("m_CursorStatus", Me.m_CursorStatus)
            writer.WriteAttribute("m_PageNum", Me.m_PageNum)
            writer.WriteAttribute("m_Index", Me.m_Index)
            writer.WriteAttribute("m_NumItems", Me.m_NumItems)
            writer.WriteAttribute("m_IgnoreRights", Me.m_IgnoreRights)
            writer.WriteAttribute("m_PageSize", Me.m_PageSize)
            writer.WriteTag("m_Fields", Me.getChangedFields.ToArray, "CCursorField")
            writer.WriteTag("m_Items", Me.m_Items)
        End Sub

        Private Sub m_dbConn_ConnectionClosed(sender As Object, e As EventArgs) Handles m_dbConn.ConnectionClosed
            If Me.m_CursorStatus <> DBCursorStatus.CLOSED Then
                Throw New InvalidOperationException(Strings.JoinW("Il cursore [", TypeName(Me), "] è ancora aperto mentre la connessione è stata chiusa"))
            End If
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub

        Protected Overridable Sub SyncPage()

        End Sub
    End Class

End Class

