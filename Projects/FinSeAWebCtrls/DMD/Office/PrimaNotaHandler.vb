Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.Office
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Web
Imports DMD.XML

Namespace Forms

    Public Class PrimaNotaHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SAnnotations)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New RigaPrimaNotaCursor
        End Function



        Function GetInfoPrimaNota(ByVal renderer As Object) As String
            Dim po As Nullable(Of Integer) = RPC.n2int(Me.GetParameter(renderer, "po", ""))
            Dim fromDate As Nullable(Of Date) = RPC.n2date(Me.GetParameter(renderer, "di", ""))
            Dim toDate As Nullable(Of Date) = RPC.n2date(Me.GetParameter(renderer, "df", ""))
            Dim sumEI As Decimal = 0 : Dim sumEF As Decimal = 0
            Dim sumUI As Decimal = 0 : Dim sumUF As Decimal = 0
            Dim dbRis As System.Data.IDataReader
            Dim wherePart As String = "[Stato]=" & ObjectStatus.OBJECT_VALID
            If Not Me.Module.UserCanDoAction("list") Then
                Dim tmp As String = ""
                If Me.Module.UserCanDoAction("list_office") Then
                    For Each u As CUfficio In Users.CurrentUser.Uffici
                        If (tmp <> "") Then tmp &= ","
                        tmp &= GetID(u)
                    Next
                End If
                If (tmp <> "") Then tmp = " [IDPuntoOperativo] In (0, " & tmp & ")"
                If Me.Module.UserCanDoAction("list_own") Then
                    tmp = Strings.Combine(tmp, " [CreatoDa]=" & GetID(Users.CurrentUser), " Or ")
                End If
                If (tmp <> "") Then
                    wherePart = Strings.Combine(wherePart, "(" & tmp & ")", " AND ")
                End If
            End If
            If (po.HasValue AndAlso po.Value <> 0) Then
                wherePart = wherePart & " AND [IDPuntoOperativo]=" & po
            End If

            If (fromDate.HasValue) Then
                dbRis = Office.Database.ExecuteReader("SELECT SUM([Entrate])  As [sumEI], SUM([Uscite]) As [sumUI] FROM [tbl_OfficePrimaNota] WHERE [Data]<" & DBUtils.DBDate(fromDate) & " AND " & wherePart)
                If (dbRis.Read) Then
                    sumEI = Formats.ToValuta(dbRis("sumEI"))
                    sumUI = Formats.ToValuta(dbRis("sumUI"))
                End If
                dbRis.Dispose()
            End If
            If (toDate.HasValue) Then
                toDate = Calendar.DateAdd(Microsoft.VisualBasic.DateInterval.Second, 24 * 3600 - 1, Calendar.GetDatePart(toDate))
                dbRis = Office.Database.ExecuteReader("SELECT SUM([Entrate])  As [sumEF], SUM([Uscite]) As [sumUF] FROM [tbl_OfficePrimaNota] WHERE [Data]<=" & DBUtils.DBDate(toDate) & " AND " & wherePart)
            Else
                dbRis = Office.Database.ExecuteReader("SELECT SUM([Entrate])  As [sumEF], SUM([Uscite]) As [sumUF] FROM [tbl_OfficePrimaNota] WHERE " & wherePart)
            End If
            If (dbRis.Read) Then
                sumEF = Formats.ToValuta(dbRis("sumEF"))
                sumUF = Formats.ToValuta(dbRis("sumUF"))
            End If
            dbRis.Dispose()
            Dim ret As New CKeyCollection
            ret.Add("sumEI", sumEI)
            ret.Add("sumUI", sumUI)
            ret.Add("sumEF", sumEF)
            ret.Add("sumUF", sumUF)
            Return XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document)
        End Function

        Public Overrides Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
            Dim ret As CCollection(Of ExportableColumnInfo) = MyBase.GetExportableColumnsList()
            ret.Add(New ExportableColumnInfo("ID", "ID", TypeCode.Int32, True))
            ret.Add(New ExportableColumnInfo("Data", "Data", TypeCode.DateTime, True))
            ret.Add(New ExportableColumnInfo("NomePuntoOperativo", "Punto Operativo", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("DescrizioneMovimento", "Descrizione Movimento", TypeCode.String, True))
            ret.Add(New ExportableColumnInfo("Entrate", "Entrate", TypeCode.Double, True))
            ret.Add(New ExportableColumnInfo("Uscite", "Uscite", TypeCode.Double, True))
            Return ret
        End Function

        Protected Overrides Sub SetColumnValue(ByVal renderer As Object, item As Object, key As String, value As Object)
            Dim tmp As RigaPrimaNota = item
            Dim tmpStr As String
            Select Case key
                Case "NomePuntoOperativo"
                    tmpStr = Trim(CStr(key))
                    If (tmpStr = vbNull) Then
                        tmp.PuntoOperativo = Nothing
                    Else
                        tmp.PuntoOperativo = Anagrafica.Uffici.GetItemByName(tmpStr)
                    End If
                Case Else : MyBase.SetColumnValue(renderer, item, key, value)
            End Select

        End Sub

        Protected Overrides Function ExportXlsFormat(renderer As Object) As String
            If Not Me.Module.UserCanDoAction("export") Then Throw New PermissionDeniedException(Me.Module, "export")


            Dim cursor As RigaPrimaNotaCursor = Nothing
            Dim fileName As String = vbNullString
            Dim xlsConn As CXlsDBConnection = Nothing
            Dim xlsTable As CDBTable = Nothing
            Dim cols As CCollection(Of ExportableColumnInfo) = Nothing
            Dim cmd As System.Data.IDbCommand = Nothing
            Dim param As System.Data.IDbDataParameter = Nothing
            Dim ufficio As CUfficio = Nothing

            Try
                cursor = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter(renderer, "cursor", "")))
                cursor.Reset1()

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
                If (cursor.IDPuntoOperativo.IsSet) Then ufficio = Anagrafica.Uffici.GetItemById(cursor.IDPuntoOperativo.Value)
                Dim di As Nullable(Of Date)
                Dim giacenza As Double
                If (cursor.Data.IsSet) Then
                    di = Calendar.DateAdd(DateInterval.Second, -1, cursor.Data.Value)
                    giacenza = DMD.Office.PrimaNota.GetGiacenzaCassa(ufficio, di)
                Else
                    di = Nothing
                    giacenza = 0 'DMD.Office.PrimaNota.GetGiacenzaCassa(ufficio)
                End If


                For Each col As ExportableColumnInfo In cols
                    If (col.Selected) Then
                        param = cmd.Parameters("@" & col.Key)
                        Select Case col.Key
                            Case "DescrizioneMovimento" : param.Value = "Giacenza iniziale:"
                            Case "Entrate" : param.Value = giacenza
                            Case "Uscite" : param.Value = 0
                            Case "Data"
                                If di.HasValue Then
                                    param.Value = di.Value
                                Else
                                    param.Value = DBNull.Value
                                End If
                            Case Else : param.Value = DBNull.Value
                        End Select

                    End If
                Next
                cmd.ExecuteNonQuery()


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
                'xlsConn.Tables("Tabella0").Drop()
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw ex
            Finally
                If (cmd IsNot Nothing) Then cmd.Dispose()
                If (xlsConn IsNot Nothing) Then xlsConn.Dispose()
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try


            'Try
            '    DMD.Excel.ExcelUtils.DeleteWorkSheetFromFile(xlsConn.Path, "Tabella0")
            '    DMD.Excel.ExcelUtils.FormatStandardTable(xlsConn.Path, "Tabella")
            'Catch ex As Exception
            '    'Manca il controllo
            'End Try

            'If (fileName <> vbNullString) Then
            'Dim token As String = ASPSecurity.CreateToken(fileName, fileName)
            Dim url As String = "/widgets/websvc/download.aspx?fp=" & fileName & "&dn=" & Me.Module.ModuleName & " " & Replace(Replace(Now, "/", "_"), ":", "_")

            Me.Module.DispatchEvent(New EventDescription("list_exported", "Dati esportati nel file " & fileName, fileName))

            'WebSite.Server.Transfer(url)
            'End If

            'WebSite.ServeFile(Server.MapPath(fileName), fileName, ServeFileMode.default)
            Return url
        End Function

        Protected Overrides Function ExportMdbFormat(renderer As Object) As String
            If Not Me.Module.UserCanDoAction("export") Then Throw New PermissionDeniedException(Me.Module, "export")


            Dim cursor As RigaPrimaNotaCursor = Nothing
            Dim fileName As String = vbNullString
            Dim dbConn As CMdbDBConnection = Nothing
            Dim dbTable As CDBTable = Nothing
            Dim cols As CCollection(Of ExportableColumnInfo) = Nothing
            Dim cmd As System.Data.IDbCommand = Nothing
            Dim param As System.Data.IDbDataParameter = Nothing
            Dim ufficio As CUfficio = Nothing
