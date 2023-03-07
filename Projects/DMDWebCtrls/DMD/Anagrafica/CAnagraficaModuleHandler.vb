Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Forms.Utils
Imports DMD.CQSPD
Imports DMD.XML

Namespace Forms


    Public Class CAnagraficaModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Function GetNomiAziende(ByVal renderer As Object) As String
            WebSite.ASP_Server.ScriptTimeout = 15

            Dim oid As Integer = RPC.n2int(GetParameter(renderer, "oid", "0"))
            Dim filter As New CRMFinlterI

            filter.text = RPC.n2str(Replace(Me.GetParameter(renderer, "_q", ""), "  ", " "))
            filter.tipoPersona = TipoPersona.PERSONA_GIURIDICA
            filter.ignoreRights = True
            filter.nMax = 50
            Dim col As CCollection(Of CPersonaInfo) = Anagrafica.Persone.Find(filter)
            col.Comparer = New FindPersonaComparer(filter.text)
            col.Sort()

            Dim ret As New System.Text.StringBuilder
            ret.Append("<list>")
            For Each info As CPersonaInfo In col
                If (oid <> 0) Then
                    If TypeOf (info.Persona) Is CAzienda Then
                        Dim per As CAzienda = DirectCast(info.Persona, CAzienda)
                        If (per.IDEntePagante <> 0) AndAlso (per.IDEntePagante <> oid) Then Continue For
                    End If
                End If

                ret.Append("<item>")
                ret.Append("<text>")
                ret.Append(Strings.HtmlEncode(info.NomePersona))
                ret.Append("</text>")
                ret.Append("<value>")
                ret.Append(Strings.HtmlEncode(info.IDPersona))
                ret.Append("</value>")
                ret.Append("<icon>")
                ret.Append(Strings.HtmlEncode(info.IconURL))
                ret.Append("</icon>")
                ret.Append("<attribute>")
                ret.Append(Strings.HtmlEncode(info.Notes))
                ret.Append("</attribute>")
                ret.Append("</item>")
            Next
            ret.Append("</list>")

            Return ret.ToString
        End Function

        Private Class FindPersonaComparer
            Implements IComparer

            Private text As String

            Public Sub New(ByVal text As String)
                Me.text = Strings.LCase(Strings.Trim(text))
            End Sub

            Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
                Dim a As CPersonaInfo = x
                Dim b As CPersonaInfo = y
                If (Me.text = "") Then
                    Return Strings.Compare(a.NomePersona, b.NomePersona, CompareMethod.Text)
                Else
                    Dim i1 As Integer = Strings.InStr(Strings.LCase(a.NomePersona), text)
                    Dim i2 As Integer = Strings.InStr(Strings.LCase(b.NomePersona), text)
                    If (i1 < 0) Then i1 = 65536
                    If (i2 < 0) Then i2 = 65536
                    Dim ret As Integer = Arrays.Compare(i1, i2)
                    If (ret = 0) Then ret = Strings.Compare(a.NomePersona(), b.NomePersona(), CompareMethod.Text)
                    Return ret
                End If
            End Function
        End Class

        Public Function GetNomiPersoneFisiche(ByVal renderer As Object) As String
            Dim col As CCollection(Of CPersonaInfo)
            Dim filter As New CRMFinlterI


            WebSite.ASP_Server.ScriptTimeout = 15

            Dim oid As Integer = RPC.n2int(GetParameter(renderer, "oid", "0"))

            filter.text = RPC.n2str(Replace(Me.GetParameter(renderer, "_q", ""), "  ", " "))
            If (Strings.Len(filter.text) > 3) Then
                filter.tipoPersona = TipoPersona.PERSONA_FISICA
                filter.nMax = 50
                col = Anagrafica.Persone.Find(filter)
                col.Comparer = New FindPersonaComparer(filter.text)
                col.Sort()
            Else
                col = New CCollection(Of CPersonaInfo)
            End If

            Dim ret As New System.Text.StringBuilder
            ret.Append("<list>")
            For Each info As CPersonaInfo In col
                If (oid <> 0) Then
                    If TypeOf (info.Persona) Is CPersonaFisica Then
                        Dim per As CPersonaFisica = DirectCast(info.Persona, CPersonaFisica)
                        If (per.ImpiegoPrincipale.IDAzienda <> 0) AndAlso (per.ImpiegoPrincipale.IDAzienda <> oid) Then Continue For
                    End If
                End If


                ret.Append("<item>")
                ret.Append("<text>")
                ret.Append(Strings.HtmlEncode(info.NomePersona))
                ret.Append("</text>")
                ret.Append("<value>")
                ret.Append(Strings.HtmlEncode(info.IDPersona))
                ret.Append("</value>")
                ret.Append("<icon>")
                ret.Append(Strings.HtmlEncode(info.IconURL))
                ret.Append("</icon>")
                ret.Append("<attribute>")
                ret.Append(Strings.HtmlEncode(info.Notes))
                ret.Append("</attribute>")
                ret.Append("</item>")
            Next
            ret.Append("</list>")

            Return ret.ToString
        End Function

        Public Function GetNomiPersone(ByVal renderer As Object) As String
            WebSite.ASP_Server.ScriptTimeout = 15

            Dim filter As New CRMFinlterI
            Dim oid As Integer = RPC.n2int(GetParameter(renderer, "oid", "0"))

            filter.text = RPC.n2str(Replace(Me.GetParameter(renderer, "_q", ""), "  ", " "))
            'filter.tipoPersona = TipoPersona.PERSONA_FISICA
            filter.nMax = 200
            Dim col As CCollection(Of CPersonaInfo) = Anagrafica.Persone.Find(filter)
            col.Comparer = New FindPersonaComparer(filter.text)
            col.Sort()

            Dim ret As New System.Text.StringBuilder
            ret.Append("<list>")
            For Each info As CPersonaInfo In col
                If (oid <> 0) Then
                    If TypeOf (info.Persona) Is CPersonaFisica Then
                        Dim per As CPersonaFisica = DirectCast(info.Persona, CPersonaFisica)
                        If (per.ImpiegoPrincipale.IDAzienda <> 0) AndAlso (per.ImpiegoPrincipale.IDAzienda <> oid) Then Continue For
                    ElseIf TypeOf (info.Persona) Is CAzienda Then
                        Dim per As CAzienda = DirectCast(info.Persona, CAzienda)
                        If (per.IDEntePagante <> 0) AndAlso (per.IDEntePagante <> oid) Then Continue For
                    End If
                End If

                ret.Append("<item>")
                ret.Append("<text>")
                ret.Append(Strings.HtmlEncode(info.NomePersona))
                ret.Append("</text>")
                ret.Append("<value>")
                ret.Append(Strings.HtmlEncode(info.IDPersona))
                ret.Append("</value>")
                ret.Append("<icon>")
                ret.Append(Strings.HtmlEncode(info.IconURL))
                ret.Append("</icon>")
                ret.Append("<attribute>")
                ret.Append(Strings.HtmlEncode(info.Notes))
                ret.Append("</attribute>")
                ret.Append("</item>")
            Next
            ret.Append("</list>")

            Return ret.ToString
        End Function

        ''' <summary>
        ''' Restituisce l'elenco dei clienti per cui si è flaggato "In Lavorazione"
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetClientiInLavorazione(ByVal renderer As Object) As String
            Dim cursor As CPersonaFisicaCursor = Nothing
            Try
                Dim ret As New CCollection(Of CPersonaInfo)
                cursor = New CPersonaFisicaCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.WhereClauses.Add("((([PFlags] AND " & PFlags.InLavorazione & ")=" & PFlags.InLavorazione & ") AND (([PFlags] AND " & PFlags.Cliente & ")=" & PFlags.Cliente & "))")
                While Not cursor.EOF
                    ret.Add(New CPersonaInfo(cursor.Item))
                    cursor.MoveNext()
                End While
                If (ret.Count > 0) Then
                    Return XML.Utils.Serializer.Serialize(ret.ToArray)
                Else
                    Return ""
                End If
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        'Public Function GetInfoParentiEAffini(ByVal renderer As Object) As String
        '    Dim pID As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
        '    If (pID = 0) Then Return ""

        '    Dim items As CCollection(Of CRelazioneParentale) = Anagrafica.RelazioniParentali.GetRelazioni(pID)
        '    Dim ret As New CCollection(Of InfoParentiEAffini)

        '    For i As Integer = 0 To items.Count - 1
        '        Dim rel As CRelazioneParentale = items(i)
        '        Dim info As New InfoParentiEAffini
        '        'Dim estinzioni As CCollection(Of DMD.CQSPD.CEstinzione)
        '        info.Relazione = rel

        '        If (rel.IDPersona1 = pID) Then
        '            If (rel.Persona2) IsNot Nothing Then
        '                info.Contatto = rel.Persona2.Recapiti.GetContattoPredefinito("Cellulare")
        '                If (info.Contatto Is Nothing) Then rel.Persona2.Recapiti.GetContattoPredefinito("Telefono")
        '                'If (rel.Persona1 IsNot Nothing) Then info.IconURL1 = rel.Persona1.IconURL
        '                info.IconURL2 = rel.Persona2.IconURL
        '            End If
        '            'Estinzioni = DMD.CQSPD.Estinzioni.GetEstinzioniByPersona(rel.Persona2)
        '        Else
        '            If (rel.Persona1 IsNot Nothing) Then
        '                info.Contatto = rel.Persona1.Recapiti.GetContattoPredefinito("Cellulare")
        '                If (info.Contatto Is Nothing) Then rel.Persona1.Recapiti.GetContattoPredefinito("Telefono")
        '                'info.Contatto = rel.Persona1.
        '                'Estinzioni = DMD.CQSPD.Estinzioni.GetEstinzioniByPersona(rel.Persona1)
        '                info.IconURL1 = rel.Persona1.IconURL
        '            End If
        '        End If

        '        info.Note = ""
        '        'For j As Integer = 0 To estinzioni.Count - 1
        '        '    Dim e As DMD.CQSPD.CEstinzione = estinzioni(j)
        '        '    If (e.InCorso) Then
        '        '        info.Note &= e.ToString & vbNewLine
        '        '    End If
        '        'Next

        '        ret.Add(info)
        '    Next
        '    If (ret.Count > 0) Then
        '        Return XML.Utils.Serializer.Serialize(ret.ToArray, XMLSerializeMethod.Document)
        '    Else
        '        Return ""
        '    End If
        'End Function

        Public Function GetRicontatto(ByVal renderer As Object) As String
            Dim pID As Nullable(Of Integer) = RPC.n2int(GetParameter(renderer, "pid", ""))
            If (pID.HasValue = False) Then Return ""
            'Dim nomeLista As String = RPC.n2str(GetParameter(renderer, "nl", ""))
            Dim ret As CRicontatto = Anagrafica.Ricontatti.GetRicontatto(pID) ', nomeLista
            If (ret IsNot Nothing) Then
                Return XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function



        Public Function GetRicontattoByOperatore(ByVal renderer As Object) As String
            Dim oID As Nullable(Of Integer) = RPC.n2int(GetParameter(renderer, "oid", ""))
            Dim pID As Nullable(Of Integer) = RPC.n2int(GetParameter(renderer, "pid", ""))
            If (pID.HasValue = False) Then Return ""
            'Dim nomeLista As String = RPC.n2str(GetParameter(renderer, "nl", ""))
            Dim ret As CRicontatto = Anagrafica.Ricontatti.GetRicontattoByOperatore(oID, pID) ', nomeLista
            If (ret IsNot Nothing) Then
                Return XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function

        Public Function CheckRicontattiMultipli(ByVal renderer As Object) As String
            Dim cursor As CRicontattiCursor = Nothing

            Try
                Dim pID As Integer = RPC.n2int(GetParameter(renderer, "pid", ""))
                Dim ric As Integer = RPC.n2int(GetParameter(renderer, "ric", ""))

                Dim items As New CCollection(Of CRicontatto)

                cursor = New CRicontattiCursor()
                cursor.ID.Value = ric
                cursor.ID.Operator = OP.OP_NE
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IDPersona.Value = pID
                cursor.DataPrevista.Value = DateUtils.ToDay
                cursor.DataPrevista.Operator = OP.OP_GE
                cursor.StatoRicontatto.Value = StatoRicontatto.PROGRAMMATO
                'cursor.NomeLista.Value = ""
                'cursor.NomeLista.IncludeNulls = True
                While (Not cursor.EOF())
                    items.Add(cursor.Item)
                    cursor.MoveNext()
                End While


                If (items.Count > 0) Then
                    Return XML.Utils.Serializer.Serialize(items.ToArray, XMLSerializeMethod.Document)
                Else
                    Return ""
                End If
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Public Function AssegnaResidentiPerProvincia(ByVal renderer As Object) As String
            Dim nomeProvincia As String = RPC.n2str(Me.GetParameter(renderer, "nome", ""))
            Dim idPuntoOperativo As Nullable(Of Integer) = RPC.n2int(Me.GetParameter(renderer, "po", ""))
            Dim po As CUfficio = Anagrafica.Uffici.GetItemById(idPuntoOperativo)
            Dim force As Boolean = Formats.ToBoolean(RPC.n2bool(Me.GetParameter(renderer, "force", "")))
            Dim ret As Integer = Anagrafica.Persone.AssegnaResidentiPerProvincia(nomeProvincia, po, force)
            Return ret
        End Function

        Public Function AssegnaResidentiPerComune(ByVal renderer As Object) As String
            Dim nomeComune As String = RPC.n2str(Me.GetParameter(renderer, "nomeComune", ""))
            Dim nomeProvincia As String = RPC.n2str(Me.GetParameter(renderer, "nomeProvincia", ""))
            Dim idPuntoOperativo As Nullable(Of Integer) = RPC.n2int(Me.GetParameter(renderer, "po", ""))
            Dim po As CUfficio = Anagrafica.Uffici.GetItemById(idPuntoOperativo)
            Dim force As Boolean = Formats.ToBoolean(RPC.n2bool(Me.GetParameter(renderer, "force", "")))
            Dim ret As Integer = Anagrafica.Persone.AssegnaResidentiPerComune(nomeComune, nomeProvincia, po, force)
            Return ret
        End Function

        Public Function GetPuntiOperativi(ByVal renderer As Object) As String
            Return XML.Utils.Serializer.Serialize(Anagrafica.Uffici.GetPuntiOperativi, XMLSerializeMethod.Document)
        End Function

        Public Function GetPuntiOperativiConsentiti(ByVal renderer As Object) As String
            Return XML.Utils.Serializer.Serialize(Anagrafica.Uffici.GetPuntiOperativiConsentiti, XMLSerializeMethod.Document)
        End Function


        Public Function FindPersona(ByVal renderer As Object) As String
            Dim cursor As CPersonaCursor = Nothing
            Dim dbRis As System.Data.IDataReader = Nothing

            Try
                Dim tmp As String = RPC.n2str(GetParameter(renderer, "cur", ""))
                'Dim txtAzienda As String = Trim(RPC.n2str(GetParameter(renderer, "na", "")))
                'Dim txtPosizione As String = Trim(RPC.n2str(GetParameter(renderer, "po", "")))
                'Dim txtTipoRapporto As String = Trim(RPC.n2str(GetParameter(renderer, "tr", "")))
                Dim txtPer As String = Trim(RPC.str2n(GetParameter(renderer, "per", "")))
                Dim txtDi As Nullable(Of Date) = RPC.n2date(GetParameter(renderer, "di", ""))
                Dim txtDf As Nullable(Of Date) = RPC.n2date(GetParameter(renderer, "df", ""))
                Dim idOP As Nullable(Of Integer) = RPC.n2int(GetParameter(renderer, "op", ""))
                Dim scopo As String = RPC.n2str(GetParameter(renderer, "scp", ""))
                Dim txtLista As String = RPC.n2str(GetParameter(renderer, "lista", ""))
                Dim nMax As Nullable(Of Integer) = RPC.n2int(GetParameter(renderer, "nmax", ""))

                Dim ret As New CCollection(Of CPersonaInfo)
                Dim info As CPersonaInfo
                Dim cnt As Integer = 0

                cursor = XML.Utils.Serializer.Deserialize(tmp)
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID


                If (txtPer <> "" OrElse scopo <> "" OrElse (idOP.HasValue AndAlso idOP.Value <> 0) OrElse txtLista <> "") Then
                    Dim wherePart As String = "", dbSQL As String = ""

                    ' dbSQL &= "SELECT T1.* FROM (" & cursor.GetSQL & ") AS [T1] "

                    'If (txtAzienda <> "" OrElse txtPosizione <> "" OrElse txtTipoRapporto <> "") Then
                    '    dbSQL &= "INNER JOIN ("
                    '    dbSQL &= "SELECT * FROM [tbl_Impiegati] WHERE "
                    '    wherePart = "[Stato]=" & ObjectStatus.OBJECT_VALID
                    '    If (txtAzienda <> "") Then wherePart = Strings.Combine(wherePart, "([NomeAzienda] Like '" & Replace(txtAzienda, "'", "''") & "%' Or [NomeEntePagante] Like '" & Replace(txtAzienda, "'", "''") & "%')", " AND ")
                    '    If (txtPosizione <> "") Then wherePart = Strings.Combine(wherePart, "[Posizione] Like '" & Replace(txtPosizione, "'", "''") & "%'", " AND ")
                    '    If (txtTipoRapporto <> "") Then wherePart = Strings.Combine(wherePart, "[TipoRapporto] = " & DBUtils.DBString(txtTipoRapporto), " AND ")
                    '    dbSQL &= wherePart
                    '    dbSQL &= ") AS [T2] ON [T1].[ID]=[T2].[Persona]"
                    'End If

                    dbSQL = cursor.GetSQL

                    If (txtPer = "Nessuna") Then
                        dbSQL = "SELECT [T3].* FROM (" & dbSQL & ") As [T3] "
