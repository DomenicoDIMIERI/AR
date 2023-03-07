Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.CustomerCalls


Namespace Forms

    Public Class ImportaFinMakerHandler
        Inherits CBaseModuleHandler

        Const TOLLERANZAMONTANTE = 1000     '1000 euro di tolleranza per il montante
        Const TOLLERANZADATA = 200          '200 giorni per la data di caricamento

        Private Shared ReadOnly ID_TipoDocumento As Integer() = {1, 2, 3, 4, 5, 6}
        Private Shared ReadOnly Nome_TipoDocumento As String() = {"Carta d'Identità", "Patente di Guida", "Passaporto", "Porto d'armi", "Tessera Postale", "Tessera Ministeriale"}

        Private message As System.Text.StringBuilder
        Private lista As CListaRicontatti
        Private tipoFonte As String
        Private fonte As IFonte
        Private m_PuntiOperativi As CKeyCollection(Of CUfficio) = Nothing
        Private nomeLista As String = ""
        Private m_Fonti As CKeyCollection(Of CFonte) = Nothing
        Private NomePO As String = ""

        Public Sub New()
            MyBase.New()
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return Nothing
        End Function

        Public Function GetPuntoOperativo(ByVal nome As String) As CUfficio
            nome = LCase(Trim(nome))
            If (nome = "") Then Return Nothing
            If (Me.m_PuntiOperativi Is Nothing) Then
                Me.m_PuntiOperativi = New CKeyCollection(Of CUfficio)
                For Each u As CUfficio In Anagrafica.Uffici.GetPuntiOperativi()
                    Me.m_PuntiOperativi.Add(LCase(u.Nome), u)
                Next
            End If
            Return Me.m_PuntiOperativi.GetItemByKey(nome)
        End Function

        Public Function GetFonteAltro(ByVal nomeFonte As String) As CFonte
            nomeFonte = Trim(nomeFonte)
            If (nomeFonte = "") Then Return Nothing
            If (Me.m_Fonti Is Nothing) Then
                Me.m_Fonti = New CKeyCollection(Of CFonte)
                Dim cursor As New CFontiCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Tipo.Value = "Altro"
                cursor.Nome.SortOrder = SortEnum.SORT_ASC
                While Not cursor.EOF
                    Me.m_Fonti.Add(cursor.Item.Nome, cursor.Item)
                    cursor.MoveNext()
                End While
                cursor.Dispose()
            End If
            Dim ret As CFonte = Me.m_Fonti.GetItemByKey(LCase(nomeFonte))
            If (ret Is Nothing) Then
                ret = New CFonte
                ret.Tipo = "Altro"
                ret.Nome = nomeFonte
                ret.Stato = ObjectStatus.OBJECT_VALID
                ret.Save()
                Me.m_Fonti.Add(nomeFonte, ret)
            End If
            Return ret
        End Function

        Public Overloads Function ImportList(ByVal fileName As String, ByVal po As CUfficio, ByVal nomeLista As String, ByVal tipoFonte As String, ByVal idFonte As Integer) As String
            Me.NomePO = po.Nome
            Me.nomeLista = nomeLista


            If (tipoFonte <> "" AndAlso idFonte <> 0) Then fonte = Anagrafica.Fonti.GetItemById(tipoFonte, tipoFonte, idFonte)

            Dim xlsConn As CXlsDBConnection

            If (Me.nomeLista = "") Then Me.nomeLista = "Lista FinMaker"
            Me.lista = ListeRicontatto.GetItemByName(nomeLista)
            If (Me.lista Is Nothing) Then
                Me.lista = New CListaRicontatti
                Me.lista.Name = nomeLista
                Me.lista.Stato = ObjectStatus.OBJECT_VALID
                Me.lista.Save()
            End If

            message = New System.Text.StringBuilder


            If (fileName = vbNullString) Then Throw New ArgumentNullException("fileName")
            xlsConn = New CXlsDBConnection(ApplicationContext.MapPath(fileName))
            xlsConn.OpenDB()

            DMD.Sistema.Events.StopEvents = True
            DBUtils.StopStatistics = True
#If Not Debug Then
            Try
#End If
            If (xlsConn.Tables.ContainsKey("Anagrafe")) Then Me.ImportaAnagrafe(xlsConn)
            If (xlsConn.Tables.ContainsKey("Amminis")) Then Me.ImportaAmministrazioni(xlsConn)
            If (xlsConn.Tables.ContainsKey("Finanziarie")) Then Me.ImportaFinanziarie(xlsConn)
            If (xlsConn.Tables.ContainsKey("Teledata")) Then Me.ImportaTelefonate(xlsConn)
            If (xlsConn.Tables.ContainsKey("Pratiche")) Then Me.ImportaPratiche(xlsConn)
#If Not Debug Then
            Catch ex As Exception
                Events.NotifyUnhandledException(ex)
                Throw
            Finally
#End If
            DBUtils.StopStatistics = False
            DMD.Sistema.Events.StopEvents = False
            xlsConn.Dispose()
#If Not Debug Then
            End Try
