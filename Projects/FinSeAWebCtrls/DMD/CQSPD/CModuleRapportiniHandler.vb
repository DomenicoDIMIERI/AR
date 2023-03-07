Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Imports DMD.Web
Imports DMD.XML

Namespace Forms

    Public Class CPraticaInfoLavorazione
        Implements DMD.XML.IDMDXMLSerializable

        Public m_IDPratica As Integer
        Public m_Pratica As CRapportino
        Public IDPersona As Integer
        Public Persona As CPersonaFisica
        Public NomePersona As String
        Public Rata As Decimal
        Public Durata As Integer
        Public NettoRicavo As Decimal
        Public NettoAllaMano As Decimal
        Public TAN As Single
        Public TAEG As Single

        Public Sub New()
            Me.m_IDPratica = 0
            Me.m_Pratica = Nothing
            Me.IDPersona = 0
            Me.Persona = Nothing
            Me.NomePersona = ""
            Me.Rata = 0
            Me.Durata = 0
            Me.NettoAllaMano = 0
            Me.NettoRicavo = 0
            Me.TAEG = 0
            Me.TAN = 0
        End Sub

        Public Sub New(ByVal p As CRapportino)
            If (p Is Nothing) Then Throw New ArgumentNullException("p")
            Me.m_IDPratica = GetID(p)
            Me.m_Pratica = p
            Me.Persona = p.Cliente
            Me.IDPersona = GetID(Me.Persona)
            Me.NomePersona = Me.Persona.Nominativo
            Me.Rata = p.OffertaCorrente.Rata
            Me.Durata = p.OffertaCorrente.Durata
            Me.NettoRicavo = p.OffertaCorrente.NettoRicavo
            'Me.m_NettoAllaMano = p.OffertaCorrente.NettoRicavo - p.altr
            Me.TAN = p.OffertaCorrente.TAN
            Me.TAEG = p.OffertaCorrente.TAEG
        End Sub

        Public Property IDPratica As Integer
            Get
                Return GetID(Me.m_Pratica, Me.m_IDPratica)
            End Get
            Set(value As Integer)
                If (Me.IDPratica = value) Then Exit Property
                Me.m_IDPratica = value
                Me.m_Pratica = Nothing
            End Set
        End Property

        Public Property Pratica As CRapportino
            Get
                If (Me.m_Pratica Is Nothing) Then Me.m_Pratica = CQSPD.Pratiche.GetItemById(Me.m_IDPratica)
                Return Me.m_Pratica
            End Get
            Set(value As CRapportino)
                Me.m_Pratica = value
                Me.m_IDPratica = GetID(value)
            End Set
        End Property



        Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "IDPratica" : Me.m_IDPratica = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDPersona" : Me.IDPersona = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePersona" : Me.NomePersona = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Rata" : Me.Rata = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "Durata" : Me.Durata = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NettoRicavo" : Me.NettoRicavo = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "NettoAllaMano" : Me.NettoAllaMano = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TAN" : Me.TAN = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "TAEG" : Me.TAEG = XML.Utils.Serializer.DeserializeDouble(fieldValue)
            End Select
        End Sub

        Public Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("IDPratica", Me.IDPratica)
            writer.WriteAttribute("IDPersona", Me.IDPersona)
            writer.WriteAttribute("NomePersona", Me.NomePersona)
            writer.WriteAttribute("Rata", Me.Rata)
            writer.WriteAttribute("Durata", Me.Durata)
            writer.WriteAttribute("NettoRicavo", Me.NettoRicavo)
            writer.WriteAttribute("NettoAllaMano", Me.NettoAllaMano)
            writer.WriteAttribute("TAN", Me.TAN)
            writer.WriteAttribute("TAEG", Me.TAEG)
        End Sub
    End Class



    '----------------------
    Public Class CModuleRapportiniHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SEdit Or ModuleSupportFlags.SAnnotations)
            Me.UseLocal = True
        End Sub

        Public Sub New(ByVal [module] As CModule)
            Me.New()
            Me.SetModule([module])
        End Sub





        Public Function CorreggiPratica(ByVal renderer As Object) As String
            If (Me.Module.UserCanDoAction("change_status") = False) Then Throw New PermissionDeniedException(Me.Module, "change_status")
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            Dim oid As Integer = RPC.n2int(Me.GetParameter(renderer, "oid", ""))
            Dim pratica As CRapportino = DMD.CQSPD.Pratiche.GetItemById(pid)
            Dim offerta As COffertaCQS = DMD.CQSPD.Offerte.GetItemById(oid)
            pratica.Correggi(offerta)
            Return XML.Utils.Serializer.Serialize(pratica.Info.Correzione, XMLSerializeMethod.Document)
        End Function

        Public Function Sollecita(ByVal renderer As Object) As String
            Dim pid As Integer = Me.GetParameter(renderer, "pid", 0)
            Dim pratica As CRapportino = DMD.CQSPD.Pratiche.GetItemById(pid)
            pratica.Sollecita()
            Return ""
        End Function


        Public Function ChiediApprovazione(ByVal renderer As Object) As String
            Dim pid As Integer = Me.GetParameter(renderer, "pid", 0)
            Dim pratica As CRapportino = DMD.CQSPD.Pratiche.GetItemById(pid)
            Dim motivo As String = RPC.n2str(GetParameter(renderer, "mot", ""))
            Dim dettaglio As String = RPC.n2str(GetParameter(renderer, "det", ""))
            Dim parametri As String = RPC.n2str(GetParameter(renderer, "par", ""))
            Dim rich As CRichiestaApprovazione = pratica.RichiediApprovazione(motivo, dettaglio, parametri)
            Return XML.Utils.Serializer.Serialize(rich)
        End Function

        Public Function PrendiInCarico(ByVal renderer As Object) As String
            If (Me.Module.UserCanDoAction("approva_sconto") = False) Then Throw New PermissionDeniedException(Me.Module, "approva_sconto")
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            Dim pratica As CRapportino = DMD.CQSPD.Pratiche.GetItemById(pid)

            Dim ret As CRichiestaApprovazione = pratica.PrendiInCarico

            Return XML.Utils.Serializer.Serialize(ret)
        End Function

        Public Function ConfermaApprovazione(ByVal renderer As Object) As String
            If (Me.Module.UserCanDoAction("approva_sconto") = False) Then Throw New PermissionDeniedException(Me.Module, "approva_sconto")
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            Dim motivoApprovazione As String = RPC.n2str(Me.GetParameter(renderer, "mot", ""))
            Dim dettaglioApprovazione As String = RPC.n2str(Me.GetParameter(renderer, "det", ""))
            Dim pratica As CRapportino = DMD.CQSPD.Pratiche.GetItemById(pid)
            Dim motivo As CMotivoScontoPratica = pratica.RichiestaApprovazione.MotivoRichiesta
            If (motivo Is Nothing) Then Throw New ArgumentException("Il motivo di sconto indicato non è previsto")
            If (motivo.Privilegiato AndAlso Not Sistema.Users.CurrentUser.IsInGroup(CQSPD.GruppoAutorizzatori.GroupName)) Then
                Throw New PermissionDeniedException("L'utente corrente non è abilitato ad approvare uno sconto privilegiato")
            End If
            Dim ret As CRichiestaApprovazione = pratica.Approva(motivoApprovazione, dettaglioApprovazione)
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

        Public Function NegaApprovazione(ByVal renderer As Object) As String
            If (Me.Module.UserCanDoAction("approva_sconto") = False) Then Throw New PermissionDeniedException(Me.Module, "approva_sconto")
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            Dim motivo As String = RPC.n2str(Me.GetParameter(renderer, "mot", ""))
            Dim dettaglio As String = RPC.n2str(Me.GetParameter(renderer, "det", ""))
            Dim pratica As CRapportino = DMD.CQSPD.Pratiche.GetItemById(pid)
            Dim ret As CRichiestaApprovazione = pratica.Nega(motivo, dettaglio)
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

        Public Function GetStatisticheLavorazione(ByVal renderer As Object) As String
            Dim cursor As CRapportiniCursor = Nothing
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim dbSQL As String
            Dim ret As New CQSPDDashInfo
            Dim params As String

            Try
                params = RPC.n2str(Me.GetParameter(renderer, "params", ""))
                cursor = XML.Utils.Serializer.Deserialize(params, "CRapportiniCursor")
                'cursor.Trasferita.Value = False
                cursor.Reset1()
                cursor.Trasferita.Value = False
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Flags.Value = PraticaFlags.HIDDEN
                cursor.Flags.Operator = OP.OP_NE

                If CQSPD.Pratiche.Module.UserCanDoAction("seestats") Then
                    dbSQL = cursor.GetSQL
                    Dim i As Integer = InStr(dbSQL, " FROM ", CompareMethod.Text)
                    If (i > 0) Then
                        dbSQL = Strings.JoinW("SELECT Count(*) As [Num], SUM([MontanteLordo]) As [ML], SUM([ProvvBroker]) As [PB], SUM([Running]) AS [RU], SUM([UpFront]) AS [UF] ", Mid(dbSQL, i + 1))  'SUM([Spread]) As [SP], 
                    Else
                        dbSQL = Strings.JoinW("SELECT Count(*) As [Num], SUM([MontanteLordo]) As [ML], SUM([ProvvBroker]) As [PB], SUM([Running]) AS [RU], SUM([UpFront]) AS [UF] FROM (", dbSQL, ")") 'SUM([Spread]) As [SP], 
                    End If

                    dbRis = CQSPD.Database.ExecuteReader(dbSQL)
                    If (dbRis.Read) Then
                        ret.Count = Formats.ToInteger(dbRis("Num"))
                        ret.SommaMontanteLordo = Formats.ToValuta(dbRis("ML"))
                        ret.SommaProvvigioneTotale = ret.SommaSpread + Formats.ToValuta(dbRis("PB"))
                        ret.SommaRunning = Formats.ToValuta(dbRis("RU"))
                        ret.SommaUpFront = Formats.ToValuta(dbRis("UF"))
                        ret.SommaSpread = ret.SommaUpFront + ret.SommaRunning ' Formats.ToValuta(dbRis("SP"))
                    End If
                    dbRis.Dispose()
                    dbRis = Nothing
                End If


            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try

            Return XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document)
        End Function



        Public Function GetInfoLavorazione(ByVal renderer As Object) As String
            Dim ret As New CQSPDDashInfo
            Dim cursor As CRapportiniCursor = Nothing
            Dim params As String
            'Dim st As CStatoPratica
            Try
                params = RPC.n2str(Me.GetParameter(renderer, "params", ""))
                cursor = XML.Utils.Serializer.Deserialize(params, "CRapportiniCursor")
                cursor.Reset1()
                cursor.Trasferita.Value = False
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                'cursor.Flags.Value = PraticaFlags.HIDDEN
                'cursor.Flags.Operator = OP.OP_NE

                Dim pratica As CRapportino
                Dim pratiche As New CCollection(Of CRapportino)

                While (Not cursor.EOF())
                    pratica = cursor.Item
                    pratiche.Add(pratica)
                    cursor.MoveNext()
                End While

                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

                'CQSPD.Pratiche.SyncOffertaCorrente(pratiche)
                CQSPD.Pratiche.SyncStatiLav(pratiche)


                For Each pratica In pratiche
                    Dim info As New CQSPDPDBInfo
                    info.IDPratica = GetID(pratica)
                    info.NomeProdotto = pratica.NomeProdotto
                    info.NominativoCliente = pratica.NominativoCliente
                    info.IDStatoPratica = pratica.IDStatoAttuale
                    info.Rata = pratica.Rata
                    info.Durata = pratica.NumeroRate
                    info.Netto = pratica.NettoRicavo
                    info.Spread = pratica.ValoreSpread
                    info.UpFront = pratica.ValoreUpFront

                    'If (pratica.OffertaCorrente IsNot Nothing) Then
                    '    With pratica.OffertaCorrente
                    '        info.Rata = .Rata
                    '        info.Durata = .Durata
                    '        info.Netto = .NettoRicavo
                    '        info.Spread = .ValoreSpread
                    '        info.UpFront = .ValoreUpFront
                    '    End With
                    'End If
                    If (pratica.StatoContatto IsNot Nothing AndAlso pratica.StatoContatto.Data.HasValue) Then info.GiorniContatto = DateDiff("d", pratica.StatoContatto.Data, Now)
                    If (pratica.StatoDiLavorazioneAttuale IsNot Nothing AndAlso pratica.StatoDiLavorazioneAttuale.Data.HasValue) Then info.GiorniStatoAttuale = DateDiff("d", pratica.StatoDiLavorazioneAttuale.Data, Now)
                    If (pratica.Prodotto IsNot Nothing) Then
                        info.Attributi.SetItemByKey("TipoContratto", pratica.Prodotto.IdTipoContratto)
                        info.Attributi.SetItemByKey("ColoreProdotto", pratica.Prodotto.Attributi.GetItemByKey("Colore"))
                    End If
                    If (pratica.TabellaFinanziaria IsNot Nothing) Then
                        info.Attributi.SetItemByKey("ColoreTabellaFinanziaria", pratica.TabellaFinanziaria.Attributi.GetItemByKey("Colore"))
                    End If
                    If (pratica.StatoAttuale IsNot Nothing AndAlso pratica.StatoAttuale.MacroStato = StatoPraticaEnum.STATO_PRECARICAMENTO) Then
                        info.Attributi.SetItemByKey("CaricabileIl", pratica.StatoDiLavorazioneAttuale.Params)
                    End If

                    ret.Pratiche.Add(info)

                Next


            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try

            Return XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document)
        End Function


        Public Overrides Function GetExportableColumnsList() As CCollection(Of ExportableColumnInfo)
            Dim ret As New CCollection(Of ExportableColumnInfo)
            ret.Add(New ExportableColumnInfo("ID", "ID", TypeCode.Int32, True))
            ret.Add(New ExportableColumnInfo("NominativoCliente", "Nominativo Cliente", TypeCode.String, False))
            ret.Add(New ExportableColumnInfo("NomeCliente", "Nome Cliente", TypeCode.String))
            ret.Add(New ExportableColumnInfo("CognomeCliente", "Cognome Cliente", TypeCode.String))
            ret.Add(New ExportableColumnInfo("NomePuntoOperativo", "Punto Operativo", TypeCode.String))
            ret.Add(New ExportableColumnInfo("StatoPraticaEx", "Stato Pratica", TypeCode.String))
            ret.Add(New ExportableColumnInfo("NumeroPratica", "NumeroPratica", TypeCode.String))
            'ret.Add(New ExportableColumnInfo("Trasferita", "NumeroPratica"))
            ret.Add(New ExportableColumnInfo("MontanteLordo", "Montante Lordo", TypeCode.Double))
            ret.Add(New ExportableColumnInfo("Rata", "Rata", TypeCode.Double))
            ret.Add(New ExportableColumnInfo("Durata", "Durata", TypeCode.Double))
            ret.Add(New ExportableColumnInfo("NettoRicavo", "Netto Ricavo", TypeCode.String))
            ret.Add(New ExportableColumnInfo("NomeProdotto", "Nome Prodotto", TypeCode.String))
            ret.Add(New ExportableColumnInfo("NomeCessionario", "Nome Cessionario", TypeCode.String))
            ret.Add(New ExportableColumnInfo("DataContatto", "Data Contatto", TypeCode.DateTime))
            ret.Add(New ExportableColumnInfo("DataRichiestaDelibera", "Data Richiesta Delibera", TypeCode.DateTime))
            ret.Add(New ExportableColumnInfo("DataPerfezionamento", "Data Perfezionamento", TypeCode.DateTime))
            ret.Add(New ExportableColumnInfo("DataArchiviazione", "Data Archiviazione", TypeCode.DateTime))
            ret.Add(New ExportableColumnInfo("DataAnnullamento", "Data Annullamento", TypeCode.DateTime))
            ret.Add(New ExportableColumnInfo("DettaglioStato", "Dettaglio Stato", TypeCode.String))
            ret.Add(New ExportableColumnInfo("NoteStato", "Note sullo Stato", TypeCode.String))

            ret.Add(New ExportableColumnInfo("PMax", "PMax [%]", TypeCode.Double))
            ret.Add(New ExportableColumnInfo("Spread", "Spread [%]", TypeCode.Double))
            ret.Add(New ExportableColumnInfo("UpFront", "UpFront [%]", TypeCode.Double))
            ret.Add(New ExportableColumnInfo("Running", "Running [%]", TypeCode.Double))
            ret.Add(New ExportableColumnInfo("ProvvigioneCollaboratore", "Provvigione Collaboratore [%]", TypeCode.Double))

            ret.Add(New ExportableColumnInfo("ValorePMax", "PMax [€]", TypeCode.Double))
            ret.Add(New ExportableColumnInfo("ValoreSpread", "Spread [€]", TypeCode.Double))
            ret.Add(New ExportableColumnInfo("ValoreUpFront", "Valore UpFront [€]", TypeCode.Double))
            ret.Add(New ExportableColumnInfo("ValoreRunning", "Valore Running [€]", TypeCode.Double))
            ret.Add(New ExportableColumnInfo("ValoreProvvigioneCollaboratore", "Provvigione Collaboratore [€]", TypeCode.Double))

            If (Me.Module.UserCanDoAction("amministrazione")) Then
                ret.Add(New ExportableColumnInfo("ProvvigioneProfilo", "Provvigione Profilo [%]", TypeCode.Double))
                ret.Add(New ExportableColumnInfo("ValoreProvvigioneProfilo", "Provvigione Profilo [€]", TypeCode.Double))
                ret.Add(New ExportableColumnInfo("Rappel", "Rappel [%]", TypeCode.Double))
                ret.Add(New ExportableColumnInfo("ValoreRappel", "Rappel [€]", TypeCode.Double))
                ret.Add(New ExportableColumnInfo("Costi", "Costi [€]", TypeCode.Double))
                ret.Add(New ExportableColumnInfo("PremioDaCessionario", "Premio da Cessionario", TypeCode.Double))
            End If

            'ret.Add(New ExportableColumnInfo("ProvvigioneTotale", "Provvigione Totale [%]", TypeCode.Double))
            ret.Add(New ExportableColumnInfo("ValoreProvvigioneTotale", "Provvigione Totale [€]", TypeCode.Double))
            ret.Add(New ExportableColumnInfo("NomeConsulente", "Nome Consulente", TypeCode.String))
            ret.Add(New ExportableColumnInfo("NomeCommerciale", "Nome Commerciale", TypeCode.String))
            'ret.Add(New ExportableColumnInfo("NomeTeamManager", "Nome Team Manager", TypeCode.String))
            ret.Add(New ExportableColumnInfo("TipoFonte", "Tipo Fonte", TypeCode.String))
            ret.Add(New ExportableColumnInfo("Fonte", "Fonte", TypeCode.String))
            ret.Add(New ExportableColumnInfo("NomeDatoreDiLavoro", "Datore di Lavoro", TypeCode.String, False))
            ret.Add(New ExportableColumnInfo("NomeEntePagante", "Ente Pagante", TypeCode.String, False))
            ret.Add(New ExportableColumnInfo("Annotazioni", "Annotazioni", TypeCode.String, False))
            ret.Add(New ExportableColumnInfo("ResidenteACAP", "CAP", TypeCode.String, False))
            ret.Add(New ExportableColumnInfo("ResidenteAComune", "Comune", TypeCode.String, False))
            ret.Add(New ExportableColumnInfo("ResidenteAProvincia", "Provincia", TypeCode.String, False))
            ret.Add(New ExportableColumnInfo("ResidenteAVia", "Via", TypeCode.String, False))
            ret.Add(New ExportableColumnInfo("BaseML", "Base ML", TypeCode.Double, False))
            Return ret
        End Function

        Protected Overrides Function GetColumnValue(ByVal renderer As Object, item As Object, key As String) As Object
            Dim p As CRapportino = item
            Dim info As CInfoPratica = p.Info
            Dim symValuta As String = WebSite.Instance.Configuration.SimboloValuta
            Dim decValuta As Integer = WebSite.Instance.Configuration.DecimaliPerValuta
            Dim decPercent As Integer = WebSite.Instance.Configuration.DecimaliPerPercentuale

            Select Case key
                'Case "NominativoCliente" : Return p.NominativoCliente
                Case "NomeCliente" : Return Strings.ToNameCase(p.NomeCliente)
                Case "CognomeCliente" : Return UCase(p.CognomeCliente)
                    'Case "NomePuntoOperativo" : Return p.NomePuntoOperativo
                Case "StatoPraticaEx" : Return p.StatoAttuale.Nome
                Case "NumeroPratica" : Return p.NumeroEsterno
                    'ret.Add(New ExportableColumnInfo("Trasferita", "NumeroPratica"))
                Case "MontanteLordo"
                    If (p.MontanteLordo.HasValue) Then Return Math.round(p.MontanteLordo.Value, decValuta)
                    Return DBNull.Value
                Case "NettoRicavo"
                    If (p.NettoRicavo.HasValue) Then Return Math.round(p.NettoRicavo.Value, decValuta)
                    Return DBNull.Value
                Case "Rata"
                    If (p.Rata.HasValue) Then Return Math.round(p.Rata.Value, decValuta)
                    Return DBNull.Value
                Case "Durata" : Return p.NumeroRate
                    'Case "NomeProdotto" : Return p.NomeProdotto
                    'Case "NomeCessionario" : Return p.NomeCessionario
                Case "DataContatto"
                    If (p.StatoContatto IsNot Nothing) Then
                        Return p.StatoContatto.Data
                    Else
                        Return DBNull.Value
                    End If
                Case "DataRichiestaDelibera"
                    If (p.StatoRichiestaDelibera IsNot Nothing) Then
                        Return p.StatoRichiestaDelibera.Data
                    Else
                        Return DBNull.Value
                    End If
                Case "DataPerfezionamento"
                    If (p.StatoLiquidata IsNot Nothing) Then
                        Return p.StatoLiquidata.Data
                    Else
                        Return DBNull.Value
                    End If
                Case "DataArchiviazione"
                    If (p.StatoArchiviata IsNot Nothing) Then
                        Return p.StatoArchiviata.Data
                    Else
                        Return DBNull.Value
                    End If
                Case "DataAnnullamento"
                    If (p.StatoAnnullata IsNot Nothing) Then
                        Return p.StatoAnnullata.Data
                    Else
                        Return DBNull.Value
                    End If
                Case "Running"
                    If (p.Running.HasValue) Then Return Math.round(p.Running.Value / 100, decPercent)
                    Return DBNull.Value
                Case "ValoreRunning"
                    If (p.ValoreRunning.HasValue) Then Return Math.round(p.ValoreRunning.Value, decValuta)
                    Return DBNull.Value
                Case "PMax"
                    If (p.ProvvigioneMassima.HasValue) Then Return Math.round(p.ProvvigioneMassima.Value, decPercent)
                    Return DBNull.Value
                Case "BaseML"
                    Dim baseML As Decimal? = p.CalcolaBaseML
                    If (baseML.HasValue) Then Return Math.round(baseML.Value, decValuta)
                    Return DBNull.Value
                Case "ValorePMax"
                    If (p.ValoreProvvigioneMassima.HasValue) Then Return Math.round(p.ValoreProvvigioneMassima.Value, decValuta)
                    Return DBNull.Value
                Case "Spread"
                    If (p.Spread.HasValue) Then Return Math.round(p.Spread.Value / 100, decPercent)
                    Return DBNull.Value
                Case "ValoreSpread"
                    If (p.ValoreSpread.HasValue) Then Return Math.round(p.ValoreSpread.Value, decValuta)
                    Return DBNull.Value
                Case "ProvvigioneCollaboratore"
                    If (p.Provvigionale.ValoreTotale.HasValue) Then Return Math.round(p.Provvigionale.PercentualeSu(p.MontanteLordo).Value / 100, decPercent)
                    Return DBNull.Value
                Case "ValoreProvvigioneCollaboratore"
                    If (p.Provvigionale.ValoreTotale.HasValue) Then Return Math.round(p.Provvigionale.ValoreTotale.Value, decValuta)
                    Return DBNull.Value
                Case "ProvvigioneProfilo"
                    If (p.Rappel.HasValue) Then Return Math.round(p.Rappel.Value / 100, decPercent)
                    Return DBNull.Value
                Case "ValoreProvvigioneProfilo"
                    If (p.ValoreRappel.HasValue) Then Return Math.round(p.ValoreRappel.Value, decValuta)
                    Return DBNull.Value
                Case "UpFront"
                    If (p.UpFront.HasValue) Then Return Math.round(p.UpFront.Value, decValuta)
                    Return DBNull.Value
                Case "ValoreUpFront"
                    If (p.ValoreUpFront.HasValue) Then Return Math.round(p.ValoreUpFront.Value, decValuta)
                    Return DBNull.Value
                Case "ValoreRappel"
                    If (p.ValoreRappel.HasValue) Then Return Math.round(p.ValoreRappel.Value, decValuta)
                    Return DBNull.Value
                Case "PremioDaCessionario"
                    If (p.PremioDaCessionario.HasValue) Then Return Math.round(p.PremioDaCessionario.Value, decValuta)
                    Return DBNull.Value
                Case "Costi"
                    If (info.Costo.HasValue) Then Return Math.round(info.Costo.Value, decValuta)
                    Return DBNull.Value
                'Case "ProvvigioneTotale"
                '    If (p.ProvvigioneTotale.HasValue) Then Return Math.round(p.ProvvigioneTotale.Value / 100, decPercent)
                '    Return DBNull.Value
                Case "ValoreProvvigioneTotale"
                    If (p.ValoreProvvigioneTotale.HasValue) Then Return Math.round(p.ValoreProvvigioneTotale.Value, decValuta)
                    Return DBNull.Value
                Case "NomeConsulente" : Return p.NomeConsulente
                Case "NomeCommerciale"
                    If (info.Commerciale IsNot Nothing) Then Return info.Commerciale.Nominativo
                    Return ""
                Case "Rappel"
                    If (p.Rappel.HasValue) Then Return Math.round(p.Rappel.Value, decPercent)
                    Return DBNull.Value
                Case "NomeTeamManager"
                    Return ""
                Case "TipoFonte" : Return p.TipoFonteContatto
                Case "Fonte"
                    If (p.Fonte IsNot Nothing) Then Return p.Fonte.Nome
                    Return ""
                Case "NomeDatoreDiLavoro" : Return p.Impiego.NomeAzienda
                Case "NomeEntePagante" : Return p.Impiego.NomeEntePagante
                Case "ResidenteACAP" : Return p.ResidenteA.CAP
                Case "ResidenteAComune" : Return p.ResidenteA.Citta
                Case "ResidenteAProvincia" : Return p.ResidenteA.Provincia
                Case "ResidenteAVia" : Return p.ResidenteA.ToponimoViaECivico
                Case "Annotazioni" : Return p.GetAnnotazioni.GetCompactString
                Case "DettaglioStato" : Return p.StatoDiLavorazioneAttuale.Params
                Case "NoteStato" : Return p.StatoDiLavorazioneAttuale.Note
                Case Else
                    Return MyBase.GetColumnValue(renderer, item, key)
            End Select

            Return DBNull.Value
        End Function

        Protected Overrides Sub SetColumnValue(ByVal renderer As Object, item As Object, key As String, ByVal value As Object)
            Dim p As CRapportino = item
            Dim info As CInfoPratica = p.Info

            Select Case key
                Case "NominativoCliente"
                Case "NumeroPratica"
                    'Case "StatoPraticaEx" : p.StatoPratica = CQSPD.Pratiche.ParseStatoPratica(value)
                Case "DataContatto" : p.StatoContatto.Data = Formats.ParseDate(value)
                Case "DataRichiestaDelibera" : p.StatoRichiestaDelibera.Data = Formats.ParseDate(value)
                Case "DataPerfezionamento" : p.StatoLiquidata.Data = Formats.ParseDate(value)
                Case "DataArchiviazione" : p.StatoArchiviata.Data = Formats.ParseDate(value)
                Case "DataAnnullamento" : p.StatoAnnullata.Data = Formats.ParseDate(value)
                Case "Spread" : p.Spread = Formats.ToDouble(value)
                Case "ValoreSpread" : p.ValoreSpread = Formats.ToValuta(value)
                Case "ProvvigioneCollaboratore" : p.Provvigionale.PercentualeSu(p.MontanteLordo) = Formats.ToDouble(value)
                Case "ValoreProvvigioneCollaboratore" : p.Provvigionale.ValorePercentuale = Formats.ToValuta(value)
                Case "ProvvigioneProfilo" : p.Rappel = Formats.ToDouble(value)
                Case "ValoreProvvigioneProfilo" 'p.Rappel = Formats.ToValuta(value) * 100 / p.MontanteLordo
                Case "Rappel" : p.Rappel = Formats.ToDouble(value)
                Case "ValoreRappel" ' p.Rappel = Formats.ToValuta(value) * 100 / p.MontanteLordo
                Case "Costi" : info.Costo = Formats.ToValuta(value)
                Case "ProvvigioneTotale"
                Case "ValoreUpFront" 'p.ValoreUpFront = Formats.ToValuta(value)
                Case "ValoreRunning" 'p.ValoreRunning = Formats.ToValuta(value)
                Case "ValoreProvvigioneTotale" 'p.ProvvigioneTotale = Formats.ToValuta(value) * 100 / p.MontanteLordo
                    'Case "NomeConsulente" : Return p.NomeConsulente
                    'Case "NomeCommerciale" : Return p.NomeCommerciale
                    'Case "NomeTeamManager" : Return vbNullString
                Case "NomeProdotto"
                    If (p.NomeProdotto <> Formats.ToString(value)) Then
                        p.NomeProdotto = Formats.ToString(value)
                        p.Prodotto = DMD.CQSPD.Prodotti.GetItemByName(p.NomeProdotto)
                    End If
                Case "NomeProfilo"
                    If (p.NomeProfilo <> Formats.ToString(value)) Then
                        p.NomeProfilo = Formats.ToString(value)
                        p.Profilo = DMD.CQSPD.Profili.GetItemByName(p.NomeProfilo)
                        p.Prodotto = DMD.CQSPD.Prodotti.GetItemByName(p.NomeProdotto)
                    End If
                Case "NomeCessionario"
                    If (p.NomeCessionario <> Formats.ToString(value)) Then
                        p.NomeCessionario = Formats.ToString(value)
                        p.Cessionario = DMD.CQSPD.Cessionari.GetItemByName(p.NomeCessionario)
                        p.Profilo = DMD.CQSPD.Profili.GetItemByName(p.NomeProfilo)
                        p.Prodotto = DMD.CQSPD.Prodotti.GetItemByName(p.NomeProdotto)
                    End If
                Case "TipoFonte"
                    If (p.TipoFonteContatto <> Formats.ToString(value)) Then
                        p.TipoFonteContatto = Formats.ToString(value)
                        p.Fonte = Anagrafica.Fonti.GetItemByName(p.TipoFonteContatto, p.TipoFonteContatto, p.NomeFonte)
                    End If
                Case "Fonte"
                    If (p.NomeFonte <> Formats.ToString(value)) Then
                        p.NomeFonte = Formats.ToString(value)
                        p.Fonte = Anagrafica.Fonti.GetItemByName(p.TipoFonteContatto, p.TipoFonteContatto, p.NomeFonte)
                    End If
                Case "NomeDatoreDiLavoro"
                    If (p.Impiego.NomeAzienda <> Formats.ToString(value)) Then
                        p.Impiego.NomeAzienda = Formats.ToString(value)
                        p.Impiego.Azienda = Anagrafica.Aziende.GetItemByName(p.Impiego.NomeAzienda)
                    End If
                    'Case "NomeEntePagante"
                    '    If (p.NomeEntePagante <> Formats.ToString(value)) Then
                    '        p.NomeEntePagante = Formats.ToString(value)
                    '        p.EntePagante = Anagrafica.Aziende.GetItemByName(p.NomeEntePagante)
                    '    End If
                Case "Annotazioni"
                Case "DettaglioStato" : p.StatoDiLavorazioneAttuale.Params = Formats.ToString(value)
                Case "NoteStato" : p.StatoDiLavorazioneAttuale.Note = Formats.ToString(value)
                Case "PremioDaCessionario" : p.PremioDaCessionario = Formats.ParseValuta(value)
                Case Else
                    MyBase.SetColumnValue(renderer, item, key, value)
            End Select
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CRapportiniCursor
        End Function

        'Public Function GetLstProfProd(ByVal renderer As Object) As String
        '    'Dim cess As Integer = RPC.n2int(GetParameter(renderer, "cess", "0"))
        '    'Dim prof As Integer = RPC.n2int(GetParameter(renderer, "prof", "0"))
        '    Dim ret As New CKeyCollection(Of String)
        '    ret.Add("profili", XML.Utils.Serializer.SerializeString(Me.GetProfList(renderer)))
        '    ret.Add("prodotti", XML.Utils.Serializer.SerializeString(Me.GetProdList(renderer)))

        '    Dim prod As Integer = RPC.n2int(Me.GetParameter(renderer, "prod", "0"))
        '    Dim prodotto As CCQSPDProdotto = CQSPD.Prodotti.GetItemById(prod)
        '    If (prodotto IsNot Nothing) Then
        '        ret.Add("tabellefin", XML.Utils.Serializer.SerializeString(XML.Utils.Serializer.Serialize(prodotto.TabelleFinanziarieRelations)))
        '        ret.Add("polizze", XML.Utils.Serializer.SerializeString(XML.Utils.Serializer.Serialize(prodotto.TabelleAssicurativeRelations)))
        '    End If
        '    Return XML.Utils.Serializer.Serialize(ret)
        'End Function

        'Public Function GetProfList(ByVal renderer As Object) As String
        '    Dim items As CCollection(Of CProfilo)
        '    Dim c, i As Integer
        '    Dim writer As New System.Text.StringBuilder
        '    Dim ov As Boolean = Formats.ToBoolean(RPC.n2bool(Me.GetParameter(renderer, "ov", "")))
        '    c = RPC.n2int(Me.GetParameter(renderer, "c", ""))
        '    Dim cessionario As CCQSPDCessionarioClass = CQSPD.Cessionari.GetItemById(c)
        '    items = DMD.CQSPD.Pratiche.GetArrayProfiliEsterni(cessionario, ov)
        '    writer.Append("<list>")
        '    For i = 0 To items.Count - 1
        '        With items.Item(i)
        '            If (.Visibile) AndAlso (ov = False OrElse items.Item(i).IsValid) Then
        '                writer.Append("<item>")
        '                writer.Append("<text>")
        '                writer.Append(Strings.HtmlEncode(.ProfiloVisibile))
        '                writer.Append("</text>")
        '                writer.Append("<value>")
        '                writer.Append(GetID(items.Item(i)))
        '                writer.Append("</value>")
        '                writer.Append("</item>")
        '            End If
        '        End With
        '    Next
        '    writer.Append("</list>")
        '    Return writer.ToString
        'End Function

        'Public Function GetProdList(ByVal renderer As Object) As String
        '    Dim profilo As CProfilo
        '    Dim pID As Integer
        '    Dim prodotti As CCollection(Of CCQSPDProdotto)
        '    Dim tipoContratto As String = RPC.n2str(Me.GetParameter(renderer, "tc", ""))
        '    Dim tipoRapporto As String = RPC.n2str(Me.GetParameter(renderer, "tr", ""))
        '    Dim ov As Boolean = Formats.ToBoolean(RPC.n2bool(Me.GetParameter(renderer, "ov", "")))
        '    Dim writer As New System.Text.StringBuilder
        '    pID = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
        '    profilo = DMD.CQSPD.Profili.GetItemById(pID)

        '    writer.Append("<list>")

        '    If (profilo IsNot Nothing) Then
        '        prodotti = profilo.ProdottiXProfiloRelations.GetProdotti
        '        For i As Integer = 0 To prodotti.Count - 1
        '            Dim prodotto As CCQSPDProdotto = prodotti(i)
        '            If (tipoContratto = "" OrElse tipoContratto = prodotti(i).IdTipoContratto) Then
        '                If (tipoRapporto = "" OrElse prodotti(i).IdTipoRapporto = tipoRapporto) Then
        '                    If (ov = False OrElse prodotti(i).IsValid) Then
        '                        writer.Append("<item>")
        '                        writer.Append("<text>")
        '                        writer.Append(Strings.HtmlEncode(prodotto.Nome))
        '                        writer.Append("</text>")
        '                        writer.Append("<value>")
        '                        writer.Append(GetID(prodotto))
        '                        writer.Append("</value>")
        '                        writer.Append("<attribute>")
        '                        If (prodotto.IsValid) Then
        '                            writer.Append("1")
        '                        Else
        '                            writer.Append("0")
        '                        End If
        '                        writer.Append("</attribute>")
        '                        writer.Append("</item>")
        '                    End If
        '                End If
        '            End If
        '        Next
        '    End If

        '    writer.Append("</list>")


        '    Return writer.ToString
        'End Function

        Public Function GetInfoPratica(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            Dim cursor As New CInfoPraticaCursor()
            Try
                cursor.IDPratica.Value = pid
                cursor.IgnoreRights = True
                cursor.PageSize = 1
                If (cursor.Item IsNot Nothing) Then
                    Return XML.Utils.Serializer.Serialize(cursor.Item)
                Else
                    Return ""
                End If
            Catch ex As Exception
                Throw
            Finally
                cursor.Dispose()
            End Try
        End Function



        Public Function GetProdsXProfilo(ByVal renderer As Object) As String
            Dim profilo As CProfilo
            Dim pID As Integer
            Dim Prodotti As CCollection(Of CCQSPDProdotto)
            pID = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            profilo = DMD.CQSPD.Profili.GetItemById(pID)
            If (profilo Is Nothing) Then Throw New ArgumentNullException("Profilo")
            Prodotti = profilo.ProdottiXProfiloRelations.GetProdotti
            If (Prodotti.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(Prodotti.ToArray, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function

        Public Function GetProdottiXProfiloRelations(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            Dim profilo As CProfilo = CQSPD.Profili.GetItemById(pid)
            Dim list As CProdottiXProfiloRelations = profilo.ProdottiXProfiloRelations ' New CProdottiXProfiloRelations(profilo)
            Return XML.Utils.Serializer.Serialize(list, XMLSerializeMethod.Document)
        End Function



        Public Function CanForceStatus(ByVal item As CRapportino) As Boolean
            Return (Me.Module.UserCanDoAction("change_status"))
            'If (nextStatus < pratica.StatoPratica AndAlso Not CanChangeStatus) Then Throw New PermissionDeniedException(Me.Module, "Non disponi delle autorizzazioni per retrocedere lo stato della pratica")
            'If (nextStatus = pratica.StatoPratica) Then Throw New InvalidOperationException("Nessun cambiamento")
        End Function

        Public Function ChangeStatus(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            Dim pratica As CRapportino = DMD.CQSPD.Pratiche.GetItemById(pid)
            Dim idStato As Integer = RPC.n2int(Me.GetParameter(renderer, "ns", ""))
            Dim idRule As Integer = RPC.n2int(Me.GetParameter(renderer, "rul", ""))
            Dim data As Date = RPC.n2date(Me.GetParameter(renderer, "dt", ""))
            Dim param As String = RPC.n2str(Me.GetParameter(renderer, "pr", ""))
            Dim notes As String = RPC.n2str(Me.GetParameter(renderer, "nt", ""))
            Dim opID As Integer = RPC.n2int(Me.GetParameter(renderer, "op", ""))
            Dim operatore As CUser = Sistema.Users.GetItemById(opID)
            If (operatore Is Nothing) Then Throw New ArgumentNullException("operatore")
            If (idRule = 0 AndAlso Not Me.CanForceStatus(pratica)) Then Throw New PermissionDeniedException(Me.Module, "Non disponi delle autorizzazioni per forzare lo stato della pratica")

            Dim stato As CStatoPratica = DMD.CQSPD.StatiPratica.GetItemById(idStato)
            Dim rule As CStatoPratRule = DMD.CQSPD.StatiPratRules.GetItemById(idRule)

            Dim stLav As CStatoLavorazionePratica = pratica.ChangeStatus(stato, rule, data, param, notes, operatore)

            Dim note As New CAnnotazione(pratica.Cliente)
            note.Stato = ObjectStatus.OBJECT_VALID
            note.Valore = Strings.JoinW("Passaggio di stato: ", stato.Nome, "<br/>Parametri: ", param, "<br/>Note: ", notes)
            note.IDContesto = GetID(pratica)
            note.TipoContesto = TypeName(pratica)
            note.Save()


            Return XML.Utils.Serializer.Serialize(stLav, XMLSerializeMethod.Document)
        End Function



        Public Function NotificaSoglia(ByVal renderer As Object) As String
            Dim pid As Integer = Me.GetParameter(renderer, "pid", 0)
            Dim pratica As CRapportino = DMD.CQSPD.Pratiche.GetItemById(pid)
            Dim msg As String = RPC.n2str(GetParameter(renderer, "msg", ""))

            pratica.Watch(msg)

            Return ""
        End Function

        Public Function GetElencoStatiSuccessivi(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            Dim pratica As CRapportino = DMD.CQSPD.Pratiche.GetItemById(pid)
            Dim ret As CCollection(Of CStatoPratRule) = CQSPD.StatiPratica.GetStatiSuccessivi(pratica)
            Return XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document)
        End Function



        Public Function GetArrayStatiAttivi(ByVal renderer As Object) As String
            Dim items As CCollection(Of CStatoPratica)
            Dim i As Integer
            Dim writer As New System.Text.StringBuilder
            items = DMD.CQSPD.StatiPratica.GetStatiAttivi
            For i = 0 To items.Count - 1
                If i > 0 Then writer.Append(Chr(10))
                writer.Append(items.Item(i).ID)
                writer.Append(Chr(10))
                writer.Append(items.Item(i).Nome)
            Next
            Return writer.ToString
        End Function

        Public Function GetArrayStatiSuccessivi(ByVal renderer As Object) As String
            Dim items As CStatoPratRulesCollection
            Dim i As Integer
            Dim writer As New System.Text.StringBuilder
            Dim currStato As CStatoPratica
            Dim cID As Integer
            cID = RPC.n2int(Me.GetParameter(renderer, "cs", ""))
            currStato = DMD.CQSPD.StatiPratica.GetItemById(cID)
            items = DMD.CQSPD.StatiPratica.GetStatiSuccessivi(currStato)

            For i = 0 To items.Count - 1
                If i > 0 Then writer.Append(Chr(10))
                writer.Append(items.Item(i).ID)
                writer.Append(Chr(10))
                writer.Append(items.Item(i).Nome)
            Next
            Return writer.ToString
        End Function

        Public Function GetNomiCessionari(ByVal renderer As Object) As String
            Dim strSearch As String = Trim(Replace(Me.GetParameter(renderer, "_q", ""), "  ", ""))
            Dim ret As New System.Text.StringBuilder

            ret.Append("<list>")
            If strSearch <> "" Then
                For Each c As CCQSPDCessionarioClass In CQSPD.Cessionari.LoadAll
                    If c.Stato = ObjectStatus.OBJECT_VALID AndAlso
                        (InStr(c.Nome, strSearch, CompareMethod.Text) > 0) Then
                        ret.Append("<item>")
                        ret.Append("<text>")
                        ret.Append(Strings.HtmlEncode(c.Nome))
                        ret.Append("</text>")
                        ret.Append("<value>")
                        ret.Append(Strings.HtmlEncode(Databases.GetID(c, 0)))
                        ret.Append("</value>")
                        ret.Append("<icon>")
                        ret.Append(Strings.HtmlEncode(c.ImagePath))
                        ret.Append("</icon>")
                        ret.Append("</item>")
                    End If
                Next
            End If
            ret.Append("</list>")

            Return ret.ToString
        End Function

        Public Function setupDocPrat(ByVal renderer As Object) As String
            Dim pratID, prodID As Integer
            Dim pratica As CRapportino
            Dim prodotto As CCQSPDProdotto

            pratID = RPC.n2int(Me.GetParameter(renderer, "ID", ""))
            prodID = RPC.n2int(Me.GetParameter(renderer, "PID", ""))
            pratica = DMD.CQSPD.Pratiche.GetItemById(pratID)
            prodotto = DMD.CQSPD.Prodotti.GetItemById(prodID)
            If (pratica Is Nothing) Then Throw New ArgumentNullException("Pratica is null")

            If (prodotto Is Nothing) Then Throw New ArgumentNullException("Prodotto is null")


            'For i = 0 To prodotto.DocumentiPerPratica.Count - 1
            '    doc = prodotto.DocumentiPerPratica.Item(i)


            '    If (doc.Stato = ObjectStatus.OBJECT_VALID) And (Not pratica.Documentazione.ContainsDoc(doc)) Then
            '        docXPrat = pratica.Documentazione.AddDoc(doc)
            '        docXPrat.Stato = ObjectStatus.OBJECT_VALID
            '        docXPrat.Save()
            '    End If
            'Next

            setupDocPrat = ""
        End Function

        Public Function handlePratica(ByVal renderer As Object) As String
            Dim src, po, xmlText As String
            Dim item As CRapportino
            Dim info As CAllowedRemoteIPs
            src = Me.GetParameter(renderer, "src", "")
            po = Me.GetParameter(renderer, "po", "")
            xmlText = RPC.n2str(Me.GetParameter(renderer, "xml", ""))
            info = DMD.CQSPD.Pratiche.GetRemoteIPInfo(WebSite.ASP_Request.ServerVariables("REMOTE_ADDR"))
            If info Is Nothing Then
                Throw New ArgumentOutOfRangeException("IP non consentito")
                Return vbNullString
            End If
            If info.Negate = True Then
                Throw New ArgumentOutOfRangeException("IP non consentito")
                Return vbNullString
            End If
            item = XML.Utils.Serializer.Deserialize(xmlText, "CRapportino")
            item.TipoFonteContatto = "Agenzia"
            item.NomeFonte = Strings.JoinW(src, " ", item.NomePuntoOperativo, " (", info.Name, "/", info.RemoteIP, ")")
            item.NomePuntoOperativo = po
            item.DaVedere = True
            Call item.ForceUser(Users.GetItemByName("RapportiniReceiver"))
            item.StatoRichiestaDelibera.Operatore = item.CreatoDa
            item.StatoLiquidata.Operatore = item.CreatoDa
            DBUtils.ResetID(item)
            item.Save()
            Return RPC.FormatID(GetID(item))
        End Function

        'Public Function GetTransferCommands(ByVal renderer As Object) As String
        '    Dim dbRis As System.Data.IDataReader = Nothing
        '    Try
        '        Dim dbSQL As String = "SELECT * FROM [tbl_Rapportini_Trasferisci] ORDER BY [Descrizione] ASC"
        '        dbRis = CQSPD.Database.ExecuteReader(dbSQL)
        '        Dim ret As New System.Text.StringBuilder
        '        ret.Append("<list>")
        '        While dbRis.Read
        '            ret.Append("<item>")
        '            ret.Append("<text>")
        '            ret.Append(Strings.HtmlEncode(dbRis("Descrizione")))
        '            ret.Append("</text>")
        '            ret.Append("<value>")
        '            ret.Append(Strings.HtmlEncode(dbRis("URL")))
        '            ret.Append("</value>")
        '            ret.Append("</item>")
        '        End While
        '        ret.Append("</list>")

        '        Return ret.ToString
        '    Catch ex As Exception
        '        Sistema.Events.NotifyUnhandledException(ex)
        '        Throw
        '    Finally
        '        If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
        '    End Try
        'End Function

        'Public Function ExecTransferCommand(ByVal renderer As Object) As String
        '    Dim pid As Integer
        '    Dim cmd, agenzia As String
        '    Dim item As CRapportino
        '    Dim xmlText As String
        '    Dim ret As String
        '    pid = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
        '    agenzia = RPC.n2str(Me.GetParameter(renderer, "age", ""))
        '    cmd = RPC.n2str(Me.GetParameter(renderer, "cmd", ""))
        '    item = DMD.CQSPD.Pratiche.GetItemById(pid)
        '    xmlText = XML.Utils.Serializer.Serialize(item, XMLSerializeMethod.Document)
        '    ret = RPC.InvokeMethod(cmd, "xml", xmlText)
        '    Dim info As CInfoPratica = item.Info
        '    With info
        '        .DataTrasferimento = Now
        '        .TrasferitoA = agenzia
        '        .IDPraticaTrasferita = RPC.ParseID(ret)
        '        .DataAggiornamentoPT = Nothing
        '        .TrasferitoDa = Users.CurrentUser
        '        .Save()
        '    End With
        '    item.Trasferita = True
        '    item.Save()
        '    Return ret
        'End Function


        'Public Function CheckPraticheRifinanziabili(ByVal renderer As Object) As String
        '    Dim cursor As CRapportiniCursor = Nothing
        '    Try
        '        Dim ric As CRicontatto
        '        Dim pratica As CRapportino
        '        Dim persona As CPersona
        '        Dim d As Date
        '        Dim percComplet As Double
        '        Dim ret As New System.Text.StringBuilder
        '        percComplet = DMD.CQSPD.Configuration.PercCompletamentoRifn

        '        cursor = New CRapportiniCursor
        '        cursor.Stato.Value = ObjectStatus.OBJECT_VALID
        '        cursor.StatoPratica.Value = StatoPraticaEnum.STATO_LIQUIDATA
        '        cursor.StatoPratica.Operator = OP.OP_GE
        '        cursor.NumeroRate.Value = 0
        '        cursor.NumeroRate.Operator = OP.OP_GT
        '        While (Not cursor.EOF) And (WebSite.ASP_Response.IsClientConnected)
        '            pratica = cursor.Item
        '            d = pratica.StatoLiquidata.Data
        '            d = Calendar.GetDatePart(DateAdd("m", Fix(percComplet * Formats.ToInteger(pratica.NumeroRate) / 100), d))
        '            persona = pratica.Cliente
        '            If (persona Is Nothing) Then
        '                ret.Append("Impossibile programmare il ricontatto per la pratica N°")
        '                ret.Append(pratica.NumeroPratica)
        '                ret.Append(" poichè non conosco il cliente")
        '                ret.Append(vbCrLf)
        '            End If
        '            If Not (persona Is Nothing) Then
        '                ric = Ricontatti.GetRicontattoBySource(TypeName(pratica), GetID(pratica))
        '                If ric Is Nothing Then
        '                    ric = Ricontatti.ProgrammaRicontatto(persona, d, Strings.JoinW("Pratica rifinanziabile: il cliente ha pagato il ", Formats.FormatPercentage(percComplet), " % dell'importo"), TypeName(pratica), GetID(pratica), "Rinnovi CQS/DEL", pratica.PuntoOperativo, pratica.StatoDiLavorazioneAttuale.Operatore)
        '                    ret.Append("Programmato ricontatto in data ")
        '                    ret.Append(Formats.FormatUserDate(d))
        '                    ret.Append(" per la pratica N°")
        '                    ret.Append(pratica.NumeroPratica)
        '                    ret.Append(vbCrLf)
        '                    ric.Save()
        '                End If
        '            End If
        '            cursor.MoveNext()
        '        End While

        '        Return ret.ToString
        '    Catch ex As Exception
        '        Sistema.Events.NotifyUnhandledException(ex)
        '        Throw
        '    Finally
        '        If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
        '    End Try
        'End Function

        Public Function getPraticheByPersona(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            Dim persona As CPersonaFisica = Anagrafica.Persone.GetItemById(pid)
            Dim pratiche As CCollection(Of CRapportino) = DMD.CQSPD.Pratiche.GetPraticheByPersona(persona)
            If (pratiche.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(pratiche.ToArray, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function

        Public Function getPraticheByAzienda(ByVal renderer As Object) As String
            Dim aid As Integer = RPC.n2int(Me.GetParameter(renderer, "aid", ""))
            Dim azienda As CAzienda = Anagrafica.Aziende.GetItemById(aid)
            Dim pratiche As CCollection(Of CRapportino) = DMD.CQSPD.Pratiche.GetPraticheByAzienda(azienda)
            If (pratiche.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(pratiche.ToArray, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function

        Public Function getPraticheByProposta(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            Dim proposta As CQSPDConsulenza = CQSPD.Consulenze.GetItemById(pid)
            Dim pratiche As CCollection(Of CRapportino) = DMD.CQSPD.Pratiche.GetPraticheByProposta(proposta)
            If (pratiche.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(pratiche.ToArray, XMLSerializeMethod.Document)
            Else
                Return ""
            End If
        End Function

        Public Function getElencoRichieste(ByVal renderer As Object) As String
            Dim cursor As CRichiesteFinanziamentoCursor = Nothing

            Try
                Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
                Dim ret As New CCollection(Of CRichiestaFinanziamento)
                If (pid = 0) Then Return ""
                cursor = New CRichiesteFinanziamentoCursor
                cursor.IDCliente.Value = pid
                cursor.Data.SortOrder = SortEnum.SORT_DESC
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                While Not cursor.EOF
                    ret.Add(cursor.Item)
                    cursor.MoveNext()
                End While

                If (ret.Count > 0) Then
                    Return XML.Utils.Serializer.Serialize(ret.ToArray, XMLSerializeMethod.Document)
                Else
                    Return vbNullString
                End If
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

        Public Function GetStatiLavorazionePratica(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            Dim pratica As CRapportino = Me.GetInternalItemById(pid)
            Return XML.Utils.Serializer.Serialize(pratica.StatiDiLavorazione, XMLSerializeMethod.Document)
        End Function

        Public Function GetDocumentiCaricatiPerPratica(ByVal renderer As Object) As String
            Dim idp As Integer = RPC.n2int(Me.GetParameter(renderer, "idp", "0"))
            Dim p As CRapportino = Me.GetInternalItemById(idp)
            'Dim items As New CDocumentoPraticaCaricatoCollection(p)
            p.Vincoli.AllineaDocumentiCaricati()
            Return XML.Utils.Serializer.Serialize(p.Vincoli)
        End Function

        Public Function GetStatisticaClientiLavorati(ByVal renderer As Object) As String
            If Not CQSPD.Module.UserCanDoAction("statisticaclientilavorati") Then Throw New PermissionDeniedException(CQSPD.Module, "statisticaclientilavorati")
            Dim filtertext As String = RPC.n2str(Me.GetParameter(renderer, "filter", ""))
            Dim filter As ClientiLavoratiFilter = XML.Utils.Serializer.Deserialize(filtertext)
            Dim stats As New ClientiLavoratiStats
            stats.Apply(filter)
            Dim t1 As Double = Timer
            Dim ret As String = XML.Utils.Serializer.Serialize(stats)
            Return ret
        End Function


        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return CQSPD.Pratiche.GetItemById(id)
        End Function


        Public Function CalcolaSomme(ByVal renderer As Object) As String
            Dim text As String = RPC.n2str(Me.GetParameter(renderer, "text", ""))
            Dim cursor As CRapportiniCursor = XML.Utils.Serializer.Deserialize(text)
            Dim ret As CQSPDMLInfo = CQSPD.Pratiche.CalcolaSomme(cursor)
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

    End Class


End Namespace