#If usa_tbl_RicontattiQuick Then
                        dbSQL &= "LEFT JOIN [tbl_RicontattiQuick] ON [T3].[ID]=[tbl_RicontattiQuick].[IDPersona] WHERE [tbl_RicontattiQuick].[DataPrevista] Is Null"
#Else
                        dbSQL &= "LEFT JOIN ("
                        dbSQL &= "SELECT * FROM [tbl_Ricontatti] WHERE "
                        dbSQL &= "[Stato]=" & ObjectStatus.OBJECT_VALID & " And ([StatoRicontatto]=" & StatoRicontatto.PROGRAMMATO & " Or [StatoRicontatto]=" & StatoRicontatto.RIMANDATO & ")"
                        dbSQL &= ") AS [tbl_RicontattiQuick] "
                        dbSQL &= " ON [T3].[ID]=[tbl_RicontattiQuick].[IDPersona] "
                        dbSQL &= " WHERE [tbl_RicontattiQuick].[DataPrevista] Is Null"
#End If
                    Else
                        dbSQL = "SELECT [T3].* FROM (" & dbSQL & ") As [T3] "
                        dbSQL &= "INNER JOIN ("
#If usa_tbl_RicontattiQuick Then
                        dbSQL &= "SELECT * FROM [tbl_RicontattiQuick] WHERE "