#End If

            Return message.ToString
        End Function

        Public Overrides Function ImportList(ByVal renderer As Object) As String
            If Not Me.Module.UserCanDoAction("import") Then Throw New PermissionDeniedException(Me.Module, "import")
            WebSite.ASP_Server.ScriptTimeout = 120000000
            Dim nomeLista As String = Trim(RPC.n2str(GetParameter(renderer, "lr", "")))
            Dim fileName As String = Trim(RPC.n2str(GetParameter(renderer, "fp", "")))
            Dim idPO As Integer = RPC.n2int(GetParameter(renderer, "po", ""))
            Dim po As CUfficio = Anagrafica.Uffici.GetItemById(idPO)
            tipoFonte = Trim(RPC.n2str(GetParameter(renderer, "tf", "")))
            Dim idFonte As Nullable(Of Integer) = RPC.n2int(GetParameter(renderer, "if", 0))

            Dim ret As String = Me.ImportList(fileName, po, nomeLista, tipoFonte, idFonte)

            message.Length = 0
            Me.Module.DispatchEvent(New EventDescription("list_imported", "Lista importata dal file " & fileName, ret))

            Return ret
        End Function

        Private Sub ImportaFinanziarie(ByVal xlsConn As CXlsDBConnection)
            Me.Log("Inizio l'importazione del foglio [Finanziarie]")
            Dim xlsTable As CDBTable = xlsConn.Tables("Finanziarie")
            Dim xlsRis As New DBReader(xlsTable)
            While xlsRis.Read
                Dim AC_CAP As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("AC_CAP", "")))
                Dim AC_INDIRIZZO_1 As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("AC_INDIRIZZO_1", "")))
                Dim AC_LOCALITA As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("AC_LOCALITA", "")))
                Dim AC_PROVINCIA As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("AC_PROVINCIA", "")))
                Dim QFIN_BANCA_ABI As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("QFIN_BANCA_ABI", "")))
                Dim QFIN_BANCA_CAB As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("QFIN_BANCA_CAB", "")))
                Dim QFIN_CODICE As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("QFIN_CODICE", "")))
                Dim QFIN_DENOMINAZIONE As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("QFIN_DENOMINAZIONE", "")))

                If (QFIN_DENOMINAZIONE = "") Then Continue While

                Dim azienda As CAzienda
                azienda = Anagrafica.Aziende.GetItemByName(QFIN_DENOMINAZIONE)
                If (azienda Is Nothing) Then
                    Me.Log("Creo l'amministrazione [" & QFIN_DENOMINAZIONE & "]")
                    azienda = New CAzienda
                    azienda.RagioneSociale = QFIN_DENOMINAZIONE
                End If
                If (azienda.ResidenteA.ToponimoViaECivico = "") Then azienda.ResidenteA.ToponimoViaECivico = AC_INDIRIZZO_1
                If (azienda.ResidenteA.CAP = "") Then azienda.ResidenteA.CAP = AC_CAP
                If (azienda.ResidenteA.Provincia = "") Then azienda.ResidenteA.Provincia = AC_PROVINCIA
                If (azienda.ResidenteA.Citta = "") Then azienda.ResidenteA.Provincia = AC_LOCALITA

                azienda.Stato = ObjectStatus.OBJECT_VALID
                azienda.Save()

                AddAnnotazione("ImportaFinMaker", QFIN_DENOMINAZIONE & "_Note", azienda, xlsRis, Nothing)
            End While
            xlsRis.Dispose()

            Me.Log("Finisco l'importazione del foglio [Finanziarie]")
            Me.Log("------------------------------------------")
        End Sub

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
            items = DMD.CQSPD.Pratiche.GetPraticheByPersona(cliente)
            'Aggiungo eventuali pratiche con lo stesso codice fiscale
            If (cliente.CodiceFiscale <> "") Then
                Dim cursor As New CRapportiniCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.CodiceFiscale.Value = cliente.CodiceFiscale
                cursor.IDCliente.Value = GetID(cliente)
                cursor.IDCliente.Operator = OP.OP_NE
                While Not cursor.EOF
                    items.Add(cursor.Item)
                    Me.Log("La pratica " & cursor.Item.NumeroPratica & " è associabile al cliente " & cliente.Nominativo & " tramite il CF (" & cliente.CodiceFiscale & ") ma appartiene ad un oggetto diverso" & vbNewLine)
                    cursor.MoveNext()
                End While
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End If

            For Each item As CRapportino In items
                If (item.StatoAttuale.MacroStato.HasValue AndAlso (item.StatoAttuale.MacroStato = StatoPraticaEnum.STATO_LIQUIDATA OrElse item.StatoAttuale.MacroStato = StatoPraticaEnum.STATO_ARCHIVIATA)) Then
                    If (item.IDProdotto = GetID(prodotto)) Then
                        okProdotto = True
                    ElseIf (prodotto IsNot Nothing) AndAlso ((item.NomeProdotto = prodotto.Nome) AndAlso (item.NomeCessionario = prodotto.NomeCessionario)) Then
                        okProdotto = True
                    ElseIf (prodotto Is Nothing) Then
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

        Private Function RemoveInvalidCharsFromName(ByVal value As String) As String
            Dim ret As String = Replace(value, vbCr, " ")
            ret = Replace(ret, vbLf, " ")
            ret = Replace(ret, vbNullChar, " ")
            Return ret
        End Function

        Private Sub ImportaAmministrazioni(ByVal xlsConn As CXlsDBConnection)
            Me.Log("Inizio l'importazione del foglio [Amminis]")
            Dim xlsTable As CDBTable = xlsConn.Tables("Amminis")
            Dim xlsRis As New DBReader(xlsTable)
            While xlsRis.Read
                Dim Residcitta As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Residcitta", "")))
                Dim Amministrazione As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Amministrazione", "")))
                Dim ResidVia As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("ResidVia", "")))
                Dim ResidCAP As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("ResidCAP", "")))
                Dim Residprov As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Residprov", "")))
                Dim Residntel As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Residntel", "")))
                Dim ntelrespons As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("n°tel  respons", "")))
                Dim Residnfax As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Residnfax", "")))
                Dim Datainserim As Nullable(Of Date) = Formats.ParseDate(xlsRis.GetValue("Data inserim"), True)
                Dim IDamministrazione As Nullable(Of Integer) = Formats.ParseInteger(xlsRis.GetValue("IDamministrazione"), True)
                Dim RelazionePratiche As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Relazione Pratiche", "")))
                Dim Note As String = xlsRis.GetValue("Note", "")
                Dim codfisc As String = Formats.ParseCodiceFiscale(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("cod fisc", "")))
                Dim Respons As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Respons", "")))
                Dim Segnalatorepraticaincorso As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Segnalatore pratica in corso", "")))
                Dim Namministr As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("N° amministr", "")))
                Dim CapitaleSoc As Nullable(Of Decimal) = Formats.ParseValuta(xlsRis.GetValue("Capitale soc"), True)
                Dim Ndipendenti As Nullable(Of Integer) = Formats.ParseInteger(xlsRis.GetValue("N° dipendenti"), True)

                Dim azienda As CAzienda = Nothing
                If (codfisc <> "") Then azienda = Anagrafica.Aziende.GetItemByCF(codfisc)
                If (azienda Is Nothing) Then azienda = Anagrafica.Aziende.GetItemByName(Amministrazione)
                If (azienda Is Nothing AndAlso Amministrazione <> "") Then
                    Me.Log("Creo l'amministrazione [" & Amministrazione & "], C.F.: " & codfisc)
                    azienda = New CAzienda
                    azienda.RagioneSociale = Amministrazione
                End If
                If (azienda Is Nothing) Then Continue While
                If (azienda.CodiceFiscale = "") Then azienda.CodiceFiscale = codfisc
                If (azienda.ResidenteA.ToponimoViaECivico = "") Then azienda.ResidenteA.ToponimoViaECivico = ResidVia
                If (azienda.ResidenteA.CAP = "") Then azienda.ResidenteA.CAP = ResidCAP
                If (azienda.ResidenteA.Provincia = "") Then azienda.ResidenteA.Provincia = Residprov
                If (Residntel <> "") Then azienda.Recapiti.Add("Telefono", Residntel, Residntel)
                If (ntelrespons <> "") Then azienda.Recapiti.Add("Telefono", "Telefono Responsabile (" & Respons & ")", ntelrespons)
                If (Residnfax <> "") Then azienda.Recapiti.Add("Fax", Residnfax, "Fax")
                If (azienda.CapitaleSociale = 0 AndAlso CapitaleSoc.HasValue) Then azienda.CapitaleSociale = CapitaleSoc
                If (azienda.NumeroDipendenti = 0 AndAlso Ndipendenti.HasValue) Then azienda.NumeroDipendenti = Ndipendenti
                If (azienda.Fonte Is Nothing AndAlso Me.fonte IsNot Nothing) Then
                    azienda.TipoFonte = Me.tipoFonte
                    azienda.Fonte = Me.fonte
                End If
                azienda.Stato = ObjectStatus.OBJECT_VALID
                azienda.Save()

                AddAnnotazione("ImportaFinMaker", Amministrazione & IDamministrazione & "Note", azienda, xlsRis, Nothing)
                If (Note <> "") Then AddAnnotazione("ImportaFinMaker", Amministrazione & IDamministrazione, azienda, Note, Nothing)
            End While
            xlsRis.Dispose()

            Me.Log("Finisco l'importazione del foglio [Amminis]")
            Me.Log("------------------------------------------")
        End Sub

        Private Sub Log(ByVal message As String)
            Me.message.Append(Formats.FormatUserDateTime(Now) & " - " & message & vbNewLine)
            Me.OnMessageLogged(message)
        End Sub

        Private Sub ImportaAnagrafe(xlsConn As CXlsDBConnection)
            Dim ricontattare As Boolean

            Me.Log("Inizio l'importazione del foglio [Anagrafe]")
            Dim xlsTable As CDBTable = xlsConn.Tables("Anagrafe")
            Dim xlsRis As New DBReader(xlsTable)
            While xlsRis.Read
                ricontattare = False
                Dim key As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("key", "")))
                Dim D01A_TipologiaOperatoreRegistrante As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D01A_TipologiaOperatoreRegistrante", "")))
                Dim D01B_CodiceOperatore As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D01B_CodiceOperatore", "")))
                Dim D03_TipoDiIdentificazione As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D03_TipoDiIdentificazione", "")))
                Dim D09_CodiceIdentificativoCliente As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D09_CodiceIdentificativoCliente", "")))
                Dim UserD09_CodiceIdentificativoCliente As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserD09_CodiceIdentificativoCliente", "")))
                Dim User2D09_CodiceIdentificativoCliente As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("User2D09_CodiceIdentificativoCliente", "")))
                Dim User3D09_CodiceIdentificativoCliente As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("User3D09_CodiceIdentificativoCliente", "")))
                Dim User4D09_CodiceIdentificativoCliente As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("User4D09_CodiceIdentificativoCliente", "")))
                Dim D10_DataDiIdentificazione As Nullable(Of Date) = Formats.ParseDate(xlsRis.GetValue("D10_DataDiIdentificazione"), True)
                Dim D11_CognomeNome_RagioneSocialeA As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D11_CognomeNome_RagioneSocialeA", "")))
                Dim D11_CognomeNome_RagioneSocialeB As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D11_CognomeNome_RagioneSocialeB", "")))
                Dim D13_PaeseEsteroDiResidenza As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D13_PaeseEsteroDiResidenza", "")))
                Dim D14_ComuneDiResidenzaAnagrafica As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D14_ComuneDiResidenzaAnagrafica", "")))
                Dim D14_B_ComuneDiResidenzaAnagrafica_DescrizioneInChiaro As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D14.B_ComuneDiResidenzaAnagrafica_DescrizioneInChiaro", "")))
                Dim D14_C_ComuneDiResidenzaAnagrafica_Provincia As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D14.C_ComuneDiResidenzaAnagrafica_Provincia", "")))
                Dim D16_Domicilio_Sede_CapDiResidenza As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D16_Domicilio_Sede_CapDiResidenza", "")))
                Dim D17_CodiceFiscale As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D17_CodiceFiscale", "")))
                Dim D18_DataDiNascita As Nullable(Of Date) = Formats.ParseDate(xlsRis.GetValue("D18_DataDiNascita"), True)
                Dim D19_ComuneDiNascita As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D19_ComuneDiNascita", "")))
                Dim D19_B_ProvinciaDiNascita As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D19.B_ProvinciaDiNascita", "")))
                Dim D19_C_StatoDiNascita As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D19.C_StatoDiNascita", "")))
                Dim D41_TipoDocumentoPresentato As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D41_TipoDocumentoPresentato", "")))
                Dim D42_NumeroDocumentoPresentato As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D42_NumeroDocumentoPresentato", "")))
                Dim D43_DataRilascioDocumentoPresentato As Nullable(Of Date) = Formats.ParseDate(xlsRis.GetValue("D43_DataRilascioDocumentoPresentato"), True)
                Dim D44_AutoritaeLocalitaDiRilascioDocumentoPresentato As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D44_AutoritaeLocalitaDiRilascioDocumentoPresentato", "")))
                Dim D45_Sesso As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D45_Sesso", "")))
                Dim D54_StatoDellaAnagrafica As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D54_StatoDellaAnagrafica", "")))
                Dim D54_A_StatoDellaAnagraficaCodiceStato As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D54.A_StatoDellaAnagraficaCodiceStato", "")))
                Dim D54_B_StatoDellaAnagraficaDataDellaRettifica As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D54.B_StatoDellaAnagraficaDataDellaRettifica", "")))
                Dim UserKeyCI As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeyCI", "")))
                Dim UserKeySR As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeySR", "")))
                Dim UserKeyTR As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeyTR", "")))
                Dim UserKeyTI As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeyTI", "")))
                Dim UserKeyVE As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeyVE", "")))
                Dim UserKeySS As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeySS", "")))
                Dim UserKeyNG As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeyNG", "")))
                Dim UserKeyTL As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeyTL", "")))
                Dim UserKeyPC As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeyPC", "")))
                Dim UserKeyXX As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeyXX", "")))
                Dim UserKeyTP As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeyTP", "")))
                Dim UserKeyPR As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserKeyPR", "")))
                Dim UserRelNG As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserRelNG", "")))
                Dim UserRelTI As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserRelTI", "")))
                Dim UserRelCI As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserRelCI", "")))
                Dim UserRelSR As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserRelSR", "")))
                Dim UserRelSS As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserRelSS", "")))
                Dim UserEtiMR As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserEtiMR", "")))
                Dim StatoRecordUser As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("StatoRecordUser", "")))
                Dim StatoRecordCalc As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("StatoRecordCalc", "")))
                Dim UserRelComuniEE As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserRelComuniEE", "")))
                Dim relazionefissaa As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("relazionefissaa", "")))
                Dim Eticmod As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Eticmod", "")))
                Dim DataArchiviazione As Nullable(Of Date) = Formats.ParseDate(xlsRis.GetValue("DataArchiviazione"), True)
                Dim UserZeri As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserZeri", "")))
                Dim DescrizioneSoggettoPerElenco As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("DescrizioneSoggettoPerElenco", "")))
                Dim UserNuovoPer As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserNuovoPer", "")))
                Dim UserRicercaPerAttivita As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserRicercaPerAttivita", "")))
                Dim UserSceltaSoggetto As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("UserSceltaSoggetto", "")))
                Dim D09_CodiceIdentificativoClientePerRelazione As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D09_CodiceIdentificativoClientePerRelazione", "")))
                Dim D45_SessoInchiaro As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("D45_SessoInchiaro", "")))
                Dim PartitaIva As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("PartitaIva", "")))
                Dim ComuneoStatoEsterodiNascita As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("ComuneoStatoEsterodiNascita", "")))
                Dim ComuneoStatoEsterodiResidenza_Copia As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("ComuneoStatoEsterodiResidenza Copia", "")))
                Dim KeyPerRelazione As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("KeyPerRelazione", "")))

                If (D11_CognomeNome_RagioneSocialeA = "" AndAlso D11_CognomeNome_RagioneSocialeB = "") Then Continue While

                'Elaboro la telefonata
                Dim persona As CPersona
                persona = Me.FindPersonaByCF(D17_CodiceFiscale)
                If (persona Is Nothing) Then
                    Me.Log("Creo l'anagrafica della persona [" & Strings.ToNameCase(D11_CognomeNome_RagioneSocialeB) & " " & Strings.UCase(D11_CognomeNome_RagioneSocialeA))
                    'Elaboro 
                    persona = New CPersonaFisica
                    With DirectCast(persona, CPersonaFisica)
                        .Cognome = D11_CognomeNome_RagioneSocialeA
                        .Nome = D11_CognomeNome_RagioneSocialeB
                        ricontattare = True
                        .Save()
                    End With
                End If

                If (persona.ResidenteA.Citta = "") Then persona.ResidenteA.Citta = D14_ComuneDiResidenzaAnagrafica
                If (persona.ResidenteA.Provincia = "") Then persona.ResidenteA.Citta = D14_C_ComuneDiResidenzaAnagrafica_Provincia
                If (persona.ResidenteA.ToponimoViaECivico = "") Then persona.ResidenteA.ToponimoViaECivico = D14_B_ComuneDiResidenzaAnagrafica_DescrizioneInChiaro
                If (persona.ResidenteA.CAP = "") Then persona.ResidenteA.CAP = D16_Domicilio_Sede_CapDiResidenza
                If (persona.CodiceFiscale = "") Then persona.CodiceFiscale = D17_CodiceFiscale
                If (persona.DataNascita.HasValue = False) Then persona.DataNascita = D18_DataDiNascita
                If (persona.NatoA.Citta = "") Then persona.NatoA.Citta = Trim(D19_C_StatoDiNascita & " &" & D19_ComuneDiNascita)
                If (persona.NatoA.Provincia = "") Then persona.NatoA.Provincia = D19_B_ProvinciaDiNascita
                If (persona.Sesso = "") Then persona.Sesso = D45_SessoInchiaro

                persona.Stato = ObjectStatus.OBJECT_VALID
                persona.Save()

                AddAnnotazione("ImportaFinMaker[Anagrafe]", key, persona, xlsRis)

                'Elaboro il ricontatto
                Dim ricontatto As CRicontatto
                If (ricontattare) Then
                    ricontatto = Ricontatti.GetRicontattoBySource("ImportaFinMaker[Anagrafe]", key)
                    If (ricontatto Is Nothing) Then ricontatto = Ricontatti.ProgrammaRicontatto(persona, Now, "Ricontatto importato da FinMaker", "ImportaFinMaker[Anagrafe]", key, Me.nomeLista, Nothing, Nothing)
                End If


            End While
            xlsRis.Dispose()

            Me.Log("Finisco l'importazione del foglio [Anagrafe]")
            Me.Log("------------------------------------------")
        End Sub

        Private Sub ImportaTelefonate(xlsConn As CXlsDBConnection)
            Dim ricontattare As Boolean

            Me.Log("Inizio l'importazione del foglio [Teledata]")
            Dim xlsTable As CDBTable = xlsConn.Tables("Teledata")
            Dim xlsRis As New DBReader(xlsTable)
            While xlsRis.Read
                ricontattare = False
                Dim Nprot As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("N° PROT", "")))
                Dim Cognome As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Cognome", "")))
                Dim Eta As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Età", "")))
                Dim Nome As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Nome", "")))
                Dim TipoPosto As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Tipo posto", "")))
                Dim Data As Nullable(Of Date) = Formats.ParseDate(xlsRis.GetValue("Data"), True)
                Dim Ora As Nullable(Of Date) = Formats.ParseDate(xlsRis.GetValue("Ora"), True)
                Dim Utente As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Utente", "")))
                Dim Motivo As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Motivo", "")))
                Dim Tipotelefonata As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Tipo telefonata", "")))
                Dim Risposta As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Risposta", "")))
                Dim Contatto As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Contatto", "")))
                Dim Contatto2 As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Contatto 2", "")))
                Dim Contatto3 As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Contatto 3", "")))
                Dim Dovelavora As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Dove lavora", "")))
                Dim Anzianitadiservizio As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Anzianità di servizio", "")))
                Dim Trattenute As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Trattenute", "")))
                Dim Importo As Nullable(Of Decimal) = Formats.ParseValuta(xlsRis.GetValue("Importo"), True)
                Dim Residuo As Nullable(Of Integer) = Formats.ParseValuta(xlsRis.GetValue("Residuo"), True)
                Dim Comeciconosce As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Come ci conosce")))
                Dim Agente2 As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Agente2", "")))
                Dim Note As String = xlsRis.GetValue("Note", "")
                Dim Trattenutecon As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Trattenute con", "")))
                Dim Cercava As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Cercava", "")))
                Dim Visto As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Visto", "")))
                Dim Contatto4 As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Contatto 4", "")))
                Dim Citta As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Città", "")))
                Dim Trattenutecon2 As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Trattenute con 2", "")))
                Dim Importo2 As Nullable(Of Decimal) = Formats.ParseValuta(xlsRis.GetValue("Importo 2"), True)
                Dim Residuo2 As Nullable(Of Integer) = Formats.ParseValuta(xlsRis.GetValue("Residuo 2"), True)
                Dim datanascita As Nullable(Of Date) = Formats.ParseDate(xlsRis.GetValue("datanascita"), True)
                Dim indirizzo As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("indirizzo", "")))
                Dim StipendioMensile As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Stipendio Mensile", "")))
                Dim ImportoRichiesto As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Importo Richiesto", "")))
                Dim Motivazione As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Motivazione", "")))
                Dim InizioScadenzarata As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Inizio Scadenza rata", "")))
                Dim DataAppuntamento As Nullable(Of Date) = Formats.ParseDate(xlsRis.GetValue("Data Appuntamento"), True)
                Dim OraAppuntamento As Nullable(Of Date) = Formats.ParseDate(xlsRis.GetValue("Ora Appuntamento"), True)
                Dim DataRicontatto As Nullable(Of Date) = Formats.ParseDate(xlsRis.GetValue("Data Ricontatto"), True)
                Dim LuogoAppuntamento As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Luogo Appuntamento", "")))
                Dim Esito As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Esito", "")))
                Dim Trattenutecon3 As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Trattenute con 3", "")))
                Dim Importo3 As Nullable(Of Decimal) = Formats.ParseValuta(xlsRis.GetValue("Importo 3"), True)
                Dim Residuo3 As Nullable(Of Integer) = Formats.ParseValuta(xlsRis.GetValue("Residuo 3"), True)
                Dim Trattenutecon4 As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Trattenute con 4", "")))
                Dim Importo4 As Nullable(Of Decimal) = Formats.ParseValuta(xlsRis.GetValue("Importo 4"), True)
                Dim Residuo4 As Nullable(Of Integer) = Formats.ParseValuta(xlsRis.GetValue("Residuo 4"), True)
                Dim CODAZIENDA As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("CODAZIENDA", "")))
                Dim puntooperativo As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("puntooperativo", "")))
                Dim Agente As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Agente", "")))
                Dim operatoreuser As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("operatoreuser", "")))
                If (Nome = "" AndAlso Cognome = "") Then Continue While

                'Elaboro la telefonata
                Dim persona As CPersona
                Dim po As CUfficio
                Dim telefonata As CTelefonata

                telefonata = Me.FindTelefonataByContesto("ImportaFinMaker[Teledata]", Nprot)
                If (telefonata IsNot Nothing) Then
                    Me.Log("Telefonata già importata ID=" & Nprot & " [" & Strings.ToNameCase(Nome) & " " & Strings.UCase(Cognome))
                    Continue While 'Già abbiamo importato questo oggetto
                End If

                po = Me.GetPuntoOperativo(puntooperativo)

                Me.Log("Importo la telefonata ID=" & Nprot & " [" & Strings.ToNameCase(Nome) & " " & Strings.UCase(Cognome))
                If (telefonata Is Nothing) Then
                    persona = Me.FindByNameAndTelefono(Cognome, Nome, datanascita, Contatto, Contatto2, Contatto3, Contatto4)
                    If (persona Is Nothing) Then
                        Me.Log("Creo l'anagrafica della persona [" & Strings.ToNameCase(Nome) & " " & Strings.UCase(Cognome))

                        'Elaboro 
                        persona = New CPersonaFisica
                        With DirectCast(persona, CPersonaFisica)
                            .Cognome = Cognome
                            .Nome = Nome
                            ricontattare = True
                            .Save()
                        End With
                    End If

                    telefonata = New CTelefonata
                    telefonata.Persona = persona
                    telefonata.Data = Calendar.MergeDateTime(Data, Ora)
                    telefonata.Scopo = Motivo
                    telefonata.NomeOperatore = Utente
                    telefonata.PuntoOperativo = po
                    telefonata.Note = Note
                    telefonata.Contesto = "ImportaFinMaker[Teledata]"
                    telefonata.IDContesto = Nprot
                    telefonata.Stato = ObjectStatus.OBJECT_VALID
                    telefonata.Save()

                Else
                    persona = telefonata.Persona
                End If

                Me.AddContatto(persona, Contatto)
                Me.AddContatto(persona, Contatto2)
                Me.AddContatto(persona, Contatto3)
                Me.AddContatto(persona, Contatto4)

                Dim valore As Double
                If (StipendioMensile <> "") Then
                    Try
                        valore = Formats.ParseValuta(StipendioMensile, True)
                        StipendioMensile = valore
                    Catch ex As Exception
                        Try
                            If (InStr(StipendioMensile, "@") > 0) Then
                                persona.Recapiti.Add("e-mail", "e-mail", StipendioMensile)
                            Else
                                persona.Recapiti.Add("Telefono", StipendioMensile, Formats.ParsePhoneNumber(StipendioMensile))
                            End If
                        Catch ex1 As Exception
                            Dim note1 As New CAnnotazione(persona)
                            note1.Valore = "Spazio esaurito per il contatto: " & StipendioMensile
                            note1.Stato = ObjectStatus.OBJECT_VALID
                            note1.Save()
                        End Try
                        StipendioMensile = vbNullString
                    End Try
                End If

                If (persona.PuntoOperativo Is Nothing) Then persona.PuntoOperativo = po

                If (persona.ResidenteA.Citta = "") Then persona.ResidenteA.NomeComune = Citta
                If (persona.ResidenteA.ToponimoViaECivico = "") Then persona.ResidenteA.ToponimoViaECivico = indirizzo
                If (persona.Fonte Is Nothing) Then
                    persona.TipoFonte = "Altro"
                    persona.Fonte = Me.GetFonteAltro(Comeciconosce)
                End If
                persona.Stato = ObjectStatus.OBJECT_VALID
                persona.Save()

                AddAnnotazione("ImportaFinMaker[Teledata]", Nprot, persona, xlsRis)


                'Elaboro la posizione lavorativa
                If (TypeOf (persona) Is CPersonaFisica) Then
                    Dim impiego As CImpiegato
                    With DirectCast(persona, CPersonaFisica)
                        impiego = .ImpiegoPrincipale
                        If impiego.NomeAzienda = "" Then impiego.NomeAzienda = Dovelavora
                        If impiego.Posizione = "" Then impiego.Posizione = TipoPosto
                        If (Not impiego.StipendioNetto.HasValue OrElse impiego.StipendioNetto.Value = 0) Then impiego.StipendioNetto = Formats.ParseValuta(StipendioMensile, True)
                        impiego.Stato = ObjectStatus.OBJECT_VALID
                        impiego.Save()
                    End With
                End If



                'Elaboro il ricontatto
                Dim ricontatto As CRicontatto
                If (ricontattare) Then
                    If (DataRicontatto.HasValue = False) Then DataRicontatto = Now
                    ricontatto = Ricontatti.GetRicontattoBySource("ImportaFinMaker[Teledata]", Nprot)
                    If (ricontatto Is Nothing) Then ricontatto = Ricontatti.ProgrammaRicontatto(persona, DataRicontatto, "Ricontatto importato da FinMaker", "ImportaFinMaker[Teledata]", Nprot, Me.nomeLista, po, Nothing)
                Else
                    If (DataRicontatto.HasValue) Then
                        ricontatto = Anagrafica.Ricontatti.GetProssimoRicontatto(persona)
                        If (ricontatto Is Nothing) Then ricontatto = Ricontatti.ProgrammaRicontatto(persona, DataRicontatto, "Ricontatto importato da FinMaker", "ImportaFinMaker[Teledata]", Nprot, "", po, Nothing)
                    End If
                End If

                'Elaboro l'appuntamento
                'Dim appuntamento As CRicontatto = Appuntamenti.GetAppuntamentoBySource("ImportaFinMaker[Teledata]", Nprot)
                'If (appuntamento Is Nothing AndAlso ((DataAppuntamento.HasValue AndAlso DataAppuntamento.Value < Today))) Then
                '    appuntamento = Appuntamenti.ProgrammaAppuntamento(persona, Calendar.MergeDateTime(DataAppuntamento, OraAppuntamento), "Appuntamento importato da FinMaker", LuogoAppuntamento, "ImportaFinMaker[Teledata]", Nprot, "", po, Nothing)
                'End If

                'Elaboro i finanziamenti in corso
                Dim estinzioni As CEstinzioniCollection = DMD.CQSPD.Estinzioni.GetEstinzioniByPersona(persona)
                If (Data.HasValue = False) Then Data = Now
                Dim e As CEstinzione = Me.FindEstinzione(persona, estinzioni, Trattenutecon, Importo, Residuo, Data)
                e = Me.FindEstinzione(persona, estinzioni, Trattenutecon2, Importo2, Residuo2, Data)
                e = Me.FindEstinzione(persona, estinzioni, Trattenutecon3, Importo3, Residuo3, Data)
                e = Me.FindEstinzione(persona, estinzioni, Trattenutecon4, Importo4, Residuo4, Data)

                'Elabora la richiesta di finanziamento
                If (ImportoRichiesto <> "") Then
                    Dim richiesta As CRichiestaFinanziamento = Me.FindRichiesta(persona, Data, ImportoRichiesto)
                End If

            End While
            xlsRis.Dispose()

            Me.Log("Finisco l'importazione del foglio [Teledata]")
            Me.Log("------------------------------------------")
        End Sub

        Private Function FindRichiesta(ByVal persona As CPersona, ByVal data As Date, ByVal importoRichiesto As String) As CRichiestaFinanziamento
            Dim cursor As New CRichiesteFinanziamentoCursor
            cursor.Data.Value = data
            cursor.IDCliente.Value = GetID(persona)
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            Dim ret As CRichiestaFinanziamento = cursor.Item
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            Dim valore As Nullable(Of Decimal)
            Try
                valore = Formats.ParseValuta(importoRichiesto, True)
            Catch ex As Exception
                valore = Nothing
            End Try
            If (ret IsNot Nothing) Then
                If (valore.HasValue) Then
                    If (ret.ImportoRichiesto = valore) Then Return ret
                Else
                    If (ret.Note = importoRichiesto) Then Return ret
                End If
            End If
            ret = New CRichiestaFinanziamento
            ret.Cliente = persona
            ret.Data = data
            ret.ImportoRichiesto = valore
            ret.Note = importoRichiesto
            ret.Stato = ObjectStatus.OBJECT_VALID
            ret.StatoRichiesta = StatoRichiestaFinanziamento.EVASA
            'ret.NomePresaInCaricoDa = 
            ret.Save()
            Return ret
        End Function

        Private Function FindEstinzione(ByVal persona As CPersona, ByVal items As CCollection(Of CEstinzione), ByVal con As String, ByVal rata As Nullable(Of Decimal), ByVal residue As Nullable(Of Integer), ByVal dataTelefonata As Date) As CEstinzione
            Dim e As CEstinzione
            con = LCase(Trim(con))
            If (con = "" AndAlso rata.HasValue = False AndAlso residue.HasValue = False) Then Return Nothing

            For Each e In items
                If (LCase(e.NomeIstituto) = con AndAlso rata.HasValue AndAlso e.Rata.HasValue AndAlso rata.Value = e.Rata) Then
                    Return e
                End If
            Next

            e = New CEstinzione
            e.Persona = persona
            e.NomeIstituto = con
            e.Rata = rata
            If (residue.HasValue) Then e.NumeroRateResidue = residue
            e.Stato = ObjectStatus.OBJECT_VALID
            e.Save()
            Return e
        End Function

        Private Function FindTelefonataByPersonaAndContesto(ByVal persona As CPersona, ByVal contesto As String, ByVal idContesto As Integer) As CTelefonata
            Dim cursor As New CTelefonateCursor
            cursor.IDPersona.Value = GetID(persona)
            cursor.Contesto.Value = contesto
            cursor.IDContesto.Value = idContesto
            Dim ret As CTelefonata = cursor.Item
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return ret
        End Function

        Private Function FindTelefonataByContesto(ByVal contesto As String, ByVal idContesto As Integer) As CTelefonata
            Dim cursor As New CTelefonateCursor
            cursor.Contesto.Value = contesto
            cursor.IDContesto.Value = idContesto
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            Dim ret As CTelefonata = cursor.Item
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            Return ret
        End Function

        Private Sub ImportaPratiche(xlsConn As CXlsDBConnection)
            Dim ricontattare As Boolean

            Me.Log("Inizio l'importazione del foglio [Pratiche]")
            Dim xlsTable As CDBTable = xlsConn.Tables("Pratiche")
            Dim xlsRis As New DBReader(xlsTable)
            While xlsRis.Read
                ricontattare = False
                Dim Datanascitadeb As Nullable(Of Date) = Formats.ParseDate(xlsRis.GetValue("Datanascitadeb"), True)
                Dim Nomedeb As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Nomedeb", "")))
                Dim ResidCAPdeb As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("ResidCAPdeb", "")))
                Dim ResidViadeb As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("ResidViadeb", "")))
                Dim Cognomedeb As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Cognomedeb", "")))
                Dim Residcittadeb As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Residcittadeb", "")))
                Dim Finanziaria As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Finanziaria", "")))
                Dim Nprot As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("NProt", "")))
                Dim luogonascitadeb As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("luogonascitadeb", "")))
                Dim Qualificadeb As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Qualificadeb", "")))
                Dim provnascdeb As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("provnascdeb", "")))
                Dim provdeb As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("provdeb", "")))
                Dim Codicefiscaledebitore As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Codicefiscaledebitore", "")))
                Dim Amministnom As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Amministnom", "")))
                Dim Tiporapportoesteso As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Tiporapportoesteso", "")))
                Dim Utente As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Utente", "")))
                Dim NPrev As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("NPrev", "")))
                Dim Sesso As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Sesso", "")))
                Dim Postit As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Postit", "")))
                Dim Telefonatericevute As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Telefonate ricevute", "")))
                Dim Telefonatericevute2 As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Telefonate ricevute 2", "")))
                Dim Telefonatericevute3 As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Telefonate ricevute 3", "")))
                Dim Telefonatericevute4 As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Telefonate ricevute 4", "")))
                Dim Telefonatericevute5 As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Telefonate ricevute 5", "")))
                Dim Nota As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Nota", "")))
                Dim Notepratica As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Note pratica", "")))
                Dim AnnotazioniAmministratore As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Annotazioni Amministratore", "")))
                Dim email As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("email", "")))
                Dim codiceprodotto As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("codice prodotto", "")))
                Dim Telufficiodeb As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Tel ufficio deb", "")))
                Dim Residcittàdebperspedizione As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Resid città deb per spedizione", "")))
                Dim ResidViadebperspedizione As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Resid Via deb per spedizione", "")))
                Dim Residprovinciadebperspedizione As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Resid provincia deb per spedizione", "")))
                Dim Residcapdebperspedizione As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Resid cap deb per spedizione", "")))
                Dim ResidNominativoperspedizione As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Resid Nominativo per spedizione", "")))
                Dim Pubblicita As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Pubblicita", "")))
                Dim Operatore As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Operatore", "")))
                Dim Operatorediannullopratica As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Operatore di annullo pratica", "")))
                Dim ImportoRata As Nullable(Of Decimal) = Formats.ParseValuta(xlsRis.GetValue("ImportoRata"), True)
                Dim NRate As Nullable(Of Integer) = Formats.ParseInteger(xlsRis.GetValue("NRate"), True)
                Dim Residtelefonodeb As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Residtelefonodeb", "")))
                Dim Residcellularedeb As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Residcellularedeb", "")))
                Dim Nettoerogatouser As Nullable(Of Decimal) = Formats.ParseValuta(xlsRis.GetValue("Netto erogato user"), True)
                Dim Stipendionetto As Nullable(Of Decimal) = Formats.ParseValuta(xlsRis.GetValue("Stipendio netto"), True)
                Dim puntooperativo As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("puntooperativo", "")))
                Dim Decorrenza As Nullable(Of Date) = Formats.ParseDate(xlsRis.GetValue("Decorrenza"), True)
                Dim Dataassunzionedeb As Nullable(Of Date) = Formats.ParseDate(xlsRis.GetValue("Dataassunzionedeb"), True)
                Dim Dataliquidazione As Nullable(Of Date) = Formats.ParseDate(xlsRis.GetValue("Data liquidazione"), True)
                Dim Dataannullo As Nullable(Of Date) = Formats.ParseDate(xlsRis.GetValue("Data annullo"), True)
                Dim Datapresentazionebanca As Nullable(Of Date) = Formats.ParseDate(xlsRis.GetValue("Data presentazione banca"), True)
                Dim DataPrev As Nullable(Of Date) = Formats.ParseDate(xlsRis.GetValue("Data Prev."), True)
                Dim po As CUfficio = Me.GetPuntoOperativo(puntooperativo)
                Dim dataRicontatto As Nullable(Of Date) = Nothing

                If (Nomedeb = "" AndAlso Cognomedeb = "" AndAlso Codicefiscaledebitore = "") Then
                    Continue While
                End If

                Dim pratica As CRapportino
                pratica = DMD.CQSPD.Pratiche.GetItemByNumeroEsterno(Nothing, "FinMaker(" & Me.NomePO & ", " & Nprot & ")")
                If (pratica IsNot Nothing) Then
                    Me.Log("Pratica già importata ID=" & Nprot & " [" & Strings.ToNameCase(Nomedeb) & " " & Strings.UCase(Cognomedeb))
                    Continue While 'Già abbiamo importato questa pratica
                End If

                Dim persona As CPersona
                persona = Me.FindPersonaByCF(Codicefiscaledebitore)
                If (persona Is Nothing) Then persona = Me.FindByNameAndTelefono(Cognomedeb, Nomedeb, Nothing, Telufficiodeb, Residtelefonodeb, Residcellularedeb, email)

                Me.Log("Importo la pratica ID=" & Nprot & " [" & Strings.ToNameCase(Nomedeb) & " " & Strings.UCase(Cognomedeb))
                If (persona Is Nothing) Then
                    Me.Log("Creo l'anagrafica della persona [" & Strings.ToNameCase(Nomedeb) & " " & Strings.UCase(Cognomedeb))

                    'Elaboro 
                    persona = New CPersonaFisica
                    With DirectCast(persona, CPersonaFisica)
                        .Cognome = Cognomedeb
                        .Nome = Nomedeb
                        ricontattare = True
                        dataRicontatto = Now
                    End With
                End If

                Me.AddContatto(persona, Telufficiodeb, "Telefono Ufficio")
                Me.AddContatto(persona, Residtelefonodeb, "Telefono Casa")
                Me.AddContatto(persona, Residcellularedeb, "Cellulare")
                Me.AddContatto(persona, email, "e-mail")

                If (persona.CodiceFiscale = "") Then persona.CodiceFiscale = Codicefiscaledebitore
                If (persona.CodiceFiscale <> "") Then
                    Dim cfCalc As New CFCalculator
                    Try
                        cfCalc.CodiceFiscale = persona.CodiceFiscale
                        cfCalc.Inverti()
                        If (persona.DataNascita.HasValue = False) Then persona.DataNascita = cfCalc.NatoIl
                        If (persona.NatoA.Citta = "") Then persona.NatoA.NomeComune = Anagrafica.Luoghi.MergeComuneeProvincia(cfCalc.NatoAComune, cfCalc.NatoAProvincia)
                        If (persona.Sesso = "") Then persona.Sesso = cfCalc.Sesso
                    Catch ex As Exception

                    End Try
                End If
                If (persona.NatoA.Citta = "") Then
                    persona.NatoA.Citta = luogonascitadeb
                    persona.NatoA.Provincia = provnascdeb
                End If
                If (persona.Sesso = "") Then persona.Sesso = Sesso
                If (persona.DataNascita.HasValue = False) Then persona.DataNascita = Datanascitadeb
                If (persona.ResidenteA.CAP = "") Then persona.ResidenteA.CAP = ResidCAPdeb
                If (persona.ResidenteA.Citta = "") Then persona.ResidenteA.Citta = Residcittadeb
                If (persona.ResidenteA.Provincia = "") Then persona.ResidenteA.Citta = provdeb
                If (persona.ResidenteA.ToponimoViaECivico = "") Then persona.ResidenteA.ToponimoViaECivico = ResidViadeb

                If (persona.DomiciliatoA.Citta = "") Then persona.DomiciliatoA.Citta = Residcittàdebperspedizione
                If (persona.DomiciliatoA.ToponimoViaECivico = "") Then persona.DomiciliatoA.ToponimoViaECivico = ResidViadebperspedizione
                If (persona.DomiciliatoA.Provincia = "") Then persona.DomiciliatoA.Provincia = Residprovinciadebperspedizione
                If (persona.DomiciliatoA.CAP = "") Then persona.DomiciliatoA.CAP = Residcapdebperspedizione
                If (ResidNominativoperspedizione <> "") Then persona.DomiciliatoA.Nome = ResidNominativoperspedizione 'persona Dim ResidNominativoperspedizione As String = Trim(Me.RemoveInvalidCharsFromName(xlsRis.GetValue("Resid Nominativo per spedizione", "")))


                If (persona.PuntoOperativo Is Nothing) Then persona.PuntoOperativo = po
                If (persona.Fonte Is Nothing) Then
                    persona.TipoFonte = "Altro"
                    persona.Fonte = Me.GetFonteAltro(Pubblicita)
                End If


                persona.Stato = ObjectStatus.OBJECT_VALID
                persona.Save()


                'Elaboro la posizione lavorativa
                If (TypeOf (persona) Is CPersonaFisica) Then
                    Dim impiego As CImpiegato
                    With DirectCast(persona, CPersonaFisica)
                        impiego = .ImpiegoPrincipale
                        If impiego.NomeAzienda = "" Then impiego.NomeAzienda = Amministnom
                        If impiego.Posizione = "" Then impiego.Posizione = Qualificadeb
                        If impiego.TipoRapporto = "" Then impiego.TipoRapporto = Tiporapportoesteso
                        If (Not impiego.StipendioNetto.HasValue OrElse impiego.StipendioNetto.Value = 0) Then impiego.StipendioNetto = Formats.ParseValuta(Stipendionetto, True)
                        impiego.Stato = ObjectStatus.OBJECT_VALID
                        impiego.Save()
                    End With
                End If

                Me.NomePO = puntooperativo

                'Elabora la pratica
                Dim montante As Nullable(Of Decimal) = Nothing
                If (ImportoRata.HasValue AndAlso NRate.HasValue) Then montante = ImportoRata.Value * NRate.Value
                If (pratica Is Nothing) Then pratica = Me.CercaPraticaVicina(persona, montante, Decorrenza, Nothing)
                If (pratica Is Nothing) Then
                    Me.Log("Creo la pratica esterna N°" & Nprot)
                    pratica = New CRapportino
                    pratica.Cliente = persona
                    pratica.FromCliente()
                End If
                If (pratica.NumeroEsterno = "") Then pratica.NumeroEsterno = "FinMaker(" & Me.NomePO & ", " & Nprot & ")"
                If (pratica.DataDecorrenza.HasValue = False) Then pratica.DataDecorrenza = Decorrenza
                If (pratica.NumeroRate.HasValue = False) Then pratica.NumeroRate = NRate
                If (pratica.Rata.HasValue = False) Then pratica.Rata = ImportoRata
                If (pratica.NomeProdotto = "") Then pratica.NomeProdotto = codiceprodotto
                If (pratica.NettoRicavo = 0 AndAlso Nettoerogatouser.HasValue) Then pratica.NettoRicavo = Nettoerogatouser
                If (pratica.NomeCessionario = "") Then pratica.NomeCessionario = Finanziaria
                'To DO
