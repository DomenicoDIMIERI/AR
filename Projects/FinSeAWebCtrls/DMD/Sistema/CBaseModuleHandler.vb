Imports DMD
Imports DMD.Sistema
Imports DMD.Forms
Imports DMD.WebSite
Imports DMD.Databases
Imports DMD.Forms.Utils

Imports DMD.Web
Imports DMD.XML

Namespace Forms

    <Flags>
    Public Enum ModuleSupportFlags As Integer
        SNone = 0
        SAnnotations = 1
        SCreate = 2
        SEdit = 4
        SDelete = 8
        SDuplicate = 16
        SImport = 32
        SExport = 64
        SPrint = 256
    End Enum


    Public Class CBaseModuleHandler
        Implements IModuleHandler


        Private m_UseLocal As Boolean
        Private m_Module As CModule
        Private m_Support As ModuleSupportFlags

        Public Event MessageLogged(ByVal sender As Object, ByVal message As String)

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Support = ModuleSupportFlags.SNone
            Me.m_UseLocal = True
            Me.m_Module = Nothing
        End Sub

        Public Sub New(ByVal support As ModuleSupportFlags)
            Me.New
            Me.m_Support = support
            Me.m_UseLocal = True
            Me.m_Module = Nothing
        End Sub

        Public Sub New(ByVal [module] As CModule, Optional ByVal support As ModuleSupportFlags = ModuleSupportFlags.SNone, Optional ByVal useLocal As Boolean = True)
            Me.New(support)
            If ([module] Is Nothing) Then Throw New ArgumentNullException("module")
            Me.SetModule([module])
            Me.m_UseLocal = useLocal
        End Sub




        Public Property UseLocal As Boolean Implements IModuleHandler.UseLocal
            Get
                Return Me.m_UseLocal
            End Get
            Set(value As Boolean)
                Me.m_UseLocal = value
            End Set
        End Property

        Public Function GetParameter(ByVal renderer As Object, ByVal paramName As String, ByVal defValue As String) As String
            'Return Sistema.ApplicationContext.GetParameter(paramName, defValue)
            Dim page As WebSite.WebPageEx = renderer
            Return page.GetParameter(paramName, defValue)
        End Function

        Public Function GetParameter(Of T As Structure)(ByVal renderer As Object, ByVal paramName As String, ByVal defValue As Object) As T
            'Return Sistema.ApplicationContext.renderer(Of T)(paramName, defValue)
            Dim page As WebSite.WebPageEx = renderer
            Return page.GetParameter(Of T)(paramName, defValue)
        End Function

        Public Overridable Function ExecuteAction(ByVal renderer As Object, actionName As String) As String Implements IModuleHandler.ExecuteAction
            Select Case LCase(actionName)
                Case "getitembyid" : Return Me.GetItemById(renderer)
                Case "edit" : Return Me.edit(renderer)
                Case "list" : Return Me.list(renderer)
                Case "create" : Return Me.create(renderer)
                Case "delete" : Return Me.delete(renderer)
                Case "importlist" : Return Me.ImportList(renderer)
                Case "exportlist" : Return Me.ExportList(renderer)
                Case "getexportablecolumns" : Return Me.GetExportableColumns(renderer)
                Case "print" : Return Me.print(renderer)
                Case Else
                    Dim m As System.Reflection.MethodInfo
                    m = Me.GetType.GetMethod(actionName, Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance, Nothing, {GetType(Object)}, Nothing)

                    If (m Is Nothing) Then
                        Throw New MissingMethodException("Il modulo [" & Me.Module.ModuleName & "] non implementa l'azione [" & actionName & "]")
                        Return vbNullString
                    End If

                    Dim ret As Object = m.Invoke(Me, {renderer})
                    If (TypeOf (ret) Is MethodResults) Then
                        Return XML.Utils.Serializer.Serialize(ret)
                    Else
                        Return CStr(ret)
                    End If
            End Select
        End Function

        'Public Overridable Function ExecuteAction1(ByVal renderer As Object, actionName As String) As MethodResults Implements IModuleHandler.ExecuteAction1
        '    Dim m As System.Reflection.MethodInfo
        '    m = Me.GetType.GetMethod(actionName, Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance, Nothing, New System.Type() {}, Nothing)
        '    If (m Is Nothing) OrElse Not (m.ReturnType Is GetType(MethodResults)) Then
        '        Throw New MissingMethodException("Il modulo [" & Me.Module.ModuleName & "] non implementa l'azione [" & actionName & "]")
        '        Return Nothing
        '    End If
        '    Try
        '        Return m.Invoke(Me, Nothing)
        '    Catch ex As Exception
        '        Throw ex.InnerException
        '    End Try
        'End Function

        Public Sub SetModule(value As CModule) Implements IModuleHandler.SetModule
            Me.m_Module = value
        End Sub

        Public ReadOnly Property [Module] As CModule Implements IModuleHandler.Module
            Get
                Return Me.m_Module
            End Get
        End Property

        Public Overridable Function CreateCursor() As DBObjectCursorBase Implements IModuleHandler.CreateCursor
            Return Nothing
        End Function



        'Public MustOverride Function GetGC As CGeneralClass(Of 

        Public Overridable Function GetItemById(ByVal renderer As Object) As String
            Dim itemID As Integer = RPC.n2int(Me.GetParameter(renderer, "ID", ""))
            Dim ret As Object = Me.GetInternalItemById(itemID)
            If ret Is Nothing Then
                Return ""
            Else
                Return XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document)
            End If
        End Function

        Public Overridable Function GetItemsById(ByVal renderer As Object) As String
            Dim cursor As DBObjectCursorBase = Nothing
            Dim ret As New CCollection
            Try
                Dim ids As String = RPC.n2str(Me.GetParameter(renderer, "ids", ""))
                Dim strids As String() = Strings.Split(ids, ",")
                Dim items() As Integer = {}
                If (Arrays.Len(strids)) > 0 Then
                    For Each str As String In strids
                        Dim id As Integer = Formats.ToInteger(str)
                        If (id <> 0) Then items = Arrays.Append(items, id)
                    Next
                End If
                If (Arrays.Len(items) > 0) Then
                    cursor = Me.CreateCursor
                    cursor.ID.ValueIn(items)
                    While Not cursor.EOF
                        ret.Add(cursor.Item)
                        cursor.MoveNext()
                    End While
                End If

                Return XML.Utils.Serializer.Serialize(ret)
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Public Overridable Function GetInternalItemById(ByVal id As Integer) As Object
            If (id = 0) Then Return Nothing
            Dim cursor As DBObjectCursorBase = Me.CreateCursor
            Dim tn As String = DBUtils.GetTableName(cursor)
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim ret As Object = Nothing
            Dim conn As CDBConnection = cursor.Connection
            Try
                'Dim t As Date = Calendar.Now
                dbRis = conn.ExecuteReader("SELECT * FROM [" & tn & "] WHERE [ID]=" & id)
                'Dim t1 As Date = Calendar.Now
                'Debug.Print(TypeName(Me) & ".GetItemById(" & id & ") -> ExecuteReader : " & ((t1 - t).TotalMilliseconds / 1000) & " s")
                If dbRis.Read Then
                    'Dim t3 As Date = Calendar.Now
                    'Debug.Print(TypeName(Me) & ".GetItemById(" & id & ") -> Read : " & ((t3 - t1).TotalMilliseconds / 1000) & " s")
                    Dim reader As New DBReader(dbRis)
                    'Dim t4 As Date = Calendar.Now
                    'Debug.Print(TypeName(Me) & ".GetItemById(" & id & ") -> new DBReader : " & ((t4 - t3).TotalMilliseconds / 1000) & " s")
                    ret = cursor.InstantiateNew(reader)
                    conn.Load(ret, reader)
                    reader.Dispose()
                    dbRis = Nothing
                    'Dim t5 As Date = Calendar.Now
                    'Debug.Print(TypeName(Me) & ".GetItemById(" & id & ") -> new Load : " & ((t5 - t4).TotalMilliseconds / 1000) & " s")
                End If
                Return ret
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                cursor.Dispose()
                If (dbRis IsNot Nothing) Then dbRis.Dispose()
            End Try


        End Function

