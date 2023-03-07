Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.WebSite
Imports DMD.Anagrafica
Imports DMD.Forms.Utils
Imports DMD.CustomerCalls
Imports DMD.XML

Namespace Forms

    Public Class CRicontattiModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SDelete)
            Me.UseLocal = True
            'AddHandler CustomerCalls.CRM.NuovoContatto, AddressOf Me.handleNuovaTelefonata
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CRicontattiCursor
        End Function

        Public Function GetRicontattiPerPersona(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(GetParameter(renderer, "pid", "0"))
            Dim ret As New CCollection(Of CRicontatto)
            If (pid <> 0) Then
                Dim cursor As New CRicontattiCursor
                Try
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor.CreatoIl.SortOrder = SortEnum.SORT_DESC
                    cursor.IDPersona.Value = pid
                    cursor.IgnoreRights = True
                    'If (Not Users.CurrentUser.IsAdministrator) Then cursor.NomeLista.Value = ""
                    'cursor.NomeLista.IncludeNulls = True
                    While (Not cursor.EOF())
                        ret.Add(cursor.Item)
                        cursor.MoveNext()
                    End While
                Catch ex As Exception
                    Throw
                Finally
                    cursor.Dispose()
                End Try
            End If
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

        Public Function GetProssimoRicontatto(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(GetParameter(renderer, "pid", "0"))
            'Dim nomeLista As String = RPC.n2str(GetParameter(renderer, "nl", ""))
            Return XML.Utils.Serializer.Serialize(Ricontatti.GetProssimoRicontatto(pid)) ', nomeLista
        End Function

        Public Function ContaRicontattiPerData(ByVal renderer As Object) As String
            Dim oid As Integer = RPC.n2int(GetParameter(renderer, "oid", ""))
            Dim d As Date = RPC.n2date(GetParameter(renderer, "d", ""))
            Return XML.Utils.Serializer.SerializeInteger(Ricontatti.ContaRicontattiPerData(oid, d))
        End Function

        Public Function GetUltimaChiamata(ByVal renderer As Object) As String
            Dim pID As Integer = RPC.n2int(GetParameter(renderer, "pid", ""))
            Dim ric As DMD.CustomerCalls.CContattoUtente = CustomerCalls.CRM.GetUltimoContatto(pID)
            If (ric Is Nothing) Then Return ""
            Return XML.Utils.Serializer.Serialize(ric, XMLSerializeMethod.Document)
        End Function

        'Public Function GetScadenze() As String
        '    Dim ret As CCalendarStats
        '    Dim text As String = RPC.n2str(GetParameter(renderer, "text", ""))
        '    Dim filter As CRMFilter = XML.Utils.Serializer.Deserialize(text)
        '    ret = CustomerCalls.Telefonate.GetStats(filter)
        '    Return XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document)
        'End Function

        'Public Function GetActivePersons(ByVal renderer As Object) As String
        '    Dim text As String = Trim(RPC.n2str(GetParameter(renderer, "text", "")))
        '    If (String.IsNullOrEmpty(text)) Then Throw New ArgumentNullException("text")
        '    Dim filter As CRMFilter = XML.Utils.Serializer.Deserialize(text)
        '    If (filter Is Nothing) Then Throw New ArgumentNullException("filter")

        '    Dim ret As CCollection(Of CActivePerson) = Ricontatti.GetActivePersons(filter)

        '    Dim persone As Integer() = {}
        '    For Each r As CActivePerson In ret
        '        If (r.PersonID <> 0) Then
        '            Dim i As Integer = Array.BinarySearch(persone, r.PersonID)
        '            If (i < 0) Then
        '                i = Arrays.GetInsertPosition(persone, r.PersonID, 0, persone.Length)
        '                persone = Arrays.Insert(persone, 0, persone.Length, r.PersonID, i)
        '            End If
        '        End If
        '    Next

        '    Dim ultimiContatti As New CKeyCollection(Of CContattoUtente)
        '    If (persone.Length > 0) Then
        '        Dim cursor As CPersonStatsCursor = Nothing
        '        Try
        '            cursor = New CPersonStatsCursor
        '            cursor.IDPersona.ValueIn(persone)
        '            While Not cursor.EOF
        '                Dim info As CPersonStats = cursor.Item
        '                Dim c As CContattoUtente = info.UltimoContattoOk
        '                If (c Is Nothing) Then c = info.UltimoContattoNo
        '                If (c IsNot Nothing) Then ultimiContatti.Add("K" & c.IDPersona, c)
        '                cursor.MoveNext()
        '            End While
        '            cursor.Dispose()
        '        Catch ex As Exception
        '            Sistema.Events.NotifyUnhandledException(ex)
        '            Throw
        '        Finally
        '            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
        '        End Try
        '    End If


        '    For Each r As CActivePerson In ret
        '        If (r.Person IsNot Nothing) Then
        '            If (r.Person.TipoPersona = TipoPersona.PERSONA_FISICA) Then
        '                With DirectCast(r.Person, CPersonaFisica)
        '                    r.MoreInfo.Add("Nato A:", .NatoA.NomeComune & " il " & Formats.FormatUserDate(.DataNascita))
        '                    If .ImpiegoPrincipale IsNot Nothing Then
        '                        r.MoreInfo.Add("Impiego", FormatImpiego(.ImpiegoPrincipale))
        '                    End If
        '                End With
        '            End If
        '            Dim u As DMD.CustomerCalls.CContattoUtente = ultimiContatti.GetItemByKey("K" & r.PersonID)
        '            If (u IsNot Nothing) Then r.MoreInfo.Add("UltimaChiamata", u)
        '        End If
        '    Next


        '    Return XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document)
        'End Function

        'Private Function FormatImpiego(ByVal impiego As CImpiegato) As String
        '    Dim ret As String = ""
        '    If (impiego.Posizione <> "") Then ret = impiego.Posizione
        '    If (impiego.TipoRapporto <> "") Then ret = Strings.Combine(ret, "(" & impiego.TipoRapporto & ")", " ")
        '    If (impiego.NomeAzienda <> "") Then ret = Strings.Combine(ret, impiego.NomeAzienda, " presso ")
        '    If (impiego.DataAssunzione.HasValue) Then ret = Strings.Combine(ret, Formats.FormatUserDate(impiego.DataAssunzione), " dal ")
        '    Return ret
        'End Function



        Function GetRitardi(ByVal renderer As Object) As String
            Dim ret As CCalendarStats
            Dim d1, d2 As Nullable(Of Date)
            Dim po, op As Integer

            d1 = RPC.n2date(Me.GetParameter(renderer, "d1", ""))
            d2 = RPC.n2date(Me.GetParameter(renderer, "d2", ""))
            po = Formats.ToInteger(RPC.n2str(Me.GetParameter(renderer, "po", "")))
            op = Formats.ToInteger(RPC.n2int(Me.GetParameter(renderer, "op", "")))

            'If TypeOf (WebSite.Application.Contents("CRICONTATTI_STATS" & GetID(Users.CurrentUser))) Is CCalendarStats Then
            '    ret = WebSite.Application.Contents("CRICONTATTI_STATS" & GetID(Users.CurrentUser))
            'Else
            ret = New CCalendarStats
            ret.Effettuate = CustomerCalls.Telefonate.ContaTelefonateEffettuatePerData(po, op, d1, d2).Numero
            ret.Ricevute = CustomerCalls.Telefonate.ContaTelefonateRicevutePerData(po, op, d1, d2).Numero
            'WebSite.Application.Contents("CRICONTATTI_STATS" & GetID(Users.CurrentUser)) = ret
            'End If

            Return XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document)
        End Function

        ' Private Sub handleNuovaTelefonata(ByVal e As CustomerCalls.ContattoEventArgs)
        'If TypeOf (WebSite.Application.Contents("CRICONTATTI_STATS" & GetID(Users.CurrentUser))) Is CCalendarStats Then
        '    Dim ret As CCalendarStats = WebSite.ASP_Session.Contents("CRICONTATTI_STATS" & GetID(Users.CurrentUser))
        '    If (Not (TypeOf (e.Contatto) Is CustomerCalls.CVisita) AndAlso e.Contatto.Ricevuta) Then
        '        ret.TelefonateRicevute += 1
        '    Else
        '        ret.TelefonateEffettuate += 1
        '    End If
        '    WebSite.Application.Contents("CRICONTATTI_STATS" & GetID(Users.CurrentUser)) = ret
        'End If
        'End Sub


        Public Function GetNomiListeRicontatto(ByVal renderer As Object) As String
            Dim ret As New System.Collections.Hashtable
            Dim items1 As CCollection(Of CListaRicontatti) = Anagrafica.ListeRicontatto.LoadAll
            For i As Integer = 0 To items1.Count - 1
                Dim nm As String = Trim(items1(i).Name)
                If (nm <> "" AndAlso Not ret.ContainsKey(nm)) Then ret.Add(nm, nm)
            Next

            If (ret.Count > 0) Then
                'Return XML.Utils.Serializer.Serialize(ret.ToArray, XMLSerializeMethod.Document)
                Dim items() As String
                ReDim items(ret.Count - 1)
                ret.Keys.CopyTo(items, 0)
                System.Array.Sort(items)
                Return XML.Utils.Serializer.Serialize(items, XMLSerializeMethod.Document)
            Else
                Return vbNullString
            End If
        End Function

        Public Function SalvaListaRicontatti(ByVal renderer As Object) As String
            Dim nome As String = RPC.n2str(GetParameter(renderer, "nome", vbNullString))
            Dim text As String = RPC.n2str(GetParameter(renderer, "text", vbNullString))
            Dim lista As CListaRicontatti

            If (nome = "") Then Throw New ArgumentNullException("Il nome della lista è vuoto")

            Dim po As CUfficio = Nothing
            If (Users.CurrentUser IsNot Nothing AndAlso Users.CurrentUser.Uffici.Count > 0) Then po = Users.CurrentUser.Uffici(0)

            lista = Anagrafica.ListeRicontatto.GetItemByName(nome)
            If (lista Is Nothing) Then
                lista = New CListaRicontatti
                lista.Name = nome
                lista.PuntoOperativo = po
                lista.Proprietario = Sistema.Users.CurrentUser
            End If
            lista.Stato = ObjectStatus.OBJECT_VALID
            lista.Save()

            text = Trim(text)
            If (text = "") Then Return vbNullString


            Dim col() As Object = XML.Utils.Serializer.Deserialize(text)
            'col = Anagrafica.Persone.Find(text, False)
            For i As Integer = 0 To col.Count - 1
                Dim item As CPersonaInfo = col(i)
                Dim ric As CRicontatto = Ricontatti.ProgrammaRicontatto(item.Persona, Now, "Salvataggio nella lista: " & nome, "", "", nome, po, Users.CurrentUser)
                ric.Save()
            Next
            Return vbNullString
        End Function


        Public Function GetArrayTipiRapporto(ByVal renderer As Object) As String
            'Dim ret As System.Collections.ArrayList
            'For i As Integer = 0 To Anagrafica.TipiRapporto.CachedItems.Count
            '    Dim item As CTipoRapporto = Anagrafica.TipiRapporto.CachedItems(i)
            '    ret.Add(item.Descrizione)
            'Next
            'Dim cursor As New CTipoRapportoCursor
            'cursor.Descrizione.SortOrder = SortEnum.SORT_ASC
            'While Not cursor.EOF
            '    ret.Add(cursor.Item)
            '    cursor.MoveNext()
            'End While
            'cursor.Reset()
            'If (ret.Count > 0) Then
            Dim ret As CCollection(Of CTipoRapporto) = Anagrafica.TipiRapporto.LoadAll
            If (ret.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(ret.ToArray, XMLSerializeMethod.Document)
            Else
                Return vbNullString
            End If
        End Function


        Public Function ElaboraRicontattoPrecedente(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            Dim ric As Integer = RPC.n2int(Me.GetParameter(renderer, "ric", ""))

            Dim cursor As New CRicontattiCursor()
            cursor.ID.Value = ric
            cursor.ID.Operator = OP.OP_NE
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.IDPersona.Value = pid
            cursor.DataPrevista.SortOrder = SortEnum.SORT_ASC
            cursor.StatoRicontatto.Value = StatoRicontatto.PROGRAMMATO
            'cursor.NomeLista.Value = ""
            'cursor.NomeLista.IncludeNulls = True

            While Not cursor.EOF()
                Dim tmp As CRicontatto = cursor.Item
                tmp.StatoRicontatto = StatoRicontatto.ANNULLATO
                tmp.Save()
                cursor.MoveNext()
            End While
            cursor.Dispose()

            Return ""
        End Function


    End Class



End Namespace