#If 0 Then
                If (DataPrev.HasValue) Then
                    pratica.StatoContatto.Data = DataPrev
                    If (pratica.StatoPratica < StatoPraticaEnum.STATO_CONTATTO) Then pratica.StatoPratica = StatoPraticaEnum.STATO_CONTATTO
                End If
                If (Datapresentazionebanca.HasValue) Then
                    If (pratica.StatoRichiestaDelibera.Data.HasValue = False) Then pratica.StatoRichiestaDelibera.Data = Datapresentazionebanca
                    If (pratica.StatoPratica < StatoPraticaEnum.STATO_RICHIESTADELIBERA) Then pratica.StatoPratica = StatoPraticaEnum.STATO_RICHIESTADELIBERA
                End If
                If (Dataliquidazione.HasValue) Then
                    If (pratica.StatoLiquidata.Data.HasValue = False) Then pratica.StatoLiquidata.Data = Dataliquidazione
                    If (pratica.StatoPratica < StatoPraticaEnum.STATO_LIQUIDATA) Then pratica.StatoPratica = StatoPraticaEnum.STATO_LIQUIDATA
                End If
                If (Dataannullo.HasValue) Then
                    If (pratica.StatoAnnullata.Data.HasValue = False) Then pratica.StatoAnnullata.Data = Dataannullo
                    pratica.StatoPratica = StatoPraticaEnum.STATO_ANNULLATA
                End If