#Region "List"

        Public Overridable Function list(ByVal renderer As Object) As String
            Return vbNullString
        End Function

#End Region

#Region "Create"

        Public Overridable ReadOnly Property SupportsCreate As Boolean
            Get
                Return (Me.m_Support And ModuleSupportFlags.SCreate) = ModuleSupportFlags.SCreate
            End Get
        End Property

        Public Overridable ReadOnly Property SupportsDuplicate As Boolean
            Get
                Return (Me.m_Support And ModuleSupportFlags.SDuplicate) = ModuleSupportFlags.SDuplicate
            End Get
        End Property

        ''' <summary>
        ''' Restituisce vero se l'utente corrente può eseguire la configurazione del modulo 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function CanConfigure() As Boolean Implements IModuleHandler.CanConfigure
            Return Me.Module.UserCanDoAction("configure")
        End Function


        Public Overridable Function CanCreate() As Boolean Implements IModuleHandler.CanCreate
            Return Me.Module.UserCanDoAction("create")
        End Function

        Public Overridable Function create(ByVal renderer As Object) As String
            Dim cursor As DBObjectCursorBase
            Dim item As DBObjectBase
            Dim fromID As Integer
            If (Me.CanCreate = False) Then Throw New PermissionDeniedException(Me.Module, "create")
            fromID = Formats.ToInteger(RPC.n2int(Me.GetParameter(renderer, "ID", "")))
            cursor = Me.CreateCursor
            item = cursor.Add
            If (fromID <> 0) AndAlso (TypeOf (item) Is ISupportInitializeFrom) Then DirectCast(item, ISupportInitializeFrom).InitializeFrom(Me.GetInternalItemById(fromID))
            'e = New Sistema.SystemEvent
            'item.OnCreate", "Sub", e)
            item.Save()
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return Me.InternalEdit(renderer, item)
        End Function

