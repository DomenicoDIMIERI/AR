Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms

    Partial Class Utils

        Public NotInheritable Class CCQSPDUtilsClass


            Public Function CreateElencoAssicurazioni(ByVal selValue As CAssicurazione) As String
                Dim writer As New System.Text.StringBuilder
                Dim t = False, t1 As Boolean = False

                For Each item As CAssicurazione In CQSPD.Assicurazioni.LoadAll
                    t = GetID(item) = GetID(selValue)
                    writer.Append("<option value=""")
                    writer.Append(GetID(item))
                    writer.Append(""" ")
                    If (t) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(item.Nome))
                    writer.Append("</option>")
                    t1 = t Or t1
                Next

                If (selValue IsNot Nothing AndAlso t1 = False) Then
                    writer.Append("<option value=""")
                    writer.Append(GetID(selValue))
                    writer.Append(""" selected style=""color:red;"">")
                    writer.Append(Strings.HtmlEncode(selValue.Nome))
                    writer.Append("</option>")
                End If

                Return writer.ToString
            End Function

            Public Function CreateElencoStatiTeamManager(ByVal selValue As StatoTeamManager) As String
                Dim values() As StatoTeamManager = New StatoTeamManager() {StatoTeamManager.STATO_ATTIVO, StatoTeamManager.STATO_DISABILITATO, StatoTeamManager.STATO_INATTIVAZIONE, StatoTeamManager.STATO_SOSPESO}
                Dim names() As String = New String() {"ATTIVO", "DISABILITATO", "IN ATTIVAZIONE", "SOSPESO"}
                Dim ret As New System.Text.StringBuilder
                For i As Integer = 0 To UBound(names)
                    ret.Append("<option value=""")
                    ret.Append(CInt(values(i)))
                    ret.Append(""" ")
                    If (selValue = values(i)) Then ret.Append("selected")
                    ret.Append(">")
                    ret.Append(Strings.HtmlEncode(names(i)))
                    ret.Append("</option>")
                Next
                Return ret.ToString
            End Function

            Public Function CreateElencoAreaManagers(ByVal selValue As CAreaManager, ByVal onlyValid As Boolean) As String
                Dim ret As New System.Text.StringBuilder
                Dim t, t1 As Boolean
                Dim cursor As New CAreaManagerCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.OnlyValid = onlyValid
                cursor.Nominativo.SortOrder = SortEnum.SORT_ASC

                t1 = False
                While Not cursor.EOF
                    t = GetID(cursor.Item) = GetID(selValue)
                    t1 = t1 Or t
                    ret.Append("<option value=""")
                    ret.Append(GetID(cursor.Item))
                    ret.Append(""" ")
                    If (t) Then ret.Append("selected")
                    ret.Append(">")
                    ret.Append(Strings.HtmlEncode(cursor.Item.Nominativo))
                    ret.Append("</option>")
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                If (selValue IsNot Nothing AndAlso t1 = False) Then
                    ret.Append("<option value=""")
                    ret.Append(GetID(selValue))
                    ret.Append(""" selected style=""color:red;"">")
                    ret.Append(Strings.HtmlEncode(selValue.Nominativo))
                    ret.Append("</option>")
                End If
                Return ret.ToString
            End Function

            Public Function CreateElencoTipiRapportoTM(ByVal selValue As String) As String
                Dim names() As String = New String() {"Agente", "Mediatore", "Segnalatore"}
                Dim ret As New System.Text.StringBuilder
                Dim t, t1 As Boolean
                t1 = False
                selValue = Trim(selValue)
                For i As Integer = 0 To UBound(names)
                    t = LCase(selValue) = LCase(names(i))
                    t1 = t1 Or t
                    ret.Append("<option value=""")
                    ret.Append(Strings.HtmlEncode(names(i)))
                    ret.Append(""" ")
                    If (t) Then ret.Append("selected")
                    ret.Append(">")
                    ret.Append(Strings.HtmlEncode(names(i)))
                    ret.Append("</option>")
                Next
                If (selValue <> "" AndAlso t1 = False) Then
                    ret.Append("<option value=""")
                    ret.Append(Strings.HtmlEncode(selValue))
                    ret.Append(""" selected style=""color:red;"">")
                    ret.Append(Strings.HtmlEncode(selValue))
                    ret.Append("</option>")
                End If
                Return ret.ToString
            End Function

            Public Function GetOperatoriAutorizz() As CCollection(Of CUser)
                Dim col As New CCollection(Of CUser)
                col.AddRange(CQSPD.GruppoAutorizzatori.Members)
                For Each u As CUser In CQSPD.GruppoSupervisori.Members
                    If col.GetItemById(GetID(u)) Is Nothing Then col.Add(u)
                Next
                col.Sort()
                Return col
            End Function

            Public Function CreateElencoOpAutorizz(ByVal selValue As CUser) As String
                Dim col As CCollection(Of CUser) = GetOperatoriAutorizz()
                Dim writer As New System.Text.StringBuilder
                Dim t = False, t1 As Boolean = False
                For i As Integer = 0 To col.Count - 1
                    Dim user As CUser = col(i)
                    t = GetID(user) = GetID(selValue)
                    t1 = t1 Or t
                    writer.Append("<option value=""")
                    writer.Append(GetID(user))
                    writer.Append(""" ")
                    If (t) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(user.Nominativo))
                    writer.Append("</option>")
                Next
                If (selValue IsNot Nothing) And Not t1 Then
                    writer.Append("<option value=""")
                    writer.Append(GetID(selValue))
                    writer.Append(""" selected style=""color:red;"">")
                    writer.Append(Strings.HtmlEncode(selValue.Nominativo))
                    writer.Append("</option>")
                End If
                Return writer.ToString
            End Function

            Public Function CreateElencoCollaboratori(ByVal selValue As CCollaboratore, Optional ByVal onlyValid As Boolean = True) As String
                Dim cursor As New CCollaboratoriCursor
                Dim writer As New System.Text.StringBuilder
                Dim t, t1 As Boolean

                cursor.NomePersona.SortOrder = SortEnum.SORT_ASC
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.OnlyValid = onlyValid
                t = False : t1 = False
                While Not cursor.EOF
                    t = (GetID(cursor.Item) = GetID(selValue))
                    t1 = t1 Or t
                    writer.Append("<option value=""")
                    writer.Append(GetID(cursor.Item))
                    writer.Append(""" ")
                    If (t) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(cursor.Item.NomePersona))
                    writer.Append("</option>")
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                If (selValue IsNot Nothing) AndAlso Not t1 Then
                    writer.Append("<option value=""")
                    writer.Append(GetID(selValue))
                    writer.Append(""" selected style=""color:red;"">")
                    writer.Append(Strings.HtmlEncode(selValue.NomePersona))
                    writer.Append("</option>")
                End If
                Return writer.ToString
            End Function

            Public Function CreateElencoConsulenti(ByVal selValue As CConsulentePratica, Optional ByVal onlyValid As Boolean = True) As String
                Dim cursor As New CConsulentiPraticaCursor
                Dim writer As New System.Text.StringBuilder
                Dim t, t1 As Boolean
                cursor.OnlyValid = onlyValid
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Nome.SortOrder = SortEnum.SORT_ASC
                t = False : t1 = False
                While Not cursor.EOF
                    t = (GetID(cursor.Item) = GetID(selValue))
                    t1 = t1 Or t
                    writer.Append("<option value=""")
                    writer.Append(GetID(cursor.Item))
                    writer.Append(""" ")
                    If (t) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(cursor.Item.Nome))
                    writer.Append("</option>")
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                If (selValue IsNot Nothing) And Not t1 Then
                    writer.Append("<option value=""")
                    writer.Append(GetID(selValue))
                    writer.Append(""" selected style=""color:red;"">")
                    writer.Append(Strings.HtmlEncode(selValue.Nome))
                    writer.Append("</option>")
                End If

                Return writer.ToString
            End Function

            Public Function CreateElencoConsulenti(ByVal selValue As Integer, Optional ByVal onlyValid As Boolean = True) As String
                Dim cursor As New CConsulentiPraticaCursor
                Dim writer As New System.Text.StringBuilder
                Dim t, t1 As Boolean
                cursor.OnlyValid = onlyValid
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Nome.SortOrder = SortEnum.SORT_ASC
                t = False : t1 = False
                While Not cursor.EOF
                    t = (GetID(cursor.Item) = selValue)
                    t1 = t1 Or t
                    writer.Append("<option value=""")
                    writer.Append(GetID(cursor.Item))
                    writer.Append(""" ")
                    If (t) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(cursor.Item.Nome))
                    writer.Append("</option>")
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing


                If (selValue <> 0) And Not t1 Then
                    writer.Append("<option value=""")
                    writer.Append(selValue)
                    writer.Append(""" selected style=""color:red;"">ID: ")
                    writer.Append(selValue)
                    writer.Append("</option>")
                End If

                Return writer.ToString
            End Function

            Public Function CreateElencoTabelleSpese(ByVal cessionario As CCQSPDCessionarioClass, ByVal selValue As CTabellaSpese, Optional ByVal onlyValid As Boolean = True) As String
                Dim cursor As New CTabellaSpeseCursor
                Dim writer As New System.Text.StringBuilder
                Dim t As Boolean

                cursor.OnlyValid = onlyValid
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                If (cessionario IsNot Nothing) Then cursor.CessionarioID.Value = GetID(cessionario)
                cursor.Nome.SortOrder = SortEnum.SORT_ASC
                t = False
                While Not cursor.EOF
                    t = t Or (GetID(cursor.Item) = GetID(selValue))
                    writer.Append("<option value=""")
                    writer.Append(GetID(cursor.Item))
                    writer.Append(""" ")
                    If (GetID(cursor.Item) = GetID(selValue)) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(cursor.Item.Nome))
                    writer.Append("</option>")
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                If (selValue IsNot Nothing) And Not t Then
                    writer.Append("<option value=""")
                    writer.Append(GetID(selValue))
                    writer.Append(""" selected style=""color:red;"">")
                    writer.Append(Strings.HtmlEncode(selValue.Nome))
                    writer.Append("</option>")
                End If

                Return writer.ToString
            End Function

            Public Function CreateElencoTabelleTEG(ByVal cessionario As CCQSPDCessionarioClass, ByVal selValue As CTabellaTEGMax, Optional ByVal onlyValid As Boolean = True) As String
                Dim cursor As New CTabelleTEGMaxCursor
                Dim writer As New System.Text.StringBuilder
                Dim t As Boolean
                'cursor.OnlyValid = onlyValid
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                If (cessionario IsNot Nothing) Then cursor.IDCessionario.Value = GetID(cessionario)
                cursor.Nome.SortOrder = SortEnum.SORT_ASC
                t = False
                While Not cursor.EOF
                    t = t Or (GetID(cursor.Item) = GetID(selValue))
                    writer.Append("<option value=""")
                    writer.Append(GetID(cursor.Item))
                    writer.Append(""" ")
                    If (GetID(cursor.Item) = GetID(selValue)) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(cursor.Item.Nome))
                    writer.Append("</option>")
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                If (selValue IsNot Nothing) And Not t Then
                    writer.Append("<option value=""")
                    writer.Append(GetID(selValue))
                    writer.Append(""" selected style=""color:red;"">")
                    writer.Append(Strings.HtmlEncode(selValue.Nome))
                    writer.Append("</option>")
                End If

                Return writer.ToString
            End Function

            Public Function CreateElencoTabelleAssicurative(ByVal selValue As CTabellaAssicurativa, Optional ByVal onlyValid As Boolean = True) As String
                Dim cursor As New CTabelleAssicurativeCursor
                Dim writer As New System.Text.StringBuilder
                Dim t As Boolean
                cursor.OnlyValid = onlyValid
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Nome.SortOrder = SortEnum.SORT_ASC
                t = False
                While Not cursor.EOF
                    t = t Or (GetID(cursor.Item) = GetID(selValue))
                    writer.Append("<option value=""")
                    writer.Append(GetID(cursor.Item))
                    writer.Append(""" ")
                    If (GetID(cursor.Item) = GetID(selValue)) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(cursor.Item.Nome))
                    writer.Append("</option>")
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                If (selValue IsNot Nothing) And Not t Then
                    writer.Append("<option value=""")
                    writer.Append(GetID(selValue))
                    writer.Append(""" selected style=""color:red;"">")
                    writer.Append(Strings.HtmlEncode(selValue.Nome))
                    writer.Append("</option>")
                End If

                Return writer.ToString
            End Function





            Public Function CreateElencoTipoEstinzione(ByVal selValue As String) As String
                Return vbNullString
            End Function

            Public Function CreateElencoUnusedTblFin(ByVal idProdotto As Integer) As String
                'Dim dbRis As System.Data.IDataReader = CQSPD.Database.ExecuteReader("SELECT * FROM [tbl_FIN_TblFin] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And Not [ID] In (SELECT DISTINCT [Tabella] FROM [tbl_FIN_ProdXTabFin] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And ([Prodotto]=" & idProdotto & ")) ORDER BY [Nome] ASC")
                Dim dbRis As System.Data.IDataReader = CQSPD.Database.ExecuteReader("SELECT * FROM [tbl_FIN_TblFin] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And Not [ID] In (SELECT [Tabella] FROM [tbl_FIN_ProdXTabFin] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And ([Prodotto]=" & idProdotto & ") GROYP BY [Tabella]) ORDER BY [Nome] ASC")
                Dim writer As New System.Text.StringBuilder
                While dbRis.Read
                    writer.Append("<option value=""")
                    writer.Append(Formats.ToInteger(dbRis("ID")))
                    writer.Append(""">")
                    writer.Append(Formats.ToString(dbRis("Nome")))
                    writer.Append(" (")
                    writer.Append(Formats.ToString(dbRis("NomeCessionario")))
                    writer.Append(")</option>")
                End While
                dbRis.Dispose() : dbRis = Nothing

                Return writer.ToString
            End Function

            Public Function CreateElencoUsedTblFin(ByVal idProdotto As Integer) As String
                'Dim dbRis As System.Data.IDataReader = CQSPD.Database.ExecuteReader("SELECT * FROM [tbl_FIN_TblFin] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And [ID] In (SELECT DISTINCT [Tabella] FROM [tbl_FIN_ProdXTabFin] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And ([Prodotto]=" & idProdotto & ")) ORDER BY [Nome] ASC")
                Dim dbRis As System.Data.IDataReader = CQSPD.Database.ExecuteReader("SELECT * FROM [tbl_FIN_TblFin] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And [ID] In (SELECT [Tabella] FROM [tbl_FIN_ProdXTabFin] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ") And ([Prodotto]=" & idProdotto & ") GROYP BY [Tabella]) ORDER BY [Nome] ASC")
                Dim writer As New System.Text.StringBuilder
                While dbRis.Read
                    writer.Append("<option value=""")
                    writer.Append(Formats.ToInteger(dbRis("ID")))
                    writer.Append(""">")
                    writer.Append(Formats.ToString(dbRis("Nome")))
                    writer.Append(" (")
                    writer.Append(Formats.ToString(dbRis("NomeCessionario")))
                    writer.Append(")</option>")
                End While
                dbRis.Dispose() : dbRis = Nothing
                Return writer.ToString
            End Function


            Public Function CreateElencoNumeroMensilita(ByVal value As Integer) As String
                Dim writer As New System.Text.StringBuilder
                writer.Append("<option></option>")
                For i As Integer = 1 To 15
                    If Formats.ToInteger(value) = i Then
                        writer.Append("<option value=""")
                        writer.Append(i)
                        writer.Append(""" selected>")
                        writer.Append(i)
                        writer.Append("</option>")
                    Else
                        writer.Append("<option value=""")
                        writer.Append(i)
                        writer.Append(""">")
                        writer.Append(i)
                        writer.Append("</option>")
                    End If
                Next
                Return writer.ToString
            End Function

            Public Function CreateElencoPreventivatoriDisponibili(ByVal value As Integer) As String
                Dim arrPreventivatori As CCollection(Of CProfilo)
                Dim writer As New System.Text.StringBuilder
                Dim t As Boolean
                arrPreventivatori = DMD.CQSPD.Profili.GetPreventivatoriUtenteOffline
                t = False
                For i As Integer = 0 To arrPreventivatori.Count - 1
                    Dim item As CProfilo = arrPreventivatori(i)
                    t = t Or (GetID(item) = value)
                    writer.Append("<option value=""")
                    writer.Append(GetID(item))
                    writer.Append(""" ")
                    If (GetID(item) = value) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(item.ProfiloVisibile))
                    writer.Append("</option>")
                Next
                Return writer.ToString
            End Function

            Public Function SommaRiga(ByVal matrice(,) As Decimal, ByVal r As Integer, ByVal startColumn As Integer, ByVal numColumns As Integer) As Decimal
                Dim sum As Decimal = 0
                For c As Integer = startColumn To startColumn + numColumns - 1
                    sum = sum + matrice(r, c)
                Next
                Return sum
            End Function

            Public Function SommaRiga(ByVal matrice(,) As Integer, ByVal r As Integer, ByVal startColumn As Integer, ByVal numColumns As Integer) As Integer
                Dim sum As Integer = 0
                For c As Integer = startColumn To startColumn + numColumns - 1
                    sum = sum + matrice(r, c)
                Next
                Return sum
            End Function

            Public Function SommaRiga(ByVal matrice(,) As Double, ByVal r As Integer, ByVal startColumn As Integer, ByVal numColumns As Integer) As Double
                Dim sum As Double = 0
                For c As Integer = startColumn To startColumn + numColumns - 1
                    sum = sum + matrice(r, c)
                Next
                Return sum
            End Function

            Public Sub SortByTotale(ByVal m(,) As Decimal, ByVal numRows As Integer, ByVal numCols As Integer, ByVal indexes() As Integer)
                Dim sums() As Decimal
                Dim r, c As Integer
                ReDim sums(numRows - 1)
                For r = 0 To numRows - 1
                    sums(r) = SommaRiga(m, r, 0, numCols)
                Next
                For r = 0 To numRows
                    For c = r + 1 To numRows
                        If sums(indexes(c)) > sums(indexes(r)) Then
                            Dim tmp As Integer
                            tmp = indexes(c)
                            indexes(c) = indexes(r)
                            indexes(r) = tmp
                        End If
                    Next
                Next
            End Sub


            Public Function CreateElencoPadri(ByVal cessionario As CCQSPDCessionarioClass, ByVal selValue As CProfilo, Optional ByVal onlyValid As Boolean = True) As String
                Dim cursor As New CProfiliCursor
                Dim writer As New System.Text.StringBuilder
                Dim t As Boolean
                Dim idCessionario As Integer = GetID(cessionario)
                Dim selValueID = GetID(selValue)
                cursor.IDCessionario.Value = idCessionario
                cursor.ID.Value = selValueID
                cursor.ID.Operator = OP.OP_NE
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.ProfiloVisibile.SortOrder = SortEnum.SORT_ASC
                cursor.OnlyValid = onlyValid
                t = False
                While Not cursor.EOF
                    t = t Or (GetID(cursor.Item) = selValueID)
                    writer.Append("<option value=""")
                    writer.Append(GetID(cursor.Item))
                    writer.Append(""" ")
                    If (GetID(cursor.Item) = selValueID) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(cursor.Item.ProfiloVisibile))
                    writer.Append("</option>")
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                If (t = False) And (selValue IsNot Nothing) Then
                    writer.Append("<option value=""")
                    writer.Append(selValueID)
                    writer.Append(""" selected style=""color:red;"">")
                    writer.Append(Strings.HtmlEncode(selValue.ProfiloVisibile))
                    writer.Append("</option>")
                End If

                Return writer.ToString
            End Function

            Public Function CreateElencoStatiDiPartenza(ByVal selValue As CStatoPratica) As String
                Dim items As CCollection(Of CStatoPratica)
                Dim cID As Integer
                Dim i As Integer
                Dim writer As New System.Text.StringBuilder
                Dim t As Boolean
                Dim selValueID As Integer = GetID(selValue)
                items = DMD.CQSPD.StatiPratica.GetStatiAttivi
                t = False
                For i = 0 To items.Count - 1
                    cID = Databases.GetID(items.Item(i), 0)
                    With items.Item(i)
                        t = t Or (cID = selValueID)
                        writer.Append("<option value=""")
                        writer.Append(cID)
                        writer.Append(""" ")
                        If (cID = selValueID) Then writer.Append("selected")
                        writer.Append(">")
                        writer.Append(Strings.HtmlEncode(.Nome))
                        writer.Append("</option>")
                    End With
                Next
                If (Not t) And (selValue IsNot Nothing) Then
                    writer.Append("<option value=""")
                    writer.Append(selValueID)
                    writer.Append(""" style=""color:red;"" selected>")
                    writer.Append(Strings.HtmlEncode(selValue.Nome))
                    writer.Append("</option>")
                End If

                Return writer.ToString
            End Function

            Public Function CreateElencoPeriodi(ByVal periodo As String) As String
                Dim items As String() = {"Oggi", "Ieri", "Questa settimana", "Questo mese", "Il mese scorso", "Quest'anno", "L'anno scorso", "Tra"}
                Dim i As Integer
                Dim writer As New System.Text.StringBuilder
                For i = 0 To UBound(items)
                    If (items(i) = periodo) Then
                        writer.Append("<option value=""")
                        writer.Append(items(i))
                        writer.Append(""" selected>")
                        writer.Append(items(i))
                        writer.Append("</option>")
                    Else
                        writer.Append("<option value=""")
                        writer.Append(items(i))
                        writer.Append(""">")
                        writer.Append(items(i))
                        writer.Append("</option>")
                    End If
                Next

                Return writer.ToString
            End Function

            Public Function CreateElencoTipoIData(ByVal tipoIntervallo As String) As String
                Dim values() As String = Calendar.GetSupportedPeriods() ' {"", "Oggi", "Ieri", "Questa settimana", "Questo mese", "Il mese scorso", "Questo anno", "L'anno scorso", "Tra"}
                Dim writer As New System.Text.StringBuilder
                Dim i As Integer
                tipoIntervallo = LCase(Trim(tipoIntervallo))
                For i = 0 To UBound(values)
                    writer.Append("<option value=""")
                    writer.Append(Strings.HtmlEncode(values(i)))
                    writer.Append(""" ")
                    If (tipoIntervallo = LCase(values(i))) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(values(i)))
                    writer.Append("</option>")
                Next
                Return writer.ToString
            End Function

            Public Function CreateElencoDataDi(ByVal value As Integer) As String
                Dim stati() As Integer = {0, 1, 2, 3, 4, 5}
                Dim valori() As String = {"Qualsiasi", "Inserimento", "Caricamento", "Perfezionamento", "Annullamento", "Trasferimento"}
                Dim i As Integer
                Dim writer As New System.Text.StringBuilder
                For i = LBound(valori) To UBound(valori)
                    writer.Append("<option value=""")
                    writer.Append(stati(i))
                    writer.Append(""" ")
                    If (value = stati(i)) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(valori(i)))
                    writer.Append("</option>")
                Next
                Return writer.ToString
            End Function


            Public Function CreateElencoProfili(ByVal nomeProfilo As String) As String
                Dim items As CCollection(Of CProfilo)
                Dim writer As New System.Text.StringBuilder
                Dim i As Integer
                items = Profili.GetPreventivatoriUtente
                For i = 0 To items.Count - 1
                    If items.Item(i).Profilo = nomeProfilo Then
                        writer.Append("<option value=""")
                        writer.Append(items.Item(i).Profilo)
                        writer.Append(""" selected>")
                        writer.Append(items.Item(i).Profilo)
                        writer.Append("</option>")
                    Else
                        writer.Append("<option value=""")
                        writer.Append(items.Item(i).Profilo)
                        writer.Append(""">")
                        writer.Append(items.Item(i).Profilo)
                        writer.Append("</option>")
                    End If
                Next

                Return writer.ToString
            End Function

            Public Function CreateElencoCessionari(ByVal cessionario As CCQSPDCessionarioClass, Optional ByVal onlyValid As Boolean = True) As String
                Dim cursor As New CCessionariCursor
                Dim t As Boolean
                Dim writer As New System.Text.StringBuilder
                Dim idCessionario As Integer

                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Nome.SortOrder = SortEnum.SORT_ASC
                cursor.Visibile.Value = True
                cursor.OnlyValid = onlyValid
                idCessionario = GetID(cessionario)
                t = False
                While Not cursor.EOF
                    t = t Or (GetID(cursor.Item) = idCessionario)
                    writer.Append("<option value=""")
                    writer.Append(GetID(cursor.Item))
                    writer.Append(""" ")
                    If (GetID(cursor.Item) = idCessionario) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(cursor.Item.Nome))
                    writer.Append("</option>")
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                If (t = False) And (cessionario IsNot Nothing) Then
                    writer.Append("<option value=""")
                    writer.Append(idCessionario)
                    writer.Append(""" style=""color:red"" selected>")
                    writer.Append(Strings.HtmlEncode(cessionario.Nome))
                    writer.Append("</option>")
                End If

                Return writer.ToString
            End Function


            Public Function CreateElencoGruppiProdotto(ByVal cessionario As CCQSPDCessionarioClass, ByVal selItem As CGruppoProdotti, Optional ByVal onlyValid As Boolean = True) As String
                Dim cursor As New CGruppoProdottiCursor
                Dim writer As New System.Text.StringBuilder
                Dim t As Boolean
                Dim idCessionario As Integer = GetID(cessionario)
                Dim selItemID As Integer = GetID(selItem)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.OnlyValid = onlyValid
                cursor.CessionarioID.Value = idCessionario
                cursor.Descrizione.SortOrder = SortEnum.SORT_ASC
                t = False
                While Not cursor.EOF
                    t = t Or (GetID(cursor.Item) = selItemID)
                    writer.Append("<option value=""")
                    writer.Append(cursor.Item.ID)
                    writer.Append(""" ")
                    If (GetID(cursor.Item) = selItemID) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(cursor.Item.Descrizione))
                    writer.Append("</option>")
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                If (t = False) And (selItem IsNot Nothing) Then
                    writer.Append("<option value=""")
                    writer.Append(selItemID)
                    writer.Append(""" style=""color:red"" selected>")
                    writer.Append(Strings.HtmlEncode(selItem.Descrizione))
                    writer.Append("</option>")
                End If
                Return writer.ToString
            End Function


            Public Function CreateElencoTipoContratto(ByVal selValue As String) As String
                Dim ret As String
                Dim t As Boolean

                Dim items As CCollection(Of CTipoContratto) = CQSPD.TipiContratto.LoadAll
                items.Sort()
                ret = ""
                t = False
                For i As Integer = 0 To items.Count - 1
                    Dim item As CTipoContratto = items(i)
                    If (item.Attivo = False) Then Continue For

                    Dim tc As String = item.IdTipoContratto

                    t = t Or (tc = selValue)
                    ret = ret & "<option value=""" & Strings.HtmlEncode(tc) & """ " & CStr(IIf(tc = selValue, "selected", "")) & ">" & Strings.HtmlEncode(item.Descrizione & " (" & tc & ")") & "</option>"
                Next

                If (t = False) And (selValue <> vbNullString) Then
                    ret = ret & "<option value=""" & Strings.HtmlEncode(selValue) & """ style=""color:red"" selected>INVALID: ID=" & Strings.HtmlEncode(selValue) & "</option>"
                End If
                Return ret
            End Function

            'Public Function CreateElencoProfiliEsterni(ByVal cessionario As CCQSPDCessionarioClass, ByVal selValue As CProfilo, Optional ByVal onlyValid As Boolean = True) As String
            '    Dim writer As New System.Text.StringBuilder
            '    Dim t As Boolean
            '    Dim items As CCollection(Of CProfilo)
            '    Dim idProfilo As Integer = GetID(selValue)
            '    items = DMD.CQSPD.Pratiche.GetArrayProfiliEsterni(cessionario)
            '    t = False
            '    For i As Integer = 0 To items.Count - 1
            '        Dim item As CProfilo = items(i)
            '        If (onlyValid = False) Or (item.IsValid) Then
            '            t = t Or (idProfilo = GetID(item))
            '            writer.Append("<option value=""")
            '            writer.Append(GetID(item))
            '            writer.Append(""" ")
            '            If (idProfilo = GetID(item)) Then writer.Append("selected")
            '            writer.Append(">")
            '            writer.Append(Strings.HtmlEncode(item.ProfiloVisibile))
            '            writer.Append("</option>")
            '        End If
            '    Next
            '    If (t = False) And (selValue IsNot Nothing) Then
            '        writer.Append("<option value=""")
            '        writer.Append(idProfilo)
            '        writer.Append(""" style=""color:red"" selected>")
            '        writer.Append(selValue.ProfiloVisibile)
            '        writer.Append("</option>")
            '    End If

            '    Return writer.ToString
            'End Function

            Public Function CreateElencoProdotti(ByVal profilo As CProfilo, ByVal prodotto As CCQSPDProdotto, Optional ByVal onlyValid As Boolean = True) As String
                Dim prodotti As CCollection(Of CCQSPDProdotto)
                Dim t As Boolean
                Dim writer As New System.Text.StringBuilder
                Dim selValue As Integer = GetID(prodotto)
                If (profilo IsNot Nothing) Then
                    prodotti = profilo.ProdottiXProfiloRelations.GetProdotti
                    t = False
                    For i As Integer = 0 To prodotti.Count - 1
                        Dim item As CCQSPDProdotto = prodotti(i)
                        If item.IsValid Or (onlyValid = False) Then
                            t = t OrElse (GetID(item) = selValue)
                            writer.Append("<option value=""")
                            writer.Append(GetID(item))
                            writer.Append(""" ")
                            If (GetID(item) = selValue) Then writer.Append("selected")
                            writer.Append(">")
                            writer.Append(Strings.HtmlEncode(item.Nome))
                            writer.Append("</option>")
                        End If
                    Next
                End If
                If (t = False) AndAlso (prodotto IsNot Nothing) Then
                    writer.Append("<option value=""")
                    writer.Append(selValue)
                    writer.Append(""" style=""color:red"" selected>")
                    writer.Append(Strings.HtmlEncode(prodotto.Nome))
                    writer.Append("</option>")
                End If

                Return writer.ToString
            End Function

            Public Function CreateElencoProdotti(ByVal idProdotto As Integer, Optional ByVal onlyValid As Boolean = True) As String
                Dim cursor As New CProdottiCursor
                Dim writer As New System.Text.StringBuilder
                Dim t, t1 As Boolean
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.OnlyValid = onlyValid
                cursor.Nome.SortOrder = SortEnum.SORT_ASC
                cursor.Visible.Value = True
                t = False : t1 = False
                While Not cursor.EOF
                    Dim strStyle As String
                    If cursor.Item.IsValid Then
                        strStyle = "display:block"
                    Else
                        strStyle = "color:red;"
                    End If
                    t = (GetID(cursor.Item) = idProdotto)
                    t1 = t1 OrElse t
                    writer.Append("<option value=""")
                    writer.Append(GetID(cursor.Item))
                    writer.Append(""" ")
                    If (t) Then writer.Append("selected")
                    writer.Append(" valid=""")
                    If (cursor.Item.IsValid) Then
                        writer.Append("1")
                    Else
                        writer.Append("0")
                    End If
                    writer.Append(""" style=""")
                    writer.Append(strStyle)
                    writer.Append(""">")
                    writer.Append(Strings.HtmlEncode(cursor.Item.Nome))
                    writer.Append(" (")
                    writer.Append(cursor.Item.NomeCessionario)
                    writer.Append(")")
                    writer.Append("</option>")

                    cursor.MoveNext()
                End While

                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                If (t1 = False) AndAlso (idProdotto <> 0) Then
                    Dim prodotto As CCQSPDProdotto = DMD.CQSPD.Prodotti.GetItemById(idProdotto)
                    Dim nomeProdotto As String = "INVALID: ID=" & idProdotto
                    If (prodotto IsNot Nothing) Then nomeProdotto = prodotto.Descrizione & " (" & prodotto.NomeCessionario & ")"
                    writer.Append("<option value=""")
                    writer.Append(idProdotto)
                    writer.Append(""" style=""color:red"" selected>")
                    writer.Append(Strings.HtmlEncode(nomeProdotto))
                    writer.Append("</option>")
                End If

                Return writer.ToString
            End Function

            Public Function CreateElencoNomiProdotto(ByVal selValue As String, Optional ByVal onlyValid As Boolean = True) As String
                Dim cursor As New CProdottiCursor
                Dim writer As New System.Text.StringBuilder
                Dim t, t1 As Boolean
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.OnlyValid = onlyValid
                cursor.Nome.SortOrder = SortEnum.SORT_ASC
                t = False : t1 = False
                While Not cursor.EOF
                    t = (cursor.Item.Nome = selValue)
                    t1 = t1 OrElse t
                    writer.Append("<option value=""")
                    writer.Append(Strings.HtmlEncode(cursor.Item.Nome))
                    writer.Append(""" ")
                    If (t) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(cursor.Item.Nome))
                    writer.Append(" (")
                    writer.Append(cursor.Item.NomeCessionario)
                    writer.Append(")")
                    writer.Append("</option>")
                    cursor.MoveNext()
                End While
                If (t = False) AndAlso (selValue <> "") Then
                    writer.Append("<option value=""")
                    writer.Append(selValue)
                    writer.Append(""" style=""color:red"" selected>INVALID: Nome=")
                    writer.Append(selValue)
                    writer.Append("</option>")
                End If
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                Return writer.ToString
            End Function


            Public Function CreateElencoDurata(ByVal tVal As Integer) As String
                Dim writer As New System.Text.StringBuilder
                For i = 12 To 120 Step 12
                    If tVal = i Then
                        writer.Append("<option value=""")
                        writer.Append(i)
                        writer.Append(""" style=""text-align:right;"" selected>")
                        writer.Append(i)
                        writer.Append("</option>")
                    Else
                        writer.Append("<option value=""")
                        writer.Append(i)
                        writer.Append(""" style=""text-align:right;"">")
                        writer.Append(i)
                        writer.Append("</option>")
                    End If
                Next
                Return writer.ToString
            End Function

            Public Function CreateElencoOperatori(ByVal operatore As CUser, Optional ByVal onlyValid As Boolean = True) As String
                Dim [module] As CModule
                Dim grp As CGroup
                Dim ret As String
                Dim t, t1 As Boolean
                Dim idOperatore As Integer = GetID(operatore)
                [module] = DMD.CQSPD.Pratiche.Module
                ret = ""
                t = False
                If [module].UserCanDoAction("list") OrElse [module].UserCanDoAction("list_office") Then
                    'Otteniamo il gruppo Rapportini
                    grp = CQSPD.GruppoOperatori
                    If Not grp Is Nothing Then
                        For i As Integer = 0 To grp.Members.Count - 1
                            Dim usr As CUser = grp.Members(i)
                            t1 = (idOperatore = GetID(usr))
                            t = t OrElse t1
                            ret = ret & "<option value=""" & GetID(usr) & """ " & CStr(IIf(t1, "selected", "")) & ">" & Strings.HtmlEncode("" & usr.Nominativo) & "</option>"
                        Next
                    End If
                End If
                If [module].UserCanDoAction("list_own") Then
                    t1 = idOperatore = GetID(Users.CurrentUser)
                    t = t OrElse t1
                    ret = ret & "<option value=""" & GetID(Users.CurrentUser) & """ " & CStr(IIf(t1, "selected", "")) & ">" & Strings.HtmlEncode("" & Users.CurrentUser.Nominativo) & "</option>"
                End If
                If (Not t AndAlso operatore IsNot Nothing) Then
                    ret = ret & "<option value=""" & idOperatore & """ selected style=""color:red;"">" & operatore.Nominativo & "</option>"
                End If

                Return ret
            End Function

            Public Function CreateElencoCommerciali(ByVal commerciale As CTeamManager, Optional ByVal onlyValid As Boolean = True) As String
                Dim cursor As New CTeamManagersCursor
                Dim writer As New System.Text.StringBuilder
                Dim iID As Integer
                Dim t1, t As Boolean
                Dim items As New CCollection(Of CTeamManager)
                Dim idCommerciale As Integer = GetID(commerciale)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                'cursor.Nominativo.SortOrder = SortEnum.SORT_ASC
                cursor.OnlyValid = onlyValid
                t = False : t1 = False
                While Not cursor.EOF
                    If (cursor.Item IsNot Nothing) Then items.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                'items.Comparer = New CTeamManager
                items.Sort()
                For i As Integer = 0 To items.Count - 1
                    Dim item As CTeamManager = items(i)
                    iID = GetID(item)
                    t = t OrElse (iID = idCommerciale)
                    writer.Append("<option value=""")
                    writer.Append(iID)
                    writer.Append(""" ")
                    If (iID = idCommerciale) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(item.Nominativo))
                    writer.Append("</option>")
                Next

                If (t = False) AndAlso (commerciale IsNot Nothing) Then
                    writer.Append("<option value=""")
                    writer.Append(idCommerciale)
                    writer.Append(""" selected style=""color:red"">")
                    writer.Append(Strings.HtmlEncode(commerciale.Nominativo))
                    writer.Append("</option>")
                End If

                Return writer.ToString
            End Function

            Public Function CreateElencoCommerciali(ByVal areaManager As CAreaManager, ByVal commerciale As CTeamManager, Optional ByVal onlyValid As Boolean = True) As String
                Dim cursor As New CTeamManagersCursor
                Dim writer As New System.Text.StringBuilder
                Dim iID As Integer
                Dim t As Boolean
                Dim idAreaManager As Integer = GetID(areaManager)
                Dim commercialeID As Integer = GetID(commerciale)
                cursor.IDReferente.Value = idAreaManager
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Nominativo.SortOrder = SortEnum.SORT_ASC
                cursor.OnlyValid = onlyValid
                t = False
                While Not cursor.EOF
                    iID = GetID(cursor.Item)
                    t = t OrElse (iID = commercialeID)
                    writer.Append("<option value=""")
                    writer.Append(iID)
                    writer.Append(""" ")
                    If (iID = commercialeID) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(cursor.Item.Nominativo))
                    writer.Append("</option>")
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                If (t = False) AndAlso (commerciale IsNot Nothing) Then
                    writer.Append("<option value=""")
                    writer.Append(commercialeID)
                    writer.Append(""" selected style=""color:red"">")
                    writer.Append(Strings.HtmlEncode(commerciale.Nominativo))
                    writer.Append("</option>")
                End If

                Return writer.ToString
            End Function


            Public Function CreateElencoStatoPratica(ByVal value As Nullable(Of StatoPraticaEnum)) As String
                Dim stati As StatoPraticaEnum() = {StatoPraticaEnum.STATO_CONTATTO, StatoPraticaEnum.STATO_RICHIESTADELIBERA, StatoPraticaEnum.STATO_DELIBERATA, StatoPraticaEnum.STATO_PRONTALIQUIDAZIONE, StatoPraticaEnum.STATO_LIQUIDATA, StatoPraticaEnum.STATO_ARCHIVIATA, StatoPraticaEnum.STATO_ANNULLATA}
                Dim valori As String() = {"CONTATTO", "RICHIESTA DELIBERA", "DELIBERATA", "PRONTA PER LIQUIDAZIONE", "PERFEZIONATA", "ARCHIVIATA", "ANNULLATA"}
                Return SystemUtils.CreateElenco(valori, stati, value)
            End Function


            Public Function CreateElencoUfficiStr(ByVal value As String) As String
                Dim [module] As CModule
                Dim canList, canListOwn As Boolean
                Dim dbRis As System.Data.IDataReader
                Dim dbSQL As String
                [module] = DMD.CQSPD.Pratiche.Module
                canList = [module].UserCanDoAction("list")
                canListOwn = [module].UserCanDoAction("list_own")
                value = Trim(value)
                If canList Then
                    dbSQL = "SELECT [ID] As [a], [Nome] As [b] FROM [tbl_AziendaUffici] ORDER BY [Nome] ASC"
                ElseIf canListOwn Then
                    dbSQL = "SELECT [tbl_UtentiXUfficio].[Ufficio] As [a], [tbl_AziendaUffici].[Nome] As [b] FROM [tbl_UtentiXUfficio] INNER JOIN [tbl_AziendaUffici] ON [tbl_UtentiXUfficio].[Ufficio]=[tbl_AziendaUffici].[ID] WHERE ([tbl_UtentiXUfficio].[Utente]=" & Users.CurrentUser.ID & ")"
                Else
                    dbSQL = "SELECT [ID] As [a], [Nome] As [b] FROM [tbl_AziendaUffici] WHERE (0<>0)"
                End If
                dbRis = APPConn.ExecuteReader(dbSQL)


                Dim writer As New System.Text.StringBuilder
                While dbRis.Read
                    Dim b As String = Trim(Formats.ToString(dbRis("b")))
                    writer.Append("<option value=""")
                    writer.Append(Strings.HtmlEncode(b))
                    writer.Append(""" ")
                    If (b = value) Then writer.Append("selected")
                    writer.Append(">")
                    writer.Append(Strings.HtmlEncode(b))
                    writer.Append("</option>")
                End While
                dbRis.Dispose() : dbRis = Nothing

                Return writer.ToString
            End Function


            Public Function CreateElencoUffici(ByVal ufficio As CUfficio) As String
                Dim writer As New System.Text.StringBuilder
                Dim t1, t As Boolean
                Dim items As CCollection(Of CUfficio) = Anagrafica.Uffici.GetPuntiOperativiConsentiti
                t1 = False : t = False
                For i As Integer = 0 To items.Count - 1
                    Dim item As CUfficio = items(i)
                    If (item.Attivo AndAlso item.Stato = ObjectStatus.OBJECT_VALID) Then
                        t = (GetID(ufficio) = GetID(item))
                        t1 = t1 Or t
                        writer.Append("<option value=""")
                        writer.Append(GetID(item))
                        writer.Append(""" ")
                        If (t) Then writer.Append("selected")
                        writer.Append(">")
                        writer.Append(Strings.HtmlEncode(item.Nome))
                        writer.Append("</option>")
                    End If
                Next
                If (t1 = False) AndAlso (ufficio IsNot Nothing) Then
                    writer.Append("<option value=""")
                    writer.Append(GetID(ufficio))
                    writer.Append(""" selected style=""color:red"">")
                    writer.Append(Strings.HtmlEncode(ufficio.Nome))
                    writer.Append("</option>")
                End If

                Return writer.ToString
            End Function

            Function CreateElencoConsulenze(idCliente As Integer, id As Integer) As String
                Dim cursor As New CQSPDConsulenzaCursor
                Try
                    Dim html As String = ""
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor.DataConsulenza.SortOrder = SortEnum.SORT_DESC
                    cursor.IDCliente.Value = idCliente
                    While Not cursor.EOF
                        Dim c As CQSPDConsulenza = cursor.Item
                        html &= "<option value=""" & GetID(c) & """ " & CStr(IIf(GetID(c) = id, "selected", "")) & ">" & c.DataConsulenza & ", Op: " & c.NomeConsulente & " (" & CQSPD.Consulenze.FormatStato(c.StatoConsulenza) & ")</option>"
                        cursor.MoveNext()
                    End While
                    Return html
                Catch ex As Exception
                    Throw
                Finally
                    cursor.Dispose()
                End Try
            End Function

            Function CreateElencoRichiesteDiFinanziamento(idCliente As Integer, id As Integer) As String
                Dim cursor As New CRichiesteFinanziamentoCursor
                Try
                    Dim html As String = ""
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    'if (this.getItem().StatoContatto() != null && this.getItem().StatoContatto().getData() != null) {
                    '//    cursor.Data().setValue(Calendar.GetLastSecond(this.getItem().StatoContatto().getData()));
                    '//} else {
                    '//    cursor.Data().setValue(Calendar.GetLastSecond(this.getItem().getCreatoIl()));
                    '//}
                    cursor.IDCliente.Value = idCliente
                    cursor.Data().Operator = OP.OP_LE
                    cursor.Data.SortOrder = SortEnum.SORT_DESC
                    While (Not cursor.EOF())
                        Dim c As CRichiestaFinanziamento = cursor.Item
                        Dim str As String
                        str = Formats.FormatUserDate(c.Data)
                        Select Case c.TipoRichiesta
                            Case TipoRichiestaFinanziamento.ALMENO
                                str &= ", Almeno " & Formats.FormatValuta(c.ImportoRichiesto)
                            Case TipoRichiestaFinanziamento.MASSIMO_POSSIBILE
                                str &= ", Massimo"
                            Case TipoRichiestaFinanziamento.TRA
                                str &= ", Tra " & Formats.FormatValuta(c.ImportoRichiesto) & " e " & Formats.FormatValuta(c.ImportoRichiesto1)
                            Case TipoRichiestaFinanziamento.UGUALEA
                                str &= ", Uguale a " & Formats.FormatValuta(c.ImportoRichiesto)
                        End Select
                        html &= "<option value=""" & GetID(c) & """ " & CStr(IIf(GetID(c) = id, "selected", "")) & ">" & str & "</option>"
                        cursor.MoveNext()
                    End While

                    Return html
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                    Throw
                Finally
                    If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                End Try
            End Function

            Public Function CreateElencoStatiRichiestaSconto(ByVal selValue As Nullable(Of StatoRichiestaApprovazione)) As String
                Dim items() As StatoRichiestaApprovazione = {StatoRichiestaApprovazione.ATTESA, StatoRichiestaApprovazione.PRESAINCARICO, StatoRichiestaApprovazione.APPROVATA, StatoRichiestaApprovazione.ATTESA}
                Dim names() As String = {"Attesa Valutazione", "In Valutazione", "Approvata", "Negata"}
                Return SystemUtils.CreateElenco(names, items, selValue)
            End Function

        End Class


        Private Shared m_CQSPDUtils As CCQSPDUtilsClass = Nothing

        Public Shared ReadOnly Property CQSPDUtils As CCQSPDUtilsClass
            Get
                If (m_CQSPDUtils Is Nothing) Then m_CQSPDUtils = New CCQSPDUtilsClass
                Return m_CQSPDUtils
            End Get
        End Property

    End Class

End Namespace