#Else
                        dbSQL &= "SELECT * FROM [tbl_Ricontatti] WHERE "
#End If
                        wherePart = "[Stato]=" & ObjectStatus.OBJECT_VALID & " And ([StatoRicontatto]=" & StatoRicontatto.PROGRAMMATO & " Or [StatoRicontatto]=" & StatoRicontatto.RIMANDATO & ")"
                        Dim int As CIntervalloData = DateUtils.PeriodoToDates(txtPer, txtDi, txtDf)
                        txtDi = int.Inizio : txtDf = int.Fine
                        If (txtDi.HasValue AndAlso txtDf.HasValue AndAlso txtDi.Value = txtDf.Value) Then txtDf = DateUtils.DateAdd(DateInterval.Second, 24 * 3600 - 1, txtDi.Value)
                        If (txtDi.HasValue) Then
                            If (txtDf.HasValue) Then
                                wherePart = Strings.Combine(wherePart, " [DataPrevista] BETWEEN " & DBUtils.DBDate(txtDi) & " And " & DBUtils.DBDate(txtDf), " And ")
                            Else
                                wherePart = Strings.Combine(wherePart, " [DataPrevista] >= " & DBUtils.DBDate(txtDi), " And ")
                            End If
                        ElseIf (txtDf.HasValue) Then
                            wherePart = Strings.Combine(wherePart, " [DataPrevista] <= " & DBUtils.DBDate(txtDf), " And ")
                        End If
                        If (scopo <> "") Then wherePart = Strings.Combine(wherePart, "[Note] Like '" & Strings.Replace(scopo, "'", "''") & "%'", " AND ")
                        If (idOP.HasValue) Then wherePart = Strings.Combine(wherePart, "[IDAssegnatoA] = " & DBUtils.DBNumber(idOP.Value), " AND ")
                        If (txtLista <> "") Then
                            wherePart = Strings.Combine(wherePart, "[NomeLista] = " & DBUtils.DBString(txtLista), " AND ")
                        Else
                            wherePart = Strings.Combine(wherePart, "([NomeLista]='' Or [NomeLista] Is Null)", " AND ")
                        End If

                        dbSQL &= wherePart
                        dbSQL &= ") AS [T4] ON [T3].[ID]=[T4].[IDPersona]  "
                    End If

                    dbRis = APPConn.ExecuteReader(dbSQL)
                    While dbRis.Read AndAlso (nMax.HasValue = False OrElse cnt < nMax.Value)
                        Dim id As Integer = Formats.ToInteger(dbRis("ID"))
                        For i1 As Integer = 0 To ret.Count - 1
                            info = ret(i1)
                            If (info.IDPersona = id) Then Continue While
                        Next
                        Dim p As CPersona = Anagrafica.Persone.Instantiate(Formats.ToInteger(dbRis("TipoPersona")))
                        APPConn.Load(p, dbRis)

                        info = New CPersonaInfo(p)
                        ret.Add(info)


                        cnt += 1
                    End While
                    dbRis.Dispose() : dbRis = Nothing

                Else
                    While Not cursor.EOF AndAlso (nMax.HasValue = False OrElse cnt < nMax.Value)
                        Dim p As CPersona = cursor.Item
                        info = New CPersonaInfo(p)
                        ret.Add(info)
                        cnt += 1
                        cursor.MoveNext()
                    End While
                End If


                Me.AddExtraInfo(ret)

                If (ret.Count = 0) Then
                    Return vbNullString
                Else
                    Return XML.Utils.Serializer.Serialize(ret.ToArray, XMLSerializeMethod.Document)
                End If
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            End Try
        End Function

        Public Function FindByCF(ByVal renderer As Object) As String
            Dim cf As String = Formats.ParseCodiceFiscale(RPC.n2str(Me.GetParameter(renderer, "CF", "")))
            Dim col As CCollection(Of CPersona) = Anagrafica.Persone.FindPersoneByCF(cf)
            If col.Count > 0 Then
                Return XML.Utils.Serializer.Serialize(col.ToArray, XMLSerializeMethod.Document)
            Else
                Return vbNullString
            End If
        End Function

        Public Function FindByName(ByVal renderer As Object) As String
            Dim cursor As CPersonaCursor = Nothing

            Try
                WebSite.ASP_Server.ScriptTimeout = 15

                Dim text As String = Trim(RPC.n2str(Me.GetParameter(renderer, "name", "")))
                If (text = "") Then Return ""

                Dim ret As New CCollection(Of CPersona)

                cursor = New CPersonaCursor
                cursor.Nominativo.Value = text
                cursor.Nominativo.Operator = OP.OP_LIKE
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Nominativo.SortOrder = SortEnum.SORT_ASC
                While Not cursor.EOF
                    ret.Add(cursor.Item)
                    cursor.MoveNext()
                End While
                If ret.Count = 0 Then
                    Return vbNullString
                Else
                    Return XML.Utils.Serializer.SerializeArray(ret.ToArray, "CPersona")
                End If
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Public Function FindByNameOrCF(ByVal renderer As Object) As String
            Dim reader As DBReader = Nothing

            Try
                WebSite.ASP_Server.ScriptTimeout = 15

                Dim name, cf As String



                name = Replace(Trim(Me.GetParameter(renderer, "name", "")), "'", "''")
                If (name = vbNullString) Then Return vbNullString

                Dim dbSQL, wherePart As String
                Dim ret As New CCollection(Of CPersona)

                cf = Formats.ParseCodiceFiscale(name)
                dbSQL = "SELECT * FROM [tbl_Persone] WHERE "
                wherePart = ""
                wherePart = Strings.Combine(wherePart, "(Trim([Nome] & ' ' & [Cognome]) Like '%" & name & "%')", " OR ")
                If (cf <> vbNullString) Then wherePart = Strings.Combine(wherePart, "([CodiceFiscale] Like '%" & cf & "%')", " OR ")
                wherePart = Strings.Combine(wherePart, "([Stato]=" & ObjectStatus.OBJECT_VALID & ")", " AND ")
                dbSQL = dbSQL & " ORDER BY Trim([Nome] & ' ' & [Cognome]) ASC"

                reader = New DBReader(APPConn.Tables("tbl_Persone"), dbSQL)
                While reader.Read
                    Dim p As CPersona = Persone.Instantiate(reader.GetValue("TipoPersona", "CPersonaFisica"))
                    If APPConn.Load(p, reader) Then ret.Add(p)
                End While

                If (ret.Count > 0) Then
                    Return XML.Utils.Serializer.SerializeArray(ret.ToArray, "CPersona")
                Else
                    Return vbNullString
                End If
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (reader IsNot Nothing) Then reader.Dispose() : reader = Nothing
            End Try
        End Function

        ''' <summary>
        ''' Restituisce una collezione di oggetti CPersonaInfo contenente tutte le persone e le aziende che corrispondono ai parametri di ricerca specificati dal campo name
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FindTelefonata(ByVal renderer As Object) As String
            Dim col As CCollection(Of CPersonaInfo)

            WebSite.ASP_Server.ScriptTimeout = 15

            Dim text As String = RPC.n2str(GetParameter(renderer, "name", ""))
            Dim txtfilter As String = RPC.n2str(GetParameter(renderer, "filter", ""))
            Dim filter As Object = Nothing
            If (txtfilter <> "") Then filter = XML.Utils.Serializer.Deserialize(txtfilter)

            If (TypeOf (filter) Is CRMFinlterI) Then
                col = Anagrafica.Persone.Find(DirectCast(filter, CRMFinlterI))
            Else
                Dim list As New System.Collections.ArrayList
                list.AddRange(filter)
                Dim arr() As CRMFinlterI = list.ToArray(GetType(CRMFinlterI))
                col = Anagrafica.Persone.Find(arr)
            End If


            Me.AddExtraInfo(col)
            If (col.Count = 0) Then
                Return ""
            Else
                Return XML.Utils.Serializer.Serialize(col.ToArray, XMLSerializeMethod.Document)
            End If
        End Function

        Private Sub AddExtraInfo(ByVal col As CCollection(Of CPersonaInfo))
            If col.Count = 0 Then Exit Sub

            For i1 As Integer = 0 To col.Count - 1
                Dim info As CPersonaInfo = col(i1)
                Dim p As CPersona = info.Persona
                Dim sessoAO As String = IIf(p.Sesso = "F", "a", "o")

                If (p.TipoPersona = TipoPersona.PERSONA_FISICA) Then
                    With DirectCast(p, CPersonaFisica)
                        Dim strImpiego As String = ""
                        If (.ImpiegoPrincipale IsNot Nothing) Then
                            If (.ImpiegoPrincipale.NomeAzienda <> "" OrElse .ImpiegoPrincipale.Posizione <> "" OrElse .ImpiegoPrincipale.TipoRapporto <> "") Then
                                If (.ImpiegoPrincipale.TipoRapporto = "H") Then
                                    strImpiego = "Pensionat" & sessoAO
                                Else
                                    strImpiego = "Lavora"
                                End If

                                If (.ImpiegoPrincipale.NomeAzienda <> "") Then strImpiego &= " presso " & .ImpiegoPrincipale.NomeAzienda
                                If (.ImpiegoPrincipale.Posizione <> "") Then strImpiego &= " come " & .ImpiegoPrincipale.Posizione
                                If (.ImpiegoPrincipale.TipoRapporto <> "") Then strImpiego &= " (" & .ImpiegoPrincipale.TipoRapporto & ")"
                            End If
                            If strImpiego <> "" Then info.Notes = Strings.Combine(info.Notes, strImpiego, vbNewLine)
                        End If
                    End With
                End If
            Next
        End Sub

        ''' <summary>
        ''' Restituisce una collezione di oggetti CPersonaInfo contenente le sole persone fisiche che corrispondono ai parametri di ricerca specificati dal campo name
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FindTelefonataPF(ByVal renderer As Object) As String
            Dim col As CCollection(Of CPersonaInfo)

            WebSite.ASP_Server.ScriptTimeout = 15

            'Dim filter As CRMFinlterI = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "filter", "")))
            'filter.ignoreRights = False
            Dim filter As Object = Nothing
            Dim txtfilter As String = RPC.n2str(GetParameter(renderer, "filter", ""))
            If (txtfilter <> "") Then filter = XML.Utils.Serializer.Deserialize(txtfilter)

            If (TypeOf (filter) Is CRMFinlterI) Then
                col = Anagrafica.Persone.FindPF(DirectCast(filter, CRMFinlterI))
            Else
                Dim list As New System.Collections.ArrayList
                list.AddRange(filter)
                Dim arr() As CRMFinlterI = list.ToArray(GetType(CRMFinlterI))
                col = Anagrafica.Persone.FindPF(arr)
            End If
            'col = Anagrafica.Persone.FindPF(filter)
            Me.AddExtraInfo(col)
            If col.Count = 0 Then
                Return vbNullString
            Else
                Return XML.Utils.Serializer.Serialize(col.ToArray, XMLSerializeMethod.Document)
            End If
        End Function

        ''' <summary>
        ''' Restituisce una collezione di oggetti CPersonaInfo contenente le sole aziende fisiche che corrispondono ai parametri di ricerca specificati dal campo name
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FindTelefonataAZ(ByVal renderer As Object) As String
            Dim col As CCollection(Of CPersonaInfo)

            WebSite.ASP_Server.ScriptTimeout = 15

            'Dim filter As CRMFinlterI = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "filter", "")))
            'col = Anagrafica.Persone.FindAZ(filter)
            Dim filter As Object = Nothing
            Dim txtfilter As String = RPC.n2str(GetParameter(renderer, "filter", ""))
            If (txtfilter <> "") Then filter = XML.Utils.Serializer.Deserialize(txtfilter)

            If (TypeOf (filter) Is CRMFinlterI) Then
                col = Anagrafica.Persone.FindAZ(DirectCast(filter, CRMFinlterI))
            Else
                Dim list As New System.Collections.ArrayList
                list.AddRange(filter)
                Dim arr() As CRMFinlterI = list.ToArray(GetType(CRMFinlterI))
                col = Anagrafica.Persone.FindAZ(arr)
            End If

            Me.AddExtraInfo(col)
            If (col.Count = 0) Then
                Return ""
            Else
                Return XML.Utils.Serializer.Serialize(col.ToArray, XMLSerializeMethod.Document)
            End If
        End Function

        Public Function CalculateCF(ByVal renderer As Object) As String
            Dim c As New CFCalculator
            Dim ret As String
            c.Nome = RPC.n2str(Me.GetParameter(renderer, "CN", ""))
            c.Cognome = RPC.n2str(Me.GetParameter(renderer, "CC", ""))
            c.CodiceCatasto = RPC.n2str(Me.GetParameter(renderer, "CT", ""))
            c.NatoAComune = RPC.n2str(Me.GetParameter(renderer, "CNAC", ""))
            c.NatoAProvincia = RPC.n2str(Me.GetParameter(renderer, "CNAP", ""))
            c.NatoIl = RPC.n2date(Me.GetParameter(renderer, "CNI", ""))
            c.Sesso = RPC.n2str(Me.GetParameter(renderer, "CS", ""))
            ret = c.Calcola
            If c.ErrorCode = 0 Then
                Return ret
            Else
                Throw New ArgumentException(c.ErrorDescription & "(0x" & Hex(c.ErrorCode) & ")")
                Return vbNullString
            End If
        End Function

        Public Function getAllegati(ByVal renderer As Object) As String
            Dim pID As Integer
            Dim persona As CPersona
            pID = RPC.n2int(Me.GetParameter(renderer, "id", ""))
            persona = Persone.GetItemById(pID)
            If persona.Attachments.Count = 0 Then
                Return vbNullString
            Else
                Return XML.Utils.Serializer.Serialize(persona.Attachments.ToArray, XMLSerializeMethod.Document)
            End If
        End Function

        Public Function GetOperatoriCRM(ByVal renderer As Object) As String
            Dim uffici As CUserUffici = Sistema.Users.CurrentUser.Uffici
            Dim crmGroup As CGroup = Sistema.Groups.GetItemByName("CRM")
            Dim ret As New CKeyCollection()
            For i As Integer = 0 To uffici.Count - 1
                Dim users As CUtentiXUfficioCollection = uffici(i).Utenti
                For j As Integer = 0 To users.Count - 1
                    Dim user As CUser = users(j)
                    If (user IsNot Nothing AndAlso user.Stato = ObjectStatus.OBJECT_VALID AndAlso Not ret.ContainsKey(user.UserName)) Then
                        If (crmGroup.Members.Contains(GetID(user))) Then ret.Add(user.UserName, user)
                    End If
                Next
            Next
            If (ret.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(ret.ToArray)
            Else
                Return ""
            End If
        End Function

        Public Function getOperatoriPerUfficio(ByVal renderer As Object) As String
            Dim uid As Integer
            Dim ufficio As CUfficio
            Dim user As CUser
            Dim ret As New CKeyCollection(Of CUser)
            Dim i1, i2 As Integer
            uid = Formats.ToInteger(RPC.n2int(Me.GetParameter(renderer, "u", "")))
            If (uid = 0) Then
                Dim uffici As CCollection(Of CUfficio) = Anagrafica.Uffici.GetPuntiOperativiConsentiti
                For i1 = 0 To uffici.Count - 1
                    ufficio = uffici(i1)
                    For i2 = 0 To ufficio.Utenti.Count - 1
                        user = ufficio.Utenti(i2)
                        If user.Stato = ObjectStatus.OBJECT_VALID Then
                            If Not ret.ContainsKey(user.UserName) Then ret.Add(user.UserName, user)
                        End If
                    Next
                Next
            Else
                ufficio = Anagrafica.Uffici.GetItemById(uid)
                For i2 = 0 To ufficio.Utenti.Count - 1
                    user = ufficio.Utenti(i2)
                    If user.Stato = ObjectStatus.OBJECT_VALID Then
                        If Not ret.ContainsKey(user.UserName) Then ret.Add(user.UserName, user)
                    End If
                Next
            End If
            ret.Comparer = New CUserComparer
            ret.Sort()


            If ret.Count = 0 Then
                Return vbNullString
            Else
                Dim tmp(ret.Count - 1) As Integer
                For i1 = 0 To ret.Count - 1
                    tmp(i1) = GetID(ret(i1))
                Next
                Return XML.Utils.Serializer.Serialize(tmp)
            End If
        End Function

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CPersonaCursor
        End Function



        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return Anagrafica.Persone.GetItemById(id)
        End Function

        'Public Function Import() As String
        '    Dim ext As New CContattoEsterno
        '    With ext
        '        .Nome = RPC.n2str(GetParameter(renderer, "nome"))
        '        .Cognome = RPC.n2str(GetParameter(renderer, "cognome"))
        '        .Telefono = RPC.n2str(GetParameter(renderer, "telefono"))
        '        .EMail = RPC.n2str(GetParameter(renderer, "email"))
        '        .Professione = RPC.n2str(GetParameter(renderer, "professione"))

        '        .NomeFonte = RPC.n2str(GetParameter(renderer, "fonte"))
        '        .Note = RPC.n2str(GetParameter(renderer, "notes"))
        '        .Riferimento = RPC.n2str(GetParameter(renderer, "Riferimento"))
        '    End With
        '    Dim nomeProvincia As String = RPC.n2str(GetParameter(renderer, "provincia"))
        '    Dim provincia As CProvincia = Anagrafica.Luoghi.Province.GetItemByName(nomeProvincia)
        '    If (provincia IsNot Nothing) Then
        '        ext.PuntoOperativo = provincia.PuntoOperativo
        '    End If
        '    ext.Save()
        '    Return vbNullString
        'End Function


        Public Function GetTipiCanaleAsArray(ByVal renderer As Object) As String
            Dim ov As Boolean = RPC.n2bool(GetParameter(renderer, "ov", "T"))
            Dim items As String() = Anagrafica.Canali.GetTipiCanale(ov)
            If (Arrays.Len(items) > 0) Then
                Return XML.Utils.Serializer.Serialize(items, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function

        Private Class FComparer
            Implements IComparer

            Public Sub New()
                DMD.DMDObject.IncreaseCounter(Me)
            End Sub

            Protected Overrides Sub Finalize()
                MyBase.Finalize()
                DMD.DMDObject.DecreaseCounter(Me)
            End Sub

            Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
                Dim a As IFonte = x
                Dim b As IFonte = y
                Return Strings.Compare(a.Nome, b.Nome, CompareMethod.Text)
            End Function
        End Class

        Public Function GetNomiFonte(ByVal renderer As Object) As String
            Dim tipoFonte As String = RPC.n2str(GetParameter(renderer, "tf", ""))
            Dim onlnyValid As Boolean = RPC.n2bool(GetParameter(renderer, "ov", ""))
            Dim q As String = Trim(RPC.n2str(Replace(Me.GetParameter(renderer, "_q", ""), "  ", "")))
            Dim ret As New System.Text.StringBuilder

            'WebSite.ASP_Server.ScriptTimeout = 15

            Dim fonti() As IFonte = {}

            ret.Append("<list>")

            If (tipoFonte = "") Then

                For Each provider As IFonteProvider In Anagrafica.Fonti.Providers
                    Dim names() As String = provider.GetSupportedNames
                    For Each name As String In names
                        fonti = provider.GetItemsAsArray(name, onlnyValid)
                        If (fonti IsNot Nothing AndAlso fonti.Length > 0) Then
                            Arrays.Sort(fonti, 0, fonti.Length, New FComparer)
                            For Each fonte As IFonte In fonti
                                If (InStr(fonte.Nome, q, CompareMethod.Text) > 0) Then

                                    ret.Append("<item>")
                                    ret.Append("<text>")
                                    ret.Append(Strings.HtmlEncode(Strings.JoinW(name, vbTab, fonte.Nome)))
                                    ret.Append("</text>")
                                    ret.Append("<value>")
                                    ret.Append(Strings.HtmlEncode(GetID(fonte)))
                                    ret.Append("</value>")
                                    ret.Append("<icon>")
                                    ret.Append(Strings.HtmlEncode(fonte.IconURL))
                                    ret.Append("</icon>")
                                    ret.Append("</item>")

                                End If
                            Next
                        End If

                    Next

                Next
            Else
                fonti = Anagrafica.Fonti.GetItemsAsArray(tipoFonte, tipoFonte, onlnyValid)

                If (fonti IsNot Nothing AndAlso fonti.Length > 0) Then
                    Arrays.Sort(fonti, 0, fonti.Length, New FComparer)
                    For Each fonte As IFonte In fonti
                        If (InStr(fonte.Nome, q, CompareMethod.Text) > 0) Then

                            ret.Append("<item>")
                            ret.Append("<text>")
                            ret.Append(Strings.HtmlEncode(fonte.Nome))
                            ret.Append("</text>")
                            ret.Append("<value>")
                            ret.Append(Strings.HtmlEncode(GetID(fonte)))
                            ret.Append("</value>")
                            ret.Append("<icon>")
                            ret.Append(Strings.HtmlEncode(fonte.IconURL))
                            ret.Append("</icon>")
                            ret.Append("</item>")

                        End If
                    Next
                End If
            End If

            ret.Append("</list>")

            Return ret.ToString
        End Function

        Public Function GetNomiCanale(ByVal renderer As Object) As String
            Dim tipoCanale As String = RPC.n2str(GetParameter(renderer, "tf", ""))
            Dim onlnyValid As Boolean = RPC.n2bool(GetParameter(renderer, "ov", ""))
            Dim q As String = Trim(RPC.n2str(Replace(Me.GetParameter(renderer, "_q", ""), "  ", "")))
            Dim ret As New System.Text.StringBuilder

            'WebSite.ASP_Server.ScriptTimeout = 15

            Dim canali As CCollection(Of CCanale) = Anagrafica.Canali.LoadAll
            canali.Sort()

            ret.Append("<list>")
            If (tipoCanale = "") Then
                For Each canale As CCanale In canali
                    If (InStr(canale.Nome, q, CompareMethod.Text) > 0) Then
                        ret.Append("<item>")
                        ret.Append("<text>")
                        ret.Append(Strings.HtmlEncode(Strings.JoinW(canale.Tipo, vbTab, canale.Nome)))
                        ret.Append("</text>")
                        ret.Append("<value>")
                        ret.Append(Strings.HtmlEncode(GetID(canale)))
                        ret.Append("</value>")
                        ret.Append("<icon>")
                        ret.Append(Strings.HtmlEncode(canale.IconURL))
                        ret.Append("</icon>")
                        ret.Append("</item>")

                    End If
                Next

            Else
                For Each canale As CCanale In canali
                    If ((tipoCanale = canale.Tipo) AndAlso InStr(canale.Nome, q, CompareMethod.Text) > 0) Then
                        ret.Append("<item>")
                        ret.Append("<text>")
                        ret.Append(Strings.HtmlEncode(canale.Nome))
                        ret.Append("</text>")
                        ret.Append("<value>")
                        ret.Append(Strings.HtmlEncode(GetID(canale)))
                        ret.Append("</value>")
                        ret.Append("<icon>")
                        ret.Append(Strings.HtmlEncode(canale.IconURL))
                        ret.Append("</icon>")
                        ret.Append("</item>")

                    End If
                Next
            End If

            ret.Append("</list>")

            Return ret.ToString
        End Function

        Public Function GetNomiFonteAsArray(ByVal renderer As Object) As String
            Dim items As New CCollection(Of String)
            For i As Integer = 0 To Anagrafica.Fonti.Providers.Count - 1
                Dim p As IFonteProvider = Anagrafica.Fonti.Providers(i)
                Dim names() As String = p.GetSupportedNames
                For j As Integer = 0 To UBound(names)
                    items.Add(names(j))
                Next
            Next
            items.Sort()
            Return XML.Utils.Serializer.Serialize(items.ToArray, XMLSerializeMethod.Document)
        End Function

        Public Function GetFontiAsArray(ByVal renderer As Object) As String
            Dim tf As String = RPC.n2str(Me.GetParameter(renderer, "tf", vbNullString))
            Dim ov As Boolean = RPC.n2str(Me.GetParameter(renderer, "ov", True))
            Dim items() As IFonte = Anagrafica.Fonti.GetItemsAsArray(tf, tf, ov)
            If (items IsNot Nothing) AndAlso (UBound(items) >= 0) Then
                Return XML.Utils.Serializer.Serialize(items, XMLSerializeMethod.Document)
            Else
                Return vbNullString
            End If
        End Function

        Public Function GetCanaliAsArray(ByVal renderer As Object) As MethodResults
            Dim tipoCanale As String = RPC.n2str(GetParameter(renderer, "tc", ""))
            Dim ov As Boolean = RPC.n2bool(GetParameter(renderer, "ov", "T"))
            Dim ret As New CCollection(Of CCanale)
            If (tipoCanale <> "") Then
                Dim cursor As New CCanaleCursor
                Try
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor.Nome.SortOrder = SortEnum.SORT_ASC
                    cursor.IgnoreRights = True
                    cursor.Tipo.Value = tipoCanale
                    If (ov) Then cursor.Valid.Value = True
                    While Not cursor.EOF
                        ret.Add(cursor.Item)
                        cursor.MoveNext()
                    End While
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                    Throw
                Finally
                    If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                End Try

            End If

            Return New MethodResults(ret)
        End Function

        Public Overridable Function CanTransfer(ByVal item As CPersona) As Boolean
            Dim ret As Boolean
            ret = Me.Module.UserCanDoAction("transfer")
            ret = Me.Module.UserCanDoAction("transfer_own") And (item.CreatoDaId = GetID(Users.CurrentUser))
            If (ret = False) Then
                If Me.Module.UserCanDoAction("transfer_office") Then
                    ret = (item.PuntoOperativo Is Nothing) OrElse (Users.CurrentUser.Uffici.HasOffice(item.PuntoOperativo))
                End If
            End If
            Return ret
        End Function

        Public Function TrasferisciContatto(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(GetParameter(renderer, "pid", ""))
            Dim oid As Integer = RPC.n2int(GetParameter(renderer, "oid", ""))
            Dim messaggio As String = RPC.n2str(GetParameter(renderer, "msg", ""))
            Dim persona As CPersona = Anagrafica.Persone.GetItemById(pid)
            If (Me.CanTransfer(persona) = False) Then Throw New PermissionDeniedException(Me.Module, "transfer")
            Dim ufficio As CUfficio = Anagrafica.Uffici.GetItemById(oid)

            persona.TransferTo(ufficio, messaggio)


            'For i As Integer = 0 To ufficio.Utenti.Count - 1
            '    Dim u As CUser = ufficio.Utenti(i)
            '    If CustomerCalls.CRM.CRMGroup.Members.GetItemById(GetID(u)) IsNot Nothing Then
            '        Sistema.Notifiche.ProgramAlert(u, "L'utente [" & Users.CurrentUser.Nominativo & "] ha passato il contatto [" & persona.Nominativo & "]<br><i>" & messaggio & "</i>", Now, persona)
            '    End If
            'Next

            Return ""
        End Function



        Public Function GetPopUpInfoPersona(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(GetParameter(renderer, "pid", "0"))
            If (pid = 0) Then Return ""
            Dim info As New CPopUpInfoPersona(Anagrafica.Persone.GetItemById(pid))
            Return XML.Utils.Serializer.Serialize(info)
        End Function

        Public Function LoadContattiPersona(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.int2n(GetParameter(renderer, "pid", ""))
            Dim persona As CPersona = Anagrafica.Persone.GetItemById(pid)
            'If (Not Anagrafica.Persone.Module.UserCanDoAction.CanList(persona) = False) Then Throw New PermissionDeniedException(Me.Module, "list")
            Dim col As CContattiPerPersonaCollection = persona.Recapiti
            If (col.Count = 0) Then
                Return ""
            Else
                Return XML.Utils.Serializer.Serialize(col.ToArray)
            End If
        End Function

        Public Function GetStatisticheStato(ByVal renderer As Object) As String
            Dim cursor As CPersonaCursor = Nothing
            Dim dbRis As System.Data.IDataReader = Nothing

            Try
                Dim ret As New CKeyCollection
                cursor = XML.Utils.Serializer.Deserialize(RPC.n2str(GetParameter(renderer, "c", "")))
                Dim dbSQL As String = "SELECT [DettaglioEsito], Count(*) As [NumItems] FROM (" & cursor.GetSQL & ") WHERE [DettaglioEsito]<>'' GROUP BY [DettaglioEsito]"
                dbRis = APPConn.ExecuteReader(dbSQL)
                While dbRis.Read
                    Dim key As String = Strings.Trim(Formats.ToString(dbRis("DettaglioEsito")))
                    Dim cnt As Integer = Formats.ToInteger(dbRis("NumItems"))
                    ret.Add(key, cnt)
                End While

                Return XML.Utils.Serializer.Serialize(ret)
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function
    End Class


End Namespace