#End Region

#Region "Delete"

        Public Overridable ReadOnly Property SupportsDelete As Boolean
            Get
                Return (Me.m_Support And ModuleSupportFlags.SDelete) = ModuleSupportFlags.SDelete
            End Get
        End Property

        Public Overridable Function CanDelete(ByVal item As Object) As Boolean Implements IModuleHandler.CanDelete
            Dim ret As Boolean
            ret = Me.Module.UserCanDoAction("delete")
            If (ret = False) AndAlso (TypeOf (item) Is IDBObject) Then
                With DirectCast(item, IDBObject)
                    ret = Me.Module.UserCanDoAction("delete_own") And (.CreatoDaId = GetID(Users.CurrentUser))
                End With
            End If
            If (ret = False) AndAlso (TypeOf (item) Is IDBPOObject) Then
                With DirectCast(item, IDBPOObject)
                    If Me.Module.UserCanDoAction("delete_office") Then
                        ret = (.PuntoOperativo Is Nothing) OrElse (Users.CurrentUser.Uffici.HasOffice(.PuntoOperativo))
                    End If
                End With
            End If
            Return ret
        End Function


        Public Overridable Function delete(ByVal renderer As Object) As String
            Dim itemID As Integer = RPC.n2int(Me.GetParameter(renderer, "ID", ""))
            Dim item As DBObjectBase = Me.GetInternalItemById(itemID)
            If Me.CanDelete(item) Then
                Dim cursor As DBObjectCursorBase = Me.CreateCursor
                If TypeOf (item) Is IDBObject Then
                    DirectCast(item, IDBObject).Stato = ObjectStatus.OBJECT_DELETED
                    item.Save()
                Else
                    item.Delete()
                End If
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                Return vbNullString
            Else
                Throw New PermissionDeniedException
                Return vbNullString
            End If
        End Function

#End Region

