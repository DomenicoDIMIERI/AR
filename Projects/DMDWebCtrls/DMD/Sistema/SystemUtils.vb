Imports DMD
Imports DMD.Sistema
Imports DMD.Forms
Imports DMD.WebSite
Imports DMD.Databases
Imports DMD.Forms.Utils



Namespace Forms

    Partial Class Utils

        Public NotInheritable Class CSystemUtilsClass
            Private priority_values() As PriorityEnum = {PriorityEnum.PRIORITY_HIGHER, PriorityEnum.PRIORITY_HIGH, PriorityEnum.PRIORITY_NORMAL, PriorityEnum.PRIOTITY_LOW, PriorityEnum.PRIORITY_LOWER}
            Private priority_names() As String = {"Altissima", "Alta", "Normale", "Bassa", "Bassissima"}
            Private priority_icons() As String = {"priority_highest.gif", "priority_high.gif", "priority_normal.gif", "priority_low.gif", "priority_lowest.gif"}


            Friend Sub New()
                DMD.DMDObject.IncreaseCounter(Me)
            End Sub

            Public Function CreateElenco(ByVal values() As String, ByVal selValue As String) As String
                Return CreateElenco(values, values, selValue)
            End Function

            Public Function CreateElenco(ByVal names() As String, ByVal values() As String, ByVal selValue As String) As String
                Dim ret As New System.Text.StringBuilder
                Dim t = False, t1 As Boolean = False
                If (names.Length <> values.Length) Then Throw New ArgumentException("Gli array names e values devono avere la stessa lunghezza")
                selValue = Trim(selValue)
                For i As Integer = 0 To UBound(names)
                    t = LCase(selValue) = LCase(values(i))
                    t1 = t1 OrElse t
                    ret.Append("<option value=""" & Strings.HtmlEncode(values(i)) & """ " & CStr(IIf(t, "selected", "")) & ">" & Strings.HtmlEncode(names(i)) & "</option>")
                Next
                If (Not t1) AndAlso (selValue <> vbNullString) Then
                    ret.Append("<option value=""" & Strings.HtmlEncode(selValue) & """ selected style=""color:red;"">" & Strings.HtmlEncode(selValue) & "</option>")
                End If
                Return ret.ToString
            End Function

            Public Function CreateElenco(Of E)(ByVal values() As E, ByVal selValue As E) As String
                Dim arr As New System.Collections.ArrayList
                For Each i As E In values
                    arr.Add(i.ToString)
                Next
                Dim names() As String = arr.ToArray(GetType(String))
                Return CreateElenco(names, values, selValue)
            End Function


            Public Function CreateElenco(Of E)(ByVal names() As String, ByVal values() As E, ByVal selValue As Object) As String
                If (GetType(E).IsEnum) Then
                    Return CreateElencoInt(names, values, selValue)
                ElseIf (GetType(E).IsAssignableFrom(GetType(DBObjectBase))) Then
                    Return CreateElencoObj(names, values, selValue)
                Else
                    Dim ret As New System.Text.StringBuilder
                    Dim t = False, t1 As Boolean = False
                    If (names.Length <> values.Length) Then Throw New ArgumentException("Gli array names e values devono avere la stessa lunghezza")
                    For i As Integer = 0 To UBound(names)
                        t = (selValue IsNot Nothing) AndAlso (selValue.Equals(values(i)))
                        t1 = t1 OrElse t
                        ret.Append("<option value=""" & Strings.HtmlEncode(values(i).ToString) & """ " & CStr(IIf(t, "selected", "")) & ">" & Strings.HtmlEncode(names(i)) & "</option>")
                    Next
                    If (Not t1 AndAlso selValue IsNot Nothing) Then
                        ret.Append("<option value=""" & Strings.HtmlEncode(selValue.ToString) & """ selected style=""color:red;"">" & Strings.HtmlEncode(selValue.ToString) & "</option>")
                    End If
                    Return ret.ToString
                End If
            End Function

            Private Function CreateElencoInt(Of E)(ByVal names() As String, ByVal values() As E, ByVal selValue As Object) As String
                Dim ret As New System.Text.StringBuilder
                Dim t = False, t1 As Boolean = False
                If (names.Length <> values.Length) Then Throw New ArgumentException("Gli array names e values devono avere la stessa lunghezza")
                For i As Integer = 0 To UBound(names)
                    t = (selValue IsNot Nothing) AndAlso (selValue.Equals(values(i)))
                    t1 = t1 OrElse t
                    ret.Append("<option value=""" & Convert.ToInt32(values(i)) & """ " & CStr(IIf(t, "selected", "")) & ">" & Strings.HtmlEncode(names(i)) & "</option>")
                Next
                If (Not t1 AndAlso selValue IsNot Nothing) Then
                    ret.Append("<option value=""" & Convert.ToInt32(selValue) & """ selected style=""color:red;"">" & Strings.HtmlEncode(selValue.ToString) & "</option>")
                End If
                Return ret.ToString
            End Function

            Private Function CreateElencoObj(ByVal items As Array, ByVal selValue As IDBObjectBase) As String
                Dim ret As New System.Text.StringBuilder
                Dim t = False, t1 As Boolean = False
                For i As Integer = 0 To UBound(items)
                    Dim item As Object = items.GetValue(i) '{i, 0})
                    t = GetID(item) = selValue.ID
                    t1 = t1 OrElse t
                    ret.Append("<option value=""" & GetID(item) & """ " & CStr(IIf(t, "selected", "")) & ">" & Strings.HtmlEncode(item.ToString) & "</option>")
                Next
                If (Not t1 AndAlso selValue IsNot Nothing) Then
                    ret.Append("<option value=""" & GetID(selValue) & """ selected style=""color:red;"">" & Strings.HtmlEncode(selValue.ToString) & "</option>")
                End If
                Return ret.ToString
            End Function

            Private Function CreateElencoObj(ByVal names() As String, ByVal items As Array, ByVal selValue As IDBObjectBase) As String
                Dim ret As New System.Text.StringBuilder
                Dim t = False, t1 As Boolean = False
                For i As Integer = 0 To UBound(items)
                    Dim item As Object = items.GetValue(i)
                    t = GetID(item) = selValue.ID
                    t1 = t1 OrElse t
                    ret.Append("<option value=""" & GetID(item) & """ " & CStr(IIf(t, "selected", "")) & ">" & Strings.HtmlEncode(names(i)) & "</option>")
                Next
                If (Not t1 AndAlso selValue IsNot Nothing) Then
                    ret.Append("<option value=""" & GetID(selValue) & """ selected style=""color:red;"">" & Strings.HtmlEncode(selValue.ToString) & "</option>")
                End If
                Return ret.ToString
            End Function

            Public Function FormatField(ByVal field As CCursorField(Of Date)) As String
                If (field Is Nothing) Then Return vbNullString
                Select Case field.Operator
                    Case OP.OP_BETWEEN : Return Formats.FormatUserDate(field.Value) & "..." & Formats.FormatUserDate(field.Value1)
                    Case OP.OP_EQ : Return Formats.FormatUserDate(field.Value)
                    Case OP.OP_GE : Return Formats.FormatUserDate(field.Value) & "..."
                    Case OP.OP_LE : Return "..." & Formats.FormatUserDate(field.Value)
                    Case Else
                        Throw New NotSupportedException
                End Select
            End Function

            Public Function FormatField(ByVal field As CCursorField(Of Double)) As String
                If (field Is Nothing) Then Return vbNullString
                Select Case field.Operator
                    Case OP.OP_BETWEEN : Return Formats.FormatNumber(field.Value, 3) & "..." & Formats.FormatNumber(field.Value1, 3)
                    Case OP.OP_EQ : Return Formats.FormatNumber(field.Value, 3)
                    Case OP.OP_GE : Return Formats.FormatNumber(field.Value, 3) & "..."
                    Case OP.OP_LE : Return "..." & Formats.FormatNumber(field.Value, 3)
                    Case Else
                        Throw New NotSupportedException
                End Select
            End Function

            Public Function FormatField(ByVal field As CCursorField(Of Decimal)) As String
                If (field Is Nothing) Then Return vbNullString
                Select Case field.Operator
                    Case OP.OP_BETWEEN : Return Formats.FormatValuta(field.Value) & "..." & Formats.FormatValuta(field.Value1)
                    Case OP.OP_EQ : Return Formats.FormatValuta(field.Value)
                    Case OP.OP_GE : Return Formats.FormatValuta(field.Value) & "..."
                    Case OP.OP_LE : Return "..." & Formats.FormatValuta(field.Value)
                    Case Else
                        Throw New NotSupportedException
                End Select
            End Function

            Public Function FormatField(ByVal field As CCursorField(Of Integer)) As String
                If (field Is Nothing) Then Return vbNullString
                Select Case field.Operator
                    Case OP.OP_BETWEEN : Return Formats.FormatInteger(field.Value) & "..." & Formats.FormatInteger(field.Value1)
                    Case OP.OP_EQ : Return Formats.FormatInteger(field.Value)
                    Case OP.OP_GE : Return Formats.FormatInteger(field.Value) & "..."
                    Case OP.OP_LE : Return "..." & Formats.FormatInteger(field.Value)
                    Case Else
                        Throw New NotSupportedException
                End Select
            End Function

            Public Sub ParseCursorField(ByVal field As CCursorField(Of Date), ByVal text As String)
                text = Trim(text)
                field.Clear()
                If (text = vbNullString) Then Exit Sub

                Dim items() As String = Split(Trim(text), "...")
                If (UBound(items) > 0) Then
                    If (items(0) = "") Then
                        field.Operator = OP.OP_LE
                        field.Value = Formats.ParseDate(items(1))
                    ElseIf items(1) = "" Then
                        field.Operator = OP.OP_GE
                        field.Value = Formats.ParseDate(items(0))
                    Else
                        field.Operator = OP.OP_BETWEEN
                        field.Value = Formats.ParseDate(items(0))
                        field.Value1 = Formats.ParseDate(items(1))
                    End If
                Else
                    field.Value = Formats.ParseDate(items(0))
                    field.Operator = OP.OP_EQ
                End If
            End Sub

            Public Sub ParseCursorField(ByVal field As CCursorField(Of Double), ByVal text As String)
                text = Trim(text)
                field.Clear()
                If (text = vbNullString) Then Exit Sub

                Dim items() As String = Split(Trim(text), "...")
                If (UBound(items) > 0) Then
                    If (items(0) = "") Then
                        field.Operator = OP.OP_LE
                        field.Value = Formats.ParseDouble(items(1))
                    ElseIf items(1) = "" Then
                        field.Operator = OP.OP_GE
                        field.Value = Formats.ParseDouble(items(0))
                    Else
                        field.Operator = OP.OP_BETWEEN
                        field.Value = Formats.ParseDouble(items(0))
                        field.Value1 = Formats.ParseDouble(items(1))
                    End If
                Else
                    field.Value = Formats.ParseDouble(items(0))
                    field.Operator = OP.OP_EQ
                End If
            End Sub

            Public Sub ParseCursorField(ByVal field As CCursorField(Of Decimal), ByVal text As String)
                text = Trim(text)
                field.Clear()
                If (text = vbNullString) Then Exit Sub

                Dim items() As String = Split(Trim(text), "...")
                If (UBound(items) > 0) Then
                    If (items(0) = "") Then
                        field.Operator = OP.OP_LE
                        field.Value = Formats.ParseDouble(items(1))
                    ElseIf items(1) = "" Then
                        field.Operator = OP.OP_GE
                        field.Value = Formats.ParseDouble(items(0))
                    Else
                        field.Operator = OP.OP_BETWEEN
                        field.Value = Formats.ParseDouble(items(0))
                        field.Value1 = Formats.ParseDouble(items(1))
                    End If
                Else
                    field.Value = Formats.ParseDouble(items(0))
                    field.Operator = OP.OP_EQ
                End If
            End Sub


            Public Sub ParseCursorField(ByVal field As CCursorField(Of Integer), ByVal text As String)
                Dim items() As String
                text = Trim(text)
                field.Clear()
                If (text = vbNullString) Then Exit Sub

                If (InStr(text, vbTab) > 0) Then
                    Dim arr As New System.Collections.ArrayList
                    items = Split(text, vbTab)
                    If (Arrays.Len(items) > 0) Then
                        For i As Integer = 0 To UBound(items)
                            If (Trim(items(i)) <> "") Then arr.Add(Formats.ToInteger(items(i)))
                        Next
                    End If
                    If (arr.Count > 0) Then field.ValueIn(arr.ToArray)
                Else
                    items = Split(Trim(text), "...")
                    If (UBound(items) > 0) Then
                        If (items(0) = "") Then
                            field.Operator = OP.OP_LE
                            field.Value = Formats.ParseInteger(items(1))
                        ElseIf items(1) = "" Then
                            field.Operator = OP.OP_GE
                            field.Value = Formats.ParseInteger(items(0))
                        Else
                            field.Operator = OP.OP_BETWEEN
                            field.Value = Formats.ParseInteger(items(0))
                            field.Value1 = Formats.ParseInteger(items(1))
                        End If
                    Else
                        field.Value = Formats.ParseInteger(items(0))
                        field.Operator = OP.OP_EQ
                    End If

                End If
            End Sub

            Public Function CreateElencoUtenti(ByVal selValue As CUser, Optional ByVal onlyValid As Boolean = True) As String
                Dim writer As New System.Text.StringBuilder
                Dim t = False, t1 As Boolean = False
                Dim col As New CCollection(Of CUser)
                Dim lastName As String = vbNullString
                For Each user As CUser In Sistema.Users.LoadAll
                    If user.Visible AndAlso (onlyValid = False OrElse user.UserStato = UserStatus.USER_ENABLED) Then
                        col.Add(user)
                    End If
                Next
                col.Sort()

                For Each user As CUser In col
                    t = GetID(user) = GetID(selValue)
                    t1 = t1 OrElse t
                    If (lastName = user.Nominativo) Then
                        writer.Append("<option value=""")
                        writer.Append(GetID(user))
                        writer.Append(""" ")
                        If (t) Then writer.Append("selected")
                        writer.Append("")
                        writer.Append(">")
                        writer.Append(Strings.HtmlEncode(user.Nominativo))
                        writer.Append(" (")
                        writer.Append(user.UserName)
                        writer.Append(")")
                        writer.Append("</option>")
                    Else
                        writer.Append("<option value=""")
                        writer.Append(GetID(user))
                        writer.Append(""" ")
                        If (t) Then writer.Append("selected")
                        writer.Append(">")
                        writer.Append(Strings.HtmlEncode(user.Nominativo))
                        writer.Append("</option>")
                    End If
                Next
                If selValue IsNot Nothing AndAlso t1 = False Then
                    writer.Append("<option value=""")
                    writer.Append(GetID(selValue))
                    writer.Append(""" selected style=""color:red;"">")
                    writer.Append(Strings.HtmlEncode(selValue.UserName))
                    writer.Append("</option>")
                End If

                Return writer.ToString
            End Function

            Public Sub SendPasswordsToEMail(ByVal eMail As String)
                eMail = Trim(eMail)
                If (eMail = "") Then Throw New ArgumentNullException("eMail")

                Dim cursor As New CUserCursor
                Dim writer As New System.Text.StringBuilder
                Try
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor.eMail.Value = eMail
                    cursor.UserName.SortOrder = SortEnum.SORT_ASC
                    cursor.IgnoreRights = True

                    If (cursor.Count = 0) Then Throw New ArgumentException("Non esiste alcun account associato a questo indirizzo e-mail")

                    writer.Append("<html>")
                    writer.Append("<body>")
                    writer.Append("<h2>Procedura di recupero dati degli account</h2>")
                    writer.Append("&Egrave; stata effettuata una richiesta per recuperare gli account dell'utente sul sito " & "<a href=""" & ApplicationContext.BaseURL & """>" & ApplicationContext.Title & "</a><br/>")
                    writer.Append("<br/>")
                    writer.Append("<table style=""border-collapse:collapse;"" border=""1"" cellsapcing=""0"" cellpadding=""0"">")
                    writer.Append("<tr>")
                    writer.Append("<th style=""width:120px;"">Username</th>")
                    writer.Append("<th style=""width:120px;"">Password</th>")
                    writer.Append("</tr>")
                    While Not cursor.EOF
                        writer.Append("<tr>")
                        writer.Append("<td style=""width:120px;"">" & Strings.HtmlEncode(cursor.Item.UserName) & "</td>")
                        writer.Append("<td>" & Strings.HtmlEncode(Formats.ToString(APPConn.ExecuteScalar("SELECT [UsrPwd] FROM [Login] WHERE [ID]=" & GetID(cursor.Item)))) & "</td>")
                        writer.Append("</tr>")
                        cursor.MoveNext()
                    End While
                    cursor.Dispose()
                    cursor = Nothing

                    writer.Append("</table>")

                    writer.Append("<table>")
                    writer.Append("<tr>")
                    writer.Append("   <td>Data: " & Now & "</td>")
                    writer.Append("</tr>")
                    writer.Append("<tr>")
                    writer.Append("   <td>Sessione: " & WebSite.ASP_Session.SessionID & "</td>")
                    writer.Append("</tr>")
                    writer.Append("<tr>")
                    writer.Append("   <td>IP Remoto: " & WebSite.Instance.CurrentSession.RemoteIP & ":" & WebSite.Instance.CurrentSession.RemotePort & "</td>")
                    writer.Append("</tr>")
                    writer.Append("</table>")
                    writer.Append("<BR/>")

                    writer.Append("</body>")
                    writer.Append("</html>")

                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                    Throw
                Finally
                    If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                End Try

                Dim m As System.Net.Mail.MailMessage = DMD.Sistema.EMailer.PrepareMessage(EMailer.Config.SMTPUserName, eMail, "", "", ApplicationContext.Title & " - Procedura di recupero password", writer.ToString, "", True)
                DMD.Sistema.EMailer.SendMessageAsync(m, True)

                'writer.Dispose()
            End Sub

            Public Sub SendUserPassword(ByVal user As CUser)
                Dim writer As New System.Text.StringBuilder
                If (user Is Nothing) Then Throw New ArgumentNullException("User")

                writer.Append("<html>")
                writer.Append("<body>")
                writer.Append("<h2>Sistema di recupero password</h2>")
                writer.Append("Si prega di non rispondere a questa e-mail.<br/>")
                writer.Append("&Egrave; stata effettuata una richiesta per recuperare la password associata all'utente: <b>" & user.Nominativo & "</b>")
                writer.Append("<table>")
                writer.Append("<tr>")
                writer.Append("<th>UserName</th>")
                writer.Append("<th>Password</th>")
                writer.Append("</tr>")
                writer.Append("<tr>")
                writer.Append("<td>" & Strings.HtmlEncode(user.UserName) & "</td>")
                writer.Append("<td>" & Strings.HtmlEncode(Formats.ToString(APPConn.ExecuteScalar("SELECT [UsrPwd] FROM [Login] WHERE [ID]=" & GetID(user)))) & "</td>")
                writer.Append("</tr>")
                writer.Append("</table>")
                writer.Append("<br/>")
                writer.Append("<hr/>")

                writer.Append("<table>")
                writer.Append("<tr>")
                writer.Append("<td>Data</td>")
                writer.Append("<td>" & Now & "</td>")
                writer.Append("</tr>")
                writer.Append("<tr>")
                writer.Append("<td>Sessione</td>")
                writer.Append("<td>" & WebSite.Instance.CurrentSession.SessionID & "</td>")
                writer.Append("</tr>")
                writer.Append("<tr>")
                writer.Append("<td>IP Remoto</td>")
                writer.Append("<td>" & WebSite.Instance.CurrentSession.RemoteIP & ":" & WebSite.Instance.CurrentSession.RemotePort & "</td>")
                writer.Append("</tr>")
                writer.Append("<tr>")
                writer.Append("<td>Sistema</td>")
                writer.Append("<td><a href=""" & ApplicationContext.BaseURL & """>" & ApplicationContext.Title & "</a></td>")
                writer.Append("</tr>")
                writer.Append("</table>")
                writer.Append("</body>")
                writer.Append("</html>")

                Dim m As System.Net.Mail.MailMessage = EMailer.PrepareMessage(EMailer.Config.SMTPUserName, user.eMail, "", "", ApplicationContext.Title & ": Procedura di recupero password per l'utente " & user.Nominativo, writer.ToString, "", True)
                EMailer.SendMessageAsync(m, True)

                'writer.Dispose()
            End Sub

            Public Function CreateElencoPossibiliGenitori(ByVal objId As Integer, ByVal selValue As Integer) As String
                Dim cursor As New CModulesCursor
                Dim writer As New System.Text.StringBuilder
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.ID.Value = objId
                cursor.ID.Operator = OP.OP_NE
                cursor.DisplayName.SortOrder = SortEnum.SORT_ASC
                While Not cursor.EOF
                    Dim item As CModule = cursor.Item
                    writer.Append("<option value=""")
                    writer.Append(GetID(item))
                    writer.Append(""" ")
                    If (GetID(item) = selValue) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(item.DisplayName))
                    writer.Append("</option>")
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                Dim ret As String = writer.ToString
                'writer.Dispose()
                Return ret
            End Function

            Public Function CreateElencoStatoUtente(ByVal value As UserStatus) As String
                Dim values As UserStatus() = {-1, UserStatus.USER_DISABLED, UserStatus.USER_DELETED, UserStatus.USER_NEW, UserStatus.USER_SUSPENDED, UserStatus.USER_ENABLED}
                Dim names As String() = {"Tutto", "Disabilitati", "Eliminati", "Da Attivare", "Sospesi", "Abilitati"}
                Dim i As Integer
                Dim writer As New System.Text.StringBuilder
                For i = 0 To UBound(values)
                    writer.Append("<option value=""")
                    writer.Append(values(i))
                    writer.Append(""" ")
                    If (values(i) = value) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(names(i)))
                    writer.Append("</option>")
                Next

                Return writer.ToString
            End Function

            Public Function CreateElencoGruppo(ByVal value As Integer) As String
                Dim cursor As CGroupCursor
                Dim writer As New System.Text.StringBuilder

                cursor = New CGroupCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.GroupName.SortOrder = SortEnum.SORT_ASC

                writer.Append("<option></option>")
                While Not cursor.EOF
                    writer.Append("<option value=""")
                    writer.Append(GetID(cursor.Item))
                    writer.Append(""" ")
                    If (GetID(cursor.Item) = value) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(cursor.Item.GroupName))
                    writer.Append("</option>")
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                Return writer.ToString
            End Function

            ''' <summary>
            ''' Crea la sequenza di &gt;option&lt; relativa ai periodo supportati dal calendario
            ''' </summary>
            ''' <param name="selValue"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Function CreateElencoPeriodi(selValue As String) As String
                Dim items() As String = DateUtils.GetSupportedPeriods()
                Return CreateElenco(items, selValue)
            End Function

            Public Function CreateElencoPriority(ByVal currValue As PriorityEnum) As String
                Dim i As Integer
                Dim writer As New System.Text.StringBuilder

                For i = LBound(priority_values) To UBound(priority_values)
                    If priority_values(i) = currValue Then
                        writer.Append("<option value=""")
                        writer.Append(priority_values(i))
                        writer.Append(""" selected=""selected"">")
                        writer.Append(priority_names(i))
                        writer.Append("</option>")
                    Else
                        writer.Append("<option value=""")
                        writer.Append(priority_values(i))
                        writer.Append(""">")
                        writer.Append(priority_names(i))
                        writer.Append("</option>")
                    End If
                Next

                Return writer.ToString
            End Function

            '---------------------------------------------------------
            Public Function GetPriorityText(ByVal value As PriorityEnum) As String
                Dim ret As String
                Select Case value
                    Case -1 : ret = "Alta"
                    Case 0 : ret = "Normale"
                    Case 1 : ret = "Bassa"
                    Case Else
                        If (value <= -2) Then
                            ret = "Altissima"
                        Else
                            ret = "Bassissima"
                        End If
                End Select
                Return ret
            End Function


            Public Function GetPriorityIcon(ByVal currValue As PriorityEnum) As String
                Dim i As Integer
                For i = 0 To UBound(priority_values)
                    If priority_values(i) = currValue Then
                        Return "/widgets/images/" & priority_icons(i)
                    End If
                Next
                Return vbNullString
            End Function

            Public Function GetFriendlyContextName(ByVal value As String) As String
                Dim values As String() = {"CRapportino"}
                Dim names As String() = {"Pratica"}
                Dim p As Integer = Strings.IndexOf(value, "(")
                Dim tipoContesto As String = Strings.Trim(Strings.Left(value, p - 1))
                Dim idContesto As String = Strings.Mid(value, p + 1, Strings.Len(value) - 1 - p)
                Dim i As Integer = Arrays.IndexOf(values, tipoContesto)
                If (i >= 0) Then tipoContesto = names(i)
                Return tipoContesto & " (" & idContesto & ")"
            End Function

            Public Function CreateElencoTipoContestoAnnotazione(ByVal obj As Object, ByVal selValue As String) As String
                Dim tipi As CCollection(Of String) = Annotazioni.GetTipiContestoPerOggetto(obj)
                Dim writer As New System.Text.StringBuilder
                Dim t = False, t1 As Boolean = False
                selValue = Strings.Trim(selValue)
                For Each tipo As String In tipi
                    t = (tipo = selValue)
                    t1 = t1 OrElse t
                    writer.Append("<option value=""")
                    writer.Append(Strings.HtmlEncode(tipo))
                    writer.Append(""" ")
                    If (t) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(SystemUtils.GetFriendlyContextName(tipo)))
                    writer.Append("</option>")
                Next
                If (selValue <> "" AndAlso Not t1) Then
                    writer.Append("<option value=""")
                    writer.Append(Strings.HtmlEncode(selValue))
                    writer.Append(""" selected style=""colo:red;"">")
                    writer.Append(Strings.HtmlEncode(SystemUtils.GetFriendlyContextName(selValue)))
                    writer.Append("</option>")
                End If
                Return writer.ToString
            End Function

            Protected Overrides Sub Finalize()
                MyBase.Finalize()
                DMD.DMDObject.DecreaseCounter(Me)
            End Sub
        End Class

        Private Shared m_SystemUtils As CSystemUtilsClass = Nothing

        Public Shared ReadOnly Property SystemUtils As CSystemUtilsClass
            Get
                If (m_SystemUtils Is Nothing) Then m_SystemUtils = New CSystemUtilsClass
                Return m_SystemUtils
            End Get
        End Property
    End Class


End Namespace