#If Not DEBUG Then
            Try
#End If
            cursor = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter(renderer, "cursor", "")))
            cursor.Reset1()

            FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(WebSite.Instance.Configuration.PublicURL & "temp"))
            fileName = WebSite.Instance.Configuration.PublicURL & "temp/" & ASPSecurity.GetRandomKey(12) & ".mdb"

            FileSystem.CopyFile(Sistema.ApplicationContext.MapPath("/templates/template.mdb"), Sistema.ApplicationContext.MapPath(fileName), True)
            dbConn = New CMdbDBConnection(Sistema.ApplicationContext.MapPath(fileName))
            dbConn.OpenDB()

            dbTable = dbConn.Tables.Add("Prima Nota")

            cols = Me.GetExportedColumns(renderer)
            For Each col As ExportableColumnInfo In cols
                If (col.Selected) Then
                    dbTable.Fields.Add(col.Key, col.TipoValore)
                End If
            Next
            dbTable.Update()


            cmd = dbTable.GetInsertCommand
            If (cursor.IDPuntoOperativo.IsSet) Then ufficio = Anagrafica.Uffici.GetItemById(cursor.IDPuntoOperativo.Value)
            Dim di As Nullable(Of Date)
            Dim giacenza As Double
            If (cursor.Data.IsSet) Then
                di = Calendar.DateAdd(DateInterval.Second, -1, cursor.Data.Value)
                giacenza = DMD.Office.PrimaNota.GetGiacenzaCassa(ufficio, di)
            Else
                di = Nothing
                giacenza = 0 'DMD.Office.PrimaNota.GetGiacenzaCassa(ufficio)
            End If


            For Each col As ExportableColumnInfo In cols
                If (col.Selected) Then
                    param = cmd.Parameters("@" & col.Key)
                    Select Case col.Key
                        Case "DescrizioneMovimento" : param.Value = "Giacenza iniziale:"
                        Case "Entrate" : param.Value = giacenza
                        Case "Uscite" : param.Value = 0
                        Case "Data"
                            If di.HasValue Then
                                param.Value = di.Value
                            Else
                                param.Value = DBNull.Value
                            End If
                        Case Else : param.Value = DBNull.Value
                    End Select

                End If
            Next
            cmd.ExecuteNonQuery()


            While Not cursor.EOF
                For Each col As ExportableColumnInfo In cols
                    If (col.Selected) Then
                        param = cmd.Parameters("@" & col.Key)
                        param.Value = dbConn.ToDB(Me.GetColumnValue(renderer, cursor.Item, col.Key))
                    End If
                Next
                cmd.ExecuteNonQuery()
                cursor.MoveNext()
            End While