#Region "Edit"

        Public Overridable ReadOnly Property SupportsEdit As Boolean
            Get
                Return (Me.m_Support And ModuleSupportFlags.SEdit) = ModuleSupportFlags.SEdit
            End Get
        End Property

        Public Overridable ReadOnly Property SupportsAnnotations As Boolean
            Get
                Return (Me.m_Support And ModuleSupportFlags.SAnnotations) = ModuleSupportFlags.SAnnotations
            End Get
        End Property

        Public Overridable Function CanEdit(ByVal item As Object) As Boolean Implements IModuleHandler.CanEdit
            Dim ret As Boolean
            ret = Me.Module.UserCanDoAction("edit")
            If (ret = False) AndAlso (TypeOf (item) Is IDBObject) Then
                With DirectCast(item, IDBObject)
                    ret = Me.Module.UserCanDoAction("edit_own") And (.CreatoDaId = GetID(Users.CurrentUser))
                End With
            End If
            If (ret = False) AndAlso (TypeOf (item) Is IDBPOObject) Then
                With DirectCast(item, IDBPOObject)
                    If Me.Module.UserCanDoAction("edit_office") Then
                        ret = (.PuntoOperativo Is Nothing) OrElse (Users.CurrentUser.Uffici.HasOffice(.PuntoOperativo))
                    End If
                End With
            End If
            Return ret
        End Function

        Public Overridable Function CanList() As Boolean Implements IModuleHandler.CanList
            Dim ret As Boolean
            ret = Me.Module.UserCanDoAction("list") OrElse Me.Module.UserCanDoAction("edit_office") OrElse Me.Module.UserCanDoAction("edit_own")
            Return ret
        End Function

        Public Overridable Function CanList(ByVal item As Object) As Boolean
            Dim ret As Boolean
            ret = Me.Module.UserCanDoAction("list")
            If (ret = False AndAlso TypeOf (item) Is IDBObject) Then
                ret = Me.Module.UserCanDoAction("edit_own") AndAlso DirectCast(item, IDBObject).CreatoDaId = GetID(Sistema.Users.CurrentUser)
            End If
            If (ret = False AndAlso TypeOf (item) Is IDBPOObject) Then
                ret = Me.Module.UserCanDoAction("edit_office") AndAlso Anagrafica.Uffici.GetPuntiOperativiConsentiti.GetItemById(DirectCast(item, IDBPOObject).IDPuntoOperativo) IsNot Nothing
            End If
            Return ret
        End Function


        Public Overridable Function edit(ByVal renderer As Object) As String
            Dim itemID As Integer = Formats.ToInteger(RPC.n2int(Me.GetParameter(renderer, "ID", "")))
            Dim itemObj As Object = Me.GetInternalItemById(itemID)
            If Not Me.CanEdit(itemObj) Then
                Throw New PermissionDeniedException(Me.Module, "edit")
            End If
            'If itemObj Is Nothing Then
            '    Throw New ArgumentNullException("Non trovo alcun elemeneto corrispondente all'ID: " & itemID)
            'End If
            'Return Me.InternalEdit(itemObj)
            'Dim editor As BaseModuleEditor = Me.GetEditor
            'With editor
            '    .UseLocal = True
            '    .Width = Me.m_Renderer.Width
            '    .Height = Me.m_Renderer.Height
            '    .Name = "ctrlEditor"
            '    .Visible = False
            '    .DoLayout()
            'End With
            'Dim writer As New HTMLWriter
            'editor.CreateHTML(writer)

            Return Me.InternalEdit(renderer, itemObj)
        End Function


        Public Overridable Function CanPrint(ByVal item As Object) As Boolean
            Return Me.Module.UserCanDoAction("print")
        End Function

        Public Overridable Function print(ByVal renderer As Object) As String
            Dim html As String = Me.ExecuteAction(renderer, "edit")
            html &= "<script type=""text-javascript"">" & vbNewLine & "Window.addListener(""onload"", new Function('window.print()'));" & vbNewLine & "</script>"
            Return html
        End Function


        Protected Overridable Function InternalEdit(ByVal renderer As Object, ByVal value As DBObjectBase) As String
            'Dim tabControl As CTabEditor
            'Dim writer As New HTMLWriter
            'Dim itemID As Integer
            'Dim message As String = vbNullString

            'tabControl = Me.CreateTabEditor(renderer, value)
            'itemID = GetID(value)

            'If Not Me.CanEdit(value) Then
            '    message = "Diritti insufficienti"
            'Else
            '    tabControl.Item = value

            '    Select Case Me.GetParameter(renderer, "_s")
            '        Case "1", "2", "3", "4", "5"
            '            Me.OnBeforeSave(value)

            '            If tabControl.Save Then
            '                If TypeOf (value) Is IDBObject Then
            '                    If DirectCast(value, IDBObject).Stato = ObjectStatus.OBJECT_TEMP Then
            '                        DirectCast(value, IDBObject).Stato = ObjectStatus.OBJECT_VALID
            '                    End If
            '                End If
            '                value.Save()
            '            Else
            '                message = "Errore sconosciuto"
            '            End If

            '            Me.OnAfterSave(value)
            '    End Select
            'End If

            'tabControl.CreateHTML(writer)
            'writer.WriteRowData("<input type=""hidden"" name=""ID"" id=""ID"" value=""" & itemID & """ />")
            'If GetParameter(renderer, "strsrc") = "" Then writer.WriteRowData("<input type=""hidden"" name=""strsrc"" id=""strsrc"" value=""" & Me.GetParameter(renderer, "strsrc") & """ />")

            'If message <> "" Then
            '    writer.WriteRowData("<script language=""javascript"" type=""text/javascript"">" & vbNewLine)
            '    writer.WriteRowData("alert(""" & message & """);")
            '    writer.WriteRowData("</script>")
            'End If

            'Dim ret As String = writer.ToString
            'writer.Dispose()
            'Return ret

            Dim ret As String = "" 'Me.list(renderer)
            ret &= "<script type=""text/javascript"">" & vbNewLine
            ret &= "Window.addListener(""onload"", new Function('setTimeout(SystemUtils.EditModuleItem(" & GetID(Me.Module) & ", " & GetID(value) & "), 500)'));"
            ret &= "</script>"

            Return ret
        End Function

        Protected Overridable Sub OnBeforeSave(ByVal item As Object)

        End Sub

        Protected Overridable Sub OnAfterSave(ByVal item As Object)

        End Sub

#End Region

#Region "Print"

        Public Overridable ReadOnly Property SupportsPrint As Boolean
            Get
                Return (Me.m_Support And ModuleSupportFlags.SPrint) = ModuleSupportFlags.SPrint
            End Get
        End Property

#End Region

