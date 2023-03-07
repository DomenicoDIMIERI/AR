Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Forms.Utils




Namespace Forms

    Partial Public Class Utils

        Public NotInheritable Class CPersoneUtilsClass
            Public Sub New()
                DMD.DMDObject.IncreaseCounter(Me)
            End Sub

            Public Function CreateElencoUfficiConsentiti(ByVal selItem As CUfficio) As String
                Dim items As CCollection(Of CUfficio) = Anagrafica.Uffici.GetPuntiOperativiConsentiti
                Dim ret As New System.Text.StringBuilder
                Dim t = False, t1 As Boolean = False
                For i As Integer = 0 To items.Count - 1
                    Dim u As CUfficio = items(i)
                    If (u.Attivo) Then
                        t1 = GetID(u) = GetID(selItem)
                        t = t OrElse t1
                        ret.Append("<option value=""" & GetID(u) & """ " & CStr(IIf(t1, "selected", "")) & ">" & Strings.HtmlEncode(u.Nome) & "</option>")
                    End If
                Next
                If (t = False) AndAlso (selItem IsNot Nothing) Then
                    ret.Append("<option value=""" & GetID(selItem) & """ selected style=""color:red;"">" & Strings.HtmlEncode(selItem.Nome) & "</option>")
                End If
                Return ret.ToString
            End Function

            Public Function CreateElencoTipoCanale(ByVal selValue As String, Optional ByVal onlyValid As Boolean = True) As String
                Dim writer As New System.Text.StringBuilder
                Dim t = False, t1 As Boolean = False
                Dim items() As String = Anagrafica.Canali.GetTipiCanale(onlyValid)
                selValue = Trim(selValue)
                For i = 0 To Arrays.Len(items) - 1
                    Dim str As String = items(i)
                    t = Strings.Compare(str, selValue, CompareMethod.Text) = 0
                    t1 = t1 OrElse t
                    writer.Append("<option value=""")
                    writer.Append(Strings.HtmlEncode(str))
                    writer.Append(""" ")
                    If (t) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(str))
                    writer.Append("</option>")
                Next
                If (selValue <> "") AndAlso (Not t1) Then
                    writer.Append("<option value=""")
                    writer.Append(Strings.HtmlEncode(selValue))
                    writer.Append(""" selected style=""color:red;"">")
                    writer.Append(Strings.HtmlEncode(selValue))
                    writer.Append("</option>")
                End If
                Return writer.ToString
            End Function

            Public Function CreateElencoCanali(ByVal tipo As String, ByVal selValue As CCanale, Optional ByVal onlyValid As Boolean = True) As String
                Dim writer As New System.Text.StringBuilder
                Dim t = False, t1 As Boolean = False
                Dim cursor As New CCanaleCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                If (onlyValid) Then cursor.Valid.Value = True
                cursor.Nome.SortOrder = SortEnum.SORT_ASC
                cursor.Tipo.Value = Trim(tipo)
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    Dim item As CCanale = cursor.Item
                    t = GetID(item) = GetID(selValue)
                    t1 = t1 OrElse t
                    writer.Append("<option value=""")
                    writer.Append(GetID(item))
                    writer.Append(""" ")
                    If (t) Then writer.Append("selected")
                    writer.Append(" imgattr=""")
                    writer.Append(Strings.URLEncode(item.IconURL))
                    writer.Append(""">")
                    writer.Append(Strings.HtmlEncode(item.Nome))
                    writer.Append("</option>")
                    cursor.MoveNext()
                End While
                cursor.Dispose()
                If (selValue IsNot Nothing) AndAlso (Not t1) Then
                    writer.Append("<option value=""")
                    writer.Append(GetID(selValue))
                    writer.Append(""" selected style=""color:red;"" imgattr=""")
                    writer.Append(Strings.URLEncode(selValue.IconURL))
                    writer.Append(""">")
                    writer.Append(Strings.HtmlEncode(selValue.Nome))
                    writer.Append("</option>")
                End If

                Return writer.ToString
            End Function

            Public Function CreateElencoFonti(ByVal tipoFonte As String, ByVal fonte As IFonte, Optional ByVal onlyValid As Boolean = True) As String
                Dim provider As IFonteProvider = Anagrafica.Fonti.GetProviderByName(tipoFonte)
                Dim writer As New System.Text.StringBuilder
                Dim t = False, t1 As Boolean = False
                If provider IsNot Nothing Then
                    Dim items As IFonte() = provider.GetItemsAsArray(tipoFonte, onlyValid)
                    If items IsNot Nothing Then
                        For i As Integer = 0 To items.Count - 1
                            Dim item As IFonte = items(i)
                            t = GetID(item) = GetID(fonte)
                            t1 = t1 OrElse t
                            writer.Append("<option value=""")
                            writer.Append(GetID(item))
                            writer.Append(""" ")
                            If (t) Then writer.Append("selected")
                            writer.Append(" imgattr=""")
                            writer.Append(Strings.URLEncode(item.IconURL))
                            writer.Append(""">")
                            writer.Append(Strings.HtmlEncode(item.Nome))
                            writer.Append("</option>")
                        Next
                    End If
                End If
                If (fonte IsNot Nothing) AndAlso (Not t1) Then
                    writer.Append("<option value=""")
                    writer.Append(GetID(fonte))
                    writer.Append(""" selected style=""color:red;"" imgattr=""")
                    writer.Append(Strings.URLEncode(fonte.IconURL))
                    writer.Append(""">")
                    writer.Append(Strings.HtmlEncode(fonte.Nome))
                    writer.Append("</option>")
                End If

                Return writer.ToString
            End Function

            Public Function CreateElencoTipoFonte(ByVal selValue As String) As String
                Dim col As New System.Collections.ArrayList
                Dim fontiP As CCollection(Of IFonteProvider) = Anagrafica.Fonti.Providers
                For i As Integer = 0 To fontiP.Count - 1
                    Dim p As IFonteProvider = fontiP(i)
                    Dim names() As String = p.GetSupportedNames
                    For j As Integer = 0 To UBound(names)
                        col.Add(names(j))
                    Next
                Next
                col.Sort()

                Dim buffer As New System.Text.StringBuilder

                Dim t = False, t1 As Boolean = False
                selValue = Trim(selValue)
                For Each n As String In col
                    t = n = selValue
                    t1 = t1 OrElse t
                    buffer.Append("<option value=""")
                    buffer.Append(Strings.HtmlEncode(n))
                    buffer.Append(""" ")
                    buffer.Append(CStr(IIf(t, "selected", "")))
                    buffer.Append(">")
                    buffer.Append(Strings.HtmlEncode(n))
                    buffer.Append("</option>")
                Next

                If (selValue <> vbNullString) AndAlso Not t1 Then
                    buffer.Append("<option value=""")
                    buffer.Append(Strings.HtmlEncode(selValue))
                    buffer.Append(""" selected style=""color:red;"">")
                    buffer.Append(Strings.HtmlEncode(selValue))
                    buffer.Append("</option>")
                End If

                Return buffer.ToString
            End Function

            Public Function CreateElencoEntiPaganti(ByVal selValue As CAzienda) As String
                Dim cursor As New CAziendeCursor
                Dim writer As New System.Text.StringBuilder
                Dim t = False, t1 As Boolean = False
                cursor.FormaGiuridica.Value = "Ente"
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Cognome.SortOrder = SortEnum.SORT_ASC

                While Not cursor.EOF
                    t = (GetID(selValue) = GetID(cursor.Item))
                    t1 = t1 OrElse t
                    writer.Append("<option value=""")
                    writer.Append(GetID(cursor.Item))
                    writer.Append(""" ")
                    writer.Append(CStr(IIf(t, "selected", "")))
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(cursor.Item.Nominativo))
                    writer.Append("</option>")
                    cursor.MoveNext()
                End While
                cursor.Dispose()

                If (selValue IsNot Nothing) AndAlso t1 = False Then
                    writer.Append("<option value=""")
                    writer.Append(GetID(selValue))
                    writer.Append(""" selected style=""color:red;"">")
                    writer.Append(Strings.HtmlEncode(selValue.Nominativo))
                    writer.Append("</option>")
                End If

                Return writer.ToString
            End Function

            Public Function CreateElencoListeRicontatto(ByVal selValue As String) As String
                Dim liste As CCollection(Of CListaRicontatti) = ListeRicontatto.GetListeRicontatto
                Dim writer As New System.Text.StringBuilder
                Dim t = False, t1 As Boolean = False
                For i As Integer = 0 To liste.Count - 1
                    Dim l As CListaRicontatti = liste(i)
                    Dim str As String = l.Name
                    If (str <> vbNullString) Then
                        t = str = selValue
                        t1 = t1 OrElse t
                        writer.Append("<option value=""")
                        writer.Append(Strings.HtmlEncode(str))
                        writer.Append(""" ")
                        writer.Append(CStr(IIf(t, "selected", "")))
                        writer.Append(">")
                        writer.Append(Strings.HtmlEncode(str))
                        writer.Append("</option>")
                        If ((selValue <> vbNullString) And Not t1) Then
                            writer.Append("<option value=""")
                            writer.Append(Strings.HtmlEncode(selValue))
                            writer.Append(""" selected style=""color:red;"">")
                            writer.Append(Strings.HtmlEncode(selValue))
                            writer.Append("</option>")
                        End If
                    End If
                Next
                Return writer.ToString
            End Function


            Public Function CreateElencoNomeTelefonoHTML(ByVal selValue As String) As String
                Dim items() As String = {"Telefono", "Telefono (casa)", "Telefono (lavoro)", "Cellulare", "Cellulare (casa)", "Cellulare (lavoro)", "Fax", "Fax (casa)", "Fax (lavoro)"}
                Dim t As Boolean = False
                Dim writer As New System.Text.StringBuilder
                For i As Integer = 0 To UBound(items)
                    t = (items(i) = selValue)
                    writer.Append("<option value=""")
                    writer.Append(Strings.HtmlEncode(items(i)))
                    writer.Append(""" ")
                    writer.Append(CStr(IIf(t, "selected", "")))
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(items(i)))
                    writer.Append("</option>")
                Next
                If ((Strings.Trim(selValue) <> "") And Not t) Then
                    writer.Append("<option value=""")
                    writer.Append(Strings.HtmlEncode(selValue))
                    writer.Append(""" selected>")
                    writer.Append(Strings.HtmlEncode(selValue))
                    writer.Append("</option>")
                End If

                Return writer.ToString
            End Function



            Public Function CreateElencoNomeWebHTML(ByVal selValue As String) As String
                Dim items() As String = {"e-mail", "web site"}
                Dim t As Boolean = False
                Dim writer As New System.Text.StringBuilder
                For i As Integer = 0 To UBound(items)
                    t = (items(i) = selValue)
                    writer.Append("<option value=""")
                    writer.Append(Strings.HtmlEncode(items(i)))
                    writer.Append(""" ")
                    If (t) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(items(i)))
                    writer.Append("</option>")
                Next
                If ((Strings.Trim(selValue) <> "") And Not t) Then
                    writer.Append("<option value=""")
                    writer.Append(Strings.HtmlEncode(selValue))
                    writer.Append(""" selected>")
                    writer.Append(Strings.HtmlEncode(selValue))
                    writer.Append("</option>")
                End If

                Return writer.ToString
            End Function

            'Public Sub UpdateContattoTelefonico(ByVal name As String, ByVal contatto As CContatto)
            '    contatto.Nome = Sistema.ApplicationContext.GetParameter(renderer, "txtNTel" & name, vbNullString)
            '    contatto.Valore = Formats.ParsePhoneNumber(Sistema.ApplicationContext.GetParameter(renderer, "txtVTel" & name, vbNullString))
            '    contatto.Validated = Sistema.ApplicationContext.GetParameter(renderer, "txtCTel" & name, "0") <> "0"
            'End Sub

            'Public Sub UpdateContattoWeb(ByVal name As String, ByVal contatto As CContatto)
            '    contatto.Nome = Sistema.ApplicationContext.GetParameter(renderer, "txtNWeb" & name, vbNullString)
            '    contatto.Valore = Sistema.ApplicationContext.GetParameter(renderer, "txtVWeb" & name, vbNullString)
            '    contatto.Validated = Sistema.ApplicationContext.GetParameter(renderer, "txtCWeb" & name, "0") <> "0"
            'End Sub

            Public Function CreateElencoTipoAzienda(ByVal selValue As String) As String
                Dim cursor As New CTipologiaAziendaCursor
                Dim ret As String = vbNullString
                Dim t1, t As Boolean
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                selValue = Trim(selValue)
                t = False
                While Not cursor.EOF
                    t1 = (Strings.Compare(selValue, cursor.Item.Nome, CompareMethod.Text) = 0)
                    t = t Or t1
                    ret &= "<option value=""" & Strings.HtmlEncode(cursor.Item.Nome) & """ " & CStr(IIf(t1, "selected", "")) & ">" & Strings.HtmlEncode(cursor.Item.Nome) & "</option>"
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                If (selValue <> vbNullString) And Not t Then
                    ret &= "<option value=""" & Strings.HtmlEncode(selValue) & """ selected>" & Strings.HtmlEncode(selValue) & "</option>"
                End If
                Return ret
            End Function

            Public Function CreateElencoCategorieAzienda(ByVal selValue As String) As String
                Dim cursor As New CCategorieAziendaCursor
                Dim ret As String = vbNullString
                Dim t1, t As Boolean
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = True
                selValue = Trim(selValue)
                t = False
                While Not cursor.EOF
                    t1 = (Strings.Compare(selValue, cursor.Item.Nome, CompareMethod.Text) = 0)
                    t = t Or t1
                    ret &= "<option value=""" & Strings.HtmlEncode(cursor.Item.Nome) & """ " & CStr(IIf(t1, "selected", "")) & ">" & Strings.HtmlEncode(cursor.Item.Nome) & "</option>"
                    cursor.MoveNext()
                End While
                cursor.Dispose()
                If (selValue <> vbNullString) And Not t Then
                    ret &= "<option value=""" & Strings.HtmlEncode(selValue) & """ selected>" & Strings.HtmlEncode(selValue) & "</option>"
                End If
                Return ret
            End Function

            Public Function CreateElencoFormeGiuridicheAzienda(ByVal selValue As String) As String
                selValue = Trim(selValue)

                Dim ret As New System.Text.StringBuilder

                Dim cursor As New CFormeGiuridicheAziendaCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = True
                cursor.Nome.SortOrder = SortEnum.SORT_ASC

                Dim t1 = False, t As Boolean = False
                While Not cursor.EOF
                    t1 = (Strings.Compare(selValue, cursor.Item.Nome, CompareMethod.Text) = 0)
                    t = t Or t1
                    ret.Append("<option value=""")
                    ret.Append(Strings.HtmlEncode(cursor.Item.Nome))
                    ret.Append(""" ")
                    If (t1) Then ret.Append("selected")
                    ret.Append(">")
                    ret.Append(Strings.HtmlEncode(cursor.Item.Nome))
                    ret.Append("</option>")
                    cursor.MoveNext()
                End While
                cursor.Dispose()
                cursor = Nothing

                If (selValue <> vbNullString) And Not t Then
                    ret.Append("<option value=""")
                    ret.Append(Strings.HtmlEncode(selValue))
                    ret.Append(""" selected>")
                    ret.Append(Strings.HtmlEncode(selValue))
                    ret.Append("</option>")
                End If

                Return ret.ToString
            End Function


            Public Function CreateElencoSesso(ByVal value As String) As String
                Dim items() As String = {"M", "F"}
                Dim writer As New System.Text.StringBuilder
                For i As Integer = 0 To UBound(items)
                    writer.Append("<option value=""")
                    writer.Append(Strings.HtmlEncode(items(i)))
                    writer.Append(""" ")
                    If (items(i) = value) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(items(i)))
                    writer.Append("</option>")
                Next
                Return writer.ToString
            End Function

            Public Function CreateElencoStatiCivili(ByVal selValue As String) As String
                Dim items() As String = {"Single", "Sposato", "Divorziato", "Vedovo"}
                Dim writer As New System.Text.StringBuilder
                Dim t = False, t1 As Boolean = False
                For i As Integer = 0 To UBound(items)
                    t = selValue = items(i)
                    t1 = t1 OrElse t
                    writer.Append("<option value=""")
                    writer.Append(Strings.HtmlEncode(items(i)))
                    writer.Append(""" ")
                    If (t) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(items(i)))
                    writer.Append("</option>")
                Next
                If Not t1 AndAlso selValue <> vbNullString Then
                    writer.Append("<option value=""")
                    writer.Append(Strings.HtmlEncode(selValue))
                    writer.Append(""" selected>")
                    writer.Append(Strings.HtmlEncode(selValue))
                    writer.Append("</option>")
                End If
                Return writer.ToString
            End Function

            'Public Function CreateElencoProfessioni(ByVal value As String) As String
            '    Dim dbRis As System.Data.IDataReader = Nothing
            '    Try
            '        dbRis = APPConn.ExecuteReader("SELECT [Professione] FROM [tbl_PersoneProfessioni] ORDER BY [Professione] ASC")
            '        Dim writer As New System.Text.StringBuilder
            '        While dbRis.Read
            '            Dim p As String = Formats.ToString(dbRis("Professione"))
            '            writer.Append("<option value=""")
            '            writer.Append(Strings.HtmlEncode(p))
            '            writer.Append(""" ")
            '            If (p = value) Then writer.Append("selected")
            '            writer.Append(">")
            '            writer.Append(Strings.HtmlEncode(p))
            '            writer.Append("</option>")
            '        End While

            '        Return writer.ToString
            '    Catch ex As Exception
            '        Sistema.Events.NotifyUnhandledException(ex)
            '        Throw
            '    Finally
            '        If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            '    End Try
            'End Function

            'Public Function CreateElencoTipoRapporto(ByVal selValue As String) As String
            '    'Dim items() As String = {"", "Ferrovia", "Medico", "Parapubblico", "Pensionato", "Pubblico", "Statale", "Privato"}
            '    Dim t1 = False, t As Boolean = False
            '    Dim writer As New System.Text.StringBuilder
            '    Dim cursor As New CTipoRapportoCursor
            '    cursor.Descrizione.SortOrder = SortEnum.SORT_ASC
            '    cursor.IgnoreRights = True
            '    'For i As Integer = 0 To UBound(items)
            '    While Not cursor.EOF
            '        Dim item As CTipoRapporto = cursor.Item
            '        t = (item.Descrizione = selValue) Or (item.IdTipoRapporto = selValue)
            '        t1 = t1 OrElse t
            '        writer.Append("<option value=""")
            '        writer.Append(Strings.HtmlEncode(item.IdTipoRapporto))
            '        writer.Append(""" ")
            '        If (t) Then writer.Append("selected")
            '        writer.Append(">")
            '        writer.Append(Strings.HtmlEncode(item.ToString))
            '        writer.Append("</option>")
            '        cursor.MoveNext()
            '    End While
            '    If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            '    If ((Strings.Trim(selValue) <> "") AndAlso Not t1) Then
            '        writer.Append("<option value=""")
            '        writer.Append(Strings.HtmlEncode(selValue))
            '        writer.Append(""" selected>")
            '        writer.Append(Strings.HtmlEncode(selValue))
            '        writer.Append("</option>")
            '    End If
            '    Return writer.ToString
            'End Function

            'Public Function CreateElencoPosizioni(ByVal selValue As String) As String
            '    Dim items() As String = {"Impiegato", "Pensionato", "Operaio", "Dirigente", "Amministratore", "Responsabile", "Disoccupato", "Carabiniere", "Poliziotto", "Militare", "Docente"}
            '    selValue = Trim(selValue)
            '    Array.Sort(items)
            '    Dim t = False, t1 As Boolean = False
            '    Dim writer As New System.Text.StringBuilder
            '    For i As Integer = 0 To UBound(items)
            '        t = (items(i) = selValue)
            '        t1 = t1 OrElse t
            '        writer.Append("<option value=""")
            '        writer.Append(Strings.HtmlEncode(items(i)))
            '        writer.Append(""" ")
            '        If (t) Then writer.Append("selected")
            '        writer.Append(">")
            '        writer.Append(Strings.HtmlEncode(items(i)))
            '        writer.Append("</option>")
            '    Next
            '    If ((selValue <> vbNullString) And Not t1) Then
            '        writer.Append("<option value=""")
            '        writer.Append(Strings.HtmlEncode(selValue))
            '        writer.Append(""" selected style=""color:Red;"">")
            '        writer.Append(Strings.HtmlEncode(selValue))
            '        writer.Append("</option>")
            '    End If
            '    Return writer.ToString
            'End Function

            'Public Function CreateElencoTitoli(ByVal value As String) As String
            '    Dim dbRis As System.Data.IDataReader = APPConn.ExecuteReader("SELECT [Titolo] FROM [tbl_PersoneTitoli] ORDER BY [Titolo] ASC")
            '    Dim writer As New System.Text.StringBuilder
            '    While dbRis.Read
            '        Dim t As String = Formats.ToString(dbRis("Titolo"))
            '        writer.Append("<option value=""")
            '        writer.Append(Strings.HtmlEncode(t))
            '        writer.Append(""" ")
            '        If (t = value) Then writer.Append("selected")
            '        writer.Append(">")
            '        writer.Append(Strings.HtmlEncode(t))
            '        writer.Append("</option>")
            '    End While
            '    dbRis.Dispose() : dbRis = Nothing
            '    Return writer.ToString
            'End Function


            Public Function CreateElencoUffici(ByVal selValue As CUfficio) As String
                Dim items As CCollection(Of CUfficio) = Anagrafica.Uffici.GetPuntiOperativiConsentiti
                Dim writer As New System.Text.StringBuilder
                For i As Integer = 0 To items.Count - 1
                    Dim item As CUfficio = items(i)
                    If (item.Attivo) Then
                        writer.Append("<option value=""")
                        writer.Append(GetID(item))
                        writer.Append(""" ")
                        If (GetID(selValue) = GetID(item)) Then writer.Append("selected")
                        writer.Append(">")
                        writer.Append(Strings.HtmlEncode(item.Nome))
                        writer.Append("</option>")
                    End If
                Next
                Return writer.ToString
            End Function

            Public Function CreateElencoOperatoriPerUfficio(ByVal ufficio As CUfficio, ByVal selValue As CUser, Optional ByVal onlyValid As Boolean = True) As String
                Dim writer As New System.Text.StringBuilder
                If (ufficio Is Nothing) Then
                    Return SystemUtils.CreateElencoUtenti(selValue, onlyValid)
                Else
                    For i As Integer = 0 To ufficio.Utenti.Count - 1
                        Dim item As CUser = ufficio.Utenti(i)
                        If (Not onlyValid OrElse item.UserStato = UserStatus.USER_ENABLED) Then
                            writer.Append("<option value=""")
                            writer.Append(GetID(item))
                            writer.Append(""" ")
                            If (GetID(selValue) = GetID(item)) Then writer.Append("selected")
                            writer.Append(">")
                            writer.Append(Strings.HtmlEncode(item.Nominativo))
                            writer.Append("</option>")
                        End If
                    Next
                End If

                Return writer.ToString
            End Function

            Public Function GetCategorieRicontatto() As String()
                Return {"Urgente", "Importante", "Normale", "Poco importante"}
            End Function

            Public Function ColorFromCategoria(ByVal value As String) As String
                Select Case (value)
                    Case "Urgente" : Return "red"
                    Case "Importante" : Return "orange"
                    Case "Normale" : Return "white"
                    Case "Poco importante" : Return "#e0e0e0"
                    Case Else : Return "white"
                End Select
            End Function

            Protected Overrides Sub Finalize()
                MyBase.Finalize()
                DMD.DMDObject.DecreaseCounter(Me)
            End Sub
        End Class

        Private Shared m_PersoneUtils As CPersoneUtilsClass = Nothing

        Public Shared ReadOnly Property PersoneUtils As CPersoneUtilsClass
            Get
                If (m_PersoneUtils Is Nothing) Then m_PersoneUtils = New CPersoneUtilsClass
                Return m_PersoneUtils
            End Get
        End Property

    End Class



End Namespace