#End If

                If (pratica.StatoContatto.NomeOperatore = Users.CurrentUser.Nominativo) Then pratica.StatoContatto.NomeOperatore = Operatore
                If (pratica.StatoRichiestaDelibera.NomeOperatore = "") Then pratica.StatoRichiestaDelibera.NomeOperatore = Operatore
                If (pratica.StatoLiquidata.NomeOperatore = "") Then pratica.StatoLiquidata.NomeOperatore = Operatore
                If (pratica.StatoAnnullata.NomeOperatore = "") Then pratica.StatoAnnullata.NomeOperatore = Operatorediannullopratica

                pratica.Stato = ObjectStatus.OBJECT_VALID
                pratica.Save()




                If (Postit <> "") Then AddAnnotazione("ImportaFinMaker[Pratiche]", Nprot & "(PostIt)", persona, "Postit: " & Postit, pratica)
                If (Telefonatericevute <> "") Then AddAnnotazione("ImportaFinMaker[Pratiche]", Nprot & "(Telefonatericevute)", persona, "Telefonatericevute: " & Telefonatericevute, pratica)
                If (Telefonatericevute2 <> "") Then AddAnnotazione("ImportaFinMaker[Pratiche]", Nprot & "(Telefonatericevute2)", persona, "Telefonatericevute2: " & Telefonatericevute2, pratica)
                If (Telefonatericevute3 <> "") Then AddAnnotazione("ImportaFinMaker[Pratiche]", Nprot & "(Telefonatericevute3)", persona, "Telefonatericevute3: " & Telefonatericevute3, pratica)
                If (Telefonatericevute4 <> "") Then AddAnnotazione("ImportaFinMaker[Pratiche]", Nprot & "(Telefonatericevute4)", persona, "Telefonatericevute4: " & Telefonatericevute4, pratica)
                If (Telefonatericevute5 <> "") Then AddAnnotazione("ImportaFinMaker[Pratiche]", Nprot & "(Telefonatericevute5)", persona, "Telefonatericevute5: " & Telefonatericevute5, pratica)
                If (Nota <> "") Then AddAnnotazione("ImportaFinMaker[Pratiche]", Nprot & "(Nota)", persona, "Nota: " & Nota, pratica)
                If (Notepratica <> "") Then AddAnnotazione("ImportaFinMaker[Pratiche]", Nprot & "(Notepratica)", persona, "Notepratica: " & Notepratica, pratica)
                If (AnnotazioniAmministratore <> "") Then AddAnnotazione("ImportaFinMaker[Pratiche]", Nprot & "(AnnotazioniAmministratore)", persona, "AnnotazioniAmministratore: " & AnnotazioniAmministratore, pratica)



                AddAnnotazione("ImportaFinMaker[Pratiche]", Nprot, persona, xlsRis, pratica)

                'Elaboro il ricontatto
                If (ricontattare) Then
                    Dim ricontatto As CRicontatto = Ricontatti.GetRicontattoBySource("ImportaFinMaker[Pratiche]", Nprot)
                    'If (ricontatto Is Nothing) Then ricontatto = Ricontatti.GetRicontattoBySource(pratica)
                    If (ricontatto Is Nothing) Then ricontatto = Ricontatti.ProgrammaRicontatto(persona, Now, "Ricontatto importato da FinMaker", "ImportaFinMaker[Pratiche]", Nprot, Me.nomeLista, po, Nothing)
                End If
            End While
            xlsRis.Dispose()

            Me.Log("Finisco l'importazione del foglio [Pratiche]")
            Me.Log("------------------------------------------")
        End Sub

        Private Function FindPersonaByCF(ByVal cf As String) As CPersonaFisica
            Dim items As CCollection(Of CPersona) = Anagrafica.Persone.FindPersoneByCF(cf)
            If (items.Count > 0) Then
                For Each item As CPersona In items
                    If (item.TipoPersona = TipoPersona.PERSONA_FISICA) Then Return item
                Next
                Return Nothing
            Else
                Return Nothing
            End If
        End Function

        Private Function IsPhoneNumber(ByVal value As String) As Boolean
            Return Formats.ParsePhoneNumber(value) <> ""
        End Function

        Private Function FindByNameAndTelefono(ByVal Cognome As String, ByVal Nome As String, ByVal datanascita As Date?, ByVal telefono1 As String, ByVal telefono2 As String, ByVal telefono3 As String, ByVal telefono4 As String) As CPersona
            Dim ret As CPersona = Nothing

            Nome = Trim(Nome)
            Cognome = Trim(Cognome)
            telefono1 = Formats.ParsePhoneNumber(telefono1)
            telefono2 = Formats.ParsePhoneNumber(telefono2)
            telefono3 = Formats.ParsePhoneNumber(telefono3)
            telefono4 = Formats.ParsePhoneNumber(telefono4)

            'Dim web1 As String = LCase(IIf(Me.IsPhoneNumber(telefono1), "", telefono1))
            'Dim web2 As String = LCase(IIf(Me.IsPhoneNumber(telefono2), "", telefono2))
            'Dim web3 As String = LCase(IIf(Me.IsPhoneNumber(telefono3), "", telefono3))
            'Dim web4 As String = LCase(IIf(Me.IsPhoneNumber(telefono4), "", telefono4))


            Dim cursor As New CPersonaCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            If (Cognome <> "") Then cursor.Cognome.Value = Cognome
            If (Nome <> "") Then cursor.Nome.Value = Nome
            If (datanascita.HasValue) Then cursor.DataNascita.Value = datanascita

            While Not cursor.EOF
                Dim tmp As CPersona = cursor.Item
                For Each c As CContatto In tmp.Recapiti
                    If (c.Valore <> "") AndAlso (c.Valore = telefono1 OrElse c.Valore = telefono2 OrElse c.Valore = telefono3 OrElse c.Valore = telefono4) Then
                        ret = tmp
                        Exit While
                    End If
                Next
                'For Each c As CContatto In tmp.ContattiWeb
                '    If (c.Valore <> "") AndAlso (LCase(c.Valore) = web1 OrElse LCase(c.Valore) = web2 OrElse c.Valore = telefono3 OrElse c.Valore = telefono4) Then
                '        ret = tmp
                '        Exit While
                '    End If
                'Next
                cursor.MoveNext()
            End While
            cursor.Dispose()
            Return ret
        End Function

        Private Sub AddContatto(persona As CPersona, Contatto As String)
            Try
                Contatto = Trim(Contatto)
                If (Contatto = "") Then Exit Sub
                Dim telefono As String = Formats.ParsePhoneNumber(Contatto)
                If (InStr(Contatto, "@") <= 0 And telefono <> "") Then
                    persona.Recapiti.Add("Telefono", telefono, telefono)
                Else
                    persona.Recapiti.Add("e-mail", Contatto, Contatto)
                End If
            Catch ex As Exception
                Dim note As New CAnnotazione(persona)
                note.Valore = "Spazio esaurito per il contatto: " & Contatto
                note.Stato = ObjectStatus.OBJECT_VALID
                note.Save()
            End Try
        End Sub

        Private Sub AddContatto(persona As CPersona, Contatto As String, ByVal nome As String)
            Try
                Contatto = Trim(Contatto)
                If (Contatto = "") Then Exit Sub
                Dim telefono As String = Formats.ParsePhoneNumber(Contatto)
                If (InStr(Contatto, "@") <= 0 And telefono <> "") Then
                    persona.Recapiti.Add("Telefono", nome, telefono)
                Else
                    persona.Recapiti.Add("e-mail", nome, Contatto)
                End If
            Catch ex As Exception
                Dim note As New CAnnotazione(persona)
                note.Valore = "Spazio esaurito per il contatto: " & nome & " = " & Contatto
                note.Stato = ObjectStatus.OBJECT_VALID
                note.Save()
            End Try
        End Sub

        Private Function AddAnnotazione(ByVal sourceName As String, ByVal sourceParam As String, ByVal oggetto As Object, ByVal xlsRis As DBReader, Optional ByVal contesto As Object = Nothing) As CAnnotazione
            Dim text As New System.Text.StringBuilder
            For Each field As CDBEntityField In xlsRis.Schema.Fields
                If (text.Length > 0) Then text.Append("<br/>")
                text.Append("<b>" & Strings.HtmlEncode(field.Name) & "</b>: <i>" & Strings.HtmlEncode(Convert.ToString(xlsRis.GetValue(field.Name))) & "</i>")
            Next
            Dim ret As CAnnotazione = Annotazioni.GetItemBySource(sourceName, sourceParam)
            If (ret Is Nothing) Then
                If (contesto Is Nothing) Then
                    ret = New CAnnotazione(oggetto)
                Else
                    ret = New CAnnotazione(oggetto, contesto)
                End If
                ret.SourceName = sourceName
                ret.SourceParam = sourceParam
            End If
            ret.Valore = text.ToString
            ret.Stato = ObjectStatus.OBJECT_VALID
            ret.Save()
            Return ret
        End Function

        Private Function AddAnnotazione(ByVal sourceName As String, ByVal sourceParam As String, ByVal oggetto As Object, ByVal testo As String, Optional ByVal contesto As Object = Nothing) As CAnnotazione
            Dim ret As CAnnotazione = Annotazioni.GetItemBySource(sourceName, sourceParam)
            If (ret Is Nothing) Then
                If (contesto Is Nothing) Then
                    ret = New CAnnotazione(oggetto)
                Else
                    ret = New CAnnotazione(oggetto, contesto)
                End If
            End If
            ret.Valore = testo
            ret.Stato = ObjectStatus.OBJECT_VALID
            ret.Save()
            Return ret
        End Function

    End Class


End Namespace