#Region "Export"

        ''' <summary>
        ''' Quando sottoposto ad override in una classe derivata restituisce un valore booleano che indica se il modulo supporta l'esportazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable ReadOnly Property SupportsExport As Boolean
            Get
                Return ((Me.m_Support And ModuleSupportFlags.SExport) = ModuleSupportFlags.SExport) Or (Me.GetExportableColumnsList.Count > 0)
            End Get
        End Property

        Public Overridable Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
            Dim ret As New CCollection(Of ExportableColumnInfo)
            'ret.Add(New ExportableColumnInfo("ID", "ID", TypeCode.Int32, True))
            Return ret
        End Function

        Public Function GetExportableColumns(ByVal renderer As Object) As String
            Dim ret As CCollection(Of ExportableColumnInfo) = Me.GetExportableColumnsList
            If (ret.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(ret.ToArray, XMLSerializeMethod.Document)
            Else
                Return vbNullString
            End If
        End Function

        Protected Overridable Function GetExportedColumns(ByVal renderer As Object) As CCollection(Of ExportableColumnInfo)
            Dim text As String = RPC.n2str(Me.GetParameter(renderer, "listCols", ""))

            Dim ret As CCollection(Of ExportableColumnInfo)
            If (text = "") Then
                ret = Me.GetExportableColumnsList
            Else
                ret = New CCollection(Of ExportableColumnInfo)
                Dim items() As Object = XML.Utils.Serializer.Deserialize(text)
                For Each item As ExportableColumnInfo In items
                    ret.Add(item)
                Next
            End If
            Return ret
        End Function

        Protected Overridable Function GetColumnValue(ByVal renderer As Object, ByVal item As Object, ByVal key As String) As Object
            Dim m As System.Reflection.PropertyInfo = item.GetType.GetProperty(key, Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)
            Return m.GetValue(item, New Object() {})
        End Function

        Public Function CanExport() As Boolean
            Return Me.Module.UserCanDoAction("export")
        End Function

        Public Overridable Function ExportList(ByVal renderer As Object) As String
            If Not Me.CanExport Then Throw New PermissionDeniedException(Me.Module, "export")

            Dim listFormat As String = Strings.LCase(Strings.Trim(RPC.n2str(GetParameter(renderer, "listFormat", "xls"))))
            Select Case listFormat
                Case "xls" : Return Me.ExportXlsFormat(renderer)
                'Case "xls" : Return Me.ExportFakeXLSFormat(renderer)
                Case "mdb" : Return Me.ExportMdbFormat(renderer)
                Case "csv" : Return Me.ExportCSVFormat(renderer)
                Case "txt" : Return Me.ExportTxtFormat(renderer)
                Case Else
                    Throw New NotSupportedException("Il formato " & listFormat & " non è supportato")
            End Select
        End Function


        Protected Overridable Function ExportXlsFormat(ByVal renderer As Object) As String
            Dim cursor As DBObjectCursorBase = Nothing
            Dim fileName As String = ""
            Dim xlsConn As CXlsDBConnection = Nothing
            Dim xlsTable As CDBTable = Nothing
            Dim cols As CCollection(Of ExportableColumnInfo)
            Dim cmd As System.Data.IDbCommand = Nothing
            Dim param As System.Data.IDbDataParameter = Nothing

            'If (Me.UseLocal) Then
            Try
                cursor = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter(renderer, "cursor", "")))
                cursor.Reset1()
                cursor.PageSize = 10000
                'cursor.Count()

                FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(WebSite.Instance.Configuration.PublicURL & "temp"))
                fileName = WebSite.Instance.Configuration.PublicURL & "temp/" & ASPSecurity.GetRandomKey(12) & ".xls"

                FileSystem.CopyFile(Sistema.ApplicationContext.MapPath("/templates/xlsfile.xls"), Sistema.ApplicationContext.MapPath(fileName), True)
                xlsConn = New CXlsDBConnection(Sistema.ApplicationContext.MapPath(fileName))
                xlsConn.OpenDB()

                xlsTable = xlsConn.Tables.Add("Tabella")
                'xlsTable = xlsConn.Tables(0)

                cols = Me.GetExportedColumns(renderer)
                For Each col As ExportableColumnInfo In cols
                    If (col.Selected) Then
                        xlsTable.Fields.Add(col.Key, col.TipoValore)
                    End If
                Next
                xlsTable.Update()

                cmd = xlsTable.GetInsertCommand
                While Not cursor.EOF
                    For Each col As ExportableColumnInfo In cols
                        If (col.Selected) Then
                            param = cmd.Parameters("@" & col.Key)
                            param.Value = xlsConn.ToDB(Me.GetColumnValue(renderer, cursor.Item, col.Key))
                        End If
                    Next
                    cmd.ExecuteNonQuery()
                    cursor.MoveNext()
                End While
                If (cmd IsNot Nothing) Then cmd.Dispose() : cmd = Nothing
                If (xlsConn IsNot Nothing) Then xlsConn.Dispose() : xlsConn = Nothing
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                Dim url As String = "/widgets/websvc/download.aspx?fp=" & fileName & "&dn=" & Me.Module.ModuleName & " " & Replace(Replace(Now, "/", "_"), ":", "_")

                Me.Module.DispatchEvent(New EventDescription("list_exported", "Dati esportati nel file " & fileName, fileName))

                Return url
            Catch ex As Exception
                Throw
            Finally
                If (cmd IsNot Nothing) Then cmd.Dispose() : cmd = Nothing
                If (xlsConn IsNot Nothing) Then xlsConn.Dispose() : xlsConn = Nothing
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Protected Overridable Function ExportMdbFormat(ByVal renderer As Object) As String
            Dim cursor As DBObjectCursorBase = Nothing
            Dim fileName As String = ""
            Dim outConn As CMdbDBConnection = Nothing
            Dim outTable As CDBTable = Nothing
            Dim cols As CCollection(Of ExportableColumnInfo)
            Dim cmd As System.Data.IDbCommand = Nothing
            Dim param As System.Data.IDbDataParameter = Nothing
            Try

                cursor = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter(renderer, "cursor", "")))
                cursor.Reset1()

                FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(WebSite.Instance.Configuration.PublicURL & "temp"))
                fileName = WebSite.Instance.Configuration.PublicURL & "temp/" & ASPSecurity.GetRandomKey(12) & ".mdb"

                FileSystem.CopyFile(Sistema.ApplicationContext.MapPath("/templates/template.mdb"), Sistema.ApplicationContext.MapPath(fileName), True)
                outConn = New CMdbDBConnection(Sistema.ApplicationContext.MapPath(fileName))
                outConn.OpenDB()

                outTable = outConn.Tables.Add(Me.Module.DisplayName)
                'xlsTable = xlsConn.Tables(0)

                cols = Me.GetExportedColumns(renderer)
                For Each col As ExportableColumnInfo In cols
                    If (col.Selected) Then
                        outTable.Fields.Add(col.Key, col.TipoValore)
                    End If
                Next
                outTable.Update()

                cmd = outTable.GetInsertCommand
                While Not cursor.EOF
                    For Each col As ExportableColumnInfo In cols
                        If (col.Selected) Then
                            param = cmd.Parameters("@" & col.Key)
                            param.Value = outConn.ToDB(Me.GetColumnValue(renderer, cursor.Item, col.Key))
                        End If
                    Next
                    cmd.ExecuteNonQuery()
                    cursor.MoveNext()
                End While

                If (cmd IsNot Nothing) Then cmd.Dispose() : cmd = Nothing
                If (outConn IsNot Nothing) Then outConn.Dispose() : outConn = Nothing
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing


                Dim url As String = "/widgets/websvc/download.aspx?fp=" & fileName & "&dn=" & Me.Module.ModuleName & " " & Replace(Replace(Now, "/", "_"), ":", "_")

                Me.Module.DispatchEvent(New EventDescription("list_exported", "Dati esportati nel file " & fileName, fileName))


                Return url
            Catch ex As Exception
                Throw
            Finally
                If (cmd IsNot Nothing) Then cmd.Dispose() : cmd = Nothing
                If (outConn IsNot Nothing) Then outConn.Dispose() : outConn = Nothing
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Protected Sub writeCSV(ByVal buffer As System.Text.StringBuilder, ByVal text As String)
            buffer.Append(Chr(34))
            buffer.Append(Replace(text, Chr(34), Chr(34)))
            buffer.Append(Chr(34))
        End Sub

        'Protected Overridable Function ExportFakeXLSFormat(ByVal renderer As Object) As String
        '    Dim cursor As DBObjectCursorBase
        '    Dim fileName As String
        '    Dim cols As CCollection(Of ExportableColumnInfo)

        '    'If (Me.UseLocal) Then
        '    cursor = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter(renderer, "cursor", "")))
        '    cursor.Reset1()

        '    FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(WebSite.Instance.Configuration.PublicURL & "temp"))
        '    fileName = WebSite.Instance.Configuration.PublicURL & "temp/" & ASPSecurity.GetRandomKey(12) & ".xls"


        '    Dim buffer As New System.Text.StringBuilder


        '    cols = Me.GetExportedColumns(renderer)
        '    Dim i As Integer = 0
        '    For Each col As ExportableColumnInfo In cols
        '        If (col.Selected) Then
        '            If (i > 0) Then buffer.Append(";")
        '            Me.writeCSV(buffer, col.Key)
        '            i += 1
        '        End If
        '    Next
        '    buffer.Append(vbCrLf)

        '    While Not cursor.EOF
        '        i = 0
        '        For Each col As ExportableColumnInfo In cols
        '            If (col.Selected) Then
        '                If (i > 0) Then buffer.Append(";")
        '                Me.writeCSV(buffer, Formats.ToString(Me.GetColumnValue(renderer, cursor.Item, col.Key)))
        '                i += 1
        '            End If
        '        Next
        '        cursor.MoveNext()
        '        buffer.Append(vbCrLf)
        '    End While
        '    If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

        '    System.IO.File.WriteAllText(Sistema.ApplicationContext.MapPath(fileName), buffer.ToString)
        '    Dim url As String = "/widgets/websvc/download.aspx?fp=" & fileName & "&dn=" & Me.Module.ModuleName & " " & Replace(Replace(Now, "/", "_"), ":", "_")

        '    Me.Module.DispatchEvent(New EventDescription("list_exported", "Dati esportati nel file " & fileName, fileName))

        '    Return url
        'End Function

        Protected Overridable Function ExportCSVFormat(ByVal renderer As Object) As String
            Dim cursor As DBObjectCursorBase
            Dim fileName As String
            Dim cols As CCollection(Of ExportableColumnInfo)

            'If (Me.UseLocal) Then
            cursor = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter(renderer, "cursor", "")))
            cursor.Reset1()

            FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(WebSite.Instance.Configuration.PublicURL & "temp"))
            fileName = WebSite.Instance.Configuration.PublicURL & "temp/" & ASPSecurity.GetRandomKey(12) & ".csv"


            Dim buffer As New System.Text.StringBuilder


            cols = Me.GetExportedColumns(renderer)
            Dim i As Integer = 0
            For Each col As ExportableColumnInfo In cols
                If (col.Selected) Then
                    If (i > 0) Then buffer.Append(";")
                    Me.writeCSV(buffer, col.Key)
                    i += 1
                End If
            Next
            buffer.Append(vbCrLf)

            While Not cursor.EOF
                i = 0
                For Each col As ExportableColumnInfo In cols
                    If (col.Selected) Then
                        If (i > 0) Then buffer.Append(";")
                        Me.writeCSV(buffer, Formats.ToString(Me.GetColumnValue(renderer, cursor.Item, col.Key)))
                        i += 1
                    End If
                Next
                cursor.MoveNext()
                buffer.Append(vbCrLf)
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            System.IO.File.WriteAllText(Sistema.ApplicationContext.MapPath(fileName), buffer.ToString)
            Dim url As String = "/widgets/websvc/download.aspx?fp=" & fileName & "&dn=" & Me.Module.ModuleName & " " & Replace(Replace(Now, "/", "_"), ":", "_")

            Me.Module.DispatchEvent(New EventDescription("list_exported", "Dati esportati nel file " & fileName, fileName))

            Return url
        End Function

        Protected Overridable Function ExportTxtFormat(ByVal renderer As Object) As String
            Dim cursor As DBObjectCursorBase
            Dim fileName As String
            Dim cols As CCollection(Of ExportableColumnInfo)

            'If (Me.UseLocal) Then
            cursor = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter(renderer, "cursor", "")))
            cursor.Reset1()

            FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(WebSite.Instance.Configuration.PublicURL & "temp"))
            fileName = WebSite.Instance.Configuration.PublicURL & "temp/" & ASPSecurity.GetRandomKey(12) & ".txt"


            Dim buffer As New System.Text.StringBuilder


            cols = Me.GetExportedColumns(renderer)
            Dim i As Integer = 0
            For Each col As ExportableColumnInfo In cols
                If (col.Selected) Then
                    If (i > 0) Then buffer.Append(vbTab)
                    buffer.Append(col.Key)
                    i += 1
                End If
            Next
            buffer.Append(vbCrLf)

            While Not cursor.EOF
                i = 0
                For Each col As ExportableColumnInfo In cols
                    If (col.Selected) Then
                        If (i > 0) Then buffer.Append(vbTab)
                        buffer.Append(Formats.ToString(Me.GetColumnValue(renderer, cursor.Item, col.Key)))
                        i += 1
                    End If
                Next
                cursor.MoveNext()
                buffer.Append(vbCrLf)
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            System.IO.File.WriteAllText(Sistema.ApplicationContext.MapPath(fileName), buffer.ToString)
            Dim url As String = "/widgets/websvc/download.aspx?fp=" & fileName & "&dn=" & Me.Module.ModuleName & " " & Replace(Replace(Now, "/", "_"), ":", "_")

            Me.Module.DispatchEvent(New EventDescription("list_exported", "Dati esportati nel file " & fileName, fileName))

            Return url
        End Function