#If Not DEBUG Then
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw ex
            Finally
#End If
            If (cmd IsNot Nothing) Then cmd.Dispose()
            If (dbConn IsNot Nothing) Then dbConn.Dispose()
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
#If Not DEBUG Then
            End Try
#End If

            Dim url As String = "/widgets/websvc/download.aspx?fp=" & fileName & "&dn=" & Me.Module.ModuleName & " " & Replace(Replace(Now, "/", "_"), ":", "_")
            Me.Module.DispatchEvent(New EventDescription("list_exported", "Dati esportati nel file " & fileName, fileName))
            Return url
        End Function

        Protected Overrides Function ExportCSVFormat(renderer As Object) As String
            If Not Me.Module.UserCanDoAction("export") Then Throw New PermissionDeniedException(Me.Module, "export")


            Dim cursor As RigaPrimaNotaCursor = Nothing
            Dim fileName As String = vbNullString
            Dim cols As CCollection(Of ExportableColumnInfo) = Nothing
            Dim ufficio As CUfficio = Nothing
            Dim buffer As New System.Text.StringBuilder

            Try
                cursor = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter(renderer, "cursor", "")))
                cursor.Reset1()

                FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(WebSite.Instance.Configuration.PublicURL & "temp"))
                fileName = WebSite.Instance.Configuration.PublicURL & "temp/" & ASPSecurity.GetRandomKey(12) & ".csv"


                cols = Me.GetExportedColumns(renderer)
                Dim i As Integer = 0
                For Each col As ExportableColumnInfo In cols
                    If (col.Selected) Then
                        If (i > 0) Then buffer.Append(";")
                        Me.writeCSV(buffer, col.Key)
                    End If
                Next
                buffer.Append(vbCrLf)


                If (cursor.IDPuntoOperativo.IsSet) Then ufficio = Anagrafica.Uffici.GetItemById(cursor.IDPuntoOperativo.Value)
                Dim di As Nullable(Of Date)
                Dim giacenza As Double
                If (cursor.Data.IsSet) Then
                    di = Calendar.DateAdd(DateInterval.Second, -1, cursor.Data.Value)
                    giacenza = DMD.Office.PrimaNota.GetGiacenzaCassa(ufficio, di)
                Else
                    di = Nothing
                    giacenza = 0 'DMD.Office.PrimaNota.GetGiacenzaCassa(ufficio)
                End If

                i = 0
                For Each col As ExportableColumnInfo In cols
                    If (col.Selected) Then
                        If (i > 0) Then buffer.Append(";")
                        Select Case col.Key
                            Case "DescrizioneMovimento" : Me.writeCSV(buffer, "Giacenza iniziale:")
                            Case "Entrate" : Me.writeCSV(buffer, giacenza)
                            Case "Uscite" : Me.writeCSV(buffer, 0)
                            Case "Data"
                                If di.HasValue Then
                                    Me.writeCSV(buffer, di.Value)
                                Else
                                    Me.writeCSV(buffer, "")
                                End If
                            Case Else : Me.writeCSV(buffer, "")
                        End Select
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
                    buffer.Append(vbCrLf)
                    cursor.MoveNext()
                End While

            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw ex
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try

            System.IO.File.WriteAllText(Sistema.ApplicationContext.MapPath(fileName), buffer.ToString)
            Dim url As String = "/widgets/websvc/download.aspx?fp=" & fileName & "&dn=" & Me.Module.ModuleName & " " & Replace(Replace(Now, "/", "_"), ":", "_")
            Me.Module.DispatchEvent(New EventDescription("list_exported", "Dati esportati nel file " & fileName, fileName))
            Return url
        End Function

        'Protected Overrides Function ExportFakeXLSFormat(renderer As Object) As String
        '    If Not Me.Module.UserCanDoAction("export") Then Throw New PermissionDeniedException(Me.Module, "export")


        '    Dim cursor As RigaPrimaNotaCursor = Nothing
        '    Dim fileName As String = vbNullString
        '    Dim cols As CCollection(Of ExportableColumnInfo) = Nothing
        '    Dim ufficio As CUfficio = Nothing
        '    Dim buffer As New System.Text.StringBuilder

        '    Try
        '        cursor = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter(renderer, "cursor", "")))
        '        cursor.Reset1()

        '        FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(WebSite.Instance.Configuration.PublicURL & "temp"))
        '        fileName = WebSite.Instance.Configuration.PublicURL & "temp/" & ASPSecurity.GetRandomKey(12) & ".xls"


        '        cols = Me.GetExportedColumns(renderer)
        '        Dim i As Integer = 0
        '        For Each col As ExportableColumnInfo In cols
        '            If (col.Selected) Then
        '                If (i > 0) Then buffer.Append(";")
        '                Me.writeCSV(buffer, col.Key)
        '            End If
        '        Next
        '        buffer.Append(vbCrLf)


        '        If (cursor.IDPuntoOperativo.IsSet) Then ufficio = Anagrafica.Uffici.GetItemById(cursor.IDPuntoOperativo.Value)
        '        Dim di As Nullable(Of Date)
        '        Dim giacenza As Double
        '        If (cursor.Data.IsSet) Then
        '            di = Calendar.DateAdd(DateInterval.Second, -1, cursor.Data.Value)
        '            giacenza = DMD.Office.PrimaNota.GetGiacenzaCassa(ufficio, di)
        '        Else
        '            di = Nothing
        '            giacenza = 0 'DMD.Office.PrimaNota.GetGiacenzaCassa(ufficio)
        '        End If

        '        i = 0
        '        For Each col As ExportableColumnInfo In cols
        '            If (col.Selected) Then
        '                If (i > 0) Then buffer.Append(";")
        '                Select Case col.Key
        '                    Case "DescrizioneMovimento" : Me.writeCSV(buffer, "Giacenza iniziale:")
        '                    Case "Entrate" : Me.writeCSV(buffer, giacenza)
        '                    Case "Uscite" : Me.writeCSV(buffer, 0)
        '                    Case "Data"
        '                        If di.HasValue Then
        '                            Me.writeCSV(buffer, di.Value)
        '                        Else
        '                            Me.writeCSV(buffer, "")
        '                        End If
        '                    Case Else : Me.writeCSV(buffer, "")
        '                End Select
        '                i += 1
        '            End If
        '        Next
        '        buffer.Append(vbCrLf)

        '        While Not cursor.EOF
        '            i = 0
        '            For Each col As ExportableColumnInfo In cols
        '                If (col.Selected) Then
        '                    If (i > 0) Then buffer.Append(";")
        '                    Me.writeCSV(buffer, Formats.ToString(Me.GetColumnValue(renderer, cursor.Item, col.Key)))
        '                    i += 1
        '                End If
        '            Next
        '            buffer.Append(vbCrLf)
        '            cursor.MoveNext()
        '        End While

        '    Catch ex As Exception
        '        Sistema.Events.NotifyUnhandledException(ex)
        '        Throw ex
        '    Finally
        '        If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
        '    End Try

        '    System.IO.File.WriteAllText(Sistema.ApplicationContext.MapPath(fileName), buffer.ToString)
        '    Dim url As String = "/widgets/websvc/download.aspx?fp=" & fileName & "&dn=" & Me.Module.ModuleName & " " & Replace(Replace(Now, "/", "_"), ":", "_")
        '    Me.Module.DispatchEvent(New EventDescription("list_exported", "Dati esportati nel file " & fileName, fileName))
        '    Return url
        'End Function

        Protected Overrides Function ExportTxtFormat(renderer As Object) As String
            If Not Me.Module.UserCanDoAction("export") Then Throw New PermissionDeniedException(Me.Module, "export")


            Dim cursor As RigaPrimaNotaCursor = Nothing
            Dim fileName As String = vbNullString
            Dim cols As CCollection(Of ExportableColumnInfo) = Nothing
            Dim ufficio As CUfficio = Nothing
            Dim buffer As New System.Text.StringBuilder

            Try
                cursor = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter(renderer, "cursor", "")))
                cursor.Reset1()

                FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(WebSite.Instance.Configuration.PublicURL & "temp"))
                fileName = WebSite.Instance.Configuration.PublicURL & "temp/" & ASPSecurity.GetRandomKey(12) & ".txt"

                cols = Me.GetExportedColumns(renderer)
                Dim i As Integer = 0
                For Each col As ExportableColumnInfo In cols
                    If (col.Selected) Then
                        If (i > 0) Then buffer.Append(vbTab)
                        buffer.Append(col.Key)
                    End If
                Next
                buffer.Append(vbCrLf)


                If (cursor.IDPuntoOperativo.IsSet) Then ufficio = Anagrafica.Uffici.GetItemById(cursor.IDPuntoOperativo.Value)
                Dim di As Nullable(Of Date)
                Dim giacenza As Double
                If (cursor.Data.IsSet) Then
                    di = Calendar.DateAdd(DateInterval.Second, -1, cursor.Data.Value)
                    giacenza = DMD.Office.PrimaNota.GetGiacenzaCassa(ufficio, di)
                Else
                    di = Nothing
                    giacenza = 0 'DMD.Office.PrimaNota.GetGiacenzaCassa(ufficio)
                End If

                i = 0
                For Each col As ExportableColumnInfo In cols
                    If (col.Selected) Then
                        If (i > 0) Then buffer.Append(vbTab)
                        Select Case col.Key
                            Case "DescrizioneMovimento" : buffer.Append("Giacenza iniziale:")
                            Case "Entrate" : buffer.Append(giacenza)
                            Case "Uscite" : buffer.Append(0)
                            Case "Data"
                                If di.HasValue Then
                                    buffer.Append(di.Value)
                                Else
                                    buffer.Append("")
                                End If
                            Case Else : buffer.Append("")
                        End Select
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
                    buffer.Append(vbCrLf)
                    cursor.MoveNext()
                End While

            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw ex
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try

            System.IO.File.WriteAllText(Sistema.ApplicationContext.MapPath(fileName), buffer.ToString)
            Dim url As String = "/widgets/websvc/download.aspx?fp=" & fileName & "&dn=" & Me.Module.ModuleName & " " & Replace(Replace(Now, "/", "_"), ":", "_")
            Me.Module.DispatchEvent(New EventDescription("list_exported", "Dati esportati nel file " & fileName, fileName))
            Return url
        End Function


    End Class


End Namespace