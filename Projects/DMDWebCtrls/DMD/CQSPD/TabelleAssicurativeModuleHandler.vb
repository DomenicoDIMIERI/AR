Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Forms.Utils
Imports DMD.XML

Namespace Forms



#Region "Tabelle Assicurative"

    Public Class TabelleAssicurativeModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(DMD.CQSPD.TabelleAssicurative.Module, ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            Me.UseLocal = True
        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CTabelleAssicurativeCursor
        End Function



        Public Function GetVincoliPXT(ByVal renderer As Object) As String
            Dim id As String = Me.GetParameter(renderer, "id", 0)
            Dim cursor As New CProdottoXTabellaAssCursor
            cursor.ID.Value = id
            Dim item As CProdottoXTabellaAss = cursor.Item
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            If (item.Vincoli.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(item.Vincoli.ToArray, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function

        Public Function GetTabellaXProdottoByID(ByVal renderer As Object) As String
            Dim id As Integer = RPC.n2int(GetParameter(renderer, "id", ""))
            Dim item As CProdottoXTabellaAss = CQSPD.TabelleAssicurative.GetTabellaXProdottoByID(id)
            If (item Is Nothing) Then
                Return ""
            Else
                Return XML.Utils.Serializer.Serialize(item, XMLSerializeMethod.Document)
            End If
        End Function

        Public Function GetArrayNomiTabelle(ByVal renderer As Object) As String
            Dim writer As New System.Text.StringBuilder
            Dim cursor As CTabelleAssicurativeCursor = Nothing

            Try
                cursor = New CTabelleAssicurativeCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Nome.SortOrder = SortEnum.SORT_ASC

                writer.Append("<list>")

                While Not cursor.EOF
                    writer.Append("<item>")
                    writer.Append("<value>")
                    writer.Append(GetID(cursor.Item))
                    writer.Append("</value>")
                    writer.Append("<text>")
                    writer.Append(Strings.HtmlEncode(cursor.Item.Nome))
                    writer.Append("</text>")
                    writer.Append("<attribute>")
                    If (cursor.Item.IsValid) Then
                        writer.Append("1")
                    Else
                        writer.Append("0")
                    End If
                    writer.Append("</attribute>")
                    writer.Append("</item>")
                    cursor.MoveNext()
                End While
                writer.Append("</list>")

                Return writer.ToString
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Public Function ExportCoefficienti(ByVal renderer As Object) As String
            If Not Me.Module.UserCanDoAction("export_coeff") Then Throw New PermissionDeniedException(Me.Module, "export_coeff")

            Dim idTabella As Integer = RPC.n2int(Me.GetParameter(renderer, "id", 0))
            Dim tabella As CTabellaAssicurativa = DMD.CQSPD.TabelleAssicurative.GetItemById(idTabella)
            If (tabella Is Nothing) Then Throw New ArgumentNullException("Tabella")

            Dim fileName As String
            Dim xlsConn As CXlsDBConnection
            Dim xlsTable As CDBTable
            Dim i As Integer

            FileSystem.CreateRecursiveFolder(Sistema.ApplicationContext.MapPath(WebSite.Instance.Configuration.PublicURL & "temp"))
            fileName = WebSite.Instance.Configuration.PublicURL & "temp/" & ASPSecurity.GetRandomKey(12) & ".xls"

            FileSystem.CopyFile(Sistema.ApplicationContext.MapPath("/templates/xlsfile.xls"), Sistema.ApplicationContext.MapPath(fileName), True)
            xlsConn = New CXlsDBConnection(Sistema.ApplicationContext.MapPath(fileName))
            xlsConn.OpenDB()

            xlsTable = xlsConn.Tables.Add("Tabella")
            'xlsTable = xlsConn.Tables(0)

            xlsTable.Fields.Add("Anni", GetType(Int32))
            xlsTable.Fields.Add("Sesso", GetType(String))
            For i = 24 To 120 Step 12
                xlsTable.Fields.Add("C" & i, GetType(Double))
            Next
            xlsTable.Update()

            Dim cmd As System.Data.IDbCommand = xlsTable.GetInsertCommand
            Dim param As System.Data.IDbDataParameter

            For Each row As CCoefficienteAssicurativo In tabella.Coefficienti
                param = cmd.Parameters("@Anni")
                param.Value = xlsConn.ToDB(row.Anni)
                param = cmd.Parameters("@Sesso")
                param.Value = xlsConn.ToDB(row.Sesso)
                For i = 24 To 120 Step 12
                    param = cmd.Parameters("@C" & i)
                    param.Value = xlsConn.ToDB(row.Coefficiente(i))
                Next
                cmd.ExecuteNonQuery()
            Next
            xlsConn.Dispose()

            'Try
            '    DMD.Excel.ExcelUtils.DeleteWorkSheetFromFile(xlsConn.Path, "Tabella0")
            '    DMD.Excel.ExcelUtils.FormatStandardTable(xlsConn.Path, "Tabella")
            'Catch ex As Exception

            'End Try

            'Dim token As String = ASPSecurity.CreateToken(fileName, fileName).Token
            WebSite.ASP_Server.Transfer("/widgets/websvc/download.aspx?fp=" & fileName & "&dn=" & Me.Module.ModuleName & " " & Replace(Replace(Now, "/", "_"), ":", "_"))
            'WebSite.ServeFile(Server.MapPath(fileName), fileName, ServeFileMode.default)
            Return vbNullString
        End Function

        Public Function ImportCoefficienti(ByVal renderer As Object) As String
            If Not Me.Module.UserCanDoAction("import_coeff") Then Throw New PermissionDeniedException(Me.Module, "import_coeff")

            Dim idTabella As Integer = RPC.n2int(Me.GetParameter(renderer, "id", 0))
            Dim tabella As CTabellaAssicurativa = DMD.CQSPD.TabelleAssicurative.GetItemById(idTabella)
            If (tabella Is Nothing) Then Throw New ArgumentNullException("Tabella")

            Dim fileName As String
            Dim xlsConn As CXlsDBConnection
            Dim xlsTable As CDBTable
            Dim xlsRis As DBReader
            Dim s As Integer
            Dim item As CCoefficienteAssicurativo
            Dim cursor As CCoefficientiAssicurativiCursor

            s = RPC.n2int(Me.GetParameter(renderer, "s", 0))
            If (s = 0) Then
                Return "L'importazione sostituirà completamente tutti i coefficienti della tabella [" & tabella.Nome & "]"
            Else
                fileName = Trim(RPC.n2str(Me.GetParameter(renderer, "fn", vbNullString)))
                If (fileName = vbNullString) Then Throw New ArgumentNullException("fileName")
                xlsConn = New CXlsDBConnection(Sistema.ApplicationContext.MapPath(fileName))
                xlsConn.OpenDB()

                xlsTable = xlsConn.Tables("Tabella")
                xlsRis = New DBReader(xlsTable)

                cursor = New CCoefficientiAssicurativiCursor
                cursor.Tabella.Value = idTabella
                cursor.ID.SortOrder = SortEnum.SORT_ASC

                While xlsRis.Read
                    If (cursor.EOF) Then
                        item = cursor.Add
                        item.Tabella = tabella
                        item.Anni = xlsRis.GetValue(Of Integer)("Anni")
                        item.Sesso = xlsRis.GetValue(Of String)("Sesso")
                        For i As Integer = 24 To 120 Step 12
                            item.Coefficiente(i) = xlsRis.GetValue(Of Double)("C" & i)
                        Next
                        cursor.Update()
                    Else
                        item = cursor.Item
                        item.Tabella = tabella
                        item.Anni = xlsRis.GetValue(Of Integer)("Anni")
                        item.Sesso = xlsRis.GetValue(Of String)("Sesso")
                        For i As Integer = 24 To 120 Step 12
                            item.Coefficiente(i) = xlsRis.GetValue(Of Double)("C" & i)
                        Next
                        cursor.Update()
                        cursor.MoveNext()
                    End If
                End While
                While Not cursor.EOF
                    cursor.Item.Delete()
                    cursor.MoveNext()
                End While
                xlsConn.Dispose()
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End If


            Return ""
        End Function


    End Class


#End Region



End Namespace