#End Region

#Region "Import"

        ''' <summary>
        ''' Quando sottoposto ad override in una classe derivata restituisce un valore booleano che indica se il modulo supporta l'importazione
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable ReadOnly Property SupportsImport As Boolean
            Get
                Return ((Me.m_Support And ModuleSupportFlags.SImport) = ModuleSupportFlags.SImport) Or (Me.GetExportableColumnsList.Count > 0)
            End Get
        End Property

        Protected Overridable Sub SetColumnValue(ByVal renderer As Object, ByVal item As Object, ByVal key As String, ByVal value As Object)
            Select Case key
                Case "ID"
                Case Else
                    Dim m As System.Reflection.PropertyInfo = item.GetType.GetProperty(key, Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)
                    If m.CanWrite = False Then
                        Throw New InvalidConstraintException("Proprietà in sola lettura: " & key)
                    End If
                    Try
                        If TypeOf (value) Is DBNull Then
                            m.SetValue(item, Nothing, New Object() {})
                        Else
                            m.SetValue(item, Types.CastTo(value, m.PropertyType), New Object() {})
                        End If
                    Catch ex As Exception
                        Throw ex
                    End Try
            End Select

        End Sub

        Protected Overridable Function FindExportedItem(ByVal cursor As DBObjectCursorBase, ByVal info As DBReader) As Object
            Dim ret As Object
            Dim id As Integer = info.GetValue(Of Integer)("ID", 0)
            If (id = 0) Then Return Nothing
            cursor.Reset1()
            If (TypeOf (cursor) Is DBObjectCursor) Then DirectCast(cursor, DBObjectCursor).Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.ID.Value = id
            ret = cursor.Item
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return ret
        End Function

        Public Function CanImport() As Boolean
            Return Me.Module.UserCanDoAction("import")
        End Function

        Public Overridable Function ImportList(ByVal renderer As Object) As String
            If Not Me.CanImport Then Throw New PermissionDeniedException(Me.Module, "import")

            Dim fileName As String
            Dim xlsConn As CXlsDBConnection
            Dim xlsTable As CDBTable
            Dim xlsRis As DBReader
            Dim cols As CCollection(Of ExportableColumnInfo)
            Dim item As DBObjectBase
            Dim cursor As DBObjectCursorBase
            Dim s As Integer

            s = RPC.n2int(Me.GetParameter(renderer, "s", 0))
            If (s = 0) Then
                Return "L'importazione modificherà i record esistenti ed aggiungerà eventuali record non trovati."
            Else
                fileName = Trim(RPC.n2str(Me.GetParameter(renderer, "fn", vbNullString)))
                If (fileName = vbNullString) Then Throw New ArgumentNullException("fileName")
                xlsConn = New CXlsDBConnection(Sistema.ApplicationContext.MapPath(fileName))
                xlsConn.OpenDB()

                xlsTable = xlsConn.Tables("Tabella")
                xlsRis = New DBReader(xlsTable)
                cols = Me.GetExportedColumns(renderer)
                cursor = Me.CreateCursor
                While xlsRis.Read
                    item = Me.FindExportedItem(cursor, xlsRis)
                    If (item Is Nothing) Then item = cursor.Add
                    If (TypeOf (item) Is IDBObject) Then DirectCast(item, IDBObject).Stato = ObjectStatus.OBJECT_VALID
                    For Each column As ExportableColumnInfo In cols
                        If xlsRis.Schema.Fields.ContainsKey(column.Key) Then
                            Me.SetColumnValue(renderer, item, column.Key, xlsRis.GetValue(column.Key))
                        End If
                    Next
                    item.Save()
                End While
                xlsConn.Dispose()
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                Me.Module.DispatchEvent(New EventDescription("list_imported", "Dati importati dal file " & fileName, Nothing))
            End If

            Return ""
        End Function

#End Region

        Protected Overridable Sub OnMessageLogged(ByVal message As String)
            RaiseEvent MessageLogged(Me, message)
        End Sub

        Public Overridable Function LoadAll(ByVal renderer As Object) As String
            Dim ret As New CCollection
            Dim cursor As DBObjectCursorBase = Me.CreateCursor
#If Not DEBUG Then
            Try
#End If
            If (TypeOf (cursor) Is DBObjectCursor) Then
                With DirectCast(cursor, DBObjectCursor)
                    .Stato.Value = ObjectStatus.OBJECT_VALID
                End With
            End If
            While Not cursor.EOF
                ret.Add(cursor.Item)
                cursor.MoveNext()
            End While
#If Not DEBUG Then
            Catch ex As Exception
                Throw
            Finally
#End If
            cursor.Dispose()
#If Not DEBUG Then
            End Try
#End If
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Namespace