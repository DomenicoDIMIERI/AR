Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms

    Public Class ImportFromCLHandler
        Inherits CBaseModuleHandler

        Const TOLLERANZAMONTANTE = 1000     '1000 euro di tolleranza per il montante
        Const TOLLERANZADATA = 200          '200 giorni per la data di caricamento
        Private message As String

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SAnnotations)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CCQSPDOfferteCursor
        End Function

        Public Function ImportFromCL(ByVal renderer As Object) As String
            If Not Me.Module.UserCanDoAction("import") Then Throw New PermissionDeniedException(Me.Module, "ImportFromCL")
            Dim fileName As String = RPC.n2str(Me.GetParameter(renderer, "fp", ""))
            Dim xlsConn As CXlsDBConnection
            Dim xlsTable As CDBTable
            Dim xlsRis As DBReader
            Dim cessionario As CCQSPDCessionarioClass
            Dim note As CAnnotazione

            Dim lista As CListaRicontatti

            lista = ListeRicontatto.GetItemByName("Lista contatti CL")
            If (lista Is Nothing) Then
                lista = New CListaRicontatti
                lista.Name = "Lista contatti CL"
                lista.Stato = ObjectStatus.OBJECT_VALID
                lista.Save()
            End If

            message = ""

            'Preparo il cessionario
            cessionario = CQSPD.Cessionari.GetItemByName("Prestitalia")
            If (cessionario Is Nothing) Then
                cessionario = New CCQSPDCessionarioClass
                cessionario.Nome = "Prestitalia"
                cessionario.Stato = ObjectStatus.OBJECT_VALID
                cessionario.Save()
            End If

            fileName = Trim(RPC.n2str(Me.GetParameter(renderer, "fn", vbNullString)))
            If (fileName = vbNullString) Then Throw New ArgumentNullException("fileName")
            xlsConn = New CXlsDBConnection(Sistema.ApplicationContext.MapPath(fileName))
            xlsConn.OpenDB()

            xlsTable = xlsConn.Tables(0)
            xlsRis = New DBReader(xlsTable)
            While xlsRis.Read
                Dim numeroPratica As String = ""
                Dim nomePuntoOperativo As String = ""
                Dim nomeOperatore As String = ""
                Dim nomeCliente As String = ""
                Dim cfCliente As String = ""
                Dim nomeAmministrazione As String = ""
                Dim statoAmministrazione As String = ""
                Dim statoPratica As String = ""
                Dim numeroRate As Nullable(Of Integer) = Nothing
                Dim rata As Nullable(Of Double) = Nothing
                Dim montante As Nullable(Of Double) = Nothing
                Dim netto As Nullable(Of Double) = Nothing
                Dim nomeBanca As String = ""
                Dim contratto As String = ""
                Dim nomeProdotto As String = ""
                Dim nomeAgenzia As String = ""
                Dim dataCaricamento As Nullable(Of Date) = Nothing
                Dim morosa As String = ""
                Dim programmaRicontatto As Boolean = False
                Dim testoRicontatto As String = ""
                Dim pratica As CRapportino
                Dim clienti As CCollection(Of CPersona)
                Dim cliente As CPersonaFisica
                Dim po As CUfficio
                Dim op As CUser
                Dim ricontatto As CRicontatto


                xlsRis.Read("Pratica", numeroPratica)
                xlsRis.Read("PuntoOperativo", nomePuntoOperativo)
                xlsRis.Read("Operatore", nomeOperatore)
                xlsRis.Read("Nominativo", nomeCliente)
                xlsRis.Read("CF", cfCliente)
                xlsRis.Read("Amministrazione", nomeAmministrazione)
                xlsRis.Read("StatoAmministrazione", statoAmministrazione)
                xlsRis.Read("StatoPratica", statoPratica)
                xlsRis.Read("Rate", numeroRate)
                xlsRis.Read("Rata", rata)
                xlsRis.Read("Montante", montante)
                xlsRis.Read("Netto", netto)
                xlsRis.Read("Banca", nomeBanca)
                xlsRis.Read("Contratto", contratto)
                xlsRis.Read("Prodotto", nomeProdotto)
                xlsRis.Read("Agenzia", nomeAgenzia)
                xlsRis.Read("DataCaricamento", dataCaricamento)
                xlsRis.Read("Morosa", morosa)


                po = Anagrafica.Uffici.GetItemByName(nomePuntoOperativo)
                op = Users.GetItemDisplayByName(nomeOperatore)

                clienti = Anagrafica.Persone.FindPersoneByCF(cfCliente)
                If (clienti.Count = 0) Then
                    message &= "Creo il cliente " & nomeCliente & " (CF: " & cfCliente & ")" & vbNewLine
                    cliente = New CPersonaFisica
                    Me.ParseNomeCliente(cliente, nomeCliente)
                    cliente.CodiceFiscale = cfCliente
                    programmaRicontatto = True
                    testoRicontatto = "Cliente importato il " & Formats.FormatUserDate(Now) & " dal modulo Importo From CL" & vbNewLine & "Pratica N°" & numeroPratica & " (" & nomeProdotto & ", " & numeroRate & " x " & Formats.FormatValuta(rata) & " = " & Formats.FormatValuta(montante) & ") del " & Formats.FormatUserDate(dataCaricamento) & vbNewLine & "Probabilmente mai ricontattato perché non noto."
                Else
                    cliente = clienti(0)
                    If (clienti.Count > 1) Then message &= "Esistono più anagrafiche corrispondenti al cliente " & nomeCliente & " (CF: " & cfCliente & ") uso la prima" & vbNewLine
                End If

                Dim cfCalculator As New CFCalculator
                cfCalculator.CodiceFiscale = cfCliente
                cfCalculator.Inverti()
                If (cliente.NatoA.Citta = "") Then cliente.NatoA.Citta = cfCalculator.NatoAComune
                If (cliente.NatoA.Provincia = "") Then cliente.NatoA.Provincia = cfCalculator.NatoAProvincia
                If (cliente.DataNascita.HasValue = False) Then cliente.DataNascita = cfCalculator.NatoIl
                If (cliente.Sesso = "") Then cliente.Sesso = cfCalculator.Sesso
                If (cliente.PuntoOperativo Is Nothing) Then cliente.PuntoOperativo = po

                cliente.Stato = ObjectStatus.OBJECT_VALID
                If (GetID(cliente) = 0) Then
                    cliente.Save()
                    note = New CAnnotazione(cliente)
                    note.Valore = "Cliente creato automaticamente dal modulo Import From CL il " & Formats.FormatUserDate(Now)
                    note.Stato = ObjectStatus.OBJECT_VALID
                    note.Save()
                ElseIf DBUtils.IsChanged(cliente) Then
                    cliente.Save()
                End If

                'Cerco il prodotto
                Dim prodotto As CCQSPDProdotto
                prodotto = CQSPD.Prodotti.GetItemByName(GetID(cessionario), nomeProdotto)
                If (prodotto Is Nothing) Then
                    prodotto = New CCQSPDProdotto
                    prodotto.Cessionario = cessionario
                    prodotto.Nome = nomeProdotto
                    prodotto.Stato = ObjectStatus.OBJECT_VALID
                    prodotto.Save()
                    note = New CAnnotazione(prodotto)
                    note.Stato = ObjectStatus.OBJECT_VALID
                    note.Valore = "Prodotto creato automaticamente dal modulo Import From CL il " & Formats.FormatUserDate(Now)
                    note.Save()
                End If

                'Cerco l'amministrazione
                Dim amministrazione As CAzienda
                amministrazione = Anagrafica.Aziende.GetItemByName(nomeAmministrazione)
                If (amministrazione Is Nothing) Then
                    message &= "Creo l'amministrazione " & nomeAmministrazione & vbNewLine
                    amministrazione = New CAzienda
                    amministrazione.RagioneSociale = nomeAmministrazione
                    amministrazione.Stato = ObjectStatus.OBJECT_VALID
                    amministrazione.Save()
                    note = New CAnnotazione(amministrazione)
                    note.Valore = "Amministrazione creata automaticamente dal modulo Importo From CL il " & Formats.FormatUserDate(Now) & vbNewLine & "Morosa: " & morosa & vbNewLine & "Stato Amministrazione: " & statoAmministrazione
                    note.Stato = ObjectStatus.OBJECT_VALID
                    note.Save()
                End If

                'Verifico se la pratica esiste già
                pratica = CQSPD.Pratiche.GetItemByNumeroEsterno(cessionario, numeroPratica)
                'Se non esiste provo a vedere se esiste una pratica "vicina"
                If (pratica Is Nothing) Then
                    pratica = Me.CercaPraticaVicina(cliente, montante, dataCaricamento, prodotto)
                    If (pratica IsNot Nothing) Then message &= "La pratica " & pratica.NumeroPratica & " è ""vicina"" alla N°" & numeroPratica & " del cliente " & nomeCliente & " (CF: " & cfCliente & "). Uso questa" & vbNewLine
                End If
                'Altrimento la creao
                If (pratica Is Nothing) Then
                    message &= "Non trovo alcuna pratica ""vicina"" alla N°" & numeroPratica & " del cliente " & nomeCliente & " (CF: " & cfCliente & "). La creao." & vbNewLine
                    Dim offerta As New COffertaCQS
                    offerta.OffertaLibera = True
                    offerta.Cliente = cliente
                    offerta.Cessionario = cessionario
                    offerta.Prodotto = prodotto
                    offerta.Rata = rata
                    offerta.Durata = numeroRate
                    offerta.MontanteLordo = montante
                    offerta.NettoRicavo = netto
                    offerta.DataDecorrenza = dataCaricamento
                    offerta.Stato = ObjectStatus.OBJECT_VALID
                    offerta.Save()

                    pratica = New CRapportino
                    pratica.Cessionario = cessionario
                    pratica.Prodotto = prodotto
                    pratica.Cliente = cliente
                    pratica.NumeroEsterno = numeroPratica
                    pratica.NumeroRate = numeroRate
                    pratica.Rata = rata
                    pratica.MontanteLordo = montante
                    pratica.NettoRicavo = netto
                    pratica.DataDecorrenza = dataCaricamento
                    pratica.StatoContatto.Data = dataCaricamento
                    pratica.StatoContatto.Offerta = offerta
                    pratica.OffertaCorrente = offerta
                    pratica.Impiego.Azienda = amministrazione
                    pratica.Impiego.EntePagante = amministrazione
                    pratica.Stato = ObjectStatus.OBJECT_VALID
                    CQSPD.Pratiche.EventEnabled = False
                    pratica.Save()
                    CQSPD.Pratiche.EventEnabled = True
                    'TO DO
                    'pratica.ChangeStatus (  
                    pratica.StatoRichiestaDelibera.Data = dataCaricamento
                    pratica.StatoRichiestaDelibera.Offerta = offerta
                    pratica.StatoDeliberata.Data = dataCaricamento
                    pratica.StatoDeliberata.Offerta = offerta
                    pratica.StatoProntaLiquidazione.Data = dataCaricamento
                    pratica.StatoProntaLiquidazione.Offerta = offerta
                    pratica.StatoLiquidata.Data = dataCaricamento
                    pratica.StatoLiquidata.Offerta = offerta
                    pratica.StatoArchiviata.Data = dataCaricamento
                    pratica.StatoArchiviata.Offerta = offerta


                    note = New CAnnotazione(cliente)
                    note.IDContesto = GetID(pratica)
                    note.TipoContesto = TypeName(pratica)
                    note.Stato = ObjectStatus.OBJECT_VALID
                    note.Valore = "Pratica creata automaticamente dal modulo Import From CL il " & Formats.FormatUserDate(Now) & vbNewLine & "Agenzia: " & nomeAgenzia & vbNewLine & "Banca: " & nomeBanca & "Contratto: " & contratto & vbNewLine & "Stato Pratica: " & statoPratica
                    note.Save()

                    offerta.Pratica = pratica
                    offerta.Save()
                End If
                If (pratica.PuntoOperativo Is Nothing) Then pratica.PuntoOperativo = po
                If (pratica.IsChanged) Then pratica.Save()

                If (programmaRicontatto = False) Then
                    Dim cursor As New CRicontattiCursor
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor.StatoRicontatto.Value = StatoRicontatto.PROGRAMMATO
                    cursor.IDPersona.Value = GetID(cliente)
                    ' cursor.NomeLista.Value = ""
                    If (cursor.Count = 0) Then
                        programmaRicontatto = True
                        testoRicontatto = "Cliente importato il " & Formats.FormatUserDate(Now) & " dal modulo Importo From CL" & vbNewLine & "Pratica N°" & numeroPratica & " (" & nomeProdotto & ", " & numeroRate & " x " & Formats.FormatValuta(rata) & " = " & Formats.FormatValuta(montante) & ") del " & Formats.FormatUserDate(dataCaricamento) & vbNewLine & "Anagrafica ok, pratica mancante e nessuna data di ricontatto."
                    End If
                    cursor.Dispose()
                End If
                If (programmaRicontatto) Then
                    If (cliente.Deceduto) Then
                        message &= "Non programmo il ricontatto per " & nomeCliente & " (CF: " & cfCliente & ") perchè risulta deceduto" & vbNewLine
                    Else
                        ricontatto = New CRicontatto
                        ricontatto.GiornataIntera = True
                        ricontatto.AssegnatoA = op
                        ricontatto.PuntoOperativo = po
                        ricontatto.Persona = cliente
                        ricontatto.DataPrevista = Now
                        ricontatto.Stato = ObjectStatus.OBJECT_VALID
                        ricontatto.StatoRicontatto = StatoRicontatto.PROGRAMMATO
                        'ricontatto.NomeLista = lista.Name
                        ricontatto.Note = testoRicontatto
                        ricontatto.Save()
                    End If
                End If
            End While
            xlsConn.Dispose()

            Me.Module.DispatchEvent(New EventDescription("importedfromcl", "Lista importata dal file " & fileName, message))

            Return message
        End Function

        Private Sub ParseNomeCliente(ByVal persona As CPersonaFisica, ByVal value As String)
            Dim p As Integer
            Dim nome, cognome As String
            value = Trim(Replace(value, "  ", " "))
            p = InStr(value, " ")
            If (p > 0) Then
                cognome = Strings.Left(value, p - 1)
                nome = Strings.Mid(value, p + 1)
                Select Case UCase(cognome)
                    Case "DI", "DE", "DELLO"
                        Dim p1 As Integer
                        p1 = InStr(nome, " ")
                        If (p1 > 0) Then
                            cognome = cognome & " " & Left(nome, p1 - 1)
                            nome = Mid(nome, p1 + 1)
                        End If
                End Select
            Else
                cognome = value
                nome = ""
            End If
            persona.Nome = nome
            persona.Cognome = cognome
        End Sub

        Private Function CercaPraticaVicina(ByVal cliente As CPersonaFisica, ByVal montante As Nullable(Of Decimal), ByVal dataCaricamento As Nullable(Of Date), ByVal prodotto As CCQSPDProdotto) As CRapportino
            Dim montanteOk, dataOk, okProdotto As Boolean
            Dim items As CCollection(Of CRapportino)
            
            'Cerco tutte le pratiche del cliente
            items = CQSPD.Pratiche.GetPraticheByPersona(cliente)
            'Aggiungo eventuali pratiche con lo stesso codice fiscale
            Dim cursor As New CRapportiniCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.CodiceFiscale.Value = cliente.CodiceFiscale
            cursor.IDCliente.Value = GetID(cliente)
            cursor.IDCliente.Operator = OP.OP_NE
            While Not cursor.EOF
                items.Add(cursor.Item)
                message &= "La pratica " & cursor.Item.NumeroPratica & " è associabile al cliente " & cliente.Nominativo & " tramite il CF (" & cliente.CodiceFiscale & ") ma appartiene ad un oggetto diverso" & vbNewLine
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            For i As Integer = 0 To items.Count - 1
                Dim item As CRapportino = items(i)
                If (item.StatoAttuale.MacroStato.HasValue AndAlso (item.StatoAttuale.MacroStato = StatoPraticaEnum.STATO_LIQUIDATA OrElse item.StatoAttuale.MacroStato = StatoPraticaEnum.STATO_ARCHIVIATA)) Then
                    If (item.IDProdotto = GetID(prodotto)) Then
                        okProdotto = True
                    ElseIf (item.NomeProdotto = prodotto.Nome) AndAlso (item.NomeCessionario = prodotto.NomeCessionario) Then
                        okProdotto = True
                    Else
                        okProdotto = False
                    End If
                    If (montante.HasValue AndAlso item.MontanteLordo > 0) Then
                        montanteOk = Math.Abs(montante.Value - item.MontanteLordo) <= TOLLERANZAMONTANTE
                    Else
                        montanteOk = False
                    End If
                    Dim dRef As Nullable(Of Date) = item.StatoRichiestaDelibera.Data
                    If (dRef.HasValue = False) Then dRef = item.StatoContatto.Data
                    If (dRef.HasValue = False) Then dRef = item.StatoLiquidata.Data
                    If (dRef.HasValue = False) Then dRef = item.DataDecorrenza

                    If (dataCaricamento.HasValue) AndAlso (dRef.HasValue) Then
                        dataOk = Math.Abs(Calendar.DateDiff(DateInterval.Day, dataCaricamento.Value, dRef.Value)) <= TOLLERANZADATA
                    Else
                        dataOk = False
                    End If
                    If (okProdotto AndAlso montanteOk AndAlso dataOk) Then Return item
                End If
            Next
            Return Nothing
        End Function
    End Class


End Namespace